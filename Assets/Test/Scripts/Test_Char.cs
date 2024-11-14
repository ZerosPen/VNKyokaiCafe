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
            //Character Raelin = CharacterManager.Instance.createChracter("Raelin"); // Corrected spelling
            //Character Lila = CharacterManager.Instance.createChracter("Lila"); // Corrected spelling
            //Character Ran = CharacterManager.Instance.createChracter("Ran"); // Corrected spelling
            //Character Lilo = CharacterManager.Instance.createChracter("Lilo"); // Corrected spelling
            StartCoroutine(SecondTest());
        }

        //IEnumerator Test()
        //{
        //    //Character Raelin = CreatCharacter("Raelin");
        //    Character Guard1 = CreatCharacter("guard1 as Generic");
        //    Character Guard2 = CreatCharacter("guard2 as Generic");
        //    Character Guard3 = CreatCharacter("guard3 as Generic");

        //    Guard1.Show();
        //    Guard2.Show();
        //    Guard2.Show();

        //    //yield return Raelin.Say("Hello");
        //    //yield return Raelin.Say("Hello again");
        //    //yield return Raelin.Say("Hello again again");
        //    //yield return Raelin.Say("Hello again again again");

        //    yield return Guard1.Say("MAKE A WAY FORM YOUR LAND LORD!");
        //    yield return Guard2.Say("YOU!{wc 1}move from the street.");
        //    yield return Guard3.Say("HEY!{wc 0.5} WATCH YOUR STEP.");
        //}

        IEnumerator SecondTest()
        {
            Character_Sprite Guard = CreatCharacter("guard1 as Generic") as Character_Sprite;
            Character_Sprite Raelin = CreatCharacter("Raelin") as Character_Sprite;
            Character fs = CreatCharacter("Female Student 2");

            Sprite bodySprite = Raelin.GetSprite("Raelin_2");
            Sprite faceSprite = Raelin.GetSprite("Raelin_12");

            Raelin.SetSprite(bodySprite, 0);
            Raelin.SetSprite(faceSprite, 1);

            Guard.Show();
            Raelin.Show();
            fs.Show();

            Guard.SetPosition(Vector2.zero);
            Raelin.SetPosition(new Vector2(0.5f, 0.5f));
            fs.SetPosition(Vector2.one);

            yield return Guard.MoveToNewPosition(Vector2.one);
            yield return Guard.MoveToNewPosition(Vector2.zero);


            Guard.SetNameFont(tempFont);
            Guard.SetDialogueFont(tempFont);
            Guard.SetDialogueColor(Color.yellow);
            Raelin.SetDialogueColor(Color.red);
            fs.SetDialogueColor(Color.cyan);

            yield return Guard.Say("MAKE A WAY FORM YOUR LAND LORD!");
            yield return fs.Say("YOU!{wc 1}move from the street.");
            yield return Raelin.Say("HEY!{wc 0.5} WATCH YOUR STEP.");
        }

        // Update is called once per frame
        void Update()
        {
            // This method can be used for future updates
        }
    }
}