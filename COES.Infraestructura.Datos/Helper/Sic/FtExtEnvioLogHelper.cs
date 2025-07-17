using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_LOG
    /// </summary>
    public class FtExtEnvioLogHelper : HelperBase
    {
        public FtExtEnvioLogHelper(): base(Consultas.FtExtEnvioLogSql)
        {
        }

        public FtExtEnvioLogDTO Create(IDataReader dr)
        {
            FtExtEnvioLogDTO entity = new FtExtEnvioLogDTO();

            int iFtelogcodi = dr.GetOrdinal(this.Ftelogcodi);
            if (!dr.IsDBNull(iFtelogcodi)) entity.Ftelogcodi = Convert.ToInt32(dr.GetValue(iFtelogcodi));

            int iFtelogusucreacion = dr.GetOrdinal(this.Ftelogusucreacion);
            if (!dr.IsDBNull(iFtelogusucreacion)) entity.Ftelogusucreacion = dr.GetString(iFtelogusucreacion);

            int iFtelogfeccreacion = dr.GetOrdinal(this.Ftelogfeccreacion);
            if (!dr.IsDBNull(iFtelogfeccreacion)) entity.Ftelogfeccreacion = dr.GetDateTime(iFtelogfeccreacion);

            int iFtelogobs = dr.GetOrdinal(this.Ftelogobs);
            if (!dr.IsDBNull(iFtelogobs)) entity.Ftelogobs = dr.GetString(iFtelogobs);

            int iFtelogcondicion = dr.GetOrdinal(this.Ftelogcondicion);
            if (!dr.IsDBNull(iFtelogcondicion)) entity.Ftelogcondicion = dr.GetString(iFtelogcondicion);

            int iFtelogfecampliacion = dr.GetOrdinal(this.Ftelogfecampliacion);
            if (!dr.IsDBNull(iFtelogfecampliacion)) entity.Ftelogfecampliacion = dr.GetDateTime(iFtelogfecampliacion);

            int iEnvarcodi = dr.GetOrdinal(this.Envarcodi);
            if (!dr.IsDBNull(iEnvarcodi)) entity.Envarcodi = Convert.ToInt32(dr.GetValue(iEnvarcodi));

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int ifTenvcodi = dr.GetOrdinal(this.Ftenvcodi);
            if (!dr.IsDBNull(ifTenvcodi)) entity.Ftenvcodi = Convert.ToInt32(dr.GetValue(ifTenvcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Ftelogcodi = "FTELOGCODI";
        public string Ftelogusucreacion = "FTELOGUSUCREACION";
        public string Ftelogfeccreacion = "FTELOGFECCREACION";
        public string Ftenvcodi = "FTENVCODI";
        public string Ftelogobs = "FTELOGOBS";
        public string Estenvcodi = "ESTENVCODI";
        public string Ftelogcondicion = "FTELOGCONDICION";
        public string Envarcodi = "ENVARCODI"; 
        public string Estenvnomb = "ESTENVNOMB";
        public string Ftelogfecampliacion = "FTELOGFECAMPLIACION";
        

        public string Faremcodi = "FAREMCODI";
        public string Faremnombre = "FAREMNOMBRE";
        public string Envarfecmaxrpta = "ENVARFECMAXRPTA";
        

        #endregion

        public string SqlGetByEnviosYEstados
        {
            get { return base.GetSqlXml("GetByEnviosYEstados"); }
        }

        public string SqlGetByIdsEnvioRevisionAreas
        {
            get { return base.GetSqlXml("GetByIdsEnvioRevisionAreas"); }
        }

        public string SqlGetByIdsEnvio
        {
            get { return GetSqlXml("GetByIdsEnvio"); }
        }

        public string SqlListarLogsEnviosAmpliados
        {
            get { return GetSqlXml("ListarLogsEnviosAmpliados"); }
        }
        
    }
}
