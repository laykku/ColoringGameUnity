using LK.PaintingGame.Code.UI;
using Zenject;

namespace LK.PaintingGame.Code.GameStates
{
    public class MenuState : IGameState
    { 
        private readonly GameController _gameController;
        private readonly GameUI _gameUI;
        
        public MenuState(GameController gameController, GameUI gameUI)
        {
            _gameController = gameController;
            _gameUI = gameUI;
        }

        public void Enter()
        {
            _gameUI.menuPanel.Show();
        }

        public void Update()
        {
        }

        public void Exit()
        {
            _gameUI.menuPanel.Hide();
        }
        
        public class Factory : PlaceholderFactory<MenuState>
        {
            
        }
    }
}