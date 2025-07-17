using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_MANTTO
    /// </summary>
    public class EveManttoHelper : HelperBase
    {
        public EveManttoHelper()
            : base(Consultas.EveManttoSql)
        {
        }

        public EveManttoDTO Create(IDataReader dr)
        {
            EveManttoDTO entity = new EveManttoDTO();

            int iManttocodi = dr.GetOrdinal(this.Manttocodi);
            if (!dr.IsDBNull(iManttocodi)) entity.Manttocodi = Convert.ToInt32(dr.GetValue(iManttocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iCompcode = dr.GetOrdinal(this.Compcode);
            if (!dr.IsDBNull(iCompcode)) entity.Compcode = Convert.ToInt32(dr.GetValue(iCompcode));

            int iEvenini = dr.GetOrdinal(this.Evenini);
            if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

            int iEvenpreini = dr.GetOrdinal(this.Evenpreini);
            if (!dr.IsDBNull(iEvenpreini)) entity.Evenpreini = dr.GetDateTime(iEvenpreini);

            int iEvenfin = dr.GetOrdinal(this.Evenfin);
            if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iEvenprefin = dr.GetOrdinal(this.Evenprefin);
            if (!dr.IsDBNull(iEvenprefin)) entity.Evenprefin = dr.GetDateTime(iEvenprefin);

            int iEvenmwindisp = dr.GetOrdinal(this.Evenmwindisp);
            if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

            int iEvenpadre = dr.GetOrdinal(this.Evenpadre);
            if (!dr.IsDBNull(iEvenpadre)) entity.Evenpadre = Convert.ToInt32(dr.GetValue(iEvenpadre));

            int iEvenindispo = dr.GetOrdinal(this.Evenindispo);
            if (!dr.IsDBNull(iEvenindispo)) entity.Evenindispo = dr.GetString(iEvenindispo);

            int iEveninterrup = dr.GetOrdinal(this.Eveninterrup);
            if (!dr.IsDBNull(iEveninterrup)) entity.Eveninterrup = dr.GetString(iEveninterrup);

            int iEventipoprog = dr.GetOrdinal(this.Eventipoprog);
            if (!dr.IsDBNull(iEventipoprog)) entity.Eventipoprog = dr.GetString(iEventipoprog);

            int iEvendescrip = dr.GetOrdinal(this.Evendescrip);
            if (!dr.IsDBNull(iEvendescrip)) entity.Evendescrip = dr.GetString(iEvendescrip);

            int iEvenobsrv = dr.GetOrdinal(this.Evenobsrv);
            if (!dr.IsDBNull(iEvenobsrv)) entity.Evenobsrv = dr.GetString(iEvenobsrv);

            int iEvenestado = dr.GetOrdinal(this.Evenestado);
            if (!dr.IsDBNull(iEvenestado)) entity.Evenestado = dr.GetString(iEvenestado);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iEvenrelevante = dr.GetOrdinal(this.Evenrelevante);
            if (!dr.IsDBNull(iEvenrelevante)) entity.Evenrelevante = Convert.ToInt32(dr.GetValue(iEvenrelevante));

            int iDeleted = dr.GetOrdinal(this.Deleted);
            if (!dr.IsDBNull(iDeleted)) entity.Deleted = Convert.ToInt32(dr.GetValue(iDeleted));

            int iMancodi = dr.GetOrdinal(this.Mancodi);
            if (!dr.IsDBNull(iMancodi)) entity.Mancodi = Convert.ToInt32(dr.GetValue(iMancodi));

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 20/02/2018: NUEVOS CAMPOS MAPEADOS
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            int iEquimantrelev = dr.GetOrdinal(this.Equimantrelev);
            if (!dr.IsDBNull(iEquimantrelev)) entity.Equimantrelev = dr.GetString(iEquimantrelev);

            int iMantrelevlastuser = dr.GetOrdinal(this.Mantrelevlastuser);
            if (!dr.IsDBNull(iMantrelevlastuser)) entity.Mantrelevlastuser = dr.GetString(iMantrelevlastuser);

            int iMantrelevlastdate = dr.GetOrdinal(this.Mantrelevlastdate);
            if (!dr.IsDBNull(iMantrelevlastdate)) entity.Mantrelevlastdate = dr.GetDateTime(iMantrelevlastdate);
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            return entity;
        }

        #region Mapeo de Campos
        public string Manttocodi = "MANTTOCODI";
        public string Equicodi = "EQUICODI";
        public string Evenclasecodi = "EVENCLASECODI";
        public string Tipoevencodi = "TIPOEVENCODI";
        public string Compcode = "COMPCODE";
        public string Evenini = "EVENINI";
        public string Evenpreini = "EVENPREINI";
        public string Evenfin = "EVENFIN";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Evenprefin = "EVENPREFIN";
        public string Evenmwindisp = "EVENMWINDISP";
        public string Evenpadre = "EVENPADRE";
        public string Evenindispo = "EVENINDISPO";
        public string Eveninterrup = "EVENINTERRUP";
        public string Eventipoprog = "EVENTIPOPROG";
        public string Evendescrip = "EVENDESCRIP";
        public string Evenobsrv = "EVENOBSRV";
        public string Evenestado = "EVENESTADO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Evenrelevante = "EVENRELEVANTE";
        public string Deleted = "DELETED";
        public string Mancodi = "MANCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Emprabrev = "EMPRABREV";
        public string Emprcodi = "EMPRCODI";
        public string Evenclasedesc = "EVENCLASEDESC";
        public string Areanomb = "AREANOMB";
        public string Areadesc = "AREADESC";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";
        public string Equiabrev = "EQUIABREV";
        public string Causaevenabrev = "CAUSAEVENABREV";
        public string Equitension = "EQUITENSION";
        public string Tipoevenabrev = "TIPOEVENABREV";
        public string Tipoevendesc = "TIPOEVENDESC";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Osigrupocodi = "OSIGRUPOCODI";
        public string Equipadre = "EQUIPADRE";
        public string Equinomb = "EQUINOMB";
        public string Central = "CENTRAL";
        public string Grupointegrante = "Grupointegrante";
        public string Fenergcodi = "FENERGCODI";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Eventipoindisp = "EVENTIPOINDISP";
        public string Evenpr = "EVENPR";
        public string Evenasocproc = "EVENASOCPROC";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        public string Mantipcodi = "MANTIPCODI";

        #region INTERVENCIONES       
        public string Equimantrelev = "EQUIMANTRELEV";
        public string Mantrelevlastuser = "MANTRELEVLASTUSER";
        public string Mantrelevlastdate = "MANTRELEVLASTDATE";
        public string Intercodi = "INTERCODI";   
        #endregion

        #region PR5
        public string Evenclaseabrev = "EVENCLASEABREV";
        public string Famabrev = "FAMABREV";
        #endregion

        #region SIOSEIN
        public string Subcausadesc = "SUBCAUSADESC";
        public string Tipoindisponibilidad = "TIPOINDISPONIBILIDAD";
        public string Areacodi = "AREACODI";
        public string Osinergcodi = "OSINERGCODI";
        #endregion

        public string Grupocodi = "GRUPOCODI";

        #region MigracionSGOCOES-GrupoB
        public string Tareacodi = "Tareacodi";
        public string Tipoevento = "TIPOEVENTO";
        public string Evenindispoparcial = "Evenindispoparcial";
        #endregion

        #endregion

        public string SqlMantEmpresas
        {
            get { return base.GetSqlXml("MantEmpresas"); }
        }

        public string SqlReporteMantto
        {
            get { return base.GetSqlXml("ReporteMantto"); }
        }

        public string SqlReporteManttoIndisponibilidades
        {
            get { return base.GetSqlXml("ReporteManttoIndisponibilidades"); }
        }

        public string SqlGetManttoEquipo
        {
            get { return base.GetSqlXml("GetManttoEquipo"); }
        }

        public string SqlObtenerManttoEquipoClaseFecha
        {
            get { return base.GetSqlXml("ObtenerManttoEquipoClaseFecha"); }
        }

        public string SqlObtenerManttoEquipoSubcausaClaseFecha
        {
            get { return base.GetSqlXml("ObtenerManttoEquipoSubcausaClaseFecha"); }
        }

        public string SqlObtenerManttoEquipoPadreClaseFecha
        {
            get { return base.GetSqlXml("ObtenerManttoEquipoPadreClaseFecha"); }
        }

        public string SqlObtenerMantenimientosProgramados
        {
            get { return base.GetSqlXml("ObtenerMantenimientosProgramados"); }
        }

        public string SqlObtenerMantenimientosProgramadosMovil
        {
            get { return base.GetSqlXml("ObtenerMantenimientosProgramadosMovil"); }
        }

        #region PR5
        public string SqlGetByFechaIni
        {
            get { return base.GetSqlXml("GetByFechaIni"); }
        }

        public string SqlGetIndispUniGeneracion
        {
            get { return base.GetSqlXml("GetIndispUniGeneracion"); }
        }
        #endregion

        #region SIOSEIN
        public string SqlGetListaHechosRelevantes
        {
            get { return base.GetSqlXml("GetListaHechosRelevantes"); }
        }

        #endregion


        #region INDISPONIBILIDADES

        public string SqlGetById2
        {
            get { return base.GetSqlXml("GetById2"); }
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListaManttosDigsilent
        {
            get { return base.GetSqlXml("ListaManttosDigsilent"); }
        }

        public string SqlListaMantenimientos25
        {
            get { return base.GetSqlXml("ListaMantenimientos25"); }
        }

        #endregion

        #region INTERVENCIONES
        public string SqlSaveConIntervencion
        {
            get { return base.GetSqlXml("SaveConIntervencion"); }
        }

        public string SqlDeleteByIntercodi
        {
            get { return base.GetSqlXml("DeleteByIntercodi"); }
        }

        public string SqlDeleteByPrograma
        {
            get { return base.GetSqlXml("DeleteByPrograma"); }
        }
        #endregion

        #region NET 20190225 - Cálculo de disponibilidad de las unidades de generación Hidráulico y Térmico

        public string Emprdomiciliolegal = "EMPRDOMICILIOLEGAL";
        public string Grupocodisddp = "GRUPOCODISDDP";
        public string Pmcindporcentaje = "PMCINDPORCENTAJE";
        public string Tipounidad = "TIPOUNIDAD";
        public string Gruponomb = "GRUPONOMB";

        #endregion

        #region siosein2
        public string SqlObtenerMatenimientoPorEquipoClaseFamilia
        {
            get { return base.GetSqlXml("ObtenerMatenimientoPorEquipoClaseFamilia"); }
        }

        public string SqlObtenerMatenimientoEjecutadoProgramado
        {
            get { return base.GetSqlXml("ObtenerMatenimientoEjecutadoProgramado"); }
        }
        #endregion

        #region Numerales Datos Base

        public string Dia = "DIA";
        public string Osicodi = "OSICODI";



        public string SqlDatosBase_5_6_1
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_1"); }
        }

        public string SqlDatosBase_5_6_7
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_7"); }
        }
        public string SqlDatosBase_5_6_8
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_8"); }
        }
        public string SqlDatosBase_5_6_9
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_9"); }
        }
        public string SqlDatosBase_5_6_10
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_10"); }
        }

        #endregion

    }
}
