namespace Reto.Models
{
    public class RefaccionModel
    {
        public int Id { get; set; }
        public string NombrePieza { get; set; }
        public string NombreTaller { get; set; }
        public string Solicitud { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaInstalacion { get; set; }
        public string Estatus { get; set; }
        public string Imagen { get; set; }
    }
}
