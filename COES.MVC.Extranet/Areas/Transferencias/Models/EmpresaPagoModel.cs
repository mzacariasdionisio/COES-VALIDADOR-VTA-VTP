using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class EmpresaPagoModel
    {
        public List<EmpresaPagoDTO> ListaEmpresasPago { get; set; }
        public EmpresaPagoDTO Entidad { get; set; }
        public int IdArea { get; set; }
    }
}