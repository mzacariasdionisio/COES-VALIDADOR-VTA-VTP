using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_INGRESO_POTEFR_DETALLE
    /// </summary>
    public class VtpIngresoPotefrDetalleHelper : HelperBase
    {
        public VtpIngresoPotefrDetalleHelper(): base(Consultas.VtpIngresoPotefrDetalleSql)
        {
        }

        public VtpIngresoPotefrDetalleDTO Create(IDataReader dr)
        {
            VtpIngresoPotefrDetalleDTO entity = new VtpIngresoPotefrDetalleDTO();

            int iIpefrdcodi = dr.GetOrdinal(this.Ipefrdcodi);
            if (!dr.IsDBNull(iIpefrdcodi)) entity.Ipefrdcodi = Convert.ToInt32(dr.GetValue(iIpefrdcodi));

            int iIpefrcodi = dr.GetOrdinal(this.Ipefrcodi);
            if (!dr.IsDBNull(iIpefrcodi)) entity.Ipefrcodi = Convert.ToInt32(dr.GetValue(iIpefrcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCenequicodi = dr.GetOrdinal(this.Cenequicodi);
            if (!dr.IsDBNull(iCenequicodi)) entity.Cenequicodi = Convert.ToInt32(dr.GetValue(iCenequicodi));

            int iUniequicodi = dr.GetOrdinal(this.Uniequicodi);
            if (!dr.IsDBNull(iUniequicodi)) entity.Uniequicodi = Convert.ToInt32(dr.GetValue(iUniequicodi));

            int iIpefrdpoteefectiva = dr.GetOrdinal(this.Ipefrdpoteefectiva);
            if (!dr.IsDBNull(iIpefrdpoteefectiva)) entity.Ipefrdpoteefectiva = dr.GetDecimal(iIpefrdpoteefectiva);

            int iIpefrdpotefirme = dr.GetOrdinal(this.Ipefrdpotefirme);
            if (!dr.IsDBNull(iIpefrdpotefirme)) entity.Ipefrdpotefirme = dr.GetDecimal(iIpefrdpotefirme);

            int iIpefrdpotefirmeremunerable = dr.GetOrdinal(this.Ipefrdpotefirmeremunerable);
            if (!dr.IsDBNull(iIpefrdpotefirmeremunerable)) entity.Ipefrdpotefirmeremunerable = dr.GetDecimal(iIpefrdpotefirmeremunerable);

            int iIpefrdusucreacion = dr.GetOrdinal(this.Ipefrdusucreacion);
            if (!dr.IsDBNull(iIpefrdusucreacion)) entity.Ipefrdusucreacion = dr.GetString(iIpefrdusucreacion);

            int iIpefrdfeccreacion = dr.GetOrdinal(this.Ipefrdfeccreacion);
            if (!dr.IsDBNull(iIpefrdfeccreacion)) entity.Ipefrdfeccreacion = dr.GetDateTime(iIpefrdfeccreacion);

            int iIpefrdusumodificacion = dr.GetOrdinal(this.Ipefrdusumodificacion);
            if (!dr.IsDBNull(iIpefrdusumodificacion)) entity.Ipefrdusumodificacion = dr.GetString(iIpefrdusumodificacion);

            int iIpefrdfecmodificacion = dr.GetOrdinal(this.Ipefrdfecmodificacion);
            if (!dr.IsDBNull(iIpefrdfecmodificacion)) entity.Ipefrdfecmodificacion = dr.GetDateTime(iIpefrdfecmodificacion);

            int iUnigrupocodi = dr.GetOrdinal(this.Unigrupocodi);
            if (!dr.IsDBNull(iUnigrupocodi)) entity.Unigrupocodi = Convert.ToInt32(dr.GetValue(iUnigrupocodi));

            int iIpefrdunidadnomb = dr.GetOrdinal(this.Ipefrdunidadnomb);
            if (!dr.IsDBNull(iIpefrdunidadnomb)) entity.Ipefrdunidadnomb = dr.GetString(iIpefrdunidadnomb);

            int iIpefrdficticio = dr.GetOrdinal(this.Ipefrdficticio);
            if (!dr.IsDBNull(iIpefrdficticio)) entity.Ipefrdficticio = Convert.ToInt32(dr.GetValue(iIpefrdficticio));

            return entity;
        }
        
        #region Mapeo de Campos

        public string Ipefrdcodi = "IPEFRDCODI";
        public string Ipefrcodi = "IPEFRCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cenequicodi = "CENEQUICODI";
        public string Uniequicodi = "UNIEQUICODI";
        public string Ipefrdpoteefectiva = "IPEFRDPOTEEFECTIVA";
        public string Ipefrdpotefirme = "IPEFRDPOTEFIRME";
        public string Ipefrdpotefirmeremunerable = "IPEFRDPOTEFIRMEREMUNERABLE";
        public string Ipefrdusucreacion = "IPEFRDUSUCREACION";
        public string Ipefrdfeccreacion = "IPEFRDFECCREACION";
        public string Ipefrdusumodificacion = "IPEFRDUSUMODIFICACION";
        public string Ipefrdfecmodificacion = "IPEFRDFECMODIFICACION";
        public string Unigrupocodi = "UNIGRUPOCODI";
        public string Ipefrdunidadnomb = "IPEFRDUNIDADNOMB";
        public string Ipefrdficticio = "IPEFRDFICTICIO";

        //Para los nombres
        public string Emprnomb = "EMPRNOMB";
        public string Cenequinomb ="CENEQUINOMB";
        public string Uniequinomb = "UNIEQUINOMB";
        //SIOSEIN-PRIE-2021
        public string Osinergcodi = "OSINERGCODI";
        public string Equicodivtp = "EQUICODIVTP";
        public string Famcodi = "FAMCODI";

        #endregion

        public string SqlDeletebyCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlGetByCriteriaSumCentral
        {
            get { return base.GetSqlXml("GetByCriteriaSumCentral"); }
        }

        public string SqlDeletebyCriteriaVersion
        {
            get { return base.GetSqlXml("DeleteByCriteriaVersion"); }
        }

        public string SqlObtenerPotenciaEFRSumPorEmpresa
        {
            get { return base.GetSqlXml("ObtenerPotenciaEFRSumPorEmpresa"); }
        }

        #region FIT - VALORIZACION DIARIA

        public string SqlGetPotenciaFirmeRemunerable
        {
            get { return base.GetSqlXml("GetPotenciaFirmeRemunerable"); }
        }

        //SIOSEIN-PRIE-2021
        public string SqlGetByCriteriaSinPRIE
        {
            get { return base.GetSqlXml("GetByCriteriaSinPRIE"); }
        }
        #endregion

        #region PrimasRER.2023
        public string SqlGetCentralUnidadByEmpresa
        {
            get { return base.GetSqlXml("GetCentralUnidadByEmpresa"); }
        }
        #endregion
    }
}
