using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BH_Engine
{
    public abstract class PoolManager : MonoBehaviour
    {
        // 初始化子弹prefab
        public GameObject Prefab;
        // 初始化子弹池数量
        [Range(0, 2000)]
        public int defaultCapacity = 200;
        public int maxSize = 10000;

        public bool collectionCheck = true;
        public ObjectPool<GameObject> pool;
        protected void Awake()
        {
            pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);
        }

        private GameObject createFunc()
        {
            var obj = Instantiate(Prefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
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