using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Yupana.Helper;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Yupana.Models
{
    public class RestriccionModel
    {

        public int IdFormato { get; set; }
        public int IdEnvio { get; set; }
        public string Anho { get; set; }
        public string Mes { get; set; }
        public string Semana { get; set; }
        public string Dia { get; set; }
        public string Fecha { get; set; }
        public string FechaHoy { get; set; }
        public string FechaProceso { get; set; }
        public int NroSemana { get; set; }
        public string FechaIniSem { get; set; }
        public string FechaFinSem { get; set; }
        public List<TipoInformacion> ListaSemana { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public List<PrRestricCfgDTO> Recursos { get; set; }
        public List<PrRestricCfgDTO> TermoGenerales { get; set; }
        public List<PrRestricCfgDTO> HidroGenerales { get; set; }
        public List<PrRestricCfgDTO> RerGenerales { get; set; }
        public List<PrRestricCfgDTO> RecursosTermo { get; set; }
        public List<PrRestricCfgDTO> RecursosHidro { get; set; }
        public List<PrRestricCfgDTO> RecursosRer { get; set; }
        public PrRestricCfgDTO Recurso { get; set; }
        public List<PrGrupoDTO> ListaModosOpCOES { get; set; }

        public DatoRestricciones DataRestricciones { get; set; }
        //archivos
        public string NombreArchivo { get; set; }
        public List<PrConceptoDTO> ListaConceptosErrores { get; set; }
        public List<PrConceptoDTO> ListaConceptosCorrectos { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public FileData Documento { get; set; }
        public string FileName { get; set; }
        public string NombreR { get; set; }
    }
}