namespace CarRental.ViewModels
{
    public class UpdateProfileViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string LicenceNumber { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
