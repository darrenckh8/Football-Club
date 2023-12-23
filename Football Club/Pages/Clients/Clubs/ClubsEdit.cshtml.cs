using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace Football_Club.Pages.Clubs
{
    public class ClubEditModel : PageModel
    {
        private readonly string _connectionString = "Data Source=DARREN-TOWER;Initial Catalog=football;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        [BindProperty]
        public EditClubInfo ClubInfo { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Clubs WHERE ClubID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ClubInfo = new EditClubInfo
                                {
                                    ClubID = reader.GetInt32(0),
                                    ClubName = reader.GetString(1),
                                    Location = reader.GetString(2)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            if (ClubInfo == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(ClubInfo.ClubName) || string.IsNullOrEmpty(ClubInfo.Location))
            {
                ErrorMessage = "Club Name and Location are required fields.";
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Clubs SET ClubName=@name, Location=@location WHERE ClubID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", ClubInfo.ClubName);
                        command.Parameters.AddWithValue("@location", ClubInfo.Location);
                        command.Parameters.AddWithValue("@id", ClubInfo.ClubID);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            SuccessMessage = "Club updated successfully";
                            return RedirectToPage("/Clients/Clubs/clubsIndex");
                        }
                        else
                        {
                            ErrorMessage = "Failed to update club";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }

    public class EditClubInfo
    {
        public int ClubID { get; set; }

        [BindProperty]
        public string ClubName { get; set; }

        [BindProperty]
        public string Location { get; set; }
    }
}
