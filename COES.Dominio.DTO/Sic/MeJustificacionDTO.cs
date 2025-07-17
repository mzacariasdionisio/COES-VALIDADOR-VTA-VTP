using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_JUSTIFICACION
    /// </summary>
    public class MeJustificacionDTO : EntityBase
    {
        public int Justcodi { get; set; } 
        public int? Enviocodi { get; set; } 
        public int? Ptomedicodi { get; set; } 
        public DateTime? Justfeccreacion { get; set; } 
        public string Justusucreacion { get; set; } 
        public int? Subcausacodi { get; set; } 
        public string Justdescripcionotros { get; set; } 
        public DateTime? Justfechainicio { get; set; } 
        public DateTime? Justfechafin { get; set; }
        //ASSETEC 201909: Nuevo atributo para distinguir la fuente de dato
        public int? Lectcodi { get; set; }

        #region Pronostico demanda - Bitacora
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Ptomedidesc { get; set; }
        public DateTime? Medifecha { get; set; }
        public string Fechainicio { get; set; }
        public string Horainicio { get; set; }
        public string Fechafin { get; set; }
        public string Horafin { get; set; }
        public string Subcausadesc { get; set; }
        public decimal? Consumoprevisto { get; set; }
        public string Gruponomb { get; set; }
        public int Grupocodi { get; set; }
        public string Area { get; set; }
        #endregion


    }
}
