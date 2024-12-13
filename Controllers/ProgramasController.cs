using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEducativoWeb.Models;

namespace SistemaEducativoWeb.Controllers
{
    public class ProgramasController : Controller
    {
        private readonly SistemaEducativoWebContext _context;

        public ProgramasController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        // GET: Programas
        public async Task<IActionResult> Index()
        {
            var programas = await _context.Programa.ToListAsync();
            return View(programas);
        }

        // GET: Programas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del programa no puede ser nulo.");
            }

            var programa = await _context.Programa.FirstOrDefaultAsync(m => m.Id == id);
            if (programa == null)
            {
                return NotFound("Programa no encontrado.");
            }

            return View(programa);
        }

        // GET: Programas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Programas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Duracion,FechaInicio,FechaFin,Estado")] Programa programa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(programa);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Programa creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear el programa: {ex.Message}";
                }
            }
            return View(programa);
        }

        // GET: Programas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del programa no puede ser nulo.");
            }

            var programa = await _context.Programa.FindAsync(id);
            if (programa == null)
            {
                return NotFound("Programa no encontrado.");
            }
            return View(programa);
        }

        // POST: Programas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Duracion,FechaInicio,FechaFin,Estado")] Programa programa)
        {
            if (id != programa.Id)
            {
                return NotFound("El ID del programa no coincide.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programa);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Programa actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramaExists(programa.Id))
                    {
                        return NotFound("Programa no encontrado.");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al actualizar el programa: {ex.Message}";
                }
            }
            return View(programa);
        }

        // GET: Programas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del programa no puede ser nulo.");
            }

            var programa = await _context.Programa.FirstOrDefaultAsync(m => m.Id == id);
            if (programa == null)
            {
                return NotFound("Programa no encontrado.");
            }

            return View(programa);
        }

        // POST: Programas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programa = await _context.Programa.FindAsync(id);
            if (programa != null)
            {
                _context.Programa.Remove(programa);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Programa eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el programa: programa no encontrado.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProgramaExists(int id)
        {
            return _context.Programa.Any(e => e.Id == id);
        }

    }
}