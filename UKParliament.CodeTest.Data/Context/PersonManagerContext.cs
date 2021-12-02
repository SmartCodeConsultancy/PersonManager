#region Namespace References
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Model;
#endregion

namespace UKParliament.CodeTest.Data.Context
{
    public class PersonManagerContext : DbContext
    {
        public PersonManagerContext(DbContextOptions<PersonManagerContext> options)
            : base(options)
        {}

        public virtual DbSet<Person> People { get; set; }
    }
}
