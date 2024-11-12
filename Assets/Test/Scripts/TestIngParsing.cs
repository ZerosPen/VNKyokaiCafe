using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class TestIngParsing : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SendFileToParse();
        }

        void SendFileToParse()
        {
            List<string> lines = FileManager.readTxtAsset("textFile");

            foreach (string line in lines)
            {
                if (line == string.Empty)
                    continue;
                DailogLine dl = DailogParser.Parse(line);
            }
        }
    }
}

