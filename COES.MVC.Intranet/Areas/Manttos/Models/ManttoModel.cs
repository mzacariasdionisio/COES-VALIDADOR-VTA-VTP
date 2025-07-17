using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Manttos.Models
{
    public class ManttoModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }
        public List<TipoEventoDTO> ListaTipoEvento { get; set; }
        public ManManttoDTO Mantto { get; set; }
        public int IdMantto { get; set; }   
    }

    public class ManRegistroModel
    {
        public int IdManRegistro { get; set; }
        public int NombManRegistro { get; set; }
        public List<TipoEventoDTO> ListaTipoEvento { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime FechaLimite { get; set; }

    }
}