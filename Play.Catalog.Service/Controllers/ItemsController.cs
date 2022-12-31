using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;
using PLay.Catalog.Service;

namespace Play.Catalog.Service.Controllers
{

  // https://localhost:5001/items
  [ApiController]
  [Route("Items")]
  public class ItemsController : ControllerBase
  {

    private readonly IItemsRepository itemsRepository;

    public ItemsController(IItemsRepository itemsRepository)
    {
      this.itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAsync()
    {
      var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
      return items;

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
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDeto updateItemDeto)
    {
      var existingItem = await itemsRepository.GetAsync(id);

      if (existingItem == null)
      {
        return NotFound();
      }

      existingItem.Name = updateItemDeto.Name;
      existingItem.Description = updateItemDeto.Description;
      existingItem.Price = updateItemDeto.Price;

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