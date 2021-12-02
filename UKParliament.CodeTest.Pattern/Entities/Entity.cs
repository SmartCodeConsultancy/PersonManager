#region Namespace References
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using UKParliament.CodeTest.Pattern.Entities.Interfaces;
#endregion

namespace UKParliament.CodeTest.Pattern.Entities
{
    public abstract class Entity<TId> : IEntity<TId>
    {
        /// <summary>
        /// Primary Key Field
        /// </summary>
        [Key]
        [IgnoreDataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TId Id { get; set; }
    }
}
