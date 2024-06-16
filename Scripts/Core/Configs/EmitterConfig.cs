
using System;
using Sirenix.OdinInspector;

namespace BH_Engine
{

    // 发射器接口， 负责控制发射行为、如发射频率
    [Serializable]
    [LabelText("发射器配置")]
    public class EmitterConfig
    {
        // 发射间隔
        [LabelText("发射间隔")]
        public DynamicFloatValue emitInterval = new DynamicFloatValue() { value = 0.1f };
        // 自动射击
        [LabelText("自动射击")]
        public bool autoEmit;

        public static EmitterConfig CopyConfig(EmitterConfig config)
        {
            return new EmitterConfig
            {
                emitInterval = config.emitInterval.Copy(),
                autoEmit = config.autoEmit
            };
        }
    }
}