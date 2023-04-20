using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Planes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlan { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int CantidadObras { get; set; }
        public int DuracionPlanMes { get; set; }
        public string Descripcion { get; set; }
    }
}
