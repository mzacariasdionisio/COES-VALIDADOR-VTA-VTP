using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_REPORTE_VARIABLE
    /// </summary>
    public class InReporteVariableHelper : HelperBase
    {
        public InReporteVariableHelper() : base(Consultas.InReporteVariableSql)
        {
        }

        public InReporteVariableDTO Create(IDataReader dr)
        {
            InReporteVariableDTO entity = new InReporteVariableDTO();

            int iInrevacodi = dr.GetOrdinal(this.Inrevacodi);
            if (!dr.IsDBNull(iInrevacodi)) entity.Inrevacodi = Convert.ToInt32(dr.GetValue(iInrevacodi));

            int iInvarcodi = dr.GetOrdinal(this.Invarcodi);
            if (!dr.IsDBNull(iInvarcodi)) entity.Invarcodi = Convert.ToInt32(dr.GetValue(iInvarcodi));

            int iInrevavalor = dr.GetOrdinal(this.Inrevavalor);
            if (!dr.IsDBNull(iInrevavalor)) entity.Inrevavalor = dr.GetString(iInrevavalor);

            int iInrevausucreacion = dr.GetOrdinal(this.Inrevausucreacion);
            if (!dr.IsDBNull(iInrevausucreacion)) entity.Inrevausucreacion = dr.GetString(iInrevausucreacion);

            int iInrevafeccreacion = dr.GetOrdinal(this.Inrevafeccreacion);
            if (!dr.IsDBNull(iInrevafeccreacion)) entity.Inrevafeccreacion = dr.GetDateTime(iInrevafeccreacion);

            int iInrevausumodificacion = dr.GetOrdinal(this.Inrevausumodificacion);
            if (!dr.IsDBNull(iInrevausumodificacion)) entity.Inrevausumodificacion = dr.GetString(iInrevausumodificacion);

            int iInrevafecmodificacion = dr.GetOrdinal(this.Inrevafecmodificacion);
            if (!dr.IsDBNull(iInrevafecmodificacion)) entity.Inrevafecmodificacion = dr.GetDateTime(iInrevafecmodificacion);

            int iInrepcodi = dr.GetOrdinal(this.Inrepcodi);
            if (!dr.IsDBNull(iInrepcodi)) entity.Inrepcodi = Convert.ToInt32(dr.GetValue(iInrepcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Inrevacodi = "INREVACODI";
        public string Inrevaidentificador = "INREVAIDENTIFICADOR";
        public string Inrevavalor = "INREVAVALOR";
        public string Inrevausucreacion = "INREVAUSUCREACION";
        public string Inrevafeccreacion = "INREVAFECCREACION";
        public string Inrevausumodificacion = "INREVAUSUMODIFICACION";
        public string Inrevafecmodificacion = "INREVAFECMODIFICACION";
        public string Inrepcodi = "INREPCODI";
        public string Invarcodi = "INVARCODI";

        public string Invardescripcion = "INVARDESCRIPCION";
        public string Invaridentificador = "INVARIDENTIFICADOR";
        public string Invarnota = "INVARNOTA";
        public string Invartipodato = "INVARTIPODATO";

        #endregion
    
    }
}
