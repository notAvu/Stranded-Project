using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// This class creates save files to store the state of the game and manages access to said files
/// </summary>
public static class SaveSystem
{
    private static string path = Path.Combine(Application.persistentDataPath, "playerInfo.basado");
    public static bool FileExists => File.Exists(path);
    /// <summary>
    /// Given a player object, this method will load the player's position and health and store
    /// them in a PlayerData object which will be written in a binary file.
    /// </summary>
    /// <param name="player"></param>
    public static void SavePlayer(Controller player)
    {
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    /// <summary>
    /// Tries to delete the save file from the "path" attribute if it exists
    /// </summary>
    public static void DeleteFile()
    {
        try
        {
            if (FileExists)
                File.Delete(path);
        }
        catch
        {
            Debug.Log("No se ha podido borrar el archivo");
        }
    }
    /// <summary>
    /// Reads the save file from path if it exists
    /// </summary>
    /// <returns>data: an object containg the info to load the player's saved state</returns>
    public static PlayerData LoadPlayer()
    {
        PlayerData data = null;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
        }
        return data;
    }
}
