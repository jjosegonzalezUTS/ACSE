using SQLite;

namespace Proyecto_ACSE
{
    [Table("user")]
    public class User
    {
        public enum UserRole
        {
            Admin,
            User
        }

        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("role")]
        public UserRole Role { get; set; }

        [Column("anonymous")]
        public bool Anonymous { get; set; }
    }
}