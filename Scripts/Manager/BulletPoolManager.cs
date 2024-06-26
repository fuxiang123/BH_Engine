using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BH_Engine
{
    public class BulletPoolManager : PoolManager
    {
        public static BulletPoolManager Instance { get; private set; }
        protected new void Awake()
        {
            Instance = this;
            base.Awake();
        }
    }
}