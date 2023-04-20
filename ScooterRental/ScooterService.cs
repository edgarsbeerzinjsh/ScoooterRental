namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private readonly IList<Scooter> _scooters;

        public ScooterService(List<Scooter> scooters)
        {
            _scooters = scooters;
        }
        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (pricePerMinute <= 0)
            {
                throw new InvalidPriceException();
            }

            var IsUniqueId = false;
            try
            {
                GetScooterById(id);
            }
            catch (ScooterNotFoundException)
            {
                _scooters.Add(new Scooter(id, pricePerMinute));
                IsUniqueId = true;
            }

            if (!IsUniqueId)
            {
                throw new NotUniqueScooterIdException();
            }
        }

        public Scooter GetScooterById(string scooterId)
        {
            if (string.IsNullOrEmpty(scooterId))
            {
                throw new ScooterIdNotProvidedException();
            }

            var scooter = _scooters.FirstOrDefault(x => x.Id == scooterId);
            if (scooter == null)
            {
                throw new ScooterNotFoundException();
            }

            return scooter;
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters.ToList();
        }

        public void RemoveScooter(string id)
        {
            var scooter = GetScooterById(id);
            if (scooter.IsRented)
            {
                throw new ScooterIsInRentException();
            }
            else
            {
                _scooters.Remove(scooter);
            }
        }
    }
}
