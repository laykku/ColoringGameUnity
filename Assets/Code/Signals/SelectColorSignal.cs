using UnityEngine;

namespace LK.PaintingGame.Code.Signals
{
    public class SelectColorSignal
    {
        public readonly Color Color;
        
        public SelectColorSignal(Color color)
        {
            Color = color;
        }
    }
}