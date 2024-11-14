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
        private const string SpriteSheet_Default_SheetName = "Default";
        private const char SpriteSheet_Tex_Sprite_Delimiter = '-';
        private CanvasGroup rootCG => mainGame.GetComponent<CanvasGroup>();

        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        private string artAssetsDirectory = "";

        public override bool isVisible
        {  
            get { return isShowing || rootCG.alpha == 1; } 
            set { rootCG.alpha = value ? 1 : 0; }
        }

        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetFolder) : base(name, config, prefab)
        {
            rootCG.alpha = Enamble_On_Start ? 1 : 0;
            artAssetsDirectory = rootAssetFolder + "/Images";

            GetLayer();

            // Initialization of rootCG.alpha should be done in Awake or Start
            //Debug.Log($"Created Sprite Character: '{name}'");
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

                Image rendererImage = child.GetComponentInChildren<Image>();

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
            if (sprite == null)
            {
                Debug.LogWarning("Attempting to set a null sprite.");
            }

            layers[layer].SetSprite(sprite);
            //Debug.Log($"Sprite set on layer {layer}: {sprite}");
        }

        public Sprite GetSprite(string spriteName)
        {
            //Debug.Log($"Attempting to get sprite: {spriteName}");
            if (config.CharType == CharacterType.SpriteSheet)
            {
                string[] data = spriteName.Split(SpriteSheet_Tex_Sprite_Delimiter);
                Sprite[] spriteArray = new Sprite[0];

                if (data.Length == 2)
                {
                    string textureName = data[0];
                    spriteName = data[1];
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{textureName}");

                }
                else
                {
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{SpriteSheet_Default_SheetName}");
                }

                if (spriteArray.Length == 0)
                    Debug.Log($"Character '{name}' does not have a default art asset calle {SpriteSheet_Default_SheetName}");
                return Array.Find(spriteArray, sprite =>  sprite.name == spriteName);
            }
            else
            {
                return Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");
            }
        }

        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1)
        {
            CharacterSpriteLayer spriteLayer = layers[layer];
            return spriteLayer.TransitionSprite(sprite, speed);
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