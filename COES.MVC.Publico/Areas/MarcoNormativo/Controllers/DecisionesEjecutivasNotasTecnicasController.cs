using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Publico.Areas.MarcoNormativo.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.MarcoNormativo.Controllers
{
    public class DecisionesEjecutivasNotasTecnicasController : Controller
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        PortalAppServicio servicio = new PortalAppServicio();

        /// <summary>
        /// Muestra el listado
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DecisionEjecutivaModel model = new DecisionEjecutivaModel();
            model.ListaDecisiones = this.ObtenerArchivos(1.ToString());
            model.ListaNotas = this.ObtenerArchivos(2.ToString());

            return View(model);
        }

       
        /// <summary>
        /// Permite obtener la lista de archivos
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private List<WbDecisionejecutivaDTO> ObtenerArchivos(string tipo)
        {
            List<WbDecisionejecutivaDTO> list = this.servicio.GetByCriteriaWbDecisionejecutivas(tipo).OrderByDescending(x => (DateTime)x.Desejefechapub).ToList();

            string path = RutaDirectorio.DirectorioNotasTecnicas;

            foreach (WbDecisionejecutivaDTO itemFile in list)
            {
                string filename = string.Format(ConstantesPortal.PrefijoFileNotaTecnica, itemFile.Desejecodi, itemFile.Desejeextension);

                if (FileServer.VerificarExistenciaFile(path, filename, string.Empty))
                {
                    itemFile.Icono = Util.ObtenerIcono(ConstantesBase.TipoFile, Constantes.CaracterPunto + itemFile.Desejeextension);
                    itemFile.FileName = filename;
                }

                foreach (WbDecisionejecutadoDetDTO item in itemFile.ListaItems)
                {
                    filename = string.Format(ConstantesPortal.PrefijoFileNotaTecnicaDet, itemFile.Desejecodi, item.Dejdetcodi, item.Desdetextension);

                    if (FileServer.VerificarExistenciaFile(path, filename, string.Empty))
                    {
                        item.Icono = Util.ObtenerIcono(ConstantesBase.TipoFile, Constantes.CaracterPunto + item.Desdetextension);
                        item.FileName = filename;
                    }
                }
            }

            return list;
        }
    }
}
