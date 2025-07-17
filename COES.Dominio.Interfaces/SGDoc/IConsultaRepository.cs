using COES.Dominio.DTO.SGDoc;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.SGDoc
{
    public interface IConsultaRepository
    {
        List<BandejaDTO> ListaDocumentosRecibidosMesaPartes(int origen, int rol, DateTime fechaIniRegistro, DateTime fechaFinRegistro,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, int idArea, string idRegistro, string numDocumento,
            string asunto, string codAtencion, string codigosEmpresa, bool indImportante, int idEtiqueta);

        List<BandejaDTO> ListaDocumentosRemitidosMesaPartes(int origen, int rol, DateTime fechaIniRegistro, DateTime fechaFinRegistro,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, int idArea, string idRegistro, string numDocumento,
            string asunto, string codAtencion, string codigosEmpresa, bool indImportante, int idEtiqueta);

        List<BandejaDTO> ListaDocumentosRecibidosAreas(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool plazo, string codAtencion, bool conRpta, int tipoRecepcion, string codigosEmpresa, bool remExterno, bool indImportante,
            int idEtiqueta);

        List<BandejaDTO> ListaDocumentosRemitidosAreas(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool plazo, string codAtencion, bool conRpta, int tipoRecepcion, string codigosEmpresa, bool remExterno, bool indImportante,
            int idEtiqueta);

        List<BandejaDTO> ListarDocumentosReclamos(DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente, string codRegistro,
            string nroDocumento, string asunto, bool indImportante);

        List<BandejaDTO> ListarDocumentosNoDespachadosRecibidos(DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente,
            string codRegistro, string nroDocumento, string asunto, bool indImportante);

        List<BandejaDTO> ListarDocumentosNoDespachadosRemitidos(DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente,
            string codRegistro, string nroDocumento, string asunto, bool indImportante);
        
        List<BandejaDTO> ListarDocumentosRecibidosPlazo(int origen, int idArea, int idPersona, int idRol, EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro,
            int idEmpresaRemitente, string codRegistro, string nroDocumento, string asunto, string codAtencion, bool indImportante);

        List<BandejaDTO> ListarDocumentosRecibidosPlazo(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal, 
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro,
            int idEmpresaRemitente, string codRegistro, string nroDocumento, string asunto, string codAtencion, bool indTodos, bool indImportante);
        
        List<BandejaDTO> ListarDocumentosDerivadosPlazo(int origen, int idArea, int idPersona, int idRol, 
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, string codAtencion, bool indJefatura, bool indImportante);

        List<BandejaDTO> ListarDocumentosPlazo(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, int diasPlazo, 
            string codAtencion, bool indImportante, string indicador);

        List<BandejaDTO> ListarDocumentosRecibidosRecordatorio(int origen, int idArea, int idPersona, int idRol, string codAtencion, 
            bool indImportante, string indicador);

        int VerificarUserRol(int userCode, int rolCode);

        List<DiaEspecialDTO> ObtenerDiasFeriados();

        List<AtencionDTO> VerAtencion(int idDetalleFlujo);

        List<SeguimientoAreaDTO> VerSeguimiento(int idFlujo, int idArea);

        List<SeguimientoEspecialistaDTO> VerSeguimientoEspecialista(int idFlujo, int idDetalleFlujo, int idArea);

        List<SelloDTO> VerSello(int idFlujo);

        List<ReferenciaDTO> VerReferenciasA(int idFlujo);

        List<ReferenciaDTO> VerReferenciasDe(int idFlujo);

        List<MensajeDTO> VerMensajes(int idDetalleFlujo);

        List<DocumentoDTO> VerDocumentos(int idFlujo, int idDetalleFlujo);

        List<DocumentoDTO> VerDocumentosV(int idFlujo);

        List<AreaDTO> VerAreas(int idArea);

        List<AreaDTO> ListarAreas();

        List<RolUsuarioDTO> LeerRolesxUsuario(int idUsuario);

        List<EtiquetaDTO> LeerEtiquetas(int idArea, int bandejaM);

        List<EtiquetaDTO> ListarEtiquetasPorArea(int idArea);

        List<ReporteDTO> VerReportes(int idAreaPadre, int idAreaDestino, bool conPlazo, DateTime fechaInicial, DateTime fechaFin,
            string atencion);

        List<AlertaTramiteDTO> LeerAlertasTramites(int codigoArea, int codigoPersona, int codigoRol, DateTime fechaInicial,
            int diasAnticip, string atencion);

        List<TipoAtencionDTO> LeerTipoAtencion();

        List<TipoEmpresaRemitenteDTO> LeerTipoEmpresaRemitente();

        string LeerDirectorioRoot(int idFlujo);

        string LeerPathArchivo(int filecodi);

        List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarMaestroEmpresas();

        List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasNoRI();

        List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasRIPorTipo(int tipoEmprCodi);

        List<MigracionDTO> ObtenerDatosMigracion(int mes, int anio);

        void ActualizarDatosMigracion(string pathPDF, string pathVOL, string pathCD, string tipoVOL, int flujocodi, int flujodetcodi);
    }
}
