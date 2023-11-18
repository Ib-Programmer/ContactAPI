using ContactAPI.Data;
using ContactAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContactAPI.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactAPIDBContext dbContext;
        public ContactsController(ContactAPIDBContext dbContext) {
            this.dbContext = dbContext; 
        }

        [HttpGet]
        public IActionResult GetContacts()
        {
            return Ok(  dbContext.Contacts.ToList());
        }
        [HttpPost]
        public async Task <IActionResult> AddContacts(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                FullName = addContactRequest.FullName,
                Address = addContactRequest.Address,
                Phone = addContactRequest.Phone,
                Email = addContactRequest.Email
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
             
        }
        [HttpPut]
        [Route("{:id guid}")]
        public async Task<IActionResult> UpdateContacts([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact =  dbContext.Contacts.Find(id);
            if(contact != null) {
                contact.FullName = updateContactRequest.FullName;
                contact.Address = updateContactRequest.Address;
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;
                await dbContext.SaveChangesAsync();
                return Ok(contact);

            }
            return NotFound();

        }
        [HttpDelete]
        [Route("{:id guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = dbContext.Contacts.Find(id);
            if (contact != null)
            {
                dbContext.Contacts.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);

            }
            return NotFound();
          }
        }
}
