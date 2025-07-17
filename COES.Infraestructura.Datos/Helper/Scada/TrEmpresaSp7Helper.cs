using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_EMPRESA_SP7
    /// </summary>
    public class TrEmpresaSp7Helper : HelperBase
    {
        public TrEmpresaSp7Helper()
            : base(Consultas.TrEmpresaSp7Sql)
        {
        }

        public TrEmpresaSp7DTO Create(IDataReader dr)
        {
            TrEmpresaSp7DTO entity = new TrEmpresaSp7DTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprenomb = dr.GetOrdinal(this.Emprenomb);
            if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);

            int iEmprabrev = dr.GetOrdinal(this.Emprabrev);
            if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

            int iEmprsiid = dr.GetOrdinal(this.Emprsiid);
            if (!dr.IsDBNull(iEmprsiid)) entity.Emprsiid = Convert.ToInt32(dr.GetValue(iEmprsiid));

            int iEmprusucreacion = dr.GetOrdinal(this.Emprusucreacion);
            if (!dr.IsDBNull(iEmprusucreacion)) entity.Emprusucreacion = dr.GetString(iEmprusucreacion);

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

            int iEmprfeccreacion = dr.GetOrdinal(this.Emprfeccreacion);
            if (!dr.IsDBNull(iEmprfeccreacion)) entity.Emprfeccreacion = dr.GetDateTime(iEmprfeccreacion);

            int iEmprusumodificacion = dr.GetOrdinal(this.Emprusumodificacion);
            if (!dr.IsDBNull(iEmprusumodificacion)) entity.Emprusumodificacion = dr.GetString(iEmprusumodificacion);

            int iEmprfecmodificacion = dr.GetOrdinal(this.Emprfecmodificacion);
            if (!dr.IsDBNull(iEmprfecmodificacion)) entity.Emprfecmodificacion = dr.GetDateTime(iEmprfecmodificacion);

            return entity;
        }

        public TrEmpresaSp7DTO CreateFromTrcoes(IDataReader dr)
        {
            TrEmpresaSp7DTO entity = new TrEmpresaSp7DTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprenomb = dr.GetOrdinal(this.Emprenomb);
            if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);

            int iEmprabrev = dr.GetOrdinal(this.Emprabrev);
            if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

            int iEmprsiid = dr.GetOrdinal(this.Emprsiid);
            if (!dr.IsDBNull(iEmprsiid)) entity.Emprsiid = Convert.ToInt32(dr.GetValue(iEmprsiid));

            int iEmprusucreacion = dr.GetOrdinal(this.Emprusucreacion);
            if (!dr.IsDBNull(iEmprusucreacion)) entity.Emprusucreacion = dr.GetString(iEmprusucreacion);

            int iEmprfeccreacion = dr.GetOrdinal(this.Emprfeccreacion);
            if (!dr.IsDBNull(iEmprfeccreacion)) entity.Emprfeccreacion = dr.GetDateTime(iEmprfeccreacion);

            int iEmprusumodificacion = dr.GetOrdinal(this.Emprusumodificacion);
            if (!dr.IsDBNull(iEmprusumodificacion)) entity.Emprusumodificacion = dr.GetString(iEmprusumodificacion);

            int iEmprfecmodificacion = dr.GetOrdinal(this.Emprfecmodificacion);
            if (!dr.IsDBNull(iEmprfecmodificacion)) entity.Emprfecmodificacion = dr.GetDateTime(iEmprfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Emprcodi = "EMPRCODI";
        public string Emprenomb = "EMPRENOMB";
        public string Emprabrev = "EMPRABREV";
        public string Emprsiid = "EMPRSIID";
        public string Emprusucreacion = "EMPRUSUCREACION";
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
        public string Emprfeccreacion = "EMPRFECCREACION";
        public string Emprusumodificacion = "EMPRUSUMODIFICACION";
        public string Emprfecmodificacion = "EMPRFECMODIFICACION";

        #endregion

        public string SqlActualizarNombreEmpresa
        {
            get { return base.GetSqlXml("ActualizarNombreEmpresa"); }
        }


        #region Mejoras IEOD

        public string SqlListarEmpresaCanal
        {
            get { return base.GetSqlXml("ListarEmpresaCanal"); }
        }

        public string SqlListarEmpresaCanalBdTreal
        {
            get { return base.GetSqlXml("ListarEmpresaCanalBdTreal"); }
        }

        #endregion
    }
}
