using DG.Tweening;
using LK.PaintingGame.Code.Signals;
using LK.PaintingGame.Code.UI;
using UnityEngine;
using Zenject;

namespace LK.PaintingGame.Code.GameStates
{
    public class GameplayState : IGameState
    {
        [Inject] private readonly GameConfig _config;
        [Inject] private readonly GameUI _gameUI;
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly BufferAllocator _bufferAllocator;

        private MeshRenderer _renderer;
        private ComputeBuffer _coverageBuffer;

        private ComputeShader _texturePaintShader;

        private int _textureSize;

        private bool _completed;

        private static int _level = 0;

        public void Enter()
        {
            // Init game field
            
            _texturePaintShader = GameObject.Instantiate(_config.texturePaintShader);

            var level = _config.levels[_level];

            _renderer = GameObject.CreatePrimitive(PrimitiveType.Quad).GetComponent<MeshRenderer>();
            _renderer.sharedMaterial = _config.paintingMaterial;

            _textureSize = level.backgroundTexture.width;

            var paintingRt = CreateRT(level.backgroundTexture);
            var maskRt = CreateRT(level.maskTexture);
            var checkRt = CreateRT(level.checkTexture);

            _renderer.sharedMaterial.mainTexture = paintingRt;

            int mainKernel = _texturePaintShader.FindKernel("CSMain");
            _texturePaintShader.SetTexture(mainKernel, "Surface", paintingRt);
            _texturePaintShader.SetTexture(mainKernel, "Mask", maskRt);

            var checkKernel = _texturePaintShader.FindKernel("CheckCoverage");
            _texturePaintShader.SetTexture(checkKernel, "Surface", paintingRt);
            _texturePaintShader.SetTexture(checkKernel, "Mask", maskRt);
            _texturePaintShader.SetTexture(checkKernel, "Check", checkRt);

            _coverageBuffer = _bufferAllocator.AllocateIntBuffer();
            _texturePaintShader.SetBuffer(checkKernel, "CoverageBuffer", _coverageBuffer);

            // ------------------

            _gameUI.gamePanel.Show();
            _gameUI.gamePanel.Init(level.colors, level.refTexture);
            
            _signalBus.Subscribe<SelectColorSignal>(ChangeColor);
            
            SetDrawingColor(level.colors[0]);
        }

        public void Update()
        {
            if (_completed) return;
            
            if (Input.GetMouseButton(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    var texX = Utils.Remap(hit.textureCoord.x, 0f, 1f, 0f, _textureSize);
                    var texY = Utils.Remap(hit.textureCoord.y, 0f, 1f, 0f, _textureSize);
                    _texturePaintShader.SetVector("MousePos", new Vector4(texX, texY, 0f, 0f));
                    _texturePaintShader.Dispatch(0, _textureSize / 8, _textureSize / 8, 1);
                }
            }

            _coverageBuffer.SetData(new int[]{0});

            var checkKernel = _texturePaintShader.FindKernel("CheckCoverage");
            _texturePaintShader.Dispatch(checkKernel, _textureSize / 8, _textureSize / 8, 1);
            int[] coverageData = new int[1];
            _coverageBuffer.GetData(coverageData);

            if (coverageData[0] / 1000f < 2000f)
            {
                _completed = true;
                DOTween.Sequence().Append(
                        _renderer.transform.DOScale(Vector3.one * 1.5f, 1f))
                    .Append(_renderer.transform.DOScale(Vector3.zero, 0.5f))
                    .onComplete += () =>
                {
                    _signalBus.Fire<LevelCompleteSignal>();
                    _level = (_level + 1) % _config.levels.Length;
                };
            }
        }

        public void Exit()
        {
            _signalBus.Unsubscribe<SelectColorSignal>(ChangeColor);
            GameObject.Destroy(_renderer.gameObject);
            GameObject.Destroy(_texturePaintShader);
            _gameUI.gamePanel.Hide();
            _bufferAllocator.ReleaseBuffer();
        }

        private RenderTexture CreateRT(Texture src)
        {
            var rt = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            rt.enableRandomWrite = true;
            rt.Create();

            RenderTexture.active = rt;
            Graphics.Blit(src, rt);
            RenderTexture.active = null;

            return rt;
        }

        private void ChangeColor(SelectColorSignal signalInfo)
        {
            SetDrawingColor(signalInfo.Color);
        }

        private void SetDrawingColor(Color color)
        {
            _texturePaintShader.SetVector("PaintingColor", color);

            Color.RGBToHSV(color, out var h, out var s, out var v);
            Camera.main.backgroundColor = Color.HSVToRGB(h, 0.25f, 1f);
        }

        [System.Serializable]
        public class GameConfig
        {
            public Level[] levels;
            public ComputeShader texturePaintShader;
            public Material paintingMaterial;
        }

        public class Factory : PlaceholderFactory<GameplayState>
        {
        }
    }
}