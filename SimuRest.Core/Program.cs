// See https://aka.ms/new-console-template for more information

using SimuRest.Core.Models;
using SimuRest.Core.Services;
using SimuRest.Core.Services.Builders;
using SimuRest.Core.Services.Http;


var builder = new SimuServerBuilder()
    .Port(5000)
    .Setup(HttpMethod.Get, "/foo")
    .Responds(req => new SimuResponse(200, "test success"))
    .Delay(0)
    .Apply();


var server = builder.Server;
var serverTask = server.Start();

// optional: testen
using var client = new HttpClient();
var response = await client.GetAsync("http://localhost:5000/foo");
Console.WriteLine(await response.Content.ReadAsStringAsync());

// Stop server
await server.Stop();