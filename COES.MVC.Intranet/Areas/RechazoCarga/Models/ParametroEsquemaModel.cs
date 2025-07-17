using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Models
{
    public class ParametroEsquemaModel
    {
        public List<SelectListItem> Anios { get; set; }
        public List<SiTipoempresaDTO> Tipos { get; set; }
        //public int Año { get; set; }
        //public List<RcaParametroEsquemaDTO> Parametros { get; set; }

        public List<TipoInstancia> ListaTipoInstancia { get; set; }

        public FormatoModel FormatoHandsonTable { get; set; }

    }

    public class TipoInstancia
    {
        public int IipoInstanciaId { get; set; }
        public string TipoInstanciaTexto { get; set; }
    }
}