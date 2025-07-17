using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_ACTIVIDAD
    /// </summary>
    public class SiActividadHelper : HelperBase
    {
        public SiActividadHelper(): base(Consultas.SiActividadSql)
        {
        }

        public SiActividadDTO Create(IDataReader dr)
        {
            SiActividadDTO entity = new SiActividadDTO();

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iActcodi = dr.GetOrdinal(this.Actcodi);
            if (!dr.IsDBNull(iActcodi)) entity.Actcodi = Convert.ToInt32(dr.GetValue(iActcodi));

            int iActabrev = dr.GetOrdinal(this.Actabrev);
            if (!dr.IsDBNull(iActabrev)) entity.Actabrev = dr.GetString(iActabrev);

            int iActnomb = dr.GetOrdinal(this.Actnomb);
            if (!dr.IsDBNull(iActnomb)) entity.Actnomb = dr.GetString(iActnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Areacodi = "AREACODI";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Actcodi = "ACTCODI";
        public string Actabrev = "ACTABREV";
        public string Actnomb = "ACTNOMB";

        #endregion

        public string SqlGetListaActividadesPersonal
        {
            get { return base.GetSqlXml("GetListaActividadesPersonal"); }
        }

        public string Areaabrev = "Areaabrev";
    }
}
