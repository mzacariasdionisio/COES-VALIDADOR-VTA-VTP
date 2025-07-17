using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class EqCategoriaEquipoDTO : EntityBase
    {
        public int Ctgdetcodi { get; set; }
        public int Equicodi { get; set; }
        public string Ctgequiestado { get; set; }

        //Datos de auditoría
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioUpdate { get; set; }
        public DateTime? FechaUpdate { get; set; }

        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }

        public int Famcodi { get; set; }
        public string Famnomb { get; set; }

        public string Areacodi { get; set; }
        public string Areanomb { get; set; }

        public string Equiabrev { get; set; }
        public string Equinomb { get; set; }

        public string Ctgequiestadodescripcion { get; set; }

        public string CtgFlagExcluyente { get; set; }
        public string Ctgdetnomb { get; set; }
        public int Ctgcodi { get; set; }
        public string Ctgnomb { get; set; }
        public string Ctgpadrenomb { get; set; }

        public int? Ctgpadre { get; set; }
        public int EquicodiOld { get; set; }
        public int CtgdetcodiOld { get; set; }

        #region SIOSEIN
        public int? Grupocodi { get; set; }
        #endregion
    }
}
