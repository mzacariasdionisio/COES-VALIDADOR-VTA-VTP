using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Intervenciones.Models
{
    public class Programacion
    {
        // -------------------------------------------------------------------------------------
        // Comandos estandar para los permisos (Botonera)
        // -------------------------------------------------------------------------------------
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Propiedades basicas del modelo de Programación.
        // -------------------------------------------------------------------------------------       
        public InProgramacionDTO Entidad { get; set; }
        public List<InProgramacionDTO> ListaProgramaciones { get; set; } = new List<InProgramacionDTO>();
        public int IdProgramacion { get; set; }
        public int IdTipoProgramacion { get; set; }
        public string sNombreProgramacion { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Listados de entedades relacionadas al modelo de Programaciones.
        // -------------------------------------------------------------------------------------
        public List<EveEvenclaseDTO> ListaTiposProgramaciones { get; set; }
        public List<InIntervencionDTO> ListaIntervenciones { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Entidades relacionadas al modelo de Intrervenciones.
        // -------------------------------------------------------------------------------------
        public EveEvenclaseDTO EntidadTipoProgramacion { get; set; }
        public InIntervencionDTO EntidadIntervencion { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Propiedades de Alertas
        // -------------------------------------------------------------------------------------
        public string sError { get; set; }
        public string sMensaje { get; set; }
        public int iNumReg { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Propiedades de Paginado
        // -------------------------------------------------------------------------------------
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }
        // -------------------------------------------------------------------------------------

        //manejo de errores
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Reporte { get; set; }
    }
}