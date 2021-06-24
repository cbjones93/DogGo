using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly IConfiguration _config;
        public WalksRepository(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walks> GetAllWalksByWalkerId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                SELECT w.id, w.Date, w.Duration, w.walkerId,                w.dogId, o.Name FROM Walks w 
                                  left join dog d on w.DogId = d.Id
                                  join Owner o on o.Id = d.OwnerId
                                  WHERE w.WalkerId = @id
                                  order by o.name;";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Walks> walks = new List<Walks>();

                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = (reader.GetDateTime(reader.GetOrdinal("Date"))).ToShortDateString(),
                            Duration = (reader.GetInt32(reader.GetOrdinal("Duration"))) / 60,
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Client = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            }
                        };
                        walks.Add(walk);
                    }
                    reader.Close();
                    return walks;
                }
            }
        }
        public int GetWalkerTime(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT SUM(w.Duration) as Duration from walks w
                                left join dog d on d.Id = w.DogId
                                join Owner o on o.id = d.OwnerId
                                WHERE w.WalkerId = @id
                                       ";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int totalwalks = 0;

                    while (reader.Read())
                    {
                        if(!reader.IsDBNull(reader.GetOrdinal("Duration")))
                        {
                            totalwalks = (reader.GetInt32(reader.GetOrdinal("Duration"))) / 60;
                        }
                    }
                    return totalwalks;
                }
            }
        }
    }
}
