internal class Program
{
    static void Main(string[] args)
    {
        var demoDiction = new Dictionary<string, string>();
        demoDiction["a1"] = "a---1";
        demoDiction["a2"] = "a2";
        demoDiction["a3"] = "a----3";

        foreach (var item in demoDiction)
        {
            Console.WriteLine(item + "\n");
        }


        Console.ReadLine();


    }

}