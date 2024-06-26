
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    public class ProfileEmitter : BaseEmitter
    {
        [SerializeField]
        private EmitterProfileSO EmitterProfileSO;
        [HideInInspector]
        public EmitterProfileSO EmitterProfile;

        private void Awake()
        {
            EmitterProfile = EmitterProfileSO.Copy(EmitterProfileSO);
            EmitterConfig = EmitterProfile.emitterConfig;
            BulletConfig = EmitterProfile.bulletConfig;
            PatternConfig = EmitterProfile.patternConfig;
            base.Awake();
        }

        public void SetEmitterProfile(EmitterProfileSO profile)
        {
            EmitterProfile = profile;
        }
    }
}