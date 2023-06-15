using AutoMapper;
using System.Collections.Generic;

namespace Common
{
    public static class ConvertModels<T, U>
    {
        public static List<T> convertModelsLists(IEnumerable<U> UGenericList)
        {
            List<T> TGenericList = new List<T>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<U, T>();
            });

            IMapper mapper = config.CreateMapper();
            TGenericList = mapper.Map<IEnumerable<U>, List<T>>(UGenericList);
            return TGenericList;
        }
        public static T convertModels(U UGeneric)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<U, T>();
            });

            IMapper mapper = config.CreateMapper();
            T TGeneric = mapper.Map<U, T>(UGeneric);
            return TGeneric;
        }

    }
}
