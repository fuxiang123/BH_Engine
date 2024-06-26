using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    public class PatternFinalConfig
    {
        public int count;
        public int[] spreadAnglePerbullet;
        public int spreadAngleTotal;
        public float[] xSpacingPerbullet;
        public float xSpacingTotal;
        public float spwanXTanslate;
        public float spwanYTanslate;
    }

    // 弹幕配置。配置在发射瞬间的各种属性
    [Serializable]
    [LabelText("弹幕配置")]
    public class PatternConfig
    {

        [LabelText("子弹数量")]
        public DynamicIntValue count = new DynamicIntValue() { value = 1 };
        [Space(20)]
        // 弹幕扩散角度
        [LabelText("每个子弹扩散角度")]
        public DynamicIntValue spreadAnglePerBullet;
        [LabelText("总扩散角度")]
        [InfoBox("总扩散角度会和每个子弹扩散角度叠加")]
        public DynamicIntValue spreadAngleTotal;

        [Space(20)]
        [LabelText("子弹间距")]
        public DynamicFloatValue xSpacingPerBullet;
        [LabelText("总间距")]
        [InfoBox("总间距会和每个子弹间距叠加")]
        public DynamicFloatValue xSpacingTotal;

        [LabelText("发射器X轴位移")]
        public DynamicFloatValue spwanXTanslate;
        [LabelText("发射器Y轴位移")]
        public DynamicFloatValue spwanYTanslate;

        public static PatternFinalConfig GetPatternFinalConfig(PatternConfig patternConfig)
        {
            var count = patternConfig.count.value;
            var spreadAnglePerBullet = new int[count - 1];
            var spreadAngleTotal = patternConfig.spreadAngleTotal.value;
            var xSpacingPerBullet = new float[count - 1];
            var xSpacingTotal = patternConfig.xSpacingTotal.value;
            for (int i = 0; i < count - 1; i++)
            {
                spreadAnglePerBullet[i] = patternConfig.spreadAnglePerBullet.value;
                xSpacingPerBullet[i] = patternConfig.xSpacingPerBullet.value;
            }
            return new PatternFinalConfig
            {
                count = count,
                spreadAnglePerbullet = spreadAnglePerBullet,
                spreadAngleTotal = spreadAngleTotal,
                xSpacingPerbullet = xSpacingPerBullet,
                xSpacingTotal = xSpacingTotal,
                spwanXTanslate = patternConfig.spwanXTanslate.value,
                spwanYTanslate = patternConfig.spwanYTanslate.value
            };
        }

        public static void ResetConfig(PatternConfig patternConfig)
        {
            patternConfig.count.Reset();
            patternConfig.spreadAnglePerBullet.Reset();
            patternConfig.spreadAngleTotal.Reset();
            patternConfig.xSpacingPerBullet.Reset();
            patternConfig.xSpacingTotal.Reset();
            patternConfig.spwanXTanslate.Reset();
            patternConfig.spwanYTanslate.Reset();
        }

        public static PatternConfig CopyConfig(PatternConfig patternConfig)
        {
            return new PatternConfig
            {
                count = patternConfig.count.Copy(),
                spreadAnglePerBullet = patternConfig.spreadAnglePerBullet.Copy(),
                spreadAngleTotal = patternConfig.spreadAngleTotal.Copy(),
                xSpacingPerBullet = patternConfig.xSpacingPerBullet.Copy(),
                xSpacingTotal = patternConfig.xSpacingTotal.Copy(),
                spwanXTanslate = patternConfig.spwanXTanslate.Copy(),
                spwanYTanslate = patternConfig.spwanYTanslate.Copy()
            };
        }
    }
}