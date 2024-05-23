using AutoMapper;
using PetAdoption.Api.Data;
using PetAdoption.Shared;
using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<PetCreateDto, Pet>();
            CreateMap<PetUpdateDto, Pet>();
            CreateMap<UserDto, User>();
            CreateMap<Pet, PetListDto>()
                .ForMember(p => p.Image,
                opt => opt.MapFrom(src => $"{AppConstants.BaseURL}/images/pets/{src.Image}"));

            CreateMap<Pet, PetDetailDto>()
                .ForMember(p => p.Image,
                opt => opt.MapFrom(src => $"{AppConstants.BaseURL}/images/pets/{src.Image}"));

            CreateMap<User, UserDto>()
                .ForMember(u => u.ProfilePicture,
                opt => opt.MapFrom(src => $"{AppConstants.BaseURL}/images/avatars/{src.ProfilePicture}"));

            CreateMap<Message, MessagesDto>();
        }
    }
}
