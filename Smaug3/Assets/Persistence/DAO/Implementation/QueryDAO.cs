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
    public class QueryDAO : IQueryDAO
    {
        public QueryDAO()
        {
        }

        public QueryDAO(ISQliteConnectionProvider connectionProvider) => ConnectionProvider = connectionProvider;

        public ISQliteConnectionProvider ConnectionProvider { get; protected set; }

        public void BananaFULLBoss()
        {
            throw new NotImplementedException();
        }

        public void BananaIdBoss(int id)
        {
            throw new NotImplementedException();
        }

        public void BananaINNERBoss()
        {
            var commandText = "SELECT * FROM Banana INNER JOIN Boss on Banana.Id=Boss.BananaId";
            //BananaModel banana = null;

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())

                {
                    command.CommandText = commandText;
                    // command.Parameters.AddWithValue("@id", id);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        /*banana  = new BananaModel();
                        banana.Id = reader.GetInt32(0);
                        banana.Damage = reader.GetInt32(1);
                        banana.Name = reader.GetString(2);
                        banana.EnergyCost = reader.GetInt32(3);
                        banana.MoveSpeed = reader.GetFloat(4);
                        Debug.Log("\tid:" + reader["Id"] + "dano:" + reader["Damage"] + "\tnome:" + reader["Name"] + "\tenergia:" + reader["EnergyCost"] + "\tvelocidade:" + reader["MoveSpeed"]); 
                        */

                    }
                    reader.Close();
                }
                connection.Close();
            }
        }

        public void BananaLEFTBoss()
        {
            var commandText = "SELECT * FROM Banana LEFT JOIN Boss on Banana.Id=Boss.BananaId";
            //BananaModel banana = null;

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())

                {
                    command.CommandText = commandText;
                    // command.Parameters.AddWithValue("@id", id);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        /*banana  = new BananaModel();
                        banana.Id = reader.GetInt32(0);
                        banana.Damage = reader.GetInt32(1);
                        banana.Name = reader.GetString(2);
                        banana.EnergyCost = reader.GetInt32(3);
                        banana.MoveSpeed = reader.GetFloat(4);
                        Debug.Log("\tid:" + reader["Id"] + "dano:" + reader["Damage"] + "\tnome:" + reader["Name"] + "\tenergia:" + reader["EnergyCost"] + "\tvelocidade:" + reader["MoveSpeed"]); 
                        */
                    }
                    reader.Close();
                }
                connection.Close();
            }
        }


    }
}