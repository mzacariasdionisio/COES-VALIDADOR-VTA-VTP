using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_RESPAGO
    /// </summary>
    public class StRespagoHelper : HelperBase
    {
        public StRespagoHelper(): base(Consultas.StRespagoSql)
        {
        }

        public StRespagoDTO Create(IDataReader dr)
        {
            StRespagoDTO entity = new StRespagoDTO();

            int iRespagcodi = dr.GetOrdinal(this.Respagcodi);
            if (!dr.IsDBNull(iRespagcodi)) entity.Respagcodi = Convert.ToInt32(dr.GetValue(iRespagcodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            //int iSistrncodi = dr.GetOrdinal(this.Sistrncodi);
            //if (!dr.IsDBNull(iSistrncodi)) entity.Sistrncodi = Convert.ToInt32(dr.GetValue(iSistrncodi));

            int iStcntgcodi = dr.GetOrdinal(this.Stcntgcodi);
            if (!dr.IsDBNull(iStcntgcodi)) entity.Stcntgcodi = Convert.ToInt32(dr.GetValue(iStcntgcodi));

            int iRespagusucreacion = dr.GetOrdinal(this.Respagusucreacion);
            if (!dr.IsDBNull(iRespagusucreacion)) entity.Respagusucreacion = dr.GetString(iRespagusucreacion);

            int iRespagfeccreacion = dr.GetOrdinal(this.Respagfeccreacion);
            if (!dr.IsDBNull(iRespagfeccreacion)) entity.Respagfeccreacion = dr.GetDateTime(iRespagfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Respagcodi = "RESPAGCODI";
        public string Strecacodi = "STRECACODI";
        //public string Sistrncodi = "SISTRNCODI";
        public string Stcntgcodi = "STCNTGCODI";
        public string Respagusucreacion = "RESPAGUSUCREACION";
        public string Respagfeccreacion = "RESPAGFECCREACION";
        //variables para consultas
        public string Stcompcodi = "STCOMPCODI";
        public string Stcompcodelemento = "STCOMPCODELEMENTO";
        //public string Sistrnnombre = "SISTRNNOMBRE";
        public string Equinomb = "EQUINOMB";
        #endregion


        public string SqlGetByCodElem
        {
            get { return base.GetSqlXml("GetByCodElem"); }
        }

        public string SqlListByStRespagoVersion
        {
            get { return base.GetSqlXml("ListByStRespagoVersion"); }
        }

        
    }
}
