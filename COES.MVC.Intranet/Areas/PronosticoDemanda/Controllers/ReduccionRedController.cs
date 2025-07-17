using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Models;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using System.Configuration;
using log4net;
using COES.MVC.Intranet.Controllers;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Controllers
{
    public class ReduccionRedController : Controller
    {
        PronosticoDemandaAppServicio servicio = new PronosticoDemandaAppServicio();

        public ActionResult Index()
        {
            ReduccionRedModel model = new ReduccionRedModel();

            model.ListVersion = this.servicio.GetListVersion();
            model.ListBarraPopCP = this.servicio.GetListBarraCPDisponibles(ConstantesProdem.Prcatecodi, 0);
            model.ListBarraPopPM = this.servicio.GetListBarraPMDisponibles(ConstantesProdem.Prcatecodi, 0);
            model.ListBarraDefecto = this.servicio.GetListBarraDefecto(ConstantesProdem.Prcatecodi, 0);
            return View(model);
        }

        /// <summary>
        /// Lista la grilla del menu principal
        /// </summary>
        /// <param name="version"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public JsonResult ReduccionRedList(int version, List<string> tipo)
        {
            List<object> data = new List<object>();
            //string tipos = (tipo.Count != 0) ? string.Join(",", tipo) : "0";
            data = this.servicio.ListReduccionVersion(version, tipo);

            return Json(data);
        }

        /// <summary>
        /// Rellenar el filtro de nivel
        /// </summary>
        /// <param name="barracp"></param>
        /// <returns></returns>
        public JsonResult RefreshListCbo(int version)
        {
            ReduccionRedModel model = new ReduccionRedModel();

            model.ListBarraPopCP = this.servicio.GetListBarraCPDisponibles(ConstantesProdem.Prcatecodi, version);
            model.ListBarraPopPM = this.servicio.GetListBarraPMDisponibles(ConstantesProdem.Prcatecodi, version);

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;

        }

        public JsonResult RefreshListDefecto(int version)
        {
            ReduccionRedModel model = new ReduccionRedModel();

            model.ListBarraDefecto = this.servicio.GetListBarraDefecto(ConstantesProdem.Prcatecodi, version);
            
            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;

        }

        /// <summary>
        /// Metodo para refrescar la lista de PM
        /// </summary>
        /// <param name="barracp"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public JsonResult UpdatePopupPM(List<int> barracp, int version)
        {
            ReduccionRedModel model = new ReduccionRedModel();
            model.ListBarraPopPM = this.servicio.ListBarrasPM(barracp, version);

            var JsonResult = Json(model.ListBarraPopPM);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }


        public JsonResult UpdatePopupPMEdit(int version)
        {
            ReduccionRedModel model = new ReduccionRedModel();
            model.ListBarraPopPM = this.servicio.ListBarrasPMEdit(version);

            var JsonResult = Json(model.ListBarraPopPM);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datareduccion"></param>
        /// <param name="dataperdida"></param>
        /// <param name="barracp"></param>
        /// <param name="barrapm"></param>
        /// <param name="nombre"></param>
        /// <param name="valSum"></param>
        /// <returns></returns>
        public JsonResult SaveReduccionRed(string[][] datareduccion, string[][] dataperdida, int[] barracp, int[] barrapm, string version, string nombre, int valSum)
        {
            PrnReduccionRedDTO entity = new PrnReduccionRedDTO();
            ReduccionRedModel model = new ReduccionRedModel();

            int respuesta = 2;
            string msjCabecera = "Estas relaciones ya existen...";
            string msjCuerpo = "";

            List<PrnReduccionRedDTO> validacionRegistros = this.servicio.ValidandoRegistrosReduccionRed(barracp, barrapm, version);

            if (validacionRegistros.Count == 0)
            {
                respuesta = 1;
                bool validacionGauss = false;
                if (valSum == 1)
                {
                    validacionGauss = this.servicio.ValidandoSumaPM(datareduccion, dataperdida, barracp, barrapm, version);
                }
                else {
                    validacionGauss = true; 
                }

                if (validacionGauss == true)
                {
                    for (int i = 0; i < barracp.Length; i++)
                    {
                        for (int j = 0; j < barrapm.Length; j++)
                        {
                            entity.Prnredbarracp = barracp[i];
                            entity.Prnvercodi = Convert.ToInt32(version);
                            entity.Prnredbarrapm = barrapm[j];
                            entity.Prnredgauss = Convert.ToDecimal(datareduccion[i][j + 1]);
                            entity.Prnredperdida = Convert.ToDecimal(dataperdida[i][j + 1]);
                            entity.Prnredfecha = DateTime.Now;
                            entity.Prnredusucreacion = User.Identity.Name;
                            entity.Prnrednombre = nombre;
                            entity.Prnredtipo = ConstantesProdem.ReduccionRedAgrupacion;
                            this.servicio.ReduccionRedSave(entity);
                        }
                    }
                }
                else
                {
                    respuesta = 0;
                }
            }
            else {
                foreach (var item in validacionRegistros)
                {
                    msjCuerpo += "  "+ item.Nombrecp.Trim() + " - " + item.Nombrepm.Trim();
                }
            }
            model.flagPop = respuesta;
            model.Mensaje = msjCabecera + msjCuerpo;

            return Json(model);
        }

        /// <summary>
        /// Graba un reduccion defecto
        /// </summary>
        /// <param name="version"></param>
        /// <param name="barra"></param>
        /// <param name="gauss"></param>
        /// <param name="perdida"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public JsonResult SaveReduccionDefecto(int version, int barra, decimal gauss, decimal perdida, string nombre, int id)
        {
            PrnReduccionRedDTO entity = new PrnReduccionRedDTO();
            ReduccionRedModel model = new ReduccionRedModel();

            entity.Prnredbarracp = barra;
            entity.Prnvercodi = version;
            entity.Prnredbarrapm = barra;
            entity.Prnredgauss = gauss;
            entity.Prnredperdida = perdida;
            entity.Prnredfecha = DateTime.Now;
            entity.Prnrednombre = nombre;
            entity.Prnredtipo = ConstantesProdem.ReduccionRedDefecto;


            if (id != -1)
            {
                entity.Prnredcodi = id;
                entity.Prnredusumodificacion = User.Identity.Name;
                entity.Prnredfecmodificacion = DateTime.Now;
                this.servicio.ReduccionRedUpdate(entity);
            }
            else {
                entity.Prnredusucreacion = User.Identity.Name;
                this.servicio.ReduccionRedSave(entity);
            }

            return Json(model);
        }


        /// <summary>
        /// Actulzia una reduccion de red
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cp"></param>
        /// <param name="pm"></param>
        /// <param name="version"></param>
        /// <param name="barraCpOld"></param>
        /// <param name="nombre"></param>
        /// <param name="valSum"></param>
        /// <returns></returns>
        public JsonResult UpdateReduccionRed(decimal[][] data, string[] cp, string pm, string version, string[] barraCpOld, string nombre, int valSum)
        {
            PrnReduccionRedDTO entity = new PrnReduccionRedDTO();
            ReduccionRedModel model = new ReduccionRedModel();

            decimal sumGauss = 0;
            model.Mensaje = "Los Valores Gaussianos asociados a la barra PM suman mas de 1.....";
            model.flagPop = 0;
            int valNegativo = 0;

            for (int i = 0; i < data.Length; i++)
            {
                //if (data[i][0] <= 0)
                //{
                //    valNegativo = 1;
                //    model.Mensaje = "No se admiten valores nulos o negativos .....";
                //}
                sumGauss += data[i][0];
            }

            //valSum == 1, se pide que se valide la suma = 1
            //valSum == otro, no se debe aplicar que la suma sea 1
            if (valSum != 1) {
                sumGauss = 1;
            }

            if (sumGauss <= 1 && valNegativo == 0)
            {

                foreach (var item in barraCpOld)
                {
                    this.servicio.DeletePrnReduccionRedBarraVersion(Convert.ToInt32(pm), Convert.ToInt32(item), Convert.ToInt32(version));
                }

                for (int i = 0; i < cp.Length; i++)
                {
                    entity.Prnredbarracp = Convert.ToInt32(cp[i]);
                    entity.Prnvercodi = Convert.ToInt32(version);
                    entity.Prnredbarrapm = Convert.ToInt32(pm);
                    entity.Prnredgauss = Convert.ToDecimal(data[i][0]);
                    entity.Prnredperdida = Convert.ToDecimal(data[i][1]);
                    entity.Prnredfecha = DateTime.Now;
                    entity.Prnredusucreacion = User.Identity.Name;
                    entity.Prnrednombre = nombre;
                    entity.Prnredtipo = ConstantesProdem.ReduccionRedAgrupacion;
                    this.servicio.ReduccionRedSave(entity);
                }

                model.Mensaje = "Actualizacion exitosa.....";
                model.flagPop = 1;
            }

            return Json(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="barracp"></param>
        /// <returns></returns>
        public JsonResult DeleteReduccionRed(int reduccionred, int version)
        {
            ReduccionRedModel model = new ReduccionRedModel();

            this.servicio.DeletePrnReduccionRed(reduccionred, version);
            model.Mensaje = "Registro Eliminado...";

            return Json(model.Mensaje);
        }


        /// <summary>
        /// Metodo para registrar una version
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public JsonResult SaveVersion(int codigo, string nombre, string estado)
        {

            PrnVersionDTO entity = new PrnVersionDTO();

            //Graba una version
            if (codigo == -1)
            {
                if (estado == "A")
                {
                    string iestado = "I";
                    this.servicio.UpdatePrnVersionInactivo(iestado);
                }
                entity.Prnvernomb = nombre;
                entity.Prnverestado = estado;
                entity.Prnverusucreacion = User.Identity.Name;
                entity.Prnverfeccreacion = DateTime.Now;
                this.servicio.SavePrnVersion(entity);
            }
            //Actualiza una version
            else {
                if (estado == "A")
                {
                    string iestado = "I";
                    this.servicio.UpdatePrnVersionInactivo(iestado);
                }
                entity.Prnvercodi = codigo;
                entity.Prnvernomb = nombre;
                entity.Prnverestado = estado;
                entity.Prnverusumodificacion = User.Identity.Name;
                entity.Prnverfecmodificacion = DateTime.Now;
                this.servicio.UpdatePrnVersion(entity);
            }

            return Json("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public JsonResult ListVersion(int codigo)
        {
            ReduccionRedModel model = new ReduccionRedModel();
            model.Version = this.servicio.GetListVersionById(codigo);

            return Json(model.Version);
        }

        /// <summary>
        /// Refresca el combo Version
        /// </summary>
        /// <returns></returns>
        public JsonResult RefreshVersion()
        {
            ReduccionRedModel model = new ReduccionRedModel();
            model.ListVersion = this.servicio.GetListVersion();

            return Json(model.ListVersion);
        }

        /// <summary>
        /// Exportar la grilla a un documento excel.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public JsonResult Exportar(string[][] form)
        {
            PrnFormatoExcel data = new PrnFormatoExcel()
            {

                Contenido = form,
                Cabecera = new string[] {
                    "NOMBRE","BARRA PM", "BARRA CP",
                    "PTO.MEDICION", "PTO.MEDICION(ID)","GAUSS", "PERDIDA"
                },
                AnchoColumnas = new int[] {
                    50,50,50,50,50,50,50
                },
                Titulo = "PRONOSTICO DE LA DEMANDA - PRODEM",
                Subtitulo1 = "Reduccion de red",
                Subtitulo2 = "sub2"
            };
            string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
            string filename = "Reporte ReduccionRed";
            string reporte = this.servicio.ExportarReporteSimple(data, pathFile, filename);

            return Json(reporte);
        }

        /*
        public JsonResult Exportar(string[][] form)
        {
            List<PrnFormatoExcel> datos = new List<PrnFormatoExcel>();

            //Creating excel book
            PrnFormatoExcel book = new PrnFormatoExcel()
            {
                Titulo = "PRUEBA",
                Subtitulo1 = "SUB-PRUEBA",
                AnchoColumnas = new int[] { 50, 50, 50, 50, 50 },
                NombreLibro = "Hoja1"
            };

            //Creating headers
            //First header row
            book.NestedHeader1 = new List<PrnExcelHeader>();
            PrnExcelHeader head = new PrnExcelHeader() { Etiqueta = "", Columnas = 1 };
            book.NestedHeader1.Add(head);
            head = new PrnExcelHeader() { Etiqueta = "EMPRESA EMP01", Columnas = 4 };
            book.NestedHeader1.Add(head);

            //Second header row
            book.NestedHeader2 = new List<PrnExcelHeader>();
            head = new PrnExcelHeader() { Etiqueta = "HORA", Columnas = 1 };
            book.NestedHeader2.Add(head);
            head = new PrnExcelHeader() { Etiqueta = "PUNTO A", Columnas = 1 };
            book.NestedHeader2.Add(head);
            head = new PrnExcelHeader() { Etiqueta = "PUNTO B", Columnas = 1 };
            book.NestedHeader2.Add(head);
            head = new PrnExcelHeader() { Etiqueta = "PUNTO C", Columnas = 1 };
            book.NestedHeader2.Add(head);
            head = new PrnExcelHeader() { Etiqueta = "PUNTO D", Columnas = 1 };
            book.NestedHeader2.Add(head);

            //Creating book content
            //dumi data
            string[] itv = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min);
            decimal[] med = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, new PrnMedicion48DTO());
            string[] str_med = Array.ConvertAll(med, x => x.ToString());

            string[][] content = new string[5][];
            content[0] = itv;
            for (int i = 1; i < content.Length; i++)
            {
                content[i] = str_med;
            }
            book.Contenido = content;
            datos.Add(book);
            
            string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
            string filename = "Reporte ReduccionRed";
            string reporte = this.servicio.ExportarReporteConLibros(datos, pathFile, filename);

            return Json(reporte);
        }*/

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString() + file;
            string app = Constantes.AppExcel;
            return File(path, app, sFecha + "_" + file);
        }
    }
}
