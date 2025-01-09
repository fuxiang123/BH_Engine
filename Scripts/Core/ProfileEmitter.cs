
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

        void Awake()
        {
            if (EmitterProfileSO != null) InitEmitterProfile();
        }

        void InitEmitterProfile()
        {
            mEmitterProfile = EmitterProfileSO.Copy(EmitterProfileSO);
            EmitterConfig = mEmitterProfile.EmitterConfig;
            BulletConfig = mEmitterProfile.BulletConfig;
            PatternConfig = mEmitterProfile.PatternConfig;
        }

        [ShowInInspector]
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