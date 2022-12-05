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


        public Result Update(string name, double quantity, double price)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result.Failure("Name can not be null or empty!");
            }

            if (quantity < 0)
            {
                return Result.Failure($"New quantity {quantity} is not valid");
            }

            if (price < 0)
            {
                return Result.Failure($"New price {price} is not valid");
            }

            this.Name = name;
            this.Quantity = quantity;
            this.UnitPrice = price;

            return Result.Success();
        }
    }
}
