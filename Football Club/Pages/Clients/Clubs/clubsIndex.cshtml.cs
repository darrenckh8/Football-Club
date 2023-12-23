using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Football_Club.Pages.Clubs
{
    public class ClubCreateModel : PageModel
    {
        private readonly string _connectionString = "Data Source=DARREN-TOWER;Initial Catalog=football;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public List<IndexClubInfo> ClubList { get; set; }

        public ClubCreateModel()
        {
            ClubList = new List<IndexClubInfo>();
        }

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Clubs";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IndexClubInfo clubInfo = new IndexClubInfo
                                {
                                    ClubID = reader.GetInt32(0),
                                    ClubName = reader.GetString(1),
                                    Location = reader.GetString(2)
                                };

                                ClubList.Add(clubInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or display an error message to the user
                Console.WriteLine("Exception:" + ex.ToString());
            }
        }
    }

    public class IndexClubInfo
    {
        public int ClubID { get; set; }
        public string ClubName { get; set; }
        public string Location { get; set; }
    }
}
