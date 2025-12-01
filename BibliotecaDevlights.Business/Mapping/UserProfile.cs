using AutoMapper;
using BibliotecaDevlights.Business.DTOs.User;
using BibliotecaDevlights.Data.Entities;

namespace BibliotecaDevlights.Business.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
