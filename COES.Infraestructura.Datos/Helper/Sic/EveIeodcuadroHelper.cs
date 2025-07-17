using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_IEODCUADRO
    /// </summary>
    public class EveIeodcuadroHelper : HelperBase
    {
        public EveIeodcuadroHelper(): base(Consultas.EveIeodcuadroSql)
        {
        }

        public EveIeodcuadroDTO Create(IDataReader dr)
        {
            EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

            int iIccodi = dr.GetOrdinal(this.Iccodi);
            if (!dr.IsDBNull(iIccodi)) entity.Iccodi = Convert.ToInt32(dr.GetValue(iIccodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iIctipcuadro = dr.GetOrdinal(this.Ictipcuadro);
            if (!dr.IsDBNull(iIctipcuadro)) entity.Ichorinicarga = dr.GetDateTime(iIctipcuadro);

            int iIchorinicarga = dr.GetOrdinal(this.Ichorinicarga);
            if (!dr.IsDBNull(iIchorinicarga)) entity.Ichorinicarga = dr.GetDateTime(iIchorinicarga);

            int iIchorini = dr.GetOrdinal(this.Ichorini);
            if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

            int iIchorfin = dr.GetOrdinal(this.Ichorfin);
            if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

            int iIcdescrip1 = dr.GetOrdinal(this.Icdescrip1);
            if (!dr.IsDBNull(iIcdescrip1)) entity.Icdescrip1 = dr.GetString(iIcdescrip1);

            int iIcdescrip2 = dr.GetOrdinal(this.Icdescrip2);
            if (!dr.IsDBNull(iIcdescrip2)) entity.Icdescrip2 = dr.GetString(iIcdescrip2);

            int iIcdescrip3 = dr.GetOrdinal(this.Icdescrip3);
            if (!dr.IsDBNull(iIcdescrip3)) entity.Icdescrip3 = dr.GetString(iIcdescrip3);

            int iIccheck1 = dr.GetOrdinal(this.Iccheck1);
            if (!dr.IsDBNull(iIccheck1)) entity.Iccheck1 = dr.GetString(iIccheck1);

            int iIcvalor1 = dr.GetOrdinal(this.Icvalor1);
            if (!dr.IsDBNull(iIcvalor1)) entity.Icvalor1 = dr.GetDecimal(iIcvalor1);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iNumtrsgsubit = dr.GetOrdinal(this.Numtrsgsubit);
            if (!dr.IsDBNull(iNumtrsgsubit)) entity.Numtrsgsubit = dr.GetDecimal(iNumtrsgsubit);

            int iNumtrsgsostn = dr.GetOrdinal(this.Numtrsgsostn);
            if (!dr.IsDBNull(iNumtrsgsostn)) entity.Numtrsgsostn = dr.GetDecimal(iNumtrsgsostn);

            int iIccheck2 = dr.GetOrdinal(this.Iccheck2);
            if (!dr.IsDBNull(iIccheck2)) entity.Iccheck2 = dr.GetString(iIccheck2);

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iIchor3 = dr.GetOrdinal(this.Ichor3);
            if (!dr.IsDBNull(iIchor3)) entity.Ichor3 = dr.GetDateTime(iIchor3);

            int iIchor4 = dr.GetOrdinal(this.Ichor4);
            if (!dr.IsDBNull(iIchor4)) entity.Ichor4 = dr.GetDateTime(iIchor4);

            int iIccheck3 = dr.GetOrdinal(this.Iccheck3);
            if (!dr.IsDBNull(iIccheck3)) entity.Iccheck3 = dr.GetString(iIccheck3);

            int iIccheck4 = dr.GetOrdinal(this.Iccheck4);
            if (!dr.IsDBNull(iIccheck4)) entity.Iccheck4 = dr.GetString(iIccheck4);

            int iIcvalor2 = dr.GetOrdinal(this.Icvalor2);
            if (!dr.IsDBNull(iIcvalor2)) entity.Icvalor2 = dr.GetDecimal(iIcvalor2);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt16(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }


        #region Mapeo de Campos

        public string Iccodi = "ICCODI";
        public string Equicodi = "EQUICODI";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Ictipcuadro = "ICTIPCUADRO";
        public string Ichorinicarga = "ICHORINICARGA";
        public string Ichorini = "ICHORINI";
        public string Ichorfin = "ICHORFIN";
        public string Icdescrip1 = "ICDESCRIP1";
        public string Icdescrip2 = "ICDESCRIP2";
        public string Icdescrip3 = "ICDESCRIP3";
        public string Iccheck1 = "ICCHECK1";
        public string Icvalor1 = "ICVALOR1";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Numtrsgsubit = "NUMTRSGSUBIT";
        public string Numtrsgsostn = "NUMTRSGSOSTN";
        public string Iccheck2 = "ICCHECK2";
        public string Evenclasecodi = "EVENCLASECODI";
        public string Ichor3 = "ICHOR3";
        public string Ichor4 = "ICHOR4";
        public string Emprnomb = "EMPRNOMB";
        public string Areanomb = "AREANOMB";
        public string Famabrev = "FAMABREV";
        public string Equiabrev = "EQUIABREV";
        public string Subcausadesc = "SUBCAUSADESC";
        public string Iccheck3 = "ICCHECK3";
        public string Iccheck4 = "ICCHECK4";
        public string Icvalor2 = "ICVALOR2";
        public string DemHP = "DEMHP";
        public string DemFP = "DEMFP";
        public string Equinomb = "EQUINOMB";
        public string Emprcodi = "EMPRCODI";
        public string Tareaabrev = "TAREAABREV";
        public string Icnombarchenvio = "ICNOMBARCHENVIO";
        public string Icnombarchfisico = "ICNOMBARCHFISICO";
        public string Icestado = "ICESTADO";
        #region PR5
        public string Areacodi = "AREACODI";
        public string Areaabrev = "AREAABREV";
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string Emprabrev = "Emprabrev";
        #endregion

        #region Indisponibilidades
        public string Famcodi = "FAMCODI";
        public string Areadesc = "AREADESC";
        #endregion

        #endregion


        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string ObtenerListadoDetallado
        {
            get { return base.GetSqlXml("ObtenerListadoDetallado"); }
        }

        public string ObtenerListadoSinPaginado
        {
            get { return base.GetSqlXml("ObtenerListadoSinPaginado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string ObtenerIeodCuadro
        {
            get { return base.GetSqlXml("ObtenerIeodCuadro"); }
        }
        
        public string ObtenerDatosEquipo
        {
            get { return base.GetSqlXml("ObtenerDatosEquipo"); }
        }

        public string SqlListarEveIeodCuadroxEmpresa
        {
            get { return base.GetSqlXml("ListarEveIeodCuadroxEmpresa"); }
        }

        public string SqlGetCriteriaxPKCodis
        {
            get { return base.GetSqlXml("GetCriteriaxPKCodis"); }
        }

        public string SqlBorradoLogico
        {
            get { return base.GetSqlXml("BorradoLogico"); }
        }

        #region PR5
        public string SqlListarEveIeodCuadroxEmpresaxEquipos
        {
            get { return base.GetSqlXml("ListarEveIeodCuadroxEmpresaxEquipos"); }
        }

        public string SqlObtenerNroElementosConsultaRestricciones
        {
            get { return base.GetSqlXml("NroRegistrosConsultaRestricciones"); }
        }

        public string SqlListarReporteOperacionVaria
        {
            get { return base.GetSqlXml("ListarReporteOperacionVaria"); }
        }

        public string SqlContarEveIeodCuadroxEmpresaxEquipos
        {
            get { return base.GetSqlXml("ContarEveIeodCuadroxEmpresaxEquipos"); }
        }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListaBitacora
        {
            get { return base.GetSqlXml("ListaBitacora"); }
        }

        public string SqlListaReqPropios
        {
            get { return base.GetSqlXml("ListaReqPropios"); }
        }
        #endregion
    }
}
