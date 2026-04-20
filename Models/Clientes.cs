namespace axiomtechno.Models
{
    public class Clientes
    {
        public int ClId { get; set; }
        public string ClNombre { get; set; }
        public string ClDesc { get; set; }
        public string ClUrl { get; set; }
        public long ClTelefono { get; set; }
        public string ClImagen { get; set; }
        public bool ClActivo { get; set; }
        public DateOnly ClFechaAlta { get; set; }
        public DateOnly ClFechaBaja { get; set; }
        public virtual ICollection<Pagos> Pagos { get; set; } = new List<Pagos>();
    }
}
