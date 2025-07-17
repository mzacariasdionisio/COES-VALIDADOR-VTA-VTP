
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using COES.Servicios.Aplicacion.General;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.SGDoc;
using COES.Framework.Base.Tools;
using AreaDTO = COES.Dominio.DTO.Sic.AreaDTO;

namespace COES.Servicios.Aplicacion.SGDoc
{
    public class SGDocAppServicio
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
        public List<BandejaDTO> ListarDocumentosMesaPartes(int origen, int rol, DateTime fechaIniRegistro, DateTime fechaFinRegistro,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, int idArea, string idRegistro, string numDocumento,
            string asunto, string codAtencion, string codigosEmpresa, bool indImportante, int idEtiqueta)
        {
            List<BandejaDTO> entitys = new List<BandejaDTO>();

            if (origen == 0)
            {
                entitys = FactorySGDoc.GetConsultaRepository().ListaDocumentosRecibidosMesaPartes(origen, rol, fechaIniRegistro, fechaFinRegistro,
                    estadoAtencion, estadoRegistro, idEmpresaRemitente, idArea, idRegistro, numDocumento, asunto, codAtencion, codigosEmpresa, indImportante, idEtiqueta);
            }
            else
            {
                entitys = FactorySGDoc.GetConsultaRepository().ListaDocumentosRemitidosMesaPartes(origen, rol, fechaIniRegistro, fechaFinRegistro,
                    estadoAtencion, estadoRegistro, idEmpresaRemitente, idArea, idRegistro, numDocumento, asunto, codAtencion, codigosEmpresa, indImportante, idEtiqueta);
            }

            return entitys;
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
            int idEtiqueta)
        {
            List<BandejaDTO> entitys = new List<BandejaDTO>();

            if (origen == 0)
            {
                entitys = FactorySGDoc.GetConsultaRepository().ListaDocumentosRecibidosAreas(origen, idArea, idPersona, idRol, fechaInicial, fechaFinal, estadoAtencion, estadoRegistro,
                    idEmpresaRemitente, codRegistro, nroDocumento, asunto, plazo, codAtencion, conRpta, tipoRecepcion, codigosEmpresa, remExterno, indImportante, idEtiqueta);
            }
            else
            {
                entitys = FactorySGDoc.GetConsultaRepository().ListaDocumentosRemitidosAreas(origen, idArea, idPersona, idRol, fechaInicial, fechaFinal, estadoAtencion, estadoRegistro,
                    idEmpresaRemitente, codRegistro, nroDocumento, asunto, plazo, codAtencion, conRpta, tipoRecepcion, codigosEmpresa, remExterno, indImportante, idEtiqueta);
            }

            return entitys;
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
            return FactorySGDoc.GetConsultaRepository().ListarDocumentosReclamos(fechaInicial, fechaFinal, idEmpresaRemitente, codRegistro,
                nroDocumento, asunto, indImportante);
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
            List<BandejaDTO> entitys = new List<BandejaDTO>();

            if (origen == 0)
            {
                entitys = FactorySGDoc.GetConsultaRepository().ListarDocumentosNoDespachadosRecibidos(fechaInicial, fechaFinal, idEmpresaRemitente,
                    codRegistro, nroDocumento, asunto, indImportante);
            }
            else
            {
                entitys = FactorySGDoc.GetConsultaRepository().ListarDocumentosNoDespachadosRemitidos(fechaInicial, fechaFinal, idEmpresaRemitente,
                    codRegistro, nroDocumento, asunto, indImportante);
            }

            return entitys;
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
            List<BandejaDTO> entitys = new List<BandejaDTO>();

            if (allPlazo)
            {
                entitys = FactorySGDoc.GetConsultaRepository().ListarDocumentosRecibidosPlazo(origen, idArea, idPersona, idRol, estadoAtencion, estadoRegistro,
                    idEmpresaRemitente, codRegistro, nroDocumento, asunto, codAtencion, indImportante);

            }
            else
            {
                entitys = FactorySGDoc.GetConsultaRepository().ListarDocumentosRecibidosPlazo(origen, idArea, idPersona, idRol, fechaInicial, fechaFinal, estadoAtencion, estadoRegistro,
                    idEmpresaRemitente, codRegistro, nroDocumento, asunto, codAtencion, indTodos, indImportante);
            }

            return entitys;
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

            List<BandejaDTO> entitys = new List<BandejaDTO>();

            if (allPlazo)
            {
                entitys = FactorySGDoc.GetConsultaRepository().ListarDocumentosDerivadosPlazo(origen, idArea, idPersona, idRol, estadoAtencion, estadoRegistro, idEmpresaRemitente,
                    codRegistro, nroDocumento, asunto, codAtencion, indJefatura, indImportante);
            }

            return entitys;

        }

        /// <summary>
        /// Muestra los recordarios
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>        
        public List<BandejaDTO> MostrarRecordatorios(int idPersona, int idArea, string bandeja, out bool indMP, out bool indP, out bool indJ)
        {
            List<BandejaDTO> listResultado = new List<BandejaDTO>();
            List<BandejaDTO> listUsuario = new List<BandejaDTO>();
            List<BandejaDTO> listArea = new List<BandejaDTO>();
            List<BandejaDTO> listMesaPartes = new List<BandejaDTO>();
            List<BandejaDTO> listUsuarioPendiente = new List<BandejaDTO>();
            List<BandejaDTO> listAreaPendiente = new List<BandejaDTO>();
            List<BandejaDTO> listMesaPartesPendiente = new List<BandejaDTO>();


            int diasPlazo = 3;
            int diasReales = this.ObtenerDiasReales(DateTime.Today, diasPlazo);

            if (this.VerificarRol(2, idPersona) || this.VerificarRol(5, idPersona))
            {
                listArea = FactorySGDoc.GetConsultaRepository().ListarDocumentosPlazo(-1, idArea, -1, -1, DateTime.Today.AddDays(1), diasReales, "11111111", false, "Jefatura");
                listAreaPendiente = FactorySGDoc.GetConsultaRepository().ListarDocumentosRecibidosRecordatorio(-1, idArea, -1, -1, "11111111", false, "Jefatura");

            }
            listUsuario = FactorySGDoc.GetConsultaRepository().ListarDocumentosPlazo(-1, idArea, idPersona, -1, DateTime.Today.AddDays(1), diasReales, "11111111", false, "Personal");
            listUsuarioPendiente = FactorySGDoc.GetConsultaRepository().ListarDocumentosRecibidosRecordatorio(-1, idArea, idPersona, -1, "11111111", false, "Jefatura");

            if (this.VerificarRol(3, idPersona))
            {
                listMesaPartes = FactorySGDoc.GetConsultaRepository().ListarDocumentosPlazo(-1, -1, -1, -1, DateTime.Today.AddDays(1), diasReales, "11111111", false, "MesaPartes");
                listMesaPartesPendiente = FactorySGDoc.GetConsultaRepository().ListarDocumentosRecibidosRecordatorio(-1, -1, -1, -1, "11111111", false, "MesaPartes");
            }

            indJ = false;
            indMP = false;
            indP = false;

            if (string.IsNullOrEmpty(bandeja))
            {
                listResultado.AddRange(listArea);
                listResultado.AddRange(listUsuario);
                listResultado.AddRange(listMesaPartes);
                listResultado.AddRange(listAreaPendiente);
                listResultado.AddRange(listUsuarioPendiente);
                listResultado.AddRange(listMesaPartesPendiente);


                if (listArea.Count > 0) indJ = true;
                if (listUsuario.Count > 0) indP = true;
                if (listMesaPartes.Count > 0) indMP = true;
            }
            else
            {
                if (bandeja == "MesaPartes")
                {
                    listResultado.AddRange(listMesaPartes);
                    listResultado.AddRange(listMesaPartesPendiente);
                    indMP = true;
                }
                else if (bandeja == "Personal")
                {
                    listResultado.AddRange(listUsuario);
                    listResultado.AddRange(listUsuarioPendiente);
                    indP = true;
                }
                else if (bandeja == "Jefatura")
                {
                    listResultado.AddRange(listArea);
                    listResultado.AddRange(listAreaPendiente);
                    indJ = true;
                }
            }

            return listResultado;
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
            return null;
        }

        /// <summary>
        /// Permite verificar si el usuario tiene rol
        /// </summary>
        /// <param name="rolCodi"></param>
        /// <param name="userCodi"></param>
        /// <returns></returns>
        private bool VerificarRol(int rolCodi, int userCodi)
        {
            int count = FactorySGDoc.GetConsultaRepository().VerificarUserRol(userCodi, rolCodi);
            if (count > 0) return true;
            return false;
        }

        /// <summary>
        /// Permite obtener los dias reales
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="diasPlazo"></param>
        /// <returns></returns>
        private int ObtenerDiasReales(DateTime fecha, int diasPlazo)
        {
            int nroDiasFestivos = 0;
            DateTime fechaAuxiliar = fecha.AddDays(1);
            DateTime fechaFeriado;
            List<DiaEspecialDTO> feriados = FactorySGDoc.GetConsultaRepository().ObtenerDiasFeriados();

            for (int i = 0; i < diasPlazo; i++)
            {
                foreach (DiaEspecialDTO item in feriados)
                {
                    fechaFeriado = item.DiaFecha;

                    if (item.DiaFrec == ConstantesAppServicio.SI)
                    {
                        if ((fechaFeriado.Day == fechaAuxiliar.Day) && (fechaFeriado.Month == fechaAuxiliar.Month))
                        {
                            nroDiasFestivos++;
                        }
                    }
                    else
                    {
                        if ((fechaFeriado.Day == fechaAuxiliar.Day) && (fechaFeriado.Month == fechaAuxiliar.Month) && (fechaFeriado.Year == fechaAuxiliar.Year))
                        {
                            nroDiasFestivos++;
                        }
                    }
                }

                if ((fechaAuxiliar.DayOfWeek == DayOfWeek.Saturday) || (fechaAuxiliar.DayOfWeek == DayOfWeek.Sunday))
                {
                    nroDiasFestivos++;
                }

                fechaAuxiliar = fechaAuxiliar.AddDays(1);
            }

            return diasPlazo + nroDiasFestivos;
        }

        /// <summary>
        /// Permite ver el sello del SGDOC
        /// </summary>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>       
        public List<AtencionDTO> VerAtencion(int idDetalleFlujo)
        {
            return FactorySGDoc.GetConsultaRepository().VerAtencion(idDetalleFlujo);
        }

        /// <summary>
        /// Permite ver el seguimiento entre las áreas al documento SGDOC
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>        
        public List<SeguimientoAreaDTO> VerSeguimiento(int idFlujo, int idArea)
        {
            return FactorySGDoc.GetConsultaRepository().VerSeguimiento(idFlujo, idArea);
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
            return FactorySGDoc.GetConsultaRepository().VerSeguimientoEspecialista(idFlujo, idDetalleFlujo, idArea);
        }

        /// <summary>
        /// Permite ver la atención asignada a las áreas involucradas en el formato de sello
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <returns></returns>        
        public List<SelloDTO> VerSello(int idFlujo)
        {
            return FactorySGDoc.GetConsultaRepository().VerSello(idFlujo);
        }

        /// <summary>
        /// Permite ver las referencias que se hacen a un documento
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <returns></returns>      
        public List<ReferenciaDTO> VerReferenciasA(int idFlujo)
        {
            return FactorySGDoc.GetConsultaRepository().VerReferenciasA(idFlujo);
        }

        /// <summary>
        /// Permite ver las referencias que el documento hace hacias otros documentos
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>      
        public List<ReferenciaDTO> VerReferenciasDe(int idFlujo)
        {
            return FactorySGDoc.GetConsultaRepository().VerReferenciasDe(idFlujo);
        }

        /// <summary>
        /// Permite ver mensajes intercambiados para un registro SGDOC
        /// </summary>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>       
        public List<MensajeDTO> VerMensajes(int idDetalleFlujo)
        {
            return FactorySGDoc.GetConsultaRepository().VerMensajes(idDetalleFlujo);
        }

        /// <summary>
        /// Permite ver documentos que son ingresados como parte inicial del flujo en el SGDOC
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <param name="idDetalleFlujo"></param>
        /// <returns></returns>       
        public List<DocumentoDTO> VerDocumentos(int idFlujo, int idDetalleFlujo)
        {
            return FactorySGDoc.GetConsultaRepository().VerDocumentos(idFlujo, idDetalleFlujo);
        }


        /// <summary>
        /// Permite ver documentos adicionales
        /// </summary>
        /// <param name="idFlujo"></param>
        /// <returns></returns>
        public List<DocumentoDTO> VerDocumentosV(int idFlujo)
        {
            return FactorySGDoc.GetConsultaRepository().VerDocumentosV(idFlujo);
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
        public List<ReporteDTO> VerReportes(int idAreaPadre, int idAreaDestino, bool conPlazo, DateTime fechaInicial, DateTime fechaFin,
            string atencion)
        {
            return FactorySGDoc.GetConsultaRepository().VerReportes(idAreaPadre, idAreaDestino, conPlazo, fechaInicial, fechaFin, atencion);
        }

        /// <summary>
        /// Permite ver las areas subordinadas a una padre
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>       
        public List<Dominio.DTO.SGDoc.AreaDTO> VerAreas(int idArea)
        {
            return FactorySGDoc.GetConsultaRepository().VerAreas(idArea);
        }

        /// <summary>
        /// Lista las areas del COES
        /// </summary>
        /// <returns></returns>
        public List<Dominio.DTO.SGDoc.AreaDTO> ListarAreas()
        {
            return FactorySGDoc.GetConsultaRepository().ListarAreas();
        }

        /// <summary>
        /// Permite listar los roles por determinado usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>        
        public List<RolUsuarioDTO> LeerRolesxUsuario(int idUsuario)
        {
            return FactorySGDoc.GetConsultaRepository().LeerRolesxUsuario(idUsuario);
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
            return FactorySGDoc.GetConsultaRepository().LeerAlertasTramites(codigoArea, codigoPersona, codigoRol,
                fechaInicial, diasAnticip, atencion);
        }

        /// <summary>
        /// Permite listtar las etiqueta por determinado usuario en su area
        /// </summary>
        /// <param name="idArea"></param>
        /// <param name="bandejaM"></param>
        /// <returns></returns>        
        public List<EtiquetaDTO> LeerEtiquetas(int idArea, int bandejaM)
        {
            return FactorySGDoc.GetConsultaRepository().LeerEtiquetas(idArea, bandejaM);
        }

        /// <summary>
        /// Permite listar las etiquetas por cada área
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        public List<EtiquetaDTO> ListarEtiquetasPorArea(int idArea)
        {
            return FactorySGDoc.GetConsultaRepository().ListarEtiquetasPorArea(idArea);
        }

        /// <summary>
        /// Permite leer la ubicación en donde se encuentran los documentos del CD
        /// </summary>
        /// <param name="ifFlujo"></param>
        /// <returns></returns>      
        public ContenidoCdDTO LeerPathUbicacion(int idFlujo)
        {
            return new ContenidoCdDTO();
        }

        /// <summary>
        /// Permite leer los tipos de atención
        /// </summary>
        /// <returns></returns>
        public List<TipoAtencionDTO> LeerTipoAtencion()
        {
            return FactorySGDoc.GetConsultaRepository().LeerTipoAtencion();
        }

        /// <summary>
        /// Permite lerr los tipos de empresas remitentes
        /// </summary>
        /// <returns></returns>        
        public List<TipoEmpresaRemitenteDTO> LeerTipoEmpresaRemitente()
        {
            return FactorySGDoc.GetConsultaRepository().LeerTipoEmpresaRemitente();
        }

        /// <summary>
        /// Permite obtener los valores por defecto
        /// </summary>
        /// <param name="idOpcion"></param>
        /// <param name="idEtiqueta"></param>
        /// <returns></returns>
        public ParametroSGDoc ObtenerParametrosSGDoc(int idOpcion, int idEtiqueta, int idArea, int idUsuario)
        {
            ParametroSGDoc entity = new ParametroSGDoc();
            entity.Indicador = ConstantesAppServicio.SI;
            entity.CodEstadoAtencion = EstadoAtencion.Todos;
            entity.CodEstadoRegistro = EstadoRegistro.Habilitado;
            entity.CodAtencion = "00000000";
            entity.IdEtiqueta = idEtiqueta;
            entity.FechaInicial = DateTime.Now;
            entity.FechaFinal = DateTime.Now;
            entity.IdEmpresaRemitente = -1;
            entity.IdArea = -1;
            entity.IdRegistro = "";
            entity.NumDocumento = "";
            entity.Asunto = "";
            entity.CodigosEmpresa = "";
            entity.IndImportante = false;
            entity.Plazo = false;
            entity.ConRpta = true;
            entity.TipoRecepcion = -1;
            entity.RemExterno = false;
            entity.IndBusqueda = true;
            entity.IndDiaActual = false;

            switch (idOpcion)
            {
                case 476://Bandeja Mesa de Partes
                    {
                        entity.Indicador = ConstantesAppServicio.NO;
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 480://    Recepción
                    {
                        entity.Origen = 0;
                        entity.Rol = 0;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 483://        Hoy
                    {
                        entity.Origen = 0;
                        entity.Rol = 0;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        entity.IndDiaActual = true;
                        break;
                    }
                case 484://        Etiquetas de Usuario
                    {
                        entity.Origen = 0;
                        entity.Rol = 0;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        if (idEtiqueta == -1) entity.Indicador = ConstantesAppServicio.NO;
                        break;
                    }
                case 481://    Remisión
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 485://        Presidencia
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 11;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 486://        Dirección
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 2;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 487://        Dirección de Operaciones
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 10;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 496://            Sub Dirección de Evaluación
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 8;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 497://            Sub Dirección de Coordinación
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 3;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 498://            Sub Dirección de Transferencias
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 4;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 499://            Sub Dirección de Programación
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 7;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 488://        Dirección de Planificación
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 13;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 489://        Departamento de Administración
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 9;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 491://        Asesoría Legal Directorio
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 16;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 492://        Oficina de Perfeccionamiento
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = 15;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 493://    Etiquetas de Usuario
                    {
                        entity.Origen = 1;
                        entity.Rol = 0;
                        entity.IdArea = -1;
                        entity.Metodo = "ListarDocumentosMesaPartes";
                        entity.Bandeja = "MesaPartes";
                        if (idEtiqueta == -1) entity.Indicador = ConstantesAppServicio.NO;
                        break;
                    }
                case 482://    Filtros
                    {
                        entity.Indicador = ConstantesAppServicio.NO;
                        entity.Bandeja = "MesaPartes";
                        break;
                    }
                case 494://        Documentos de Reclamos
                    {
                        entity.Bandeja = "MesaPartes";
                        entity.Metodo = "ListarDocumentosReclamos";
                        break;
                    }
                case 495://        Pendientes de derivación desde recepción
                    {
                        entity.Bandeja = "MesaPartes";
                        entity.Metodo = "ListarDocumentosNoDespachado";
                        entity.Origen = 0;
                        break;
                    }
                case 477://Bandeja Jefatura
                    {
                        entity.Indicador = ConstantesAppServicio.NO;
                        entity.Bandeja = "Jefatura";
                        break;
                    }
                case 500://    Recepción
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 0;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.TipoRecepcion = 3;
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 503://        Recepción COES
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 0;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.TipoRecepcion = 2;
                        entity.IdEmpresaRemitente = 1;
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 504://        Recepción Empresas
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 0;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.TipoRecepcion = 1;
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 505://        Etiquetas de Usuario
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 0;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.TipoRecepcion = 3;
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosAreas";
                        if (idEtiqueta == -1) entity.Indicador = ConstantesAppServicio.NO;
                        break;
                    }
                case 501://    Remisión
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 1;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.ConRpta = false;
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 506://        Remisión Interna
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 1;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.ConRpta = false;
                        entity.IdEmpresaRemitente = 1;
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 507://        Remisión Externa
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 1;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.ConRpta = false;
                        entity.RemExterno = true;
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 508://        Etiquetas de Usuario
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 1;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.ConRpta = false;
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosAreas";
                        if (idEtiqueta == -1) entity.Indicador = ConstantesAppServicio.NO;
                        break;
                    }
                case 502://    Filtros
                    {
                        entity.Bandeja = "Jefatura";
                        entity.Indicador = ConstantesAppServicio.NO;
                        break;
                    }
                case 509://        Trámites pendientes
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 0;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.ConRpta = false;
                        entity.TipoRecepcion = 3;
                        entity.Bandeja = "Jefatura";
                        entity.CodEstadoAtencion = EstadoAtencion.Pendiente;
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 510://    Trámites pendientes atención
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 0;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.ConRpta = false;
                        entity.TipoRecepcion = 3;
                        entity.CodAtencion = "11111100";
                        entity.Bandeja = "Jefatura";
                        entity.CodEstadoAtencion = EstadoAtencion.Pendiente;
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 511://    Trámites pendientes con plazo
                    {
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosPlazo";
                        entity.Origen = 0;
                        entity.IdArea = idArea;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.IndTodos = true;
                        entity.AllPlazo = true;
                        break;
                    }
                case 512://        Trámites pendientes derivados con plazo
                    {
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "ListarDocumentosDerivadosPlazo";
                        entity.Origen = 0;
                        entity.IdArea = idArea;
                        entity.IdPersona = -1;
                        entity.Rol = 0;
                        entity.AllPlazo = true;
                        entity.CodAtencion = "11111100";
                        entity.IndJefatura = true;

                        break;
                    }
                case 513://        Recordatorio trámites pendientes
                    {
                        entity.Bandeja = "Jefatura";
                        entity.Metodo = "MostrarRecordatorios";
                        entity.IdPersona = idUsuario;
                        entity.IdArea = idArea;
                        entity.IndBusqueda = false;
                        break;
                    }
                case 478://Bandeja Personal
                    {
                        entity.Bandeja = "Personal";
                        entity.Indicador = ConstantesAppServicio.NO;
                        break;
                    }
                case 514://    Recepción
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 0;
                        entity.IdPersona = idUsuario;
                        entity.Rol = 0;
                        entity.Bandeja = "Personal";
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 517://        Etiquetas de Usuario
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 0;
                        entity.IdPersona = idUsuario;
                        entity.Rol = 0;
                        entity.Bandeja = "Personal";
                        entity.Metodo = "ListarDocumentosAreas";
                        if (idEtiqueta == -1) entity.Indicador = ConstantesAppServicio.NO;
                        break;
                    }
                case 515://    Remisión
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 1;
                        entity.IdPersona = idUsuario;
                        entity.Rol = 0;
                        entity.ConRpta = false;
                        entity.Bandeja = "Personal";
                        entity.Metodo = "ListarDocumentosAreas";
                        break;
                    }
                case 518://        Etiquetas de Usuario
                    {
                        entity.IdArea = idArea;
                        entity.Origen = 1;
                        entity.IdPersona = idUsuario;
                        entity.Rol = 0;
                        entity.Bandeja = "Personal";
                        entity.Metodo = "ListarDocumentosAreas";
                        if (idEtiqueta == -1) entity.Indicador = ConstantesAppServicio.NO;
                        break;
                    }
                case 516://    Filtros
                    {
                        entity.Indicador = ConstantesAppServicio.NO;
                        entity.Bandeja = "Personal";
                        break;
                    }
                case 519://        Trámites pendientes con plazo
                    {
                        entity.Bandeja = "Personal";
                        entity.Metodo = "ListarDocumentosPlazo";
                        entity.Origen = 0;
                        entity.IdArea = idArea;
                        entity.IdPersona = idUsuario;
                        entity.Rol = 0;
                        entity.IndTodos = true;
                        entity.AllPlazo = false;

                        break;
                    }
                case 520://        Trámites pendientes derivados con plazo
                    {
                        entity.Bandeja = "Personal";
                        entity.Metodo = "ListarDocumentosDerivadosPlazo";
                        entity.Origen = 0;
                        entity.IdArea = idArea;
                        entity.IdPersona = idUsuario;
                        entity.Rol = 0;
                        entity.AllPlazo = true;
                        entity.CodAtencion = "11111100";
                        entity.IndJefatura = false;
                        break;
                    }
                case 521://        Recordatorio trámites pendientes
                    {
                        entity.Bandeja = "Personal";
                        entity.Metodo = "MostrarRecordatorios";
                        entity.IdPersona = idUsuario;
                        entity.IdArea = idArea;
                        entity.IndBusqueda = false;
                        break;
                    }
                case 479://Bandeja Especial
                    {
                        entity.Bandeja = "Especial";
                        entity.Indicador = ConstantesAppServicio.NO;
                        break;
                    }
                case 522://Documentos de Reclamos
                    {
                        entity.Bandeja = "Especial";
                        entity.Metodo = "ListarDocumentosReclamos";
                        break;
                    }
            }

            return entity;
        }


        #region Directorios

        /// <summary>
        /// Permite obtener los directorios root
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public List<ContenidoCdDTO> LeerDirectoriosRoot(int codigo)
        {
            CFile_mng servicio = new CFile_mng();
            List<ContenidoCdDTO> entitys = new List<ContenidoCdDTO>();
            string dirVirtualRoot = string.Empty;
            string dirVirtualCd = string.Empty;
            string dirFisicoRoot = string.Empty;

            try
            {
                dirVirtualCd = FactorySGDoc.GetConsultaRepository().LeerDirectorioRoot(codigo);
                dirVirtualRoot = "/datasgdoccd" + "\\" + dirVirtualCd;
                dirFisicoRoot = servicio.nf_GetPathfromDirVirtual(dirVirtualRoot);
                if (dirFisicoRoot != null)
                {
                    entitys = LeerDirectorios(Encriptacion.Encripta(dirFisicoRoot));
                }
                return entitys;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los archivos root
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public List<ContenidoCdDTO> LeerArchivosRoot(int codigo)
        {
            CFile_mng servicio = new CFile_mng();
            string dirVirtualRoot = string.Empty;
            string dirVirtualCd = string.Empty;
            string dirFisicoRoot = string.Empty;

            try
            {
                dirVirtualCd = FactorySGDoc.GetConsultaRepository().LeerDirectorioRoot(codigo);
                dirVirtualRoot = "/datasgdoccd" + "\\" + dirVirtualCd;
                dirFisicoRoot = servicio.nf_GetPathfromDirVirtual(dirVirtualRoot);
                if (dirFisicoRoot != null)
                {
                    return this.LeerArchivos(Encriptacion.Encripta(dirFisicoRoot));
                }
                else
                {
                    return new List<ContenidoCdDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Listar directorios
        /// </summary>
        /// <param name="ruta_dir_virtual_enc"></param>
        /// <returns></returns>
        public List<ContenidoCdDTO> LeerDirectorios(string dirVirtual)
        {
            try
            {
                List<ContenidoCdDTO> entitys = new List<ContenidoCdDTO>();
                ContenidoCdDTO entity = null;
                string pathDirEnc = string.Empty;
                string linkDir = string.Empty;
                dirVirtual = dirVirtual.Replace("%2b", "+");

                string pathFisico = Encriptacion.Desencripta(dirVirtual);
                n_InfoDirectory[] directorios = this.GetDirectoriosPath(pathFisico);

                if (directorios != null)
                {
                    foreach (n_InfoDirectory directory in directorios)
                    {
                        entity = new ContenidoCdDTO();
                        entity.Nombre = directory.is_Name;
                        pathDirEnc = Encriptacion.Encripta(directory.is_path);
                        linkDir = @"http:\\localhost\proceso.aspx?dx=" + pathDirEnc;
                        linkDir = linkDir.Replace("+", "%2b");
                        entity.Enlace = pathDirEnc;
                        entity.Peso = directory.id_Size.ToString();
                        entity.Tipo = "D";
                        entitys.Add(entity);
                    }
                }

                return entitys;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite leer los archivos
        /// </summary>
        /// <param name="ruta_dir_virtual_enc"></param>
        /// <returns></returns>
        public List<ContenidoCdDTO> LeerArchivos(string dirVirtual)
        {
            try
            {
                List<ContenidoCdDTO> entitys = new List<ContenidoCdDTO>();
                string pathFileEnc = string.Empty;
                string linkFile = string.Empty;
                dirVirtual = dirVirtual.Replace("%2b", "+");
                string pathFisico = Encriptacion.Desencripta(dirVirtual);

                n_InfoFile[] list = this.GetArchivosPath(pathFisico);

                if (list != null)
                {
                    foreach (n_InfoFile file in list)
                    {
                        ContenidoCdDTO entity = new ContenidoCdDTO();
                        entity.Nombre = file.is_Name;
                        pathFileEnc = Encriptacion.Encripta(file.is_Path);
                        linkFile = "http://sicoes.coes.org.pe/appsgdoc/wpa.aspx?fx=" + pathFileEnc;
                        linkFile = linkFile.Replace("+", "%2b");
                        entity.Enlace = linkFile;
                        entity.Peso = file.id_Size.ToString();
                        entity.Tipo = "F";
                        entitys.Add(entity);
                    }
                }

                return entitys;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de direcotorios
        /// </summary>
        /// <param name="pathFisico"></param>
        /// <returns></returns>
        private n_InfoDirectory[] GetDirectoriosPath(string pathFisico)
        {
            try
            {
                CFile_mng servicio = new CFile_mng();
                n_InfoDirectory[] list = servicio.nf_ws_getDirectorios(pathFisico);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de archivos
        /// </summary>
        /// <param name="as_path_fisico"></param>
        /// <returns></returns>
        private n_InfoFile[] GetArchivosPath(string pathFisico)
        {
            try
            {
                CFile_mng servicio = new CFile_mng();
                n_InfoFile[] list = servicio.nf_ws_getFiles(pathFisico);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite listar el maestro de empresas
        /// </summary>
        /// <returns></returns>
        public List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarMaestroEmpresas()
        {
            return FactorySGDoc.GetConsultaRepository().ListarMaestroEmpresas();

        }

        /// <summary>
        /// Permite obtener las empreas que no son pertenecientes al registro de integrantes
        /// </summary>
        /// <returns></returns>        
        public List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasNoRI()
        {
            return FactorySGDoc.GetConsultaRepository().ListarEmpresasNoRI();
        }

        /// <summary>
        /// Permite listar las empreas del registro de integrantes tipo
        /// </summary>
        /// <param name="tipoEmprCodi"></param>
        /// <returns></returns>        
        public List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasRIPorTipo(int tipoEmprCodi)
        {
            return FactorySGDoc.GetConsultaRepository().ListarEmpresasRIPorTipo(tipoEmprCodi);
        }

        #endregion

        private static readonly ILog Logger = LogManager.GetLogger(typeof(SGDocAppServicio));

        GeneralAppServicio servicioGeneral = new GeneralAppServicio();

        public List<DocTipoDTO> GetTipoDocList()
        {
            return FactorySic.GetDocTipoRepository().List();
        }

        public List<SgdEstadisticasDTO> GetList(SgdEstadisticasDTO filterData)
        {
            return FactorySic.GetSgdEstadisticasRepository().List(filterData);
        }

        public String GetCodigoDocrespuesta(int fljcodiref)
        {
            return FactorySic.GetDocFlujoRepository().GetDocRespuesta(fljcodiref);
        }

        public bool EsFeriado(DateTime adt_fecha, List<DocDiaEspDTO> an_TablaFeriados)
        {
            bool lb_feriado;
            lb_feriado = false;
            DateTime ldt_feriado;
            string ls_frecuencia;

            foreach (var ln_dr in an_TablaFeriados)
            {
                ldt_feriado = Convert.ToDateTime(ln_dr.Diafecha);
                ls_frecuencia = ln_dr.Diafrec;

                if (ldt_feriado.Day == adt_fecha.Day && ldt_feriado.Month == adt_fecha.Month) //Es un dia Feriado
                {
                    //Preguntamos si es frecuente para todos los años
                    if (adt_fecha.Year >= ldt_feriado.Year && ls_frecuencia == "S")
                    {
                        lb_feriado = true;
                        break;
                    }
                    else if (adt_fecha.Year < ldt_feriado.Year && ls_frecuencia == "S")
                    {
                        lb_feriado = true;
                        break;
                    }
                    //Fechas donde el feriado es valido solo ese año
                    else if (adt_fecha.Year == ldt_feriado.Year && ls_frecuencia == "N")
                    {
                        lb_feriado = true;
                        break;
                    }
                    else
                    {
                        lb_feriado = false;
                    }
                }
            }
            return lb_feriado;
        }

        public List<DocDiaEspDTO> ListDocDiaEsps()
        {
            return FactorySic.GetDocDiaEspRepository().List();
        }

        public int NumDiasHabiles(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            DateTime dtFechaAux = dtFechaInicio;//Convert.ToDateTime(dtFechaInicio.ToString("dd/MM/yyyy"));
            DateTime dtFechaAuxFin = dtFechaFin; //Convert.ToDateTime(dtFechaFin.ToString("dd/MM/yyyy"));
            var lsDiasFeriados = ListDocDiaEsps();
            int i = 0, DiasHabiles = 0;
            while (dtFechaAux <= dtFechaAuxFin)
            {
                var bFeriado = EsFeriado(dtFechaAux, lsDiasFeriados);
                if (!bFeriado && dtFechaAux.DayOfWeek != DayOfWeek.Saturday && dtFechaAux.DayOfWeek != DayOfWeek.Sunday)
                {
                    DiasHabiles++;
                }
                dtFechaAux = dtFechaAux.AddDays(1);
                i++;
            }
            return DiasHabiles;
        }


        public int Import(SgdEstadisticasDTO filterData)
        {
            try
            {
                int message = 0;
                DateTime fechaInicio = (filterData.FilterFechaInicio == null) ? DateTime.Now.AddDays(-1) : (DateTime)filterData.FilterFechaInicio;
                DateTime fechaFin = (filterData.FilterFechaFin == null) ? DateTime.Now : (DateTime)filterData.FilterFechaFin;
                //Lista
                List<DocFlujoDTO> GetListEstad = FactorySic.GetDocFlujoRepository().ListEstad(fechaInicio, fechaFin, filterData.ListaTipoAtencion);
                for (int i = 0; i < GetListEstad.Count; i++)
                {

                    filterData.Fljcodi = GetListEstad[i].Fljcodi;
                    filterData.Fljdetcodi = GetListEstad[i].Fljdetcodi;
                    filterData.Fljdetcodiref = GetListEstad[i].Fljdetcodiref;
                    filterData.Sgdefeccreacion = DateTime.Now;
                    filterData.Emprcodi = GetListEstad[i].Emprcodi;
                    filterData.Tipcodi = GetListEstad[i].Tipcodi;
                    filterData.Fljfecharecep = GetListEstad[i].Fljfechaproce;
                    filterData.Fljfechaorig = GetListEstad[i].Fljfechaorig;
                    filterData.Sgdefecderarearesp = GetListEstad[i].Fljfechainicio;
                    filterData.Sgdefecderdirresp = GetListEstad[i].Fljfecharecep;
                    filterData.Fljnombre = GetListEstad[i].Fljnombre;
                    filterData.Fljfechaterm = GetListEstad[i].Fljfechaterm;
                    filterData.Fljestado = GetListEstad[i].Fljestado;
                    filterData.Fljnumext = GetListEstad[i].Fljnumext;
                    filterData.Fljfechaproce = GetListEstad[i].Fljfechaproce;


                    //Calculo Area Responsable
                    List<DocFlujoDTO> listaAreasResp = FactorySic.GetDocFlujoRepository().GetAreasResponsables(GetListEstad[i].Fljcodi, filterData.ListaTipoAtencion);
                    int[] areas = logicaNivelSuperior(filterData.Emprcodi, listaAreasResp, filterData.ListaAreasPadres);
                    filterData.Areacodedest = areas[1]; //Area Responsable
                    filterData.Sgdearearespcodi = areas[1]; //Actualizacion
                    FwAreaDTO areaFWArea = FactorySic.GetFwAreaRepository().GetById(filterData.Areacodedest);
                    filterData.Sgdearearespnomb = areaFWArea.Areaname.TrimEnd();

                    //Calcular dia de semana
                    DateTime fecharegsgdoc = (DateTime)filterData.Fljfechaproce;
                    switch (Convert.ToInt32(fecharegsgdoc.DayOfWeek))
                    {
                        case 1: filterData.Sgdediadoc = "Lu"; break;
                        case 2: filterData.Sgdediadoc = "Ma"; break;
                        case 3: filterData.Sgdediadoc = "Mi"; break;
                        case 4: filterData.Sgdediadoc = "Ju"; break;
                        case 5: filterData.Sgdediadoc = "Vi"; break;
                        case 6: filterData.Sgdediadoc = "Sa"; break;
                        case 7: filterData.Sgdediadoc = "Do"; break;
                    }

                    // Se calcula direccion responsable
                    filterData.Sgdedirrespcodi = areas[0]; //Direccion Responsable
                    FwAreaDTO dirArea = FactorySic.GetFwAreaRepository().GetById(filterData.Sgdedirrespcodi);
                    filterData.Sgdedirrespnomb = dirArea.Areaname;

                    //Se calcula numero de dias de derivacion a la direccion. Fecha Proce -- Fecharecep
                    DateTime fecharecep = (DateTime)filterData.Fljfecharecep;
                    filterData.Sgdediasdedir = (fecharecep - fecharegsgdoc).Days + 1;

                    //Numero de dias de derivacion al area
                    DateTime fechainicio = (DateTime)filterData.Sgdefecderarearesp;
                    filterData.Sgdediasdearea = (fecharecep - fechainicio).Days + 1;



                    //Dias de atencion
                    DateTime? fechatermino = null;
                    if (filterData.Fljfechaterm != null) fechatermino = (DateTime)filterData.Fljfechaterm;
                    fechatermino = ((fechatermino == null) ? DateTime.Now : (DateTime)fechatermino);
                    //filterData.Sgdediasatencion = filterData.Fljestado.Equals("A") ? ((DateTime)fechatermino - fecharecep).Days + 1 : (DateTime.Now - fecharecep).Days + 1;

                    filterData.Sgdediasatencion = filterData.Fljestado.Equals("A") ? this.NumDiasHabiles(fecharecep, (DateTime)fechatermino) : this.NumDiasHabiles(fecharecep, DateTime.Now);

                    //consulta
                    SgdEstadisticasDTO estadDTO = FactorySic.GetSgdEstadisticasRepository().GetById(filterData.Fljcodi);

                    if (estadDTO != null) //Existe
                    {
                        FactorySic.GetSgdEstadisticasRepository().Update(filterData);
                    }//No existe
                    else
                    {
                        // Se graba
                        message = FactorySic.GetSgdEstadisticasRepository().Save(filterData);
                    }

                }

                //Luego de Grabar Actualiza Codigos de Aprobadas
                List<SgdEstadisticasDTO> lista = FactorySic.GetSgdEstadisticasRepository().ListCodigosRef(fechaInicio, fechaFin);
                for (int i = 0; i < lista.Count; i++)
                {
                    //                FactorySic.GetSgdEstadisticasRepository().UpdateCodiref(lista[i].Fljcodi,lista[i].Fljdetcodiref);
                    FactorySic.GetSgdEstadisticasRepository().UpdateNumext(lista[i].Fljdetcodiref, lista[i].Fljcodi);
                }

                return message;
            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);
            }
        }

        /// <summary>
        /// Revisar si solo hay Areas Apoyo -
        /// 07 - Nov - 2017
        /// </summary>
        /// <param name="listaAreasResp"></param>
        /// <returns></returns>
        public bool soloAreasApoyo(List<DocFlujoDTO> listaAreasResp)
        {

            for (int i = 0; i < listaAreasResp.Count; i++)
                if (FactorySic.GetFwAreaRepository().GetById(listaAreasResp[i].Areacode).Areapadre != 2) return false; // STS 30 Nov 2017

            return true;
        }

        /// <summary>
        /// Revisar si existe areas apoyo
        /// </summary>
        /// <param name="listaAreasResp"></param>
        /// <returns></returns>
        public bool existeAreasApoyo(List<DocFlujoDTO> listaAreasResp)
        {

            for (int i = 0; i < listaAreasResp.Count; i++)
                if (FactorySic.GetFwAreaRepository().GetById(listaAreasResp[i].Areacode).Areapadre == 2) return true; // STS 30 Nov 2017

            return false;
        }

        /// <summary>
        /// Logica para identificar Nivel Superior en caso Diferentes Areas Responsables
        /// </summary>
        /// <param name="listaAreasResp"></param>
        /// <returns></returns>
        public int[] logicaNivelSuperior(int emprcodi, List<DocFlujoDTO> listaAreasResp, String ListaAreasPadres)
        {
            try
            {
                int[] result = new int[2];
                int nAreas = listaAreasResp.Count;
                List<DocFlujoDTO> listaAreasNivel = new List<DocFlujoDTO>();
                int[] areacode = new int[20];
                int nareas = 0;
                int npadres = 0;
                int[] areapadre = new int[20];
                int areapadretmp = 0, areapadretmp2 = 0;
                bool escalarareapadre = false;

                //07 - Nov - 2017 STS
                if (nAreas == 1)
                {
                    result[0] = listaAreasResp[0].Areapadre; //Direccion Responsable
                    result[1] = listaAreasResp[0].Areacode; //Area Responsable
                    return result;
                }
                else // Mas de 01 Area
                {
                    //Mas de un area responsable en el ultimo nivel?
                    if (listaAreasResp[0].Fljdetnivel != listaAreasResp[1].Fljdetnivel) //SI no es asi, se selecciona el nivel mayor - array = 0
                    {
                        result[0] = listaAreasResp[0].Areapadre; //Direccion Responsable
                        result[1] = listaAreasResp[0].Areacode; //Area Responsable
                        return result;
                    }

                    //Mas de un area responsable en el nivel mayor
                    //Solo analizar sobre el nivel mayor
                    int nivelInicial = listaAreasResp[0].Fljdetnivel;

                    for (int k = 0; k < nAreas; k++)
                        if (listaAreasResp[k].Fljdetnivel == nivelInicial)
                            listaAreasNivel.Add(listaAreasResp[k]);


                    if (existeAreasApoyo(listaAreasNivel) == true)
                    {
                        if (soloAreasApoyo(listaAreasNivel) == true)
                        { // Un area de apoyo es responsable
                            for (int j = 0; j < listaAreasNivel.Count; j++)
                            {
                                if (listaAreasNivel[j].Fljcadatencion.Substring(1, 1) == "1") //Prep Rpta 
                                {
                                    result[0] = listaAreasNivel[j].Areapadre; //Direccion Responsable
                                    result[1] = listaAreasNivel[j].Areacode; //Area Responsable
                                    return result;
                                }
                            }

                            //Llego aqui porque no encontro un area con prep Rpta. Se asigna al primero de la lista
                            result[0] = listaAreasNivel[0].Areapadre; //Direccion Responsable
                            result[1] = listaAreasNivel[0].Areacode; //Area Responsable
                            return result;

                        }
                        else
                        {
                            if (listaAreasNivel.Count == 2) //La area que no es de apoyo es responsable
                            {
                                for (int j = 0; j < listaAreasNivel.Count; j++)
                                    if (FactorySic.GetFwAreaRepository().GetById(listaAreasNivel[j].Areacode).Areapadre != 2) // STS 30 Nov 2017
                                    {
                                        result[0] = listaAreasNivel[j].Areapadre; //Direccion Responsable
                                        result[1] = listaAreasNivel[j].Areacode; //Area Responsable
                                        return result;
                                    }
                            }
                            else
                            {
                                //Retirar areas de apoyo para analizar las restantes
                                for (int j = 0; j < listaAreasNivel.Count; j++) //STS 30 Nov 2017
                                    if (FactorySic.GetFwAreaRepository().GetById(listaAreasNivel[j].Areacode).Areapadre == 2 && listaAreasNivel[j].Fljcadatencion.Substring(1, 1) == "0") //Area de Apoyo que no tiene Prep Rpta 
                                    {
                                        listaAreasNivel.RemoveAt(j); //nAreas--; STS 30 Nov 2017 
                                    }

                                //Debe continuar....
                            }
                        }

                    }
                    else // No hay areas de Apoyo
                    {
                        //Debe continuar....
                    }

                }
                //Fin del cambio 07 - Nov - 2017


                for (int k = 0; k < 20; k++) { areacode[k] = -1; areapadre[k] = -1; };

                if (listaAreasNivel.Count > 0 /**&& emprcodi != 1**/) //No aplica para documentos de COES internos
                {

                    for (int j = 0; j < listaAreasNivel.Count; j++)
                    {
                        if (j == 0 && listaAreasNivel[j].Areacode != -1 && listaAreasNivel[j].Areapadre == -1)
                        {
                            result[0] = listaAreasNivel[j].Areacode; //Direccion Responsable
                            result[1] = listaAreasNivel[j].Areacode; //Area Responsable
                            areapadre[0] = result[0];
                        }
                        else if (j == 0 && listaAreasNivel[j].Areacode == -1 && listaAreasNivel[j].Areapadre != -1)
                        {
                            result[0] = listaAreasNivel[j].Areapadre; //Direccion Responsable
                            result[1] = -1; //Area Responsable
                            areapadre[0] = result[0];
                        }
                        else if (j == 0 && listaAreasNivel[j].Areacode != -1 && listaAreasNivel[j].Areapadre != -1)
                        {
                            result[0] = listaAreasNivel[j].Areapadre; //Direccion Responsable
                            result[1] = listaAreasNivel[j].Areacode; //Area Responsable
                            areapadre[0] = result[0];
                        }
                        else //j>0
                        {
                            bool encontro = false;
                            bool encontropadre = false;
                            for (int n = 0; n < 20; n++)
                                if (listaAreasNivel[j].Areacode == areacode[n]) { encontro = true; break; }
                            if (!encontro)
                            {
                                areacode[nareas] = listaAreasNivel[j].Areacode;
                                areapadretmp = ((listaAreasNivel[j].Areapadre == 0) || (listaAreasNivel[j].Areapadre == -1)) ? areacode[nareas] : listaAreasNivel[j].Areapadre;
                                encontropadre = false;
                                //Busca Areas Padres almacenadas para absorver
                                for (int h = 0; h <= npadres; h++) if (areapadretmp == areapadre[h]) encontropadre = true;
                                if (encontropadre == false)
                                { //Valida para almacenar padre
                                    areapadretmp2 = FactorySic.GetFwAreaRepository().GetDirResponsable(areapadretmp);
                                    encontropadre = false;
                                    for (int i = 0; i <= npadres; i++) if (areapadretmp2 == areapadre[i]) encontropadre = true;
                                    if (encontropadre == false)
                                        escalarareapadre = true;

                                    npadres++;
                                    areapadre[npadres] = areapadretmp;

                                }

                                nareas++;
                            }
                        }
                    }

                    //Evaluar si hay mas de un areapadre
                    if (escalarareapadre == true)
                    {
                        result[0] = FactorySic.GetFwAreaRepository().GetDirResponsable(areapadre[0]) == 0 ? areapadre[0] : FactorySic.GetFwAreaRepository().GetDirResponsable(areapadre[0]);
                        result[1] = areapadre[0];
                    }
                    else
                    {
                        result[0] = (areapadre[0] == -1 ? result[0] : areapadre[0]);
                        result[1] = (areacode[0] == -1 ? result[1] : areacode[0]);
                    }

                }
                else
                { //Para documentos internos en COES
                    result[0] = -1; //Direccion Responsable
                    result[1] = -1; //Area Responsable
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);
            }

        }

        public int GetDirResponsable(int codigoArea)
        {
            int codigoDirResp = FactorySic.GetFwAreaRepository().GetDirResponsable(codigoArea);
            if (codigoDirResp == 0) codigoDirResp = codigoArea;
            return codigoDirResp;
        }

        public List<SiTipoempresaDTO> GetCompanyTypes()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
        }

        public List<FwAreaDTO> GetAreas()
        {
            return FactorySic.GetFwAreaRepository().List();
        }

        public int GetPlazoDoc(int tipoDoc)
        {
            DocTipoDTO docDto = FactorySic.GetDocTipoRepository().GetById(tipoDoc);
            return docDto.Tipplazo;
        }
        public List<SiEmpresaDTO> GetListCompany(SgdEstadisticasDTO filterDto)
        {
            if (filterDto == null) return FactorySic.GetSiEmpresaRepository().ListEmpresasClientes();
            else return FactorySic.GetSiEmpresaRepository().ListaEmpresasSeleccionadas(filterDto);
        }

        public DocTipoDTO GetTipoDoc(int tipoDoc)
        {
            DocTipoDTO docDto = FactorySic.GetDocTipoRepository().GetById(tipoDoc);
            return docDto;
        }

        public SiTipoempresaDTO GetTipoEmpresa(int tipoemprcodi)
        {
            return FactorySic.GetSiTipoempresaRepository().GetById(tipoemprcodi);
        }

        public FwAreaDTO GetFwArea(int codigoArea)
        {
            FwAreaDTO codigoDirResp = FactorySic.GetFwAreaRepository().GetById(codigoArea);
            return codigoDirResp;
        }

        public SiEmpresaDTO GetCompany(int codigoEmpresa)
        {
            SiEmpresaDTO codigoDirResp = FactorySic.GetSiEmpresaRepository().GetById(codigoEmpresa);
            return codigoDirResp;
        }

        /// <summary>
        /// Permite obtener los datos para analizar
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public void ObtenerDatosMigracion(int mes, int anio)
        {
            List<MigracionDTO> entitys = FactorySGDoc.GetConsultaRepository().ObtenerDatosMigracion(mes, anio);

            string path = @"\\fs-web\sg\";
            string pathDocumento = @"datasgdocfilex\";
            string pathCD = @"cd\";


            foreach (MigracionDTO entity in entitys)
            {
                string docPDF = string.Empty;
                string docVOL = string.Empty;
                string docCD = string.Empty;
                string tipoVOL = string.Empty;
                string pathFile = pathDocumento + COES.Base.Tools.Util.ObtenerNombreMes(entity.Mes) + "_" + entity.Anio.ToString() + @"\" + entity.Flujocodi.ToString() + @"\";
                string pathAdicional = pathCD + COES.Base.Tools.Util.ObtenerNombreMes(entity.Mes) + "_" + entity.Anio.ToString() + @"\" + entity.Flujocodi.ToString() + @"\";

                List<FileData> list = FileServer.ListarArhivos(pathFile, path);

                if (list.Count == 1)
                {
                    docPDF = path + pathFile + list[0].FileName;
                }
                else if (list.Count > 1)
                {
                    string[] cad = entity.Fileruta.Split('/');
                    string ultimo = cad.Last();

                    if (list.Where(x => x.FileName.ToUpper() == ultimo.ToUpper()).Count() == 1)
                    {
                        docPDF = path + pathFile + list.Where(x => x.FileName.ToUpper() == ultimo.ToUpper()).FirstOrDefault().FileName;
                    }
                }

                List<FileData> listCD = FileServer.ListarArhivos(pathAdicional + @"CD\", path);

                if (listCD.Count > 0)
                {
                    docCD = path + pathAdicional + @"CD\";
                }

                List<FileData> listVOL = FileServer.ListarArhivos(pathAdicional + @"VOL\", path);

                if (listVOL.Count == 1)
                {
                    docVOL = path + pathAdicional + @"VOL\" + listVOL[0].FileName;
                    tipoVOL = "F";
                }
                else if (listVOL.Count > 0)
                {
                    docVOL = path + pathAdicional + @"VOL\";
                    tipoVOL = "D";
                }

                try
                {
                    FactorySGDoc.GetConsultaRepository().ActualizarDatosMigracion(docPDF, docVOL, docCD, tipoVOL, entity.Flujocodi, entity.Flujodetcodi);
                }
                catch (Exception ex)
                {
                    FactorySGDoc.GetConsultaRepository().ActualizarDatosMigracion("-1000", "-1000", "-1000", tipoVOL, entity.Flujocodi, entity.Flujodetcodi);
                }
            }
        }

    }
    /// <summary>
    /// Clase para almacenar los valores de los parametros
    /// </summary>
    public class ParametroSGDoc
    {
        public EstadoRegistro CodEstadoRegistro { get; set; }
        public EstadoAtencion CodEstadoAtencion { get; set; }
        public EstadoTramite CodEstadoTramite { get; set; }
        public int Origen { get; set; }
        public int Rol { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public int IdEmpresaRemitente { get; set; }
        public int IdArea { get; set; }
        public string IdRegistro { get; set; }
        public string NumDocumento { get; set; }
        public string Asunto { get; set; }
        public string CodAtencion { get; set; }
        public string CodigosEmpresa { get; set; }
        public bool IndImportante { get; set; }
        public int IdEtiqueta { get; set; }
        public bool RemExterno { get; set; }
        public int TipoRecepcion { get; set; }
        public bool ConRpta { get; set; }
        public bool Plazo { get; set; }
        public int IdPersona { get; set; }
        public string Metodo { get; set; }
        public string Indicador { get; set; }
        public string Bandeja { get; set; }
        public bool IndTodos { get; set; }
        public bool AllPlazo { get; set; }
        public bool IndJefatura { get; set; }
        public bool IndBusqueda { get; set; }
        public bool IndDiaActual { get; set; }
    }
}
