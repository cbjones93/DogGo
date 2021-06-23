﻿using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;
        public DogRepository(IConfiguration config)
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
        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT ID, [Name], OwnerId, Breed, Notes, ImageUrl FROM Dog";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))

                        };
                        dogs.Add(dog);

                    }
                    reader.Close();
                    return dogs;
                }
            }
        }
        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT ID, [Name], OwnerId, Breed, Notes, ImageUrl FROM Dog
                    WHERE Id = @id";

                    cmd.Parameters.AddWithValue("id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };
                        reader.Close();
                        return dog;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }
    
    public void AddDog(Dog dog)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        INSERT INTO Dog ([Name], OwnerId, Breed, Notes, ImageUrl)
                        OUTPUT Inserted.Id
                        Values (@name, @ownerId, @breed, @notes, @imageUrl); ";

                cmd.Parameters.AddWithValue("@name", dog.Name);
                cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                cmd.Parameters.AddWithValue("@breed", dog.Breed);
                if (dog.Notes != null)
                {
                    cmd.Parameters.AddWithValue("@notes", dog.Notes);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                }
                if (dog.ImageUrl != null)
                {
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@imageUrl", DBNull.Value);
                }
                int id = (int)cmd.ExecuteScalar();

                dog.Id = id;
            }

        }
    }
    public void UpdateDog(Dog dog)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE Dog
                                        SET
                                        [Name] =@name,
                                        OwnerId= @ownerId,
                                        Breed = @breed,
                                        Notes =@notes,
                                        ImageUrl = @imageUrl
                                       WHERE Id = @id";
                cmd.Parameters.AddWithValue("@name", dog.Name);
                cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                cmd.Parameters.AddWithValue("@breed", dog.Breed);
                if (dog.Notes != null)
                {
                    cmd.Parameters.AddWithValue("@notes", dog.Notes);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                }
                if (dog.ImageUrl != null)
                {
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@imageUrl", DBNull.Value);
                }
                    cmd.Parameters.AddWithValue("@id", dog.Id);
                cmd.ExecuteNonQuery();

            }
        }
    }
}
}
