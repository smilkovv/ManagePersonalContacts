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
    public class PersonalContactsController : Controller
    {
        private PersonalContactDBContext db = new PersonalContactDBContext();

        // GET: /PersonalContacts/
        public ActionResult Index()
        {
            return View(db.PersonalContacts.ToList());
        }

        // GET: /PersonalContacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalContact personalcontact = db.PersonalContacts.Find(id);
            if (personalcontact == null)
            {
                return HttpNotFound();
            }
            return View(personalcontact);
        }

        // GET: /PersonalContacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /PersonalContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonalContactID,FirstName,SurName,DateOfBirth,Address,PhoneNumber,IBAN,ImgFileName,ImgContentType,ImgContent,ImgFileNameGen")] PersonalContact personalcontact, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                //NEw
                Address address = new Address();
                PhoneNumber phoneNumber = new PhoneNumber();
                Bank bank = new Bank();
                if (!String.IsNullOrWhiteSpace(personalcontact.Address))
                {
                    address.Desc = personalcontact.Address;
                    address.Default = true;
                    personalcontact.Address = String.Empty;
                }

                if (!String.IsNullOrWhiteSpace(personalcontact.PhoneNumber))
                {
                    phoneNumber.PhoneNo = personalcontact.PhoneNumber;
                    phoneNumber.Default = true;
                    personalcontact.PhoneNumber = null;
                }

                if (!String.IsNullOrWhiteSpace(personalcontact.IBAN))
                {
                    bank.IBAN = personalcontact.IBAN;
                    bank.Default = true;
                    personalcontact.IBAN = String.Empty;
                }

                #region Upload file
                if (upload != null && upload.ContentLength > 0)
                {
                    personalcontact.ImgFileName = System.IO.Path.GetFileName(upload.FileName);
                    personalcontact.ImgContentType = upload.ContentType;

                    #region Upload to Server
                    //****************************.
                    //Upload to Server
                    try
                    {
                        //Generate File Name for Upload on the Server 
                        string fileNameGen = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(upload.FileName);

                        personalcontact.ImgFileNameGen = fileNameGen;

                        //String path = Server.MapPath("~/files/") + fileNameGen; //"~/images/");
                        String path = Server.MapPath("~/files/") + fileNameGen; //"~/images/");
                        //upload.SaveAs(path + upload.FileName);
                        upload.SaveAs(path);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                    }
                    //END Upload to Server
                    #endregion

                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        personalcontact.ImgContent = reader.ReadBytes(upload.ContentLength);
                    }
                }
                #endregion

                db.PersonalContacts.Add(personalcontact);
                db.SaveChanges();

                //New
                if (!String.IsNullOrWhiteSpace(address.Desc))
                {
                    address.PersonalContactID = personalcontact.PersonalContactID;
                    db.Addresses.Add(address);
                    db.SaveChanges();
                }

                if (!String.IsNullOrWhiteSpace(phoneNumber.PhoneNo))
                {
                    phoneNumber.PersonalContactID = personalcontact.PersonalContactID;
                    db.PhoneNumbers.Add(phoneNumber);
                    db.SaveChanges();
                }

                if (!String.IsNullOrWhiteSpace(bank.IBAN))
                {
                    bank.PersonalContactID = personalcontact.PersonalContactID;
                    db.Banks.Add(bank);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(personalcontact);
        }

        // GET: /PersonalContacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalContact personalcontact = db.PersonalContacts.Find(id);
            if (personalcontact == null)
            {
                return HttpNotFound();
            }
            return View(personalcontact);
        }

        // POST: /PersonalContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonalContactID,FirstName,SurName,DateOfBirth,Address,PhoneNumber,IBAN,ImgFileName,ImgContentType,ImgContent,ImgFileNameGen")] PersonalContact personalcontact, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                //Image 
                #region Upload the file (Delete previous)
                //****************************.
                //*** Upload the image file
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        //*isImageCh = true;

                        personalcontact.ImgFileName = System.IO.Path.GetFileName(upload.FileName);
                        personalcontact.ImgContentType = upload.ContentType;

                        #region Upload to Server
                        //********************
                        //Upload to Server
                        try
                        {
                            //Generate File Name for Upload on the Server 
                            string fileNameGen = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(upload.FileName);

                            personalcontact.ImgFileNameGen = fileNameGen;

                            String path = Server.MapPath("~/files/") + fileNameGen; //"~/images/");
                            //upload.SaveAs(path + upload.FileName);
                            upload.SaveAs(path);
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = ex.Message;
                        }
                        //END Upload to Server
                        //********************
                        #endregion

                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            personalcontact.ImgContent = reader.ReadBytes(upload.ContentLength);
                        }

                        #region Delete previous file from Server
                        //******************************
                        //Delete previous file from Server 
                        try
                        {
                            PersonalContactDBContext dbREC = new PersonalContactDBContext();
                            PersonalContact personalcontactREC = dbREC.PersonalContacts.Find(personalcontact.PersonalContactID);
                            if(personalcontactREC.ImgFileNameGen != null)
                            {
                               System.IO.File.Delete(Server.MapPath("~/files/") + personalcontactREC.ImgFileNameGen);
                               ViewBag.Message = String.Format("{0} {1}", "Record Deleted File" ,personalcontactREC.ImgFileName);
                            }

                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorMessage = ex.Message;
                        }
                        //******************************
                        #endregion

                    }


                }

                //*** END of Upload the image file
                //****************************.
                #endregion

                db.Entry(personalcontact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personalcontact);
        }

        // GET: /PersonalContacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalContact personalcontact = db.PersonalContacts.Find(id);
            if (personalcontact == null)
            {
                return HttpNotFound();
            }
            return View(personalcontact);
        }

        // POST: /PersonalContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonalContact personalcontact = db.PersonalContacts.Find(id);

            #region Delete previous file from Server
            //******************************
            //Delete previous file from Server 
            try
            {
                if (personalcontact.ImgFileNameGen != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/files/") + personalcontact.ImgFileNameGen);
                    ViewBag.Message = String.Format("{0} {1}", "Record Deleted File", personalcontact.ImgFileName);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            //******************************
            #endregion

            db.PersonalContacts.Remove(personalcontact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult DeleteFile(int? id)
        {
            PersonalContact personalContact = db.PersonalContacts.Find(id);

            #region Sure Delete

            ViewBag.LinkTextSubmit = "Delete";
 
            //ViewBag.Link = "/PersonalContacts/Edit/" + personalContact.PersonalContactID;
            //ViewBag.LinkText = "Cancel";

            ViewBag.MessageJF = String.Format("Attached file to Personal Contacts. File: '{0}')", personalContact.ImgFileName);

            
            #endregion

            return View(personalContact);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(int id)
        {
            PersonalContact personalcontact = db.PersonalContacts.Find(id);

            string imgFileName = personalcontact.ImgFileName;

            #region Delete previous file from Server
            //******************************
            //Delete previous file from Server 
            if (!String.IsNullOrEmpty(personalcontact.ImgFileNameGen))
            {
                try
                {
                    System.IO.File.Delete(Server.MapPath("~/files/") + personalcontact.ImgFileNameGen);
                    //isFileDel = true;


                    //Edit
                    if (ModelState.IsValid)
                    {
                        //*** Delete vendor Logo


                        personalcontact.ImgFileName = null;
                        personalcontact.ImgContentType = null;
                        personalcontact.ImgContent = null;
                        personalcontact.ImgFileNameGen = null;

                        db.Entry(personalcontact).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            //******************************
            #endregion

            //return RedirectToAction("Method" , "Controller" );
            string httpString = "Edit/" + personalcontact.PersonalContactID;
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
