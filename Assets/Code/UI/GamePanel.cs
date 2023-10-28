using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LK.PaintingGame.Code.UI
{
    public class GamePanel : UIPanel
    {
        [SerializeField] private Transform colorsContent;
        [SerializeField] private RawImage reference;
        
        [Inject] private readonly ColorSelector.Factory _colorSelectorFactory;

        private readonly List<ColorSelector> _colorSelectors = new();
        
        public void Init(Color[] colors, Texture refTexture)
        {
            _colorSelectors.ForEach(s => Destroy(s.gameObject));
            _colorSelectors.Clear();
            
            foreach (var color in colors)
            {
                var colorSelector = _colorSelectorFactory.Create(color);
                colorSelector.transform.SetParent(colorsContent);
                _colorSelectors.Add(colorSelector);
            }

            reference.texture = refTexture;
        }
    }
}