
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
        private bool isInit = false;

        void Awake()
        {
            // 如果在Awake之前就调用过InitEmitterProfile，则不进行初始化
            if (EmitterProfileSO != null && !isInit)
                InitEmitterProfile();
        }

        void InitEmitterProfile()
        {
            isInit = true;
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