using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    // 子弹生成后遵循的属性
    public struct BulletFinalConfig
    {
        public float speed;
        public GameObject prefab;
        public float maxDistance;
        public float acceleration;
        public float lifeTime;
    }

    // 子弹接口, 负责子弹的外观，速度，大小, 生命周期等
    [Serializable]
    [LabelText("子弹配置")]
    public class BulletConfig
    {
        [LabelText("子弹预制体")] public GameObject bulletPrefab;

        [LabelText("子弹速度")] public DynamicFloatValue speed = new() { value = 5f };

        [LabelText("子弹生命周期")] public DynamicFloatValue lifeTime = new() { value = 10f };

        [LabelText("子弹最大射程")] public DynamicFloatValue maxDistance = new() { value = 20f };

        // 移动时属性
        [Header("子弹移动时属性")] [LabelText("子弹加速度")]
        public DynamicFloatValue acceleration = new() { value = 0f };

        [InfoBox("给子弹附加一个emitter profile，用于实现移动时发射额外子弹的效果")]
        public List<EmitterProfileSO> emitterProfile = new();

        [InfoBox("子弹飞行过程中的行为脚本")] [SerializeReference]
        public List<IBulletMoveScript> BulletMoveScript = new();

        public BulletFinalConfig GetFinalConfig(float time)
        {
            return new BulletFinalConfig
            {
                speed = speed.GetValue(time),
                maxDistance = maxDistance.GetValue(time),
                acceleration = acceleration.GetValue(time),
                lifeTime = lifeTime.GetValue(time)
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
            var emitterProfileListCopy = bulletConfig.emitterProfile.Select(x => EmitterProfileSO.Copy(x)).ToList();
            return new BulletConfig
            {
                bulletPrefab = bulletConfig.bulletPrefab,
                speed = bulletConfig.speed.Copy(),
                lifeTime = bulletConfig.lifeTime.Copy(),
                maxDistance = bulletConfig.maxDistance.Copy(),
                acceleration = bulletConfig.acceleration.Copy(),
                emitterProfile = emitterProfileListCopy,
                BulletMoveScript = bulletConfig.BulletMoveScript
            };
        }
    }
}