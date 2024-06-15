using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    [Serializable]
    public class RandomFloat : IDynamicFloatHandler
    {
        [LabelText("最大值"), SerializeField]
        private float min;
        [LabelText(text: "最小值"), SerializeField]
        private float max;
        public float Calculate(float x)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    [Serializable]
    public class RandomInt : IDynamicIntHandler
    {
        [LabelText("最大值"), SerializeField]
        private int min;
        [LabelText(text: "最小值"), SerializeField]
        private int max;
        public int Calculate(int x)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}