using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using MessangerContacts;
using MessangerContracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessangerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //Add for force
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //For Source control
            services.AddControllers();
            #region addServices
            services.AddTransient<ISocket, WebSocketAdapter>();
            services.AddSingleton<IMessanger, WebSocketMessangerAdapter>();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
#region usewebsockets
            app.UseWebSockets();
            #endregion
#region usefunction
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        #region MessangerInfraStructure
                        var messanger = app.ApplicationServices.GetService<IMessanger>();
                        var id = context.Request.Query["id"];
                        var webSocketAdapter = new WebSocketAdapter();
                        webSocketAdapter.Socket = webSocket;
                        await messanger.Add(id, webSocketAdapter);
                        await messanger.Send(id,new MessageBody() { Code="1234" });
                        #endregion
                        await webSocket.ReceiveAsync(new Memory<byte>(), CancellationToken.None);
                        


                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });

            #endregion


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
