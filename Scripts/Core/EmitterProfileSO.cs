
using UnityEngine;

namespace BH_Engine
{
    [CreateAssetMenu(fileName = "EmitterProfile", menuName = "BH_Engine/EmitterProfileSO")]
    public class EmitterProfileSO : ScriptableObject
    {
        public EmitterConfig EmitterConfig = new EmitterConfig();
        public BulletConfig BulletConfig = new BulletConfig();
        public PatternConfig PatternConfig = new PatternConfig();

        public static EmitterProfileSO Copy(EmitterProfileSO emitterProfile)
        {
            var instance = CreateInstance<EmitterProfileSO>();
            instance.EmitterConfig = EmitterConfig.CopyConfig(emitterProfile.EmitterConfig);
            instance.BulletConfig = BulletConfig.CopyConfig(emitterProfile.BulletConfig);
            instance.PatternConfig = PatternConfig.CopyConfig(emitterProfile.PatternConfig);
            return instance;
        }
    }
}