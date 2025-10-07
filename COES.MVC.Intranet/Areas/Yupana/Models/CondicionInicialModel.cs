using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Yupana.Helper;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Yupana.Models
{
    public class CondicionInicialModel
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
        public List<TipoInformacion> ListaSemana { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public List<PrRestricCfgDTO> ModosGenerales { get; set; }
        public List<PrRestricCfgDTO> Recursos { get; set; }
        public PrRestricCfgDTO Recurso { get; set; }
        public DatoCondicionesIniciales DataCondicionInicial{ get; set; }
        public List<PrGrupoDTO> ListaModosOpCOES { get; set; }
        public List<EqEquipoDTO> ListaUnidadesCOES { get; set; }
        public List<PrGrupoDTO> ListaRerCOES { get; set; }

    }
}