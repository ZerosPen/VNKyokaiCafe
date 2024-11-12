using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Commands
{
    public class CommandDataBase
    {
        private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

        public bool hasCommand(string commandName) => database.ContainsKey(commandName);

        public void addCommand(string commandName, Delegate command)
        {
            if (!database.ContainsKey(commandName))
            {
                database.Add(commandName, command);
            }
            else
                Debug.LogError($"Command Already exists in the database '{commandName}'"); // Corrected spelling

        }
        public Delegate GetCommand(string commandName)
        {
            if (!database.ContainsKey(commandName))
            {
                Debug.LogError($"Command '{commandName}' does not exist in the DataBaseCommand"); // Corrected spelling
                return null;
            }
            return database[commandName];
        }
    }
}