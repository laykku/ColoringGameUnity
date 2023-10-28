using LK.PaintingGame.Code.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LK.PaintingGame.Code.UI
{
    public class MenuPanel : UIPanel
    {
        [Inject] private readonly SignalBus _signalBus;
        
        [SerializeField] private TMP_Text message;
        [SerializeField] private Button playButton;

        private void Start()
        {
            playButton.onClick.AddListener(() => _signalBus.Fire<StartGameSignal>());
        }
    }
}