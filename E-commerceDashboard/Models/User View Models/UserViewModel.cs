using System.ComponentModel.DataAnnotations;

namespace E_commerceDashboard.Models.User_View_Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Phone]
        public string PhoneNumber { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = null!;
    }
}
