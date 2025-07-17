using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnVersiongrpHelper : HelperBase
    {
        public PrnVersiongrpHelper() : base(Consultas.PrnVersiongrpSql) { }

        public PrnVersiongrpDTO Create(IDataReader dr)
        {
            PrnVersiongrpDTO entity = new PrnVersiongrpDTO();

            int iVergrpcodi = dr.GetOrdinal(this.Vergrpcodi);
            if (!dr.IsDBNull(iVergrpcodi)) entity.Vergrpcodi = Convert.ToInt32(dr.GetValue(iVergrpcodi));

            int iVergrpnomb = dr.GetOrdinal(this.Vergrpnomb);
            if (!dr.IsDBNull(iVergrpnomb)) entity.Vergrpnomb = dr.GetString(iVergrpnomb);

            return entity;
        }

        #region Mapeo de los campos
        public string Vergrpcodi = "VERGRPCODI";
        public string Vergrpnomb = "VERGRPNOMB";
        public string Vergrpareausuaria = "VERGRPAREAUSUARIA";
        #endregion

        #region Consultas a la BD
        public string SqlGetByName
        {
            get { return GetSqlXml("GetByName"); }
        }  
        
        public string SqlListVersionesPronosticoPorFecha
        {
            get { return GetSqlXml("ListVersionesPronosticoPorFecha"); }
        }

        public string SqlListVersionByArea
        {
            get { return GetSqlXml("ListVersionByArea"); }
        }
        public string SqlListVersionByAreaFecha
        {
            get { return GetSqlXml("ListVersionByAreaFecha"); }
        }
        public string SqlGetByNameArea
        {
            get { return GetSqlXml("GetByNameArea"); }
        }
        #endregion
    }
}
