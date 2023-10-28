using UnityEngine;

namespace LK.PaintingGame.Code.UI
{
    public class GameUI : MonoBehaviour
    {
        public MenuPanel menuPanel;
        public GamePanel gamePanel;

        [System.Serializable]
        public class Config
        {
            public GameObject colorSelectorPrefab;
        }
    }
}