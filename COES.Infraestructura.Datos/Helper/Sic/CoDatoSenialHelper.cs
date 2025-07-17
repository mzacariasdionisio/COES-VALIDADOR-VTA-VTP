using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_DATO_SENIAL
    /// </summary>
    public class CoDatoSenialHelper : HelperBase
    {
        public CoDatoSenialHelper(): base(Consultas.CoDatoSenialSql)
        {
        }

        public CoDatoSenialDTO Create(IDataReader dr)
        {
            CoDatoSenialDTO entity = new CoDatoSenialDTO();

            int iCodasecodi = dr.GetOrdinal(this.Codasecodi);
            if (!dr.IsDBNull(iCodasecodi)) entity.Codasecodi = Convert.ToInt32(dr.GetValue(iCodasecodi));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iCodasefechahora = dr.GetOrdinal(this.Codasefechahora);
            if (!dr.IsDBNull(iCodasefechahora)) entity.Codasefechahora = dr.GetDateTime(iCodasefechahora);

            int iCodasevalor = dr.GetOrdinal(this.Codasevalor);
            if (!dr.IsDBNull(iCodasevalor)) entity.Codasevalor = dr.GetDecimal(iCodasevalor);

            int iCodaseusucreacion = dr.GetOrdinal(this.Codaseusucreacion);
            if (!dr.IsDBNull(iCodaseusucreacion)) entity.Codaseusucreacion = dr.GetString(iCodaseusucreacion);

            int iCodasefeccreacion = dr.GetOrdinal(this.Codasefeccreacion);
            if (!dr.IsDBNull(iCodasefeccreacion)) entity.Codasefeccreacion = dr.GetDateTime(iCodasefeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Codasecodi = "CODASECODI";
        public string Canalcodi = "CANALCODI";
        public string Codasefechahora = "CODASEFECHAHORA";
        public string Codasevalor = "CODASEVALOR";
        public string Codaseusucreacion = "CODASEUSUCREACION";
        public string Codasefeccreacion = "CODASEFECCREACION";
        public string Canalnomb = "CANALNOMB";

        #endregion

        public string SqlGetPorFechas
        {
            get { return base.GetSqlXml("GetPorFechas"); }
        }
    }
}
