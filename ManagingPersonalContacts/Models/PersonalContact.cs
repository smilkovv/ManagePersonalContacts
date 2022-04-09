using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManagingPersonalContacts.Models
{
    public class PersonalContact
    {
        public PersonalContact()
        {
            this.Addresses = new HashSet<Address>();
            this.PhoneNumbers = new HashSet<PhoneNumber>();
            this.Banks = new HashSet<Bank>();
        }

        [Display(Name = "Personal Contact")]
        public int PersonalContactID { get; set; }

        //[Required(ErrorMessageResourceName = "Required field")]
        [Required(ErrorMessage = "Field is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Sur Name")]
        public string SurName { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "IBAN")]
        public string IBAN { get; set; }

        /*
         Required data:
            first name
            surname
            Date of Birth
            address
            phone number
            IBAN
         */

        //*** User Picture
        [Display(Name = "File")]
        public string ImgFileName { get; set; }
        public string ImgContentType { get; set; }

        [Display(Name = "File")]
        public byte[] ImgContent { get; set; }

        //Generated File Name to be Uploaded on the Server 
        public string ImgFileNameGen { get; set; }
        //*** END of User Picture




        //Relations
        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }
    }

    public class PersonalContactDBContext : DbContext
    {
        public DbSet<PersonalContact> PersonalContacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<Bank> Banks { get; set; }
    }

}