using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla cp_fuentegamsversion
    /// </summary>
    public class CpFuentegamsversionHelper : HelperBase
    {
        public CpFuentegamsversionHelper(): base(Consultas.CpFuentegamsversionSql)
        {
        }

        public CpFuentegamsversionDTO Create(IDataReader dr)
        {
            CpFuentegamsversionDTO entity = new CpFuentegamsversionDTO();

            int iFverscodi = dr.GetOrdinal(this.Fverscodi);
            if (!dr.IsDBNull(iFverscodi)) entity.Fverscodi = Convert.ToInt32(dr.GetValue(iFverscodi));

            int iFtegcodi = dr.GetOrdinal(this.Ftegcodi);
            if (!dr.IsDBNull(iFtegcodi)) entity.Ftegcodi = Convert.ToInt32(dr.GetValue(iFtegcodi));

            int iFversnum = dr.GetOrdinal(this.Fversnum);
            if (!dr.IsDBNull(iFversnum)) entity.Fversnum = Convert.ToInt32(dr.GetValue(iFversnum));

            int iFversdescrip = dr.GetOrdinal(this.Fversdescrip);
            if (!dr.IsDBNull(iFversdescrip)) entity.Fversdescrip = dr.GetString(iFversdescrip);

            int iFversusumodificacion = dr.GetOrdinal(this.Fversusumodificacion);
            if (!dr.IsDBNull(iFversusumodificacion)) entity.Fversusumodificacion = dr.GetString(iFversusumodificacion);

            int iFversfecmodificacion = dr.GetOrdinal(this.Fversfecmodificacion);
            if (!dr.IsDBNull(iFversfecmodificacion)) entity.Fversfecmodificacion = dr.GetDateTime(iFversfecmodificacion);

            int iFversestado = dr.GetOrdinal(this.Fversestado);
            if (!dr.IsDBNull(iFversestado)) entity.Fversestado = Convert.ToInt32(dr.GetValue(iFversestado));

            int iFversinputdata = dr.GetOrdinal(this.Fversinputdata);
            if (!dr.IsDBNull(iFversinputdata)) entity.Fversinputdata = dr.GetString(iFversinputdata);

            int iFversruncase = dr.GetOrdinal(this.Fversruncase);
            if (!dr.IsDBNull(iFversruncase)) entity.Fversruncase = dr.GetString(iFversruncase);

            int iFverscodigoencrip = dr.GetOrdinal(this.Fverscodigoencrip);
            if (!dr.IsDBNull(iFverscodigoencrip)) entity.Fverscodigoencrip = (byte[])dr.GetValue(iFverscodigoencrip);

            return entity;
        }


        #region Mapeo de Campos
        public string Fverscodi = "FVERSCODI";
        public string Ftegcodi = "FTEGCODI";
        public string Fversnum = "FVERSNUM";
        public string Fversdescrip = "FVERSDESCRIP";
        public string Fversusumodificacion = "FVERSUSUMODIFICACION";
        public string Fversfecmodificacion = "FVERSFECMODIFICACION";
        public string Fversestado = "FVERSESTADO";
        public string Fversinputdata = "FVERSINPUTDATA";
        public string Fversruncase = "FVERSRUNCASE";
        public string Fverscodigoencrip = "FVERSCODIGOENCRIP";

        #endregion

        public string SqlGetMaxVersion
        {
            get { return base.GetSqlXml("GetMaxVersion"); }
        }

    }
}
