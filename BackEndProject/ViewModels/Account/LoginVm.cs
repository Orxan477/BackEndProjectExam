using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels.Account
{
    public class LoginVm
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
