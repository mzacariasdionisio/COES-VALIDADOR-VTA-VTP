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
    /// Clase que contiene el mapeo de la tabla VTP_SALDO_EMPRESA
    /// </summary>
    public class VtpPeajeSaldoTransmisionHelper : HelperBase
    {
        public VtpPeajeSaldoTransmisionHelper() : base(Consultas.VtpPeajeSaldoTransmisionSql)
        {
        }

        public VtpPeajeSaldoTransmisionDTO Create(IDataReader dr)
        {
            VtpPeajeSaldoTransmisionDTO entity = new VtpPeajeSaldoTransmisionDTO();

            int iPstrnscodi = dr.GetOrdinal(this.Pstrnscodi);
            if (!dr.IsDBNull(iPstrnscodi)) entity.Pstrnscodi = Convert.ToInt32(dr.GetValue(iPstrnscodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPstrnstotalrecaudacion = dr.GetOrdinal(this.Pstrnstotalrecaudacion);
            if (!dr.IsDBNull(iPstrnstotalrecaudacion)) entity.Pstrnstotalrecaudacion = dr.GetDecimal(iPstrnstotalrecaudacion);

            int iPstrnstotalpago = dr.GetOrdinal(this.Pstrnstotalpago);
            if (!dr.IsDBNull(iPstrnstotalpago)) entity.Pstrnstotalpago = dr.GetDecimal(iPstrnstotalpago);

            int iPstrnssaldotransmision = dr.GetOrdinal(this.Pstrnssaldotransmision);
            if (!dr.IsDBNull(iPstrnssaldotransmision)) entity.Pstrnssaldotransmision = dr.GetDecimal(iPstrnssaldotransmision);

            int iPstrnsusucreacion = dr.GetOrdinal(this.Pstrnsusucreacion);
            if (!dr.IsDBNull(iPstrnsusucreacion)) entity.Pstrnsusucreacion = dr.GetString(iPstrnsusucreacion);

            int iPstrnsfeccreacion = dr.GetOrdinal(this.Pstrnsfeccreacion);
            if (!dr.IsDBNull(iPstrnsfeccreacion)) entity.Pstrnsfeccreacion = dr.GetDateTime(iPstrnsfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pstrnscodi = "PSTRNSCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Pstrnstotalrecaudacion = "PSTRNSTOTALRECAUDACION";
        public string Pstrnstotalpago = "PSTRNSTOTALPAGO";
        public string Pstrnssaldotransmision = "PSTRNSSALDOTRANSMISION";
        public string Pstrnsusucreacion = "PSTRNSUSUCREACION";
        public string Pstrnsfeccreacion = "PSTRNSFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListEmpresaEgreso
        {
            get { return base.GetSqlXml("ListEmpresaEgreso"); }
        }

        public string SqlGetByIdEmpresa
        {
            get { return base.GetSqlXml("GetByIdEmpresa"); }
        }
    }
}
