using SibersProject.ApiModels.DTOs.BaseDTOs;
using SibersProject.MainDomain.Models.Abstractions.BaseEntities;
using SibersProject.Services.Mapping.Config;
using SibersProject.Validator;

namespace SibersProject.Services.Mapping.Helpers
{
    public class MapperHelperForDto<T, Tmodel>
         where T : BaseEntity
         where Tmodel : BaseEntityDTO
    {
        /// <summary>
        /// Maps a collection of source entities to a collection of destination DTOs.
        /// </summary>
        /// <param name="sourceCollection">The collection of source entities.</param>
        /// <returns>A collection of destination DTOs.</returns>
        public static IEnumerable<Tmodel> Map(IEnumerable<T> sourceCollection)
        {

            ObjectValidator<IEnumerable<T>>.CheckIsNotNullObject(sourceCollection);
            var mapper = AutoMapperConfig<T, Tmodel>.Initialize();
            return mapper.Map<IEnumerable<Tmodel>>(sourceCollection);
        }
        /// <summary>
        /// Maps a single source entity to a destination DTO.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <returns>The destination DTO.</returns>
        public static Tmodel Map(T source)
        {
            ObjectValidator<T>.CheckIsNotNullObject(source);
            var mapper = AutoMapperConfig<T, Tmodel>.Initialize();
            return mapper.Map<Tmodel>(source);
        }
    }
}
