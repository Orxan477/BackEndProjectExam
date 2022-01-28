using AutoMapper;
using BackEndProject.ViewModels.Team;
using Core.Models;

namespace BackEndProject.Profiles
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Team, UpdateTeamVM>();
        }
    }
}
