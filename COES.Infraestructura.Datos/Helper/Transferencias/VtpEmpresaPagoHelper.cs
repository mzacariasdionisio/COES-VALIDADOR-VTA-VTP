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
    /// Clase que contiene el mapeo de la tabla VTP_EMPRESA_PAGO
    /// </summary>
    public class VtpEmpresaPagoHelper : HelperBase
    {
        public VtpEmpresaPagoHelper() : base(Consultas.VtpEmpresaPagoSql)
        {
        }

        public VtpEmpresaPagoDTO Create(IDataReader dr)
        {
            VtpEmpresaPagoDTO entity = new VtpEmpresaPagoDTO();

            int iPotepcodi = dr.GetOrdinal(this.Potepcodi);
            if (!dr.IsDBNull(iPotepcodi)) entity.Potepcodi = Convert.ToInt32(dr.GetValue(iPotepcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iPotsecodi = dr.GetOrdinal(this.Potsecodi);
            if (!dr.IsDBNull(iPotsecodi)) entity.Potsecodi = Convert.ToInt32(dr.GetValue(iPotsecodi));

            int iEmprcodipago = dr.GetOrdinal(this.Emprcodipago);
            if (!dr.IsDBNull(iEmprcodipago)) entity.Emprcodipago = Convert.ToInt32(dr.GetValue(iEmprcodipago));

            int iEmprcodicobro = dr.GetOrdinal(this.Emprcodicobro);
            if (!dr.IsDBNull(iEmprcodicobro)) entity.Emprcodicobro = Convert.ToInt32(dr.GetValue(iEmprcodicobro));

            int iPotepmonto = dr.GetOrdinal(this.Potepmonto);
            if (!dr.IsDBNull(iPotepmonto)) entity.Potepmonto = dr.GetDecimal(iPotepmonto);

            int iPotepusucreacion = dr.GetOrdinal(this.Potepusucreacion);
            if (!dr.IsDBNull(iPotepusucreacion)) entity.Potepusucreacion = dr.GetString(iPotepusucreacion);

            int iPotepfeccreacion = dr.GetOrdinal(this.Potepfeccreacion);
            if (!dr.IsDBNull(iPotepfeccreacion)) entity.Potepfeccreacion = dr.GetDateTime(iPotepfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Potepcodi = "POTEPCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Potsecodi = "POTSECODI";
        public string Emprcodipago = "EMPRCODIPAGO";
        public string Emprcodicobro = "EMPRCODICOBRO";
        public string Potepmonto = "POTEPMONTO";
        public string Potepusucreacion = "POTEPUSUCREACION";
        public string Potepfeccreacion = "POTEPFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnombpago = "EMPRNOMBPAGO";
        public string Emprnombcobro = "EMPRNOMBCOBRO";

        public string Recpotnombre = "RECPOTNOMBRE";
        public string Perinombre = "PERINOMBRE";
        public string Perianio = "PERIANIO";
        public string Perimes = "PERIMES";
        public string Perianiomes = "PERIANIOMES";
        public string Recanombre = "RECANOMBRE";

        public string Emprnomb = "EMPRNOMB";
        public string Pericodiini = "periinicio";
        public string Pericodifin = "perifin";
        public string Recpotini = "recpotinicio";
        public string Recpotfin = "recpotfin";

        #region SIOSEIN
        public string Emprcodosinergmincobro = "EMPRCODOSINERGMINCOBRO";
        public string Emprcodosinergminpago = "EMPRCODOSINERGMINPAGO";
        #endregion

        public string Emprruc = "EMPRRUC";

        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListPago
        {
            get { return base.GetSqlXml("ListPago"); }
        }

        public string SqlListCobro
        {
            get { return base.GetSqlXml("ListCobro"); }
        }

        public string SqlListCobroConsultaHistoricos
        {
            get { return base.GetSqlXml("ListCobroConsultaHistoricos"); }
        }

        public string SqlObtenerListaEmpresaPago
        {
            get { return base.GetSqlXml("ObtenerListaEmpresaPago"); }
        }


        public string SqlGetEmpresaPagoByComparative
        {
            get { return base.GetSqlXml("GetEmpresaPagoByComparative"); }
        }

        public string SqlGetEmpresaPagoHistoricoByComparative
        {
            get { return base.GetSqlXml("GetEmpresaPagoHistoricoByComparative"); }
        }
        public string SqlGetEmpresaPagoHistoricoByComparative2
        {
            get { return base.GetSqlXml("GetEmpresaPagoHistoricoByComparative2"); }
        }

        public string SqlGetEmpresaPagoByComparativeUnique
        {
            get { return base.GetSqlXml("GetEmpresaPagoByComparativeUnique"); }
        }

        public string SqlGetEmpresaPagoByHist
        {
            get { return base.GetSqlXml("GetEmpresaPagoByHist"); }
        }

        public string SqlGetEmpresaPagoByHistUnique
        {
            get { return base.GetSqlXml("GetEmpresaPagoByHistUnique"); }
        }
    }
}
