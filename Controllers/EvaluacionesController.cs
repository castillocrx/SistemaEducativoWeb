using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEducativoWeb.Models;

namespace SistemaEducativoWeb.Controllers
{
    public class EvaluacionesController : Controller
    {
        private readonly SistemaEducativoWebContext _context;

        public EvaluacionesController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        // GET: Evaluaciones
        public async Task<IActionResult> Index()
        {
            var evaluaciones = await _context.Evaluacion
                .Include(e => e.Curso)
                .Include(e => e.Estudiante)
                .ToListAsync();

            return View(evaluaciones);
        }

        // GET: Evaluaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID no puede ser nulo.");
            }

            var evaluacion = await _context.Evaluacion
                .Include(e => e.Curso)
                .Include(e => e.Estudiante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evaluacion == null)
            {
                return NotFound("Evaluación no encontrada.");
            }

            return View(evaluacion);
        }

        // GET: Evaluaciones/Create
        public IActionResult Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre");
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos");
            return View();
        }

        // POST: Evaluaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EstudianteId,CursoId,FechaEvaluacion,Calificacion,Comentarios")] Evaluacion evaluacion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    evaluacion.FechaEvaluacion=DateTime.Now;
                    _context.Add(evaluacion);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Evaluación creada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear la evaluación: {ex.Message}";
                }
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre", evaluacion.CursoId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos", evaluacion.EstudianteId);
            return View(evaluacion);
        }

        // GET: Evaluaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID no puede ser nulo.");
            }

            var evaluacion = await _context.Evaluacion.FindAsync(id);
            if (evaluacion == null)
            {
                return NotFound("Evaluación no encontrada.");
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre", evaluacion.CursoId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos", evaluacion.EstudianteId);
            return View(evaluacion);
        }

        // POST: Evaluaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EstudianteId,CursoId,FechaEvaluacion,Calificacion,Comentarios")] Evaluacion evaluacion)
        {
            if (id != evaluacion.Id)
            {
                return BadRequest("El ID de la evaluación no coincide.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluacion);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Evaluación actualizada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluacionExists(evaluacion.Id))
                    {
                        return NotFound("Evaluación no encontrada.");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al editar la evaluación: {ex.Message}";
                }
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", " Nombre", evaluacion.CursoId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos", evaluacion.EstudianteId);
            return View(evaluacion);
        }

        // GET: Evaluaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID no puede ser nulo.");
            }

            var evaluacion = await _context.Evaluacion
                .Include(e => e.Curso)
                .Include(e => e.Estudiante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evaluacion == null)
            {
                return NotFound("Evaluación no encontrada.");
            }

            return View(evaluacion);
        }

        // POST: Evaluaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var evaluacion = await _context.Evaluacion.FindAsync(id);
                if (evaluacion != null)
                {
                    _context.Evaluacion.Remove(evaluacion);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Evaluación eliminada exitosamente.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar la evaluación: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool EvaluacionExists(int id)
        {
            return _context.Evaluacion.Any(e => e.Id == id);
        }
    }
}