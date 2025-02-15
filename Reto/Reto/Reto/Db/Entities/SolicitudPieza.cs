using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reto.Db.Entities
{
    public class SolicitudPieza
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Vin { get; set; }
        public int Cantidad { get; set; }
        [ForeignKey("Pieza")]
        public int PiezaId { get; set; }
        public Piezas Pieza { get; set; }
        public string Mecanico { get; set; }

        [ForeignKey("TallerSolicitaId")]
        public int TallerSolicitaId { get; set; }
        public Taller TallerSolicita { get; set; }

        [ForeignKey("TallerSolicitado")]
        public int TallerSolicitadoId { get; set; }
        public Taller TallerSolicitado { get; set; }
        public string EstatusSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
    }
}
