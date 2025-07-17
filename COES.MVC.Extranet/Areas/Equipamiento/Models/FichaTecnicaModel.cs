using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.Equipamiento.Models
{
    public class FichaTecnicaModel
    {
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        

        public bool TienePermiso { get; set; }

        public FtFichaTecnicaDTO FichaMaestra { get; set; }
        public List<FtFictecXTipoEquipoDTO> ListaFichaTecnicaSelec { get; set; }
        public int Origen { get; set; }
        public int TipoElementoId { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<ElementoFichaTecnica> ListaElemento { get; set; }
        public int IdFicha { get; set; }

        public ElementoFichaTecnica Elemento { get; set; }
        public int TipoElemento { get; set; }
        public int Codigo { get; set; }
        public List<TreeItemFichaTecnica> ListaItemsJson { get; set; }
        public List<FtFictecNotaDTO> ListaNota { get; set; }
        public FtFictecXTipoEquipoDTO FichaTecnica { get; set; }
        public int FlagExisteComentario { get; set; }

        //LISTA PARA VISTA PREVIA
        public List<FtFictecItemDTO> ListaAllItems { get; set; }
        public List<FtFictecItemDTO> ListaTreeItems { get; set; }
        public List<FtFictecXTipoEquipoDTO> ListaHijo { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }
    }
}