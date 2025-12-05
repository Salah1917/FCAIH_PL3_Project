namespace Dictionary
#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.EntityFrameworkCore

module Program =
    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        // DbContext
        builder.Services.AddDbContext<DictionaryContext>(fun options ->
            options.UseSqlServer(
                "Server=SalahMagdy;Database=DictionaryApp;Trusted_Connection=True;TrustServerCertificate=True"
            ) |> ignore
        )

        // Controllers
        builder.Services.AddControllers() |> ignore

        // ?? Swagger Services
        builder.Services.AddEndpointsApiExplorer() |> ignore
        builder.Services.AddSwaggerGen() |> ignore

        let app = builder.Build()

        // ?? Swagger UI in Development
        if app.Environment.IsDevelopment() then
            app.UseSwagger() |> ignore
            app.UseSwaggerUI() |> ignore


        app.UseHttpsRedirection()
        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        0
