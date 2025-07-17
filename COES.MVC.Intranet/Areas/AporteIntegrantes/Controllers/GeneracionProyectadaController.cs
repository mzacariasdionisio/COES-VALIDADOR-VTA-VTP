using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.Servicios.Aplicacion.CalculoPorcentajes.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.SistemasTransmision.Helper;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class GeneracionProyectadaController : BaseController
    {
        //
        // GET: /AporteIntegrantes/GeneracionProyectada/

        public GeneracionProyectadaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        CalculoPorcentajesAppServicio servicioCAI = new CalculoPorcentajesAppServicio();

        public ActionResult Index(int caiprscodi = 0, int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            Log.Info("Lista de Presupuestos - ListCaiPresupuestos");
            model.ListaPresupuesto = this.servicioCAI.ListCaiPresupuestos();
            if (model.ListaPresupuesto.Count > 0 && caiprscodi == 0)
            {
                caiprscodi = model.ListaPresupuesto[0].Caiprscodi;

            }
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCAI.GetByIdCaiPresupuesto(caiprscodi);
            Log.Info("Lista de Versiones de ajuste - ListCaiAjustes");
            model.ListaAjuste = this.servicioCAI.ListCaiAjustes(caiprscodi); //Ordenado en descendente
            if (model.ListaAjuste.Count > 0 && caiajcodi == 0)
            {
                caiajcodi = (int)model.ListaAjuste[0].Caiajcodi;
            }

            if (caiprscodi > 0 && caiajcodi > 0)
            {
                Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
                model.EntidadAjuste = this.servicioCAI.GetByIdCaiAjuste(caiajcodi);
            }
            else
            {
                model.EntidadAjuste = new CaiAjusteDTO();
            }
            model.Caiprscodi = caiprscodi;
            model.Caiajcodi = caiajcodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public JsonResult ProcesarArchivoDuracion(int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
            string path = FileServer.GetDirectory() + ConstantesCalculoPorcentajes.SDDP_DURACION;
            try
            {
                //Elimina información de la tabla CAI_SDDP_DURACION
                Log.Info("Elimina registro - DeleteCaiSddpDuracion");
                this.servicioCAI.DeleteCaiSddpDuracion();

                ////Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                Log.Info("Genera dataset - GeneraDataset");
                ds = this.servicioCAI.GeneraDataset(path, 1);
                int iFila = 0;
                iFila = 0;
                int i = 0;

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 1)
                    {
                        continue;
                    }
                    //INSERTAR EL REGISTRO
                    model.EntidadDuracion = new CaiSddpDuracionDTO();

                    model.EntidadDuracion.Sddpducodi = 0;
                    model.EntidadDuracion.Caiajcodi = caiajcodi;
                    model.EntidadDuracion.Sddpduusucreacion = User.Identity.Name;
                    model.EntidadDuracion.Sddpdufeccreacion = DateTime.Now;
                    if (dtRow[i].ToString() == "null")
                    {
                        break;
                    }
                    model.EntidadDuracion.Sddpduetapa = Convert.ToInt32(dtRow[i].ToString());
                    model.EntidadDuracion.Sddpduserie = Convert.ToInt32(dtRow[i + 1].ToString());
                    model.EntidadDuracion.Sddpdubloque = Convert.ToInt32(dtRow[i + 2].ToString());
                    model.EntidadDuracion.Sddpduduracion = Convert.ToDecimal(dtRow[i + 3].ToString());
                    Log.Info("Inserta registro - SaveCaiSddpDuracion");
                    this.servicioCAI.SaveCaiSddpDuracion(model.EntidadDuracion);

                }
                model.sMensaje = "Felicidades, la carga de información de Duracion fue exitosa para los " + (iFila) + " registros, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        [HttpPost]
        public JsonResult ProcesarArchivoRenovables(int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
           
            string path = FileServer.GetDirectory() + ConstantesCalculoPorcentajes.SDDP_RENOVABLES;
            string tipo = "GND";
            try
            {
                //Elimina información de la tabla CAI_SDDP_DURACION
                Log.Info("Elimina registro - DeleteCaiSddpGenmarg");
                this.servicioCAI.DeleteCaiSddpGenmarg(tipo);

                ////Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                Log.Info("Genera dataset - GeneraDataset");
                ds = this.servicioCAI.GeneraDataset(path, 1);
                Log.Info("Obterner GenmargCODI - GetCodigoGeneradoCaiSddpGen");
                int iCaiSddpGenmargCODI = new CalculoPorcentajesAppServicio().GetCodigoGeneradoCaiSddpGen();

                //Lista detalle de Cabecera
                string[] listaCabecera = new string[(ds.Tables[0].Columns.Count - 3)];//le quitamos las 3 primeras columnas
                int iFila = 0;
                int iNroCabeceras = 0;
                int ix = 0;

                foreach (DataRow dtoRow in ds.Tables[0].Rows)
                {
                    for (int i = 3; i < dtoRow.ItemArray.Count(); i++)
                    {
                        listaCabecera[iNroCabeceras] = dtoRow[i].ToString();
                        iNroCabeceras++;
                    }
                    break;

                }
                //Lista Bulk
                List<CaiSddpGenmargDTO> entitys = new List<CaiSddpGenmargDTO>();
                int iAux = 0;
                iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 2)
                    {
                        continue;
                    }
                    int iColumna = 3; //donde empiece la data
                    //INSERTAR EL REGISTRO
                    for (int i = 0; i < iNroCabeceras; i++)
                   {
                        model.EntidadGen = new CaiSddpGenmargDTO();
                        model.EntidadGen.Sddpgmcodi = iCaiSddpGenmargCODI++;
                        model.EntidadGen.Caiajcodi = caiajcodi;
                        model.EntidadGen.Sddpgmfecha = DateTime.Now;
                        model.EntidadGen.Sddpgmusucreacion = User.Identity.Name;
                        model.EntidadGen.Sddpgmfeccreacion = DateTime.Now;
                        model.EntidadGen.Sddpgmtipo = tipo;
                        model.EntidadGen.Sddpgmnombre = listaCabecera[i].ToString();
                        model.EntidadGen.Sddpgmetapa = Convert.ToInt32(dtRow[ix].ToString());
                        model.EntidadGen.Sddpgmserie = Convert.ToInt32(dtRow[ix + 1].ToString());
                        model.EntidadGen.Sddpgmbloque = Convert.ToInt32(dtRow[ix + 2].ToString());
                        string sValor = dtRow[iColumna].ToString();
                        decimal dVariable = Decimal.Parse(sValor, System.Globalization.NumberStyles.Float);
                        model.EntidadGen.Sddpgmenergia = dVariable;
                        entitys.Add(model.EntidadGen);
                        iColumna++;
                        iAux++;
                        if (iAux == 3000)
                        {
                            Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                            new CalculoPorcentajesAppServicio().BulkInsertValorSddpGenmarg(entitys);
                            entitys = new List<CaiSddpGenmargDTO>();
                            iAux = 0;
                        }
                    }

                }
                if (iAux > 0)
                {
                    //quedo registros no ingresados a la bd
                    Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                    new CalculoPorcentajesAppServicio().BulkInsertValorSddpGenmarg(entitys);
                }
                model.sMensaje = "Felicidades, la carga de información de Renovables fue exitosa para los " + (iFila) + " registros, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        [HttpPost]
        public JsonResult ProcesarArchivoTermicas(int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
           
            string path = FileServer.GetDirectory() + ConstantesCalculoPorcentajes.SDDP_TERMICAS;
            string tipo = "TER";
            try
            {
                //Elimina información de la tabla CAI_SDDP_DURACION
                Log.Info("Elimina registro - DeleteCaiSddpGenmarg");
                this.servicioCAI.DeleteCaiSddpGenmarg(tipo);

                ////Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                Log.Info("Genera dataset - GeneraDataset");
                ds = this.servicioCAI.GeneraDataset(path, 1);
                Log.Info("Obterner GenmargCODI - Cai_Sddp_Genmarg");
                int iCaiSddpGenmargCODI = new CalculoPorcentajesAppServicio().GetCodigoGeneradoCaiSddpGen();

                //Lista detalle de Cabecera
                string[] listaCabecera = new string[(ds.Tables[0].Columns.Count - 3)];//le quitamos las 3 primeras columnas
                int iFila = 0;
                int iNroCabeceras = 0;
                int ix = 0;

                foreach (DataRow dtoRow in ds.Tables[0].Rows)
                {
                    for (int i = 3; i < dtoRow.ItemArray.Count(); i++)
                    {
                        listaCabecera[iNroCabeceras] = dtoRow[i].ToString();
                        iNroCabeceras++;
                    }
                    break;

                }
                //Lista Bulk
                List<CaiSddpGenmargDTO> entitys = new List<CaiSddpGenmargDTO>();
                int iAux = 0;                
                iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 2)
                    {
                        continue;
                    }
                    int iColumna = 3; //donde empiece la data
                    //INSERTAR EL REGISTRO
                    for (int i = 0; i < iNroCabeceras; i++)
                    {

                        model.EntidadGen = new CaiSddpGenmargDTO();
                        model.EntidadGen.Sddpgmcodi = iCaiSddpGenmargCODI++;
                        model.EntidadGen.Caiajcodi = caiajcodi;
                        model.EntidadGen.Sddpgmfecha = DateTime.Now;
                        model.EntidadGen.Sddpgmusucreacion = User.Identity.Name;
                        model.EntidadGen.Sddpgmfeccreacion = DateTime.Now;
                        model.EntidadGen.Sddpgmtipo = tipo;
                        model.EntidadGen.Sddpgmnombre = listaCabecera[i].ToString();
                        if (dtRow[ix].ToString() == "null")
                        {
                            break;
                        }
                        model.EntidadGen.Sddpgmetapa = Convert.ToInt32(dtRow[ix].ToString());
                        model.EntidadGen.Sddpgmserie = Convert.ToInt32(dtRow[ix + 1].ToString());
                        model.EntidadGen.Sddpgmbloque = Convert.ToInt32(dtRow[ix + 2].ToString());
                        string sValor = dtRow[iColumna].ToString();
                        decimal dVariable = Decimal.Parse(sValor, System.Globalization.NumberStyles.Float);
                        model.EntidadGen.Sddpgmenergia = dVariable;
                        entitys.Add(model.EntidadGen);
                        
                        iColumna++;
                        iAux++;
                        if (iAux == 3000)
                        {
                            Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                            new CalculoPorcentajesAppServicio().BulkInsertValorSddpGenmarg(entitys);
                            entitys = new List<CaiSddpGenmargDTO>();
                            iAux = 0;
                        }
                    }

                }
                if (iAux > 0)
                {
                    //quedo registros no ingresados a la bd
                    Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                    new CalculoPorcentajesAppServicio().BulkInsertValorSddpGenmarg(entitys);
                }
                model.sMensaje = "Felicidades, la carga de información de Termicas fue exitosa para los " + (iFila) + " registros, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        [HttpPost]
        public JsonResult ProcesarArchivoHidraulicas(int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
            string path = FileServer.GetDirectory() + ConstantesCalculoPorcentajes.SDDP_HIDRAULICAS;
            string tipo = "HID";
            try
            {
                //Elimina información de la tabla CAI_SDDP_DURACION
                Log.Info("Elimina datos CaiSddpGenmarg - DeleteCaiSddpGenmarg");
                this.servicioCAI.DeleteCaiSddpGenmarg(tipo);

                ////Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                Log.Info("Genera dataset - GeneraDataset");
                ds = this.servicioCAI.GeneraDataset(path, 1);
                Log.Info("Obterner GenmargCODI - Cai_Sddp_Genmarg");
                int iCaiSddpGenmargCODI = new CalculoPorcentajesAppServicio().GetCodigoGeneradoCaiSddpGen();

                //Lista detalle de Cabecera
                string[] listaCabecera = new string[(ds.Tables[0].Columns.Count - 3)];//le quitamos las 3 primeras columnas
                int iFila = 0;
                int iNroCabeceras = 0;
                int ix = 0;

                foreach (DataRow dtoRow in ds.Tables[0].Rows)
                {
                    for (int i = 3; i < dtoRow.ItemArray.Count(); i++)
                    {
                        listaCabecera[iNroCabeceras] = dtoRow[i].ToString();
                        iNroCabeceras++;
                    }
                    break;

                }
                //Lista Bulk
                List<CaiSddpGenmargDTO> entitys = new List<CaiSddpGenmargDTO>();
                int iAux = 0;
                iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 2)
                    {
                        continue;
                    }
                    int iColumna = 3; //donde empiece la data
                    //INSERTAR EL REGISTRO
                    for (int i = 0; i < iNroCabeceras; i++)
                    {

                        model.EntidadGen = new CaiSddpGenmargDTO();
                        model.EntidadGen.Sddpgmcodi = iCaiSddpGenmargCODI++;
                        model.EntidadGen.Caiajcodi = caiajcodi;
                        model.EntidadGen.Sddpgmfecha = DateTime.Now;
                        model.EntidadGen.Sddpgmusucreacion = User.Identity.Name;
                        model.EntidadGen.Sddpgmfeccreacion = DateTime.Now;
                        model.EntidadGen.Sddpgmtipo = tipo;
                        model.EntidadGen.Sddpgmnombre = listaCabecera[i].ToString();
                        model.EntidadGen.Sddpgmetapa = Convert.ToInt32(dtRow[ix].ToString());
                        model.EntidadGen.Sddpgmserie = Convert.ToInt32(dtRow[ix + 1].ToString());
                        model.EntidadGen.Sddpgmbloque = Convert.ToInt32(dtRow[ix + 2].ToString());
                        string sValor = dtRow[iColumna].ToString();
                        decimal dVariable = Decimal.Parse(sValor, System.Globalization.NumberStyles.Float);
                        model.EntidadGen.Sddpgmenergia = dVariable;
                        entitys.Add(model.EntidadGen);                        
                        iColumna++;
                        iAux++;
                        if (iAux == 3000)
                        {
                            Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                            new CalculoPorcentajesAppServicio().BulkInsertValorSddpGenmarg(entitys);
                            entitys = new List<CaiSddpGenmargDTO>();
                            iAux = 0;
                        }
                    }

                }
                if (iAux > 0)
                {
                    //quedo registros no ingresados a la bd
                    Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                    new CalculoPorcentajesAppServicio().BulkInsertValorSddpGenmarg(entitys);
                }
                model.sMensaje = "Felicidades, la carga de información de Hidraulicas fue exitosa para los " + (iFila) + " registros, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        [HttpPost]
        public JsonResult ProcesarArchivoCostoMarginal(int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
            string path = FileServer.GetDirectory() + ConstantesCalculoPorcentajes.SDDP_COSTOMARGINAL;
            string tipo = "CMG";
            try
            {
                //Elimina información de la tabla CAI_SDDP_DURACION
                Log.Info("Elimina datos CaiSddpGenmarg - DeleteCaiSddpGenmarg");
                this.servicioCAI.DeleteCaiSddpGenmarg(tipo);

                ////Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                ds = this.servicioCAI.GeneraDataset(path, 1);
                Log.Info("Obterner GenmargCODI - Cai_Sddp_Genmarg");
                int iCaiSddpGenmargCODI = new CalculoPorcentajesAppServicio().GetCodigoGeneradoCaiSddpGen();

                //Lista detalle de Cabecera
                string[] listaCabecera = new string[(ds.Tables[0].Columns.Count - 3)];//le quitamos las 3 primeras columnas
                int iFila = 0;
                int iNroCabeceras = 0;
                int ix = 0;

                foreach (DataRow dtoRow in ds.Tables[0].Rows)
                {
                    for (int i = 3; i < dtoRow.ItemArray.Count(); i++)
                    {
                        listaCabecera[iNroCabeceras] = dtoRow[i].ToString();
                        iNroCabeceras++;
                    }
                    break;

                }
                //Lista Bulk
                List<CaiSddpGenmargDTO> entitys = new List<CaiSddpGenmargDTO>();
                int iAux = 0;
                iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 2)
                    {
                        continue;
                    }
                    int iColumna = 3; //donde empiece la data
                    //INSERTAR EL REGISTRO
                    for (int i = 0; i < iNroCabeceras; i++)
                    {

                        model.EntidadGen = new CaiSddpGenmargDTO();
                        model.EntidadGen.Sddpgmcodi = iCaiSddpGenmargCODI++;
                        model.EntidadGen.Caiajcodi = caiajcodi;
                        model.EntidadGen.Sddpgmfecha = DateTime.Now;
                        model.EntidadGen.Sddpgmusucreacion = User.Identity.Name;
                        model.EntidadGen.Sddpgmfeccreacion = DateTime.Now;
                        model.EntidadGen.Sddpgmtipo = tipo;
                        model.EntidadGen.Sddpgmnombre = listaCabecera[i].ToString();
                        model.EntidadGen.Sddpgmetapa = Convert.ToInt32(dtRow[ix].ToString());
                        model.EntidadGen.Sddpgmserie = Convert.ToInt32(dtRow[ix + 1].ToString());
                        model.EntidadGen.Sddpgmbloque = Convert.ToInt32(dtRow[ix + 2].ToString());
                        string sValor = dtRow[iColumna].ToString();
                        decimal dVariable = Decimal.Parse(sValor, System.Globalization.NumberStyles.Float);
                        model.EntidadGen.Sddpgmenergia = dVariable;
                        entitys.Add(model.EntidadGen);
                        iColumna++;
                        iAux++;
                        if (iAux == 3000)
                        {
                            Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                            new CalculoPorcentajesAppServicio().BulkInsertValorSddpGenmarg(entitys);
                            entitys = new List<CaiSddpGenmargDTO>();
                            iAux = 0;
                        }
                    }

                }
                if (iAux > 0)
                {
                    //quedo registros no ingresados a la bd
                    Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                    new CalculoPorcentajesAppServicio().BulkInsertValorSddpGenmarg(entitys);
                }
                model.sMensaje = "Felicidades, la carga de información de Costo Marginal fue exitosa para los " + (iFila) + " registros, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        [HttpPost]
        public JsonResult ProcesarArchivoParametros(int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
            string path = FileServer.GetDirectory() + ConstantesCalculoPorcentajes.SDDP_PARAMETROS;
            try
            {
                //Eliminar informacion de las tablas CAI_SDDP_PARAMSEM, CAI_SDDP_PARAMDIA, CAI_SDDP_PARAMINT y CAI_SDDP_PARAMETRO
                Log.Info("Elimina registro - DeleteCaiSddpParamsem");
                this.servicioCAI.DeleteCaiSddpParamsem();
                Log.Info("Elimina registro - DeleteCaiSddpParamdia");
                this.servicioCAI.DeleteCaiSddpParamdia();
                Log.Info("Elimina registro - DeleteCaiSddpParamint");
                this.servicioCAI.DeleteCaiSddpParamint();
                Log.Info("Elimina registro - DeleteCaiSddpParametro");
                this.servicioCAI.DeleteCaiSddpParametro();

                ////Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                Log.Info("Genera dataset - GeneraDataset");
                ds = this.servicioCAI.GeneraDataset(path, 1);
                int iFila = 0;
                int i = 0;                
                //insertamos numero de semanas
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 2)
                    {
                        continue;
                    }

                    if (dtRow[i] == "null")
                    {
                        break;
                    }
                    else{
                        //INSERTAR EL REGISTRO
                        model.EntidadSddpSemana = new CaiSddpParamsemDTO();

                        model.EntidadSddpSemana.Sddppscodi = 0;
                        model.EntidadSddpSemana.Caiajcodi = caiajcodi;
                        model.EntidadSddpSemana.Sddppsusucreacion = User.Identity.Name;
                        model.EntidadSddpSemana.Sddppsfeccreacion = DateTime.Now;
                        model.EntidadSddpSemana.Sddppsnumsem = Convert.ToInt32(dtRow[i].ToString());
                        model.EntidadSddpSemana.Sddppsdiaini = Convert.ToDateTime(dtRow[i + 1].ToString());
                        model.EntidadSddpSemana.Sddppsdiafin = Convert.ToDateTime(dtRow[i + 2].ToString());
                        Log.Info("Inserta Entidad SddpSemana - SaveCaiSddpParamsem");
                        this.servicioCAI.SaveCaiSddpParamsem(model.EntidadSddpSemana);
                    }
                                       

                }

                //insertamos dias 
                iFila = 0;
                i = 5;                
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 2)
                    {
                        continue;
                    }

                    if (dtRow[i] == "null")
                    {
                        break;
                    }
                    else
                    {
                        //INSERTAR EL REGISTRO
                        model.EntidadSddpDia = new CaiSddpParamdiaDTO();

                        model.EntidadSddpDia.Sddppdcodi = 0;
                        model.EntidadSddpDia.Caiajcodi = caiajcodi;
                        model.EntidadSddpDia.Sddppdusucreacion = User.Identity.Name;
                        model.EntidadSddpDia.Sddppdfeccreacion = DateTime.Now;
                        model.EntidadSddpDia.Sddppddia = Convert.ToDateTime(dtRow[i].ToString());
                        model.EntidadSddpDia.Sddppdlaboral = Convert.ToInt32(dtRow[i + 1].ToString());
                        Log.Info("Inserta Entidad Sddpdia - SaveCaiSddpParamdia");
                        this.servicioCAI.SaveCaiSddpParamdia(model.EntidadSddpDia);
                    }
                    

                }

                //insertamos bloques 
                iFila = 0;
                i = 9;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 2)
                    {
                        continue;
                    }
                    if (dtRow[i] == "null")
                    {
                        break;
                    }
                    else
                    {
                        //INSERTAR EL REGISTRO
                        model.EntidadSddpIntervalo = new CaiSddpParamintDTO();

                        model.EntidadSddpIntervalo.Sddppicodi = 0;
                        model.EntidadSddpIntervalo.Caiajcodi = caiajcodi;
                        model.EntidadSddpIntervalo.Sddppiusucreacion = User.Identity.Name;
                        model.EntidadSddpIntervalo.Sddppifeccreacion = DateTime.Now;
                        model.EntidadSddpIntervalo.Sddppiintervalo = Convert.ToDateTime(dtRow[i].ToString());
                        model.EntidadSddpIntervalo.Sddppilaboral = 1;
                        model.EntidadSddpIntervalo.Sddppibloque = Convert.ToInt32(dtRow[i + 1].ToString());
                        Log.Info("Inserta Entidad Sddpint - SaveCaiSddpParamint");
                        this.servicioCAI.SaveCaiSddpParamint(model.EntidadSddpIntervalo);
                    }
                    

                }

                //insertamos bloques 
                iFila = 0;
                i = 9;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 2)
                    {
                        continue;
                    }
                    if (dtRow[i] == "null")
                    {
                        break;
                    }
                    else
                    {
                        //INSERTAR EL REGISTRO
                        model.EntidadSddpIntervalo = new CaiSddpParamintDTO();

                        model.EntidadSddpIntervalo.Sddppicodi = 0;
                        model.EntidadSddpIntervalo.Caiajcodi = caiajcodi;
                        model.EntidadSddpIntervalo.Sddppiusucreacion = User.Identity.Name;
                        model.EntidadSddpIntervalo.Sddppifeccreacion = DateTime.Now;
                        model.EntidadSddpIntervalo.Sddppiintervalo = Convert.ToDateTime(dtRow[i].ToString());
                        model.EntidadSddpIntervalo.Sddppilaboral = 0;
                        model.EntidadSddpIntervalo.Sddppibloque = Convert.ToInt32(dtRow[i + 2].ToString());
                        Log.Info("Inserta Entidad Sddpint - SaveCaiSddpParamint");
                        this.servicioCAI.SaveCaiSddpParamint(model.EntidadSddpIntervalo);
                    }


                }

                //insertamos parametros generales 
                iFila = 0;
                i = 14;
                model.EntidadSddpParametro = new CaiSddpParametroDTO();
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 1)
                    {
                        continue;
                    }
                    //INSERTAR EL REGISTRO                   
                    if (dtRow[i].ToString() == "Tipo de cambio")
                    {
                        model.EntidadSddpParametro.Sddppmtc = Convert.ToDecimal(dtRow[i + 1].ToString());
                        continue;
                    }
                    if (dtRow[i].ToString() == "Tipo de cambio")
                    {
                        model.EntidadSddpParametro.Sddppmsemini = Convert.ToInt32(dtRow[i + 1].ToString());
                        continue;
                    }
                    if (dtRow[i].ToString() == "Semana Inicio reporte")
                    {
                        model.EntidadSddpParametro.Sddppmsemini = Convert.ToInt32(dtRow[i + 1].ToString());
                        continue;
                    }
                    if (dtRow[i].ToString() == "Numero de semanas")
                    {
                        model.EntidadSddpParametro.Sddppmnumsem = Convert.ToInt32(dtRow[i + 1].ToString());
                        continue;
                    }
                    if (dtRow[i].ToString() == "Cantidad de Bloques")
                    {
                        model.EntidadSddpParametro.Sddppmcantbloque = Convert.ToInt32(dtRow[i + 1].ToString());
                        continue;
                    }
                    if (dtRow[i].ToString() == "Número de Series")
                    {
                        model.EntidadSddpParametro.Sddppmnumserie = Convert.ToInt32(dtRow[i + 1].ToString());
                        model.EntidadSddpParametro.Sddppmcodi = 0;
                        model.EntidadSddpParametro.Caiajcodi = caiajcodi;
                        model.EntidadSddpParametro.Sddppmusucreacion = User.Identity.Name;
                        model.EntidadSddpParametro.Sddppmfeccreacion = DateTime.Now;
                        Log.Info("Inserta Entidad SddpParametro - SaveCaiSddpParametro");
                        this.servicioCAI.SaveCaiSddpParametro(model.EntidadSddpParametro);
                    }                

                }
                
                model.sMensaje = "Felicidades, la carga de información de Parámetros fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        [HttpPost]
        public JsonResult ProcesarCalculoCMG(int caiajcodi = 0)
        {

            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
            string sddpgmtipo = "CMG";
            string fuentedatos = "CM";
            try
            {
                //IMPLEMENTAR DELETE CON PARAMETRO DE AJUSTE
                Log.Info("Elimina Entidad Generdeman - DeleteCaiGenerdeman");
                this.servicioCAI.DeleteCaiGenerdeman(caiajcodi, fuentedatos);

                //validamos las barras que se encuentre inscritas
                Log.Info("Lista SddpGenmargDTO - GetByCriteriaCaiSddpGenmargsBarrNoIns");
                List<CaiSddpGenmargDTO> ListaCostoMarginalNoIns = this.servicioCAI.GetByCriteriaCaiSddpGenmargsBarrNoIns(sddpgmtipo);
                var BarrasNoIns = ListaCostoMarginalNoIns.Select(x => x.Sddpgmnombre).Distinct();
                string barrasFaltantes = string.Join(",", BarrasNoIns.ToArray());
                if (BarrasNoIns.Count() > 0)
                {
                    model.sError = "las siguientes barras no estan inscritas : <b> " + barrasFaltantes + "</b>";
                    return Json(model);
                }
                    
                //Cargamos Parametros 
                Log.Info("Entidad SddpParametro - GetByIdCaiSddpParametro");
                model.EntidadSddpParametro = this.servicioCAI.GetByIdCaiSddpParametro(caiajcodi);
                Log.Info("Lista SddpParamdia - GetByCriteriaCaiSddpParamdia");
                model.ListDiaLaboral = this.servicioCAI.GetByCriteriaCaiSddpParamdia(caiajcodi);
                Log.Info("Lista SddpParamint - GetByCriteriaCaiSddpParamint");
                model.ListNumeroBloque = this.servicioCAI.GetByCriteriaCaiSddpParamint(caiajcodi);
                Log.Info("Lista SddpParamsem - GetByCriteriaCaiSddpParamsem");
                model.ListNumeroSemana = this.servicioCAI.GetByCriteriaCaiSddpParamsem(caiajcodi);
                //Almacenamos parametros de configuracion
                int NumSerie = model.EntidadSddpParametro.Sddppmnumserie;
                int SemanaInicio = model.EntidadSddpParametro.Sddppmsemini;
                int NumSemana = model.EntidadSddpParametro.Sddppmnumsem + SemanaInicio;
                decimal TC = model.EntidadSddpParametro.Sddppmtc;
                //Cargamo Costo Marginnal
                Log.Info("Lista SddpGenmarg - GetByCriteriaCaiSddpGenmargs");
                List<CaiSddpGenmargDTO> ListaCostoMarginal = this.servicioCAI.GetByCriteriaCaiSddpGenmargs(sddpgmtipo);
                var nombrebarratodos = ListaCostoMarginal.Select(x => x.Sddpgmnombre).Distinct();
                //CaiGenerdemanDTO EntidadGenerDemanda = new CaiGenerdemanDTO();
                decimal total = 0;
                List<CaiGenerdemanDTO> entitys = new List<CaiGenerdemanDTO>();
                int iAux = 0;
                Log.Info("Obterner GenerdemanCodi - GetCodigoGeneradoCaiGenerdeman");
                int iCaiGenerdemanCodi = new CalculoPorcentajesAppServicio().GetCodigoGeneradoCaiGenerdeman();

                foreach (var barra in nombrebarratodos)
                {                    
                    foreach (var semana in model.ListNumeroSemana)
                    {
                        if (semana.Sddppsnumsem > NumSemana)
                        {
                            break;
                        }
                        if (semana.Sddppsnumsem >= SemanaInicio && semana.Sddppsnumsem < NumSemana)
                        {
                            Log.Info("Obterner TotalBloque1 - GetSumSddpGenmargByEtapaB1");
                            decimal TotalBloque1 = this.servicioCAI.GetSumSddpGenmargByEtapaB1(semana.Sddppsnumsem, barra);
                            Log.Info("Obterner TotalBloque2 - GetSumSddpGenmargByEtapaB2");
                            decimal TotalBloque2 = this.servicioCAI.GetSumSddpGenmargByEtapaB2(semana.Sddppsnumsem, barra);
                            Log.Info("Obterner TotalBloque3 - GetSumSddpGenmargByEtapaB3");
                            decimal TotalBloque3 = this.servicioCAI.GetSumSddpGenmargByEtapaB3(semana.Sddppsnumsem, barra);
                            Log.Info("Obterner TotalBloque4 - GetSumSddpGenmargByEtapaB4");
                            decimal TotalBloque4 = this.servicioCAI.GetSumSddpGenmargByEtapaB4(semana.Sddppsnumsem, barra);
                            Log.Info("Obterner TotalBloque5 - GetSumSddpGenmargByEtapaB5");
                            decimal TotalBloque5 = this.servicioCAI.GetSumSddpGenmargByEtapaB5(semana.Sddppsnumsem, barra);
                            
                            for (DateTime i = semana.Sddppsdiaini; i < semana.Sddppsdiafin; i = i.AddDays(1))
                            {
                                model.EntidadGenerDemanda = new CaiGenerdemanDTO();
                                model.EntidadGenerDemanda.Cagdcmcodi = iCaiGenerdemanCodi++;
                                model.EntidadGenerDemanda.Caiajcodi = caiajcodi;
                                model.EntidadGenerDemanda.Cagdcmfuentedat = fuentedatos; //Proyectados de Generación
                                //se obtiene de las equivalencias
                                CaiEquisddpbarrDTO EquivalenciaBarra = new CaiEquisddpbarrDTO();
                                Log.Info("Entidad Equisddpbarr - GetByNombreBarraSddp");
                                EquivalenciaBarra = this.servicioCAI.GetByNombreBarraSddp(barra);
                                CaiEquiunidbarrDTO EquivalenciaUnidad = new CaiEquiunidbarrDTO();
                                Log.Info("Entidad Equiunidbarr - GetByIdBarrcodi");
                                EquivalenciaUnidad = this.servicioCAI.GetByIdBarrcodi(EquivalenciaBarra.Barrcodi);
                                model.EntidadGenerDemanda.Ptomedicodi = EquivalenciaUnidad.Ptomedicodi;
                                model.EntidadGenerDemanda.Emprcodi = EquivalenciaUnidad.Emprcodi;
                                model.EntidadGenerDemanda.Equicodicen = EquivalenciaUnidad.Equicodicen;
                                model.EntidadGenerDemanda.Equicodiuni = EquivalenciaUnidad.Equicodiuni;
                                model.EntidadGenerDemanda.Cagdcmcalidadinfo = "S";
                                model.EntidadGenerDemanda.Cagdcmdia = i;
                                //asignamos el valos por intervalos de 15 minutos
                                total += model.EntidadGenerDemanda.H1 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H2 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H3 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H4 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H5 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H6 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H7 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H8 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H9 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H10 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H11 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H12 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H13 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H14 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H15 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H16 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H17 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H18 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H19 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H20 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H21 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H22 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H23 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H24 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H25 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H26 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H27 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H28 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H29 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H30 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H31 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H32 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H33 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H34 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H35 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H36 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H37 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H38 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H39 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H40 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H41 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H42 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H43 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H44 = TotalBloque4 / NumSerie * TC;
                                //varia
                                CaiSddpParamdiaDTO TipoDiaVariante1 = new CaiSddpParamdiaDTO();
                                Log.Info("Entidad SddpParamdia - GetByDiaCaiSddpParamdia");
                                TipoDiaVariante1 = this.servicioCAI.GetByDiaCaiSddpParamdia(i);
                                if (TipoDiaVariante1.Sddppdlaboral == 0)
                                {
                                    total += model.EntidadGenerDemanda.H45 = TotalBloque4 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H46 = TotalBloque4 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H47 = TotalBloque4 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H48 = TotalBloque4 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H77 = TotalBloque3 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H78 = TotalBloque3 / NumSerie * TC;
                                }
                                else
                                {
                                    total += model.EntidadGenerDemanda.H45 = TotalBloque2 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H46 = TotalBloque2 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H47 = TotalBloque2 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H48 = TotalBloque2 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H77 = TotalBloque1 / NumSerie * TC;
                                    total += model.EntidadGenerDemanda.H78 = TotalBloque1 / NumSerie * TC;
                                }
                                //----------------
                                total += model.EntidadGenerDemanda.H49 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H50 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H51 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H52 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H53 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H54 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H55 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H56 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H57 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H58 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H59 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H60 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H61 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H62 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H63 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H64 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H65 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H66 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H67 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H68 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H69 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H70 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H71 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H72 = TotalBloque4 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H73 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H74 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H75 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H76 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H79 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H80 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H81 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H82 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H83 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H84 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H85 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H86 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H87 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H88 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H89 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H90 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H91 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H92 = TotalBloque3 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H93 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H94 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H95 = TotalBloque5 / NumSerie * TC;
                                total += model.EntidadGenerDemanda.H96 = TotalBloque5 / NumSerie * TC;
                                model.EntidadGenerDemanda.Cagdcmtotaldia = total;
                                //Tipo de informacion por intervalo en este caso P de Proyectado
                                model.EntidadGenerDemanda.T1 = "P";
                                model.EntidadGenerDemanda.T2 = "P";
                                model.EntidadGenerDemanda.T3 = "P";
                                model.EntidadGenerDemanda.T4 = "P";
                                model.EntidadGenerDemanda.T5 = "P";
                                model.EntidadGenerDemanda.T6 = "P";
                                model.EntidadGenerDemanda.T7 = "P";
                                model.EntidadGenerDemanda.T8 = "P";
                                model.EntidadGenerDemanda.T9 = "P";
                                model.EntidadGenerDemanda.T10 = "P";
                                model.EntidadGenerDemanda.T11 = "P";
                                model.EntidadGenerDemanda.T12 = "P";
                                model.EntidadGenerDemanda.T13 = "P";
                                model.EntidadGenerDemanda.T14 = "P";
                                model.EntidadGenerDemanda.T15 = "P";
                                model.EntidadGenerDemanda.T16 = "P";
                                model.EntidadGenerDemanda.T17 = "P";
                                model.EntidadGenerDemanda.T18 = "P";
                                model.EntidadGenerDemanda.T19 = "P";
                                model.EntidadGenerDemanda.T20 = "P";
                                model.EntidadGenerDemanda.T21 = "P";
                                model.EntidadGenerDemanda.T22 = "P";
                                model.EntidadGenerDemanda.T23 = "P";
                                model.EntidadGenerDemanda.T24 = "P";
                                model.EntidadGenerDemanda.T25 = "P";
                                model.EntidadGenerDemanda.T26 = "P";
                                model.EntidadGenerDemanda.T27 = "P";
                                model.EntidadGenerDemanda.T28 = "P";
                                model.EntidadGenerDemanda.T29 = "P";
                                model.EntidadGenerDemanda.T30 = "P";
                                model.EntidadGenerDemanda.T31 = "P";
                                model.EntidadGenerDemanda.T32 = "P";
                                model.EntidadGenerDemanda.T33 = "P";
                                model.EntidadGenerDemanda.T34 = "P";
                                model.EntidadGenerDemanda.T35 = "P";
                                model.EntidadGenerDemanda.T36 = "P";
                                model.EntidadGenerDemanda.T37 = "P";
                                model.EntidadGenerDemanda.T38 = "P";
                                model.EntidadGenerDemanda.T39 = "P";
                                model.EntidadGenerDemanda.T40 = "P";
                                model.EntidadGenerDemanda.T41 = "P";
                                model.EntidadGenerDemanda.T42 = "P";
                                model.EntidadGenerDemanda.T43 = "P";
                                model.EntidadGenerDemanda.T44 = "P";
                                model.EntidadGenerDemanda.T45 = "P";
                                model.EntidadGenerDemanda.T46 = "P";
                                model.EntidadGenerDemanda.T47 = "P";
                                model.EntidadGenerDemanda.T48 = "P";
                                model.EntidadGenerDemanda.T49 = "P";
                                model.EntidadGenerDemanda.T50 = "P";
                                model.EntidadGenerDemanda.T51 = "P";
                                model.EntidadGenerDemanda.T52 = "P";
                                model.EntidadGenerDemanda.T53 = "P";
                                model.EntidadGenerDemanda.T54 = "P";
                                model.EntidadGenerDemanda.T55 = "P";
                                model.EntidadGenerDemanda.T56 = "P";
                                model.EntidadGenerDemanda.T57 = "P";
                                model.EntidadGenerDemanda.T58 = "P";
                                model.EntidadGenerDemanda.T59 = "P";
                                model.EntidadGenerDemanda.T60 = "P";
                                model.EntidadGenerDemanda.T61 = "P";
                                model.EntidadGenerDemanda.T62 = "P";
                                model.EntidadGenerDemanda.T63 = "P";
                                model.EntidadGenerDemanda.T64 = "P";
                                model.EntidadGenerDemanda.T65 = "P";
                                model.EntidadGenerDemanda.T66 = "P";
                                model.EntidadGenerDemanda.T67 = "P";
                                model.EntidadGenerDemanda.T68 = "P";
                                model.EntidadGenerDemanda.T69 = "P";
                                model.EntidadGenerDemanda.T70 = "P";
                                model.EntidadGenerDemanda.T71 = "P";
                                model.EntidadGenerDemanda.T72 = "P";
                                model.EntidadGenerDemanda.T73 = "P";
                                model.EntidadGenerDemanda.T74 = "P";
                                model.EntidadGenerDemanda.T75 = "P";
                                model.EntidadGenerDemanda.T76 = "P";
                                model.EntidadGenerDemanda.T77 = "P";
                                model.EntidadGenerDemanda.T78 = "P";
                                model.EntidadGenerDemanda.T79 = "P";
                                model.EntidadGenerDemanda.T80 = "P";
                                model.EntidadGenerDemanda.T81 = "P";
                                model.EntidadGenerDemanda.T82 = "P";
                                model.EntidadGenerDemanda.T83 = "P";
                                model.EntidadGenerDemanda.T84 = "P";
                                model.EntidadGenerDemanda.T85 = "P";
                                model.EntidadGenerDemanda.T86 = "P";
                                model.EntidadGenerDemanda.T87 = "P";
                                model.EntidadGenerDemanda.T88 = "P";
                                model.EntidadGenerDemanda.T89 = "P";
                                model.EntidadGenerDemanda.T90 = "P";
                                model.EntidadGenerDemanda.T91 = "P";
                                model.EntidadGenerDemanda.T92 = "P";
                                model.EntidadGenerDemanda.T93 = "P";
                                model.EntidadGenerDemanda.T94 = "P";
                                model.EntidadGenerDemanda.T95 = "P";
                                model.EntidadGenerDemanda.T96 = "P";
                                model.EntidadGenerDemanda.Cagdcmusucreacion = User.Identity.Name; ;
                                model.EntidadGenerDemanda.Cagdcmfeccreacion = DateTime.Now;
                                entitys.Add(model.EntidadGenerDemanda);
                                iAux++;

                                if (iAux == 3000)
                                {
                                    Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                                    new CalculoPorcentajesAppServicio().BulkInsertCaiGenerdeman(entitys);
                                    entitys = new List<CaiGenerdemanDTO>();
                                    iAux = 0;
                                }

                                
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                if (iAux > 0)
                {
                    Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                    new CalculoPorcentajesAppServicio().BulkInsertCaiGenerdeman(entitys);
                }

                model.sMensaje = "Felicidades, cálculo exitoso, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        [HttpPost]
        public JsonResult ProcesarCalculoEnergia(int caiajcodi = 0)
        {

            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
            string[] sddpgmtipo = { "TER", "HID", "GND" };

            try
            {
                //IMPLEMENTAR DELETE CON PARAMETRO DE AJUSTE
                foreach (string tipo in sddpgmtipo)
                {

                    //validamos las barras que se encuentre inscritas
                    Log.Info("Lista SddpGenmarg - GetByCriteriaCaiSddpGenmargsBarrNoIns");
                    List<CaiSddpGenmargDTO> ListaEnergiaNoIns = this.servicioCAI.GetByCriteriaCaiSddpGenmargsBarrNoIns(tipo);
                    var BarrasNoIns = ListaEnergiaNoIns.Select(x => x.Sddpgmnombre).Distinct();
                    string barrasFaltantes = string.Join(",", BarrasNoIns.ToArray());
                    if (BarrasNoIns.Count() > 0)
                    {
                        model.sError = "las siguientes barras no estan inscritas : <b> " + barrasFaltantes + "</b>";
                        return Json(model);
                    }
                    //Cargamos Parametros 
                    Log.Info("Entidad SddpParametro - GetByIdCaiSddpParametro");
                    model.EntidadSddpParametro = this.servicioCAI.GetByIdCaiSddpParametro(caiajcodi);
                    Log.Info("Lista SddpParamdia - GetByCriteriaCaiSddpParamdia");
                    model.ListDiaLaboral = this.servicioCAI.GetByCriteriaCaiSddpParamdia(caiajcodi);
                    Log.Info("Lista SddpParamint - GetByCriteriaCaiSddpParamint");
                    model.ListNumeroBloque = this.servicioCAI.GetByCriteriaCaiSddpParamint(caiajcodi);
                    Log.Info("Lista SddpParamsem - GetByCriteriaCaiSddpParamsem");
                    model.ListNumeroSemana = this.servicioCAI.GetByCriteriaCaiSddpParamsem(caiajcodi);

                    //Almacenamos parametros de configuracion
                    int NumSerie = model.EntidadSddpParametro.Sddppmnumserie;
                    int SemanaInicio = model.EntidadSddpParametro.Sddppmsemini;
                    int NumSemana = model.EntidadSddpParametro.Sddppmnumsem + SemanaInicio;
                    decimal TC = model.EntidadSddpParametro.Sddppmtc;

                    //Cargamo Costo Marginnal
                    Log.Info("Lista SddpGenmarg - GetByCriteriaCaiSddpGenmargs");
                    List<CaiSddpGenmargDTO> ListaCostoMarginal = this.servicioCAI.GetByCriteriaCaiSddpGenmargs(tipo);
                    var nombrebarratodos = ListaCostoMarginal.Select(x => x.Sddpgmnombre).Distinct();

                    //CaiGenerdemanDTO EntidadGenerDemanda = new CaiGenerdemanDTO();
                    decimal total = 0;
                    List<CaiGenerdemanDTO> entitys = new List<CaiGenerdemanDTO>();
                    int iAux = 0;
                    Log.Info("Obterner GenerdemanCodi - GetCodigoGeneradoCaiGenerdeman");
                    int iCaiGenerdemanCodi = new CalculoPorcentajesAppServicio().GetCodigoGeneradoCaiGenerdeman();
                    decimal HoraBloq1 = 0;
                    decimal HoraBloq2 = 0;
                    decimal HoraBloq3 = 0;
                    decimal HoraBloq4 = 0;
                    decimal HoraBloq5 = 0;
                    foreach (var barra in nombrebarratodos)
                    {
                        foreach (var semana in model.ListNumeroSemana)
                        {
                            if (semana.Sddppsnumsem >= SemanaInicio && semana.Sddppsnumsem < NumSemana)
                            {

                                Log.Info("Lista SddpDuracion - ListCaiSddpDuracionPorEtapa");
                                model.ListaDuracionPorEtapa = this.servicioCAI.ListCaiSddpDuracionPorEtapa(semana.Sddppsnumsem);
                                foreach (var bloque in model.ListaDuracionPorEtapa)
                                {
                                    if (bloque.Sddpdubloque == 1)
                                    {
                                        HoraBloq1 = bloque.Sddpduduracion;
                                    }
                                    if (bloque.Sddpdubloque == 2)
                                    {
                                        HoraBloq2 = bloque.Sddpduduracion;
                                    }
                                    if (bloque.Sddpdubloque == 3)
                                    {
                                        HoraBloq3 = bloque.Sddpduduracion;
                                    }
                                    if (bloque.Sddpdubloque == 4)
                                    {
                                        HoraBloq4 = bloque.Sddpduduracion;
                                    }
                                    if (bloque.Sddpdubloque == 5)
                                    {
                                        HoraBloq5 = bloque.Sddpduduracion;
                                    }
                                }



                                Log.Info("Obtener TotalBloque1 - GetSumSddpGenmargByEtapa");
                                decimal TotalBloque1 = this.servicioCAI.GetSumSddpGenmargByEtapa(tipo, semana.Sddppsnumsem, 1, barra);
                                Log.Info("Obtener TotalBloque2 - GetSumSddpGenmargByEtapa");
                                decimal TotalBloque2 = this.servicioCAI.GetSumSddpGenmargByEtapa(tipo, semana.Sddppsnumsem, 2, barra);
                                Log.Info("Obtener TotalBloque3 - GetSumSddpGenmargByEtapa");
                                decimal TotalBloque3 = this.servicioCAI.GetSumSddpGenmargByEtapa(tipo, semana.Sddppsnumsem, 3, barra);
                                Log.Info("Obtener TotalBloque4 - GetSumSddpGenmargByEtapa");
                                decimal TotalBloque4 = this.servicioCAI.GetSumSddpGenmargByEtapa(tipo, semana.Sddppsnumsem, 4, barra);
                                Log.Info("Obtener TotalBloque5 - GetSumSddpGenmargByEtapa");
                                decimal TotalBloque5 = this.servicioCAI.GetSumSddpGenmargByEtapa(tipo, semana.Sddppsnumsem, 5, barra);

                                for (DateTime i = semana.Sddppsdiaini; i < semana.Sddppsdiafin; i = i.AddDays(1))
                                {

                                    model.EntidadGenerDemanda = new CaiGenerdemanDTO();
                                    model.EntidadGenerDemanda.Cagdcmcodi = iCaiGenerdemanCodi++;
                                    model.EntidadGenerDemanda.Caiajcodi = caiajcodi;
                                    model.EntidadGenerDemanda.Cagdcmfuentedat = "PG"; //Proyectados de Generación
                                    //se obtiene de las equivalencias
                                    CaiEquisddpbarrDTO EquivalenciaBarra = new CaiEquisddpbarrDTO();
                                    Log.Info("Entidad Equisddpbarr - GetByNombreBarraSddp");
                                    EquivalenciaBarra = this.servicioCAI.GetByNombreBarraSddp(barra);
                                    CaiEquiunidbarrDTO EquivalenciaUnidad = new CaiEquiunidbarrDTO();
                                    Log.Info("Entidad Equiunidbarr - GetByIdBarrcodi");
                                    EquivalenciaUnidad = this.servicioCAI.GetByIdBarrcodi(EquivalenciaBarra.Barrcodi);
                                    model.EntidadGenerDemanda.Ptomedicodi = EquivalenciaUnidad.Ptomedicodi;
                                    model.EntidadGenerDemanda.Emprcodi = EquivalenciaUnidad.Emprcodi;
                                    model.EntidadGenerDemanda.Equicodicen = EquivalenciaUnidad.Equicodicen;
                                    model.EntidadGenerDemanda.Equicodiuni = EquivalenciaUnidad.Equicodiuni;
                                    model.EntidadGenerDemanda.Cagdcmcalidadinfo = "S";
                                    model.EntidadGenerDemanda.Cagdcmdia = i;
                                    //asignamos el valos por intervalos de 15 minutos

                                    // Formula
                                    //energia = energia / (duracion * 4 * nseries) * 1000

                                    total += model.EntidadGenerDemanda.H1 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H2 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H3 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H4 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H5 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H6 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H7 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H8 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H9 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H10 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H11 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H12 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H13 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H14 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H15 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H16 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H17 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H18 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H19 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H20 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H21 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H22 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H23 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H24 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H25 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H26 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H27 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H28 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H29 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H30 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H31 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H32 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H33 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H34 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H35 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H36 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H37 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H38 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H39 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H40 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H41 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H42 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H43 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H44 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    //varia
                                    CaiSddpParamdiaDTO TipoDiaVariante1 = new CaiSddpParamdiaDTO();
                                    Log.Info("Entidad SddpParamdia - GetByDiaCaiSddpParamdia");
                                    TipoDiaVariante1 = this.servicioCAI.GetByDiaCaiSddpParamdia(i);
                                    if (TipoDiaVariante1.Sddppdlaboral == 0)
                                    {
                                        total += model.EntidadGenerDemanda.H45 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H46 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H47 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H48 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H77 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H78 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    }
                                    else
                                    {
                                        total += model.EntidadGenerDemanda.H45 = TotalBloque2 / (HoraBloq2 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H46 = TotalBloque2 / (HoraBloq2 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H47 = TotalBloque2 / (HoraBloq2 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H48 = TotalBloque2 / (HoraBloq2 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H77 = TotalBloque1 / (HoraBloq1 * 4 * NumSerie) * 1000;
                                        total += model.EntidadGenerDemanda.H78 = TotalBloque1 / (HoraBloq1 * 4 * NumSerie) * 1000;
                                    }
                                    //----------------
                                    total += model.EntidadGenerDemanda.H49 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H50 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H51 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H52 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H53 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H54 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H55 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H56 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H57 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H58 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H59 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H60 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H61 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H62 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H63 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H64 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H65 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H66 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H67 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H68 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H69 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H70 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H71 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H72 = TotalBloque4 / (HoraBloq4 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H73 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H74 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H75 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H76 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H79 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H80 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H81 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H82 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H83 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H84 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H85 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H86 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H87 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H88 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H89 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H90 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H91 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H92 = TotalBloque3 / (HoraBloq3 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H93 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H94 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H95 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    total += model.EntidadGenerDemanda.H96 = TotalBloque5 / (HoraBloq5 * 4 * NumSerie) * 1000;
                                    model.EntidadGenerDemanda.Cagdcmtotaldia = total;
                                    //Tipo de informacion por intervalo en este caso P de Proyectado
                                    model.EntidadGenerDemanda.T1 = "PG";
                                    model.EntidadGenerDemanda.T2 = "PG";
                                    model.EntidadGenerDemanda.T3 = "PG";
                                    model.EntidadGenerDemanda.T4 = "PG";
                                    model.EntidadGenerDemanda.T5 = "PG";
                                    model.EntidadGenerDemanda.T6 = "PG";
                                    model.EntidadGenerDemanda.T7 = "PG";
                                    model.EntidadGenerDemanda.T8 = "PG";
                                    model.EntidadGenerDemanda.T9 = "PG";
                                    model.EntidadGenerDemanda.T10 = "PG";
                                    model.EntidadGenerDemanda.T11 = "PG";
                                    model.EntidadGenerDemanda.T12 = "PG";
                                    model.EntidadGenerDemanda.T13 = "PG";
                                    model.EntidadGenerDemanda.T14 = "PG";
                                    model.EntidadGenerDemanda.T15 = "PG";
                                    model.EntidadGenerDemanda.T16 = "PG";
                                    model.EntidadGenerDemanda.T17 = "PG";
                                    model.EntidadGenerDemanda.T18 = "PG";
                                    model.EntidadGenerDemanda.T19 = "PG";
                                    model.EntidadGenerDemanda.T20 = "PG";
                                    model.EntidadGenerDemanda.T21 = "PG";
                                    model.EntidadGenerDemanda.T22 = "PG";
                                    model.EntidadGenerDemanda.T23 = "PG";
                                    model.EntidadGenerDemanda.T24 = "PG";
                                    model.EntidadGenerDemanda.T25 = "PG";
                                    model.EntidadGenerDemanda.T26 = "PG";
                                    model.EntidadGenerDemanda.T27 = "PG";
                                    model.EntidadGenerDemanda.T28 = "PG";
                                    model.EntidadGenerDemanda.T29 = "PG";
                                    model.EntidadGenerDemanda.T30 = "PG";
                                    model.EntidadGenerDemanda.T31 = "PG";
                                    model.EntidadGenerDemanda.T32 = "PG";
                                    model.EntidadGenerDemanda.T33 = "PG";
                                    model.EntidadGenerDemanda.T34 = "PG";
                                    model.EntidadGenerDemanda.T35 = "PG";
                                    model.EntidadGenerDemanda.T36 = "PG";
                                    model.EntidadGenerDemanda.T37 = "PG";
                                    model.EntidadGenerDemanda.T38 = "PG";
                                    model.EntidadGenerDemanda.T39 = "PG";
                                    model.EntidadGenerDemanda.T40 = "PG";
                                    model.EntidadGenerDemanda.T41 = "PG";
                                    model.EntidadGenerDemanda.T42 = "PG";
                                    model.EntidadGenerDemanda.T43 = "PG";
                                    model.EntidadGenerDemanda.T44 = "PG";
                                    model.EntidadGenerDemanda.T45 = "PG";
                                    model.EntidadGenerDemanda.T46 = "PG";
                                    model.EntidadGenerDemanda.T47 = "PG";
                                    model.EntidadGenerDemanda.T48 = "PG";
                                    model.EntidadGenerDemanda.T49 = "PG";
                                    model.EntidadGenerDemanda.T50 = "PG";
                                    model.EntidadGenerDemanda.T51 = "PG";
                                    model.EntidadGenerDemanda.T52 = "PG";
                                    model.EntidadGenerDemanda.T53 = "PG";
                                    model.EntidadGenerDemanda.T54 = "PG";
                                    model.EntidadGenerDemanda.T55 = "PG";
                                    model.EntidadGenerDemanda.T56 = "PG";
                                    model.EntidadGenerDemanda.T57 = "PG";
                                    model.EntidadGenerDemanda.T58 = "PG";
                                    model.EntidadGenerDemanda.T59 = "PG";
                                    model.EntidadGenerDemanda.T60 = "PG";
                                    model.EntidadGenerDemanda.T61 = "PG";
                                    model.EntidadGenerDemanda.T62 = "PG";
                                    model.EntidadGenerDemanda.T63 = "PG";
                                    model.EntidadGenerDemanda.T64 = "PG";
                                    model.EntidadGenerDemanda.T65 = "PG";
                                    model.EntidadGenerDemanda.T66 = "PG";
                                    model.EntidadGenerDemanda.T67 = "PG";
                                    model.EntidadGenerDemanda.T68 = "PG";
                                    model.EntidadGenerDemanda.T69 = "PG";
                                    model.EntidadGenerDemanda.T70 = "PG";
                                    model.EntidadGenerDemanda.T71 = "PG";
                                    model.EntidadGenerDemanda.T72 = "PG";
                                    model.EntidadGenerDemanda.T73 = "PG";
                                    model.EntidadGenerDemanda.T74 = "PG";
                                    model.EntidadGenerDemanda.T75 = "PG";
                                    model.EntidadGenerDemanda.T76 = "PG";
                                    model.EntidadGenerDemanda.T77 = "PG";
                                    model.EntidadGenerDemanda.T78 = "PG";
                                    model.EntidadGenerDemanda.T79 = "PG";
                                    model.EntidadGenerDemanda.T80 = "PG";
                                    model.EntidadGenerDemanda.T81 = "PG";
                                    model.EntidadGenerDemanda.T82 = "PG";
                                    model.EntidadGenerDemanda.T83 = "PG";
                                    model.EntidadGenerDemanda.T84 = "PG";
                                    model.EntidadGenerDemanda.T85 = "PG";
                                    model.EntidadGenerDemanda.T86 = "PG";
                                    model.EntidadGenerDemanda.T87 = "PG";
                                    model.EntidadGenerDemanda.T88 = "PG";
                                    model.EntidadGenerDemanda.T89 = "PG";
                                    model.EntidadGenerDemanda.T90 = "PG";
                                    model.EntidadGenerDemanda.T91 = "PG";
                                    model.EntidadGenerDemanda.T92 = "PG";
                                    model.EntidadGenerDemanda.T93 = "PG";
                                    model.EntidadGenerDemanda.T94 = "PG";
                                    model.EntidadGenerDemanda.T95 = "PG";
                                    model.EntidadGenerDemanda.T96 = "PG";
                                    model.EntidadGenerDemanda.Cagdcmusucreacion = User.Identity.Name; ;
                                    model.EntidadGenerDemanda.Cagdcmfeccreacion = DateTime.Now;
                                    entitys.Add(model.EntidadGenerDemanda);
                                    iAux++;

                                    if (iAux == 3000)
                                    {
                                        Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                                        new CalculoPorcentajesAppServicio().BulkInsertCaiGenerdeman(entitys);
                                        entitys = new List<CaiGenerdemanDTO>();
                                        iAux = 0;
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    if (iAux > 0)
                    {
                        Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                        new CalculoPorcentajesAppServicio().BulkInsertCaiGenerdeman(entitys);
                    }
                }
                model.sMensaje = "Felicidades, cálculo exitoso, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        [HttpPost]
        public JsonResult ProcesarResultadosCMG(int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
            string tipo = "CM";
            string path = ConfigurationManager.AppSettings[ConstantesCalculoPorcentajes.RepositorioDirectorio].ToString() + ConstantesCalculoPorcentajes.SDDP_RESULTADOSCMG;
            try
            {
                //Elimina información de la tabla CAI_SDDP_DURACION
                Log.Info("Elimina registro - DeleteCaiGenerdeman");
                this.servicioCAI.DeleteCaiGenerdeman(caiajcodi, tipo);

                ////Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                ds = this.servicioCAI.GeneraDataset(path, 1);
                Log.Info("Obterner GenerdemanCodi - GetCodigoGeneradoCaiGenerdeman");
                Int32 iCaiGenerdemanCodi = new CalculoPorcentajesAppServicio().GetCodigoGeneradoCaiGenerdeman();

                //Lista detalle de Cabecera
                string[] listaCabecera = new string[(ds.Tables[0].Columns.Count - 2)];//le quitamos las 2 primeras columnas
                List<string> BarrasNoIns = new List<string>();
                List<string> BarrasPtNoIns = new List<string>();
                int iFila = 0;
                int iNroCabeceras = 0;
                int iNroBarrasNoIns = 0;
                int ix = 0;
                int iAux = 0;
                decimal total = 0;
                ArrayList arrylist = new ArrayList();
                decimal suma = 0;

                foreach (DataColumn dtoCol in ds.Tables[0].Columns)
                {
                    iFila++;
                    if (iFila < 3)
                    {
                        continue;
                    }
                    listaCabecera[iNroCabeceras] = dtoCol.ToString();
                    iNroCabeceras++;
                    //}
                    //break;

                }

                foreach (var barra in listaCabecera)
                {
                    //validamos las barras que se encuentre inscritas
                    Log.Info("Entidad CaiEquisddpbarrDTO - GetByNombreBarraSddp");
                    CaiEquisddpbarrDTO EntidadBarraNoIns = this.servicioCAI.GetByNombreBarraSddp(barra.Trim());
                    if (EntidadBarraNoIns == null)
                    {
                        BarrasNoIns.Add(barra);
                    }
                    else
                    {
                        continue;
                    }
                }

                if (BarrasNoIns.Count() > 0)
                {
                    string barrasFaltantes = string.Join(",", BarrasNoIns.ToArray());
                    model.sError = "las siguiente barra no estan inscritas : <b> " + barrasFaltantes + "</b>";
                    return Json(model);
                }

                foreach (var barra in listaCabecera)
                {
                    //validamos las barras que se encuentre inscritas
                    Log.Info("Entidad CaiEquisddpbarrDTO - GetByNombreBarraSddp");
                    CaiEquisddpbarrDTO EntidadBarraNoIns = this.servicioCAI.GetByNombreBarraSddp(barra.Trim());
                    CaiEquiunidbarrDTO EntidadBarrPtNoIns = this.servicioCAI.GetByIdBarrcodi(EntidadBarraNoIns.Barrcodi);
                    if (EntidadBarrPtNoIns == null)
                    {
                        BarrasPtNoIns.Add(barra);
                    }
                    else
                    {
                        continue;
                    }
                }

                if (BarrasPtNoIns.Count() > 0)
                {
                    string barrasPtFaltantes = string.Join(",", BarrasPtNoIns.ToArray());
                    model.sError = "las siguiente barras no estan asociadas a un Pt Medicion : <b> " + barrasPtFaltantes + "</b>";
                    return Json(model);
                }


                //Lista Bulk
                List<CaiGenerdemanDTO> entitys = new List<CaiGenerdemanDTO>();
                ArrayList listaValores = new ArrayList();
                ArrayList ListFechas = new ArrayList();
                //Recorrer columna por columna
                foreach (DataColumn dtCol in ds.Tables[0].Columns)
                {
                    //capturando las fechas                    
                    bool bSalir = false;
                    //int iFilaFec = 0;
                    foreach (DataRow dtRowfec in ds.Tables[0].Rows)
                    {
                        if (dtCol.ColumnName.Equals("Column1"))
                        {
                            //iFilaFec++;
                            //if (iFilaFec < 2)
                            //{
                            //    continue;
                            //}
                            string svalor = dtRowfec[dtCol].ToString();
                            ListFechas.Add(svalor);
                        }
                    }

                    foreach (DataRow dtRow in ds.Tables[0].Rows)
                    {
                        if (dtCol.ColumnName.Equals("Column1") || dtCol.ColumnName.Equals("Column2"))
                        {
                            bSalir = true;
                            break;
                        }
                        else
                        {
                            string svalor = dtRow[dtCol].ToString();
                            listaValores.Add(svalor);
                            bSalir = false;
                        }
                    }

                    if (!bSalir)
                    {

                        int cantidadve = 96;
                        ArrayList Listpordias = new ArrayList(cantidadve);
                        for (int c = 0; c < listaValores.Count; c += cantidadve)
                        {
                            var arraydia = new ArrayList();
                            arraydia.AddRange(listaValores.GetRange(c, cantidadve));
                            Listpordias.Add(arraydia);
                        }
                        int Fec = 0;
                        for (int c = 0; c < Listpordias.Count; c++)
                        {
                            // grabar detalle
                            model.EntidadGenerDemanda = new CaiGenerdemanDTO();
                            model.EntidadGenerDemanda.Caiajcodi = caiajcodi;
                            model.EntidadGenerDemanda.Cagdcmfuentedat = tipo; //Proyectados de Generación
                            //se obtiene de las equivalencias
                            CaiEquisddpbarrDTO EquivalenciaBarra = new CaiEquisddpbarrDTO();
                            Log.Info("Entidad Equisddpbarr - GetByNombreBarraSddp");
                            EquivalenciaBarra = this.servicioCAI.GetByNombreBarraSddp(dtCol.ToString().Trim());
                            CaiEquiunidbarrDTO EquivalenciaUnidad = new CaiEquiunidbarrDTO();
                            Log.Info("Entidad Equiunidbarr - GetByIdBarrcodi");
                            EquivalenciaUnidad = this.servicioCAI.GetByIdBarrcodi(EquivalenciaBarra.Barrcodi);
                            model.EntidadGenerDemanda.Ptomedicodi = EquivalenciaUnidad.Ptomedicodi;
                            model.EntidadGenerDemanda.Emprcodi = EquivalenciaUnidad.Emprcodi;
                            model.EntidadGenerDemanda.Equicodicen = EquivalenciaUnidad.Equicodicen;
                            model.EntidadGenerDemanda.Equicodiuni = EquivalenciaUnidad.Equicodiuni;
                            model.EntidadGenerDemanda.Cagdcmcalidadinfo = "S";

                            model.EntidadGenerDemanda.Cagdcmdia = DateTime.Parse(ListFechas[Fec].ToString());
                            Fec = Fec + 96;
                            arrylist = (ArrayList)Listpordias[c];
                            suma = 0;

                            try
                            {

                                suma += model.EntidadGenerDemanda.H1 = Decimal.Parse(arrylist[0].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H2 = Decimal.Parse(arrylist[1].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H3 = Decimal.Parse(arrylist[2].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H4 = Decimal.Parse(arrylist[3].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H5 = Decimal.Parse(arrylist[4].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H6 = Decimal.Parse(arrylist[5].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H7 = Decimal.Parse(arrylist[6].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H8 = Decimal.Parse(arrylist[7].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H9 = Decimal.Parse(arrylist[8].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H10 = Decimal.Parse(arrylist[9].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H11 = Decimal.Parse(arrylist[10].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H12 = Decimal.Parse(arrylist[11].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H13 = Decimal.Parse(arrylist[12].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H14 = Decimal.Parse(arrylist[13].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H15 = Decimal.Parse(arrylist[14].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H16 = Decimal.Parse(arrylist[15].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H17 = Decimal.Parse(arrylist[16].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H18 = Decimal.Parse(arrylist[17].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H19 = Decimal.Parse(arrylist[18].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H20 = Decimal.Parse(arrylist[19].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H21 = Decimal.Parse(arrylist[20].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H22 = Decimal.Parse(arrylist[21].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H23 = Decimal.Parse(arrylist[22].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H24 = Decimal.Parse(arrylist[23].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H25 = Decimal.Parse(arrylist[24].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H26 = Decimal.Parse(arrylist[25].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H27 = Decimal.Parse(arrylist[26].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H28 = Decimal.Parse(arrylist[27].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H29 = Decimal.Parse(arrylist[28].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H30 = Decimal.Parse(arrylist[29].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H31 = Decimal.Parse(arrylist[30].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H32 = Decimal.Parse(arrylist[31].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H33 = Decimal.Parse(arrylist[32].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H34 = Decimal.Parse(arrylist[33].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H35 = Decimal.Parse(arrylist[34].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H36 = Decimal.Parse(arrylist[35].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H37 = Decimal.Parse(arrylist[36].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H38 = Decimal.Parse(arrylist[37].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H39 = Decimal.Parse(arrylist[38].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H40 = Decimal.Parse(arrylist[39].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H41 = Decimal.Parse(arrylist[40].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H42 = Decimal.Parse(arrylist[41].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H43 = Decimal.Parse(arrylist[42].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H44 = Decimal.Parse(arrylist[43].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H45 = Decimal.Parse(arrylist[44].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H46 = Decimal.Parse(arrylist[45].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H47 = Decimal.Parse(arrylist[46].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H48 = Decimal.Parse(arrylist[47].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H49 = Decimal.Parse(arrylist[48].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H50 = Decimal.Parse(arrylist[49].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H51 = Decimal.Parse(arrylist[50].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H52 = Decimal.Parse(arrylist[51].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H53 = Decimal.Parse(arrylist[52].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H54 = Decimal.Parse(arrylist[53].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H55 = Decimal.Parse(arrylist[54].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H56 = Decimal.Parse(arrylist[55].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H57 = Decimal.Parse(arrylist[56].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H58 = Decimal.Parse(arrylist[57].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H59 = Decimal.Parse(arrylist[58].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H60 = Decimal.Parse(arrylist[59].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H61 = Decimal.Parse(arrylist[60].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H62 = Decimal.Parse(arrylist[61].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H63 = Decimal.Parse(arrylist[62].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H64 = Decimal.Parse(arrylist[63].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H65 = Decimal.Parse(arrylist[64].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H66 = Decimal.Parse(arrylist[65].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H67 = Decimal.Parse(arrylist[66].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H68 = Decimal.Parse(arrylist[67].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H69 = Decimal.Parse(arrylist[68].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H70 = Decimal.Parse(arrylist[69].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H71 = Decimal.Parse(arrylist[70].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H72 = Decimal.Parse(arrylist[71].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H73 = Decimal.Parse(arrylist[72].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H74 = Decimal.Parse(arrylist[73].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H75 = Decimal.Parse(arrylist[74].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H76 = Decimal.Parse(arrylist[75].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H77 = Decimal.Parse(arrylist[76].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H78 = Decimal.Parse(arrylist[77].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H79 = Decimal.Parse(arrylist[78].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H80 = Decimal.Parse(arrylist[79].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H81 = Decimal.Parse(arrylist[80].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H82 = Decimal.Parse(arrylist[81].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H83 = Decimal.Parse(arrylist[82].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H84 = Decimal.Parse(arrylist[83].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H85 = Decimal.Parse(arrylist[84].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H86 = Decimal.Parse(arrylist[85].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H87 = Decimal.Parse(arrylist[86].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H88 = Decimal.Parse(arrylist[87].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H89 = Decimal.Parse(arrylist[88].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H90 = Decimal.Parse(arrylist[89].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H91 = Decimal.Parse(arrylist[90].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H92 = Decimal.Parse(arrylist[91].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H93 = Decimal.Parse(arrylist[92].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H94 = Decimal.Parse(arrylist[93].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H95 = Decimal.Parse(arrylist[94].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H96 = Decimal.Parse(arrylist[95].ToString(), System.Globalization.NumberStyles.Float);
                                model.EntidadGenerDemanda.Cagdcmtotaldia = suma;
                                //Tipo de informacion por intervalo en este caso P de Proyectado
                                model.EntidadGenerDemanda.T1 = "P";
                                model.EntidadGenerDemanda.T2 = "P";
                                model.EntidadGenerDemanda.T3 = "P";
                                model.EntidadGenerDemanda.T4 = "P";
                                model.EntidadGenerDemanda.T5 = "P";
                                model.EntidadGenerDemanda.T6 = "P";
                                model.EntidadGenerDemanda.T7 = "P";
                                model.EntidadGenerDemanda.T8 = "P";
                                model.EntidadGenerDemanda.T9 = "P";
                                model.EntidadGenerDemanda.T10 = "P";
                                model.EntidadGenerDemanda.T11 = "P";
                                model.EntidadGenerDemanda.T12 = "P";
                                model.EntidadGenerDemanda.T13 = "P";
                                model.EntidadGenerDemanda.T14 = "P";
                                model.EntidadGenerDemanda.T15 = "P";
                                model.EntidadGenerDemanda.T16 = "P";
                                model.EntidadGenerDemanda.T17 = "P";
                                model.EntidadGenerDemanda.T18 = "P";
                                model.EntidadGenerDemanda.T19 = "P";
                                model.EntidadGenerDemanda.T20 = "P";
                                model.EntidadGenerDemanda.T21 = "P";
                                model.EntidadGenerDemanda.T22 = "P";
                                model.EntidadGenerDemanda.T23 = "P";
                                model.EntidadGenerDemanda.T24 = "P";
                                model.EntidadGenerDemanda.T25 = "P";
                                model.EntidadGenerDemanda.T26 = "P";
                                model.EntidadGenerDemanda.T27 = "P";
                                model.EntidadGenerDemanda.T28 = "P";
                                model.EntidadGenerDemanda.T29 = "P";
                                model.EntidadGenerDemanda.T30 = "P";
                                model.EntidadGenerDemanda.T31 = "P";
                                model.EntidadGenerDemanda.T32 = "P";
                                model.EntidadGenerDemanda.T33 = "P";
                                model.EntidadGenerDemanda.T34 = "P";
                                model.EntidadGenerDemanda.T35 = "P";
                                model.EntidadGenerDemanda.T36 = "P";
                                model.EntidadGenerDemanda.T37 = "P";
                                model.EntidadGenerDemanda.T38 = "P";
                                model.EntidadGenerDemanda.T39 = "P";
                                model.EntidadGenerDemanda.T40 = "P";
                                model.EntidadGenerDemanda.T41 = "P";
                                model.EntidadGenerDemanda.T42 = "P";
                                model.EntidadGenerDemanda.T43 = "P";
                                model.EntidadGenerDemanda.T44 = "P";
                                model.EntidadGenerDemanda.T45 = "P";
                                model.EntidadGenerDemanda.T46 = "P";
                                model.EntidadGenerDemanda.T47 = "P";
                                model.EntidadGenerDemanda.T48 = "P";
                                model.EntidadGenerDemanda.T49 = "P";
                                model.EntidadGenerDemanda.T50 = "P";
                                model.EntidadGenerDemanda.T51 = "P";
                                model.EntidadGenerDemanda.T52 = "P";
                                model.EntidadGenerDemanda.T53 = "P";
                                model.EntidadGenerDemanda.T54 = "P";
                                model.EntidadGenerDemanda.T55 = "P";
                                model.EntidadGenerDemanda.T56 = "P";
                                model.EntidadGenerDemanda.T57 = "P";
                                model.EntidadGenerDemanda.T58 = "P";
                                model.EntidadGenerDemanda.T59 = "P";
                                model.EntidadGenerDemanda.T60 = "P";
                                model.EntidadGenerDemanda.T61 = "P";
                                model.EntidadGenerDemanda.T62 = "P";
                                model.EntidadGenerDemanda.T63 = "P";
                                model.EntidadGenerDemanda.T64 = "P";
                                model.EntidadGenerDemanda.T65 = "P";
                                model.EntidadGenerDemanda.T66 = "P";
                                model.EntidadGenerDemanda.T67 = "P";
                                model.EntidadGenerDemanda.T68 = "P";
                                model.EntidadGenerDemanda.T69 = "P";
                                model.EntidadGenerDemanda.T70 = "P";
                                model.EntidadGenerDemanda.T71 = "P";
                                model.EntidadGenerDemanda.T72 = "P";
                                model.EntidadGenerDemanda.T73 = "P";
                                model.EntidadGenerDemanda.T74 = "P";
                                model.EntidadGenerDemanda.T75 = "P";
                                model.EntidadGenerDemanda.T76 = "P";
                                model.EntidadGenerDemanda.T77 = "P";
                                model.EntidadGenerDemanda.T78 = "P";
                                model.EntidadGenerDemanda.T79 = "P";
                                model.EntidadGenerDemanda.T80 = "P";
                                model.EntidadGenerDemanda.T81 = "P";
                                model.EntidadGenerDemanda.T82 = "P";
                                model.EntidadGenerDemanda.T83 = "P";
                                model.EntidadGenerDemanda.T84 = "P";
                                model.EntidadGenerDemanda.T85 = "P";
                                model.EntidadGenerDemanda.T86 = "P";
                                model.EntidadGenerDemanda.T87 = "P";
                                model.EntidadGenerDemanda.T88 = "P";
                                model.EntidadGenerDemanda.T89 = "P";
                                model.EntidadGenerDemanda.T90 = "P";
                                model.EntidadGenerDemanda.T91 = "P";
                                model.EntidadGenerDemanda.T92 = "P";
                                model.EntidadGenerDemanda.T93 = "P";
                                model.EntidadGenerDemanda.T94 = "P";
                                model.EntidadGenerDemanda.T95 = "P";
                                model.EntidadGenerDemanda.T96 = "P";
                                model.EntidadGenerDemanda.Cagdcmusucreacion = User.Identity.Name; ;
                                model.EntidadGenerDemanda.Cagdcmfeccreacion = DateTime.Now;
                                model.EntidadGenerDemanda.Cagdcmcodi = iCaiGenerdemanCodi;
                                entitys.Add(model.EntidadGenerDemanda);
                                iCaiGenerdemanCodi++;
                                iAux++;

                                if (iAux == 3000)
                                {
                                    Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                                    new CalculoPorcentajesAppServicio().BulkInsertCaiGenerdeman(entitys);
                                    entitys = new List<CaiGenerdemanDTO>();
                                    iAux = 0;
                                }
                            }
                            catch
                            {
                                return Json("Lo sentimos, se ha producido un error en la lectura de información del código: ");
                            }
                        }
                        listaValores.Clear();
                    }
                }

                if (iAux > 0)
                {
                    Log.Info("Bulk Insert - BulkInsertValorSddpGenmarg");
                    new CalculoPorcentajesAppServicio().BulkInsertCaiGenerdeman(entitys);
                }

                model.sMensaje = "Felicidades, la carga de información de Costo Marginal fue exitosa , Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        [HttpPost]
        public JsonResult ProcesarResultadosEnergia(int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            GeneracionProyectadaModel model = new GeneracionProyectadaModel();
            model.sMensaje = "";
            model.sError = "";
            string tipo = "PG";
            string path = ConfigurationManager.AppSettings[ConstantesCalculoPorcentajes.RepositorioDirectorio].ToString() + ConstantesCalculoPorcentajes.SDDP_RESULTADOSENERGIA;

            try
            {
                //elimino la informacion de la tabla CAI_GENERDEMAN
                this.servicioCAI.DeleteCaiGenerdeman(caiajcodi, tipo);

                //leemos el contenido del archivo excel 
                DataSet ds = new DataSet();
                ds = this.servicioCAI.GeneraDataset(path, 1);
                Int32 iCaiGenerdemancodi = new CalculoPorcentajesAppServicio().GetCodigoGeneradoCaiGenerdeman();

                //Lista detalle de cabecera
                string[] listaCabecera = new string[(ds.Tables[0].Columns.Count - 2)];// le quitamos las 2 primeras columnas
                List<string> BarrasNoIns = new List<string>();
                List<string> BarrasPtNoIns = new List<string>();
                int iFila = 0;
                int iNroCabeceras = 0;
                int iNroBarrasNoIns = 0;
                int iAux = 0;
                ArrayList arraylist = new ArrayList();
                decimal suma = 0;

                //
                foreach (DataColumn dtoCol in ds.Tables[0].Columns)
                {
                    iFila++;
                    if (iFila < 3)
                    {
                        continue;
                    }
                    //se guardan los nombres de las barras
                    listaCabecera[iNroCabeceras] = dtoCol.ToString().Trim();
                    iNroCabeceras++;
                }

                //validamos que las barras esten inscritas
                foreach (var barra in listaCabecera)
                {
                    CaiEquisddpuniDTO EntidadBarraNoIns = this.servicioCAI.GetByNombreEquipoSddp(barra.Trim());
                    if (EntidadBarraNoIns == null)
                    {
                        BarrasNoIns.Add(barra);
                    }
                    else
                    {
                        continue;
                    }
                }

                if (BarrasNoIns.Count() > 0)
                {
                    string BarrasFaltantes = string.Join(",", BarrasNoIns.ToArray());
                    model.sError = "Las siguientes unidades no estan inscritas: <b> " + BarrasFaltantes + "</b>";
                    return Json(model);
                }

                foreach (var barra in listaCabecera)
                {
                    //validamos las barras que se encuentre inscritas
                    CaiEquisddpuniDTO EntidadBarraNoIns = this.servicioCAI.GetByNombreEquipoSddp(barra.Trim());
                    CaiEquiunidbarrDTO EntidadBarrPtNoIns = this.servicioCAI.GetByIdEquicodiUNI(EntidadBarraNoIns.Equicodi);
                    if (EntidadBarrPtNoIns == null)
                    {
                        BarrasPtNoIns.Add(barra);
                    }
                    else
                    {
                        continue;
                    }
                }

                if (BarrasPtNoIns.Count() > 0)
                {
                    string barrasPtFaltantes = string.Join(",", BarrasPtNoIns.ToArray());
                    model.sError = "las siguientes unidades no estan asociadas a un Pt medicion : <b> " + barrasPtFaltantes + "</b>";
                    return Json(model);
                }

                //Lista Bulk
                List<CaiGenerdemanDTO> entitys = new List<CaiGenerdemanDTO>();
                ArrayList listavalores = new ArrayList();
                ArrayList ListFechas = new ArrayList();
                //recorrer columna por columna
                foreach (DataColumn dtCol in ds.Tables[0].Columns)
                {
                    bool bSalir = false;

                    foreach (DataRow dtRowfec in ds.Tables[0].Rows)
                    {
                        if (dtCol.ColumnName.Equals("Column1"))
                        {
                            //iFilaFec++;
                            //if (iFilaFec < 2)
                            //{
                            //    continue;
                            //}
                            string svalor = dtRowfec[dtCol].ToString();
                            ListFechas.Add(svalor);
                        }
                    }

                    foreach (DataRow dtRow in ds.Tables[0].Rows)
                    {
                        //en la columna 1 y 2 del excel no hay datos
                        if (dtCol.ColumnName.Equals("Column1") || dtCol.ColumnName.Equals("Column2"))
                        {
                            bSalir = true;
                            break;
                        }
                        else
                        {
                            //llena listavalores con los datos del excel
                            string svalor = dtRow[dtCol].ToString();
                            listavalores.Add(svalor);
                            bSalir = false;
                        }
                    }

                    if (!bSalir)
                    {

                        int cantidadve = 96;
                        ArrayList ListporDias = new ArrayList(cantidadve);
                        for (int c = 0; c < listavalores.Count; c += cantidadve)
                        {
                            var arraydia = new ArrayList();
                            arraydia.AddRange(listavalores.GetRange(c, cantidadve));
                            ListporDias.Add(arraydia);
                        }
                        int Fec = 0;
                        for (int c = 0; c < ListporDias.Count; c++)
                        {
                            //grabarDetalle
                            model.EntidadGenerDemanda = new CaiGenerdemanDTO();
                            model.EntidadGenerDemanda.Caiajcodi = caiajcodi;
                            model.EntidadGenerDemanda.Cagdcmfuentedat = tipo;
                            //se obtiene datos de la equivalencia
                            CaiEquisddpuniDTO EquivalenciaEquipo = new CaiEquisddpuniDTO();
                            EquivalenciaEquipo = this.servicioCAI.GetByNombreEquipoSddp(dtCol.ToString().Trim());//columna actual
                            CaiEquiunidbarrDTO EquivalenciaUnidad = new CaiEquiunidbarrDTO();
                            EquivalenciaUnidad = this.servicioCAI.GetByIdEquicodiUNI(EquivalenciaEquipo.Equicodi);
                            model.EntidadGenerDemanda.Ptomedicodi = EquivalenciaUnidad.Ptomedicodi;
                            model.EntidadGenerDemanda.Emprcodi = EquivalenciaUnidad.Emprcodi;
                            model.EntidadGenerDemanda.Equicodicen = EquivalenciaUnidad.Equicodicen;
                            model.EntidadGenerDemanda.Equicodiuni = EquivalenciaUnidad.Equicodiuni;
                            model.EntidadGenerDemanda.Cagdcmcalidadinfo = "S";
                            model.EntidadGenerDemanda.Cagdcmdia = DateTime.Parse(ListFechas[Fec].ToString());
                            Fec = Fec + 96;
                            arraylist = (ArrayList)ListporDias[c];
                            suma = 0;

                            try
                            {
                                suma += model.EntidadGenerDemanda.H1 = Decimal.Parse(arraylist[0].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H2 = Decimal.Parse(arraylist[1].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H3 = Decimal.Parse(arraylist[2].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H4 = Decimal.Parse(arraylist[3].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H5 = Decimal.Parse(arraylist[4].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H6 = Decimal.Parse(arraylist[5].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H7 = Decimal.Parse(arraylist[6].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H8 = Decimal.Parse(arraylist[7].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H9 = Decimal.Parse(arraylist[8].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H10 = Decimal.Parse(arraylist[9].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H11 = Decimal.Parse(arraylist[10].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H12 = Decimal.Parse(arraylist[11].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H13 = Decimal.Parse(arraylist[12].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H14 = Decimal.Parse(arraylist[13].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H15 = Decimal.Parse(arraylist[14].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H16 = Decimal.Parse(arraylist[15].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H17 = Decimal.Parse(arraylist[16].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H18 = Decimal.Parse(arraylist[17].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H19 = Decimal.Parse(arraylist[18].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H20 = Decimal.Parse(arraylist[19].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H21 = Decimal.Parse(arraylist[20].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H22 = Decimal.Parse(arraylist[21].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H23 = Decimal.Parse(arraylist[22].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H24 = Decimal.Parse(arraylist[23].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H25 = Decimal.Parse(arraylist[24].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H26 = Decimal.Parse(arraylist[25].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H27 = Decimal.Parse(arraylist[26].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H28 = Decimal.Parse(arraylist[27].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H29 = Decimal.Parse(arraylist[28].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H30 = Decimal.Parse(arraylist[29].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H31 = Decimal.Parse(arraylist[30].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H32 = Decimal.Parse(arraylist[31].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H33 = Decimal.Parse(arraylist[32].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H34 = Decimal.Parse(arraylist[33].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H35 = Decimal.Parse(arraylist[34].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H36 = Decimal.Parse(arraylist[35].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H37 = Decimal.Parse(arraylist[36].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H38 = Decimal.Parse(arraylist[37].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H39 = Decimal.Parse(arraylist[38].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H40 = Decimal.Parse(arraylist[39].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H41 = Decimal.Parse(arraylist[40].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H42 = Decimal.Parse(arraylist[41].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H43 = Decimal.Parse(arraylist[42].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H44 = Decimal.Parse(arraylist[43].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H45 = Decimal.Parse(arraylist[44].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H46 = Decimal.Parse(arraylist[45].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H47 = Decimal.Parse(arraylist[46].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H48 = Decimal.Parse(arraylist[47].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H49 = Decimal.Parse(arraylist[48].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H50 = Decimal.Parse(arraylist[49].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H51 = Decimal.Parse(arraylist[50].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H52 = Decimal.Parse(arraylist[51].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H53 = Decimal.Parse(arraylist[52].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H54 = Decimal.Parse(arraylist[53].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H55 = Decimal.Parse(arraylist[54].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H56 = Decimal.Parse(arraylist[55].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H57 = Decimal.Parse(arraylist[56].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H58 = Decimal.Parse(arraylist[57].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H59 = Decimal.Parse(arraylist[58].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H60 = Decimal.Parse(arraylist[59].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H61 = Decimal.Parse(arraylist[60].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H62 = Decimal.Parse(arraylist[61].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H63 = Decimal.Parse(arraylist[62].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H64 = Decimal.Parse(arraylist[63].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H65 = Decimal.Parse(arraylist[64].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H66 = Decimal.Parse(arraylist[65].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H67 = Decimal.Parse(arraylist[66].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H68 = Decimal.Parse(arraylist[67].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H69 = Decimal.Parse(arraylist[68].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H70 = Decimal.Parse(arraylist[69].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H71 = Decimal.Parse(arraylist[70].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H72 = Decimal.Parse(arraylist[71].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H73 = Decimal.Parse(arraylist[72].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H74 = Decimal.Parse(arraylist[73].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H75 = Decimal.Parse(arraylist[74].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H76 = Decimal.Parse(arraylist[75].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H77 = Decimal.Parse(arraylist[76].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H78 = Decimal.Parse(arraylist[77].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H79 = Decimal.Parse(arraylist[78].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H80 = Decimal.Parse(arraylist[79].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H81 = Decimal.Parse(arraylist[80].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H82 = Decimal.Parse(arraylist[81].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H83 = Decimal.Parse(arraylist[82].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H84 = Decimal.Parse(arraylist[83].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H85 = Decimal.Parse(arraylist[84].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H86 = Decimal.Parse(arraylist[85].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H87 = Decimal.Parse(arraylist[86].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H88 = Decimal.Parse(arraylist[87].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H89 = Decimal.Parse(arraylist[88].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H90 = Decimal.Parse(arraylist[89].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H91 = Decimal.Parse(arraylist[90].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H92 = Decimal.Parse(arraylist[91].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H93 = Decimal.Parse(arraylist[92].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H94 = Decimal.Parse(arraylist[93].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H95 = Decimal.Parse(arraylist[94].ToString(), System.Globalization.NumberStyles.Float);
                                suma += model.EntidadGenerDemanda.H96 = Decimal.Parse(arraylist[95].ToString(), System.Globalization.NumberStyles.Float);
                                model.EntidadGenerDemanda.Cagdcmtotaldia = suma;
                                //Tipo de informacion por intervalo en este caso P de Proyectado
                                model.EntidadGenerDemanda.T1 = "P";
                                model.EntidadGenerDemanda.T2 = "P";
                                model.EntidadGenerDemanda.T3 = "P";
                                model.EntidadGenerDemanda.T4 = "P";
                                model.EntidadGenerDemanda.T5 = "P";
                                model.EntidadGenerDemanda.T6 = "P";
                                model.EntidadGenerDemanda.T7 = "P";
                                model.EntidadGenerDemanda.T8 = "P";
                                model.EntidadGenerDemanda.T9 = "P";
                                model.EntidadGenerDemanda.T10 = "P";
                                model.EntidadGenerDemanda.T11 = "P";
                                model.EntidadGenerDemanda.T12 = "P";
                                model.EntidadGenerDemanda.T13 = "P";
                                model.EntidadGenerDemanda.T14 = "P";
                                model.EntidadGenerDemanda.T15 = "P";
                                model.EntidadGenerDemanda.T16 = "P";
                                model.EntidadGenerDemanda.T17 = "P";
                                model.EntidadGenerDemanda.T18 = "P";
                                model.EntidadGenerDemanda.T19 = "P";
                                model.EntidadGenerDemanda.T20 = "P";
                                model.EntidadGenerDemanda.T21 = "P";
                                model.EntidadGenerDemanda.T22 = "P";
                                model.EntidadGenerDemanda.T23 = "P";
                                model.EntidadGenerDemanda.T24 = "P";
                                model.EntidadGenerDemanda.T25 = "P";
                                model.EntidadGenerDemanda.T26 = "P";
                                model.EntidadGenerDemanda.T27 = "P";
                                model.EntidadGenerDemanda.T28 = "P";
                                model.EntidadGenerDemanda.T29 = "P";
                                model.EntidadGenerDemanda.T30 = "P";
                                model.EntidadGenerDemanda.T31 = "P";
                                model.EntidadGenerDemanda.T32 = "P";
                                model.EntidadGenerDemanda.T33 = "P";
                                model.EntidadGenerDemanda.T34 = "P";
                                model.EntidadGenerDemanda.T35 = "P";
                                model.EntidadGenerDemanda.T36 = "P";
                                model.EntidadGenerDemanda.T37 = "P";
                                model.EntidadGenerDemanda.T38 = "P";
                                model.EntidadGenerDemanda.T39 = "P";
                                model.EntidadGenerDemanda.T40 = "P";
                                model.EntidadGenerDemanda.T41 = "P";
                                model.EntidadGenerDemanda.T42 = "P";
                                model.EntidadGenerDemanda.T43 = "P";
                                model.EntidadGenerDemanda.T44 = "P";
                                model.EntidadGenerDemanda.T45 = "P";
                                model.EntidadGenerDemanda.T46 = "P";
                                model.EntidadGenerDemanda.T47 = "P";
                                model.EntidadGenerDemanda.T48 = "P";
                                model.EntidadGenerDemanda.T49 = "P";
                                model.EntidadGenerDemanda.T50 = "P";
                                model.EntidadGenerDemanda.T51 = "P";
                                model.EntidadGenerDemanda.T52 = "P";
                                model.EntidadGenerDemanda.T53 = "P";
                                model.EntidadGenerDemanda.T54 = "P";
                                model.EntidadGenerDemanda.T55 = "P";
                                model.EntidadGenerDemanda.T56 = "P";
                                model.EntidadGenerDemanda.T57 = "P";
                                model.EntidadGenerDemanda.T58 = "P";
                                model.EntidadGenerDemanda.T59 = "P";
                                model.EntidadGenerDemanda.T60 = "P";
                                model.EntidadGenerDemanda.T61 = "P";
                                model.EntidadGenerDemanda.T62 = "P";
                                model.EntidadGenerDemanda.T63 = "P";
                                model.EntidadGenerDemanda.T64 = "P";
                                model.EntidadGenerDemanda.T65 = "P";
                                model.EntidadGenerDemanda.T66 = "P";
                                model.EntidadGenerDemanda.T67 = "P";
                                model.EntidadGenerDemanda.T68 = "P";
                                model.EntidadGenerDemanda.T69 = "P";
                                model.EntidadGenerDemanda.T70 = "P";
                                model.EntidadGenerDemanda.T71 = "P";
                                model.EntidadGenerDemanda.T72 = "P";
                                model.EntidadGenerDemanda.T73 = "P";
                                model.EntidadGenerDemanda.T74 = "P";
                                model.EntidadGenerDemanda.T75 = "P";
                                model.EntidadGenerDemanda.T76 = "P";
                                model.EntidadGenerDemanda.T77 = "P";
                                model.EntidadGenerDemanda.T78 = "P";
                                model.EntidadGenerDemanda.T79 = "P";
                                model.EntidadGenerDemanda.T80 = "P";
                                model.EntidadGenerDemanda.T81 = "P";
                                model.EntidadGenerDemanda.T82 = "P";
                                model.EntidadGenerDemanda.T83 = "P";
                                model.EntidadGenerDemanda.T84 = "P";
                                model.EntidadGenerDemanda.T85 = "P";
                                model.EntidadGenerDemanda.T86 = "P";
                                model.EntidadGenerDemanda.T87 = "P";
                                model.EntidadGenerDemanda.T88 = "P";
                                model.EntidadGenerDemanda.T89 = "P";
                                model.EntidadGenerDemanda.T90 = "P";
                                model.EntidadGenerDemanda.T91 = "P";
                                model.EntidadGenerDemanda.T92 = "P";
                                model.EntidadGenerDemanda.T93 = "P";
                                model.EntidadGenerDemanda.T94 = "P";
                                model.EntidadGenerDemanda.T95 = "P";
                                model.EntidadGenerDemanda.T96 = "P";
                                model.EntidadGenerDemanda.Cagdcmusucreacion = User.Identity.Name;
                                model.EntidadGenerDemanda.Cagdcmfeccreacion = DateTime.Now;
                                model.EntidadGenerDemanda.Cagdcmcodi = iCaiGenerdemancodi;
                                entitys.Add(model.EntidadGenerDemanda);
                                iCaiGenerdemancodi++;
                                iAux++;

                                if (iAux == 3000)
                                {
                                    Log.Info("Bulk Insert - BulkInsertCaiGenerdeman");
                                    new CalculoPorcentajesAppServicio().BulkInsertCaiGenerdeman(entitys);
                                    entitys = new List<CaiGenerdemanDTO>();
                                    iAux = 0;
                                }
                            }
                            catch
                            {
                                return Json("Lo sentimos, se ha producido un error en la lectura de información del código: ");
                            }
                        }
                        listavalores.Clear();
                    }
                }

                if (iAux > 0)
                {

                    Log.Info("Bulk Insert - BulkInsertCaiGenerdeman");
                    new CalculoPorcentajesAppServicio().BulkInsertCaiGenerdeman(entitys);

                }

                model.sMensaje = "Felicidades, la carga de información de Energia fue exitosa , Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        /// <summary>
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
