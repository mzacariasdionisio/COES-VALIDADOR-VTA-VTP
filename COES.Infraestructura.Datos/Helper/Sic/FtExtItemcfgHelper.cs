using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ITEMCFG
    /// </summary>
    public class FtExtItemcfgHelper : HelperBase
    {
        public FtExtItemcfgHelper() : base(Consultas.FtExtItemcfgSql)
        {
        }

        public FtExtItemcfgDTO Create(IDataReader dr)
        {
            FtExtItemcfgDTO entity = new FtExtItemcfgDTO();

            int iFtitcodi = dr.GetOrdinal(this.Ftitcodi);
            if (!dr.IsDBNull(iFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(iFtitcodi));

            int iFitcfgcodi = dr.GetOrdinal(this.Fitcfgcodi);
            if (!dr.IsDBNull(iFitcfgcodi)) entity.Fitcfgcodi = Convert.ToInt32(dr.GetValue(iFitcfgcodi));

            int iFitcfgflagcomentario = dr.GetOrdinal(this.Fitcfgflagcomentario);
            if (!dr.IsDBNull(iFitcfgflagcomentario)) entity.Fitcfgflagcomentario = dr.GetString(iFitcfgflagcomentario);

            int iFitcfgflagvalorconf = dr.GetOrdinal(this.Fitcfgflagvalorconf);
            if (!dr.IsDBNull(iFitcfgflagvalorconf)) entity.Fitcfgflagvalorconf = dr.GetString(iFitcfgflagvalorconf);

            int iFitcfgflagbloqedicion = dr.GetOrdinal(this.Fitcfgflagbloqedicion);
            if (!dr.IsDBNull(iFitcfgflagbloqedicion)) entity.Fitcfgflagbloqedicion = dr.GetString(iFitcfgflagbloqedicion);

            int iFitcfgflagsustento = dr.GetOrdinal(this.Fitcfgflagsustento);
            if (!dr.IsDBNull(iFitcfgflagsustento)) entity.Fitcfgflagsustento = dr.GetString(iFitcfgflagsustento);

            int iFitcfgflagsustentoconf = dr.GetOrdinal(this.Fitcfgflagsustentoconf);
            if (!dr.IsDBNull(iFitcfgflagsustentoconf)) entity.Fitcfgflagsustentoconf = dr.GetString(iFitcfgflagsustentoconf);

            int iFitcfgflaginstructivo = dr.GetOrdinal(this.Fitcfgflaginstructivo);
            if (!dr.IsDBNull(iFitcfgflaginstructivo)) entity.Fitcfgflaginstructivo = dr.GetString(iFitcfgflaginstructivo);

            int iFitcfgflagvalorobligatorio = dr.GetOrdinal(this.Fitcfgflagvalorobligatorio);
            if (!dr.IsDBNull(iFitcfgflagvalorobligatorio)) entity.Fitcfgflagvalorobligatorio = dr.GetString(iFitcfgflagvalorobligatorio);

            int iFitcfgflagsustentoobligatorio = dr.GetOrdinal(this.Fitcfgflagsustentoobligatorio);
            if (!dr.IsDBNull(iFitcfgflagsustentoobligatorio)) entity.Fitcfgflagsustentoobligatorio = dr.GetString(iFitcfgflagsustentoobligatorio);

            int iFitcfginstructivo = dr.GetOrdinal(this.Fitcfginstructivo);
            if (!dr.IsDBNull(iFitcfginstructivo)) entity.Fitcfginstructivo = dr.GetString(iFitcfginstructivo);

            int iFitcfgusucreacion = dr.GetOrdinal(this.Fitcfgusucreacion);
            if (!dr.IsDBNull(iFitcfgusucreacion)) entity.Fitcfgusucreacion = dr.GetString(iFitcfgusucreacion);

            int iFitcfgfeccreacion = dr.GetOrdinal(this.Fitcfgfeccreacion);
            if (!dr.IsDBNull(iFitcfgfeccreacion)) entity.Fitcfgfeccreacion = dr.GetDateTime(iFitcfgfeccreacion);

            int iFitcfgusumodificacion = dr.GetOrdinal(this.Fitcfgusumodificacion);
            if (!dr.IsDBNull(iFitcfgusumodificacion)) entity.Fitcfgusumodificacion = dr.GetString(iFitcfgusumodificacion);

            int iFitcfgfecmodificacion = dr.GetOrdinal(this.Fitcfgfecmodificacion);
            if (!dr.IsDBNull(iFitcfgfecmodificacion)) entity.Fitcfgfecmodificacion = dr.GetDateTime(iFitcfgfecmodificacion);

            int iFtfmtcodi = dr.GetOrdinal(this.Ftfmtcodi);
            if (!dr.IsDBNull(iFtfmtcodi)) entity.Ftfmtcodi = Convert.ToInt32(dr.GetValue(iFtfmtcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Ftitcodi = "FTITCODI";
        public string Fitcfgcodi = "FITCFGCODI";
        public string Fitcfgflagcomentario = "FITCFGFLAGCOMENTARIO";
        public string Fitcfgflagvalorconf = "FITCFGFLAGVALORCONF";
        public string Fitcfgflagbloqedicion = "FITCFGFLAGBLOQEDICION";
        public string Fitcfgflagsustento = "FITCFGFLAGSUSTENTO";
        public string Fitcfgflagsustentoconf = "FITCFGFLAGSUSTENTOCONF";
        public string Fitcfgflaginstructivo = "FITCFGFLAGINSTRUCTIVO";
        public string Fitcfgflagvalorobligatorio = "FITCFGFLAGVALOROBLIGATORIO";
        public string Fitcfgflagsustentoobligatorio = "FITCFGFLAGSUSTENTOOBLIGATORIO";
        public string Fitcfginstructivo = "FITCFGINSTRUCTIVO";
        public string Fitcfgusucreacion = "FITCFGUSUCREACION";
        public string Fitcfgfeccreacion = "FITCFGFECCREACION";
        public string Fitcfgusumodificacion = "FITCFGUSUMODIFICACION";
        public string Fitcfgfecmodificacion = "FITCFGFECMODIFICACION";
        public string Ftfmtcodi = "FTFMTCODI";

        public string Ftpropcodi = "FTPROPCODI";
        public string Concepcodi = "CONCEPCODI";
        public string Propcodi = "PROPCODI";

        #endregion

        public string SqlListarPorFormato
        {
            get { return base.GetSqlXml("ListarPorFormato"); }
        }

        public string SqlEliminarPorFormato
        {
            get { return base.GetSqlXml("EliminarPorFormato"); }
        }

        public string SqlListarPorIds
        {
            get { return base.GetSqlXml("ListarPorIds"); }
        }

        

    }
}
