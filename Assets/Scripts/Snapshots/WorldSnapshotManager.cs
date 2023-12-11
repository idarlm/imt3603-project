using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Snapshot {
    /// <summary>
    /// ISnapshotable is an interface that MonoBehaviours can implement
    /// to allow snapshots to be made.
    /// </summary>
    public interface ISnapshotable
    {
        void OnMakeSnapshot(IWorldSnapshotWriter ws);
        void OnLoadSnapshot(IWorldSnapshotReader ws);
    }

    /// <summary>
    /// IWorldSnapshotWriter defines the usage of a world snapshot during the
    /// creation process.
    /// </summary>
    public interface IWorldSnapshotWriter
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
    }

    /// <summary>
    /// IWorldSnapshotReader defines the usage of a world snapshot 
    /// during the data loading phase.
    /// </summary>
    public interface IWorldSnapshotReader
    {
       public IObjectSnapshotReader LoadSnapshotOf(MonoBehaviour obj);
    }

    /// <summary>
    /// Contains static methods to create and load snapshots of world state.
    /// </summary>
    public static class WorldSnapshotManager
    {
        /// <summary>
        /// Create a snapshot of all snapshotable GameObjects in the current scene.
        /// The snapshot is serializable.
        /// </summary>
        /// <returns>WorldSnapshot containing the world state.</returns>
        public static WorldSnapshot MakeSnapshot()
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
        public static void LoadSnapshot(WorldSnapshot snapshot)
        {
            Debug.Log("Loading world snapshot...");
            var ts = System.DateTime.Now;

            // reload scene?

            // Get list of all non-static GameObjects using LINQ
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
    /// WorldSnapshot implements the world snapshot reader and writer interfaces.
    /// It lets users create and load snapshots by providing a monobehaviour,
    /// and is serializable to allow for persistent storage.
    /// </summary>
    public class WorldSnapshot : IWorldSnapshotWriter
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
}
