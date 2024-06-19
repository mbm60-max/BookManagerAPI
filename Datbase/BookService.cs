using Supabase;
using System;
using System.Threading.Tasks;
using BManagerAPi.Entities;
using System.Collections.Generic;
using System.Linq;
using BManagerAPi.Repositories;
using Newtonsoft.Json;

public class BookService
{
    private readonly Supabase.Client _supabase;
    private readonly UserOrderService _orderService;
    public BookService(Supabase.Client supabase, UserOrderService orderService)
    {
        _supabase = supabase;
         _orderService = orderService;
    }

public async Task CreateBookAsync(Book book)
{
    // Generate a new Guid for the book (and note)
    try
    {
        // Create a note with the same ID as the book
         var newId = Guid.NewGuid();
        book.Id = newId;
        book.NotesId = newId;
         book.CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        await UpdateBookOrderAsync(book);
        // Create a note with the same ID as the book
        var note = new Note(newId);

        // Insert the note record
        var noteResponse = await _supabase.From<Note>().Insert(note);

        // Check if the note insertion was successful
        if (noteResponse == null || noteResponse.Models.Count == 0)
        {
            throw new Exception("Failed to insert the note into the database.");
        }

        // Insert the book record
        var response = await _supabase.From<Book>().Insert(book);

        // Check if the book insertion was successful
        if (response == null || response.Models.Count == 0)
        {
            // If the book insertion fails, delete the note record
            //await DeleteNoteAsync(book.Id);

            throw new Exception("Failed to insert the book into the database.");
        }
    }
    catch (Exception ex)
    {
        // Handle any exceptions
        Console.WriteLine("Error creating book: " + ex.Message);
        throw;
    }
}





    public async Task<Book> GetBookAsync(Guid id)
{
    var response = await _supabase
        .From<Book>()
        .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id.ToString())
        .Single();

    if (response == null)
    {
        throw new Exception("Book not found.");
    }
    return response;
}

    public async Task<IEnumerable<Book>> GetBooksAsync(string OwnerId)
{
        var response = await _supabase.From<Book>().Where(x => x.OwnerId == OwnerId).Get();
        if (response == null || !response.Models.Any())
        {
            throw new Exception("No books found.");
        }
        return response.Models;
    }

    public async Task UpdateBookAsync(Guid id, Book book)
{
    var update = await _supabase
    .From<Book>()
    .Where(x => x.Id == id)
    .Set(x => x.Name, book.Name)
    .Set(x => x.Author, book.Author)
    .Set(x => x.TotalPages, book.TotalPages)
    .Set(x => x.PagesRead,book.PagesRead)
    .Set(x => x.ImageRef,book.ImageRef)
    .Update();
    if (update == null)
    {
        throw new Exception("Failed to update book.");
    }
}


    public async Task DeleteBookAsync(Guid id)
    {
        await _supabase.From<Book>().Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id.ToString()).Delete();
    }
    
    public async Task UpdateBookOrderAsync(Book book)
{
   
    var existingOrder = await _orderService.GetUserOrderAsync(book.OwnerId);
    
    
    if (existingOrder == null)
    {
        throw new Exception("No existing order found for the user.");
    }
    if (string.IsNullOrEmpty(existingOrder.OrderJsonAsString))
    {
        throw new Exception(" the order data is invalid.");
    }
    // Parse the existing order into a dictionary
    var orderData = JsonConvert.DeserializeObject<Dictionary<int, OrderItem>>(existingOrder.OrderJsonAsString);
    if (orderData == null)
    {
        orderData = new Dictionary<int, OrderItem>();
    }

    if (orderData.Count == 0)
    {
        // If the order data is empty, insert the book data as the first item
        orderData[0] = new OrderItem { bookId = book.Id, ownerId = book.OwnerId, name = book.Name };
    }
    else
    {
        // Otherwise, insert the new book data at the end of the order
        int newKey = orderData.Keys.Max() + 1;
        orderData[newKey] = new OrderItem { bookId = book.Id, ownerId = book.OwnerId, name = book.Name };
    }
    
    // Convert the updated dictionary back to JSON
    var updatedOrder = JsonConvert.SerializeObject(orderData);
    
    // Update the user order with the new data
    await _orderService.UpdateUserOrderAsync(book.OwnerId, updatedOrder);
}

public class OrderItem
{
    public Guid bookId { get; set; }
    public string ownerId { get; set; }
    public string name { get; set; }
}

}

