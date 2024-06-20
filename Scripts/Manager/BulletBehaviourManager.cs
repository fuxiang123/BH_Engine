
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BH_Engine
{
    public class ActiveBullet
    {
        public GameObject bullet;
        public Vector3 spwanPosition;
        public float currentTime;
        public BulletFinalConfig bulletFinalConfig;
    }

    // 统一更新所有子弹的移动逻辑
    public class BulletBehaviourManager : MonoBehaviour
    {
        public static BulletBehaviourManager instance;

        // 正在移动的子弹
        public List<ActiveBullet> activeBullets = new List<ActiveBullet>();
        // 需要移除的子弹
        private List<ActiveBullet> bulletToRemove = new List<ActiveBullet>();


        private void Awake()
        {
            instance = this;
        }

        private void FixedUpdate()
        {
            if (activeBullets.Count == 0)
            {
                return;
            }

            foreach (var item in activeBullets)
            {
                if (item.bullet != null && item.bullet.gameObject.activeSelf)
                {
                    UpdateSingleBulletBehaviour(item);
                }
                else
                {
                    bulletToRemove.Add(item);
                    continue;
                }
            }

            for (int i = 0; i < bulletToRemove.Count; i++)
            {
                var activeBullet = bulletToRemove[i];
                activeBullets.Remove(activeBullet);
                if (activeBullet.bullet != null && activeBullet.bullet.gameObject.activeSelf)
                {
                    BulletPoolManager.pool.Release(bulletToRemove[i].bullet);
                }
            }
            bulletToRemove.Clear();
        }

        // 更新单个子弹行为
        private void UpdateSingleBulletBehaviour(ActiveBullet activeBullet)
        {
            activeBullet.currentTime += Time.deltaTime;
            float timer = activeBullet.currentTime;
            float speed = activeBullet.bulletFinalConfig.speed;
            float maxDistance = activeBullet.bulletFinalConfig.maxDistance;
            // 加速度
            float acceleration = activeBullet.bulletFinalConfig.acceleration;
            float bulletRotateSpeed = activeBullet.bulletFinalConfig.bulletRotate;
            Vector2 spawn = activeBullet.spwanPosition;
            Vector2 dir = activeBullet.bullet.transform.up;
            activeBullet.bullet.transform.position = CalculateNextPosition(spawn, dir, speed, timer, acceleration);
            activeBullet.bullet.transform.rotation = Quaternion.Euler(0, 0, activeBullet.bullet.transform.rotation.eulerAngles.z + bulletRotateSpeed * Time.deltaTime);

            // 当前移动距离
            var distance = CalculateTotalDistance(speed, timer, acceleration);
            if (activeBullet.currentTime >= activeBullet.bulletFinalConfig.lifeTime || (maxDistance > 0 && distance >= maxDistance))
            {
                bulletToRemove.Add(activeBullet);
            }
        }

        // 计算当前总移动距离
        public float CalculateTotalDistance(
            float speed,
            float lifeTime,
            float acceleration = 0
        )
        {
            return speed * lifeTime + acceleration * lifeTime * lifeTime / 2;
        }

        // 计算下一个位置
        public Vector3 CalculateNextPosition(Vector3 position, Vector3 direction, float speed, float time, float acceleration = 0)
        {
            return position + direction * speed * time + direction * acceleration * time * time / 2;
        }

        public void AddActiveBullet(GameObject bullet, BulletFinalConfig bulletFinalConfig)
        {
            activeBullets.Add(new ActiveBullet
            {
                bullet = bullet,
                currentTime = 0,
                spwanPosition = bullet.transform.position,
                bulletFinalConfig = bulletFinalConfig,
            });
        }
    }
}