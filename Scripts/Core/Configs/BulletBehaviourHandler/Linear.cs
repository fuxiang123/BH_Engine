
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    [LabelText("直线移动")]
    public class Linear : IBulletBehaviourHandler
    {
        public void HandleBulletBehaviour(BulletFinalConfig bulletFinalConfig, ActiveBullet activeBullet, float time)
        {
            float speed = bulletFinalConfig.speed;
            float acceleration = bulletFinalConfig.acceleration;
            var direction = activeBullet.bullet.transform.up;
            activeBullet.bullet.transform.position = activeBullet.spwanPosition + direction * speed * time + direction * acceleration * time * time / 2;
        }
    }
}