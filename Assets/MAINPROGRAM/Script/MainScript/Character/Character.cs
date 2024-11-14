using DIALOGUE;
using System.Collections.Generic;
using System.Collections;
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

        // Coroutine references
        protected Coroutine co_Showing, co_hiding, co_moving;

        protected CharacterManager manager => CharacterManager.Instance;
        public DialogController dialogController => DialogController.Instance;

        public bool isShowing => co_Showing != null;
        public bool isHidding => co_hiding != null;
        public bool isMoving => co_moving != null;
        public virtual bool isVisible => false;

        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;

            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, manager.characterPanel);
                ob.name = manager.FormatCharacterPath(manager.CharacterPerfabNameFormat, name);
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
            if (isShowing)
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

        public virtual void SetPosition(Vector2 position)
        {
            if (mainGame == null)
                return;

            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorPointTargets(position);
            mainGame.anchorMin = minAnchorTarget;
            mainGame.anchorMax = maxAnchorTarget;
        }
        public virtual Coroutine MoveToNewPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            if (mainGame == null)
                return null;

            if (isMoving)
                manager.StopCoroutine(co_moving);

            co_moving = manager.StartCoroutine(MovingToPosition(position, speed, smooth));

            return co_moving;
        }


        private IEnumerator MovingToPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorPointTargets(position);
            Vector2 padding = mainGame.anchorMax - mainGame.anchorMin;

            while(mainGame.anchorMin != minAnchorTarget ||  mainGame.anchorMax != maxAnchorTarget)
            {
                mainGame.anchorMin = smooth ? 
                    Vector2.Lerp(mainGame.anchorMin, minAnchorTarget, speed * Time.deltaTime)
                    : Vector2.MoveTowards(mainGame.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);

                mainGame.anchorMax = mainGame.anchorMin + padding;

                if (smooth && Vector2.Distance(mainGame.anchorMin, minAnchorTarget) <= 0.001f)
                {
                    mainGame.anchorMin = minAnchorTarget;
                    mainGame.anchorMax = maxAnchorTarget;
                    break;
                }

                yield return null;
            }

            Debug.Log("Done Moving");
            co_moving = null;
        }

        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorPointTargets(Vector2 position)
        {
            Vector2 padding = mainGame.anchorMax - mainGame.anchorMin;

            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);

            Vector2 maxAnchorTarget = minAnchorTarget + padding;

            return (minAnchorTarget, maxAnchorTarget);
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