using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_NOTA
    /// </summary>
    public class SiNotaHelper : HelperBase
    {
        public SiNotaHelper()
            : base(Consultas.SiNotaSql)
        {
        }

        public SiNotaDTO Create(IDataReader dr)
        {
            SiNotaDTO entity = new SiNotaDTO();

            int iSinotacodi = dr.GetOrdinal(this.Sinotacodi);
            if (!dr.IsDBNull(iSinotacodi)) entity.Sinotacodi = Convert.ToInt32(dr.GetValue(iSinotacodi));

            int iSinotadesc = dr.GetOrdinal(this.Sinotadesc);
            if (!dr.IsDBNull(iSinotadesc)) entity.Sinotadesc = dr.GetString(iSinotadesc);

            int iSinotausucreacion = dr.GetOrdinal(this.Sinotausucreacion);
            if (!dr.IsDBNull(iSinotausucreacion)) entity.Sinotausucreacion = dr.GetString(iSinotausucreacion);

            int iSinotafeccreacion = dr.GetOrdinal(this.Sinotafeccreacion);
            if (!dr.IsDBNull(iSinotafeccreacion)) entity.Sinotafeccreacion = dr.GetDateTime(iSinotafeccreacion);

            int iSinotausumodificacion = dr.GetOrdinal(this.Sinotausumodificacion);
            if (!dr.IsDBNull(iSinotausumodificacion)) entity.Sinotausumodificacion = dr.GetString(iSinotausumodificacion);

            int iSinotafecmodificacion = dr.GetOrdinal(this.Sinotafecmodificacion);
            if (!dr.IsDBNull(iSinotafecmodificacion)) entity.Sinotafecmodificacion = dr.GetDateTime(iSinotafecmodificacion);

            int iSinotaestado = dr.GetOrdinal(this.Sinotaestado);
            if (!dr.IsDBNull(iSinotaestado)) entity.Sinotaestado = Convert.ToInt32(dr.GetValue(iSinotaestado));

            int iSinotaperiodo = dr.GetOrdinal(this.Sinotaperiodo);
            if (!dr.IsDBNull(iSinotaperiodo)) entity.Sinotaperiodo = dr.GetDateTime(iSinotaperiodo);

            int iMrepcodi = dr.GetOrdinal(this.Mrepcodi);
            if (!dr.IsDBNull(iMrepcodi)) entity.Mrepcodi = Convert.ToInt32(dr.GetValue(iMrepcodi));

            int iSinotaorden = dr.GetOrdinal(this.Sinotaorden);
            if (!dr.IsDBNull(iSinotaorden)) entity.Sinotaorden = Convert.ToInt32(dr.GetValue(iSinotaorden));

            int iSinotatipo = dr.GetOrdinal(this.Sinotatipo);
            if (!dr.IsDBNull(iSinotatipo)) entity.Sinotatipo = Convert.ToInt32(dr.GetValue(iSinotatipo));

            int iVerscodi = dr.GetOrdinal(this.Verscodi);
            if (!dr.IsDBNull(iVerscodi)) entity.Verscodi = dr.GetInt32(iVerscodi);

            return entity;
        }


        #region Mapeo de Campos

        public string Sinotacodi = "SINOTACODI";
        public string Sinotadesc = "SINOTADESC";
        public string Sinotausucreacion = "SINOTAUSUCREACION";
        public string Sinotafeccreacion = "SINOTAFECCREACION";
        public string Sinotausumodificacion = "SINOTAUSUMODIFICACION";
        public string Sinotafecmodificacion = "SINOTAFECMODIFICACION";
        public string Sinotaestado = "SINOTAESTADO";
        public string Sinotaperiodo = "SINOTAPERIODO";
        public string Mrepcodi = "MREPCODI";
        public string Sinotaorden = "SINOTAORDEN";
        public string Sinotatipo = "SINOTATIPO";
        public string Verscodi = "VERSCODI";


        #endregion

        public string SqlUpdateOrden
        {
            get { return GetSqlXml("UpdateOrden"); }
        }

        public string SqlGetMaxSinotaorden
        {
            get { return GetSqlXml("GetMaxSinotaorden"); }
        }
    }
}
