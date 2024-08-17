
namespace BH_Engine
{
    public interface IBulletBehaviourHandler
    {
        // 处理子弹在移动中的行为，每帧调用
        void HandleBulletBehaviour(BulletFinalConfig bulletFinalConfig, BulletBehaviour bulletBehaviour);
    }
}