namespace AutoTestEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        public int ID { get; set; }

        public int Grade { get; set; }

        [Required]
        public string Stem { get; set; }

        [Required]
        [StringLength(50)]
        public string Answer { get; set; }
    }
}
