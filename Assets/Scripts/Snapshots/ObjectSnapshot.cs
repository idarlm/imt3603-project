using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snapshot
{
    /// <summary>
    /// IObjectSnapshot is the interface that allows for 
    /// creation of snapshot objects using builder syntax.
    /// </summary>
    public interface IObjectSnapshotWriter
    {
        public IObjectSnapshotWriter Add(string key, string value);
        public IObjectSnapshotWriter Add(string key, int value);
        public IObjectSnapshotWriter Add(string key, float value);
        public IObjectSnapshotWriter Add(string key, Vector3 value);
        public IObjectSnapshotWriter Add(string key, Quaternion value);
    }

    /// <summary>
    /// IObjectSnapshotReader is the interface provided when loading data from a snapshot.
    /// </summary>
    public interface IObjectSnapshotReader
    {
        public string GetString(string key);
        public int GetInt(string key);
        public float GetFloat(string key);
        public Vector3 GetVector3(string key);
        public Quaternion GetQuaternion(string key);
    }

    [Serializable]
    struct SnapshotProperty
    {
        public string name;
        public Vector3 vecValue;
        public Quaternion quatValue;
        public float floatValue;
        public int intValue;
        public string stringValue;
    }

    /// <summary>
    /// ObjectSnapshot objects allow data to be stored in dictionaries. 
    /// Using the reader and writer interfaces, it acts like a key/value store.
    /// 
    /// The implementation of this is not super safe or performant, but since it uses interfaces
    /// the implementation (or the entire class itself) could be replaced in the future while staying backwards compatible.
    /// </summary>
    [Serializable]
    class ObjectSnapshot : IObjectSnapshotWriter, IObjectSnapshotReader
    {
        // Maps to store key and value pairs
        [SerializeField] public string name;
        [SerializeField] public Vector3 position;
        [SerializeField] public Quaternion rotation;
        [SerializeField] public Vector3 scale;

        //private readonly Dictionary<string, string> _strings = new();
        //private readonly Dictionary<string, float> _floats = new();
        //private readonly Dictionary<string, int> _ints = new();

        [SerializeField] private SnapshotProperty[] _properties = new SnapshotProperty[0];

        // Pre-allocating the array and resizing it exponentially would be faster
        // but that would be an optimization for the future.
        private void addProperty(SnapshotProperty property)
        {
            var l = _properties.Length;
            var newArray = new SnapshotProperty[l + 1];
            for (int i = 0; i < l; i ++)
            {
                newArray[i] = _properties[i];
            }
            newArray[l] = property;

            _properties = newArray;
        }

        private SnapshotProperty findProperty(string name)
        {
            foreach (var p in _properties)
            {
                if (p.name == name) return p;
            }

            return new SnapshotProperty { name = "NOT FOUND" };
        }

        public IObjectSnapshotWriter Add(string key, float value)
        {
            addProperty(new SnapshotProperty { name = key, floatValue = value });
            return this;
        }

        public IObjectSnapshotWriter Add(string key, int value)
        {
            addProperty(new SnapshotProperty { name = key, intValue = value });
            return this;
        }

        public IObjectSnapshotWriter Add(string key, Quaternion value)
        {
            addProperty(new SnapshotProperty { name = key, quatValue = value });
            return this;
        }

        public IObjectSnapshotWriter Add(string key, string value)
        {
            addProperty(new SnapshotProperty { name = key, stringValue = value });
            return this;
        }

        public IObjectSnapshotWriter Add(string key, Vector3 value)
        {
            addProperty(new SnapshotProperty { name = key, vecValue = value });
            return this;
        }

        public float GetFloat(string key)
        {
            return findProperty(key).floatValue;
        }

        public int GetInt(string key)
        {
            return findProperty(key).intValue;
        }

        public Quaternion GetQuaternion(string key)
        {
            return findProperty(key).quatValue;
        }

        public string GetString(string key)
        {
            return findProperty(key).stringValue;
        }

        public Vector3 GetVector3(string key)
        {
            return findProperty(key).vecValue;
        }
    }
}
