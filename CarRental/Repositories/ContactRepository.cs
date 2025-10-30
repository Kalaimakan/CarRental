using CarRental.Database;
using CarRental.Interfaces;
using CarRental.Models;

namespace CarRental.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbcontext _context;

        public ContactRepository(AppDbcontext context)
        {
            _context = context;
        }

        public void AddContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();
        }

        public List<Contact> GetAllContacts()
        {
            return _context.Contacts.ToList();
        }
    }
}
