using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.PMPO.Helper;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.PMPO.Models
{
    public class RemisionModel
    {
        public string Mes { get; set; }

        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<SiTipoinformacionDTO> ListarTipoinformacion { get; set; }
        public List<MeMensajeDTO> ListaMensaje { get; set; }
        public List<PmpoFile> ListaFile { get; set; }
        public int NumMsjPendiente { get; set; }

        public string HtmlReporte { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public string Fecha { get; set; }
        public string Anho { get; set; }
        public string Semana { get; set; }
        public int NroSemana { get; set; }
        public int Periodo { get; set; }

        public int IdFormato { get; set; }
        public int IdEmpresa { get; set; }
        public int IdHoja { get; set; }
        public int IdEnvio { get; set; }
        public int Tipoinfocodi { get; set; }
        public List<MeHojaDTO> ListaMeHoja { get; set; }
        public List<MeHojaDTO> ListaMeHojaPadre { get; set; }

        public FormatoModel Tab1 { get; set; }
        public FormatoModel Tab2 { get; set; }
    }

}