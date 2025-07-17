using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_OFERTA_DETALLE
    /// </summary>
    public partial class SmaOfertaDetalleDTO : EntityBase
    {
        public int Urscodi { get; set; }
        public string Ofdehorainicio { get; set; }
        public string Ofdehorafin { get; set; }
        public string Ofdeprecio { get; set; }
        public string Ofdedusucreacion { get; set; }
        public DateTime? Ofdefeccreacion { get; set; }
        public string Ofdemoneda { get; set; }
        public string Ofdeusumodificacion { get; set; }
        public DateTime? Ofdefecmodificacion { get; set; }
        public int Ofdecodi { get; set; }
        public int? Ofercodi { get; set; }
        public decimal Ofdepotofertada { get; set; }
        public int Ofdetipo { get; set; }
    }

    public partial class SmaOfertaDetalleDTO
    {
        public decimal Ofdepotmaxofer { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }

        #region Cambios Setiembre 2020
        public decimal BandaCalificada { get; set; }
        public decimal BandaDisponible { get; set; }

        public int Horinicio { get; set; }
        public int Horfin { get; set; }
        #endregion

        public string Oferfuente { get; set; }
        public string Ursnomb { get; set; }
        public string Emprnomb { get; set; }
        public string Observacion { get; set; }
        public List<SmaRelacionOdMoDTO> RelacionesODMO { get; set; }
    }
}
