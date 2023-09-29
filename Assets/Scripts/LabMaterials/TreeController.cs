using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabMaterials
{
    public class TreeController : MonoBehaviour
    {
        // now I will contact thee
        private void Start()
        {
            var gameManager = GameManager.Instance;

            foreach (var tree in gameManager.Trees)
            {
                var treeCanopy = tree.Canopy;

                tree.OnTreeCut.RemoveListener(OnTreeCut);
                tree.OnTreeCut.AddListener(OnTreeCut);

                tree.OnTreeCut.AddListener((tree, treeHits) => Debug.Log(string.Format("Tree was hit {0} times", treeHits), tree));

                // 1 in 2 chance of spawning enemy monkey
                if (SpawnRandomly(2))
                {
                    // TODO - Monkey Business
                }
            }
        }


        private void OnTreeCut(Tree tree, int hits)
        {
            Debug.Log("Stop destroying my forest!", this);
        }

        private bool SpawnRandomly(int chanceIn)
        {
            if (chanceIn < 1)
                chanceIn = 1;

            // if the max limit was a 1 in 2 chance, then it can either be a value of 1 or 2, so a 50-50 chance of occuring
            bool willSpawn = (chanceIn == UnityEngine.Random.Range(1, chanceIn));

            return willSpawn;
        }
    }
}
