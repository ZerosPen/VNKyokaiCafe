using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Characters
{
    public class CharacterSpriteLayer
    {
        private CharacterManager characterManager => CharacterManager.Instance;

        private const float Default_Transition_Speed = 2f;
        private const float transitionSpeedMultiplier = 1f;

        public int layer { get; private set; } = 0;
        public Image renderer { get; private set; } = null;
        public CanvasGroup rendererCG => renderer.GetComponent<CanvasGroup>();

        private List<CanvasGroup> oldRenderer = new List<CanvasGroup>();

        private Coroutine co_transitioningLayer = null;
        private Coroutine co_LevelingAlpha = null;
        public bool isTransitioningLayer => co_transitioningLayer != null;
        public bool isLevelingAlpha => co_LevelingAlpha != null;

        public CharacterSpriteLayer(Image defaultRenderer, int layer = 0)
        {
            renderer = defaultRenderer;
            this.layer = layer;
        }

        public void SetSprite(Sprite sprite)
        {
            if (renderer != null)
            {
                renderer.sprite = sprite;
            }
            else
                Debug.LogWarning("Missing renderer");
        }

        public Coroutine TransitionSprite(Sprite sprite, float speed = 1)
        {
            if (sprite == renderer.sprite)
                return null;

            if (isTransitioningLayer)
                characterManager.StopCoroutine(co_transitioningLayer);

            co_transitioningLayer = characterManager.StartCoroutine(TransitioningSprite(sprite, speed));

            return co_transitioningLayer;
        }

        private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier)
        {
            Image newRenderer = CreateRenderer(renderer.transform.parent);
            newRenderer.sprite = sprite;

            yield return TryStartLevelingAlpha();

            co_transitioningLayer = null;
        }

        private Image CreateRenderer(Transform parent)
        {
            Image newRenderer = Object.Instantiate(renderer, parent);
            oldRenderer.Add(rendererCG);

            newRenderer.name = renderer.name;
            renderer = newRenderer;
            rendererCG.alpha = 0; // Ensure this is set to 0 for fading in

            return newRenderer;
        }

        private Coroutine TryStartLevelingAlpha()
        {
            if (isLevelingAlpha)
            {
                // Optionally log that we're trying to start the alpha leveling
                Debug.Log("Alpha leveling is already in progress.");
                return co_LevelingAlpha; // Return existing coroutine if it's already running
            }

            co_LevelingAlpha = characterManager.StartCoroutine(RunAlphaLeveling());
            return co_LevelingAlpha;
        }

        private IEnumerator RunAlphaLeveling()
        {
            // Ensure new renderer starts with alpha 0
            rendererCG.alpha = 0;
            float speed = Default_Transition_Speed * transitionSpeedMultiplier;

            while (rendererCG.alpha < 1 || oldRenderer.Any(oldCG => oldCG.alpha > 0))
            {
                // Adjust alpha of the new renderer for fade-in effect
                rendererCG.alpha = Mathf.MoveTowards(rendererCG.alpha, 1, speed * Time.deltaTime);
                Debug.Log($"New Renderer Alpha: {rendererCG.alpha}");

                // Adjust alpha of old renderers for fade-out effect
                for (int i = oldRenderer.Count - 1; i >= 0; i--)
                {
                    CanvasGroup oldCG = oldRenderer[i];
                    oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed * Time.deltaTime);
                    Debug.Log($"Old Renderer {i} Alpha: {oldCG.alpha}");

                    if (oldCG.alpha <= 0)
                    {
                        oldRenderer.RemoveAt(i);
                        Object.Destroy(oldCG.gameObject);
                    }
                }

                yield return null; // Wait for the next frame
            }

            co_LevelingAlpha = null; // Reset coroutine reference after completion
        }
    }
}