using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Obras
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdObra { get; set; }

        [MaxLength(80)]
        public string Nombre { get; set; }

        [MaxLength]
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public bool Estado { get; set; }

        [MaxLength]
        public string RutaImagen { get; set; }
        public string IdUser { get; set; }
        public int IdCategoria { get; set; }

        [ForeignKey("IdUser")]
        public virtual Users User { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual Categorias Categoria { get; set; }


    }
}
