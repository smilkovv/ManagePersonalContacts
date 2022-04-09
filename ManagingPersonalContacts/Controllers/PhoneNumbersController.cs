using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagingPersonalContacts.Models;

namespace ManagingPersonalContacts.Controllers
{
    public class PhoneNumbersController : Controller
    {
        private PersonalContactDBContext db = new PersonalContactDBContext();

        // GET: /PhoneNumbers/
        public ActionResult Index()
        {
            var phonenumbers = db.PhoneNumbers.Include(p => p.PersonalContact);
            return View(phonenumbers.ToList());
        }

        // GET: /PhoneNumbers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneNumber phonenumber = db.PhoneNumbers.Find(id);
            if (phonenumber == null)
            {
                return HttpNotFound();
            }
            return View(phonenumber);
        }

        // GET: /PhoneNumbers/Create
        public ActionResult Create(int PcID)
        {
            //*ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName");

            PhoneNumber phoneNumberDefault_PersonalContactID = new PhoneNumber();
            phoneNumberDefault_PersonalContactID.PersonalContactID = PcID;

            //*return View();
            return View(phoneNumberDefault_PersonalContactID);
        }

        // POST: /PhoneNumbers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PhoneNumberID,PhoneNo,Desc,Default,PersonalContactID")] PhoneNumber phonenumber)
        {
            if (ModelState.IsValid)
            {
                PersonalContact personalContact = db.PersonalContacts.Find(phonenumber.PersonalContactID);

                //If this is first record for Personal Contact set it as Default 
                if (personalContact.PhoneNumbers.Count() == 0)
                {
                    phonenumber.Default = true;
                }
                else
                {
                    //If more than 1 record and is checked as Default - delete Default from other records
                    if (phonenumber.Default == true)
                    {
                        PhoneNumber.RemoveDefaultFromOthers(phonenumber);
                    }
                }


                db.PhoneNumbers.Add(phonenumber);
                db.SaveChanges();
                //return RedirectToAction("Index");

                //return RedirectToAction("Method" , "Controller" );
                string httpString = "Details/" + phonenumber.PersonalContactID;
                return RedirectToAction(httpString, "PersonalContacts");
            }

            ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName", phonenumber.PersonalContactID);
            return View(phonenumber);
        }

        // GET: /PhoneNumbers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneNumber phonenumber = db.PhoneNumbers.Find(id);
            if (phonenumber == null)
            {
                return HttpNotFound();
            }
            //*ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName", phonenumber.PersonalContactID);
            return View(phonenumber);
        }

        // POST: /PhoneNumbers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PhoneNumberID,PhoneNo,Desc,Default,PersonalContactID")] PhoneNumber phonenumber)
        {
            if (ModelState.IsValid)
            {
                //If checked as Default - delete Default from other records
                if (phonenumber.Default == true && !PhoneNumber.isDefault(phonenumber))
                {
                    PhoneNumber.RemoveDefaultFromOthers(phonenumber);
                }

                db.Entry(phonenumber).State = EntityState.Modified;
                db.SaveChanges();
                
                //return RedirectToAction("Index");

                //return RedirectToAction("Method" , "Controller" );
                string httpString = "Details/" + phonenumber.PersonalContactID;
                return RedirectToAction(httpString, "PersonalContacts");
            }
            //*ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName", phonenumber.PersonalContactID);
            return View(phonenumber);
        }

        // GET: /PhoneNumbers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneNumber phonenumber = db.PhoneNumbers.Find(id);
            if (phonenumber == null)
            {
                return HttpNotFound();
            }
            return View(phonenumber);
        }

        // POST: /PhoneNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhoneNumber phonenumber = db.PhoneNumbers.Find(id);

            //Note* save subobject-PersonalContactID to redirect to the correct document after delete
            int phonenumber_PersonalContactID = phonenumber.PersonalContactID; 

            db.PhoneNumbers.Remove(phonenumber);
            db.SaveChanges();

            //return RedirectToAction("Index");

            //return RedirectToAction("Method" , "Controller" );
            string httpString = "Details/" + phonenumber_PersonalContactID;
            return RedirectToAction(httpString, "PersonalContacts");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
