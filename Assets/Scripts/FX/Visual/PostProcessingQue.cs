using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FX.Visual.Effects;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FX.Visual
{
    
    public sealed class PostProcessingQue
    {
        private static List<Queue<Effect>> _effectQueue = new List<Queue<Effect>>();
        
        private static PostProcessingQue instance = null;
        
        public static PostProcessingQue Instance {
            get { return instance ??= new PostProcessingQue(); }
        }
        
        public void QueEffect(Effect effect)
        {
            var queue = new Queue<Effect>();
            queue.Enqueue(effect);
            _effectQueue.Add(queue);
        }

        public void QueEffectSequence(Queue<Effect> effects)
        {
            _effectQueue.Add(effects);
        }

        public bool IsEmpty()
        {
            return _effectQueue.Count == 0;
        }

        public void Progress(float deltaTime, Volume volume)
        {
            if (_effectQueue.Count != 0)
            {
                _effectQueue = _effectQueue
                    .Select( x =>
                    {
                        if (!x.TryPeek(out var effect)) return x;
                        if (effect.IsDone())
                        {
                            x.Dequeue();
                        }
                        else
                        {
                            x.Peek().Progress(deltaTime, volume);
                        }
                        return x;
                    })
                    .Where(que => que.Count > 0)
                    .ToList();
            }
        }
    }
 }