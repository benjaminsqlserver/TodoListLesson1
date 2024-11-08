using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDoList.Server.Models.ConData
{
    [Table("Statuses", Schema = "dbo")]
    public partial class Status
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
        public int StatusID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string StatusName { get; set; }

        public ICollection<ToDoList> ToDoLists { get; set; }
    }
}