using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Publico.Areas.ReportePotencia.Models
{
    public class DatoComboBox
    {
        public string Descripcion { get; set; }
        public string Valor { get; set; }
    }
    public class EstadoModel
    {
        public string EstadoCodigo { get; set; }
        public string EstadoDescripcion { get; set; }
    }
    public class IndexConfiguracionModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
    }

    public class ListadoConfiguracion
    {
        public List<PrGrupoDTO> ListadoModosOperacionConfigurados { get; set; }
    }
}