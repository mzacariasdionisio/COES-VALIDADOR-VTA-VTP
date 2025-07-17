using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla DOC_DIA_ESP
    /// </summary>
    public class DocDiaEspHelper : HelperBase
    {
        public DocDiaEspHelper() : base(Consultas.DocDiaEspSql)
        {
        }

        public DocDiaEspDTO Create(IDataReader dr)
        {
            DocDiaEspDTO entity = new DocDiaEspDTO();

            int iDiacodi = dr.GetOrdinal(this.Diacodi);
            if (!dr.IsDBNull(iDiacodi)) entity.Diacodi = Convert.ToInt32(dr.GetValue(iDiacodi));

            int iDiafecha = dr.GetOrdinal(this.Diafecha);
            if (!dr.IsDBNull(iDiafecha)) entity.Diafecha = dr.GetDateTime(iDiafecha);

            int iDiatipo = dr.GetOrdinal(this.Diatipo);
            if (!dr.IsDBNull(iDiatipo)) entity.Diatipo = Convert.ToInt32(dr.GetValue(iDiatipo));

            int iDiafrec = dr.GetOrdinal(this.Diafrec);
            if (!dr.IsDBNull(iDiafrec)) entity.Diafrec = dr.GetString(iDiafrec);

            int iDiadesc = dr.GetOrdinal(this.Diadesc);
            if (!dr.IsDBNull(iDiadesc)) entity.Diadesc = dr.GetString(iDiadesc);

            return entity;
        }


        #region Mapeo de Campos

        public string Diacodi = "DIACODI";
        public string Diafecha = "DIAFECHA";
        public string Diatipo = "DIATIPO";
        public string Diafrec = "DIAFREC";
        public string Diadesc = "DIADESC";

        #endregion
    }
}
