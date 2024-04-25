using System;

class Buscaminas
{
    static void Main()
    {
        Console.WriteLine("\t\t---Bienvenido a Buscaminas---\n");

        //Se pregunta en primera instancia al usuario qué desea hacer, si jugar o salir del programa
        Console.WriteLine("¿Qué desea hacer?\n1. Jugar\n2. Salir");
        int opcion = Int32.Parse(Console.ReadLine());

        /*
        Si se digita un número válido, no se entra a este bucle while, pero en caso de que el valor no sea válido,
        se repite la misma pregunta una y otra vez hasta que el usuario introduzca un valor válido en la variable
        opcion
        */
        while (opcion < 1 || opcion > 2)
        {
            Console.WriteLine("Por favor ingrese una opción válida");

            Console.WriteLine("¿Qué desea hacer?\n1. Jugar\n2. Salir");
            opcion = Int32.Parse(Console.ReadLine());
        }

        //Se evalua la opción ingresada para realizar la instrucción correspondiente.
        switch (opcion)
        {
            case 1: //Si se elige la opción 1 de Jugar, entonces se prosigue a correr la función Configuración
                Configuracion();
                break;
            case 2:
                break;
        }
    }

    static void Configuracion()
    {
        //En esta función, se establecen los parámetros que se utilizarán para el tablero
        Console.Write("Por favor ingrese las dimesiones del tablero\nX: ");
        int columnas = Int32.Parse(Console.ReadLine());

        //El usuario podría hacer un tablero demasiado grande, así que se impone un límite de tamaño en ambos ejes
        while (columnas < 5 || columnas > 20)
        {
            Console.WriteLine("El máximo de cuadros en X es 20 y el mínimo es 5. Intentelo otra vez");
            Console.Write("Por favor ingrese las dimesiones del tablero\nX: ");
            columnas = Int32.Parse(Console.ReadLine());
        }

        Console.Write("Y: ");
        int filas = Int32.Parse(Console.ReadLine());

        while (filas < 5 || filas > 20)
        {
            Console.WriteLine("El máximo de cuadros en Y es 20 y el mínimo es 5. Intentelo otra vez");
            Console.Write("Y: ");
            filas = Int32.Parse(Console.ReadLine());
        }

        Console.Write("Digite el número de minas: ");
        int minas = Int32.Parse(Console.ReadLine());

        /*No pueden haber demasiadas minas en un tablero, por lo que se establece un límite según el tamaño del tablero.
        En este caso, el número de minas no puede superar la mitad de celdas en un tablero
        */
        while (minas < 9 || minas > (columnas * filas) * 0.5)
        {
            Console.WriteLine("El número de minas no puede exceder la mitad de celdas y deben haber al menos 9. Intentelo de nuevo");

            Console.Write("Digite el número de minas: ");
            minas = Int32.Parse(Console.ReadLine());
        }

        Tableros(columnas, filas, minas);
    }

    /*Se tiene una tabla "real", donde se guarda es estado de una celda como disponible u ocupada. Luego, con el número de minas que introdujo el usuario,
    se van llenando celdas aleatorias del tablero sin que se repitan, y en cada vuelta se le resta una mina hasta que el número de minas llegue a 0.
    A la tabla para mostrar se le llena de algún símbolo que oculte su estado real.
    
    Nota: Utilizar bucle for para llenar la tabla real de valores y la tabla falsa de símbolos que oculten los valores; luego, usar un bucle for para modificar celdas
    aleatorias con un valor distinto que represente las minas.
    */
    static void Tableros(int columnas, int filas, int minas)
    {
        int[,] tableroValores = new int[filas, columnas];
        char[,] tableroImp = new char[filas, columnas];
        char[,] sobreTablero = new char[filas, columnas];
        char accion;
        int x, y;

        
        //Recorremos dos matrices al mismo tiempo rellenando sus casillas con valores false para la matriz que contiene valores y \u2584 en sobreTablero, que corresponde a un caracter especial unicode
        for (int i = 0; i < filas;  i++)
        {
            for(int j = 0; j < columnas; j++)
            {
                tableroValores[i, j] = 0;
                sobreTablero[i, j] = '\u2584';
            }
        }

        Console.WriteLine("Seleccione una letra: seleccionar<s>, marcar<m>, desmarcar<d>. Seguido de dos números <x> <y>");
        accion = Char.Parse(Console.ReadLine());
        Console.Write("x: ");
        x = Int16.Parse(Console.ReadLine());
        Console.Write("y: ");
        y = Int16.Parse(Console.ReadLine());

        if (accion == 's')
        {
            tableroValores[y, x] = 2;
        }

        while (minas > 0)
        {
            //Se crea una instancia de la clase Random, a la cual llamamos rnd
            Random rnd = new Random();

            /*Se manda a llamar a la clase Random dos veces para asignar un valor dentro de lo límites
            del tablero establecidos por el usuario.

            Ambas llamadas a la clase son independientes, por lo que pueden generar números diferentes o iguales,
            lo que es deseable para que cualquier casilla del tablero pueda tener una mina
            */
            int rndx = rnd.Next(0, columnas);
            int rndy = rnd.Next(0, filas);

            if (tableroValores[rndy, rndx] == 2)
            {
                continue;
            }
            /*El usuario digita un número finito de minas; no queremos que se desperdicie ninguna de ellas.
            Es por esto que verificamos si la casilla tiene un valor 0 para colocar una mina allí, de lo contrario, no se coloca nada
            ni se resta una mina al contador porque no se han colocado.

            Es por este motivo que se utiliza un bucle while en lugar de for, porque con for, siempre restaría una mina al contador
            */
            if (tableroValores[rndy, rndx] == 0)
            {
                tableroValores[rndy,rndx] = 1;
                minas--;
            }
        }


        CalcularTableroImp(tableroValores,tableroImp,filas,columnas);
    }

    static void CalcularTableroImp(int[,] tableroValores, char[,] tableroImp, int filas, int columnas)
    {
        int cantidadMinas;

        /*Este bucle anidado recorre toda la matriz. Si encuentra celdas sin minas, entonces se detiene en ellas y realiza otro bucle anidado, el cual recorre las casillas vecinas en un radio de 1
        para verificar si hay minas en esas casillas, ya que el acumulador cantidadMinas va sumando 1 si encuentra una mina en las casillas vecinas y se reinicia a 0 cuando se termina de calcular el número correspondiente
        para esa celda y pasa a la siguiente
        */
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                cantidadMinas = 0;

                if (tableroValores[i, j] == 0)
                {
                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            //Esta condicional es importante, ya que k y l pueden salirse de las referencias dentro de la matriz, así que omite estos valores para no generar excepciones
                            if ((k > -1 && k < filas) && (l > -1 && l < columnas))
                            {
                                switch (tableroValores[k, l])
                                {
                                    case 1:
                                        cantidadMinas++;
                                        continue;
                                    default:
                                        continue;
                                }
                            }
                        }
                    }

                    //Si no se detectó mina en la casilla ni en sus casillas vecinas, se le asigna el caracter ' '
                    if (cantidadMinas == 0)
                    {
                        tableroImp[i, j] = ' ';
                    }
                    else //De otra manera, la cual solo puede ser si cantidadMinas aumentó de 0, se le asigna a la casilla el número que se haya calculado como un caracter
                    {
                        tableroImp[i, j] = cantidadMinas.ToString()[0];
                    }
                }
                else //Si se encontró mina en primer lugar, entonces no se hace nada de lo anterior y se le asigna un * que representa una mina
                {
                    tableroImp[i, j] = '*';
                }
            }
        }
    }

    static void Juego()
    {

    }

    static void ImprimirTablero(char[,] tableroImp, int filas, int columnas)
    {
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                Console.Write("[" + tableroImp[i, j] + "]");
            }
            Console.WriteLine();
        }
    }
}