using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace BH_Engine
{
    public enum EmitterState
    {
        Ready, // 准备射击
        EmitBeforeDelay, // 发射前延迟
        Emit, // 射击
        Cooldown, // 冷却中
    }

    // 发射器，负责弹幕
    public class BaseEmitter : MonoBehaviour
    {
        [LabelText("发射器配置")]
        public EmitterConfig EmitterConfig;
        [LabelText("子弹配置")]
        public BulletConfig BulletConfig;
        [LabelText("弹幕配置")]
        public PatternConfig PatternConfig;
        [LabelText("是否自动射击")]
        public bool IsAutoEmit = false;
        private EmitterState mEmitterState = EmitterState.Ready;
        public EmitterState EmitterState // 发射器状态
        {
            get { return mEmitterState; }
            set
            {
                mEmitterState = value;
                OnEmitterStateChange.Invoke(mEmitterState);
            }
        }
        [HideInInspector] public UnityEvent<EmitterState> OnEmitterStateChange = new UnityEvent<EmitterState>(); // 发射器状态改变事件

        private float mTimer = 0;

        protected void OnDisable()
        {
            StopShoot();
        }

        protected void FixedUpdate()
        {

            if (EmitterState == EmitterState.Ready && IsAutoEmit)
            {
                HandleReadyState();
            }
            else if (EmitterState == EmitterState.EmitBeforeDelay)
            {
                HandleEmitBeforeDelayState();
            }
            else if (EmitterState == EmitterState.Emit)
            {
                HandleEmitState();
            }
            else if (EmitterState == EmitterState.Cooldown)
            {
                HandleCooldownState();
            }
        }

        // 开始或继续自动射击
        public void StartShoot()
        {
            IsAutoEmit = true;
        }

        // 暂停自动射击, 恢复后悔继承之前的属性，如射击角度的变化等
        public void PauseShoot()
        {
            IsAutoEmit = false;
        }

        // 停止自动射击，会恢复到初始状态
        public void StopShoot()
        {
            IsAutoEmit = false;
            EmitterState = EmitterState.Ready;
            mTimer = 0;
            BulletConfig.ResetConfig(BulletConfig);
            PatternConfig.ResetConfig(PatternConfig);
        }

        #region 状态处理

        private void HandleReadyState()
        {
            EmitterState = EmitterState.EmitBeforeDelay;
            mTimer = 0;
        }

        private void HandleEmitBeforeDelayState()
        {
            mTimer += Time.deltaTime;
            if (mTimer >= EmitterConfig.emitBeforeDelay.value)
            {
                EmitterState = EmitterState.Emit;
                mTimer = 0;
            }
        }

        private void HandleEmitState()
        {
            Emit();
            EmitterState = EmitterState.Cooldown;
            mTimer = 0;
        }

        private void HandleCooldownState()
        {
            mTimer += Time.deltaTime;
            if (mTimer >= EmitterConfig.emitInterval.value)
            {
                EmitterState = EmitterState.Ready;
                mTimer = 0;
            }
        }

        #endregion

        /// <summary>
        /// 获取子弹实例。可以通过子类覆写，以从不同对象池获取子弹实例
        /// </summary>
        protected virtual GameObject GetBullet(GameObject prefab)
        {
            return Instantiate(prefab);
        }

        /// <summary>
        /// 释放子弹实例
        /// </summary>
        protected virtual void ReleaseBullet(GameObject bullet)
        {
            Destroy(bullet);
        }

        // 发射单次子弹
        public virtual List<GameObject> Emit()
        {
            PatternFinalConfig patternFinalConfig = PatternConfig.GetPatternFinalConfig(PatternConfig);
            if (transform.parent != null)
            {
                transform.localRotation = Quaternion.Euler(0, 0, EmitterConfig.emitterAngle.value);
                transform.localPosition = new Vector3(patternFinalConfig.spwanXTanslate, patternFinalConfig.spwanYTanslate, transform.position.z);
            }

            var patternCount = patternFinalConfig.count;

            // 每颗子弹的真实间距
            float[] realXSpacingPerbullet = new float[patternCount - 1];
            // 综合xSpacingTotal和xSpacingPerbullet，计算最终的总宽度
            float xSpacingTotalFinal = 0;
            for (int i = 0; i < patternCount - 1; i++)
            {
                realXSpacingPerbullet[i] = patternFinalConfig.xSpacingPerbullet[i] + patternFinalConfig.xSpacingTotal / (patternCount - 1);
                xSpacingTotalFinal += realXSpacingPerbullet[i];
            }

            // 每颗子弹的真实角度
            float[] realSpreadAnglePerbullet = new float[patternCount - 1];
            // 计算总角度
            float totalAngle = 0;
            for (int i = 0; i < patternCount - 1; i++)
            {
                realSpreadAnglePerbullet[i] = patternFinalConfig.spreadAnglePerbullet[i] + patternFinalConfig.spreadAngleTotal / (patternCount - 1);
                totalAngle += realSpreadAnglePerbullet[i];
            }

            // 当前子弹xspacing的位置, 只有一个子弹时从正中发射即可
            float currentBulletXpacing = patternCount == 1 ? 0 : -xSpacingTotalFinal / 2;
            // 当前子弹的角度，只有一个子弹时从正中发射即可
            float currentBulletSpreadAngle = patternCount == 1 ? 0 : -totalAngle / 2;

            List<GameObject> bullets = new List<GameObject>();
            for (int i = 0; i < patternCount; i++)
            {
                if (BulletConfig.bulletPrefab == null)
                {
                    Debug.LogError(gameObject.name + " 子弹预制体为空");
                    continue;
                }
                var bullet = GetBullet(BulletConfig.bulletPrefab);

                // 计算子弹的位置
                currentBulletXpacing += i == 0 ? 0 : realXSpacingPerbullet[i - 1];
                // 当前子弹的实际生成位置
                Vector3 spawnPosition = transform.position + (transform.right * currentBulletXpacing);
                bullet.transform.position = spawnPosition;

                // 计算子弹的角度
                currentBulletSpreadAngle += i == 0 ? 0 : realSpreadAnglePerbullet[i - 1];
                var baseRotation = transform.rotation * Quaternion.Euler(0, 0, -currentBulletSpreadAngle);
                var direction = baseRotation * Vector3.up;
                if (PatternConfig.bulletDirectionType == BulletDirectionType.EmitterDirection)
                {
                    // 旋转角度减少90
                    bullet.transform.rotation = baseRotation * Quaternion.Euler(0, 0, 90);
                }
                else if (PatternConfig.bulletDirectionType == BulletDirectionType.FlipX)
                {
                    var originScale = bullet.transform.localScale;
                    var isMoveLeft = direction.x < 0;
                    bullet.transform.localScale = new Vector3(isMoveLeft ? -originScale.x : originScale.x, originScale.y, originScale.z);
                }
                bullet.GetComponent<BulletBehaviour>().Init(BulletConfig, direction, ReleaseBullet);
                bullets.Add(bullet);
            }
            return bullets;
        }

        public void UpdateBullet(BulletConfig BulletConfig)
        {
            this.BulletConfig = BulletConfig.CopyConfig(BulletConfig);
        }

        public void UpdatePattern(PatternConfig PatternConfig)
        {
            this.PatternConfig = PatternConfig.CopyConfig(PatternConfig);
        }

        public void UpdateEmitter(EmitterConfig EmitterConfig)
        {
            this.EmitterConfig = EmitterConfig.CopyConfig(EmitterConfig);
        }

        // 发射方向
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.up * 1);
        }
    }
}