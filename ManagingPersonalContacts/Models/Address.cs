using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManagingPersonalContacts.Models
{
    public class Address
    {
        public int AddressID { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Display(Name = "Description")]
        public string Desc { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Street")]
        public string Street { get; set; }

        [Display(Name = "Number")]
        public string Number { get; set; }

        [Display(Name = "Entrance")]
        public string Entrance { get; set; }

        [Display(Name = "Floor")]
        public string Floor { get; set; }

        [Display(Name = "Apartment")]
        public string Apartment { get; set; }

        [Display(Name = "Use as Default")]
        public bool Default { get; set; }


        //Relation to PersonalContact 
        public int PersonalContactID { get; set; }

        public virtual PersonalContact PersonalContact { get; set; }



        private static PersonalContactDBContext db = new PersonalContactDBContext();
        //Check if records is Default in Database
        public static bool isDefault(Address address)
        {
            Address addressRec = db.Addresses.Find(address.AddressID);
            return addressRec.Default;
        }

        public static void RemoveDefaultFromOthers(Address address)
        {
            PersonalContact personalContact = db.PersonalContacts.Find(address.PersonalContactID);
            bool isChanged = false;
            foreach (Address i in personalContact.Addresses)
            {
                if (i.AddressID != address.AddressID)
                {
                    i.Default = false;
                    db.Entry(i).State = EntityState.Modified;
                    isChanged = true;
                }
            }
            if(isChanged)
            {
                db.SaveChanges();
            }
        }

    }
}