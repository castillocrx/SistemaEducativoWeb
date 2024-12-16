using Microsoft.AspNetCore.Mvc;
using SistemaEducativoWeb.Models;

namespace SistemaEducativoWeb.Controllers
{
    public class ReportesController : Controller
    {
        private readonly SistemaEducativoWebContext _context;

        public ReportesController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ReporteCursos()
        {
            // Ejecuta el procedimiento almacenado
            var datos = await _context.GetCantidadEstudiantesCursosAsync();
            return View(datos);
        }
    }
}
