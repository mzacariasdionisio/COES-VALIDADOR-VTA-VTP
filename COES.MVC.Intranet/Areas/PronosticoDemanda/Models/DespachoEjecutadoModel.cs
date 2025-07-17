using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    /// <summary>
    /// Model para el despacho ejecutado - servicio rpf
    /// </summary>
    public class DespachoEjecutadoModel
    {
        //
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }
        public List<PrnMedicioneqDTO> ListaDespachos { get; set; }
        public List<EqAreaDTO> ListaAreaOperativa { get; set; }
        public string Fecha { get; set; }
        public int Equicodi { get; set; }
        public List<ComparativoItemModel> ListComparativo { get; set; }
    }

    /// <summary>
    /// Estructura para manajear los datos medio horarios
    /// </summary>
    public class ComparativoItemModel 
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public string Central { get; set; }
        public string Hora { get; set; }
        public decimal ValorRPF { get; set; }
        public decimal ValorDespacho { get; set; }
        public decimal Desviacion { get; set; }
        public decimal Pronostico { get; set; }
    }

}