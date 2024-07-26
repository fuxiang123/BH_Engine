using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BH_Engine.Example
{
    public class EnemyMove : MonoBehaviour
    {
        // 敌人随机移动
        public bool randomMove = false;
        // 敌人向玩家移动
        public bool moveToPlayer = false;
        // 敌人移动速度
        public float moveSpeed = 0.1f;
        // 随机移动范围
        public float randomRange = 1;
        // 移动频率
        public float moveFrequency = 1;
        private float timeCount = 0;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            timeCount += Time.deltaTime;
            if (randomMove && timeCount > moveFrequency)
            {
                timeCount = 0;
                RandomMove();
            }
            if (moveToPlayer)
            {
                MoveToPlayer();
            }
        }

        void RandomMove()
        {
            transform.position += new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), 0);
        }

        void MoveToPlayer()
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}