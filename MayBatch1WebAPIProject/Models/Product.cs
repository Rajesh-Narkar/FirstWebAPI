using System.ComponentModel.DataAnnotations;

namespace MayBatch1WebAPIProject.Models
{
    public class Product { 

        [Key]
        public int id { get; set; }
        public string Pname { get; set; }
        public string Pcat { get; set; }
        public string Prize { get; set; }

    }
}
