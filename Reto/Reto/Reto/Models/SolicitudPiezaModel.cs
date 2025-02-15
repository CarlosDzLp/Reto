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
        public string TallerSolicita { get; set; }
        public double TSolicitadoLatitud { get; set; }
        public double TSolicitadoLongitud { get; set; }
        public double TSolicitaLatitud { get; set; }
        public double TSolicitaLongitud { get; set; }
        public int IdPieza { get; set; }
        public bool IsVisibleBtn { get; set; }
    }
}
