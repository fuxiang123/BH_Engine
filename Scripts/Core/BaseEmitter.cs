using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
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
        [LabelText("自动射击延迟")]
        public float AutoEmitDelay = 0;
        [HideInInspector] public Action OnEmit; // 发射子弹时调用
        private bool isAutoEmitDelay = false;
        private float mTimer = 0;

        protected void OnDisable()
        {
            StopShoot();
        }

        protected void FixedUpdate()
        {
            if (IsAutoEmit)
            {
                if (!isAutoEmitDelay)
                {
                    mTimer += Time.deltaTime;
                    if (mTimer >= AutoEmitDelay)
                    {
                        isAutoEmitDelay = true;
                        mTimer = 0;
                    }
                }
                else
                {
                    mTimer += Time.deltaTime;
                    if (mTimer >= EmitterConfig.emitInterval.value)
                    {
                        Emit();
                        mTimer = 0;
                    }
                }
            }
        }

        // 开始或继续射击
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
            isAutoEmitDelay = false;
            mTimer = 0;
            BulletConfig.ResetConfig(BulletConfig);
            PatternConfig.ResetConfig(PatternConfig);
        }

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

        // 发射单行子弹
        public void Emit()
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
                // 旋转角度减少90
                bullet.transform.rotation = baseRotation * Quaternion.Euler(0, 0, 90);
                bullet.GetComponent<BulletBehaviour>().Init(BulletConfig, direction, ReleaseBullet);
            }
            OnEmit?.Invoke();
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