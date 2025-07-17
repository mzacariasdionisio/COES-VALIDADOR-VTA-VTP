using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.PMPO.Helper;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.PMPO.Models
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

        public List<PmpoNotificacion> ListaResultadoNotif { get; set; }
        public List<PmpoReportOsinergDTO> ListaReporteOsinerg { get; set; }
        public string NameFileLog { get; set; }

        public MeFormatoDTO PlazoFormato { get; set; }
        public List<MeFormatoDTO> ListaFormatoAmpl { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }
        public MeAmpliacionfechaDTO Ampliacion { get; set; }
        public string HtmlReporte { get; set; }

        public List<PmpoConfiguracionDTO> ListaConfig { get; set; }

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

        //plazos
        public int DiaPlazo { get; set; }
        public int DiaFinPlazo { get; set; }
        public int DiaFinFueraPlazo { get; set; }

        public int MinutoPlazo { get; set; }
        public int MinutoFinPlazo { get; set; }
        public int MinutoFinFueraPlazo { get; set; }

        public int Mesplazo { get; set; }
        public int Mesfinplazo { get; set; }
        public int Mesfinfueraplazo { get; set; }

        public string FechaPeriodo { get; set; }
        public string MesPeriodo { get; set; }

        //filserver
        public bool TienePermisoDTI { get; set; }
        public string PathPrincipal { get; set; }
        public string PathAplicativo { get; set; }
        public string PathSubcarpeta { get; set; }

        //Importación masiva
        public List<FileData> ListaDocumentos { get; set; }
        public FileData Documento { get; set; }
        public string FileName { get; set; } //nombre archivo
        public string PathLog { get; set; }
    }
}