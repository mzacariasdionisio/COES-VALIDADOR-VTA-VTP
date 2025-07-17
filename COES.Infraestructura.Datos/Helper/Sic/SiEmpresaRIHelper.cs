using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;


namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class SiEmpresaRIHelper : HelperBase
    {
        public SiEmpresaRIHelper()
            : base(Consultas.SiEmpresaRISql)
        {

        }

        public string ReviCodiSGI = "ReviCodiSGI";
        public string ReviCodiDJR = "ReviCodiDJR";
        public string ReviEstadoSGI = "ReviEstadoSGI";
        public string ReviEstadoDJR = "ReviEstadoDJR";
        //integrantes
        public string RpteNombres = "RPTENOMBRES";
        public string RpteCorreoElectronico = "RPTECORREOELECTRONICO";
        public string RpteTelefono = "RPTETELEFONO";
        public string RpteTelfMovil = "RPTETELFMOVIL";
        //Representante Legal
        public string RpteTipo = "RPTETIPO";
        public string RpteTipRepresentanteLegal = "RPTETIPREPRESENTANTELEGAL";
        public string RpteDocIdentidad = "RPTEDOCIDENTIDAD";
        public string RpteCargoEmpresa = "RPTECARGOEMPRESA";
        public string RpteBaja = "RPTEBAJA";
        public string RpteFechaVigenciaPoder = "RPTEFECHAVIGENCIAPODER";
        //Empresa        
        public string Modalidad = "MODALIDAD";
        public string TipoAgente = "TIPOTIPAGENTE";

        //Evolución Empresa        
        public string TisoNombre = "TISONOMBRE";
        public string SoliEstado = "SOLIESTADO";
        public string SoliFecSolicitud = "SOLIFECSOLICITUD";
        public string SoliFecEnviado = "SOLIFECENVIADO";

        //Tiempos Proceso        
        public string reviiteracion = "REVIITERACION";
        public string tiporevision = "TIPOREVISION";
        public string revifeccreacion = "REVIFECCREACION";
        public string revifecrevision = "REVIFECREVISION";
        public string reviestado = "REVIESTADO";
        public string hora = "HORA";


        //Campos agregados Flujo
        public string Emflcodi = "EMFLCODI";
        public string Fljcodi = "FLJCODI";
        public string FLJFECHAORIG = "FLJFECHAORIG";
        public string FLJFECHARECEP = "FLJFECHARECEP";
        public string FLJFECHAPROCE = "FLJFECHAPROCE";
        public string FLJESTADO = "FLJESTADO";
        public string observacion = "OBSERVACION";
        public string DocumentoAdjuntoFileName = "DocumentoAdjuntoFileName";
        public string DocumentoAdjunto = "DocumentoAdjunto";
        public string corrnumproc = "corrnumproc";
        public string filecodi = "filecodi";

        public string SqlGetMaxNroConstancia
        {
            get { return base.GetSqlXml("GetMaxNroConstancia"); }
        }
        public string SqlGetMaxNroRegistro
        {
            get { return base.GetSqlXml("GetMaxNroRegistro"); }
        }

        public string SqlActualizarEmpresaNroRegistro
        {
            get { return base.GetSqlXml("ActualizarEmpresaNroRegistro"); }
        }


        public string SqlGetByIdGestionModificacion
        {
            get { return GetSqlXml("GetByIdGestionModificacion"); }
        }

        public string SqlGetByIdConRevision
        {
            get { return base.GetSqlXml("GetByIdConRevision"); }
        }

        public string SqlGetCabeceraSolicitudById
        {
            get { return GetSqlXml("GetCabeceraSolicitudById"); }
        }

        public string SqlActualizarEmpresaGestionModificacion
        {
            get { return GetSqlXml("ActualizarEmpresaGestionModificacion"); }
        }

        public string SqlActualizarEmpresaGestionModificacion_Agente
        {
            get { return GetSqlXml("ActualizarEmpresaGestionModificacionAgente"); }
        }

        public string SqlActualizarEmpresaCambioDenom
        {
            get { return GetSqlXml("ActualizarEmpresaCambioDenom"); }
        }

        public string SqlListarIntegrantesporTipo
        {
            get { return GetSqlXml("ListarIntegrantesporTipo"); }
        }

        public string SqlNroRegListarIntegrantesporTipo
        {
            get { return GetSqlXml("NroRegListarIntegrantesporTipo"); }
        }

        public string SqlListarporTipoNombreRepresentante
        {
            get { return GetSqlXml("ListarporTipoNombreRepresentante"); }
        }

        public string SqlListarporTipoNombreRepresentanteconFechaVigencia
        {
            get { return GetSqlXml("ListarporTipoNombreRepresentanteconFechaVigencia"); }
        }

        public string SqlNroRegListarporTipoNombreRepresentante
        {
            get { return GetSqlXml("NroRegListarporTipoNombreRepresentante"); }
        }
        public string SqlNroRegListarporTipoNombreRepresentanteconFechaVigencia
        {
            get { return GetSqlXml("NroRegListarporTipoNombreRepresentanteconFechaVigencia"); }
        }
        public string SqlListarporTipoNombreRepresentanteFiltroXls
        {
            get { return GetSqlXml("ListarporTipoNombreRepresentanteFiltroXls"); }
        }
        public string SqlListarporTipoNombreRepresentanteFiltroXlsconFechaVigencia
        {
            get { return GetSqlXml("ListarporTipoNombreRepresentanteFiltroXlsconFechaVigencia"); }
        }
        public string SqlListarIntegrantesporFechaTipo
        {
            get { return GetSqlXml("ListarIntegrantesporFechaTipo"); }
        }

        public string SqlNroRegListarIntegrantesporFechaTipo
        {
            get { return GetSqlXml("NroRegListarIntegrantesporFechaTipo"); }
        }

        public string SqlListarIntegrantesporFechaTipoFiltroXls
        {
            get { return GetSqlXml("ListarIntegrantesporFechaTipoFiltroXls"); }
        }

        public string SqlListarIntegrantesporFechaTipoSolicitud
        {
            get { return GetSqlXml("ListarIntegrantesporFechaTipoSolicitud"); }
        }

        public string SqlNroRegListarIntegrantesporFechaTipoSolicitud
        {
            get { return GetSqlXml("NroRegListarIntegrantesporFechaTipoSolicitud"); }
        }

        public string SqlListarIntegrantesporFechaTipoFiltroXlsSolicitud
        {
            get { return GetSqlXml("ListarIntegrantesporFechaTipoSolicitudFiltroXls"); }
        }

        public string SqlListarHistoricoSolicitudes
        {
            get { return GetSqlXml("ListarHistoricoSolicitudes"); }
        }

        public string SqlNroRegListarHistoricoSolicitudes
        {
            get { return GetSqlXml("NroRegListarHistoricoSolicitudes"); }
        }

        public string SqlListarHistoricoSolicitudesFiltroXls
        {
            get { return GetSqlXml("ListarHistoricoSolicitudesFiltroXls"); }
        }

        public string SqlListarHistoricoRevisiones
        {
            get { return GetSqlXml("ListarHistoricoRevisiones"); }
        }

        public string SqlNroRegListarHistoricoRevisiones
        {
            get { return GetSqlXml("NroRegListarHistoricoRevisiones"); }
        }

        public string SqlListarHistoricoRevisionesFiltroXls
        {
            get { return GetSqlXml("ListarHistoricoRevisionesFiltroXls"); }
        }

        public string SqlListarHistoricoModificaciones
        {
            get { return GetSqlXml("ListarHistoricoModificaciones"); }
        }

        public string SqlNroRegListarHistoricoModificaciones
        {
            get { return GetSqlXml("NroRegListarHistoricoModificaciones"); }
        }

        public string SqlListarHistoricoModificacionesFiltroXls
        {
            get { return GetSqlXml("ListarHistoricoModificacionesFiltroXls"); }
        }

        public string SqlListarTiempoProceso
        {
            get { return GetSqlXml("ListarTiempoProceso"); }
        }

        public string SqlNroRegListarTiempoProceso
        {
            get { return GetSqlXml("NroRegListarTiempoProceso"); }
        }

        public string SqlListarTiempoProcesoFiltroXls
        {
            get { return GetSqlXml("ListarTiempoProcesoFiltroXls"); }
        }

        public string SqlListarFlujoSolicitud
        {
            get { return GetSqlXml("ListarFlujoSolicitud"); }
        }

        public string SqlListarFlujoEmpresa
        {
            get { return GetSqlXml("ListarFlujoEmpresa"); }
        }

        public string SqlActualizarEstadoRegistro
        {
            get { return base.GetSqlXml("ActualizarEstadoRegistro"); }
        }

        public string SqlActualizarCondicion
        {
            get { return base.GetSqlXml("ActualizarCondicion"); }
        }

        public string SqlActualizarFechaIngreso
        {
            get { return base.GetSqlXml("ActualizarFechaIngreso"); }
        }

        public string SqlActualizarFechaBaja
        {
            get { return base.GetSqlXml("ActualizarFechaBaja"); }
        }

        public string SqlObtenerAgentesParticipantes
        {
            get { return base.GetSqlXml("ObtenerAgentesParticipantes"); }
        }

        public string SqlListarEmpresasPublico
        {
            get { return GetSqlXml("ListarEmpresasPublico"); }
        }

        #region FIT - VALORIZACION DIARIA
        public string SqlObtenerAgentesParticipantesMME
        {
            get { return base.GetSqlXml("ObtenerAgentesParticipantesMME"); }
        }
        #endregion
    }
}
