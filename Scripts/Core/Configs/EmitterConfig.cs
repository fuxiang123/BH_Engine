using System;
using Sirenix.OdinInspector;

namespace BH_Engine
{
    public enum EmitterTransformType
    {
        [LabelText("世界坐标")] World,
        [LabelText("本地坐标")] Local
    }

    public class EmitterFinalConfig
    {
        public float autoEmitDelay;
        public float emitInterval;
        public float emitterAngle;
    }

    // 发射器接口， 负责控制发射行为、如发射频率
    [Serializable]
    [LabelText("发射器配置")]
    public class EmitterConfig
    {
        // 发射间隔
        [LabelText("发射间隔")] public DynamicFloatValue emitInterval = new() { value = 0.1f };

        [LabelText("发射前延迟")] [InfoBox("每次发射之前的延迟时间，可以配合发射器事件调整攻击准备动画等")]
        public DynamicFloatValue emitBeforeDelay = new() { value = 0 };

        [LabelText("发射器角度")] public DynamicFloatValue emitterAngle = new() { value = 0 };

        public EmitterFinalConfig GetEmitterFinalConfig(float time = 0)
        {
            return new EmitterFinalConfig
            {
                emitInterval = emitInterval.GetValue(time),
                emitterAngle = emitterAngle.GetValue(time),
                autoEmitDelay = emitBeforeDelay.GetValue(time)
            };
        }

        public static EmitterConfig CopyConfig(EmitterConfig config)
        {
            return new EmitterConfig
            {
                emitInterval = config.emitInterval.Copy(),
                emitterAngle = config.emitterAngle.Copy(),
                emitBeforeDelay = config.emitBeforeDelay.Copy()
            };
        }
    }
}