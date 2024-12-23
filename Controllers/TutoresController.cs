﻿using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaEducativoWeb.Models;

namespace SistemaEducativoWeb.Controllers
{
    public class TutoresController : Controller
    {
        private readonly SistemaEducativoWebContext _context;
        private readonly HttpClient _httpClient;

        public TutoresController(SistemaEducativoWebContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _httpClient = clientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7007/");
        }

        // GET: Tutores
        public async Task<IActionResult> Index()
        {
            var tutores = await _context.Tutor.ToListAsync();
            return View(tutores);
        }

        // GET: Tutores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del tutor no puede ser nulo.");
            }

            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor == null)
            {
                return NotFound("Tutor no encontrado.");
            }

            return View(tutor);
        }

        // GET: Tutores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tutores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellidos,Correo,Telefono,Especialidad,FechaRegistro")] Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    tutor.FechaRegistro = DateTime.Now;
                    _context.Add(tutor);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Tutor creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear el tutor: {ex.Message}";
                }
            }

            return View(tutor);
        }

        // GET: Tutores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del tutor no puede ser nulo.");
            }

            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor == null)
            {
                return NotFound("Tutor no encontrado.");
            }

            return View(tutor);
        }

        // POST: Tutores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellidos,Correo,Telefono,Especialidad,FechaRegistro")] Tutor tutor)
        {
            if (id != tutor.Id)
            {
                return NotFound("El ID del tutor no coincide.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tutor.FechaRegistro = DateTime.Now;
                    _context.Update(tutor);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Tutor actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorExists(tutor.Id))
                    {
                        return NotFound("Tutor no encontrado.");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al actualizar el tutor: {ex.Message}";
                }
            }

            return View(tutor);
        }

        // GET: Tutores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del tutor no puede ser nulo.");
            }

            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor == null)
            {
                return NotFound("Tutor no encontrado.");
            }

            return View(tutor);
        }

        // POST: Tutores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tutor = await _context.Tutor.FindAsync(id);
                if (tutor != null)
                {
                    _context.Tutor.Remove(tutor);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Tutor eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al eliminar el tutor: tutor no encontrado.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el tutor: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // GET: Tutores
        public async Task<IActionResult> TutoresDesdeApi()
        {
            var tutoresDB = await _context.Tutor.ToListAsync();

            List<Tutor> tutoresAPI = new();
            try
            {
                var response = await _httpClient.GetAsync("api/TutoresApi"); 
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    tutoresAPI = System.Text.Json.JsonSerializer.Deserialize<List<Tutor>>(jsonString, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener datos de la API: {ex.Message}";
            }

            var todosTutores = tutoresDB.Union(tutoresAPI).ToList();

            return View(todosTutores);
        }

        private bool TutorExists(int id)
        {
            return _context.Tutor.Any(e => e.Id == id);
        }
    }
}