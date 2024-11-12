using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class Character_Sprite : Character
    {
        private CanvasGroup rootCG => mainGame.GetComponent<CanvasGroup>();
        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab) : base(name, config, prefab)
        {
            rootCG.alpha = 0;

            Debug.Log($"Created Sprite Character: '{name}'");
        }

        public override IEnumerator ShowOrHidingCharacter(bool show)
        {
            float targerAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;

            while (self.alpha != targerAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targerAlpha, 3f * Time.deltaTime);
                yield return null;
            }

            co_Showing = null;
            co_hiding = null;
        }
    }
}