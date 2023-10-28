using UnityEngine;
using Zenject;

namespace LK.PaintingGame.Code.UI
{
    public class ColorSelectorFactory : IFactory<Color, ColorSelector>
    {
        private readonly DiContainer _container;
        private readonly GameUI.Config _uiConfig;
        
        public ColorSelectorFactory(DiContainer container, GameUI.Config uiConfig)
        {
            _container = container;
            _uiConfig = uiConfig;
        }

        public ColorSelector Create(Color color)
        {
            var colorSelector = _container.InstantiatePrefab(_uiConfig.colorSelectorPrefab).GetComponent<ColorSelector>();
            colorSelector.Init(color);
            return colorSelector;
        }
    }
}