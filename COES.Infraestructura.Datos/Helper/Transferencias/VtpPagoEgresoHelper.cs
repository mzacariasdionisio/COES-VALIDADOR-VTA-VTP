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
    public class VtpPagoEgresoHelper : HelperBase
    {
        public VtpPagoEgresoHelper() : base(Consultas.VtpPagoEgresoSql)
        {
        }

        public VtpPagoEgresoDTO Create(IDataReader dr)
        {
            VtpPagoEgresoDTO entity = new VtpPagoEgresoDTO();

            int iPagegrcodi = dr.GetOrdinal(this.Pagegrcodi);
            if (!dr.IsDBNull(iPagegrcodi)) entity.Pagegrcodi = Convert.ToInt32(dr.GetValue(iPagegrcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPagegregreso = dr.GetOrdinal(this.Pagegregreso);
            if (!dr.IsDBNull(iPagegregreso)) entity.Pagegregreso = dr.GetDecimal(iPagegregreso);

            int iPagegrsaldo = dr.GetOrdinal(this.Pagegrsaldo);
            if (!dr.IsDBNull(iPagegrsaldo)) entity.Pagegrsaldo = dr.GetDecimal(iPagegrsaldo);

            int iPagegrpagoegreso = dr.GetOrdinal(this.Pagegrpagoegreso);
            if (!dr.IsDBNull(iPagegrpagoegreso)) entity.Pagegrpagoegreso = dr.GetDecimal(iPagegrpagoegreso);

            int iPagegrusucreacion = dr.GetOrdinal(this.Pagegrusucreacion);
            if (!dr.IsDBNull(iPagegrusucreacion)) entity.Pagegrusucreacion = dr.GetString(iPagegrusucreacion);

            int iPagegrfeccreacion = dr.GetOrdinal(this.Pagegrfeccreacion);
            if (!dr.IsDBNull(iPagegrfeccreacion)) entity.Pagegrfeccreacion = dr.GetDateTime(iPagegrfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Pagegrcodi = "PAGEGRCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Pagegregreso = "PAGEGREGRESO";
        public string Pagegrsaldo = "PAGEGRSALDO";
        public string Pagegrpagoegreso = "PAGEGRPAGOEGRESO";
        public string Pagegrusucreacion = "PAGEGRUSUCREACION";
        public string Pagegrfeccreacion = "PAGEGRFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }
    }
}
