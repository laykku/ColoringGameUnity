using LK.PaintingGame.Code.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace LK.PaintingGame.Code.UI
{
    public class ColorSelector : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image image;
        
        [Inject] private readonly SignalBus _signalBus;

        public void Init(Color color)
        {
            if (!image) Debug.LogError("Color selector image is not set.");
            color.a = 1f;
            image.color = color;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _signalBus.Fire(new SelectColorSignal(image.color));
        }

        public class Factory : PlaceholderFactory<Color, ColorSelector>
        {
        }
    }
}