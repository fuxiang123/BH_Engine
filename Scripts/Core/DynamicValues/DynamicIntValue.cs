using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace BH_Engine
{

    /// <summary>
    /// 动态Int值
    /// </summary>
    [System.Serializable]
    public class DynamicIntValue
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
        private int m_Value;
        [LabelText("曲线"), SerializeField, ShowIf("valueType", DynamicValueType.Curve)]
        public AnimationCurve curve;
        [LabelText("自定义值处理函数"), SerializeReference, ShowIf("valueType", DynamicValueType.Custom)]
        public IDynamicIntHandler CustomValueHandler;

        private bool isFirst = true;
        // 记录配置的初始值
        private int initValue;

        [HideInInspector]
        public int value
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

        public int GetValue()
        {
            if (isFirst)
            {
                isFirst = false;
                initValue = m_Value;
            }

            switch (valueType)
            {
                case DynamicValueType.Fixed:
                    return m_Value;
                case DynamicValueType.Curve:
                    return (int)curve.Evaluate(Time.time);
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
    }
}