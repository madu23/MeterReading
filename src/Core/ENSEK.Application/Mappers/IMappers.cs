using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Application.Mappers
{
    public interface IMapFromEntityToVm<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
    }

    public interface IMapFromVmToEntity<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T)).ReverseMap();
    }
}
