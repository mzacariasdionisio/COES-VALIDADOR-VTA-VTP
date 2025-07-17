using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Coordinacion.Helper;
using COES.MVC.Intranet.Areas.Coordinacion.Models;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CambioTurno;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.Coordinacion.Controllers
{
    public class CambioTurnoController : BaseController
    {
        /// <summary>
        /// Acceso  a los datos
        /// </summary>
        CambioTurnoAppServicio servicio = new CambioTurnoAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        /// <summary>
        /// Muestra la pantalla para el registro de datos
        /// </summary>
        /// <returns></returns>
        public ActionResult Registro()
        {
            CambioTurnoModel model = new CambioTurnoModel();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaMaximo = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);
            model.ListaResponsables = this.servicio.ObtenerResponsables();
            model.IndGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return View(model);
        }

        /// <summary>
        /// Permite obtener la configuracion de la grilla
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDataGrilla(string fecha, int turno)
        {
            CambioTurnoModel model = new CambioTurnoModel();

            DateTime fechaConsulta = DateTime.Now;
            if (!string.IsNullOrEmpty(fecha))
                fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int indicador = 0;
            SiCambioTurnoDTO datos = new SiCambioTurnoDTO();
            List<SiCambioTurnoSubseccionDTO> list = this.servicio.ObtenerContenidoSecciones(fechaConsulta, turno, out datos, out indicador, User.Identity.Name);
            List<MergeModel> merge = new List<MergeModel>();
            List<int> titulos = new List<int>();
            List<int> subTitulos = new List<int>();
            List<int> agrupaciones = new List<int>();
            List<int> comentarios = new List<int>();
            List<int> adicionales = new List<int>();
            List<int> finales = new List<int>();
            List<int> mantenimientos = new List<int>();
            List<int> reprogramas = new List<int>();
            List<ValidacionListaCelda> validaciones = new List<ValidacionListaCelda>();
            List<ReprogramaCelda> reprog = new List<ReprogramaCelda>();
            List<CeldaValidacionLongitud> longitudes = new List<CeldaValidacionLongitud>();
            List<AlineacionCelda> centros = new List<AlineacionCelda>();
            List<AlineacionCelda> derechos = new List<AlineacionCelda>();

            List<string> listModosOperacion = this.servicio.ObtenerModosOperacion();
            List<string> listPersonal = this.servicio.ObtenerResponsables().Select(x => x.Pernomb).ToList();
            List<string> listURSAilstado = HelperCoordinacion.ObtieneCentralesURSAislado();
            List<string> listSubEstaciones = HelperCoordinacion.ListarSubEstaciones();
            List<string> listHoras = HelperCoordinacion.ListarHoras();
            List<string> listEquipo = HelperCoordinacion.ListarEquipos();
            List<string> listCondicion = HelperCoordinacion.ListarCondicion();
            List<string> listSiNo = HelperCoordinacion.ListaSiNo();
            List<string> listSorteo = HelperCoordinacion.ListaSorteo();
            List<string> listResultadoSorteo = HelperCoordinacion.ListaResultadoSorteo();
            List<string> listPruebaSorteo = HelperCoordinacion.ListaPruebaSorteo();
            //List<string> listReprogramas = HelperCoordinacion.ListaReprogramas(); /**/

            model.ListaResponsables = this.servicio.ObtenerResponsables();

            string[][] data = new string[list.Count + 46][];
            data = this.PintarCeldas(data);

            List<SiCambioTurnoSubseccionDTO> list11 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion11).ToList();
            List<SiCambioTurnoSubseccionDTO> list12 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion12).ToList();
            List<SiCambioTurnoSubseccionDTO> list21 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion21).OrderBy(x => x.Manhoraconex).ToList();
            List<SiCambioTurnoSubseccionDTO> list22 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion22).OrderBy(x => x.Manhoraconex).ToList();
            List<SiCambioTurnoSubseccionDTO> list31 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion31).ToList();
            List<SiCambioTurnoSubseccionDTO> list41 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion41).ToList();
            List<SiCambioTurnoSubseccionDTO> list42 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion42).ToList();
            List<SiCambioTurnoSubseccionDTO> list43 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion43).ToList();
            List<SiCambioTurnoSubseccionDTO> list44 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion44).ToList();
            List<SiCambioTurnoSubseccionDTO> list45 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion45).ToList();

            //-Agregado SCO
            List<SiCambioTurnoSubseccionDTO> list51 = list.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion51).ToList();
            //-Fin agregado SCO

            #region Pintado de datos

            int indice = 0;

            data[indice][0] = "1. DESPACHO";
            data[indice + 1][0] = "CENTRAL MARGINAL";
            data[indice + 1][1] = "RSF-AUTOMÁTICA SEIN";
            data[indice + 1][4] = "RSF-MANUAL  SEIN";
            data[indice + 1][7] = "RSF-SISTEMAS AISLADOS";
            data[indice + 2][1] = "URS";
            data[indice + 2][3] = "MAGNITUD";
            data[indice + 2][4] = "URS";
            data[indice + 2][6] = "MAGNITUD";
            data[indice + 2][7] = "URS";
            data[indice + 2][9] = "MAGNITUD";

            titulos.Add(indice);
            subTitulos.Add(indice + 1);
            subTitulos.Add(indice + 2);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice + 1, col = 1, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice + 1, col = 4, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice + 1, col = 7, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice + 2, col = 1, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 2, col = 4, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 2, col = 7, rowspan = 1, colspan = 2 });

            indice = indice + 3;
            foreach (SiCambioTurnoSubseccionDTO item in list11)
            {
                data[indice][0] = item.Despcentromarginal;
                data[indice][1] = item.Despursautomatica;
                data[indice][3] = (item.Despmagautomatica != null) ? item.Despmagautomatica.ToString() : string.Empty;
                data[indice][4] = item.Despursmanual;
                data[indice][6] = (item.Despmagmanual != null) ? item.Despmagmanual.ToString() : string.Empty;
                data[indice][7] = item.Despcentralaislado;
                data[indice][9] = (item.Despmagaislado != null) ? item.Despmagaislado.ToString() : string.Empty;

                merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 7, rowspan = 1, colspan = 2 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 0, Elementos = listModosOperacion });
                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 7, Elementos = listURSAilstado });

                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 100 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 1, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 4, Longitud = 100 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 7, Longitud = 100 });

                derechos.Add(new AlineacionCelda { Row = indice, Column = 3 });
                derechos.Add(new AlineacionCelda { Row = indice, Column = 6 });
                derechos.Add(new AlineacionCelda { Row = indice, Column = 9 });

                indice++;
            }


            if (indicador == 0)
            {
                data[indice][0] = "REPROGRAMAS";
                data[indice][1] = "HORA";
    
                data[indice][2] = "MOTIVO PRINCIPAL";

                data[indice][6] = "PREMISAS IMPORTANTES";

                merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 4 });
                merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 4 });
            }
            else
            {
      
                data[indice][0] = "REPROGRAMAS";
                data[indice][1] = "HORA";
        
                data[indice][2] = "MOTIVO PRINCIPAL";
                data[indice][5] = "ATR PUBLICADO";
                data[indice][6] = "PREMISAS IMPORTANTES";

                merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 4 });
      
            }




            subTitulos.Add(indice);

            indice = indice + 1;
            list12 = list12.OrderBy(x => x.Desphorareprog).ToList();
            foreach (SiCambioTurnoSubseccionDTO item in list12)
            {
                reprogramas.Add(indice);
                reprog.Add(new ReprogramaCelda { Row = indice, Column = 0 });
                reprog.Add(new ReprogramaCelda { Row = indice, Column = 1 });
                reprog.Add(new ReprogramaCelda { Row = indice, Column = 5 });

     
                if (indicador == 0)
                {
                    data[indice][0] = item.Despreprogramas;
                    data[indice][1] = item.Desphorareprog;
               
                    data[indice][2] = item.Despmotivorepro;
                    data[indice][5] = item.Desparchivoatr;
                    data[indice][6] = item.Desppremisasreprog;
                    data[indice][8] = item.Subseccioncodi.ToString();
                    data[indice][9] = item.Desptiporeprog;

                    merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 4 });
                    merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 4 });


                    longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 60 });
                    longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 1, Longitud = 20 });

                    longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 2, Longitud = 300 });
                 

                    longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 6, Longitud = 200 });
                }
                else
                {
               
                    data[indice][0] = item.Despreprogramas;
                    data[indice][1] = item.Desphorareprog;
           
                    data[indice][2] = item.Despmotivorepro;
                    data[indice][5] = item.Desparchivoatr;
                    data[indice][6] = item.Desppremisasreprog;
                    data[indice][8] = item.Subseccioncodi.ToString();
                    data[indice][9] = item.Desptiporeprog;


                    merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 3 });
                    merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 4 });


                    longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 60 });
                    longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 1, Longitud = 20 });

                    longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 2, Longitud = 270 });
                    longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 5, Longitud = 30 });

                    longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 6, Longitud = 200 });
           
                }





                centros.Add(new AlineacionCelda { Row = indice, Column = 1 });

                indice++;
            }

            SiCambioTurnoSeccionDTO seccion12 = datos.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion12).FirstOrDefault();
            if(seccion12 != null) { data[indice][0] = (string.IsNullOrEmpty(seccion12.Descomentario)) ? "Comentarios adicionales" : seccion12.Descomentario; }
            else data[indice][0] = "Comentarios adicionales";
            comentarios.Add(indice);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 10 });
            longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 400 });

            indice = indice + 2;

            data[indice][0] = "2.  MANTENIMIENTOS RELEVANTES";
            data[indice + 1][0] = "EQUIPO";
            data[indice + 1][1] = "TIPO";
            data[indice + 1][3] = "HORA CONEXÓN";
            data[indice + 1][4] = "CONSIDERACIONES IMPORTANTES PARA LA MANIOBRA";
            data[indice + 2][1] = "PROGR";
            data[indice + 2][2] = "CORREC";

            titulos.Add(indice);
            subTitulos.Add(indice + 1);
            subTitulos.Add(indice + 2);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice + 1, col = 1, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 4, rowspan = 2, colspan = 6 });
            merge.Add(new MergeModel { row = indice + 1, col = 3, rowspan = 2, colspan = 1 });

            indice = indice + 3;
            foreach (SiCambioTurnoSubseccionDTO item in list21)
            {
                data[indice][0] = item.Manequipo;
                if (item.Mantipo == "P") data[indice][1] = "X";
                if (item.Mantipo == "C") data[indice][2] = "X";
                data[indice][3] = item.Manhoraconex;
                data[indice][4] = item.Manconsideraciones;

                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 6 });

                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 1, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 2, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 3, Longitud = 20 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 4, Longitud = 300 });

                centros.Add(new AlineacionCelda { Row = indice, Column = 1 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 2 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 3 });

                if (item.Manhoraconex == "00:00")
                {
                    mantenimientos.Add(indice);
                }

                indice++;
            }

            SiCambioTurnoSeccionDTO seccion21 = datos.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion21).FirstOrDefault();
            if(seccion21 != null) { data[indice][0] = (string.IsNullOrEmpty(seccion21.Descomentario)) ? "Comentarios adicionales" : seccion21.Descomentario; }
            else data[indice][0] = "Comentarios adicionales";
            longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 400 });

            comentarios.Add(indice);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 10 });
            indice = indice + 2;

        
            data[indice][0] = "EQUIPO";
            data[indice][1] = "TIPO";
            data[indice][3] = "HORA CONEXÓN";
            data[indice][4] = "CONSIDERACIONES IMPORTANTES PARA LA MANIOBRA";
            data[indice + 1][1] = "PROGR";
            data[indice + 1][2] = "CORREC";

      
            subTitulos.Add(indice);
            subTitulos.Add(indice + 1);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice, col = 1, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice, col = 4, rowspan = 2, colspan = 6 });
            merge.Add(new MergeModel { row = indice, col = 3, rowspan = 2, colspan = 1 });



            indice = indice + 2;
            foreach (SiCambioTurnoSubseccionDTO item in list22)
            {
                data[indice][0] = item.Manequipo;
                if (item.Mantipo == "P") data[indice][1] = "X";
                if (item.Mantipo == "C") data[indice][2] = "X";
                data[indice][3] = item.Manhoraconex;
                data[indice][4] = item.Manconsideraciones;

                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 6 });

                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 1, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 2, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 3, Longitud = 20 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 4, Longitud = 300 });

                centros.Add(new AlineacionCelda { Row = indice, Column = 1 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 2 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 3 });

                if (item.Manhoraconex == "00:00")
                {
                    mantenimientos.Add(indice);
                }

                indice++;
            }

            data[indice][0] = "3.  SUMINISTRO DE ENERGIA";
            data[indice + 1][0] = "SUBESTACIÓN";
            data[indice + 1][1] = "MOTIVO DEL CORTE";
            data[indice + 1][5] = "HORA INICIO";
            data[indice + 1][6] = "REPOSICIÓN";
            data[indice + 1][7] = "CONSIDERACIONES IMPORTANTES";

            data[indice + 2][1] = "Deficit/Sobrecarga";
            data[indice + 2][2] = "FALLA";
            data[indice + 2][3] = "TENSIÓN";
            data[indice + 2][4] = "MANTTO";

            titulos.Add(indice);
            subTitulos.Add(indice + 1);
            subTitulos.Add(indice + 2);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice + 1, col = 1, rowspan = 1, colspan = 4 });
            merge.Add(new MergeModel { row = indice + 1, col = 5, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice + 1, col = 6, rowspan = 2, colspan = 1 });
            merge.Add(new MergeModel { row = indice + 1, col = 7, rowspan = 2, colspan = 3 });

            indice = indice + 3;
            foreach (SiCambioTurnoSubseccionDTO item in list31)
            {
                data[indice][0] = item.Sumsubestacion;
                if (item.Summotivocorte == "D") data[indice][1] = "X";
                if (item.Summotivocorte == "F") data[indice][2] = "X";
                if (item.Summotivocorte == "T") data[indice][3] = "X";
                if (item.Summotivocorte == "M") data[indice][4] = "X";
                data[indice][5] = item.Sumhorainicio;
                data[indice][6] = item.Sumreposicion;
                data[indice][7] = item.Sumconsideraciones;

                merge.Add(new MergeModel { row = indice, col = 7, rowspan = 1, colspan = 3 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 6, Elementos = listSiNo });

                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 1, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 2, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 3, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 4, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 5, Longitud = 20 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 6, Longitud = 2 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 7, Longitud = 300 });

                centros.Add(new AlineacionCelda { Row = indice, Column = 1 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 2 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 3 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 4 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 5 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 6 });

                indice++;
            }

            SiCambioTurnoSeccionDTO seccion31 = datos.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion31).FirstOrDefault();
            if(seccion31 != null) { data[indice][0] = (string.IsNullOrEmpty(seccion31.Descomentario)) ? "Comentarios adicionales" : seccion31.Descomentario; }
            else data[indice][0] = "Comentarios adicionales";
            try
            {
                data[indice][0] = (string.IsNullOrEmpty(seccion31.Descomentario)) ? "Comentarios adicionales" : seccion31.Descomentario;
            }
            catch(Exception ex)
            {

                data[indice][0] = "";
            }
            longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 400 });

            comentarios.Add(indice);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 10 });
            indice = indice + 2;

            data[indice][0] = "4.  OTROS ASPECTOS RELEVANTES PARA LA OPERACIÓN";
            data[indice + 1][0] = "Regulación de tensión";
            data[indice + 2][0] = "OPERACIÓN DE CENTRALES";
            data[indice + 2][3] = "SUBESTACIÓN";
            data[indice + 2][7] = "HORA FIN APROX";

            titulos.Add(indice);
            agrupaciones.Add(indice + 1);
            subTitulos.Add(indice + 2);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 2, col = 0, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice + 2, col = 3, rowspan = 1, colspan = 4 });
            merge.Add(new MergeModel { row = indice + 2, col = 7, rowspan = 1, colspan = 3 });

            indice = indice + 3;
            foreach (SiCambioTurnoSubseccionDTO item in list41)
            {
                data[indice][0] = item.Regopecentral;
                data[indice][3] = item.Regcentralsubestacion;
                data[indice][7] = item.Regcentralhorafin;

                merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 3, rowspan = 1, colspan = 4 });
                merge.Add(new MergeModel { row = indice, col = 7, rowspan = 1, colspan = 3 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 3, Elementos = listSubEstaciones });
                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 7, Elementos = listHoras });

                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 3, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 7, Longitud = 20 });

                centros.Add(new AlineacionCelda { Row = indice, Column = 7 });

                indice++;
            }

            SiCambioTurnoSeccionDTO seccion41 = datos.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion41).FirstOrDefault();
            if (seccion41 != null) { data[indice][0] = (string.IsNullOrEmpty(seccion41.Descomentario)) ? "Comentarios adicionales" : seccion41.Descomentario; }
            else data[indice][0] = "Comentarios adicionales";

            longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 400 });

            comentarios.Add(indice);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 10 });
            indice = indice + 2;

            data[indice][0] = "Lineas desconectadas";
            data[indice + 1][0] = "LÍNEA";
            data[indice + 1][3] = "SUBESTACIÓN";
            data[indice + 1][7] = "HORA FIN APROX";

            agrupaciones.Add(indice);
            subTitulos.Add(indice + 1);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice + 1, col = 3, rowspan = 1, colspan = 4 });
            merge.Add(new MergeModel { row = indice + 1, col = 7, rowspan = 1, colspan = 3 });

            indice = indice + 2;
            foreach (SiCambioTurnoSubseccionDTO item in list42)
            {
                data[indice][0] = item.Reglineas;
                data[indice][3] = item.Reglineasubestacion;
                data[indice][7] = item.Reglineahorafin;

                merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 3, rowspan = 1, colspan = 4 });
                merge.Add(new MergeModel { row = indice, col = 7, rowspan = 1, colspan = 3 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 3, Elementos = listSubEstaciones });
                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 7, Elementos = listHoras });

                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 3, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 7, Longitud = 20 });

                centros.Add(new AlineacionCelda { Row = indice, Column = 7 });

                indice++;
            }

            SiCambioTurnoSeccionDTO seccion42 = datos.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion42).FirstOrDefault();
            if (seccion42 != null) { data[indice][0] = (string.IsNullOrEmpty(seccion42.Descomentario)) ? "Comentarios adicionales" : seccion42.Descomentario; }
            else data[indice][0] = "Comentarios adicionales";

            longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 400 });
            comentarios.Add(indice);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 10 });
            indice = indice + 2;

            data[indice][0] = "Gestión de Mantenimientos fuera del PDO";
            data[indice + 1][0] = "EQUIPO";
            data[indice + 1][1] = "ACEPTADO";
            data[indice + 1][2] = "RECHAZADO";
            data[indice + 1][3] = "DETALLE (descripción, fecha, hora inicio, hora fin) (motivo de rechazo)";

            agrupaciones.Add(indice);
            subTitulos.Add(indice + 1);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 3, rowspan = 1, colspan = 7 });

            indice = indice + 2;
            foreach (SiCambioTurnoSubseccionDTO item in list43)
            {
                data[indice][0] = item.Gesequipo;
                if (item.Gesaceptado == "A") data[indice][1] = "X";
                if (item.Gesaceptado == "R") data[indice][2] = "X";
                data[indice][3] = item.Gesdetalle;

                merge.Add(new MergeModel { row = indice, col = 3, rowspan = 1, colspan = 7 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 0, Elementos = listEquipo });
                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 1, Elementos = listCondicion });
                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 2, Elementos = listCondicion });

                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 1, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 2, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 3, Longitud = 200 });

                centros.Add(new AlineacionCelda { Row = indice, Column = 1 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 2 });

                indice++;
            }

            SiCambioTurnoSeccionDTO seccion43 = datos.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion43).FirstOrDefault();
            if(seccion43 != null) { data[indice][0] = (string.IsNullOrEmpty(seccion43.Descomentario)) ? "Comentarios adicionales" : seccion43.Descomentario; }
            else data[indice][0] = "Comentarios adicionales";
            longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 400 });

            comentarios.Add(indice);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 10 });
            indice = indice + 2;

            data[indice][0] = "Eventos Importantes ";
            data[indice + 1][0] = "EQUIPO";
            data[indice + 1][2] = "HORA INICIO";
            data[indice + 1][3] = "REPOSICIÓN";
            data[indice + 1][4] = "RESUMEN (hora falla, carga interrumpida,reposición del equipo)";

            agrupaciones.Add(indice);
            subTitulos.Add(indice + 1);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 4, rowspan = 1, colspan = 6 });

            indice = indice + 2;
            foreach (SiCambioTurnoSubseccionDTO item in list44)
            {
                data[indice][0] = item.Eveequipo;
                data[indice][2] = item.Evehorainicio;
                data[indice][3] = item.Evereposicion;
                data[indice][4] = item.Everesumen;

                merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 6 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 3, Elementos = listSiNo });

                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 75 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 2, Longitud = 20 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 3, Longitud = 2 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 4, Longitud = 255 });

                centros.Add(new AlineacionCelda { Row = indice, Column = 2 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 3 });

                indice++;
            }
            SiCambioTurnoSeccionDTO seccion44 = datos.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion44).FirstOrDefault();
            if(seccion44 != null) data[indice][0] = (string.IsNullOrEmpty(seccion44.Descomentario)) ? "Comentarios adicionales" : seccion44.Descomentario;
            else data[indice][0] = "Comentarios adicionales";
            longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 400 });
            comentarios.Add(indice);
            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 2, colspan = 10 });
            indice = indice + 2;


            data[indice][0] = "Informes Finales de Falla próximos a vencer (Tipo N1 dentro de las 24h)";
            data[indice + 1][0] = "EQUIPO";
            data[indice + 1][3] = "ENVIADO";
            data[indice + 1][5] = "PENDIENTE";
            data[indice + 1][7] = "PLAZO (h)";

            agrupaciones.Add(indice);
            subTitulos.Add(indice + 1);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 1, colspan = 3 });
            merge.Add(new MergeModel { row = indice + 1, col = 3, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 5, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 7, rowspan = 1, colspan = 3 });

            indice = indice + 2;
            foreach (SiCambioTurnoSubseccionDTO item in list45)
            {
                data[indice][0] = item.Infequipo;
                if (item.Infestado == "E") data[indice][3] = "X";
                if (item.Infestado == "P") data[indice][5] = "X";
                data[indice][7] = item.Infplazo;

                merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 3 });
                merge.Add(new MergeModel { row = indice, col = 3, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 5, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 7, rowspan = 1, colspan = 3 });

                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 0, Longitud = 60 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 3, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 5, Longitud = 1 });
                longitudes.Add(new CeldaValidacionLongitud { Row = indice, Column = 7, Longitud = 10 });

                centros.Add(new AlineacionCelda { Row = indice, Column = 3 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 5 });
                centros.Add(new AlineacionCelda { Row = indice, Column = 7 });

                indice++;
            }


            //*Modificaciones SCO

            data[indice][0] = "5.  VISUALIZACIÓN DE SORTEO DE PRUEBAS ALEATORIAS";
            data[indice + 1][0] = "FECHA DE SORTEO";
            data[indice + 1][2] = "SORTEO";
            data[indice + 1][4] = "RESULTADO";
            data[indice + 1][6] = "GENERADOR";
            data[indice + 1][8] = "PRUEBA";

            titulos.Add(indice);
            subTitulos.Add(indice + 1);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 2, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 4, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 6, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 8, rowspan = 1, colspan = 2 });

            indice = indice + 2;
            foreach (SiCambioTurnoSubseccionDTO item in list51)
            {
                merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 2, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 4, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 6, rowspan = 1, colspan = 2 });
                merge.Add(new MergeModel { row = indice, col = 8, rowspan = 1, colspan = 2 });

                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 2, Elementos = listSorteo });
                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 4, Elementos = listResultadoSorteo });
                validaciones.Add(new ValidacionListaCelda { Row = indice, Column = 8, Elementos = listPruebaSorteo });


                data[indice][0] = item.Pafecha;
                data[indice][2] = item.Pasorteo;
                data[indice][4] = item.Paresultado;
                data[indice][6] = item.Pagenerador;
                data[indice][8] = item.Paprueba;

                indice++;
            }

            //*Fin Modificaciones SCO

            data[indice][0] = "Otros";
            data[indice + 1][0] = "CASOS SIN RESERVA";
            data[indice + 1][2] = "EMS OPERATIVO";
            data[indice + 1][8] = "HORA DE ENTREGA DE TURNO";

            data[indice + 2][0] = "Se entrega todos los casos SIN RESERVA";
            data[indice + 2][2] = "SI";
            data[indice + 2][3] = "NO";
            data[indice + 2][4] = "OBSERVACIONES";

            //Completar con los datos
            data[indice + 3][0] = (datos.CasoSinReserva == "SI") ? datos.CasoSinReserva : "NO";
            data[indice + 3][2] = (datos.Emsoperativo == Constantes.SI) ? "X" : "";
            data[indice + 3][3] = (datos.Emsoperativo == Constantes.NO) ? "X" : "";
            data[indice + 3][4] = datos.Emsobservaciones;
            data[indice + 3][8] = (indicador == 0) ? DateTime.Now.ToString("HH:mm:ss") : datos.Horaentregaturno;

            longitudes.Add(new CeldaValidacionLongitud { Row = indice + 3, Column = 0, Longitud = 2 });
            longitudes.Add(new CeldaValidacionLongitud { Row = indice + 3, Column = 2, Longitud = 1 });
            longitudes.Add(new CeldaValidacionLongitud { Row = indice + 3, Column = 3, Longitud = 1 });
            longitudes.Add(new CeldaValidacionLongitud { Row = indice + 3, Column = 4, Longitud = 300 });
            longitudes.Add(new CeldaValidacionLongitud { Row = indice + 3, Column = 8, Longitud = 20 });

            validaciones.Add(new ValidacionListaCelda { Row = indice + 3, Column = 0, Elementos = listSiNo });
            validaciones.Add(new ValidacionListaCelda { Row = indice + 3, Column = 2, Elementos = listCondicion });
            validaciones.Add(new ValidacionListaCelda { Row = indice + 3, Column = 3, Elementos = listCondicion });

            centros.Add(new AlineacionCelda { Row = indice + 3, Column = 0 });
            centros.Add(new AlineacionCelda { Row = indice + 3, Column = 2 });
            centros.Add(new AlineacionCelda { Row = indice + 3, Column = 3 });
            centros.Add(new AlineacionCelda { Row = indice + 3, Column = 8 });


            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 2, rowspan = 1, colspan = 6 });
            merge.Add(new MergeModel { row = indice + 1, col = 8, rowspan = 2, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 2, col = 0, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 2, col = 4, rowspan = 1, colspan = 4 });
            merge.Add(new MergeModel { row = indice + 2, col = 8, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 3, col = 0, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 3, col = 4, rowspan = 1, colspan = 4 });
            merge.Add(new MergeModel { row = indice + 3, col = 8, rowspan = 1, colspan = 2 });

            agrupaciones.Add(indice);
            subTitulos.Add(indice + 1);
            subTitulos.Add(indice + 2);
            adicionales.Add(indice + 3);

            indice = indice + 4;

            //Completar con los datos
            data[indice][0] = "";

            data[indice + 1][0] = datos.Coordinadorrecibe;
            data[indice + 1][2] = datos.Especialistarecibe;
            data[indice + 1][6] = datos.Analistarecibe;

            longitudes.Add(new CeldaValidacionLongitud { Row = indice + 1, Column = 0, Longitud = 50 });
            longitudes.Add(new CeldaValidacionLongitud { Row = indice + 1, Column = 2, Longitud = 50 });
            longitudes.Add(new CeldaValidacionLongitud { Row = indice + 1, Column = 6, Longitud = 50 });

            validaciones.Add(new ValidacionListaCelda { Row = indice + 1, Column = 0, Elementos = listPersonal });
            validaciones.Add(new ValidacionListaCelda { Row = indice + 1, Column = 2, Elementos = listPersonal });
            validaciones.Add(new ValidacionListaCelda { Row = indice + 1, Column = 6, Elementos = listPersonal });

            centros.Add(new AlineacionCelda { Row = indice + 1, Column = 0 });
            centros.Add(new AlineacionCelda { Row = indice + 1, Column = 2 });
            centros.Add(new AlineacionCelda { Row = indice + 1, Column = 6 });


            data[indice + 2][0] = "COORDINADOR QUE RECIBE EL TURNO";
            data[indice + 2][2] = "ESPECIALISTA QUE RECIBE EL TURNO";
            data[indice + 2][6] = "ANALISTA QUE RECIBE EL TURNO";

            adicionales.Add(indice);
            adicionales.Add(indice + 1);
            adicionales.Add(indice + 2);

            finales.Add(indice + 2);

            merge.Add(new MergeModel { row = indice, col = 0, rowspan = 1, colspan = 10 });
            merge.Add(new MergeModel { row = indice + 1, col = 0, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 1, col = 2, rowspan = 1, colspan = 4 });
            merge.Add(new MergeModel { row = indice + 1, col = 6, rowspan = 1, colspan = 4 });
            merge.Add(new MergeModel { row = indice + 2, col = 0, rowspan = 1, colspan = 2 });
            merge.Add(new MergeModel { row = indice + 2, col = 2, rowspan = 1, colspan = 4 });
            merge.Add(new MergeModel { row = indice + 2, col = 6, rowspan = 1, colspan = 4 });


            #endregion

            model.Datos = data;
            model.Merge = merge.ToArray();
            model.IndicesTitulo = titulos.ToArray();
            model.IndicesSubtitulo = subTitulos.ToArray();
            model.IndicesAgrupacion = agrupaciones.ToArray();
            model.IndicesComentario = comentarios.ToArray();
            model.IndicesAdicional = adicionales.ToArray();
            model.IndicesFinal = finales.ToArray();
            model.IndiceMantenimiento = mantenimientos.ToArray();
            model.IdPersona = datos.Coordinadorresp;
            model.Validaciones = validaciones;
            model.Longitudes = longitudes;
            model.Indicador = indicador;
            model.Centros = centros;
            model.Derechos = derechos;
            model.Fecha = fecha;
            model.IndicesReprogramas = reprogramas.ToArray();
            model.Reprog = reprog;
            model.NumReprogramas = list12.Count;

            return Json(model);
        }

        /// <summary>
        /// Permite pintar la data por defecto
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string[][] PintarCeldas(string[][] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new string[10];

                for (int j = 0; j < 10; j++)
                {
                    data[i][j] = string.Empty;
                }
            }

            return data;
        }
        
        /// <summary>
        /// Permite grabar la data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="titulos"></param>
        /// <param name="subtitulos"></param>
        /// <param name="agrupaciones"></param>
        /// <param name="comentarios"></param>
        /// <param name="adicionales"></param>
        /// <param name="finales"></param>
        /// <param name="fecha"></param>
        /// <param name="turno"></param>
        /// <param name="responsable"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(string[][] data, string dataAdicional, int[] subtitulos, int[] agrupaciones, int[] comentarios,
            int[] adicionales, string fecha, int turno, int responsable)
        {
            try
            {
                //Actualiza la data (tabla handson) con dato que si TIENE o NO archivo Atr
                if (dataAdicional != null && dataAdicional != "")
                {
                    var lstReg = dataAdicional.Split(',');

                    foreach (var registro in lstReg)
                    {
                        var objeto = registro.Split('/');

                        AnadirNuevaInfoTabla(data, objeto);
                    }
                }

                SiCambioTurnoDTO entity = this.ObtenerDatos(data, subtitulos.ToList(), comentarios.ToList(),
                    agrupaciones.ToList(), adicionales.ToList());

                entity.Turno = turno;
                entity.Coordinadorresp = responsable;
                entity.Fecturno = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                this.servicio.SaveSiCambioTurno(entity, User.Identity.Name);


                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Agrega informacion al handsontable
        /// </summary>
        /// <param name="data"></param>
        /// <param name="objeto"></param>
        private void AnadirNuevaInfoTabla(string[][] data, string[] objeto)
        {
            var subseccioncodi = objeto[0];
            var tipo = objeto[1];
            var tieneArchivo = objeto[2];

            var tamData = data.Length;

            for (int i = 0; i < tamData; i++)
            {
                var regSubseccioncodi = data[i][8];
                var regTipo = data[i][9];
                if (regSubseccioncodi == subseccioncodi && regTipo == tipo)
                {
                    data[i][4] = tieneArchivo;
                    break;
                }
            }

        }
        /// <summary>
        /// Permite obtener los datos para ser grabados
        /// </summary>
        /// <returns></returns>
        private SiCambioTurnoDTO ObtenerDatos(string[][] estructura, List<int> subTitulos,
           List<int> comentarios, List<int> agrupaciones, List<int> adicionales)
        {
            SiCambioTurnoDTO result = new SiCambioTurnoDTO();

            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            #region Recopilacion de datos

            //seccion 11
            for (int i = subTitulos[1] + 1; i < subTitulos[2]; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Despcentromarginal = estructura[i][0];
                entity.Despursautomatica = estructura[i][1];
                entity.Despmagautomatica = (!string.IsNullOrEmpty(estructura[i][3])) ?
                    (decimal?)decimal.Parse(estructura[i][3]) : null;
                entity.Despursmanual = estructura[i][4];
                entity.Despmagmanual = (!string.IsNullOrEmpty(estructura[i][6])) ?
                    (decimal?)decimal.Parse(estructura[i][6]) : null;
                entity.Despcentralaislado = estructura[i][7];
                entity.Despmagaislado = (!string.IsNullOrEmpty(estructura[i][9])) ?
                    (decimal?)decimal.Parse(estructura[i][9]) : null;
                entity.Subseccionnumber = SubSeccionesCambio.Seccion11;

                entitys.Add(entity);
            }

            //seccion 12
            for (int i = subTitulos[2] + 1; i < comentarios[0]; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Despreprogramas = estructura[i][0];
                entity.Desphorareprog = estructura[i][1];
                entity.Despmotivorepro = estructura[i][2];
                entity.Desparchivoatr = estructura[i][4];
                entity.Desppremisasreprog = estructura[i][6];
                entity.Subseccioncodi = estructura[i][8] != "" ? (int.Parse(estructura[i][8])) : 0;
                entity.Desptiporeprog = estructura[i][9];
                entity.Subseccionnumber = SubSeccionesCambio.Seccion12;

                entitys.Add(entity);
            }

            //seccion 21
            for (int i = subTitulos[4] + 1; i < comentarios[1]; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Manequipo = estructura[i][0];
                if (!string.IsNullOrEmpty(estructura[i][1])) entity.Mantipo = "P";
                else if (!string.IsNullOrEmpty(estructura[i][2])) entity.Mantipo = "C";
                entity.Manhoraconex = estructura[i][3];
                entity.Manconsideraciones = estructura[i][4];
                entity.Subseccionnumber = SubSeccionesCambio.Seccion21;

                entitys.Add(entity);
            }

            //seccion 22
            for (int i = subTitulos[6] + 1; i < subTitulos[7] - 1; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Manequipo = estructura[i][0];
                if (!string.IsNullOrEmpty(estructura[i][1])) entity.Mantipo = "P";
                else if (!string.IsNullOrEmpty(estructura[i][2])) entity.Mantipo = "C";
                entity.Manhoraconex = estructura[i][3];
                entity.Manconsideraciones = estructura[i][4];
                entity.Subseccionnumber = SubSeccionesCambio.Seccion22;

                entitys.Add(entity);
            }

            //seccion 31
            for (int i = subTitulos[8] + 1; i < comentarios[2]; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Sumsubestacion = estructura[i][0];
                if (!string.IsNullOrEmpty(estructura[i][1])) entity.Summotivocorte = "D";
                else if (!string.IsNullOrEmpty(estructura[i][2])) entity.Summotivocorte = "F";
                else if (!string.IsNullOrEmpty(estructura[i][3])) entity.Summotivocorte = "T";
                else if (!string.IsNullOrEmpty(estructura[i][4])) entity.Summotivocorte = "M";
                entity.Sumhorainicio = estructura[i][5];
                entity.Sumreposicion = estructura[i][6];
                entity.Sumconsideraciones = estructura[i][7];
                entity.Subseccionnumber = SubSeccionesCambio.Seccion31;

                entitys.Add(entity);
            }

            //seccion 41
            for (int i = subTitulos[9] + 1; i < comentarios[3]; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Regopecentral = estructura[i][0];
                entity.Regcentralsubestacion = estructura[i][3];
                entity.Regcentralhorafin = estructura[i][7];
                entity.Subseccionnumber = SubSeccionesCambio.Seccion41;

                entitys.Add(entity);
            }

            //seccion 42
            for (int i = subTitulos[10] + 1; i < comentarios[4]; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Reglineas = estructura[i][0];
                entity.Reglineasubestacion = estructura[i][3];
                entity.Reglineahorafin = estructura[i][7];
                entity.Subseccionnumber = SubSeccionesCambio.Seccion42;

                entitys.Add(entity);
            }

            //secccion 43
            for (int i = subTitulos[11] + 1; i < comentarios[5]; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Gesequipo = estructura[i][0];
                if (!string.IsNullOrEmpty(estructura[i][1])) entity.Gesaceptado = "A";
                else if (!string.IsNullOrEmpty(estructura[i][2])) entity.Gesaceptado = "R";
                entity.Gesdetalle = estructura[i][3];
                entity.Subseccionnumber = SubSeccionesCambio.Seccion43;

                entitys.Add(entity);
            }

            //seccion 44
            for (int i = subTitulos[12] + 1; i < comentarios[6]; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Eveequipo = estructura[i][0];
                entity.Evehorainicio = estructura[i][2];
                entity.Evereposicion = estructura[i][3];
                entity.Everesumen = estructura[i][4];
                entity.Subseccionnumber = SubSeccionesCambio.Seccion44;

                entitys.Add(entity);
            }

            //seccion 45
            for (int i = subTitulos[13] + 1; i < subTitulos[14] - 1; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Infequipo = estructura[i][0];
                if (!string.IsNullOrEmpty(estructura[i][3])) entity.Infestado = "E";
                else if (!string.IsNullOrEmpty(estructura[i][5])) entity.Infestado = "P";
                entity.Infplazo = estructura[i][7];
                entity.Subseccionnumber = SubSeccionesCambio.Seccion45;

                entitys.Add(entity);
            }


            //seccion 51
            for (int i = subTitulos[14] + 1; i < subTitulos[15] - 1; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                entity.Pafecha = estructura[i][0];
                entity.Pasorteo = estructura[i][2];
                entity.Paresultado = estructura[i][4];
                entity.Pagenerador = estructura[i][6];
                entity.Paprueba = estructura[i][8];

                entity.Subseccionnumber = SubSeccionesCambio.Seccion51;

                entitys.Add(entity);
            }

            if (estructura[adicionales[2]].Length > 0)
            {

                result.Coordinadorrecibe = estructura[adicionales[2]][0];
                result.Especialistarecibe = estructura[adicionales[2]][2];
                result.Analistarecibe = estructura[adicionales[2]][6];
            }

            int indice = subTitulos[16] + 1;

            result.CasoSinReserva = estructura[indice][0];
            if (!string.IsNullOrEmpty(estructura[indice][2])) result.Emsoperativo = Constantes.SI;
            else if (!string.IsNullOrEmpty(estructura[indice][3])) result.Emsoperativo = Constantes.NO;
            result.Emsobservaciones = estructura[indice][4];
            result.Horaentregaturno = estructura[indice][8];

            //llenamos las secciones del formato

            result.ListaSeccion = new List<SiCambioTurnoSeccionDTO>();

            SiCambioTurnoSeccionDTO seccion11 = new SiCambioTurnoSeccionDTO();
            seccion11.Nroseccion = SubSeccionesCambio.Seccion11;
            seccion11.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion11).ToList();
            result.ListaSeccion.Add(seccion11);

            SiCambioTurnoSeccionDTO seccion12 = new SiCambioTurnoSeccionDTO();
            seccion12.Nroseccion = SubSeccionesCambio.Seccion12;
            seccion12.Descomentario = estructura[comentarios[0]][0];
            seccion12.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion12).ToList();
            result.ListaSeccion.Add(seccion12);

            SiCambioTurnoSeccionDTO seccion21 = new SiCambioTurnoSeccionDTO();
            seccion21.Nroseccion = SubSeccionesCambio.Seccion21;
            seccion21.Descomentario = estructura[comentarios[1]][0];
            seccion21.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion21).ToList();
            result.ListaSeccion.Add(seccion21);


            SiCambioTurnoSeccionDTO seccion22 = new SiCambioTurnoSeccionDTO();
            seccion22.Nroseccion = SubSeccionesCambio.Seccion22;
            seccion22.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion22).ToList();
            result.ListaSeccion.Add(seccion22);

            SiCambioTurnoSeccionDTO seccion31 = new SiCambioTurnoSeccionDTO();
            seccion31.Nroseccion = SubSeccionesCambio.Seccion31;
            seccion31.Descomentario = estructura[comentarios[2]][0];
            seccion31.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion31).ToList();
            result.ListaSeccion.Add(seccion31);

            SiCambioTurnoSeccionDTO seccion41 = new SiCambioTurnoSeccionDTO();
            seccion41.Nroseccion = SubSeccionesCambio.Seccion41;
            seccion41.Descomentario = estructura[comentarios[3]][0];
            seccion41.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion41).ToList();
            result.ListaSeccion.Add(seccion41);

            SiCambioTurnoSeccionDTO seccion42 = new SiCambioTurnoSeccionDTO();
            seccion42.Nroseccion = SubSeccionesCambio.Seccion42;
            seccion42.Descomentario = estructura[comentarios[4]][0];
            seccion42.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion42).ToList();
            result.ListaSeccion.Add(seccion42);

            SiCambioTurnoSeccionDTO seccion43 = new SiCambioTurnoSeccionDTO();
            seccion43.Nroseccion = SubSeccionesCambio.Seccion43;
            seccion43.Descomentario = estructura[comentarios[5]][0];
            seccion43.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion43).ToList();
            result.ListaSeccion.Add(seccion43);

            SiCambioTurnoSeccionDTO seccion44 = new SiCambioTurnoSeccionDTO();
            seccion44.Nroseccion = SubSeccionesCambio.Seccion44;
            seccion44.Descomentario = estructura[comentarios[6]][0];
            seccion44.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion44).ToList();
            result.ListaSeccion.Add(seccion44);

            SiCambioTurnoSeccionDTO seccion45 = new SiCambioTurnoSeccionDTO();
            seccion45.Nroseccion = SubSeccionesCambio.Seccion45;
            seccion45.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion45).ToList();
            result.ListaSeccion.Add(seccion45);


            SiCambioTurnoSeccionDTO seccion51 = new SiCambioTurnoSeccionDTO();
            seccion51.Nroseccion = SubSeccionesCambio.Seccion51;
            seccion51.ListItems = entitys.Where(x => x.Subseccionnumber == SubSeccionesCambio.Seccion51).ToList();
            result.ListaSeccion.Add(seccion51);

            #endregion

            return result;
        }

        /// <summary>
        /// Permite generar el archivo
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="turno"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fecha, int turno, int formato)
        {
            CambioTurnoModel model = new CambioTurnoModel();
            try
            {
                string path = HttpContext.Server.MapPath("~/") + ConstantesCoordinacion.RutaReporte;
                string fileName = (formato == 1) ? ConstantesCoordinacion.FileCambioTurnoXLS : ConstantesCoordinacion.FileCambioTurnoDPF;
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                this.servicio.ExportarFormato(fechaProceso, turno, formato, path, fileName, User.Identity.Name);

                model.Indicador = 1;
            }
            catch (Exception ex)
            {
                model.Indicador = -1; //ocurrio algun error
                Log.Error(NameController, ex);
                model.StrMensaje = "Se produjo un error: " + ex.Message;
            }
            return Json(model);
        }

        /// <summary>
        /// Descarga el formato
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int formato)
        {
            string file = (formato == 1) ? ConstantesCoordinacion.FileCambioTurnoXLS : ConstantesCoordinacion.FileCambioTurnoDPF;
            string path = HttpContext.Server.MapPath("~/") + ConstantesCoordinacion.RutaReporte + file;
            string app = (formato == 1) ? Constantes.AppExcel : Constantes.AppPdf;

            return File(path, app, file);
        }

        /// <summary>
        /// Permite manejar la auditoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Auditoria(int id)
        {
            CambioTurnoModel model = new CambioTurnoModel();
            model.ListaAuditoria = this.servicio.ObtenerAuditoria(id);
            return PartialView(model);
        }

        /// <summary>
        /// Funcion que coloca valor a la celda ATR PUBLICADO
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="reprograma"></param>
        /// <param name="baseDirectory"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsonResult CompletarCeldaATR(string fechaConsulta, string agrupados)
        {
            int resultado = 0;

            CambioTurnoModel model = new CambioTurnoModel();

            try
            {
                if (agrupados != null && agrupados != "")
                {

                    base.ValidarSesionJsonResult();
                    if (!base.VerificarAccesoAccion(Acciones.Editar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);


                    resultado = servicio.VerificarExistenciaArchivosATR(fechaConsulta, agrupados, out List<string> lstEnvio); //0: faltan atr / 1: todos tienen atr / -1: error

                    model.DataEnvio = string.Join(",", lstEnvio);
                    model.Indicador = resultado;
                }
            }
            catch (Exception ex)
            {
                model.Indicador = -1; //ocurrio algun error
                Log.Error(NameController, ex);
                model.StrMensaje = "Se produjo un error: " + ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Verifica la existencia de archivos ATR, si hay almenos uno sin archivo lanza un popup
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="agrupados"></param>
        /// <returns> -1: ocurrio un error </returns>
        /// <returns> 0: almenos hay uno que no tiene su archivo atr </returns>
        /// <returns> 1: todos tienen sus archivos atr </returns>
        public JsonResult VerificarSoloExistenciaDeArchivosATR(bool esNuevoHT, int numReprogramas, string fechaConsulta, string agrupados)
        {
            int resultado = 0;

            CambioTurnoModel model = new CambioTurnoModel();

            try
            {
                if (numReprogramas > 0)
                {
                    if (esNuevoHT)
                    {
                        if (agrupados != null && agrupados != "")
                        {
                            base.ValidarSesionJsonResult();
                            if (!base.VerificarAccesoAccion(Acciones.Editar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                            resultado = servicio.VerificarSoloExistenciaArchivos(fechaConsulta, agrupados);

                            model.Indicador = resultado;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (agrupados != null && agrupados != "")
                        {
                            base.ValidarSesionJsonResult();
                            if (!base.VerificarAccesoAccion(Acciones.Editar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                            resultado = servicio.VerificarSoloExistenciaArchivos(fechaConsulta, agrupados);

                            model.Indicador = resultado;
                        }
                        else
                        {
                            model.Indicador = 3;
                        }
                    }

                }
                else
                {
                    model.Indicador = 2;
                }

            }
            catch (Exception ex)
            {
                model.Indicador = -1; //ocurrio algun error
                Log.Error(NameController, ex);
                model.StrMensaje = "Se produjo un error: " + ex.Message;
            }

            return Json(model);
        }
    }
}
