using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BH_Engine
{
    public class EmitterPoolManager : PoolManager
    {
        public static EmitterPoolManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            base.Awake();
        }
    }
}