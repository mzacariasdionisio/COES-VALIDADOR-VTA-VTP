using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_MEDIDORES_VALIDACION
    /// </summary>
    public class WbMedidoresValidacionHelper : HelperBase
    {
        public WbMedidoresValidacionHelper()
            : base(Consultas.WbMedidoresValidacionSql)
        {
        }

        public WbMedidoresValidacionDTO Create(IDataReader dr)
        {
            WbMedidoresValidacionDTO entity = new WbMedidoresValidacionDTO();

            int iMedivalcodi = dr.GetOrdinal(this.Medivalcodi);
            if (!dr.IsDBNull(iMedivalcodi)) entity.Medivalcodi = Convert.ToInt32(dr.GetValue(iMedivalcodi));

            int iPtomedicodimed = dr.GetOrdinal(this.Ptomedicodimed);
            if (!dr.IsDBNull(iPtomedicodimed)) entity.Ptomedicodimed = Convert.ToInt32(dr.GetValue(iPtomedicodimed));

            int iPtomedicodidesp = dr.GetOrdinal(this.Ptomedicodidesp);
            if (!dr.IsDBNull(iPtomedicodidesp)) entity.Ptomedicodidesp = Convert.ToInt32(dr.GetValue(iPtomedicodidesp));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iIndestado = dr.GetOrdinal(this.Indestado);
            if (!dr.IsDBNull(iIndestado)) entity.Indestado = dr.GetString(iIndestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Medivalcodi = "MEDIVALCODI";
        public string Ptomedicodimed = "PTOMEDICODIMED";
        public string Ptomedicodidesp = "PTOMEDICODIDESP";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Indestado = "INDESTADO";
        public string EmprNomb = "EMPRNOMB";
        public string Grupocodi = "GRUPOCODI";
        public string GrupoNomb = "GRUPONOMB";
        public string GrupoAbrev = "GRUPOABREV";
        public string EmprCodi = "EMPRCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Central = "CENTRAL";
        public string Equinomb = "EQUINOMB";

        public string Centralmed = "CENTRALMED";
        public string Equipomed = "EQUIPOMED";
        public string Empresamed = "EMPRESAMED";
        public string Centraldesp = "CENTRALDESP";
        public string Equipodesp = "EQUIPODESP";
        public string Empresadesp = "EMPRESADESP";

        public string Ptomediestado = "PTOMEDIESTADO";

        public string SqlObtenerPuntosPorEmpresa
        {
            get { return base.GetSqlXml("ObtenerPuntosPorEmpresa"); }
        }

        public string SqlValidarExistencia
        {
            get { return base.GetSqlXml("ValidarExistencia"); }
        }

        public string SqlObtenerRelaciones
        {
            get { return base.GetSqlXml("ObtenerRelaciones"); }
        }

        public string SqlObtenerEmpresaGrafico
        {
            get { return base.GetSqlXml("ObtenerEmpresaGrafico"); }
        }

        public string SqlObtenerGrupoGrafico
        {
            get { return base.GetSqlXml("ObtenerGrupoGrafico"); }
        }

        #endregion
    }
}
