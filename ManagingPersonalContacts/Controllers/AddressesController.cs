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
    public class AddressesController : Controller
    {
        private PersonalContactDBContext db = new PersonalContactDBContext();

        // GET: /Addresses/
        public ActionResult Index()
        {
            var addresses = db.Addresses.Include(a => a.PersonalContact);
            return View(addresses.ToList());
        }

        // GET: /Addresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // GET: /Addresses/Create
        public ActionResult Create(int PcID)
        {
            //*ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName");

            Address addressDefault_PersonalContactID = new Address();
            addressDefault_PersonalContactID.PersonalContactID = PcID;

            //*return View();
            return View(addressDefault_PersonalContactID);
        }

        // POST: /Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AddressID,Desc,Country,City,Street,Number,Entrance,Floor,Apartment,Default,PersonalContactID")] Address address)
        {
            if (ModelState.IsValid)
            {
                PersonalContact personalContact = db.PersonalContacts.Find(address.PersonalContactID);

                //If this is first record for Personal Contact set it as Default 
                if(personalContact.Addresses.Count() == 0)
                {
                    address.Default = true;
                }
                else
                {
                    //If more than 1 record and is checked as Default - delete Default from other records
                    if (address.Default == true)
                    {
                        Address.RemoveDefaultFromOthers(address);
                    }
                }

                db.Addresses.Add(address);
                db.SaveChanges();
                //return RedirectToAction("Index");

                //return RedirectToAction("Method" , "Controller" );
                string httpString = "Details/" + address.PersonalContactID;
                return RedirectToAction(httpString, "PersonalContacts");
            }

            ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName", address.PersonalContactID);
            return View(address);
        }

        // GET: /Addresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            //*ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName", address.PersonalContactID);
            return View(address);
        }

        // POST: /Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="AddressID,Desc,Country,City,Street,Number,Entrance,Floor,Apartment,Default,PersonalContactID")] Address address)
        {
            if (ModelState.IsValid)
            {
                //If checked as Default - delete Default from other records
                if(address.Default == true && !Address.isDefault(address))
                {
                    Address.RemoveDefaultFromOthers(address);
                }

                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();

                //return RedirectToAction("Index");

                //return RedirectToAction("Method" , "Controller" );
                string httpString = "Details/" + address.PersonalContactID;
                return RedirectToAction(httpString, "PersonalContacts");
            }
            //*ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName", address.PersonalContactID);
            return View(address);
        }

        // GET: /Addresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: /Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address address = db.Addresses.Find(id);

            //Note* save subobject-PersonalContactID to redirect to the correct document after delete
            int address_PersonalContactID = address.PersonalContactID;

            db.Addresses.Remove(address);
            db.SaveChanges();

            //return RedirectToAction("Index");

            //return RedirectToAction("Method" , "Controller" );
            string httpString = "Details/" + address_PersonalContactID;
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
