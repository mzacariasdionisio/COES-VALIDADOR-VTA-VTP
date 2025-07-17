using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_EVALUACION_ENERGIAUNIDAD
    /// </summary>
    public class RerEvaluacionEnergiaUnidadDTO : EntityBase
    {
        //Table
        public int Rereeucodi { get; set; }
        public int Reresecodi { get; set; }
        public int Rerevacodi { get; set; }
        public int Rereucodi { get; set; }
        public int Rersedcodi { get; set; }
        public int Equicodi { get; set; }
        public string Rereeuenergiaunidad { get; set; }
        public decimal Rereeutotenergia { get; set; }
        public string Rereeuusucreacionext { get; set; }
        public DateTime Rereeufeccreacionext { get; set; }
        public string Rereeuusucreacion { get; set; }
        public DateTime Rereeufeccreacion { get; set; }

        //Additional
        public string Equinomb { get; set; }
    }
}