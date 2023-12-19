using FX.Visual.Effects;
using UnityEngine;
using UnityEngine.Rendering;

namespace FX.Visual
{
    public class PostProcessingController : MonoBehaviour
    {
        private PostProcessingQue _postProcessingQue;
        [SerializeField] private Volume volume;
        void Start()
        {
            AIInteractionFXManager.Instance.PostProcessingController = this;
            _postProcessingQue = PostProcessingQue.Instance;
            _postProcessingQue.QueEffect(new FadeToColor(Color.black,0.001f));
            _postProcessingQue.QueEffect(new FadeToColor(Color.white,4f));
        }
        
        void Update()
        {
            _postProcessingQue.Progress(Time.deltaTime, volume);
        }
    }
}