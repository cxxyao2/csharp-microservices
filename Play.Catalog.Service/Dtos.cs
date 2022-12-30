using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos
{
  public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

  public record CreateTimeDto([Required]string Name, string
  Description, [Range(0,10_000)] decimal Price);

  public record UpdateItemDeto([Required]string Name, string Description, [Range(0, 10_000)] decimal Price);
}