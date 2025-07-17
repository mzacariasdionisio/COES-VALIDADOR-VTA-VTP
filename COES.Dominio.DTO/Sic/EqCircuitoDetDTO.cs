using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_CIRCUITO_DET
    /// </summary>
    public class EqCircuitoDetDTO : EntityBase
    {
        public int Circdtcodi { get; set; }
        public int Circodi { get; set; }
        public int? Equicodihijo { get; set; }
        public int? Circodihijo { get; set; }
        public int Circdtagrup { get; set; }
        public int Circdtestado { get; set; }
        public DateTime? Circdtfecmodificacion { get; set; }
        public string Circdtusumodificacion { get; set; }
        public DateTime? Circdtfeccreacion { get; set; }
        public string Circdtusucreacion { get; set; }
        public DateTime Circdtfecvigencia { get; set; }

        public string Equinombhijo { get; set; }
        public string Circnombhijo { get; set; }
        public string Emprnombequihijo { get; set; }
        public string Emprnombcirchijo { get; set; }

        public int TipoDet { get; set; }
        public string Nombre { get; set; }
        public string Empresa { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string FechaVigencia { get; set; }

        public int EquicodiAsociado { get; set; }
    }
}
