using System;
using System.Threading.Tasks;
using BManagerAPi.Entities;
using BManagerAPi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BManagerAPi.Controllers{
        [ApiController]
        [Route("order")]
        public class OrderController : ControllerBase{
            private readonly UserOrderService _orderService;
            private readonly BookService _bookService;
            public OrderController(UserOrderService userService,BookService bookService)
            {
                _orderService = userService;
                _bookService = bookService;
            }
            [HttpGet("{id}")]
            public async Task<IActionResult> GetUserOrder([FromRoute] string id)
            {
                try
                {
                    var order = await _orderService.GetUserOrderAsync(id);
                    return Ok(order);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] string id, [FromBody]List<BookOrder> order)
        {
            try
            {
                await _orderService.UpdateUserOrderAsync(id,BookOrder.ToJsonString(order));
                Console.WriteLine(BookOrder.ToJsonString(order));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] string id)
        {
            try
            {
                await _orderService.DeleteUserOrderAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        }

    public class BookOrder
    {
        public string bookId { get; set; }
        public string ownerId { get; set; }
        public string name { get; set; }
        public static string ToJsonString(List<BookOrder> orders)
        {
            var orderDictionary = new Dictionary<int, BookOrder>();

            for (int i = 0; i < orders.Count; i++)
            {
                orderDictionary.Add(i, orders[i]);
            }

            return JsonConvert.SerializeObject(orderDictionary, Formatting.Indented);
        }
    }
    }
    

