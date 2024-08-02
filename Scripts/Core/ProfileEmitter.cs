
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH_Engine
{
    public class ProfileEmitter : BaseEmitter
    {
        [SerializeField]
        public EmitterProfileSO EmitterProfileSO;
        [HideInInspector]
        private EmitterProfileSO mEmitterProfile;

        private void Awake()
        {
            if (EmitterProfileSO != null) InitEmitterProfile();
        }

        private void InitEmitterProfile()
        {
            mEmitterProfile = EmitterProfileSO.Copy(EmitterProfileSO);
            EmitterConfig = mEmitterProfile.emitterConfig;
            BulletConfig = mEmitterProfile.bulletConfig;
            PatternConfig = mEmitterProfile.patternConfig;
        }

        public void SetEmitterProfile(EmitterProfileSO profile)
        {
            EmitterProfileSO = profile;
            InitEmitterProfile();
        }

        public EmitterProfileSO GetEmitterProfile()
        {
            return mEmitterProfile;
        }
    }
}