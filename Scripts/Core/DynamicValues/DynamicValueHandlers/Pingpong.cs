using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    [Serializable]
    public class PingpongFloat : IDynamicFloatHandler
    {
        [LabelText("最大值"), SerializeField]
        private float min;
        [LabelText(text: "最小值"), SerializeField]
        private float max;
        [LabelText("速度"), SerializeField]
        private float speed = 1f;
        public float Calculate(float x)
        {
            if (min == max)
            {
                return min;
            }
            return Mathf.PingPong(Time.time * speed, max - min) + min;
        }
    }

    [Serializable]
    public class PingpongInt : IDynamicIntHandler
    {
        [LabelText("最大值"), SerializeField]
        private int min;
        [LabelText(text: "最小值"), SerializeField]
        private int max;
        [LabelText("速度"), SerializeField]
        private float speed = 1f;
        public int Calculate(int x)
        {
            if (min == max)
            {
                return min;
            }
            return Mathf.RoundToInt(Mathf.PingPong(Time.time * speed, max - min) + min);
        }
    }
}