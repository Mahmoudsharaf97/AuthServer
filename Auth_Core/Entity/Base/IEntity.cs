using System.ComponentModel.DataAnnotations.Schema;

namespace Auth_Core.Entities
{
    public interface IEntity
    {
        [Column(Order = 0)]
        long Id { get; set; }
    }
    public interface IEntity<TKey>
    {
        [Column(Order = 0)]
        TKey Id { get; set; }
    }
}
