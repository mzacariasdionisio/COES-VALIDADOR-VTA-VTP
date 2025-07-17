using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_PROVISIONBASE
    /// </summary>
    public class VcrProvisionbaseDTO : EntityBase
    {
        public int Vcrpbcodi { get; set; } 
        public DateTime Vcrpbperiodoini { get; set; } 
        public DateTime Vcrpbperiodofin { get; set; } 
        public int Equicodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public decimal Vcrpbpotenciabf { get; set; } 
        public decimal Vcrpbpreciobf { get; set; } 
        public string Vcrpbusucreacion { get; set; } 
        public DateTime Vcrpbfeccreacion { get; set; } 
        public string Vcrpbusumodificacion { get; set; } 
        public DateTime Vcrpbfecmodificacion { get; set; }
        //ASSETEC: 202010 - NUEVO ATRIBUTO
        public decimal Vcrpbpotenciabfb { get; set; }
        public decimal Vcrpbpreciobfb { get; set; }


        //Para la consulta
        public string Equinomb { get; set; }
    }
}
