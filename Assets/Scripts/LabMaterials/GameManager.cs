using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LabMaterials.Utils;

namespace LabMaterials
{
    public class GameManager : SingletonManager<GameManager>
    {
        [SerializeField]
        private List<Tree> _trees;

        public IEnumerable<Tree> Trees { get { return _trees; } }
    }
}
