using BuberBreakfast.Services.Breakfasts;

var builder = WebApplication.CreateBuilder(args);

{

builder.Services.AddControllers();
builder.Services.AddScoped<IBreakfastService,BreakfastService>();

}

var app = builder.Build();

{
                                    // middle ware ...
app.UseExceptionHandler("/error"); /// we dont want out client to see the error stack or logs
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

}