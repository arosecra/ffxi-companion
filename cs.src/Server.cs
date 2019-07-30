using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Net.WebSockets; 
using Microsoft.AspNetCore.Mvc;
using System.Threading;  
using System.Threading.Tasks;
using System.Text;

namespace FFXICompanion.Server
{

    public class HttpServer
    {
        private IWebHost httpHost;
        private ModuleData moduleData;

        public HttpServer(ModuleData moduleData) {
            this.moduleData = moduleData;
        }
        public void start()
        {
            httpHost = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseUrls("http://+:5000")
                .UseStartup<Startup>()
                .Build();
 

            httpHost.StartAsync(moduleData.cancellationToken);
            Console.WriteLine("Server loaded");
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();
        }
 
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseFileServer();
            app.UseWebSockets();
            app.Use(async (context, next) =>  
            {  
                if (context.Request.Path == "/angular-client")  
                {  
                    Console.WriteLine("Use angular client");
                    if (context.WebSockets.IsWebSocketRequest)  
                    {  
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();  
                        await handleAngularClientWebSocket(webSocket);  
                    }  
                    else  
                    {  
                        context.Response.StatusCode = 400;  
                    }  
                }  
                else if (context.Request.Path == "/ffxi-client")  
                {  
                    Console.WriteLine("Use ffxi client");
                    if (context.WebSockets.IsWebSocketRequest)  
                    {  
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();  
                        await handleFfxiClientWebSocket(webSocket);  
                    }  
                    else  
                    {  
                        context.Response.StatusCode = 400;  
                    }  
                }  
                // else  
                // {  
                //     Console.WriteLine("Use other client");
                //     await next();  
                // }  
            }); 
 
            // app.UseMvc(routes =>
            // {
            //     // routes.MapRoute(
            //     //         name: "Root",
            //     //         template: "",
            //     //         physicalFile: "~/index.html"
            //     //     );

            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller}/{action}/{id?}");
            // });
        }
        private async Task handleAngularClientWebSocket(WebSocket webSocket) {
            var buffer = new byte[1024 * 4];  
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
            while (!result.CloseStatus.HasValue)  
            {  
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);  
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
            }  
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        
        private async Task handleFfxiClientWebSocket(WebSocket webSocket) {
            Console.WriteLine("FFXI Client Connected");
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
            while (!result.CloseStatus.HasValue)
            {  
                // await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None); 

                Console.WriteLine("New message received : "+ Encoding.UTF8.GetString(buffer,0,result.Count));
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
            }  
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }




}