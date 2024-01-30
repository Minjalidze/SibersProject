using AutoMapper;

namespace SibersProject.Services.Mapping.Config
{
    /// <summary>
    /// Configuration class for AutoMapper to map entities between two models.
    /// </summary>
    /// <typeparam name="Tmodel">Source model type.</typeparam>
    /// <typeparam name="T">Destination model type.</typeparam>
    public class AutoMapperConfig<T, Tmodel>
    {
        /// <summary>
        /// Initializes and configures the AutoMapper with the specified mappings between the source entity type and destination DTO type.
        /// </summary>
        /// <returns>An instance of IMapper.</returns>
        public static IMapper Initialize()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, Tmodel>();
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
