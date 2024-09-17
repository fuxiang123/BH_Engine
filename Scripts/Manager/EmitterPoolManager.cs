using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BH_Engine
{
    internal class EmitterPoolManager : MonoBehaviour
    {
        public static EmitterPoolManager Instance { get; private set; }
        public GameObject Prefab;
        // 初始化子弹池数量
        [Range(0, 2000)]
        public int defaultCapacity = 100;
        public int maxSize = 2000;

        public bool collectionCheck = true;
        public ObjectPool<GameObject> pool;
        protected void Awake()
        {
            pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);
            Instance = this;
        }

        private GameObject createFunc()
        {
            var obj = Instantiate(Prefab);
            obj.SetActive(false);
            return obj;
        }

        private void actionOnGet(GameObject obj)
        {
            obj.SetActive(true);
        }

        private void actionOnRelease(GameObject obj)
        {
            obj.SetActive(false);
        }

        private void actionOnDestroy(GameObject obj)
        {
            Destroy(obj);
        }

        public GameObject Get()
        {
            return pool.Get();
        }

        public GameObject[] Get(int count)
        {
            GameObject[] objs = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                objs[i] = pool.Get();
            }
            return objs;
        }

        public void Release(GameObject obj)
        {
            pool.Release(obj);
        }

        public void Release(GameObject[] obj)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                pool.Release(obj[i]);
            }
        }
    }
}