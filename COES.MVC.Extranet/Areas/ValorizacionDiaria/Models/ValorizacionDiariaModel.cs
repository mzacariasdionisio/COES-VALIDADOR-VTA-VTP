using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.ValorizacionDiaria.Models
{
    public class ValorizacionDiariaModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public int empresa { get; set; }
        public string EmprNomb { get; set; }
        public string ViewHtml { get; set; }
        public List<string> ListaSemanas { get; set; }
        public int NroSemana { get; set; }
        public string NombreMes { get; set; }
        public int Dia { get; set; }
        public string Fecha { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaPlazo { get; set; }
        public string AnhoMes { get; set; }
        public int HoraPlazo { get; set; }
        public string Resultado { get; set; }
        public int IdModulo { get; set; }
        public List<MeHojaptomedDTO> ListaHojaPto { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public List<MeHeadcolumnDTO> ListaHeadColumn { get; set; }
        public List<MePtomedicionDTO> ListaMedicion { get; set; }
        public List<PtoMedida> ListaPtoMedida { get; set; }
        public List<EqEquipoDTO> ListaCuenca { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }
        public bool EnPlazo { get; set; }
        public string ValidacionPlazo { get; set; }
        public string StrFormatCodi { get; set; }
        public string StrFormatPeriodo { get; set; }
        public string StrFormatDescrip { get; set; }
        public string cadenaLectCodi { get; set; }
        public string cadenaLectPeriodo { get; set; }
    }

    public class BusquedaModel
    {
        public List<TipoInformacion> ListaSemanas { get; set; }
    }
    
    public class PtoMedida
    {
        public int IdMedida { get; set; }
        public string NombreMedida { get; set; }
    }  

}