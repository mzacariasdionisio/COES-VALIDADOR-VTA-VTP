using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_EMPRESA
    /// </summary>
    public class VtpPeajeSaldoTransmisionDTO : EntityBase
    {
        public int Pstrnscodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal Pstrnstotalrecaudacion { get; set; }
        public decimal Pstrnstotalpago { get; set; }
        public decimal Pstrnssaldotransmision { get; set; }
        public string Pstrnsusucreacion { get; set; }
        public DateTime Pstrnsfeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
    }
}
