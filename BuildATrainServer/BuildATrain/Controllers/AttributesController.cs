using BuildATrain.Database.Models;
using BuildATrain.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        public async Task<IActionResult> GetAttributesById(int id)
        {
            var attribute = await _attributesRepository.GetByIdAsync(id);

            if (attribute == null)
            {
                return NotFound();
            }

            return View(attribute);
        }
    }
}
