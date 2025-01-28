// Clase genérica para manejar listas de números
class ListaNumeros<T>
{
    private List<T> numeros = new List<T>();

    public void AgregarNumero(T numero)
    {
        numeros.Add(numero);
    }

    public List<T> ObtenerNumeros()
    {
        return numeros;
    }
}

// Delegado para operaciones matemáticas
delegate T Operacion<T>(T a, T b);

class Program
{
    static void Main()
    {
        Console.WriteLine("Bienvenido al programa de operaciones matemáticas");

        ListaNumeros<double> lista = new ListaNumeros<double>();
        bool continuar = true;

        while (continuar)
        {
            Console.WriteLine("Escriba un número (o \"exit\" para seleccionar una operación):");
            string input = Console.ReadLine();

            if (input.ToLower() == "exit")
                break;

            try
            {
                double numero = Convert.ToDouble(input);
                lista.AgregarNumero(numero);
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Entrada no válida. Ingrese un número válido.");
            }
        }

        List<double> numeros = lista.ObtenerNumeros();
        if (numeros.Count < 2)
        {
            Console.WriteLine("Error: Se requieren al menos dos números para operar.");
            return;
        }

        Console.WriteLine("Seleccione una operación: \n\n1) Suma \n2) Resta \n3) Multiplicación \n4) División\n");
        string opcion = Console.ReadLine();

        Operacion<double> operacion = null;

        switch (opcion)
        {
            case "1":
                operacion = (a, b) => a + b;
                break;
            case "2":
                operacion = (a, b) => a - b;
                break;
            case "3":
                operacion = (a, b) => a * b;
                break;
            case "4":
                operacion = (a, b) =>
                {
                    if (b == 0)
                    {
                        Console.WriteLine("Error: No se puede dividir por cero.");
                        return a;
                    }
                    return a / b;
                };
                break;
            default:
                Console.WriteLine("Opción no válida.");
                return;
        }

        try
        {
            double resultado = numeros[0];
            for (int i = 1; i < numeros.Count; i++)
            {
                resultado = operacion(resultado, numeros[i]);
            }
            Console.WriteLine("Resultado: " + resultado);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inesperado: " + ex.Message);
        }
    }
}
