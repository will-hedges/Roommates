using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{
    internal class ChoreRepository : BaseRepository
    {
        /// <summary>
        ///  When new ChoreRepository is instantiated
        ///     pass the connection string along to the BaseRepository
        /// </summary>
        public ChoreRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        ///  Get a list of all Chores in the database
        /// </summary>
        public List<Chore> GetAll()
        {
            // open the db connection
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // build up the command
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    // execute the cmd, get the reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // hold all the chores in a list
                        List<Chore> chores = new List<Chore>();
                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);
                            
                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            // create a new chore obj using the data
                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            // add the chore to our list
                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }
        
        public List<Chore> GetUnnasignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = @"SELECT ch.Id, ch.Name FROM Chore ch
                                        FULL OUTER JOIN RoommateChore rc ON rc.ChoreId = ch.Id
                                        WHERE RoommateId IS NULL";
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();
                        while (reader.Read())
                        {
                            // add the resultant objs to the list
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }
        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;
                        // no while loop, since we are only expecting 1 row
                        if (reader.Read())
                        {
                            chore = new Chore 
                            { 
                                Id = id, 
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                        }
                        return chore;
                    }
                    
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name) 
                                        OUTPUT INSERTED.Id 
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }
    }
}
