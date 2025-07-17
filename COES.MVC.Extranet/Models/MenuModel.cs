using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Models
{
    public class MenuModel
    {
        public int OpcionId { get; set; }
        public string Nombre { get; set; }
        public int PadreId { get; set; }
        public int NroOrden { get; set; }
        public string OpcionURL { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public bool Selected { get; set; }
        public bool Folder { get; set; }
        public bool Expanded { get; set; }
        public string DesControlador { get; set; }
        public string DesAccion { get; set; }
        private List<MenuModel> listItems = new List<MenuModel>();

        public List<MenuModel> ListItems
        {
            get { return listItems; }
            set { listItems = value; }
        }
    }
}