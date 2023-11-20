using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Snapshot {
    /// <summary>
    /// Contains static methods to create and load snapshots of world state.
    /// </summary>
    public static class WorldSnapshotManager
    {
        /// <summary>
        /// Create a snapshot of all snapshotable GameObjects in the current scene.
        /// The snapshot should be serializable.
        /// </summary>
        /// <returns>IWorldSnapshot containing the world state.</returns>
        public static IWorldSnapshot MakeSnapshot()
        {
            Debug.Log("Making world snapshot...");
            var timeStamp = System.DateTime.Now;
            var scene = SceneManager.GetActiveScene();

            // Get list of all non-static GameObjects using LINQ
            var objs =
                from obj in scene.GetRootGameObjects()
                where !obj.isStatic
                select obj;

            // Create new WorldSnapshot
            var ws = new WorldSnapshot(timeStamp);
            
            // Send OnMakeSnapshot message to all objects, with a reference to ws
            foreach (var obj in objs)
            {
                obj.BroadcastMessage("OnMakeSnapshot", ws, SendMessageOptions.DontRequireReceiver);
            }

            // Log stats
            Debug.Log($"World snapshot made in {System.DateTime.Now - timeStamp}");

            return ws;
        }

        /// <summary>
        /// Loads the state stored in the world snapshot
        /// </summary>
        /// <param name="snapshot"></param>
        public static void LoadSnapshot(IWorldSnapshot snapshot)
        {
            Debug.Log("Loading world snapshot...");
            var ts = System.DateTime.Now;

            // reload scene?

            // Get list of all non-static GameObjects using LINQ
            // (i love LINQ)
            var scene = SceneManager.GetActiveScene();
            var objs =
                from obj in scene.GetRootGameObjects()
                where !obj.isStatic
                select obj;

            // Send OnMakeSnapshot message to all objects, with a reference to ws
            foreach (var obj in objs)
            {
                obj.BroadcastMessage("OnLoadSnapshot", snapshot, SendMessageOptions.DontRequireReceiver);
            }

            Debug.Log($"World snapshot loaded in {System.DateTime.Now - ts}");
        }
    }

    /// <summary>
    /// ISnapshotable is an interface that MonoBehaviours can implement
    /// to allow snapshots to be made.
    /// </summary>
    public interface ISnapshotable
    {
        void OnMakeSnapshot(IWorldSnapshot ws);
        void OnLoadSnapshot(IWorldSnapshot ws);
    }

    /// <summary>
    /// IWorldSnapshot defines the methods and properties of a WorldSnapshot.
    /// </summary>
    public interface IWorldSnapshot
    {
        /// <summary>
        /// Add an Object snapshot.
        /// </summary>
        /// <param name="obj"></param>
        public void Add(IObjectSnapshotWriter obj);

        /// <summary>
        /// Create and add an object snapshot of obj.
        /// Additional values can be added to the Snapshot
        /// using builder like syntax.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IObjectSnapshotWriter AddSnapshotOf(MonoBehaviour obj);
        public IObjectSnapshotReader LoadSnapshotOf(MonoBehaviour obj);
    }

    /// <summary>
    /// IObjectSnapshot allows for creation of a snapshot object using builder syntax.
    /// </summary>
    public interface IObjectSnapshotWriter
    {
        public IObjectSnapshotWriter Add(string key, string value);
        public IObjectSnapshotWriter Add(string key, int value);
        public IObjectSnapshotWriter Add(string key, float value);
        public IObjectSnapshotWriter Add(string key, Vector3 value);
        public IObjectSnapshotWriter Add(string key, Quaternion value);
    }

    public interface IObjectSnapshotReader
    {
        public string GetString(string key);
        public int GetInt(string key);
        public float GetFloat(string key);
        public Vector3 GetVector3(string key);
        public Quaternion GetQuaternion(string key);
    }

    class WorldSnapshot : IWorldSnapshot
    {
        public WorldSnapshot(System.DateTime timestamp)
        {
            this.timestamp = timestamp;
            scene = SceneManager.GetActiveScene().name;
        }

        private readonly List<ObjectSnapshot> _objects = new();
        public readonly System.DateTime timestamp;
        public readonly string scene;

        public void Add(IObjectSnapshotWriter obj)
        {
            // TODO: Make this safe.
            _objects.Add(obj as ObjectSnapshot);
        }

        public IObjectSnapshotWriter AddSnapshotOf(MonoBehaviour m)
        {
            // Create a unique and persistent name for the object
            var name = GetFullName(m);
            Debug.Log($"Creating snapshot of: {name}");
            
            // Take snapshot of transform
            var snap = new ObjectSnapshot();
            snap.Add("__NAME", name);
            snap.Add("__TRANSFORM_POS", m.transform.position);
            snap.Add("__TRANSFORM_SCALE", m.transform.localScale);
            snap.Add("__TRANSFORM_ROT", m.transform.rotation);

            _objects.Add(snap);

            return snap;
        }

        public IObjectSnapshotReader LoadSnapshotOf(MonoBehaviour obj)
        {
            var name = GetFullName(obj);
            Debug.Log($"Loading snapshot of: {name}");

            // Find snapshot of object with given root
            var s = from o in _objects
                       where o.GetString("__NAME") == name
                       select o;

            if (s.Count() == 0)
            {
                Debug.LogWarning($"Could not find snapshot of: {name}");
                return null;
            }

            var snapshot = s.FirstOrDefault();

            // Load transform snapshot state - this is inefficient but convenient
            obj.transform.position = snapshot.GetVector3("__TRANSFORM_POS");
            obj.transform.localScale = snapshot.GetVector3("__TRANSFORM_SCALE");
            obj.transform.rotation = snapshot.GetQuaternion("__TRANSFORM_ROT");

            // TODO: make this safe
            return snapshot;
        }

        /// <summary>
        /// Creates a unique and persistent name for any given MonoBehaviour.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        private string GetFullName(MonoBehaviour m)
        {
            var name = m.ToString() + m.name;
            Transform parent = m.transform.parent;
            while (parent != null)
            {
                name += parent.name;
            }
            
            //TODO: Hash the name value?
            return name;
        }
    }

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

            foreach ( var c in components )
            {
                success &= _floats.TryGetValue($"__QUATERNION_{c}_{key}", out float value);

                if (success) {
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