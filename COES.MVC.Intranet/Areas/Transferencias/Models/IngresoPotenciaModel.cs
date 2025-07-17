using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class IngresoPotenciaModel
    {
        public List<IngresoPotenciaDTO> ListaIngresoPotencia { get; set; }
        public IngresoPotenciaDTO Entidad { get; set; }
        public int IdIngresoPotencia { get; set; }
    }
}