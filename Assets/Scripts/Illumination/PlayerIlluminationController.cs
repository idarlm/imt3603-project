using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Illumination
{
    public class PlayerIlluminationController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            IlluminationChannel.Instance.ResetIllumination();
        }
    }
}

