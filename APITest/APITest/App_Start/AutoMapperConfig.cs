using APITest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            //AutoMapper.Mapper.CreateMap<Book, BookViewModel>()
              //  .ForMember(dest => dest.Author,
                //           opts => opts.MapFrom(src => src.Author.Name));
            AutoMapper.Mapper.CreateMap<Sucursal, SucursalDTO>();
            AutoMapper.Mapper.CreateMap<Empresa, EmpresaDTO>();
        }
    }
}