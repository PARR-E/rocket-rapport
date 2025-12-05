using UnityEngine;
using SQLite;
using System;

[Table("PlayerData")]
public class PlayerData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public float HighScore { get; set; }
    public string DateAchieved { get; set; }

}
