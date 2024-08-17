
using System;
using UnityEngine;

namespace BH_Engine
{
    public class BulletBehaviour : MonoBehaviour
    {
        public BulletConfig bulletConfig;
        public Action<BulletBehaviour> OnReleaseBullet;

        // 子弹的初始位置
        public Vector3 spwanPosition;
        // 当前子弹飞行时间
        public float currentTime;
        // 子弹移动的距离
        public float distance;
        // 子弹上附加的发射器
        public GameObject[] emitters;
        // 子弹行为脚本
        public IBulletBehaviourHandler[] bulletBehaviourHandlers;

        public void Init(BulletConfig bulletConfig, Action<BulletBehaviour> OnReleaseBullet)
        {
            this.bulletConfig = bulletConfig;
            this.OnReleaseBullet = OnReleaseBullet;

            spwanPosition = transform.position;
            // 给子弹绑定行为脚本
            if (bulletConfig.bulletBehaviourHandler != null && bulletConfig.bulletBehaviourHandler.Length > 0)
            {
                bulletBehaviourHandlers = bulletConfig.bulletBehaviourHandler;
            }

            // 给当前子弹绑定发射器
            if (bulletConfig.emitterProfile != null && bulletConfig.emitterProfile.Length > 0)
            {
                var configEmitterProfiles = bulletConfig.emitterProfile;
                GameObject[] emitters = EmitterPoolManager.Instance.Get(configEmitterProfiles.Length);
                for (int i = 0; i < configEmitterProfiles.Length; i++)
                {
                    ProfileEmitter profileEmitter = emitters[i].GetComponent<ProfileEmitter>();
                    profileEmitter.SetEmitterProfile(configEmitterProfiles[i]);
                    profileEmitter.IsAutoEmit = true;
                    profileEmitter.transform.position = transform.position;
                    profileEmitter.transform.rotation = transform.rotation;
                }
                this.emitters = emitters;
            }
        }

        void Update()
        {
            var bulletFinalConfig = BulletConfig.GetFinalConfig(bulletConfig);
            currentTime += Time.deltaTime;
            var prePosition = transform.position;
            if (bulletBehaviourHandlers?.Length > 0)
            {
                for (int i = 0; i < bulletBehaviourHandlers.Length; i++)
                {
                    bulletBehaviourHandlers[i].HandleBulletBehaviour(bulletFinalConfig, this);
                }
            }
            else
            {
                var speed = bulletFinalConfig.speed + bulletFinalConfig.acceleration * currentTime;
                transform.position = transform.position + transform.up * speed * Time.deltaTime;
            }

            // 更新emitter位置
            if (emitters != null)
            {
                for (int i = 0; i < emitters.Length; i++)
                {
                    emitters[i].transform.position = transform.position;
                    emitters[i].transform.rotation = transform.rotation;
                }
            }

            // 当前移动距离
            distance += Vector3.Distance(prePosition, transform.position);
            var realDistance = Vector3.Distance(spwanPosition, transform.position);
            if (currentTime >= bulletFinalConfig.lifeTime || (bulletFinalConfig.maxDistance > 0 && realDistance >= bulletFinalConfig.maxDistance))
            {
                OnReleaseBullet?.Invoke(this);
            }
        }
    }
}