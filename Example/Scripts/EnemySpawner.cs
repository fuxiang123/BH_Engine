using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BH_Engine.Example
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner instance;
        public GameObject enemyPrefab;
        // 生成速度
        public float spawnInterval = 1;
        // 每次生成数量
        public int spawnCountPerTime = 1;
        // 生成最大数量
        public int maxSpawnCount = 10;
        // 生成范围
        public float spawnRadius = 1;
        // 生成数量
        [HideInInspector]
        public int spawnCount = 0;

        private float timeCount = 0;
        // 对象池
        private List<GameObject> enemyPool = new List<GameObject>();

        // Start is called before the first frame update
        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            for (int i = 0; i < maxSpawnCount; i++)
            {
                var enemy = Instantiate(enemyPrefab, transform);
                enemy.SetActive(false);
                enemyPool.Add(enemy);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (spawnCount >= maxSpawnCount)
            {
                return;
            }

            timeCount += Time.deltaTime;
            if (timeCount >= spawnInterval)
            {
                for (int i = 0; i < spawnCountPerTime; i++)
                {
                    SpawnEnemy();
                    spawnCount++;
                }
                timeCount = 0;
            }
        }

        void SpawnEnemy()
        {
            var enemy = enemyPool.Find(e => !e.activeSelf);
            if (enemy == null)
            {
                enemy = Instantiate(enemyPrefab, transform);
                enemyPool.Add(enemy);
            }
            enemy.transform.position = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0);
            enemy.SetActive(true);
        }
    }
}