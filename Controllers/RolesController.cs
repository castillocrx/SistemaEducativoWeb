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
    public class RolesController : Controller
    {
        private readonly SistemaEducativoWebContext _context;

        public RolesController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Rol.ToListAsync();
            return View(roles);
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del rol no puede ser nulo.");
            }

            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }

            return View(rol);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreRol,Descripcion")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(rol);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Rol creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear el rol: {ex.Message}";
                }
            }

            return View(rol);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del rol no puede ser nulo.");
            }

            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }

            return View(rol);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreRol,Descripcion")] Rol rol)
        {
            if (id != rol.Id)
            {
                return NotFound("El ID del rol no coincide.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rol);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Rol actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolExists(rol.Id))
                    {
                        return NotFound("Rol no encontrado.");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al actualizar el rol: {ex.Message}";
                }
            }

            return View(rol);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del rol no puede ser nulo.");
            }

            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }

            return View(rol);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rol = await _context.Rol.FindAsync(id);
            if (rol != null)
            {
                _context.Rol.Remove(rol);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rol eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el rol: rol no encontrado.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
            return _context.Rol.Any(e => e.Id == id);
        }
    }
}