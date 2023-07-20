using BuildATrain.Database.Models;
using BuildATrain.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BuildATrain.Controllers
{
    public class AttributesController : Controller
    {
        private readonly IRepository<Attributes> _attributesRepository;

        public AttributesController(IRepository<Attributes> attributesRepository)
        {
            _attributesRepository = attributesRepository;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _attributesRepository.GetAllAsync();
            return View(entities);
        }
    }
}
