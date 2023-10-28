using LK.PaintingGame.Code.GameStates;
using LK.PaintingGame.Code.Signals;
using LK.PaintingGame.Code.UI;
using UnityEngine;
using Zenject;

namespace LK.PaintingGame.Code.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameUI gameUIPrefab;
        
        public override void InstallBindings()
        {
            InitSignals();
            BindStates();
            
            Container.BindFactory<Color, ColorSelector, ColorSelector.Factory>().FromFactory<ColorSelectorFactory>();
            Container.Bind<GameUI>().FromComponentInNewPrefab(gameUIPrefab).AsSingle();
            Container.Bind<BufferAllocator>().FromNewComponentOnNewGameObject().WithGameObjectName("_allocator").AsSingle();
        }

        private void InitSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<SelectColorSignal>();
            Container.DeclareSignal<LevelCompleteSignal>();
            Container.DeclareSignal<StartGameSignal>();
            Container.BindSignal<LevelCompleteSignal>()
                .ToMethod<GameController>(gameController => gameController.LevelCompleted).FromResolve();
            Container.BindSignal<StartGameSignal>()
                .ToMethod<GameController>(gameController => gameController.StartGame).FromResolve();
        }

        private void BindStates()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
            Container.BindFactory<MenuState, MenuState.Factory>();
            Container.BindFactory<GameplayState, GameplayState.Factory>();
        }
    }
}