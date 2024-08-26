using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    // 子弹生成后遵循的属性
    public class BulletFinalConfig
    {
        public float speed;
        public GameObject prefab;
        public float maxDistance;
        public float acceleration;
        public float lifeTime;
        public EmitterProfileSO[] emitterProfile;
        public IBulletMoveScript[] BulletMoveScript;
    }

    // 子弹接口, 负责子弹的外观，速度，大小, 生命周期等
    [Serializable]
    [LabelText("子弹配置")]
    public class BulletConfig
    {
        [LabelText("子弹预制体")]
        public GameObject bulletPrefab;
        [LabelText("子弹速度")]
        public DynamicFloatValue speed = new DynamicFloatValue() { value = 5f };
        [LabelText("子弹生命周期")]
        public DynamicFloatValue lifeTime = new DynamicFloatValue() { value = 5f };
        [LabelText("子弹最大射程")]
        public DynamicFloatValue maxDistance = new DynamicFloatValue() { value = 10f };

        // 移动时属性
        [Header("子弹移动时属性")]
        [LabelText("子弹加速度")]
        public DynamicFloatValue acceleration;

        [LabelText("给子弹附加一个emitter profile，用于实现移动时发射额外子弹的效果")]
        public List<EmitterProfileSO> emitterProfile;
        [LabelText("子弹飞行过程中的行为脚本"), SerializeReference]
        public List<IBulletMoveScript> BulletMoveScript;

        public static BulletFinalConfig GetFinalConfig(BulletConfig bulletConfig)
        {
            return new BulletFinalConfig
            {
                speed = bulletConfig.speed.value,
                maxDistance = bulletConfig.maxDistance.value,
                acceleration = bulletConfig.acceleration.value,
                lifeTime = bulletConfig.lifeTime.value,
            };
        }

        // 重置状态，将因为DynamicFloatValue改变的值初始化
        public static void ResetConfig(BulletConfig bulletConfig)
        {
            bulletConfig.speed.Reset();
            bulletConfig.lifeTime.Reset();
            bulletConfig.maxDistance.Reset();
            bulletConfig.acceleration.Reset();
        }

        public static BulletConfig CopyConfig(BulletConfig bulletConfig)
        {
            return new BulletConfig
            {
                bulletPrefab = bulletConfig.bulletPrefab,
                speed = bulletConfig.speed.Copy(),
                lifeTime = bulletConfig.lifeTime.Copy(),
                maxDistance = bulletConfig.maxDistance.Copy(),
                acceleration = bulletConfig.acceleration.Copy(),
                emitterProfile = bulletConfig.emitterProfile,
                BulletMoveScript = bulletConfig.BulletMoveScript
            };
        }
    }
}