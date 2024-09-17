
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    [LabelText("正弦波移动")]
    public class SinWave : IBulletMoveScript
    {
        [LabelText("振幅，波的高度")]
        public float amplitude = 2f; // 正弦波振幅
        [LabelText("频率，波的宽度")]
        public float frequency = 2f; // 正弦波频率
        public void UpdateBulletMove(BulletFinalConfig bulletFinalConfig, BulletBehaviour bulletBehaviour)
        {
            float time = bulletBehaviour.currentTime;
            float acceleration = bulletFinalConfig.acceleration;
            float speed = bulletFinalConfig.speed + acceleration * time;

            var sine = amplitude * Mathf.Sin(frequency * time * speed); // 只使用时间来计算正弦波
            var direction = bulletBehaviour.direction;
            var spawnPosition = bulletBehaviour.spawnPosition;
            // 定义一条垂直的轴，在垂直轴上施加正弦函数
            var crossDirection = new Vector3(direction.y, -direction.x);
            bulletBehaviour.transform.position = spawnPosition + (direction * speed * time) + (crossDirection * sine);
        }
    }
}