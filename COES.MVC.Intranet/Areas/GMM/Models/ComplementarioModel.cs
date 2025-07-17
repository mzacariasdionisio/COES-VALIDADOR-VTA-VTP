using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.GMM.Models
{
    public class ComplementarioModel
    {
        public List<GmmDatInsumoDTO> listadoComplementarios { get; set; }
        public List<GmmDatInsumoDTO> listadoEntregas { get; set; }
        public List<GmmDatInsumoDTO> listadoInflexibilidad { get; set; }
        public List<GmmDatInsumoDTO> listadoRecaudacion { get; set; }
        public List<GmmDatInsumoDTO> listadoInsumos { get; set; }
        public GmmGarantiaDTO insumos1vez { get; set; }
        public List<GmmGarantiaDTO> listadoinsumos1vez { get; set; }
        public string Estado { get; set; }


        public ComplementarioModel()
        {
            listadoComplementarios = new List<GmmDatInsumoDTO>();
            listadoEntregas = new List<GmmDatInsumoDTO>();
            listadoInflexibilidad = new List<GmmDatInsumoDTO>();
            listadoRecaudacion = new List<GmmDatInsumoDTO>();
            listadoinsumos1vez = new List<GmmGarantiaDTO>();
        }
    }
}