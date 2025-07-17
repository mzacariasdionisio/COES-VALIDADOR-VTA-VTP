using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Interconexiones.Models;
using COES.MVC.Extranet.Models;

namespace COES.MVC.Extranet.Areas.Campanias.Models
{
    public class CampaniasModel
    {
        public long Proycodi { get; set; }
        public int Plancodi { get; set; }
        public int PlancodiN { get; set; }
        public string Modo { get; set; }
        public int Pericodi { get; set; }
        public string Codempresa { get; set; }
        public TransmisionProyectoDTO TransmisionProyecto { get; set; }

        public List<int> ListaHojas { get; set; }

        public List<PlanTransmisionDTO> ListaPlanTransmicion { get; set; }

        public List<ObservacionDTO> ListaObservacion { get; set; }
        public bool observacionCerrado { get; set; }

    }
 
    }