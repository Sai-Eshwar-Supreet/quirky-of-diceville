using UnityEngine;
using System.IO;

public static class IOHelper
{
    public static string GetFileData(string baseURL, string route)
    {
        var path = Path.Combine(baseURL, route);
        try
        {
            if(!File.Exists(path)) return null;

            return File.ReadAllText(path);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to read file at {path}\n{ex}");
            return null;
        }
    }

    public static void SetFileData(string baseURL, string route, string data)
    {
        var path = Path.Combine(baseURL, route);
        try
        {
            string directory = Path.GetDirectoryName(path);

            if(!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);

            File.WriteAllText(path, data);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to write file at {path}\n{ex}");
        }
    }

    public static void ClearDirectory(string baseURL, string dir)
    {
        var path = Path.Combine(baseURL, dir);
        try
        {
            if(Directory.Exists(path)) Directory.Delete(path, true);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to clear directory at {path}\n{ex}");
        }
    }
}
