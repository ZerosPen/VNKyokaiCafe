using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager
{
    public static List<string> readTxtFiles(string filepath, bool includeBlankLines = true)
    {
        if (!filepath.StartsWith('/'))
            filepath = FilePaths.root + filepath;

        List<string> lines = new List<string>();
        try
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (includeBlankLines || !string.IsNullOrEmpty(line))
                        lines.Add(line);
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"File not found: '{ex.FileName}'");
        }

        return lines;
    }

    public static List<string> readTxtAsset(string filepath, bool includeBlankLines = true)
    {
        TextAsset asset = Resources.Load<TextAsset>(filepath);
        if (asset == null)
        {
            Debug.LogError($"Asset Not Found: '{filepath}'");
            return null;
        }


        return readTxtAsset(asset, includeBlankLines);
    }

    public static List<string> readTxtAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(asset.text))
        {
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                if (includeBlankLines || !string.IsNullOrEmpty(line))
                    lines.Add(line);
            }
        }
        return lines;
    }
}
