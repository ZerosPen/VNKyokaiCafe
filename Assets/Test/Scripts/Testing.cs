using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTINGSC 
{
    public class Testing : MonoBehaviour
    {
        DialogController dc;
        TextArchitech txtArc;

        public TextArchitech.BuildMethod bm = TextArchitech.BuildMethod.instant;

        string[] lines = new string[5]
        {
            "This place is bit creapy, right?",
            "Why we going in middle of no where in late at night?",
            "But I have part-time job tomorrow in cafe.",
            "So what we going to do tonight?",
            "Find A GHOST??? what are you thingking of??",
        };

        // Start is called before the first frame update
        void Start()
        {
            dc = DialogController.Instance;
            txtArc = new TextArchitech(dc.DialogContainer.DialogText);
            txtArc.buildMetdod = TextArchitech.BuildMethod.fade;
            txtArc.speed = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            if (bm != txtArc.buildMetdod)
            {
                txtArc.buildMetdod = bm;
                txtArc.Stop();
            }

            if (Input.GetKeyDown(KeyCode.S))
                txtArc.Stop();

            string longLine = "I want you to know that i love some thing is very long and it make sound like not make sense but i like it, do you like that stuff? i like the stuff, we all love the stuff even a goat is willing to doing a stuff";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (txtArc.isBuildingText)
                {
                    if (!txtArc.speedUP)
                        txtArc.speedUP = true;
                    else
                        txtArc.forceComplete();
                }
                else
                    txtArc.Build(longLine);
                //txtArc.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                txtArc.Append(longLine);
                //txtArc.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}
