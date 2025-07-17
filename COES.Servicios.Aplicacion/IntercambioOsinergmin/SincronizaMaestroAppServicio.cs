// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 24/10/2016
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using System;
using System.Collections.Generic;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using log4net;
using Org.BouncyCastle.Security;
using System.Text;
using System.Configuration;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Servicios.Aplicacion.IntercambioOsinergmin
{

    /// <summary>
    /// Controla la sincronización de los maestros. Esta clases se instancia desde el procesamiento automático.
    /// </summary>
    public class SincronizaMaestroAppServicio : AppServicioBase
    {

        #region CAMPOS: Variables de la clase.

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ImportacionAppServicio));

        /// <summary>
        /// Sostiene la lista de ererores por entidad que deberá ser reportado al administrador de la sincronización.
        /// </summary>
        private Dictionary<EntidadSincroniza, List<ErrorSincroniza>> listaErrores = null;

        /// <summary>
        /// Fecha de la sincronización que permite identificar un trabajo de sincronización.
        /// </summary>
        private DateTime fechaSincronizacion;

        /// <summary>
        /// Servicio de sincronización.
        /// </summary>
        private SincronizacionMaestrosEndpointService epSincronizaMaestros = null;

        #endregion

        #region METODOS: Metodos de la clase.

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        public SincronizaMaestroAppServicio()
        {
            string url = ConfigurationManager.AppSettings["UrlSincronizacionMaestro"].ToString();
            if (string.IsNullOrEmpty(url))
            {
                throw new ApplicationException("No se ha encontrado la definición de la dirección URL del servicio de sincronización de maestros");
            }

            this.epSincronizaMaestros = new SincronizacionMaestrosEndpointService(url);
        }

        /// <summary>
        /// Realiza la sincronización de todas las entidades.
        /// </summary>
        public void IniciarSincronizacionTotal()
        {
            listaErrores = new Dictionary<EntidadSincroniza, List<ErrorSincroniza>>();
            try
            {
                this.MarcarFechaSincronizacion();

                List<Sincroniza> listaClasesHijas = this.ConfigurarEntidades(this.epSincronizaMaestros);

                int resultadoSinc = 0;

                foreach (Sincroniza sincronizador in listaClasesHijas)
                {
                    resultadoSinc = sincronizador.IniciarSincronizacion();
                    if (resultadoSinc == 0)
                    {
                        this.listaErrores.Add(sincronizador.ObtenerEntidadSincroniza(), sincronizador.ObtenerListaErrores());
                    }
                }

                this.NotificarAsignaciones();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                string asunto = string.Format(@"COES\Sincronización de maestros con el Osinergmin | Fecha-hora {0} | {1}"
                                            , this.fechaSincronizacion.ToString(ConstantesIntercambio.FormatoDotNETFechaHora24)
                                            , "Error");

                string contenido = string.Format("Estimado(a), buen día."
                                               + "<br><br>"
                                               + "Ha ocurrido un error en la sincronización de maestros con el Osinergmin. Revise el detalle siguiente:"
                                               + "<br><br>"
                                               + "<b>1. Mensaje</b>: {0}"
                                               + "<br><br>"
                                               + "<b>2. Descripción del error:</b>"
                                               + "<br><br>{1}"
                                               , ex.Message
                                               , ex.StackTrace);

                COES.Base.Tools.Util.SendEmail(ConstantesIntercambio.CorreoAdmSincronizaMaestro
                                             , asunto
                                             , contenido);
            }
        }

        /// <summary>
        /// Configura la lista de entidades que se tomarán como parte de la sincronización.
        /// </summary>
        /// <param name="servicioOsiSincroniza"></param>
        /// <returns></returns>
        private List<Sincroniza> ConfigurarEntidades(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
        {
            List<Sincroniza> listaClasesHijas = new List<Sincroniza>();

            //- =========================================
            //- Entidades de sincronización Bidireccional
            //- =========================================

            listaClasesHijas.Add(new SincronizaEmpresa(this.fechaSincronizacion, servicioOsiSincroniza));
            listaClasesHijas.Add(new SincronizaUsuarioLibre(this.fechaSincronizacion, servicioOsiSincroniza));
            listaClasesHijas.Add(new SincronizaSuministro(this.fechaSincronizacion, servicioOsiSincroniza));
            listaClasesHijas.Add(new SincronizaBarra(this.fechaSincronizacion, servicioOsiSincroniza));

            //- ====================================================
            //- Entidades de sincronización COES determina el código
            //- ====================================================

            //- Entidad: Grupo

            listaClasesHijas.Add(new SincronizaCentralGeneracion(this.fechaSincronizacion, servicioOsiSincroniza));
            listaClasesHijas.Add(new SincronizaGrupoGeneracion(this.fechaSincronizacion, servicioOsiSincroniza));
            listaClasesHijas.Add(new SincronizaModoOperacion(this.fechaSincronizacion, servicioOsiSincroniza));

            //- Entidad: Equipo

            listaClasesHijas.Add(new SincronizaCuenca(this.fechaSincronizacion, servicioOsiSincroniza));
            listaClasesHijas.Add(new SincronizaEmbalse(this.fechaSincronizacion, servicioOsiSincroniza));
            listaClasesHijas.Add(new SincronizaLago(this.fechaSincronizacion, servicioOsiSincroniza));

            return listaClasesHijas;
        }

        /// <summary>
        /// Obtiene la lista de valores para la respectiva homologación.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ObtenerListaValoresParaHomologacion(EntidadSincroniza entidad)
        {

            Dictionary<string, string> valoresHomologacion = new Dictionary<string, string>();
            Sincroniza sincronizaEntidad = null;

            switch (entidad)
            {
                case EntidadSincroniza.Empresa:                    
                    sincronizaEntidad = new SincronizaEmpresa(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.UsuarioLibre:
                    sincronizaEntidad = new SincronizaUsuarioLibre(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.Suministro:
                    sincronizaEntidad = new SincronizaSuministro(this.epSincronizaMaestros);                    
                    break;

                case EntidadSincroniza.Barra:
                    sincronizaEntidad = new SincronizaBarra(this.epSincronizaMaestros);
                    break;
                    
                case EntidadSincroniza.CentralGeneracion:
                    sincronizaEntidad = new SincronizaCentralGeneracion(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.GrupoGeneracion:
                    sincronizaEntidad = new SincronizaGrupoGeneracion(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.ModoOperacion:
                    sincronizaEntidad = new SincronizaModoOperacion(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.Cuenca:
                    sincronizaEntidad = new SincronizaCuenca(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.Embalse:
                    sincronizaEntidad = new SincronizaEmbalse(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.Lago:
                    sincronizaEntidad = new SincronizaLago(this.epSincronizaMaestros);
                    break;

                default:
                    throw new ApplicationException("No es posible obtener la lista debido a que no existe la implementación");
            }

            valoresHomologacion = sincronizaEntidad.ObtenerValoresHomologacion();

            return valoresHomologacion;
        }

        //- alpha.HDT - 14/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener los RUC de los SuministroUsuario
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, suministroUsuarioDTO> ObtenerSuministroXSuministroUsuario()
        {
            SincronizaSuministro sincronizaSuministro = new SincronizaSuministro(this.epSincronizaMaestros);
            return sincronizaSuministro.ObtenerSuministroXSuministroUsuario();
        }

        //- alpha.HDT - 23/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la lista de Barras Osinergmin.
        /// </summary>
        /// <returns></returns>
        public List<barraDTO> ObtenerBarrasOsinergmin()
        {
            SincronizaBarra sincronizaBarra = new SincronizaBarra(this.epSincronizaMaestros);
            return sincronizaBarra.ObtenerBarras();
        }

        /// <summary>
        /// Permite obtener el nombre de la columna de hologación.
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public ColumnaSincroniza ObtenerColumnaHomologacion(EntidadSincroniza entidad)
        {

            ColumnaSincroniza columna = ColumnaSincroniza.Ninguno;

            switch (entidad)
            {
                case EntidadSincroniza.Ninguno:
                    break;

                case EntidadSincroniza.Empresa:
                    columna = ColumnaSincroniza.EMPRCODOSINERGMIN;
                    break;

                case EntidadSincroniza.UsuarioLibre:
                    columna = ColumnaSincroniza.EMPRRUC;
                    break;

                case EntidadSincroniza.Suministro:
                case EntidadSincroniza.Barra:
                case EntidadSincroniza.CentralGeneracion:
                case EntidadSincroniza.GrupoGeneracion:
                case EntidadSincroniza.ModoOperacion:
                case EntidadSincroniza.Cuenca:
                case EntidadSincroniza.Embalse:
                case EntidadSincroniza.Lago:
                    columna = ColumnaSincroniza.OSINERGCODI;
                    break;

                default:
                    break;
            }

            return columna;
        }

        /// <summary>
        /// Permite actualizar la información en el Osinergmin.
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="codigoOsinergmin"></param>
        /// <param name="codigoCOES"></param>
        public void ActualizarVinculoEnOsinergmin(EntidadSincroniza entidad, string codigoOsinergmin, string codigoCOES)
        {
            Sincroniza sincronizaEntidad = null;

            switch (entidad)
            {
                case EntidadSincroniza.Empresa:
                    sincronizaEntidad = new SincronizaEmpresa(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.UsuarioLibre:
                    sincronizaEntidad = new SincronizaUsuarioLibre(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.Suministro:
                    sincronizaEntidad = new SincronizaSuministro(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.Barra:
                    sincronizaEntidad = new SincronizaBarra(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.CentralGeneracion:
                    sincronizaEntidad = new SincronizaCentralGeneracion(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.GrupoGeneracion:
                    sincronizaEntidad = new SincronizaGrupoGeneracion(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.ModoOperacion:
                    sincronizaEntidad = new SincronizaModoOperacion(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.Cuenca:
                    sincronizaEntidad = new SincronizaCuenca(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.Embalse:
                    sincronizaEntidad = new SincronizaEmbalse(this.epSincronizaMaestros);
                    break;

                case EntidadSincroniza.Lago:
                    sincronizaEntidad = new SincronizaLago(this.epSincronizaMaestros);
                    break;

                default:
                    throw new ApplicationException("No es posible actualizar el vínculo debido a que no existe la implementación");
            }

            sincronizaEntidad.ActualizarVinculoEnOsinergmin(codigoOsinergmin, codigoCOES);
        }

        /// <summary>
        /// Marca el inicio de la sincronización almacenando la fecha hora actual en un miembro privado
        /// de la clase.
        /// </summary>
        private void MarcarFechaSincronizacion()
        {
            fechaSincronizacion = DateTime.Now;
        }


        /// <summary>
        /// Permite informar a través de una notificación sobre las asignaciones en sus distintos estados
        /// de sincronización.
        /// </summary>
        private void NotificarAsignaciones()
        {
            List<IioAsignacionPendienteDTO> listaAsignacionPendiente = null;

            listaAsignacionPendiente
                = FactorySic.GetIioAsignacionPendienteRepository().ListByCreationDate(this.fechaSincronizacion.ToString(ConstantesIntercambio.FormatoDotNETFechaHora24));
            int idPlantilla = ConstantesIntercambio.IdPlantillaResultadoSincronizacion;

            //- 2. Procesando los registros exitosos.
            //- =====================================
            int cantidadListas;
            StringBuilder sbTablaListas = new StringBuilder();
            this.ProcesarAsignacionesListas(sbTablaListas, listaAsignacionPendiente, out cantidadListas);

            //- 3. Procesando los registros con observaciones.
            //- =============================================
            int cantidadPendientes;
            StringBuilder sbTablaPendientes = new StringBuilder();
            this.ProcesarAsignacionesPendientes(sbTablaPendientes, listaAsignacionPendiente, out cantidadPendientes);

            //- 3. Procesando los errores.
            //- =============================================
            int cantidadErrores = 0;
            StringBuilder sbTablaErrores = new StringBuilder();
            this.ProcesarErrores(sbTablaErrores, out cantidadErrores);

            //- 4. Enviado la notificación.
            //- ===========================
            SiPlantillacorreoDTO siPlantillaCorreo = FactorySic.GetSiPlantillacorreoRepository().GetById(idPlantilla);
            string fechaHora = this.fechaSincronizacion.ToString(ConstantesIntercambio.FormatoDotNETFechaHora24);
            string correoPara = ConstantesIntercambio.CorreoAdmSincronizaMaestro;
            string asunto = string.Format(siPlantillaCorreo.Plantasunto, fechaHora);
            string contenido = siPlantillaCorreo.Plantcontenido;

            contenido = contenido.Replace("{0}", cantidadListas.ToString());
            contenido = contenido.Replace("{1}", cantidadListas > 0 ? sbTablaListas.ToString() : string.Empty);
            contenido = contenido.Replace("{2}", cantidadPendientes.ToString());
            contenido = contenido.Replace("{3}", cantidadPendientes > 0 ? sbTablaPendientes.ToString() : string.Empty);
            contenido = contenido.Replace("{4}", cantidadErrores.ToString());
            contenido = contenido.Replace("{5}", cantidadErrores > 0 ? sbTablaErrores.ToString() : string.Empty);

            COES.Base.Tools.Util.SendEmail(correoPara, asunto, contenido);

            //- 5. Persistiendo la notificación realizada.
            //- ==========================================
            SiCorreoDTO correo = new SiCorreoDTO();
            correo.Corrasunto = asunto;
            correo.Corrcontenido = contenido;
            correo.Corrfechaenvio = DateTime.Now;
            correo.Corrto = correoPara;
            correo.Plantcodi = siPlantillaCorreo.Plantcodi;

            FactorySic.GetSiCorreoRepository().Save(correo);
        }

        /// <summary>
        /// Procesa los registros exitosos generando un StringBuilder con la tabla de registros sincronizados con éxito.
        /// </summary>
        /// <param name="sbTablaExito"></param>
        /// <param name="listaAsignacionPendiente"></param>
        /// <param name="cantidadRegistrosExito"></param>
        private void ProcesarAsignacionesListas(StringBuilder sbTablaExito, List<IioAsignacionPendienteDTO> listaAsignacionPendiente, out int cantidadRegistrosExito)
        {
            cantidadRegistrosExito = 0;

            MotivoPendiente motivoActual = MotivoPendiente.Ninguno;

            using (TablaHTML.Tabla tabla = new TablaHTML.Tabla(sbTablaExito))
            {
                //listaAsignacionPendiente.Sort((x, y) => x.Mapendescripcion.CompareTo(y.Mapendescripcion));

                listaAsignacionPendiente.Sort((x, y) =>
                {
                    var ret = x.Mapenentidad.CompareTo(y.Mapenentidad);
                    if (ret == 0) ret = x.Mapendescripcion.CompareTo(y.Mapendescripcion);
                    return ret;
                });

                foreach (IioAsignacionPendienteDTO asignacionPendiente in listaAsignacionPendiente)
                {
                    if (!Enum.IsDefined(typeof(MotivoPendiente), asignacionPendiente.Mapenestado))
                    {
                        throw new ApplicationException("No se ha definido el Motivo Pendiente con el identificador " + asignacionPendiente.Mapenestado
                                                     + ". El administrador debe revisar que exista el código del motivo (estado) de asignación.");
                    }

                    motivoActual = (MotivoPendiente)asignacionPendiente.Mapenestado;

                    if (motivoActual != MotivoPendiente.Ninguno)
                    {
                        //- Sincronización fallida.
                        continue;
                    }

                    cantidadRegistrosExito++;

                    if (cantidadRegistrosExito == 1)
                    {
                        using (TablaHTML.Fila fila = tabla.AddRow())
                        {
                            //- Se trata del primer error.
                            fila.AgregarCeldaCabecera("Entidad");
                            fila.AgregarCeldaCabecera("Código");
                            fila.AgregarCeldaCabecera("Descripción");
                            fila.AgregarCeldaCabecera("Observación");
                        }
                    }

                    using (TablaHTML.Fila fila = tabla.AddRow())
                    {
                        fila.AgregarCelda(asignacionPendiente.Mapenentidad);
                        fila.AgregarCelda(asignacionPendiente.Mapencodigo);
                        fila.AgregarCelda(asignacionPendiente.Mapendescripcion);
                        fila.AgregarCelda(asignacionPendiente.Mapenindicacionestado);
                    }
                }
            }
        }

        /// <summary>
        /// Procesa los registros de sincronización fallida generando un StringBuilder con la tabla de registros sincronizados sin éxito.
        /// </summary>
        /// <param name="sbTablaFracaso"></param>
        /// <param name="listaAsignacionPendiente"></param>
        /// <param name="cantidadSincFracaso"></param>
        private void ProcesarAsignacionesPendientes(StringBuilder sbTablaFracaso, List<IioAsignacionPendienteDTO> listaAsignacionPendiente, out int cantidadSincFracaso)
        {
            cantidadSincFracaso = 0;

            MotivoPendiente motivoActual = MotivoPendiente.Ninguno;

            using (TablaHTML.Tabla tabla = new TablaHTML.Tabla(sbTablaFracaso))
            {
                //listaAsignacionPendiente.Sort((x, y) => x.Mapendescripcion.CompareTo(y.Mapendescripcion));

                listaAsignacionPendiente.Sort((x, y) =>
                {
                    var ret = x.Mapenentidad.CompareTo(y.Mapenentidad);
                    if (ret == 0) ret = x.Mapendescripcion.CompareTo(y.Mapendescripcion);
                    return ret;
                });

                foreach (IioAsignacionPendienteDTO asignacionPendiente in listaAsignacionPendiente)
                {
                    if (!Enum.IsDefined(typeof(MotivoPendiente), asignacionPendiente.Mapenestado))
                    {
                        throw new ApplicationException("No se ha definido el Motivo Pendiente con el identificador " + asignacionPendiente.Mapenestado
                                                     + ". El administrador debe revisar que exista el código del motivo (estado) de asignación.");
                    }

                    motivoActual = (MotivoPendiente)asignacionPendiente.Mapenestado;

                    if (motivoActual == MotivoPendiente.Ninguno)
                    {
                        //- Sincronización exitosa.
                        continue;
                    }

                    cantidadSincFracaso++;
                    if (cantidadSincFracaso == 1)
                    {
                        using (TablaHTML.Fila fila = tabla.AddRow())
                        {
                            //- Se trata del primer error.
                            fila.AgregarCeldaCabecera("Entidad");
                            fila.AgregarCeldaCabecera("Código");
                            fila.AgregarCeldaCabecera("Descripción");
                            fila.AgregarCeldaCabecera("Observación");
                        }
                    }

                    using (TablaHTML.Fila fila = tabla.AddRow())
                    {
                        fila.AgregarCelda(asignacionPendiente.Mapenentidad);
                        fila.AgregarCelda(asignacionPendiente.Mapencodigo);
                        fila.AgregarCelda(asignacionPendiente.Mapendescripcion);
                        fila.AgregarCelda(asignacionPendiente.Mapenindicacionestado);
                    }
                }
            }
        }

        /// <summary>
        /// Procesa los errores de la sincronización generando un StringBuilder con la tabla de errores generados en la sincronización.
        /// </summary>
        /// <param name="sbTablaErrores"></param>
        /// <param name="cantidadErrores"></param>
        private void ProcesarErrores(StringBuilder sbTablaErrores, out int cantidadErrores)
        {
            cantidadErrores = 0;

            using (TablaHTML.Tabla tabla = new TablaHTML.Tabla(sbTablaErrores))
            {
                foreach (KeyValuePair<EntidadSincroniza, List<ErrorSincroniza>> kvpErrores in this.listaErrores)
                {
                    cantidadErrores++;
                    if (cantidadErrores == 1)
                    {
                        using (TablaHTML.Fila fila = tabla.AddRow())
                        {
                            //- Se trata del primer error.
                            fila.AgregarCeldaCabecera("Entidad");
                            fila.AgregarCeldaCabecera("Código del mensaje");
                            fila.AgregarCeldaCabecera("Mensaje");
                        }
                    }

                    foreach (ErrorSincroniza error in kvpErrores.Value)
                    {
                        using (TablaHTML.Fila fila = tabla.AddRow())
                        {
                            fila.AgregarCelda(kvpErrores.Key.ToString());
                            fila.AgregarCelda(error.CodigoMensaje);
                            fila.AgregarCelda(error.Mensaje);
                        }
                    }
                }
            }
        }

        #endregion


        //- ALPHA.JDEL - Inicio 04/01/2017: Cambio para atender el requerimiento.

        public string GetDatosArbolMaestros()
        {
            try
            {
                return ArbolHelper.ObtenerEstructuraDatos(ArbolHelper.IniciArbolMaestrosDtos());
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        public List<EntidadListadoDTO> ListMaestroEmpresa(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroEmpresa(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListMaestroUsuarioLibre(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroUsuarioLibre(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListMaestroSuministrador(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroSuministrador(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListMaestroBarra(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroBarra(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListMaestroCentralGeneracion(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroCentralGeneracion(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListMaestroGrupoGeneracion(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroGrupoGeneracion(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListMaestroModoOperacion(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroModoOperacion(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListMaestroCuenca(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroCuenca(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListMaestroEmbalse(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroEmbalse(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListMaestroLago(string nombre)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListMaestroLago(nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Genera el reporte de la consulta de entidades en Excel
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="filtro"></param>
        /// <param name="path"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarReporte(EntidadSincroniza entidad, string filtro, string path, string pathLogo)
        {
            if (!Enum.IsDefined(typeof(EntidadSincroniza), entidad)) throw new ArgumentOutOfRangeException("entidad");
            try
            {
                if (filtro == null)
                {
                    filtro = "";
                }


                string filename = path + ConstantesIio.ReporteSincronizacionFileName;

                List<EntidadListadoDTO> listado = new List<EntidadListadoDTO>();

                string ent = "";

                switch (entidad)
                {
                    case EntidadSincroniza.Empresa:
                        listado = this.ListMaestroEmpresa(filtro);
                        ent = "Empresa";
                        break;
                    case EntidadSincroniza.UsuarioLibre:
                        listado = this.ListMaestroUsuarioLibre(filtro);
                        ent = "UsuarioLibre";
                        break;
                    case EntidadSincroniza.Suministro:
                        listado = this.ListMaestroSuministrador(filtro);
                        ent = "Suministro";
                        break;
                    case EntidadSincroniza.Barra:
                        listado = this.ListMaestroBarra(filtro);
                        ent = "Barra";
                        break;
                    case EntidadSincroniza.CentralGeneracion:
                        listado = this.ListMaestroCentralGeneracion(filtro);
                        ent = "CentralGeneracion";
                        break;
                    case EntidadSincroniza.GrupoGeneracion:
                        listado = this.ListMaestroGrupoGeneracion(filtro);
                        ent = "GrupoGeneracion";
                        break;
                    case EntidadSincroniza.ModoOperacion:
                        listado = this.ListMaestroModoOperacion(filtro);
                        ent = "ModoOperacion";
                        break;
                    case EntidadSincroniza.Cuenca:
                        listado = this.ListMaestroCuenca(filtro);
                        ent = "Cuenca";
                        break;
                    case EntidadSincroniza.Embalse:
                        listado = this.ListMaestroEmbalse(filtro);
                        ent = "Embalse";
                        break;
                    case EntidadSincroniza.Lago:
                        listado = this.ListMaestroLago(filtro);
                        ent = "Lago";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("entidad", entidad, null);
                }

                ExcelDocument.GenerarFormato(filename, listado, ent);

                return filename;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Genera el reporte de los datos maestros del Osinergmin.
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="path"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarReporteDatosOsinergmin(EntidadSincroniza entidad, string path, string pathLogo)
        {
            if (!Enum.IsDefined(typeof(EntidadSincroniza), entidad))
            {
                throw new ArgumentOutOfRangeException("entidad");
            }
                
            try
            {
                string nombreArchivo = path + String.Format(ConstantesIio.ReporteDatosOsinergminFileName, entidad.ToString());

                List<EntidadListadoDTO> listado = new List<EntidadListadoDTO>();                

                switch (entidad)
                {
                    case EntidadSincroniza.Empresa:
                        roObtenerMaestroEmpresa 
                            roObtenerMaestroEmpresa = this.epSincronizaMaestros.obtenerMaestroEmpresas();

                        if (roObtenerMaestroEmpresa.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                        {
                            throw new ApplicationException(roObtenerMaestroEmpresa.codigoMensaje 
                                                           + " " +  roObtenerMaestroEmpresa.mensajeResultante);
                        }

                        ExcelDocument.GenerarFormatoMaestroEmpresa(nombreArchivo, roObtenerMaestroEmpresa.listaEmpresas);

                        break;
                    case EntidadSincroniza.UsuarioLibre:
                        roObtenerMaestroUsuarioLibre 
                            roObtenerMaestroUsuarioLibre = this.epSincronizaMaestros.obtenerMaestroUsuarioLibre();

                        if (roObtenerMaestroUsuarioLibre.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                        {
                            throw new ApplicationException(roObtenerMaestroUsuarioLibre.codigoMensaje 
                                                           + " " + roObtenerMaestroUsuarioLibre.mensajeResultante);
                        }

                        ExcelDocument.GenerarFormatoMaestroUsuarioLibre(nombreArchivo, roObtenerMaestroUsuarioLibre.listaUsuariosLibres);

                        break;
                    case EntidadSincroniza.Suministro:
                        roObtenerMaestroSuministroUsuario
                            roObtenerMaestroSuministroUsuario = this.epSincronizaMaestros.obtenerMaestroSuministroUsuario();

                        if (roObtenerMaestroSuministroUsuario.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                        {
                            throw new ApplicationException(roObtenerMaestroSuministroUsuario.codigoMensaje 
                                                           + " " + roObtenerMaestroSuministroUsuario.mensajeResultante);
                        }

                        ExcelDocument.GenerarFormatoMaestroSuministro(nombreArchivo, roObtenerMaestroSuministroUsuario.listaSuministrosUsuario);

                        break;
                    case EntidadSincroniza.Barra:
                        throw new ApplicationException("Funcionalidad no implementada");

                    case EntidadSincroniza.CentralGeneracion:
                        roObtenerMaestroCentralGeneracion
                            roObtenerMaestroCentralGeneracion = this.epSincronizaMaestros.obtenerMaestroCentralGeneracion();

                        if (roObtenerMaestroCentralGeneracion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                        {
                            throw new ApplicationException(roObtenerMaestroCentralGeneracion.codigoMensaje
                                                           + " " + roObtenerMaestroCentralGeneracion.mensajeResultante);
                        }

                        ExcelDocument.GenerarFormatoMaestroCentralGeneracion(nombreArchivo, roObtenerMaestroCentralGeneracion.listaCentrales);

                        break;
                    case EntidadSincroniza.GrupoGeneracion:
                        roObtenerMaestroGrupoGeneracion
                            roObtenerMaestroGrupoGeneracion = this.epSincronizaMaestros.obtenerMaestroGrupoGeneracion();

                        if (roObtenerMaestroGrupoGeneracion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                        {
                            throw new ApplicationException(roObtenerMaestroGrupoGeneracion.codigoMensaje
                                                           + " " + roObtenerMaestroGrupoGeneracion.mensajeResultante);
                        }

                        ExcelDocument.GenerarFormatoMaestroGrupoGeneracion(nombreArchivo, roObtenerMaestroGrupoGeneracion.listaGruposGeneracion);

                        break;
                    case EntidadSincroniza.ModoOperacion:
                        roObtenerMaestroModoOperacion
                            roObtenerMaestroModoOperacion = this.epSincronizaMaestros.obtenerMaestroModoOperacion();

                        if (roObtenerMaestroModoOperacion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                        {
                            throw new ApplicationException(roObtenerMaestroModoOperacion.codigoMensaje
                                                           + " " + roObtenerMaestroModoOperacion.mensajeResultante);
                        }

                        ExcelDocument.GenerarFormatoMaestroModoOperacion(nombreArchivo, roObtenerMaestroModoOperacion.listaModosOperacion);

                        break;
                    case EntidadSincroniza.Cuenca:
                        roObtenerMaestroCuenca
                            roObtenerMaestroCuenca = this.epSincronizaMaestros.obtenerMaestroCuenca();

                        if (roObtenerMaestroCuenca.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                        {
                            throw new ApplicationException(roObtenerMaestroCuenca.codigoMensaje
                                                           + " " + roObtenerMaestroCuenca.mensajeResultante);
                        }

                        ExcelDocument.GenerarFormatoMaestroCuenca(nombreArchivo, roObtenerMaestroCuenca.listaCuencas);

                        break;
                    case EntidadSincroniza.Embalse:
                        roObtenerMaestroEmbalse
                            roObtenerMaestroEmbalse = this.epSincronizaMaestros.obtenerMaestroEmbalse();

                        if (roObtenerMaestroEmbalse.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                        {
                            throw new ApplicationException(roObtenerMaestroEmbalse.codigoMensaje
                                                           + " " + roObtenerMaestroEmbalse.mensajeResultante);
                        }

                        ExcelDocument.GenerarFormatoMaestroEmbalse(nombreArchivo, roObtenerMaestroEmbalse.listaEmbalses);

                        break;
                    case EntidadSincroniza.Lago:
                        roObtenerMaestroLago
                            roObtenerMaestroLago = this.epSincronizaMaestros.obtenerMaestroLago();

                        if (roObtenerMaestroLago.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                        {
                            throw new ApplicationException(roObtenerMaestroLago.codigoMensaje
                                                           + " " + roObtenerMaestroLago.mensajeResultante);
                        }

                        ExcelDocument.GenerarFormatoMaestroLago(nombreArchivo, roObtenerMaestroLago.listaLago);

                        break;
                    default:
                        throw new ApplicationException("Funcionalidad no implementada");
                }

                return nombreArchivo;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public string ObtenerFechaUltSincronizacion()
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().GetFechaUltSincronizacion();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EntidadListadoDTO> ListResutado(string entidad, string pendiente)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ListResutado(entidad, pendiente);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public string SaveHomologacion(EntidadSincroniza entidad, string codigoCoes, string codigoOsinergmin, string mapencodi)
        {
            if (!Enum.IsDefined(typeof(EntidadSincroniza), entidad)) throw new ArgumentOutOfRangeException("entidad");
            try
            {
                string columna = this.ObtenerColumnaHomologacion(entidad).ToString();
                string query = "";

                switch (entidad)
                {
                    case EntidadSincroniza.Empresa:
                        query = "UPDATE SI_EMPRESA SET " + columna + " = '" + codigoOsinergmin + "' WHERE TIPOEMPRCODI IN (1,2,3) AND EMPRCODI = " + codigoCoes;
                        break;
                    case EntidadSincroniza.UsuarioLibre:
                        query = "UPDATE SI_EMPRESA SET " + columna + " = '" + codigoOsinergmin + "' WHERE  EMPRCODI = " + codigoCoes; //TIPOEMPRCODI = 4 AND
                        break;
                    case EntidadSincroniza.Suministro:
                        query = "UPDATE EQ_EQUIPO SET " + columna + " = '" + codigoOsinergmin + "' WHERE FAMCODI = 45 AND EQUICODI = " + codigoCoes;
                        break;
                    case EntidadSincroniza.Barra:
                        query = "UPDATE PR_BARRA SET " + columna + " = '" + codigoOsinergmin + "' WHERE BARRCODI = " + codigoCoes;
                        break;
                    //return "La homologación de la entidad Barra no se encuentra.";

                    case EntidadSincroniza.CentralGeneracion:
                        query = "UPDATE PR_GRUPO SET " + columna + " = '" + codigoOsinergmin + "' WHERE CATECODI IN (4,6) AND GRUPOCODI = " + codigoCoes;
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, codigoCoes);
                        break;
                    case EntidadSincroniza.GrupoGeneracion:
                        query = "UPDATE PR_GRUPO SET " + columna + " = '" + codigoOsinergmin + "' WHERE CATECODI IN (3,5) AND GRUPOCODI = " + codigoCoes;
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, codigoCoes);
                        break;
                    case EntidadSincroniza.ModoOperacion:
                        query = "UPDATE PR_GRUPO SET " + columna + " = '" + codigoOsinergmin + "' WHERE CATECODI IN (2,9) AND GRUPOCODI = " + codigoCoes;
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, codigoCoes);
                        break;
                    case EntidadSincroniza.Cuenca:
                        query = "UPDATE EQ_EQUIPO SET " + columna + " = '" + codigoOsinergmin + "' WHERE FAMCODI = 41 AND EQUICODI = " + codigoCoes;
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, codigoCoes);
                        break;
                    case EntidadSincroniza.Embalse:
                        query = "UPDATE EQ_EQUIPO SET " + columna + " = '" + codigoOsinergmin + "' WHERE FAMCODI = 19 AND EQUICODI = " + codigoCoes;
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, codigoCoes);
                        break;
                    case EntidadSincroniza.Lago:
                        query = "UPDATE EQ_EQUIPO SET " + columna + " = '" + codigoOsinergmin + "' WHERE FAMCODI = 47 AND EQUICODI = " + codigoCoes;
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, codigoCoes);

                        break;
                }

                return FactorySic.GetEntidadListadoRepository().SaveHomologacion(query, mapencodi);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public string ObtenerIdPendiente(string entidad, string codigo)
        {
            try
            {
                return FactorySic.GetEntidadListadoRepository().ObtenerIdPendiente(entidad, codigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public string DeleteHomologacion(EntidadSincroniza entidad, string codigo, string codigoOsinergmin)
        {
            if (!Enum.IsDefined(typeof(EntidadSincroniza), entidad)) throw new ArgumentOutOfRangeException("entidad");
            try
            {
                string rpta = "";
                string sqlQuery = "";

                EntidadListadoHelper helper = new EntidadListadoHelper();

                switch (entidad)
                {
                    case EntidadSincroniza.Empresa:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionEmpresa, codigo);                       
                        break;
                    case EntidadSincroniza.UsuarioLibre:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionUsuarioLibre, codigo);    
                        break;
                    case EntidadSincroniza.Suministro:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionSuministro, codigo);   
                        break;
                    case EntidadSincroniza.Barra:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionBarra, codigo);   
                        break;
                    case EntidadSincroniza.CentralGeneracion:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionCentralGeneracion, codigo);
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, null);
                        break;
                    case EntidadSincroniza.GrupoGeneracion:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionGrupoGeneracion, codigo);   
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, null);
                        break;
                    case EntidadSincroniza.ModoOperacion:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionModoOperacion, codigo);   
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, null);
                        break;
                    case EntidadSincroniza.Cuenca:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionCuenca, codigo);  
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, null);
                        break;
                    case EntidadSincroniza.Embalse:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionEmbalse, codigo); 
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, null);
                        break;
                    case EntidadSincroniza.Lago:
                        sqlQuery = string.Format(helper.SqlQuitarAsignacionLago, codigo);  
                        this.ActualizarVinculoEnOsinergmin(entidad, codigoOsinergmin, null);
                        break;
                }
                return FactorySic.GetEntidadListadoRepository().DeleteHomologacion(sqlQuery);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- JDEL Fin
        
    }
}