using System.Text;
using System.Text.Json;
using Entities;

namespace basicMongoDBandMiddleware.Middleware
{
    public class CapitalizeMiddleware
    {
        private readonly RequestDelegate _next;

        public CapitalizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post && context.Request.Path == "/user")
            {
                // Enable request body buffering to allow multiple reads
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset stream position

                if (!string.IsNullOrEmpty(body))
                {
                    // Deserialize the JSON into a Person object
                    var person = JsonSerializer.Deserialize<Person>(body);

                    if (person != null && !string.IsNullOrEmpty(person.Name))
                    {
                        // Capitalize the first letter of the 'Name'
                        person.Name = char.ToUpper(person.Name[0]) + person.Name[1..];

                        // Serialize the modified object back to JSON
                        var updatedBody = JsonSerializer.Serialize(person);

                        // Update the request body
                        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(updatedBody));
                        context.Request.ContentLength = updatedBody.Length;

                        // Print the updated person object
                        Console.WriteLine($"Updated Name: {person.Name}");
                    }
                }
            }
            Console.WriteLine("UpperCase Middleware Start");
            await _next(context);
            Console.WriteLine("UpperCase Middleware End");
        }
    }
}