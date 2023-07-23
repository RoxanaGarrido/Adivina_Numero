using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adivina_Numero1
{
    public class Game
    {
        public int Minimo { get; set; }
        public int _Minimo { get; set; }
        public int Maximo { get; set; }
        public int _Maximo { get; set; }
        public int Medio { get; set; }
        public List<int> Opciones { get; set; } = new();
        public Random Random { get; set; } = new();
        public int Intentos { get; set; }
        public bool Exit { get; set; }

        public Game(int min, int max)
        {
            Minimo = min; _Minimo = min;
            Maximo = max; _Maximo = max;
            Medio = (min + max) / 2;
            Intentos = 1;
            Exit = false;
        }

        public void Init()
        {
            do
            {
                Menu();
            } while (!Exit);
        }

        public void Menu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================");
            Console.WriteLine("ADIVINA NUMERO");
            Console.WriteLine("================");
            Console.ResetColor();
            Console.WriteLine("1. Empezar nueva partida");
            Console.WriteLine("2. Configuracion");
            Console.WriteLine("0. Exit");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Start();
                    break;
                case "2":
                    Configuracion();
                    break;
                case "0":
                    Console.WriteLine("Bye...");
                    Exit = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEscoge una opcion del menu");
                    Console.ResetColor();
                    break;
            }
        }

        public void RestartDefault()
        {
            Minimo = _Minimo;
            Maximo = _Maximo;
            Medio = (Minimo + Maximo) / 2;
            Intentos = 1;
            Opciones = new();
        }


        public void Configuracion()
        {
            Console.WriteLine($"\nRango actual: {_Minimo}-{_Maximo}");
            Console.WriteLine("1. Cambiar rango");
            Console.WriteLine("0. Atras");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CambiarRango();
                    break;
                case "0":
                    break;
                default:
                    break;
            }
        }

        public void CambiarRango()
        {
            bool InputValid = false;
            int[] range = new int[2];
            do
            {
                Console.WriteLine("\nIntroduce nuevo rango (ej. 50-100)");
                var input = Console.ReadLine();
                string[] numeros = input.Split("-");
                if (numeros.Length == 2 &&
                    int.TryParse(numeros[0], out range[0]) &&
                    int.TryParse(numeros[1], out range[1]))
                {
                    if (range[0] < range[1])
                    {
                        InputValid = true;
                        _Minimo = range[0];
                        _Maximo = range[1];
                        RestartDefault();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nEl primer valor debe ser menor que el segundo. (min-max)");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nIntroduce un rango valido (2 numeros separados por -)");
                    Console.ResetColor();
                }
            } while (!InputValid);

            Console.WriteLine("\nNuevo rango configurado");
            Console.WriteLine("Presiona ENTER para volver al menu...");
            Console.ReadLine();
        }

        public void Start()
        {
            bool terminar = false;
            bool adivina = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nNUEVA PARTIDA");
            Console.WriteLine("================");
            Console.ResetColor();
            Console.WriteLine($"\nEscoge un numero entre {Minimo}-{Maximo} e intentare adivinarlo");
            Console.WriteLine($"Presiona ENTER cuando estes listo...");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine($"\nGenial! Has escogido un numero entre {Minimo} y {Maximo}, ahora te hare algunas preguntas:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Escribe 'exit' para salir");
            Console.ResetColor();

            //// PARTE 1 ////

            while ((Maximo - Minimo) > 10)
            {
                var respuesta = string.Empty;
                Console.WriteLine($"\n¿Es el numero en el que estas pensando mayor que {Medio}? (s/n):");
                respuesta = Console.ReadLine().ToLower();
                if (respuesta.Equals("s"))
                {
                    EsMayor();
                }
                else if (respuesta.Equals("n"))
                {
                    EsMenor();
                }
                else if (respuesta.Equals("exit"))
                {
                    terminar = true;
                    break;
                }
                else
                {
                    ErrorInput();
                }
            }

            //// PARTE 2 ////
            //En este punto quedan menos de 10 opciones para adivinar
            if (!terminar)
            {
                Console.WriteLine($"\nVale! Tu numero esta entre {Minimo} - {Maximo}");
                for (var i = Minimo; i <= Maximo; i++) { Opciones.Add(i); }
                var input = string.Empty;
                do
                {
                    var XNumber = Opciones[GetRandomIndex()];
                    Console.WriteLine($"\n¿Es tu numero {XNumber}? (s/n):");
                    input = Console.ReadLine().ToLower();
                    if (input.Equals("s"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nHe adivinado tu numero en {Intentos} intento(s). Gracias por jugar!");
                        adivina = true;
                        Opciones = new();
                    }
                    else if (input.Equals("n"))
                    {
                        Opciones.Remove(XNumber);
                        Intentos++;
                    }
                    else if (input.Equals("exit"))
                    {
                        terminar = true;
                        break;
                    }
                    else
                    {
                        ErrorInput();
                    }
                } while (Opciones.Count > 0);
            }
            if (!adivina && !terminar)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSe han agotado las opciones!");
            }

            EndGame();
        }

        public void EndGame()
        {
            Console.ResetColor();
            Console.WriteLine("\nFin de partida");
            Console.WriteLine("Presiona ENTER para volver al menu...");
            RestartDefault();
            Console.ReadLine();
        }

        public void ErrorInput()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nPor favor escribe: 's' para Si, 'n' para No o 'exit' para Salir.");
            Console.ResetColor();
        }

        public void EsMayor()
        {
            Minimo = Medio + 1;
            Medio = (Minimo + Maximo) / 2;
        }

        public void EsMenor()
        {
            Maximo = Medio;
            Medio = (Minimo + Maximo) / 2;
        }

        public int GetRandomIndex()
        {
            return (int)Random.Next(0, Opciones.Count);
        }
    }
}
