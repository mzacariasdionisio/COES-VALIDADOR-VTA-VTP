using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_REPORTEDETALLE
    /// </summary>
    public class CmReportedetalleDTO : EntityBase
    {
        public int? Cmrepcodi { get; set; }
        public int? Barrcodi { get; set; }
        public DateTime? Cmredefecha { get; set; }
        public int? Cmredeperiodo { get; set; }
        public decimal? Cmredecmtotal { get; set; }
        public decimal? Cmredecmenergia { get; set; }
        public decimal? Cmredecongestion { get; set; }
        public int? Cmredevaltotal { get; set; }
        public int? Cmredevalenergia { get; set; }
        public int? Cmredevalcongestion { get; set; }
        public string Cmredetiporeporte { get; set; }
        public int Cmredecodi { get; set; }
        public string Barrnombre { get; set; }

        public int? Barrcodi2 { get; set; }
        public string Barrnombre2 { get; set; }

        public int Bloque { get; set; }
        public int SubBloque { get; set; }

        public CmReportedetalleDTO()
        { }

        public CmReportedetalleDTO(CmReportedetalleDTO entity)
        {
            Cmrepcodi = entity.Cmrepcodi;
            Barrcodi = entity.Barrcodi;
            Cmredefecha = entity.Cmredefecha;
            Cmredeperiodo = entity.Cmredeperiodo;
            Cmredecmtotal = entity.Cmredecmtotal;
            Cmredecmenergia = entity.Cmredecmenergia;
            Cmredecongestion = entity.Cmredecongestion;
            Cmredevaltotal = entity.Cmredevaltotal;
            Cmredevalenergia = entity.Cmredevalenergia;
            Cmredevalcongestion = entity.Cmredevalcongestion;
            Cmredetiporeporte = entity.Cmredetiporeporte;
            Cmredecodi = entity.Cmredecodi;
            Barrnombre = entity.Barrnombre;
        }
    }
}
