using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_VERREPNUM
    /// </summary>
    public class SpoVerrepnumHelper : HelperBase
    {
        public SpoVerrepnumHelper(): base(Consultas.SpoVerrepnumSql)
        {
        }

        public SpoVerrepnumDTO Create(IDataReader dr)
        {
            SpoVerrepnumDTO entity = new SpoVerrepnumDTO();

            int iVerrcodi = dr.GetOrdinal(this.Verrcodi);
            if (!dr.IsDBNull(iVerrcodi)) entity.Verrcodi = Convert.ToInt32(dr.GetValue(iVerrcodi));

            int iVerncodi = dr.GetOrdinal(this.Verncodi);
            if (!dr.IsDBNull(iVerncodi)) entity.Verncodi = Convert.ToInt32(dr.GetValue(iVerncodi));

            int iVerrncodi = dr.GetOrdinal(this.Verrncodi);
            if (!dr.IsDBNull(iVerrncodi)) entity.Verrncodi = Convert.ToInt32(dr.GetValue(iVerrncodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Verrcodi = "VERRCODI";
        public string Verncodi = "VERNCODI";
        public string Verrncodi = "VERRNCODI";
        public string Vernnro = "VERNNRO";
        public string Numhisabrev = "NUMHISABREV";
        public string Numecodi = "NUMECODI";

        #endregion

        public string SqlGetByVersionReporte
        {
            get { return GetSqlXml("GetByVersionReporte"); }
        }
    }
}
