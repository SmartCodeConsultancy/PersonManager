#region Namespace References
using UKParliament.CodeTest.Pattern.Entities;
using System;
#endregion

namespace UKParliament.CodeTest.Data.Model
{
    public class Person : Entity<int>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
