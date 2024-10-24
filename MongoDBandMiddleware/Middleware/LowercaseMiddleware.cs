using Entities;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace basicMongoDBandMiddleware.Middleware
{
    public class LowercaseMiddleware
    {
        private readonly RequestDelegate _next;

        public LowercaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post && context.Request.Path == "/user")
            {
                var originBody = context.Response.Body;
                try
                {
                    var memStream = new MemoryStream();
                    context.Response.Body = memStream;

                    await _next(context).ConfigureAwait(false);

                    memStream.Position = 0;
                    var responseBody = new StreamReader(memStream).ReadToEnd();
                    Console.WriteLine("Response Body: " + responseBody); // Debug log

                    // responseBody = char.ToLower(responseBody[0]) + responseBody[1..];
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Use camel case to match the JSON
                    };
                    var person = JsonSerializer.Deserialize<Person>(responseBody, options);

                    person.Name = char.ToLower(person.Name[0]) + person.Name[1..];

                    // var person = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody);
                    // person["name"] = char.ToLower(person["name"][0]) + person["name"][1..];
                    Console.WriteLine("Person type : " + person.GetType());
                    Console.WriteLine("Dictionary Object : " + person);

                    // Serialize the updated person object back to JSON
                    var modifiedBody = JsonSerializer.Serialize(person);

                    Console.WriteLine("modifiedBody type : " + modifiedBody.GetType());
                    Console.WriteLine("modifiedBody Object: " + modifiedBody);
                    var memoryStreamModified = new MemoryStream();
                    var sw = new StreamWriter(memoryStreamModified);
                    sw.Write(modifiedBody);
                    sw.Flush();
                    memoryStreamModified.Position = 0;

                    await memoryStreamModified.CopyToAsync(originBody);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error is: {e.Message}");
                }
                finally
                {
                    Console.WriteLine("ddddd");
                    context.Response.Body = originBody;
                }
            }
            else await _next(context).ConfigureAwait(false);
        }
    }

}