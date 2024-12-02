using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEducativoWeb.Models;

namespace SistemaEducativoWeb.Controllers
{
    public class MatriculasController : Controller
    {
        private readonly SistemaEducativoWebContext _context;

        public MatriculasController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        // GET: Matriculas
        public async Task<IActionResult> Index()
        {
            var matriculas = await _context.Matricula
                .Include(m => m.Curso)
                .Include(m => m.Estudiante)
                .ToListAsync();

            return View(matriculas);
        }

        // GET: Matriculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID no puede ser nulo.");
            }

            var matricula = await _context.Matricula
                .Include(m => m.Curso)
                .Include(m => m.Estudiante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matricula == null)
            {
                return NotFound("Matrícula no encontrada.");
            }

            return View(matricula);
        }

        // GET: Matriculas/Create
        public IActionResult Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre");
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos");
            return View();
        }

        // POST: Matriculas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EstudianteId,CursoId,Estado,FechaInscripcion")] Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(matricula);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Matrícula creada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear la matrícula: {ex.Message}";
                }
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre", matricula.CursoId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos", matricula.EstudianteId);
            return View(matricula);
        }

        // GET: Matriculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID no puede ser nulo.");
            }

            var matricula = await _context.Matricula.FindAsync(id);
            if (matricula == null)
            {
                return NotFound("Matrícula no encontrada.");
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre", matricula.CursoId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos", matricula.EstudianteId);
            return View(matricula);
        }

        // POST: Matriculas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EstudianteId,CursoId,Estado,FechaInscripcion")] Matricula matricula)
        {
            if (id != matricula.Id)
            {
                return BadRequest("El ID de la matrícula no coincide.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matricula);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Matrícula actualizada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatriculaExists(matricula.Id))
                    {
                        return NotFound("Matrícula no encontrada.");
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al editar la matrícula: {ex.Message}";
                }
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre", matricula.CursoId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos", matricula.EstudianteId);
            return View(matricula);
        }

        // GET: Matriculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID no puede ser nulo.");
            }

            var matricula = await _context.Matricula
                .Include(m => m.Curso)
                .Include(m => m.Estudiante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matricula == null)
            {
                return NotFound("Matrícula no encontrada.");
            }

            return View(matricula);
        }

        // POST: Matriculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var matricula = await _context.Matricula.FindAsync(id);
            if (matricula != null)
            {
                _context.Matricula.Remove(matricula);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Matrícula eliminada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar la matrícula.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MatriculaExists(int id)
        {
            return _context.Matricula.Any(e => e.Id == id);
        }
    }
}