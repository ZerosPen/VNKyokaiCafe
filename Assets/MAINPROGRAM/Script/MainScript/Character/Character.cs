using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Characters
{
    public abstract class Character
    {
        public string name = "";
        public string displayName = "";
        public RectTransform mainGame = null;
        public CharacterConfigData config;
        public Animator animator;

        //Coroutine
        protected Coroutine co_Showing, co_hiding;

        protected CharacterManager manager => CharacterManager.Instance;
        public DialogController dialogController => DialogController.Instance;

        public bool isShowing => co_Showing != null;
        public bool isHidding => co_hiding != null;
        public virtual bool isVisible => false;

        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;

            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, manager.characterPanel);
                ob.SetActive(true);
                mainGame = ob.GetComponent<RectTransform>();
                animator = mainGame.GetComponentInChildren<Animator>();
            }
        }

        public Coroutine Say(string dialogue) => Say(new List<string>() { dialogue });
        
        public Coroutine Say(List<string> dialogue)
        {
            dialogController.showSpeakerName(displayName);
            UpdateTextCostumizationOnScreen();
            return dialogController.Say(dialogue);
        }

        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;

        public void ResetConfiguratioData() => config = CharacterManager.Instance.GetCharacterConfig(name);

        public void UpdateTextCostumizationOnScreen() => dialogController.ApplySpeakerDataToDialogContainer(config);

        public virtual Coroutine Show()
        {
            if(isShowing)
                return co_Showing;
            if (isHidding)
                manager.StopCoroutine(co_hiding);

            co_Showing = manager.StartCoroutine(ShowOrHidingCharacter(true));

            return co_Showing;
        }

        public virtual Coroutine Hide()
        {
            if (isHidding)
                return co_hiding;
            if (isShowing)
                manager.StopCoroutine(co_Showing);

            co_hiding = manager.StartCoroutine(ShowOrHidingCharacter(false));

            return co_hiding;
        }

        public virtual IEnumerator ShowOrHidingCharacter(bool show)
        {
            Debug.Log("Show/Hide cannot be called from a base charType");
            yield return null;
        }

        public enum CharacterType
        {
            text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D
        }

    }
}