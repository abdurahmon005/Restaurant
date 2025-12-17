using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebApp.Domain.Entities;
using WebApp.Domain.Enums;

namespace WebApp.Aplication.Models.Order
{
    public class CreateOrderModel
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public StatusType Status { get; set; }
        [JsonIgnore]
        public virtual User Customer { get; set; }
    }
}
