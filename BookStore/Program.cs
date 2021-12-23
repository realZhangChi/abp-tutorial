using BookStore;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseAutofac();
builder.Services.AddApplication<BookStoreModule>(
    options =>
    {
        options.Services.ReplaceConfiguration(builder.Configuration);
    });
var app = builder.Build();
app.InitializeApplication();
app.Run();