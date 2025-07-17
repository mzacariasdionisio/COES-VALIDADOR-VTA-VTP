using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.IEOD.Models
{
    public class GestionAdministradorModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Mes { get; set; }
        public string MesInicio { get; set; }
        public string MesFin { get; set; }
        public List<MeEstadoenvioDTO> ListaEstadoEnvio { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }

        public List<MeEnvioDTO> ListaEnvio { get; set; }
        public List<MeCambioenvioDTO> ListaCambioEnvio { get; set; }
        public List<MeValidacionDTO> ListaValidacion { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

        public string Resultado { get; set; }
        public string NombreFortmato { get; set; }

        public int Columnas { get; set; }
        public int Resolucion { get; set; }

        public string Fecha { get; set; }
        public string FechaPlazo { get; set; }
        public int HoraPlazo { get; set; }
        public int DiaMes { get; set; }
    }

    public class BusquedaIEODModel
    {
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
        public List<MePerfilRuleDTO> ListaFormulas { get; set; }
        public List<EqAreaDTO> ListaAreaOperativa { get; set; }
        public List<EqAreaDTO> ListaSubEstacion { get; set; }
        public List<PrGrupoDTO> ListaModo { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListaUnidades { get; set; }
        public List<EstadoModel> listaEstadoSistemaA { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }

        public List<EqAreaDTO> ListaUbicacion { get; set; }

        public List<EqEquipoDTO> ListaAreas { get; set; }


        public List<MeMedicion48DTO> ListaTipoCentrales { get; set; }
        public List<MeMedicion48DTO> ListaTipoRecurso { get; set; }
        public List<MeMedicion48DTO> ListaTipoGeneracion { get; set; }
        public List<MeMedicion48DTO> ListaCombustibles { get; set; }
        public List<EveCausaeventoDTO> ListaCausa { get; set; }
        public List<SiFuenteenergiaDTO> ListaTipoCombustibles { get; set; }//Oliver
        public List<EveSubcausaeventoDTO> ListaTipoOperacion { get; set; }
        public List<PrTipogrupoDTO> ListaClasificacion { get; set; }
        public List<MeMedicion48DTO> ListaPotenciaxTipoRecurso { get; set; }
        public List<MeGpsDTO> ListaGps { get; set; }
        public string Anho { get; set; }
        public string Mes { get; set; }
        public string Semana { get; set; }
        public string Dia { get; set; }
        public string Fecha { get; set; }
        public int NroSemana { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int TensionBarra { get; set; }

        //Paginado
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

        public string none = "inline";
        public string gerencia { get; set; }
        public string informe { get; set; }
        public string anexo { get; set; }
        public string Menu { get; set; }

        public List<ListaSelect> Reptiprepcodi { get; set; }
        public List<SiMenureporteDTO> ListMenuCate { get; set; }
        public List<SiMenureporteDTO> ListMenuReporte { get; set; }
        public int CountMenu { get; set; }

    }
}