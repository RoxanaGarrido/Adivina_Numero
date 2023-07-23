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
            Console.WriteLine("ADIVINA NÚMERO");
            Console.WriteLine("================");
            Console.ResetColor();
            Console.WriteLine("1. Empezar nueva partida");
            Console.WriteLine("2. Configuración");
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
                    Console.WriteLine("\nEscoge una opción del menú");
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
            Console.WriteLine("0. Atrás");
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
                    Console.WriteLine("\nIntroduce un rango válido (2 números separados por -)");
                    Console.ResetColor();
                }
            } while (!InputValid);

            Console.WriteLine("\nNuevo rango configurado");
            Console.WriteLine("Presiona ENTER para volver al menú...");
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
            Console.WriteLine($"\nEscoge un número entre {Minimo}-{Maximo} e intentaré adivinarlo");
            Console.WriteLine($"Presiona ENTER cuando estés listo...");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine($"\nGenial! Has escogido un número entre {Minimo} y {Maximo}, ahora te haré algunas preguntas:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Escribe 'exit' para salir");
            Console.ResetColor();

            //// PARTE 1 ////

            while ((Maximo - Minimo) > 10)
            {
                var respuesta = string.Empty;
                Console.WriteLine($"\n¿Es el número en el que estás pensando mayor que {Medio}? (s/n):");
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
                Console.WriteLine($"\nVale! Tu número está entre {Minimo} - {Maximo}");
                for (var i = Minimo; i <= Maximo; i++) { Opciones.Add(i); }
                var input = string.Empty;
                do
                {
                    var XNumber = Opciones[GetRandomIndex()];
                    Console.WriteLine($"\n¿Es tu número {XNumber}? (s/n):");
                    input = Console.ReadLine().ToLower();
                    if (input.Equals("s"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nHe adivinado tu número en {Intentos} intento(s). Gracias por jugar!");
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
            Console.WriteLine("Presiona ENTER para volver al menú...");
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
