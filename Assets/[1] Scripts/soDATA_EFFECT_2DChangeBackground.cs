using UnityEngine;

namespace Ash {
    namespace SOEffectSystem {
        [CreateAssetMenu(fileName = "[Effect] New [ENDPOINT][CHANGEBACKGROUND]", menuName = "EFFECT/ENDPOINT/++CHANGEBACKGROUND")]
        public class EFFECT_2DChangeBackground : soDATA_Effect
        {
            [SerializeField] Sprite background;
            public override void Apply(GameObject target = null)
            {
                GameManager.SetBackground(background);
                
                base.Apply(target);
            }
        }
    }
}