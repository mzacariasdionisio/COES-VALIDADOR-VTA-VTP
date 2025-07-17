using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_CATEGORIA
    /// </summary>
    public class EqCategoriaDTO : EntityBase
    {
        public int Ctgcodi { get; set; }
        public int? Ctgpadre { get; set; }
        public int Famcodi { get; set; }
        public string Ctgnomb { get; set; }
        public string CtgFlagExcluyente { get; set; }
        public string Ctgestado { get; set; }

        //Datos de auditoría
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioUpdate { get; set; }
        public DateTime? FechaUpdate { get; set; }

        public string Famnomb { get; set; }
        public string Ctgpadrenomb { get; set; }
        public string CtgestadoDescripcion { get; set; }
        public string CtgFlagExcluyenteDescripcion { get; set; }

        public int? TotalDetalle { get; set; }
        //inicio agregado
        public int? TotalHijo { get; set; }
        public int? TotalEquipo { get; set; }
        //fin agregado
    }
}
