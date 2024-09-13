

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    // 弹幕调度器状态
    public enum PatternSchedulerState
    {
        Prepare,
        Running,
        Finished
    }

    public class PatternScheduler : MonoBehaviour
    {
        public PatternSchedulerSO PatternSchedulerSO;

        // 预制体实例化的位置，不设置默认以当前gameobject的位置为中心
        public Transform Center;
        public ProfileEmitter ProfileEmitter;
        // 当前生效的弹幕预制体
        private GameObject currentPatternPrefab;
        private int currentPatternIndex = 0;
        private float currentTime = 0;
        [HideInInspector]
        public PatternSchedulerState State = PatternSchedulerState.Prepare;

        private PatternSchedulerItem curreentPatternItem => PatternSchedulerSO.SchedulerItems[currentPatternIndex];

        private void Awake()
        {
            if (Center == null)
            {
                Center = transform;
            }
        }

        private void Update()
        {
            if (PatternSchedulerSO.SchedulerItems.Length == 0 || currentPatternIndex >= PatternSchedulerSO.SchedulerItems.Length || State == PatternSchedulerState.Finished)
            {
                return;
            }

            currentTime += Time.deltaTime;
            HandlePrepareState();
            HandleRunningState();
        }

        private void HandlePrepareState()
        {
            if (State == PatternSchedulerState.Prepare && currentTime >= curreentPatternItem.Delay)
            {
                State = PatternSchedulerState.Running;
            }
        }

        private void HandleRunningState()
        {

            if (State == PatternSchedulerState.Running)
            {
                // 第一次初始化时currentPatternPrefab为空
                if (currentPatternPrefab == null)
                {
                    InstantiatePattern(curreentPatternItem);
                }
                if (currentTime >= curreentPatternItem.Duration + curreentPatternItem.Delay)
                {
                    currentPatternIndex++;
                    currentTime = 0;
                    if (currentPatternIndex >= PatternSchedulerSO.SchedulerItems.Length)
                    {
                        State = PatternSchedulerState.Finished;
                        currentPatternPrefab.gameObject.SetActive(false);
                    }
                    else
                    {
                        InstantiatePattern(curreentPatternItem);
                        State = PatternSchedulerState.Prepare;
                    }
                }
            }
        }

        // 实例化新的弹幕
        private void InstantiatePattern(PatternSchedulerItem item)
        {
            if (currentPatternPrefab != null)
            {
                Destroy(currentPatternPrefab);
            }

            if (item.PatternConfigType == PatternConfigType.Prefab)
            {
                currentPatternPrefab = Instantiate(item.PatternPrefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                if (item.EmitterProfileSO != null)
                {
                    currentPatternPrefab = ProfileEmitter.gameObject;
                    // 通过ProfileEmitter实例化
                    ProfileEmitter.SetEmitterProfile(item.EmitterProfileSO);
                    ProfileEmitter.IsAutoEmit = true;
                }
            }
            currentPatternPrefab.transform.position = Center.position;
            currentPatternPrefab.transform.SetParent(transform);
        }

        // 切换弹幕调度器配置
        public void ChangePatternSchedulerSO(PatternSchedulerSO patternSchedulerSO)
        {
            PatternSchedulerSO = patternSchedulerSO;
            currentPatternIndex = 0;
            currentTime = 0;
            State = PatternSchedulerState.Prepare;
        }
    }
}