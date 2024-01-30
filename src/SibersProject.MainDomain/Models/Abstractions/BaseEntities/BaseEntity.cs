using System.Text.Json.Serialization;

namespace SibersProject.MainDomain.Models.Abstractions.BaseEntities
{
    /// <summary>
    /// Base entity class with a unique identifier.
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
