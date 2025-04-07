using AutoMapper;
using eStore.DAL.Models;
using eStore.Web.Areas.Customer.Models;
using eStore.Web.Areas.Order.Models;
using eStore.Web.Areas.Product.Models;

namespace eStoreAPI.Common
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Map between DAL model and Web model
            CreateMap<CustomerDTO, CustomerModel>().ReverseMap();
            CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
            CreateMap<ProductDTO, ProductModel>().ReverseMap();
            CreateMap<OrderDetailDTO, OrderDetailModel>().ReverseMap();
            CreateMap<OrderDTO, OrderModel>().ReverseMap();

        }
    }
}
