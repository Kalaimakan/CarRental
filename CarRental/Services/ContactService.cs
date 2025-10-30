using CarRental.DTOs;
using CarRental.Interfaces;
using CarRental.Models;

namespace CarRental.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepo;

        public ContactService(IContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }

        public void SendContact(ContactDto contactDto)
        {
            var contact = new Contact
            {
                Name = contactDto.Name,
                Email = contactDto.Email,
                Subject = contactDto.Subject,
                Message = contactDto.Message
            };
            _contactRepo.AddContact(contact);
        }

        public List<ContactDto> GetAllContacts()
        {
            return _contactRepo.GetAllContacts()
                .Select(c => new ContactDto
                {
                    Name = c.Name,
                    Email = c.Email,
                    Subject = c.Subject,
                    Message = c.Message
                }).ToList();
        }
    }
}
