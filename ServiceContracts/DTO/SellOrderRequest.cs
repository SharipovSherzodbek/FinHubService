using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Enities;

namespace ServiceContracts.DTO
{
  public class SellOrderRequest : IValidatableObject
  {
    [Required]
    public string? StockSymbol { get; set; }
    [Required]
    public string? StockName { get; set; }

    [Range(typeof(DateTime), "2000-01-01", "9999-12-31", 
      ErrorMessage = "Date must be after January 1, 2000.")]
    public DateTime DateAndTimeOfOrder { get; set; }
    [Range(1, 100000)]
    public uint Quantity { get; set; }
    [Range(1, 10000)]
    public double Price { get; set; }

    public SellOrder ToSellOrder()
    {
      return new SellOrder
      {
        StockSymbol = StockSymbol,
        StockName = StockName,
        Price = Price,
        Quantity = Quantity,
        DateAndTimeOfOrder = DateAndTimeOfOrder
      };
    }

    public IEnumerable<ValidationResult> Validate(
      ValidationContext validationContext)
    {
      List<ValidationResult> result = new List<ValidationResult>();

     if(DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
      {
        result.Add(new ValidationResult("The Date time cannot be " +
          "older than 2000-01-01"));
      }

     return result;
    }
  }
}
