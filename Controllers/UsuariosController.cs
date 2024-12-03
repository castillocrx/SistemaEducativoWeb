using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEducativoWeb.Models;

namespace SistemaEducativoWeb.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly SistemaEducativoWebContext _context;

        public UsuariosController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        
        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var sistemaEducativoWebContext = _context.Usuario.Include(u => u.Rol);
            return View(await sistemaEducativoWebContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["RolId"] = new SelectList(_context.Rol, "Id", "NombreRol");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreUsuario,Contraseña,RolId,FechaCreacion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaCreacion = DateTime.Now;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RolId"] = new SelectList(_context.Rol, "Id", "NombreRol", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["RolId"] = new SelectList(_context.Rol, "Id", "NombreRol", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,Contraseña,RolId,FechaCreacion")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.FechaCreacion = DateTime.Now;
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["RolId"] = new SelectList(_context.Rol, "Id", "NombreRol", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string nombreUsuario, string contraseña)
        {
            var usuario = _context.Usuario
                .Include(u => u.Rol) 
                .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Contraseña == contraseña);

            if (usuario == null)
            {
                return RedirectToAction("Create");
            }

         
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol.NombreRol) 
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true 
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Perfil");
        }

        public IActionResult Perfil()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                return RedirectToAction("Login");
            }

            var usuario = _context.Usuario.Include(u => u.Rol) .FirstOrDefault(u => u.Id.ToString() == usuarioId);

            if (usuario == null)
            {
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        public IActionResult EditarPerfil()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                return RedirectToAction("Login");
            }

            var usuario = _context.Usuario.FirstOrDefault(u => u.Id.ToString() == usuarioId);

            if (usuario == null)
            {
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarPerfil(Usuario usuarioEditado)
        {
            if (!ModelState.IsValid)
            {
                return View(usuarioEditado);
            }

            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuario = _context.Usuario.FirstOrDefault(u => u.Id.ToString() == usuarioId);

            if (usuario == null)
            {
                return RedirectToAction("Login");
            }

            usuario.NombreUsuario = usuarioEditado.NombreUsuario;
            usuario.Contraseña = usuarioEditado.Contraseña;

            _context.Update(usuario);
            _context.SaveChanges();

            return RedirectToAction("Perfil");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


    }
}
