using COES.Dominio.DTO.Scada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Coordinacion.Models
{
    public class RegistroCorreoModel
    {
        public List<ScEmpresaDTO> ListaEmpresas { get; set; }
        public TrObservacionCorreoDTO Entidad { get; set; }
        public List<TrObservacionCorreoDTO> ListaCuentas { get; set; }
    }
}