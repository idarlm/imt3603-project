using Snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snap : MonoBehaviour
{
    WorldSnapshot snapshot;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            snapshot = WorldSnapshotManager.MakeSnapshot();
        }

        if (Input.GetKeyDown(KeyCode.F9) && snapshot != null)
        {
            WorldSnapshotManager.LoadSnapshot(snapshot);
        }
    }
}
