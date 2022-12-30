using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{

  // https://localhost:5001/items
  [ApiController]
  [Route("Items")]
  public class ItemsController : ControllerBase
  {
    private static readonly List<ItemDto> items = new()
    {
      new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
      new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
      new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a samll amount of damage", 20, DateTimeOffset.UtcNow)

    };

    [HttpGet]
    public IEnumerable<ItemDto> Get()
    {

      return items;

    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public ItemDto GetById(Guid id)
    {
      var item = items.Where(item => item.Id == id).SingleOrDefault();
      return item;
    }

    [HttpPost]
    public ActionResult<ItemDto> Post(CreateTimeDto createTimeDto)
    {
      var item = new ItemDto(Guid.NewGuid(), createTimeDto.Name, createTimeDto.Description, createTimeDto.Price, DateTimeOffset.UtcNow);
      items.Add(item);
      return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    // Put /items/{id}
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateItemDeto updateItemDeto)
    {
      var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

      var updatedItem = existingItem with
      {
        Name = updateItemDeto.Name,
        Description = updateItemDeto.Description,
        Price = updateItemDeto.Price
      };

      var index = items.FindIndex(existingItem => existingItem.Id == id);
      items[index] = updatedItem;

      return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
      var index = items.FindIndex(existingItem => existingItem.Id == id);
      items.RemoveAt(index);

      return NoContent();
    }
  }
}