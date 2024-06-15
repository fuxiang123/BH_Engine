using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    [Serializable]
    public class ListValueFloat : IDynamicFloatHandler
    {
        [LabelText("值列表"), SerializeField]
        private float[] listValues;
        private int index = 0;
        public float Calculate(float x)
        {
            if (listValues.Length == 0)
            {
                return 0;
            }
            if (index >= listValues.Length)
            {
                index = 0;
            }
            return listValues[index++];
        }
    }

    [Serializable]
    public class ListValueInt : IDynamicIntHandler
    {
        [LabelText("值列表"), SerializeField]
        private int[] listValues;
        private int index = 0;
        public int Calculate(int x)
        {
            if (listValues.Length == 0)
            {
                return 0;
            }
            if (index >= listValues.Length)
            {
                index = 0;
            }
            return listValues[index++];
        }
    }
}