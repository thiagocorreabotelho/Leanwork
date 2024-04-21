using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Leanwork.Rh.API.Extension
{
    public static class ServiceExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Configuração básica do Swagger
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebAPI",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Leanwork Group",
                        Url = new Uri("https://www.leanwork.com.br/")
                    },
                    Description = @"
                        Seja muito bem-vindo ao nosso sistema de contratação da Leanwork Group. 
                        Estamos aqui para oferecer a oportunidade de você se juntar a nós, trazendo todas as competências 
                        desejadas para a vaga. Estamos ansiosos para conhecer você e discutir como podemos colaborar juntos.
                    "
                });

                // Inclui comentários XML para documentação
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Leanwork Group API";

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");


                c.HeadContent = $@"
                <style>
                    .swagger-ui .topbar-wrapper img {{
                        content:url('https://www.leanwork.com.br/_next/image/?url=%2F_next%2Fstatic%2Fmedia%2Flogo-white.eea0e02a.png&w=256&q=100');
                        display: inline-block;
                        width: 130px; 
                        height: auto; 
                    }}
                </style>";

                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Model);
                c.DefaultModelsExpandDepth(-1);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.EnableValidator();
                c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete);
                c.EnableDeepLinking();
            });
        }
    }
}
