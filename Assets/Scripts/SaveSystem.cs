using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Handles saving and loading game state.
/// Saves player position and other game data to a JSON file.
/// </summary>
public static class SaveSystem
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "savegame.json");

    [Serializable]
    public class SaveData
    {
        public float playerPositionX;
        public float playerPositionY;
        public float playerPositionZ;
        public bool facingRight;
    }

    /// <summary>
    /// Saves the current game state
    /// </summary>
    public static void SaveGame(Vector3 playerPosition, bool facingRight)
    {
        SaveData data = new SaveData
        {
            playerPositionX = playerPosition.x,
            playerPositionY = playerPosition.y,
            playerPositionZ = playerPosition.z,
            facingRight = facingRight
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"Game saved to {savePath}");
    }

    /// <summary>
    /// Loads the saved game state
    /// </summary>
    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"Game loaded from {savePath}");
            return data;
        }
        else
        {
            Debug.Log("No save file found");
            return null;
        }
    }

    /// <summary>
    /// Checks if a save file exists
    /// </summary>
    public static bool SaveExists()
    {
        return File.Exists(savePath);
    }

    /// <summary>
    /// Deletes the save file (for testing purposes)
    /// </summary>
    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted");
        }
    }
}

