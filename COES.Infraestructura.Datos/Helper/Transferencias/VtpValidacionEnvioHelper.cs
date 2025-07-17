using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class VtpValidacionEnvioHelper: HelperBase
    {
        public VtpValidacionEnvioHelper()
            : base(Consultas.VtpValidacionEnvioSql)
        {
        }

        public VtpValidacionEnvioDTO Create(IDataReader dr)
        {
            VtpValidacionEnvioDTO entity = new VtpValidacionEnvioDTO();


            int iVaencodi = dr.GetOrdinal(this.VaenCodi);
            if (!dr.IsDBNull(iVaencodi)) entity.VaenCodi = Convert.ToInt32(dr.GetValue(iVaencodi));

            int iPegrcodi = dr.GetOrdinal(this.PegrCodi);
            if (!dr.IsDBNull(iPegrcodi)) entity.PegrCodi = Convert.ToInt32(dr.GetValue(iPegrcodi));

            int iPegrdcodi = dr.GetOrdinal(this.PegrdCodi);
            if (!dr.IsDBNull(iPegrdcodi)) entity.PegrdCodi = Convert.ToInt32(dr.GetValue(iPegrdcodi));

            int iVaentipovalidacion = dr.GetOrdinal(this.VaenTipoValidacion);
            if (!dr.IsDBNull(iVaentipovalidacion)) entity.VaenTipoValidacion = dr.GetString(iVaentipovalidacion);

            int iVaennomcliente = dr.GetOrdinal(this.VaenNomCliente);
            if (!dr.IsDBNull(iVaennomcliente)) entity.VaenNomCliente = dr.GetString(iVaennomcliente);

            int iVaencodvtea = dr.GetOrdinal(this.VaenCodVtea);
            if (!dr.IsDBNull(iVaencodvtea)) entity.VaenCodVtea = dr.GetString(iVaencodvtea);

            int iVaencodvtp = dr.GetOrdinal(this.VaenCodVtp);
            if (!dr.IsDBNull(iVaencodvtp)) entity.VaenCodVtp = dr.GetString(iVaencodvtp);

            int iVaenbarratra = dr.GetOrdinal(this.VaenBarraTra);
            if (!dr.IsDBNull(iVaenbarratra)) entity.VaenBarraTra = dr.GetString(iVaenbarratra);

            int iVaenbarrasum = dr.GetOrdinal(this.VaenBarraSum);
            if (!dr.IsDBNull(iVaenbarrasum)) entity.VaenBarraSum = dr.GetString(iVaenbarrasum);

            int iVarcodporcentaje = dr.GetOrdinal(this.VaenValorVtea);
            if (!dr.IsDBNull(iVarcodporcentaje)) entity.VaenValorVtea = dr.GetDecimal(iVarcodporcentaje);

            int iVaenvalorvtp= dr.GetOrdinal(this.VaenValorVtp);
            if (!dr.IsDBNull(iVaenvalorvtp)) entity.VaenValorVtp = dr.GetDecimal(iVaenvalorvtp);

            int iVaenvalorreportado = dr.GetOrdinal(this.VaenValorReportado);
            if (!dr.IsDBNull(iVaenvalorreportado)) entity.VaenValorReportado = dr.GetDecimal(iVaenvalorreportado);

            int iVaenvalorcoes = dr.GetOrdinal(this.VaenValorCoes);
            if (!dr.IsDBNull(iVaenvalorcoes)) entity.VaenValorCoes = dr.GetDecimal(iVaenvalorcoes);

            int iVaenvariacion = dr.GetOrdinal(this.VaenVariacion);
            if (!dr.IsDBNull(iVaenvariacion)) entity.VaenVariacion = dr.GetDecimal(iVaenvariacion);

            int iVaenrevisionanterior = dr.GetOrdinal(this.VaenRevisionAnterior);
            if (!dr.IsDBNull(iVaenrevisionanterior)) entity.VaenRevisionAnterior = dr.GetDecimal(iVaenrevisionanterior);

            int iVaenpreciopotencia = dr.GetOrdinal(this.VaenPrecioPotencia);
            if (!dr.IsDBNull(iVaenpreciopotencia)) entity.VaenPrecioPotencia = dr.GetDecimal(iVaenpreciopotencia);

            int iVaenpeajeunitario = dr.GetOrdinal(this.VaenPeajeUnitario);
            if (!dr.IsDBNull(iVaenpeajeunitario)) entity.VaenPeajeUnitario = dr.GetDecimal(iVaenpeajeunitario);
            return entity;
        }

        #region Mapeo de Campos

        public string VaenCodi = "VAENCODI";
        public string PegrCodi = "PEGRCODI";
        public string PegrdCodi = "PEGRDCODI";
        public string VaenTipoValidacion = "VAENTIPOVALIDACION";
        public string VaenNomCliente = "VAENNOMCLIENTE";
        public string VaenCodVtea = "VAENCODVTEA";
        public string VaenCodVtp = "VAENCODVTP";
        public string VaenBarraTra = "VAENBARRATRA";
        public string VaenBarraSum = "VAENBARRASUM";
        public string VaenValorVtea = "VAENVALORVTEA";
        public string VaenValorVtp = "VAENVALORVTP";
        public string VaenValorReportado = "VAENVALORREPORTADO";
        public string VaenValorCoes = "VAENVALORCOES";
        public string VaenVariacion = "VAENVARIACION";
        public string VaenRevisionAnterior = "VAENREVISIONANTERIOR";
        public string VaenPrecioPotencia = "VAENPRECIOPOTENCIA";
        public string VaenPeajeUnitario = "VAENPEAJEUNITARIO";

        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string NroPagina = "nropagina";
        public string PageSize = "pagesize";
        public string Fila = "Fila";

        #endregion

        public string SqlGetValidacionEnvioByPegrcodi
        {
            get { return base.GetSqlXml("GetValidacionEnvioByPegrcodi"); }
        }
    }
}
