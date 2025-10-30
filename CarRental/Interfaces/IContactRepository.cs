using CarRental.Models;

namespace CarRental.Interfaces
{
    public interface IContactRepository
    {
        void AddContact(Contact contact);
        List<Contact> GetAllContacts();
    }
}
