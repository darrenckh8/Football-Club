using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace Football_Club.Pages.Clubs
{
    public class ClubsModel : PageModel
    {
        public ClubInfo clubInfo = new ClubInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            clubInfo.clubName = Request.Form["clubName"];
            clubInfo.location = Request.Form["location"];

            if (clubInfo.clubName.Length == 0 || clubInfo.location.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                string connectionString = "Data Source=DARREN-TOWER;Initial Catalog=football;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Clubs (ClubName, Location) VALUES (@clubName, @location)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@clubName", clubInfo.clubName);
                        command.Parameters.AddWithValue("@location", clubInfo.location);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clubInfo.clubName = "";
            clubInfo.location = "";

            successMessage = "New club added successfully";
            Response.Redirect("/Clients/Clubs/clubsIndex");
        }
    }

    public class ClubInfo
    {
        public string clubName { get; set; }
        public string location { get; set; }
    }
}
