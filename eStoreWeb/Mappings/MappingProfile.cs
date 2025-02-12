using AutoMapper;
using eStore.DAL.Models;
using eStore.Web.Areas.Customer.Models;

namespace eStoreAPI.Common
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Map between DAL model and Web model
            CreateMap<CategoryDTO, CustomerModel>().ReverseMap();
        }
    }
}
