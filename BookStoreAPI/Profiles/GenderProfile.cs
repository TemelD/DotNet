using AutoMapper;
using BookStoreAPI.Entities;
using BookStoreAPI.Models;

namespace BookStoreAPI.Profiles;

public class GenderProfile : Profile
{
    public GenderProfile()
    {
        CreateMap<Gender, GenderDto>();
        CreateMap<GenderCreateRequestDto, Gender>();
    }
}