using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using FFXICompanion.Game;
using System.Collections.Generic;
using System.IO.Pipes;

namespace FFXICompanion.Server
{

    public class NamedPipeServer 
    {

        public void start()
        {
            Console.WriteLine("Named Pipe Server loaded");
            while(!ModuleData.getInstance().cancellationToken.IsCancellationRequested) {
                NamedPipeServerStream pipeServer =
                        new NamedPipeServerStream("testpipe", PipeDirection.In, 100, PipeTransmissionMode.Message);
                // ModuleData.getInstance().cancellationToken.Register(() => pipeServer.EndWaitForConnection);
                pipeServer.WaitForConnection();
                StreamReader reader = new StreamReader(pipeServer);
                string data = reader.ReadToEnd();
                Console.WriteLine(data);
                pipeServer.Disconnect();

                List<Character> party = JsonConvert.DeserializeObject<List<Character>>(data);

                ModuleData.getInstance().mut.WaitOne();
                foreach (Character character in party) {
                    ModuleData.getInstance().party[character.name] = character;
                }
                ModuleData.getInstance().mut.ReleaseMutex();
            }

            
        }
    }

    public class SocketServer
    {
        TcpListener listener = null;
        public SocketServer()
        {
        }

        public void start()
        {
            // IPAddress localAddr = IPAddress.Parse("localhost");
            this.listener = new TcpListener(IPAddress.Any, 11000);
            ModuleData.getInstance().cancellationToken.Register(() => this.listener.Stop());
            this.listener.Start(1000);
            Console.WriteLine("Socket Server loaded");

            try
            {
                while (!ModuleData.getInstance().cancellationToken.IsCancellationRequested)
                {
                    ModuleData.getInstance().cancellationToken.ThrowIfCancellationRequested();
                    TcpClient client = listener.AcceptTcpClient();
                    new Thread(new ThreadStart(new SocketRequestHandler(client).start)).Start();
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception) { ModuleData.getInstance().cancellationToken.ThrowIfCancellationRequested(); }
            finally
            {
                this.listener.Stop();
            }

        }

    }

    public class SocketRequestHandler
    {
        private TcpClient client;
        public SocketRequestHandler(TcpClient client)
        {
            this.client = client;
        }

        public void start()
        {
            Byte[] bytes = new Byte[256];
            String data = null;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            int i;

            // Loop to receive all the data sent by the client.
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                List<Character> party = JsonConvert.DeserializeObject<List<Character>>(data);

                ModuleData.getInstance().mut.WaitOne();
                foreach (Character character in party) {
                    ModuleData.getInstance().party[character.name] = character;
                }
                ModuleData.getInstance().mut.ReleaseMutex();
            }

            // Shutdown and end connection
            client.Close();
        }

    }

    public class HttpServer
    {
        private IWebHost httpHost;

        public HttpServer()
        {
        }
        public void start()
        {
            httpHost = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseUrls("http://+:5000")
                .UseStartup<Startup>()
                .Build();


            Console.WriteLine("Server loaded");
            httpHost.StartAsync(ModuleData.getInstance().cancellationToken);
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
            // loggerFactory.AddConsole();
            // loggerFactory.AddDebug();

            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseFileServer();
            // app.UseWebSockets();
            // app.Use(async (context, next) =>  
            // {  
            //     if (context.Request.Path == "/angular-client")  
            //     {  
            //         Console.WriteLine("Use angular client");
            //         if (context.WebSockets.IsWebSocketRequest)  
            //         {  
            //             WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();  
            //             await handleAngularClientWebSocket(webSocket);  
            //         }  
            //         else  
            //         {  
            //             context.Response.StatusCode = 400;  
            //         }  
            //     }  
            // }); 

            app.UseMvc(routes =>
            {
                // routes.MapRoute(
                //         name: "Root",
                //         template: "",
                //         physicalFile: "~/index.html"
                //     );

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
        // private async Task handleAngularClientWebSocket(WebSocket webSocket) {
        //     var buffer = new byte[1024 * 4];  
        //     WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
        //     while (!result.CloseStatus.HasValue)  
        //     {  
        //         await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);  
        //         result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
        //     }  
        //     await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        // }


        // private async Task handleFfxiClientWebSocket(WebSocket webSocket) {
        //     Console.WriteLine("FFXI Client Connected");
        //     var buffer = new byte[1024 * 4];
        //     WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
        //     while (!result.CloseStatus.HasValue)
        //     {  
        //         // await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None); 

        //         Console.WriteLine("New message received : "+ Encoding.UTF8.GetString(buffer,0,result.Count));
        //         result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
        //     }  
        //     await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        // }
    }

    [Route("/api/character")]
    [ApiController]
    public class HttpController : Controller
    {

        [HttpGet]
        public List<Character> Get()
        {
            List<Character> result = new List<Character>();
            ModuleData.getInstance().mut.WaitOne();
            foreach(Character character in ModuleData.getInstance().party.Values) {
                result.Add(character);
            }
            ModuleData.getInstance().mut.ReleaseMutex();
            return result;
        }

    }




}