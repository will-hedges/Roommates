using Microsoft.Data.SqlClient;

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

        //public List<Roommate> GetAll()
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"SELECT 
	       //                             rm.Id, 
	       //                             rm.FirstName, 
	       //                             rm.LastName, 
	       //                             rm.RentPortion, 
	       //                             rm.MoveInDate, 
	       //                             rm.RoomId 
        //                                FROM Roommate rm
	       //                                 LEFT JOIN Room ro on rm.RoomId = ro.Id";
                    
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                List<Roommate> roommates = new List<Roommate>();

        //                while (reader.Read())
        //                {
        //                    int idColumnPosition = reader.GetOrdinal("id");
        //                    int idValue = reader.GetInt32(idColumnPosition);

        //                    int firstNameColumnPosition = reader.GetOrdinal("FirstName");
        //                    string firstNameValue = reader.GetString(firstNameColumnPosition);

        //                    int lastNameColumnPosition = reader.GetOrdinal("LastName");
        //                    string lastNameValue = reader.GetString(lastNameColumnPosition);

        //                    int rentPortionColumnPosition = reader.GetOrdinal("RentPortion");
        //                    int rentPortionValue = reader.GetInt32(rentPortionColumnPosition);

        //                    int movedInDateColumnPosition = reader.GetOrdinal("MovedInDate");
        //                    DateTime movedInDateValue = reader.GetDateTime(movedInDateColumnPosition);

        //                    int roomColumnPosition = reader.GetOrdinal("Room");
        //                    Room roomValue = reader.GetValue(roomColumnPosition);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
