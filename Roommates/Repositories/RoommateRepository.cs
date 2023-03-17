using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

using Roommates.Models;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }
        public Roommate GetById(int id)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                    cmd.CommandText = @"SELECT rm.Id, rm.FirstName, rm.RentPortion, ro.Name 
                                        FROM Roommate rm 
                                        LEFT JOIN Room ro on ro.Id = rm.RoomId
                                        WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;
                        // only anticipating one row to match
                        //  so no while loop
                        if (reader.Read())
                        {
                            roommate = new Roommate()
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                // 'Room' prop on Roommate is actually a Room type
                                //  so have to create a new room obj to attach the room name
                                Room = new Room
                                {
                                    Name = reader.GetString(reader.GetOrdinal(("Name"))),
                                }
                            };
                        }
                        return roommate;
                        }
                    }
                }
            }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select Id, FirstName FROM Roommate";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("FirstName");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Roommate roommate = new Roommate
                            {
                                Id = idValue,
                                FirstName = nameValue,
                            };
                            roommates.Add(roommate);
                        }
                        return roommates;
                    }
                }
            }
        }
    }
}
