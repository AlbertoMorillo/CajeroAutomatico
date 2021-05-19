using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cajero.Models;
using Cajero.ViewModel;

namespace Cajero.Controllers
{
    public class CuentaBancoesController : Controller
    {
        private ATMDBEntities db = new ATMDBEntities();

        // GET: CuentaBancoes
        public ActionResult Index()
        {
            return View(db.CuentaBancoes.ToList());
        }

        // GET: CuentaBancoes/Details/5
        public ActionResult Deposito(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuentaBanco cuentaBanco = db.CuentaBancoes.Find(id);
            CuentaBancoViewModel cuenta = new CuentaBancoViewModel();
            cuenta.PIN = cuentaBanco.PIN;
            cuenta.NoCuenta = cuentaBanco.NoCuenta;
            cuenta.IdUser = cuentaBanco.IdUser;
            cuenta.Id = cuentaBanco.Id;
            cuenta.Balance = cuentaBanco.Balance;
            if (cuentaBanco == null)
            {
                return HttpNotFound();
            }
            return View(cuenta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposito([Bind(Include = "Id,Balance,NoCuenta,PIN,IdUser,Deposito,Retiro")] CuentaBancoViewModel cuentaBanco)
        {
            CuentaBanco cuentaBancoTemp = db.CuentaBancoes.Find(cuentaBanco.Id);
            if (cuentaBanco.PIN == cuentaBancoTemp.PIN)
            {
                if (ModelState.IsValid)
                {

                    cuentaBancoTemp.Balance += cuentaBanco.Deposito;
                    cuentaBancoTemp.Id = cuentaBanco.Id;
                    cuentaBancoTemp.IdUser = cuentaBanco.IdUser;
                    cuentaBancoTemp.NoCuenta = cuentaBanco.NoCuenta;
                    cuentaBancoTemp.PIN = cuentaBanco.PIN;
                    if (cuentaBancoTemp.Balance < 0)
                    {
                        TempData["EXCEED"] = "true";
                        return RedirectToAction("Index");
                    }
                    db.Entry(cuentaBanco).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["WRONGPIN"] = "true";
                return RedirectToAction("Index");
            }
            return View(cuentaBanco);
        }

        // GET: CuentaBancoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CuentaBancoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Balance,NoCuenta,PIN,IdUser")] CuentaBanco cuentaBanco)
        {
            if (ModelState.IsValid)
            {
                db.CuentaBancoes.Add(cuentaBanco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cuentaBanco);
        }

        // GET: CuentaBancoes/Edit/5
        public ActionResult Retiro(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuentaBanco cuentaBanco = db.CuentaBancoes.Find(id);
            CuentaBancoViewModel cuenta = new CuentaBancoViewModel();
            cuenta.PIN = cuentaBanco.PIN;
            cuenta.NoCuenta = cuentaBanco.NoCuenta;
            cuenta.IdUser = cuentaBanco.IdUser;
            cuenta.Id = cuentaBanco.Id;
            cuenta.Balance = cuentaBanco.Balance;
            if (cuentaBanco == null)
            {
                return HttpNotFound();
            }
            return View(cuenta);
        }

        // POST: CuentaBancoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Retiro([Bind(Include = "Id,Balance,NoCuenta,PIN,IdUser,Deposito,Retiro")] CuentaBancoViewModel cuentaBanco)
        {
            CuentaBanco cuentaBancoTemp = db.CuentaBancoes.Find(cuentaBanco.Id);
            if (cuentaBanco.PIN == cuentaBancoTemp.PIN)
            {
                if (ModelState.IsValid)
                {
                    cuentaBancoTemp.Balance = cuentaBancoTemp.Balance - cuentaBanco.Retiro;
                    cuentaBancoTemp.Id = cuentaBanco.Id;
                    cuentaBancoTemp.IdUser = cuentaBanco.IdUser;
                    cuentaBancoTemp.NoCuenta = cuentaBanco.NoCuenta;
                    cuentaBancoTemp.PIN = cuentaBanco.PIN;
                    if(cuentaBancoTemp.Balance < 0)
                    {
                        TempData["EXCEED"] = "true";
                        return RedirectToAction("Index");
                    }
                    db.Entry(cuentaBancoTemp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["WRONGPIN"] = "true";
                return RedirectToAction("Index");
            }
            return View(cuentaBanco);
        }

        // GET: CuentaBancoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuentaBanco cuentaBanco = db.CuentaBancoes.Find(id);
            if (cuentaBanco == null)
            {
                return HttpNotFound();
            }
            return View(cuentaBanco);
        }

        // POST: CuentaBancoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CuentaBanco cuentaBanco = db.CuentaBancoes.Find(id);
            db.CuentaBancoes.Remove(cuentaBanco);
            db.SaveChanges();
            return RedirectToAction("Index");
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
