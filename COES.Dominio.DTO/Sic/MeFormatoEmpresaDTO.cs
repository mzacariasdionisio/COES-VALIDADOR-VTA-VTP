using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_FORMATO_EMPRESA
    /// </summary>
    public class MeFormatoEmpresaDTO : EntityBase
    {
        public int Formatcodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int? Foremdiatomamedicion { get; set; } 
        public string Foremusucreacion { get; set; } 
        public DateTime Foremfeccreacion { get; set; } 
        public string Foremusumodificacion { get; set; } 
        public DateTime? Foremfecmodificacion { get; set; }

        //- remision-pr16.JDEL - Inicio 19/05/2016: Cambio para atender el requerimiento del PR16.
        public DateTime? PeriodoFechaIni { get; set; }
        public DateTime? PeriodoFechaFin { get; set; }
        //- JDEL Fin
       
        public string Emprnomb { get; set; }
    }
}
