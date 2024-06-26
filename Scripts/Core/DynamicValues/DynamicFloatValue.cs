using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace BH_Engine
{

    /// <summary>
    /// 动态float值
    /// </summary>
    [System.Serializable]
    public class DynamicFloatValue
    {
        [LabelText("值的类型")]
        [SerializeField]
        private DynamicValueType mValueType;
        public DynamicValueType valueType
        {
            get => mValueType;
            set
            {
                mValueType = value;
                isFirst = true;
            }
        }
        [LabelText("初始值值"), SerializeField, ShowIf("valueType", DynamicValueType.Fixed)]
        private float m_Value;
        [LabelText("曲线"), SerializeField, ShowIf("valueType", DynamicValueType.Curve)]
        public AnimationCurve curve;
        [LabelText("自定义值处理函数"), SerializeReference, ShowIf("valueType", DynamicValueType.Custom)]
        public IDynamicFloatHandler CustomValueHandler;

        private bool isFirst = true;
        // 记录配置的初始值
        private float initValue;

        [HideInInspector]
        public float lastValue { get; private set; }

        [HideInInspector]
        public float value
        {
            get
            {
                return GetValue();
            }
            set => m_Value = value;
        }

        // 重置所有数据
        public void Reset()
        {
            isFirst = true;
            m_Value = initValue;
        }

        public float GetValue()
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
                    return curve.Evaluate(Time.time);
                case DynamicValueType.Custom:
                    if (CustomValueHandler != null)
                    {
                        m_Value = CustomValueHandler.Calculate(m_Value);
                    }
                    return m_Value;
                default:
                    return 0;
            }
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