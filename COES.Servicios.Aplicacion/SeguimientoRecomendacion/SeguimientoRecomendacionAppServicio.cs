using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.SeguimientoRecomendacion.Helper;
using System.Linq;
using COES.Servicios.Aplicacion.Correo;
//using System.Web.Mvc;
using System.Web;
using COES.Servicios.Aplicacion.Auditoria;
using COES.Dominio.DTO.Transferencias;
using System.Configuration;
//using System.Web.Mvc;


namespace COES.Servicios.Aplicacion.Recomendacion
{
    /// <summary>
    /// Clases con métodos del módulo SeguimientoRecomendacion
    /// </summary>
    public class SeguimientoRecomendacionAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SeguimientoRecomendacionAppServicio));
        CorreoAppServicio servCorreo = new CorreoAppServicio();
        AuditoriaAppServicio servAuditoria = new AuditoriaAppServicio(); //aplicativo Seg. Recomendaciones

        #region Métodos Tabla SRM_RECOMENDACION

        /// <summary>
        /// Inserta un registro de la tabla SRM_RECOMENDACION
        /// </summary>
        public void SaveSrmRecomendacion(SrmRecomendacionDTO entity)
        {
            try
            {
                FactorySic.GetSrmRecomendacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SRM_RECOMENDACION
        /// </summary>
        public void UpdateSrmRecomendacion(SrmRecomendacionDTO entity)
        {
            try
            {
                FactorySic.GetSrmRecomendacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SRM_RECOMENDACION
        /// </summary>
        public void DeleteSrmRecomendacion(int srmreccodi)
        {
            try
            {
                FactorySic.GetSrmRecomendacionRepository().Delete(srmreccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SRM_RECOMENDACION
        /// </summary>
        public SrmRecomendacionDTO GetByIdSrmRecomendacion(int srmreccodi)
        {
            return FactorySic.GetSrmRecomendacionRepository().GetById(srmreccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SRM_RECOMENDACION
        /// </summary>
        public List<SrmRecomendacionDTO> ListSrmRecomendacions()
        {
            return FactorySic.GetSrmRecomendacionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SrmRecomendacion
        /// </summary>
        public List<SrmRecomendacionDTO> GetByCriteriaSrmRecomendacions()
        {
            return FactorySic.GetSrmRecomendacionRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla SRM_RECOMENDACION
        /// </summary>
        public int SaveSrmRecomendacionId(SrmRecomendacionDTO entity)
        {
            return FactorySic.GetSrmRecomendacionRepository().SaveSrmRecomendacionId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperaciones(int evenCodi,int equiCodi,int srmcrtcodi,int srmstdcodi,int usercode,DateTime srmrecFecharecomend,DateTime srmrecFechavencim,int nroPage, int pageSize)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperaciones(evenCodi,equiCodi,srmcrtcodi,srmstdcodi,usercode,srmrecFecharecomend,srmrecFechavencim, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SRM_RECOMENDACION
        /// </summary>
        public int ObtenerNroFilas(int evenCodi,int equiCodi,int srmcrtcodi,int srmstdcodi,int usercode,DateTime srmrecFecharecomend,DateTime srmrecFechavencim)
        {
            return FactorySic.GetSrmRecomendacionRepository().ObtenerNroFilas(evenCodi,equiCodi,srmcrtcodi,srmstdcodi,usercode,srmrecFecharecomend,srmrecFechavencim);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION gestión
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperaciones(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, string detRecomend, int estado,
            int criticidad, int nroPage, int pageSize)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperaciones(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, detRecomend, estado,
             criticidad, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SRM_RECOMENDACION gestion
        /// </summary>
        public int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, string detRecomend, int estado,
            int criticidad)
        {
            return FactorySic.GetSrmRecomendacionRepository().ObtenerNroFilas(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, detRecomend, estado,
             criticidad);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION gestión faltantes
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperaciones(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int nroPage, int pageSize)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperaciones(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SRM_RECOMENDACION faltANTA
        /// </summary>
        public int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi)
        {
            return FactorySic.GetSrmRecomendacionRepository().ObtenerNroFilas(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION por evento
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesRec(int evenCodi, string activo, int nroPage, int pageSize)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperaciones(evenCodi, activo, nroPage, pageSize);
        }

        /// <summary>
        /// Cuenta las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION por evento
        /// </summary>
        public int ObtenerNroFilasRec(int evenCodi, string activo)
        {
            return FactorySic.GetSrmRecomendacionRepository().ObtenerNroFilas(evenCodi, activo);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION reporte listado
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesReporteListado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode, int nroPage, int pageSize)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesReporteListado(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, estado,
             criticidad, recomendacion, usercode,nroPage, pageSize);
        }

        /// <summary>
        /// Cuenta las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION reporte listado
        /// </summary>
        public int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode)
        {
            return FactorySic.GetSrmRecomendacionRepository().ObtenerNroFilas(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, estado,
 criticidad, recomendacion, usercode);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION. Empresa por Criticidad
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesEmpresaCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesEmpresaCriticidad(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, estado,
             criticidad, recomendacion, usercode);
        }

        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION. Empresa por Estado
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesEmpresaEstado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesEmpresaEstado(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, estado,
             criticidad, recomendacion, usercode);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION. Tipo de Equipo por Criticidad
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesTipoEquipoCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesTipoEquipoCriticidad(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, estado,
             criticidad, recomendacion, usercode);
        }

        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION. Tipo de Equipo por Estado
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesTipoEquipoEstado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesTipoEquipoEstado(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, estado,
             criticidad, recomendacion, usercode);
        }

        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION. Por Estado
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesEstado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesEstado(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, estado,
             criticidad, recomendacion, usercode);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION. Estado y Criticidad
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesEstadoCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesEstadoCriticidad(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, estado,
             criticidad, recomendacion, usercode);
        }

        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_RECOMENDACION. Por Criticidad
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesCriticidad(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, estado,
             criticidad, recomendacion, usercode);
        }

        /// <summary>
        /// Busca las operaciones para envío de alarmas
        /// </summary>
        /// <param name="fecha">fecha</param>
        /// <param name="reporteDiaVencimiento">día luego del vencimiento</param>
        /// <param name="repeticionAlarma">repetición de alarma</param>
        /// <returns></returns>
        public List<SrmRecomendacionDTO> BuscarOperacionesAlarma(DateTime fecha, int reporteDiaVencimiento, int repeticionAlarma)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesAlarma(fecha, reporteDiaVencimiento, repeticionAlarma);
        }

        /// <summary>
        /// Permite relizar el envío de alarmas (correo recordatorio)
        /// </summary>        
        public void EnvioAlarma()
        {
            DateTime fecha = DateTime.Now;

            //DateTime fecha = Convert.ToDateTime("2019-06-26");

            List<SrmRecomendacionDTO> lista = BuscarOperacionesAlarma(fecha,
                ConstantesSeguimientoRecomendacion.ReporteDiaVencimiento, ConstantesSeguimientoRecomendacion.RepeticionAlarma);

            string listaFrom = "webapp@coes.org.pe";
            string listaTo = ConfigurationManager.AppSettings["RecomendacionesMailTo"];

            //a. Días previos a la notificación de plazo
            List<SrmRecomendacionDTO> listaAvencer = lista.Where(x => x.Avencer == 1).ToList();
            EnvioAlarmaAvencer(listaAvencer, ConstantesSeguimientoRecomendacion.PlantillaAlarmaAvencer, listaFrom, listaTo);



            //b. Al Día siguiente del vencimiento
            List<SrmRecomendacionDTO> listaVencido = lista.Where(x => x.Vencido == 1).ToList();
            EnvioAlarmaAlDiaSgteVencimiento(listaVencido, ConstantesSeguimientoRecomendacion.PlantillaAlarmaVencido, listaFrom, listaTo);

            //c. Venció hace varios días (recordatorio cada n días)
            List<SrmRecomendacionDTO> listaCiclico = lista.Where(x => x.Ciclico == 1).ToList();
            EnvioAlarmaCiclico(listaCiclico, ConstantesSeguimientoRecomendacion.PlantillaAlarmaCiclico, listaFrom, listaTo);
                                    
        }



        /// <summary>
        /// Permite enviar las alarmas próximas a vencer
        /// </summary>
        /// <param name="lista"></param>
        public void EnvioAlarmaAvencer(List<SrmRecomendacionDTO> lista, int plantcodi,string listaFrom, string listaTo)
        {

            var plantilla = new SiPlantillacorreoDTO();
            string from = String.Empty, to = String.Empty, cc = String.Empty, bcc = String.Empty;
            string asunto = String.Empty;
            string contenido = String.Empty;


            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

            ObtenerCampoCorreo(listaFrom, out from,
                                    listaTo, out to, plantilla.PlanticorreosCc, out cc,
                                    plantilla.PlanticorreosBcc, out bcc);


            foreach (SrmRecomendacionDTO item in lista)
            {
                try
                {
                    string[] parametroTitulo = new string[3];
                    string[] parametroContenido = new string[7];

                    parametroTitulo[0] = item.Equiabrev;
                    parametroTitulo[1] = ((DateTime)(item.Srmrecfechavencim)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroTitulo[2] = item.Srmcrtdescrip;

                    parametroContenido[0] = item.Emprnomb;
                    parametroContenido[1] = item.Areanomb;
                    parametroContenido[2] = item.Equiabrev;
                    parametroContenido[3] = item.Srmrectitulo;
                    parametroContenido[4] = item.Srmrecrecomendacion;
                    parametroContenido[5] = ((DateTime)(item.Srmrecfechavencim)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroContenido[6] = item.Srmcrtdescrip;

                    asunto = plantilla.Plantasunto;
                    asunto = String.Format(asunto, parametroTitulo[0], parametroTitulo[1], parametroTitulo[2]);

                    contenido = plantilla.Plantcontenido;
                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                        parametroContenido[2], parametroContenido[3], parametroContenido[4], parametroContenido[5], parametroContenido[6]);


                    //FormatoCorreoModel
                    FormatoCorreoModel model = new FormatoCorreoModel();
                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                    EnviarCorreo(model);
                }
                catch
                {
                }
            }
        }


        /// <summary>
        /// Permite asociar los campos de correo
        /// </summary>
        /// <param name="formato">formato</param>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        /// <param name="cc">cc</param>
        /// <param name="bcc">bcc</param>
        /// <param name="asunto">asunto</param>
        /// <param name="contenido">contenido</param>
        /// <param name="plantcodi">código de plantilla</param>
        public void AsociarCamposCorreo(ref FormatoCorreoModel formato, string from, string to, string cc, string bcc, string asunto, string contenido, int plantcodi)
        {
            formato.From = from;
            formato.To = to;
            formato.CC = cc;
            formato.BCC = bcc;

            formato.Asunto = asunto;
            formato.Contenido = contenido;
            formato.Plantcodi = plantcodi;
        }

        /// <summary>
        /// Permite enviar un formato de correo actualizado
        /// </summary>
        /// <param name="model">Formato de correo</param>
        /// <returns>1: envío satisfactorio. -1: error</returns>
        public int EnviarCorreo(FormatoCorreoModel model)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesSeguimientoRecomendacion.RutaCorreo;
                List<string> listFiles = new List<string>();
                string files = model.Archivo;

                if (!string.IsNullOrEmpty(files))
                    listFiles = files.Split('/').ToList();

                CorreoAppServicio servCorreo = new CorreoAppServicio();
                servCorreo.EnviarCorreo(model.From, model.To, model.CC, model.BCC, model.Asunto, model.Contenido,
                    model.Plantcodi, path, listFiles);

                return 1;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite enviar las alarmas al día siguiente del vencimiento
        /// </summary>
        /// <param name="lista"></param>
        public void EnvioAlarmaAlDiaSgteVencimiento(List<SrmRecomendacionDTO> lista, int plantcodi, string listaFrom, string listaTo)
        {

            var plantilla = new SiPlantillacorreoDTO();
            string from = String.Empty, to = String.Empty, cc = String.Empty, bcc = String.Empty;
            string asunto = String.Empty;
            string contenido = String.Empty;


            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

            ObtenerCampoCorreo(listaFrom, out from,
                                    listaTo, out to, plantilla.PlanticorreosCc, out cc,
                                    plantilla.PlanticorreosBcc, out bcc);


            foreach (SrmRecomendacionDTO item in lista)
            {
                try
                {
                    string[] parametroTitulo = new string[3];
                    string[] parametroContenido = new string[8];

                    parametroTitulo[0] = item.Equiabrev;
                    parametroTitulo[1] = ((DateTime)(item.Srmrecfechavencim)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroTitulo[2] = item.Srmcrtdescrip;

                    parametroContenido[0] = ((DateTime)(item.Srmrecfechavencim)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroContenido[1] = item.Emprnomb;
                    parametroContenido[2] = item.Areanomb;
                    parametroContenido[3] = item.Equiabrev;
                    parametroContenido[4] = item.Srmrectitulo;
                    parametroContenido[5] = item.Srmrecrecomendacion;
                    parametroContenido[6] = ((DateTime)(item.Srmrecfechavencim)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroContenido[7] = item.Srmcrtdescrip;

                    asunto = plantilla.Plantasunto;
                    asunto = String.Format(asunto, parametroTitulo[0], parametroTitulo[1], parametroTitulo[2]);

                    contenido = plantilla.Plantcontenido;
                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                        parametroContenido[2], parametroContenido[3], parametroContenido[4], parametroContenido[5], parametroContenido[6], parametroContenido[7]);


                    //FormatoCorreoModel
                    FormatoCorreoModel model = new FormatoCorreoModel();
                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                    EnviarCorreo(model);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Permite enviar las alarmas de manera cíclica luego del vecimiento
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="plantcodi"></param>
        public void EnvioAlarmaCiclico(List<SrmRecomendacionDTO> lista, int plantcodi, string listaFrom, string listaTo)
        {

            var plantilla = new SiPlantillacorreoDTO();
            string from = String.Empty, to = String.Empty, cc = String.Empty, bcc = String.Empty;
            string asunto = String.Empty;
            string contenido = String.Empty;


            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

            ObtenerCampoCorreo(listaFrom, out from,
                                    listaTo, out to, plantilla.PlanticorreosCc, out cc,
                                    plantilla.PlanticorreosBcc, out bcc);


            foreach (SrmRecomendacionDTO item in lista)
            {
                try
                {
                    string[] parametroTitulo = new string[3];
                    string[] parametroContenido = new string[8];

                    parametroTitulo[0] = item.Equiabrev;
                    parametroTitulo[1] = ((DateTime)(item.Srmrecfechavencim)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroTitulo[2] = item.Srmcrtdescrip;

                    parametroContenido[0] = ((DateTime)(item.Srmrecfechavencim)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroContenido[1] = item.Emprnomb;
                    parametroContenido[2] = item.Areanomb;
                    parametroContenido[3] = item.Equiabrev;
                    parametroContenido[4] = item.Srmrectitulo;
                    parametroContenido[5] = item.Srmrecrecomendacion;
                    parametroContenido[6] = ((DateTime)(item.Srmrecfechavencim)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroContenido[7] = item.Srmcrtdescrip;

                    asunto = plantilla.Plantasunto;
                    asunto = String.Format(asunto, parametroTitulo[0], parametroTitulo[1], parametroTitulo[2]);

                    contenido = plantilla.Plantcontenido;
                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                        parametroContenido[2], parametroContenido[3], parametroContenido[4], parametroContenido[5], parametroContenido[6], parametroContenido[7]);


                    //FormatoCorreoModel
                    FormatoCorreoModel model = new FormatoCorreoModel();
                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                    EnviarCorreo(model);
                }
                catch
                {
                }
            }
        }

        private void ObtenerCampoCorreo(string fromValor, out string fromCampo, string toValor, out string toCampo,
string ccValor, out string ccCampo, string bccValor, out string bccCampo)
        {

            fromCampo = (fromValor != null ? fromValor : "");
            toCampo = (toValor != null ? toValor : "");
            ccCampo = (ccValor != null ? ccValor : "");
            bccCampo = (bccValor != null ? bccValor : "");
        }

        /// <summary>
        /// Valida si existe la recomendación ctaf en módulo de recomendaciones
        /// </summary>
        public bool VerificarExisteRecomendacionCtaf(int Afrrec)
        {
            int? contRecomendación = null;
            contRecomendación = FactorySic.GetSrmRecomendacionRepository().ObtenerNroRecomendacionCtaf(Afrrec);
            if (contRecomendación > 0)
            {
                return true;
            }
            return false;          
        }
        /// <summary>
        /// Valida si existe la recomendación ctaf en módulo de recomendaciones x evento, equipo, criticidad y estado
        /// </summary>
        public bool VerificarExisteRecomendacionxEvento(int Evencodi, int Equicodi, int Srmcrtcodi, int Srmstdcodi)
        {
            int? contRecomendación = null;
            contRecomendación = FactorySic.GetSrmRecomendacionRepository().ObtenerRecomendacionEvento(Evencodi, Equicodi, Srmcrtcodi, Srmstdcodi);
            if (contRecomendación > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Busca las operaciones ctaf de acuerdo a filtro de la tabla SRM_RECOMENDACION gestión faltantes
        /// </summary>
        public List<SrmRecomendacionDTO> BuscarOperacionesCtaf(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int nroPage, int pageSize)
        {
            return FactorySic.GetSrmRecomendacionRepository().BuscarOperacionesCtaf(fechaInicio, fechaFin, famcodi, equiAbrev, tipoEmpresa, emprcodi, nroPage, pageSize);
        }

        /// <summary>
        /// Lista recomendaciones de eventos ctaf
        /// </summary>
        public List<SrmRecomendacionDTO> ListadoRecomendacionesEventosCtaf(int evencodi)
        {
            return FactorySic.GetSrmRecomendacionRepository().ListadoRecomendacionesEventosCtaf(evencodi);
        }

        /// <summary>
        /// Permite enviar las alarmas al día siguiente del vencimiento
        /// </summary>
        /// <param name="recomendacion"></param>
        /// <param name="plantcodi"></param>
        /// <param name="listaFrom"></param>
        /// <param name="listaTo"></param>
        public void EnvioCorreoRecomendacionCTAF(SrmRecomendacionDTO recomendacion, int plantcodi, string listaFrom, string listaTo)
        {

            var plantilla = new SiPlantillacorreoDTO();
            string from = String.Empty, to = String.Empty, cc = String.Empty, bcc = String.Empty;
            string asunto = String.Empty;
            string contenido = String.Empty;


            plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

            ObtenerCampoCorreo(listaFrom, out from,
                                    plantilla.Planticorreos, out to, plantilla.PlanticorreosCc, out cc,
                                    plantilla.PlanticorreosBcc, out bcc);

                try
                {
                    string[] parametroTitulo = new string[3];
                    string[] parametroContenido = new string[8];

                    parametroTitulo[0] = recomendacion.Equiabrev;
                    parametroTitulo[1] = ((DateTime)(recomendacion.Srmrecfecharecomend)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroTitulo[2] = recomendacion.Srmcrtdescrip;

                    parametroContenido[0] = ((DateTime)(recomendacion.Srmrecfecharecomend)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroContenido[1] = recomendacion.Emprnomb; //Empresa
                    parametroContenido[2] = recomendacion.Areanomb; //Subestación
                    parametroContenido[3] = recomendacion.Equiabrev;//Equipo
                    parametroContenido[4] = recomendacion.Srmrectitulo;
                    parametroContenido[5] = recomendacion.Srmrecrecomendacion;
                    parametroContenido[6] = ((DateTime)(recomendacion.Srmrecfechavencim)).ToString(ConstantesSeguimientoRecomendacion.FormatoFecha);
                    parametroContenido[7] = recomendacion.Srmcrtdescrip;

                    asunto = plantilla.Plantasunto;
                    asunto = String.Format(asunto, parametroTitulo[0], parametroTitulo[1], parametroTitulo[2]);

                    contenido = plantilla.Plantcontenido;
                    contenido = String.Format(contenido, parametroContenido[0], parametroContenido[1],
                        parametroContenido[2], parametroContenido[3], parametroContenido[4], parametroContenido[5], parametroContenido[6], parametroContenido[7]);


                    //FormatoCorreoModel
                    FormatoCorreoModel model = new FormatoCorreoModel();
                    AsociarCamposCorreo(ref model, from, to, cc, bcc, asunto, contenido, plantcodi);
                    EnviarCorreo(model);
                }
                catch
                {
                }
            
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SRM_RECOMENDACION
        /// </summary>
        public SrmRecomendacionDTO GetByIdSrmRecomendacioncxAfrrec(int afrrec)
        {
            return FactorySic.GetSrmRecomendacionRepository().GetByIdxAfrrec(afrrec);
        }

        /// <summary>
        /// Valida si el evento tiene recomendaciones de acuerdo a un estado
        /// </summary>
        public int ValidaRecomendacionxEventoEstado(int evenCodi, int srmstdcodi)
        {
            return FactorySic.GetSrmRecomendacionRepository().ValidaRecomendacionxEventoEstado(evenCodi, srmstdcodi);
        }

        #endregion

        #region Métodos Tabla SRM_COMENTARIO

        /// <summary>
        /// Inserta un registro de la tabla SRM_COMENTARIO
        /// </summary>
        public void SaveSrmComentario(SrmComentarioDTO entity)
        {
            try
            {
                FactorySic.GetSrmComentarioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SRM_COMENTARIO
        /// </summary>
        public void UpdateSrmComentario(SrmComentarioDTO entity)
        {
            try
            {
                FactorySic.GetSrmComentarioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SRM_COMENTARIO
        /// </summary>
        public void DeleteSrmComentario(int srmcomcodi)
        {
            try
            {
                FactorySic.GetSrmComentarioRepository().Delete(srmcomcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SRM_COMENTARIO
        /// </summary>
        public SrmComentarioDTO GetByIdSrmComentario(int srmcomcodi)
        {
            return FactorySic.GetSrmComentarioRepository().GetById(srmcomcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SRM_COMENTARIO
        /// </summary>
        public List<SrmComentarioDTO> ListSrmComentarios()
        {
            return FactorySic.GetSrmComentarioRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SrmComentario
        /// </summary>
        public List<SrmComentarioDTO> GetByCriteriaSrmComentarios()
        {
            return FactorySic.GetSrmComentarioRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla SRM_COMENTARIO
        /// </summary>
        public int SaveSrmComentarioId(SrmComentarioDTO entity)
        {
            return FactorySic.GetSrmComentarioRepository().SaveSrmComentarioId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_COMENTARIO
        /// </summary>
        public List<SrmComentarioDTO> BuscarOperaciones(int srmreccodi, string activo, int nroPage, int pageSize)
        {
            return FactorySic.GetSrmComentarioRepository().BuscarOperaciones(srmreccodi, activo, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SRM_COMENTARIO
        /// </summary>
        public int ObtenerNroFilas(int srmreccodi, string activo)
        {
            return FactorySic.GetSrmComentarioRepository().ObtenerNroFilas(srmreccodi, activo);
        }

        #endregion

        #region Métodos Tabla SRM_CRITICIDAD

        /// <summary>
        /// Inserta un registro de la tabla SRM_CRITICIDAD
        /// </summary>
        public void SaveSrmCriticidad(SrmCriticidadDTO entity)
        {
            try
            {
                FactorySic.GetSrmCriticidadRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SRM_CRITICIDAD
        /// </summary>
        public void UpdateSrmCriticidad(SrmCriticidadDTO entity)
        {
            try
            {
                FactorySic.GetSrmCriticidadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SRM_CRITICIDAD
        /// </summary>
        public void DeleteSrmCriticidad(int srmcrtcodi)
        {
            try
            {
                FactorySic.GetSrmCriticidadRepository().Delete(srmcrtcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SRM_CRITICIDAD
        /// </summary>
        public SrmCriticidadDTO GetByIdSrmCriticidad(int srmcrtcodi)
        {
            return FactorySic.GetSrmCriticidadRepository().GetById(srmcrtcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SRM_CRITICIDAD
        /// </summary>
        public List<SrmCriticidadDTO> ListSrmCriticidads()
        {
            return FactorySic.GetSrmCriticidadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SrmCriticidad
        /// </summary>
        public List<SrmCriticidadDTO> GetByCriteriaSrmCriticidads()
        {
            return FactorySic.GetSrmCriticidadRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla SRM_CRITICIDAD
        /// </summary>
        public int SaveSrmCriticidadId(SrmCriticidadDTO entity)
        {
            return FactorySic.GetSrmCriticidadRepository().SaveSrmCriticidadId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_CRITICIDAD
        /// </summary>
        public List<SrmCriticidadDTO> BuscarOperaciones(DateTime srmcrtFeccreacion, DateTime srmcrtFecmodificacion, int nroPage, int pageSize)
        {
            return FactorySic.GetSrmCriticidadRepository().BuscarOperaciones(srmcrtFeccreacion, srmcrtFecmodificacion, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SRM_CRITICIDAD
        /// </summary>
        public int ObtenerNroFilas(DateTime srmcrtFeccreacion, DateTime srmcrtFecmodificacion)
        {
            return FactorySic.GetSrmCriticidadRepository().ObtenerNroFilas(srmcrtFeccreacion, srmcrtFecmodificacion);
        }

        #endregion
        
        #region Métodos Tabla SRM_ESTADO

        /// <summary>
        /// Inserta un registro de la tabla SRM_ESTADO
        /// </summary>
        public void SaveSrmEstado(SrmEstadoDTO entity)
        {
            try
            {
                FactorySic.GetSrmEstadoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SRM_ESTADO
        /// </summary>
        public void UpdateSrmEstado(SrmEstadoDTO entity)
        {
            try
            {
                FactorySic.GetSrmEstadoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SRM_ESTADO
        /// </summary>
        public void DeleteSrmEstado(int srmstdcodi)
        {
            try
            {
                FactorySic.GetSrmEstadoRepository().Delete(srmstdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SRM_ESTADO
        /// </summary>
        public SrmEstadoDTO GetByIdSrmEstado(int srmstdcodi)
        {
            return FactorySic.GetSrmEstadoRepository().GetById(srmstdcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SRM_ESTADO
        /// </summary>
        public List<SrmEstadoDTO> ListSrmEstados()
        {
            return FactorySic.GetSrmEstadoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SrmEstado
        /// </summary>
        public List<SrmEstadoDTO> GetByCriteriaSrmEstados()
        {
            return FactorySic.GetSrmEstadoRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla SRM_ESTADO
        /// </summary>
        public int SaveSrmEstadoId(SrmEstadoDTO entity)
        {
            return FactorySic.GetSrmEstadoRepository().SaveSrmEstadoId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SRM_ESTADO
        /// </summary>
        /*
        public List<SrmEstadoDTO> BuscarOperaciones(DateTime srmstdFeccreacion, DateTime srmstdFecmodificacion, int nroPage, int pageSize)
        {
            return FactorySic.GetSrmEstadoRepository().BuscarOperaciones(srmstdFeccreacion, srmstdFecmodificacion, nroPage, pageSize);
        }*/

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SRM_ESTADO
        /// </summary>
        /*
        public int ObtenerNroFilas(DateTime srmstdFeccreacion, DateTime srmstdFecmodificacion)
        {
            return FactorySic.GetSrmEstadoRepository().ObtenerNroFilas(srmstdFeccreacion, srmstdFecmodificacion);
        }*/

        #endregion

    }

        
    public class FormatoCorreoModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Asunto { get; set; }

        public string ruta { get; set; }

        //[AllowHtml]
        public string Contenido { get; set; }
        public int Plantcodi { get; set; }
        public string Archivo { get; set; }

        public string LinkArchivo { get; set; }
    }
    
}
