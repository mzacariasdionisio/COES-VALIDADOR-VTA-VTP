using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_GENERACIONRER
    /// </summary>
    public class WbGeneracionrerHelper : HelperBase
    {
        public WbGeneracionrerHelper(): base(Consultas.WbGeneracionrerSql)
        {
        }

        public WbGeneracionrerDTO Create(IDataReader dr)
        {
            WbGeneracionrerDTO entity = new WbGeneracionrerDTO();

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iEstado = dr.GetOrdinal(this.Estado);
            if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

            int iUserupdate = dr.GetOrdinal(this.Userupdate);
            if (!dr.IsDBNull(iUserupdate)) entity.Userupdate = dr.GetString(iUserupdate);

            int iUsercreate = dr.GetOrdinal(this.Usercreate);
            if (!dr.IsDBNull(iUsercreate)) entity.Usercreate = dr.GetString(iUsercreate);

            int iFecupdate = dr.GetOrdinal(this.Fecupdate);
            if (!dr.IsDBNull(iFecupdate)) entity.Fecupdate = dr.GetDateTime(iFecupdate);

            int iFeccreate = dr.GetOrdinal(this.Feccreate);
            if (!dr.IsDBNull(iFeccreate)) entity.Feccreate = dr.GetDateTime(iFeccreate);

            int iGenrermax = dr.GetOrdinal(this.Genrermax);
            if (!dr.IsDBNull(iGenrermax)) entity.Genrermax = dr.GetDecimal(iGenrermax);

            int iGenrermin = dr.GetOrdinal(this.Genrermin);
            if (!dr.IsDBNull(iGenrermin)) entity.Genrermin = dr.GetDecimal(iGenrermin);

            return entity;
        }


        #region Mapeo de Campos

        public string Ptomedicodi = "PTOMEDICODI";
        public string Estado = "ESTADO";
        public string Userupdate = "USERUPDATE";
        public string Usercreate = "USERCREATE";
        public string Fecupdate = "FECUPDATE";
        public string Feccreate = "FECCREATE";
        public string Emprnomb = "EMPRNOMB";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string CentralNomb = "CENTRAL";
        public string IndPorCentral = "INDICADOR";
        public string CentralCodi = "CENTRALCODI";
        public string UserLogin = "USERLOGIN";
        public string Ptodespacho = "PTODESPACHO";
        public string Genrermax = "GENRERMAX";
        public string Genrermin = "GENRERMIN";

        public string SqlObtenerEmpresas
        {
            get { return base.GetSqlXml("ObtenerEmpresas"); }
        }

        public string SqlObtenerCentrales
        {
            get { return base.GetSqlXml("ObtenerCentrales"); }
        }

        public string SqlObtenerUnidades
        {
            get { return base.GetSqlXml("ObtenerUnidades"); }
        }

        public string SqlValidarExistencia
        {
            get { return base.GetSqlXml("ValidarExistencia"); }
        }

        public string SqlValidarExistenciaUnidad
        {
            get { return base.GetSqlXml("ValidarExistenciaUnidad"); }
        }

        public string SqlObtenerPuntoFormato
        {
            get { return base.GetSqlXml("ObtenerPuntoFormato"); }
        }

        public string SqlValidarExistenciaGeneral
        {
            get { return base.GetSqlXml("ValidarExistenciaGeneral"); }
        }

        public string SqlObtenerEmpresaUsuario
        {
            get { return base.GetSqlXml("ObtenerEmpresaUsuario"); }
        }

        public string SqlGrabarConfiguracion
        {
            get { return base.GetSqlXml("GrabarConfiguracion"); }
        }

        #endregion


    }
}

