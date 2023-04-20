using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Users:IdentityUser
    {
        [MaxLength(50)]
        public string Nombre { get; set; }
        [MaxLength(50)]
        public string Apellido { get; set; }
        [MaxLength(30)]
        public string Pais { get; set; }
        [MaxLength(30)]
        public string Ciudad { get; set; }
        [MaxLength(50)]
        public string Localidad { get; set; }
        [MaxLength]
        public string Direccion { get; set; }
        [MaxLength]
        public string ImagenPerfil { get; set; }
        public int IdPlan { get; set; }

        [ForeignKey("IdPlan")]
        public virtual Planes Planes { get; set; }

    }
}
