using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_CONVOCATORIAS
    /// </summary>
    public class WbConvocatoriasHelper : HelperBase
    {
        public WbConvocatoriasHelper() : base(Consultas.WbConvocatoriasSql)
        {
        }

        public WbConvocatoriasDTO Create(IDataReader dr)
        {
            WbConvocatoriasDTO entity = new WbConvocatoriasDTO();

            int iConvcodi = dr.GetOrdinal(this.Convcodi);
            if (!dr.IsDBNull(iConvcodi)) entity.Convcodi = Convert.ToInt32(dr.GetValue(iConvcodi));

            int iConvabrev = dr.GetOrdinal(this.Convabrev);
            if (!dr.IsDBNull(iConvabrev)) entity.Convabrev = dr.GetString(iConvabrev);

            int iConvnomb = dr.GetOrdinal(this.Convnomb);
            if (!dr.IsDBNull(iConvnomb)) entity.Convnomb = dr.GetString(iConvnomb);

            int iConvdesc = dr.GetOrdinal(this.Convdesc);
            if (!dr.IsDBNull(iConvdesc)) entity.Convdesc = dr.GetString(iConvdesc);

            int iConvlink = dr.GetOrdinal(this.Convlink);
            if (!dr.IsDBNull(iConvlink)) entity.Convlink = dr.GetString(iConvlink);

            int iConvfechaini = dr.GetOrdinal(this.Convfechaini);
            if (!dr.IsDBNull(iConvfechaini)) entity.Convfechaini = dr.GetDateTime(iConvfechaini);

            int iConvfechafin = dr.GetOrdinal(this.Convfechafin);
            if (!dr.IsDBNull(iConvfechafin)) entity.Convfechafin = dr.GetDateTime(iConvfechafin);

            int iConvestado = dr.GetOrdinal(this.Convestado);
            if (!dr.IsDBNull(iConvestado)) entity.Convestado = dr.GetString(iConvestado);

            int iDatecreacion = dr.GetOrdinal(this.Datecreacion);
            if (!dr.IsDBNull(iDatecreacion)) entity.Datecreacion = dr.GetDateTime(iDatecreacion);

            int iUsercreacion = dr.GetOrdinal(this.Usercreacion);
            if (!dr.IsDBNull(iUsercreacion)) entity.Usercreacion = dr.GetString(iUsercreacion);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            return entity;
        }


        #region Mapeo de Campos

        public string Convcodi = "CONVCODI";
        public string Convabrev = "CONVABREV";
        public string Convnomb = "CONVNOMB";
        public string Convdesc = "CONVDESC";
        public string Convlink = "CONVLINK";
        public string Convfechaini = "CONVFECHAINI";
        public string Convfechafin = "CONVFECHAFIN";
        public string Convestado = "CONVESTADO";
        public string Datecreacion = "DATECREACION";
        public string Usercreacion = "USERCREACION";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion
    }
}
