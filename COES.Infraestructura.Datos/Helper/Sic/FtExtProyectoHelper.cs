using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_PROYECTO
    /// </summary>
    public class FtExtProyectoHelper : HelperBase
    {
        public FtExtProyectoHelper() : base(Consultas.FtExtProyectoSql)
        {
        }

        public FtExtProyectoDTO Create(IDataReader dr)
        {
            FtExtProyectoDTO entity = new FtExtProyectoDTO();

            int iFtprycodi = dr.GetOrdinal(this.Ftprycodi);
            if (!dr.IsDBNull(iFtprycodi)) entity.Ftprycodi = Convert.ToInt32(dr.GetValue(iFtprycodi));

            int iFtprynombre = dr.GetOrdinal(this.Ftprynombre);
            if (!dr.IsDBNull(iFtprynombre)) entity.Ftprynombre = dr.GetString(iFtprynombre);

            int iFtpryeonombre = dr.GetOrdinal(this.Ftpryeonombre);
            if (!dr.IsDBNull(iFtpryeonombre)) entity.Ftpryeonombre = dr.GetString(iFtpryeonombre);

            int iFtpryeocodigo = dr.GetOrdinal(this.Ftpryeocodigo);
            if (!dr.IsDBNull(iFtpryeocodigo)) entity.Ftpryeocodigo = dr.GetString(iFtpryeocodigo);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEsteocodi = dr.GetOrdinal(this.Esteocodi);
            if (!dr.IsDBNull(iEsteocodi)) entity.Esteocodi = Convert.ToInt32(dr.GetValue(iEsteocodi));

            int iFtpryestado = dr.GetOrdinal(this.Ftpryestado);
            if (!dr.IsDBNull(iFtpryestado)) entity.Ftpryestado = dr.GetString(iFtpryestado);

            int iFtpryusucreacion = dr.GetOrdinal(this.Ftpryusucreacion);
            if (!dr.IsDBNull(iFtpryusucreacion)) entity.Ftpryusucreacion = dr.GetString(iFtpryusucreacion);

            int iFtpryfeccreacion = dr.GetOrdinal(this.Ftpryfeccreacion);
            if (!dr.IsDBNull(iFtpryfeccreacion)) entity.Ftpryfeccreacion = dr.GetDateTime(iFtpryfeccreacion);

            int iFtpryusumodificacion = dr.GetOrdinal(this.Ftpryusumodificacion);
            if (!dr.IsDBNull(iFtpryusumodificacion)) entity.Ftpryusumodificacion = dr.GetString(iFtpryusumodificacion);

            int iFtpryfecmodificacion = dr.GetOrdinal(this.Ftpryfecmodificacion);
            if (!dr.IsDBNull(iFtpryfecmodificacion)) entity.Ftpryfecmodificacion = dr.GetDateTime(iFtpryfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ftprycodi = "FTPRYCODI";
        public string Ftprynombre = "FTPRYNOMBRE";
        public string Ftpryeonombre = "FTPRYEONOMBRE";
        public string Ftpryeocodigo = "FTPRYEOCODIGO";
        public string Emprcodi = "EMPRCODI";
        public string Esteocodi = "ESTEOCODI";
        public string Ftpryestado = "FTPRYESTADO";
        public string Ftpryusucreacion = "FTPRYUSUCREACION";
        public string Ftpryfeccreacion = "FTPRYFECCREACION";
        public string Ftpryusumodificacion = "FTPRYUSUMODIFICACION";
        public string Ftpryfecmodificacion = "FTPRYFECMODIFICACION";

        public string Esteocodiusu = "ESTEOCODIUSU";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string SqlListarPorRangoYEmpresa
        {
            get { return base.GetSqlXml("ListarPorRangoYEmpresa"); }
        }

        public string SqlListarProyectosSinCodigoEOPorAnio
        {
            get { return base.GetSqlXml("ListarProyectosSinCodigoEOPorAnio"); }
        }

        public string SqlListarPorEstado
        {
            get { return base.GetSqlXml("ListarPorEstado"); }
        }

        public string SqlListarGrupo
        {
            get { return base.GetSqlXml("ListarGrupo"); }
        }
        public string SqlListarPorEmpresaYEtapa
        {
            get { return base.GetSqlXml("ListarPorEmpresaYEtapa"); }
        }


    }
}
