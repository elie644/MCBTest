using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MCBTest.Models
{
    public class MCElement
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime? DateBirthOrIncorp { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public decimal NumShare { get; set; }
        [Required]
        public decimal SharePrice { get; set; }
        [Required]
        public decimal Balance { get; set; }
    }
}