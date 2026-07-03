using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleProductController : ControllerBase
    {
        // Giả lập Data Store trong bộ nhớ (In-Memory)
        private static readonly List<ProductDto> Products = new()
        {
            new(1, "Laptop ASUS ROG", 1500, "Laptop gaming cấu hình khủng"),
            new(2, "Chuột Không Dây Logitech", 50, "Chuột văn phòng silent")
        };

        [HttpGet]
        [EndpointSummary("Lấy toàn bộ danh sách sản phẩm")]
        [EndpointDescription("API này sẽ trả về tất cả các sản phẩm hiện có trong hệ thống không phân trang.")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(Products);
        }

        [HttpGet("{id:int}")]
        [EndpointSummary("Lấy chi tiết một sản phẩm theo ID")]
        [EndpointDescription("Truyền ID của sản phẩm dưới dạng số nguyên để lấy thông tin chi tiết.")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { Message = $"Không tìm thấy sản phẩm với Id = {id}" });
            }
            return Ok(product);
        }

        [HttpPost]
        [EndpointSummary("Tạo mới một sản phẩm")]
        [EndpointDescription("Thêm một sản phẩm mới vào hệ thống. Dữ liệu đầu vào sẽ được tự động validate.")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] CreateProductDto input)
        {
            // LƯU Ý: Trong .NET 10, nhờ có thuộc tính [ApiController], 
            // bạn KHÔNG cần check `if (!ModelState.IsValid)` bằng tay nữa. 
            // Nếu sai định dạng, API sẽ tự động trả về 400 Bad Request kèm chi tiết lỗi.

            var newId = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
            var newProduct = new ProductDto(newId, input.Name, input.Price, input.Description);
            Products.Add(newProduct);

            // Trả về code 201 Created kèm đường dẫn gọi lại chính sản phẩm đó
            return CreatedAtAction(nameof(GetById), new { id = newId }, newProduct);
        }
    }

    #region Data Transfer Objects (DTOs)
    // Sử dụng Record giúp code ngắn gọn, bất biến (Immutable)
    public record ProductDto(int Id, string Name, decimal Price, string? Description);

    public record CreateProductDto(
        [Required(ErrorMessage = "Tên sản phẩm bắt buộc phải nhập")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên sản phẩm phải từ 3 đến 100 ký tự")]
    string Name,

        [Range(0.01, 99999, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
    decimal Price,

        string? Description
    );
    #endregion
}
