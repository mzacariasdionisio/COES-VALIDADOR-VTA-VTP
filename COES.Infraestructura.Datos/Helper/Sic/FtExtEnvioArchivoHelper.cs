using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_ARCHIVO
    /// </summary>
    public class FtExtEnvioArchivoHelper : HelperBase
    {
        public FtExtEnvioArchivoHelper() : base(Consultas.FtExtEnvioArchivoSql)
        {
        }

        public FtExtEnvioArchivoDTO Create(IDataReader dr)
        {
            FtExtEnvioArchivoDTO entity = new FtExtEnvioArchivoDTO();

            int iFtearccodi = dr.GetOrdinal(this.Ftearccodi);
            if (!dr.IsDBNull(iFtearccodi)) entity.Ftearccodi = Convert.ToInt32(dr.GetValue(iFtearccodi));

            int iFtearcnombreoriginal = dr.GetOrdinal(this.Ftearcnombreoriginal);
            if (!dr.IsDBNull(iFtearcnombreoriginal)) entity.Ftearcnombreoriginal = dr.GetString(iFtearcnombreoriginal);

            int iFtearcnombrefisico = dr.GetOrdinal(this.Ftearcnombrefisico);
            if (!dr.IsDBNull(iFtearcnombrefisico)) entity.Ftearcnombrefisico = dr.GetString(iFtearcnombrefisico);

            int iFtearcorden = dr.GetOrdinal(this.Ftearcorden);
            if (!dr.IsDBNull(iFtearcorden)) entity.Ftearcorden = Convert.ToInt32(dr.GetValue(iFtearcorden));

            int iFtearcestado = dr.GetOrdinal(this.Ftearcestado);
            if (!dr.IsDBNull(iFtearcestado)) entity.Ftearcestado = Convert.ToInt32(dr.GetValue(iFtearcestado));

            int iFtearcflagsustentoconf = dr.GetOrdinal(this.Ftearcflagsustentoconf);
            if (!dr.IsDBNull(iFtearcflagsustentoconf)) entity.Ftearcflagsustentoconf = dr.GetString(iFtearcflagsustentoconf);

            int iFtearctipo = dr.GetOrdinal(this.Ftearctipo);
            if (!dr.IsDBNull(iFtearctipo)) entity.Ftearctipo = Convert.ToInt32(dr.GetValue(iFtearctipo));

            return entity;
        }

        #region Mapeo de Campos

        public string Ftearccodi = "FTEARCCODI";
        public string Ftearcnombreoriginal = "FTEARCNOMBREORIGINAL";
        public string Ftearcnombrefisico = "FTEARCNOMBREFISICO";
        public string Ftearcorden = "FTEARCORDEN";
        public string Ftearcestado = "FTEARCESTADO";
        public string Ftearcflagsustentoconf = "FTEARCFLAGSUSTENTOCONF";
        public string Ftearctipo = "FTEARCTIPO";

        public string Ftereqcodi = "FTEREQCODI";
        public string Fevrqcodi = "FEVRQCODI";
        public string Fteeqcodi = "FTEEQCODI";
        public string Fterdacodi = "FTERDACODI";
        public string Ftedatcodi = "FTEDATCODI";
        public string Ftrevcodi = "FTREVCODI";

        public string Revacodi = "REVACODI";

        public string Ftevercodi = "FTEVERCODI";
        public string Faremcodi = "FAREMCODI";


        #endregion

        public string SqlListByVersionYReq
        {
            get { return GetSqlXml("ListByVersionYReq"); }
        }

        public string SqlListByVersionYEq
        {
            get { return GetSqlXml("ListByVersionYEq"); }
        }

        public string SqlListByVersionYDato
        {
            get { return GetSqlXml("ListByVersionYDato"); }
        }

        public string SqlListarPorIds
        {
            get { return GetSqlXml("ListarPorIds"); }
        }

        public string SqlListByRevision
        {
            get { return GetSqlXml("ListByRevision"); }
        }

        public string SqlListByRevisionAreas
        {
            get { return GetSqlXml("ListByRevisionAreas"); }
        }

        public string SqlDeletePorIds
        {
            get { return GetSqlXml("DeletePorIds"); }
        }

        public string SqlListByVersionAreas
        {
            get { return GetSqlXml("ListByVersionAreas"); }
        }

        public string SqlListarRelacionesPorVersionAreaYEquipo
        {
            get { return GetSqlXml("ListarRelacionesPorVersionAreaYEquipo"); }
        }

        public string SqlListarRelacionesContenidoPorVersionArea
        {
            get { return GetSqlXml("ListarRelacionesContenidoPorVersionArea"); }
        }
        

    }
}
