namespace Api.Database
{
    public class Producto
    {
        public int Id { get; set; } // Clave primaria
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int CantidadEnStock { get; set; }
        public string Imagen { get; set; }
    }
}
