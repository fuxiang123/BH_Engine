using System;
using Sirenix.OdinInspector;

namespace BH_Engine
{
    // 值的类型
    [Serializable]
    public enum DynamicValueType
    {
        [LabelText("固定值")]
        Fixed,
        [LabelText("曲线值")]
        Curve,
        [LabelText("自定义值")]
        Custom,
    }
    // 循环方式
    [Serializable]
    public enum LoopType
    {
        [LabelText("从头循环")]
        Loop,
        [LabelText("往复循环")]
        PingPong
    }
}