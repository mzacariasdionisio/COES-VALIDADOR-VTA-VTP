using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface para metodos de RI
    /// </summary>
    public interface ISiEmpresaRIRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int ObtenerNroConstancia();
        SiEmpresaDTO GetByIdGestionModificacion(int emprcodi);
        SiEmpresaDTO GetCabeceraSolicitudById(int emprcodi);
        SiEmpresaDTO GetByIdConRevision(int emprcodi);
        int ActualizarEmpresaGestionModificacion(int idEmpresa, string nombreComercial, string razonSocial,
            string domicilioLegal, string sigla, string nroPartida, string telefono, string fax, string paginaWeb, string nroRegistro);
        int ActualizarEmpresaGestionModificacion(int idEmpresa, string domicilioLegal, string telefono, string fax, string paginaWeb);
        int ActualizarEmpresaCambioDenom(int idEmpresa, string nombreComercial, string razonSocial, string sigla);
        List<SiEmpresaDTO> ListarIntegrantesporTipo(string tipoemprcodi, string nombre, int nroPage, int pageSize);
        int ObtenerTotalListarIntegrantesporTipo(string tipoemprcodi, string nombre);
        List<SiEmpresaDTO> ListarporTipoNombreRepresentante(string tipoemprcodi, string emprnomb, string rptetiprepresentantelegal, string rptetiprepresentantelegalcontacto, string estado, DateTime fecha, int nroPage, int pageSize);
        int ObtenerTotalListarporTipoNombreRepresentante(string tipoemprcodi, string emprnomb, string rptetiprepresentantelegal, string tiporepresentantecontacto, string estado, DateTime fecha);
        List<SiEmpresaDTO> ListarporTipoNombreRepresentanteFiltroXls(string tipoemprcodi, string emprnomb, string rptetiprepresentantelegal, string rptetiprepresentantelegalcontacto, string estado, DateTime fecha);
        List<SiEmpresaDTO> ListarEmpresas(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string nombre, string tipomodalidad, int nroPage, int pageSize);
        int ObtenerTotalRegListarEmpresas(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string nombre, string tipomodalidad);
        List<SiEmpresaDTO> ListarEmpresasFiltroXls(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string nombre, string tipomodalidad);
        List<SiEmpresaDTO> ListarEvolucionEmpresas(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiposolicitud, int nroPage, int pageSize);
        int ObtenerTotalRegListarEvolucionEmpresas(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiposolicitud);
        List<SiEmpresaDTO> ListarEvolucionEmpresasFiltroXls(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiposolicitud);
        List<SiEmpresaDTO> ListarHistoricoSolicitudes(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiposolicitud, string empresa, int nroPage, int pageSize);
        int ObtenerTotalRegListarHistoricoSolicitudes(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiposolicitud, string empresa);
        List<SiEmpresaDTO> ListarHistoricoSolicitudesFiltroXls(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiposolicitud, string empresa);
        List<SiEmpresaDTO> ListarHistoricoRevisiones(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiporevision, string empresa, int nroPage, int pageSize);
        int ObtenerTotalRegListarHistoricoRevisiones(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiporevision, string empresa);
        List<SiEmpresaDTO> ListarHistoricoRevisionesFiltroXls(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiporevision, string empresa);
        List<SiEmpresaDTO> ListarHistoricoModificaciones(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiposolicitud, string empresa, int nroPage, int pageSize);
        int ObtenerTotalRegListarHistoricoModificaciones(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiposolicitud, string empresa);
        List<SiEmpresaDTO> ListarHistoricoModificacionesFiltroXls(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiposolicitud, string empresa);
        List<SiEmpresaDTO> ListarTiempoProceso(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiporevision, int nroPage, int pageSize);
        int ObtenerTotalRegListarTiempoProceso(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiporevision);
        List<SiEmpresaDTO> ListarTiempoProcesoFiltroXls(DateTime fechaInicio, DateTime fechaFin, string tipoempresa, string tiporevision);
        List<SiEmpresaDTO> ListarFlujoEmpresa(int emprcodi);
        List<SiEmpresaDTO> ListarFlujoEmpresaSolicitud(int emprcodi, int solicodi);
        void ActualizarEstadoRegistro(SiEmpresaDTO entity);
        void ActualizarCondicion(SiEmpresaDTO entity);
        void ActualizarFechaIngreso(SiEmpresaDTO entity);
        void ActualizarFechaBaja(SiEmpresaDTO entity);
        List<SiEmpresaMMEDTO> ObtenerAgentesParticipantes(int tipo);
        int ObtenerNroRegistro();
        void ActualizarEmpresaNroRegistro(int emprcodi, int emprnroregistro);
        List<SiEmpresaDTO> ListarEmpresasPublico(string tipoempresa, string tiposolicitud);

        #region FIT - VALORIZACIONES DIARIAS
        List<SiEmpresaDTO> ObtenerAgentesParticipantesMME();
        #endregion
    }
}
