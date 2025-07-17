using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_INTERRUPCION
    /// </summary>
    [Serializable]
    public class EveInterrupcionDTO : EntityBase
    {
        public decimal? InterrmwDe { get; set; } 
        public decimal? InterrmwA { get; set; } 
        public decimal? Interrminu { get; set; } 
        public decimal? Interrmw { get; set; } 
        public string Interrdesc { get; set; } 
        public int Interrupcodi { get; set; } 
        public int? Ptointerrcodi { get; set; } 
        public int? Evencodi { get; set; } 
        public string Interrnivel { get; set; } 
        public string Interrracmf { get; set; } 
        public int? Interrmfetapa { get; set; } 
        public string Interrmanualr { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; }
        public string PtoInterrupNomb { get; set; }
        public string PtoEntreNomb { get; set; }
        public string AreaNomb { get; set; }
        public string EquiAbrev { get; set; }
        public decimal EquiTension { get; set; }
        public string EmprNomb { get; set; }


        #region SIOSEIN
        
        public decimal? Bajomw { get; set; }
        public decimal? Energia { get; set; }
        public string Equipo { get; set; }
        public string Famnomb { get; set; }

        public int Emprcodi { get; set; }
        public string TipoEquipo { get; set; }
        public decimal? Generacion { get; set; }
        public decimal? Transmision { get; set; }
        public decimal? Distribucion { get; set; }
        public decimal? UsuarioLibre { get; set; }
        public decimal? Total { get { return Generacion + Transmision + Distribucion + UsuarioLibre; } }

        public EveInterrupcionDTO()
        {
            Generacion = 0.000m;
            Transmision = 0.000m;
            Distribucion = 0.000m;
            UsuarioLibre = 0.000m;
        }

        public decimal Interrupcion { get; set; }
        public string Tipoemprdesc { get; set; }
        public int Tipoemprcodi { get; set; }
        public int Famcodi { get; set; }
        public string Famnom { get; set; }
        public decimal? Interrupciontotal { get; set; }
        
        #endregion

        #region MigracionSGOCOES-GrupoB

        public DateTime Evenini { get; set; }
        public string InterrmfetapaDesc { get; set; }

        #endregion
    }

}
