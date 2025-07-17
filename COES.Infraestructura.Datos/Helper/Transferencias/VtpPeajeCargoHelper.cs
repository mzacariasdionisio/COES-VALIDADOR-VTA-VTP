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
    /// Clase que contiene el mapeo de la tabla VTP_PEAJE_CARGO
    /// </summary>
    public class VtpPeajeCargoHelper : HelperBase
    {
        public VtpPeajeCargoHelper()
            : base(Consultas.VtpPeajeCargoSql)
        {
        }

        public VtpPeajeCargoDTO Create(IDataReader dr)
        {
            VtpPeajeCargoDTO entity = new VtpPeajeCargoDTO();

            int iPecarcodi = dr.GetOrdinal(this.Pecarcodi);
            if (!dr.IsDBNull(iPecarcodi)) entity.Pecarcodi = Convert.ToInt32(dr.GetValue(iPecarcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPingcodi = dr.GetOrdinal(this.Pingcodi);
            if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

            int iPecartransmision = dr.GetOrdinal(this.Pecartransmision);
            if (!dr.IsDBNull(iPecartransmision)) entity.Pecartransmision = dr.GetString(iPecartransmision);

            int iPecarpeajecalculado = dr.GetOrdinal(this.Pecarpeajecalculado);
            if (!dr.IsDBNull(iPecarpeajecalculado)) entity.Pecarpeajecalculado = dr.GetDecimal(iPecarpeajecalculado);

            int iPecarpeajedeclarado = dr.GetOrdinal(this.Pecarpeajedeclarado);
            if (!dr.IsDBNull(iPecarpeajedeclarado)) entity.Pecarpeajedeclarado = dr.GetDecimal(iPecarpeajedeclarado);

            int iPecarpeajerecaudado = dr.GetOrdinal(this.Pecarpeajerecaudado);
            if (!dr.IsDBNull(iPecarpeajerecaudado)) entity.Pecarpeajerecaudado = dr.GetDecimal(iPecarpeajerecaudado);

            int iPecarsaldoanterior = dr.GetOrdinal(this.Pecarsaldoanterior);
            if (!dr.IsDBNull(iPecarsaldoanterior)) entity.Pecarsaldoanterior = dr.GetDecimal(iPecarsaldoanterior);

            int iPecarajuste = dr.GetOrdinal(this.Pecarajuste);
            if (!dr.IsDBNull(iPecarajuste)) entity.Pecarajuste = dr.GetDecimal(iPecarajuste);

            int iPecarsaldo = dr.GetOrdinal(this.Pecarsaldo);
            if (!dr.IsDBNull(iPecarsaldo)) entity.Pecarsaldo = dr.GetDecimal(iPecarsaldo);

            int iPecarpericodidest = dr.GetOrdinal(this.Pecarpericodidest);
            if (!dr.IsDBNull(iPecarpericodidest)) entity.Pecarpericodidest = Convert.ToInt32(dr.GetValue(iPecarpericodidest));

            int iPecarusucreacion = dr.GetOrdinal(this.Pecarusucreacion);
            if (!dr.IsDBNull(iPecarusucreacion)) entity.Pecarusucreacion = dr.GetString(iPecarusucreacion);

            int iPecarfeccreacion = dr.GetOrdinal(this.Pecarfeccreacion);
            if (!dr.IsDBNull(iPecarfeccreacion)) entity.Pecarfeccreacion = dr.GetDateTime(iPecarfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Pecarcodi = "PECARCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Pingcodi = "PINGCODI";
        public string Pecartransmision = "PECARTRANSMISION";
        public string Pecarpeajecalculado = "PECARPEAJECALCULADO";
        public string Pecarpeajedeclarado = "PECARPEAJEDECLARADO";
        public string Pecarpeajerecaudado = "PECARPEAJERECAUDADO";
        public string Pecarsaldoanterior = "PECARSALDOANTERIOR";
        public string Pecarajuste = "PECARAJUSTE";
        public string Pecarsaldo = "PECARSALDO";
        public string Pecarpericodidest = "PECARPERICODIDEST";
        public string Pecarusucreacion = "PECARUSUCREACION";
        public string Pecarfeccreacion = "PECARFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";
        public string Pingnombre = "PINGNOMBRE";

        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListEmpresa
        {
            get { return base.GetSqlXml("ListEmpresa"); }
        }

        public string SqlListPagoNo
        {
            get { return base.GetSqlXml("ListPagoNo"); }
        }

        public string SqlListPagoAdicional
        {
            get { return base.GetSqlXml("ListPagoAdicional"); }
        }

        public string SqlGetByIdSaldo
        {
            get { return base.GetSqlXml("GetByIdSaldo"); }
        }

        public string SqlUpdatePeriodoDestino
        {
            get { return base.GetSqlXml("UpdatePeriodoDestino"); }
        }

        public string SqlGetSaldoAnterior
        {
            get { return base.GetSqlXml("GetSaldoAnterior"); }
        }


    }
}
