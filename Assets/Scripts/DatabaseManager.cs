using UnityEngine;
using SQLite;
using System.IO;
using System.Linq;

//Big thank you to Dr. Zheng for providing this file! :D

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private SQLiteConnection conn;
    private string dbPath;

    //Create the singleton:
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        InitializeDatabase();
    }

    //Determines where the database file is stored:
    private void InitializeDatabase()
    {
        dbPath = Path.Combine(Application.persistentDataPath, "GameData.db");
        //Opens or creates the file. If file doesn't exist, it creates it:
        conn = new SQLiteConnection(dbPath);
        //Creates the table if missing:
        conn.CreateTable<PlayerData>();
        //Path on Windows is C:\Users\JaredCrow\AppData\LocalLow\DefaultCompany\Rocket-Rapport
        //Zheng recommends installing DB Browser for SQLite.
        Debug.Log("Wrote to database at " + dbPath);
    }
    
    //Looks up rows in PlayerData where PlayerName matches, & returns the top match:
    public PlayerData LoadPlayerData(string playerName)
    {
        var player = conn.Table<PlayerData>().Where<PlayerData>(p=>p.PlayerName==playerName).OrderByDescending(p => p.HighScore).FirstOrDefault();
        //if (player != null)
        //{
        //    Debug.Log($"Loaded Player: {player.PlayerName} (High Score {player.HighScore}).");
        //}
        //else
        //{
        //    Debug.Log("No player data found.");
        //    SavePlayerData("Player1", 10000);
        //}
        return player;
    }

    //loads the highest score among ALL players:
    public PlayerData LoadPlayerData()
    {
        var player = conn.Table<PlayerData>().OrderByDescending(p => p.HighScore).FirstOrDefault();
        return player;
    }

    //If the player already exists, update the record if the new score is higher.
    //Else, Creates a new row & insert it into the database.
    public void SavePlayerData(string name, float highScore)
    {
        PlayerData player = LoadPlayerData(name);
        if (player != null)
        {
            if (highScore > player.HighScore)
            {
                player.HighScore = highScore;
                player.DateAchieved = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UpdatePlayer(player);
                Debug.Log("Saved new high score " + highScore + " AU for player " + name + "!");
            }
            return;
        }
        else
        {
            player = new PlayerData
            {
                PlayerName = name,
                HighScore = highScore,
                DateAchieved = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            Debug.Log("Saved new high score " + highScore + " AU to NEW player " + name + "!");
            conn.Insert(player);
        }
       
    }

    //Refresh/update the PlayerData instance:
    public void UpdatePlayer(PlayerData player)
    {
        conn.Update(player);
        //Debug.Log($"Updated Player: {player.PlayerName} (High Score {player.HighScore}).");
    }

    
}
