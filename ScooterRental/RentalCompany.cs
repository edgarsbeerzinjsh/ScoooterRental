namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private readonly IScooterService _scooterService;
        private readonly List<IScooterRent> _scooterRents;
        public string Name { get; }

        public RentalCompany(string name, IScooterService scooterService, List<IScooterRent> scooterRents)
        {
            Name = name;
            _scooterService = scooterService;
            _scooterRents = scooterRents;
        }

        public void StartRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            if (scooter.IsRented) throw new ScooterIsInRentException();
            scooter.IsRented = true;
            _scooterRents.Add(new ScooterRent(scooter));
        }

        public decimal EndRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            var scooterRent = _scooterRents.FirstOrDefault(x => x.RentEnd == null && x.Scooter.Id == id && x.Scooter.IsRented);
            if (scooterRent == null) throw new ScooterIsNotRentedNowException();
            scooter.IsRented = false;
            scooterRent.CalculateBill(DateTime.Now);
            return scooterRent.Bill;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            var income = 0m;
            foreach (var rent in _scooterRents)
            {
                if (rent.RentEnd.Value.Year == year || year == null)
                {
                    var rentBill = rent.Bill;
                    if (rentBill == 0 && includeNotCompletedRentals)
                    {
                        rentBill = rent.CalculateRentedScooterBill(DateTime.Now);
                    }

                    income += rentBill;
                }
            }

            return income;
        }
    }
}
