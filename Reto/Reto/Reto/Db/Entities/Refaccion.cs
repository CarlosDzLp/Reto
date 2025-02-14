using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reto.Db.Entities
{
    public class Refaccion
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Solicitud { get; set; }

        [ForeignKey("Piezas")]
        public int IdPieza { get; set; }

        public Piezas Piezas { get; set; }

        [ForeignKey("Taller")]
        public int IdTaller { get; set; }

        public Taller Taller { get; set; }

        public DateTime FechaInstalacion { get; set; }

        public string Estatus { get; set; }

        public string Imagen { get; set; }
        public int Cantidad { get; set; }
    }
}
