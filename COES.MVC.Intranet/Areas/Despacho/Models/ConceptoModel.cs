using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Despacho.Models
{
    public class ConceptoModel
    {
        public int IdConcepto { get; set; }
        public PrConceptoDTO Entidad { get; set; }
        public List<DatoComboBox> ListaFichatecnica { get; set; }
        public List<DatoComboBox> ListaTipoDato { get; set; }
        public List<PrCategoriaDTO> ListaCategoria { get; set; }
        public List<PrConceptoDTO> ListaConcepto { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public bool TienePermiso { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        //public bool AccionEliminar { get; set; }
        //public bool AccionGrabar { get; set; }

        public string NombreArchivo { get; set; }
        public List<PrConceptoDTO> ListaConceptosErrores { get; set; }
        public List<PrConceptoDTO> ListaConceptosCorrectos { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public FileData Documento { get; set; }
        public string FileName { get; set; } //nombre archivo
    }

    public class DatoComboBox
    {
        public string Descripcion { get; set; }
        public string Valor { get; set; }
    }
}