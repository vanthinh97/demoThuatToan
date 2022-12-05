// See https://aka.ms/new-console-template for more information
using demoCalculateTimeElementAwait;

//Console.WriteLine("Hello, World!");



var watch = new System.Diagnostics.Stopwatch();

watch.Start();

var moto = new Moto();

var a0 = await moto.AccessTheWebAsync();
var a1 = await moto.AccessTheWebAsync1();
var a2 = await moto.AccessTheWebAsync2();
var a3 = await moto.AccessTheWebAsync3();
//await Task.WhenAll(a0, a1, a2, a3);

//int b0 = await a0;
//int b1 = await a1;
//int b2 = await a2;
//int b3 = await a3;

int b0 = a0;
int b1 = a1;
int b2 = a2;
int b3 = a3;

watch.Stop();

Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
