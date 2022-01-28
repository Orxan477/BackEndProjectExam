using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels.Account
{
    public class RegisterVM
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
