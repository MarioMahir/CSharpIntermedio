// Aplicación de los principios SOLID con soluciones

// Principio de Responsabilidad Única (SRP)

// Clase que representa el pedido (solo se encarga de la gestión)
public class Pedido
{
    public int Id { get; set; }
    public string Cliente { get; set; }

    public Pedido(int id, string cliente)
    {
        Id = id;
        Cliente = cliente;
    }
}

// Clase separada para manejar el envío
public class EnvioPedido
{
    public void EnviarPedido(Pedido pedido)
    {
        Console.WriteLine($"Enviando el pedido {pedido.Id} al cliente {pedido.Cliente}");
    }
}

// Clase separada para la generación de facturas
public class Factura
{
    public void GenerarFactura(Pedido pedido)
    {
        Console.WriteLine($"Generando factura para el pedido {pedido.Id}");
    }
}

// Principio de Abierto/Cerrado (OCP)

// Interfaz para los descuentos
public interface IDescuento
{
    decimal AplicarDescuento(decimal precio);
}

// Aquí se implementan cosas específicas para todos los tipos de clientes
public class DescuentoClienteRegular : IDescuento
{
    public decimal AplicarDescuento(decimal precio) => precio * 0.90m; // 10% de descuento
}

public class DescuentoClienteVIP : IDescuento
{
    public decimal AplicarDescuento(decimal precio) => precio * 0.80m; // 20% de descuento
}

// Clase que calcula el descuento sin necesidad de modificarla en el futuro
public class CalculadoraDeDescuentos
{
    public decimal CalcularPrecioFinal(decimal precio, IDescuento descuento)
    {
        return descuento.AplicarDescuento(precio);
    }
}

// Principio de Sustitución de Liskov (LSP - Liskov Substitution Principle)

public abstract class Vehiculo
{
    public abstract void Arrancar();
}

public class Coche : Vehiculo
{
    public override void Arrancar()
    {
        Console.WriteLine("El coche ha arrancado.");
    }
}

public class Bicicleta : Vehiculo
{
    public override void Arrancar()
    {
        Console.WriteLine("La bicicleta no arranca, solo se pedalea.");
    }
}

// Principio de Segregación de Interfaces (ISP - Interface Segregation Principle)

// Interfaces pequeñas en lugar de una única grande
public interface ITrabajo
{
    void RealizarTrabajo();
}

public interface IReporte
{
    void GenerarReporte();
}

// Clases que implementan solo las interfaces necesarias
public class Programador : ITrabajo, IReporte
{
    public void RealizarTrabajo() => Console.WriteLine("Escribiendo código...");
    public void GenerarReporte() => Console.WriteLine("Generando reporte de progreso...");
}

public class Mantenimiento : ITrabajo
{
    public void RealizarTrabajo() => Console.WriteLine("Realizando mantenimiento de la oficina...");
}

// Principio de Inversión de Dependencias (DIP - Dependency Inversion Principle)

// Definimos una interfaz para enviar correos
public interface ICorreoServicio
{
    void EnviarCorreo(string destinatario, string mensaje);
}

// Implementación concreta del servicio de correo
public class SmtpCorreoServicio : ICorreoServicio
{
    public void EnviarCorreo(string destinatario, string mensaje)
    {
        Console.WriteLine($"Enviando correo a {destinatario}: {mensaje}");
    }
}

// Clase que depende de la abstracción en lugar de la implementación concreta
public class Notificacion
{
    private readonly ICorreoServicio _correoServicio;

    public Notificacion(ICorreoServicio correoServicio)
    {
        _correoServicio = correoServicio;
    }

    public void Enviar(string destinatario, string mensaje)
    {
        _correoServicio.EnviarCorreo(destinatario, mensaje);
    }
}
