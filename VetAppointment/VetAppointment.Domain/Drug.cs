using System;
using VetAppointment.Domain.Helpers;
#nullable disable
namespace VetAppointment.Domain
{
    public class Drug
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public double Quantity { get; private set; }
        public double UnitPrice { get; private set; }

        public static Result<Drug> Create(string name, double quantity, double unitPrice)
        {
            if (quantity < 0)
            {
                return Result<Drug>.Failure($"Quantity {quantity} is not valid");
            }
            if (unitPrice < 0)
            {
                return Result<Drug>.Failure($"Unit price {unitPrice} is not valid");
            }
            var drug = new Drug
            {
                Name = name,
                Quantity = quantity,
                UnitPrice = unitPrice
            };

            return Result<Drug>.Success(drug);
        }

        public Result UpdateQuantity(double quantity)
        {
            if (quantity < 0)
            {
                return Result.Failure($"New quantity {quantity} is not valid");
            }
 
            Quantity = quantity;

            return Result.Success();
        }

        public Result UpdatePrice(double price)
        {
            if (price < 0)
            {
                return Result.Failure($"New price {price} is not valid");
            }

            UnitPrice = price;

            return Result.Success();
        }
    }
}
