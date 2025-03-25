using Microsoft.AspNetCore.Mvc;
using MyWebAPI.data;
using MyWebAPI.Dto;
using MyWebAPI.Exceptions;
using MyWebAPI.Helper;
using MyWebAPI.Repositories.Interfaces;

namespace MyWebAPI.controllers
{
    [Route("item")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        private readonly IItemRepository _itemRepository;
        public ItemController(IItemRepository _itemRepository)
        {
            this._itemRepository = _itemRepository;
        }
        [HttpGet]
        public async Task<IActionResult> getAllItems([FromQuery] ItemQuerry querry)
        {
            var items = await _itemRepository.GetAllAsync(querry);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemDTO item)
        {
            await _itemRepository.AddItemAsync(item);
            if(!ModelState.IsValid)
                return BadRequest();
            return Ok(new {
                Success = "true"
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getItemsById(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemById(int id, UpdateItemDTO item)
        {
            await _itemRepository.UpdateItemAsync(id, item);

            return Ok(new {
                Success = "true"
            });
        }

        
    }
}