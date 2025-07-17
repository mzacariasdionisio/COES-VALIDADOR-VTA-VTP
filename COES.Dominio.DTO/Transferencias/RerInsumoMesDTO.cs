using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_INSUMO_MES
    /// </summary>
    public class RerInsumoMesDTO : EntityBase
    {
        public int Rerinmmescodi { get; set; }
        public int Rerinscodi { get; set; }
        public int Rerpprcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public int Rerinmanio { get; set; }
        public int Rerinmmes { get; set; }
        public string Rerinmtipresultado { get; set; }
        public string Rerinmtipinformacion { get; set; }
        public string Rerinmdetalle { get; set; }
        public decimal Rerinmmestotal { get; set; }
        public string Rerinmmesusucreacion { get; set; }
        public DateTime Rerinmmesfeccreacion { get; set; }

        //Atributos de consulta
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
    }
}

