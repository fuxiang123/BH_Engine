
namespace BH_Engine
{
    public interface IBulletMoveScript
    {
        // 处理子弹在移动中的行为，每帧调用
        void UpdateBulletMove(BulletFinalConfig bulletFinalConfig, BulletBehaviour bulletBehaviour);
    }

    public interface ICloneableBulletMoveScript : IBulletMoveScript
    {
        // 如果需要每个子弹持有不同的成员变量，需要提供当前脚本的实例拷贝，防止所有子弹共用一个脚本变量
        ICloneableBulletMoveScript Clone();
    }
}