
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
        // 子弹的GameObject
        public GameObject bullet;
        // 子弹的初始位置
        public Vector3 spwanPosition;
        // 当前子弹飞行时间
        public float currentTime;
        // 子弹的配置
        public BulletConfig bulletConfig;
        // 子弹上附加的发射器
        public GameObject[] emitters;
        // 子弹行为脚本
        public IBulletBehaviourHandler[] bulletBehaviourHandlers;
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
            var bulletFinalConfig = BulletConfig.GetFinalConfig(activeBullet.bulletConfig);
            activeBullet.currentTime += Time.deltaTime;
            if (activeBullet.bulletBehaviourHandlers?.Length > 0)
            {
                for (int i = 0; i < activeBullet.bulletBehaviourHandlers.Length; i++)
                {
                    activeBullet.bulletBehaviourHandlers[i].HandleBulletBehaviour(bulletFinalConfig, activeBullet);
                }
            }
            else
            {
                var speed = bulletFinalConfig.speed + bulletFinalConfig.acceleration * activeBullet.currentTime;
                activeBullet.bullet.transform.position = activeBullet.bullet.transform.position + activeBullet.bullet.transform.up * speed * Time.deltaTime;
            }

            // 当前移动距离
            var distance = CalculateTotalDistance(activeBullet.spwanPosition, activeBullet.bullet.transform.position);
            if (activeBullet.currentTime >= bulletFinalConfig.lifeTime || (bulletFinalConfig.maxDistance > 0 && distance >= bulletFinalConfig.maxDistance))
            {
                activeBulletPoolManager.Pool.Release(activeBullet);
            }
        }

        // 计算当前总移动距离
        public float CalculateTotalDistance(
            Vector3 spwanPosition,
            Vector3 currentPosition
        )
        {
            return Vector3.Distance(spwanPosition, currentPosition);
        }

        public void AddActiveBullet(GameObject bullet, BulletConfig bulletConfig)
        {
            ActiveBullet activeBullet = activeBulletPoolManager.Pool.Get();
            activeBullet.bullet = bullet;
            activeBullet.spwanPosition = bullet.transform.position;
            activeBullet.currentTime = 0;
            activeBullet.bulletConfig = bulletConfig;
            if (bulletConfig.bulletBehaviourHandler != null && bulletConfig.bulletBehaviourHandler.Length > 0)
            {
                activeBullet.bulletBehaviourHandlers = bulletConfig.bulletBehaviourHandler;
            }


            if (bulletConfig.emitterProfile != null && bulletConfig.emitterProfile.Length > 0)
            {
                var configEmitterProfiles = bulletConfig.emitterProfile;
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