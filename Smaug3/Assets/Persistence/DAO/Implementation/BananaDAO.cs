using Assets.Scripts.Persistence.DAO.Specification;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.TextCore.Text;
using System;

namespace Assets.Scripts.Persistence.DAO.Implementation
{
    public class BananaDAO : IBananaDAO
    {
        public BananaDAO(ISQliteConnectionProvider connectionProvider) => ConnectionProvider = connectionProvider;

        public ISQliteConnectionProvider ConnectionProvider { get; protected set; }
        public bool DeleteBanana(int id)
        {

            using (var connection = ConnectionProvider.Connection)
            {
                var commandText = "DELETE FROM Banana WHERE Id = @id;";
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

        public BananaModel GetBanana(int id)
        {
            var commandText = "SELECT * FROM Banana where Id = @id;";
            BananaModel banana = null;

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
                        banana = new BananaModel();
                        banana.Id = reader.GetInt32(0);
                        banana.Damage = reader.GetInt32(1);
                        banana.Name = reader.GetString(2);
                        banana.EnergyCost = reader.GetInt32(3);
                        banana.MoveSpeed = reader.GetFloat(4);
                        Debug.Log("\tid:" + reader["Id"] + "dano:" + reader["Damage"] + "\tnome:" + reader["Name"] + "\tenergia:" + reader["EnergyCost"] + "\tvelocidade:" + reader["MoveSpeed"]);
                        
                    }
                }
                return banana;
            }
        }

        public bool SetBanana(BananaModel banana)

        {
            var commandText = "INSERT INTO Banana (Id, Damage, Name, EnergyCost, MoveSpeed) VALUES (@id, @damage, @name, @energyCost, @moveSpeed);";

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;

                    command.Parameters.AddWithValue("@id", banana.Id);
                    command.Parameters.AddWithValue("@damage", banana.Damage);
                    command.Parameters.AddWithValue("@name", banana.Name);
                    command.Parameters.AddWithValue("@energyCost", banana.EnergyCost);
                    command.Parameters.AddWithValue("@moveSpeed", banana.MoveSpeed);
                    return command.ExecuteNoQueryWithFK() > 0;

                }
            }
        }



        public bool UpdateBanana(BananaModel banana)
        {
            var commandText = "UPDATE Banana SET " +
             "Damage = @damage," +
             "Name = @name," +
             "EnergyCost = @energyCost," +
             "MoveSpeed = @moveSpeed," +
             "WHERE Id=@id;";

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {


                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", banana.Id);
                    command.Parameters.AddWithValue("@damage", banana.Damage);
                    command.Parameters.AddWithValue("@name", banana.Name);
                    command.Parameters.AddWithValue("@energyCost", banana.EnergyCost);
                    command.Parameters.AddWithValue("@moveSpeed", banana.MoveSpeed);
                    return command.ExecuteNoQueryWithFK() > 0;
                    //Debug.Log("UPDATE Banana"); 
                }
            }
        }

        BananaModel IBananaDAO.GetBanana(int id)
        {
            throw new NotImplementedException();
        }
    }
}