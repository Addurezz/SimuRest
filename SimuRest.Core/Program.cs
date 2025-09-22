// See https://aka.ms/new-console-template for more information

using SimuRest.Core.Models;
using SimuRest.Core.Services;
using SimuRest.Core.Services.Builders;
using SimuRest.Core.Services.Http;


var builder = new SimuServerBuilder()
    .Setup(HttpMethod.Get, "/foo")
    .Responds(req => new SimuResponse(200, "test success"))
    .End();
    
SimuServer server = builder.Server;
var serverTask = Task.Run(() => server.Start());
Console.ReadLine();
server.Stop();