using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Interconexiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Interconexiones.Models
{
    public class InformeModel
    {
        public int Anio { get; set; }
        public List<MeInformeInterconexionDTO> ListaVersion { get; set; }
        public List<InformacionSemana> ListaSemana { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

    }

    public class AntecedentesModel
    {
        public List<MeInformeAntecedenteDTO> ListaAntecedentes { get; set; }
        public MeInformeAntecedenteDTO Entidad { get; set; }
        public int Codigo { get; set; }
        [AllowHtml]
        public string Contenido { get; set; }
    }
}