using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabMaterials.Utils
{
    public class SingletonManager<T> : MonoBehaviour
        where T : MonoBehaviour
    {

        private static SingletonManager<T> _instance;

        public static T Instance
        {
            get
            {
                if(_instance != null)
                {
                    return _instance as T;
                }
                else
                {
                    _instance = FindObjectOfType(typeof(T)) as SingletonManager<T>;

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        singleton.name = typeof(T).ToString();

                        var component = singleton.AddComponent<T>() as SingletonManager<T>;

                        if (Application.isPlaying)
                            DontDestroyOnLoad(singleton);

                        _instance = component;

                        return _instance as T;
                    }

                    return _instance as T;
                }
            }
        }
    }
}
