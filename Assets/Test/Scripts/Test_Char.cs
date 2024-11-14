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
            //Character_Sprite Student = CreatCharacter("Female Student 2") as Character_Sprite;
            Guard.isVisible = false;

            yield return new WaitForSeconds(1);

            Sprite body = Raelin.GetSprite("B2");
            Sprite face = Raelin.GetSprite("B_Blush");
            Raelin.TransitionSprite(body);
            yield return Raelin.TransitionSprite(face, 1);

            //Raelin.MoveToNewPosition(Vector2.zero);
            //Guard.Show();
            //yield return Guard.MoveToNewPosition(new Vector2(1, 0));

            yield return Raelin.TransitionSprite(Raelin.GetSprite("B_Shock"), layer:1);
            Raelin.TransitionSprite(Raelin.GetSprite("B1"));

            //body = Guard.GetSprite("Monk");
            //Guard.TransitionSprite(body);
            

            yield return null;
        }

        // Update is called once per frame
        void Update()
        {
            // This method can be used for future updates
        }
    }
}