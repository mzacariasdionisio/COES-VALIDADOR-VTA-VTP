using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SC_EMPRESA
    /// </summary>
    public class ScEmpresaHelper : HelperBase
    {
        public ScEmpresaHelper(): base(Consultas.ScEmpresaSql)
        {
        }

        public ScEmpresaDTO Create(IDataReader dr)
        {
            ScEmpresaDTO entity = new ScEmpresaDTO();

            int iEmprcodisic = dr.GetOrdinal(this.Emprcodisic);
            if (!dr.IsDBNull(iEmprcodisic)) entity.Emprcodisic = Convert.ToInt32(dr.GetValue(iEmprcodisic));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprenomb = dr.GetOrdinal(this.Emprenomb);
            if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);

            int iEmprabrev = dr.GetOrdinal(this.Emprabrev);
            if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

            int iEmpriccppri = dr.GetOrdinal(this.Empriccppri);
            if (!dr.IsDBNull(iEmpriccppri)) entity.Empriccppri = dr.GetString(iEmpriccppri);

            int iEmpriccpsec = dr.GetOrdinal(this.Empriccpsec);
            if (!dr.IsDBNull(iEmpriccpsec)) entity.Empriccpsec = dr.GetString(iEmpriccpsec);

            int iEmpriccpconect = dr.GetOrdinal(this.Empriccpconect);
            if (!dr.IsDBNull(iEmpriccpconect)) entity.Empriccpconect = dr.GetString(iEmpriccpconect);

            int iEmpriccplastdate = dr.GetOrdinal(this.Empriccplastdate);
            if (!dr.IsDBNull(iEmpriccplastdate)) entity.Empriccplastdate = dr.GetDateTime(iEmpriccplastdate);

            int iEmprinvertrealq = dr.GetOrdinal(this.Emprinvertrealq);
            if (!dr.IsDBNull(iEmprinvertrealq)) entity.Emprinvertrealq = dr.GetString(iEmprinvertrealq);

            int iEmprinvertstateq = dr.GetOrdinal(this.Emprinvertstateq);
            if (!dr.IsDBNull(iEmprinvertstateq)) entity.Emprinvertstateq = dr.GetString(iEmprinvertstateq);

            int iEmprconec = dr.GetOrdinal(this.Emprconec);
            if (!dr.IsDBNull(iEmprconec)) entity.Emprconec = dr.GetString(iEmprconec);

            int iLinkcodi = dr.GetOrdinal(this.Linkcodi);
            if (!dr.IsDBNull(iLinkcodi)) entity.Linkcodi = Convert.ToInt32(dr.GetValue(iLinkcodi));

            int iEmprstateqgmt = dr.GetOrdinal(this.Emprstateqgmt);
            if (!dr.IsDBNull(iEmprstateqgmt)) entity.Emprstateqgmt = dr.GetString(iEmprstateqgmt);

            int iEmprrealqgmt = dr.GetOrdinal(this.Emprrealqgmt);
            if (!dr.IsDBNull(iEmprrealqgmt)) entity.Emprrealqgmt = dr.GetString(iEmprrealqgmt);

            int iEmprreenviar = dr.GetOrdinal(this.Emprreenviar);
            if (!dr.IsDBNull(iEmprreenviar)) entity.Emprreenviar = dr.GetString(iEmprreenviar);

            int iEmprlatencia = dr.GetOrdinal(this.Emprlatencia);
            if (!dr.IsDBNull(iEmprlatencia)) entity.Emprlatencia = Convert.ToInt32(dr.GetValue(iEmprlatencia));

            return entity;
        }


        #region Mapeo de Campos

        public string Emprcodisic = "EMPRCODISIC";
        public string Emprcodi = "EMPRCODI";
        public string Emprenomb = "EMPRENOMB";
        public string Emprabrev = "EMPRABREV";
        public string Empriccppri = "EMPRICCPPRI";
        public string Empriccpsec = "EMPRICCPSEC";
        public string Empriccpconect = "EMPRICCPCONECT";
        public string Empriccplastdate = "EMPRICCPLASTDATE";
        public string Emprinvertrealq = "EMPRINVERTREALQ";
        public string Emprinvertstateq = "EMPRINVERTSTATEQ";
        public string Emprconec = "EMPRCONEC";
        public string Linkcodi = "LINKCODI";
        public string Emprstateqgmt = "EMPRSTATEQGMT";
        public string Emprrealqgmt = "EMPRREALQGMT";
        public string Emprreenviar = "EMPRREENVIAR";
        public string Emprlatencia = "EMPRLATENCIA";

        #endregion



        public string SqlGetInfoScEmpresa
        {
            get { return base.GetSqlXml("GetInfoScEmpresa"); }
        }

        public string SqlGetListaScEmpresa
        {
            get { return base.GetSqlXml("GetListaScEmpresa"); }
        }


        #region FIT - SEÑALES NO DISPONIBLES - ASOCIACION EMPRESAS

        public ScEmpresaDTO CreateAsociacion(IDataReader dr)
        {
            ScEmpresaDTO entity = new ScEmpresaDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprenomb = dr.GetOrdinal(this.Emprenomb);
            if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);

            int iEmprcodisic = dr.GetOrdinal(this.Emprcodisic);
            if (!dr.IsDBNull(iEmprcodisic)) entity.Emprcodisic = Convert.ToInt32(dr.GetValue(iEmprcodisic));

            int iemprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iemprnomb)) entity.Emprnomb = dr.GetString(iemprnomb);

            int iScadacodi = dr.GetOrdinal(this.Scadacodi);
            if (!dr.IsDBNull(iScadacodi)) entity.Scadacodi = Convert.ToInt32(dr.GetValue(iScadacodi));

            int iEmprfecmodificacion = dr.GetOrdinal(this.Emprfecmodificacion);
            if (!dr.IsDBNull(iEmprfecmodificacion)) entity.Emprfecmodificacion = dr.GetDateTime(iEmprfecmodificacion);

            int iEmprusumodificacion = dr.GetOrdinal(this.Emprusumodificacion);
            if (!dr.IsDBNull(iEmprusumodificacion)) entity.Emprusumodificacion = dr.GetString(iEmprusumodificacion);


            return entity;
        }


        #region Mapeo de Campos

        public string Emprnomb = "EMPRNOMB";
        public string Scadacodi = "SCADACODI";
        public string Emprfecmodificacion = "EMPRFECMODIFICACION";
        public string Emprusumodificacion = "EMPRUSUMODIFICACION";
        #endregion        

        public string SqlObtenerBusquedaAsociocionesEmpresa
        {
            get { return base.GetSqlXml("ObtenerBusquedaAsociocionesEmpresa"); }
        }
        public string SqlGuardarAsociocionesEmpresa
        {
            get { return base.GetSqlXml("GuardarAsociocionesEmpresa"); }
        }

        public string SqlNuevoAsociacionEmpresa
        {
            get { return base.GetSqlXml("NuevoAsociacionEmpresa"); }
        }

        public string SqlEliminarAsociocionesEmpresa
        {
            get { return base.GetSqlXml("EliminarAsociocionesEmpresa"); }
        }
        #endregion

    }
}
