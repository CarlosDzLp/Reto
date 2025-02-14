using System.ComponentModel.DataAnnotations;

namespace Reto.Db.Entities
{
    public class Taller
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Icon { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public List<Piezas> Piezas { get; set; } = new();
        public List<Refaccion> Refaccion { get; set; } = new();
    }
}
