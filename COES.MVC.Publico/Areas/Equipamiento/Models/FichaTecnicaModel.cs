using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Publico.Areas.Equipamiento.Models
{
    public class FichaTecnicaModel
    {
        public FtFichaTecnicaDTO FichaMaestra { get; set; }
        public List<FtFictecXTipoEquipoDTO> ListaFichaTecnicaSelec { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public int TipoElemento { get; set; }
        public int TipoElementoId { get; set; }
        public int Codigo { get; set; }
        public ElementoFichaTecnica Elemento { get; set; }
        public string DetalleHtml { get; set; }

        public List<ElementoFichaTecnica> ListaElemento { get; set; }
        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }

        public int Origen { get; set; }
        public int IdFicha { get; set; }
        public List<FtFictecNotaDTO> ListaNota { get; set; }
        public FtFictecXTipoEquipoDTO FichaTecnica { get; set; }
        public List<TreeItemFichaTecnica> ListaItemsJson { get; set; }
        public List<FtFictecItemDTO> ListaAllItems { get; set; }
        public List<FtFictecItemDTO> ListaTreeItems { get; set; }
        public List<FtFictecXTipoEquipoDTO> ListaHijo { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }

        public int FlagExisteComentario { get; set; }
    }
}