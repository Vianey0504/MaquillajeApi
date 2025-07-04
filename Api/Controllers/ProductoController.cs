using Api.Database;
using Api.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly MaquillajeDbContext _context;

        public ProductoController(MaquillajeDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, [FromForm] ProductoDto productoDto)
        {
            var productoExistente = await _context.Productos.FindAsync(id);

            if (productoExistente == null)
                return NotFound("Producto no encontrado.");

            // Si se sube una nueva imagen
            if (productoDto.Imagen != null && productoDto.Imagen.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productoDto.Imagen.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productoDto.Imagen.CopyToAsync(stream);
                }

                // Eliminar imagen anterior (opcional)
                if (!string.IsNullOrEmpty(productoExistente.Imagen))
                {
                    var rutaAnterior = Path.Combine(uploadsFolder, productoExistente.Imagen);
                    if (System.IO.File.Exists(rutaAnterior))
                        System.IO.File.Delete(rutaAnterior);
                }

                productoExistente.Imagen = uniqueFileName;
            }

            // Actualizar campos
            productoExistente.Nombre = productoDto.Nombre;
            productoExistente.Descripcion = productoDto.Descripcion;
            productoExistente.Precio = productoDto.Precio;
            productoExistente.CantidadEnStock = productoDto.CantidadEnStock;

            await _context.SaveChangesAsync();
            return Ok(productoExistente);
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}