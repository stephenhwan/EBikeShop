using CleanArchitect.Application.DTOs.BikeShop;
using CleanArchitect.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitect.API.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]

    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _serviceCategory;

        public CategoriesController(ICategoryService serviceCategory)
        {
            //_context = context;
            _serviceCategory = serviceCategory;
        }

        [HttpPost("CreateBike")]
        public async Task<IActionResult> Create(CategoryDTO dtoCategory)
        {
            var isOK = false;
            var message = "Chưa thực thi";
            var idReturn = Guid.Empty;

            //var file = dtoBike.Image;
            //var storedFileName = await MediaUtility.SaveImage(file);

            //dtoBike.ImagePath = storedFileName;

            var bike = await _serviceCategory.Create(dtoCategory);
            if (bike != null)
            {
                idReturn = bike.Id;
            }

            return Ok(new { isOK, message, idReturn });
        }

        [HttpGet("GetAllCates")]
        public async Task<IActionResult> GetAll()
        {
            var cates = await _serviceCategory.GetAll();
            return Ok(cates);
        }

    }
}
