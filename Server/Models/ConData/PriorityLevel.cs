using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDoList.Server.Models.ConData
{
    [Table("PriorityLevels", Schema = "dbo")]
    public partial class PriorityLevel
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
        [Required]
        public int PriorityID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string PriorityName { get; set; }

        public ICollection<ToDoList> ToDoLists { get; set; }
    }
}