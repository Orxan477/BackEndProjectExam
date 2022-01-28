using Microsoft.AspNetCore.Http;

namespace BackEndProject.ViewModels.Team
{
    public class UpdateTeamVM
    {
        public int Id { get; set; }
        public IFormFile Photo { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
