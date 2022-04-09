using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManagingPersonalContacts.Models
{
    public class Bank
    {
        public int BankID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Desc { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Display(Name = "IBAN")]
        public string IBAN { get; set; }

        [Display(Name = "Use as Default")]
        public bool Default { get; set; }


        //Relation to PersonalContact 
        public int PersonalContactID { get; set; }

        public virtual PersonalContact PersonalContact { get; set; }


        private static PersonalContactDBContext db = new PersonalContactDBContext();
        //Check if records is Default in Database
        public static bool isDefault(Bank bank)
        {
            Bank bankRec = db.Banks.Find(bank.BankID);
            return bankRec.Default;
        }

        public static void RemoveDefaultFromOthers(Bank bank)
        {
            PersonalContact personalContact = db.PersonalContacts.Find(bank.PersonalContactID);
            bool isChanged = false;
            foreach (Bank i in personalContact.Banks)
            {
                if (i.BankID != bank.BankID)
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