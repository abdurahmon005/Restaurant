using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Products
{
    public class CreateProductModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? Description { get; set; }


    }
}
