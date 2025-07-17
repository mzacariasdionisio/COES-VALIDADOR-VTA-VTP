using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Models
{
    /// <summary>
    /// Model para el comparativo ejecutado - servicio rpf
    /// </summary>
    public class ComparativoModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }
        public string Fecha { get; set; }
    }

    /// <summary>
    /// Model para las equivalencias entre puntos
    /// </summary>
    public class EquivalenciaModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListCentrales { get; set; }
        public List<MePtorelacionDTO> ListaServicioRpf { get; set; }
        public List<MePtorelacionDTO> ListaDespacho { get; set; }
        public string Empresa { get; set; }
        public string Central { get; set; }
        public int IdCentral { get; set; }
    }
}