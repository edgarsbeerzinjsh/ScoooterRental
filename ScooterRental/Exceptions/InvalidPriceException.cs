namespace ScooterRental
{
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException() : base("Provided price not valid. Price must be positive number.")
        {

        }
    }
}
