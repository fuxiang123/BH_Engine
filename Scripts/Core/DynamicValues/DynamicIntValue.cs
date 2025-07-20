using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    /// <summary>
    ///     动态Int值
    /// </summary>
    [Serializable]
    public class DynamicIntValue
    {
        [LabelText("值的类型")] [SerializeField] private DynamicValueType mValueType;

        [LabelText("初始值值")] [SerializeField] [ShowIf("valueType", DynamicValueType.Fixed)]
        private int m_Value;

        [LabelText("曲线")] [SerializeField] [ShowIf("valueType", DynamicValueType.Curve)]
        public AnimationCurve curve;

        [LabelText("自定义值处理函数")] [SerializeReference] [ShowIf("valueType", DynamicValueType.Custom)]
        public IDynamicIntHandler CustomValueHandler;

        // 记录配置的初始值
        private int initValue;

        private bool isFirst = true;

        public DynamicValueType valueType
        {
            get => mValueType;
            set
            {
                mValueType = value;
                isFirst = true;
            }
        }

        public float lastValue { get; private set; }

        public int value
        {
            get => GetValue();
            set => m_Value = value;
        }

        // 原始值，获取时不会影响动态值的计算
        public int RawValue
        {
            get => m_Value;
            set => m_Value = value;
        }


        // 重置所有数据
        public void Reset()
        {
            isFirst = true;
            m_Value = initValue;
        }

        public int GetValue(float time)
        {
            if (isFirst)
            {
                isFirst = false;
                initValue = m_Value;
            }

            lastValue = m_Value;

            switch (valueType)
            {
                case DynamicValueType.Fixed:
                    return m_Value;
                case DynamicValueType.Curve:
                    return (int)curve.Evaluate(time);
                case DynamicValueType.Custom:
                    if (CustomValueHandler != null) m_Value = CustomValueHandler.Calculate(m_Value);
                    return m_Value;
                default:
                    return 0;
            }
        }

        public int GetValue()
        {
            return GetValue(Time.time);
        }

        public DynamicIntValue Copy()
        {
            return new DynamicIntValue
            {
                valueType = valueType,
                m_Value = m_Value,
                curve = curve,
                CustomValueHandler = CustomValueHandler
            };
        }
    }
}