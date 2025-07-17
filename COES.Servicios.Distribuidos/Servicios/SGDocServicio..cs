using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Dominio.DTO.SGDoc;
using COES.Servicios.Distribuidos.Contratos;
using COES.Servicios.Aplicacion.SGDoc;
using COES.Framework.Base.Tools;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class SGDocServicio : ISGDocServicio
    {
        /// <summary>
        /// Instancia de la clase aplicación
        /// </summary>
        SGDocAppServicio servicio = new SGDocAppServicio();

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
        public List<BandejaDTO> ListarDocumentosMesaPartes(int origen, int rol, DateTime fechaIniRegistro, DateTime fechaFinRegistro,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, int idArea, string idRegistro, 
            string numDocumento,
            string asunto, string codAtencion, string codigosEmpresa, bool indImportante, int idEtiqueta,
            int nroPagina, int lenPagina, out int nroRegistros)
        {

            List<BandejaDTO> list = this.servicio.ListarDocumentosMesaPartes(origen, rol, fechaIniRegistro, fechaFinRegistro, estadoAtencion, estadoRegistro,
                idEmpresaRemitente, idArea, idRegistro, numDocumento, asunto, codAtencion, codigosEmpresa, indImportante, idEtiqueta);
            nroRegistros = list.Count;

            if (nroPagina == -1)
            {
                return list;
            }
            else
            {
                return list.Skip((nroPagina - 1) * lenPagina).Take(lenPagina).ToList();
            }
        }

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
        public List<BandejaDTO> ListarDocumentosAreas(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool plazo, string codAtencion, bool conRpta, int tipoRecepcion, string codigosEmpresa, bool remExterno, bool indImportante,
            int idEtiqueta, int nroPagina, int lenPagina, out int nroRegistros)
        {

            List<BandejaDTO> list = this.servicio.ListarDocumentosAreas(origen, idArea, idPersona, idRol, fechaInicial, fechaFinal, estadoAtencion, estadoRegistro,
                idEmpresaRemitente, codRegistro, nroDocumento, asunto, plazo, codAtencion, conRpta, tipoRecepcion, codigosEmpresa, remExterno, 
                indImportante, idEtiqueta);
            nroRegistros = list.Count;
           
            if (nroPagina == -1)
            {
                return list;
            }
            else
            {
                return list.Skip((nroPagina - 1) * lenPagina).Take(lenPagina).ToList();
            }
        }


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
        public List<BandejaDTO> ListarDocumentosReclamos(DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente, string codRegistro,
            string nroDocumento, string asunto, bool indImportante)
        {
            return this.servicio.ListarDocumentosReclamos(fechaInicial, fechaFinal, idEmpresaRemitente, codRegistro, nroDocumento, asunto, indImportante);
        }

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
        public List<BandejaDTO> ListarDocumentosNoDespachado(int origen, DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente,
            string codRegistro, string nroDocumento, string asunto, bool indImportante)
        {
            return this.servicio.ListarDocumentosNoDespachado(origen, fechaInicial, fechaFinal, idEmpresaRemitente, codRegistro,
                nroDocumento, asunto, indImportante);
        }

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
        public List<BandejaDTO> ListarDocumentosPlazo(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool allPlazo, string codAtencion, bool indTodos, bool indImportante)
        {
            return this.servicio.ListarDocumentosPlazo(origen, idArea, idPersona, idRol, fechaInicial, fechaFinal,
                estadoAtencion, estadoRegistro, idEmpresaRemitente, codRegistro, nroDocumento, asunto, allPlazo, codAtencion, indTodos, indImportante);
        }

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
        public List<BandejaDTO> ListarDocumentosDerivadosPlazo(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool allPlazo, string codAtencion, bool indJefatura, bool indImportante)
        {
            return this.servicio.ListarDocumentosDerivadosPlazo(origen, idArea, idPersona, idRol, fechaInicial, fechaFinal, estadoAtencion, estadoRegistro,
                idEmpresaRemitente, codRegistro, nroDocumento, asunto, allPlazo, codAtencion, indJefatura, indImportante);
        }

        /// <summary>
        /// Muestra los recordarios
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>       
        public List<BandejaDTO> MostrarRecordatorios(int idPersona, int idArea, string bandeja, out bool indMP, out bool indP, out bool indJ)
        {
            return this.servicio.MostrarRecordatorios(idPersona, idArea, bandeja, out indMP, out indP, out indJ);
        }


        /// <summary>
        /// Listado de recordatorios para grilla de alertas
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="idArea"></param>
        /// <param name="idRol"></param>
        /// <param name="atencion"></param>
        /// <param name="indImportante"></param>
        /// <returns></returns>
        public List<BandejaDTO> ListarDocumentosRecibidosRecordatorio(int idPersona, int idArea, int idRol, string atencion, bool indImportante)
        {
            return this.servicio.ListarDocumentosRecibidosRecordatorio(idPersona, idArea, idRol, atencion, indImportante);
        }

        /// <summary>
        /// Permite ver el sello del SGDOC
        /// </summary>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>       
        public List<AtencionDTO> VerAtencion(int idDetalleFlujo)
        {
            return this.servicio.VerAtencion(idDetalleFlujo);
        }

        /// <summary>
        /// Permite ver el seguimiento entre las áreas al documento SGDOC
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>        
        public List<SeguimientoAreaDTO> VerSeguimiento(int idFlujo, int idArea)
        {
            return this.servicio.VerSeguimiento(idFlujo, idArea);
        }

        /// <summary>
        /// Permite ver el seguimiento al detalle del documento de una área
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idDetalleFlujo"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>      
        public List<SeguimientoEspecialistaDTO> VerSeguimientoEspecialista(int idFlujo, int idDetalleFlujo, int idArea)
        {
            return this.servicio.VerSeguimientoEspecialista(idFlujo, idDetalleFlujo, idArea);
        }

        /// <summary>
        /// Permite ver la atención asignada a las áreas involucradas en el formato de sello
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <returns></returns>        
        public List<SelloDTO> VerSello(int idFlujo)
        {
            return this.servicio.VerSello(idFlujo);
        }

        /// <summary>
        /// Permite ver las referencias que se hacen a un documento
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <returns></returns>      
        public List<ReferenciaDTO> VerReferenciasA(int idFlujo)
        {
            return this.servicio.VerReferenciasA(idFlujo);
        }

        /// <summary>
        /// Permite ver las referencias que el documento hace hacias otros documentos
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>      
        public List<ReferenciaDTO> VerReferenciasDe(int idFlujo)
        {
            return this.servicio.VerReferenciasDe(idFlujo);
        }

        /// <summary>
        /// Permite ver mensajes intercambiados para un registro SGDOC
        /// </summary>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>       
        public List<MensajeDTO> VerMensajes(int idDetalleFlujo)
        {
            return this.servicio.VerMensajes(idDetalleFlujo);
        }

        /// <summary>
        /// Permite ver documentos que son ingresados como parte inicial del flujo en el SGDOC
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>       
        public List<DocumentoDTO> VerDocumentos(int idFlujo, int idDetalleFlujo)
        {
            return this.servicio.VerDocumentos(idFlujo, idDetalleFlujo);
        }

        /// <summary>
        /// Permite ver documentos V
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <returns></returns>
        public List<DocumentoDTO> VerDocumentosV(int idFlujo)
        {
            return this.servicio.VerDocumentosV(idFlujo);
        }

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
        public List<ReporteDTO> VerReportes(int idAreaPadre, int idAreaDestino, bool conPlazo, DateTime fechaInicial, DateTime fechaFin, string atencion) 
        {
            return this.servicio.VerReportes(idAreaPadre, idAreaDestino, conPlazo, fechaInicial, fechaFin, atencion);
        }

        /// <summary>
        /// Permite ver las areas subordinadas a una padre
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>       
        public List<AreaDTO> VerAreas(int idArea)
        {
            return this.servicio.VerAreas(idArea);
        }

        /// <summary>
        /// Listar areas
        /// </summary>
        /// <returns></returns>
        public List<AreaDTO> ListarAreas()
        {
            return this.servicio.ListarAreas();
        }

        /// <summary>
        /// Permite listar los roles por determinado usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>        
        public List<RolUsuarioDTO> LeerRolesxUsuario(int idUsuario) 
        {
            return this.servicio.LeerRolesxUsuario(idUsuario);
        }

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
        public List<AlertaTramiteDTO> LeerAlertasTramites(int codigoArea, int codigoPersona, int codigoRol, DateTime fechaInicial,
            int diasAnticip, string atencion)
        {
            return this.servicio.LeerAlertasTramites(codigoArea, codigoPersona, codigoRol, fechaInicial,
                    diasAnticip, atencion);
        }

        /// <summary>
        /// Permite listtar las etiqueta por determinado usuario en su area
        /// </summary>
        /// <param name="idArea"></param>
        /// <param name="bandejaM"></param>
        /// <returns></returns>        
        public List<EtiquetaDTO> LeerEtiquetas(int idArea, int bandejaM) 
        {
            return this.servicio.LeerEtiquetas(idArea, bandejaM);
        }

        /// <summary>
        /// Permite listar las etiquetas por area
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        public List<EtiquetaDTO> ListarEtiquetaPorArea(int idArea)
        {
            return this.servicio.ListarEtiquetasPorArea(idArea);
        }

        /// <summary>
        /// Permite leer la ubicación en donde se encuentran los documentos del CD
        /// </summary>
        /// <param name="ifFlujo"></param>
        /// <returns></returns>      
        public ContenidoCdDTO LeerPathUbicacion(int idFlujo)
        {
            return this.servicio.LeerPathUbicacion(idFlujo);
        }

        /// <summary>
        /// Permite leer los tipos de atención
        /// </summary>
        /// <returns></returns>
        public List<TipoAtencionDTO> LeerTipoAtencion()
        {
            return this.servicio.LeerTipoAtencion();
        }

        /// <summary>
        /// Permite lerr los tipos de empresas remitentes
        /// </summary>
        /// <returns></returns>        
        public List<TipoEmpresaRemitenteDTO> LeerTipoEmpresaRemitente()
        {
            return this.servicio.LeerTipoEmpresaRemitente();
        }

        /// <summary>
        /// Permite obtener los parametros por defecto
        /// </summary>
        /// <param name="idOpcion"></param>
        /// <param name="idEtiqueta"></param>
        /// <returns></returns>
        public ParametroSGDoc ObtenerParametrosSGDoc(int idOpcion, int idEtiqueta, int idArea, int idUsuario)
        {
            return this.servicio.ObtenerParametrosSGDoc(idOpcion, idEtiqueta, idArea, idUsuario);
        }
        
        /// <summary>
        /// Listar los directorios en la raíz del CD que corresponde a algún trámite.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>        
        public List<ContenidoCdDTO> LeerDirectoriosRoot(int idFlujo)
        {
            return this.servicio.LeerDirectoriosRoot(idFlujo);
        }

        /// <summary>
        /// Listar los archivos en la raíz del CD que corresponde a algún trámite.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>       
        public List<ContenidoCdDTO> LeerArchivosRoot(int idFlujo)
        {
            return this.servicio.LeerArchivosRoot(idFlujo);
        }

        /// <summary>
        /// Listar los sub directorios.
        /// </summary>
        /// <param name="ruta_dir_virtual_enc">Ruta encriptada del directorio, campo Enlace de la clase ContenidoCdDTO</param>
        /// <returns></returns>       
        public List<ContenidoCdDTO> LeerDirectorios(string ruta)
        {
            return this.servicio.LeerDirectorios(ruta);
        }

        /// <summary>
        /// Listar los archivos de un directorio
        /// </summary>
        /// <param name="ruta_dir_virtual_enc">Ruta encriptada del directorio, campo Enlace de la clase ContenidoCdDTO</param>
        /// <returns></returns>       
        public List<ContenidoCdDTO> LeerArchivos(string ruta) 
        {
            return this.servicio.LeerArchivos(ruta);
        }


        /// <summary>
        /// Permite listar el maestro de empresas
        /// </summary>
        /// <returns></returns>
        public List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarMaestroEmpresas()
        {
            return this.servicio.ListarMaestroEmpresas();
        }

        /// <summary>
        /// Permite obtener las empreas que no son pertenecientes al registro de integrantes
        /// </summary>
        /// <returns></returns>        
        public List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasNoRI()
        {
            return this.servicio.ListarEmpresasNoRI();
        }

        /// <summary>
        /// Permite listar las empreas del registro de integrantes tipo
        /// </summary>
        /// <param name="tipoEmprCodi"></param>
        /// <returns></returns>        
        public List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasRIPorTipo(int tipoEmprCodi)
        {
            return this.servicio.ListarEmpresasRIPorTipo(tipoEmprCodi);
        }

        
    }   
}
