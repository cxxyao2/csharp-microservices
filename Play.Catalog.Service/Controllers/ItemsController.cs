using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;
using PLay.Catalog.Service;

namespace Play.Catalog.Service.Controllers
{

  // https://localhost:5001/items
  [ApiController]
  [Route("Items")]
  public class ItemsController : ControllerBase
  {

    private readonly IRepository<Item> itemsRepository;
    private static int requestCounter = 0;

    public ItemsController(IRepository<Item> itemsRepository)
    {
      this.itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
    {
      requestCounter++;
      Console.WriteLine($"Request {requestCounter}: Strating...");

      if (requestCounter <= 2)
      {
        Console.WriteLine($"Request {requestCounter}: Delay...");
        await Task.Delay(TimeSpan.FromSeconds(10));
      }


      if (requestCounter <= 4)
      {
        Console.WriteLine($"Request {requestCounter}: 500 (Internal Server Error)..");
        return StatusCode(500);
      }

      var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
      Console.WriteLine($"Request {requestCounter}: 200 (Ok).");
      return Ok(items);

    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
      var item = await itemsRepository.GetAsync(id);

      if (item == null)
      {
        return NotFound();
      }
      return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(CreateTimeDto createTimeDto)
    {
      var item = new Item
      {
        Name = createTimeDto.Name,
        Description = createTimeDto.Description,
        Price = createTimeDto.Price,
        CreatedDate = DateTimeOffset.UtcNow
      };

      await itemsRepository.CreateAsync(item);
      return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    // Put /items/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
    {
      var existingItem = await itemsRepository.GetAsync(id);

      if (existingItem == null)
      {
        return NotFound();
      }

      existingItem.Name = updateItemDto.Name;
      existingItem.Description = updateItemDto.Description;
      existingItem.Price = updateItemDto.Price;

      await itemsRepository.UpdateAsync(existingItem);

      return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {

      var item = await itemsRepository.GetAsync(id);
      if (item == null)
      {
        return NotFound();
      }

      await itemsRepository.RemoveAsync(item.Id);
      return NoContent();
    }
  }
}