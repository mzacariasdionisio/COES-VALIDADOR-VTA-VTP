using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_CUADRO_PROG
    /// </summary>
    public class RcaCuadroProgHelper : HelperBase
    {
        public RcaCuadroProgHelper(): base(Consultas.RcaCuadroProgSql)
        {
        }

        public RcaCuadroProgDTO Create(IDataReader dr)
        {
            RcaCuadroProgDTO entity = new RcaCuadroProgDTO();

            int iRccuadcodi = dr.GetOrdinal(this.Rccuadcodi);
            if (!dr.IsDBNull(iRccuadcodi)) entity.Rccuadcodi = Convert.ToInt32(dr.GetValue(iRccuadcodi));

            int iRcprogcodi = dr.GetOrdinal(this.Rcprogcodi);
            if (!dr.IsDBNull(iRcprogcodi)) entity.Rcprogcodi = Convert.ToInt32(dr.GetValue(iRcprogcodi));

            int iRccuadenergiaracionar = dr.GetOrdinal(this.Rccuadenergiaracionar);
            if (!dr.IsDBNull(iRccuadenergiaracionar)) entity.Rccuadenergiaracionar = dr.GetDecimal(iRccuadenergiaracionar);

            int iRccuadumbral = dr.GetOrdinal(this.Rccuadumbral);
            if (!dr.IsDBNull(iRccuadumbral)) entity.Rccuadumbral = dr.GetDecimal(iRccuadumbral);

            int iRccuadmotivo = dr.GetOrdinal(this.Rccuadmotivo);
            if (!dr.IsDBNull(iRccuadmotivo)) entity.Rccuadmotivo = dr.GetString(iRccuadmotivo);

            int iRccuadbloquehor = dr.GetOrdinal(this.Rccuadbloquehor);
            if (!dr.IsDBNull(iRccuadbloquehor)) entity.Rccuadbloquehor = dr.GetString(iRccuadbloquehor);

            int iRcconpcodi = dr.GetOrdinal(this.Rcconpcodi);
            if (!dr.IsDBNull(iRcconpcodi)) entity.Rcconpcodi = Convert.ToInt32(dr.GetValue(iRcconpcodi));

            int iRccuadflageracm = dr.GetOrdinal(this.Rccuadflageracmf);
            if (!dr.IsDBNull(iRccuadflageracm)) entity.Rccuadflageracmf = dr.GetString(iRccuadflageracm);

            int iRccuadflageracmt = dr.GetOrdinal(this.Rccuadflageracmt);
            if (!dr.IsDBNull(iRccuadflageracmt)) entity.Rccuadflageracmt = dr.GetString(iRccuadflageracmt);

            int iRccuadflagregulado = dr.GetOrdinal(this.Rccuadflagregulado);
            if (!dr.IsDBNull(iRccuadflagregulado)) entity.Rccuadflagregulado = dr.GetString(iRccuadflagregulado);

            int iRccuadfechorinicio = dr.GetOrdinal(this.Rccuadfechorinicio);
            if (!dr.IsDBNull(iRccuadfechorinicio)) entity.Rccuadfechorinicio = dr.GetDateTime(iRccuadfechorinicio);

            int iRccuadfechorfin = dr.GetOrdinal(this.Rccuadfechorfin);
            if (!dr.IsDBNull(iRccuadfechorfin)) entity.Rccuadfechorfin = dr.GetDateTime(iRccuadfechorfin);

            int iRccuadubicacion = dr.GetOrdinal(this.Rccuadubicacion);
            if (!dr.IsDBNull(iRccuadubicacion)) entity.Rccuadubicacion = dr.GetString(iRccuadubicacion);

            int iRcestacodi = dr.GetOrdinal(this.Rcestacodi);
            if (!dr.IsDBNull(iRcestacodi)) entity.Rcestacodi = dr.GetInt32(iRcestacodi);

            int iRccuadestregistro = dr.GetOrdinal(this.Rccuadestregistro);
            if (!dr.IsDBNull(iRccuadestregistro)) entity.Rccuadestregistro = dr.GetString(iRccuadestregistro);

            int iRccuadusucreacion = dr.GetOrdinal(this.Rccuadusucreacion);
            if (!dr.IsDBNull(iRccuadusucreacion)) entity.Rccuadusucreacion = dr.GetString(iRccuadusucreacion);

            int iRccuadfeccreacion = dr.GetOrdinal(this.Rccuadfeccreacion);
            if (!dr.IsDBNull(iRccuadfeccreacion)) entity.Rccuadfeccreacion = dr.GetDateTime(iRccuadfeccreacion);

            int iRccuadusumodificacion = dr.GetOrdinal(this.Rccuadusumodificacion);
            if (!dr.IsDBNull(iRccuadusumodificacion)) entity.Rccuadusumodificacion = dr.GetString(iRccuadusumodificacion);

            int iRccuadfecmodificacion = dr.GetOrdinal(this.Rccuadfecmodificacion);
            if (!dr.IsDBNull(iRccuadfecmodificacion)) entity.Rccuadfecmodificacion = dr.GetDateTime(iRccuadfecmodificacion);

            int iRccuadcodeventoctaf = dr.GetOrdinal(this.Rccuadcodeventoctaf);
            if (!dr.IsDBNull(iRccuadcodeventoctaf)) entity.Rccuadcodeventoctaf = dr.GetString(iRccuadcodeventoctaf);

            return entity;
        }


        #region Mapeo de Campos

        public string Rccuadcodi = "RCCUADCODI";
        public string Rcprogcodi = "RCPROGCODI";
        public string Rccuadenergiaracionar = "RCCUADENERGIARACIONAR";
        public string Rccuadmotivo = "RCCUADMOTIVO";
        public string Rccuadbloquehor = "RCCUADBLOQUEHOR";
        //public string Rcconpcodi = "RCCUADCONFIGURACION";
        public string Rccuadflageracmf = "RCCUADFLAGERACMF";
        public string Rccuadflageracmt = "RCCUADFLAGERACMT";
        public string Rccuadflagregulado = "RCCUADFLAGREGULADO";
        public string Rccuadfechorinicio = "RCCUADFECHORINICIO";
        public string Rccuadfechorfin = "RCCUADFECHORFIN";
        public string Rccuadestregistro = "RCCUADESTREGISTRO";
        public string Rccuadusucreacion = "RCCUADUSUCREACION";
        public string Rccuadfeccreacion = "RCCUADFECCREACION";
        public string Rccuadusumodificacion = "RCCUADUSUMODIFICACION";
        public string Rccuadfecmodificacion = "RCCUADFECMODIFICACION";
        public string Rchorpcodi = "RCHORPCODI";
        public string Rcprognombre = "RCPROGNOMBRE";

        public string Rchorpnombre = "RCHORPNOMBRE";

        public string Rcconpcodi = "RCCONPCODI";
        public string Rcconpnombre = "RCCONPNOMBRE";

        public string Rccuadubicacion = "RCCUADUBICACION";
        public string Rcprogabrev = "RCPROGABREV";

        public string Rccuadumbral = "RCCUADUMBRAL";

        public string Rccuadfechorinicioejec = "RCCUADFECHORINICIOEJEC";
        public string Rccuadfechorfinejec = "RCCUADFECHORFINEJEC";

        public string Rcestacodi = "RCESTACODI";
        public string Rcestanombre = "RCESTANOMBRE";
        public string Rccuadcodeventoctaf = "RCCUADCODEVENTOCTAF";

        #endregion

        public string SqlListCuadroProgFiltro
        {
            get { return GetSqlXml("ListCuadroProgFiltro"); }
        }
        public string SqlListConfiguracionPrograma
        {
            get { return GetSqlXml("ListConfiguracionPrograma"); }
        }
        public string SqlListHorizontePrograma
        {
            get { return GetSqlXml("ListHorizontePrograma"); }
        }

        public string SqlListCuadroEnvioArchivoPorPrograma
        {
            get { return GetSqlXml("ListCuadroEnvioArchivoPorPrograma"); }
        }

        public string SqlUpdateCuadroProgramaEjecucion
        {
            get { return GetSqlXml("UpdateCuadroProgramaEjecucion"); }
        }
        public string SqlListEstadoCuadroPrograma
        {
            get { return GetSqlXml("ListEstadoCuadroPrograma"); }
        }
        public string SqlUpdateCuadroEstado
        {
            get { return GetSqlXml("UpdateCuadroEstado"); }
        }

        public string SqlUpdateCuadroEvento
        {
            get { return GetSqlXml("UpdateCuadroEvento"); }
        }
    }
}
