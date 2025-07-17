using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class EmpresaModel
    {
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public SiEmpresaDTO Entidad { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public string Emprabrev { get; set; }
        public string EmprCodOsinergmin { get; set; }
        public string Emprnomb { get; set; }
        public int Tipoemprcodi { get; set; }
        public string Emprrazsocial { get; set; }
        public string Emprruc { get; set; }
        public string Emprsein { get; set; }
        public string Emprestado { get; set; }
        public string Inddemanda { get; set; }
        public int Emprcodi { get; set; }
        public string Indicador { get; set; }
        public string IndicadorGrabar { get; set; }
        public string Emprcoes { get; set; }
        public string Emprdomiciliada { get; set; }
        public string Emprambito { get; set; }
        public int Emprrubro { get; set; }
        public string Empragente { get; set; }
        public List<SiEmpresadatDTO> ListaSiEmpresadat { get; set; }
        public string Fecha { get; set; }
        public string Mensaje { get; set; }
        public string ListaEmpresas { get; set; }
        public string IndicadorIntegrante { get; set; }
        public string Emprusucreacion { get; set; }
        public DateTime? Emprfeccreacion { get; set; }
        public string Emprusumodificacion { get; set; }
        public DateTime? Emprfecmodificacion { get; set; }
        public string Emprindproveedor { get; set; }

    }

    public class EmpresaMMEModel
    {
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public SiEmpresaMMEDTO Entidad { get; set; }
        public List<SiEmpresaMMEDTO> ListaEmpresaMME { get; set; }
        public List<SiEmpresaMMEDTO> ListaHistorialMME { get; set; }
        public int Emprmmecodi { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int TipoEmprcodi { get; set; }
        public DateTime? Emprfecparticipacion { get; set; }
        public DateTime? Emprfecretiro { get; set; }
        public string Emprcomentario { get; set; }
        public string Emprusucreacion { get; set; }
        public DateTime? Emprfeccreacion { get; set; }
        public string Emprusumodificacion { get; set; }
        public DateTime? Emprfecmodificacion { get; set; }
        public string Indicador { get; set; }
        public string IndicadorGrabar { get; set; }

    }
}

