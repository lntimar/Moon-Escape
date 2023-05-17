using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.Networking;
using System;
using Assets.Scripts.Persistence.DAO.Specification;


public class SQLiteDataSource : MonoBehaviour, ISQliteConnectionProvider
{
    [SerializeField]
    protected string databaseName;
    protected string databasePath;
    public string DatabaseName => this.databaseName;

    public SqliteConnection Connection => new SqliteConnection($"Data Source = {this.databasePath};");

    [SerializeField]
    protected bool copyDatabase;

    protected void Awake()
    {
        print("SQLiteDataSource Awake");
        if (string.IsNullOrEmpty(this.databaseName))
        {
            Debug.LogError("Database name is empty!");
            return;
        }

        // if (this.copyDatabase)
        CopyDatabaseFileIfNotExists();
        //else
        CreateDatabaseFileIfNotExists();



    }

    #region Create database

    protected void CopyDatabaseFileIfNotExists()
    {
        this.databasePath = Path.Combine(Application.persistentDataPath, this.databaseName);
        Debug.Log("PATH: " + this.databasePath);


        if (File.Exists(this.databasePath))
            return;

        var originDatabasePath = string.Empty;
        var isAndroid = false;

#if UNITY_EDITOR || UNITY_WP8 || UNITY_WINRT || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX

        originDatabasePath = Path.Combine(Application.streamingAssetsPath, this.databaseName);

#elif UNITY_STANDALONE_OSX

        originDatabasePath = Path.Combine(Application.dataPath, "/Resources/Data/StreamingAssets/", this.DatabaseName);
        
#elif UNITY_IOS

        originDatabasePath = Path.Combine(Application.dataPath, "Raw", this.DatabaseName);        

#elif UNITY_ANDROID

        isAndroid = true;
        originDatabasePath = "jar:file://" + Application.dataPath + "!/assets/" + this.DatabaseName;
        StartCoroutine(GetInternalFileAndroid(originDatabasePath));

#endif

        if (!isAndroid)
        {
            Debug.Log($"COPY FILE: {originDatabasePath} to {this.databasePath}");
            File.Copy(originDatabasePath, this.databasePath);
        }
    }


    protected void CreateDatabaseFileIfNotExists()
    {
        this.databasePath = Path.Combine(Application.persistentDataPath, this.databaseName);

        if (!File.Exists(this.databasePath))
        {
            SqliteConnection.CreateFile(this.databasePath);
            Debug.Log($"Database path: {this.databasePath}");
        }
    }

    protected IEnumerator GetInternalFileAndroid(string path)
    {
        var request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)  // comando obsoleto mas o de baixo dá erro
                                                            //  if (UnityWebRequest.result == UnityWebRequest.Result.ConnectionError)  
        {
            Debug.LogError($"Error reading android file!: {request.error}");
            throw new Exception($"Error reading android file!: {request.error}");
        }
        else
        {
            File.WriteAllBytes(this.databasePath, request.downloadHandler.data);
            Debug.Log("File copied! ->" + this.databasePath);
        }
    }

    #endregion
}
