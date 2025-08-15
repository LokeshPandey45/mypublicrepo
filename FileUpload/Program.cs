var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Intentional issue: Duplicate service registration
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
// Intentional issue: Duplicate static files middleware
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = context =>
    {
        context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
        context.Context.Response.Headers.Add("Expires", "-1");
    }
});
app.UseStaticFiles();
app.UseStaticFiles(); // Intentional duplicate

app.UseRouting();

// Intentional issue: Authorization middleware before authentication
app.UseAuthorization();

app.MapRazorPages();

app.Run();

// Intentional issue: Unreachable code
Console.WriteLine("This line will never be executed.");
