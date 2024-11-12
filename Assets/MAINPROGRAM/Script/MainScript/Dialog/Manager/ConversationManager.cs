using Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogController dialogController => DialogController.Instance;
        private Coroutine process = null;
        public bool isRunning => process != null;

        private TextArchitech TxtArch = null;
        private bool UserPromt = false;
        public ConversationManager(TextArchitech TxtArch)
        {
            this.TxtArch = TxtArch;
            dialogController.onUserPrompt_Next += OnUserPromt_Next;
        }

        private void OnUserPromt_Next()
        {
            UserPromt = true;
        }

        public Coroutine startConversation(List<string> conversation)
        {
            stopConversation();

            process = dialogController.StartCoroutine(RunningConversation(conversation));

            return process;
        }

        public void stopConversation()
        {
            if (!isRunning)
            {
                return;
            }
            dialogController.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;
                DailogLine line = DailogParser.Parse(conversation[i]);
                //show dialogue
                if (line.hasDialogue)
                {
                    yield return Line_RunDialogue(line);
                }

                // shoew commmand
                if (line.hasCommands)
                {
                    yield return Line_RunCommands(line);
                }

                if(line.hasDialogue)
                    //wiat for userInput
                    yield return WaitForUserInput();
            }
        }
        IEnumerator Line_RunDialogue(DailogLine line)
        {
            //show or hide char name
            if (line.hasSpeaker)
                dialogController.showSpeakerName(line.speakerData.displayName);

            //build dailog
            yield return BuildLineSegments(line.dialogueData);

        }

        IEnumerator Line_RunCommands(DailogLine line)
        {
           List<DL_CommandData.Command> commands = line.commandData.commands;
           foreach(DL_CommandData.Command command in commands)
            {
                if (command.waitForComplete)
                {
                    yield return CommandManager.Instance.Executed(command.name, command.arguments);
                }
                else
                    CommandManager.Instance.Executed(command.name, command.arguments);
            }
           yield return null;
        }

        IEnumerator BuildLineSegments(DL_DialogueData line)
        {
            for(int i = 0; i < line.segments.Count; i++)
            {
                DL_DialogueData.Dialogue_Segment segment = line.segments[i];

                yield return WaitForDialogueSegmentSingalToTrigger(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendTxt);

            }
        }

        IEnumerator WaitForDialogueSegmentSingalToTrigger(DL_DialogueData.Dialogue_Segment segment)
        {
            switch(segment.startSingal)
            {
                case DL_DialogueData.Dialogue_Segment.StartSingal.C:
                case DL_DialogueData.Dialogue_Segment.StartSingal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DialogueData.Dialogue_Segment.StartSingal.WA:
                case DL_DialogueData.Dialogue_Segment.StartSingal.WC:
                    yield return new WaitForSeconds(segment.singalDelay);
                    break;
                default:
                    break;
            }
        }

        IEnumerator BuildDialogue(string dailogue, bool Append = false)
        {
            if (!Append)
                TxtArch.Build(dailogue);
            else
                TxtArch.Append(dailogue);

            while (TxtArch.isBuildingText)
            {
                if (UserPromt)
                {
                    if (!TxtArch.speedUP)
                    {
                        TxtArch.speedUP = true;
                    }
                    else
                        TxtArch.forceComplete();
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while (!UserPromt)
            {
                yield return null;
            }
            UserPromt = false;
        }
    }
}
