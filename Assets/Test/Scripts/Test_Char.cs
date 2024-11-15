using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using DIALOGUE;
using TMPro;

namespace TESTING
{
    public class Test_Char : MonoBehaviour
    {
        private Character CreatCharacter(string name) => CharacterManager.Instance.createChracter(name);
        public TMP_FontAsset tempFont;
        // Start is called before the first frame update
        void Start()
        {
            
            StartCoroutine(SecondTest());
        }

        IEnumerator SecondTest()
        {
            Character_Sprite Raelin = CreatCharacter("Raelin") as Character_Sprite;
           // Character_Sprite Guard1 = CreatCharacter("guard1 as Generic") as Character_Sprite;
            Character_Sprite Monk = CreatCharacter("Monk as Generic") as Character_Sprite;
            // Character_Sprite Guard = CreatCharacter("Generic") as Character_Sprite;
            Sprite body = Monk.GetSprite("Monk");
            Monk.SetSprite(body);

            Raelin.SetPosition(Vector2.zero);
            Monk.SetPosition(new Vector2(1, 0));

            yield return new WaitForSeconds(1);

            Raelin.FaceRight();
            yield return Raelin.MoveToNewPosition(new Vector2(0.35f, 0));
            Raelin.TransitionSprite(Raelin.GetSprite("A1"));
            Raelin.TransitionSprite(Raelin.GetSprite("A_Scold"), layer:1);
            Raelin.Animate("Hop");
            yield return Raelin.Say("AHHh{wc 1} Another mage");

            yield return Monk.Say("HAHAAH{wa 1} another Traveler just come by");

            Raelin.TransitionSprite(Raelin.GetSprite("A_Shock"), layer: 1);
            Raelin.Animate("Shiver", true);
            yield return Raelin.Say(". . .");

            yield return Monk.Say("A your scare little girl?");

            Raelin.TransitionSprite(Raelin.GetSprite("A_Worried"), layer: 1);
            Raelin.Animate("Shiver", false);
            yield return Raelin.Say("No");

            yield return null;
        }

        // Update is called once per frame
        void Update()
        {
            // This method can be used for future updates
        }
    }
}