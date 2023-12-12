using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

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
            var reader = (IWorldSnapshotReader)snapshot;
            foreach (var obj in objs)
            {
                obj.BroadcastMessage("OnLoadSnapshot", reader, SendMessageOptions.DontRequireReceiver);
            }

            Debug.Log($"World snapshot loaded in {System.DateTime.Now - ts}");
        }
    }
}
