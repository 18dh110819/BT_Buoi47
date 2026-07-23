// using AutoMapper;
// using backend.Models.DbUser;
// using backend_netcore_dotnet06.Helper;

// namespace backend.Helper;

// public class MappingProfile : Profile
// {
//     public MappingProfile()
//     {
//         CreateMap<ProductInsertDto, Product>()
//             .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => HelperFunction.StringToSlug(src.Name)));
//             // .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow)); // Mapping từ ProductInsertDto sang Product
//         // CreateMap<Product, ProductInsertDto>(); // Mapping từ Product sang ProductDto

//         // Mapping từ ProductUpdateDto sang Product
//         CreateMap<ProductUpdateDto, Product>()
//             .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => HelperFunction.StringToSlug(src.Name)));
        
//         CreateMap<UserDto, User>();
//     }
// }