// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
for(int i = 0; i < 1000; i++)
{
    Console.WriteLine(" Looped as " + i.ToString());
    Thread.Sleep(100);
}
Console.WriteLine("Good Bye!");