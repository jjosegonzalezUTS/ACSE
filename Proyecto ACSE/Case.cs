using SQLite;
using System;

namespace Proyecto_ACSE
{
    [Table("case")]
    public class Case
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("date")]
        public string Date { get; set; }

        [Column("time")]
        public string Time { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("persons_involved")]
        public string PersonsInvolved { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("evidence")]
        public byte[] Evidence { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("anonymous_report")]
        public bool AnonymousReport { get; set; }

        [Column("in_progress")]
        public bool InProgress { get; set; }

        [Column("resolved")]
        public bool Resolved { get; set; }

        [Column("notes")]
        public string notes { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("registration_date")]
        public string RegistrationDate { get; set; }

        [Column("registration_time")]
        public string RegistrationTime { get; set; }
    }
}
