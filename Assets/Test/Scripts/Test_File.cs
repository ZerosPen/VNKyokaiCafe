using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_File : MonoBehaviour
{
    [SerializeField] private TextAsset fileName;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        List<string> lines = FileManager.readTxtAsset(fileName, false);

        foreach (string line in lines)
        {
            Debug.Log(line);
        }

        yield return null;
    }
}
