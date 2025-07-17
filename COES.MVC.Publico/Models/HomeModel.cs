using COES.Dominio.DTO.Sic;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Models
{
    /// <summary>
    /// Model para el home
    /// </summary>
    public class HomeModel
    {
        public List<EveEventoDTO> ListaEventos { get; set; }
        public List<WbComunicadosDTO> ListaComunicado { get; set; }
        public List<WbBannerDTO> ListaBanner { get; set; }
        public List<WbComunicadosDTO> ListaSalaPrensa { get; set; }
    }

}