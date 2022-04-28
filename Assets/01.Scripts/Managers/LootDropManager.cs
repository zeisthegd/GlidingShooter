using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using Penwyn.Tools;

namespace Penwyn.Game
{
    public class LootDropManager : SingletonMonoBehaviour<LootDropManager>
    {
        public MapData MapData;
        public DestructileTilemap _destructileTilemap;
        protected ObjectPooler _coinPooler;

        protected virtual void Awake()
        {
            _coinPooler = GetComponent<ObjectPooler>();
        }

        protected virtual void OnEnable()
        {

        }
        protected virtual void OnDisable()
        {

        }

    }
}

