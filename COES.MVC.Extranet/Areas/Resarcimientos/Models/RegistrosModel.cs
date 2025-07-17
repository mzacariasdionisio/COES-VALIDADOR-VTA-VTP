using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Resarcimientos.Models
{
    public class RegistrosModel
    {

        private List<RegistroPuntoEntregaModel> listaPuntosEntrega = new List<RegistroPuntoEntregaModel>();
        private List<RegistroRechazoCargaModel> listaRechazoCarga = new List<RegistroRechazoCargaModel>();
        private List<System.Web.Mvc.SelectListItem> listaEmpresas = new List<System.Web.Mvc.SelectListItem>();

        public string Registro { get; set; }
        public string Titulo { get; set; }
        public string Key { get; set; }
        public int EmpresaGeneradora { get; set; }
        public int Periodo { get; set; }
        public int Cliente { get; set; }
        public int PEntrega { get; set; }
        public int Ntension { get; set; }


        public int NroPaginado { get; set; }

        public List<RegistroPuntoEntregaModel> ListaPuntosEntrega
        {
            get { return listaPuntosEntrega; }
            set { listaPuntosEntrega = value; }
        }

        public List<RegistroRechazoCargaModel> ListaRechazoCarga
        {
            get { return listaRechazoCarga; }
            set { listaRechazoCarga = value; }
        }
    }
}