
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BH_Engine
{
    public class BulletBehaviour : MonoBehaviour
    {
        public BulletConfig bulletConfig;
        public Action<GameObject> OnReleaseBullet;
        // 子弹的初始位置
        public Vector3 spwanPosition;
        // 当前子弹飞行时间
        public float currentTime;
        // 子弹的飞行方向
        public Vector3 direction;
        // 子弹移动的距离
        public float distance;
        // 子弹上附加的发射器
        public List<GameObject> emitters = new List<GameObject>();
        // 子弹行为脚本
        public List<IBulletMoveScript> BulletMoveScript = new List<IBulletMoveScript>();

        private void OnDisable()
        {
            InitSettings();
        }

        public void Init(BulletConfig bulletConfig, Vector3 direction, Action<GameObject> OnReleaseBullet)
        {
            this.bulletConfig = bulletConfig;
            this.OnReleaseBullet = OnReleaseBullet;
            this.direction = direction;

            spwanPosition = transform.position;
            // 给子弹绑定行为脚本
            if (bulletConfig.BulletMoveScript != null && bulletConfig.BulletMoveScript.Count > 0)
            {
                BulletMoveScript = bulletConfig.BulletMoveScript;
            }

            // 给当前子弹绑定发射器
            if (bulletConfig.emitterProfile != null && bulletConfig.emitterProfile.Count > 0)
            {
                var configEmitterProfiles = bulletConfig.emitterProfile;
                emitters.AddRange(EmitterPoolManager.Instance.Get(configEmitterProfiles.Count));
                for (int i = 0; i < configEmitterProfiles.Count; i++)
                {
                    ProfileEmitter profileEmitter = emitters[i].GetComponent<ProfileEmitter>();
                    profileEmitter.SetEmitterProfile(configEmitterProfiles[i]);
                    profileEmitter.IsAutoEmit = true;
                    profileEmitter.transform.position = transform.position;
                    float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var emitterConfigAngle = profileEmitter.EmitterConfig.emitterAngle.value;
                    profileEmitter.transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90 + emitterConfigAngle);
                }
            }
        }

        void FixedUpdate()
        {
            var bulletFinalConfig = BulletConfig.GetFinalConfig(bulletConfig);
            currentTime += Time.fixedDeltaTime;
            var prePosition = transform.position;
            if (BulletMoveScript?.Count > 0)
            {
                for (int i = 0; i < BulletMoveScript.Count; i++)
                {
                    BulletMoveScript[i].UpdateBulletMove(bulletFinalConfig, this);
                }
            }
            else
            {
                var speed = bulletFinalConfig.speed + bulletFinalConfig.acceleration * currentTime;
                transform.position = transform.position + direction * speed * Time.fixedDeltaTime;
            }

            // 更新emitter位置
            if (emitters.Count > 0)
            {
                for (int i = 0; i < emitters.Count; i++)
                {
                    emitters[i].transform.position = transform.position;
                    float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var emitterConfigAngle = emitters[i].GetComponent<BaseEmitter>()?.EmitterConfig?.emitterAngle?.value ?? 0;
                    emitters[i].transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90 + emitterConfigAngle);
                }
            }

            // 当前移动距离
            distance += Vector3.Distance(prePosition, transform.position);
            var realDistance = Vector3.Distance(spwanPosition, transform.position);
            if (currentTime >= bulletFinalConfig.lifeTime || (bulletFinalConfig.maxDistance > 0 && realDistance >= bulletFinalConfig.maxDistance))
            {
                OnReleaseBullet?.Invoke(gameObject);
            }
        }
        public void InitSettings()
        {
            if (emitters.Count > 0)
            {
                emitters.ForEach(e =>
                {
                    if (e != null) EmitterPoolManager.Instance.Release(e);
                });
                emitters.Clear();
            }
            BulletMoveScript.Clear();
            currentTime = 0;
            distance = 0;
            OnReleaseBullet = null;
            bulletConfig = null;
        }
    }
}