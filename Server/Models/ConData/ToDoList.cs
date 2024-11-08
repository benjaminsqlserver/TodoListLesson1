using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDoList.Server.Models.ConData
{
    [Table("ToDoLists", Schema = "dbo")]
    public partial class ToDoList
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string TaskDescription { get; set; }

        [ConcurrencyCheck]
        public int? PriorityID { get; set; }

        public PriorityLevel PriorityLevel { get; set; }

        [ConcurrencyCheck]
        public int? StatusID { get; set; }

        public Status Status { get; set; }

        [ConcurrencyCheck]
        public DateTime? DueDate { get; set; }

        [ConcurrencyCheck]
        public DateTime? CreatedAt { get; set; }

        [ConcurrencyCheck]
        public DateTime? UpdatedAt { get; set; }

        [ConcurrencyCheck]
        public string Notes { get; set; }
    }
}