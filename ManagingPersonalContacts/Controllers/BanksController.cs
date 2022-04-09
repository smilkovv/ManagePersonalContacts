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
    public class BanksController : Controller
    {
        private PersonalContactDBContext db = new PersonalContactDBContext();

        // GET: /Banks/
        public ActionResult Index()
        {
            var banks = db.Banks.Include(b => b.PersonalContact);
            return View(banks.ToList());
        }

        // GET: /Banks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            return View(bank);
        }

        // GET: /Banks/Create
        public ActionResult Create(int PcID)
        {
            //*ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName");

            Bank bankDefault_PersonalContactID = new Bank();
            bankDefault_PersonalContactID.PersonalContactID = PcID;

            //return View();
            return View(bankDefault_PersonalContactID);
        }

        // POST: /Banks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="BankID,Name,Desc,IBAN,Default,PersonalContactID")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                PersonalContact personalContact = db.PersonalContacts.Find(bank.PersonalContactID);

                //If this is first record for Personal Contact set it as Default 
                if (personalContact.Banks.Count() == 0)
                {
                    bank.Default = true;
                }
                else
                {
                    //If more than 1 record and is checked as Default - delete Default from other records
                    if (bank.Default == true)
                    {
                        Bank.RemoveDefaultFromOthers(bank);
                    }
                }


                db.Banks.Add(bank);
                db.SaveChanges();
                //return RedirectToAction("Index");

                //return RedirectToAction("Method" , "Controller" );
                string httpString = "Details/" + bank.PersonalContactID;
                return RedirectToAction(httpString, "PersonalContacts");
            }

            ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName", bank.PersonalContactID);
            return View(bank);
        }

        // GET: /Banks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            //*ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName", bank.PersonalContactID);
            return View(bank);
        }

        // POST: /Banks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="BankID,Name,Desc,IBAN,Default,PersonalContactID")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                //If checked as Default - delete Default from other records
                if (bank.Default == true && !Bank.isDefault(bank))
                {
                    Bank.RemoveDefaultFromOthers(bank);
                }

                db.Entry(bank).State = EntityState.Modified;
                db.SaveChanges();

                //return RedirectToAction("Index");

                //return RedirectToAction("Method" , "Controller" );
                string httpString = "Details/" + bank.PersonalContactID;
                return RedirectToAction(httpString, "PersonalContacts");
            }
            //*ViewBag.PersonalContactID = new SelectList(db.PersonalContacts, "PersonalContactID", "FirstName", bank.PersonalContactID);
            return View(bank);
        }

        // GET: /Banks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            return View(bank);
        }

        // POST: /Banks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bank bank = db.Banks.Find(id);

            //Note* save subobject-PersonalContactID to redirect to the correct document after delete
            int bank_PersonalContactID = bank.PersonalContactID; ;

            db.Banks.Remove(bank);
            db.SaveChanges();

            //return RedirectToAction("Index");

            //return RedirectToAction("Method" , "Controller" );
            string httpString = "Details/" + bank_PersonalContactID;
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
