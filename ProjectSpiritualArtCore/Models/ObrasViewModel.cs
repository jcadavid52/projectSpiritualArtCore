using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSpiritualArtCore.Models
{
    public class ObrasViewModel
    {
        public int IdObra { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public bool Estado { get; set; }
        public string RutaImagen { get; set; }
        public string IdUser { get; set; }
        public int IdCategoria { get; set; }
    }
}
