using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_EVALUACION_SOLICITUDEDI
    /// </summary>
    public class RerEvaluacionSolicitudEdiDTO : EntityBase
    {   
        //table
        public int Reresecodi { get; set; }
        public int Rerevacodi { get; set; }
        public int Rersedcodi { get; set; }
        public int Rercencodi { get; set; }
        public int Emprcodi { get; set; }
        public int Ipericodi { get; set; }
        public int Reroricodi { get; set; }
        public DateTime Reresefechahorainicio { get; set; }
        public DateTime Reresefechahorafin { get; set; }
        public string Reresedesc { get; set; }
        public decimal Reresetotenergia { get; set; }
        public string Reresesustento { get; set; }
        public string Rereseestadodeenvio { get; set; }
        public string Rereseeliminado { get; set; }
        public string Rereseusucreacionext { get; set; }
        public DateTime Reresefeccreacionext { get; set; }
        public string Rereseusumodificacionext { get; set; }
        public DateTime Reresefecmodificacionext { get; set; }
        public string Rereseusucreacion { get; set; }
        public DateTime Reresefeccreacion { get; set; }
        public string Rereseusumodificacion { get; set; }
        public DateTime Reresefecmodificacion { get; set; }
        public decimal? Reresetotenergiaestimada { get; set; } 
        public decimal? Rereseediaprobada { get; set; }
        public decimal? Rereserfpmc { get; set; }
        public string Rereseresdesc { get; set; }
        public string Rereseresestado { get; set; }

        //additional
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public string Emprnomb { get; set; }
        public string Reroridesc { get; set; }
        public string Rercenestado { get; set; }
        public int Iperianio { get; set; }
        public int Iperimes { get; set; }
    }
}
