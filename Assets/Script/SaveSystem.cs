using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(Controller player)
    {
        string path = Path.Combine(Application.persistentDataPath, "playerInfo.pene");
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    public static PlayerData LoadPlayer()
    {
        PlayerData data = null;
        string path = Path.Combine(Application.persistentDataPath, "playerInfo.pene");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
        return data;
    }
}
