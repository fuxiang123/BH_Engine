using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    /// <summary>
    ///     动态float值
    /// </summary>
    [Serializable]
    public class DynamicFloatValue
    {
        [LabelText("值的类型")] [SerializeField] private DynamicValueType mValueType;

        [LabelText("初始值值")] [SerializeField] [ShowIf("valueType", DynamicValueType.Fixed)]
        private float m_Value;

        [LabelText("曲线")] [SerializeField] [ShowIf("valueType", DynamicValueType.Curve)]
        public AnimationCurve curve;

        [LabelText("自定义值处理函数")] [SerializeReference] [ShowIf("valueType", DynamicValueType.Custom)]
        public IDynamicFloatHandler CustomValueHandler;

        // 记录配置的初始值
        private float initValue;

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

        public float value
        {
            get => GetValue();
            set => m_Value = value;
        }

        // 原始值，获取时不会影响动态值的计算
        public float RawValue
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

        public float GetValue(float time)
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
                    return curve.Evaluate(time);
                case DynamicValueType.Custom:
                    if (CustomValueHandler != null) m_Value = CustomValueHandler.Calculate(m_Value);
                    return m_Value;
                default:
                    return 0;
            }
        }

        public float GetValue()
        {
            return GetValue(Time.time);
        }

        public DynamicFloatValue Copy()
        {
            return new DynamicFloatValue
            {
                valueType = valueType,
                m_Value = m_Value,
                curve = curve,
                CustomValueHandler = CustomValueHandler
            };
        }
    }
}