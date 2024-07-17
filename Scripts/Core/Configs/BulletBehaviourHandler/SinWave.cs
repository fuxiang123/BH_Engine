
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    [LabelText("直线移动")]
    public class SinWave : IBulletBehaviourHandler
    {
        [LabelText("振幅，波的高度")]
        public float amplitude = 2f; // 正弦波振幅
        [LabelText("频率，波的宽度")]
        public float frequency = 2f; // 正弦波频率
        public void HandleBulletBehaviour(BulletFinalConfig bulletFinalConfig, ActiveBullet activeBullet)
        {
            float time = activeBullet.currentTime;
            float acceleration = bulletFinalConfig.acceleration;
            float speed = bulletFinalConfig.speed + acceleration * time;

            var sine = amplitude * Mathf.Sin(frequency * time * speed);
            var direction = activeBullet.bullet.transform.up;
            var spwanPosition = activeBullet.spwanPosition;
            // 定义一条垂直的轴，在垂直轴上施加正弦函数
            var crossDirection = new Vector3(direction.y, -direction.x);
            activeBullet.bullet.transform.position = spwanPosition + (direction * speed * time) + (crossDirection * sine);
        }
    }
}