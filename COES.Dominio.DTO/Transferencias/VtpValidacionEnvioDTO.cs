using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_VALIDACION_ENVIO
    /// </summary>
    [Serializable]
    public class VtpValidacionEnvioDTO: EntityBase
    {
        public int VaenCodi { get; set; }
        public int PegrCodi { get; set; }
        public int PegrdCodi { get; set; }
        public string VaenTipoValidacion { get; set; }
        public string VaenNomCliente { get; set; }
        public string VaenCodVtea { get; set; }
        public string VaenCodVtp { get; set; }
        public string VaenBarraTra { get; set; }
        public string VaenBarraSum { get; set; }
        public decimal? VaenValorVtea { get; set; }
        public decimal? VaenValorVtp { get; set; }
        public decimal? VaenValorReportado { get; set; }
        public decimal? VaenValorCoes { get; set; }
        public decimal? VaenVariacion { get; set; }
        public decimal? VaenRevisionAnterior { get; set; }
        public decimal? VaenPrecioPotencia { get; set; }
        public decimal? VaenPeajeUnitario { get; set; }
        public List<Object> lstValidacionEnvio { get; set; }
        public int VaenValorAgrupamiento { get; set; }
    }
}
