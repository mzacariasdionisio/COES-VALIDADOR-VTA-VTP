using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class TrnDemandaHelper : HelperBase
    {
        public TrnDemandaHelper() : base(Consultas.TrnDemandaSql)
        {
        }

        public TrnDemandaDTO Create(IDataReader dr)
        {
            TrnDemandaDTO entity = new TrnDemandaDTO();

            #region Campos Originales
            // DEMCODI
            int iDemcodi = dr.GetOrdinal(this.Demcodi);
            if (!dr.IsDBNull(iDemcodi)) entity.Demcodi = dr.GetInt32(iDemcodi);

            // EMPRCODI
            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            // VALORMAXIMO
            int iValormaximo = dr.GetOrdinal(this.Valormaximo);
            if (!dr.IsDBNull(iValormaximo)) entity.Valormaximo = dr.GetDecimal(iValormaximo);

            // PERIODODEMANDA
            int iPeriododemanda = dr.GetOrdinal(this.Periododemanda);
            if (!dr.IsDBNull(iPeriododemanda)) entity.Periododemanda = dr.GetString(iPeriododemanda);

            // LASTUSER
            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            // LASTDATE
            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            #endregion

            return entity;
        }

        #region Campos Originales
        public string Demcodi = "DEMCODI";
        public string Emprcodi = "EMPRCODI";
        public string Valormaximo = "VALORMAXIMO";
        public string Periododemanda = "PERIODODEMANDA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        #endregion

        public string SqlListxEmpresaPeriodo
        {
            get { return base.GetSqlXml("GetListxEmpresaPeriodo"); }
        }
    }
}
