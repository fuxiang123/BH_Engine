
using System;
using Sirenix.OdinInspector;

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
        [LabelText("发射器角度")]
        public DynamicFloatValue emitterAngle;

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