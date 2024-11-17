using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using System.Linq;

namespace Commands
{
    public class CMD_DataBaseExtensionCharacter : cmd_DataBaseExtension
    {
        private static string[] Param_Enable => new string[] { "-e", "-enable" };
        private static string[] Param_Immediate => new string[] { "-i", "-immediate" };
        private static string[] Param_Speed => new string[] { "-spd", "-speed" };
        private static string[] Param_Smooth => new string[] { "-sm", "-smooth" };
        private static string Param_XPos => "-x";
        private static string Param_YPos => "-y";

        new public static void  Extend(CommandDataBase dataBase)
        {
            dataBase.addCommand("creatcharacter", new Action<string[]>(CreatCharacter));
            dataBase.addCommand("movecharacter", new Func<string[], IEnumerator>(MoveCharacter));
            dataBase.addCommand("show", new Func<string[], IEnumerator>(ShowAll));
            dataBase.addCommand("hide", new Func<string[], IEnumerator>(HideAll));
        }

        public static void CreatCharacter(string[] data)
        {
            string characterName =  data[0];
            bool enable = false;
            bool immediate = false;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(Param_Enable, out enable, defaultValue: false);
            parameters.TryGetValue(Param_Immediate, out immediate, defaultValue: false);

            Character character = CharacterManager.Instance.createChracter(characterName);

            if(!enable)
                return;

            if(immediate)
                character.isVisible = true;
            else if(enable)
                character.Show();
        }

        private static IEnumerator MoveCharacter(string[] data)
        {
            string characterName = data[0];
            Character character = CharacterManager.Instance.GetCharacter(characterName);
            if (character == null)
                yield break;

            float x = 0, y = 0;
            float speed = 1;
            bool smooth = false;
            bool immediate = false;

            var parameters = ConvertDataToParameters(data);

            //Try to get the X axis pos
            parameters.TryGetValue(Param_XPos, out x);

            //Try to get the Y axis pos
            parameters.TryGetValue(Param_YPos, out y);

            //Try to get the speed
            parameters.TryGetValue(Param_Speed, out speed, defaultValue: 1);

            //Try to get the smooth
            parameters.TryGetValue(Param_Smooth, out smooth, defaultValue: false);

            //Try to get the immedaite
            parameters.TryGetValue(Param_Immediate, out immediate, defaultValue: false);

            Vector2 position = new Vector2(x, y);

            if(immediate)
                character.SetPosition(position);
            else 
                yield return character.MoveToNewPosition(position, speed, smooth);
        }

        public static IEnumerator ShowAll(string[] data)
        {
            List<Character> chacarters = new List<Character>();
            bool immediate = false;

            foreach (string s in data)
            {
                Character character = CharacterManager.Instance.GetCharacter(s, creatIfDoesNotAxist: false);
                if (character != null) 
                    chacarters.Add(character);
            }

            if(chacarters.Count == 0)
                yield break;

            //Convert the data arry to a parameter container 
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(Param_Immediate, out immediate, defaultValue: false);

            //call the logic on all the character
            foreach(Character character in chacarters)
            {
                if(immediate)
                    character.isVisible = true;
                else 
                    character.Show();
            }

            if (!immediate)
            {
                while (chacarters.Any(c => c.isShowing))
                    yield return null;
            }
        }

        public static IEnumerator HideAll(string[] data)
        {
            List<Character> chacarters = new List<Character>();
            bool immediate = false;

            foreach (string s in data)
            {
                Character character = CharacterManager.Instance.GetCharacter(s, creatIfDoesNotAxist: false);
                if (character != null)
                    chacarters.Add(character);
            }

            if (chacarters.Count == 0)
                yield break;

            //Convert the data arry to a parameter container 
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(Param_Immediate, out immediate, defaultValue: false);

            //call the logic on all the character
            foreach (Character character in chacarters)
            {
                if (immediate)
                    character.isVisible = false;
                else
                    character.Hide();
            }

            if (!immediate)
            {
                while (chacarters.Any(c => c.isHidding))
                    yield return null;
            }
        }

    }
}