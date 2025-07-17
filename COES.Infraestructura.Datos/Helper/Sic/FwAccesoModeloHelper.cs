using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FW_ACCESO_MODELO
    /// </summary>
    public class FwAccesoModeloHelper : HelperBase
    {
        public FwAccesoModeloHelper(): base(Consultas.FwAccesoModeloSql)
        {
        }

        public FwAccesoModeloDTO Create(IDataReader dr)
        {
            FwAccesoModeloDTO entity = new FwAccesoModeloDTO();

            int iAcmodcodi = dr.GetOrdinal(this.Acmodcodi);
            if (!dr.IsDBNull(iAcmodcodi)) entity.Acmodcodi = Convert.ToInt32(dr.GetValue(iAcmodcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iAcmodfecinicio = dr.GetOrdinal(this.Acmodfecinicio);
            if (!dr.IsDBNull(iAcmodfecinicio)) entity.Acmodfecinicio = dr.GetDateTime(iAcmodfecinicio);

            int iAcmodfin = dr.GetOrdinal(this.Acmodfin);
            if (!dr.IsDBNull(iAcmodfin)) entity.Acmodfin = dr.GetDateTime(iAcmodfin);

            int iAcmodestado = dr.GetOrdinal(this.Acmodestado);
            if (!dr.IsDBNull(iAcmodestado)) entity.Acmodestado = dr.GetString(iAcmodestado);

            int iAcmodkey = dr.GetOrdinal(this.Acmodkey);
            if (!dr.IsDBNull(iAcmodkey)) entity.Acmodkey = dr.GetString(iAcmodkey);

            int iAcmodnrointentos = dr.GetOrdinal(this.Acmodnrointentos);
            if (!dr.IsDBNull(iAcmodnrointentos)) entity.Acmodnrointentos = Convert.ToInt32(dr.GetValue(iAcmodnrointentos));

            int iEmpcorcodi = dr.GetOrdinal(this.Empcorcodi);
            if (!dr.IsDBNull(iEmpcorcodi)) entity.Empcorcodi = Convert.ToInt32(dr.GetValue(iEmpcorcodi));

            int iAcmodusucreacion = dr.GetOrdinal(this.Acmodusucreacion);
            if (!dr.IsDBNull(iAcmodusucreacion)) entity.Acmodusucreacion = dr.GetString(iAcmodusucreacion);

            int iAcmodfeccreacion = dr.GetOrdinal(this.Acmodfeccreacion);
            if (!dr.IsDBNull(iAcmodfeccreacion)) entity.Acmodfeccreacion = dr.GetDateTime(iAcmodfeccreacion);

            int iAcmodusumodificacion = dr.GetOrdinal(this.Acmodusumodificacion);
            if (!dr.IsDBNull(iAcmodusumodificacion)) entity.Acmodusumodificacion = dr.GetString(iAcmodusumodificacion);

            int iAcmodfecmodificacion = dr.GetOrdinal(this.Acmodfecmodificacion);
            if (!dr.IsDBNull(iAcmodfecmodificacion)) entity.Acmodfecmodificacion = dr.GetDateTime(iAcmodfecmodificacion);

            int iAcmodveces = dr.GetOrdinal(this.Acmodveces);
            if (!dr.IsDBNull(iAcmodveces)) entity.Acmodveces = Convert.ToInt32(dr.GetValue(iAcmodveces));

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Acmodcodi = "ACMODCODI";
        public string Emprcodi = "EMPRCODI";
        public string Acmodfecinicio = "ACMODFECINICIO";
        public string Acmodfin = "ACMODFIN";
        public string Acmodestado = "ACMODESTADO";
        public string Acmodkey = "ACMODKEY";
        public string Acmodnrointentos = "ACMODNROINTENTOS";
        public string Empcorcodi = "EMPCORCODI";
        public string Acmodusucreacion = "ACMODUSUCREACION";
        public string Acmodfeccreacion = "ACMODFECCREACION";
        public string Acmodusumodificacion = "ACMODUSUMODIFICACION";
        public string Acmodfecmodificacion = "ACMODFECMODIFICACION";
        public string Acmodveces = "ACMODVECES";
        public string Modcodi = "MODCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Modnomb = "MODNOMB";
        public string Contactonomb = "CONTACNOMB";
        public string Contactocorreo = "CONTACCORREO";

        public string SqlUpdateClave
        {
            get { return base.GetSqlXml("UpdateClave"); }
        }

        public string SqlDeletePorContacto
        {
            get { return base.GetSqlXml("DeletePorContacto"); }
        }

        #endregion
    }
}
