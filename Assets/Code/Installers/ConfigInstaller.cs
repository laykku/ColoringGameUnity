using LK.PaintingGame.Code.GameStates;
using LK.PaintingGame.Code.UI;
using UnityEngine;
using Zenject;

namespace LK.PaintingGame.Code.Installers
{
    [CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
    public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
    {
        public GameplayState.GameConfig gameConfig;
        public GameUI.Config uiConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(gameConfig);
            Container.BindInstance(uiConfig);
        }
    }
}