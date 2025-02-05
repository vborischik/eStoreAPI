using AutoMapper;
using eStore.DAL.Models;
using eStoreAPI.Areas.Models;

namespace eStore.Common
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Map between DAL model and Web model
            CreateMap<CustomerDTO, CustomerModel>().ReverseMap();
        }
    }
}
