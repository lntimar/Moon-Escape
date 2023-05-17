using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public static class MySQliteCommandExt
{
    public static int ExecuteNoQueryWithFK(this SqliteCommand command)
    {
        var tmp = command.CommandText;
        command.CommandText = $"PRAGMA foreign Keys = true;{tmp}";
        return command.ExecuteNonQuery();
    }

}