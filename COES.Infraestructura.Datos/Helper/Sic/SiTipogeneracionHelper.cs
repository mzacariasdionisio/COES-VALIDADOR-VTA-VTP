using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_TIPOGENERACION
    /// </summary>
    public class SiTipogeneracionHelper : HelperBase
    {
        public SiTipogeneracionHelper(): base(Consultas.SiTipogeneracionSql)
        {
        }

        public SiTipogeneracionDTO Create(IDataReader dr)
        {
            SiTipogeneracionDTO entity = new SiTipogeneracionDTO();

            int iTgenercodi = dr.GetOrdinal(this.Tgenercodi);
            if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

            int iTgenerabrev = dr.GetOrdinal(this.Tgenerabrev);
            if (!dr.IsDBNull(iTgenerabrev)) entity.Tgenerabrev = dr.GetString(iTgenerabrev);

            int iTgenernomb = dr.GetOrdinal(this.Tgenernomb);
            if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

            int iTgenercolor = dr.GetOrdinal(this.Tgenercolor);
            if (!dr.IsDBNull(iTgenercolor)) entity.Tgenercolor = dr.GetString(iTgenercolor);

            return entity;
        }


        #region Mapeo de Campos

        public string Tgenercodi = "TGENERCODI";
        public string Tgenerabrev = "TGENERABREV";
        public string Tgenernomb = "TGENERNOMB";
        public string Tgenercolor = "TGENERCOLOR";

        #endregion

        #region PR5
        public string SqlTipoGeneracionxCentral
        {
            get { return base.GetSqlXml("SqlTipoGeneracionxCentral"); }
        }
        #endregion
    }
}

