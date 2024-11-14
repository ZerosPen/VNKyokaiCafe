using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogController.Instance.config.characterConfigationAsset;

        //private const string Char
        private const string Character_Casting_ID = " as ";
        private const string CharacterNameID = "<charactername>";
        public string characterRootPathFormat => $"Characters/{CharacterNameID}";
        public  string CharacterPerfabNameFormat => $"Character - [{CharacterNameID}]";
        public  string characterPrefabPathFormat => $"{characterRootPathFormat}/{CharacterPerfabNameFormat}";

        [SerializeField] private RectTransform _characterPanel = null;
        public RectTransform characterPanel => _characterPanel;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                DestroyImmediate(gameObject);
        }

        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }

        public Character GetCharacter(string charaterName, bool creatIfDoesNotAxist = false)
        {
            if (characters.ContainsKey(charaterName.ToLower()))
            {
                return characters[charaterName.ToLower()];
            }
            else if (creatIfDoesNotAxist)
            {
                return createChracter(charaterName);
            }

            return null;
        }

        public Character createChracter(string characterName)
        {
            //check you are making duplic character
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A Character called '{characterName}' already exist.  Do not create the character");
                return null;
            }

            CharacterInfo info = getCharacterInfo(characterName);

            Character character = CreaterCharFromInfo(info);

            characters.Add(characterName.ToLower(), character);

            return character;
        }

        private CharacterInfo getCharacterInfo(string characterName)
        {
            CharacterInfo result = new CharacterInfo();

            string[] nameData = characterName.Split(Character_Casting_ID, System.StringSplitOptions.RemoveEmptyEntries);
            result.name = nameData[0];
            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;

            result.config = config.GetConfig(result.castingName);

            result.prefab = getPrefabForCharacter(result.castingName);

            result.rootCharacterFolder = FormatCharacterPath(characterRootPathFormat, result.castingName);

            return result;
        }

        private  GameObject getPrefabForCharacter(string characterName)
        {
           string prefabPath = FormatCharacterPath(characterPrefabPathFormat, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        public  string FormatCharacterPath(string path, string characterName) => path.Replace(CharacterNameID, characterName);


        private Character CreaterCharFromInfo(CharacterInfo info)
        {
            CharacterConfigData config = info.config;

            switch (config.CharType)
            {
                case Character.CharacterType.text:
                    return new Character_Text(info.name, config);

                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.name, config, info.prefab, info.rootCharacterFolder);

                case Character.CharacterType.Live2D:
                    return new Character_Live2D(info.name, config, info.prefab, info.rootCharacterFolder);

                case Character.CharacterType.Model3D:
                    return new Character_Model3D(info.name, config, info.prefab, info.rootCharacterFolder);

                default:
                    return null;
            }
        }

        private class CharacterInfo
        {
            public string name = "";
            public string castingName = "";

            public string rootCharacterFolder = "";
            public CharacterConfigData config = null;
            public GameObject prefab = null;
        }
    }
}