using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManagingPersonalContacts.Models
{
    public class PhoneNumber
    {
        public int PhoneNumberID { get; set; }

        [Phone]
        [Required(ErrorMessage = "Field is required")]
        [Display(Name = "Phone Number")]
        public string PhoneNo { get; set; }

        [Display(Name = "Description")]
        public string Desc { get; set; }

        [Display(Name = "Use as Default")]
        public bool Default { get; set; }


        //Relation to PersonalContact 
        public int PersonalContactID { get; set; }

        public virtual PersonalContact PersonalContact { get; set; }


        private static PersonalContactDBContext db = new PersonalContactDBContext();
        //Check if records is Default in Database
        public static bool isDefault(PhoneNumber phoneNumber)
        {
            PhoneNumber phoneNumberRec = db.PhoneNumbers.Find(phoneNumber.PhoneNumberID);
            return phoneNumberRec.Default;
        }

        public static void RemoveDefaultFromOthers(PhoneNumber phoneNumber)
        {
            PersonalContact personalContact = db.PersonalContacts.Find(phoneNumber.PersonalContactID);
            bool isChanged = false;
            foreach (PhoneNumber i in personalContact.PhoneNumbers)
            {
                if (i.PhoneNumberID != phoneNumber.PhoneNumberID)
                {
                    i.Default = false;
                    db.Entry(i).State = EntityState.Modified;
                    isChanged = true;
                }
            }
            if (isChanged)
            {
                db.SaveChanges();
            }
        }

    }
}