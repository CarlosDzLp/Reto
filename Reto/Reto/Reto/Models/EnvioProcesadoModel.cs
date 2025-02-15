namespace Reto.Models
{
    public class EnvioProcesadoModel
    {
        public string Solicitud { get; set; }
        public string Mecanico { get; set; }
        public DateTime FechaEnvio { get; set; }
        public byte[] Imagen { get; set; }
        public int Id { get; set; }
        public string Estatus { get; set; }
        public string ImagenPath { get; set; }
        public int SolicitudId { get; set; }
    }
}
