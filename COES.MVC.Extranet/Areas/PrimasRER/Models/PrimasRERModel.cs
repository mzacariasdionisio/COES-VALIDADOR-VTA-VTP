using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.PrimasRER;

namespace COES.MVC.Extranet.Areas.PrimasRER.Models
{
    public class PrimasRERModel
    {
        //Acciones de permisos y validación
        public bool bNuevo { get; set; }
        public bool bEditar { get; set; }
        public bool bGrabar { get; set; }
        public bool bEliminar { get; set; }
        public bool bAccion { get; set; }
        public string sMensajeError { get; set; }
        public string sMensaje { get; set; }
        public int iRegError { get; set; }

        public bool EsNuevaSolicitud { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        //Identificadores a tablas

        public int Emprcodi { get; set; }
        public int Pericodi { get; set; }

        //Listas de tablas
        public List<COES.Dominio.DTO.Transferencias.EmpresaDTO> ListaEmpresas { get; set; }
        public List<IndPeriodoDTO> ListaPeriodos { get; set; }
        public List<COES.Dominio.DTO.Transferencias.RerSolicitudEdiDTO> ListaSolicitudEDI { get; set; }
        public List<COES.Dominio.DTO.Transferencias.RerCentralDTO> ListaCentral { get; set; }
        public List<COES.Dominio.DTO.Transferencias.RerOrigenDTO> ListaOrigen { get; set; }
        public List<COES.Dominio.DTO.Transferencias.RerEnergiaUnidadDTO> ListaEnergiaUnidad { get; set; }

        //Entidades
        public COES.Dominio.DTO.Transferencias.EmpresaDTO EntidadEmpresa { get; set; }
        public IndPeriodoDTO EntidadPeriodo { get; set; }
        public COES.Dominio.DTO.Transferencias.RerSolicitudEdiDTO EntidadSolicitudEDI { get; set; }
        public COES.Dominio.DTO.Transferencias.RerCentralDTO EntidadCentralRER { get; set; }
        public COES.Dominio.DTO.Transferencias.RerOrigenDTO EntidadOrigen { get; set; }
        public COES.Dominio.DTO.Transferencias.RerEnergiaUnidadDTO EntidadEnergiaUnidad { get; set; }

        public string Fechainicio { get; set; }
        public string Fechafin { get; set; }
        public string Horainicio { get; set; }
        public string Horafin { get; set; }
        public string jsonListaEnergiaUnidad { get; set; }
        public HttpPostedFileBase[] ArchivoSustento { get; set; }
        public string nombreArchivo { get; set; }
        public int iperimes { get; set; }
        public int iperianio { get; set; }

        // entidades de Procesar calculo
        public List<RerAnioTarifarioDTO> ListaAniosTarifario { get; set; }
        public List<RerVersionDTO> ListaVersiones { get; set; }
        public List<ReporteDTO> ListaReportes { get; set; }

    }
}
