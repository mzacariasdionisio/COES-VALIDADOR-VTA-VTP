using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_FAMREL
    /// </summary>
    public class EqFamrelHelper : HelperBase
    {
        public EqFamrelHelper(): base(Consultas.EqFamrelSql)
        {
        }

        public EqFamrelDTO Create(IDataReader dr)
        {
            EqFamrelDTO entity = new EqFamrelDTO();

            int iTiporelcodi = dr.GetOrdinal(this.Tiporelcodi);
            if (!dr.IsDBNull(iTiporelcodi)) entity.Tiporelcodi = Convert.ToInt32(dr.GetValue(iTiporelcodi));

            int iFamcodi1 = dr.GetOrdinal(this.Famcodi1);
            if (!dr.IsDBNull(iFamcodi1)) entity.Famcodi1 = Convert.ToInt32(dr.GetValue(iFamcodi1));

            int iFamcodi2 = dr.GetOrdinal(this.Famcodi2);
            if (!dr.IsDBNull(iFamcodi2)) entity.Famcodi2 = Convert.ToInt32(dr.GetValue(iFamcodi2));

            int iFamnumconec = dr.GetOrdinal(this.Famnumconec);
            if (!dr.IsDBNull(iFamnumconec)) entity.Famnumconec = Convert.ToInt32(dr.GetValue(iFamnumconec));

            int iFamreltension = dr.GetOrdinal(this.Famreltension);
            if (!dr.IsDBNull(iFamreltension)) entity.Famreltension = dr.GetString(iFamreltension);

            int iFamrelestado = dr.GetOrdinal(this.Famrelestado);
            if (!dr.IsDBNull(iFamrelestado)) entity.Famrelestado = dr.GetString(iFamrelestado);

            int iFamrelusuariocreacion = dr.GetOrdinal(this.Famrelusuariocreacion);
            if (!dr.IsDBNull(iFamrelusuariocreacion)) entity.Famrelusuariocreacion = dr.GetString(iFamrelusuariocreacion);

            int iFamrelfechacreacion = dr.GetOrdinal(this.Famrelfechacreacion);
            if (!dr.IsDBNull(iFamrelfechacreacion)) entity.Famrelfechacreacion = dr.GetDateTime(iFamrelfechacreacion);

            int iFamrelusuarioupdate = dr.GetOrdinal(this.Famrelusuarioupdate);
            if (!dr.IsDBNull(iFamrelusuarioupdate)) entity.Famrelusuarioupdate = dr.GetString(iFamrelusuarioupdate);

            int iFamrelfechaupdate = dr.GetOrdinal(this.Famrelfechaupdate);
            if (!dr.IsDBNull(iFamrelfechaupdate)) entity.Famrelfechaupdate = dr.GetDateTime(iFamrelfechaupdate);

            return entity;
        }


        #region Mapeo de Campos
        
        public string Tiporelcodi = "TIPORELCODI";
        public string Famcodi1 = "FAMCODI1";
        public string Famcodi2 = "FAMCODI2";
        public string Famcodi1old = "FAMCODI1OLD";
        public string Famcodi2old = "FAMCODI2OLD";
        public string Famnumconec = "FAMNUMCONEC";
        public string Famreltension = "FAMRELTENSION";
        public string Famrelestado = "FAMRELESTADO";
        public string Famrelusuariocreacion = "FAMRELUSUARIOCREACION";
        public string Famrelfechacreacion = "FAMRELFECHACREACION";
        public string Famrelusuarioupdate = "FAMRELUSUARIOUPDATE";
        public string Famrelfechaupdate = "FAMRELFECHAUPDATE";

        #endregion

        public string SqlGetByTipoRel
        {
            get { return base.GetSqlXml("GetByTipoRel"); }
        }
    }
}

