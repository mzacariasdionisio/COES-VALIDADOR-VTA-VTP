using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using System;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IEOD.Models
{
    public class PublicacionIEODModel
    {
        public string Resultado { get; set; }
        public string Resultado2 { get; set; }
        public string Resultado3 { get; set; }
        public string Mensaje { get; set; }
        public List<string> ListaMensaje { get; set; }
        public string Detalle { get; set; }
        public string FiltroFechaDesc { get; set; }

        public List<string> ResultadosHtml { get; set; }
        public List<GraficoWeb> Graficos { get; set; }

        public GraficoWeb Grafico { get; set; }
        public List<SerieDuracionCarga> ListaGrafico { get; set; }

        public int Total { get; set; }

        public MeFormatoDTO Formato { get; set; }
        public List<MeHojaptomedDTO> ListaHojaPto { get; set; }
        public List<EqAreaDTO> ListaSubestacion { get; set; }
        public List<RegistroSerie> ListaDataProdenergia { get; set; }
        public List<MaximaDemandaDTO> ListaResumenDemanda { get; set; }
        public List<TipoEnergiaPrimariaIEOD> ListaTipoEnergiaPrimaria { get; set; }
        public List<MeReporptomedDTO> ListaAreaOperativa { get; set; }

        public List<TipoInformacionPR5> ListaSemanas { get; set; }
        public string TipoSemana { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Titulo { get; set; }
        public string Titulo2 { get; set; }
        public string Titulo3 { get; set; }
        public string Titulo4 { get; set; }
        public string SubTitulo { get; set; }

        public DateTime Fecha { get; set; }
        public FechasPR5 ObjFecha { get; set; }

        public string NombreArchivo { get; set; }
        public string SheetName { get; set; }

        public int NRegistros { get; set; }

        public List<SiVersionDTO> ListaVersion { get; set; }
        public List<SiVersionDTO> ListaVersion2 { get; set; }
        public bool TieneVersion { get; set; }
        public bool VersionSeleccionada { get; set; }

        public List<MeGpsDTO> ListaGPS { get; set; }

        public List<MeDespachoProdgenDTO> ListaDetalleProduccion { get; set; }

        public List<InfSGIFilaResumenInterc> ListaDetalleInterconexion { get; set; }
    }

    public class TipoInformacionPR5
    {
        public int IdTipoInfo { get; set; }
        public string NombreTipoInfo { get; set; }
        public string FechaIniSem { get; set; }
        public string FechaFinSem { get; set; }
    }

}