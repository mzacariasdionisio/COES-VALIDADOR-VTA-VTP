using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Coordinacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Coordinacion.Models
{
    /// <summary>
    /// Model para el tratamiento de equivalencia de codigos
    /// </summary>
    public class RelacionModel
    {
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqRelacionDTO> ListaRelacion { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public EqRelacionDTO Entidad { get; set; }
        public int Equicodi { get; set; }
        public string Nombarra { get; set; }
        public string Idgener { get; set; }
        public int Codncp { get; set; }
        public string Nombrencp { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int Relacioncodi { get; set; }
        public int Registro { get; set; } 
        public int Canalcodi { get; set; }
        public string Canaliccp { get; set; }
        public string Fuente { get; set; }
        public string Canaliccpint { get; set; }
        public string Canalsigno { get; set; }
        public string Canaluso { get; set; }
    }


    public class SupervisionDemandaModel
    {
        public List<EqRelacionDTO> ListaGrupos { get; set; }
        public List<SupDemandaDato> ListaDatos { get; set; }
        public string[][] Datos { get; set; }
        public int[] Indices { get; set; }
    }
}