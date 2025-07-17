using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using System.Linq;
using System.Configuration;

namespace COES.Servicios.Aplicacion.General
{
    /// <summary>
    /// Clases con métodos del módulo GENERAL
    /// </summary>
    public class GeneralAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GeneralAppServicio));

        /// <summary>
        /// Permite obtener un registro de la tabla SI_FUENTEENERGIA
        /// </summary>
        public SiFuenteenergiaDTO GetByIdSiFuenteenergia(int fenergcodi)
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetById(fenergcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_FUENTEENERGIA
        /// </summary>
        public List<SiFuenteenergiaDTO> ListSiFuenteenergias()
        {
            return FactorySic.GetSiFuenteenergiaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiFuenteenergia
        /// </summary>
        public List<SiFuenteenergiaDTO> GetByCriteriaSiFuenteenergias()
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetByCriteria();
        }

        public List<SiEmpresaDTO> ListarEmpresasPorTipoEmpresa(int iTipoEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().List(iTipoEmpresa);
        }

        public List<SiTipoempresaDTO> ListarTiposEmpresa()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
        }

        public List<SiEmpresaDTO> ListadoComboEmpresasPorTipo(int iTipoEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().ListadoComboEmpresasPorTipo(iTipoEmpresa);
        }


        /// <summary>
        /// Permite listar las empresas de generación eléctrica
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasGeneracion()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasGeneracion();
        }

        /// <summary>
        /// Permite listar empresas por tipo de negocio (Distribuidoas, Generadoras .. etc)
        /// </summary>
        /// <param name="tiposEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasPorTipo(string tiposEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetByCriteria(tiposEmpresa);
        }

        /// <summary>
        /// Devuelve lista de empresas que poseen centrales Hidraulicas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasHidro()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasHidro();
        }

        public List<EqEquipoDTO> ObtenerEquiposCuencaHidro()
        {
            return FactorySic.GetEqEquipoRepository().ObtenerEquipoPadresHidrologicosCuenca();
        }

        /// <summary>
        /// Devuelve lista de empresas que poseen centrales Hidraulicas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasPtoMedicion()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasPtoMedicion();
        }

        /// <summary>
        /// Permite listar las empresas del SEIN
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasCOES()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        #region Metodos de Fechas y Feriados

        /// <summary>
        /// Permite listar todos los registros de la tabla DOC_DIA_ESP
        /// </summary>
        public List<DocDiaEspDTO> ListDocDiaEsps()
        {
            return FactorySic.GetDocDiaEspRepository().List();
        }


        /// <summary>
        /// Retorna los dias habiles
        /// </summary>
        /// <param name="dtFechaInicio"></param>
        /// <param name="iDiasHabiles"></param>
        /// <returns></returns>
        public DateTime FechaEnDiasHabiles(DateTime dtFechaInicio, int iDiasHabiles)
        {
            DateTime dtFechaAux = dtFechaInicio;
            var lsDiasFeriados = ListDocDiaEsps();
            int signo = 1;
            if (iDiasHabiles < 0)
                signo = -1;
            int i = 1;
            while (i <= (signo * iDiasHabiles))
            {
                dtFechaAux = dtFechaAux.AddDays(signo);
                var bFeriado = EsFeriado(dtFechaAux, lsDiasFeriados);
                if (!bFeriado && dtFechaAux.DayOfWeek != DayOfWeek.Saturday && dtFechaAux.DayOfWeek != DayOfWeek.Sunday)
                    i++;
            }
            return dtFechaAux;
        }

        /// <summary>
        /// Indica sui un dia es feriado
        /// </summary>
        /// <param name="adt_fecha"></param>
        /// <param name="an_TablaFeriados"></param>
        /// <returns></returns>
        public bool EsFeriado(DateTime adt_fecha, List<DocDiaEspDTO> an_TablaFeriados)
        {
            bool lb_feriado;
            lb_feriado = false;
            DateTime ldt_feriado;
            string ls_frecuencia;

            var ln_dr2 = an_TablaFeriados.Find(x => x.Diafecha == adt_fecha && x.Diafrec == "E"); //FECHAS CON EXCEPCIONES 29-07-2020 LABORABLE

            if (ln_dr2 != null)
            {
                lb_feriado = false;
            }
            else
            {
                foreach (var ln_dr in an_TablaFeriados)
                {
                    ldt_feriado = Convert.ToDateTime(ln_dr.Diafecha);
                    ls_frecuencia = ln_dr.Diafrec;

                    if (ldt_feriado.Day == adt_fecha.Day && ldt_feriado.Month == adt_fecha.Month) //Es un dia Feriado
                    {
                        //Preguntamos si es frecuente para todos los años
                        if (adt_fecha.Year >= ldt_feriado.Year && ls_frecuencia == "S")
                        {
                            lb_feriado = true;
                            break;
                        }
                        else if (adt_fecha.Year < ldt_feriado.Year && ls_frecuencia == "S")
                        {
                            lb_feriado = true;
                            break;
                        }
                        //Fechas donde el feriado es valido solo ese año
                        else if (adt_fecha.Year == ldt_feriado.Year && ls_frecuencia == "N")
                        {
                            lb_feriado = true;
                            break;
                        }
                        else
                        {
                            lb_feriado = false;
                        }
                    }
                }
            }

            return lb_feriado;
        }
        /// <summary>
        /// Indica sui un dia es feriado
        /// </summary>
        /// <param name="adt_fecha"></param>
        /// <param name="an_TablaFeriados"></param>
        /// <returns></returns>
        public bool EsFeriado(DateTime adt_fecha)
        {
            var an_TablaFeriados = FactorySic.GetDocDiaEspRepository().List();
            return EsFeriado(adt_fecha, an_TablaFeriados);
        }

        /// <summary>
        /// Indica si la fecha es feriado o no
        /// </summary>
        /// <param name="adt_fecha"></param>
        /// <param name="an_TablaFeriados"></param>
        /// <returns></returns>
        public bool EsFeriadoByFecha(DateTime adt_fecha, List<DocDiaEspDTO> an_TablaFeriados)
        {
            bool lb_feriado;
            lb_feriado = false;
            DateTime ldt_feriado;
            string ls_frecuencia;

            var ln_dr2 = an_TablaFeriados.Find(x => x.Diafecha == adt_fecha && x.Diafrec == "E"); //FECHAS CON EXCEPCIONES 29-07-2020 LABORABLE

            if (ln_dr2 != null)
            {
                lb_feriado = false;
            }
            else
            {
                foreach (var ln_dr in an_TablaFeriados)
                {
                    ldt_feriado = Convert.ToDateTime(ln_dr.Diafecha);
                    ls_frecuencia = ln_dr.Diafrec;

                    if (ldt_feriado.Day == adt_fecha.Day && ldt_feriado.Month == adt_fecha.Month) //Es un dia Feriado
                    {
                        //Preguntamos si es frecuente para todos los años
                        if (adt_fecha.Year >= ldt_feriado.Year && ls_frecuencia == "S")
                        {
                            lb_feriado = true;
                            break;
                        }
                        else if (adt_fecha.Year < ldt_feriado.Year && ls_frecuencia == "S")
                        {
                            lb_feriado = true;
                            break;
                        }
                        //Fechas donde el feriado es valido solo ese año
                        else if (adt_fecha.Year == ldt_feriado.Year && ls_frecuencia == "N")
                        {
                            lb_feriado = true;
                            break;
                        }
                        else
                        {
                            lb_feriado = false;
                        }
                    }
                }
            }
            return lb_feriado;
        }

        #endregion

        #region Tablas de IDS FW_COUNTER

        /// <summary>
        /// Permite obtener el id de una table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public int ObtenerNextIdTabla(string table)
        {
            FwCounterDTO counter = FactorySic.GetFwCounterRepository().GetById(table);

            int id = 1;

            if (counter != null)
            {
                if (counter.Maxcount != null) 
                {
                    id = (int)counter.Maxcount + 1;
                }
            }

            return id;
        }

        /// <summary>
        /// Permite actualizar el id de la tabla
        /// </summary>
        /// <param name="table"></param>
        /// <param name="id"></param>
        public void ActualizarIdTabla(string table, int id)
        {
            try
            {
                FwCounterDTO counter = new FwCounterDTO();
                counter.Maxcount = id;
                counter.Tablename = table;

                FactorySic.GetFwCounterRepository().Update(counter);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        public List<SiTipogeneracionDTO> ListarTiposGeneracion()
        {
            return FactorySic.GetSiTipogeneracionRepository().List();
        }

        /// <summary>
        /// Permite obtener los correos de los usuarios de las empresas por módulo
        /// </summary>
        /// <param name="idModulo"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaCorreoDTO> ObtenerCorreosPorEmpresaModulo(int idModulo, int idEmpresa)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().ObtenerCorreosPorEmpresaModulo(idModulo, idEmpresa);
        }

        /*Registro de MODPLANs*/

        /// <summary>
        /// Permite grabar un registro de descarga del modplan
        /// </summary>
        /// <param name="entity"></param>
        public void RegistrarDescargarModPlan(WbRegistroModplanDTO entity)
        {
            try
            {
                FactorySic.GetWbRegistroModplanRepository().Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un reporte de descarga del MODPLAN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<WbRegistroModplanDTO> ObtenerReporteDescarga(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetWbRegistroModplanRepository().GetByCriteria(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite grabar el acceso al modelo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarAccesoModelo(FwAccesoModeloDTO entity)
        {
            try
            {
                FactorySic.GetFwAccesoModeloRepository().Update(entity);
                int idResultado = FactorySic.GetFwAccesoModeloRepository().Save(entity);
                SiEmpresaCorreoDTO contacto = FactorySic.GetSiEmpresaCorreoRepository().GetById((int)entity.Empcorcodi);
                SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById((int)entity.Emprcodi);
               
                string enlace = Encriptacion.EncryptString(entity.Emprcodi + "@" + entity.Modcodi + "@" + entity.Empcorcodi);
                string mensaje = ObtenerCuerpoCorreoIntegrante(contacto.Empcornomb, empresa.Emprnomb, empresa.Emprruc, 
                    ((DateTime)entity.Acmodfecinicio).ToString("dd/MM/yyyy"), ((DateTime)entity.Acmodfin).ToString("dd/MM/yyyy"),
                    entity.Acmodveces.ToString(), enlace);
                mensaje = mensaje.Replace("[", "{");
                mensaje = mensaje.Replace("}", "]");
                List<string> emailsTo = new List<string>();
                string asunto = "Acceso al Software MODPLAN";
                string pathImagen = "fondo_modplan.png";

                emailsTo.Add(contacto.Empcoremail);
                (new TramiteVirtualAppServicio()).EnviarCorreoCredencial(emailsTo, asunto, mensaje, pathImagen, 1);

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }            
        }


        public string ObtenerCuerpoCorreoIntegrante(string nombre, string razonsocial, string ruc, string desde, string hasta, string intentos, string enlace)
        {
            string mensaje = @"
                <html>
                <head>       
                <style type='text/css'>
                <!--
                body
                [
	                font-family:Arial, Helvetica, sans-serif;
	                font-size:12px;
	                top:0;
	                left:0;
	                background-color:#ffffff;	
                ]
                .celdacon
                [
	                color:#333333;
                    width:200px;
	                font-size:11px;
	                font-family:Arial, Helvetica, sans-serif;
	                font-weight:bold;
	                line-height:25px;
	                padding-left:20px;
                ]
                .celdaenlace
                [
                    padding: 10px;
                    background-color: #3074B7;
                ]
                .celdacon1
                [
	                color:#333333;
	                font-size:11px;
	                font-family:Arial, Helvetica, sans-serif;
	                font-weight:normal;
	                line-height:25px;	        
                ]
                .celda
                [
	                color:#4171A0;
	                font-size:11px;
	                font-family:Arial, Helvetica, sans-serif;
	                font-weight:bold;
	                line-height:25px;
	                padding-left:20px;
                ]
                .celdalink a
                [
	                color:#4171A0;
	                font-size:11px;
	                font-family:Arial, Helvetica, sans-serif;
	                font-weight:bold;
	                line-height:25px;
	                text-indent:25px;
                ]
                .tabla_general 
                [
	                width: 100%;
	                border: 1px solid #B4C0C0;
	                font-size:10px;
                ]
                .tabla_general td.tb_celda
                [
	                background-color:#F2F2F2;
	                height:17px;
	                text-align:center;
	                color:#3A3A3A;
	                border-left:1px solid #D8DEDE;	
	                border-right:1px solid #D8DEDE;	
                ]
                .tabla_general th.tb_header
                [
	                background-color:#4171A0;
	                height:17px;
	                text-align:center;
	                color:#FFFFFF;
	                font-size:10px;
	                font-weight:bold;
	                line-height:18px;
                ]
                -->
                </style>
                </head>
                <body>
                <table width='605'>
                <tr>
                <td> <img src=cid:topImageID /></td>
                </tr>
                <tr>
                <td>
                    <br />
	                <table cellspacing='0' border='0' width='100%' >		
		                <tr>
			                <td class='celda' colspan='2'>Estimado(a): {0},<br /><br /></td>			       
		                </tr>
		                <tr>			        
			                <td class='celdacon' colspan='2'>

Se ha concedido acceso para la descarga del Software MODPLAN, los datos son los siguientes:<br /><br />
                                </td>
		                </tr>	                
                        <tr>
                            <td class='celdacon'  style='width:300px'>Empresa:</td>
                            <td class='celdacon1' style='width:400px'>{1}</td>
                        </tr>       
                        <tr>
                            <td class='celdacon'  style='width:300px'>RUC:</td>
                            <td class='celdacon1' style='width:400px'>{2}</td>
                        </tr>                       
                        <tr>
                            <td class='celdacon'>Vigente desde:</td>
                            <td class='celdacon1'>{3}</td>
                        </tr>                                    
                        <tr>
                            <td class='celdacon'>Vigente hasta:</td>
                            <td class='celdacon1'>{4}</td>
                        </tr>     
                        
                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
                                <br />
                                

Por favor acceder al siguiente enlace para realizar la descarga del Software. <br />
<br />

<table style='width:500px' cellpadding='4'>
<tr>
<td style='width:45%; background-color:#3074B7; text-align:center;'><a href='https://www.coes.org.pe/portal/planificacion/descargamodplan/?id={5}' style='color:white; text-decoration:none; font-weight:bold'>Descargar Software MODPLAN</a></td>
<td style='width:10%'></td>

</tr>
</table>


<br/ >
                            

</td>
                        </tr>

                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
                                Atentamente, <br />
                                COES
                            </td>
                        </tr>

                      
	                </table>	        
	                <br/>	
	                </td>
	                </tr>
                </table>
                </body>
                </html>
             ";

            return String.Format(mensaje, nombre, razonsocial, ruc, desde, hasta, enlace);
        }


        //
        public string ObtenerCuerpoCorreoConfirmacion(string nombre, string clave)
        {

            string mensaje = @"
                <html>
                <head>       
                <style type='text/css'>
                <!--
                body
                [
	                font-family:Arial, Helvetica, sans-serif;
	                font-size:12px;
	                top:0;
	                left:0;
	                background-color:#ffffff;	
                ]
                .celdacon
                [
	                color:#333333;
                    width:200px;
	                font-size:11px;
	                font-family:Arial, Helvetica, sans-serif;
	                font-weight:bold;
	                line-height:25px;
	                padding-left:20px;
                ]
                .celdaenlace
                [
                    padding: 10px;
                    background-color: #3074B7;
                ]
                .celdacon1
                [
	                color:#333333;
	                font-size:11px;
	                font-family:Arial, Helvetica, sans-serif;
	                font-weight:normal;
	                line-height:25px;	        
                ]
                .celda
                [
	                color:#4171A0;
	                font-size:11px;
	                font-family:Arial, Helvetica, sans-serif;
	                font-weight:bold;
	                line-height:25px;
	                padding-left:20px;
                ]
                .celdalink a
                [
	                color:#4171A0;
	                font-size:11px;
	                font-family:Arial, Helvetica, sans-serif;
	                font-weight:bold;
	                line-height:25px;
	                text-indent:25px;
                ]
                .tabla_general 
                [
	                width: 100%;
	                border: 1px solid #B4C0C0;
	                font-size:10px;
                ]
                .tabla_general td.tb_celda
                [
	                background-color:#F2F2F2;
	                height:17px;
	                text-align:center;
	                color:#3A3A3A;
	                border-left:1px solid #D8DEDE;	
	                border-right:1px solid #D8DEDE;	
                ]
                .tabla_general th.tb_header
                [
	                background-color:#4171A0;
	                height:17px;
	                text-align:center;
	                color:#FFFFFF;
	                font-size:10px;
	                font-weight:bold;
	                line-height:18px;
                ]
                -->
                </style>
                </head>
                <body>
                <table width='605'>
                <tr>
                <td> <img src=cid:topImageID /></td>
                </tr>
                <tr>
                <td>
                    <br />
	                <table cellspacing='0' border='0' width='100%' >		
		                <tr>
			                <td class='celda' colspan='2'>Estimado(a): {0},<br /><br /></td>			       
		                </tr>
		                <tr>			        
			                <td class='celdacon' colspan='2'>

Por favor utilice el siguiente código para habilitar la descarga del Software MODPLAN:<br /><br />
                                </td>
		                </tr>	                
                       
                   
                        <tr>                           
                            <td class='celdacon1' colspan='2' style='font-size:25px; font-weight:bold; text-align:center'>{1}</td>
                        </tr>                            

                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
                                Atentamente, <br />
                                COES
                            </td>
                        </tr>

                      
	                </table>	        
	                <br/>	
	                </td>
	                </tr>
                </table>
                </body>
                </html>
             ";

            return String.Format(mensaje, nombre, clave);
        }


        /// <summary>
        /// Permite listar los accesos a la descarga de modelos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idModulo"></param>
        /// <returns></returns>
        public List<FwAccesoModeloDTO> ListadoAccesos(int idEmpresa, int idModulo)
        {
            return FactorySic.GetFwAccesoModeloRepository().GetByCriteria(idEmpresa, idModulo);
        }

        public FwAccesoModeloDTO ObtenerAccesoModPlan(int idEmpresa, int idModulo, int idEmpresaCorreo)
        {
            return FactorySic.GetFwAccesoModeloRepository().GetById(idEmpresa, idModulo, idEmpresaCorreo);
        }

        /// <summary>
        /// Permite obtener la cuenta del agente
        /// </summary>
        /// <param name="idEmpresaCorreo"></param>
        /// <returns></returns>
        public SiEmpresaCorreoDTO ObtenerEmpresaCorreo(int idEmpresaCorreo)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().GetById(idEmpresaCorreo);
        }

        public void EliminarAccesoModplan(int id)
        {
            try
            {
                FactorySic.GetFwAccesoModeloRepository().Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void EliminarAccesosModPlanPorContacto(int idEmpresaCorreo)
        {
            try
            {
                FactorySic.GetFwAccesoModeloRepository().EliminarPorContacto(idEmpresaCorreo);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void ConfirmarAccesoModPlan(FwAccesoModeloDTO entity)
        {
            try
            {
                GeneradorPassword generador = new GeneradorPassword(6, 0, 0, 100);
                SiEmpresaCorreoDTO cuenta = FactorySic.GetSiEmpresaCorreoRepository().GetById((int)entity.Empcorcodi);

                string clave = generador.GetNewPassword();
                entity.Acmodkey = clave;
                entity.Acmodusumodificacion = cuenta.Empcoremail;
                entity.Acmodfecmodificacion = DateTime.Now;

                FactorySic.GetFwAccesoModeloRepository().UpdateClave(entity);
                
               
                string mensaje = ObtenerCuerpoCorreoConfirmacion(cuenta.Empcornomb, clave);
                mensaje = mensaje.Replace("[", "{");
                mensaje = mensaje.Replace("}", "]");
                List<string> emailsTo = new List<string>();
                string asunto = "Verificación de acceso al MODPLAN";
                string pathImagen = "fondo_modplan.png";

                emailsTo.Add(cuenta.Empcoremail);
                (new TramiteVirtualAppServicio()).EnviarCorreoCredencial(emailsTo, asunto, mensaje, pathImagen, 1);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listado de empresas con centrales activas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListadoEmpresasCentralesActivas()
        {
            try
            {
                return FactorySic.GetSiEmpresaRepository().ListadoEmpresasCentralesActivas();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void AlmacenamientoInformacionBI()
        {
            try
            {
                Logger.Info("Inicio proceso programado de AlmacenamientoInformacionBI");
                ReporteMedidoresAppServicio servReporte = new ReporteMedidoresAppServicio();
                string mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("dd/MM/yyyy");

                DateTime fechaIni = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                DateTime fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

                Logger.Info("Se inicio el proceso Potencia Efectiva. Ini:" + fechaIni + " - Fin: " + fechaFin);
                servReporte.GuardarEstructurasPotEfectiva(fechaIni, fechaFin, "root");
                Logger.Info("El proceso Potencia Efectiva ha sido existoso.");
                Logger.Info("Se inicio el proceso Produccion Generacion y Resumen. Ini:" + fechaIni + " - Fin: " + fechaFin);
                servReporte.GuardarEstructurasProduccionGeneracionYResumen(fechaIni, fechaFin, "root");
                Logger.Info("El proceso Produccion Generacion y Resumen ha sido existoso.");
                Logger.Info("Se inicio el proceso Factor Planta. Ini:" + fechaIni + " - Fin: " + fechaFin);
                servReporte.GuardarEstructurasFactorPlanta(fechaIni, fechaFin, "root");
                Logger.Info("El proceso Produccion Generacion y Resumen ha sido existoso.");
                Logger.Info("Se inicio el proceso Máxima Demanda. Ini:" + fechaIni + " - Fin: " + fechaFin);
                servReporte.GuardarEstructurasMaximaDemanda(fechaIni, fechaFin, "root");
                Logger.Info("El proceso Máxima Demanda ha sido existoso.");

                //NOTIFICACION DE PROCESO CULMINADO
                this.EnviarCorreoNotificacion();

                Logger.Info("Fin proceso programado de AlmacenamientoInformacionBI!!!");

            }
            catch (Exception ex)
            {
                Logger.Error("Error en el proceso programado de AlmacenamientoInformacionBI");
                throw new Exception(ex.Message, ex);
            }
        }

        public void EnviarCorreoNotificacion()
        {
            string mensaje = "Proceso de Almacenamiento de Información para aplicativos de BI se ejecutó correctamente.";

            string sTo = ConfigurationManager.AppSettings["BuzonSoporteAplicaciones"].ToString();
            List<string> listTo = new List<string>();
            listTo = sTo.Split(';').ToList();
            List<string> listBCc = new List<string>();
            List<string> listCC = new List<string>();

            Base.Tools.Util.SendEmail(listTo, listCC, listBCc, "Notificación de Almacenamiento BI", mensaje, null);
        }

        public void NotificacionReportesDiarios()
        {
            var plantilla = new SiPlantillacorreoDTO();
            string contenido = String.Empty;
            string sto = String.Empty;
            string asunto = String.Empty;
            string listaBCc = String.Empty;
            string listaCC = String.Empty;
            DateTime final = DateTime.Now;
            string listFrom = String.Empty;

            plantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(121);
            contenido = plantilla.Plantcontenido;
            sto = plantilla.Planticorreos;
            listaBCc = plantilla.PlanticorreosBcc;
            listaCC = plantilla.PlanticorreosCc;
            asunto = plantilla.Plantasunto;
            listFrom = plantilla.PlanticorreoFrom;

            List<string> listTo = new List<string>();
            listTo = sto.Split(';').ToList();

            List<string> listBCc = new List<string>();
            listBCc = listaBCc.Split(';').ToList();

            List<string> listCC = new List<string>();

            Base.Tools.Util.SendEmail(listTo, listBCc, listCC, asunto, contenido, listFrom);
        }
        public void notificacionProcedimientoTecnico()
        {
            var plantilla = new SiPlantillacorreoDTO();
            string contenido = String.Empty;
            string sto = String.Empty;
            string asunto = String.Empty;
            string listaBCc = String.Empty;
            string listaCC = String.Empty;
            DateTime final = DateTime.Now;
            string listFrom = String.Empty;

            plantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(107);
            contenido = plantilla.Plantcontenido;
            sto = plantilla.Planticorreos;
            listaBCc = plantilla.PlanticorreosBcc;
            listaCC = plantilla.PlanticorreosCc;
            asunto = plantilla.Plantasunto;
            listFrom = plantilla.PlanticorreoFrom;

            List<string> listTo = new List<string>();
            listTo = sto.Split(';').ToList();

            List<string> listBCc = new List<string>();
            listBCc = listaBCc.Split(';').ToList();

            List<string> listCC = new List<string>();
            //listCC = listaCC.Split(';').ToList();


            var mensaje = String.Format(contenido, final.ToString("dd - MMMM - yyyy"));
            var mensajeFin = mensaje.Replace("-", "de");

            Base.Tools.Util.SendEmail(listTo, listBCc, listCC, asunto, mensajeFin, listFrom);
        }
    }
}
