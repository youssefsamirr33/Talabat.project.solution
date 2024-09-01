using AutoMapper;
using E_commerceDashboard.Models.User_View_Models;
using Talabat.Core.Entities.Identity;

namespace E_commerceDashboard.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>();
        }
    }
}
