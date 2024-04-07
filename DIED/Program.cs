using SharpDX.DirectInput;
using System.Diagnostics;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, args) =>
{
    cts.Cancel();
    args.Cancel = true;
};

long enumerationCount = 0;
using var di = new DirectInput();

Console.WriteLine("This program will continuously poll DirectInput for supported devices.");
Console.WriteLine("It will report any enumeration that takes more than 1 second.");
Console.WriteLine();
Console.WriteLine("You can use this to see if any of your devices misbehave when connected.");
Console.WriteLine("Press CTRL+C to exit...");
Console.WriteLine();

while (!cts.Token.IsCancellationRequested)
{
    Console.Write($"\rRunning enumeration {++enumerationCount}...");
    var stopwatch = Stopwatch.StartNew();

    _ = di.GetDevices();

    stopwatch.Stop();
    if (stopwatch.ElapsedMilliseconds > 1000)
        Console.WriteLine($"\rTook {stopwatch.ElapsedMilliseconds} ms to enumerate devices.");

    try
    {
        await Task.Delay(100, cts.Token);
    }
    catch (TaskCanceledException) { }
}