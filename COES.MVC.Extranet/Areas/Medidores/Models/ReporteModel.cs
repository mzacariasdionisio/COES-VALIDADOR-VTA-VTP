using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Medidores.Models
{
    public class ReporteModel
    {
    }

    public class ReporteConsolidadoEnvioModel
    {
        public int IdFormato { get; set; }
        public int idEmpresa { get; set; }
        public string MesIni { get; set; }
        public string MesFin { get; set; }
        public string Fecha { get; set; }
        public int IdEnvio { get; set; }
        public string FechaEnvio { get; set; }
        public List<ConsolidadoCentral> ListaConsolidado { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioCargo { get; set; }
        public string UsuarioCorreo { get; set; }
        public string UsuarioTelefono { get; set; }
        public string FechaConsulta { get; set; }
    }

    public class ConsolidadoCentral
    {
        public string Empresa { get; set; }
        public string CentralHead { get; set; }
        public decimal total { get; set; }
        public int Ngrupo { get; set; }
        public DateTime AnoMes { get; set; }
        public List<GrupoSSAA> listaGrupo { get; set; }
    }

    public class GrupoSSAA
    {
        public string Nombre { get; set; }
        public decimal? SubTotal { get; set; }
        public short tipoG { get; set; }
    }
}