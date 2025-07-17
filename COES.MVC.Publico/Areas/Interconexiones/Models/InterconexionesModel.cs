using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.Interconexiones.Models
{
    public class InterconexionesModel
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Resultado { get; set; }
        public string SheetName { get; set; }
        public string TituloCapExc { get; set; }
        public int IdPtomedicion { get; set; }

        public List<TipoInformacion> ListaVersion { get; set; }
        public List<MeLecturaDTO> ListaHorizonte { get; set; }
        public List<InInterconexionDTO> ListaInterconexion { get; set; }
        public GraficoWeb Grafico { get; set; }
        public List<MeMedicion24DTO> ListaEnerPot { get; set; }

        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public int IdParametro { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int IdMedidor { get; set; }
        public List<MeMedidorDTO> ListaMedidor { get; set; }
    }

    public class ContratoModel
    {
        public int Contcodi { get; set; }
        public int Conttipoper { get; set; }
        public string Contfechaini { get; set; }
        public string Contfechafin { get; set; }
        public string Lastuser { get; set; }
        public string Lastdate { get; set; }
        public decimal Contpotmax { get; set; }
        public int Emprecodi { get; set; }
        public string Contagautnom { get; set; }
        public string Contagautdirec { get; set; }
        public string Contagauttipoact { get; set; }
        public string Contagautreplegal { get; set; }
        public string Contagautemail { get; set; }
        public string Contagauttelef { get; set; }
        public string Contaghabnom { get; set; }
        public string Contaghabdirec { get; set; }
        public string Contaghabreplegal { get; set; }
        public string Contaghabemail { get; set; }
        public string Contaghabtelef { get; set; }
        //public string Contcopcontrato { get; set; }
        //public string Contacuinter { get; set; }
        public string Contfecha { get; set; }
        public string Contagautrepaut { get; set; }
        public string Contagautrepautemail { get; set; }
        public string Contagautrepauttel { get; set; }
        public int Conttipocopia { get; set; }
        public int HoraIni { get; set; }
        public int HoraFin { get; set; }
        public int Isnuevo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<TipoInformacion> ListaTipoOpContrato { get; set; }
        public List<TipoInformacion> ListaHoras { get; set; }
        public List<TipoInformacion> ListaTipoCopia { get; set; }
    }


    public class PtoMedida
    {
        public int IdMedida { get; set; }
        public string NombreMedida { get; set; }
    }

    public class TipoInformacion
    {
        public int IdTipoInfo { get; set; }
        public string NombreTipoInfo { get; set; }
    }

}