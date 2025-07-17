using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.Eventos.Models
{
    public class AnalisisFallasModel
    {
        public string EmpresaPropietaria { get; set; }
        public string EmpresaInvolucrada { get; set; }
        public string TipoEquipo { get; set; }
        public string Estado { get; set; }
        //public string Impugnacion { get; set; }
        //public string TipoReunion { get; set; }
        public string RNC { get; set; }
        public string ERACMF { get; set; }
        public string ERACMT { get; set; }
        public string EDAGSF { get; set; }
        public string DI { get; set; }
        public string DF { get; set; }
        //public string FuerzaMayor { get; set; }
        //public string Anulado { get; set; }
        public string EveSinDatosReportados { get; set; }

        //
        public string FechEvento { get; set; }
        public string Emprcodi { get; set; }
        public string Descripcion { get; set; }
        public string ObsArchivo { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public string NombreArchivo { get; set; }

        public string Afecodi { get; set; }
        public List<EventoDTO> LstEvento { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public AnalisisFallaDTO oAnalisisFallaDTO { get; set; }
        public EventoDTO oEventoDTO { get; set; }

        //Lista Solicitudes
        public List<AfSolicitudRespDTO> ListSolicitudes { get; set; }
        public int CodSolicitud { get; set; }
        public List<string> ListaArchivos { get; set; }
        public string ArchivoFinal { get; set; }

        public List<SiEmpresaDTO> ListaEmpresa { get; internal set; }
        public List<SiFuentedatosDTO> ListaTipoInformacion { get; internal set; }
        public int NRegistros { get; internal set; }
        public List<AfInterrupSuministroDTO> ListaInterrupSuministro { get; internal set; }

        public List<MeEnvioDTO> ListaMeEnvio { get; set; }
        public string FechaEnvio { get; set; }
        public int Enviocodi { get; set; }

        public string Modulo { get; set; }
    }
}