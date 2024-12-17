using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEducativoWeb.Models;

namespace SistemaEducativoWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutoresApiController : ControllerBase
    {
        private readonly SistemaEducativoWebContext _context;

        public TutoresApiController(SistemaEducativoWebContext context)
        {
            _context = context;
        }

        // GET: api/TutoresApi
        [HttpGet]
        public async Task<IActionResult> GetTutores()
        {
            var tutores = await _context.Tutor.ToListAsync();
            return Ok(tutores);
        }

        // GET: api/TutoresApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTutor(int id)
        {
            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }
            return Ok(tutor);
        }
    }
}
