using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Hidrologia.Models
{
    public class BusquedaFormatoMedicionModel
    {
        public List<MeOrigenlecturaDTO> ListaOrigenLectura { get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<FwAreaDTO> ListaAreasCoes { get; set; }
    }

    public class FormatoHidrologiaModel
    {
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<MeFormatohojaDTO> ListaFormatoHojas { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<FwAreaDTO> ListaAreasCoes { get; set; }
        public List<MeOrigenlecturaDTO> ListaOrigenLectura { get; set; }
        public List<MeHojaptomedDTO> ListaHojaPto { get; set; }
        public MeHojaptomedDTO HojaPto { get; set; }
        public List<Dominio.DTO.Sic.EmpresaDTO> ListaEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa2 { get; set; }
        public List<SiEmpresaDTO> ListaEmpresaFormato { get; set; }
        public List<SiTipoinformacionDTO> ListaMedidas { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<MeModuloDTO> ListaModulo { get; set; }
        public List<MePtomedicionDTO> ListaPtos { get; set; }
        public List<PrGrupoDTO> ListaGrupos { get; set; }
        public List<MeCabeceraDTO> ListaCabecera { get; set; }
        public int CodigoApp { get; set; }
        public int FormatoCodigo { get; set; }
        public int IdFormato { get; set; }
        public int? IdFormato2 { get; set; }
        public int IdHoja { get; set; }
        public int IdEmpresa { get; set; }
        public int HojaNumero { get; set; }
        public int EmpresaCodigo { get; set; }
        public string HeadColFormato { get; set; }
        public int HeadColAncho { get; set; }
        public int HeadColPos { get; set; }
        public int HeadColActivo { get; set; }
        public decimal HeadColLimsup { get; set; }
        public int Resultado { get; set; }
        public int IdLectura { get; set; }
        public int IdModulo { get; set; }
        public int IdArea { get; set; }
        public int IdCabecera { get; set; }
        public int Periodo { get; set; }
        public int Horizonte { get; set; }
        public int Resolucion { get; set; }
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
        public string SemanaPeriodo { get; set; }
        public string AnioPeriodo { get; set; }
        public List<GenericoDTO> ListaGenSemanas { get; set; }
        public int CheckBlanco { get; set; }
        public int CheckPlazo { get; set; }
        public int AllEmpresa { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public int Accion { get; set; }
        public List<MeHojaDTO> ListaHoja { get; set; }
        public string IndicadorHoja { get; set; }
        public List<PrCategoriaDTO> ListaTipoGrupo { get; set; }
        public List<MePlazoptoDTO> ListaPlazoPto { get; set; }

        #region Modificación Tipo punto de medición
        public List<MeTipopuntomedicionDTO> ListaTipoPuntoMedicion { get; set; }
        #endregion

        public int Formatcheckplazopunto { get; set; }

        public List<BarraDTO> ListBarra { get; set; }
        public bool IndicadorTransferencias { get; set; }

        public int IdFormatoOrigen { get; set; }
        public bool FormatoTieneCheckAdicional { get; set; }

        public string Url { get; set; }
        public string NombreOrigen { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Detalle2 { get; set; }
        public int IdFamilia { get; set; }
        public int Origlectcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int IdEquipo { get; set; }
        public int IdGrupo { get; set; }
        public int IdTipoGrupo { get; set; }
    }

    //inicio agregado
    public class VerificacionFormatoModel
    {
        public int Formatcodi { get; set; }
        public int Verifcodi { get; set; }
        public string Fmtverifestado { get; set; }
        public string Fmtverifusucreacion { get; set; }
        public string Fmtveriffeccreacion { get; set; }
        public string Fmtverifusumodificacion { get; set; }
        public string Fmtveriffecmodificacion { get; set; }
        
        public int CodigoApp { get; set; }
        public string Formatnomb { get; set; }
        public string Verifnomb { get; set; }
        public string FmtverifestadoDescripcion { get; set; }
        public List<MeVerificacionDTO> ListaVerificacion { get; set; }
        public List<MeVerificacionFormatoDTO> ListaVerificacionFormato { get; set; }
        public List<EstadoModel> ListaEstadoFlag { get; set; }
    }
    //fin agregado
}