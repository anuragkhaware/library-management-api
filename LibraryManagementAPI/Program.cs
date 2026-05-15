using LibraryManagementAPI.Repositories;
using LibraryManagementAPI.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register our dependencies
builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IBookService, BookService>();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
        app.MapOpenApi();
        app.MapScalarApiReference("/library", options =>
        {
            options.WithTitle("Library Management API");
            options.Servers = new List<ScalarServer>
             {
                 new("https://localhost:7269", "Local Development")
             }; ;
            options.WithTheme(ScalarTheme.DeepSpace);
            options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();