using AutoMapper;
using ProductShop.DTO.Product;
using ProductShop.DTO.Users;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Problem - 02
            this.CreateMap<UserImportDTO, User>();

            //Problem - 06
            this.CreateMap<Product, ListProductsInRangeDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x.Seller.FirstName + " " + x.Seller.LastName));

            //Problem - 07
            this.CreateMap<Product, BuyerWithProductsDTO>()
                .ForMember(x => x.BuyerFirstName, y => y.MapFrom(x => x.Buyer.FirstName))
                .ForMember(x => x.BuyerLastName, y => y.MapFrom(x => x.Buyer.LastName));

            this.CreateMap<User, UserWithSoldProductsDTO>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(p => p.ProductsSold.Where(x => x.Buyer != null)));
        }
    }
}
