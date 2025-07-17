using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.IEOD;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.IEOD.Models
{
    public class HorasOperacionModel
    {
        public List<EqEquipoDTO> ListaTipoCentral { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }
        public List<PrGrupoDTO> ListaModosOperacion { get; set; }
        public List<EqEquipoDTO> ListaGrupo { get; set; }
        public List<EveHoraoperacionDTO> ListaHorasOperacion { get; set; }
        public List<EveHoraoperacionDTO> ListaHorasOperacionAnt { get; set; }
        public List<EqEquipoDTO> ListaUnidades { get; set; }
        public List<PrGrupoDTO> ListaUnidXModoOP { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MeEnvioDTO> ListaEnvios { get; set; }
        public SiPlazoenvioDTO PlazoEnvio { get; set; }

        public bool EsEmpresaVigente { get; set; }

        public int IdEmpresa { get; set; }
        public int IdTipoCentral { get; set; }
        public int TipoHidrologiaCentral { get; set; }
        public int IdCentralSelect { get; set; }
        public int FlagCentralRsvFriaToRegistrarUnidad { get; set; }
        public int IdEquipoOrIdModo { get; set; }
        public int IdGrupoModo { get; set; }
        public int IdEquipo { get; set; }
        public string EtiquetaFiltro { get; set; }
        public string EtiquetaFiltroGrupoModo { get; set; }
        public string ActFiltroCtral { get; set; }
        public List<UnidadesExtra> MatrizunidadesExtra { get; set; }

        public string FechaEnvio { get; set; }
        public string FechaListado { get; set; }
        public string FechaAnterior { get; set; }
        public string Fecha { get; set; }
        public string FechaSiguiente { get; set; }
        public string HoraIni { get; set; }
        public string FechaFin { get; set; }
        public string HoraFin { get; set; }
        public string Fechahorordarranq { get; set; }
        public string Hophorordarranq { get; set; }
        public string FechaHophorparada { get; set; }
        public string Hophorparada { get; set; }
        public List<string> ListaFechaArranque { get; set; }

        public string Hopdesc { get; set; }
        public string Hopobs { get; set; }
        public int IdPos { get; set; }
        public int TipoModOp { get; set; }
        public int IdTipoOperSelect { get; set; }

        public int IdEnvio { get; set; }
        public int OpfueraServ { get; set; }
        public int OpArranqBlackStart { get; set; }
        public int OpEnsayope { get; set; }

        public SiParametroValorDTO ParamSolar { get; set; }

        public List<EveSubcausaeventoDTO> ListaTipoOperacion { get; set; }

        public int Hopnotifuniesp { get; set; }
        public string MensajeNotifuniesp { get; set; }

        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacionIntervencion { get; set; }
    }

}