using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_HISTORICO_INDICADOR_DET
    /// </summary>
    public class EpoHistoricoIndicadorDetHelper : HelperBase
    {
        public EpoHistoricoIndicadorDetHelper(): base(Consultas.EpoHistoricoIndicadorDetSql)
        {
        }

        public EpoHistoricoIndicadorDetDTO Create(IDataReader dr)
        {
            EpoHistoricoIndicadorDetDTO entity = new EpoHistoricoIndicadorDetDTO();

            int iHincodi = dr.GetOrdinal(this.Hincodi);
            if (!dr.IsDBNull(iHincodi)) entity.Hincodi = Convert.ToInt32(dr.GetValue(iHincodi));

            int iPercodi = dr.GetOrdinal(this.Percodi);
            if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

            int iHidvalor = dr.GetOrdinal(this.Hidvalor);
            if (!dr.IsDBNull(iHidvalor)) entity.Hidvalor = dr.GetDecimal(iHidvalor);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            return entity;
        }


        #region Mapeo de Campos

        public string Hincodi = "HINCODI";
        public string Percodi = "PERCODI";
        public string Hidvalor = "HIDVALOR";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion
    }
}
