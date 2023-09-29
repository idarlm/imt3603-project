using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LabMaterials
{
    public abstract class AbstractUnityTask : MonoBehaviour
    {
        [System.Serializable]
        public class UnityTaskEvent : UnityEvent<UnityTaskEvent>
        {
    
        }
    
        public enum UnityTaskState
        {
            Inactive,
            Running,
            Complete
        }
    
        [SerializeField]
        protected UnityEvent _onComplete;
    
        [SerializeField]
        protected UnityTaskState _state;
    
        public UnityTaskState State { get { return _state; } }
    
        public bool IsInactive { get { return _state == UnityTaskState.Inactive; } }
        public bool IsRunning { get { return _state == UnityTaskState.Running; } }
        public bool IsComplete { get { return _state == UnityTaskState.Complete; } }
        
        public void Register(UnityAction onCallback)
        {
            _onComplete.RemoveListener(onCallback);
            _onComplete.AddListener(onCallback);
        }
    
        public void Deregister(UnityAction onCallback)
        {
            _onComplete.RemoveListener(onCallback);
        }
    
        public virtual void ExecuteTask()
        {
            if (State != UnityTaskState.Inactive)
            {
                Debug.LogError("Cant execute since task state was: " + State, this);
                return;
            }
    
            StartCoroutine(ExecuteCoroutine());
            _state = UnityTaskState.Running;
        }
    
        protected abstract IEnumerator ExecuteCoroutine();
    
        public void ResetTask()
        {
            _state = UnityTaskState.Inactive;
            _onComplete.RemoveAllListeners();
    
            StopCoroutine(ExecuteCoroutine());
        }
    
        /*
         *  You must call this CompleteTask method at the end of your ExecuteCoroutine
         */
        public void CompleteTask()
        {
            _state = UnityTaskState.Complete;
            _onComplete.Invoke();
        }
    }
}
