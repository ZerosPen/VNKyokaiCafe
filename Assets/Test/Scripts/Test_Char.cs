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

        public TMP_FontAsset tempFont;
        // Start is called before the first frame update
        void Start()
        {
            //Character Raelin = CharacterManager.Instance.createChracter("Raelin"); // Corrected spelling
            //Character Lila = CharacterManager.Instance.createChracter("Lila"); // Corrected spelling
            //Character Ran = CharacterManager.Instance.createChracter("Ran"); // Corrected spelling
            //Character Lilo = CharacterManager.Instance.createChracter("Lilo"); // Corrected spelling
            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            Character Raelin = CharacterManager.Instance.createChracter("Raelin");

            yield return new WaitForSeconds(1f);

            yield return Raelin.Hide();

            yield return new WaitForSeconds(0.5f);

            yield return Raelin.Show();

            yield return Raelin.Say("Hello");
        }

        // Update is called once per frame
        void Update()
        {
            // This method can be used for future updates
        }
    }
}