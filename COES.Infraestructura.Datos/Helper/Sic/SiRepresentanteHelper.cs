using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_REPRESENTANTE
    /// </summary>
    public class SiRepresentanteHelper : HelperBase
    {
        public SiRepresentanteHelper() : base(Consultas.SiRepresentanteSql)
        {
        }

        public SiRepresentanteDTO Create(IDataReader dr)
        {
            SiRepresentanteDTO entity = new SiRepresentanteDTO();

            int iRptecodi = dr.GetOrdinal(this.Rptecodi);
            if (!dr.IsDBNull(iRptecodi)) entity.Rptecodi = Convert.ToInt32(dr.GetValue(iRptecodi));

            int iRptetipo = dr.GetOrdinal(this.Rptetipo);
            if (!dr.IsDBNull(iRptetipo)) entity.Rptetipo = dr.GetString(iRptetipo).Trim();

            int iRptetiprepresentantelegal = dr.GetOrdinal(this.Rptetiprepresentantelegal);
            if (!dr.IsDBNull(iRptetiprepresentantelegal)) entity.Rptetiprepresentantelegal = dr.GetString(iRptetiprepresentantelegal);

            int iRptebaja = dr.GetOrdinal(this.Rptebaja);
            if (!dr.IsDBNull(iRptebaja)) entity.Rptebaja = dr.GetString(iRptebaja);

            int iRpteinicial = dr.GetOrdinal(this.Rpteinicial);
            if (!dr.IsDBNull(iRpteinicial)) entity.Rpteinicial = dr.GetString(iRpteinicial);            

            int iRptetipdocidentidad = dr.GetOrdinal(this.Rptetipdocidentidad);
            if (!dr.IsDBNull(iRptetipdocidentidad)) entity.Rptetipdocidentidad = dr.GetString(iRptetipdocidentidad);

            int iRptedocidentidad = dr.GetOrdinal(this.Rptedocidentidad);
            if (!dr.IsDBNull(iRptedocidentidad)) entity.Rptedocidentidad = dr.GetString(iRptedocidentidad);

            int iRptedocidentidadadj = dr.GetOrdinal(this.Rptedocidentidadadj);
            if (!dr.IsDBNull(iRptedocidentidadadj)) entity.Rptedocidentidadadj = dr.GetString(iRptedocidentidadadj);

            int iRptedocidentidadfilename = dr.GetOrdinal(this.Rptedocidentidadadjfilename);
            if (!dr.IsDBNull(iRptedocidentidadfilename)) entity.Rptedocidentidadadjfilename = dr.GetString(iRptedocidentidadfilename);

            int iRptenombres = dr.GetOrdinal(this.Rptenombres);
            if (!dr.IsDBNull(iRptenombres)) entity.Rptenombres = dr.GetString(iRptenombres);

            int iRpteapellidos = dr.GetOrdinal(this.Rpteapellidos);
            if (!dr.IsDBNull(iRpteapellidos)) entity.Rpteapellidos = dr.GetString(iRpteapellidos);

            int iRptevigenciapoder = dr.GetOrdinal(this.Rptevigenciapoder);
            if (!dr.IsDBNull(iRptevigenciapoder)) entity.Rptevigenciapoder = dr.GetString(iRptevigenciapoder);

            int iRptevigenciapoderfilename = dr.GetOrdinal(this.Rptevigenciapoderfilename);
            if (!dr.IsDBNull(iRptevigenciapoderfilename)) entity.Rptevigenciapoderfilename = dr.GetString(iRptevigenciapoderfilename);

            int iRptecargoempresa = dr.GetOrdinal(this.Rptecargoempresa);
            if (!dr.IsDBNull(iRptecargoempresa)) entity.Rptecargoempresa = dr.GetString(iRptecargoempresa);

            int iRptetelefono = dr.GetOrdinal(this.Rptetelefono);
            if (!dr.IsDBNull(iRptetelefono)) entity.Rptetelefono = dr.GetString(iRptetelefono);

            int iRptetelfmovil = dr.GetOrdinal(this.Rptetelfmovil);
            if (!dr.IsDBNull(iRptetelfmovil)) entity.Rptetelfmovil = dr.GetString(iRptetelfmovil);

            int iRptecorreoelectronico = dr.GetOrdinal(this.Rptecorreoelectronico);
            if (!dr.IsDBNull(iRptecorreoelectronico)) entity.Rptecorreoelectronico = dr.GetString(iRptecorreoelectronico);

            int iRptefeccreacion = dr.GetOrdinal(this.Rptefeccreacion);
            if (!dr.IsDBNull(iRptefeccreacion)) entity.Rptefeccreacion = dr.GetDateTime(iRptefeccreacion);

            int iRpteusucreacion = dr.GetOrdinal(this.Rpteusucreacion);
            if (!dr.IsDBNull(iRpteusucreacion)) entity.Rpteusucreacion = dr.GetString(iRpteusucreacion);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iRpteusumodificacion = dr.GetOrdinal(this.Rpteusumodificacion);
            if (!dr.IsDBNull(iRpteusumodificacion)) entity.Rpteusumodificacion = dr.GetString(iRpteusumodificacion);

            int iRptefecmodificacion = dr.GetOrdinal(this.Rptefecmodificacion);
            if (!dr.IsDBNull(iRptefecmodificacion)) entity.Rptefecmodificacion = dr.GetDateTime(iRptefecmodificacion);

            int iRptefechavigenciapoder = dr.GetOrdinal(this.Rptefechavigenciapoder);
            if (!dr.IsDBNull(iRptefechavigenciapoder)) entity.Rptefechavigenciapoder = dr.GetDateTime(iRptefechavigenciapoder);

            return entity;
        }

        #region Consultas SQL
        public string SqlGetByEmprcodi
        {
            get { return base.GetSqlXml("GetByEmprcodi"); }
        }
        public string SqlUpdateEstadoRegistro
        {
            get { return base.GetSqlXml("UpdateEstadoRegistro"); }
        }
        public string SqlActualizarRepresentanteGestionModificacion
        {
            get { return GetSqlXml("ActualizarRepresentanteGestionModificacion"); }
        }
        public string SqlActualizarRepresentanteGestionModificacion_Agente
        {
            get { return GetSqlXml("ActualizarRepresentanteGestionModificacionAgente"); }
        }
        public string SqlActualizarRepresentanteGestionModificacionVigenciaPoder
        {
            get { return GetSqlXml("ActualizarRepresentanteGestionModificacionVigenciaPoder"); }
        }

        public string SqlObtenerRepresentantesTitulares
        {
            get { return base.GetSqlXml("ObtenerRepresentantesTitulares"); }
        }

        public string SqlActualizarNotificacion
        {
            get { return base.GetSqlXml("ActualizarNotificacion"); }
        }

        public string SqlActualizarRepresentante
        {
            get { return base.GetSqlXml("ActualizarRepresentante"); }
        }

        public string SqlDarBajaRepresentante
        {
            get { return base.GetSqlXml("DarBajaRepresentante"); }
        }

        #endregion

        #region Mapeo de Campos

        public string Rptecodi = "RPTECODI";
        public string Rptetipo = "RPTETIPO";
        public string Rptetiprepresentantelegal = "RPTETIPREPRESENTANTELEGAL";
        public string Rptebaja = "RPTEBAJA";
        public string Rpteinicial = "RPTEINICIAL";
        public string Rptetipdocidentidad = "RPTETIPDOCIDENTIDAD";
        public string Rptedocidentidad = "RPTEDOCIDENTIDAD";
        public string Rptedocidentidadadj = "RPTEDOCIDENTIDADADJ";
        public string Rptenombres = "RPTENOMBRES";
        public string Rpteapellidos = "RPTEAPELLIDOS";
        public string Rptevigenciapoder = "RPTEVIGENCIAPODER";
        public string Rptecargoempresa = "RPTECARGOEMPRESA";
        public string Rptetelefono = "RPTETELEFONO";
        public string Rptetelfmovil = "RPTETELFMOVIL";
        public string Rptecorreoelectronico = "RPTECORREOELECTRONICO";
        public string Rptefeccreacion = "RPTEFECCREACION";
        public string Rpteusucreacion = "RPTEUSUCREACION";
        public string Emprcodi = "EMPRCODI";
        public string Rpteusumodificacion = "RPTEUSUMODIFICACION";
        public string Rptefecmodificacion = "RPTEFECMODIFICACION";
        public string Rptefechavigenciapoder = "RPTEFECHAVIGENCIAPODER";
        public string Rptedocidentidadadjfilename = "RPTEDOCIDENTIDADADJFILENAME";
        public string Rptevigenciapoderfilename = "RPTEVIGENCIAPODERFILENAME";
        public string Emprruc = "EMPRRUC";
        public string Emprnomb = "EMPRNOMB";
        public string Rpteindnotic = "RPTEINDNOTIC";

        #endregion
    }
}
