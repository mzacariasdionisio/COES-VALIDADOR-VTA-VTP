using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_FICTECITEM
    /// </summary>
    public partial class FtFictecItemDTO : EntityBase
    {
        public int Ftitcodi { get; set; }
        public int Fteqcodi { get; set; }
        public int? Propcodi { get; set; }
        public int? Concepcodi { get; set; }
        public int? Ftpropcodi { get; set; }
        public int Ftitorden { get; set; }
        public int Ftitactivo { get; set; }
        public string Ftitnombre { get; set; }
        public int Ftitdet { get; set; }
        public int Ftitpadre { get; set; }
        public string Ftitorientacion { get; set; }
        public int Ftittipoitem { get; set; }
        public int Ftittipoprop { get; set; }

        public DateTime? Ftitfeccreacion { get; set; }
        public string Ftitusucreacion { get; set; }
        public DateTime? Ftitfecmodificacion { get; set; }
        public string Ftitusumodificacion { get; set; }
    }

    public partial class FtFictecItemDTO
    {
        public string FtitfeccreacionDesc { get; set; }
        public string FtitfecmodificacionDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public string Proptipo { get; set; }
        public string Propnomb { get; set; }
        public string Propunidad { get; set; }
        public string Propfile { get; set; }
        public string Concepdesc { get; set; }
        public string Concepunid { get; set; }
        public string Conceptipo { get; set; }
        public string Ftproptipo { get; set; }
        public string Ftpropnomb { get; set; }
        public string Ftpropunidad { get; set; }

        public List<FtFictecItemDTO> ListaHijos { get; set; } = new List<FtFictecItemDTO>();
        public int Origen { get; set; }
        public string OrigenDesc { get; set; }
        public int OrigenTipo { get; set; }
        public string OrigenTipoDesc { get; set; }
        public string ItemUnidad { get; set; }
        public string ItemUnidadDesc { get; set; }
        public string ItemTipo { get; set; }
        public string Valor { get; set; }
        public string ValorFormula { get; set; }
        public string FechaVigenciaDesc { get; set; }
        public int CheckCeroCorrecto { get; set; }
        public string CheckCeroCorrectoDesc { get; set; }
        public string Orden { get; set; }
        public string OrdenPad { get; set; }
        public int OrdenNumerico { get; set; }
        public long OrdenNumericoLong { get; set; }
        public bool EsArchivo { get; set; }
        public bool EsNumerico { get; set; }
        public int Nivel { get; set; }
        public string ListaNotacodi { get; set; }
        public string ListaNotanum { get; set; }
        public int FilaExcel { get; set; }

        //para mostrar comentario
        public int Codigo { get; set; }
        public string Itemcomentario { get; set; }
        public string ItemValConfidencial { get; set; }
        public string ItemSustentoConfidencial { get; set; }
        public string Itemocultocomentario { get; set; }

        public bool TieneColorInflexOp { get; set; }
        public int Concepflagcolor { get; set; }
        public int Propflagcolor { get; set; }

        public string Fitcfginstructivo { get; set; }
        public bool EsArchivoAdjuntado { get; set; }

        public bool EsReplica { get; set; }
        public int? FtitcodiDependiente { get; set; }
        public int? FtitcodiFuente { get; set; }

        public bool HabilitarCheckValorConfidencial { get; set; }
        public bool HabilitarCheckSustentoConfidencial { get; set; }
        public int Ftedatcodi { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivoAdjunto { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivoValor { get; set; }
        public int Fitcfgcodi { get; set; }
        public int Fteeqcodi { get; set; }

        public string UrlItemSustento { get; set; }
        public string ItemSustento { get; set; }

        public string ItemUsuariomodif { get; set; }
        public string ItemFechamodif { get; set; }
        public bool EsValorVigente { get; set; }

        public FtExtEnvioRevisionDTO EnvioRevision { get; set; }
        public FtExtEnvioRevareaDTO EnvioRevisionAreas { get; set; }
        public int Ftetcodi { get; set; } // ETAPA
        public int Estenvcodi { get; set; }
        public int Ambiente { get; set; }

        //lista de areas por item
        public List<FtExtRelareaitemcfgDTO> ListaAreasXItem { get; set; }
        public int IdreaRevision { get; set; }

        public bool EsFilaEditableExtranet { get; set; }
        public bool EsFilaRevisableIntranet { get; set; }
    }

    /// <summary>
    /// Clase Json para jqueryTree
    /// </summary>
    public class TreeItemFichaTecnica
    {
        public string title { get; set; }
        public bool folder { get; set; }
        public bool expanded { get; set; }
        public FtFictecItemDTO data { get; set; }
        public List<TreeItemFichaTecnica> children { get; set; }
    }

    /// <summary>
    /// Clase para obtener la lista de equipos o grupos de una ficha tecnica
    /// </summary>
    public class ElementoFichaTecnica
    {
        public int Tipo { get; set; }
        public int TipoId { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public string Empresa { get; set; }
        public string Ubicacion { get; set; }
        public string Estado { get; set; }
        public int Existe { get; set; }
        public string TituloFicha { get; set; }
        public string OrigenTipoDesc { get; set; }
        public string OrigenPadreTipoDesc { get; set; }
        public string Ftveroculto { get; set; }
        public string FtverocultoExtranet { get; set; }
        public string FtverocultoIntranet { get; set; }
        public int? Areacodi { get; set; }
        public int Emprcodi { get; set; }

        public string NombreHoja { get; set; }
        public int Fteqcodi { get; set; }

        public int? FlagCheckComent { get; set; }
        public int? FlagCheckSust { get; set; }
        public int? FlagCheckFech { get; set; }
    }

    public class NotificacionFTItems
    {
        public int Ftitcodi { get; set; }
        public string Ftitnombre { get; set; }
        public string FtitnombreNew { get; set; }
        public int Ftitorden { get; set; }
        public int FtitordenNew { get; set; }
        public int? NumNotas { get; set; }
        public int? NumNotasNew { get; set; }
        public string Ftitusumodificacion { get; set; }
        public DateTime? Ftitfecmodificacion { get; set; }
    }
}
