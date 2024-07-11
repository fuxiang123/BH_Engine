
using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace BH_Engine
{
    public class ActiveBullet
    {
        public GameObject bullet;
        public Vector3 spwanPosition;
        public float currentTime;
        public BulletFinalConfig bulletFinalConfig;
        public GameObject[] emitters;
        // 是否正在被使用
        public bool enabled = true;
    }


    // 统一更新所有子弹的移动逻辑
    public class BulletBehaviourManager : MonoBehaviour
    {

        private class ActiveBulletPoolManager
        {
            public ObjectPool<ActiveBullet> Pool;

            public void init(int defaultCapacity = 100, int maxSize = 2000, bool collectionCheck = true)
            {
                Pool = new ObjectPool<ActiveBullet>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);
            }

            private ActiveBullet createFunc()
            {
                return new ActiveBullet();
            }

            private void actionOnGet(ActiveBullet activeBullet)
            {
                activeBullet.enabled = true;
            }

            public void actionOnRelease(ActiveBullet activeBullet)
            {
                if (activeBullet.bullet != null)
                {
                    BulletPoolManager.Instance.Release(activeBullet.bullet);
                    activeBullet.bullet = null;
                }
                if (activeBullet.emitters != null)
                {
                    for (int i = 0; i < activeBullet.emitters.Length; i++)
                    {
                        var emitter = activeBullet.emitters[i];
                        if (emitter != null) EmitterPoolManager.Instance.Release(emitter);
                    }
                    activeBullet.emitters = null;
                }
                activeBullet.enabled = false;
            }

            private void actionOnDestroy(ActiveBullet activeBullet) { }
        }

        public static BulletBehaviourManager Instance;

        // 正在移动的子弹
        public List<ActiveBullet> activeBullets = new List<ActiveBullet>();
        // ActiveBullet对象池
        private ActiveBulletPoolManager activeBulletPoolManager = new ActiveBulletPoolManager();

        private void Awake()
        {
            Instance = this;
            activeBulletPoolManager.init();
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
            }
            activeBullets.RemoveAll(item => item.bullet == null || !item.bullet.gameObject.activeSelf || !item.enabled);
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
                activeBulletPoolManager.Pool.Release(activeBullet);
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
            var activeBullet = activeBulletPoolManager.Pool.Get();
            activeBullet.bullet = bullet;
            activeBullet.spwanPosition = bullet.transform.position;
            activeBullet.currentTime = 0;
            activeBullet.bulletFinalConfig = bulletFinalConfig;

            if (bulletFinalConfig.emitterProfile != null && bulletFinalConfig.emitterProfile.Length > 0)
            {
                var configEmitterProfiles = bulletFinalConfig.emitterProfile;
                GameObject[] emitters = EmitterPoolManager.Instance.Get(configEmitterProfiles.Length);
                for (int i = 0; i < configEmitterProfiles.Length; i++)
                {
                    ProfileEmitter profileEmitter = emitters[i].GetComponent<ProfileEmitter>();
                    profileEmitter.SetEmitterProfile(configEmitterProfiles[i]);
                    profileEmitter.IsAutoEmit = true;
                    profileEmitter.transform.SetParent(bullet.transform);
                    profileEmitter.transform.localPosition = Vector3.zero;
                    profileEmitter.transform.localRotation = Quaternion.identity;
                }
                activeBullet.emitters = emitters;
            }
            activeBullets.Add(activeBullet);
        }

        // 释放所有子弹
        public void ReleaseAllBullets()
        {
            foreach (var item in activeBullets)
            {
                activeBulletPoolManager.Pool.Release(item);
            }
            activeBullets.Clear();
        }
    }
}