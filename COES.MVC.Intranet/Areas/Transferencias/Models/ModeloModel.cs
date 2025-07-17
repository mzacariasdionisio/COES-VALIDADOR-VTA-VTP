using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    // ASSETEC 2019-11
    public class ModeloModel
    {
        public List<TrnModeloDTO> ListaModelos { get; set; }
        public TrnModeloDTO EntidadModelo { get; set; }
        public int IdModelo { get; set; }
        public List<TrnModeloRetiroDTO> ListaModelosRetiro { get; set; }        
        public TrnModeloRetiroDTO EntidadModeloRetiro { get; set; }        
        public int IdModeloRetiro { get; set; }

        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<BarraDTO> ListaBarras { get; set; }
        public List<CodigoRetiroDTO> ListaCodigosRetiro { get; set; }
                
        public string sError { get; set; }       
    }
}