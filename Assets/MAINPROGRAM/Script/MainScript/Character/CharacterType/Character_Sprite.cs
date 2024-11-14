using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Characters
{
    public class Character_Sprite : Character
    {
        private const string Sprite_Renderer_Parent_Name = "Renderers";
        private CanvasGroup rootCG => mainGame.GetComponent<CanvasGroup>();

        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        private string artAssetsDirectory = "";

        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetFolder) : base(name, config, prefab)
        {
            rootCG.alpha = 0;
            artAssetsDirectory = rootAssetFolder + "/Images";

            GetLayer();

            // Initialization of rootCG.alpha should be done in Awake or Start
            Debug.Log($"Created Sprite Character: '{name}'");
        }

        private void GetLayer()
        {
            Transform rendererRoot = animator.transform.Find(Sprite_Renderer_Parent_Name);

            if(rendererRoot == null)
            {
                return;
            }

            for(int i = 0; i < rendererRoot.transform.childCount; i++)
            {
                Transform child = rendererRoot.transform.GetChild(i);

                Image rendererImage = child.GetComponent<Image>();

                if(rendererImage != null)
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    layers.Add(layer);
                    child.name = $"Layer : {i}";
                }
            }
        }

        public void SetSprite(Sprite sprite, int layer = 0)
        {
            layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName)
        {
            if(config.CharType == CharacterType.SpriteSheet)
            {
                return null;
            }
            else
            {
                Sprite loadedSprite = Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");

                if (loadedSprite == null)
                {
                    Debug.LogWarning($"Sprite '{spriteName}' not found in '{artAssetsDirectory}' directory.");
                }

                return loadedSprite;
            }
        }

        public override IEnumerator ShowOrHidingCharacter(bool show)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;

            while(self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime);
                yield return null;
            }
            co_Showing = null;
            co_hiding  = null;
        }
    }
}