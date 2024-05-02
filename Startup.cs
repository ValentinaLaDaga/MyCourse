using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using System.IO;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastractures;

namespace MyCourse
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //registrazione del servizio di dependency a injection:
           // services.AddTransient<ICourseService, CourseService>(); 
           services.AddTransient<ICourseService, AdoNetCourseService>(); // il servizio precedente (riga 28) che implementava il servizio in maniera random viene sostituito dal nuovo servizio che leggera i dati da db
           
            //quando un controller ha nel suo costruttore un oggetto di tipo ICourseService, crea un oggetto di tipo CourseService
            services.AddTransient<IDatabaseAccessor, SqliteDatabaseAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //aggiorniamo il file per notificare al Browser Sync che deve aggiornare la pagina
                lifetime.ApplicationStarted.Register(() =>
                {
                    string filePath = Path.Combine(env.ContentRootPath,"bin/reload.txt");
                    File.WriteAllText(filePath,DateTime.Now.ToString());
                    
                });
            }

            //app.UseLiveReload();
            app.UseStaticFiles(); //css, immagini e altri file statici contenuti nella cartella wwwroot
            
            app.UseMvcWithDefaultRoute(); //IMPOSTA DI DEFAULT controller = Home / action = Index
            //se vuoi modificare la Route puoi usare il metodo app.MapRoute
        } 
    }
}