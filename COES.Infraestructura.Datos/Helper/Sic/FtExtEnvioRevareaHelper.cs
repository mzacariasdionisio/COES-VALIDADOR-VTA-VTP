using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_REVAREA
    /// </summary>
    public class FtExtEnvioRevareaHelper : HelperBase
    {
        public FtExtEnvioRevareaHelper(): base(Consultas.FtExtEnvioRevareaSql)
        {
        }

        public FtExtEnvioRevareaDTO Create(IDataReader dr)
        {
            FtExtEnvioRevareaDTO entity = new FtExtEnvioRevareaDTO();

            int iRevacodi = dr.GetOrdinal(this.Revacodi);
            if (!dr.IsDBNull(iRevacodi)) entity.Revacodi = Convert.ToInt32(dr.GetValue(iRevacodi));

            int iRevaestadoronda1 = dr.GetOrdinal(this.Revaestadoronda1);
            if (!dr.IsDBNull(iRevaestadoronda1)) entity.Revaestadoronda1 = dr.GetString(iRevaestadoronda1);

            int iRevahtmlronda1 = dr.GetOrdinal(this.Revahtmlronda1);
            if (!dr.IsDBNull(iRevahtmlronda1)) entity.Revahtmlronda1 = dr.GetString(iRevahtmlronda1);

            int iRevaestadoronda2 = dr.GetOrdinal(this.Revaestadoronda2);
            if (!dr.IsDBNull(iRevaestadoronda2)) entity.Revaestadoronda2 = dr.GetString(iRevaestadoronda2);

            int iRevahtmlronda2 = dr.GetOrdinal(this.Revahtmlronda2);
            if (!dr.IsDBNull(iRevahtmlronda2)) entity.Revahtmlronda2 = dr.GetString(iRevahtmlronda2);

            int iFtevercodi = dr.GetOrdinal(this.Ftevercodi);
            if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Revacodi = "REVACODI";
        public string Revaestadoronda1 = "REVAESTADORONDA1";
        public string Revahtmlronda1 = "REVAHTMLRONDA1";
        public string Revaestadoronda2 = "REVAESTADORONDA2";
        public string Revahtmlronda2 = "REVAHTMLRONDA2";
        public string Ftevercodi = "FTEVERCODI";

        
        public string Revadcodi = "REVADCODI";
        public string Ftedatcodi = "FTEDATCODI";
        public string Fteeqcodi = "FTEEQCODI";
        public string Ftereqcodi = "FTEREQCODI";
        public string Ftitcodi = "FTITCODI";
        public string Faremcodi = "FAREMCODI";
        public string Envarestado = "ENVARESTADO";
        #endregion

        public string SqlListarRevisionPorAreaVersionYDatos
        {
            get { return GetSqlXml("ListarRevisionPorAreaVersionYDatos"); }
        }
        
        public string SqlListarPorDatos
        {
            get { return GetSqlXml("ListarPorDatos"); }
        }

        public string SqlListarRevisionPorAreaVersionYReq
        {
            get { return GetSqlXml("ListarRevisionPorAreaVersionYReq"); }
        }

        public string SqlListarPorModoOp
        {
            get { return GetSqlXml("ListarPorModoOp"); }
        }

        public string SqlDeletePorGrupo
        {
            get { return GetSqlXml("DeletePorGrupo"); }
        }

        public string SqlDeletePorIds
        {
            get { return GetSqlXml("DeletePorIds"); }
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
