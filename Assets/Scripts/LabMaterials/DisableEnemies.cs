using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabMaterials
{
    public class DisableEnemies : AbstractCommand
    {
        [SerializeField]
        private List<GameObject> _gameObjects;

        public override void Execute()
        {
            foreach(var gameObject in _gameObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
