using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.RegistroIntegrantes
{
    public class ConstantesRegistroIntegrantes
    {
        public const string NombreReporteEnvios = "RptEnvios.xlsx";
        public const string NombreReporteEnviosWord = "RptRepresentantesLegales.docx";
        public const string NombreReporteEnviosTexto = "RptRepresentantesLegales.txt";
        public const string HojaReporteExcel = "REPORTE";
        public const string NombreLogoCoes = "coes.png";
        public const string ParametroDefecto = "-1";
        public const string FromName = "Asunto coreo";
        public const string FromEmail = "webapp@coes.org.pe";
        public const string FolderReporte = "Areas\\RegistroIntegrante\\Reporte\\";
        public const string FolderUpload = "Areas\\RegistroIntegrante\\Uploads\\";
        public const string FolderUploadRutaCompleta = @"\\coes.org.pe\archivosapp\webapp\";
        public const string FolderRI = @"RegistroIntegrantes\";



        public const int SoliCambioDenominacion = 1;
        public const int SoliCambioRepresentante = 2;
        public const int SoliBajaEmpresa = 3;
        public const int SoliFusionEmpresa = 4;
        public const int SoliCambioTipo = 5;
        public const string EsAdjunto = "S";
        public const string Emprnombcomercial = "EMPRNOMBCOMERCIAL";
        public const string Emprruc = "EMPRRUC";
        public const string Emprrazsocial = "EMPRRAZSOCIAL";
        public const string Emprsigla = "EMPRSIGLA";
        public const string SolCambioDenominacionView = "SolCambioDenominacion";
        public const string SolCambioRepresentanteView = "SolCambioRepresentante";
        public const string SolBajaEmpresaView = "SolBajaEmpresa";
        public const string SolFusionEmpresaView = "SolFusionEmpresa";
        public const string SolCambioTipoView = "SolCambioTipo";
        public const string SolicitudenCursoSI = "SI";
        public const string SolicitudenCursoNO = "NO";
        public const string SoliAceptada = "ACEPTADO";
        public const string SoliDenegada = "DENEGADO";
        public const string SoliPendiente = "PENDIENTE";

        public const string SoliAprobadoDigitalCodigo = "APROBADO_DIGITAL";
        public const string SoliAprobadoFisicaCodigo = "APROBADO_FISICA";
        public const string SoliAprobadoDigital = "REVISADO DIGITALMENTE";
        public const string SoliAprobadoFisica = "APROBADO";

        public const string SoliTodos = "TODOS";
        public const string SoliAceptadaCodi = "ACEPTADO";
        public const string SoliDenegadaCodi = "DENEGADO";
        public const string SoliPendienteCodi = "PENDIENTE";
        public const string SoliTodosCodi = "";
        public const string EnviadoSolicitudFalso = "N";
        public const string CondicionBajaCampo = "CONDICIONBAJA";
        public const string CondicionBajaVoluntario = "Retiro de integrante voluntario, plazo de retiro 90 días calendarios.";
        public const string CondicionBajaVoluntarioCodi = "VOLUNTARIO";
        public const string CondicionBajaPerdidaCondicionAgente = "Perdida de la condición de Agente.";
        public const string CondicionBajaPerdidaCondicionAgenteCodi = "PERDIDA";
        public const string CondicionBajaPerdidaCondicionObligatorio = "Perdida de las condiciones para ser integrante obligatorio.";
        public const string CondicionBajaPerdidaCondicionObligatorioCodi = "OBLIGATORIO";
        public const string CondicionBajaConclusion = "Conclusion de la operación comercial de la totalidad de sus instalaciones.";
        public const string CondicionBajaConclusionCodi = "CONCLUSION";
        public const string RepresentanteLegalTitular = "TITULAR";
        public const string RepresentanteLegalAlterno = "ALTERNO";
        public const string RepresentanteLegalTipoTitular = "T";
        public const string RepresentanteLegalTipoAlterno = "A";

        public const string RepresentanteTipoLegal = "L";
        public const string RepresentanteTipoContacto = "C";
        public const string RepresentanteTipoResponsableTramite = "R";

        public const string RpteTipoLegalDescripcion = "REPRESENTANTE LEGAL";
        public const string RpteTipoResponsableDescripcion = "RESPONSABLE TRAMITE";
        public const string RpteTipoContactoDescripcion = "PERSONA DE CONTACTO";
        public const string RpteBajaSi = "S";
        public const string RpteBajaNo = "N";
        public const string SolicitudNotificadoSi = "S";
        public const string SolicitudNotificadoNo = "N";
        public const string SolicitudEnviadoSi = "S";
        public const string SolicitudEnviadoNo = "N";
        public const string RpteCodigoTipoDocumentoDNI = "D";
        public const string RpteCodigoTipoDocumentoCarnetExtrangeria = "C";
        public const string RpteDescripcionTipoDocumentoDNI = "DNI";
        public const string RpteDescripcionTipoDocumentoCarnetExtrangeria = "CARNET EXTRANGERIA";
        public const string DetalleSolicitudRepresentanteLegalAdjuntoDNI = "DNI";
        public const string DetalleSolicitudRepresentanteLegalAdjuntoVigenciaPoder = "VP";
        public const string DetalleSolicitudRepresentanteLegalCadenaRepresentante = "Representante";
        public const string DetalleSolicitudDocumentoSustentatorio = "DocumentoSustentatorio";
        public const string TipoAgenteCodigoPrincipal = "S";
        public const string TipoAgenteCodigoSecundario = "N";
        public const string TipoAgenteDescripcionPrincipal = "Principal";
        public const string TipoAgenteDescripcionSecundario = "Secundario";
        public const string TipoAgenteInicialSi = "S";
        public const string TipoAgenteInicialNo = "N";
        public const int TipoDocumentoSustentatorioCodigoAutorizacion = 1;
        public const string TipoDocumentoSustentatorioDescripcionAutorizacion = "AUTORIZACIÓN";
        public const int TipoDocumentoSustentatorioCodigoConcesion = 2;
        public const string TipoDocumentoSustentatorioDescripcionConcesion = "CONCESIÓN";
        public const int TipoDocumentoSustentatorioCodigoDeclaracionJurada = 3;
        public const string TipoDocumentoSustentatorioDescripcionDeclaracionJurada = "DECLARACIÓN JURADA";
        public const string DetalleSolicitudTipoCadenaTipo = "Tipo";
        public const string DetalleSolicitudTipoAdjuntoArchivoDigitalizado = "Archivo";
        public const string DetalleSolicitudAgregado = "AGREGADO";
        public const string DetalleSolicitudBaja = "BAJA";
        public const string ModalidadVoluntarioObligatorioCodigoVoluntario = "VOLUNTARIO";
        public const string ModalidadVoluntarioObligatorioCodigoObligatorio = "OBLIGATORIO";
        public const string ModalidadVoluntarioObligatorioDescripcionVoluntario = "VOLUNTARIO";
        public const string ModalidadVoluntarioObligatorioDescripcionObligatorio = "OBLIGATORIO";
        public const string SoloLecturaSI = "S";

        //Revisiones
        public const string Titular = "T";
        public const string TitularDesc = "TITULAR";
        public const string AlternoDesc = "ALTERNO";
        public const string NombreRevision = "NOMBRE";
        public const string CargoRevision = "REVISION";
        public const string DNIRevision = "DNI";
        public const string OBSRevision = "OBS";
        public const string EstSGI = "ESTSGI";
        public const string EstDJR = "ESTDJR";
        public const string ReviPendiente = "PENDIENTE";
        public const string ReviFinalizado = "FINALIZADO";
        public const string PrincipalTipoComportamiento = "S";
        public const string ArchDigComent = "ARCHDIGCOM";
        public const string DocSusComent = "DOCSUSCOM";
        public const string MaxConcComent = "MAXCONCCOM";
        public const string EtapaSGI = "SGI";
        public const string EtapaDJR = "DJR";
        public const string EtapaEstadoActivo = "DJR";
        public const string RevisionDetalleAdjuntoSi = "S";
        public const string RevisionDetalleAdjuntoNo = "N";
        public const string RevisionDetalleEstadoActivo = "A";
        public const string RevisionDetalleEstadoInactivo = "I";
        public const string RevisionUsuarioCreacionWeb = "Web";
        public const string RevisionEstadoPendiente = "PENDIENTE";
        public const string RevisionEstadoConforme = "CONFORME";
        public const string RevisionEstadoObservado = "OBSERVADO";
        public const string RevisionEstadoRevisado = "REVISADO";
        public const string RevisionEstadoPendienteCodigo = "PENDIENTE";
        public const string RevisionEstadoConformeCodigo = "CONFORME";
        public const string RevisionEstadoObservadoCodigo = "OBSERVADO";
        public const string RevisionEstadoRevisadoCodigo = "REVISADO";
        public const string DerCampoRepresentante = "Representante";
        public const string DerCampoTA = "TituloAdicional";
        public const string EmpresaEstadoActivo = "A";
        public const string EmpresaEstadoRegistroPendienteCodigo = "PENDIENTE";
        public const string EmpresaEstadoRegistroAprobadoDigitalCodigo = "APROBADO_DIGITAL";
        public const string EmpresaEstadoRegistroAprobadoFisicaCodigo = "APROBADO_FISICA";
        public const string EmpresaEstadoRegistroPendiente = "PENDIENTE";
        public const string EmpresaEstadoRegistroAprobadoDigital = "REVISADO DIGITALMENTE";
        public const string EmpresaEstadoRegistroAprobadoFisica = "APROBADO";

        public const int EmpresaCondicionVarianteGenerador = 50;
        public const int EmpresaCondicionVarianteTransmisor = 50;
        public const int EmpresaCondicionVarianteDistribuidor = 50;
        public const int EmpresaCondicionVarianteUsuarioLibre = 10;

        public const string EmpresaCondicionObligatorio = "OBLIGATORIO";
        public const string EmpresaCondicionVoluntario = "VOLUNTARIO";
        public const int TipoComportamientoGeneradorCodigo = 3;
        public const int TipoComportamientoTrasmisorCodigo = 1;
        public const int TipoComportamientoDistribuidorCodigo = 2;
        public const int TipoComportamientoUsuarioLibreCodigo = 4;
        public const string TipoComportamientoPrincipalSi = "S";
        public const string TipoComportamientoPrincipalNo = "N";
        public const string TipoDocumentoIdentidadDNI = "D";
        public const string TipoDocumentoIdentidadCarneExtranjeria = "C";
        public const string RepresentanteBajaSi = "S";
        public const string RepresentanteBajaNo = "N";
        public const string RepresentanteInicialSi = "S";
        public const string RepresentanteInicialNo = "N";
        public const string RevisionNotificadoSi = "S";
        public const string RevisionNotificadoNo = "N";
        public const string RevisionFinalizadoSi = "S";
        public const string RevisionFinalizadoNo = "N";
        public const string RevisionTerminadoSi = "S";
        public const string RevisionTerminadoNo = "N";
        public const string RevisionEnviadoSi = "S";
        public const string RevisionEnviadoNo = "N";
        public const string RevisionEstadoRegistroActivo = "A";
        public const string RevisionEstadoRegistroInactivo = "I";
        public const int EtrvSGI = 1;
        public const int EtrvDJR = 2;

        // Tipo Comportamiento Campos
        public const string DocumentoSustentatorio = "DocumentoSustentatorio";
        public const string ArchivoDigitalizado = "ArchivoDigitalizado";
        public const string PotenciaInstalada = "PotenciaInstalada";
        public const string NumeroCentrales = "NumeroCentrales";
        public const string TotalLineaTransmision = "TotalLineaTransmision";
        public const string MaximaDemandaCoincidente = "MaximaDemandaCoincidente";
        public const string MaximaDemandaContratada = "MaximaDemandaContratada";
        public const string NumeroSuministrador = "NumeroSuministrador";

        //Reporte
        public const string TodosCodigoString = "-1";
        public const int TodosCodigoEntero = -1;
        public const string Todos = "TODOS";
        public const string Si = "SI";
        public const string No = "NO";
    }
}
