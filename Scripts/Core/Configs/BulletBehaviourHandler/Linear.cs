
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    [LabelText("直线移动")]
    public class Linear : IBulletBehaviourHandler
    {
        public void HandleBulletBehaviour(BulletFinalConfig bulletFinalConfig, ActiveBullet activeBullet)
        {
            // 经过的总时间
            float time = activeBullet.currentTime;
            float acceleration = bulletFinalConfig.acceleration;
            float speed = bulletFinalConfig.speed + acceleration * time;
            var direction = activeBullet.bullet.transform.up;
            activeBullet.bullet.transform.position = activeBullet.bullet.transform.position + direction * speed * Time.deltaTime;
        }
    }
}