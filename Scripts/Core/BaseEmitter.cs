using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VisualScripting;
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
        private float mTimer = 0;

        protected void OnDisable()
        {
            StopShoot();
        }

        protected void Update()
        {
            if (IsAutoEmit)
            {
                mTimer += Time.deltaTime;
                if (mTimer >= EmitterConfig.emitInterval.value)
                {
                    Emit();
                    mTimer = 0;
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
            mTimer = 0;
            BulletConfig.ResetConfig(BulletConfig);
            PatternConfig.ResetConfig(PatternConfig);
        }

        // 发射单行子弹
        public void Emit()
        {
            BulletFinalConfig bulletFinalConfig = BulletConfig.GetFinalConfig(BulletConfig);
            PatternFinalConfig patternFinalConfig = PatternConfig.GetPatternFinalConfig(PatternConfig);
            transform.localRotation = Quaternion.Euler(0, 0, EmitterConfig.emitterAngle.value);
            transform.localPosition = new Vector3(patternFinalConfig.spwanXTanslate, patternFinalConfig.spwanYTanslate, transform.position.z);

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
            int[] realSpreadAnglePerbullet = new int[patternCount - 1];
            // 计算总角度
            float totalAngle = 0;
            for (int i = 0; i < patternCount - 1; i++)
            {
                realSpreadAnglePerbullet[i] = patternFinalConfig.spreadAnglePerbullet[i] + patternFinalConfig.spreadAngleTotal / (patternCount - 1);
                totalAngle += realSpreadAnglePerbullet[i];
            }

            var prefabSprite = BulletConfig.prefab.GetComponent<SpriteRenderer>();
            var bullets = BulletPoolManager.Instance.Get(patternCount);
            // 当前子弹xspacing的位置, 只有一个子弹时从正中发射即可
            float currentBulletXpacing = patternCount == 1 ? 0 : -xSpacingTotalFinal / 2;
            // 当前子弹的角度，只有一个子弹时从正中发射即可
            float currentBulletSpreadAngle = patternCount == 1 ? 0 : -totalAngle / 2;
            for (int i = 0; i < patternCount; i++)
            {
                // 修改对象池子弹的sprite
                var spriteRenderer = bullets[i].GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && prefabSprite != null)
                {
                    spriteRenderer.sprite = prefabSprite.sprite;
                    spriteRenderer.color = prefabSprite.color;
                    spriteRenderer.flipX = prefabSprite.flipX;
                    spriteRenderer.flipY = prefabSprite.flipY;
                    spriteRenderer.drawMode = prefabSprite.drawMode;
                    spriteRenderer.maskInteraction = prefabSprite.maskInteraction;
                    spriteRenderer.spriteSortPoint = prefabSprite.spriteSortPoint;
                    spriteRenderer.sortingLayerID = prefabSprite.sortingLayerID;
                    spriteRenderer.sortingLayerName = prefabSprite.sortingLayerName;
                    spriteRenderer.sortingOrder = prefabSprite.sortingOrder;
                    spriteRenderer.gameObject.layer = prefabSprite.gameObject.layer;
                }

                // 计算子弹的位置
                currentBulletXpacing += i == 0 ? 0 : realXSpacingPerbullet[i - 1];
                // 当前子弹的实际生成位置
                Vector3 spawnPosition = transform.position + (transform.right * currentBulletXpacing);
                bullets[i].transform.position = spawnPosition;

                // 计算子弹的角度
                currentBulletSpreadAngle += i == 0 ? 0 : realSpreadAnglePerbullet[i - 1];
                bullets[i].transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -currentBulletSpreadAngle);
                BulletBehaviourManager.Instance.AddActiveBullet(bullets[i], bulletFinalConfig);
            }
        }



        public void UpdateBullet(BulletConfig BulletConfig)
        {
            this.BulletConfig = BulletConfig;
        }

        public void UpdatePattern(PatternConfig PatternConfig)
        {
            this.PatternConfig = PatternConfig;
        }

        public void UpdateEmitter(EmitterConfig EmitterConfig)
        {
            this.EmitterConfig = EmitterConfig;
        }

        // 发射方向
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.up * 1);
        }
    }
}