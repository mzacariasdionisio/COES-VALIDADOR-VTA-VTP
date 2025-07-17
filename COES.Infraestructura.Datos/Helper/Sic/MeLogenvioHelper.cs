using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_LOGENVIO
    /// </summary>
    public class MeLogenvioHelper : HelperBase
    {
        public MeLogenvioHelper(): base(Consultas.MeLogenvioSql)
        {
        }

        public MeLogenvioDTO Create(IDataReader dr)
        {
            MeLogenvioDTO entity = new MeLogenvioDTO();

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iLogenvsec = dr.GetOrdinal(this.Logenvsec);
            if (!dr.IsDBNull(iLogenvsec)) entity.Logenvsec = Convert.ToInt32(dr.GetValue(iLogenvsec));

            int iLogenvfecha = dr.GetOrdinal(this.Logenvfecha);
            if (!dr.IsDBNull(iLogenvfecha)) entity.Logenvfecha = dr.GetDateTime(iLogenvfecha);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLogenvdescrip = dr.GetOrdinal(this.Logenvdescrip);
            if (!dr.IsDBNull(iLogenvdescrip)) entity.Logenvdescrip = dr.GetString(iLogenvdescrip);

            int iMencodi = dr.GetOrdinal(this.Mencodi);
            if (!dr.IsDBNull(iMencodi)) entity.Mencodi = Convert.ToInt32(dr.GetValue(iMencodi));

            int iLogenvmencant = dr.GetOrdinal(this.Logenvmencant);
            if (!dr.IsDBNull(iLogenvmencant)) entity.Logenvmencant = Convert.ToInt32(dr.GetValue(iLogenvmencant));

            return entity;
        }


        #region Mapeo de Campos

        public string Enviocodi = "ENVIOCODI";
        public string Logenvsec = "LOGENVSEC";
        public string Logenvfecha = "LOGENVFECHA";
        public string Lastuser = "LASTUSER";
        public string Logenvdescrip = "LOGENVDESCRIP";
        public string Mencodi = "MENCODI";
        public string Logenvmencant = "LOGENVMENCANT";

        #endregion
    }
}
