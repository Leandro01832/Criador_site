using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DataContextCriacaoSite;
using business;

namespace CriadorSites.Controllers
{
    public class CodigoController : Controller
    {
        private BD db = new BD();

        // GET: Codigo
        public ActionResult Index()
        {
            var codigo = db.Codigo.Include(c => c.Pagina);
            return View(codigo.ToList());
        }

        // GET: Codigo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Codigo codigo = db.Codigo.Find(id);
            if (codigo == null)
            {
                return HttpNotFound();
            }
            return View(codigo);
        }

        // GET: Codigo/Create
        [ValidateInput(false)]
        public ActionResult Create()
        {
            ViewBag.pagina_ = new SelectList(db.Pagina, "IdPagina", "Titulo");
            return View();
        }

        // POST: Codigo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "IdCodigo,Efeito,pagina_")] Codigo codigo)
        {
            if (ModelState.IsValid)
            {
                db.Codigo.Add(codigo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pagina_ = new SelectList(db.Pagina, "IdPagina", "Titulo", codigo.pagina_);
            return View(codigo);
        }

        // GET: Codigo/Edit/5
        [ValidateInput(false)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Codigo codigo = db.Codigo.Find(id);
            if (codigo == null)
            {
                return HttpNotFound();
            }
            ViewBag.pagina_ = new SelectList(db.Pagina, "IdPagina", "Titulo", codigo.pagina_);
            return View(codigo);
        }

        // POST: Codigo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "IdCodigo,Efeito,pagina_")] Codigo codigo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(codigo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pagina_ = new SelectList(db.Pagina, "IdPagina", "Titulo", codigo.pagina_);
            return View(codigo);
        }

        // GET: Codigo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Codigo codigo = db.Codigo.Find(id);
            if (codigo == null)
            {
                return HttpNotFound();
            }
            return View(codigo);
        }

        // POST: Codigo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Codigo codigo = db.Codigo.Find(id);
            db.Codigo.Remove(codigo);
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
