using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.ReportesMedicion.Models
{
    public class FormatoReporteModel
    {
        public List<FwModuloDTO> ListaModulos { get; set; }
        public List<MeReporteDTO> ListaReporte { get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<MeCabeceraDTO> ListaCabecera { get; set; }
        public List<MeReporptomedDTO> ListaReportPto { get; set; }
        public List<MePtomedicionDTO> ListaReportPtoCal { get; set; }
        public MeReporptomedDTO HojaPto { get; set; }

        public int IdReporte { get; set; }
        public string Nombre { get; set; }
        public int? IdModulo { get; set; }
        public int? IdCabecera { get; set; }
        public int? IdLectura { get; set; }

        public int CheckEmpresa { get; set; }
        public int CheckEquipo { get; set; }
        public int CheckTipoEquipo { get; set; }
        public int CheckTipoMedida { get; set; }
        public int CheckEsGrafico { get; set; }

        public List<FwAreaDTO> ListaAreasCoes { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<MeFormatohojaDTO> ListaFormatoHojas { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public List<MeOrigenlecturaDTO> ListaOrigenLectura { get; set; }

        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresaFormato { get; set; }
        public List<SiTipoinformacionDTO> ListaMedidas { get; set; }
        public List<MeTipopuntomedicionDTO> ListaTipoMedidas { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }

        public List<MePtomedicionDTO> ListaPtos { get; set; }
        public List<TipoInformacion> ListaResolucionPto { get; set; }

        public int FormatoCodigo { get; set; }

        public int IdTipoempresa { get; set; }
        public int IdEmpresa { get; set; }
        public int IdFamilia { get; set; }
        public int IdEquipo { get; set; }
        public int HojaNumero { get; set; }
        public int EmpresaCodigo { get; set; }
        public string HeadColFormato { get; set; }
        public int HeadColAncho { get; set; }
        public int HeadColPos { get; set; }
        public int HeadColActivo { get; set; }
        public decimal HeadColLimsup { get; set; }

        public int Resultado { get; set; }
        public string StrResultado { get; set; }
        public string StrMensaje { get; set; }
        public string StrDetalle { get; set; }
        public string StrError { get; set; }

        public int? IdArea { get; set; }

        public int Periodo { get; set; }
        public int Horizonte { get; set; }
        public int Resolucion { get; set; }
        public int DiaPlazo { get; set; }
        public int MinutoPlazo { get; set; }

        public int AllEmpresa { get; set; }
        public string Descripcion { get; set; }

        public int Accion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int Ptomedicodi { get; set; }

        //paginacion
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }

        public int Origlectcodi { get; set; }
        public int Reptiprepcodi { get; set; }
        public int Mrepcodi { get; set; }

        public List<SiMenureporteTipoDTO> ListaTipoReporte { get; set; }

        public List<SiMenureporteDTO> ListaMenuReporte { get; set; }

        //mensaje de algún error
        public string MensajeErrorLectura { get; set; }
        public string ReporcodiValido { get; set; }

        public string NombreEjeY { get; set; }

        public bool EsReporteEditable { get; set; }
        public string IndicadorCopiado { get; set; }

    }
}