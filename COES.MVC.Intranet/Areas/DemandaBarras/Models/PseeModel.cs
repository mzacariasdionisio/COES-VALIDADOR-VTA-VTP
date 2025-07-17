using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.DemandaBarras.Models
{
    /// <summary>
    /// Models para manejo de relacion PSSE
    /// </summary>
    public class PseeModel
    {
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public EqRelacionDTO Entidad { get; set; }
        public List<EqRelacionDTO> ListaRelacion { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }

        /// <summary>
        /// Campos para el grabado de datos
        /// </summary>
        public int Equicodi { get; set; }
        public string Nombarra { get; set; }
        public string Idgener { get; set; }
        public int Codncp { get; set; }
        public string Nombrencp { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int Relacioncodi { get; set; }
    }
}