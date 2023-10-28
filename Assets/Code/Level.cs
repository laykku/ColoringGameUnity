using UnityEngine;

namespace LK.PaintingGame.Code
{
    [System.Serializable]
    public class Level
    {
        public Texture backgroundTexture;
        public Texture maskTexture;
        public Texture checkTexture;
        public Texture refTexture;
        [ColorUsage(false)]
        public Color[] colors;
        public float cameraSize;
    }
}