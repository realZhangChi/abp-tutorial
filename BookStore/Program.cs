using BookStore;

using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs-.txt", rollingInterval: RollingInterval.Day))
#if DEBUG
    .WriteTo.Async(c => c.Console())
#endif
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host
        .UseAutofac()
        .UseSerilog();
    builder.Services.AddApplication<BookStoreModule>(
        options =>
        {
            options.Services.ReplaceConfiguration(builder.Configuration);
        });
    var app = builder.Build();
    app.InitializeApplication();
    await app.RunAsync();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
