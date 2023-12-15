using AutoMapper;
using BookStoreAPI.Entities;
using BookStoreAPI.Models;

namespace BookStoreAPI.Profiles;

public class EditorProfile : Profile
{
    public EditorProfile()
    {
        CreateMap<Editor, EditorDto>();
        CreateMap<EditorCreateRequestDto, Editor>();
    }
}