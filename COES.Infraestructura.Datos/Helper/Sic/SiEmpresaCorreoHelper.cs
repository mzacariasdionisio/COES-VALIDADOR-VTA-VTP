using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_EMPRESA_CORREO
    /// </summary>
    public class SiEmpresaCorreoHelper : HelperBase
    {
        public SiEmpresaCorreoHelper(): base(Consultas.SiEmpresaCorreoSql)
        {
        }

        public SiEmpresaCorreoDTO Create(IDataReader dr)
        {
            SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO();

            int iEmpcorcodi = dr.GetOrdinal(this.Empcorcodi);
            if (!dr.IsDBNull(iEmpcorcodi)) entity.Empcorcodi = Convert.ToInt32(dr.GetValue(iEmpcorcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iEmpcornomb = dr.GetOrdinal(this.Empcornomb);
            if (!dr.IsDBNull(iEmpcornomb)) entity.Empcornomb = dr.GetString(iEmpcornomb);

            int iEmpcordesc = dr.GetOrdinal(this.Empcordesc);
            if (!dr.IsDBNull(iEmpcordesc)) entity.Empcordesc = dr.GetString(iEmpcordesc);

            int iEmpcoremail = dr.GetOrdinal(this.Empcoremail);
            if (!dr.IsDBNull(iEmpcoremail)) entity.Empcoremail = dr.GetString(iEmpcoremail);

            int iEmpcorestado = dr.GetOrdinal(this.Empcorestado);
            if (!dr.IsDBNull(iEmpcorestado)) entity.Empcorestado = dr.GetString(iEmpcorestado);

            int iEmpcorcargo = dr.GetOrdinal(this.Empcorcargo);
            if (!dr.IsDBNull(iEmpcorcargo)) entity.Empcorcargo = dr.GetString(iEmpcorcargo);

            int iEmpcortelefono = dr.GetOrdinal(this.Empcortelefono);
            if (!dr.IsDBNull(iEmpcortelefono)) entity.Empcortelefono = dr.GetString(iEmpcortelefono);

            int iEmprocmovil = dr.GetOrdinal(this.Empcormovil);
            if (!dr.IsDBNull(iEmprocmovil)) entity.Empcormovil = dr.GetString(iEmprocmovil);

            int iEmpcorindnotic = dr.GetOrdinal(this.Empcorindnotic);
            if (!dr.IsDBNull(iEmpcorindnotic)) entity.Empcorindnotic = dr.GetString(iEmpcorindnotic);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Empcorcodi = "EMPCORCODI";
        public string Emprcodi = "EMPRCODI";
        public string Modcodi = "MODCODI";
        public string Empcornomb = "EMPCORNOMB";
        public string Empcordesc = "EMPCORDESC";
        public string Empcoremail = "EMPCOREMAIL";
        public string Empcorestado = "EMPCORESTADO";
        public string Emprnomb = "EMPRNOMB";
        public string Tipoemprnomb = "TIPOEMPRDESC";
        public string Indnotificacion = "EMPRINDNOTIFICACION";
        public string Lastuser = "LASTUSER";
        public string Username = "USERNAME";
        public string Useremail = "USEREMAIL";
        public string Empcorcargo = "EMPCORCARGO";
        public string Empcortelefono = "EMPCORTELEFONO";
        public string Empcormovil = "EMPCORMOVIL";
        public string Emprruc = "EMPRRUC";
        public string Empcortipo = "EMPRCORTIPO";
        public string Empcorindnotic = "EMPCORINDNOTIC";
        public string Lastdate = "LASTDATE";

        public string SqlObtenerEmpresasIncumplimiento
        {
            get { return base.GetSqlXml("ObtenerEmpresasIncumplimiento"); }
        }

        public string SqlObtenerEmpresasDisponibles
        {
            get { return base.GetSqlXml("ObtenerEmpresasDisponibles"); }
        }

        public string SqlHabilitarNotificacion
        {
            get { return base.GetSqlXml("HabilitarNotificacion"); }
        }

        public string SqlObtenerCorreoPorModulo
        {
            get { return base.GetSqlXml("ObtenerCorreoPorModulo"); }
        }

        public string SqlObtenerCorreoPorEmpresaModulo
        {
            get { return base.GetSqlXml("ObtenerCorreoPorEmpresaModulo"); }
        }

        public string SqlObtenerCorreosPorEmpresa
        {
            get { return base.GetSqlXml("ObtenerCorreosPorEmpresa"); }
        }

        public string SqlObtenerReportePersonasContacto
        {
            get { return base.GetSqlXml("ObtenerReportePersonasContacto"); }
        }

        public string SqlObtenerCorreosNotificacion
        {
            get { return base.GetSqlXml("ObtenerCorreosNotificacion"); }
        }

        public string SqlObtenerListaCorreosNotificacion
        {
            get { return base.GetSqlXml("ObtenerListaCorreosNotificacion"); }
        }

        public string SqlObtenerListaCorreosProveedor
        {
            get { return base.GetSqlXml("ObtenerListaCorreosNotificacionProveedor"); }
        }

        #region Resarcimiento

        public string SqlCorreosPorEmpresaResarcimiento
        {
            get { return base.GetSqlXml("CorreosPorEmpresaResarcimiento"); }
        }

        public string SqlEliminarPorEmpresaResarcimiento
        {
            get { return base.GetSqlXml("EliminarPorEmpresaResarcimiento"); }
        }

        public string SqlListarSoloResarcimiento
        {
            get { return base.GetSqlXml("ListarSoloResarcimiento"); }
        }
        

        #endregion

        #endregion
    }
}
