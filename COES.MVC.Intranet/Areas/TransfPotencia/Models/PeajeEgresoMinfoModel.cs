using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class PeajeEgresoMinfoModel : BaseModel
    {
        //Objetos del Modelo IngresoPotefr
        public VtpPeajeEgresoMinfoDTO Entidad { get; set; }
        public List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo { get; set; }

        //Otros Objetos para la consulta
        public string[] ListaTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
        public string[] ListaLicitacion = { "Si", "No" };
        public string[] ListaCalidad = { "Final", "Preliminar", "Mejor información", "Final/Preliminar", "Final/Mejor información", "Preliminar/Mejor información" };
    }
}