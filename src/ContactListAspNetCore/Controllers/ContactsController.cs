using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ContactListAspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactListAspNetCore.Controllers
{
  
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private const string FILENAME = "contacts.json";
        private GenericStorage _storage;
        private IMyDatabase _myDatabase;
        public ContactsController(IMyDatabase myDatabase)
        {
            _myDatabase = myDatabase;
            _storage = new GenericStorage();
        }
        private async Task<IEnumerable<Contact>> GetContacts()
        {
            var contacts = await _storage.Get(FILENAME);

            if (contacts == null)
            {
                await _storage.Save(new Contact[]{
                        new Contact { Id = 1, EmailAddress = "barney@contoso.com", Name = "Barney Poland"},
                        new Contact { Id = 2, EmailAddress = "lacy@contoso.com", Name = "Lacy Barrera"},
                        new Contact { Id = 3, EmailAddress = "lora@microsoft.com", Name = "Lora Riggs"}
                    }
                    , FILENAME);
            }

            return contacts;
        }

        /// <summary>
        /// Gets the list of contacts
        /// </summary>
        /// <returns>The contacts</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Contact>), 200)]
      
        [Route("~/contacts")]
        public async Task<IEnumerable<Contact>> Get()
        {
            return await GetContacts();
        }
        /// <summary>
        /// Gets a specific contact
        /// </summary>
        /// <param name="id">Identifier for the contact</param>
        /// <returns>The requested contact</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Contact>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Contact>), (int)HttpStatusCode.NotFound)]
        [Route("~/contacts/{id}")]
        public async Task<Contact> Get( int id)
        {
            var contacts = await GetContacts();
            return contacts.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="contact">The new contact</param>
        /// <returns>The saved contact</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.OK)]
        [Route("~/contacts")]
        public async Task<Contact> Post([FromBody] Contact contact)
        {
            var contacts = await GetContacts();
            var contactList = contacts.ToList();
            contactList.Add(contact);
            await _storage.Save(contactList, FILENAME);
            return contact;
        }

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="id">Identifier of the contact to be deleted</param>
        /// <returns>True if the contact was deleted</returns>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("~/contacts/{id}")]
        public async Task<IActionResult> Delete( int id)
        {
            var contacts = await GetContacts();
            var contactList = contacts.ToList();

            if (!contactList.Any(x => x.Id == id))
            {
                return NotFound();
            }
            else
            {
                contactList.RemoveAll(x => x.Id == id);
                await _storage.Save(contactList, FILENAME);
                return new NoContentResult();
            }
        }
    }

    public interface IMyDatabase
    {
    }

    public class MyDatabase : IMyDatabase
    {
        
    }
}