using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reto.Db.Entities
{
    public class ProcesoEnvio
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Mecanico { get; set; }
        [ForeignKey("Taller")]
        public int TallerEnviaId { get; set; }
        public Taller Taller { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string Estatus { get; set; }
        public string Imagen { get; set; }
        [ForeignKey("SolicitudPieza")]
        public int SolicitudId { get; set; }
        public SolicitudPieza SolicitudPieza { get; set; }
    }
}
