using Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTseting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CommandManager.Instance.Executed("MoveCharacterDemo", "left");
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            CommandManager.Instance.Executed("MoveCharacterDemo", "right");
    }

    IEnumerator Running()
    {
        yield return CommandManager.Instance.Executed("print");
        yield return CommandManager.Instance.Executed("print_1p", "Hello Worlds!");
        yield return CommandManager.Instance.Executed("print_mp", "Lines1", "Lines2", "Lines3");

        yield return CommandManager.Instance.Executed("lambda");
        yield return CommandManager.Instance.Executed("lambda_1p", "Hello Worlds!");
        yield return CommandManager.Instance.Executed("lambda_mp", "lambda Lines1", "lambda Lines2", "lambda Lines3");

        yield return CommandManager.Instance.Executed("process");
        yield return CommandManager.Instance.Executed("process_1p", "3");
        yield return CommandManager.Instance.Executed("process_mp", "process Lines1", "process Lines2", "process Lines3");
    }
}
