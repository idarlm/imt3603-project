using System;
using UnityEngine;
using System.IO;

namespace Snapshot
{
    public class Snap : MonoBehaviour
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MouseGame");
        string filename = "snapshot.json";

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                var snap = WorldSnapshotManager.MakeSnapshot();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    File.Create(Path.Combine(path, filename)).Close();
                }

                File.WriteAllText(Path.Combine(path, filename), JsonUtility.ToJson(snap));
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                string text;
                try
                {
                    text = File.ReadAllText(Path.Combine(path, filename));
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return;
                }


                var snap = JsonUtility.FromJson<WorldSnapshot>(text);

                WorldSnapshotManager.LoadSnapshot(snap);
            }
        }
    }
}