using AutoMapper;
using BookStoreAPI.Entities;
using BookStoreAPI.Models;

namespace BookStoreAPI.Profiles;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorCreateRequestDto, Author>();
    }
}