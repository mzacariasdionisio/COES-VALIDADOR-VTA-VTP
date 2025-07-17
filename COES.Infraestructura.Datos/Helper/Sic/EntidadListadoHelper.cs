using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la Entidad Listado
    /// </summary>
    public class EntidadListadoHelper : HelperBase
    {
        public EntidadListadoHelper() : base(Consultas.EntidadListadoSql)
        {
        }

        #region Mapeo de Campos
        //EMPRESA
        public string Codigo = "CODIGO";
        public string Descripcion = "DESCRIPCION";
        public string CodOsinergmin = "CODOSINERGMIN";
        public string CampoAdicional = "CAMPOADICIONAL";
        public string Estado = "ESTADO";

        #endregion

        public string SqlListMaestroEmpresa
        {
            get { return GetSqlXml("ListMaestroEmpresa"); }
        }

        public string SqlListMaestroUsuarioLibre
        {
            get { return GetSqlXml("ListMaestroUsuarioLibre"); }
        }

        public string SqlListMaestroSuministro
        {
            get { return GetSqlXml("ListMaestroSuministro"); }
        }
        public string SqlListMaestroBarra
        {
            get { return GetSqlXml("ListMaestroBarra"); }
        }
        public string SqlListMaestroCentralGeneracion
        {
            get { return GetSqlXml("ListMaestroCentralGeneracion"); }
        }
        public string SqlListMaestroGrupoGeneracion
        {
            get { return GetSqlXml("ListMaestroGrupoGeneracion"); }
        }
        public string SqlListMaestroModoOperacion
        {
            get { return GetSqlXml("ListMaestroModoOperacion"); }
        }
        public string SqlListMaestroCuenca
        {
            get { return GetSqlXml("ListMaestroCuenca"); }
        }
        public string SqlListMaestroEmbalse
        {
            get { return GetSqlXml("ListMaestroEmbalse"); }
        }
        public string SqlListMaestroLago
        {
            get { return GetSqlXml("ListMaestroLago"); }
        }

        public string SqlListResultado
        {
            get { return GetSqlXml("ListResultado"); }
        }

        public string SqlFechaUltSincronizacion
        {
            get { return GetSqlXml("FechaUltSincronizacion"); }
        }

        public string SqlActualizarEstadoHomologacion
        {
            get { return GetSqlXml("ActualizarEstadoHomologacion"); }
        }

        public string SqlObtenerIdPendiente
        {
            get { return GetSqlXml("ObtenerIdPendiente"); }
        }


        public string SqlQuitarAsignacionEmpresa
        {
            get { return GetSqlXml("QuitarAsignacionEmpresa"); }
        }

        public string SqlQuitarAsignacionUsuarioLibre
        {
            get { return GetSqlXml("QuitarAsignacionUsuarioLibre"); }
        }

        public string SqlQuitarAsignacionSuministro
        {
            get { return GetSqlXml("QuitarAsignacionSuministro"); }
        }

        public string SqlQuitarAsignacionBarra
        {
            get { return GetSqlXml("QuitarAsignacionBarra"); }
        }

        public string SqlQuitarAsignacionCentralGeneracion
        {
            get { return GetSqlXml("QuitarAsignacionCentralGeneracion"); }
        }

        public string SqlQuitarAsignacionGrupoGeneracion
        {
            get { return GetSqlXml("QuitarAsignacionGrupoGeneracion"); }
        }

        public string SqlQuitarAsignacionModoOperacion
        {
            get { return GetSqlXml("QuitarAsignacionModoOperacion"); }
        }

        public string SqlQuitarAsignacionCuenca
        {
            get { return GetSqlXml("QuitarAsignacionCuenca"); }
        }

        public string SqlQuitarAsignacionEmbalse
        {
            get { return GetSqlXml("QuitarAsignacionEmbalse"); }
        }

        public string SqlQuitarAsignacionLago
        {
            get { return GetSqlXml("QuitarAsignacionLago"); }
        }

    }
}
