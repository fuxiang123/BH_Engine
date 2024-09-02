using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BH_Engine
{
    internal class BulletPoolManager : MonoBehaviour
    {
        public static BulletPoolManager Instance { get; private set; }
        public GameObject PlayerPrefab;
        public GameObject EnemyPrefab;
        // 初始化子弹池数量
        [Range(0, 2000)]
        public int defaultCapacity = 100;
        public int maxSize = 2000;

        public bool collectionCheck = true;
        public ObjectPool<GameObject> playerPool;
        public ObjectPool<GameObject> enemyPool;

        protected void Awake()
        {
            playerPool = new ObjectPool<GameObject>(createPlayerFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);
            enemyPool = new ObjectPool<GameObject>(createEnemyFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);
            Instance = this;
        }

        protected void OnDisable()
        {
            playerPool.Dispose();
            enemyPool.Dispose();
        }

        public void ClearAll()
        {
            playerPool.Clear();
            enemyPool.Clear();
        }

        private GameObject createPlayerFunc()
        {
            var obj = Instantiate(PlayerPrefab);
            return obj;
        }

        private GameObject createEnemyFunc()
        {
            var obj = Instantiate(EnemyPrefab);
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
        #region 玩家对象池操作
        public GameObject GetPlayerBullet()
        {
            return playerPool.Get();
        }

        public GameObject[] GetPlayerBullets(int count)
        {
            GameObject[] objs = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                objs[i] = playerPool.Get();
            }
            return objs;
        }

        public void ReleasePlayerBullet(GameObject obj)
        {
            playerPool.Release(obj);
        }

        public void ReleasePlayerBullets(GameObject[] obj)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                playerPool.Release(obj[i]);
            }
        }
        #endregion

        #region 敌人对象池操作
        public GameObject GetEnemyBullet()
        {
            return enemyPool.Get();
        }

        public GameObject[] GetEnemyBullets(int count)
        {
            GameObject[] objs = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                objs[i] = enemyPool.Get();
            }
            return objs;
        }

        public void ReleaseEnemyBullet(GameObject obj)
        {
            enemyPool.Release(obj);
        }

        public void ReleaseEnemyBullets(GameObject[] obj)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                enemyPool.Release(obj[i]);
            }
        }
        #endregion
    }
}