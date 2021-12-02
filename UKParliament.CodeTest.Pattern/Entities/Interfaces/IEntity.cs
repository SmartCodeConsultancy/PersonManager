#region Namespace References
using System.ComponentModel.DataAnnotations;
#endregion#

namespace UKParliament.CodeTest.Pattern.Entities.Interfaces
{
    public interface IEntity<TId>
    {
        [Key]
        TId Id { get; set; }
    }
}
