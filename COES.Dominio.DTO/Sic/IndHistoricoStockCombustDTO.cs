using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_HISTORICO_STOCKCOMBUST
    /// </summary>
    public partial class IndHistoricoStockCombustDTO : EntityBase, ICloneable
    {
        public int Hststkcodi { get; set; }
        public int? Stkcmtcodi { get; set; }
        public int? Ipericodi { get; set; }
        public int? Emprcodi { get; set; }
        public int? Equicodicentral { get; set; }
        public int? Equicodiunidad { get; set; }
        public int? Tipoinfocodi { get; set; }
        public string Hststkperiodo { get; set; }
        public string Hststkempresa { get; set; }
        public string Hststkcentral { get; set; }
        public string Hststkunidad { get; set; }
        public string Hststktipoinfo { get; set; }
        public DateTime? Hststkfecha { get; set; }
        public string Hststkoriginal { get; set; }
        public string Hststkmodificado { get; set; }
        public string Hststktipaccion { get; set; }
        public string Hststkusucreacion { get; set; }
        public DateTime? Hststkfeccreacion { get; set; }
        public string Hststkusumodificacion { get; set; }
        public DateTime? Hststkfecmodificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class IndHistoricoStockCombustDTO
    {

    }

}
