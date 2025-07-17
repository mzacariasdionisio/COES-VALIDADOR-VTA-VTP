using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_CATEGORIA_DETALLE
    /// </summary>
    public partial class EqCategoriaDetDTO : EntityBase
    {
        public int Ctgdetcodi { get; set; }
        public int Ctgcodi { get; set; }
        public string Ctgdetnomb { get; set; }
        public string Ctgdetestado { get; set; }

        //Datos de auditoría
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioUpdate { get; set; }
        public DateTime? FechaUpdate { get; set; }

        public string Famnomb { get; set; }
        public string Ctgnomb { get; set; }
        public int Ctgpadrecodi { get; set; }
        public string Ctgpadrenomb { get; set; }
        public string CtgdetestadoDescripcion { get; set; }
        //inicio agregado
        public int TotalEquipo { get; set; }
        //fin agregado
    }

    public partial class EqCategoriaDetDTO 
    {
        public decimal? Total { get; set; }
        public decimal? Porcentaje { get; set; }
        public List<int> ListaEquicodi { get; set; }
    }
}
