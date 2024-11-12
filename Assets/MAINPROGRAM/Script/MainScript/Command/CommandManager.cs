using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Specialized;

namespace Commands
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager Instance { get; private set; }
        private static Coroutine process = null;
        private static bool isRunningProcess => process != null;
        private CommandDataBase commandDB;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                commandDB = new CommandDataBase();

                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(cmd_DataBaseExtension))).ToArray();

                foreach (Type extension in extensionTypes)
                {
                    MethodInfo extendMethod = extension.GetMethod("Extend");
                    extendMethod.Invoke(null, new object[] { commandDB });
                }
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public Coroutine Executed(string commandName, params string[] args)
        {
            Delegate command = commandDB.GetCommand(commandName);


            if (command == null) // Change this line to check if command is not null
            {
                return null;
            }

            return StartProcess(commandName, command, args);
        }

        private Coroutine StartProcess(string commandName, Delegate command, string[] args)
        {
            StopCurrentProcess();
            process = StartCoroutine(RunningProcess(command, args));
            return process;
        }

        private void StopCurrentProcess()
        {
            if (process != null)
                StopCoroutine(process);
            process = null;
        }

        private IEnumerator RunningProcess(Delegate command, string[] args)
        {
            yield return WaitForProcessToComplete(command, args);

            process = null;
        }

        private IEnumerator WaitForProcessToComplete(Delegate command, string[] args)
        {
            if (command is Action)
                command.DynamicInvoke();

            else if (command is Action<string>)
                command.DynamicInvoke(args[0]);

            else if (command is Action<string[]>)
                command.DynamicInvoke((object)args);

            else if (command is Func<IEnumerator>)
                yield return ((Func<IEnumerator>)command)();

            else if (command is Func<string, IEnumerator>)
                yield return ((Func<string, IEnumerator>)command)(args[0]);

            else if (command is Func<string[], IEnumerator>)
                yield return ((Func<string[], IEnumerator>)command)(args);
        }
    }
}