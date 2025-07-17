using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class ModeloEnvioModel
    {
        public List<PeriodoDTO> ListaPeriodos { get; set; }
        public List<RecalculoDTO> ListaRecalculo { get; set; }
        public List<TrnModeloEnvioDTO> ListaEnvio { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
    }
}