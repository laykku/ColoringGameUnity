using UnityEngine;

namespace LK.PaintingGame.Code
{
    public class Painter : MonoBehaviour
    {
        [SerializeField] private MeshRenderer mr;
        [SerializeField] private ComputeShader cs;
        [SerializeField] private Texture bg;
        [SerializeField] private Texture mask;
        
        private bool _alpha = false;

        private void Start()
        {
 
        }



        private void Update()
        {

        }
    }
}