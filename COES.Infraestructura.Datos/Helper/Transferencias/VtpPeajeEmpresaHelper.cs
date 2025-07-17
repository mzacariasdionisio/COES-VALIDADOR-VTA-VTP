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
    public class VtpPeajeEmpresaHelper : HelperBase
    {
        public VtpPeajeEmpresaHelper() : base(Consultas.VtpPeajeEmpresaSql)
        {
        }

        public VtpPeajeEmpresaDTO Create(IDataReader dr)
        {
            VtpPeajeEmpresaDTO entity = new VtpPeajeEmpresaDTO();

            int iPempcodi = dr.GetOrdinal(this.Pempcodi);
            if (!dr.IsDBNull(iPempcodi)) entity.Pempcodi = Convert.ToInt32(dr.GetValue(iPempcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPemptotalrecaudacion = dr.GetOrdinal(this.Pemptotalrecaudacion);
            if (!dr.IsDBNull(iPemptotalrecaudacion)) entity.Pemptotalrecaudacion = dr.GetDecimal(iPemptotalrecaudacion);

            int iPempporctrecaudacion = dr.GetOrdinal(this.Pempporctrecaudacion);
            if (!dr.IsDBNull(iPempporctrecaudacion)) entity.Pempporctrecaudacion = dr.GetDecimal(iPempporctrecaudacion);

            int iPempusucreacion = dr.GetOrdinal(this.Pempusucreacion);
            if (!dr.IsDBNull(iPempusucreacion)) entity.Pempusucreacion = dr.GetString(iPempusucreacion);

            int iPempfeccreacion = dr.GetOrdinal(this.Pempfeccreacion);
            if (!dr.IsDBNull(iPempfeccreacion)) entity.Pempfeccreacion = dr.GetDateTime(iPempfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Pempcodi = "PEMPCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Pemptotalrecaudacion = "PEMPTOTALRECAUDACION";
        public string Pempporctrecaudacion = "PEMPPORCTRECAUDACION";
        public string Pempusucreacion = "PEMPUSUCREACION";
        public string Pempfeccreacion = "PEMPFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListPeriodo
        {
            get { return base.GetSqlXml("ListPeriodo"); }
        }
    }
}
