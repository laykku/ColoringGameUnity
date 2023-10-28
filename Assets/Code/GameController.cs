using LK.PaintingGame.Code.GameStates;
using LK.PaintingGame.Code.Signals;
using Zenject;

namespace LK.PaintingGame.Code
{
    public class GameController : IInitializable, ITickable
    {
        [Inject] private readonly MenuState.Factory _menuStateFactory;
        [Inject] private readonly GameplayState.Factory _gameplayStateFactory;
     
        private IGameState _currentState;

        public void Initialize()
        {
            ChangeState(_gameplayStateFactory.Create());
        }

        public void ChangeState(IGameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void Tick()
        {
            _currentState?.Update();
        }

        public void LevelCompleted(LevelCompleteSignal signalInfo)
        {
            ChangeState(_menuStateFactory.Create());
        }

        public void StartGame(StartGameSignal signalInfo)
        {
            ChangeState(_gameplayStateFactory.Create());
        }
    }
}