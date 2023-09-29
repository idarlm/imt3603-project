using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LabMaterials
{
    public class LoaderUnityTask : AbstractUnityTask
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private float _sliderInterval = 0.25f;

        [SerializeField]
        private float _sliderIncrement = 0.15f;

        protected override IEnumerator ExecuteCoroutine()
        {
            // DO stuff - procedural building, loading, whatever

            while(_slider.value < 1)
            {
                _slider.value = Mathf.Clamp(_slider.value + _sliderIncrement * Time.deltaTime, 0f, 1f);
                yield return new WaitForSeconds(_sliderInterval);
            }

            CompleteTask();
        }
    }
}
