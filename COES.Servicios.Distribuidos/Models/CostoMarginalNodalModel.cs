using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.Servicios.Distribuidos.Models
{
    /// <summary>
    /// Model para presentacion de resultados
    /// </summary>
    public class CostoMarginalNodalModel
    {
        public List<CmCostomarginalDTO> Listado { get; set; }
        public List<CmGeneracionEmsDTO> ListadoGeneracionEms { get; set; } 
        public List<CmRestriccionDTO> ListaRestricciones { get; set; }
        public CmVersionprogramaDTO VersionPrograma { get; set; }
    }
}