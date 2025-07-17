using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_GERCSV
    /// </summary>
    public class RerGerCsvDTO : EntityBase
    {
        public int Regercodi { get; set; }
        public int Resddpcodi { get; set; }
        public string Regergndarchivo { get; set; }
        public string Regerhidarchivo { get; set; }
        public string Regerterarchivo { get; set; }
        public string Regerusucreacion { get; set; }
        public DateTime Regerfeccreacion { get; set; }
    }

    /// <summary>
    /// Clase que mapea la tabla RER_LECCSV_TEMP
    /// </summary>
    public class RerLecCsvTemp : EntityBase
    {
        public DateTime Rerfecinicio { get; set; }
        public int Reretapa { get; set; }
        public int Rerserie { get; set; }
        public int Rerbloque { get; set; }
        public string Rercentrsddp { get; set; }
        public decimal Rervalor { get; set; }
        public string Rertipcsv { get; set; }

        // Campos adicionales para el procesamiento de Sddp
        public int? Emprcodi { get; set; }
        public int? Equicodi { get; set; }
    }
}

