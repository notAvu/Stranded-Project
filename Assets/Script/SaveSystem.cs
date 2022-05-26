using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(Controller player)
    {
        string path = Path.Combine(Application.persistentDataPath, "playerInfo.basado");
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static bool FileExists()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerInfo.basado");
        return File.Exists(path);
    }
    public static void DeleteFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerInfo.basado");
        try
        {
            File.Delete(path);
        }
        catch
        {
            Debug.Log("No se ha podido borrar el archivo");
        }
    }
    public static PlayerData LoadPlayer()
    {
        PlayerData data = null;
        string path = Path.Combine(Application.persistentDataPath, "playerInfo.basado");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        return data;
    }
}
