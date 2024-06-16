
using UnityEngine;

namespace BH_Engine
{
    [CreateAssetMenu(fileName = "EmitterProfile", menuName = "BH_Engine/EmitterProfileSO")]
    public class EmitterProfileSO : ScriptableObject
    {
        public EmitterConfig emitterConfig;
        public BulletConfig bulletConfig;
        public PatternConfig patternConfig;

        public static EmitterProfileSO Copy(EmitterProfileSO emitterProfile)
        {
            var instance = CreateInstance<EmitterProfileSO>();
            instance.emitterConfig = EmitterConfig.CopyConfig(emitterProfile.emitterConfig);
            instance.bulletConfig = BulletConfig.CopyConfig(emitterProfile.bulletConfig);
            instance.patternConfig = PatternConfig.CopyConfig(emitterProfile.patternConfig);
            return instance;
        }
    }
}