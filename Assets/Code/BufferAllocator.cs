using UnityEngine;

namespace LK.PaintingGame.Code
{
    public class BufferAllocator : MonoBehaviour
    {
        private ComputeBuffer _buffer;
        
        public ComputeBuffer AllocateIntBuffer()
        {
            _buffer?.Dispose();
            _buffer = new ComputeBuffer(1, sizeof(int));
            return _buffer;
        }

        public void ReleaseBuffer()
        {
            _buffer.Dispose();
        }

        private void OnDestroy()
        {
            ReleaseBuffer();
        }
    }
}