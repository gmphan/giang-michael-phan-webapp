using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;
using Microsoft.Extensions.Logging;

namespace Gmphan.BusinessAccessLib
{
    public class ContactServ : IContactServ
    {
        private readonly ILogger<ContactServ> _logger;
        private readonly IUnityOfWork _uow;
        public ContactServ(ILogger<ContactServ> logger
                        , IUnityOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }
        public async Task SaveContactMeServAsync(ContactMe obj)
        {
            obj.UpdatedDate = DateTime.UtcNow;
            try
            {
                await _uow.ContactMeRepoUOW.AddAsync(obj);
                await _uow.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error from SaveContactMeServAsync");
                throw ex;
            }
        }
        public async Task<ContactMeListView> GetContactMeListViewSerAsync()
        {
            ContactMeListView contactMeListView = new ContactMeListView();
            try
            {
                contactMeListView.ContactMeDTOs = (await _uow.ContactMeRepoUOW.GetAllAsync())
                .Select(contact => new ContactMeDTO
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    PhoneNum = contact.PhoneNum,
                    Message = contact.Message
                    // Add other mappings as necessary
                })
                .ToList();
                if (contactMeListView.ContactMeDTOs.Count > 0)
                {
                    return contactMeListView;
                }
                return null;   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // return a message. 
                return null;
            }
        }
    }
}