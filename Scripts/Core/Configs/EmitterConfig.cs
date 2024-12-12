
using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{

    public enum EmitterTransformType
    {
        [LabelText("世界坐标")]
        World,
        [LabelText("本地坐标")]
        Local
    }

    public class EmitterFinalConfig
    {
        public float emitInterval;
        public float emitterAngle;
        public EmitterTransformType emitterAngleType;
        public float autoEmitDelay;
    }

    // 发射器接口， 负责控制发射行为、如发射频率
    [Serializable]
    [LabelText("发射器配置")]
    public class EmitterConfig
    {
        // 发射间隔
        [LabelText("发射间隔")]
        public DynamicFloatValue emitInterval = new DynamicFloatValue() { value = 0.1f };
        [LabelText("发射前延迟"), InfoBox("每次发射之前的延迟时间，可以配合发射器事件调整攻击准备动画等")]
        public DynamicFloatValue emitBeforeDelay = new DynamicFloatValue() { value = 0 };
        [LabelText("发射器角度")]
        public DynamicFloatValue emitterAngle = new DynamicFloatValue() { value = 0 };
        public static EmitterConfig CopyConfig(EmitterConfig config)
        {
            return new EmitterConfig
            {
                emitInterval = config.emitInterval.Copy(),
                emitterAngle = config.emitterAngle.Copy(),
            };
        }
    }
}