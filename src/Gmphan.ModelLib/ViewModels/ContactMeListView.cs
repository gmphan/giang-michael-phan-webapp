using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ContactMeListView
    {
        public List<ContactMeDTO> ContactMeDTOs { get; set; }
        public ContactMeListView()
        {
            ContactMeDTOs = new List<ContactMeDTO>();
        }
        public void SortByCreatedDate()
        {
            if(ContactMeDTOs.Count > 0)
            {
                ContactMeDTOs = ContactMeDTOs.OrderByDescending(x => x.CreatedDate).ToList();
            }
        }
    }
}