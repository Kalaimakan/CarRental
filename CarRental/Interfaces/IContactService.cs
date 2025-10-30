using CarRental.DTOs;

namespace CarRental.Interfaces
{
    public interface IContactService
    {
        void SendContact(ContactDto contactDto);
        List<ContactDto> GetAllContacts();
    }
}
