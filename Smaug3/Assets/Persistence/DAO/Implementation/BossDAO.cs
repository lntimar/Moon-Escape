using Assets.Scripts.Persistence.DAO.Specification;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Persistence.DAO.Implementation
{
    public class BossDAO : IBossDAO
    {
        public ISQliteConnectionProvider ConnectionProvider { get; set; }
        public BossDAO(ISQliteConnectionProvider connectionProvider) => ConnectionProvider = connectionProvider;


        public bool DeleteBoss(int id)
        {
            using (var connection = ConnectionProvider.Connection)
            {
                var commandText = "DELETE FROM Boss WHERE Id = @id;";
                connection.Open();

                using (var command = connection.CreateCommand())
                {


                    {
                        command.CommandText = commandText;
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNoQueryWithFK() > 0;


                    }


                }
            }
        }

        public BossModel GetBoss(int id)
        {

            var commandText = "SELECT * FROM Boss where Id = @id;";
            BossModel boss = null;

            //using (IDataReader reader = command.ExecuteReader())

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())

                {
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", id);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        boss = new BossModel();
                        boss.Id = reader.GetInt32(0);
                        boss.MaxHealth = reader.GetInt32(1);
                        boss.Damage = reader.GetInt32(2);
                        boss.Name = reader.GetString(3);
                        boss.MoveSpeed = reader.GetFloat(4);

                        if (boss.Id != 1 && boss.Id != 5)
                        {
                            boss.BananaId = reader.GetInt32(5);
                            Debug.Log("\tid:" + reader["Id"] + "\tvida:" + reader["MaxHealth"] + "dano:" + reader["Damage"] + "\tnome:" + reader["Name"] + "\tvelocidade:" + reader["MoveSpeed"] + "\tbananaid:" + reader["BananaId"]);
                        }
                        else
                        {
                            Debug.Log("\tid:" + reader["Id"] + "\tvida:" + reader["MaxHealth"] + "dano:" + reader["Damage"] + "\tnome:" + reader["Name"] + "\tvelocidade:" + reader["MoveSpeed"] + "\tbananaid: NULL");
                        }
                    }
                }
                return boss;
            }
        }

        public bool SetBoss(BossModel boss)
        {

            var commandText = "PRAGMA foreign_keys=true;" + "INSERT INTO Boss  (Id , MaxHealth, Damage, Name, MoveSpeed, BananaId) VALUES(@id, @maxHealth, @damage, @name, @moveSpeed, @bananaId); Select cast(Scope_identy) AS int";

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", boss.Id);
                    command.Parameters.AddWithValue("@maxHealth", boss.MaxHealth);
                    command.Parameters.AddWithValue("@damage", boss.Damage);
                    command.Parameters.AddWithValue("@name", boss.Name);
                    command.Parameters.AddWithValue("@moveSpeed", boss.MoveSpeed);
                    command.Parameters.AddWithValue("@bananaId", boss.BananaId);
                    return command.ExecuteNoQueryWithFK() > 0;
                    //boss.Id = (int)command.ExecuteScalar();
                }
            }
        }


        public bool UpdateBoss(BossModel boss)
        {
            var commandText = "UPDATE Boss  SET " +
             "MaxHealth = @maxHealth," +
             "Damage = @damage," +
             "Name = @name," +
             "MoveSpeed = @moveSpeed," +
             "BananaId = @bananaId" +
             "WHERE Id=@id;";

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", boss.Id);
                    command.Parameters.AddWithValue("@maxHealth", boss.MaxHealth);
                    command.Parameters.AddWithValue("@damage", boss.Damage);
                    command.Parameters.AddWithValue("@name", boss.Name);
                    command.Parameters.AddWithValue("@moveSpeed", boss.MoveSpeed);
                    return command.ExecuteNoQueryWithFK() > 0;
                }
            }
        }

        BananaModel IBossDAO.GetBoss(int id)
        {
            throw new NotImplementedException();
        }
    }
}