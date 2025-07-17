using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_COMPMENSUAL
    /// </summary>
    public class StCompmensualHelper : HelperBase
    {
        public StCompmensualHelper(): base(Consultas.StCompmensualSql)
        {
        }

        public StCompmensualDTO Create(IDataReader dr)
        {
            StCompmensualDTO entity = new StCompmensualDTO();

            int iCmpmencodi = dr.GetOrdinal(this.Cmpmencodi);
            if (!dr.IsDBNull(iCmpmencodi)) entity.Cmpmencodi = Convert.ToInt32(dr.GetValue(iCmpmencodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            //int iSistrncodi = dr.GetOrdinal(this.Sistrncodi);
            //if (!dr.IsDBNull(iSistrncodi)) entity.Sistrncodi = Convert.ToInt32(dr.GetValue(iSistrncodi));

            int iStcntgcodi = dr.GetOrdinal(this.Stcntgcodi);
            if (!dr.IsDBNull(iStcntgcodi)) entity.Stcntgcodi = Convert.ToInt32(dr.GetValue(iStcntgcodi));

            int iCmpmenusucreacion = dr.GetOrdinal(this.Cmpmenusucreacion);
            if (!dr.IsDBNull(iCmpmenusucreacion)) entity.Cmpmenusucreacion = dr.GetString(iCmpmenusucreacion);

            int iCmpmenfeccreacion = dr.GetOrdinal(this.Cmpmenfeccreacion);
            if (!dr.IsDBNull(iCmpmenfeccreacion)) entity.Cmpmenfeccreacion = dr.GetDateTime(iCmpmenfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmpmencodi = "CMPMENCODI";
        public string Strecacodi = "STRECACODI";
        //public string Sistrncodi = "SISTRNCODI";
        public string Stcntgcodi = "STCNTGCODI";
        public string Cmpmenusucreacion = "CMPMENUSUCREACION";
        public string Cmpmenfeccreacion = "CMPMENFECCREACION";

        //variables para consultas

        //public string Sistrnnombre = "SISTRNNOMBRE";
        public string Equinomb = "EQUINOMB";

        #endregion

        public string SqlListByStCompMensualVersion
        {
            get { return base.GetSqlXml("ListByStCompMensualVersion"); }
        }

    }
}
