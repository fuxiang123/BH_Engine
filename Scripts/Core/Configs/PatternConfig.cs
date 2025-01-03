using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    public struct PatternFinalConfig
    {
        public int count;
        public float[] spreadAnglePerbullet;
        public float spreadAngleTotal;
        public float[] xSpacingPerbullet;
        public float xSpacingTotal;
        public float spwanXTanslate;
        public float spwanYTanslate;
    }

    // 子弹朝向类型
    public enum BulletDirectionType
    {
        [LabelText("不进行任何方向处理")]
        None,
        [LabelText("跟随发射器方向")]
        EmitterDirection,
        [LabelText("只进行左右翻转")]
        FlipX,
    }

    // 弹幕配置。配置在发射瞬间的各种属性
    [Serializable]
    [LabelText("弹幕配置")]
    public class PatternConfig
    {
        [LabelText("子弹朝向")]
        public BulletDirectionType bulletDirectionType = BulletDirectionType.EmitterDirection;

        [LabelText("子弹数量")]
        public DynamicIntValue count = new DynamicIntValue() { value = 1 };
        [Space(20)]
        // 弹幕扩散角度
        [LabelText("每个子弹扩散角度")]
        public DynamicFloatValue spreadAnglePerBullet = new DynamicFloatValue() { value = 0 };
        [LabelText("总扩散角度")]
        [InfoBox("总扩散角度会和每个子弹扩散角度叠加")]
        public DynamicFloatValue spreadAngleTotal = new DynamicFloatValue() { value = 0 };

        [Space(20)]
        [LabelText("子弹间距")]
        public DynamicFloatValue xSpacingPerBullet = new DynamicFloatValue() { value = 0f };
        [LabelText("总间距")]
        [InfoBox("总间距会和每个子弹间距叠加")]
        public DynamicFloatValue xSpacingTotal = new DynamicFloatValue() { value = 0f };

        [LabelText("发射器X轴位移")]
        public DynamicFloatValue spwanXTanslate = new DynamicFloatValue() { value = 0f };
        [LabelText("发射器Y轴位移")]
        public DynamicFloatValue spwanYTanslate = new DynamicFloatValue() { value = 0f };

        public static PatternFinalConfig GetPatternFinalConfig(PatternConfig patternConfig)
        {
            var count = patternConfig.count.value < 1 ? 1 : patternConfig.count.value;
            var spreadAnglePerBullet = new float[count - 1];
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
                bulletDirectionType = patternConfig.bulletDirectionType,
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