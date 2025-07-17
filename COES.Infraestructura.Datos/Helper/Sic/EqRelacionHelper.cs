using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_RELACION
    /// </summary>
    public class EqRelacionHelper : HelperBase
    {
        public EqRelacionHelper()
            : base(Consultas.EqRelacionSql)
        {
        }

        public EqRelacionDTO Create(IDataReader dr)
        {
            EqRelacionDTO entity = new EqRelacionDTO();

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iRelacioncodi = dr.GetOrdinal(this.Relacioncodi);
            if (!dr.IsDBNull(iRelacioncodi)) entity.Relacioncodi = Convert.ToInt32(dr.GetValue(iRelacioncodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iCodincp = dr.GetOrdinal(this.Codincp);
            if (!dr.IsDBNull(iCodincp)) entity.Codincp = Convert.ToInt32(dr.GetValue(iCodincp));

            int iNombrencp = dr.GetOrdinal(this.Nombrencp);
            if (!dr.IsDBNull(iNombrencp)) entity.Nombrencp = dr.GetString(iNombrencp);

            int iCodbarra = dr.GetOrdinal(this.Codbarra);
            if (!dr.IsDBNull(iCodbarra)) entity.Codbarra = dr.GetString(iCodbarra);

            int iIdgener = dr.GetOrdinal(this.Idgener);
            if (!dr.IsDBNull(iIdgener)) entity.Idgener = dr.GetString(iIdgener);

            int iDescripcion = dr.GetOrdinal(this.Descripcion);
            if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

            int iNombarra = dr.GetOrdinal(this.Nombarra);
            if (!dr.IsDBNull(iNombarra)) entity.Nombarra = dr.GetString(iNombarra);

            int iEstado = dr.GetOrdinal(this.Estado);
            if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

            int iIndfuente = dr.GetOrdinal(this.Indfuente);
            if (!dr.IsDBNull(iIndfuente)) entity.Indfuente = dr.GetString(iIndfuente);

            int iNombretna = dr.GetOrdinal(this.Nombretna);
            if (!dr.IsDBNull(iNombretna)) entity.Nombretna = dr.GetString(iNombretna);

            //- Linea agregada movisoft 25.02.2021
            int iIndgeneracionrer = dr.GetOrdinal(this.Indgeneracionrer);
            if (!dr.IsDBNull(iIndgeneracionrer)) entity.Indgeneracionrer = dr.GetString(iIndgeneracionrer);


            #region Ticket 2022-004245

            int iIndnomodeladatna = dr.GetOrdinal(this.Indnomodeladatna);
            if (!dr.IsDBNull(iIndnomodeladatna)) entity.Indnomodeladatna = dr.GetString(iIndnomodeladatna);

            int iIndtnaadicional = dr.GetOrdinal(this.Indtnaadicional);
            if (!dr.IsDBNull(iIndtnaadicional)) entity.Indtnaadicional = dr.GetString(iIndtnaadicional);


            #endregion

            return entity;
        }


        #region Mapeo de Campos

        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Relacioncodi = "RELACIONCODI";
        public string Equicodi = "EQUICODI";
        public string Codincp = "CODINCP";
        public string Nombrencp = "NOMBRENCP";
        public string Codbarra = "CODBARRA";
        public string Idgener = "IDGENER";
        public string Descripcion = "DESCRIPCION";
        public string Nombarra = "NOMBARRA";
        public string Estado = "ESTADO";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Indtipo = "INDTIPO";
        public string Indcc = "INDCC";
        public string Modosoperacion = "MODOSOPERACION";
        public string Contador = "CONTADOR";
        public string Grupocodi = "GRUPOCODI";
        public string Grupopadre = "GRUPOPADRE";
        public string Ccombcodi = "CCOMBCODI";
        public string Indtvcc = "INDTVCC";
        public string Indfuente = "INDFUENTE";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Famcodi = "FAMCODI";
        public string Desubicacion = "DESUBICACION";
        public string Famnomb = "FAMNOMB";
        public string Subcausacmg = "SUBCAUSACMG";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Propcodi = "PROPCODI";
        public string Propiedad = "PROPIEDAD";
        public string Equipadre = "EQUIPADRE";
        public string VelTomaCarga = "TOMACARGA";
        public string VelReduccionCarga = "REDUCCIONCARGA";
        public string Indcoes = "INDCOES";
        public string Canalcodi = "CANALCODI";
        public string Canaliccp = "CANALICCP";
        public string Indrvarte = "INDRVARTE";
        public string Estadorvarte = "ESTADORVARTE";
        public string Gruponomb = "GRUPONOMB";
        public string Canaliccpint = "CANALICCPINT";
        public string Canalsigno = "CANALSIGNO";
        public string Canaluso = "CANALUSO";
        public string Canalcero = "CANALCERO";
        public string Tgenercodi = "TGENERCODI";
        public string Tgenernomb = "TGENERNOMB";
        public string Nombretna = "NOMBRETNA";
        public string Hoplimtrans = "HOPLIMTRANS";
        public string Indnoforzada = "INDNOFORZADA";

        //- Linea agregada movisoft 25.02.2021
        public string Indgeneracionrer = "INDGENERACIONRER";

        public string Indtnaadicional = "INDTNAADICIONAL";

        public string SqlListarEmpresas
        {
            get { return base.GetSqlXml("ListarEmpresas"); }
        }

        public string SqlListarEmpresasReservaRotante
        {
            get { return base.GetSqlXml("ListarEmpresasReservaRotante"); }
        }

        public string SqlObtenerPorEquipo
        {
            get { return base.GetSqlXml("ObtenerPorEquipo"); }
        }

        public string SqlObtenerPorEquipoReservaRotante
        {
            get { return base.GetSqlXml("ObtenerPorEquipoReservaRotante"); }
        }

        public string SqlObtenerEquipoRelacion
        {
            get { return base.GetSqlXml("ObtenerEquipoRelacion"); }
        }

        public string SqlListHidraulico
        {
            get { return base.GetSqlXml("ListHidraulico"); }
        }

        public string SqlObtenerConfiguracionProceso
        {
            get { return base.GetSqlXml("ObtenerConfiguracionProceso"); }
        }

        public string SqlObtenerContadorGrupo
        {
            get { return base.GetSqlXml("ObtenerContadorGrupo"); }
        }

        public string SqlObtenerModosOperacion
        {
            get { return base.GetSqlXml("ObtenerModosOperacion"); }
        }

        public string SqlObtenerModosOperacionEspeciales
        {
            get { return base.GetSqlXml("ObtenerModosOperacionEspeciales"); }
        }

        public string SqlObtenerUnidadesEnOperacion
        {
            get { return base.GetSqlXml("ObtenerUnidadesEnOperacion"); }
        }

        public string SqlObtenerModosOperacionLimiteTransmision
        {
            get { return base.GetSqlXml("ModosOperacionLimiteTransmision"); }
        }

        public string SqlObtenerCalificacionUnidades
        {
            get { return base.GetSqlXml("ObtenerCalificacionUnidades"); }
        }

        public string SqlObtenerModoOperacionUnidad
        {
            get { return base.GetSqlXml("ObtenerModoOperacionUnidad"); }
        }

        public string SqlObtenerConfiguracionProcesoDemanda
        {
            get { return base.GetSqlXml("ObtenerConfiguracionProcesoDemanda"); }
        }

        public string SqlObtenerRestricionOperativa
        {
            get { return base.GetSqlXml("ObtenerRestricionOperativa"); }
        }

        public string SqlObtenerPropiedadHidraulicos
        {
            get { return base.GetSqlXml("ObtenerPropiedadHidraulicos"); }
        }

        public string SqlObtenerPropiedadHidraulicosCentral
        {
            get { return base.GetSqlXml("ObtenerPropiedadHidraulicosCentral"); }
        }

        public string SqlObtenerPropiedadesHidroCM
        {
            get { return base.GetSqlXml("ObtenerPropiedadesHidroCM"); }
        }

        public string SqlObtenerPropiedadesTermoCM
        {
            get { return base.GetSqlXml("ObtenerPropiedadesTermoCM"); }
        }

        public string SqlObtenerNroUnidades
        {
            get { return base.GetSqlXml("ObtenerNroUnidades"); }
        }

        public string SqlObtenerConfiguracionReservaRotante
        {
            get { return base.GetSqlXml("ObtenerConfiguracionReservaRotante"); }
        }

        public string SqlGetByCriteriaReservaRotante
        {
            get { return base.GetSqlXml("GetByCriteriaReservaRotante"); }
        }

        public string SqlSaveReservaRotante
        {
            get { return base.GetSqlXml("SaveReservaRotante"); }
        }

        public string SqlUpdateReservaRotante
        {
            get { return base.GetSqlXml("UpdateReservaRotante"); }
        }

        public string SqlObtenerListadoReservaRotante
        {
            get { return base.GetSqlXml("ObtenerListadoReservaRotante"); }
        }

        #endregion

        #region Mejoras CMgN

        public string SqlObtenerUnidadComparativoCM
        {
            get { return base.GetSqlXml("ObtenerUnidadComparativoCM"); }
        }

        #endregion

        #region Ticket 2022-004245
        public string Indnomodeladatna = "INDNOMODELADATNA";
        #endregion
    }
}
