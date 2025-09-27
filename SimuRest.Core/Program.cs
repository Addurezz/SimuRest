// See https://aka.ms/new-console-template for more information

using System.Text;
using SimuRest.Core.Models;
using SimuRest.Core.Services;
using SimuRest.Core.Services.Builders;
using SimuRest.Core.Services.Http;


var builder = new SimuServerBuilder()
    .Port(5000)
    .Setup(HttpMethod.Post, "/foo")
    .SaveInMemory()
    .Apply()
    .Setup(HttpMethod.Get, "/foo")
    .RespondFromMemory()
    .Apply();


var server = builder.Server;
var serverTask = server.Start();

var client = new HttpClient();
var content = new StringContent("{\"hello\":\"world\"}", Encoding.UTF8, "text/plain");

var response = await client.PostAsync("http://localhost:5000/foo", content);

var body = await response.Content.ReadAsStringAsync();
var b = await client.GetAsync("http://localhost:5000/foo");
Console.WriteLine(body);
Console.WriteLine(await b.Content.ReadAsStringAsync());
// Stop server
await server.Stop();