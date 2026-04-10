using UnityEngine;

namespace Ash{
    namespace SOEffectSystem{
        [CreateAssetMenu(fileName = "[Effect] New [ENDPOINT][EXPLICIT]", menuName = "EFFECT/ENDPOINT/++EXPLICIT || Uses enums to describe destinct commands that are unique and unexpandable!")]
        public class EFFECT_Explicit : soDATA_Effect
        {
            [Space(10)]

            [Header("-Explicit-")]
            [SerializeField] EFFECT_ExplicitType type;

            public override void Apply(GameObject target = null)
            {
                switch (type)
                {
                    case EFFECT_ExplicitType.None:
                        break;
                    case EFFECT_ExplicitType.ThrowKnife:
                        KnifeThrower.ThrowKnife();
                        break;
                    case EFFECT_ExplicitType.EndDay:
                        GameManager.EndDay();
                        break;
                    case EFFECT_ExplicitType.EndNight:
                        GameManager.EndNight();
                        break;
                    case EFFECT_ExplicitType.SwitchTo3D:
                        GameManager.SwitchTo3D();
                        break;
                    case EFFECT_ExplicitType.SwitchTo2D:
                        GameManager.SwitchTo2D();
                        break;
                }
            }
        }

        enum EFFECT_ExplicitType
        {
            None,
            ThrowKnife,
            EndDay,
            EndNight,
            SwitchTo3D,
            SwitchTo2D
        }
    }
}