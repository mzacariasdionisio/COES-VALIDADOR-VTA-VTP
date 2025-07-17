using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_VERSION
    /// </summary>
    public class FtExtEnvioVersionHelper : HelperBase
    {
        public FtExtEnvioVersionHelper() : base(Consultas.FtExtEnvioVersionSql)
        {
        }

        public FtExtEnvioVersionDTO Create(IDataReader dr)
        {
            FtExtEnvioVersionDTO entity = new FtExtEnvioVersionDTO();

            int iFtevercodi = dr.GetOrdinal(this.Ftevercodi);
            if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

            int iFtevertipo = dr.GetOrdinal(this.Ftevertipo);
            if (!dr.IsDBNull(iFtevertipo)) entity.Ftevertipo = Convert.ToInt32(dr.GetValue(iFtevertipo));

            int iFteveroperacion = dr.GetOrdinal(this.Fteveroperacion);
            if (!dr.IsDBNull(iFteveroperacion)) entity.Fteveroperacion = Convert.ToInt32(dr.GetValue(iFteveroperacion));

            int iFteverdescripcion = dr.GetOrdinal(this.Fteverdescripcion);
            if (!dr.IsDBNull(iFteverdescripcion)) entity.Fteverdescripcion = dr.GetString(iFteverdescripcion);

            int iFteverconexion = dr.GetOrdinal(this.Fteverconexion);
            if (!dr.IsDBNull(iFteverconexion)) entity.Fteverconexion = Convert.ToInt32(dr.GetValue(iFteverconexion));

            int iFteverestado = dr.GetOrdinal(this.Fteverestado);
            if (!dr.IsDBNull(iFteverestado)) entity.Fteverestado = dr.GetString(iFteverestado);

            int iFteverautoguardado = dr.GetOrdinal(this.Fteverautoguardado);
            if (!dr.IsDBNull(iFteverautoguardado)) entity.Fteverautoguardado = dr.GetString(iFteverautoguardado);

            int iFteverfeccreacion = dr.GetOrdinal(this.Fteverfeccreacion);
            if (!dr.IsDBNull(iFteverfeccreacion)) entity.Fteverfeccreacion = dr.GetDateTime(iFteverfeccreacion);

            int iFteverusucreacion = dr.GetOrdinal(this.Fteverusucreacion);
            if (!dr.IsDBNull(iFteverusucreacion)) entity.Fteverusucreacion = dr.GetString(iFteverusucreacion);

            int iFtenvcodi = dr.GetOrdinal(this.Ftenvcodi);
            if (!dr.IsDBNull(iFtenvcodi)) entity.Ftenvcodi = Convert.ToInt32(dr.GetValue(iFtenvcodi));

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Ftevercodi = "FTEVERCODI";
        public string Fteverestado = "FTEVERESTADO";
        public string Fteverfeccreacion = "FTEVERFECCREACION";
        public string Fteverusucreacion = "FTEVERUSUCREACION";
        public string Ftenvcodi = "FTENVCODI";
        public string Ftevertipo = "Ftevertipo";
        public string Fteverdescripcion = "FTEVERDESCRIPCION";
        public string Fteverconexion = "Fteverconexion";
        public string Fteveroperacion = "Fteveroperacion";
        public string Fteverautoguardado = "Fteverautoguardado";
        public string Estenvcodi = "ESTENVCODI";

        #endregion

        public string SqlUpdateListaVersion
        {
            get { return GetSqlXml("UpdateListaVersion"); }
        }

    }
}
