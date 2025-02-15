namespace Reto.Models
{
    public class SolicitudPiezaModel
    {
        public int Id { get; set; }
        public string Vin { get; set; }
        public int Cantidad { get; set; }
        public string Pieza { get; set; }
        public string Mecanico { get; set; }
        public string TallerSolicitado { get; set; }
        public string EstatusSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
    }
}
