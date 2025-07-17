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
    public class VtpSaldoEmpresaHelper : HelperBase
    {
        public VtpSaldoEmpresaHelper()
            : base(Consultas.VtpSaldoEmpresaSql)
        {
        }

        public VtpSaldoEmpresaDTO Create(IDataReader dr)
        {
            VtpSaldoEmpresaDTO entity = new VtpSaldoEmpresaDTO();

            int iPotseusucreacion = dr.GetOrdinal(this.Potseusucreacion);
            if (!dr.IsDBNull(iPotseusucreacion)) entity.Potseusucreacion = dr.GetString(iPotseusucreacion);

            int iPotsefeccreacion = dr.GetOrdinal(this.Potsefeccreacion);
            if (!dr.IsDBNull(iPotsefeccreacion)) entity.Potsefeccreacion = dr.GetDateTime(iPotsefeccreacion);

            int iPotsecodi = dr.GetOrdinal(this.Potsecodi);
            if (!dr.IsDBNull(iPotsecodi)) entity.Potsecodi = Convert.ToInt32(dr.GetValue(iPotsecodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPotseingreso = dr.GetOrdinal(this.Potseingreso);
            if (!dr.IsDBNull(iPotseingreso)) entity.Potseingreso = dr.GetDecimal(iPotseingreso);

            int iPotseegreso = dr.GetOrdinal(this.Potseegreso);
            if (!dr.IsDBNull(iPotseegreso)) entity.Potseegreso = dr.GetDecimal(iPotseegreso);

            int iPotsesaldo = dr.GetOrdinal(this.Potsesaldo);
            if (!dr.IsDBNull(iPotsesaldo)) entity.Potsesaldo = dr.GetDecimal(iPotsesaldo);

            int iPotseajuste = dr.GetOrdinal(this.Potseajuste);
            if (!dr.IsDBNull(iPotseajuste)) entity.Potseajuste = dr.GetDecimal(iPotseajuste);

            int iPotsesaldoanterior = dr.GetOrdinal(this.Potsesaldoanterior);
            if (!dr.IsDBNull(iPotsesaldoanterior)) entity.Potsesaldoanterior = dr.GetDecimal(iPotsesaldoanterior);

            int iPotsesaldoreca = dr.GetOrdinal(this.Potsesaldoreca);
            if (!dr.IsDBNull(iPotsesaldoreca)) entity.Potsesaldoreca = dr.GetDecimal(iPotsesaldoreca);

            int iPotsepericodidest = dr.GetOrdinal(this.Potsepericodidest);
            if (!dr.IsDBNull(iPotsepericodidest)) entity.Potsepericodidest = Convert.ToInt32(dr.GetValue(iPotsepericodidest));

            return entity;
        }

        #region Mapeo de Campos

        public string Potseusucreacion = "POTSEUSUCREACION";
        public string Potsefeccreacion = "POTSEFECCREACION";
        public string Potsecodi = "POTSECODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Potseingreso = "POTSEINGRESO";
        public string Potseegreso = "POTSEEGRESO";
        public string Potsesaldoanterior = "POTSESALDOANTERIOR";
        public string Potsesaldo = "POTSESALDO";
        public string Potseajuste = "POTSEAJUSTE";
        public string Potsesaldoreca = "POTSESALDORECA";
        public string Potsepericodidest = "POTSEPERICODIDEST";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";
        public string Potsetotalsaldopositivo = "POTSETOTALSALDOPOSITIVO";
        public string Potsetotalsaldonegativo = "POTSETOTALSALDONEGATIVO";
        public string Perinombre = "PERINOMBRE";

        #endregion

        public string SqlListCalculaSaldo
        {
            get { return base.GetSqlXml("ListCalculaSaldo"); }
        }

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListPositiva
        {
            get { return base.GetSqlXml("ListPositiva"); }
        }

        public string SqlListNegativa
        {
            get { return base.GetSqlXml("ListNegativa"); }
        }

        public string SqlGetByIdSaldo
        {
            get { return base.GetSqlXml("GetByIdSaldo"); }
        }

        public string SqlGetByIdSaldoGeneral
        {
            get { return base.GetSqlXml("GetByIdSaldoGeneral"); }
        }

        public string SqlUpdatePeriodoDestino
        {
            get { return base.GetSqlXml("UpdatePeriodoDestino"); }
        }

        public string SqlGetSaldoAnterior
        {
            get { return base.GetSqlXml("GetSaldoAnterior"); }
        }

        public string SqlListPeriodosDestino
        {
            get { return base.GetSqlXml("ListPeriodosDestino"); }
        }

        public string SqlGetSaldoEmpresaPeriodo
        {
            get { return base.GetSqlXml("GetSaldoEmpresaPeriodo"); }
        }


    }
}
