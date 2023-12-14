using FX.Effects;
using UnityEngine;
using UnityEngine.Rendering;

namespace FX
{
    public class PostProcessingController : MonoBehaviour
    {
        private PostProcessingQue _postProcessingQue;
        [SerializeField] private Volume volume;
        void Start()
        {
            _postProcessingQue = PostProcessingQue.Instance;
            // _postProcessingQue.QueEffect(new Fear(5));
        }
        
        void Update()
        {
            // if (_postProcessingQue.IsEmpty())
            // {
            //
            // }
            _postProcessingQue.Progress(Time.deltaTime, volume);
        }
    }
}