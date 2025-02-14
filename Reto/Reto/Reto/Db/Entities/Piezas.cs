using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reto.Db.Entities
{
    public class Piezas
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }

        [ForeignKey("Taller")]
        public int TallerId { get; set; }
        public Taller Taller { get; set; }

        public List<Refaccion> Refaccion = new();
    }
}
