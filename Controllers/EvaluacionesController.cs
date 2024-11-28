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
    public class EvaluacionesController : Controller
    {
        private readonly SistemaEducativoWebContext _context;

        public EvaluacionesController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        // GET: Evaluacions
        public async Task<IActionResult> Index()
        {
            var sistemaEducativoWebContext = _context.Evaluacion.Include(e => e.Curso).Include(e => e.Estudiante);
            return View(await sistemaEducativoWebContext.ToListAsync());
        }

        // GET: Evaluacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacion = await _context.Evaluacion
                .Include(e => e.Curso)
                .Include(e => e.Estudiante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evaluacion == null)
            {
                return NotFound();
            }

            return View(evaluacion);
        }

        // GET: Evaluacions/Create
        public IActionResult Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre");
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos");
            return View();
        }

        // POST: Evaluacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EstudianteId,CursoId,FechaEvaluacion,Calificacion,Comentarios")] Evaluacion evaluacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evaluacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre", evaluacion.CursoId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos", evaluacion.EstudianteId);
            return View(evaluacion);
        }

        // GET: Evaluacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacion = await _context.Evaluacion.FindAsync(id);
            if (evaluacion == null)
            {
                return NotFound();
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre", evaluacion.CursoId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos", evaluacion.EstudianteId);
            return View(evaluacion);
        }

        // POST: Evaluacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EstudianteId,CursoId,FechaEvaluacion,Calificacion,Comentarios")] Evaluacion evaluacion)
        {
            if (id != evaluacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluacionExists(evaluacion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Nombre", evaluacion.CursoId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Apellidos", evaluacion.EstudianteId);
            return View(evaluacion);
        }

        // GET: Evaluacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacion = await _context.Evaluacion
                .Include(e => e.Curso)
                .Include(e => e.Estudiante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evaluacion == null)
            {
                return NotFound();
            }

            return View(evaluacion);
        }

        // POST: Evaluacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evaluacion = await _context.Evaluacion.FindAsync(id);
            if (evaluacion != null)
            {
                _context.Evaluacion.Remove(evaluacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluacionExists(int id)
        {
            return _context.Evaluacion.Any(e => e.Id == id);
        }
    }
}
