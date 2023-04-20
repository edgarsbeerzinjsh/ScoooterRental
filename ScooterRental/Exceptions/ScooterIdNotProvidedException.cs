namespace ScooterRental
{
    public class ScooterIdNotProvidedException : Exception
    {
        public ScooterIdNotProvidedException() : base("Scooter Id not provided.")
        {

        }
    }
}
