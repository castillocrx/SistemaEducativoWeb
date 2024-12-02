using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEducativoWeb.Models;

namespace SistemaEducativoWeb.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly SistemaEducativoWebContext _context;

        public EstudiantesController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index()
        {
            var estudiantes = await _context.Estudiante.ToListAsync();
            return View(estudiantes);
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiante.FindAsync(id.Value);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estudiantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellidos,Correo,Telefono,FechaNacimiento,FechaRegistro")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(estudiante);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Estudiante creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear el estudiante: {ex.Message}";
                }
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiante.FindAsync(id.Value);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellidos,Correo,Telefono,FechaNacimiento,FechaRegistro")] Estudiante estudiante)
        {
            if (id != estudiante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Estudiante actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.Id))
                    {
                        return NotFound();
                    }
                    throw; // Re-throwing the exception if the student still exists.
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al actualizar el estudiante: {ex.Message}";
                }
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiante.FindAsync(id.Value);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var estudiante = await _context.Estudiante.FindAsync(id);
                if (estudiante != null)
                {
                    _context.Estudiante.Remove(estudiante);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Estudiante eliminado exitosamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el estudiante: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiante.Any(e => e.Id == id);
        }
    }
}