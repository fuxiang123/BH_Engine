
using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    // 弹幕文件类型
    public enum PatternConfigType
    {
        [LabelText("使用预制体")]
        Prefab,
        [LabelText("使用配置文件")]
        Profile
    }

    // 规划每波弹幕的配置
    [Serializable]
    public class PatternSchedulerItem
    {
        // 延迟，在上一波弹幕结束后延迟多久开始下一波
        public float Delay = 0;
        // 持续时间
        public float Duration = 20;
        public PatternConfigType PatternConfigType;
        // 弹幕的预制体
        [ShowIf("PatternConfigType", PatternConfigType.Prefab)]
        public GameObject PatternPrefab;
        // 弹幕的配置文件
        [ShowIf("PatternConfigType", PatternConfigType.Profile)]
        public EmitterProfileSO EmitterProfileSO;
    }

    // 弹幕调度器配置
    [CreateAssetMenu(fileName = "PatternScheduler", menuName = "BH_Engine/PatternScheduler")]
    public class PatternSchedulerSO : ScriptableObject
    {
        public PatternSchedulerItem[] SchedulerItems;
    }
}