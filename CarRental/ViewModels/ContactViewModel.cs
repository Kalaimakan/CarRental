using CarRental.DTOs;
using CarRental.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class ContactViewModel
    {
        public ContactDto Contact { get; set; } = new ContactDto(); 
        public string SuccessMessage { get; set; }
    }
}
