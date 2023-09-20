using AspCoreApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AspCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(DataContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("Events")]
        public async Task<ActionResult<List<Event>>> Get()
        {
            try
            {
                return Ok(await _context.Events.ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                return Ok("hata");
            }

        }


        [HttpPost("AddEvent")]
        public async Task<ActionResult<List<Event>>> AddEvent([FromForm] EventInputModel EventInputModel)
        {
            try
            {
                if (EventInputModel.Photo != null && EventInputModel.Photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + EventInputModel.Photo.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await EventInputModel.Photo.CopyToAsync(fileStream);
                    }


                    var newEvent = new Event
                    {
                        Title = EventInputModel.Title,
                        Description = EventInputModel.Description,
                        Location = EventInputModel.Location,
                        Time = EventInputModel.Time,
                        Photo = uniqueFileName
                    };

                    _context.Events.Add(newEvent);
                    await _context.SaveChangesAsync();

                    return Ok(await _context.Events.ToListAsync());
                }
                else
                {
                    return BadRequest("Geçersiz fotoğraf.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                return StatusCode(500, "Dosya yükleme hatası: " + ex.Message);
            }
        }

    }

}
