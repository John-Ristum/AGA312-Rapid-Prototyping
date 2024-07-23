using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Game Data contains all the variables 
/// we wish to store in our save data file
/// </summary>
[Serializable]
public class ThisGameSave
{
    public int score;
    public int highestScore;
    public float health;
    public Vector3 lastCheckpoint;
    public List<int> topScores;
    public bool soundOff;
    public Color colour;

    //Time
    public int hoursPlayed = 0;
    public int minutesPlayed = 0;
    public int secondsPlayed = 0;
    public float totalSeconds = 0;
}

public class SaveManager : SaveData
{
    //Singleton Setup
    public static SaveManager INSTANCE;

    //The game data for this game
    public ThisGameSave save = new ThisGameSave();

    //Time of the last save
    public DateTime timeofLastSave;

    void Awake()
    {
        //Singleton Setup
        if (INSTANCE != null)
        {
            Debug.Log("GameSaveManager already instanced!");
            return;
        }
        INSTANCE = this;

        //Load game data
        save = LoadDataObject<ThisGameSave>();

        //If data does not exist
        if (save == null)
        {
            // Initialize new game data
            save = new ThisGameSave();
            Debug.Log("New Game Save Created");

            // Initialze default data values
            save.score = 0;
            save.highestScore = 0;
            save.health = 100;
            save.soundOff = false;
            save.topScores = new List<int>(10);
            save.colour = Color.white;
        }
        //Set the time of last save to now
        timeofLastSave = DateTime.Now;
    }

    #region Setting Data
    public void SetScore(int _score)
    {
        save.score = _score;
        if (_score > save.highestScore)
            SetHighestScore(_score);
    }
    private void SetHighestScore(int _score)
    {
        save.highestScore = _score;
    }
    public void SetHealth(float _health)
    {
        save.health = _health;
    }
    public void SetLastPosition(Vector3 _lastPos)
    {
        save.lastCheckpoint = _lastPos;
    }
    public void SetTimePlayed()
    {
        ElapsedTime();
    }

    public void SetColour(Color _colour)
    {
        save.colour = _colour;
    }
    #endregion

    #region Getting Data
    public int GetScore()
    {
        return save.score;
    }
    public int GetHighestScore()
    {
        return save.highestScore;
    }
    public float GetHealth()
    {
        return save.health;
    }
    public Vector3 GetLastCheckpoint()
    {
        return save.lastCheckpoint;
    }
    public string GetTimeFormatted()
    {
        ElapsedTime();
        return String.Format("{0:00}:{1:00}:{2:00}",
            save.hoursPlayed, save.minutesPlayed, save.secondsPlayed);
    }

    public Color GetColour()
    {
        return save.colour;
    }
    #endregion

    #region Game Time
    public void ElapsedTime()
    {
        DateTime now = DateTime.Now;
        int seconds = (now - timeofLastSave).Seconds;
        save.totalSeconds += seconds;
        save.hoursPlayed = GetHours(save.totalSeconds);
        save.minutesPlayed = GetMinutes(save.totalSeconds);
        save.secondsPlayed = GetSeconds(save.totalSeconds);
        Debug.Log(save.hoursPlayed + " Hours, " + save.minutesPlayed +
                  " Minutes, " + save.secondsPlayed + " Seconds");
        timeofLastSave = DateTime.Now;
    }
    int GetSeconds(float _seconds)
    {
        return Mathf.FloorToInt(_seconds % 60);
    }
    int GetMinutes(float _seconds)
    {
        return Mathf.FloorToInt(_seconds / 60);
    }
    int GetHours(float _seconds)
    {
        return Mathf.FloorToInt(_seconds / 3600);
    }
    #endregion

    public override void Save()
    {
        ElapsedTime();
        SaveDataObject(save);
    }

    public override void Delete()
    {
        DeleteDataObject();
    }
}
