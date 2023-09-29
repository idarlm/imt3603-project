using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This code could be used within a scene's setup component
namespace LabMaterials
{
    public class TaskRunner : MonoBehaviour
    {
        [SerializeField]
        private List<AbstractUnityTask> _tasks;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            foreach(var task in _tasks)
            {
                task.ExecuteTask();

                while (!task.IsComplete)
                    yield return null;
            }

            OnLoaded();
        }

        public virtual void OnLoaded()
        {
            // Continue with game
        }
    }
}
