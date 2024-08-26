
namespace BH_Engine
{
    public interface IBulletMoveScript
    {
        // 处理子弹在移动中的行为，每帧调用
        void UpdateBulletMove(BulletFinalConfig bulletFinalConfig, BulletBehaviour bulletBehaviour);
    }
}