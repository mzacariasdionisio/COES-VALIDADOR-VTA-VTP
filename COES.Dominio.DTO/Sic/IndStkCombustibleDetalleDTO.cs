using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_STKCOMBUSTIBLE_DETALLE
    /// </summary>
    public partial class IndStkCombustibleDetalleDTO : EntityBase, ICloneable
    {
        public int Stkdetcodi { get; set; }
        public int Stkcmtcodi { get; set; }
        public string Stkdettipo { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string D3 { get; set; }
        public string D4 { get; set; }
        public string D5 { get; set; }
        public string D6 { get; set; }
        public string D7 { get; set; }
        public string D8 { get; set; }
        public string D9 { get; set; }
        public string D10 { get; set; }
        public string D11 { get; set; }
        public string D12 { get; set; }
        public string D13 { get; set; }
        public string D14 { get; set; }
        public string D15 { get; set; }
        public string D16 { get; set; }
        public string D17 { get; set; }
        public string D18 { get; set; }
        public string D19 { get; set; }
        public string D20 { get; set; }
        public string D21 { get; set; }
        public string D22 { get; set; }
        public string D23 { get; set; }
        public string D24 { get; set; }
        public string D25 { get; set; }
        public string D26 { get; set; }
        public string D27 { get; set; }
        public string D28 { get; set; }
        public string D29 { get; set; }
        public string D30 { get; set; }
        public string D31 { get; set; }
        public string Stkdetusucreacion { get; set; }
        public DateTime Stkdetfeccreacion { get; set; }
        public string Stkdetusumodificacion { get; set; }
        public DateTime Stkdetfecmodificacion { get; set; }


        //Additional
        public int Ipericodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodicentral { get; set; }
        public int Equicodiunidad { get; set; }
        public int Tipoinfocodi { get; set; }
        public string Emprnomb { get; set; }
        public string Equinombcentral { get; set; }
        public string Equinombunidad { get; set; }
        public string Tipoinfodesc { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class IndStkCombustibleDetalleDTO
    {

    }
}
