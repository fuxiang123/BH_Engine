using System.Collections.Generic;
using System.Linq;
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

        public HashSet<GameObject> ActivePlayerBullets = new HashSet<GameObject>();
        public HashSet<GameObject> ActiveEnemyBullets = new HashSet<GameObject>();

        protected void Awake()
        {
            playerPool = new ObjectPool<GameObject>(createPlayerFunc, actionOnPlayerBulletGet, actionOnPlayerBulletRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);
            enemyPool = new ObjectPool<GameObject>(createEnemyFunc, actionOnEnemyBulletGet, actionOnEnemyBulletRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);
            Instance = this;
        }

        protected void OnDisable()
        {
            ClearAllPool();
        }

        // 清空所有对象迟
        public void ClearAllPool()
        {
            playerPool.Clear();
            enemyPool.Clear();
        }

        // 释放所有子弹
        public void ReleaseAllBullets()
        {
            ReleaseAllPlayerBullets();
            ReleaseAllEnemyBullets();
        }

        private void actionOnDestroy(GameObject obj)
        {
            Destroy(obj);
        }

        #region 玩家对象池初始化
        private GameObject createPlayerFunc()
        {
            var obj = Instantiate(PlayerPrefab);
            return obj;
        }

        private void actionOnPlayerBulletGet(GameObject obj)
        {
            obj.SetActive(true);
            ActivePlayerBullets.Add(obj);
        }

        private void actionOnPlayerBulletRelease(GameObject obj)
        {
            obj.SetActive(false);
            ActivePlayerBullets.Remove(obj);
        }
        #endregion

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

        // 回收所有玩家子弹
        public void ReleaseAllPlayerBullets()
        {
            var bullets = ActivePlayerBullets.ToArray();
            foreach (var bullet in bullets)
            {
                playerPool.Release(bullet);
            }
            ActivePlayerBullets.Clear();
        }

        // 清除所有玩家子弹并销毁
        public void ClearAllPlayerBullets()
        {
            var bullets = ActivePlayerBullets.ToArray();
            foreach (var bullet in bullets)
            {
                Destroy(bullet);
            }
            ActivePlayerBullets.Clear();
            playerPool.Clear();
        }
        #endregion

        #region 敌人对象池初始化

        private GameObject createEnemyFunc()
        {
            var obj = Instantiate(EnemyPrefab);
            return obj;
        }

        private void actionOnEnemyBulletGet(GameObject obj)
        {
            obj.SetActive(true);
            ActiveEnemyBullets.Add(obj);
        }

        private void actionOnEnemyBulletRelease(GameObject obj)
        {
            obj.SetActive(false);
            ActiveEnemyBullets.Remove(obj);
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

        // 回收所有敌人子弹
        public void ReleaseAllEnemyBullets()
        {
            var bullets = ActivePlayerBullets.ToArray();
            foreach (var bullet in bullets)
            {
                enemyPool.Release(bullet);
            }
            ActivePlayerBullets.Clear();
        }

        // 清除所有敌人子弹并销毁
        public void ClearAllEnemyBullets()
        {
            var bullets = ActiveEnemyBullets.ToArray();
            foreach (var bullet in bullets)
            {
                Destroy(bullet);
            }
            ActiveEnemyBullets.Clear();
            enemyPool.Clear();
        }
        #endregion
    }
}