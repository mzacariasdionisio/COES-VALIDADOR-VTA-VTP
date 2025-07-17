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
    public class VtpPeajeEmpresaPagoHelper : HelperBase
    {
        public VtpPeajeEmpresaPagoHelper()
            : base(Consultas.VtpPeajeEmpresaPagoSql)
        {
        }

        public VtpPeajeEmpresaPagoDTO Create(IDataReader dr)
        {
            VtpPeajeEmpresaPagoDTO entity = new VtpPeajeEmpresaPagoDTO();

            int iPempagcodi = dr.GetOrdinal(this.Pempagcodi);
            if (!dr.IsDBNull(iPempagcodi)) entity.Pempagcodi = Convert.ToInt32(dr.GetValue(iPempagcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodipeaje = dr.GetOrdinal(this.Emprcodipeaje);
            if (!dr.IsDBNull(iEmprcodipeaje)) entity.Emprcodipeaje = Convert.ToInt32(dr.GetValue(iEmprcodipeaje));

            int iPingcodi = dr.GetOrdinal(this.Pingcodi);
            if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

            int iEmprcodicargo = dr.GetOrdinal(this.Emprcodicargo);
            if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

            int iPempagtransmision = dr.GetOrdinal(this.Pempagtransmision);
            if (!dr.IsDBNull(iPempagtransmision)) entity.Pempagtransmision = dr.GetString(iPempagtransmision);

            int iPempagpeajepago = dr.GetOrdinal(this.Pempagpeajepago);
            if (!dr.IsDBNull(iPempagpeajepago)) entity.Pempagpeajepago = dr.GetDecimal(iPempagpeajepago);

            int iPempagsaldoanterior = dr.GetOrdinal(this.Pempagsaldoanterior);
            if (!dr.IsDBNull(iPempagsaldoanterior)) entity.Pempagsaldoanterior = dr.GetDecimal(iPempagsaldoanterior);

            int iPempagajuste = dr.GetOrdinal(this.Pempagajuste);
            if (!dr.IsDBNull(iPempagajuste)) entity.Pempagajuste = dr.GetDecimal(iPempagajuste);

            int iPempagsaldo = dr.GetOrdinal(this.Pempagsaldo);
            if (!dr.IsDBNull(iPempagsaldo)) entity.Pempagsaldo = dr.GetDecimal(iPempagsaldo);

            int iPempagpericodidest = dr.GetOrdinal(this.Pempagpericodidest);
            if (!dr.IsDBNull(iPempagpericodidest)) entity.Pempagpericodidest = Convert.ToInt32(dr.GetValue(iPempagpericodidest));

            int iPempagusucreacion = dr.GetOrdinal(this.Pempagusucreacion);
            if (!dr.IsDBNull(iPempagusucreacion)) entity.Pempagusucreacion = dr.GetString(iPempagusucreacion);

            int iPempagfeccreacion = dr.GetOrdinal(this.Pempagfeccreacion);
            if (!dr.IsDBNull(iPempagfeccreacion)) entity.Pempagfeccreacion = dr.GetDateTime(iPempagfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Pempagcodi = "PEMPAGCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodipeaje = "EMPRCODIPEAJE";
        public string Pingcodi = "PINGCODI";
        public string Emprcodicargo = "EMPRCODICARGO";
        public string Pempagtransmision = "PEMPAGTRANSMISION";
        public string Pempagpeajepago = "PEMPAGPEAJEPAGO";
        public string Pempagsaldoanterior = "PEMPAGSALDOANTERIOR";
        public string Pempagajuste = "PEMPAGAJUSTE";
        public string Pempagsaldo = "PEMPAGSALDO";
        public string Pempagpericodidest = "PEMPAGPERICODIDEST";
        public string Pempagusucreacion = "PEMPAGUSUCREACION";
        public string Pempagfeccreacion = "PEMPAGFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnombpeaje = "EMPRNOMBPEAJE";
        public string Emprnombcargo = "EMPRNOMBCARGO";
        public string Pingnombre = "PINGNOMBRE";
        public string Rrpecodi = "RRPECODI";

        public string Recpotnombre = "RECPOTNOMBRE";
        public string Perinombre = "PERINOMBRE";
        public string Perianio = "PERIANIO";
        public string Perimes = "PERIMES";
        public string Perianiomes = "PERIANIOMES";
        public string Recanombre = "RECANOMBRE";

        public string Pericodiini = "periinicio";
        public string Pericodifin = "perifin";
        public string Recpotini = "recpotinicio";
        public string Recpotfin = "recpotfin";

        public string Emprruc = "EMPRRUC";

        #region SIOSEIN
        public string Pingtipo = "PINGTIPO";
        public string Emprcodosinergminpeaje = "EMPRCODOSINERGMINPEAJE";
        public string Emprcodosinergmincargo = "EMPRCODOSINERGMINCARGO";
        #endregion

        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListPeajePago
        {
            get { return base.GetSqlXml("ListPeajePago"); }
        }

        public string SqlListPeajeCobro
        {
            get { return base.GetSqlXml("ListPeajeCobro"); }
        }

        public string SqlListPeajeCobroHistoricos
        {
            get { return base.GetSqlXml("ListPeajeCobroHistoricos"); }
        }

        public string SqlGetPeajeEmpresaPagoByEmprCodiAndRecPotCodi
        {
            get { return base.GetSqlXml("GetPeajeEmpresaPagoByEmprCodiAndRecPotCodi"); }
        }

        public string SqlListPeajeCobroSelect
        {
            get { return base.GetSqlXml("ListPeajeCobroSelect"); }
        }
        
        public string SqlListPeajeCobroNoTransm
        {
            get { return base.GetSqlXml("ListPeajeCobroNoTransm"); }
        }

        public string SqlListPeajeCobroReparto
        {
            get { return base.GetSqlXml("ListPeajeCobroReparto"); }
        }

        public string SqlGetSaldoAnterior
        {
            get { return base.GetSqlXml("GetSaldoAnterior"); }
        }

        public string SqlGetByIdSaldo
        {
            get { return base.GetSqlXml("GetByIdSaldo"); }
        }

        public string SqlUpdatePeriodoDestino
        {
            get { return base.GetSqlXml("UpdatePeriodoDestino"); }
        }

        public string SqlObtenerListPeajeEmpresaPago
        {
            get { return base.GetSqlXml("ObtenerListPeajeEmpresaPago"); }
        }

        public string SqlGetPeajeEmpresaPagoByComparative
        {
            get { return base.GetSqlXml("GetPeajeEmpresaPagoByComparative"); }
        }

        public string SqlGetPeajeEmpresaPagoByComparativeUnique
        {
            get { return base.GetSqlXml("GetPeajeEmpresaPagoByComparativeUnique"); }
        }

        public string SqlGetPeajeEmpresaPagoByCompHist
        {
            get { return base.GetSqlXml("GetPeajeEmpresaPagoByCompHist"); }
        }

        public string SqlGetPeajeEmpresaPagoByCompHistUnique
        {
            get { return base.GetSqlXml("GetPeajeEmpresaPagoByCompHistUnique"); }
        }
        
        //CU21
        public string SqlGetByCargoPrima
        {
            get { return base.GetSqlXml("GetByCargoPrima"); }
        }
    }
}
