using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LabMaterials
{
    public class Tree : MonoBehaviour
    {
        // maybe monkey enemies or something will be spawned and then can attack the player if they enter a boundary trigger
        public GameObject Canopy;

        [System.Serializable]
        public class TreeCut : UnityEvent<Tree, int>
        {


        }

        [SerializeField]
        private int _treeHits;

        [Range(0, 10)]
        [SerializeField]
        private int _treeHitpoints = 10;

        [SerializeField]
        public TreeCut OnTreeCut;
        
        public System.Action<Tree, int> OnTreeCutAction;

        private void Awake()
        {


        }

        public void HitTree()
        {
            _treeHits++;

            OnTreeCutAction?.Invoke(this, _treeHits);
            OnTreeCut.Invoke(this, _treeHits);

            if(_treeHits >=_treeHitpoints)
            {
                // kill tree
            }
        }
    }
}
