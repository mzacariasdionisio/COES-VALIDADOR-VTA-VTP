using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.Servicios.Aplicacion.Despacho;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Combustibles;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class PropiedadesController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
       
        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>        
        public ActionResult Lista()
        {
            PropiedadesModel model = new PropiedadesModel();            
            return View(model);
        }

        /// <summary>
        /// Permite obtener los datos de las propiedades
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerData(int famcodi)
        {
            PropiedadesModel model = new PropiedadesModel();

            List<EqRelacionDTO> listEquipo = new List<EqRelacionDTO>();
            List<EqRelacionDTO> listModo = new List<EqRelacionDTO>();

            int registrosTotal = 0;

            if (famcodi == ConstantesCortoPlazo.IdGeneradorHidro)
            {
                listEquipo = this.servicio.ObtenerPropiedadesConfiguracion(0);
                registrosTotal = (listEquipo.Count == 0 ? 1 : listEquipo.Count);
            }
            else
            {
                listModo = this.servicio.ObtenerPropiedadesConfiguracion(1);
                registrosTotal = (listModo.Count == 0 ? 1 : listModo.Count);
            }                                              

            string[][] data = new string[registrosTotal + 1][];
            data = CalculoHelper.PintarCeldas(data, 4);
            
            int indice = 0;

            if (famcodi == ConstantesCortoPlazo.IdGeneradorHidro)
            {
                foreach (var item in listEquipo)
                {
                    data[indice][0] = item.Equicodi.ToString();
                    data[indice][1] = item.Equinomb;
                    data[indice][2] = item.VelocidadCarga == null ? "" : item.VelocidadCarga.ToString();
                    data[indice][3] = item.VelocidadDescarga == null ? "" : item.VelocidadDescarga.ToString();
                  
                    indice++;
                }
            }
            else
            {
                foreach (var item in listModo)
                {
                    data[indice][0] = item.Equicodi.ToString();
                    data[indice][1] = item.Equinomb;
                    data[indice][2] = item.VelocidadCarga == null ? "" : item.VelocidadCarga.ToString();
                    data[indice][3] = item.VelocidadDescarga == null ? "" : item.VelocidadDescarga.ToString();

                    indice++;
                }
            }

            model.Datos = data;
            
            return new JsonResult { Data = model, MaxJsonLength = Int32.MaxValue };
        }

        /// <summary>
        /// Permite grabar los registros de datos
        /// </summary>
        /// <param name="famcodi"></param>
        /// <param name="dataExcel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(int famcodi, string dataExcel)
        {
            try
            {
                //obtener dator antiguos para ver si se graban
                List<EqRelacionDTO> listEquipo = new List<EqRelacionDTO>();
                List<EqRelacionDTO> listModo = new List<EqRelacionDTO>();

                if (famcodi == ConstantesCortoPlazo.IdGeneradorHidro)
                {
                    listEquipo = this.servicio.ObtenerPropiedadesConfiguracion(0);                
                }
                else
                {
                    listModo = this.servicio.ObtenerPropiedadesConfiguracion(1);                    
                }
                
                int nroColumnas = 4;
                List<string> celdas = new List<string>();
                celdas = dataExcel.Split(',').ToList();
                string[][] matriz = CalculoHelper.GetMatrizExcel(celdas, 0, nroColumnas, 0, celdas.Count / nroColumnas);
                int filaFinal = 0;
                int filas = (celdas.Count / nroColumnas);
                for (int i = 0; i < filas; i++)
                {
                    int conteoBlanco = 0;
                    for (int j = 0; j < nroColumnas; j++)
                    {
                        conteoBlanco += (matriz[i][j] == "" ? 1 : 0);
                    }

                    if (conteoBlanco == nroColumnas)
                        return Json(1);
                    else
                    {
                        if (famcodi == ConstantesCortoPlazo.IdGeneradorHidro)
                        {                            
                            int equicodi = Convert.ToInt32(matriz[i][0]);
                            double valorCarga = 0;
                            double valorDescarga = 0;

                            EqRelacionDTO equipo = listEquipo.Where(x => x.Equicodi == equicodi).FirstOrDefault();
                            
                            try
                            {
                                valorCarga = equipo.VelocidadCarga == null ? 0 : Convert.ToDouble(equipo.VelocidadCarga);
                            }
                            catch
                            {
                                valorCarga = 0;
                            }                           

                            try
                            {
                                valorDescarga = equipo.VelocidadDescarga == null ? 0 : Convert.ToDouble(equipo.VelocidadDescarga);
                            }
                            catch
                            {
                                valorDescarga = 0;
                            }

                            //carga
                            if (matriz[i][2] + "X" != "X")
                            {
                                double valorCargaNuevo = Convert.ToDouble(matriz[i][2]);

                                if (valorCargaNuevo != valorCarga)
                                {
                                    this.GrabarPropEqui(equicodi, ConstantesCortoPlazo.HidroVelocidadTomaCarga, valorCargaNuevo);
                                }
                            }
                            
                            //descarga
                            if (matriz[i][3] + "X" != "X")
                            {
                                double valorDescargaNuevo = Convert.ToDouble(matriz[i][3]);
                                
                                if (valorDescargaNuevo != valorDescarga)
                                {
                                    this.GrabarPropEqui(equicodi, ConstantesCortoPlazo.HidroVelocidadReduccionCarga, valorDescargaNuevo);
                                }
                            }        
                        }
                        else
                        {
                            int grupocodi = Convert.ToInt32(matriz[i][0]);
                            EqRelacionDTO grupo = listModo.Where(x => x.Equicodi == grupocodi).FirstOrDefault();
                            double valorCarga = 0;
                            double valorDescarga = 0;
                            
                            try
                            {
                                valorCarga = grupo.VelocidadCarga == null ? 0 : Convert.ToDouble(grupo.VelocidadCarga);
                            }
                            catch
                            {
                                valorCarga = 0;
                            }

                            try
                            {
                                valorDescarga = grupo.VelocidadDescarga == null ? 0 : Convert.ToDouble(grupo.VelocidadDescarga);
                            }
                            catch
                            {
                                valorDescarga = 0;
                            }

                            //carga
                            if (matriz[i][2] + "X" != "X")
                            {
                                double valorCargaNuevo = Convert.ToDouble(matriz[i][2]);
                                if (valorCargaNuevo != valorCarga)
                                {
                                    GrabarGrupodat(grupocodi, ConstantesCortoPlazo.TermoVelocidadTomaCarga, valorCargaNuevo);
                                }
                            }
                            
                            //descarga
                            if (matriz[i][3] + "X" != "X")
                            {
                                double valorDescargaNuevo = Convert.ToDouble(matriz[i][3]);
                                if (valorDescargaNuevo != valorDescarga)
                                {
                                    GrabarGrupodat(grupocodi, ConstantesCortoPlazo.TermoVelocidadReduccionCarga, valorDescargaNuevo);
                                }                                
                            }     
                        }
                    }

                    filaFinal++;
                }
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar las propiedades de equipos
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="propcodi"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        private int GrabarPropEqui(int equicodi, int propcodi, double valor)
        {
            try
            {
                EqPropequiDTO entity = new EqPropequiDTO();
                entity.Equicodi = equicodi;
                entity.Propcodi = propcodi;
                entity.Valor = valor+"";
                entity.Propequiusucreacion = base.UserName;
                entity.Propequifeccreacion = DateTime.Now; //fechas diferentes cada vez
                entity.Fechapropequi = DateTime.Today;

                //- grabar registro
                (new EquipamientoAppServicio()).SaveEqPropequi(entity);   
            }
            catch
            {
                return -1;
            }

            return 1;            
        }
        
        /// <summary>
        /// Permite grabar las propiedades de modos de operación
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="concepcodi"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        private int GrabarGrupodat(int grupocodi, int concepcodi, double valor)
        {
            try
            {
                PrGrupodatDTO entity = new PrGrupodatDTO();
                entity.Fechadat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                entity.Grupocodi = grupocodi;
                entity.Concepcodi = concepcodi;
                entity.Formuladat = valor + "";
                entity.Lastuser = base.UserName;
                entity.Fechaact = DateTime.Now;

                PrGrupodatDTO grupo = (new DespachoAppServicio()).GetByIdPrGrupodat((DateTime)entity.Fechadat, concepcodi, grupocodi, 0);

                if (grupo == null)
                {
                    (new DespachoAppServicio()).SavePrGrupodat(entity);
                }
                else
                {
                    (new DespachoAppServicio()).UpdatePrGrupodat(entity);
                }
            }
            catch
            {
                return -1;
            }

            return 1;
        }
    }
}
