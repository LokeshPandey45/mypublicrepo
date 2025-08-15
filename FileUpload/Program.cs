var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = context =>
    {
        context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
        context.Context.Response.Headers.Add("Expires", "-1");
    }
});
//app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
