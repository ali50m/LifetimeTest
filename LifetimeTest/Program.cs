using Microsoft.AspNetCore.Mvc;

namespace LifetimeTest;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ICounter, Counter>();
        builder.Services.AddTransient<IFirstCounter, FirstCounter>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapGet("/test", ([FromServices] IFirstCounter firstCounter) => firstCounter.IncrementAndGet());

        app.Run();
    }
}

internal interface ICounter
{
    void Increment();
    int Get();
}

internal class Counter : ICounter
{
    private int _count;

    public void Increment()
    {
        _count++;
    }

    public int Get()
    {
        return _count;
    }
}

internal interface IFirstCounter
{
    int IncrementAndGet();
}

internal class FirstCounter : IFirstCounter
{
    private readonly ICounter _counter;

    public FirstCounter(ICounter counter)
    {
        _counter = counter;
    }

    public int IncrementAndGet()
    {
        _counter.Increment();
        return _counter.Get();
    }
}