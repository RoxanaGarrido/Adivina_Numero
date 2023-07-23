using Adivina_Numero1;

int min = 1, max = 100;
try
{
    if (args.Length == 2)
    {
        min = Convert.ToInt32(args[1]);
        max = Convert.ToInt32(args[2]);
    }
    var game = new Game(min, max);
    game.Init();
    Console.ResetColor();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\nUps! Algo salio mal...");
    Console.ResetColor();
    Console.WriteLine(ex.ToString());
}
