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

    /// <summary>
    /// ObjectSnapshot objects allow data to be stored in dictionaries. 
    /// Using the reader and writer interfaces, it acts like a key/value store.
    /// 
    /// The implementation of this is not super safe or performant, but since it uses interfaces
    /// the implementation (or the entire class itself) could be replaced in the future while staying backwards compatible.
    /// </summary>
    class ObjectSnapshot : IObjectSnapshotWriter, IObjectSnapshotReader
    {
        // Maps to store key and value pairs
        private readonly Dictionary<string, string> _strings = new();
        private readonly Dictionary<string, float> _floats = new();
        private readonly Dictionary<string, int> _ints = new();


        public IObjectSnapshotWriter Add(string key, float value)
        {
            _floats.Add(key, value);
            return this;
        }

        public IObjectSnapshotWriter Add(string key, int value)
        {
            _ints.Add(key, value);
            return this;
        }

        public IObjectSnapshotWriter Add(string key, Quaternion value)
        {
            _floats.Add($"__QUATERNION_X_{key}", value.x);
            _floats.Add($"__QUATERNION_Y_{key}", value.y);
            _floats.Add($"__QUATERNION_Z_{key}", value.z);
            _floats.Add($"__QUATERNION_W_{key}", value.w);
            return this;
        }

        public IObjectSnapshotWriter Add(string key, string value)
        {
            _strings.Add(key, value);
            return this;
        }

        public IObjectSnapshotWriter Add(string key, Vector3 value)
        {
            _floats.Add($"__VECTOR3_X_{key}", value.x);
            _floats.Add($"__VECTOR3_Y_{key}", value.y);
            _floats.Add($"__VECTOR3_Z_{key}", value.z);
            return this;
        }

        public float GetFloat(string key)
        {
            var success = _floats.TryGetValue(key, out float result);
            if (success) return result;
            else
            {
                Debug.LogWarning("Could not find float with key \"{key}\".");
                return default;
            }
        }

        public int GetInt(string key)
        {
            var success = _ints.TryGetValue(key, out int result);
            if (success) return result;
            else
            {
                Debug.LogWarning("Could not find int with key \"{key}\".");
                return default;
            }
        }

        public Quaternion GetQuaternion(string key)
        {
            var success = true;
            Quaternion q = new();

            string[] components = { "X", "Y", "Z", "W" };
            Dictionary<string, float> vals = new();

            foreach (var c in components)
            {
                success &= _floats.TryGetValue($"__QUATERNION_{c}_{key}", out float value);

                if (success)
                {
                    vals.Add(c, value);
                }
            }

            if (success)
            {
                q.x = vals["X"];
                q.y = vals["Y"];
                q.z = vals["Z"];
                q.w = vals["W"];

                return q;
            }

            Debug.LogWarning($"Could not find Quaternion with key \"{key}\"");
            return default;
        }

        public string GetString(string key)
        {
            var success = _strings.TryGetValue(key, out string result);
            if (success) return result;
            else
            {
                Debug.LogWarning("Could not find string with key \"{key}\".");
                return default;
            }
        }

        public Vector3 GetVector3(string key)
        {
            var success = true;
            Vector3 vec = new();

            string[] components = { "X", "Y", "Z" };
            Dictionary<string, float> vals = new();

            foreach (var c in components)
            {
                success &= _floats.TryGetValue($"__VECTOR3_{c}_{key}", out float value);

                if (success)
                {
                    vals.Add(c, value);
                }
            }

            if (success)
            {
                vec.x = vals["X"];
                vec.y = vals["Y"];
                vec.z = vals["Z"];

                return vec;
            }

            Debug.LogWarning($"Could not find Vector3 with key \"{key}\".");
            return default;
        }
    }
}