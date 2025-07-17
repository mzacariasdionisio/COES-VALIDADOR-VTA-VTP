using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_GASEODUCTOXCENTRAL
    /// </summary>
    public class IndGaseoductoxcentralHelper : HelperBase
    {
        public IndGaseoductoxcentralHelper(): base(Consultas.IndGaseoductoxcentralSql)
        {
        }

        public IndGaseoductoxcentralDTO Create(IDataReader dr)
        {
            IndGaseoductoxcentralDTO entity = new IndGaseoductoxcentralDTO();

            int iGasctrcodi = dr.GetOrdinal(this.Gasctrcodi);
            if (!dr.IsDBNull(iGasctrcodi)) entity.Gasctrcodi = Convert.ToInt32(dr.GetValue(iGasctrcodi));

            int iGaseoductoequicodi = dr.GetOrdinal(this.Gaseoductoequicodi);
            if (!dr.IsDBNull(iGaseoductoequicodi)) entity.Gaseoductoequicodi = Convert.ToInt32(dr.GetValue(iGaseoductoequicodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGasctrestado = dr.GetOrdinal(this.Gasctrestado);
            if (!dr.IsDBNull(iGasctrestado)) entity.Gasctrestado = dr.GetString(iGasctrestado);

            int iGasctrfeccreacion = dr.GetOrdinal(this.Gasctrfeccreacion);
            if (!dr.IsDBNull(iGasctrfeccreacion)) entity.Gasctrfeccreacion = dr.GetDateTime(iGasctrfeccreacion);

            int iGasctrusucreacion = dr.GetOrdinal(this.Gasctrusucreacion);
            if (!dr.IsDBNull(iGasctrusucreacion)) entity.Gasctrusucreacion = dr.GetString(iGasctrusucreacion);

            int iGasctrusumodificacion = dr.GetOrdinal(this.Gasctrusumodificacion);
            if (!dr.IsDBNull(iGasctrusumodificacion)) entity.Gasctrusumodificacion = dr.GetString(iGasctrusumodificacion);

            int iGasctrfecmodificacion = dr.GetOrdinal(this.Gasctrfecmodificacion);
            if (!dr.IsDBNull(iGasctrfecmodificacion)) entity.Gasctrfecmodificacion = dr.GetDateTime(iGasctrfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Gasctrcodi = "GASCTRCODI";
        public string Gaseoductoequicodi = "GASEODUCTOEQUICODI";
        public string Equicodi = "EQUICODI";
        public string Gasctrestado = "GASCTRESTADO";
        public string Gasctrfeccreacion = "GASCTRFECCREACION";
        public string Gasctrusucreacion = "GASCTRUSUCREACION";
        public string Gasctrusumodificacion = "GASCTRUSUMODIFICACION";
        public string Gasctrfecmodificacion = "GASCTRFECMODIFICACION";

        #endregion


        public string SqlInactivar => GetSqlXml("Inactivar");

        public string Emprnomb = "EMPRNOMB";
        public string Central = "CENTRAL";
        public string Gaseoducto = "GASEODUCTO";
    }
}
