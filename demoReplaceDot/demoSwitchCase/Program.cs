// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var a = 100;
var amount = 2;
switch (amount)
{
    case 0:
        a = 0;
        break;

    case 1:
        a = 1;
        break;

    case var _ when amount > 1:
        a = 2;
        break;

    default:
        break;
}

Console.WriteLine(a);
