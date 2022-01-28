namespace Core.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public bool IsDeleted { get; set; }
    }
}
