using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_PROPEQUI
    /// </summary>
    public class EqPropequiDTO : EntityBase
    {
        public int Propcodi { get; set; }
        public int Equicodi { get; set; }
        public DateTime? Fechapropequi { get; set; }
        public string Valor { get; set; }
        public string Propequiusucreacion { get; set; }
        public DateTime? Propequifeccreacion { get; set; }
        public string Propequiusumodificacion { get; set; }
        public DateTime? Propequifecmodificacion { get; set; }
        public string Propequiobservacion { get; set; }
        public int Propequideleted { get; set; }
        public string Propequisustento { get; set; }
        public int? Propequicheckcero { get; set; }
        public string Propequicomentario { get; set; }
        public string Lastuser { get; set; }

        public string osinergcodi { get; set; }
        //Datos de Propiedad
        public string Propnomb { get; set; }
        public string Propunidad { get; set; }
        public string Propfile { get; set; }

        #region SIOSEIN

        public string Equinomb { get; set; }

        #endregion
        #region NotificacionesCambiosEquipamiento
        public string Emprnomb { get; set; }
        #endregion


        #region PR5
        public int Famcodi { get; set; }
        public decimal? ValorDecimal { get; set; }
        public string ValorDesc { get; set; }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public int Grupocodi { get; set; }
        #endregion

        public string Equiabrev { get; set; }
        public string Famnomb { get; set; }
        public string Equiestado { get; set; }
        public string FechapropequiDesc { get; set; }
        public int? Orden { get; set; }

        //VALORES DE PROPIEDAD (FICHA TÉCNICA)
        public string Propfichaoficial { get; set; }// para filtro
        public string Propnombficha { get; set; }
        public int NroItem { get; set; } // Indice para la importación
        public string Observaciones { get; set; }
        public string PropequicheckceroDesc { get; set; }
        public string Propocultocomentario { get; set; }

        public int Propequideleted2 { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public DateTime FechaCambio { get; set; }
        public string FechaCambioDesc { get; set; }
        public bool EsSustentoConfidencial { get; set; }

        #region SIOSEIN2
        public int Equipadre { get; set; }
        #endregion

        #region Numerales Datos Base
        public string Osigrupocodi { get; set; }
        #endregion

        #region Equipos sin datos de ficha técnica
        public int Emprcodi { get; set; }
        public string PropVaciasCount { get; set; }
        public int Proptotal { get; set; }
        public int Propsinvacio { get; set; }
        public string Areadesc { get; set; }
        public string Areanomb { get; set; }
        #endregion


        #region GESTPROTECT
        public int Epproycodi { get; set; }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
