using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.Common;
using Mono.Data.Sqlite;
using System.Drawing;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager Instance { get; private set; }
    private string dbUri = "URI=file:mydb.sqlite";
    private string SQL_COUNT_ELEMNTS = "SELECT count(*) FROM Posiciones;";
    private string SQL_CREATE_POSICIONES = "CREATE TABLE IF NOT EXISTS Posiciones ("+
                                            "Name STRING NOT NULL , "+
                                            "Timestamp REAL, "+
                                            "PosicionX REAL, " +
                                            "PosicionY REAL, " +
                                            "Posicionz REAL);";
    private IDbConnection dbConnection;
    private string[] NAMES = { "Andrea", "Veronica", "Lucia", "Carmen" };
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        OpenDatabase();
        InitializeDB();
    }

    private void OpenDatabase()
    {
        dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "PRAGMA foreign_keys = ON";
        dbCommand.ExecuteNonQuery();
    }

    private void InitializeDB()
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE_POSICIONES;
        dbCmd.ExecuteReader();
    }

    public void SavePosition(CharacterPosition position)
    {
        float positionX = position.position.x;
        float positionY = position.position.y;
        float positionZ = position.position.z;
        string command = "INSERT INTO Posiciones (Name, PosicionX, PosicionY, PosicionZ) VALUES ";
        IDbCommand dbCommand = dbConnection.CreateCommand();
        System.Random rnd = new System.Random();
      
       
        string nombre = NAMES[rnd.Next(NAMES.Length)];
        command += $"('{nombre}','{positionX}', '{positionY}','{positionZ}'),";
        
        command = command.Remove(command.Length - 1, 1);
        command += ";";
        dbCommand.CommandText = command;
        dbCommand.ExecuteNonQuery();
    }

    private void OnDestroy()
    {
        dbConnection.Close();
    }
}
