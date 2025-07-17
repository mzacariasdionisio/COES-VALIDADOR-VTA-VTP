using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_DESTINATARIOMENSAJE
    /// </summary>
    public class InDestinatariomensajeHelper : HelperBase
    {
        public InDestinatariomensajeHelper() : base(Consultas.InDestinatariomensajeSql)
        {
        }

        public InDestinatariomensajeDTO Create(IDataReader dr)
        {
            InDestinatariomensajeDTO entity = new InDestinatariomensajeDTO();

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iIndemecodi = dr.GetOrdinal(this.Indemecodi);
            if (!dr.IsDBNull(iIndemecodi)) entity.Indemecodi = Convert.ToInt32(dr.GetValue(iIndemecodi));

            int iIndemeestado = dr.GetOrdinal(this.Indemeestado);
            if (!dr.IsDBNull(iIndemeestado)) entity.Indemeestado = dr.GetString(iIndemeestado);

            int iIndememotivobaja = dr.GetOrdinal(this.Indememotivobaja);
            if (!dr.IsDBNull(iIndememotivobaja)) entity.Indememotivobaja = dr.GetString(iIndememotivobaja);

            int iIndemeusucreacion = dr.GetOrdinal(this.Indemeusucreacion);
            if (!dr.IsDBNull(iIndemeusucreacion)) entity.Indemeusucreacion = dr.GetString(iIndemeusucreacion);

            int iIndemefeccreacion = dr.GetOrdinal(this.Indemefeccreacion);
            if (!dr.IsDBNull(iIndemefeccreacion)) entity.Indemefeccreacion = dr.GetDateTime(iIndemefeccreacion);

            int iIndemeusumodificacion = dr.GetOrdinal(this.Indemeusumodificacion);
            if (!dr.IsDBNull(iIndemeusumodificacion)) entity.Indemeusumodificacion = dr.GetString(iIndemeusumodificacion);

            int iIndemefecmodificacion = dr.GetOrdinal(this.Indemefecmodificacion);
            if (!dr.IsDBNull(iIndemefecmodificacion)) entity.Indemefecmodificacion = dr.GetDateTime(iIndemefecmodificacion);

            int iIndemevigente = dr.GetOrdinal(this.Indemevigente);
            if (!dr.IsDBNull(iIndemevigente)) entity.Indemevigente = dr.GetString(iIndemevigente);

            return entity;
        }


        #region Mapeo de Campos

        public string Usercode = "USERCODE";
        public string Emprcodi = "EMPRCODI";
        public string Indemecodi = "INDEMECODI";
        public string Indemeestado = "INDEMEESTADO";
        public string Indememotivobaja = "INDEMEMOTIVOBAJA";
        public string Indemeusucreacion = "INDEMEUSUCREACION";
        public string Indemefeccreacion = "INDEMEFECCREACION";
        public string Indemeusumodificacion = "INDEMEUSUMODIFICACION";
        public string Indemefecmodificacion = "INDEMEFECMODIFICACION";
        public string Indemevigente = "INDEMEVIGENTE";

        public string Evenclasecodi = "EVENCLASECODI";
        public string Indmdeacceso = "INDMDEACCESO";
        public string Emprnomb = "EMPRNOMB";
        public string Username = "USERNAME";
        public string Useremail = "USEREMAIL";
        public string Indmdecodi = "INDMDECODI";

        #endregion

        public string SqlObtenerConsulta
        {
            get { return base.GetSqlXml("ObtenerConsulta"); }
        }

        public string SqlObtenerHistorico
        {
            get { return base.GetSqlXml("ObtenerHistorico"); }
        }

        public string SqlObtenerConfiguracionVigente
        {
            get { return base.GetSqlXml("ObtenerConfiguracionVigente"); }
        }

        public string SqlObtenerConfiguracionVigentePorUsuario
        {
            get { return base.GetSqlXml("ObtenerConfiguracionVigentePorUsuario"); }
        }
    }
}
