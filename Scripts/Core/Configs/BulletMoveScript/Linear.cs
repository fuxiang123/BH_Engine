
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    [LabelText("直线移动")]
    public class Linear : IBulletMoveScript
    {
        public void UpdateBulletMove(BulletFinalConfig bulletFinalConfig, BulletBehaviour bulletBehaviour)
        {
            // 经过的总时间
            float time = bulletBehaviour.currentTime;
            float acceleration = bulletFinalConfig.acceleration;
            float speed = bulletFinalConfig.speed + acceleration * time;
            var direction = bulletBehaviour.direction;
            bulletBehaviour.transform.position = bulletBehaviour.transform.position + direction * speed * Time.deltaTime;
        }
    }
}