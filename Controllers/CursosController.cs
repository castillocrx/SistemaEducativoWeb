using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEducativoWeb.Datos;
using SistemaEducativoWeb.Models;

namespace SistemaEducativoWeb.Controllers
{
    public class CursosController : Controller
    {
        private readonly SistemaEducativoWebContext _context;

        public CursosController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        // GET: Cursos
        public async Task<IActionResult> Index()
        {
            var cursos = await _context.Curso
                .Include(c => c.Programa)
                .Include(c => c.Tutor)
                .ToListAsync();

            return View(cursos);
        }

        // GET: Cursos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del curso no puede ser nulo.");
            }

            var curso = await _context.Curso
                .Include(c => c.Programa)
                .Include(c => c.Tutor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (curso == null)
            {
                return NotFound("Curso no encontrado.");
            }

            return View(curso);
        }

        // GET: Cursos/Create
        public IActionResult Create()
        {
            ViewData["ProgramaId"] = new SelectList(_context.Programa, "Id", "Nombre");
            ViewData["TutorId"] = new SelectList(_context.Tutor, "Id", "Apellidos");
            return View();
        }

        // POST: Cursos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Duracion,TutorId,ProgramaId")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(curso);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Curso creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear el curso: {ex.Message}";
                }
            }

            await CargarDatosSelect(curso);
            return View(curso);
        }

        // GET: Cursos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del curso no puede ser nulo.");
            }

            var curso = await _context.Curso.FindAsync(id);
            if (curso == null)
            {
                return NotFound("Curso no encontrado.");
            }

            await CargarDatosSelect(curso);
            return View(curso);
        }

        // POST: Cursos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Duracion,TutorId,ProgramaId")] Curso curso)
        {
            if (id != curso.Id)
            {
                return NotFound("El ID del curso no coincide.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Curso actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.Id))
                    {
                        return NotFound("Curso no encontrado.");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al actualizar el curso: {ex.Message}";
                }
            }

            await CargarDatosSelect(curso);
            return View(curso);
        }

        // GET: Cursos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del curso no puede ser nulo.");
            }

            var curso = await _context.Curso
                .Include(c => c.Programa)
                .Include(c => c.Tutor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (curso == null)
            {
                return NotFound("Curso no encontrado.");
            }

            return View(curso);
        }

        // POST: Cursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Curso.FindAsync(id);
            if (curso != null)
            {
                _context.Curso.Remove(curso);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Curso eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el curso: curso no encontrado.";
            }

            return RedirectToAction(nameof(Index));
        }

        //[HttpGet]
        //public JsonResult ReporteCursos()
        //{
        //    DT_Reporte objDT_Reporte = new DT_Reporte();

        //    List<ReporteCursos> objLista = objDT_Reporte.ReporteCursos();

        //    return Json(objLista, JsonRequestBehavior.AllowGet);
        //}

        private bool CursoExists(int id)
        {
            return _context.Curso.Any(e => e.Id == id);
        }

        private async Task CargarDatosSelect(Curso curso)
        {
            ViewData["ProgramaId"] = new SelectList(await _context.Programa.ToListAsync(), "Id", "Nombre", curso.ProgramaId);
            ViewData["TutorId"] = new SelectList(await _context.Tutor.ToListAsync(), "Id", "Apellidos", curso.TutorId);
        }
    }
}