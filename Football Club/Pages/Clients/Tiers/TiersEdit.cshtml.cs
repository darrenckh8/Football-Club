using Football_Club.Pages.Clubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace Football_Club.Pages.Clients.Tiers
{
    public class TierEditModel : PageModel
    {
        private readonly string _connectionString = "Data Source=DARREN-TOWER;Initial Catalog=football;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        [BindProperty]
        public EditTierInfo TierInfo { get; set; }

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
                    string sql = "SELECT * FROM MembershipTiers WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TierInfo = new EditTierInfo
                                {
                                    ID = reader.GetInt32(0),
                                    TierName = reader.GetString(1),
                                    Description = reader.GetString(2)
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

            if (TierInfo == null)
            {
                return NotFound();
            }
            // ... (existing OnGet code)

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(TierInfo.TierName) || string.IsNullOrEmpty(TierInfo.Description))
            {
                ErrorMessage = "Tier Name and Description are required fields.";
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE MembershipTiers SET TierName=@name, Description=@description WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", TierInfo.TierName);
                        command.Parameters.AddWithValue("@description", TierInfo.Description);
                        command.Parameters.AddWithValue("@id", TierInfo.ID);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            SuccessMessage = "Tier updated successfully";
                            return RedirectToPage("/Clients/Tiers/TiersIndex");
                        }
                        else
                        {
                            ErrorMessage = "Failed to update tier";
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

    public class EditTierInfo
    {
        public int ID { get; set; }

        [BindProperty]
        public string TierName { get; set; }

        [BindProperty]
        public string Description { get; set; }
    }
}
