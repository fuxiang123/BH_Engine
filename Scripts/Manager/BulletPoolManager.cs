using System.Collections.Generic;
using UnityEngine;

namespace BH_Engine
{
    // 子弹池管理器
    public class BulletPoolManager : MonoBehaviour
    {
        // 子弹池
        private Queue<GameObject> poolQueue = new Queue<GameObject>();
        // 初始化子弹prefab
        public GameObject bulletPrefab;
        // 初始化子弹池数量
        [Range(0, 2000)]
        public int initCount;
        public int poolCount;
        // 单例
        public static BulletPoolManager instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            InitBulletPool();
        }

        // 初始化子弹池
        public void InitBulletPool()
        {
            poolQueue.Clear();
            poolCount = initCount;
            for (int i = 0; i < initCount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                bullet.transform.SetParent(transform);
                poolQueue.Enqueue(bullet);
            }
        }

        // 从子弹池中获取子弹
        public GameObject[] GetBullet(int count)
        {
            GameObject[] bullets = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                if (poolQueue.Count <= poolCount * 0.1)
                {
                    // 扩容15%
                    poolCount = (int)(poolCount * 1.15f);
                    for (int j = 0; j < poolCount; j++)
                    {
                        GameObject bullet = Instantiate(bulletPrefab);
                        bullet.SetActive(false);
                        bullet.transform.SetParent(transform);
                        poolQueue.Enqueue(bullet);
                    }
                }
                bullets[i] = poolQueue.Dequeue();
            }
            return bullets;
        }

        // 回收子弹
        public void RecycleBullet(GameObject bullet)
        {
            bullet.SetActive(false);
            poolQueue.Enqueue(bullet);
        }

        // 回收多个子弹
        public void RecycleBullet(GameObject[] bullets)
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].SetActive(false);
                poolQueue.Enqueue(bullets[i]);
            }
        }
    }
}