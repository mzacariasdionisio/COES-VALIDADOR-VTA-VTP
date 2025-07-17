using System;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Dominio.DTO.SGDoc;
using COES.Servicios.Aplicacion.SGDoc;
using COES.Framework.Base.Tools;

namespace COES.Servicios.Distribuidos.Contratos
{
    /// <summary>
    /// Interface con los contratos de los servicios
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ISGDocServicio
    {
        /// <summary>
        /// Lista los documentos del SGODOC para cada una de las bandejas
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="rol"></param>
        /// <param name="fechaIniRegistro"></param>
        /// <param name="fechaFinRegistro"></param>
        /// <param name="estado"></param>
        /// <param name="idEmpresaRemitente"></param>
        /// <param name="idArea"></param>
        /// <param name="idRegistro"></param>
        /// <param name="codAlfaDoc"></param>
        /// <param name="asunto"></param>
        /// <param name="codAtencion"></param>
        /// <param name="codigosEmpresa"></param>
        /// <param name="indImportante"></param>
        /// <param name="indEtiqueta"></param>
        /// <returns></returns>
        [OperationContract]
        List<BandejaDTO> ListarDocumentosMesaPartes(int origen, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, int idArea, string codRegistro, string nroDocumento,
            string asunto, string codAtencion, string codigosEmpresa, bool indImportante, int idEtiqueta,
            int nroPagina, int lenPagina, out int nroRegistros);

        /// <summary>
        /// Permite listar los documentos de las áreas
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="idArea"></param>
        /// <param name="idPersona"></param>
        /// <param name="idRol"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="estadoAtencion"></param>
        /// <param name="estadoRegistro"></param>
        /// <param name="idEmpresaRemitente"></param>
        /// <param name="codRegistro"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="asunto"></param>
        /// <param name="plazo"></param>
        /// <param name="codAtencion"></param>
        /// <param name="conRpta"></param>
        /// <param name="tipoRecepcion"></param>
        /// <param name="codigosEmpresa"></param>
        /// <param name="remExterno"></param>
        /// <param name="indImportante"></param>
        /// <param name="indEtiqueta"></param>
        /// <returns></returns>
        [OperationContract]
        List<BandejaDTO> ListarDocumentosAreas(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool plazo, string codAtencion, bool conRpta, int tipoRecepcion, string codigosEmpresa, bool remExterno, bool indImportante,
            int idEtiqueta, int nroPagina, int lenPagina, out int nroRegistros);


        /// <summary>
        /// Permite listar los documentos de reclamos
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idEmpresaRemitente"></param>
        /// <param name="codRegistro"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="asunto"></param>
        /// <param name="indImportante"></param>
        /// <returns></returns>
        [OperationContract]
        List<BandejaDTO> ListarDocumentosReclamos(DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente, string codRegistro,
            string nroDocumento, string asunto, bool indImportante);


        /// <summary>
        /// Permite listar los documentos no despachados
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idEmpresaRemitente"></param>
        /// <param name="codRegistro"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="asunto"></param>
        /// <param name="indImportante"></param>
        /// <returns></returns>
        [OperationContract]
        List<BandejaDTO> ListarDocumentosNoDespachado(int origen, DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente,
            string codRegistro, string nroDocumento, string asunto, bool indImportante);
                

        /// <summary>
        /// Permite listar las recpeciones con plazo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="idArea"></param>
        /// <param name="idPersona"></param>
        /// <param name="idRol"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="estadoAtencion"></param>
        /// <param name="estadoRegistro"></param>
        /// <param name="idEmpresaRemitente"></param>
        /// <param name="codRegistro"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="asunto"></param>
        /// <param name="plazo"></param>
        /// <param name="codAtencion"></param>
        /// <param name="indTodos"></param>
        /// <param name="indImportante"></param>
        /// <returns></returns>
        [OperationContract]
        List<BandejaDTO> ListarDocumentosPlazo(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool allPlazo, string codAtencion, bool indTodos, bool indImportante);

        /// <summary>
        /// Permite listar los documentos derivados con plazo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="idArea"></param>
        /// <param name="idPersona"></param>
        /// <param name="idRol"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="estadoAtencion"></param>
        /// <param name="estadoRegistro"></param>
        /// <param name="idEmpresaRemitente"></param>
        /// <param name="codRegistro"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="asunto"></param>
        /// <param name="plazo"></param>
        /// <param name="codAtencion"></param>
        /// <param name="indJefatura"></param>
        /// <param name="indImportante"></param>
        /// <returns></returns>
        [OperationContract]
        List<BandejaDTO> ListarDocumentosDerivadosPlazo(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool allPlazo, string codAtencion, bool indJefatura, bool indImportante);

        /// <summary>
        /// Muestra los recordarios
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>
        [OperationContract]
        List<BandejaDTO> MostrarRecordatorios(int idPersona, int idArea, string bandeja, out bool indMP, out bool indP, out bool indJ);

        /// <summary>
        /// Listado de recordatorios para grilla de alertas
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="idArea"></param>
        /// <param name="idRol"></param>
        /// <param name="atencion"></param>
        /// <param name="indImportante"></param>
        /// <returns></returns>
        [OperationContract]
        List<BandejaDTO> ListarDocumentosRecibidosRecordatorio(int idPersona, int idArea, int idRol, string atencion, bool indImportante);
        
        /// <summary>
        /// Permite ver el sello del SGDOC
        /// </summary>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>
        [OperationContract]
        List<AtencionDTO> VerAtencion(int idDetalleFlujo);
        
        /// <summary>
        /// Permite ver el seguimiento entre las áreas al documento SGDOC
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>
        [OperationContract]
        List<SeguimientoAreaDTO> VerSeguimiento(int idFlujo, int idArea);
        
        /// <summary>
        /// Permite ver el seguimiento al detalle del documento de una área
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idDetalleFlujo"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>
        [OperationContract]
        List<SeguimientoEspecialistaDTO> VerSeguimientoEspecialista(int idFlujo, int idDetalleFlujo, int idArea);

        /// <summary>
        /// Permite ver la atención asignada a las áreas involucradas en el formato de sello
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelloDTO> VerSello(int idFlujo);

        /// <summary>
        /// Permite ver las referencias que se hacen a un documento
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <returns></returns>
        [OperationContract]
        List<ReferenciaDTO> VerReferenciasA(int idFlujo);

        /// <summary>
        /// Permite ver las referencias que el documento hace hacias otros documentos
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>
        [OperationContract]
        List<ReferenciaDTO> VerReferenciasDe(int idFlujo);

        /// <summary>
        /// Permite ver mensajes intercambiados para un registro SGDOC
        /// </summary>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>
        [OperationContract]
        List<MensajeDTO> VerMensajes(int idDetalleFlujo);

        /// <summary>
        /// Permite ver documentos que son ingresados como parte inicial del flujo en el SGDOC
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>
        [OperationContract]
        List<DocumentoDTO> VerDocumentos(int idFlujo, int idDetalleFlujo);

        /// <summary>
        /// Permite ver documentos adicionales
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <returns></returns>
        [OperationContract]
        List<DocumentoDTO> VerDocumentosV(int idFlujo);

        /// <summary>
        /// Permite ver documentos que son ingresados como parte inicial del flujo en el sgodoc
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="rol"></param>
        /// <param name="area"></param>
        /// <param name="fechaInical"></param>
        /// <param name="fechaFin"></param>
        /// <param name="codPlazo"></param>
        /// <returns></returns>
        [OperationContract]
        List<ReporteDTO> VerReportes(int idAreaPadre, int idAreaDestino, bool conPlazo, DateTime fechaInicial, DateTime fechaFin, string atencion);

        /// <summary>
        /// Permite ver las areas subordinadas a una padre
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        [OperationContract]
        List<AreaDTO> VerAreas(int idArea);

        [OperationContract]
        List<AreaDTO> ListarAreas();

        /// <summary>
        /// Permite listar los roles por determinado usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [OperationContract]
        List<RolUsuarioDTO> LeerRolesxUsuario(int idUsuario);
        
        /// <summary>
        /// Permite listar aquellos documentos vencidos o que se encuentran por vencer en un 
        /// determinado número de dias
        /// </summary>
        /// <param name="idAreaOrig"></param>
        /// <param name="idArea"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idRol"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nDiasPlazo"></param>
        /// <returns></returns>
        [OperationContract]
        List<AlertaTramiteDTO> LeerAlertasTramites(int codigoArea, int codigoPersona, int codigoRol, DateTime fechaInicial, 
            int diasAnticip, string atencion);

        /// <summary>
        /// Permite listtar las etiqueta por determinado usuario en su area
        /// </summary>
        /// <param name="idArea"></param>
        /// <param name="bandejaM"></param>
        /// <returns></returns>
        [OperationContract]
        List<EtiquetaDTO> LeerEtiquetas(int idArea, int bandejaM);


        /// <summary>
        /// Permite listar las etiquetas por area
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        [OperationContract]
        List<EtiquetaDTO> ListarEtiquetaPorArea(int idArea);
        
        /// <summary>
        /// Permite leer los tipos de atención
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<TipoAtencionDTO> LeerTipoAtencion();

        /// <summary>
        /// Permite lerr los tipos de empresas remitentes
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<TipoEmpresaRemitenteDTO> LeerTipoEmpresaRemitente();

        /// <summary>
        /// Permite obtener los parametros por defecto
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ParametroSGDoc ObtenerParametrosSGDoc(int idOpcion, int idEtiqueta, int idArea, int idUsuario);

        /// <summary>
        /// Listar los directorios en la raíz del CD que corresponde a algún trámite.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [OperationContract]        
        List<ContenidoCdDTO> LeerDirectoriosRoot(int idFlujo);

        /// <summary>
        /// Listar los archivos en la raíz del CD que corresponde a algún trámite.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [OperationContract]
        List<ContenidoCdDTO> LeerArchivosRoot(int idFlujo);

        /// <summary>
        /// Listar los sub directorios.
        /// </summary>
        /// <param name="ruta_dir_virtual_enc">Ruta encriptada del directorio, campo Enlace de la clase ContenidoCdDTO</param>
        /// <returns></returns>
        [OperationContract]
        List<ContenidoCdDTO> LeerDirectorios(string ruta);

        /// <summary>
        /// Listar los archivos de un directorio
        /// </summary>
        /// <param name="ruta_dir_virtual_enc">Ruta encriptada del directorio, campo Enlace de la clase ContenidoCdDTO</param>
        /// <returns></returns>
        [OperationContract]
        List<ContenidoCdDTO> LeerArchivos(string ruta);


        /// <summary>
        /// Permite listar el maestro de empresas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarMaestroEmpresas();

        /// <summary>
        /// Permite obtener las empreas que no son pertenecientes al registro de integrantes
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasNoRI();

        /// <summary>
        /// Permite listar las empreas del registro de integrantes tipo
        /// </summary>
        /// <param name="tipoEmprCodi"></param>
        /// <returns></returns>
        [OperationContract]
        List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasRIPorTipo(int tipoEmprCodi);
    }  

}
