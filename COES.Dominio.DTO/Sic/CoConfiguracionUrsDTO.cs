using System;
using System.Collections.Generic;
using COES.Base.Core;
using COES.Dominio.DTO.Scada;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_CONFIGURACION_URS
    /// </summary>
    public class CoConfiguracionUrsDTO : EntityBase
    {
        public int Conurscodi { get; set; } 
        public int? Copercodi { get; set; } 
        public int? Covercodi { get; set; } 
        public int? Grupocodi { get; set; } 
        public DateTime? Conursfecinicio { get; set; } 
        public DateTime? Conursfecfin { get; set; } 
        public string Conursusucreacion { get; set; } 
        public DateTime? Conursfeccreacion { get; set; } 
        public string Conursusumodificacion { get; set; } 
        public DateTime? Conursfecmodificacion { get; set; } 
        public List<List<string>> DatosOperacionURS { get; set; }
        public List<List<string>> DatosReporteExtranet { get; set; }
        public List<List<string>> DatosEquipoRPF { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public List<TrZonaSp7DTO> ListaZonas { get; set; }
        public List<List<string>> DataSeniales { get; set; }
        public List<ConfiguracionURSData> DataCanales { get; set; }
    }

    public class ConfiguracionURSData
    {
        public int Indice { get; set; }
        public List<TrCanalSp7DTO> Data { get; set; }
    }
}
