using COES.Base.Core;
using System.Globalization;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.CPPA.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using COES.Framework.Base.Core;
using System.Configuration;
using COES.Framework.Base.Tools;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenSeparatorHandlers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.EMMA;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using DocumentFormat.OpenXml.Presentation;
using Org.BouncyCastle.Crypto;
using COES.Servicios.Aplicacion.Transferencias;
using DocumentFormat.OpenXml.Drawing.Charts;
using iTextSharp.text.pdf;
using COES.Servicios.Aplicacion.PrimasRER;
using System.Windows;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using Microsoft.Win32;
using DocumentFormat.OpenXml.Bibliography;
using static COES.Servicios.Aplicacion.PotenciaFirme.ConstantesPotenciaFirmeRemunerable;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Drawing.Printing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using COES.Servicios.Aplicacion.Eventos;
using System.Security.RightsManagement;
using Color = System.Drawing.Color;
using COES.Servicios.Aplicacion.CortoPlazo;
using System.Web.Services.Description;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using MathNet.Numerics;

namespace COES.Servicios.Aplicacion.CPPA
{
    /// <summary>
    /// Clase Servicio de CPPA
    /// </summary>
    public class CPPAAppServicio : AppServicioBase
    {
        #region Variables Generales
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CPPAAppServicio));

        //Servicios
        ConsultaMedidoresAppServicio servicioConsultaMedidores = new ConsultaMedidoresAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        PrimasRERAppServicio servicioPrimasRER = new PrimasRERAppServicio();

        #endregion

        #region Extranet

        #endregion

        #region Intranet

        #region CU02 - Registrar Revisiones de los Ajustes del Año Presupuestal
        /// <summary>
        /// Permite listar el ajuste de anio presupuestal.
        /// </summary>
        /// <returns>Lista de RerSolicitudEdiDTO</returns>
        public List<CpaAjustePresupuestalDTO> ListaAjustePresupuestal()
        {
            List<CpaAjustePresupuestalDTO> lista = new List<CpaAjustePresupuestalDTO>();
            lista.Add(new CpaAjustePresupuestalDTO
            {
                Cpaapcodi = 1,
                Cpaapanio = 2024,
                Cpaapajuste = "A2",
                //Cpaaprevision = "Normal",
                //Cpaapestado = "Cerrado",
                Cpaapusucreacion = "camila.ayllon",
                Cpaapfeccreacion = DateTime.Now,
                //Cpaapusumodificacion = "camila.ayllon",
                //Cpaapfecmodificacion = DateTime.Now
            });

            return lista;
        }

        /// <summary>
        /// Obtiene una lista de revisiones, en base al filtro especificado
        /// </summary>
        /// <param name="cpaapaniofrom"></param>
        /// <param name="cpaapaniountil"></param>
        /// <param name="cparajuste"></param>
        /// <param name="cparestados">Ej: "'A'" o "'A','C'"</param>
        /// <returns></returns>
        public List<CpaRevisionDTO> ListarRevisiones(int cpaapaniofrom, int cpaapaniountil, string cparajuste, string cparestados)
        {
            try
            {
                if (cpaapaniountil < cpaapaniofrom)
                {
                    throw new Exception("La 'Fecha hasta' no debe ser menor a la 'Fecha desde'.");
                }

                List<CpaRevisionDTO> list = FactoryTransferencia.GetCpaRevisionRepository().GetByCriteria(cpaapaniofrom, cpaapaniountil, cparajuste, cparestados);
                bool existeList = (list != null && list.Count > 0);
                if (existeList)
                {
                    foreach (var reg in list)
                    {
                        FormatearCpaRevision(reg);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtener la siguiente revisión a crear en base al año presupuestal y a su ajuste respectivo
        /// </summary>
        /// <param name="cpaapanio"></param>
        /// <param name="cpaapajuste"></param>
        /// <returns></returns>
        public CpaRevisionDTO ObtenerNuevaRevision(int cpaapanio, string cpaapajuste)
        {
            try
            {
                int cparcmpmpo = Int32.Parse(string.Format("{0}{1}", cpaapanio - 1, "01"));
                string nombreRevision = ConstantesCPPA.revisionNormal;
                //List<CpaRevisionDTO> list = FactoryTransferencia.GetCpaRevisionRepository().GetByCriteria(cpaapanio, cpaapanio, cpaapajuste, ConstantesCPPA.estadoRevisionTodos);
                List<CpaRevisionDTO> list = FactoryTransferencia.GetCpaRevisionRepository().GetByCriteria(cpaapanio, cpaapanio, cpaapajuste, "'A','C'");
                bool existList = (list != null && list.Count > 0);

                if (existList)
                {
                    CpaRevisionDTO lastRevision = list.OrderBy(x => x.Cpaapanio).ThenBy(x => x.Cpaapajuste).ThenByDescending(x => x.Cparcodi).ThenByDescending(x => x.Cparcorrelativo).First();
                    //if (lastRevision.Cparestado == ConstantesCPPA.estadoRevisionAnulado)
                    //{
                    //    nombreRevision = lastRevision.Cparrevision;
                    //}
                    //else
                    //{
                    if (lastRevision.Cparrevision == ConstantesCPPA.revisionNormal)
                    {
                        nombreRevision = ConstantesCPPA.revisionRevision1;
                    }
                    else
                    {
                        int index = lastRevision.Cparrevision.IndexOf(' ') + 1;
                        int num = Int32.Parse(lastRevision.Cparrevision.Substring(index)) + 1;
                        nombreRevision = ConstantesCPPA.revisionRevision + " " + num;
                    }
                    //}
                }

                CpaRevisionDTO revision = new CpaRevisionDTO
                {
                    Cpaapanio = cpaapanio,
                    Cpaapajuste = cpaapajuste,
                    Cparrevision = nombreRevision,
                    Cparestado = ConstantesCPPA.estadoRevisionAbierto,
                    Cparcmpmpo = cparcmpmpo
                };

                return revision;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Crea una nueva revisión
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="usuario"></param>
        public void CrearNuevaRevision(CpaRevisionDTO revision, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validación
                string mensajeError = ValidarNuevaRevision(revision, out CpaRevisionDTO lastRevisionByAjusteByRevision);
                bool existError = !string.IsNullOrWhiteSpace(mensajeError);
                if (existError)
                {
                    throw new Exception(mensajeError);
                }
                #endregion

                #region Generar la Revision

                #region Inicializar variables
                conn = FactoryTransferencia.GetCpaRevisionRepository().BeginConnection();
                tran = FactoryTransferencia.GetCpaRevisionRepository().StartTransaction(conn);
                #endregion

                #region Obtener cpaapcodi y cparcorrelativo
                int cpaapcodi = 0;
                int cparcorrelativo = 1;
                if (lastRevisionByAjusteByRevision != null)
                {
                    cpaapcodi = lastRevisionByAjusteByRevision.Cpaapcodi;
                    cparcorrelativo = lastRevisionByAjusteByRevision.Cparcorrelativo + 1;
                }
                else
                {
                    List<CpaAjustePresupuestalDTO> listAP = FactoryTransferencia.GetCpaAjustePresupuestalRepository().GetByCriteria(revision.Cpaapanio);
                    bool existListAP = (listAP != null && listAP.Count > 0);
                    if (!existListAP)
                    {
                        int nextCpaapcodi = FactoryTransferencia.GetCpaAjustePresupuestalRepository().GetMaxId();
                        foreach (var ajuste in ConstantesCPPA.ajustesRevision)
                        {
                            DateTime date = DateTime.Now;
                            CpaAjustePresupuestalDTO ajustePresupuestal = new CpaAjustePresupuestalDTO
                            {
                                Cpaapcodi = nextCpaapcodi,
                                Cpaapanio = revision.Cpaapanio,
                                Cpaapanioejercicio = revision.Cpaapanio - 1,
                                Cpaapajuste = ajuste,
                                Cpaapusucreacion = usuario,
                                Cpaapfeccreacion = date
                            };

                            FactoryTransferencia.GetCpaAjustePresupuestalRepository().Save(ajustePresupuestal, conn, tran);
                            nextCpaapcodi++;

                            if (ajustePresupuestal.Cpaapanio == revision.Cpaapanio && ajustePresupuestal.Cpaapajuste == revision.Cpaapajuste)
                            {
                                cpaapcodi = ajustePresupuestal.Cpaapcodi;
                                cparcorrelativo = 1;
                            }
                        }
                    }
                    else
                    {
                        List<CpaAjustePresupuestalDTO> listAP1 = listAP.Where(x => x.Cpaapanio == revision.Cpaapanio && x.Cpaapajuste == revision.Cpaapajuste).ToList();
                        if (listAP1.Count > 0)
                        {
                            cpaapcodi = listAP1[0].Cpaapcodi;
                            cparcorrelativo = 1;
                        }
                    }
                }

                if (cpaapcodi < 1)
                {
                    throw new Exception("No se pudo asignar un valor a cpaapcodi.");
                }
                #endregion

                #region Declarar a las revisiones de su mismo ajuste que no son la última
                FactoryTransferencia.GetCpaRevisionRepository().UpdateUltimoByAnioByAjuste(ConstantesCPPA.ultimoNO, revision.Cpaapanio, revision.Cpaapajuste, conn, tran);
                #endregion

                #region Crear la revisión
                int cparcodi = FactoryTransferencia.GetCpaRevisionRepository().GetMaxId();
                DateTime fecha = DateTime.Now;
                revision.Cparcodi = cparcodi;
                revision.Cpaapcodi = cpaapcodi;
                revision.Cparcorrelativo = cparcorrelativo;
                revision.Cparultimo = ConstantesCPPA.ultimoSI;
                revision.Cparusucreacion = usuario;
                revision.Cparfeccreacion = fecha;
                FactoryTransferencia.GetCpaRevisionRepository().Save(revision, conn, tran);
                #endregion

                #region Crear el historial
                int cpahcodi = FactoryTransferencia.GetCpaHistoricoRepository().GetMaxId();
                CpaHistoricoDTO historico = new CpaHistoricoDTO
                {
                    Cpahcodi = cpahcodi,
                    Cparcodi = cparcodi,
                    Cpahtipo = ConstantesCPPA.tipoNuevo,
                    Cpahusumodificacion = usuario,
                    Cpahfecmodificacion = fecha
                };
                FactoryTransferencia.GetCpaHistoricoRepository().Save(historico, conn, tran);
                #endregion

                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Anular una revisión.
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="usuario"></param>
        public void AnularRevision(int cparcodi, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validación
                if (cparcodi < 0)
                {
                    throw new Exception("El código de la Revisión es inválido.");
                }
                if (string.IsNullOrWhiteSpace(usuario))
                {
                    throw new Exception("El usuario es requerido.");
                }
                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new Exception("No existe la revisión.");
                }
                if (revision.Cparestado == ConstantesCPPA.estadoRevisionAnulado)
                {
                    throw new Exception("La Revisión ya está Anulada.");
                }
                #endregion

                #region Generar actualización de estado

                #region Inicializar variables
                conn = FactoryTransferencia.GetCpaRevisionRepository().BeginConnection();
                tran = FactoryTransferencia.GetCpaRevisionRepository().StartTransaction(conn);
                #endregion

                #region Actualizar estado a anulado
                DateTime fecha = DateTime.Now;
                FactoryTransferencia.GetCpaRevisionRepository().UpdateEstado(revision.Cparcodi, ConstantesCPPA.estadoRevisionAnulado, usuario, fecha, conn, tran);
                #endregion

                #region Declarar a la última revisión (que su estado sea diferente a Anulado) de su mismo ajuste que también es última 
                List<CpaRevisionDTO> list = FactoryTransferencia.GetCpaRevisionRepository().GetByCriteria(revision.Cpaapanio, revision.Cpaapanio, revision.Cpaapajuste, "'A','C'");
                bool existList = (list != null && list.Count > 0);
                if (existList)
                {
                    list = list.Where(x => x.Cparcodi != cparcodi).ToList();
                    if (list.Count > 0)
                    {
                        list = list.OrderBy(x => x.Cpaapanio).ThenBy(x => x.Cpaapajuste).ThenByDescending(x => x.Cparcodi).ThenByDescending(x => x.Cparcorrelativo).ToList();
                        FactoryTransferencia.GetCpaRevisionRepository().UpdateUltimoByCodi(ConstantesCPPA.ultimoSI, list[0].Cparcodi, conn, tran);
                    }
                }
                #endregion

                #region Crear el historial
                int cpahcodi = FactoryTransferencia.GetCpaHistoricoRepository().GetMaxId();
                CpaHistoricoDTO historico = new CpaHistoricoDTO
                {
                    Cpahcodi = cpahcodi,
                    Cparcodi = cparcodi,
                    Cpahtipo = ConstantesCPPA.tipoAnular,
                    Cpahusumodificacion = usuario,
                    Cpahfecmodificacion = fecha
                };
                FactoryTransferencia.GetCpaHistoricoRepository().Save(historico, conn, tran);
                #endregion

                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }

        /// <summary>
        /// Actualiza estado y el CMg PMPO de una revisión. El estado debe ser diferente a anulado 
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cparestado"></param>
        /// <param name="cparcmpmpo"></param>
        /// <param name="usuario"></param>
        public void ActualizarEstadoYCMgPMPORevision(int cparcodi, string cparestado, int cparcmpmpo, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validación
                if (cparcodi < 0)
                {
                    throw new Exception("El código de la Revisión es inválido.");
                }
                if (string.IsNullOrWhiteSpace(cparestado))
                {
                    throw new Exception("El estado de la Revisión es requerido.");
                }
                if (!ConstantesCPPA.estadosRevision.Contains(cparestado))
                {
                    throw new Exception("El estado de la Revisión es inválido");
                }
                if (cparestado == ConstantesCPPA.estadoRevisionAnulado)
                {
                    throw new Exception("El estado a actualizar debe ser diferente a 'Anulado'");
                }
                if (string.IsNullOrWhiteSpace(usuario))
                {
                    throw new Exception("El usuario es requerido.");
                }
                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new Exception("No existe la revisión.");
                }
                if (revision.Cparestado == ConstantesCPPA.estadoRevisionAnulado)
                {
                    throw new Exception("La Revisión está Anulada. Por lo tanto, no se puede modificar su estado.");
                }
                RangoCparcmpmpo(revision.Cpaapanio, out int cparcmpmpoInicio, out int cparcmpmpoFin);
                if (cparcmpmpo < cparcmpmpoInicio || cparcmpmpo > cparcmpmpoFin)
                {
                    throw new Exception("El CMg PMPO debe estar dentro del rango de " + cparcmpmpoInicio + " a " + cparcmpmpoFin);
                }
                #endregion

                #region Generar actualización de estado

                #region Inicializar variables
                conn = FactoryTransferencia.GetCpaRevisionRepository().BeginConnection();
                tran = FactoryTransferencia.GetCpaRevisionRepository().StartTransaction(conn);
                #endregion

                #region Actualizar estado y costo margina pmpo
                DateTime fecha = DateTime.Now;
                FactoryTransferencia.GetCpaRevisionRepository().UpdateEstadoYCMgPMPO(cparcodi, cparestado, cparcmpmpo, usuario, fecha, conn, tran);
                #endregion

                #region Crear el historial
                int cpahcodi = FactoryTransferencia.GetCpaHistoricoRepository().GetMaxId();
                CpaHistoricoDTO historico = new CpaHistoricoDTO
                {
                    Cpahcodi = cpahcodi,
                    Cparcodi = cparcodi,
                    Cpahtipo = ConstantesCPPA.tipoEditar,
                    Cpahusumodificacion = usuario,
                    Cpahfecmodificacion = fecha
                };
                FactoryTransferencia.GetCpaHistoricoRepository().Save(historico, conn, tran);
                #endregion

                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }

        /// <summary>
        /// Obtiene el listorial de cambios de una revisión
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns></returns>
        public List<CpaHistoricoDTO> ListaHistorico(int cparcodi)
        {
            try
            {
                if (cparcodi < 0)
                {
                    throw new Exception("El código de la Revisión es inválido.");
                }

                List<CpaHistoricoDTO> list = FactoryTransferencia.GetCpaHistoricoRepository().GetByCriteria(cparcodi);
                bool existeList = (list != null && list.Count > 0);
                if (existeList)
                {
                    foreach (var reg in list)
                    {
                        FormatearCpaHistorico(reg);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los años con respecto a las Revisiones.
        /// Además, devuelve la lista de Revisiones
        /// </summary>
        /// <returns></returns>
        public List<GenericoDTO> ObtenerAnios(out List<CpaRevisionDTO> ListRevision)
        {
            try
            {
                List<GenericoDTO> listaAnio = new List<GenericoDTO>();
                ListRevision = ListarRevisiones(-1, -1, ConstantesCPPA.todos, ConstantesCPPA.estadoRevisionTodos);
                bool existListRevision = (ListRevision != null && ListRevision.Count > 0);
                if (existListRevision)
                {
                    foreach (var cpaapanio in ListRevision.Select(x => x.Cpaapanio).Distinct().OrderByDescending(x => x))
                    {
                        GenericoDTO reg = new GenericoDTO
                        {
                            Entero1 = cpaapanio,
                            String1 = cpaapanio.ToString()
                        };
                        listaAnio.Add(reg);
                    }
                }
                return listaAnio;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
        }

        /// <summary>
        /// Obtiene los años con respecto a las Revisiones con el estado especificado. 
        /// Además devuelve la lista de Revisiones con el estado especificado
        /// </summary>
        /// <returns></returns>
        public List<GenericoDTO> ObtenerAnios(string cparestados, out List<CpaRevisionDTO> ListRevision)
        {
            try
            {
                List<GenericoDTO> listaAnio = new List<GenericoDTO>();
                ListRevision = ListarRevisiones(-1, -1, ConstantesCPPA.todos, cparestados);
                bool existListRevision = (ListRevision != null && ListRevision.Count > 0);
                if (existListRevision)
                {
                    foreach (var cpaapanio in ListRevision.Select(x => x.Cpaapanio).Distinct().OrderByDescending(x => x))
                    {
                        GenericoDTO reg = new GenericoDTO
                        {
                            Entero1 = cpaapanio,
                            String1 = cpaapanio.ToString()
                        };
                        listaAnio.Add(reg);
                    }
                }
                return listaAnio;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
        }

        /// <summary>
        /// Formatea CpaRevision
        /// </summary>
        /// <param name="reg"></param>
        private static void FormatearCpaRevision(CpaRevisionDTO reg)
        {
            reg.CparestadoDesc = string.IsNullOrWhiteSpace(reg.Cparestado) ? "" :
                (reg.Cparestado == ConstantesCPPA.estadoRevisionAbierto ? ConstantesCPPA.descEstadoAbierto :
                (reg.Cparestado == ConstantesCPPA.estadoRevisionCerrado ? ConstantesCPPA.descEstadoCerrado :
                (reg.Cparestado == ConstantesCPPA.estadoRevisionAnulado ? ConstantesCPPA.descEstadoAnulado : "")));
            reg.CparfeccreacionDesc = reg.Cparfeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull2);
            reg.CparfecmodificacionDesc = (reg.Cparfecmodificacion != null) ? reg.Cparfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
        }

        /// <summary>
        /// Formatea CpaHistorico
        /// </summary>
        /// <param name="reg"></param>
        private static void FormatearCpaHistorico(CpaHistoricoDTO reg)
        {
            reg.CpahtipoDesc = string.IsNullOrWhiteSpace(reg.Cpahtipo) ? "" :
                (reg.Cpahtipo == ConstantesCPPA.tipoNuevo ? ConstantesCPPA.descTipoNuevo :
                (reg.Cpahtipo == ConstantesCPPA.tipoEditar ? ConstantesCPPA.descTipoEditar :
                (reg.Cpahtipo == ConstantesCPPA.tipoAnular ? ConstantesCPPA.descTipoAnular :
                (reg.Cpahtipo == ConstantesCPPA.tipoCopiarParametros ? ConstantesCPPA.descTipoCopiarParametros : ""))));
            reg.CpahfecmodificacionDesc = reg.Cpahfecmodificacion.ToString(ConstantesAppServicio.FormatoFechaFull2);
        }

        /// <summary>
        /// Obtiene rango de valores para el dato cparcmpmpo
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="cparcmpmpoInicio"></param>
        /// <param name="cparcmpmpoFin"></param>
        private static void RangoCparcmpmpo(int anio, out int cparcmpmpoInicio, out int cparcmpmpoFin)
        {
            cparcmpmpoInicio = Int32.Parse(string.Format("{0}{1}", anio - 1, "01"));
            cparcmpmpoFin = Int32.Parse(string.Format("{0}{1}", anio + 1, "12"));
        }

        /// <summary>
        /// Valida los datos de una Revisión, según la operación que se desea realizar con esta.
        /// </summary>
        /// <param name="revision"></param>        
        /// <param name="lastRevisionByAjusteByRevision"></param>        
        /// <returns>Retorna un mensaje de error, en caso no pase la validación</returns>
        private string ValidarNuevaRevision(CpaRevisionDTO revision, out CpaRevisionDTO lastRevisionByAjusteByRevision)
        {
            try
            {
                #region Validar datos ingresados
                lastRevisionByAjusteByRevision = null;
                if (revision == null)
                {
                    return "No se ha enviado ninguna revisión para ser creada.";
                }

                StringBuilder sb = new StringBuilder();

                if (revision.Cpaapanio < 2025)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("El año no puede ser menor a 2025");
                }
                if (string.IsNullOrWhiteSpace(revision.Cpaapajuste))
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("El ajuste es requerido");
                }
                else
                {
                    revision.Cpaapajuste = revision.Cpaapajuste.Trim();
                    if (!(ConstantesCPPA.ajustesRevision.Contains(revision.Cpaapajuste)))
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.Append("El ajuste es inválido");
                    }
                }
                if (string.IsNullOrWhiteSpace(revision.Cparrevision))
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("El nombre de la revisión es requerido");
                }
                revision.Cparrevision = revision.Cparrevision.Trim();
                if (string.IsNullOrWhiteSpace(revision.Cparestado))
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("El estado es requerido");
                }
                else
                {
                    revision.Cparestado = revision.Cparestado.Trim();
                    if (!(ConstantesCPPA.estadosRevision.Contains(revision.Cparestado)))
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.Append("El estado es inválido");
                    }
                }
                RangoCparcmpmpo(revision.Cpaapanio, out int cparcmpmpoInicio, out int cparcmpmpoFin);
                if (revision.Cparcmpmpo < cparcmpmpoInicio || revision.Cparcmpmpo > cparcmpmpoFin)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("El CMg PMPO debe estar dentro del rango de ");
                    sb.Append(cparcmpmpoInicio);
                    sb.Append(" a ");
                    sb.Append(cparcmpmpoFin);
                }

                if (sb.Length > 0)
                {
                    return sb.ToString();
                }
                #endregion

                #region Validar datos ingresados con lógica de negocio

                #region En caso no existan registros para el filtro especificado
                List<CpaRevisionDTO> list = FactoryTransferencia.GetCpaRevisionRepository().GetByCriteria(revision.Cpaapanio - 1, revision.Cpaapanio, ConstantesCPPA.todos, ConstantesCPPA.estadoRevisionTodos);
                bool existList = (list != null && list.Count > 0);
                if (!existList)
                {
                    int index = Array.IndexOf(ConstantesCPPA.ajustesRevision, revision.Cpaapajuste);
                    string ajuste = index == 0 ? ConstantesCPPA.A1 : ConstantesCPPA.ajustesRevision[index - 1];
                    int anio = index == 0 ? revision.Cpaapanio - 1 : revision.Cpaapanio;

                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("No existe la Revisión 'Normal' del Ajuste '");
                    sb.Append(ajuste);
                    sb.Append("' del Año Presupuestal '");
                    sb.Append(anio);
                    sb.Append("'");

                    return sb.ToString();
                }
                #endregion

                #region Validar que no exista otra revisión igual con estado Abierto o Cerrado
                List<CpaRevisionDTO> listDuplicate = list.Where(x =>
                        x.Cpaapanio == revision.Cpaapanio &&
                        x.Cpaapajuste == revision.Cpaapajuste &&
                        x.Cparrevision == revision.Cparrevision &&
                        x.Cparestado != ConstantesCPPA.estadoRevisionAnulado
                ).ToList();
                if (listDuplicate.Count > 0)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("Ya existe dicha revisión en la base de datos");
                    return sb.ToString();
                }
                #endregion

                #region Validar que exista la Revisión 'Normal' para su Ajuste anterior
                bool isRevisionInitial = (revision.Cpaapanio == ConstantesCPPA.number2025 && revision.Cpaapajuste == ConstantesCPPA.A1 && revision.Cparrevision == ConstantesCPPA.revisionNormal);
                if (!isRevisionInitial)
                {
                    int index = Array.IndexOf(ConstantesCPPA.ajustesRevision, revision.Cpaapajuste);
                    string ajuste = index == 0 ? ConstantesCPPA.A1 : ConstantesCPPA.ajustesRevision[index - 1];
                    int anio = revision.Cpaapanio == ConstantesCPPA.number2025 || index > 0 ? revision.Cpaapanio : revision.Cpaapanio - 1;

                    List<CpaRevisionDTO> listRevisionBefore = list.Where(x => x.Cpaapanio == anio && x.Cpaapajuste == ajuste && x.Cparrevision == ConstantesCPPA.revisionNormal && x.Cparestado != ConstantesCPPA.estadoRevisionAnulado).ToList();
                    if (listRevisionBefore.Count < 1)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.Append("No existe la Revisión 'Normal' del Ajuste '");
                        sb.Append(ajuste);
                        sb.Append("' del Año Presupuestal '");
                        sb.Append(anio);
                        sb.Append("'");

                        return sb.ToString();
                    }
                }
                #endregion

                #region Obtener lastRevisionByAjusteByRevision
                List<CpaRevisionDTO> listRev = list.Where(x => x.Cpaapanio == revision.Cpaapanio && x.Cpaapajuste == revision.Cpaapajuste && x.Cparrevision == revision.Cparrevision).ToList();
                if (listRev.Count > 0)
                {
                    listRev = listRev.OrderBy(x => x.Cpaapanio).ThenBy(x => x.Cpaapajuste).ThenByDescending(x => x.Cparcodi).ThenByDescending(x => x.Cparcorrelativo).ToList();
                    lastRevisionByAjusteByRevision = listRev[0];
                }
                #endregion

                #endregion

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        #endregion

        #region CU03 - Registrar Empresas Integrantes
        /// <summary>
        /// Método que genera un reporte excel simple
        /// </summary>
        public string ExportarReporteSimple(CPPAFormatoExcel formato, string pathFile, string filename)
        {
            string Reporte = filename + ".xlsx";
            ExcelDocumentCPPA.GenerarArchivoExcel(formato, pathFile + Reporte);

            return Reporte;
        }

        /// <summary>
        /// Método que genera un reporte excel simple con libros
        /// </summary>
        public string ExportarReporteConLibros(List<PrnFormatoExcel> formatos, string pathFile, string filename)
        {
            //string Reporte = filename + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";
            string Reporte = filename + ".xlsx";
            ExcelDocumentCPPA.GenerarArchivoExcelConLibros(formatos, pathFile + Reporte);

            return Reporte;
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_AJUSTEPRESUPUESTAL
        /// </summary>
        public void UpdateEstadoEmpresaIntegrante(CpaEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaEmpresaRepository().UpdateEstadoEmpresaIntegrante(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_AJUSTEPRESUPUESTAL
        /// </summary>
        public void UpdateAuditoriaEmpresaIntegrante(CpaEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaEmpresaRepository().UpdateAuditoriaEmpresaIntegrante(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_CENTRAL
        /// </summary>
        public void UpdateEstadoCentralIntegrante(CpaCentralDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCentralRepository().UpdateEstadoCentralIntegrante(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// LIsta las un join entre la tabla CPA_EMPRESA y SI_EMPRESA
        /// </summary>
        /// <param name="revision">El ID de la revision, cparcodi.</param>
        /// <param name="estado">El estado de la empresa registrada.</param>
        /// <param name="tipo">El tipo de la empresa registrada.</param>
        /// <returns></returns>
        public List<CpaEmpresaDTO> ListaEmpresasIntegrantes(int revision, string estado, string tipo)
        {
            return FactoryTransferencia.GetCpaEmpresaRepository()
                                        .ListaEmpresasIntegrantes(revision, estado, tipo);
        }

        /// <summary>
        /// Permite armar el registo a insertar en la tabla CPA_EMPRESA
        /// </summary>
        /// <param name="revision">El ID de la revision, cparcodi.</param>
        /// <param name="empresa">Identificador de la empresa.</param>
        /// <param name="user">Usuario que realiza el registro.</param>
        /// <param name="tipo">Tipo de la empresa</param>
        /// <returns></returns>
        public void RegistraEmpresaIntegrante(int revision, int empresa, string user, string tipo) {
            try
            {
                CpaEmpresaDTO entity = new CpaEmpresaDTO
                {
                    Cparcodi = revision,
                    Emprcodi = empresa,
                    Cpaemptipo = tipo,
                    Cpaempestado = ConstantesCPPA.eActivo,
                    Cpaempusucreacion = user,
                    Cpaempfeccreacion = DateTime.Now
                };

                this.SaveCpaEmpresa(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Permite actualizar el estado(Activo -> Anulado) de la tabla CPA_EMPRESA
        /// </summary>
        /// <param name="codigo">Identificador de la tabla CPA_EMPRESA.</param>
        /// <param name="empresa">Identificado de la tabla SI_EMPRESA.</param>
        /// <param name="revision">Identificado de la tabla CPA_REVISION.</param>
        /// <param name="tipo">Indica el tipo generadora, distribuidora, etc.</param>
        /// <param name="user">Usuario que realiza el registro.</param>
        /// <returns></returns>
        public object AnulaEmpresaIntegrante(int codigo, int empresa, int revision, string tipo, string user)
        {
            string dataMsg = string.Empty;
            string typeMsg = string.Empty;

            CpaEmpresaDTO emp1 = this.GetByIdCpaEmpresa(codigo);
            List<CpaEmpresaDTO> emp = this.ListaEmpresasIntegrantes(revision, ConstantesCPPA.eAnulado, $"'{tipo}'").Where(x => x.Emprcodi == empresa).ToList();
            List<CpaCentralDTO> listaCentrales = FactoryTransferencia.GetCpaCentralRepository().ListaCentralesByEmpresa(codigo);

            if (emp.Count > 0)
            {
                dataMsg = "No puede anular la empresa porque ya existe un registro anulado de esta...";
                typeMsg = ConstantesCPPA.MsgWarning;

                return new { dataMsg, typeMsg };
            }

            try
            {
                CpaEmpresaDTO entEmpresa = new CpaEmpresaDTO
                {
                    Cpaempcodi = codigo,
                    Cpaempestado = ConstantesCPPA.eAnulado,
                    Cpaempusumodificacion = user,
                    Cpaempfecmodificacion = DateTime.Now
                };
                this.UpdateEstadoEmpresaIntegrante(entEmpresa);

                foreach (CpaCentralDTO cnt in listaCentrales) {
                    
                    CpaCentralDTO entCentral = new CpaCentralDTO
                    {
                        Cpacntcodi = cnt.Cpacntcodi,
                        Cpacntestado = ConstantesCPPA.eAnulado,
                        Cpacntusumodificacion = user,
                        Cpacntfecmodificacion = DateTime.Now
                    };
                    this.UpdateEstadoCentralIntegrante(entCentral);
                }

                dataMsg = "El registro fue anulado...";
                typeMsg = ConstantesCPPA.MsgSuccess;

                return new { dataMsg, typeMsg };
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Lista las un join entre la tabla CPA_CENTRAL y EQ_EQUIPO
        /// </summary>
        /// <param name="empresa">El ID de la revision, cparcodi.</param>
        /// <param name="revision">El ID de la revision, cparcodi.</param>
        /// <param name="estado">El estado de la central registrada.</param>
        /// <returns></returns>
        public List<CpaCentralDTO> ListaCentralesIntegrantes(int empresa, int revision, string estado)
        {
            return FactoryTransferencia.GetCpaCentralRepository()
                                        .ListaCentralesIntegrantes(empresa, revision, estado);
        }


        /// <summary>
        /// Permite armar el registo a insertar en la tabla CPA_CENTRAL
        /// </summary>
        /// <param name="anio">Anio presupuestal</param>
        /// <param name="empresa">Identificador de la tabla CPA_EMPRESA.</param>
        /// <param name="revision">El ID de la revision, cparcodi.</param>
        /// <param name="central">Identificador de la central.</param>
        /// <param name="ajuste">Tipo de ajuste</param>
        /// <param name="user">Usuario que realiza el registro.</param>
        /// <returns></returns>
        public void RegistraCentralIntegrante(int anio, int empresa, int revision, int central, string ajuste, string user)
        {

            try
            {
                CpaCentralDTO entity = new CpaCentralDTO
                {
                    Cpaempcodi = empresa,
                    Cparcodi = revision,
                    Equicodi = central,
                    Cpacntestado = ConstantesCPPA.eActivo,
                    Cpacnttipo = ConstantesCPPA.tipoNormal,
                    Cpacntfecejecinicio = new DateTime(anio, 1, 1),
                    Cpacntfecejecfin = (ajuste == ConstantesCPPA.tipoA1) ? new DateTime(anio, 8, 31) : new DateTime(anio, 12, 31),
                    Cpacntfecproginicio = (ajuste == ConstantesCPPA.tipoA1) ? new DateTime(anio, 9, 1) : (DateTime?)null,
                    Cpacntfecprogfin = (ajuste == ConstantesCPPA.tipoA1) ? new DateTime(anio, 12, 31) : (DateTime?)null,
                    Cpacntusucreacion = user,
                    Cpacntfeccreacion = DateTime.Now
                };

                this.SaveCpaCentral(entity);

                //Agregar update a Empresa
                CpaEmpresaDTO emp = new CpaEmpresaDTO
                {
                    Cpaempcodi = empresa,
                    Cpaempusumodificacion = user,
                    Cpaempfecmodificacion = DateTime.Now
                };
                this.UpdateAuditoriaEmpresaIntegrante(emp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Permite actualizar el estado(Activo -> Anulado) de la tabla CPA_CENTRAL
        /// </summary>
        /// <param name="central">Identificador de la tabla CPA_CENTRAL.</param>
        /// <param name="empresa">Identificador de la tabla CPA_EMPRESA.</param>
        /// <param name="user">Usuario que realiza el registro.</param>
        /// <returns></returns>
        public void AnulaCentralIntegrante(int central, int empresa, string user)
        {

            try
            {
                //Registra la central
                CpaCentralDTO entity = new CpaCentralDTO
                {
                    Cpacntcodi = central,
                    Cpacntestado = ConstantesCPPA.eAnulado,
                    Cpacntusumodificacion = user,
                    Cpacntfecmodificacion = DateTime.Now
                };
                this.UpdateEstadoCentralIntegrante(entity);

                //Actualiza los campos de empresa
                CpaEmpresaDTO entityEmpresa = new CpaEmpresaDTO
                {
                    Cpaempcodi = empresa,
                    Cpaempusumodificacion = user,
                    Cpaempfecmodificacion = DateTime.Now
                };
                this.UpdateAuditoriaEmpresaIntegrante(entityEmpresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Permite armar el registo a insertar en la tabla CPA_CENTRAL
        /// </summary>
        /// <param name="revision">Identificador de la revision</param>
        /// <param name="ajuste">Ajuste de la revision</param>
        /// <param name="id">Identificador de la tabla CPA_CENTRAL</param>
        /// <param name="transferencia">Identificador de la barra de transferencia.</param>
        /// <param name="barraPMPO">Identificador de la barra PMPO</param>
        /// <param name="centralesPMPO">Identificadores de las centrales PMPO.</param>
        /// <param name="ejecInicio">Fecha ejecutada inicio</param>
        /// <param name="ejecFin">Fecha ejecutada fin</param>
        /// <param name="progInicio">Fecha programada inicio</param>
        /// <param name="progFin">Fecha programda fin</param>
        /// <param name="user">Usuario que realiza el registro.</param>
        /// <returns></returns>
        public void RegistraCentralPMPO(int revision, string ajuste, int id, int transferencia, int barraPMPO, List<MePtomedicionDTO> centralesPMPO, string ejecInicio, string ejecFin, string progInicio, string progFin, string user)
        {

            try
            {
                DateTime? eInicio = string.IsNullOrEmpty(ejecInicio) ? (DateTime?)null : DateTime.ParseExact(ejecInicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime? eFin = string.IsNullOrEmpty(ejecFin) ? (DateTime?)null : DateTime.ParseExact(ejecFin, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime? pInicio = string.IsNullOrEmpty(progInicio) ? (DateTime?)null : DateTime.ParseExact(progInicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime? pFin = string.IsNullOrEmpty(progFin) ? (DateTime?)null : DateTime.ParseExact(progFin, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture);

                string tp = this.AnalisisFechasNormalEspecial(ajuste, eInicio, eFin, pInicio, pFin);

                CpaCentralDTO entityCentral = new CpaCentralDTO
                {
                    Cpacntcodi = id,
                    Barrcodi = transferencia,
                    Ptomedicodi = barraPMPO,
                    Cpacnttipo = tp,
                    Cpacntfecejecinicio = eInicio,
                    Cpacntfecejecfin = eFin,
                    Cpacntfecproginicio = pInicio,
                    Cpacntfecprogfin = pFin,
                    Cpacntusumodificacion = user,
                    Cpacntfecmodificacion = DateTime.Now
                };

                this.UpdateCpaCentralPMPO(entityCentral);

                //lista las centrales pmpo por el codigo de CPA_CENTRAL
                List<CpaCentralPmpoDTO> pmpo = this.ListCpaCentralPmpobyCentral(id);
                if (pmpo.Count() > 0)
                {
                    foreach (CpaCentralPmpoDTO item in pmpo)
                    {
                        this.DeleteCpaCentralPmpo(item.Cpacnpcodi);
                    }
                    if (centralesPMPO != null) {
                        foreach (MePtomedicionDTO item in centralesPMPO)
                        {
                            CpaCentralPmpoDTO entityPMPO = new CpaCentralPmpoDTO
                            {
                                Cpacntcodi = id,
                                Cparcodi = revision,
                                Ptomedicodi = item.Ptomedicodi,
                                Cpacnpusumodificacion = user,
                                Cpacnpfecmodificacion = DateTime.Now
                            };
                            this.SaveCpaCentralPmpo(entityPMPO);
                        }
                    }
                }
                else {
                    if (centralesPMPO != null) {
                        foreach (MePtomedicionDTO item in centralesPMPO)
                        {
                            CpaCentralPmpoDTO entityPMPO = new CpaCentralPmpoDTO
                            {
                                Cpacntcodi = id,
                                Cparcodi = revision,
                                Ptomedicodi = item.Ptomedicodi,
                                Cpacnpusumodificacion = user,
                                Cpacnpfecmodificacion = DateTime.Now
                            };
                            this.SaveCpaCentralPmpo(entityPMPO);
                        }
                    }
                }

                //Agregar update a Empresa
                CpaCentralDTO cen = this.GetByIdCpaCentral(id);
                CpaEmpresaDTO emp = new CpaEmpresaDTO
                {
                    Cpaempcodi = cen.Cpaempcodi,
                    Cpaempusumodificacion = user,
                    Cpaempfecmodificacion = DateTime.Now
                };
                this.UpdateAuditoriaEmpresaIntegrante(emp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }
        #endregion

        #region CU04 - Registrar parámetros para las Centrales de Generación
        /// <summary>
        /// Lista de centrales PMPO para una empresa
        /// </summary>
        /// <param name="empresa">Identificador de la empresa.</param>
        /// <param name="central">Identificador de la central.</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListaCentralesFiltradasPMPO(int empresa, int central)
        {
            List<MePtomedicionDTO> medicion = this.ListaCentralesPMPO(empresa);
            List<CpaCentralPmpoDTO> pmpo = this.ListCpaCentralPmpobyCentral(central);

            List<MePtomedicionDTO> medicionNoRegistradas = medicion
        .Where(e => !pmpo.Any(reg => reg.Ptomedicodi == e.Ptomedicodi))
        .ToList();

            return medicionNoRegistradas;
        }


        #endregion

        #region CU05 - Registrar parámetros por cada mes
        /// <summary>
        /// Registra los parametros
        /// </summary>
        /// <param name="revision">Identificador de la empresa.</param>
        /// <param name="anio">anio.</param>
        /// <param name="mes">mes</param>
        /// <param name="registro">Indica si es proyecto o ejecutado</param>
        /// <param name="fecha">fecha.</param>
        /// <param name="hora">hora.</param>
        /// <param name="cambio">tipo de cambio.</param>
        /// <param name="precio">precio.</param>
        /// <param name="user">usuario que realiza el registro.</param>
        /// <returns></returns>
        public object RegistrarParametros(int revision, int anio, int mes, string registro, string fecha, string hora, decimal cambio, decimal precio, string user)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            List<CpaParametroDTO> result = this.ListaParametrosByRevisionAnioMesEstado(revision, anio, mes, ConstantesCPPA.eActivo);
            if (result.Count > 0) {
                dataMsg = "Ya existe un registro activo para la revision, mes y año seleccionados.";
                typeMsg = ConstantesCPPA.MsgWarning;
                return new { typeMsg, dataMsg };
            }

            string fechaHoraString = $"{fecha} {hora}";
            DateTime dateTime = DateTime.ParseExact(fechaHoraString, ConstantesCPPA.FormatoFechaHora, CultureInfo.InvariantCulture);

            int anioFecha = dateTime.Year;
            if (anioFecha != anio)
            {
                dataMsg = "La fecha MD no pertenece al año seleccionado";
                typeMsg = ConstantesCPPA.MsgWarning;
                return new { typeMsg, dataMsg };
            }

            int mesFecha = dateTime.Month;
            if (mesFecha != mes)
            {
                dataMsg = "La fecha MD no pertenece al mes seleccionado";
                typeMsg = ConstantesCPPA.MsgWarning;
                return new { typeMsg, dataMsg };
            }

            CpaParametroDTO parametro = new CpaParametroDTO
            {
                Cparcodi = revision,
                Cpaprmanio = anio,
                Cpaprmmes = mes,
                Cpaprmtipomd = registro,
                Cpaprmfechamd = dateTime,//DateTime.ParseExact(formatoDeseado, ConstantesCPPA.FormatoFechaHora, CultureInfo.InvariantCulture),
                Cpaprmcambio = cambio,
                Cpaprmprecio = precio,
                Cpaprmestado = ConstantesCPPA.eActivo,
                Cpaprmusucreacion = user,
                Cpaprmfeccreacion = DateTime.Now
            };
            int cpaprmcodi = this.SaveCpaParametro(parametro);

            CpaParametroHistoricoDTO historico = new CpaParametroHistoricoDTO
            {
                Cpaprmcodi = cpaprmcodi,
                Cpaphstipo = ConstantesCPPA.accionNuevo,
                Cpaphsusuario = user,
                Cpaphsfecha = DateTime.Now
            };
            this.SaveCpaParametroHistorico(historico);

            dataMsg = "El registro se grabó de manera exitosa.";
            typeMsg = ConstantesCPPA.MsgSuccess;

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <param name="codigo">Identificador de la tabla CPA_PARAMETRO</param>
        /// <param name="anio">anio.</param>
        /// <param name="mes">mes.</param>
        /// <param name="registro">Indica si es proyectado o ejecutado.</param>
        /// <param name="fecha">fecha.</param>
        /// <param name="hora">hora.</param>
        /// <param name="cambio">tipo de cambio.</param>
        /// <param name="precio">precio.</param>
        /// <param name="user">usuario que realiza los cambios.</param>
        /// <returns></returns>
        public object EditarParametros(int codigo, int anio, int mes, string registro, string fecha, string hora, decimal cambio, decimal precio, string user)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            string fechaHoraString = $"{fecha} {hora}";
            DateTime dateTime = DateTime.ParseExact(fechaHoraString, ConstantesCPPA.FormatoFechaHora, CultureInfo.InvariantCulture);

            int anioFecha = dateTime.Year;
            if (anioFecha != anio)
            {
                dataMsg = "La fecha MD no pertenece al año seleccionado";
                typeMsg = ConstantesCPPA.MsgWarning;
                return new { typeMsg, dataMsg };
            }

            int mesFecha = dateTime.Month;
            if (mesFecha != mes)
            {
                dataMsg = "La fecha MD no pertenece al mes seleccionado";
                typeMsg = ConstantesCPPA.MsgWarning;
                return new { typeMsg, dataMsg };
            }

            CpaParametroDTO parametro = new CpaParametroDTO
            {
                Cpaprmcodi = codigo,
                Cpaprmtipomd = registro,
                Cpaprmfechamd = dateTime,
                Cpaprmcambio = cambio,
                Cpaprmprecio = precio,
                Cpaprmusumodificacion = user,
                Cpaprmfecmodificacion = DateTime.Now
            };
            this.UpdateCpaParametroTipoYCambio(parametro);

            CpaParametroHistoricoDTO historico = new CpaParametroHistoricoDTO
            {
                Cpaprmcodi = codigo,
                Cpaphstipo = ConstantesCPPA.accionEditar,
                Cpaphsusuario = user,
                Cpaphsfecha = DateTime.Now
            };
            this.SaveCpaParametroHistorico(historico);

            dataMsg = "Todo correcto.";
            typeMsg = ConstantesCPPA.MsgSuccess;

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Anula los parametros, cambia el estado CPAPRMESTADO de A a X
        /// </summary>
        /// <param name="codigo">Identificador de la tabla CPA_PARAMETRO</param>
        /// <param name="user">usuario que realiza los cambios.</param>
        /// <returns></returns>
        public void AnularParametros(int codigo, string user)
        {
            CpaParametroDTO parametro = new CpaParametroDTO
            {
                Cpaprmcodi = codigo,
                Cpaprmestado = ConstantesCPPA.eAnulado,
                Cpaprmusumodificacion = user,
                Cpaprmfecmodificacion = DateTime.Now
            };
            this.UpdateCpaParametroEstado(parametro);

            CpaParametroHistoricoDTO historico = new CpaParametroHistoricoDTO
            {
                Cpaprmcodi = codigo,
                Cpaphstipo = ConstantesCPPA.accionAnular,
                Cpaphsusuario = user,
                Cpaphsfecha = DateTime.Now
            };
            this.SaveCpaParametroHistorico(historico);
        }

        /// <summary>
        /// Permite copiar a la tabla CPA_PARAMETRO de LVTP
        /// </summary>
        /// <param name="revision">Identificador de la tabla CPA_PARAMETRO</param>
        /// <param name="anio">usuario que realiza los cambios.</param>
        /// <param name="user">usuario que realiza los cambios.</param>
        /// <returns></returns>
        public string CopiarMDLVTP(int revision, int anio, string user) {
            List<VtpRecalculoPotenciaDTO> vtps = this.ListRecalculoByPeriodo(anio);
            string formattedValue = $"'{ConstantesCPPA.eActivo}'";
            List<CpaParametroDTO> parametros = this.ListaParametrosRegistrados(revision, formattedValue, anio);
            if (vtps.Count == 0) {
                return "No se encontraron datos LVTP para el anio: " + anio;
            }
            List<CpaParametroDTO> parametrosAnular = parametros
            .Where(e => vtps.Any(reg => reg.Perimes == e.Cpaprmmes))
            .ToList();

            CpaParametroHistoricoDTO historico = new CpaParametroHistoricoDTO();
            CpaParametroDTO nuevoParametro = new CpaParametroDTO();

            if (parametrosAnular.Count > 0) {
                foreach (var item in parametrosAnular)
                {
                    //Actualiza el estado = Anulado de CPA_PARAMETRO.
                    item.Cpaprmestado = ConstantesCPPA.accionAnular;
                    item.Cpaprmusumodificacion = user;
                    item.Cpaprmfecmodificacion = DateTime.Now;
                    this.UpdateCpaParametroEstado(item);

                    //Registra en el historico la eliminacion
                    //del registro en CPA_PARAMETRO_HISTORICO.
                    historico = new CpaParametroHistoricoDTO();
                    historico.Cpaprmcodi = item.Cpaprmcodi;
                    historico.Cpaphstipo = ConstantesCPPA.accionAnular;
                    historico.Cpaphsusuario = user;
                    historico.Cpaphsfecha = DateTime.Now;
                    this.SaveCpaParametroHistorico(historico);
                }
            }

            if (vtps.Count > 0) {
                foreach (var item in vtps)
                {
                    //Rgistra los datos de LVTP en CPA_PARAMETRO
                    string fechaHoraFormateada = item.Recpotinterpuntames?.ToString(ConstantesCPPA.FormatoFechaHora) ?? "Fecha no disponible";

                    nuevoParametro = new CpaParametroDTO
                    {
                        Cparcodi = revision,
                        Cpaprmanio = item.Perianio,
                        Cpaprmmes = item.Perimes,
                        Cpaprmtipomd = ConstantesCPPA.tipoRegistroMDEjecutado,
                        Cpaprmfechamd = DateTime.ParseExact(fechaHoraFormateada, ConstantesCPPA.FormatoFechaHora, CultureInfo.InvariantCulture),
                        Cpaprmcambio = 0,
                        Cpaprmprecio = Convert.ToDecimal(item.Recpotpreciopoteppm),
                        Cpaprmestado = ConstantesCPPA.eActivo,
                        Cpaprmusucreacion = user,
                        Cpaprmfeccreacion = DateTime.Now
                    };
                    int cpaprmcodi = this.SaveCpaParametro(nuevoParametro);

                    //Registra la insercion de LVTP en CPA_PARAMETRO_HISTORICO
                    historico = new CpaParametroHistoricoDTO();
                    historico.Cpaprmcodi = cpaprmcodi;
                    historico.Cpaphstipo = ConstantesCPPA.accionNuevo;
                    historico.Cpaphsusuario = user;
                    historico.Cpaphsfecha = DateTime.Now;
                    this.SaveCpaParametroHistorico(historico);
                }
            }

            return "Los datos se copiaron correctamente...";
        }
        #endregion

        #region CU06 - Copiar parámetros de una Revisión
        /// <summary>
        /// Anula los parametros, cambia el estado CPAPRMESTADO de A a X
        /// </summary>
        /// <param name="revisionHasta">Identificador de la revison donde se copiara la estructura de la revisionDesde</param>
        /// <param name="anioHasta">Anio destino</param>
        /// <param name="ajusteHasta">Ajuste destino</param>
        /// <param name="revisionDesde">Identificador de la revision que proporciona la estructura</param>
        /// <param name="anioDesde">Anio desde donde se copia la informacion</param>
        /// <param name="ajusteDesde">Ajuste desde donde se copia la informacion</param>
        /// <param name="user">Usuario que realiza el copiado</param>
        /// <returns></returns>
        public object CopiarEstructuraIntegrante(int revisionHasta, int anioHasta, string ajusteHasta, int revisionDesde, int anioDesde, string ajusteDesde, string user)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            var tipos = new List<string> { ConstantesCPPA.tipoEmpresaGeneradora, ConstantesCPPA.tipoEmpresaDistribuidora, ConstantesCPPA.tipoEmpresaUsuarioLibre, ConstantesCPPA.tipoEmpresaTransmisora };
            string tipoTodos = string.Join(",", tipos.Select(t => $"'{t}'"));

            List<CpaEmpresaDTO> empIntDesde = this.ListaEmpresasIntegrantes(revisionDesde, ConstantesCPPA.eActivo, tipoTodos);

            List<CpaEmpresaDTO> empIntHasta = this.ListaEmpresasIntegrantes(revisionHasta, ConstantesCPPA.eActivo, tipoTodos);

            List<CpaCentralDTO> centralesIntegrantes = this.ListaCentralesEmpresasParticipantes(revisionDesde, -1, -1, -1);

            string cpacntcodis = string.Join(",", centralesIntegrantes.Select(c => c.Cpacntcodi));

            List<CpaCentralPmpoDTO> pmpoCentrales = FactoryTransferencia.GetCpaCentralPmpoRepository().GetByCentral(cpacntcodis);

            //Preguntar que se hace si la revision HASTA ya tiene relaciones
            if (empIntHasta.Count > 0)
            {
                dataMsg = "Esta revision ya tiene empresas y/o centrales relacionadas. En caso se desee copiar parámetros a esta revisión, proceder a anularla y volver a crear el mismo caso de revisión";
                typeMsg = ConstantesCPPA.MsgWarning;

                return new { dataMsg, typeMsg };
            }

            //Preguntar que se hace si la revisio DESDE no tiene nada
            if (empIntDesde.Count < 1) {
                dataMsg = "No hay registros que copiar...";
                typeMsg = ConstantesCPPA.MsgWarning;

                return new { dataMsg, typeMsg };
            }

            CpaEmpresaDTO entityEmpresa = new CpaEmpresaDTO();
            CpaCentralDTO entityCentral = new CpaCentralDTO();
            CpaCentralPmpoDTO entityPmpo = new CpaCentralPmpoDTO();

            //Insertando las empresas
            foreach (CpaEmpresaDTO e in empIntDesde)
            {
                entityEmpresa = new CpaEmpresaDTO
                {
                    Cparcodi = revisionHasta,
                    Emprcodi = e.Emprcodi,
                    Cpaemptipo = e.Cpaemptipo,
                    Cpaempestado = ConstantesCPPA.eActivo,
                    Cpaempusucreacion = user,
                    Cpaempfeccreacion = DateTime.Now
                };
                int cpaempcodi = this.SaveCpaEmpresa(entityEmpresa);

                //Insertando las centrales
                List<CpaCentralDTO> centralesByEmpresa = centralesIntegrantes.Where(x => x.Cpaempcodi == e.Cpaempcodi).ToList();

                if (centralesByEmpresa.Count > 0) {

                    foreach (CpaCentralDTO c in centralesByEmpresa)
                    {
                        DateTime? eInicio = null;
                        DateTime? eFin = null;
                        DateTime? pInicio = null;
                        DateTime? pFin = null;

                        this.FechasEjecutadasProyectadas(out eInicio, out eFin, out pInicio, out pFin, anioHasta, ajusteHasta, anioDesde, ajusteDesde, c.Cpacnttipo, c.Cpacntfecejecinicio, c.Cpacntfecejecfin, c.Cpacntfecproginicio, c.Cpacntfecprogfin);

                        entityCentral = new CpaCentralDTO
                        {
                            Cpaempcodi = cpaempcodi,
                            Cparcodi = revisionHasta,
                            Equicodi = c.Equicodi,
                            Barrcodi = c.Barrcodi,
                            Ptomedicodi = c.Ptomedicodi,
                            Cpacntestado = ConstantesCPPA.eActivo,
                            Cpacnttipo = c.Cpacnttipo, //aca hay q hallar el tipo manana
                            Cpacntfecejecinicio = eInicio,
                            Cpacntfecejecfin = eFin,
                            Cpacntfecproginicio = pInicio,
                            Cpacntfecprogfin = pFin,
                            Cpacntusucreacion = user,
                            Cpacntfeccreacion = DateTime.Now
                        };

                        int cpacntcodi = this.SaveCpaCentral(entityCentral);

                        //Insertando las centrales PMPO
                        List<CpaCentralPmpoDTO> pmpoByCentral = pmpoCentrales.Where(x => x.Cpacntcodi == c.Cpacntcodi).ToList();

                        if (pmpoByCentral.Count > 0) {
                            foreach (CpaCentralPmpoDTO p in pmpoByCentral)
                            {
                                entityPmpo = new CpaCentralPmpoDTO
                                {
                                    Cpacntcodi = cpacntcodi,
                                    Cparcodi = revisionHasta,
                                    Ptomedicodi = p.Ptomedicodi,
                                    Cpacnpusumodificacion = user,
                                    Cpacnpfecmodificacion = DateTime.Now
                                };

                                this.SaveCpaCentralPmpo(entityPmpo);
                            }
                        }
                    }
                }
            }

            CpaHistoricoDTO historico = new CpaHistoricoDTO()
            {
                Cparcodi = revisionHasta,
                Cpahtipo = ConstantesCPPA.tipoCopiarParametros,
                Cpahusumodificacion = user,
                Cpahfecmodificacion = DateTime.Now
            };
            this.SaveCpaHistorico(historico);

            dataMsg = "Copiado de datos exitoso...";
            typeMsg = ConstantesCPPA.MsgSuccess;

            return new { dataMsg, typeMsg };
        }

        /// <summary>
        /// Devuelve el tipo del registro N si es normal y E si es especial
        /// </summary>
        /// <param name="ajusteDesde">ajuste del registro desde donde se copian los datos</param>
        /// <param name="ejecinicio">fecha ejecutada de inicio</param>
        /// <param name="ejecfin">fecha ejecutada final</param>
        /// <param name="proginicio">fecha programada de inicio</param>
        /// <param name="progfin">Fecha programada final</param>
        /// <returns></returns>
        public string AnalisisFechasNormalEspecial(string ajusteDesde, DateTime? ejecinicio, DateTime? ejecfin, DateTime? proginicio, DateTime? progfin) {

            string respuesta = string.Empty;

            int dEjecInicio = ejecinicio.HasValue ? ejecinicio.Value.Day : 0;
            int mEjecInicio = ejecinicio.HasValue ? ejecinicio.Value.Month : 0;

            int dEjecFin = ejecfin.HasValue ? ejecfin.Value.Day : 0;
            int mEjecFin = ejecfin.HasValue ? ejecfin.Value.Month : 0;

            int dProgInicio = proginicio.HasValue ? proginicio.Value.Day : 0;
            int mProgInicio = proginicio.HasValue ? proginicio.Value.Month : 0;

            int dProgFin = progfin.HasValue ? progfin.Value.Day : 0;
            int mProgFin = progfin.HasValue ? progfin.Value.Month : 0;

            if (ajusteDesde == ConstantesCPPA.A1)
            {
                if (!ejecinicio.HasValue || !ejecfin.HasValue || !proginicio.HasValue || !progfin.HasValue)
                {
                    respuesta = ConstantesCPPA.tipoEspecial;//"E";
                }
                else
                {
                    if ((dEjecInicio == 1 && mEjecInicio == 1) && (dEjecFin == 31 && mEjecFin == 8) && (dProgInicio == 1 && mProgInicio == 9) && (dProgFin == 31 && mProgFin == 12))
                    {
                        respuesta = ConstantesCPPA.tipoNormal;//"N";
                    }
                    else
                    {
                        respuesta = ConstantesCPPA.tipoEspecial;//"E";
                    }
                }
            }
            else {
                if (ejecinicio.HasValue && ejecfin.HasValue && !proginicio.HasValue && !progfin.HasValue)
                {
                    if (dEjecInicio == 1 && mEjecInicio == 1 && dProgInicio == 31 && dProgFin == 12)
                    {
                        respuesta = ConstantesCPPA.tipoNormal;//"N";
                    }
                    else {
                        respuesta = ConstantesCPPA.tipoEspecial;//"E";
                    }
                }
                else {
                    respuesta = ConstantesCPPA.tipoEspecial;//"E";
                }
            }

            return respuesta;
        }

        /// <summary>
        /// Devuelve las fechas ejecutadas y pogramadas para el copiado de parametros
        /// </summary>
        /// <param name="eInicio">fecha de inicio ejecutada</param>
        /// <param name="eFin">fecha de fin ejecutada</param>
        /// <param name="pInicio">fecha de inicio programada</param>
        /// <param name="pFin">fecha de fin programada</param>
        /// <param name="anioHasta">anio hasta</param>
        /// <param name="ajusteHasta">ajuste hasta</param>
        /// <param name="anioDesde">anio desde</param>
        /// <param name="ajusteDesde">ajuste desde</param>
        /// <param name="tipo">ajuste desde</param>
        /// <param name="eInicioActual">fecha de inicio ejecutada registrada en la bd</param>
        /// <param name="eFinActual">fecha de fin ejecutada registrada en la bd</param>
        /// <param name="pInicioActual">fecha de inicio programada registrada en la bd</param>
        /// <param name="pFinActual">fecha de fin programada registrada en la bd</param>
        /// <returns></returns>
        public void FechasEjecutadasProyectadas(out DateTime? eInicio, out DateTime? eFin, out DateTime? pInicio, out DateTime? pFin, int anioHasta, string ajusteHasta, int anioDesde, string ajusteDesde, string tipo, DateTime? eInicioActual, DateTime? eFinActual, DateTime? pInicioActual, DateTime? pFinActual) {

            eInicio = (DateTime?)null;
            eFin = (DateTime?)null;
            pInicio = (DateTime?)null;
            pFin = (DateTime?)null;

            if (tipo == ConstantesCPPA.tipoNormal)
            {
                if (ajusteHasta == ConstantesCPPA.A1)
                {
                    eInicio = new DateTime(anioHasta, 1, 1);
                    eFin = new DateTime(anioHasta, 8, 31);
                    pInicio = new DateTime(anioHasta, 9, 1);
                    pFin = new DateTime(anioHasta, 12, 31);
                }
                else {
                    eInicio = new DateTime(anioHasta, 1, 1);
                    eFin = new DateTime(anioHasta, 12, 31);
                }
            }
            else {
                // Obtener día, mes y año de eInicioActual
                int diaEInicio = eInicioActual.HasValue ? eInicioActual.Value.Day : -1;
                int mesEInicio = eInicioActual.HasValue ? eInicioActual.Value.Month : -1;
                int anioEInicio = eInicioActual.HasValue ? eInicioActual.Value.Year : -1;

                // Obtener día, mes y año de eFinActual
                int diaEFin = eFinActual.HasValue ? eFinActual.Value.Day : -1;
                int mesEFin = eFinActual.HasValue ? eFinActual.Value.Month : -1;
                int anioEFin = eFinActual.HasValue ? eFinActual.Value.Year : -1;

                // Obtener día, mes y año de pInicioActual
                int diaPInicio = pInicioActual.HasValue ? pInicioActual.Value.Day : -1;
                int mesPInicio = pInicioActual.HasValue ? pInicioActual.Value.Month : -1;
                int anioPInicio = pInicioActual.HasValue ? pInicioActual.Value.Year : -1;

                // Obtener día, mes y año de pFinActual
                int diaPFin = pFinActual.HasValue ? pFinActual.Value.Day : -1;
                int mesPFin = pFinActual.HasValue ? pFinActual.Value.Month : -1;
                int anioPFin = pFinActual.HasValue ? pFinActual.Value.Year : -1;

                int anio1 = (anioEInicio == anioEFin) ? anioHasta : anioHasta + 1;
                int anio2 = (anioEFin == anioPInicio) ? anio1 : anio1 + 1;
                int anio3 = (anioPInicio == anioPFin) ? anio2 : anio2 + 1;

                eInicio = eInicioActual.HasValue ?
                            new DateTime(anioHasta, mesEInicio, diaEInicio) : (DateTime?)null;
                eFin = eFinActual.HasValue ?
                            new DateTime(anio1, mesEFin, diaEFin) : (DateTime?)null;
                pInicio = pInicioActual.HasValue ?
                            new DateTime(anio2, mesPInicio, diaPInicio) : (DateTime?)null;
                pFin = pFinActual.HasValue ?
                            new DateTime(anio3, mesPFin, diaPFin) : (DateTime?)null;
            }

        }

        #endregion

        #region CU07 - Importar insumo generación ejecutada 
        /// <summary>
        /// Proceso que se encarga de Importar la información de Medidores de Generación, por un ajuste de un año presupuesta y versión
        /// <param name="dtoCpaAjustePresupuestal">DTO CPA_AJUSTEPRESUPUESTAL</param>
        /// <param name="dtoCpaRevision">DTO CPA_REVISIÓN</param>
        /// <param name="sUser">Usuario en sesión</param>
        /// </summary>
        public string ImportarMedidoresGeneración(CpaAjustePresupuestalDTO dtoCpaAjustePresupuestal, CpaRevisionDTO dtoCpaRevision, string sUser)
        {
            string sResultado = "";
            int iNumReg = 0;
            int iCpainscodi = 0;
            string sTipinsumo = ConstantesCPPA.insumoMedidoresGeneracion;
            string sTipproceso = "A"; //Automático
            string sLog = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se inicia el proceso de importación automática del insumo de Ene a Dic de " + dtoCpaAjustePresupuestal.Cpaapanioejercicio.ToString() + "<br>";
            List<string> logList = new List<string>();
            bool conErroresNoControlados = false;
            bool conErroresValidaciones = false;
            try
            {
                //Calculamos las fechas para todo el año dtoCpaAjustePresupuestal.Cpaapanioejercicio
                DateTime dFecEjercicio = DateTime.ParseExact("01/01/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Inicio de año de ejercicio
                DateTime dFecEjercicioFin = DateTime.ParseExact("31/12/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Fin de año de ejercicio

                //Insertamos el Insumo
                iCpainscodi = InsertarInsumo(dtoCpaRevision.Cparcodi, sTipinsumo, sTipproceso, sLog, sUser);

                #region Validaciones
                List<CpaCentralDTO> listaCentral = ListCpaCentralByRevision(dtoCpaRevision.Cparcodi);
                if (listaCentral.Count == 0)
                {
                    conErroresValidaciones = true;
                    sResultado = "No se encontrarón centrales registradas para la revisión seleccionada.";
                    throw new ArgumentException(sResultado);
                }
                #endregion

                //Parametros de consulta
                List<int> idsTiposEmpresa = this.servicioConsultaMedidores.ListaTipoEmpresas().Select(x => x.Tipoemprcodi).ToList();
                string tiposEmpresa = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsTiposEmpresa);  //"3";

                List<int> idsTiposGeneracion = this.servicioConsultaMedidores.ListaTipoGeneracion().Select(x => x.Tgenercodi).ToList();
                string tiposGeneracion = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsTiposGeneracion);  //"4,1,3,2";

                //Traemos la lista de todas las empresa, de todos los tipos: 1,2,3,4,5
                List<int> idsEmpresas = this.servicioConsultaMedidores.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                string empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);

                int central = 0; //TODOS - Es una lista dura

                int lectcodi = Convert.ToInt32(ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);

                //Importamos la Información de Generadores y lo almacenamos en CPA_GERCSVDET
                List<MeMedicion96DTO> listActiva = new List<MeMedicion96DTO>();
                listActiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(dFecEjercicio, dFecEjercicioFin, central, tiposGeneracion, empresas,
                        ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva,
                        ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
                if (listActiva.Count > 0)
                {
                    TruncateCpaGercsvDetTmp(); //Truncate la tabla
                    //INSERTANDO CON BULKINSERT
                    List<CpaGercsvDetDTO> listGDBullk = new List<CpaGercsvDetDTO>();
                    int iContadorGD = 0;
                    Int32 iCpagedcodi = 1;
                    foreach (MeMedicion96DTO dtoME96 in listActiva)
                    {
                        CpaGercsvDetDTO dtoGD = new CpaGercsvDetDTO();
                        dtoGD.Cpagedcodi = iCpagedcodi;
                        dtoGD.Cpagercodi = dtoCpaRevision.Cparcodi; //Almacenamos el Identificador de la revisión
                        dtoGD.Emprcodi = dtoME96.Emprcodi;
                        dtoGD.Equicodi = dtoME96.Equipadre;
                        dtoGD.Cpagedtipcsv = "A"; //Migrado del sgocoes
                        dtoGD.Cpagedfecha = (DateTime)dtoME96.Medifecha;
                        //dtoGD.Cpagedtotaldia = (decimal)dtoME96.Meditotal;
                        int j = 0;
                        while (j < 96)
                        {
                            decimal dCpagedh = ((decimal)dtoME96.GetType().GetProperty($"H{j + 1}").GetValue(dtoME96)) / ConstantesCPPA.numero4;
                            dtoGD.GetType().GetProperty($"Cpagedh{(j + 1)}").SetValue(dtoGD, dCpagedh);
                            j++;
                        }
                        dtoGD.Cpagedusucreacion = sUser;
                        dtoGD.Cpagedfeccreacion = DateTime.Now;
                        listGDBullk.Add(dtoGD);
                        iContadorGD++;
                        if (iContadorGD >= 50000)
                        {
                            FactoryTransferencia.GetCpaGercsvDetRepository().BulkInsertCpaGerCsvDet(listGDBullk, "CPA_GERCSVDET_TMP");
                            listGDBullk = new List<CpaGercsvDetDTO>();
                            iNumReg += iContadorGD;
                            iContadorGD = 0;
                        }
                        iCpagedcodi++;
                    }
                    if (iContadorGD > 0)
                    {
                        //Quedo un faltante que insertar
                        FactoryTransferencia.GetCpaGercsvDetRepository().BulkInsertCpaGerCsvDet(listGDBullk, "CPA_GERCSVDET_TMP");
                        iNumReg += iContadorGD;
                    }
                    /**********************************************************************************************************************/
                    if (listaCentral.Count > 0) {
                        //Eliminando la información Importada anteriormente para esta Revisión e Insumo 
                        DeleteCpaInsumoDiaByRevision(dtoCpaRevision.Cparcodi, sTipinsumo);
                        DeleteCpaInsumoMesByRevision(dtoCpaRevision.Cparcodi, sTipinsumo);

                        //Para los 12 meses del año
                        for (int iMes = 1; iMes <= 12; iMes++)
                        {
                            DateTime dFecInicio = dFecEjercicio.AddMonths(iMes - 1);
                            DateTime dFecFin = dFecInicio.AddMonths(1).AddDays(-1); //Fin de mes del ejercicio

                            //Para cada Central
                            foreach (CpaCentralDTO dtoCentral in listaCentral)
                            {
                                //Insertamos el registro CPA_INSUMO_MES de la Central
                                decimal dTotalMes = 0;
                                int iCpainmcodi = InsertarInsumoMes(iCpainscodi, dtoCpaRevision.Cparcodi, dtoCentral.Emprcodi, dtoCentral.Equicodi, sTipinsumo, sTipproceso, iMes, dTotalMes, sUser);
                                int iCpaindcodi = FactoryTransferencia.GetCpaInsumoDiaRepository().GetMaxId();
                                InsertarInsumoDiaByTMP(iCpaindcodi, iCpainmcodi, dtoCentral.Emprcodi, dtoCentral.Equicodi, dFecInicio, dFecFin);
                                UpdateInsumoMesTotal(iCpainmcodi, dtoCentral.Equicodi, sTipinsumo, dFecInicio, dFecFin);
                            }

                            sLog += "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se procesó con éxito " + ConstantesCPPA.mesesDescCorta[iMes - 1] + "/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio.ToString() + "<br>";
                        }

                        sResultado = "Finalizó satisfactoriamente la importación de '" + ConstantesCPPA.insumosDesc[int.Parse(sTipinsumo) - 1] + "' a la base de datos.";
                    }
                }
                conErroresNoControlados = false;
            }
            catch (Exception e)
            {
                Logger.Error(ConstantesAppServicio.LogError, e);
                string innerExceptionMessage = (e.InnerException != null) ? (" " + e.InnerException.Message) : "";
                //sResultado = e.Message + innerExceptionMessage;7
                sResultado = "Culminó con error la importación de '" + ConstantesCPPA.insumosDesc[int.Parse(sTipinsumo) - 1] + "' a la base de datos. Ver detalle en el Log de este insumo.";

                conErroresNoControlados = true;
                sLog = sLog + "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Error-> " + e.Message + ". Se canceló el proceso.<br>";
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en CPA_INSUMO
                    if (iCpainscodi > 0)
                    {
                        CpaInsumoDTO dtoInsumo = GetByIdCpaInsumo(iCpainscodi);
                        if (conErroresNoControlados == false && conErroresValidaciones == false)
                        {
                            sLog += "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Finalizó el proceso de importación del insumo satisfactoriamente.";
                        }
                        else {
                            sLog += "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Finalizó el proceso de importación del insumo con error.";
                        }
                        dtoInsumo.Cpainslog = sLog;
                        UpdateCpaInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    Logger.Error(ConstantesAppServicio.LogError, e2);
                    sResultado = e2.Message;
                }
            }

            return sResultado;
        }

        #endregion

        #region CU08 - Importar insumo costo marginal ejecutado
        /// <summary>
        /// Proceso que se encarga de ImportarCostoMarginal, por un ajuste de un año presupuesta y versión
        /// </summary>
        /// <param name="dtoCpaAjustePresupuestal">DTO CPA_AJUSTEPRESUPUESTAL</param>
        /// <param name="dtoCpaRevision">DTO CPA_REVISIÓN</param>
        /// <param name="sUser">Usuario en sesión</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarCostoMarginal(CpaAjustePresupuestalDTO dtoCpaAjustePresupuestal, CpaRevisionDTO dtoCpaRevision, string sUser)
        {
            string sResultado = "";
            int iCpainscodi = 0;
            string sTipinsumo = ConstantesCPPA.insumoCostoMarginalLVTEA;
            string sTipproceso = "A"; //Automático
            string sLog = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se inicia el proceso de importación automática del insumo de Ene a Dic de " + dtoCpaAjustePresupuestal.Cpaapanioejercicio.ToString() + "<br>";
            List<string> logList = new List<string>();
            bool conErroresNoControlados = false;
            bool conErroresValidaciones = false;
            try
            {
                //Calculamos las fechas para todo el año dtoCpaAjustePresupuestal.Cpaapanioejercicio
                DateTime dFecEjercicio = DateTime.ParseExact("01/01/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Inicio de año de ejercicio
                DateTime dFecEjercicioFin = DateTime.ParseExact("31/12/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Fin de año de ejercicio

                //Insertamos el Insumo
                iCpainscodi = InsertarInsumo(dtoCpaRevision.Cparcodi, sTipinsumo, sTipproceso, sLog, sUser);

                List<CpaCentralDTO> listaCentral = ListCpaCentralByRevision(dtoCpaRevision.Cparcodi);

                #region Validaciones
                if (listaCentral.Count == 0)
                {
                    conErroresValidaciones = true;
                    sResultado = "No se encontrarón centrales registradas para la revisión seleccionada.";
                    throw new ArgumentException(sResultado);
                }
                var nombresCentralesConBarrcodiNull = listaCentral.Where(c => c.Barrcodi == null).Select(c => c.Equinomb);
                string nombresConcatenados = string.Join(", ", nombresCentralesConBarrcodiNull);
                if (!string.IsNullOrEmpty(nombresConcatenados))
                {
                    conErroresValidaciones = true;
                    sResultado = "No se puede realizar la importación automática debido a que las siguientes centrales no tienen registradas una barra de transferencia: " + nombresConcatenados;
                    throw new ArgumentException(sResultado);
                }
                #endregion

                if (listaCentral.Count > 0) {
                    //Eliminando la información Importada anteriormente para esta Revisión e Insumo 
                    DeleteCpaInsumoDiaByRevision(dtoCpaRevision.Cparcodi, sTipinsumo);
                    DeleteCpaInsumoMesByRevision(dtoCpaRevision.Cparcodi, sTipinsumo);

                    //Para los 12 meses del año
                    for (int iMes = 1; iMes <= 12; iMes++)
                    {
                        //Para cada Central
                        foreach (CpaCentralDTO dtoCentral in listaCentral)
                        {
                            if (dtoCentral.Barrcodi == null)
                            {
                                //Retornar un mensaje al usuario indicando que falta la relaión con la barra de transferencia
                                sResultado = "Falta información de la barra de transferencia en la central: " + dtoCentral.Equinomb;
                                throw new ArgumentException(sResultado);
                            }
                            DateTime dFecInicio = dFecEjercicio.AddMonths(iMes - 1);
                            DateTime dFecFin = dFecInicio.AddMonths(1).AddDays(-1); //Fin de mes del ejercicio

                            int iPeriAnioMes = int.Parse(dFecInicio.ToString("yyyyMM")); //ejemplo: 202401
                            PeriodoDTO dtoPeriodo = servicioPeriodo.GetByAnioMes(iPeriAnioMes);

                            if (dtoPeriodo != null) {
                                List<RecalculoDTO> listRecalculo = servicioRecalculo.ListRecalculosByAnioMes(dtoPeriodo.AnioCodi, dtoPeriodo.MesCodi);
                                if (listRecalculo.Count > 0) {
                                    RecalculoDTO dtoRecalcuo = listRecalculo.OrderByDescending(m => m.RecaCodi).First();
                                    //Insertamos el registro CPA_INSUMO_MES de la Central
                                    decimal dTotalMes = 0;
                                    int iCpainmcodi = InsertarInsumoMes(iCpainscodi, dtoCpaRevision.Cparcodi, dtoCentral.Emprcodi, dtoCentral.Equicodi, sTipinsumo, sTipproceso, iMes, dTotalMes, sUser);
                                    int iCpaindcodi = FactoryTransferencia.GetCpaInsumoDiaRepository().GetMaxId();
                                    InsertarInsumoDiaByCMg(iCpaindcodi, iCpainmcodi, dtoCpaRevision.Cparcodi, dtoCentral.Emprcodi, dtoCentral.Equicodi, dFecInicio, dtoPeriodo.PeriCodi, dtoRecalcuo.RecaCodi, (int)dtoCentral.Barrcodi, sUser);
                                    UpdateInsumoMesTotal(iCpainmcodi, dtoCentral.Equicodi, sTipinsumo, dFecInicio, dFecFin);
                                }
                            }
                        }
                        sLog += "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se procesó con éxito " + ConstantesCPPA.mesesDescCorta[iMes - 1] + "/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio.ToString() + "<br>";
                    }
                    sResultado = "Finalizó satisfactoriamente la importación de '" + ConstantesCPPA.insumosDesc[int.Parse(sTipinsumo) - 1] + "' a la base de datos.";
                }
                conErroresNoControlados = false;
            }
            catch (Exception e)
            {
                Logger.Error(ConstantesAppServicio.LogError, e);
                string innerExceptionMessage = (e.InnerException != null) ? (" " + e.InnerException.Message) : "";
                sResultado = "Culminó con error la importación de '" + ConstantesCPPA.insumosDesc[int.Parse(sTipinsumo) - 1] + "' a la base de datos. Ver detalle en el Log de este insumo.";
                conErroresNoControlados = true;
                sLog = sLog + "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Error-> " + e.Message + ". Se canceló el proceso.<br>";
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en RER_INSUMO
                    if (iCpainscodi > 0)
                    {
                        CpaInsumoDTO dtoInsumo = GetByIdCpaInsumo(iCpainscodi);
                        if (conErroresNoControlados == false && conErroresValidaciones == false)
                        {
                            sLog += "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Finalizó el proceso de importación del insumo satisfactoriamente.";
                        }
                        else {
                            sLog += "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Finalizó el proceso de importación del insumo con error.";
                        }
                        dtoInsumo.Cpainslog = sLog;
                        UpdateCpaInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    Logger.Error(ConstantesAppServicio.LogError, e2);
                    sResultado = e2.Message;
                }
            }

            return sResultado;
        }

        #endregion

        #region CU09 - Importar insumo generación programada
        /// <summary>
        /// Procesar los archivos “sddp.dat” y “*.csv”
        /// </summary>
        /// <param name="dtoCpaRevision">DTO CPA_REVISIÓN</param>
        /// <param name="pathDirectorio"></param>
        /// <param name="usuario"></param>
        /// <param name="continuar"></param>
        /// <param name="error"></param>
        /// <returns>string</returns>
        public string ProcesarArchivosSddp(CpaRevisionDTO dtoCpaRevision, string pathDirectorio, string usuario, out bool continuar, out string error)
        {
            CpaSddpDTO entitySddp;
            CpaGercsvDTO entityGerCsv;
            List<string> arrayDataSddpDat = new List<string>();
            List<RerLecCsvTemp> entitysLecCsvTemp = new List<RerLecCsvTemp>();
            List<CpaGercsvDetDTO> entitysGerCsvDet = new List<CpaGercsvDetDTO>();
            string sResultado = "";
            continuar = true;
            error = "";
            try
            {
                /* Valida el directorio de los archivos generados */
                bool existe = FileServer.VerificarLaExistenciaDirectorio(pathDirectorio);
                if (!existe)
                {
                    sResultado = "\n" + "La ruta '" + pathDirectorio + "' no es una ruta válida o el usuario no tiene acceso a dicha ruta.";
                    Logger.Error(sResultado);
                    continuar = false;
                    return sResultado;
                }

                /* Valida la existencia de los archivos */
                StringBuilder sb = new StringBuilder();
                bool exiteSddp = FileServer.VerificarExistenciaFile(null, "sddp.dat", pathDirectorio);
                bool exiteGergnd = FileServer.VerificarExistenciaFile(null, "gergnd.csv", pathDirectorio);
                bool exiteGerhid = FileServer.VerificarExistenciaFile(null, "gerhid.csv", pathDirectorio);
                bool exiteGerter = FileServer.VerificarExistenciaFile(null, "gerter.csv", pathDirectorio);
                bool exiteDuraci = FileServer.VerificarExistenciaFile(null, "duraci.csv", pathDirectorio);

                if (!exiteSddp)
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append("sddp.dat");
                }
                if (!exiteGergnd)
                {
                    sb.Append("gergnd.csv");
                }
                if (!exiteGerhid)
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append("gerhid.csv");
                }
                if (!exiteGerter)
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append("gerter.csv");
                }
                if (!exiteDuraci)
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append("duraci.csv");
                }

                if (sb.Length > 0)
                {
                    sResultado = "\n" + "No se encontraron los archivos SDDP: " + sb.ToString();
                    Logger.Error(sResultado);
                    continuar = false;
                    return sResultado;
                }

                /* Lee archivo .DAT de acuerdo a la logica del ECUS20 */
                using (StreamReader sr = FileServer.OpenReaderFile("sddp.dat", pathDirectorio))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null) arrayDataSddpDat.Add(s);
                }
                int semIni = Convert.ToInt32(arrayDataSddpDat[12].Substring(28, 2).Trim());
                int anioIni = Convert.ToInt32(arrayDataSddpDat[13].Substring(26, 4).Trim());
                int serie = Convert.ToInt32(arrayDataSddpDat[18].Substring(28, 2).Trim());
                DateTime diaInicio = EPDate.f_fechainiciosemana(anioIni, semIni);
                int iCorrelativo = GetByCorrelativoSddp(dtoCpaRevision.Cparcodi);
                entitySddp = new CpaSddpDTO
                {
                    Cparcodi = dtoCpaRevision.Cparcodi,
                    Cpsddpcorrelativo = iCorrelativo,
                    Cpsddpnomarchivo = "[" + DateTime.Now.ToString("yyyyMMddHHmmss") + "] sddp.dat",
                    Cpsddpsemanaini = semIni,
                    Cpsddpanioini = anioIni,
                    Cpsddpnroseries = serie,
                    Cpsddpdiainicio = diaInicio,
                    Cpsddpusucreacion = usuario,
                    Cpsddpfeccreacion = DateTime.Now
                };
                int idSddp = SaveCpaSddp(entitySddp);
                sResultado += "\n" + "[Inf] [" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se culminó el proceso de Lectura del archivo sddp.dat";

                /* Llena tabla cabecera archivos gergnd.csv y gerhid.csv de acuerdo a la logica del ECUS20 */
                entityGerCsv = new CpaGercsvDTO
                {
                    Cpsddpcodi = idSddp,
                    Cpagergndarchivo = "[" + DateTime.Now.ToString("yyyyMMddHHmmss") + "] gergnd.csv",
                    Cpagerhidarchivo = "[" + DateTime.Now.ToString("yyyyMMddHHmmss") + "] gerhid.csv",
                    Cpagerterarchivo = "[" + DateTime.Now.ToString("yyyyMMddHHmmss") + "] gerter.csv",
                    Cpagerdurarchivo = "[" + DateTime.Now.ToString("yyyyMMddHHmmss") + "] duraci.csv",
                    Cpagerusucreacion = usuario,
                    Cpagerfeccreacion = DateTime.Now
                };
                int idGerCsv = SaveCpaGercsv(entityGerCsv);

                /* Elimina los datos de las tablas temporales */
                FactoryTransferencia.GetRerGerCsvRepository().TruncateTablaTemporal(ConstantesCPPA.tablaLeccsvTemp);
                FactoryTransferencia.GetRerGerCsvRepository().TruncateTablaTemporal(ConstantesCPPA.tablaGerCsvDet);

                /* Lee detalle archivo gergnd.csv de acuerdo a la logica del ECUS09 */
                entitysLecCsvTemp = LeerArchivoGerCsv("gergnd.csv", pathDirectorio, "G", semIni, anioIni, serie);
                sResultado += "\n" + "[Inf] [" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se culminó el proceso de Lectura del archivo gergnd.csv";
                /*-----------------------------------------------------------------------------------------------------------------------------------*/
                /* Lee detalle archivo gerhid.csv de acuerdo a la logica del ECUS09 */
                entitysLecCsvTemp = LeerArchivoGerCsv("gerhid.csv", pathDirectorio, "H", semIni, anioIni, serie);
                sResultado += "\n" + "[Inf] [" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se culminó el proceso de Lectura del archivo gerhid.csv";
                /*-----------------------------------------------------------------------------------------------------------------------------------*/
                /* Lee detalle archivo gergnd.csv de acuerdo a la logica del ECUS09 */
                entitysLecCsvTemp = LeerArchivoGerCsv("gerter.csv", pathDirectorio, "T", semIni, anioIni, serie);
                sResultado += "\n" + "[Inf] [" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se culminó el proceso de Lectura del archivo gerter.csv";
                /*-----------------------------------------------------------------------------------------------------------------------------------*/
                /* Lee detalle archivo gergnd.csv de acuerdo a la logica del ECUS09 */
                entitysLecCsvTemp = LeerArchivoGerCsv("duraci.csv", pathDirectorio, "D", semIni, anioIni, serie);
                sResultado += "\n" + "[Inf] [" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se culminó el proceso de Lectura del archivo duraci.csv";

                /* Trae los datos filtrados y ordenados por etapa y bloque de la tabla temporal CPA_LECCSV_TEMP para almacenarlos en CPA_GERCSV_DET */
                entitysGerCsvDet = ObtenerCentralesPMPO(dtoCpaRevision.Cparcodi, idGerCsv, usuario, serie);

                if (entitysGerCsvDet.Count > 0)
                {
                    /* Inserta los registros Detalle*/
                    FactoryTransferencia.GetCpaGercsvDetRepository().BulkInsertCpaGerCsvDet(entitysGerCsvDet, ConstantesCPPA.tablaGerCsvDet);
                }
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                continuar = false;
                return sResultado + ex.Message; 
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                sResultado = ex.StackTrace;
                error = ex.Message;
                sResultado = "\n Ha ocurrido un error inesperado en tiempo de ejecución. Por favor, comunicarse con el Administrador";
                continuar = false;
                return sResultado;
            }

            return sResultado;
        }

        /// <summary>
        /// Procesar archivos .csv de acuerdo a la logica del ECUS09 y lo almacena temporalmente en RER_LECCSV_TMP
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="tipo"></param>
        /// <param name="semIni"></param>
        /// <param name="anioIni"></param>
        /// <param name="serie"></param>
        /// <returns>List RerLeccsvTemp</returns>
        public List<RerLecCsvTemp> LeerArchivoGerCsv(string fileName, string path, string tipo, int semIni, int anioIni, int serie)
        {
            RerLecCsvTemp entityRerLeccsvTemp;
            List<RerLecCsvTemp> entitysRerLeccsvTemp = new List<RerLecCsvTemp>();

            List<string> arrayDataGerGndCsv = new List<string>();
            using (StreamReader sr = FileServer.OpenReaderFile(fileName, path)) // Abre y lee el archivo
            {
                string s = "";
                while ((s = sr.ReadLine()) != null) arrayDataGerGndCsv.Add(s); // Llena el array con los datos del archivo
            }
            arrayDataGerGndCsv.RemoveRange(0, 3); // Elimina las 3 primeras filas

            List<string[]> cleanArrayData = new List<string[]>(); // Limpia los datos del array y lo combierte a matriz
            for (int i = 0; i <= arrayDataGerGndCsv.Count - 1; i++)
            {
                string[] arry = arrayDataGerGndCsv[i].Split(',');
                cleanArrayData.Add(arry);
            }

            // Trabajo las cabeceras de la matriz de la lectura
            for (int cabecera = 3; cabecera < cleanArrayData[0].Length; cabecera++) // Recorro las cabeceras (columnas) apartir 3ra (Centrales)
            {
                for (int fila = 1; fila < cleanArrayData.Count - 1; fila++) // Recorro los datos (filas) apartir 3ra cabecera (Centrales)
                {   //Control de cambios 2024-02-01 "<= serie"
                    int iEtapa = Convert.ToInt32(cleanArrayData[fila][0]);
                    if (iEtapa > 80)
                        break; // De la semana que inicia la lectura, 80 semanas mas. Para evitar quedarnos sin memoria
                    if (Convert.ToInt32(cleanArrayData[fila][1]) <= serie)
                    {
                        // Armo la lista con la data de las centrales (cabeceras)
                        entityRerLeccsvTemp = new RerLecCsvTemp();
                        entityRerLeccsvTemp.Rerfecinicio = EPDate.f_fechainiciosemana(anioIni, (Convert.ToInt32(cleanArrayData[fila][0]) + semIni - 1)); // Mayo.2022-Abril.2023 Semana Inicio = 27
                        entityRerLeccsvTemp.Reretapa = iEtapa;
                        entityRerLeccsvTemp.Rerserie = Convert.ToInt32(cleanArrayData[fila][1]);
                        entityRerLeccsvTemp.Rerbloque = Convert.ToInt32(cleanArrayData[fila][2]);
                        entityRerLeccsvTemp.Rercentrsddp = cleanArrayData[0][cabecera].Trim().ToUpper();
                        if (UtilCPPA.ValidarString(entityRerLeccsvTemp.Rercentrsddp))
                            entityRerLeccsvTemp.Rercentrsddp = UtilCPPA.RemplazarN(entityRerLeccsvTemp.Rercentrsddp);
                        entityRerLeccsvTemp.Rervalor = (decimal)double.Parse(cleanArrayData[fila][cabecera]);
                        entityRerLeccsvTemp.Rertipcsv = tipo;
                        entitysRerLeccsvTemp.Add(entityRerLeccsvTemp);
                    }
                }
                if (entitysRerLeccsvTemp.Count > 20000)
                {
                    /* Inserta los datos leidos del archivo xxx.csv en la tabla temporal, liberando memoria */
                    FactoryTransferencia.GetRerGerCsvRepository().BulkInsertTablaTemporal(entitysRerLeccsvTemp, ConstantesPrimasRER.tablaRerLeccsvTemp);
                    entitysRerLeccsvTemp = new List<RerLecCsvTemp>();
                }
            }

            if (entitysRerLeccsvTemp.Count > 0)
            {
                /* Inserta los datos leidos restantes del archivo xxx.csv en la tabla temporal */
                FactoryTransferencia.GetRerGerCsvRepository().BulkInsertTablaTemporal(entitysRerLeccsvTemp, ConstantesPrimasRER.tablaRerLeccsvTemp);
                entitysRerLeccsvTemp = new List<RerLecCsvTemp>();
            }
            return entitysRerLeccsvTemp;
        }

        /// <summary>
        /// Procesar archivos .csv de acuerdo a la logica del ECUS09
        /// </summary>
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="idGerCsv"></param>
        /// <param name="usuario"></param>
        /// <param name="serie"></param>
        /// <returns>List<RerGerCsvDetDTO></returns>
        public List<CpaGercsvDetDTO> ObtenerCentralesPMPO(int cparcodi, int idGerCsv, string usuario, int serie)
        {
            try
            {
                List<CpaGercsvDetDTO> entitysGerCsvDetDTO = new List<CpaGercsvDetDTO>();
                int iCpagedcodi = 1;
                //Definimos los dias de la semana
                char[] diasSemana = { 'S', 'D', 'L', 'M', 'X', 'J', 'V' };
                //Traemos la lista de feriados: listaferiado.Pmfrdofecha: Almacena las fechas que son Feriado PMPO
                List<PmoFeriadoDTO> listaferiado = FactorySic.GetPmoFeriadoRepository().List();
                //Lista de DURACIÓN
                List<RerLecCsvTemp> entitysRerLeccsvDuracion = FactoryTransferencia.GetRerGerCsvRepository().ListTablaTemporal("PERU");
                //Traemos la lista de Centrales Activos
                List<CpaCentralDTO> entitysCentral = ListCpaCentralByRevision(cparcodi);
                foreach (CpaCentralDTO central in entitysCentral)
                {
                    //Para cada Centrarl RER, traemos la lista de Centrales PMPO
                    List<CpaCentralPmpoDTO> entitysCentralPmpo = ListCpaCentralPmpo(central.Cpacntcodi);
                    //Recorremos la lista de Centrales PMPO
                    foreach (CpaCentralPmpoDTO centralPmpo in entitysCentralPmpo)
                    {
                        //Por cada Central PMPO, lo relacionamos con la Central SDDP y lo asignamos a Central
                        PmoSddpCodigoDTO entidad = FactoryTransferencia.GetRerGerCsvRepository().GetByCentralesSddp(centralPmpo.Ptomedicodi);
                        if (entidad == null)
                        {
                            throw new MyCustomException("\n" + "[Error] No existe información del Nombre SDDP para el punto de medición: " + centralPmpo.Ptomedicodi.ToString());
                        }
                        //entidad.Sddpnomb -> Tiene el Nombre de la CentralSDDP
                        //entidad.Emprcodi deberia ser igual a central.Emprcodi, pero para asegurarnos mantengo el central.Emprcodi
                        entidad.Emprcodi = central.Emprcodi; //Empresa
                        entidad.Equicodi = central.Equicodi; //Central

                        //Vamos a poblar la tabla RER_GERCSV_DET
                        List<RerLecCsvTemp> entitysLeccsvTemp = FactoryTransferencia.GetRerGerCsvRepository().ListTablaTemporal(entidad.Sddpnomb.Trim().ToUpper());
                        //Lista de RerLeccsvTemp por una central SDDP
                        int iLimiteEtapa = 80;
                        if (entitysLeccsvTemp.Count < 80) iLimiteEtapa = entitysLeccsvTemp.Count;
                        for (int iContadorEtapa = 0; iContadorEtapa < iLimiteEtapa; iContadorEtapa++)
                        {
                            //Energia = 1000 * energia(j, k) /  (duraci(j, k) * 4 * nseries) 
                            decimal dDenom1 = entitysRerLeccsvDuracion[5 * iContadorEtapa].Rervalor * 4 * serie;
                            decimal dDenom2 = entitysRerLeccsvDuracion[5 * iContadorEtapa + 1].Rervalor * 4 * serie;
                            decimal dDenom3 = entitysRerLeccsvDuracion[5 * iContadorEtapa + 2].Rervalor * 4 * serie;
                            decimal dDenom4 = entitysRerLeccsvDuracion[5 * iContadorEtapa + 3].Rervalor * 4 * serie;
                            decimal dDenom5 = entitysRerLeccsvDuracion[5 * iContadorEtapa + 4].Rervalor * 4 * serie;
                            //Tomamos los bloques de 5 en 5
                            decimal dBloque1 = 1000 * (entitysLeccsvTemp[5 * iContadorEtapa].Rervalor / dDenom1);
                            decimal dBloque2 = 1000 * (entitysLeccsvTemp[5 * iContadorEtapa + 1].Rervalor / dDenom2);
                            decimal dBloque3 = 1000 * (entitysLeccsvTemp[5 * iContadorEtapa + 2].Rervalor / dDenom3);
                            decimal dBloque4 = 1000 * (entitysLeccsvTemp[5 * iContadorEtapa + 3].Rervalor / dDenom4);
                            decimal dBloque5 = 1000 * (entitysLeccsvTemp[5 * iContadorEtapa + 4].Rervalor / dDenom5);
                            //Las 7 fechas de la semana 

                            //iniciando en Sabado:
                            DateTime dFecha = entitysLeccsvTemp[5 * iContadorEtapa].Rerfecinicio;
                            string sTipoCSV = entitysLeccsvTemp[5 * iContadorEtapa].Rertipcsv;

                            for (int i = 0; i < diasSemana.Length; i++)
                            {
                                char sFeriado = 'N';
                                var feriadoEncontrado = listaferiado.Where(x => x.Pmfrdofecha == dFecha).FirstOrDefault();
                                if (feriadoEncontrado != null)
                                    sFeriado = 'S';
                                entitysGerCsvDetDTO.Add(addRerGerCsvDetalle(iCpagedcodi++, dFecha, idGerCsv, entidad.Emprcodi, entidad.Equicodi, sTipoCSV, diasSemana[i], dBloque1, dBloque2, dBloque3, dBloque4, dBloque5, sFeriado, usuario));
                                dFecha = dFecha.AddDays(1);
                            }
                        }
                    }
                    //CADA CENTRAL PMPO SE ALMACENARA EN CPA_GERCVDET.
                    //POR CONSIGUIENTE PUDEN EXISTIR MAS DE UN REGISTRO DE LA MISMA CENTRAL PARA EL MISMO DIA.
                }
                return entitysGerCsvDetDTO;
            }
            catch (MyCustomException ex)
            {
                //2.- Remito el error controlado
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw; 
            }
            catch (Exception ex)
            {
                //1.- Capturo el error general, no controlado
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
        }

        /// <summary>
        /// Completar un dia de insumos de acuerdo a la logica del ECUS09
        /// </summary>
        /// <param name="iCpagedcodi"></param>
        /// <param name="dFecha"></param>
        /// <param name="idGerCsv"></param>
        /// <param name="iEmprcodi"></param>
        /// <param name="iEquicodi"></param>
        /// <param name="sTipoCSV"></param>
        /// <param name="sDia"></param>
        /// <param name="dBloque1"></param>
        /// <param name="dBloque2"></param>
        /// <param name="dBloque3"></param>
        /// <param name="dBloque4"></param>
        /// <param name="dBloque5"></param>
        /// <param name="sFeriado"></param>
        /// <param name="usuario"></param>
        /// <returns>CpaGercsvDetDTO</returns>
        public CpaGercsvDetDTO addRerGerCsvDetalle(int iCpagedcodi, DateTime dFecha, int idGerCsv, int? iEmprcodi, int? iEquicodi, string sTipoCSV,
            char sDia, decimal dBloque1, decimal dBloque2, decimal dBloque3, decimal dBloque4, decimal dBloque5, char sFeriado, string usuario)
        {
            CpaGercsvDetDTO dtoGerCsvDet = new CpaGercsvDetDTO();
            dtoGerCsvDet.Cpagedcodi = iCpagedcodi;
            dtoGerCsvDet.Cpagercodi = idGerCsv;
            dtoGerCsvDet.Emprcodi = (int)iEmprcodi;
            dtoGerCsvDet.Equicodi = (int)iEquicodi;
            dtoGerCsvDet.Cpagedtipcsv = sTipoCSV;
            dtoGerCsvDet.Cpagedfecha = dFecha;
            dtoGerCsvDet.Cpagedusucreacion = usuario;
            dtoGerCsvDet.Cpagedfeccreacion = DateTime.Now;

            //De Sabado a Viernes, el Bloque 5 se asigna sin restrigción
            //h1 -> h32 // 00:15 A 08:00
            int j = 1;
            while (j <= 32)
            {
                dtoGerCsvDet.GetType().GetProperty($"Cpagedh{(j)}").SetValue(dtoGerCsvDet, dBloque5);
                j++;
            }
            //h93 -> h96 // 23:15 A 00:00 DEL DIA SIGUIENTE
            j = 93;
            while (j <= 96)
            {
                dtoGerCsvDet.GetType().GetProperty($"Cpagedh{(j)}").SetValue(dtoGerCsvDet, dBloque5);
                j++;
            }
            /**************************************************************************************************/
            //De Sabado a Viernes, el Bloque 4 se asigna en el horario del h33 -> h72,
            //con Excepciones los L, M. X, J y V en el intervalo h45-> h48 donde se asigna el Bloque 2
            j = 33;
            while (j <= 72)
            {
                if ((j >= 45 && j <= 48) && (sDia != 'S' && sDia != 'D' && sFeriado != 'S'))
                {
                    //Cumple para L, M. X, J y V en el intervalo h45-> h48
                    dtoGerCsvDet.GetType().GetProperty($"Cpagedh{(j)}").SetValue(dtoGerCsvDet, dBloque2);
                }
                else
                {
                    dtoGerCsvDet.GetType().GetProperty($"Cpagedh{(j)}").SetValue(dtoGerCsvDet, dBloque4);
                }
                j++;
            }
            /**************************************************************************************************/
            //De Sabado a Viernes, el Bloque 3 se asigna en el horario del h73 -> h92,
            //con Excepciones los L, M. X, J y V en el intervalo h45-> h48 donde se asigna el Bloque 1
            j = 73;
            while (j <= 92)
            {
                if ((j >= 77 && j <= 78) && (sDia != 'S' && sDia != 'D' && sFeriado != 'S'))
                {
                    //Cumple para L, M. X, J y V en el intervalo h45-> h48
                    dtoGerCsvDet.GetType().GetProperty($"Cpagedh{(j)}").SetValue(dtoGerCsvDet, dBloque1);
                }
                else
                {
                    dtoGerCsvDet.GetType().GetProperty($"Cpagedh{(j)}").SetValue(dtoGerCsvDet, dBloque3);
                }
                j++;
            }
            return dtoGerCsvDet;
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto a los archivos SDDP
        /// </summary>
        /// <param name="dtoCpaAjustePresupuestal">DTO CPA_AJUSTEPRESUPUESTAL</param>
        /// <param name="dtoCpaRevision">DTO CPA_REVISIÓN</param>
        /// <param name="rutaArchivo"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="listExcelHoja"></param>
        public void GenerarArchivoExcelSDDP(CpaAjustePresupuestalDTO dtoCpaAjustePresupuestal, CpaRevisionDTO dtoCpaRevision, string rutaArchivo, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja)
        {
            try
            {
                listExcelHoja = new List<CpaExcelHoja>();
                //Calculamos las fechas para todo el año dtoCpaAjustePresupuestal.Cpaapanioejercicio
                DateTime dFecEjercicio = DateTime.ParseExact("01/01/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Inicio de año de ejercicio
                DateTime dFecEjercicioFin = DateTime.ParseExact("31/12/" + (dtoCpaAjustePresupuestal.Cpaapanioejercicio + 1), ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Fin de año de ejercicio

                nombreArchivo = "ResumenSDDP_" + dtoCpaAjustePresupuestal.Cpaapanio + "_" + dtoCpaAjustePresupuestal.Cpaapajuste + "_" + dtoCpaRevision.Cparrevision;

                #region Obtener datos
                CpaSddpDTO dtoSddp = GetByIdCpaSddp(dtoCpaRevision.Cparcodi); //Trae de la versióon, el correlativo mas reciente
                if (dtoSddp == null)
                {
                    throw new Exception("Aún no se realizó la importación para el Año prespuestal, Ajuste y versión seleccionados. Por favor, realizar la acción de 'Procesar'.");
                }

                CpaGercsvDTO dtoGerCsv = GetByIdCpaGercsv(dtoSddp.Cpsddpcodi);
                if (dtoGerCsv == null)
                {
                    throw new Exception("No se encontro un archivo para el id = " + dtoSddp.Cpsddpcodi.ToString());
                }

                string sSDDP = dtoSddp.Cpsddpnomarchivo;
                int iMes_semanaInicial = dtoSddp.Cpsddpsemanaini;
                int iAnio = dtoSddp.Cpsddpanioini;
                int iNroSerie = dtoSddp.Cpsddpnroseries;

                string sGergnd = dtoGerCsv.Cpagergndarchivo;
                string sGerhid = dtoGerCsv.Cpagerhidarchivo;
                string sGerter = dtoGerCsv.Cpagerterarchivo;

                List<CpaGercsvDetDTO> listGerGnd = ListCpaGercsvDet(dtoGerCsv.Cpagercodi, "G", dFecEjercicio, dFecEjercicioFin);
                List<CpaGercsvDetDTO> listGerHid = ListCpaGercsvDet(dtoGerCsv.Cpagercodi, "H", dFecEjercicio, dFecEjercicioFin);
                List<CpaGercsvDetDTO> listGerTer = ListCpaGercsvDet(dtoGerCsv.Cpagercodi, "T", dFecEjercicio, dFecEjercicioFin);

                var listEmprcodisEquicodisGerGnd = listGerGnd
                .Select(item => new { Emprcodi = item.Emprcodi, Equicodi = item.Equicodi, Equinomb = item.Equinomb, Emprnomb = item.Emprnomb })
                .Distinct()
                .ToList();

                var listEmprcodisEquicodisGerHid = listGerHid
                .Select(item => new { Emprcodi = item.Emprcodi, Equicodi = item.Equicodi, Equinomb = item.Equinomb, Emprnomb = item.Emprnomb })
                .Distinct()
                .ToList();

                var listEmprcodisEquicodisGerTer = listGerTer
                .Select(item => new { Emprcodi = item.Emprcodi, Equicodi = item.Equicodi, Equinomb = item.Equinomb, Emprnomb = item.Emprnomb })
                .Distinct()
                .ToList();
                #endregion

                #region Variables
                string subtitulo1 = "Año presupuestal " + dtoCpaAjustePresupuestal.Cpaapanio + " ";
                string subtitulo2 = "Resumen del archivo: ";
                #endregion

                #region Hoja SDDP

                #region Titulo
                string tituloSDDP = "Resumen del archivo SDDP";
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasSDDP = new List<CpaExcelModelo>[1];
                List<CpaExcelModelo> listaCabecera1SDDP = new List<CpaExcelModelo>
                {
                    CrearExcelModelo("Datos SDDP"),
                    CrearExcelModelo("Valor")
                };
                List<int> listaAnchoColumnaSDDP = new List<int>
                {
                    30,
                    30
                };

                listaCabecerasSDDP[0] = listaCabecera1SDDP;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalSDDP = new List<string> { "left", "left" };
                List<string> listaTipoSDDP = new List<string> { "string", "string" };
                List<CpaExcelEstilo> listaEstiloSDDP = new List<CpaExcelEstilo> {
                    CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"),
                    null
                };

                List<string>[] listaRegistrosSDDP = new List<string>[7];
                listaRegistrosSDDP[0] = new List<string> { "SDDP", sSDDP };
                listaRegistrosSDDP[1] = new List<string> { "Mes/Semana Inicial", iMes_semanaInicial.ToString() };
                listaRegistrosSDDP[2] = new List<string> { "Año", iAnio.ToString() };
                listaRegistrosSDDP[3] = new List<string> { "Nro. serie", iNroSerie.ToString() };
                listaRegistrosSDDP[4] = new List<string> { "gergnd", sGergnd };
                listaRegistrosSDDP[5] = new List<string> { "gerhid", sGerhid };
                listaRegistrosSDDP[6] = new List<string> { "gerter", sGerter };

                CpaExcelCuerpo cuerpoSDDP = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosSDDP,
                    ListaAlineaHorizontal = listaAlineaHorizontalSDDP,
                    ListaTipo = listaTipoSDDP,
                    ListaEstilo = listaEstiloSDDP
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaSDDP = new CpaExcelHoja
                {
                    NombreHoja = "SDDP",
                    Titulo = tituloSDDP,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaSDDP,
                    ListaCabeceras = listaCabecerasSDDP,
                    Cuerpo = cuerpoSDDP
                };
                listExcelHoja.Add(excelHojaSDDP);
                #endregion

                #endregion
                //--------------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------------
                #region Hoja gergnd

                #region Titulo
                string tituloGergnd = dtoCpaAjustePresupuestal.Cpaapajuste + "_" + dtoCpaRevision.Cparrevision;
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasGergnd = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabecera1Gergnd = new List<CpaExcelModelo>
                {
                    CrearExcelModelo(dtoCpaAjustePresupuestal.Cpaapanioejercicio.ToString(), "center", 1, 2)
                };
                List<CpaExcelModelo> listaCabecera2Gergnd = new List<CpaExcelModelo> { };
                List<int> listaAnchoColumnaGergnd = new List<int> { 30 };
                foreach (var item in listEmprcodisEquicodisGerGnd)
                {
                    listaCabecera1Gergnd.Add(CrearExcelModelo(item.Equinomb.ToString()));
                    listaCabecera2Gergnd.Add(CrearExcelModelo(item.Equicodi.ToString()));
                    listaAnchoColumnaGergnd.Add(30);
                }

                listaCabecerasGergnd[0] = listaCabecera1Gergnd;
                listaCabecerasGergnd[1] = listaCabecera2Gergnd;
                #endregion

                #region Cuerpo
                var registrosPorFechaGerGnd = listGerGnd.GroupBy(item => item.Cpagedfecha.Date);
                int cantidadFechasDiferentesGerGnd = registrosPorFechaGerGnd.Count();

                List<string> listaAlineaHorizontalGergnd = new List<string> { "center" };
                List<string> listaTipoGergnd = new List<string> { "string" };
                List<CpaExcelEstilo> listaEstiloGergnd = new List<CpaExcelEstilo>();

                if (cantidadFechasDiferentesGerGnd > 0)
                {
                    listaEstiloGergnd.Add(CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"));
                }

                foreach (var item in listEmprcodisEquicodisGerGnd)
                {
                    listaAlineaHorizontalGergnd.Add("right");
                    listaTipoGergnd.Add("double");
                    listaEstiloGergnd.Add(CrearExcelEstilo("#,##0.0000000"));
                }

                List<string>[] listaRegistrosGergnd;
                int posicionFecha = 0;

                if (cantidadFechasDiferentesGerGnd == 0)
                {
                    listaRegistrosGergnd = new List<string>[1];
                    listaRegistrosGergnd[0] = new List<string> { "No existen registros importados desde gergnd.csv" };
                }
                else
                {
                    listaRegistrosGergnd = new List<string>[96 * cantidadFechasDiferentesGerGnd];
                }

                foreach (var grupo in registrosPorFechaGerGnd)
                {
                    var fecha = grupo.Key;
                    List<string> intervalosFormateados = ObtenerIntervalosConFormato(fecha);

                    List<CpaGercsvDetDTO> listEmprCent = grupo.ToList();

                    for (int indice = 1; indice <= 96; indice++)
                    {
                        int posicionRegistro = posicionFecha * 96 + indice - 1;
                        listaRegistrosGergnd[posicionRegistro] = new List<string> { intervalosFormateados[indice - 1] };
                        foreach (var emprCentr in listEmprcodisEquicodisGerGnd)
                        {
                            CpaGercsvDetDTO emprCent = listEmprCent.Where(item => item.Equicodi == emprCentr.Equicodi && item.Emprcodi == emprCentr.Emprcodi).FirstOrDefault();
                            string valorH = "0";
                            if (emprCent != null)
                            {
                                var propertyName = $"Cpagedh{indice}";
                                valorH = emprCent.GetType().GetProperty(propertyName).GetValue(emprCent).ToString();
                            }

                            listaRegistrosGergnd[posicionRegistro].Add(valorH);
                        }
                    }

                    posicionFecha++;
                }

                CpaExcelCuerpo cuerpoGergnd = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosGergnd,
                    ListaAlineaHorizontal = listaAlineaHorizontalGergnd,
                    ListaTipo = listaTipoGergnd,
                    ListaEstilo = listaEstiloGergnd
                };

                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaGergnd = new CpaExcelHoja
                {
                    NombreHoja = "Gergnd",
                    Titulo = tituloGergnd,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaGergnd,
                    ListaCabeceras = listaCabecerasGergnd,
                    Cuerpo = cuerpoGergnd
                };
                listExcelHoja.Add(excelHojaGergnd);
                #endregion

                #endregion
                //--------------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------------
                #region hoja gerhid

                #region Titulo
                string tituloGerhid = dtoCpaAjustePresupuestal.Cpaapajuste + "_" + dtoCpaRevision.Cparrevision;
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasGerhid = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabecera1Gerhid = new List<CpaExcelModelo>
                {
                    CrearExcelModelo(dtoCpaAjustePresupuestal.Cpaapanioejercicio.ToString(), "center", 1, 2)
                };
                List<CpaExcelModelo> listaCabecera2Gerhid = new List<CpaExcelModelo> { };
                List<int> listaAnchoColumnaGerhid = new List<int> { 30 };
                foreach (var item in listEmprcodisEquicodisGerHid)
                {
                    listaCabecera1Gerhid.Add(CrearExcelModelo(item.Equinomb.ToString()));
                    listaCabecera2Gerhid.Add(CrearExcelModelo(item.Equicodi.ToString()));
                    listaAnchoColumnaGerhid.Add(30);
                }

                listaCabecerasGerhid[0] = listaCabecera1Gerhid;
                listaCabecerasGerhid[1] = listaCabecera2Gerhid;
                #endregion

                #region Cuerpo
                var registrosPorFechaGerHid = listGerHid.GroupBy(item => item.Cpagedfecha.Date);
                int cantidadFechasDiferentesGerHid = registrosPorFechaGerHid.Count();

                List<string> listaAlineaHorizontalGerhid = new List<string> { "center" };
                List<string> listaTipoGerhid = new List<string> { "string" };
                List<CpaExcelEstilo> listaEstiloGerhid = new List<CpaExcelEstilo>();

                if (cantidadFechasDiferentesGerHid > 0)
                {
                    listaEstiloGerhid.Add(CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"));
                }

                foreach (var item in listEmprcodisEquicodisGerHid)
                {
                    listaAlineaHorizontalGerhid.Add("right");
                    listaTipoGerhid.Add("double");
                    listaEstiloGerhid.Add(CrearExcelEstilo("#,##0.0000000"));
                }

                List<string>[] listaRegistrosGerhid;
                posicionFecha = 0;
                if (cantidadFechasDiferentesGerHid == 0)
                {
                    listaRegistrosGerhid = new List<string>[1];
                    listaRegistrosGerhid[0] = new List<string> { "No existen registros importados desde gerhid.csv" };
                }
                else
                {
                    listaRegistrosGerhid = new List<string>[96 * cantidadFechasDiferentesGerHid];
                }

                foreach (var grupo in registrosPorFechaGerHid)
                {
                    var fecha = grupo.Key;
                    List<string> intervalosFormateados = ObtenerIntervalosConFormato(fecha);

                    List<CpaGercsvDetDTO> listEmprCent = grupo.ToList();

                    for (int indice = 1; indice <= 96; indice++)
                    {
                        int posicionRegistro = posicionFecha * 96 + indice - 1;
                        listaRegistrosGerhid[posicionRegistro] = new List<string> { intervalosFormateados[indice - 1] };
                        foreach (var emprCentr in listEmprcodisEquicodisGerHid)
                        {
                            CpaGercsvDetDTO emprCent = listEmprCent.Where(item => item.Equicodi == emprCentr.Equicodi && item.Emprcodi == emprCentr.Emprcodi).FirstOrDefault();
                            string valorH = "0";
                            if (emprCent != null)
                            {
                                var propertyName = $"Cpagedh{indice}";
                                valorH = emprCent.GetType().GetProperty(propertyName).GetValue(emprCent).ToString();
                            }

                            listaRegistrosGerhid[posicionRegistro].Add(valorH);
                        }
                    }

                    posicionFecha++;
                }

                CpaExcelCuerpo cuerpoGerhid = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosGerhid,
                    ListaAlineaHorizontal = listaAlineaHorizontalGerhid,
                    ListaTipo = listaTipoGerhid,
                    ListaEstilo = listaEstiloGerhid
                };

                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaGerhid = new CpaExcelHoja
                {
                    NombreHoja = "Gerhid",
                    Titulo = tituloGerhid,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaGerhid,
                    ListaCabeceras = listaCabecerasGerhid,
                    Cuerpo = cuerpoGerhid
                };
                listExcelHoja.Add(excelHojaGerhid);
                #endregion

                #endregion
                //--------------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------------
                #region hoja gerter

                #region Titulo
                string tituloGerter = dtoCpaAjustePresupuestal.Cpaapajuste + "_" + dtoCpaRevision.Cparrevision;
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasGerter = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabecera1Gerter = new List<CpaExcelModelo>
                {
                    CrearExcelModelo(dtoCpaAjustePresupuestal.Cpaapanioejercicio.ToString(), "center", 1, 2)
                };
                List<CpaExcelModelo> listaCabecera2Gerter = new List<CpaExcelModelo> { };
                List<int> listaAnchoColumnaGerter = new List<int> { 30 };
                foreach (var item in listEmprcodisEquicodisGerTer)
                {
                    listaCabecera1Gerter.Add(CrearExcelModelo(item.Equinomb.ToString()));
                    listaCabecera2Gerter.Add(CrearExcelModelo(item.Equicodi.ToString()));
                    listaAnchoColumnaGerter.Add(30);
                }

                listaCabecerasGerter[0] = listaCabecera1Gerter;
                listaCabecerasGerter[1] = listaCabecera2Gerter;
                #endregion

                #region Cuerpo
                var registrosPorFechaGerTer = listGerTer.GroupBy(item => item.Cpagedfecha.Date);
                int cantidadFechasDiferentesGerTer = registrosPorFechaGerTer.Count();

                List<string> listaAlineaHorizontalGerTer = new List<string> { "center" };
                List<string> listaTipoGerter = new List<string> { "string" };
                List<CpaExcelEstilo> listaEstiloGerter = new List<CpaExcelEstilo>();

                if (cantidadFechasDiferentesGerTer > 0)
                {
                    listaEstiloGerter.Add(CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"));
                }

                foreach (var item in listEmprcodisEquicodisGerHid)
                {
                    listaAlineaHorizontalGerTer.Add("right");
                    listaTipoGerter.Add("double");
                    listaEstiloGerter.Add(CrearExcelEstilo("#,##0.0000000"));
                }

                List<string>[] listaRegistrosGerter;
                posicionFecha = 0;
                if (cantidadFechasDiferentesGerTer == 0)
                {
                    listaRegistrosGerter = new List<string>[1];
                    listaRegistrosGerter[0] = new List<string> { "No existen registros importados desde gerter.csv" };
                }
                else
                {
                    listaRegistrosGerter = new List<string>[96 * cantidadFechasDiferentesGerTer];
                }

                foreach (var grupo in registrosPorFechaGerTer)
                {
                    var fecha = grupo.Key;
                    List<string> intervalosFormateados = ObtenerIntervalosConFormato(fecha);

                    List<CpaGercsvDetDTO> listEmprCent = grupo.ToList();

                    for (int indice = 1; indice <= 96; indice++)
                    {
                        int posicionRegistro = posicionFecha * 96 + indice - 1;
                        listaRegistrosGerter[posicionRegistro] = new List<string> { intervalosFormateados[indice - 1] };
                        foreach (var emprCentr in listEmprcodisEquicodisGerTer)
                        {
                            CpaGercsvDetDTO emprCent = listEmprCent.Where(item => item.Equicodi == emprCentr.Equicodi && item.Emprcodi == emprCentr.Emprcodi).FirstOrDefault();
                            string valorH = "0";
                            if (emprCent != null)
                            {
                                var propertyName = $"Cpagedh{indice}";
                                valorH = emprCent.GetType().GetProperty(propertyName).GetValue(emprCent).ToString();
                            }

                            listaRegistrosGerter[posicionRegistro].Add(valorH);
                        }
                    }

                    posicionFecha++;
                }

                CpaExcelCuerpo cuerpoGerter = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosGerter,
                    ListaAlineaHorizontal = listaAlineaHorizontalGerTer,
                    ListaTipo = listaTipoGerter,
                    ListaEstilo = listaEstiloGerter
                };

                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaGerter = new CpaExcelHoja
                {
                    NombreHoja = "Gerter",
                    Titulo = tituloGerter,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaGerter,
                    ListaCabeceras = listaCabecerasGerter,
                    Cuerpo = cuerpoGerter
                };
                listExcelHoja.Add(excelHojaGerter);
                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Proceso que se encarga de Importar la Generación Programada, por un ajuste de un año presupuesta y versión
        /// </summary>
        /// <param name="dtoCpaAjustePresupuestal">DTO CPA_AJUSTEPRESUPUESTAL</param>
        /// <param name="dtoCpaRevision">DTO CPA_REVISIÓN</param>
        /// <param name="sUser">Usuario en sesión</param>
        /// <param name="sLog">Log de transacciones</param>
        /// <param name="continuar"></param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarGeneraciónPMPO(CpaAjustePresupuestalDTO dtoCpaAjustePresupuestal, CpaRevisionDTO dtoCpaRevision, string sUser, string sLog, out bool continuar)
        {
            string sResultado = "";
            int iCpainscodi = 0;
            string sTipinsumo = ConstantesCPPA.insumoGeneraciónProgramadaPMPO;
            string sTipproceso = "A"; //Automático
            continuar = true;

            try
            {
                //Calculamos las fechas para todo el año dtoCpaAjustePresupuestal.Cpaapanioejercicio
                DateTime dFecEjercicio = DateTime.ParseExact("01/01/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Inicio de año de ejercicio
                DateTime dFecEjercicioFin = DateTime.ParseExact("31/12/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Fin de año de ejercicio

                List<CpaCentralDTO> listaCentral = ListCpaCentralByRevision(dtoCpaRevision.Cparcodi);
                if (listaCentral.Count > 0)
                {
                    //Eliminando la información Importada anteriormente para esta Revisión e Insumo 
                    DeleteCpaInsumoDiaByRevision(dtoCpaRevision.Cparcodi, sTipinsumo);
                    DeleteCpaInsumoMesByRevision(dtoCpaRevision.Cparcodi, sTipinsumo);

                    //Insertamos el Insumo Año
                    iCpainscodi = InsertarInsumo(dtoCpaRevision.Cparcodi, sTipinsumo, sTipproceso, ".", sUser);

                    foreach (CpaCentralDTO dtoCentral in listaCentral)
                    {
                        //Para cada Central
                        //Para los 12 meses del año
                        for (int iMes = 1; iMes <= 12; iMes++)
                        {
                            DateTime dFecInicio = dFecEjercicio.AddMonths(iMes - 1);
                            DateTime dFecFin = dFecInicio.AddMonths(1).AddDays(-1); //Fin de mes del ejercicio

                            //Insertamos el registro CPA_INSUMO_MES de la Central
                            decimal dTotalMes = 0;
                            int iCpainmcodi = InsertarInsumoMes(iCpainscodi, dtoCpaRevision.Cparcodi, dtoCentral.Emprcodi, dtoCentral.Equicodi, sTipinsumo, sTipproceso, iMes, dTotalMes, sUser);
                            int iCpaindcodi = FactoryTransferencia.GetCpaInsumoDiaRepository().GetMaxId();
                            InsertarInsumoDiaBySddp(iCpaindcodi, iCpainmcodi, dtoCpaRevision.Cparcodi, dtoCentral.Emprcodi, dtoCentral.Equicodi, dFecInicio, dFecFin);
                            UpdateInsumoMesTotal(iCpainmcodi, dtoCentral.Equicodi, sTipinsumo, dFecInicio, dFecFin);
                        }
                    }
                    sResultado = "1";
                }
            }
            catch (Exception e)
            {
                Logger.Error(ConstantesAppServicio.LogError, e);
                sResultado = "Ha ocurrido un error en tiempo de ejecución. Por favor, comunicarse con el Administrador."; //e.Message + innerExceptionMessage;
                continuar = false;
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en CPA_INSUMO
                    if (iCpainscodi > 0)
                    {
                        CpaInsumoDTO dtoInsumo = GetByIdCpaInsumo(iCpainscodi);
                        sLog = sLog + "\n" + "Se finalizó la importación automática del insumo. ";
                        dtoInsumo.Cpainslog = sLog;
                        UpdateCpaInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    Logger.Error(ConstantesAppServicio.LogError, e2);
                    sResultado = "Ha ocurrido un error en tiempo de ejecución. Por favor, comunicarse con el Administrador."; //= e2.Message;
                    continuar = false;
                }
            }

            return sResultado;
        }

        #endregion

        #region CU10 - Importar insumo costo marginal programado
        /// <summary>
        /// Proceso que se encarga de ImportarCostoMarginal PMPO, por un ajuste de un año presupuesta y versión
        /// </summary>
        /// <param name="dtoCpaAjustePresupuestal">DTO CPA_AJUSTEPRESUPUESTAL</param>
        /// <param name="dtoCpaRevision">DTO CPA_REVISIÓN</param>
        /// <param name="sUser">Usuario en sesión</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarCostoMarginalPMPO(CpaAjustePresupuestalDTO dtoCpaAjustePresupuestal, CpaRevisionDTO dtoCpaRevision, string sUser)
        {
            string sResultado = "";
            int iCpainscodi = 0;
            string sTipinsumo = ConstantesCPPA.insumoCostoMarginalPMPO;
            string sTipproceso = "A"; //Automático
            string sLog = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se inicia el proceso de importación automática del insumo de Ene a Dic de " + dtoCpaAjustePresupuestal.Cpaapanioejercicio.ToString() + "<br>";
            List<string> logList = new List<string>();
            bool conErroresNoControlados = false;
            bool conErroresValidaciones = false;

            try
            {
                //Insertamos el Insumo
                iCpainscodi = InsertarInsumo(dtoCpaRevision.Cparcodi, sTipinsumo, sTipproceso, sLog, sUser);

                //Calculamos las fechas para todo el año dtoCpaAjustePresupuestal.Cpaapanioejercicio
                DateTime dFecEjercicio = DateTime.ParseExact("01/01/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Inicio de año de ejercicio
                DateTime dFecEjercicioFin = DateTime.ParseExact("31/12/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio, ConstantesCPPA.FormatoFecha, CultureInfo.InvariantCulture); //Fin de año de ejercicio

                #region Validaciones
                List<string> sMesesTipoCambioErrores = new List<string>();

                List<CpaCentralDTO> listaCentral = ListCpaCentralByRevision(dtoCpaRevision.Cparcodi);
                if (listaCentral.Count == 0)
                {
                    conErroresValidaciones = true;
                    sResultado = "No se encontrarón centrales registradas para la revisión seleccionada.";
                    throw new ArgumentException(sResultado);
                }
                for (int iMes = 1; iMes <= 12; iMes++)
                {
                    CpaParametroDTO dtoCpaParametro = FactoryTransferencia.GetCpaParametroRepository().GetByRevisionMes(dtoCpaRevision.Cparcodi, iMes);//3.7M + Convert.ToDecimal(iMes) * 0.001M;
                    if (dtoCpaParametro == null) {
                        sMesesTipoCambioErrores.Add(ConstantesCPPA.mesesDesc[iMes - 1]);
                    }
                }
                if (sMesesTipoCambioErrores.Count > 0) {
                    string sMesesSinTipoCambio = string.Join(", ", sMesesTipoCambioErrores);
                    conErroresValidaciones = true;

                    sResultado = "No se encontrarón tipos de cambios registradas para la revisión seleccionada para los siguientes meses: " + string.Join(", ", sMesesSinTipoCambio);
                    throw new ArgumentException(sResultado);
                }
                #endregion

                //Verifica si tiene el tipo de cambio para todo el año en ejercicio
                decimal[] dTipoCambio = new decimal[12];
                for (int iMes = 1; iMes <= 12; iMes++)
                {
                    //Hay que consulta CPA_PARAMETRO
                    //Si no existe información, detener la lectura y remitir un aviso al usuario

                    dTipoCambio[iMes - 1] = FactoryTransferencia.GetCpaParametroRepository().GetByRevisionMes(dtoCpaRevision.Cparcodi, iMes).Cpaprmcambio;//3.7M + Convert.ToDecimal(iMes) * 0.001M;
                                                                                                                                                          //result sResultado = "No existe el tipo de cambio para el mes: " + Convert.ToDecimal(iMes);

                }

                /* Elimina los datos de la tabla temporal la tabla temporal RER_INSUMO_CM_TEMP */
                FactoryTransferencia.GetRerInsumoRepository().TruncateTablaTemporal(ConstantesPrimasRER.tablaRerInsumoCmTemp);

                /* Elimina los datos de la tabla temporal la tabla temporal RER_INSUMO_DIA_TEMP */
                FactoryTransferencia.GetRerInsumoDiaRepository().TruncateTablaTemporal(ConstantesPrimasRER.tablaRerInsumoDiaTemp);

                //Leemos la información de CMgPMPO
                int iPeriAnioMes = dtoCpaRevision.Cparcmpmpo;
                //int iPeriAnioMes = int.Parse(dFecEjercicio.ToString("yyyyMM")); //ejemplo: 202401

                int codigoenvio = servicioPrimasRER.ObtenerCodigoEnvio(iPeriAnioMes);

                if (codigoenvio == 0)
                {
                    conErroresValidaciones = true;

                    sResultado = "No existe procesamiento para el periodo " + iPeriAnioMes.ToString() + ". No está permitido la descarga de información";
                    throw new ArgumentException(sResultado);
                }

                // Obtener los datos del PMPO para este mes
                List<MeMedicionxintervaloDTO> listaData = servicioPrimasRER.GetDatosPMPOReporteEnvio(codigoenvio, "I");

                List<RerInsumoTemporalDTO> lstTemporal = new List<RerInsumoTemporalDTO>();
                RerInsumoTemporalDTO entityInsumoTemporal;
                foreach (MeMedicionxintervaloDTO dataPmpo in listaData)
                {
                    entityInsumoTemporal = new RerInsumoTemporalDTO();
                    entityInsumoTemporal.Rerfecinicio = dataPmpo.Medintfechaini;
                    entityInsumoTemporal.Reretapa = Convert.ToInt32(dataPmpo.Semana.Split('/')[0]);
                    entityInsumoTemporal.Rerbloque = dataPmpo.Medintblqnumero;
                    entityInsumoTemporal.Ptomedicodi = dataPmpo.Ptomedicodi;
                    entityInsumoTemporal.Ptomedidesc = dataPmpo.Ptomedidesc;
                    entityInsumoTemporal.Rervalor = dataPmpo.Medinth1;

                    lstTemporal.Add(entityInsumoTemporal);
                }
                //Se inserta en la tabla RER_INSUMO_CM_TEMP
                FactoryTransferencia.GetRerInsumoRepository().BulkInsertTablaTemporal(lstTemporal, ConstantesPrimasRER.tablaRerInsumoCmTemp);

                /* Obtiene la lista procesada de insumos de los costos marginales pronosticada de acuerdo a la logica del CUS10 */
                List<RerInsumoDiaTemporalDTO> entitysInsumoDiaTemporal = ObtenerMatrizInsumoDia(dtoCpaRevision.Cparcodi);

                /* Inserta los registros en la tabla RER_INSUMO_DIA_TEMP; */
                FactoryTransferencia.GetRerInsumoDiaRepository().BulkInsertRerInsumoDiaTemporal(entitysInsumoDiaTemporal, ConstantesPrimasRER.tablaRerInsumoDiaTemp);
                /* El valor de los Costos Marginales importados a este punto estan en dolares */

                if (listaCentral.Count > 0)
                {
                    //Eliminando la información Importada anteriormente para esta Revisión e Insumo 
                    DeleteCpaInsumoDiaByRevision(dtoCpaRevision.Cparcodi, sTipinsumo);
                    DeleteCpaInsumoMesByRevision(dtoCpaRevision.Cparcodi, sTipinsumo);

                    //Para los 12 meses del año
                    for (int iMes = 1; iMes <= 12; iMes++)
                    {
                        //Para cada Central   
                        foreach (CpaCentralDTO dtoCentral in listaCentral)
                        {
                            if (dtoCentral.Ptomedicodi == null)
                                continue;

                            DateTime dFecInicio = dFecEjercicio.AddMonths(iMes - 1);
                            DateTime dFecFin = dFecInicio.AddMonths(1).AddDays(-1); //Fin de mes del ejercicio
                            decimal dTC = dTipoCambio[iMes - 1] / ConstantesCPPA.numero1000;

                            //Insertamos el registro CPA_INSUMO_MES de la Central
                            decimal dTotalMes = 0;
                            int iCpainmcodi = InsertarInsumoMes(iCpainscodi, dtoCpaRevision.Cparcodi, dtoCentral.Emprcodi, dtoCentral.Equicodi, sTipinsumo, sTipproceso, iMes, dTotalMes, sUser);
                            int iCpaindcodi = FactoryTransferencia.GetCpaInsumoDiaRepository().GetMaxId();
                            InsertarInsumoDiaByCMgPMPO(iCpaindcodi, iCpainmcodi, dtoCpaRevision.Cparcodi, dtoCentral.Emprcodi, dtoCentral.Equicodi, (int)dtoCentral.Ptomedicodi, dFecInicio, dFecFin, dTC, sUser);
                            UpdateInsumoMesTotal(iCpainmcodi, dtoCentral.Equicodi, sTipinsumo, dFecInicio, dFecFin);
                        }
                        sLog += "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Se procesó con éxito " + ConstantesCPPA.mesesDescCorta[iMes - 1] + "/" + dtoCpaAjustePresupuestal.Cpaapanioejercicio.ToString() + "<br>";
                    }
                    sResultado = "Finalizó satisfactoriamente la importación de '" + ConstantesCPPA.insumosDesc[int.Parse(sTipinsumo) - 1] + "' a la base de datos.";
                }
                conErroresNoControlados = false;
            }
            catch (Exception e)
            {
                Logger.Error(ConstantesAppServicio.LogError, e);
                string innerExceptionMessage = (e.InnerException != null) ? (" " + e.InnerException.Message) : "";
                sResultado = "Culminó con error la importación de '" + ConstantesCPPA.insumosDesc[int.Parse(sTipinsumo) - 1] + "' a la base de datos. Ver detalle en el Log de este insumo.";

                conErroresNoControlados = true;
                sLog = sLog + "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Error-> " + e.Message + ". Se canceló el proceso.<br>";
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en RER_INSUMO
                    if (iCpainscodi > 0)
                    {
                        CpaInsumoDTO dtoInsumo = GetByIdCpaInsumo(iCpainscodi);
                        if (conErroresNoControlados == false && conErroresValidaciones == false)
                        {
                            sLog += "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Finalizó el proceso de importación del insumo satisfactoriamente.";
                        }
                        else
                        {
                            sLog += "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Finalizó el proceso de importación del insumo con error.";
                        }
                        dtoInsumo.Cpainslog = sLog;
                        UpdateCpaInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    Logger.Error(ConstantesAppServicio.LogError, e2);
                    sResultado = e2.Message;
                }
            }

            return sResultado;
        }

        /// <summary>
        /// Llenar matriz de insumos de costos marginales pronosticado de acuerdo a la logica del ECUS10
        /// </summary>
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <returns>List RerGerCsvDetDTO</returns>
        public List<RerInsumoDiaTemporalDTO> ObtenerMatrizInsumoDia(int cparcodi)
        {
            List<RerInsumoDiaTemporalDTO> entitysRerInsumoDiaTemporalDTO = new List<RerInsumoDiaTemporalDTO>();
            //Definimos los dias de la semana
            char[] diasSemana = { 'S', 'D', 'L', 'M', 'X', 'J', 'V' };
            //Traemos la lista de feriados: listaferiado.Pmfrdofecha: Almacena las fechas que son Feriado PMPO
            List<PmoFeriadoDTO> listaferiado = FactorySic.GetPmoFeriadoRepository().List();
            //Traemos la lista de CentralesRER Activos
            List<CpaCentralDTO> entitysPtoMedicion = ListCpaCentralByRevision(cparcodi);
            foreach (CpaCentralDTO entidad in entitysPtoMedicion)
            {
                //Para cada PtoMedición en Centrarl RER
                if (entidad.Ptomedicodi == null)
                {
                    throw new Exception("Verificar que la central " + entidad.Equinomb + " tengan asociado una barra PMPO.");
                }
                int iPtoMediCodi = (int)entidad.Ptomedicodi;
                //Traemos la lista RerInsumoTemporalDTO de un punto de medición: 
                List<RerInsumoTemporalDTO> entitysRerInsumoTemporalDTO = FactoryTransferencia.GetRerGerCsvRepository().ListTablaCMTemporal(iPtoMediCodi);
                //Vamos a poblar la tabla CPA_INSUMO_DIA_TEMP
                int iLimiteEtapa = 80;
                if (entitysRerInsumoTemporalDTO.Count < 80) iLimiteEtapa = entitysRerInsumoTemporalDTO.Count;
                for (int iContadorEtapa = 0; iContadorEtapa < iLimiteEtapa; iContadorEtapa++)
                {
                    //Tomamos los bloques de 5 en 5
                    decimal dBloque1 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa].Rervalor;
                    decimal dBloque2 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa + 1].Rervalor;
                    decimal dBloque3 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa + 2].Rervalor;
                    decimal dBloque4 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa + 3].Rervalor;
                    decimal dBloque5 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa + 4].Rervalor;
                    //Las 7 fechas de la semana 

                    //iniciando en Sabado:
                    DateTime dFecha = entitysRerInsumoTemporalDTO[5 * iContadorEtapa].Rerfecinicio;
                    //string sPtomediDesc = entitysRerInsumoTemporalDTO[5 * iContadorEtapa].Ptomedidesc;
                    string sPtomediDesc = entidad.Equicodi.ToString();

                    for (int i = 0; i < diasSemana.Length; i++)
                    {
                        char sFeriado = 'N';
                        var feriadoEncontrado = listaferiado.Where(x => x.Pmfrdofecha == dFecha).FirstOrDefault();
                        if (feriadoEncontrado != null)
                            sFeriado = 'S';
                        entitysRerInsumoDiaTemporalDTO.Add(addRerInsumoDiaTemporal(iPtoMediCodi, sPtomediDesc, dFecha, diasSemana[i], dBloque1, dBloque2, dBloque3, dBloque4, dBloque5, sFeriado));
                        dFecha = dFecha.AddDays(1);
                    }
                }
            }

            return entitysRerInsumoDiaTemporalDTO;
        }

        /// <summary>
        /// Completar un dia de insumos de los costos marginales pronosticados de acuerdo a la logica del ECUS20
        /// </summary>
        /// <param name="iPtoMediCodi"></param>
        /// <param name="sPtomediDesc"></param>
        /// <param name="dFecha"></param>
        /// <param name="sDia"></param>
        /// <param name="dBloque1"></param>
        /// <param name="dBloque2"></param>
        /// <param name="dBloque3"></param>
        /// <param name="dBloque4"></param>
        /// <param name="dBloque5"></param>
        /// <param name="sFeriado"></param>
        /// <returns>RerInsumoDiaTemporalDTO</returns>
        public RerInsumoDiaTemporalDTO addRerInsumoDiaTemporal(int iPtoMediCodi, string sPtomediDesc, DateTime dFecha,
            char sDia, decimal dBloque1, decimal dBloque2, decimal dBloque3, decimal dBloque4, decimal dBloque5, char sFeriado)
        {
            RerInsumoDiaTemporalDTO dtoRerInsumoDiaTemporal = new RerInsumoDiaTemporalDTO();
            dtoRerInsumoDiaTemporal.Ptomedicodi = iPtoMediCodi;
            dtoRerInsumoDiaTemporal.Ptomedidesc = sPtomediDesc;
            dtoRerInsumoDiaTemporal.Rerinddiafecdia = dFecha;
            decimal dTotal = 0;
            //De Sabado a Viernes, el Bloque 5 se asigna sin restrigción
            //h1 -> h32
            int j = 1;
            while (j <= 32)
            {
                dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque5);
                dTotal += dBloque5;
                j++;
            }
            //h93 -> h96
            j = 93;
            while (j <= 96)
            {
                dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque5);
                dTotal += dBloque5;
                j++;
            }
            /**************************************************************************************************/
            //De Sabado a Viernes, el Bloque 4 se asigna en el horario del h33 -> h72,
            j = 33;
            while (j <= 72)
            {
                if ((j >= 45 && j <= 48) && ((sDia != 'S' && sDia != 'D') || sFeriado == 'S'))
                {
                    //con Excepciones los L, M. X, J y V en el intervalo h45-> h48 donde se asigna el Bloque 2
                    dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque2);
                    dTotal += dBloque2;
                }
                else
                {
                    dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque4);
                    dTotal += dBloque4;
                }
                j++;
            }
            /**************************************************************************************************/
            //De Sabado a Viernes, el Bloque 3 se asigna en el horario del h73 -> h92,
            j = 73;
            while (j <= 92)
            {
                if ((j >= 77 && j <= 78) && ((sDia != 'S' && sDia != 'D') || sFeriado == 'S'))
                {
                    //con Excepciones los L, M. X, J y V en el intervalo h77-> h78 donde se asigna el Bloque 1
                    dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque1);
                    dTotal += dBloque1;
                }
                else
                {
                    dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque3);
                    dTotal += dBloque3;
                }
                j++;
            }
            dtoRerInsumoDiaTemporal.Rerinddiatotal = dTotal;
            return dtoRerInsumoDiaTemporal;
        }
        #endregion

        #region CU11 - Calcular los totales de generación

        /// <summary>
        /// Obtener el log de un proceso de cálculo de totales de generación
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public string GetLogProceso(int cparcodi, out List<GenericoDTO> listaReporte)
        {
            try
            {
                if (cparcodi < 0)
                {
                    throw new MyCustomException("El código de la Revisión es inválido.");
                }

                CpaRevisionDTO revision = GetByIdCpaRevision(cparcodi);

                listaReporte = new List<GenericoDTO>();
                CpaCalculoDTO calculo = FactoryTransferencia.GetCpaCalculoRepository().GetByCriteria(revision.Cparcodi);
                if (calculo == null)
                {
                    return "";
                }

                string anio = revision.Cpaapanioejercicio.ToString().Substring(2);
                string nombreRevision = (revision.Cparrevision != ConstantesCPPA.revisionNormal) ? ("_Rev" + revision.Cparrevision.Substring(ConstantesCPPA.numero9)) : "";
                foreach (int i in ConstantesCPPA.numMeses)
                {
                    GenericoDTO item = new GenericoDTO
                    {
                        Entero1 = revision.Cparcodi,
                        Entero2 = i,
                        String1 = string.Format("{0}-{1}", ConstantesCPPA.mesesDesc[i - 1], revision.Cpaapanioejercicio),
                        String2 = string.Format("Montos_G{0}-{1}-{2}{3}.xlsx", i.ToString("D2"), anio, revision.Cpaapajuste, nombreRevision)
                    };
                    listaReporte.Add(item);
                }

                return calculo.Cpaclog;
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
        }

        /// <summary>
        /// Procesa el Cálculo de Totales de Generación
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="logProceso"></param>
        /// <param name="listaReporte"></param>
        public void ProcesarCalculo(int cparcodi, string usuario, out string logProceso, out List<GenericoDTO> listaReporte)
        {
            #region Definiendo Variables Globales
            logProceso = null;
            listaReporte = new List<GenericoDTO>();
            IDbConnection conn = null;
            DbTransaction tran = null;
            string cpaclog = "";
            StringBuilder sbLog = new StringBuilder();
            sbLog.Append("[Inf] ");
            sbLog.Append(DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2));
            sbLog.Append(" Inicio del Proceso [Usuario] ");
            sbLog.Append(usuario);
            sbLog.Append("\n");
            #endregion

            try
            {
                #region Validar Datos de Entrada
                if (cparcodi < 0)
                {
                    throw new MyCustomException("No se puede ejecutar el cálculo porque el código de la revisión es inválido.");
                }

                CpaRevisionDTO revision = GetByIdCpaRevision(cparcodi);
                string descRevision = string.Format("Revisión '{0}' del Ajuste '{1}' del Año Presupuestal '{2}'", revision.Cparrevision, revision.Cpaapajuste, revision.Cpaapanio);

                CpaPorcentajeDTO porcentaje = FactoryTransferencia.GetCpaPorcentajeRepository().GetByCriteria(revision.Cparcodi);
                if (porcentaje != null)
                {
                    throw new MyCustomException("No se puede ejecutar el cálculo porque existe un 'Cálculo de Porcentaje de Presupuesto' para dicha revisión. En caso desee ejecutar este cálculo, debe borrar el 'Cálculo de Porcentaje de Presupuesto' para dicha revisión.");
                }
                #endregion

                #region Obtener Datos para realizar el cálculo

                #region Obtener Insumos
                List<CpaInsumoDiaDTO> listInsumoDia = FactoryTransferencia.GetCpaInsumoDiaRepository().GetByRevisionByTipo(revision.Cparcodi, ConstantesCPPA.todos);
                bool existListInsumoDia = (listInsumoDia != null && listInsumoDia.Count > 0);
                if (!existListInsumoDia)
                {
                    throw new MyCustomException("No se puede ejecutar el cálculo porque no existen insumos importados para la " + descRevision);
                }
                #endregion

                #region Obtener Centrales
                List<CpaCentralDTO> listCentral = FactoryTransferencia.GetCpaCentralRepository().GetByRevisionByTipoEmpresaByEstadoEmpresaByEstadoCentral(revision.Cparcodi, ConstantesCPPA.tipoEmpresaGeneradora, ConstantesCPPA.estadoEmpresaActivo, ConstantesCPPA.estadoCentralActivo);
                bool existListCentral = (listCentral != null && listCentral.Count > 0);
                if (!existListCentral)
                {
                    throw new MyCustomException("No se puede ejecutar el cálculo porque no existen 'Empresas Generadoras' y 'Centrales de Generación' con estado activo para la " + descRevision);
                }

                List<CpaCentralPmpoDTO> listCentralPmpo = FactoryTransferencia.GetCpaCentralPmpoRepository().GetByRevision(revision.Cparcodi);

                listCentral = ObtenerCentralesValidas(listCentral, listCentralPmpo, out string logCentral);
                existListCentral = (listCentral != null && listCentral.Count > 0);
                if (!existListCentral)
                {
                    #region Guardar Datos del Proceso del Cálculo, debido a que no hay centrales válidas
                    sbLog.Append("[Inf] No existen centrales válidas para ser procesadas\n");
                    sbLog.Append("[Inf] ");
                    sbLog.Append(DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2));
                    sbLog.Append(" Fin del proceso\n");

                    if (logCentral != null && logCentral.Length > 0)
                    {
                        sbLog.Append("[Inf] Más Información:\n");
                        sbLog.Append(logCentral);
                    }
                    cpaclog = ObtenerLog(sbLog.ToString());

                    conn = FactoryTransferencia.GetCpaCalculoRepository().BeginConnection();
                    tran = FactoryTransferencia.GetCpaCalculoRepository().StartTransaction(conn);
                    FactoryTransferencia.GetCpaCalculoCentralRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                    FactoryTransferencia.GetCpaCalculoEmpresaRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                    FactoryTransferencia.GetCpaCalculoRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                    tran.Commit();

                    tran = FactoryTransferencia.GetRerCalculoMensualRepository().StartTransaction(conn);
                    int nextCpaccodi1 = FactoryTransferencia.GetCpaCalculoRepository().GetMaxId();
                    CpaCalculoDTO calculo1 = new CpaCalculoDTO
                    {
                        Cpaccodi = nextCpaccodi1,
                        Cparcodi = revision.Cparcodi,
                        Cpaclog = cpaclog,
                        Cpacusucreacion = usuario,
                        Cpacfeccreacion = DateTime.Now,
                    };
                    FactoryTransferencia.GetCpaCalculoRepository().Save(calculo1, conn, tran);
                    tran.Commit();

                    logProceso = cpaclog;
                    return;
                    #endregion
                }
                listCentral = listCentral.OrderBy(x => x.Emprcodi).ThenBy(x => x.Equicodi).ToList();
                #endregion

                #region Obtener Parametros de Máxima Demanda
                List<CpaParametroDTO> listParametro = FactoryTransferencia.GetCpaParametroRepository().GetByRevisionByEstado(revision.Cparcodi, ConstantesCPPA.estadoParametroActivo);
                string logParametro = ObtenerMesesSinMaximaDemandaYPrecioPotencia(listParametro);
                #endregion

                #endregion

                #region Obtener Cálculo de Totales de Generación

                #region Variables del Proceso 
                StringBuilder sbLogPeriodo = new StringBuilder();
                List<int> listEmprcodi = listCentral.Select(x => x.Emprcodi).Distinct().ToList();
                List<CpaInsumoDiaDTO> listEnergiaActivaEjecutada = listInsumoDia.Where(x => x.Cpaindtipinsumo == ConstantesCPPA.tipoInusmoEnergiaActivaEjecutada).ToList();
                List<CpaInsumoDiaDTO> listEnergiaActivaProgramada = listInsumoDia.Where(x => x.Cpaindtipinsumo == ConstantesCPPA.tipoInusmoEnergiaActivaProgramada).ToList();
                List<CpaInsumoDiaDTO> listCostoMarginalEjecutada = listInsumoDia.Where(x => x.Cpaindtipinsumo == ConstantesCPPA.tipoInusmoCostoMarginalEjecutada).ToList();
                List<CpaInsumoDiaDTO> listCostoMarginalProgramada = listInsumoDia.Where(x => x.Cpaindtipinsumo == ConstantesCPPA.tipoInusmoCostoMarginalProgramada).ToList();
                List<CpaCalculoEmpresaDTO> listCalculoEmpresa = new List<CpaCalculoEmpresaDTO>();
                #endregion

                #region Procesar Cálculo
                foreach (int mes in ConstantesCPPA.numMeses)
                {
                    #region Obtener Lista de Parametros MD Por Año y Mes
                    bool existListParametro = (listParametro != null && listParametro.Count > 0);
                    List<CpaParametroDTO> listParametroByMes = (!existListParametro) ? new List<CpaParametroDTO>() : listParametro.Where(x => x.Cpaprmanio == revision.Cpaapanioejercicio && x.Cpaprmmes == mes).ToList();
                    #endregion

                    foreach (int emprcodi in listEmprcodi)
                    {
                        #region Variables de Cálculo de la Empresa
                        decimal? cpacetotenemwh = null;
                        decimal? cpacetotenesoles = null;
                        decimal? cpacetotpotmwh = null;
                        decimal? cpacetotpotsoles = null;

                        List<CpaCalculoCentralDTO> listCalculoCentral = new List<CpaCalculoCentralDTO>();
                        List<CpaCentralDTO> lisCentralByEmpresa = listCentral.Where(x => x.Emprcodi == emprcodi).ToList();
                        List<CpaInsumoDiaDTO> listEAEByMesByEmpresa = listEnergiaActivaEjecutada.Where(x => x.Cpainmmes == mes && x.Emprcodi == emprcodi).ToList();
                        List<CpaInsumoDiaDTO> listEAPByMesByEmpresa = listEnergiaActivaProgramada.Where(x => x.Cpainmmes == mes && x.Emprcodi == emprcodi).ToList();
                        List<CpaInsumoDiaDTO> listCMEByMesByEmpresa = listCostoMarginalEjecutada.Where(x => x.Cpainmmes == mes && x.Emprcodi == emprcodi).ToList();
                        List<CpaInsumoDiaDTO> listCMPByMesByEmpresa = listCostoMarginalProgramada.Where(x => x.Cpainmmes == mes && x.Emprcodi == emprcodi).ToList();
                        #endregion

                        foreach (CpaCentralDTO central in lisCentralByEmpresa)
                        {
                            #region Variables de Cálculo de la Central
                            decimal? cpacctotenemwh = null;
                            decimal? cpacctotenesoles = null;
                            decimal? cpacctotpotmwh = null;
                            decimal? cpacctotpotsoles = null;

                            List<CpaInsumoDiaDTO> listEAEByMesByCentral = listEAEByMesByEmpresa.Where(x => x.Equicodi == central.Equicodi).ToList();
                            List<CpaInsumoDiaDTO> listEAPByMesByCentral = listEAPByMesByEmpresa.Where(x => x.Equicodi == central.Equicodi).ToList();
                            List<CpaInsumoDiaDTO> listCMEByMesByCentral = listCMEByMesByEmpresa.Where(x => x.Equicodi == central.Equicodi).ToList();
                            List<CpaInsumoDiaDTO> listCMPByMesByCentral = listCMPByMesByEmpresa.Where(x => x.Equicodi == central.Equicodi).ToList();
                            #endregion

                            #region Obtener Totales de Generación para la Central
                            ObtenerTotalesGeneracionPorMesPorCentral(revision.Cpaapanioejercicio, mes, central,
                                listEAEByMesByCentral, listEAPByMesByCentral, listCMEByMesByCentral, listCMPByMesByCentral, listParametroByMes,
                                out cpacctotenemwh, out cpacctotenesoles, out cpacctotpotmwh, out cpacctotpotsoles);

                            listCalculoCentral.Add(new CpaCalculoCentralDTO
                            {
                                Equicodi = central.Equicodi,
                                Barrcodi = central.Barrcodi.Value,
                                Cpacctotenemwh = cpacctotenemwh,
                                Cpacctotenesoles = cpacctotenesoles,
                                Cpacctotpotmwh = cpacctotpotmwh,
                                Cpacctotpotsoles = cpacctotpotsoles
                            });
                            #endregion

                            #region Sumar Totales de Generación para la Empresa
                            if (cpacctotenemwh != null) { cpacetotenemwh = (cpacetotenemwh != null ? cpacetotenemwh.Value : 0M) + cpacctotenemwh.Value; }
                            if (cpacctotenesoles != null) { cpacetotenesoles = (cpacetotenesoles != null ? cpacetotenesoles.Value : 0M) + cpacctotenesoles.Value; }
                            if (cpacctotpotmwh != null) { cpacetotpotmwh = (cpacetotpotmwh != null ? cpacetotpotmwh.Value : 0M) + cpacctotpotmwh.Value; }
                            if (cpacctotpotsoles != null) { cpacetotpotsoles = (cpacetotpotsoles != null ? cpacetotpotsoles.Value : 0M) + cpacctotpotsoles.Value; }
                            #endregion
                        }

                        #region Obtener Totales de Generacion para la Empresa
                        listCalculoEmpresa.Add(new CpaCalculoEmpresaDTO
                        {
                            Emprcodi = emprcodi,
                            Cpacetipo = ConstantesCPPA.tipoEmpresaGeneradora,
                            Cpacemes = mes,
                            Cpacetotenemwh = cpacetotenemwh,
                            Cpacetotenesoles = cpacetotenesoles,
                            Cpacetotpotmwh = cpacetotpotmwh,
                            Cpacetotpotsoles = cpacetotpotsoles,
                            ListCalculoCentral = listCalculoCentral
                        });
                        #endregion
                    }

                    #region Generando resultado del Log con respecto al mes
                    sbLogPeriodo.Append("[Inf] ");
                    sbLogPeriodo.Append(DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2));
                    sbLogPeriodo.Append(" Culminó el proceso ");
                    sbLogPeriodo.Append(ConstantesCPPA.mesesDescCorta[mes - 1]);
                    sbLogPeriodo.Append("-");
                    sbLogPeriodo.Append(revision.Cpaapanioejercicio);
                    sbLogPeriodo.Append("\n");
                    #endregion
                }

                #region Generando resultado final del Log
                sbLog.Append(sbLogPeriodo.ToString());
                sbLog.Append("[Inf] ");
                sbLog.Append(DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2));
                sbLog.Append(" Fin del proceso\n");

                if ((logParametro != null && logParametro.Length > 0) || (logCentral != null && logCentral.Length > 0))
                {
                    sbLog.Append("[Inf] Más Información:\n");
                    if (logParametro != null && logParametro.Length > 0) { sbLog.Append(logParametro); }
                    if (logCentral != null && logCentral.Length > 0) { sbLog.Append(logCentral); }
                }

                cpaclog = ObtenerLog(sbLog.ToString());
                #endregion
                #endregion

                #endregion

                #region Guardar Datos del Cálculo de Totales de Generación
                conn = FactoryTransferencia.GetCpaCalculoRepository().BeginConnection();
                tran = FactoryTransferencia.GetCpaCalculoRepository().StartTransaction(conn);
                FactoryTransferencia.GetCpaCalculoCentralRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaCalculoEmpresaRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaCalculoRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                tran.Commit();

                tran = FactoryTransferencia.GetCpaCalculoRepository().StartTransaction(conn);
                int nextCpaccodi = FactoryTransferencia.GetCpaCalculoRepository().GetMaxId();
                int nextCpacecodi = FactoryTransferencia.GetCpaCalculoEmpresaRepository().GetMaxId();
                int nextCpacccodi = FactoryTransferencia.GetCpaCalculoCentralRepository().GetMaxId();

                CpaCalculoDTO calculo = new CpaCalculoDTO
                {
                    Cpaccodi = nextCpaccodi,
                    Cparcodi = revision.Cparcodi,
                    Cpaclog = cpaclog,
                    Cpacusucreacion = usuario,
                    Cpacfeccreacion = DateTime.Now
                };
                FactoryTransferencia.GetCpaCalculoRepository().Save(calculo, conn, tran);

                foreach (CpaCalculoEmpresaDTO calculoEmpresa in listCalculoEmpresa)
                {
                    calculoEmpresa.Cpacecodi = nextCpacecodi;
                    calculoEmpresa.Cpaccodi = nextCpaccodi;
                    calculoEmpresa.Cparcodi = revision.Cparcodi;
                    calculoEmpresa.Cpaceusucreacion = usuario;
                    calculoEmpresa.Cpacefeccreacion = DateTime.Now;
                    FactoryTransferencia.GetCpaCalculoEmpresaRepository().Save(calculoEmpresa, conn, tran);

                    foreach (CpaCalculoCentralDTO calculoCentral in calculoEmpresa.ListCalculoCentral)
                    {
                        calculoCentral.Cpacccodi = nextCpacccodi;
                        calculoCentral.Cpacecodi = nextCpacecodi;
                        calculoCentral.Cpaccodi = nextCpaccodi;
                        calculoCentral.Cparcodi = revision.Cparcodi;
                        calculoCentral.Cpaccusucreacion = usuario;
                        calculoCentral.Cpaccfeccreacion = DateTime.Now;
                        FactoryTransferencia.GetCpaCalculoCentralRepository().Save(calculoCentral, conn, tran);
                        nextCpacccodi++;
                    }

                    nextCpacecodi++;
                }

                tran.Commit();
                #endregion

                #region Llenar Log y Listado de Reportes a ser retornados
                logProceso = cpaclog;
                string anio = revision.Cpaapanioejercicio.ToString().Substring(2);
                string nombre_revision = (revision.Cparrevision != ConstantesCPPA.revisionNormal) ? ("_Rev" + revision.Cparrevision.Substring(ConstantesCPPA.numero9)) : "";
                foreach (int i in ConstantesCPPA.numMeses)
                {
                    GenericoDTO item = new GenericoDTO
                    {
                        Entero1 = revision.Cparcodi,
                        Entero2 = i,
                        String1 = string.Format("{0}-{1}", ConstantesCPPA.mesesDesc[i - 1], revision.Cpaapanioejercicio),
                        String2 = string.Format("Montos_G{0}-{1}-{2}{3}.xlsx", i.ToString("D2"), anio, revision.Cpaapajuste, nombre_revision)
                    };
                    listaReporte.Add(item);
                }
                #endregion
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
            finally
            {
                bool conexionEstaAbierta = (conn != null && conn.State == ConnectionState.Open);
                if (conexionEstaAbierta)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Elimina el Cálculo de Totales de Generación
        /// </summary>
        /// <param name="cparcodi"></param>
        public void EliminarCalculo(int cparcodi)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validaciones
                if (cparcodi < 0)
                {
                    throw new MyCustomException("No se puede eliminar el cálculo porque él código de la revisión es inválido.");
                }

                CpaRevisionDTO revision = GetByIdCpaRevision(cparcodi);
                if (revision.Cparestado == ConstantesCPPA.estadoRevisionCerrado)
                {
                    throw new MyCustomException("No se puede eliminar el cálculo porque su revisión está con estado 'Cerrado'. En caso desee eliminarlo, cambie el estado de la revisión a 'Abierto', e inténtelo de nuevo.");
                }

                CpaPorcentajeDTO porcentaje = FactoryTransferencia.GetCpaPorcentajeRepository().GetByCriteria(revision.Cparcodi);
                if (porcentaje != null)
                {
                    throw new MyCustomException("No se puede eliminar el cálculo porque existe un 'Cálculo de Porcentaje de Presupuesto' para dicha revisión. En caso desee eliminar este cálculo, debe borrar el 'Cálculo de Porcentaje de Presupuesto' para dicha revisión.");
                }
                #endregion

                #region Eliminar Cálculo
                conn = FactoryTransferencia.GetCpaCalculoRepository().BeginConnection();
                tran = FactoryTransferencia.GetCpaCalculoRepository().StartTransaction(conn);
                FactoryTransferencia.GetCpaCalculoCentralRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaCalculoEmpresaRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaCalculoRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                tran.Commit();
                #endregion
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
            finally
            {
                bool conexionEstaAbierta = (conn != null && conn.State == ConnectionState.Open);
                if (conexionEstaAbierta)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Obtiene las centrales que tengan sus datos son válidos. 
        /// Adicionalmente, para las centrales válidas obtiene sus fechas programadas antes de las fechas ejecutadas en caso existiesen.
        /// </summary>
        /// <param name="listCentral"></param>
        /// <param name="listCentralPmpo"></param>
        /// <param name="log">En caso hubiera algún error, lo especifica en esta variable</param>
        /// <returns></returns>
        private List<CpaCentralDTO> ObtenerCentralesValidas(List<CpaCentralDTO> listCentral, List<CpaCentralPmpoDTO> listCentralPmpo, out string log)
        {
            try
            {
                log = null;
                StringBuilder sbLog = new StringBuilder();
                StringBuilder sbLogLVTEA = new StringBuilder();
                StringBuilder sbLogPMPO = new StringBuilder();
                List<CpaCentralDTO> list = new List<CpaCentralDTO>();

                foreach (CpaCentralDTO central in listCentral)
                {
                    bool existBarraTransferenciaLVTEA = (central.Barrcodi != null && central.Barrcodi > 0);
                    if (!existBarraTransferenciaLVTEA)
                    {
                        sbLogLVTEA.Append("[Inf] ");
                        sbLogLVTEA.Append(central.Equinomb);
                        sbLogPMPO.Append("\n");
                        continue;
                    }

                    bool sonFechasProgramadasValidas = FechasValidas(central.Cpacntfecproginicio, central.Cpacntfecprogfin);
                    bool existBarraPMPO = (central.Ptomedicodi != null && central.Ptomedicodi > 0);
                    bool existListCentralPmpoByCentral = false;
                    bool existListCentralPmpo = (listCentralPmpo != null && listCentralPmpo.Count > 0);
                    if (existListCentralPmpo)
                    {
                        List<CpaCentralPmpoDTO> listCentraPmpoByCentral = listCentralPmpo.Where(x => x.Cpacntcodi == central.Cpacntcodi).ToList();
                        existListCentralPmpoByCentral = (listCentraPmpoByCentral.Count > 0);
                    }

                    //if (sonFechasProgramadasValidas && (!existListCentralPmpoByCentral || !existBarraPMPO))
                    //{
                    //    sbLogPMPO.Append("[Inf] ");
                    //    sbLogPMPO.Append(central.Equinomb);
                    //    sbLogPMPO.Append("\n");
                    //    continue;
                    //}

                    ObtenerFechasProgramadasAntesFechasEjecutadas(central.Cpacntfecejecinicio, out DateTime? fechaInicio, out DateTime? fechaFin);
                    central.Cpacntfecproginicioantesfecejec = fechaInicio;
                    central.Cpacntfecprogfinantesfecejec = fechaFin;

                    list.Add(central);
                }

                if (sbLogLVTEA.Length > 0)
                {
                    sbLog.Append("[Inf] Centrales que no participaron en el cálculo, por no contar con una barra LVTEA:\n");
                    sbLog.Append(sbLogLVTEA.ToString());
                }

                if (sbLogPMPO.Length > 0)
                {
                    sbLog.Append("[Inf] Centrales que no participaron en el cálculo, por no contar con una central PMPO y/o una barra PMPO:\n");
                    sbLog.Append(sbLogPMPO.ToString());
                }

                log = sbLog.ToString();
                return list;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene en un mensaje de log, los meses que no tienen Máxima Demanda y Precio de Potencia
        /// </summary>
        /// <param name="listParametro"></param>
        /// <returns></returns>
        private string ObtenerMesesSinMaximaDemandaYPrecioPotencia(List<CpaParametroDTO> listParametro)
        {
            StringBuilder sbLogParametro = new StringBuilder();

            if (listParametro == null || listParametro.Count < 1)
            {
                sbLogParametro.Append("[Inf] Meses sin datos de Máxima Demanda y Precio de Potencia:\n");
                sbLogParametro.Append("[Inf] Ningún mes tiene datos.\n");
            }
            else
            {
                foreach (int mes in ConstantesCPPA.numMeses)
                {
                    List<CpaParametroDTO> list = listParametro.Where(x => x.Cpaprmmes == mes).ToList();
                    if (list.Count < 1)
                    {
                        if (sbLogParametro.Length > 0)
                        {
                            sbLogParametro.Append(", ");
                        }

                        if (sbLogParametro.Length < 1)
                        {
                            sbLogParametro.Append("[Inf] Meses sin datos de Máxima Demanda y Precio de Potencia:\n");
                            sbLogParametro.Append("[Inf] ");
                        }

                        sbLogParametro.Append(ConstantesCPPA.mesesDesc[mes - 1]);
                    }
                }
                if (sbLogParametro.Length > 0)
                {
                    sbLogParametro.Append("\n");
                }
            }

            return sbLogParametro.ToString();
        }

        /// <summary>
        /// Revisa que los valores de las fechas de la Central, sean válidos
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private bool FechasValidas(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (fechaInicio == null || fechaFin == null) { return false; }
            if (fechaFin.Value < fechaInicio.Value) { return false; }
            return true;
        }

        /// <summary>
        /// Obtener totales generación de una central, para un mes específico
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="central"></param>
        /// <param name="listEnergiaActivaEjecutada">listEnergiaActivaEjecutada de un mes y de una central</param>
        /// <param name="listEnergiaActivaProgramada">listEnergiaActivaProgramada de un mes y de una central</param>
        /// <param name="listCostoMarginalEjecutada">listCostoMarginalEjecutada de un mes y de una central</param>
        /// <param name="listCostoMarginalProgramada">listCostoMarginalProgramada de un mes y de una central</param>
        /// <param name="listParametroByMes"></param>
        /// <param name="cpacctotenemwh"></param>
        /// <param name="cpacctotenesoles"></param>
        /// <param name="cpacctotpotmwh"></param>
        /// <param name="cpacctotpotsoles"></param>
        private void ObtenerTotalesGeneracionPorMesPorCentral(int anio, int mes, CpaCentralDTO central,
            List<CpaInsumoDiaDTO> listEnergiaActivaEjecutada, List<CpaInsumoDiaDTO> listEnergiaActivaProgramada,
            List<CpaInsumoDiaDTO> listCostoMarginalEjecutada, List<CpaInsumoDiaDTO> listCostoMarginalProgramada,
            List<CpaParametroDTO> listParametroByMes,
            out decimal? cpacctotenemwh, out decimal? cpacctotenesoles, out decimal? cpacctotpotmwh, out decimal? cpacctotpotsoles)
        {
            try
            {
                #region Definición Variables
                cpacctotenemwh = null;
                cpacctotenesoles = null;
                cpacctotpotmwh = null;
                cpacctotpotsoles = null;
                bool sonFechasEjecutadaValidas = FechasValidas(central.Cpacntfecejecinicio, central.Cpacntfecejecfin);
                bool sonFechasProgramadaValidas = FechasValidas(central.Cpacntfecproginicio, central.Cpacntfecprogfin);
                bool sonFechasProgramadaAntesFechasEjecutadasValidas = FechasValidas(central.Cpacntfecproginicioantesfecejec, central.Cpacntfecprogfinantesfecejec);
                bool existListEAEjecutada = listEnergiaActivaEjecutada != null && listEnergiaActivaEjecutada.Count > 0;
                bool existListCMEjecutada = listCostoMarginalEjecutada != null && listCostoMarginalEjecutada.Count > 0;
                bool existListEAProgramada = listEnergiaActivaProgramada != null && listEnergiaActivaProgramada.Count > 0;
                bool existListCMProgramada = listCostoMarginalProgramada != null && listCostoMarginalProgramada.Count > 0;
                #endregion

                #region Operaciones Fechas Programadas antes de las Fechas de Ejecucíón 
                if (sonFechasProgramadaAntesFechasEjecutadasValidas && existListEAProgramada)
                {
                    ObtenerDiasDelPeriodo(anio, mes,
                        central.Cpacntfecproginicioantesfecejec.Value, central.Cpacntfecprogfinantesfecejec.Value,
                        out DateTime? fechaProgramadaInicioMes, out DateTime? fechaProgramadaFinMes);

                    ObtenerTotalesGeneracion(fechaProgramadaInicioMes, fechaProgramadaFinMes,
                        listParametroByMes,
                        listEnergiaActivaProgramada, existListCMProgramada, listCostoMarginalProgramada,
                        ref cpacctotenemwh, ref cpacctotenesoles, ref cpacctotpotmwh, ref cpacctotpotsoles);
                }
                #endregion

                #region Operaciones Fechas Ejecutadas
                if (sonFechasEjecutadaValidas && existListEAEjecutada)
                {
                    ObtenerDiasDelPeriodo(anio, mes,
                        central.Cpacntfecejecinicio.Value, central.Cpacntfecejecfin.Value,
                        out DateTime? fechaEjecutadaInicioMes, out DateTime? fechaEjecutadaFinMes);

                    ObtenerTotalesGeneracion(fechaEjecutadaInicioMes, fechaEjecutadaFinMes,
                        listParametroByMes,
                        listEnergiaActivaEjecutada, existListCMEjecutada, listCostoMarginalEjecutada,
                        ref cpacctotenemwh, ref cpacctotenesoles, ref cpacctotpotmwh, ref cpacctotpotsoles);
                }
                #endregion

                #region Operaciones Fechas Programadas después de las Fechas de Ejecución
                if (sonFechasProgramadaValidas && existListEAProgramada)
                {
                    ObtenerDiasDelPeriodoParaProgramadas(anio, mes,
                        central.Cpacntfecproginicio.Value, central.Cpacntfecprogfin.Value,
                        out DateTime? fechaProgramadaInicioMes, out DateTime? fechaProgramadaFinMes);

                    ObtenerTotalesGeneracion(fechaProgramadaInicioMes, fechaProgramadaFinMes,
                        listParametroByMes,
                        listEnergiaActivaProgramada, existListCMProgramada, listCostoMarginalProgramada,
                        ref cpacctotenemwh, ref cpacctotenesoles, ref cpacctotpotmwh, ref cpacctotpotsoles);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene el rango de días del mes-anio que debe recorrer
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="centralFechaInicio"></param>
        /// <param name="centralFechaFin"></param>
        /// <param name="fechaInicioMes"></param>
        /// <param name="fechaFinMes"></param>
        private void ObtenerDiasDelPeriodo(int anio, int mes,
            DateTime centralFechaInicio, DateTime centralFechaFin,
            out DateTime? fechaInicioMes, out DateTime? fechaFinMes)
        {
            fechaInicioMes = null;
            fechaFinMes = null;

            int centralMesInicio = centralFechaInicio.Month;
            int centralAnioInicio = centralFechaInicio.Year;
            int centralMesFin = centralFechaFin.Month;
            int centralAnioFin = centralFechaFin.Year;

            if (mes == centralMesInicio && anio == centralAnioInicio && mes == centralMesFin && anio == centralAnioFin)
            {
                fechaInicioMes = centralFechaInicio;
                fechaFinMes = centralFechaFin;
            }
            else if (mes > centralMesInicio && mes < centralMesFin && anio == centralAnioInicio && anio == centralAnioFin)
            {
                fechaInicioMes = new DateTime(anio, mes, 1, 0, 0, 0);
                fechaFinMes = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes), 0, 0, 0);
            }
            else if (mes == centralMesInicio && anio == centralAnioInicio && mes < centralMesFin && anio == centralAnioFin)
            {
                fechaInicioMes = centralFechaInicio;
                fechaFinMes = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes), 0, 0, 0);
            }
            else if (mes > centralMesInicio && anio == centralAnioInicio && mes == centralMesFin && anio == centralAnioFin)
            {
                fechaInicioMes = new DateTime(anio, mes, 1, 0, 0, 0);
                fechaFinMes = centralFechaFin;
            }
        }

        /// <summary>
        /// Obtiene el rango de días del mes-anio que debe recorrer, para las fechas programadas de una central
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="centralFechaInicio"></param>
        /// <param name="centralFechaFin"></param>
        /// <param name="fechaInicioMes"></param>
        /// <param name="fechaFinMes"></param>
        private void ObtenerDiasDelPeriodoParaProgramadas(int anio, int mes,
            DateTime centralFechaInicio, DateTime centralFechaFin,
            out DateTime? fechaInicioMes, out DateTime? fechaFinMes)
        {
            fechaInicioMes = null;
            fechaFinMes = null;
            int centralAnioInicio = centralFechaInicio.Year;
            int centralAnioFin = centralFechaFin.Year;

            if (centralAnioInicio == anio && centralAnioFin == anio)
            {
                ObtenerDiasDelPeriodo(anio, mes,
                    centralFechaInicio, centralFechaFin,
                    out fechaInicioMes, out fechaFinMes);
            }
            else if (centralAnioInicio == anio && centralAnioFin == (anio + 1))
            {
                ObtenerDiasDelPeriodo(anio, mes,
                    centralFechaInicio, new DateTime(anio, 12, 31, 0, 0, 0),
                    out fechaInicioMes, out fechaFinMes);
            }
        }

        /// <summary>
        /// Obtiene el parametro en base al tipo de máxima demanda
        /// </summary>
        /// <param name="listParametroActivosByMes">Lista de parámetros activos de un periodo</param>
        /// <returns></returns>
        private CpaParametroDTO ObtenerParametro(List<CpaParametroDTO> listParametroActivosByMes)
        {
            CpaParametroDTO parametro = null;
            if (listParametroActivosByMes != null && listParametroActivosByMes.Count > 0)
            {
                parametro = listParametroActivosByMes[0];
            }
            return parametro;
        }

        /// <summary>
        /// Obtener fechas programadas antes de las fechas ejecutadas de una central
        /// </summary>
        /// <param name="centralFechaInicio"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private void ObtenerFechasProgramadasAntesFechasEjecutadas(DateTime? centralFechaInicio,
            out DateTime? fechaInicio, out DateTime? fechaFin)
        {
            fechaInicio = null;
            fechaFin = null;

            if (centralFechaInicio != null)
            {
                DateTime fechaInicial = new DateTime(centralFechaInicio.Value.Year, 1, 1, 0, 0, 0);
                int result = DateTime.Compare(fechaInicial, centralFechaInicio.Value);

                if (result < 0)
                {
                    fechaInicio = new DateTime(centralFechaInicio.Value.Year, 1, 1, 0, 0, 0);
                    fechaFin = centralFechaInicio.Value.AddDays(-1);
                }
            }
        }


        /// <summary>
        /// Obtiene los totales de generación para un rango de fecha especificado, tipo de registro de MD, tipo de energía activa y tipo de costo marginal
        /// </summary>
        /// <param name="fechaInicioMes"></param>
        /// <param name="fechaFinMes"></param>
        /// <param name="listParametroByMes"></param>
        /// <param name="listEnergiaActiva"></param>
        /// <param name="existListCostoMarginal"></param>
        /// <param name="listCostoMarginal"></param>
        /// <param name="cpacctotenemwh"></param>
        /// <param name="cpacctotenesoles"></param>
        /// <param name="cpacctotpotmwh"></param>
        /// <param name="cpacctotpotsoles"></param>
        private void ObtenerTotalesGeneracion(DateTime? fechaInicioMes, DateTime? fechaFinMes,
            List<CpaParametroDTO> listParametroByMes,
            List<CpaInsumoDiaDTO> listEnergiaActiva, bool existListCostoMarginal, List<CpaInsumoDiaDTO> listCostoMarginal,
            ref decimal? cpacctotenemwh, ref decimal? cpacctotenesoles, ref decimal? cpacctotpotmwh, ref decimal? cpacctotpotsoles)
        {
            bool sonFechasValidas = FechasValidas(fechaInicioMes, fechaFinMes);
            if (sonFechasValidas)
            {
                #region Obtener Parametro Máxima Demanda por Tipo de Registro MD
                CpaParametroDTO parametro = ObtenerParametro(listParametroByMes);
                #endregion

                for (DateTime fecha = fechaInicioMes.Value; fecha <= fechaFinMes.Value; fecha = fecha.AddDays(1))
                {
                    #region Total Energía MWh
                    List<CpaInsumoDiaDTO> listEAByDia = listEnergiaActiva.Where(x => x.Cpainddia == fecha).ToList();
                    if (listEAByDia.Count > 0)
                    {
                        cpacctotenemwh = (cpacctotenemwh != null ? cpacctotenemwh.Value : 0M) + listEAByDia[0].Cpaindtotaldia;
                    }
                    #endregion

                    #region Total Energía en soles
                    bool existValues = (listEAByDia.Count > 0 && existListCostoMarginal);
                    if (existValues)
                    {
                        List<CpaInsumoDiaDTO> listCMByDia = listCostoMarginal.Where(x => x.Cpainddia == fecha).ToList();
                        if (listCMByDia.Count > 0)
                        {
                            for (int i = 1; i <= ConstantesCPPA.numero96; i++)
                            {
                                decimal value_ea_n_t = (decimal)listEAByDia[0].GetType().GetProperty("Cpaindh" + i).GetValue(listEAByDia[0]);
                                decimal value_cm_n_t = (decimal)listCMByDia[0].GetType().GetProperty("Cpaindh" + i).GetValue(listCMByDia[0]);
                                cpacctotenesoles = (cpacctotenesoles != null ? cpacctotenesoles.Value : 0M) + (value_ea_n_t * value_cm_n_t);
                            }
                        }
                    }
                    #endregion

                    #region Total Potencia MW y Total Potencia en soles
                    existValues = (listEAByDia.Count > 0 && parametro != null && parametro.Cpaprmfechamd.Date == fecha);
                    if (existValues)
                    {
                        int hour = parametro.Cpaprmfechamd.Hour;
                        int minute = parametro.Cpaprmfechamd.Minute;
                        int minutes = hour * ConstantesCPPA.numero60 + minute;
                        int i = minutes == 0 ? ConstantesCPPA.numero96 : (minutes / ConstantesCPPA.numero15);

                        decimal value_ea_n_t = (decimal)listEAByDia[0].GetType().GetProperty("Cpaindh" + i).GetValue(listEAByDia[0]);
                        cpacctotpotmwh = (cpacctotpotmwh != null ? cpacctotpotmwh.Value : 0M) + (value_ea_n_t * ConstantesCPPA.numero4);
                        cpacctotpotsoles = (cpacctotpotsoles != null ? cpacctotpotsoles.Value : 0M) + (value_ea_n_t * ConstantesCPPA.numero4 * parametro.Cpaprmprecio);
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Obtiene el log final. En donde, si pase su longuitud de 2000, trunca su contenido
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private string ObtenerLog(string log)
        {
            string result = (log != null) ? log.Trim() : "";

            if (result.Length > ConstantesCPPA.numero2000)
            {
                result = string.Format("{0}{1}", result.Substring(0, ConstantesCPPA.numero1997), "...");
            }

            return result;
        }

        #endregion

        #region CU12 - Generar reportes de totales de generación 

        /// <summary>
        /// Genera los datos del archivo Excel para el Reporte de Generación
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cpacemes"></param>
        /// <param name="invocadoPor"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="listExcelHoja"></param>
        public void GenerarArchivoExcelReporteGeneracion(int cparcodi, int cpacemes, string invocadoPor, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja)
        {
            try
            {
                #region Validación
                if (cparcodi < 0)
                {
                    throw new MyCustomException("No se puede generar el reporte porque el código de la revisión es inválido.");
                }

                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existe la Revisión con id = " + cparcodi);
                }

                if (!ConstantesCPPA.numMeses.Contains(cpacemes))
                {
                    throw new MyCustomException("No se puede generar el reporte porque el mes es inválido. Mes = " + cpacemes);
                }

                ValidarReporteParaExtranet(revision, invocadoPor);
                #endregion

                #region Obtener datos
                List<CpaCalculoEmpresaDTO> listCalculoEmpresa = FactoryTransferencia.GetCpaCalculoEmpresaRepository().GetByCriteria(revision.Cparcodi, cpacemes.ToString());
                bool existeListCalculoEmpresa = (listCalculoEmpresa != null && listCalculoEmpresa.Count > 0);
                if (!existeListCalculoEmpresa)
                {
                    string mensaje = (invocadoPor == ConstantesCPPA.invocadoPorExtranet) 
                        ? "No se puede generar el reporte porque no existen registros a exportar."
                        : string.Format("No se puede generar el reporte porque no existen registros a exportar. Por favor, ejecute el proceso del cálculo de Totales de Generación para la Revisión '{0}' del Ajuste '{1}' del Año Presupuestal '{2}'.", revision.Cparrevision, revision.Cpaapajuste, revision.Cpaapanio);

                    throw new MyCustomException(mensaje);
                }
                listCalculoEmpresa = listCalculoEmpresa.OrderBy(x => x.Emprnomb).ThenBy(x => x.Cpacemes).ToList();

                List<CpaCalculoCentralDTO> listCalculoCentral = FactoryTransferencia.GetCpaCalculoCentralRepository().GetByCriteria(revision.Cparcodi, cpacemes.ToString());
                bool existeListCalculoCentral = (listCalculoCentral != null && listCalculoCentral.Count > 0);
                if (!existeListCalculoCentral)
                {
                    string mensaje = (invocadoPor == ConstantesCPPA.invocadoPorExtranet)
                        ? "No se puede generar el reporte porque no existen registros a exportar."
                        : string.Format("No se puede generar el reporte porque no existen registros de Centrales de Generación para el filtro de búsqueda: cparcodi = {0} y cpacemes = {1}", cparcodi, cpacemes);

                    throw new MyCustomException(mensaje);
                }
                listCalculoCentral = listCalculoCentral.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.BarrBarraTransferencia).ToList();
                #endregion

                #region Variables
                string nombreRevision = (revision.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("_Rev" + revision.Cparrevision.Substring(ConstantesCPPA.numero9));
                nombreArchivo = string.Format("Montos_G{0}-{1}-{2}{3}", cpacemes.ToString("D2"), revision.Cpaapanioejercicio.ToString().Substring(2), revision.Cpaapajuste, nombreRevision);
                listExcelHoja = new List<CpaExcelHoja>();
                int i = 0;
                #endregion

                #region Hoja Resumen

                #region Titulos
                nombreRevision = (revision.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("Rev. " + revision.Cparrevision.Substring(ConstantesCPPA.numero9));
                StringBuilder sbTitulo = new StringBuilder();
                sbTitulo.Append("Total de Generación - Presupuesto Anual ");
                sbTitulo.Append(revision.Cpaapanio);
                StringBuilder sbSubTitulo1 = new StringBuilder();
                sbSubTitulo1.Append("Ajuste ");
                sbSubTitulo1.Append(revision.Cpaapajuste.Substring(1));
                if (nombreRevision.Length > 0)
                {
                    sbSubTitulo1.Append(" - ");
                    sbSubTitulo1.Append(nombreRevision);
                }
                sbSubTitulo1.Append(" - ");
                sbSubTitulo1.Append(cpacemes.ToString("D2"));
                sbSubTitulo1.Append(" ");
                sbSubTitulo1.Append(ConstantesCPPA.mesesDesc[cpacemes - 1]);
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasResumen = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabeceraResumen1 = new List<CpaExcelModelo> {
                    CrearExcelModelo(sbTitulo.ToString(), "left"),
                    CrearExcelModelo("Energía", "center", 2),
                    CrearExcelModelo("Potencia", "center", 2)
                };
                List<CpaExcelModelo> listaCabeceraResumen2 = new List<CpaExcelModelo> {
                    CrearExcelModelo(sbSubTitulo1.ToString(), "left"),
                    CrearExcelModelo("MWh", "center"),
                    CrearExcelModelo("S/.", "center"),
                    CrearExcelModelo("MW", "center"),
                    CrearExcelModelo("S/.", "center")
                };
                List<int> listaAnchoColumnaResumen = new List<int> { 40, 15, 15, 15, 15 };

                listaCabecerasResumen[0] = listaCabeceraResumen1;
                listaCabecerasResumen[1] = listaCabeceraResumen2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalResumen = new List<string> { "left", "right", "right", "right", "right" };
                List<string> listaTipoResumen = new List<string> { "string", "double", "double", "double", "double" };
                List<CpaExcelEstilo> listaEstiloResumen = new List<CpaExcelEstilo> {
                    CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00")
                };

                i = 0;
                List<string>[] listaRegistrosResumen = new List<string>[listCalculoEmpresa.Count];
                foreach (CpaCalculoEmpresaDTO calculoEmpresa in listCalculoEmpresa)
                {
                    listaRegistrosResumen[i] = new List<string> {
                        calculoEmpresa.Emprnomb,
                        calculoEmpresa.Cpacetotenemwh == null ? "" : calculoEmpresa.Cpacetotenemwh.Value.ToString(),
                        calculoEmpresa.Cpacetotenesoles == null ? "" : calculoEmpresa.Cpacetotenesoles.Value.ToString(),
                        calculoEmpresa.Cpacetotpotmwh == null ? "" : calculoEmpresa.Cpacetotpotmwh.Value.ToString(),
                        calculoEmpresa.Cpacetotpotsoles == null ? "" : calculoEmpresa.Cpacetotpotsoles.Value.ToString()
                    };
                    i++;
                }

                CpaExcelCuerpo cuerpoResumen = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosResumen,
                    ListaAlineaHorizontal = listaAlineaHorizontalResumen,
                    ListaTipo = listaTipoResumen,
                    ListaEstilo = listaEstiloResumen
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaResumen = new CpaExcelHoja
                {
                    NombreHoja = "Resumen",
                    Titulo = sbTitulo.ToString(),
                    Subtitulo1 = sbSubTitulo1.ToString(),
                    ListaAnchoColumna = listaAnchoColumnaResumen,
                    ListaCabeceras = listaCabecerasResumen,
                    Cuerpo = cuerpoResumen
                };
                listExcelHoja.Add(excelHojaResumen);
                #endregion

                #endregion

                #region Hoja Detalle

                #region Titulos
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasDetalle = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabeceraDetalle1 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Empresa", "center", 1, 2),
                    CrearExcelModelo("Central", "center", 1, 2),
                    CrearExcelModelo("Barra", "center", 1, 2),
                    CrearExcelModelo("Energía", "center", 2),
                    CrearExcelModelo("Potencia", "center", 2)
                };
                List<CpaExcelModelo> listaCabeceraDetalle2 = new List<CpaExcelModelo> {
                    CrearExcelModelo("MWh", "center"),
                    CrearExcelModelo("S/.", "center"),
                    CrearExcelModelo("MW", "center"),
                    CrearExcelModelo("S/.", "center")
                };
                List<int> listaAnchoColumnaDetalle = new List<int> { 40, 40, 40, 15, 15, 15, 15 };

                listaCabecerasDetalle[0] = listaCabeceraDetalle1;
                listaCabecerasDetalle[1] = listaCabeceraDetalle2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalDetalle = new List<string> { "left", "left", "left", "right", "right", "right", "right" };
                List<string> listaTipoDetalle = new List<string> { "string", "string", "string", "double", "double", "double", "double" };
                List<CpaExcelEstilo> listaEstiloDetalle = new List<CpaExcelEstilo> {
                    null,
                    null,
                    null,
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00")
                };

                i = 0;
                List<string>[] listaRegistrosDetalle = new List<string>[listCalculoCentral.Count];
                foreach (CpaCalculoCentralDTO calculoCentral in listCalculoCentral)
                {
                    listaRegistrosDetalle[i] = new List<string> {
                        calculoCentral.Emprnomb,
                        calculoCentral.Equinomb,
                        calculoCentral.BarrBarraTransferencia,
                        calculoCentral.Cpacctotenemwh == null ? "" : calculoCentral.Cpacctotenemwh.Value.ToString(),
                        calculoCentral.Cpacctotenesoles == null ? "" : calculoCentral.Cpacctotenesoles.Value.ToString(),
                        calculoCentral.Cpacctotpotmwh == null ? "" : calculoCentral.Cpacctotpotmwh.Value.ToString(),
                        calculoCentral.Cpacctotpotsoles == null ? "" : calculoCentral.Cpacctotpotsoles.Value.ToString()
                    };
                    i++;
                }

                CpaExcelCuerpo cuerpoDetalle = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosDetalle,
                    ListaAlineaHorizontal = listaAlineaHorizontalDetalle,
                    ListaTipo = listaTipoDetalle,
                    ListaEstilo = listaEstiloDetalle
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaDetalle = new CpaExcelHoja
                {
                    NombreHoja = "Detalle",
                    Titulo = sbTitulo.ToString(),
                    Subtitulo1 = sbSubTitulo1.ToString(),
                    ListaAnchoColumna = listaAnchoColumnaDetalle,
                    ListaCabeceras = listaCabecerasDetalle,
                    Cuerpo = cuerpoDetalle
                };
                listExcelHoja.Add(excelHojaDetalle);
                #endregion

                #endregion                
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
                throw;
            }
        }

        #endregion

        #region CU13 - Cargar archivos de integrantes D y T en la extranet
        /// <summary>
        /// Obtiene los años con respecto a los Revisiones con estado activo y cerrado
        /// </summary>
        /// <returns></returns>
        public List<GenericoDTO> AniosRevision(List<CpaRevisionDTO> ListRevision)
        {
            try
            {
                List<GenericoDTO> listaAnio = new List<GenericoDTO>();
                //ListRevision = ListarRevisiones(-1, -1, ConstantesCPPA.todos, ConstantesCPPA.estadoRevisionTodos);
                bool existListRevision = (ListRevision != null && ListRevision.Count > 0);
                if (existListRevision)
                {
                    foreach (var cpaapanio in ListRevision.Select(x => x.Cpaapanio).Distinct().OrderByDescending(x => x))
                    {
                        GenericoDTO reg = new GenericoDTO
                        {
                            Entero1 = cpaapanio,
                            String1 = cpaapanio.ToString()
                        };
                        listaAnio.Add(reg);
                    }
                }
                return listaAnio;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite registrar la informacion de archivos a cargar
        /// </summary>
        /// <param name="emprcodi"> identificador de la empresa</param>
        /// <param name="idRevision"> identificador de la revision</param>
        /// <param name="user"> usuario que realiza el registro</param>
        public int? SaveCargaDocumentos(int emprcodi, int idRevision, string user)
        {
            int? result = 0;

            CpaDocumentosDTO documentos = new CpaDocumentosDTO()
            {
                Emprcodi = emprcodi,
                Cparcodi = idRevision,
                Cpadocusucreacion = user,
                Cpadocfeccreacion = DateTime.Now
            };
            result = this.SaveCpaDocumentos(documentos);

            return result;
        }

        /// <summary>
        /// Permite registrar la informacion de archivos a cargar
        /// </summary>
        /// <param name="cpacodcodi"> identificador de la tabal CPA_DOCUMENTOS</param>
        /// <param name="ruta"> ruta donde se aloja el archivo</param>
        /// <param name="nombre"> nombre del archivo</param>
        /// <param name="tamano"> tamano del archivo</param>
        /// <param name="user"> usuario que realiza el registro</param>
        public void SaveCargaDocumentosDetalle(int? cpacodcodi, string ruta, string nombre, string tamano, string user)
        {
            CpaDocumentosDetalleDTO documentos = new CpaDocumentosDetalleDTO()
            {
                Cpadoccodi = cpacodcodi ?? 0,
                Cpaddtruta = ruta,
                Cpaddtnombre = nombre,
                Cpaddttamano = tamano,
                Cpaddtusucreacion = user,
                Cpaddtfeccreacion = DateTime.Now
            };

            this.SaveCpaDocumentosDetalle(documentos);
        }
        #endregion

        #region CU14 - Descarga archivos de integrantes D y T en la intranet
        /// <summary>
        /// Lista de centrales PMPO para una empresa
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresasDescargaIntegrantes()
        {
            return FactorySic.GetSiEmpresaRepository().ListaEmpresasDescargaIntegrantes();
        }
        #endregion

        #region CU17 - Calcular Porcentajes Presupuesto

        /// <summary>
        /// Obtener el log de un proceso de cálculo de porcentaje presupuestal, y los nombres de los reportes
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="estadoPublicacion"></param>
        /// <param name="listaReporteMensuales"></param>
        /// <param name="listaReporteAnuales"></param>
        /// <returns></returns>
        public string GetLogProcesoPorcentaje(int cparcodi, out string estadoPublicacion, out List<GenericoDTO> listaReporteMensuales, out List<GenericoDTO> listaReporteAnuales)
        {
            try
            {
                if (cparcodi < 0)
                {
                    throw new MyCustomException("No se puede leer el Log del Proceso porque el código de la revisión es inválido.");
                }

                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new MyCustomException("No se puede leer el Log de Proceso porque no existe la Revisión con id = " + cparcodi);
                }

                estadoPublicacion = "";
                listaReporteMensuales = new List<GenericoDTO>();
                listaReporteAnuales = new List<GenericoDTO>();

                CpaPorcentajeDTO porcentaje = FactoryTransferencia.GetCpaPorcentajeRepository().GetByCriteria(revision.Cparcodi);
                if (porcentaje == null)
                {
                    return "";
                }

                estadoPublicacion = porcentaje.Cpapestpub;
                ObtenerNombresDeReportes(revision, out listaReporteMensuales, out listaReporteAnuales);

                return porcentaje.Cpaplog;
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
        }

        /// <summary>
        /// Procesa el Cálculo de Porcentaje de Presupuesto para una Revisión
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="logProceso"></param>
        /// <param name="estadoPublicacion"></param>
        /// <param name="listaReporteMensuales"></param>
        /// <param name="listaReporteAnuales"></param>
        public void ProcesarCalculoPorcentaje(int cparcodi, string usuario, out string logProceso, out string estadoPublicacion, out List<GenericoDTO> listaReporteMensuales, out List<GenericoDTO> listaReporteAnuales)
        {
            #region Definiendo Variables Globales
            logProceso = null;
            estadoPublicacion = "";
            listaReporteMensuales = new List<GenericoDTO>();
            listaReporteAnuales = new List<GenericoDTO>();
            IDbConnection conn = null;
            DbTransaction tran = null;
            string cpaclog = "";
            StringBuilder sbLog = new StringBuilder();
            sbLog.Append("[Inf] ");
            sbLog.Append(DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2));
            sbLog.Append(" Inicio del Proceso [Usuario] ");
            sbLog.Append(usuario);
            sbLog.Append("\n");
            #endregion

            try
            {
                #region Validar Datos de Entrada
                if (cparcodi < 0)
                {
                    throw new MyCustomException("No se puede ejecutar el cálculo porque el código de la revisión es inválido.");
                }

                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new MyCustomException("No se puede ejecutar el cálculo porque no existe la Revisión con id = " + cparcodi);
                }
                string descRevision = string.Format("Revisión '{0}' del Ajuste '{1}' del Año Presupuestal '{2}'", revision.Cparrevision, revision.Cpaapajuste, revision.Cpaapanio);

                if (revision.Cparestado != ConstantesCPPA.estadoRevisionAbierto)
                {
                    throw new MyCustomException("No se puede ejecutar el cálculo de la revisión seleccionada porque su estado actual no es ‘Abierto’. En caso desee ejecutarlo, cambie dicho estado a ‘Abierto’, e inténtelo de nuevo.");
                }
                #endregion

                #region Obtener Datos para realizar el cálculo

                #region Obtener Empresas Integrantes únicas y con estado activo
                List<SiEmpresaDTO> listEmpresasUnicas = FactoryTransferencia.GetCpaEmpresaRepository().ListEmpresasUnicasByRevisionByEstado(revision.Cparcodi, "'A'");
                bool existListEmpresasUnicas = (listEmpresasUnicas != null && listEmpresasUnicas.Count > 0);
                if (!existListEmpresasUnicas)
                {
                    throw new MyCustomException("No es posible calcular los porcentajes pues no existen Empresas Integrantes con estado 'activo' para la " + descRevision);
                }
                #endregion

                #region Obtener Empresas Integrantes únicas y con estado inactivo
                StringBuilder sbLogEmpresasUnicasInactivas = new StringBuilder();
                List<SiEmpresaDTO> listEmpresasUnicasInactivas = FactoryTransferencia.GetCpaEmpresaRepository().ListEmpresasUnicasByRevisionByEstado(revision.Cparcodi, "'X'");
                bool existListEmpresasUnicasInactivas = (listEmpresasUnicasInactivas != null && listEmpresasUnicasInactivas.Count > 0);
                if (existListEmpresasUnicasInactivas)
                {
                    foreach (SiEmpresaDTO empresa in listEmpresasUnicasInactivas)
                    {
                        if (sbLogEmpresasUnicasInactivas.Length > 0)
                        {
                            sbLogEmpresasUnicasInactivas.Append(", ");
                        }
                        sbLogEmpresasUnicasInactivas.Append(empresa.Emprnomb);
                    }
                }
                #endregion

                #region Obtener: Cálculo Totales de Generación, Total de Demanda (UL, D) y Total de Transmisores
                List<CpaCalculoEmpresaDTO> listCalculoEmpresa = FactoryTransferencia.GetCpaCalculoEmpresaRepository().GetByCriteria(revision.Cparcodi, "-1");
                bool existListCalculoEmpresa = (listCalculoEmpresa != null && listCalculoEmpresa.Count > 0);

                List<CpaTotalDemandaDetDTO> listTotalDemandaDet = FactoryTransferencia.GetCpaTotalDemandaDetRepository().ListLastByRevision(revision.Cparcodi);
                bool existListTotalDemandaDet = (listTotalDemandaDet != null && listTotalDemandaDet.Count > 0);

                List<CpaTotalTransmisoresDetDTO> listTotalTransmisoresDet = FactoryTransferencia.GetCpaTotalTransmisoresDetRepository().ListLastByRevision(revision.Cparcodi);
                bool existListTotalTransmisoresDet = (listTotalTransmisoresDet != null && listTotalTransmisoresDet.Count > 0);

                if (!existListCalculoEmpresa && !existListTotalDemandaDet && !existListTotalTransmisoresDet)
                {
                    throw new MyCustomException("No es posible calcular los porcentajes pues no se han generado previamente ninguno de los totales de cada tipo de integrante.");
                }

                listCalculoEmpresa = listCalculoEmpresa.OrderBy(x => x.Emprcodi).ToList();
                List<CpaTotalDemandaDetDTO> listTotalDemandaDetByUsuarioLibre = listTotalDemandaDet.Where(x => x.Cpatdtipo == ConstantesCPPA.tipoEmpresaUsuarioLibre).ToList();
                List<CpaTotalDemandaDetDTO> listTotalDemandaDetByDistribuidor = listTotalDemandaDet.Where(x => x.Cpatdtipo == ConstantesCPPA.tipoEmpresaDistribuidora).ToList();
                #endregion

                #endregion

                #region Obtener Cálculo de Porcentaje Presupuestal

                #region Variables del Proceso 
                decimal sumaTotalEmpresa = 0M;
                List<CpaPorcentajeEnvioDTO> listPorcentajeEnvio = ObtenerListPorcentajeEnvio(listTotalDemandaDetByUsuarioLibre, listTotalDemandaDetByDistribuidor, listTotalTransmisoresDet);
                List<CpaPorcentajeEnergiaPotenciaDTO> listPorcentajeEnergiaPotencia = new List<CpaPorcentajeEnergiaPotenciaDTO>();
                List<CpaPorcentajeMontoDTO> listPorcentajeMonto = new List<CpaPorcentajeMontoDTO>();
                List<CpaPorcentajePorcentajeDTO> listPorcentajePorcentaje = new List<CpaPorcentajePorcentajeDTO>();
                #endregion

                #region Procesar Cálculo

                #region Obtener totales por empresa
                Dictionary<int, decimal> dTotalEmpresa = new Dictionary<int, decimal>();
                foreach (var emprcodi in listEmpresasUnicas.Select(x => x.Emprcodi))
                {
                    CpaPorcentajeEnergiaPotenciaDTO porcentajeEnergiaPotencia = new CpaPorcentajeEnergiaPotenciaDTO { Emprcodi = emprcodi };
                    CpaPorcentajeMontoDTO porcentajeMonto = new CpaPorcentajeMontoDTO { Emprcodi = emprcodi };
                    CpaPorcentajePorcentajeDTO porcentajePorcentaje = new CpaPorcentajePorcentajeDTO { Emprcodi = emprcodi };

                    decimal? totalGenerador = ObtenerTotalGenerador(emprcodi, listCalculoEmpresa, porcentajeEnergiaPotencia, porcentajeMonto, porcentajePorcentaje);
                    decimal? totalUsuariolibre = ObtenerTotalDemanda(emprcodi, ConstantesCPPA.tipoEmpresaUsuarioLibre, listTotalDemandaDetByUsuarioLibre, porcentajeEnergiaPotencia, porcentajeMonto, porcentajePorcentaje);
                    decimal? totalDistribuidor = ObtenerTotalDemanda(emprcodi, ConstantesCPPA.tipoEmpresaDistribuidora, listTotalDemandaDetByDistribuidor, porcentajeEnergiaPotencia, porcentajeMonto, porcentajePorcentaje);
                    decimal? totalTransmisor = ObtenerTotalTransmisor(emprcodi, listTotalTransmisoresDet, porcentajeMonto, porcentajePorcentaje);

                    decimal totalEmpresa = ObtenerTotalEmpresa(totalGenerador, totalUsuariolibre, totalDistribuidor, totalTransmisor);
                    sumaTotalEmpresa += totalEmpresa;

                    dTotalEmpresa.Add(emprcodi, totalEmpresa);
                    listPorcentajeEnergiaPotencia.Add(porcentajeEnergiaPotencia);
                    listPorcentajeMonto.Add(porcentajeMonto);
                    listPorcentajePorcentaje.Add(porcentajePorcentaje);
                }
                #endregion

                #region Obtener porcentaje
                int tipoemprecodi = listEmpresasUnicas[0].Tipoemprcodi;
                foreach (SiEmpresaDTO empresa in listEmpresasUnicas)
                {
                    decimal dporcentaje = ObtenerPorcentaje(dTotalEmpresa[empresa.Emprcodi], sumaTotalEmpresa);
                    listPorcentajePorcentaje.Where(x => x.Emprcodi == empresa.Emprcodi).ToList()[0].Cpappporcentaje = dporcentaje;

                    if (tipoemprecodi != empresa.Tipoemprcodi)
                    {
                        sbLog.Append(getLogTipoEmpresa(tipoemprecodi));
                        tipoemprecodi = empresa.Tipoemprcodi;
                    }
                }
                #endregion

                #region Generar resultado final del Log
                sbLog.Append(getLogTipoEmpresa(tipoemprecodi));
                sbLog.Append("[Inf] ");
                sbLog.Append(DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2));
                sbLog.Append(" Fin del proceso\n");

                if (sumaTotalEmpresa == 0M || sbLogEmpresasUnicasInactivas.Length > 0)
                {
                    sbLog.Append("[Inf] Más Información:\n");
                    if (sumaTotalEmpresa == 0M) { sbLog.Append("[Err] El valor del divisor de la fórmula, que es la sumatoria de los montos de las empresas integrantes participantes, es cero\n"); }
                    if (sbLogEmpresasUnicasInactivas.Length > 0) {
                        sbLog.Append("[Inf] Enpresas que no participaron en el cálculo por tener su estado 'inactivo' en parámetros de la revisión:\n");
                        sbLog.Append(sbLogEmpresasUnicasInactivas.ToString()); 
                    }
                }

                cpaclog = ObtenerLog(sbLog.ToString());
                #endregion

                #endregion

                #endregion

                #region Guardar Datos del Cálculo de Porcentaje Presupuestal
                conn = FactoryTransferencia.GetCpaPorcentajeRepository().BeginConnection();
                tran = FactoryTransferencia.GetCpaPorcentajeRepository().StartTransaction(conn);
                FactoryTransferencia.GetCpaPorcentajeEnergiaPotenciaRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaPorcentajeMontoRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaPorcentajePorcentajeRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaPorcentajeEnvioRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaPorcentajeRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                tran.Commit();

                tran = FactoryTransferencia.GetCpaPorcentajeRepository().StartTransaction(conn);
                int nextCpapcodi = FactoryTransferencia.GetCpaPorcentajeRepository().GetMaxId();
                int nextCpapecodi = FactoryTransferencia.GetCpaPorcentajeEnvioRepository().GetMaxId();
                int nextCpapepcodi = FactoryTransferencia.GetCpaPorcentajeEnergiaPotenciaRepository().GetMaxId();
                int nextCpapmtcodi = FactoryTransferencia.GetCpaPorcentajeMontoRepository().GetMaxId();
                int nextCpappcodi = FactoryTransferencia.GetCpaPorcentajePorcentajeRepository().GetMaxId();

                CpaPorcentajeDTO porcentaje = new CpaPorcentajeDTO
                {
                    Cpapcodi = nextCpapcodi,
                    Cparcodi = revision.Cparcodi,
                    Cpaplog = cpaclog,
                    Cpapestpub = ConstantesCPPA.estadoPublicacionNo,
                    Cpapusucreacion = usuario,
                    Cpapfeccreacion = DateTime.Now
                };
                FactoryTransferencia.GetCpaPorcentajeRepository().Save(porcentaje, conn, tran);

                foreach (CpaPorcentajeEnvioDTO porcentajeEnvio in listPorcentajeEnvio)
                {
                    porcentajeEnvio.Cpapecodi = nextCpapecodi;
                    porcentajeEnvio.Cpapcodi = nextCpapcodi;
                    porcentajeEnvio.Cparcodi = revision.Cparcodi;
                    porcentajeEnvio.Cpapeusucreacion = usuario;
                    porcentajeEnvio.Cpapefeccreacion = DateTime.Now;
                    FactoryTransferencia.GetCpaPorcentajeEnvioRepository().Save(porcentajeEnvio, conn, tran);
                    nextCpapecodi++;
                }

                foreach (CpaPorcentajeEnergiaPotenciaDTO porcentajeEnergiaPotencia in listPorcentajeEnergiaPotencia)
                {
                    porcentajeEnergiaPotencia.Cpapepcodi = nextCpapepcodi;
                    porcentajeEnergiaPotencia.Cpapcodi = nextCpapcodi;
                    porcentajeEnergiaPotencia.Cparcodi = revision.Cparcodi;
                    porcentajeEnergiaPotencia.Cpapepusucreacion = usuario;
                    porcentajeEnergiaPotencia.Cpapepfeccreacion = DateTime.Now;
                    FactoryTransferencia.GetCpaPorcentajeEnergiaPotenciaRepository().Save(porcentajeEnergiaPotencia, conn, tran);
                    nextCpapepcodi++;
                }

                foreach (CpaPorcentajeMontoDTO porcentajeMonto in listPorcentajeMonto)
                {
                    porcentajeMonto.Cpapmtcodi = nextCpapmtcodi;
                    porcentajeMonto.Cpapcodi = nextCpapcodi;
                    porcentajeMonto.Cparcodi = revision.Cparcodi;
                    porcentajeMonto.Cpapmtusucreacion = usuario;
                    porcentajeMonto.Cpapmtfeccreacion = DateTime.Now;
                    FactoryTransferencia.GetCpaPorcentajeMontoRepository().Save(porcentajeMonto, conn, tran);
                    nextCpapmtcodi++;
                }

                foreach (CpaPorcentajePorcentajeDTO porcentajePorcentaje in listPorcentajePorcentaje)
                {
                    porcentajePorcentaje.Cpappcodi = nextCpappcodi;
                    porcentajePorcentaje.Cpapcodi = nextCpapcodi;
                    porcentajePorcentaje.Cparcodi = revision.Cparcodi;
                    porcentajePorcentaje.Cpappusucreacion = usuario;
                    porcentajePorcentaje.Cpappfeccreacion = DateTime.Now;
                    FactoryTransferencia.GetCpaPorcentajePorcentajeRepository().Save(porcentajePorcentaje, conn, tran);
                    nextCpappcodi++;
                }

                tran.Commit();
                #endregion

                #region Llenar Log, Estado Publicación, y Listado de Reportes a ser retornados
                logProceso = cpaclog;
                estadoPublicacion = ConstantesCPPA.estadoPublicacionNo;
                ObtenerNombresDeReportes(revision, out listaReporteMensuales, out listaReporteAnuales);
                #endregion
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
            finally
            {
                bool conexionEstaAbierta = (conn != null && conn.State == ConnectionState.Open);
                if (conexionEstaAbierta)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Elimina el Cálculo de Porcentaje Presupuestal de una Revisión
        /// </summary>
        /// <param name="cparcodi"></param>
        public void EliminarCalculoPorcentaje(int cparcodi)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validaciones
                if (cparcodi < 0)
                {
                    throw new MyCustomException("No se puede eliminar el cálculo porque él código de la revisión es inválido.");
                }

                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new MyCustomException("No se puede eliminar el cálculo porque no existe la Revisión con id = " + cparcodi);
                }

                if (revision.Cparestado == ConstantesCPPA.estadoRevisionCerrado)
                {
                    throw new MyCustomException("No se puede eliminar el cálculo de la revisión seleccionada porque su estado actual es 'Cerrado'. En caso desee eliminarlo, cambie dicho estado a 'Abierto', e inténtelo de nuevo.");
                }
                #endregion

                #region Eliminar Cálculo
                conn = FactoryTransferencia.GetCpaPorcentajeRepository().BeginConnection();
                tran = FactoryTransferencia.GetCpaPorcentajeRepository().StartTransaction(conn);
                FactoryTransferencia.GetCpaPorcentajeEnergiaPotenciaRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaPorcentajeMontoRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaPorcentajePorcentajeRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaPorcentajeEnvioRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                FactoryTransferencia.GetCpaPorcentajeRepository().DeleteByRevision(revision.Cparcodi, conn, tran);
                tran.Commit();
                #endregion
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
            finally
            {
                bool conexionEstaAbierta = (conn != null && conn.State == ConnectionState.Open);
                if (conexionEstaAbierta)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Obtener nombres de los reportes mensuales y anuales
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="listaReporteMensuales"></param>
        /// <param name="listaReporteAnuales"></param>
        private void ObtenerNombresDeReportes(CpaRevisionDTO revision, out List<GenericoDTO> listaReporteMensuales, out List<GenericoDTO> listaReporteAnuales)
        {
            listaReporteMensuales = new List<GenericoDTO>();
            listaReporteAnuales = new List<GenericoDTO>();
            string anio = revision.Cpaapanioejercicio.ToString().Substring(2);
            string nombreRevision = (revision.Cparrevision != ConstantesCPPA.revisionNormal) ? ("_Rev" + revision.Cparrevision.Substring(ConstantesCPPA.numero9)) : "";
            foreach (int i in ConstantesCPPA.numMeses)
            {
                GenericoDTO item = new GenericoDTO
                {
                    Entero1 = revision.Cparcodi,
                    Entero2 = i,
                    String1 = string.Format("{0}-{1}", ConstantesCPPA.mesesDesc[i - 1], revision.Cpaapanioejercicio),
                    String2 = string.Format("Montos_G{0}-{1}-{2}{3}.xlsx", i.ToString("D2"), anio, revision.Cpaapajuste, nombreRevision),
                    String3 = string.Format("Montos_D{0}-{1}-{2}{3}.xlsx", i.ToString("D2"), anio, revision.Cpaapajuste, nombreRevision)
                };
                listaReporteMensuales.Add(item);
            }

            GenericoDTO item2 = new GenericoDTO
            {
                Entero1 = revision.Cparcodi,
                String1 = string.Format("{0} {1}", "Transmisión", revision.Cpaapanioejercicio),
                String2 = string.Format("Montos_T-{0}-{1}{2}.xlsx", anio, revision.Cpaapajuste, nombreRevision),
            };
            listaReporteAnuales.Add(item2);

            item2 = new GenericoDTO
            {
                Entero1 = revision.Cparcodi,
                String1 = string.Format("{0} {1}", "Porcentaje Presupuestal", revision.Cpaapanioejercicio),
                String2 = string.Format("Ajuste_Aportes-{0}-{1}{2}.xlsx", anio, revision.Cpaapajuste, nombreRevision),
            };
            listaReporteAnuales.Add(item2);
        }

        /// <summary>
        /// Obtiene los valoes para ListPorcentajeEnvio
        /// </summary>
        /// <param name="listTotalDemandaDetByUsuarioLibre"></param>
        /// <param name="listTotalDemandaDetByDistribuidor"></param>
        /// <param name="listTotalTransmisoresDet"></param>
        /// <returns></returns>
        private List<CpaPorcentajeEnvioDTO> ObtenerListPorcentajeEnvio(List<CpaTotalDemandaDetDTO> listTotalDemandaDetByUsuarioLibre, 
            List<CpaTotalDemandaDetDTO> listTotalDemandaDetByDistribuidor, List<CpaTotalTransmisoresDetDTO> listTotalTransmisoresDet)
        {
            List<CpaPorcentajeEnvioDTO> listPorcentajeEnvio = new List<CpaPorcentajeEnvioDTO>();
            bool existListTotalDemandaDetByUsuarioLibre = (listTotalDemandaDetByUsuarioLibre != null && listTotalDemandaDetByUsuarioLibre.Count > 0);
            bool existListTotalDemandaDetByDistribuidor = (listTotalDemandaDetByDistribuidor != null && listTotalDemandaDetByDistribuidor.Count > 0);
            bool existListTotalTransmisoresDet = (listTotalTransmisoresDet != null && listTotalTransmisoresDet.Count > 0);

            if (existListTotalDemandaDetByUsuarioLibre)
            {
                List<CpaTotalDemandaDetDTO> listTDD = listTotalDemandaDetByUsuarioLibre
                    .GroupBy(x => new { x.Cpatdcodi, x.Cparcodi, x.Cpatdtipo, x.Cpatdmes })
                    .Select(x => new CpaTotalDemandaDetDTO { 
                        Cpatdcodi = x.Key.Cpatdcodi, 
                        Cparcodi = x.Key.Cparcodi, 
                        Cpatdtipo = x.Key.Cpatdtipo, 
                        Cpatdmes = x.Key.Cpatdmes }
                    ).ToList();

                foreach(CpaTotalDemandaDetDTO tdd in listTDD)
                {
                    CpaPorcentajeEnvioDTO porcentajeEnvio = new CpaPorcentajeEnvioDTO
                    {
                        Cparcodi = tdd.Cparcodi,
                        Cpapetipo = tdd.Cpatdtipo,
                        Cpapemes = tdd.Cpatdmes,
                        Cpapenumenvio = tdd.Cpatdcodi
                    };

                    listPorcentajeEnvio.Add(porcentajeEnvio);
                }
            }

            if (existListTotalDemandaDetByDistribuidor)
            {
                List<CpaTotalDemandaDetDTO> listTDD = listTotalDemandaDetByDistribuidor
                    .GroupBy(x => new { x.Cpatdcodi, x.Cparcodi, x.Cpatdtipo, x.Cpatdmes })
                    .Select(x => new CpaTotalDemandaDetDTO { 
                        Cpatdcodi = x.Key.Cpatdcodi, 
                        Cparcodi = x.Key.Cparcodi, 
                        Cpatdtipo = x.Key.Cpatdtipo, 
                        Cpatdmes = x.Key.Cpatdmes }
                    ).ToList();

                foreach (CpaTotalDemandaDetDTO tdd in listTDD)
                {
                    CpaPorcentajeEnvioDTO porcentajeEnvio = new CpaPorcentajeEnvioDTO
                    {
                        Cparcodi = tdd.Cparcodi,
                        Cpapetipo = tdd.Cpatdtipo,
                        Cpapemes = tdd.Cpatdmes,
                        Cpapenumenvio = tdd.Cpatdcodi
                    };

                    listPorcentajeEnvio.Add(porcentajeEnvio);
                }
            }

            if (existListTotalTransmisoresDet)
            {
                List<CpaTotalTransmisoresDetDTO> listTTD = listTotalTransmisoresDet
                    .GroupBy(x => new { x.Cpattcodi, x.Cparcodi })
                    .Select(x => new CpaTotalTransmisoresDetDTO { 
                        Cpattcodi = x.Key.Cpattcodi, 
                        Cparcodi = x.Key.Cparcodi }
                    ).ToList();

                foreach (CpaTotalTransmisoresDetDTO ttd in listTTD)
                {
                    CpaPorcentajeEnvioDTO porcentajeEnvio = new CpaPorcentajeEnvioDTO
                    {
                        Cparcodi = ttd.Cparcodi,
                        Cpapetipo = ConstantesCPPA.tipoEmpresaTransmisora,
                        Cpapenumenvio = ttd.Cpattcodi
                    };

                    listPorcentajeEnvio.Add(porcentajeEnvio);
                }
            }

            return listPorcentajeEnvio;
        }

        /// <summary>
        /// Obtener el total de Generador para una empresa específica
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="listCpaCalculoEmpresa"></param>
        /// <param name="porcentajeEnergiaPotencia"></param>
        /// <param name="porcentajeMonto"></param>
        /// <param name="porcentajePorcentaje"></param>
        /// <returns></returns>
        private decimal? ObtenerTotalGenerador(int emprcodi, List<CpaCalculoEmpresaDTO> listCpaCalculoEmpresa, 
            CpaPorcentajeEnergiaPotenciaDTO porcentajeEnergiaPotencia, CpaPorcentajeMontoDTO porcentajeMonto, CpaPorcentajePorcentajeDTO porcentajePorcentaje)
        {
            string mes;
            decimal? totalGeneradorSoles = null;
            decimal? totalCpacetotenemwh = null;
            decimal? totalCpacetotpotmwh = null;
            decimal? totalCpacetotenesoles = null;
            decimal? totalCpacetotpotsoles = null;
            List<CpaCalculoEmpresaDTO> list = listCpaCalculoEmpresa.Where(x => x.Emprcodi == emprcodi).ToList();

            if (list.Count > 0)
            {
                foreach(CpaCalculoEmpresaDTO calculoEmpresa in list)
                {
                    UtilCPPA.SumarDecimalesNulos(ref totalCpacetotenemwh, calculoEmpresa.Cpacetotenemwh);
                    UtilCPPA.SumarDecimalesNulos(ref totalCpacetotpotmwh, calculoEmpresa.Cpacetotpotmwh);
                    UtilCPPA.SumarDecimalesNulos(ref totalCpacetotenesoles, calculoEmpresa.Cpacetotenesoles);
                    UtilCPPA.SumarDecimalesNulos(ref totalGeneradorSoles, calculoEmpresa.Cpacetotenesoles);
                    UtilCPPA.SumarDecimalesNulos(ref totalCpacetotpotsoles, calculoEmpresa.Cpacetotpotsoles);
                    UtilCPPA.SumarDecimalesNulos(ref totalGeneradorSoles, calculoEmpresa.Cpacetotpotsoles);

                    mes = calculoEmpresa.Cpacemes.ToString("D2");
                    UtilCPPA.SumarDecimalesNulos(porcentajeEnergiaPotencia, $"Cpapepenemes{mes}", calculoEmpresa.Cpacetotenemwh);
                    UtilCPPA.SumarDecimalesNulos(porcentajeEnergiaPotencia, $"Cpapeppotmes{mes}", calculoEmpresa.Cpacetotpotmwh);
                    UtilCPPA.SumarDecimalesNulos(porcentajeMonto, $"Cpapmtenemes{mes}", calculoEmpresa.Cpacetotenesoles);
                    UtilCPPA.SumarDecimalesNulos(porcentajeMonto, $"Cpapmtpotmes{mes}", calculoEmpresa.Cpacetotpotsoles);
                }

                UtilCPPA.SumarDecimalesNulos(porcentajeEnergiaPotencia, "Cpapepenetotal", totalCpacetotenemwh);
                UtilCPPA.SumarDecimalesNulos(porcentajeEnergiaPotencia, "Cpapeppottotal", totalCpacetotpotmwh);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmtenetotal", totalCpacetotenesoles);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmtpottotal", totalCpacetotpotsoles);
                UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpappgentotene", totalCpacetotenesoles);
                UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpappgentotpot", totalCpacetotpotsoles);
                UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpapptotal", totalGeneradorSoles);
            }

            return totalGeneradorSoles;
        }

        /// <summary>
        /// Obtener el total de Demanda para una empresa específica. Se debe especificar el tipoEmpresaDemanda
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="tipoEmpresaDemanda: [U, D]"></param>
        /// <param name="listTotalDemandaDet"></param>
        /// <param name="porcentajeEnergiaPotencia"></param>
        /// <param name="porcentajeMonto"></param>
        /// <param name="porcentajePorcentaje"></param>
        /// <returns></returns>
        private decimal? ObtenerTotalDemanda(int emprcodi, string tipoEmpresaDemanda, List<CpaTotalDemandaDetDTO> listTotalDemandaDet, 
            CpaPorcentajeEnergiaPotenciaDTO porcentajeEnergiaPotencia, CpaPorcentajeMontoDTO porcentajeMonto, CpaPorcentajePorcentajeDTO porcentajePorcentaje)
        {
            string mes;
            decimal? totalDemandaSoles = null;
            decimal? totalCpatddtotenemwh = null;
            decimal? totalCpatddtotpotmw = null;
            decimal? totalCpatddtotenesoles = null;
            decimal? totalCpatddtotpotsoles = null;
            List<CpaTotalDemandaDetDTO> list = listTotalDemandaDet.Where(x => x.Emprcodi == emprcodi).ToList();

            if (list.Count > 0)
            {
                foreach (CpaTotalDemandaDetDTO totalDemandaDet in list)
                {
                    UtilCPPA.SumarDecimalesNulos(ref totalCpatddtotenemwh, totalDemandaDet.Cpatddtotenemwh);
                    UtilCPPA.SumarDecimalesNulos(ref totalCpatddtotpotmw, totalDemandaDet.Cpatddtotpotmw);
                    UtilCPPA.SumarDecimalesNulos(ref totalCpatddtotenesoles, totalDemandaDet.Cpatddtotenesoles);
                    UtilCPPA.SumarDecimalesNulos(ref totalDemandaSoles, totalDemandaDet.Cpatddtotenesoles);
                    UtilCPPA.SumarDecimalesNulos(ref totalCpatddtotpotsoles, totalDemandaDet.Cpatddtotpotsoles);
                    UtilCPPA.SumarDecimalesNulos(ref totalDemandaSoles, totalDemandaDet.Cpatddtotpotsoles);

                    mes = totalDemandaDet.Cpatdmes.ToString("D2");
                    UtilCPPA.SumarDecimalesNulos(porcentajeEnergiaPotencia, $"Cpapepenemes{mes}", totalDemandaDet.Cpatddtotenemwh);
                    UtilCPPA.SumarDecimalesNulos(porcentajeEnergiaPotencia, $"Cpapeppotmes{mes}", totalDemandaDet.Cpatddtotpotmw);
                    UtilCPPA.SumarDecimalesNulos(porcentajeMonto, $"Cpapmtenemes{mes}", totalDemandaDet.Cpatddtotenesoles);
                    UtilCPPA.SumarDecimalesNulos(porcentajeMonto, $"Cpapmtpotmes{mes}", totalDemandaDet.Cpatddtotpotsoles);
                }

                UtilCPPA.SumarDecimalesNulos(porcentajeEnergiaPotencia, "Cpapepenetotal", totalCpatddtotenemwh);
                UtilCPPA.SumarDecimalesNulos(porcentajeEnergiaPotencia, "Cpapeppottotal", totalCpatddtotpotmw);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmtenetotal", totalCpatddtotenesoles);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmtpottotal", totalCpatddtotpotsoles);
                if (tipoEmpresaDemanda == ConstantesCPPA.tipoEmpresaUsuarioLibre)
                {
                    UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpappultotene", totalCpatddtotenesoles);
                    UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpappultotpot", totalCpatddtotpotsoles);
                }
                else if (tipoEmpresaDemanda == ConstantesCPPA.tipoEmpresaDistribuidora)
                {
                    UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpappdistotene", totalCpatddtotenesoles);
                    UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpappdistotpot", totalCpatddtotpotsoles);
                }
                UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpapptotal", totalDemandaSoles);
            }

            return totalDemandaSoles;
        }

        /// <summary>
        /// Obtener el total de Transmisor para una empresa específica
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="listTotalTransmisoresDet"></param>
        /// <param name="porcentajeMonto"></param>
        /// <param name="porcentajePorcentaje"></param>
        /// <returns></returns>
        private decimal? ObtenerTotalTransmisor(int emprcodi, List<CpaTotalTransmisoresDetDTO> listTotalTransmisoresDet, CpaPorcentajeMontoDTO porcentajeMonto, CpaPorcentajePorcentajeDTO porcentajePorcentaje)
        {
            decimal? totalTransmisorSoles = null;

            List<CpaTotalTransmisoresDetDTO> list = listTotalTransmisoresDet.Where(x => x.Emprcodi == emprcodi).ToList();
            if (list.Count > 0)
            {
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames01", list[0].Cpattdtotmes01);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames02", list[0].Cpattdtotmes02);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames03", list[0].Cpattdtotmes03);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames04", list[0].Cpattdtotmes04);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames05", list[0].Cpattdtotmes05);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames06", list[0].Cpattdtotmes06);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames07", list[0].Cpattdtotmes07);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames08", list[0].Cpattdtotmes08);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames09", list[0].Cpattdtotmes09);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames10", list[0].Cpattdtotmes10);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames11", list[0].Cpattdtotmes11);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttrames12", list[0].Cpattdtotmes12);
                UtilCPPA.SumarDecimalesNulos(porcentajeMonto, "Cpapmttratotal", list[0].Cpattdtotal);

                UtilCPPA.SumarDecimalesNulos(ref totalTransmisorSoles, list[0].Cpattdtotal);
                UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpapptratot", list[0].Cpattdtotal);
                UtilCPPA.SumarDecimalesNulos(porcentajePorcentaje, "Cpapptotal", list[0].Cpattdtotal);
            }

            return totalTransmisorSoles;
        }

        /// <summary>
        /// Obtener el total para una empresa específica
        /// </summary>
        /// <param name="totalGenerador"></param>
        /// <param name="totalUsuariolibre"></param>
        /// <param name="totalDistribuidor"></param>
        /// <param name="totalTransmisor"></param>
        /// <returns></returns>
        private decimal ObtenerTotalEmpresa(decimal? totalGenerador, decimal? totalUsuariolibre, decimal? totalDistribuidor, decimal? totalTransmisor)
        {
            decimal totalEmpresa = 0M;

            if (totalGenerador != null)     { totalEmpresa += totalGenerador.Value; }
            if (totalUsuariolibre != null)  { totalEmpresa += totalUsuariolibre.Value; }
            if (totalDistribuidor != null)  { totalEmpresa += totalDistribuidor.Value; }
            if (totalTransmisor != null)    { totalEmpresa += totalTransmisor.Value; }

            return totalEmpresa;
        }

        /// <summary>
        /// Obtener porcentaje para una empresa
        /// </summary>
        /// <param name="totalEmpresa"></param>
        /// <param name="sumaTotalEmpresa"></param>
        /// <returns></returns>
        private decimal ObtenerPorcentaje(decimal totalEmpresa, decimal sumaTotalEmpresa)
        {
            decimal porcentaje = 0M;
            if (sumaTotalEmpresa != 0M)
            {
                porcentaje = (totalEmpresa / sumaTotalEmpresa) * 100;
            }
            return porcentaje;
        }

        /// <summary>
        /// Retorna log para el cúlmino de un tipo de empresa
        /// </summary>
        /// <param name="tipoemprecodi"></param>
        /// <returns></returns>
        private string getLogTipoEmpresa(int tipoemprecodi)
        {
            StringBuilder sbLogTipoEmpresa = new StringBuilder();
            sbLogTipoEmpresa.Append("[Inf] ");
            sbLogTipoEmpresa.Append(DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2));
            sbLogTipoEmpresa.Append(" Culminó el proceso para ");
            sbLogTipoEmpresa.Append(ConstantesCPPA.tipoEmpresa[tipoemprecodi - 1]);
            sbLogTipoEmpresa.Append("\n");

            return sbLogTipoEmpresa.ToString();
        }
        #endregion

        #region CU18 - Descargar y Publicar Reportes Ajustes

        /// <summary>
        /// Actualizar estado de publicación de reportes relacionados a una revisión
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cpapestpub"></param>
        /// <param name="cpapusumodificacion"></param>
        public void ActualizarEstadoPublicacionDeReportes(int cparcodi, string cpapestpub, string cpapusumodificacion)
        {
            try
            {
                #region Validación
                if (cparcodi < 0)
                {
                    throw new MyCustomException("El código de la Revisión es inválido.");
                }

                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new MyCustomException("No se puede actualizar el estado de publicación porque no existe la Revisión con id = " + cparcodi);
                }

                if (cpapestpub != ConstantesCPPA.estadoPublicacionSi &&
                    cpapestpub != ConstantesCPPA.estadoPublicacionNo)
                {
                    throw new MyCustomException("No se puede actualizar el estado de publicación porque el nuevo valor a actualizar es inválido. Nuevo valor = " + cpapestpub);
                }
                #endregion

                #region Actualizar estado
                FactoryTransferencia.GetCpaPorcentajeRepository().UpdateEstadoPublicacion(revision.Cparcodi, cpapestpub, cpapusumodificacion, DateTime.Now);
                #endregion
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }
        }

        /// <summary>
        /// Genera los datos del archivo Excel para el Reporte de Demanda para una Revisión y un mes específico
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cpatdmes"></param>
        /// <param name="invocadoPor"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="listExcelHoja"></param>
        public void GenerarArchivoExcelReporteDemanda(int cparcodi, int cpatdmes, string invocadoPor, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja)
        {
            try
            {
                #region Validación
                if (cparcodi < 0)
                {
                    throw new MyCustomException("No se puede generar el reporte porque el código de la revisión es inválido.");
                }

                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existe la Revisión con id = " + cparcodi);
                }

                if (!ConstantesCPPA.numMeses.Contains(cpatdmes))
                {
                    throw new MyCustomException("No se puede generar el reporte porque el mes es inválido. Mes = " + cpatdmes);
                }

                ValidarReporteParaExtranet(revision, invocadoPor);
                #endregion

                #region Obtener datos
                List<CpaPorcentajeEnvioDTO> listPorcentajeEnvio = FactoryTransferencia.GetCpaPorcentajeEnvioRepository().ListByRevision(revision.Cparcodi);
                bool existListPorcentajeEnvio = (listPorcentajeEnvio != null && listPorcentajeEnvio.Count > 0);
                if (!existListPorcentajeEnvio)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existen registros a exportar.");
                }

                List<CpaPorcentajeEnvioDTO> listPorcentajeEnvioByUsuarioLibre = listPorcentajeEnvio.Where(x => x.Cpapetipo == ConstantesCPPA.tipoEmpresaUsuarioLibre && x.Cpapemes == cpatdmes).ToList();
                List<CpaPorcentajeEnvioDTO> listPorcentajeEnvioByDistribuidor = listPorcentajeEnvio.Where(x => x.Cpapetipo == ConstantesCPPA.tipoEmpresaDistribuidora && x.Cpapemes == cpatdmes).ToList();
                if (listPorcentajeEnvioByUsuarioLibre.Count < 1 && listPorcentajeEnvioByDistribuidor.Count < 1)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existen registros a exportar.");
                }

                List<CpaTotalDemandaDetDTO> listTotalDemandaDetByUsuarioLibre = new List<CpaTotalDemandaDetDTO>();
                bool existListTotalDemandaDetByUsuarioLibre = false;
                if (listPorcentajeEnvioByUsuarioLibre.Count > 0)
                {
                    int cpatdcodi = listPorcentajeEnvioByUsuarioLibre[0].Cpapenumenvio;
                    listTotalDemandaDetByUsuarioLibre = FactoryTransferencia.GetCpaTotalDemandaDetRepository().ListByCpatdcodi(cpatdcodi);
                    existListTotalDemandaDetByUsuarioLibre = (listTotalDemandaDetByUsuarioLibre != null && listTotalDemandaDetByUsuarioLibre.Count > 0);
                    if (existListTotalDemandaDetByUsuarioLibre)
                    {
                        listTotalDemandaDetByUsuarioLibre = listTotalDemandaDetByUsuarioLibre.OrderBy(x => x.Emprnomb).ToList();
                    }
                }

                List<CpaTotalDemandaDetDTO> listTotalDemandaDetByDistribuidor = new List<CpaTotalDemandaDetDTO>();
                bool existListTotalDemandaDetByDistribuidor = false;
                if (listPorcentajeEnvioByDistribuidor.Count > 0)
                {
                    int cpatdcodi = listPorcentajeEnvioByDistribuidor[0].Cpapenumenvio;
                    listTotalDemandaDetByDistribuidor = FactoryTransferencia.GetCpaTotalDemandaDetRepository().ListByCpatdcodi(cpatdcodi);
                    existListTotalDemandaDetByDistribuidor = (listTotalDemandaDetByDistribuidor != null && listTotalDemandaDetByDistribuidor.Count > 0);
                    if (existListTotalDemandaDetByDistribuidor)
                    {
                        listTotalDemandaDetByDistribuidor = listTotalDemandaDetByDistribuidor.OrderBy(x => x.Emprnomb).ToList();
                    }
                }

                if (!existListTotalDemandaDetByUsuarioLibre && !existListTotalDemandaDetByDistribuidor)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existen registros a exportar.");
                }
                #endregion

                #region Variables
                string nombreRevision = (revision.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("_Rev" + revision.Cparrevision.Substring(ConstantesCPPA.numero9));
                nombreArchivo = string.Format("Montos_D{0}-{1}-{2}{3}", cpatdmes.ToString("D2"), revision.Cpaapanioejercicio.ToString().Substring(2), revision.Cpaapajuste, nombreRevision);
                nombreRevision = (revision.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("Rev. " + revision.Cparrevision.Substring(ConstantesCPPA.numero9));
                listExcelHoja = new List<CpaExcelHoja>();
                #endregion

                #region Hoja Usuario Libre

                #region Titulos
                StringBuilder sbTituloUL = new StringBuilder();
                sbTituloUL.Append("Usuarios Libres - Presupuesto Anual ");
                sbTituloUL.Append(revision.Cpaapanio);
                StringBuilder sbSubTituloUL1 = new StringBuilder();
                sbSubTituloUL1.Append("Ajuste ");
                sbSubTituloUL1.Append(revision.Cpaapajuste.Substring(1));
                if (nombreRevision.Length > 0)
                {
                    sbSubTituloUL1.Append(" - ");
                    sbSubTituloUL1.Append(nombreRevision);
                }
                sbSubTituloUL1.Append(" - ");
                sbSubTituloUL1.Append(cpatdmes.ToString("D2"));
                sbSubTituloUL1.Append(" ");
                sbSubTituloUL1.Append(ConstantesCPPA.mesesDesc[cpatdmes - 1]);
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasUL = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabeceraUL1 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Empresa", "center", 1, 2),
                    CrearExcelModelo("Energía", "center", 2),
                    CrearExcelModelo("Potencia", "center", 2)
                };
                List<CpaExcelModelo> listaCabeceraUL2 = new List<CpaExcelModelo> {
                    CrearExcelModelo("MWh", "center"),
                    CrearExcelModelo("S/.", "center"),
                    CrearExcelModelo("MW", "center"),
                    CrearExcelModelo("S/.", "center")
                };
                List<int> listaAnchoColumnaUL = new List<int> { 40, 15, 15, 15, 15 };

                listaCabecerasUL[0] = listaCabeceraUL1;
                listaCabecerasUL[1] = listaCabeceraUL2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalUL = new List<string> { "left", "right", "right", "right", "right" };
                List<string> listaTipoUL = new List<string> { "string", "double", "double", "double", "double" };
                List<CpaExcelEstilo> listaEstiloUL = new List<CpaExcelEstilo> {
                    CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00")
                };

                List<string>[] listaRegistrosUL = ObtenerRegistrosDemanda(listTotalDemandaDetByUsuarioLibre);

                CpaExcelCuerpo cuerpoUL = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosUL,
                    ListaAlineaHorizontal = listaAlineaHorizontalUL,
                    ListaTipo = listaTipoUL,
                    ListaEstilo = listaEstiloUL
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaUL = new CpaExcelHoja
                {
                    NombreHoja = "Usuarios Libres",
                    Titulo = sbTituloUL.ToString(),
                    Subtitulo1 = sbSubTituloUL1.ToString(),
                    ListaAnchoColumna = listaAnchoColumnaUL,
                    ListaCabeceras = listaCabecerasUL,
                    Cuerpo = cuerpoUL
                };
                listExcelHoja.Add(excelHojaUL);
                #endregion

                #endregion

                #region Hoja Distribuidores

                #region Titulos
                StringBuilder sbTituloD = new StringBuilder();
                sbTituloD.Append("Distribuidores - Presupuesto Anual ");
                sbTituloD.Append(revision.Cpaapanio);
                StringBuilder sbSubTituloD1 = new StringBuilder();
                sbSubTituloD1.Append("Ajuste ");
                sbSubTituloD1.Append(revision.Cpaapajuste.Substring(1));
                if (nombreRevision.Length > 0)
                {
                    sbSubTituloD1.Append(" - ");
                    sbSubTituloD1.Append(nombreRevision);
                }
                sbSubTituloD1.Append(" - ");
                sbSubTituloD1.Append(cpatdmes.ToString("D2"));
                sbSubTituloD1.Append(" ");
                sbSubTituloD1.Append(ConstantesCPPA.mesesDesc[cpatdmes - 1]);
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasD = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabeceraD1 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Empresa", "center", 1, 2),
                    CrearExcelModelo("Energía", "center", 2),
                    CrearExcelModelo("Potencia", "center", 2)
                };
                List<CpaExcelModelo> listaCabeceraD2 = new List<CpaExcelModelo> {
                    CrearExcelModelo("MWh", "center"),
                    CrearExcelModelo("S/.", "center"),
                    CrearExcelModelo("MW", "center"),
                    CrearExcelModelo("S/.", "center")
                };
                List<int> listaAnchoColumnaD = new List<int> { 40, 15, 15, 15, 15 };

                listaCabecerasD[0] = listaCabeceraD1;
                listaCabecerasD[1] = listaCabeceraD2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalD = new List<string> { "left", "right", "right", "right", "right" };
                List<string> listaTipoD = new List<string> { "string", "double", "double", "double", "double" };
                List<CpaExcelEstilo> listaEstiloD = new List<CpaExcelEstilo> {
                    CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00")
                };

                List<string>[] listaRegistrosD = ObtenerRegistrosDemanda(listTotalDemandaDetByDistribuidor);

                CpaExcelCuerpo cuerpoD = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosD,
                    ListaAlineaHorizontal = listaAlineaHorizontalD,
                    ListaTipo = listaTipoD,
                    ListaEstilo = listaEstiloD
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaD = new CpaExcelHoja
                {
                    NombreHoja = "Distribuidores",
                    Titulo = sbTituloD.ToString(),
                    Subtitulo1 = sbSubTituloD1.ToString(),
                    ListaAnchoColumna = listaAnchoColumnaD,
                    ListaCabeceras = listaCabecerasD,
                    Cuerpo = cuerpoD
                };
                listExcelHoja.Add(excelHojaD);
                #endregion

                #endregion                
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos del archivo Excel para el Reporte de Transmisión
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="invocadoPor"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="listExcelHoja"></param>
        public void GenerarArchivoExcelReporteTransmision(int cparcodi, string invocadoPor, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja)
        {
            try
            {
                #region Validación
                if (cparcodi < 0)
                {
                    throw new MyCustomException("No se puede generar el reporte porque el código de la revisión es inválido.");
                }

                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existe la Revisión con id = " + cparcodi);
                }

                ValidarReporteParaExtranet(revision, invocadoPor);
                #endregion

                #region Obtener datos
                List<CpaPorcentajeEnvioDTO> listPorcentajeEnvio = FactoryTransferencia.GetCpaPorcentajeEnvioRepository().ListByRevision(revision.Cparcodi);
                bool existListPorcentajeEnvio = (listPorcentajeEnvio != null && listPorcentajeEnvio.Count > 0);
                if (!existListPorcentajeEnvio)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existen registros a exportar.");
                }

                listPorcentajeEnvio = listPorcentajeEnvio.Where(x => x.Cpapetipo == ConstantesCPPA.tipoEmpresaTransmisora && x.Cpapemes == null).ToList();
                if (listPorcentajeEnvio.Count < 1)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existen registros a exportar.");
                }
                int cpattcodi = listPorcentajeEnvio[0].Cpapenumenvio;
                
                List<CpaTotalTransmisoresDetDTO> listTotalTransmisoresDet = FactoryTransferencia.GetCpaTotalTransmisoresDetRepository().ListByCpattcodi(cpattcodi);
                bool existListTotalTransmisoresDet = (listTotalTransmisoresDet != null && listTotalTransmisoresDet.Count > 0);
                if (!existListTotalTransmisoresDet)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existen registros a exportar.");
                }
                listTotalTransmisoresDet = listTotalTransmisoresDet.OrderBy(x => x.Emprnomb).ToList();
                #endregion

                #region Variables
                string anioEjercicioCorto = revision.Cpaapanioejercicio.ToString().Substring(2);
                string nombreRevision = (revision.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("_Rev" + revision.Cparrevision.Substring(ConstantesCPPA.numero9));
                nombreArchivo = string.Format("Montos_T-{0}-{1}{2}", revision.Cpaapanioejercicio.ToString().Substring(2), revision.Cpaapajuste, nombreRevision);
                listExcelHoja = new List<CpaExcelHoja>();
                int i = 0;
                #endregion

                #region Hoja Transmisión

                #region Titulos
                nombreRevision = (revision.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("Rev. " + revision.Cparrevision.Substring(ConstantesCPPA.numero9));
                StringBuilder sbTitulo = new StringBuilder();
                sbTitulo.Append("Total de Transmisión - Presupuesto Anual ");
                sbTitulo.Append(revision.Cpaapanio);
                StringBuilder sbSubTitulo1 = new StringBuilder();
                sbSubTitulo1.Append("Ajuste ");
                sbSubTitulo1.Append(revision.Cpaapajuste.Substring(1));
                if (nombreRevision.Length > 0)
                {
                    sbSubTitulo1.Append(" - ");
                    sbSubTitulo1.Append(nombreRevision);
                }
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabeceras = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabecera1 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Empresa", "center", 1, 2),
                    CrearExcelModelo("Montos - Transmisión (S/.)", "center", 13)
                };
                List<CpaExcelModelo> listaCabecera2 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Ene-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Feb-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Mar-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Abr-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("May-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jun-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jul-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Ago-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Sep-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Oct-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Nov-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Dic-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Total", "center")
                };
                List<int> listaAnchoColumna = new List<int> { 40,
                    15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15 };

                listaCabeceras[0] = listaCabecera1;
                listaCabeceras[1] = listaCabecera2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontal = new List<string> { "left",
                    "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right" };
                List<string> listaTipo = new List<string> { "string",
                    "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double" };
                List<CpaExcelEstilo> listaEstilo = new List<CpaExcelEstilo> {
                    CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00")
                };

                i = 0;
                List<string>[] listaRegistros = new List<string>[listTotalTransmisoresDet.Count];
                foreach (CpaTotalTransmisoresDetDTO totalTransmisionDet in listTotalTransmisoresDet)
                {
                    listaRegistros[i] = new List<string> {
                        totalTransmisionDet.Emprnomb,
                        totalTransmisionDet.Cpattdtotmes01 == null ? "" : totalTransmisionDet.Cpattdtotmes01.ToString(),
                        totalTransmisionDet.Cpattdtotmes02 == null ? "" : totalTransmisionDet.Cpattdtotmes02.ToString(),
                        totalTransmisionDet.Cpattdtotmes03 == null ? "" : totalTransmisionDet.Cpattdtotmes03.ToString(),
                        totalTransmisionDet.Cpattdtotmes04 == null ? "" : totalTransmisionDet.Cpattdtotmes04.ToString(),
                        totalTransmisionDet.Cpattdtotmes05 == null ? "" : totalTransmisionDet.Cpattdtotmes05.ToString(),
                        totalTransmisionDet.Cpattdtotmes06 == null ? "" : totalTransmisionDet.Cpattdtotmes06.ToString(),
                        totalTransmisionDet.Cpattdtotmes07 == null ? "" : totalTransmisionDet.Cpattdtotmes07.ToString(),
                        totalTransmisionDet.Cpattdtotmes08 == null ? "" : totalTransmisionDet.Cpattdtotmes08.ToString(),
                        totalTransmisionDet.Cpattdtotmes09 == null ? "" : totalTransmisionDet.Cpattdtotmes09.ToString(),
                        totalTransmisionDet.Cpattdtotmes10 == null ? "" : totalTransmisionDet.Cpattdtotmes10.ToString(),
                        totalTransmisionDet.Cpattdtotmes11 == null ? "" : totalTransmisionDet.Cpattdtotmes11.ToString(),
                        totalTransmisionDet.Cpattdtotmes12 == null ? "" : totalTransmisionDet.Cpattdtotmes12.ToString(),
                        totalTransmisionDet.Cpattdtotal == null ? "" : totalTransmisionDet.Cpattdtotal.ToString()
                    };
                    i++;
                }

                CpaExcelCuerpo cuerpo = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistros,
                    ListaAlineaHorizontal = listaAlineaHorizontal,
                    ListaTipo = listaTipo,
                    ListaEstilo = listaEstilo
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHoja = new CpaExcelHoja
                {
                    NombreHoja = "Transmisión",
                    Titulo = sbTitulo.ToString(),
                    Subtitulo1 = sbSubTitulo1.ToString(),
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    Cuerpo = cuerpo
                };
                listExcelHoja.Add(excelHoja);
                #endregion

                #endregion

            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos del archivo Excel para el Reporte de Porcentaje (Ajuste de Aportes)
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="invocadoPor"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="listExcelHoja"></param>
        public void GenerarArchivoExcelReportePorcentaje(int cparcodi, string invocadoPor, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja)
        {
            try
            {
                #region Validación
                if (cparcodi < 0)
                {
                    throw new MyCustomException("No se puede generar el reporte porque el código de la revisión es inválido.");
                }

                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                if (revision == null)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existe la Revisión con id = " + cparcodi);
                }
                string descRevision = string.Format("Revisión '{0}' del Ajuste '{1}' del Año Presupuestal '{2}'", revision.Cparrevision, revision.Cpaapajuste, revision.Cpaapanio);

                ValidarReporteParaExtranet(revision, invocadoPor);
                #endregion

                #region Obtener datos
                List<CpaPorcentajeEnergiaPotenciaDTO> listPorcentajeEnergiaPotencia = FactoryTransferencia.GetCpaPorcentajeEnergiaPotenciaRepository().ListByRevision(revision.Cparcodi);
                bool existeListPorcentajeEnergiaPotencia = (listPorcentajeEnergiaPotencia != null && listPorcentajeEnergiaPotencia.Count > 0);

                List<CpaPorcentajeMontoDTO> listPorcentajeMonto = FactoryTransferencia.GetCpaPorcentajeMontoRepository().ListByRevision(revision.Cparcodi);
                bool existeListPorcentajeMonto = (listPorcentajeMonto != null && listPorcentajeMonto.Count > 0);

                List<CpaPorcentajePorcentajeDTO> listPorcentajePorcentaje = FactoryTransferencia.GetCpaPorcentajePorcentajeRepository().ListByRevision(revision.Cparcodi);
                bool existeListPorcentajePorcentaje = (listPorcentajePorcentaje != null && listPorcentajePorcentaje.Count > 0);

                if (!existeListPorcentajeEnergiaPotencia && !existeListPorcentajeMonto && !existeListPorcentajePorcentaje)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existen registros a exportar. Por favor, ejecute el cálculo de Porcentaje Presupuestal para la " + descRevision);
                }
                #endregion

                #region Variables
                string anioEjercicioCorto = revision.Cpaapanioejercicio.ToString().Substring(2);
                string nombreRevision = (revision.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("_Rev" + revision.Cparrevision.Substring(ConstantesCPPA.numero9));
                nombreArchivo = string.Format("Ajuste_Aportes-{0}-{1}{2}", revision.Cpaapanioejercicio.ToString().Substring(2), revision.Cpaapajuste, nombreRevision);
                listExcelHoja = new List<CpaExcelHoja>();
                int i = 0;
                #endregion

                #region Hoja Energía Potencia

                #region Titulos
                nombreRevision = (revision.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("Rev. " + revision.Cparrevision.Substring(ConstantesCPPA.numero9));
                StringBuilder sbTituloEnergiaPotencia = new StringBuilder();
                sbTituloEnergiaPotencia.Append("Energía Potencia ");
                sbTituloEnergiaPotencia.Append(revision.Cpaapanioejercicio);
                StringBuilder sbSubTitulo1 = new StringBuilder();
                sbSubTitulo1.Append("Ajuste ");
                sbSubTitulo1.Append(revision.Cpaapajuste.Substring(1));
                if (nombreRevision.Length > 0)
                {
                    sbSubTitulo1.Append(" - ");
                    sbSubTitulo1.Append(nombreRevision);
                }
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasEnergiaPotencia = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabeceraEnergiaPotencia1 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Tipo", "center", 1, 2),
                    CrearExcelModelo("Empresa", "center", 1, 2),
                    CrearExcelModelo("RUC", "center", 1, 2),
                    CrearExcelModelo("Energía (MWh)", "center", 13),
                    CrearExcelModelo("Potencia (MW)", "center", 13)
                };
                List<CpaExcelModelo> listaCabeceraEnergiaPotencia2 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Ene-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Feb-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Mar-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Abr-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("May-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jun-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jul-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Ago-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Sep-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Oct-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Nov-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Dic-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Total", "center"),
                    CrearExcelModelo("Ene-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Feb-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Mar-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Abr-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("May-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jun-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jul-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Ago-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Sep-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Oct-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Nov-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Dic-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Total", "center")
                };
                List<int> listaAnchoColumnaEnergiaPotencia = new List<int> { 20, 50, 15,
                    15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 
                    15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15 };

                listaCabecerasEnergiaPotencia[0] = listaCabeceraEnergiaPotencia1;
                listaCabecerasEnergiaPotencia[1] = listaCabeceraEnergiaPotencia2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalEnergiaPotencia = new List<string> { "left", "left", "right",
                    "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", 
                    "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right" };
                List<string> listaTipoEnergiaPotencia = new List<string> { "string", "string", "string",
                    "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double",
                    "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double" };
                List<CpaExcelEstilo> listaEstiloEnergiaPotencia = new List<CpaExcelEstilo> {
                    null, 
                    null, 
                    null, 

                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),

                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00")
                };

                i = 0;
                List<string>[] listaRegistrosEnergiaPotencia = new List<string>[listPorcentajeEnergiaPotencia.Count];
                foreach (CpaPorcentajeEnergiaPotenciaDTO porcentajeEnergiaPotencia in listPorcentajeEnergiaPotencia)
                {
                    listaRegistrosEnergiaPotencia[i] = new List<string> {
                        porcentajeEnergiaPotencia.Tipoemprdesc,
                        porcentajeEnergiaPotencia.Emprnomb,
                        porcentajeEnergiaPotencia.Emprruc,

                        porcentajeEnergiaPotencia.Cpapepenemes01 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes01.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes02 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes02.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes03 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes03.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes04 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes04.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes05 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes05.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes06 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes06.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes07 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes07.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes08 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes08.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes09 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes09.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes10 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes10.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes11 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes11.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenemes12 == null ? "" : porcentajeEnergiaPotencia.Cpapepenemes12.ToString(),
                        porcentajeEnergiaPotencia.Cpapepenetotal == null ? "" : porcentajeEnergiaPotencia.Cpapepenetotal.ToString(),

                        porcentajeEnergiaPotencia.Cpapeppotmes01 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes01.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes02 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes02.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes03 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes03.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes04 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes04.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes05 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes05.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes06 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes06.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes07 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes07.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes08 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes08.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes09 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes09.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes10 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes10.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes11 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes11.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppotmes12 == null ? "" : porcentajeEnergiaPotencia.Cpapeppotmes12.ToString(),
                        porcentajeEnergiaPotencia.Cpapeppottotal == null ? "" : porcentajeEnergiaPotencia.Cpapeppottotal.ToString()
                    };
                    i++;
                }

                CpaExcelCuerpo cuerpoEnergiaPotencia = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosEnergiaPotencia,
                    ListaAlineaHorizontal = listaAlineaHorizontalEnergiaPotencia,
                    ListaTipo = listaTipoEnergiaPotencia,
                    ListaEstilo = listaEstiloEnergiaPotencia
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaEnergiaPotencia = new CpaExcelHoja
                {
                    NombreHoja = "Energía-Potencia " + revision.Cpaapanioejercicio,
                    Titulo = sbTituloEnergiaPotencia.ToString(),
                    Subtitulo1 = sbSubTitulo1.ToString(),
                    ListaAnchoColumna = listaAnchoColumnaEnergiaPotencia,
                    ListaCabeceras = listaCabecerasEnergiaPotencia,
                    Cuerpo = cuerpoEnergiaPotencia
                };
                listExcelHoja.Add(excelHojaEnergiaPotencia);
                #endregion

                #endregion

                #region Hoja Montos

                #region Titulos
                StringBuilder sbTituloMontos = new StringBuilder();
                sbTituloMontos.Append("Montos ");
                sbTituloMontos.Append(revision.Cpaapanioejercicio);
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasMontos = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabeceraMontos1 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Tipo", "center", 1, 2),
                    CrearExcelModelo("Empresa", "center", 1, 2),
                    CrearExcelModelo("RUC", "center", 1, 2),
                    CrearExcelModelo("Montos - Energía (S/.)", "center", 13),
                    CrearExcelModelo("Montos - Potencia (S/.)", "center", 13),
                    CrearExcelModelo("Montos - Transmisión (S/.)", "center", 13)
                };
                List<CpaExcelModelo> listaCabeceraMontos2 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Ene-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Feb-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Mar-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Abr-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("May-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jun-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jul-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Ago-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Sep-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Oct-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Nov-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Dic-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Total", "center"),
                    CrearExcelModelo("Ene-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Feb-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Mar-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Abr-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("May-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jun-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jul-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Ago-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Sep-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Oct-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Nov-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Dic-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Total", "center"),
                    CrearExcelModelo("Ene-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Feb-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Mar-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Abr-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("May-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jun-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Jul-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Ago-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Sep-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Oct-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Nov-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Dic-" + anioEjercicioCorto, "center"),
                    CrearExcelModelo("Total", "center")
                };
                List<int> listaAnchoColumnaMontos = new List<int> { 20, 50, 15,
                    15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                    15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                    15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15 };

                listaCabecerasMontos[0] = listaCabeceraMontos1;
                listaCabecerasMontos[1] = listaCabeceraMontos2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalMontos = new List<string> { "left", "left", "right",
                    "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right",
                    "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right",
                    "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right", "right" };
                List<string> listaTipoMontos = new List<string> { "string", "string", "string",
                    "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double",
                    "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double",
                    "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double", "double" };
                List<CpaExcelEstilo> listaEstiloMontos = new List<CpaExcelEstilo> {
                    null,
                    null,
                    null,

                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),

                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),

                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00")
                };

                i = 0;
                List<string>[] listaRegistrosMontos = new List<string>[listPorcentajeMonto.Count];
                foreach (CpaPorcentajeMontoDTO porcentajeMonto in listPorcentajeMonto)
                {
                    listaRegistrosMontos[i] = new List<string> {
                        porcentajeMonto.Tipoemprdesc,
                        porcentajeMonto.Emprnomb,
                        porcentajeMonto.Emprruc,

                        porcentajeMonto.Cpapmtenemes01 == null ? "" : porcentajeMonto.Cpapmtenemes01.ToString(),
                        porcentajeMonto.Cpapmtenemes02 == null ? "" : porcentajeMonto.Cpapmtenemes02.ToString(),
                        porcentajeMonto.Cpapmtenemes03 == null ? "" : porcentajeMonto.Cpapmtenemes03.ToString(),
                        porcentajeMonto.Cpapmtenemes04 == null ? "" : porcentajeMonto.Cpapmtenemes04.ToString(),
                        porcentajeMonto.Cpapmtenemes05 == null ? "" : porcentajeMonto.Cpapmtenemes05.ToString(),
                        porcentajeMonto.Cpapmtenemes06 == null ? "" : porcentajeMonto.Cpapmtenemes06.ToString(),
                        porcentajeMonto.Cpapmtenemes07 == null ? "" : porcentajeMonto.Cpapmtenemes07.ToString(),
                        porcentajeMonto.Cpapmtenemes08 == null ? "" : porcentajeMonto.Cpapmtenemes08.ToString(),
                        porcentajeMonto.Cpapmtenemes09 == null ? "" : porcentajeMonto.Cpapmtenemes09.ToString(),
                        porcentajeMonto.Cpapmtenemes10 == null ? "" : porcentajeMonto.Cpapmtenemes10.ToString(),
                        porcentajeMonto.Cpapmtenemes11 == null ? "" : porcentajeMonto.Cpapmtenemes11.ToString(),
                        porcentajeMonto.Cpapmtenemes12 == null ? "" : porcentajeMonto.Cpapmtenemes12.ToString(),
                        porcentajeMonto.Cpapmtenetotal == null ? "" : porcentajeMonto.Cpapmtenetotal.ToString(),

                        porcentajeMonto.Cpapmtpotmes01 == null ? "" : porcentajeMonto.Cpapmtpotmes01.ToString(),
                        porcentajeMonto.Cpapmtpotmes02 == null ? "" : porcentajeMonto.Cpapmtpotmes02.ToString(),
                        porcentajeMonto.Cpapmtpotmes03 == null ? "" : porcentajeMonto.Cpapmtpotmes03.ToString(),
                        porcentajeMonto.Cpapmtpotmes04 == null ? "" : porcentajeMonto.Cpapmtpotmes04.ToString(),
                        porcentajeMonto.Cpapmtpotmes05 == null ? "" : porcentajeMonto.Cpapmtpotmes05.ToString(),
                        porcentajeMonto.Cpapmtpotmes06 == null ? "" : porcentajeMonto.Cpapmtpotmes06.ToString(),
                        porcentajeMonto.Cpapmtpotmes07 == null ? "" : porcentajeMonto.Cpapmtpotmes07.ToString(),
                        porcentajeMonto.Cpapmtpotmes08 == null ? "" : porcentajeMonto.Cpapmtpotmes08.ToString(),
                        porcentajeMonto.Cpapmtpotmes09 == null ? "" : porcentajeMonto.Cpapmtpotmes09.ToString(),
                        porcentajeMonto.Cpapmtpotmes10 == null ? "" : porcentajeMonto.Cpapmtpotmes10.ToString(),
                        porcentajeMonto.Cpapmtpotmes11 == null ? "" : porcentajeMonto.Cpapmtpotmes11.ToString(),
                        porcentajeMonto.Cpapmtpotmes12 == null ? "" : porcentajeMonto.Cpapmtpotmes12.ToString(),
                        porcentajeMonto.Cpapmtpottotal == null ? "" : porcentajeMonto.Cpapmtpottotal.ToString(),

                        porcentajeMonto.Cpapmttrames01 == null ? "" : porcentajeMonto.Cpapmttrames01.ToString(),
                        porcentajeMonto.Cpapmttrames02 == null ? "" : porcentajeMonto.Cpapmttrames02.ToString(),
                        porcentajeMonto.Cpapmttrames03 == null ? "" : porcentajeMonto.Cpapmttrames03.ToString(),
                        porcentajeMonto.Cpapmttrames04 == null ? "" : porcentajeMonto.Cpapmttrames04.ToString(),
                        porcentajeMonto.Cpapmttrames05 == null ? "" : porcentajeMonto.Cpapmttrames05.ToString(),
                        porcentajeMonto.Cpapmttrames06 == null ? "" : porcentajeMonto.Cpapmttrames06.ToString(),
                        porcentajeMonto.Cpapmttrames07 == null ? "" : porcentajeMonto.Cpapmttrames07.ToString(),
                        porcentajeMonto.Cpapmttrames08 == null ? "" : porcentajeMonto.Cpapmttrames08.ToString(),
                        porcentajeMonto.Cpapmttrames09 == null ? "" : porcentajeMonto.Cpapmttrames09.ToString(),
                        porcentajeMonto.Cpapmttrames10 == null ? "" : porcentajeMonto.Cpapmttrames10.ToString(),
                        porcentajeMonto.Cpapmttrames11 == null ? "" : porcentajeMonto.Cpapmttrames11.ToString(),
                        porcentajeMonto.Cpapmttrames12 == null ? "" : porcentajeMonto.Cpapmttrames12.ToString(),
                        porcentajeMonto.Cpapmttratotal == null ? "" : porcentajeMonto.Cpapmttratotal.ToString()
                    };
                    i++;
                }

                CpaExcelCuerpo cuerpoMontos = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosMontos,
                    ListaAlineaHorizontal = listaAlineaHorizontalMontos,
                    ListaTipo = listaTipoMontos,
                    ListaEstilo = listaEstiloMontos
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaMontos = new CpaExcelHoja
                {
                    NombreHoja = "Montos " + revision.Cpaapanioejercicio,
                    Titulo = sbTituloMontos.ToString(),
                    Subtitulo1 = sbSubTitulo1.ToString(),
                    ListaAnchoColumna = listaAnchoColumnaMontos,
                    ListaCabeceras = listaCabecerasMontos,
                    Cuerpo = cuerpoMontos
                };
                listExcelHoja.Add(excelHojaMontos);
                #endregion

                #endregion

                #region Hoja Porcentaje

                #region Titulos
                StringBuilder sbTituloPorcentaje = new StringBuilder();
                sbTituloPorcentaje.Append("Porcentaje de Aportes Anual de los Integrantes Registrados para el Presupuesto del COES - ");
                sbTituloPorcentaje.Append(revision.Cpaapanio);
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasPorcentaje = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabeceraPorcentaje1 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Tipo", "center", 1, 2),
                    CrearExcelModelo("Empresa", "center", 1, 2),
                    CrearExcelModelo("RUC", "center", 1, 2),
                    CrearExcelModelo("Concesión de Generación", "center", 2),
                    CrearExcelModelo("Concesión de Distribución", "center", 2),
                    CrearExcelModelo("Concesión de Usuario Libre", "center", 2),
                    CrearExcelModelo("Concesión de Transmisión", "center"),
                    CrearExcelModelo("Total (S/.)", "center", 1, 2),
                    CrearExcelModelo("Porcentaje de Aporte Anual (%)", "center", 1, 2)
                };
                List<CpaExcelModelo> listaCabeceraPorcentaje2 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Montos - Energía (S/.)", "center"),
                    CrearExcelModelo("Montos - Potencia (S/.)", "center"),
                    CrearExcelModelo("Montos - Energía (S/.)", "center"),
                    CrearExcelModelo("Montos - Potencia (S/.)", "center"),
                    CrearExcelModelo("Montos - Energía (S/.)", "center"),
                    CrearExcelModelo("Montos - Potencia (S/.)", "center"),
                    CrearExcelModelo("Montos - Transmisión (S/.)", "center")
                };
                List<int> listaAnchoColumnaPorcentaje = new List<int> { 20, 50, 15, 
                    20, 20, 20, 20, 20, 20, 25,
                    15, 15 };

                listaCabecerasPorcentaje[0] = listaCabeceraPorcentaje1;
                listaCabecerasPorcentaje[1] = listaCabeceraPorcentaje2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalPorcentaje = new List<string> { "left", "left", "right", 
                    "right", "right", "right", "right", "right", "right", "right",
                    "right", "right" };
                List<string> listaTipoPorcentaje = new List<string> { "string", "string", "string", 
                    "double", "double", "double", "double", "double", "double", "double",
                    "double", "double" };
                List<CpaExcelEstilo> listaEstiloPorcentaje = new List<CpaExcelEstilo> {
                    null,
                    null,
                    null,

                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),

                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.000")
                };

                i = 0;
                List<string>[] listaRegistrosPorcentaje = new List<string>[listPorcentajePorcentaje.Count];
                foreach (CpaPorcentajePorcentajeDTO porcentajePorcentaje in listPorcentajePorcentaje)
                {
                    listaRegistrosPorcentaje[i] = new List<string> {
                        porcentajePorcentaje.Tipoemprdesc,
                        porcentajePorcentaje.Emprnomb,
                        porcentajePorcentaje.Emprruc,

                        porcentajePorcentaje.Cpappgentotene == null ? "" : porcentajePorcentaje.Cpappgentotene.ToString(),
                        porcentajePorcentaje.Cpappgentotpot == null ? "" : porcentajePorcentaje.Cpappgentotpot.ToString(),
                        porcentajePorcentaje.Cpappdistotene == null ? "" : porcentajePorcentaje.Cpappdistotene.ToString(),
                        porcentajePorcentaje.Cpappdistotpot == null ? "" : porcentajePorcentaje.Cpappdistotpot.ToString(),
                        porcentajePorcentaje.Cpappultotene == null ? "" : porcentajePorcentaje.Cpappultotene.ToString(),
                        porcentajePorcentaje.Cpappultotpot == null ? "" : porcentajePorcentaje.Cpappultotpot.ToString(),
                        porcentajePorcentaje.Cpapptratot == null ? "" : porcentajePorcentaje.Cpapptratot.ToString(),

                        porcentajePorcentaje.Cpapptotal == null ? "" : porcentajePorcentaje.Cpapptotal.ToString(),
                        porcentajePorcentaje.Cpappporcentaje.ToString()
                    };
                    i++;
                }

                CpaExcelCuerpo cuerpoPorcentaje = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosPorcentaje,
                    ListaAlineaHorizontal = listaAlineaHorizontalPorcentaje,
                    ListaTipo = listaTipoPorcentaje,
                    ListaEstilo = listaEstiloPorcentaje
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaPorcentaje = new CpaExcelHoja
                {
                    NombreHoja = "Porcentaje",
                    Titulo = sbTituloPorcentaje.ToString(),
                    Subtitulo1 = sbSubTitulo1.ToString(),
                    ListaAnchoColumna = listaAnchoColumnaPorcentaje,
                    ListaCabeceras = listaCabecerasPorcentaje,
                    Cuerpo = cuerpoPorcentaje
                };
                listExcelHoja.Add(excelHojaPorcentaje);
                #endregion

                #endregion
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
                throw;
            }
        }

        /// <summary>
        /// Validaciones en caso sea invocado desde la Extranet
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="invocadoPor"></param>
        /// <exception cref="MyCustomException"></exception>
        private void ValidarReporteParaExtranet(CpaRevisionDTO revision, string invocadoPor)
        {
            if (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet)
            {
                if (revision.Cparestado == ConstantesCPPA.estadoRevisionAnulado)
                {
                    throw new MyCustomException("El reporte no puede ser descargado, debido a que la Revisión del Ajuste del Año presupuestal seleccionada ha sido anulada por el COES.");
                }

                GetLogProcesoPorcentaje(revision.Cparcodi, out string estadoPublicacion, out List<GenericoDTO> listaReporteMensuales, out List<GenericoDTO> listaReporteAnuales);

                if (estadoPublicacion != ConstantesCPPA.estadoPublicacionSi)
                {
                    throw new MyCustomException("Los reportes no están disponibles para la Revisión del Ajuste del Año presupuestal seleccionada, pues aún no han sido publicados por el COES.");
                }
            }
        }

        /// <summary>
        /// Obtener lista de registros de demanda
        /// </summary>
        /// <param name="listTotalDemandaDet"></param>
        private List<string>[] ObtenerRegistrosDemanda(List<CpaTotalDemandaDetDTO> listTotalDemandaDet)
        {
            int i = 0;
            bool existListTotalDemandaDet = (listTotalDemandaDet != null && listTotalDemandaDet.Count > 0);
            int count = (existListTotalDemandaDet) ? listTotalDemandaDet.Count : 1;
            List<string>[] listaRegistros = new List<string>[count];

            if (existListTotalDemandaDet)
            {
                foreach (CpaTotalDemandaDetDTO totalDemandaDet in listTotalDemandaDet)
                {
                    listaRegistros[i] = new List<string> {
                            totalDemandaDet.Emprnomb,
                            totalDemandaDet.Cpatddtotenemwh == null ? "" : totalDemandaDet.Cpatddtotenemwh.Value.ToString(),
                            totalDemandaDet.Cpatddtotenesoles == null ? "" : totalDemandaDet.Cpatddtotenesoles.Value.ToString(),
                            totalDemandaDet.Cpatddtotpotmw == null ? "" : totalDemandaDet.Cpatddtotpotmw.Value.ToString(),
                            totalDemandaDet.Cpatddtotpotsoles == null ? "" : totalDemandaDet.Cpatddtotpotsoles.Value.ToString()
                        };
                    i++;
                }
            }
            else
            {
                listaRegistros[i] = new List<string> {
                        "No existen registros",
                        "",
                        "",
                        "",
                        ""
                    };
            }

            return listaRegistros;
        }

        #endregion

        #region CU19 - Generar Comparativo Totales

        /// <summary>
        /// Genera los datos del archivo Excel para el Reporte Comparativo para dos Revisiones
        /// </summary>
        /// <param name="cparcodiBase"></param>
        /// <param name="cparcodiComparar"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="listExcelHoja"></param>
        public void GenerarArchivoExcelReporteComparativo(int cparcodiBase, int cparcodiComparar, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja)
        {
            try
            {
                #region Validación
                if (cparcodiBase < 0)
                {
                    throw new MyCustomException("No se puede generar el reporte porque el código de la revisión 'base' es inválido.");
                }

                if (cparcodiComparar < 0)
                {
                    throw new MyCustomException("No se puede generar el reporte porque el código de la revisión 'comparar' es inválido.");
                }

                if (cparcodiBase == cparcodiComparar)
                {
                    throw new MyCustomException("Debe seleccionar Revisiones de Ajustes Presupuestales diferentes.");
                }

                CpaRevisionDTO revisionBase = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodiBase);
                if (revisionBase == null)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existe la revisión 'base' con id = " + cparcodiBase);
                }

                CpaRevisionDTO revisionComparar = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodiComparar);
                if (revisionComparar == null)
                {
                    throw new MyCustomException("No se puede generar el reporte porque no existe la revisión 'comparar' con id = " + cparcodiComparar);
                }
                #endregion

                #region Obtener datos
                List<CpaPorcentajePorcentajeDTO> listPPB = FactoryTransferencia.GetCpaPorcentajePorcentajeRepository().ListByRevision(revisionBase.Cparcodi);
                bool existeListPPB = (listPPB != null && listPPB.Count > 0);
                if (!existeListPPB)
                {
                    throw new MyCustomException("No se ha ejecutado el cálculo de porcentajes presupuestal para la Revisión ‘Base’ del Ajuste Presupuestal seleccionada. Por lo tanto, no es posible continuar.");
                }

                List<CpaPorcentajePorcentajeDTO> listPPC = FactoryTransferencia.GetCpaPorcentajePorcentajeRepository().ListByRevision(revisionComparar.Cparcodi);
                bool existeListPPC = (listPPC != null && listPPC.Count > 0);
                if (!existeListPPC)
                {
                    throw new MyCustomException("No se ha ejecutado el cálculo de porcentajes presupuestal para la Revisión ‘Comparar’ del Ajuste Presupuestal seleccionada. Por lo tanto, no es posible continuar.");
                }

                List<CpaPorcentajePorcentajeDTO> listNoParticipantes = listPPC.Where(x => !listPPB.Exists(y => y.Emprcodi == x.Emprcodi)).ToList();

                foreach(CpaPorcentajePorcentajeDTO ppb in listPPB)
                {
                    List<CpaPorcentajePorcentajeDTO> _listPPC = listPPC.Where(x => x.Emprcodi == ppb.Emprcodi).ToList();
                    if (_listPPC.Count > 0)
                    {
                        bool isOk = ppb.Cpapptotal != null || _listPPC[0].Cpapptotal != null;
                        if (isOk)
                        {
                            ppb.CpapptotalComparar = _listPPC[0].Cpapptotal;
                            decimal CT = _listPPC[0].Cpapptotal ?? 0M;
                            decimal BT = ppb.Cpapptotal ?? 0M;
                            ppb.DesviacionTotal = (CT != 0) ? ((CT - BT) / CT) : 0M;
                        }

                        ppb.CpappporcentajeComparar = _listPPC[0].Cpappporcentaje;
                        decimal CP = _listPPC[0].Cpappporcentaje;
                        decimal BP = ppb.Cpappporcentaje;
                        ppb.DesviacionPorcentaje = (CP != 0) ? ((CP - BP) / CP) : 0M;
                    }
                    else 
                    {
                        if (ppb.Cpapptotal != null)
                        {
                            ppb.DesviacionTotal = 0M;
                        }
                    }
                }
                #endregion

                #region Variables
                string nombreRevisionBase = (revisionBase.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("-Rev" + revisionBase.Cparrevision.Substring(ConstantesCPPA.numero9));
                string nombreRevisionComparar = (revisionComparar.Cparrevision == ConstantesCPPA.revisionNormal) ? "" : ("-Rev" + revisionComparar.Cparrevision.Substring(ConstantesCPPA.numero9));
                nombreArchivo = string.Format("Comparativo_{0}-{1}{2}_{3}-{4}{5}", 
                    revisionBase.Cpaapanio.ToString().Substring(2), revisionBase.Cpaapajuste, nombreRevisionBase,
                    revisionComparar.Cpaapanio.ToString().Substring(2), revisionComparar.Cpaapajuste, nombreRevisionComparar);
                string tituloBase = string.Format("{0}-{1}{2}", revisionBase.Cpaapanio, revisionBase.Cpaapajuste, nombreRevisionBase);
                string tituloComparar = string.Format("{0}-{1}{2}", revisionComparar.Cpaapanio, revisionComparar.Cpaapajuste, nombreRevisionComparar);
                listExcelHoja = new List<CpaExcelHoja>();
                int i = 0;
                #endregion

                #region Hoja Comparativo

                #region Titulos
                StringBuilder sbTituloC = new StringBuilder();
                sbTituloC.Append("Comparativo de Totales (S/.) del Presupuesto del COES");
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasC = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabeceraC1 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Tipo de Integrante Registrado", "center", 1, 2),
                    CrearExcelModelo("Integrante Registrado", "center", 1, 2),
                    CrearExcelModelo("Total (S/.)", "center", 3, 1),
                    CrearExcelModelo("Porcentaje", "center", 3, 1)
                };
                List<CpaExcelModelo> listaCabeceraC2 = new List<CpaExcelModelo> {
                    CrearExcelModelo(tituloBase, "center"),
                    CrearExcelModelo(tituloComparar, "center"),
                    CrearExcelModelo("% Desviación", "center"),
                    CrearExcelModelo(tituloBase.ToString(), "center"),
                    CrearExcelModelo(tituloComparar.ToString(), "center"),
                    CrearExcelModelo("% Desviación", "center")
                };
                List<int> listaAnchoColumnaC = new List<int> { 20, 50, 15, 15, 15, 15, 15, 15 };

                listaCabecerasC[0] = listaCabeceraC1;
                listaCabecerasC[1] = listaCabeceraC2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalC = new List<string> { "left", "left", "right", "right", "right", "right", "right", "right" };
                List<string> listaTipoC = new List<string> { "string", "string", "double", "double", "double", "double", "double", "double" };
                List<CpaExcelEstilo> listaEstiloC = new List<CpaExcelEstilo> {
                    null, 
                    null,
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.000"),
                    CrearExcelEstilo("#,##0.000"),
                    CrearExcelEstilo("#,##0.000")
                };

                i = 0;
                List<string>[] listaRegistrosC = new List<string>[listPPB.Count];
                foreach(CpaPorcentajePorcentajeDTO ppb in listPPB)
                {
                    listaRegistrosC[i] = new List<string> {
                        ppb.Tipoemprdesc,
                        ppb.Emprnomb,
                        (ppb.Cpapptotal != null) ? ppb.Cpapptotal.ToString() : "",
                        (ppb.CpapptotalComparar != null) ? ppb.CpapptotalComparar.ToString() : "",
                        (ppb.DesviacionTotal != null) ? ppb.DesviacionTotal.ToString() : "",
                        ppb.Cpappporcentaje.ToString(),
                        ppb.CpappporcentajeComparar.ToString(),
                        ppb.DesviacionPorcentaje.ToString()
                    };
                    i++;
                }

                CpaExcelCuerpo cuerpoC = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosC,
                    ListaAlineaHorizontal = listaAlineaHorizontalC,
                    ListaTipo = listaTipoC,
                    ListaEstilo = listaEstiloC
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaC = new CpaExcelHoja
                {
                    NombreHoja = "Comparativo",
                    Titulo = sbTituloC.ToString(),
                    ListaAnchoColumna = listaAnchoColumnaC,
                    ListaCabeceras = listaCabecerasC,
                    Cuerpo = cuerpoC
                };
                listExcelHoja.Add(excelHojaC);
                #endregion

                #endregion

                #region Hoja No Participantes

                #region Titulos
                StringBuilder sbTituloNP = new StringBuilder();
                sbTituloNP.Append("Comparativo de Totales (S/.) del Presupuesto del COES");
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabecerasNP = new List<CpaExcelModelo>[2];
                List<CpaExcelModelo> listaCabeceraNP1 = new List<CpaExcelModelo> {
                    CrearExcelModelo(tituloComparar.ToString(), "center", 4, 1)
                };
                List<CpaExcelModelo> listaCabeceraNP2 = new List<CpaExcelModelo> {
                    CrearExcelModelo("Tipo de Integrante Registrado", "center"),
                    CrearExcelModelo("Integrante Registrado", "center"),
                    CrearExcelModelo("Total (S/.)", "center"),
                    CrearExcelModelo("Porcentaje", "center")
                };
                List<int> listaAnchoColumnaNP = new List<int> { 20, 50, 15, 15 };

                listaCabecerasNP[0] = listaCabeceraNP1;
                listaCabecerasNP[1] = listaCabeceraNP2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalNP = new List<string> { "left", "left", "right", "right" };
                List<string> listaTipoNP = new List<string> { "string", "string", "double", "double" };
                List<CpaExcelEstilo> listaEstiloNP = new List<CpaExcelEstilo> {
                    null, 
                    null,
                    CrearExcelEstilo("#,##0.00"),
                    CrearExcelEstilo("#,##0.000")
                };

                i = 0;
                int countNP = (listNoParticipantes.Count > 0) ? listNoParticipantes.Count : 1;
                List<string>[] listaRegistrosNP = new List<string>[countNP];
                if (listNoParticipantes.Count > 0)
                {
                    foreach (CpaPorcentajePorcentajeDTO np in listNoParticipantes)
                    {
                        listaRegistrosNP[i] = new List<string> {
                            np.Tipoemprdesc,
                            np.Emprnomb,
                            np.Cpapptotal != null ? np.Cpapptotal.ToString() : "",
                            np.Cpappporcentaje.ToString()
                        };
                        i++;
                    }
                }
                else
                {
                    listaRegistrosNP[i] = new List<string> {
                        "No existen registros",
                        "",
                        "",
                        ""
                    };
                }

                CpaExcelCuerpo cuerpoNP = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosNP,
                    ListaAlineaHorizontal = listaAlineaHorizontalNP,
                    ListaTipo = listaTipoNP,
                    ListaEstilo = listaEstiloNP
                };
                #endregion

                #region Definir hoja
                CpaExcelHoja excelHojaNP = new CpaExcelHoja
                {
                    NombreHoja = "No Participantes",
                    Titulo = sbTituloNP.ToString(),
                    ListaAnchoColumna = listaAnchoColumnaNP,
                    ListaCabeceras = listaCabecerasNP,
                    Cuerpo = cuerpoNP
                };
                listExcelHoja.Add(excelHojaNP);
                #endregion

                #endregion                
            }
            catch (MyCustomException ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
                throw;
            }
        }


        #endregion

        #region CU20 - Descargar Reportes Ajustes
        //Se invocan los reportes del CU18 - Descargar y Publicar Reportes Ajustes
        #endregion

        #endregion

        #region Métodos generales 
        #region CU06
        /// <summary>
        /// Lista de registros por revision, anio, mes y estado
        /// </summary>
        /// <param name="revision">Identificador de la revision</param>
        /// <param name="anio">Anio</param>
        /// <param name="mes">Mes</param>
        /// <param name="estado">Estado del parametro A = Activo y X = Anulado</param>
        /// <returns></returns>
        public List<CpaParametroDTO> ListaParametrosByRevisionAnioMesEstado(int revision, int anio, int mes, string estado) {
            return FactoryTransferencia.GetCpaParametroRepository().ListaParametrosByRevisionAnioMesEstado(revision, anio, mes, estado);
        }
        #endregion

        #region CU05
        /// <summary>
        /// Lista de registros para para grabarlos en CPA_PARAMETROS
        /// </summary>
        /// <param name="anio">Identificador de la empresa.</param>
        /// <returns></returns>
        public List<VtpRecalculoPotenciaDTO> ListRecalculoByPeriodo(int anio)
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().ListRecalculoByPeriodo(anio);
        }

        /// <summary>
        /// Lista parametros por anio, revision y estado para la gilla principal del CU05
        /// </summary>
        /// <param name="revision">Identificador de la empresa.</param>
        /// <param name="estado">Identificador de la empresa.</param>
        /// <param name="anio">Identificador de la empresa.</param>
        /// <returns></returns>
        public List<CpaParametroDTO> ListaParametrosRegistrados(int revision, string estado, int anio)
        {
            return FactoryTransferencia.GetCpaParametroRepository().ListaParametrosRegistrados(revision, estado, anio);
        }

        /// <summary>
        /// Lista parametros hsitoricos por identificador de la tabla CPA_PARAMETRO
        /// </summary>
        /// <param name="cpaprmcodi">Identificador de la tabla CPA_PARAMETRO.</param>
        /// <returns></returns>
        public List<CpaParametroHistoricoDTO> ListaParametrosHistoricos(int cpaprmcodi)
        {
            return FactoryTransferencia.GetCpaParametroHistoricoRepository().ListaParametrosHistoricos(cpaprmcodi);
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_CENTRAL
        /// </summary>
        public void UpdateCpaParametroTipoYCambio(CpaParametroDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaParametroRepository().UpdateCpaParametroTipoYCambio(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_CENTRAL
        /// </summary>
        public void UpdateCpaParametroEstado(CpaParametroDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaParametroRepository().UpdateCpaParametroEstado(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region CU04
        /// <summary>
        /// Lista de centrales PMPO para una CPA_CENTRAL
        /// </summary>
        /// <param name="cpacntcodi">Identificador de la tabla CPA_CENTRAL_PMPO.</param>
        /// <returns></returns>
        public List<CpaCentralPmpoDTO> ListCpaCentralPmpobyCentral(int cpacntcodi)
        {
            return FactoryTransferencia.GetCpaCentralPmpoRepository().ListCpaCentralPmpobyCentral(cpacntcodi);
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_CENTRAL
        /// </summary>
        public void UpdateCpaCentralPMPO(CpaCentralDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCentralRepository().UpdateCentralPMPO(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region CU03
        /// <summary>
        /// Lista de centrales PMPO para una CPA_CENTRAL
        /// </summary>
        /// <param name="empresa">Identificador de la empresa.</param>
        /// <param name="revision">Identificador de la revision.</param>
        /// <param name="central">Identificador de la central.</param>
        /// <returns></returns>
        public List<CpaCentralDTO> ListaCentralesPorEmpresaRevison(int empresa, int revision, int central)
        {
            return FactoryTransferencia.GetCpaCentralRepository().ListaCentralesPorEmpresaRevison(empresa, revision, central);
        }

        /// <summary>
        /// Lista de centrales PMPO para una CPA_CENTRAL
        /// </summary>
        /// <param name="revision">Identificador de la revision.</param>
        /// <param name="central">Identificador de la central.</param>
        /// <returns></returns>
        public List<CpaCentralDTO> ListaCentralesPorRevison(int revision, int central)
        {
            return FactoryTransferencia.GetCpaCentralRepository().ListaCentralesPorRevison(revision, central);
        }

        /// <summary>
        /// Lista de centrales PMPO para una empresa
        /// </summary>
        /// <param name="revision">Identificador de la revision.</param>
        /// <param name="tipo">Tipo de la empresa.</param>
        /// <param name="empresa">Identificador de la empresa.</param>
        /// <returns></returns>
        public List<CpaEmpresaDTO> ListaEmpresaPorRevisionTipo(int revision, string tipo, int empresa)
        {
            return FactoryTransferencia.GetCpaEmpresaRepository().ListaEmpresaPorRevisionTipo(revision, tipo, empresa);
        }

        /// <summary>
        /// Lista de centrales PMPO para una empresa
        /// </summary>
        /// <param name="empresa">Identificador de la empresa.</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListaCentralesPMPO(int empresa)
        {
            return FactorySic.GetMePtomedicionRepository().ListaCentralesPMPO(empresa);
        }

        /// <summary>
        /// Lista de barras PMPO
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListaBarrasPMPO()
        {
            return FactorySic.GetMePtomedicionRepository().ListaBarrasPMPO();
        }

        /// <summary>
        /// Lista de barras transferencia
        /// </summary>
        /// <returns></returns>
        public List<BarraDTO> ListaBarrasTransFormato()
        {
            return FactoryTransferencia.GetBarraRepository().ListaBarrasTransFormato();
        }

        /// <summary>
        /// Lista de centrales + empresas a utilizar en la grilla principal del CU04
        /// </summary>
        /// <param name="revision">Identificador de la revision.</param>
        /// <param name="central">Identificador de la central.</param>
        /// <param name="empresa">Identificador de la empresa.</param>
        /// <param name="barraTrans">Identificador de la barra de transferencia.</param>
        /// <returns></returns>
        public List<CpaCentralDTO> ListaCentralesEmpresasParticipantes(int revision, int central, int empresa, int barraTrans)
        {
            return FactoryTransferencia.GetCpaCentralRepository().ListaCentralesEmpresasParticipantes(revision, central, empresa, barraTrans);
        }

        /// <summary>
        /// Lista de barras a utilizar en el filtro Barra Transferencia CU04
        /// </summary>
        /// <param name="revision">Identificador de la revision.</param>
        /// <returns></returns>
        public List<BarraDTO> FiltroBarrasTransIntegrantes(int revision)
        {
            return FactoryTransferencia.GetBarraRepository().FiltroBarrasTransIntegrantes(revision);
        }

        /// <summary>
        /// Lista de empresas a utilizar en el filtro Empresa CU04
        /// </summary>
        /// <param name="revision">Identificador de la revision.</param>
        /// <returns></returns>
        public List<CpaEmpresaDTO> FiltroEmpresasIntegrantes(int revision) {
            return FactoryTransferencia.GetCpaEmpresaRepository().FiltroEmpresasIntegrantes(revision);
        }

        /// <summary>
        /// Lista de centrales a utilizar en el filtro Central CU04
        /// </summary>
        /// <param name="revision">Identificador de la revision.</param>
        /// <returns></returns>
        public List<CpaCentralDTO> FiltroCentralesIntegrantes(int revision)
        {
            return FactoryTransferencia.GetCpaCentralRepository().FiltroCentralesIntegrantes(revision);
        }

        /// <summary>
        /// Lista las centrales disponibles a utilizar en el proyecto CPPA, se trae el codigo y 
        /// el nombre ([codigo] Nombre [estado si es baja])
        /// </summary>
        /// <param name="revision">Identificador de la revision.</param>
        /// <param name="empresa">Identificador de la empresa.</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListaCentralesTipoCPPA(int revision, int empresa)
        {
            List<CpaCentralDTO> listaCentralesRegistradas = ListaCentralesIntegrantes(empresa, revision, ConstantesCPPA.eActivo);

            List<EqEquipoDTO> listaCentrales = this.ListaCentralesCPPA();

            List<EqEquipoDTO> centralesNoRegistradas = listaCentrales
        .Where(e => !listaCentralesRegistradas.Any(reg => reg.Equicodi == e.Equicodi))
        .ToList();

            return centralesNoRegistradas;
        }

        /// <summary>
        /// Lista las empresas disponibles a utilizar en el proyecto CPPA, se trae el codigo y 
        /// el nombre ([codigo] Nombre [estado si es baja])
        /// </summary>
        /// <param name="revision">Identificador de la revision.</param>
        /// <param name="tipo">Tipo de empresa.</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresasTipoCPPA(int revision, string tipo)
        {
            List<CpaEmpresaDTO> listaEmpresasRegistradas = this.ListaEmpresasIntegrantes(revision,  ConstantesCPPA.eActivo, $"'{tipo}'");

            List<SiEmpresaDTO> listaEmpresas = FactorySic.GetSiEmpresaRepository().ListaEmpresasCPPA();

            List<SiEmpresaDTO> empresasNoRegistradas = listaEmpresas
        .Where(e => !listaEmpresasRegistradas.Any(reg => reg.Emprcodi == e.Emprcodi))
        .ToList();

            return empresasNoRegistradas;
        }

        /// <summary>
        /// Lista las centrales a relacionar con una empresa generadora, se trae el codigo y 
        /// el nombre ([codigo] Nombre [estado si es baja])
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListaCentralesCPPA()
        {
            return FactorySic.GetEqEquipoRepository().ListaCentralesGeneracion();
        }
        #endregion

        #region CU011
        /// <summary>
        /// Lista los documentos que cumplen el criterio de busqueda
        /// </summary>
        /// <param name="cparcodi">codigo de la revision</param>
        /// <param name="user">usuario</param>
        /// <param name="inicio">fecha de inicio</param>
        /// <param name="fin">fecha de fin</param>
        /// <param name="emprcodi">codigo de la empresa</param>
        /// <returns></returns>
        public List<CpaDocumentosDTO> GetDocumentosByFilters(int cparcodi, string user, int emprcodi)
        {
            return FactoryTransferencia.GetCpaDocumentosRepository().GetDocumentosByFilters(cparcodi, user, emprcodi);
        }

        /// <summary>
        /// Lista el detalle de un documento
        /// </summary>
        /// <param name="cpadoccodi">codigo del documento</param>
        /// <returns></returns>
        public List<CpaDocumentosDetalleDTO> GetDetalleByDocumento(int cpadoccodi)
        {
            return FactoryTransferencia.GetCpaDocumentosDetalleRepository().GetDetalleByDocumento(cpadoccodi);
        }
        #endregion

        #region CU07-CU08-CU09-CU10 

        #region Listar insumos y meses
        /// <summary>
        /// Lista de Insumos
        /// </summary>
        /// <returns></returns>
        public List<CpaInsumoDTO> ListarInsumos(string cpaapanio, string cpaapajuste, string cparrevision)
        {
            string sMensajeError = "";
            List<CpaInsumoDTO> listCpaInsumoDto = new List<CpaInsumoDTO>();

            try
            {
                #region Validaciones
                if (cpaapanio == "" || cpaapajuste == "" || cparrevision == "")
                {
                    sMensajeError = "Por favor, seleccione un año, ajuste y revisión para ver los datos";
                    throw new Exception(sMensajeError);
                }

                CpaRevisionDTO cpaRevisionDto = GetByIdCpaRevision(Int32.Parse(cparrevision));//ListarRevisiones(Int32.Parse(cpaapanio), Int32.Parse(cpaapanio), ConstantesCPPA.todos, "'A'").Where(item => item.Cpaapajuste == cpaapajuste && item.Cparrevision == cparrevision).ToList().FirstOrDefault();
                if (cpaRevisionDto == null)
                {
                    sMensajeError = "No se encontro una revisión con los datos proporcionados en la Base de Datos.";
                    throw new Exception(sMensajeError);
                }
                #endregion

                int[] ordenInsumos = ConstantesCPPA.ordenInsumos;
                foreach (int insumoTipo in ordenInsumos)
                {
                    CpaInsumoDTO cpaInsumoDTO = FactoryTransferencia.GetCpaInsumoRepository().GetByCparcodiByCpainstipinsumo(cpaRevisionDto.Cparcodi, insumoTipo.ToString());

                    if (cpaInsumoDTO == null)
                    {
                        cpaInsumoDTO = new CpaInsumoDTO();
                        cpaInsumoDTO.Cparcodi = cpaRevisionDto.Cparcodi;
                        cpaInsumoDTO.Cpainstipinsumo = insumoTipo.ToString();
                        cpaInsumoDTO.Cpainstipproceso = "Sin registro";
                        cpaInsumoDTO.Cpainsfecusuario = "Sin registro";
                    }
                    else
                    {
                        cpaInsumoDTO.Cpainsfecusuario = "[" + cpaInsumoDTO.Cpainsfeccreacion.ToString("yyyy-MM-dd HH:mm:ss") + "] - " + cpaInsumoDTO.Cpainsusucreacion;
                    }
                    cpaInsumoDTO.Cpainsdescinsumo = ConstantesCPPA.insumosDesc[insumoTipo - 1];
                    listCpaInsumoDto.Add(cpaInsumoDTO);
                }
                CpaInsumoDTO cpaInsumo4DTO = FactoryTransferencia.GetCpaInsumoRepository().GetAllByCparcodiByCpainstipinsumo(cpaRevisionDto.Cparcodi, "4").Where(insumo=>insumo.Cpainstipproceso == "A").OrderByDescending(insumo => insumo.Cpainscodi).FirstOrDefault();
                if (cpaInsumo4DTO == null) {
                    cpaInsumo4DTO = new CpaInsumoDTO();
                }
                listCpaInsumoDto.Add(cpaInsumo4DTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }

            return listCpaInsumoDto;
        }

        /// <summary>
        /// Lista de meses de un insumo
        /// </summary>
        /// <returns></returns>
        public List<MesAnioPresupuestalDTO> ListarMeses(string cpaapanio, string cpaapajuste, string cparrevision, string sOpcion)
        {
            string sMensajeError = "";
            List<MesAnioPresupuestalDTO> entitys = new List<MesAnioPresupuestalDTO>();

            try
            {
                #region Validaciones
                if (cpaapanio == "" || cpaapajuste == "" || cparrevision == "")
                {
                    sMensajeError = "Por favor, seleccione un año, ajuste y revisión para ver los datos";
                    throw new Exception(sMensajeError);
                }

                CpaRevisionDTO cpaRevisionDto = GetByIdCpaRevision(Int32.Parse(cparrevision));
                if (cpaRevisionDto == null)
                {
                    sMensajeError = "No se encontró una revisión con los datos proporcionados en la Base de Datos.";
                    throw new Exception(sMensajeError);
                }

                CpaAjustePresupuestalDTO cpaAjustePresupuestalDto = GetByIdCpaAjustePresupuestal(cpaRevisionDto.Cpaapcodi);
                if (cpaAjustePresupuestalDto == null)
                {
                    sMensajeError = "No se encontro un Año Presupuestal con los datos proporcionados en la Base de Datos.";
                    throw new Exception(sMensajeError);
                }

                List<CpaCentralDTO> listaCentral = ListCpaCentralByRevision(cpaRevisionDto.Cparcodi);
                listaCentral.RemoveAll(c => c.Barrbarratransferencia == null);
                if (listaCentral.Count() == 0)
                {
                    sMensajeError = "No se puede realizar ningun procedimiento debido a que no se encontraron centrales registradas en la revisión seleccionada.";
                    throw new Exception(sMensajeError);
                }
                #endregion

                foreach (var numMes in ConstantesCPPA.numMeses)
                {
                    MesAnioPresupuestalDTO mesAnioPresupuestal = new MesAnioPresupuestalDTO
                    {
                        Id = numMes,
                        Cparcodi = cpaRevisionDto.Cparcodi,
                        NomMesAnio = ConstantesCPPA.mesesDesc[numMes - 1] + "." + (cpaAjustePresupuestalDto.Cpaapanioejercicio).ToString(),
                        NomMes = ConstantesCPPA.mesesDesc[numMes - 1],
                        Cparcodi_tipoInsumo = cpaAjustePresupuestalDto.Cpaapanio.ToString() + "_" + sOpcion,
                        TipoInsumo = sOpcion
                    };
                    entitys.Add(mesAnioPresupuestal);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }

            return entitys;
        }

        #endregion

        #region Listar insumos para el log
        public List<CpaInsumoDTO> ListarInsumoParaLog(string cpaapanio, string cpaapajuste, string cparrevision, string sOpcion)
        {
            string sMensajeError = "";
            List<CpaInsumoDTO> listCpaInsumoDto = new List<CpaInsumoDTO>();

            try
            {
                #region Validaciones
                if (cpaapanio == "" || cpaapajuste == "" || cparrevision == "")
                {
                    sMensajeError = "Por favor, seleccione un año, ajuste y revisión para ver los datos";
                    throw new Exception(sMensajeError);
                }

                CpaRevisionDTO cpaRevisionDto = GetByIdCpaRevision(Int32.Parse(cparrevision));
                if (cpaRevisionDto == null)
                {
                    sMensajeError = "No se encontro una revisión con los datos proporcionados en la Base de Datos.";
                    throw new Exception(sMensajeError);
                }
                #endregion

                listCpaInsumoDto = FactoryTransferencia.GetCpaInsumoRepository().GetAllByCparcodiByCpainstipinsumo(cpaRevisionDto.Cparcodi, sOpcion);
                foreach (CpaInsumoDTO cpaInsumoDTO in listCpaInsumoDto)
                {
                    cpaInsumoDTO.Cpainsfecusuario = "[" + cpaInsumoDTO.Cpainsfeccreacion.ToString("yyyy-MM-dd HH:mm:ss") + "] - " + cpaInsumoDTO.Cpainsusucreacion;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }

            return listCpaInsumoDto;
        }
        #endregion

        #region Plantilla Insumos de Carga Manual

        /// <summary>
        /// Genera el archivo Excel Plantilla para la importación de Carga manual
        /// </summary>
        /// <param name="sMes">Año Presupuestal seleccionado</param>
        /// <param name="sAnio">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sOpcion"></param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public List<CpaExcelHoja> GenerarArchivoPlantillaExcelCargaManual(string sMes, string sAnio, string sAjuste, string sRevision, string sOpcion, out string nombreArchivo)
        {
            try
            {
                string sMensajeError = "";
                #region Validaciones
                if (sAnio == "" || sAjuste == "" || sRevision == "")
                {
                    sMensajeError = "Por favor, seleccione un año, ajuste y revisión para ver los datos";
                    throw new Exception(sMensajeError);
                }

                CpaRevisionDTO cpaRevisionDto = GetByIdCpaRevision(Int32.Parse(sRevision));
                if (cpaRevisionDto == null)
                {
                    sMensajeError = "No se encontro una revisión con los datos proporcionados en la Base de Datos.";
                    throw new Exception(sMensajeError);
                }
                #endregion

                #region Obtener datos para exportar 

                DateTime dCpaFecha = DateTime.ParseExact(string.Format("01/{0}/{1}", int.Parse(sMes).ToString("D2"), (int.Parse(sAnio) - 1).ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                // Obtener datos: listado de centrales
                List<CpaCentralDTO> listaCentral = ListCpaCentralByRevision(cpaRevisionDto.Cparcodi);
                listaCentral.RemoveAll(c => c.Barrbarratransferencia == null);
                listaCentral = listaCentral.OrderBy(c => c.Equinomb).ToList();

                // Obtengo el nombre del archivo a exportar
                nombreArchivo = ObtenerNombreArchivoPlantillaInsumo(sAnio, sMes, sOpcion);

                #region Armando el archivo Excel

                #region Variables generales
                bool conBarraCabecera = ContieneFilaNombreBarra(sOpcion);
                int numeroCabeceras = conBarraCabecera ? 3 : 2;
                int numeroCabecerasOcultas = 2;
                #endregion

                #region Titulo
                DateTime primerDiaDelMes = new DateTime(dCpaFecha.Year, dCpaFecha.Month, 1);
                DateTime ultimoDiaDelMes = new DateTime(dCpaFecha.Year, dCpaFecha.Month, DateTime.DaysInMonth(dCpaFecha.Year, dCpaFecha.Month));

                string primerDiaFormateado = primerDiaDelMes.ToString("dd/MM/yyyy");
                string ultimoDiaFormateado = ultimoDiaDelMes.ToString("dd/MM/yyyy");

                string titulo = ConstantesCPPA.insumosTituloDesc[Int32.Parse(sOpcion) - 1] + " - " + (int.Parse(sAnio) - 1).ToString() + "_" + ConstantesCPPA.mesesDesc[int.Parse(sMes) - 1];
                string subtitulo1 = "Presupuesto " + sAnio + "-" + sAjuste;
                subtitulo1 += cpaRevisionDto.Cparrevision == "Normal" ? "" : "-" + cpaRevisionDto.Cparrevision;
                #endregion

                #region Cabecera
                List<CpaExcelModelo>[] listaCabeceras = new List<CpaExcelModelo>[numeroCabeceras];
                List<CpaExcelModelo> listaCabecera1 = new List<CpaExcelModelo> { };
                List<CpaExcelModelo> listaCabecera2 = new List<CpaExcelModelo> { };
                List<CpaExcelModelo> listaCabecera3 = new List<CpaExcelModelo> { };

                List<int> listaAnchoColumna = new List<int> { };
                listaCabecera1.Add(CrearExcelModelo("Cargar=>"));
                if (conBarraCabecera) {
                    listaCabecera2.Add(CrearExcelModelo("Central"));
                    listaCabecera3.Add(CrearExcelModelo("S/. /MWh"));
                }
                else {
                    listaCabecera2.Add(CrearExcelModelo("Fecha/Central"));
                }

                listaAnchoColumna.Add(20);

                foreach (CpaCentralDTO central in listaCentral)
                {
                    listaCabecera1.Add(CrearExcelModelo("S"));
                    if (conBarraCabecera)
                    {
                        listaCabecera2.Add(CrearExcelModelo(central.Equinomb.ToString()));
                        listaCabecera3.Add(CrearExcelModelo(central.Barrbarratransferencia));
                    }
                    else
                    {
                        listaCabecera2.Add(CrearExcelModelo(central.Equinomb.ToString()));
                    }

                    listaAnchoColumna.Add(25);
                }

                listaCabeceras[0] = listaCabecera1;
                listaCabeceras[1] = listaCabecera2;
                if (conBarraCabecera)
                {
                    listaCabeceras[2] = listaCabecera3;
                }
                #endregion

                #region Cuerpo Oculto
                List<string>[] listaRegistrosOcultos = new List<string>[numeroCabecerasOcultas];
                List<string> listaCuerpoOculto1 = new List<string> { };
                List<string> listaCuerpoOculto2 = new List<string> { };

                listaCuerpoOculto1.Add("Equicodi");
                listaCuerpoOculto2.Add("Emprcodi");

                foreach (CpaCentralDTO central in listaCentral)
                {
                    listaCuerpoOculto1.Add(central.Equicodi.ToString());
                    listaCuerpoOculto2.Add(central.Emprcodi.ToString());
                }
                listaRegistrosOcultos[0] = listaCuerpoOculto1;
                listaRegistrosOcultos[1] = listaCuerpoOculto2;

                CpaExcelCuerpo cuerpoOculto = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistrosOcultos,
                };
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontal = new List<string> { };
                List<string> listaTipo = new List<string> { };
                List<CpaExcelEstilo> listaEstilo = new List<CpaExcelEstilo> { };
                listaAlineaHorizontal.Add("center");
                listaTipo.Add("string");
                listaEstilo.Add(CrearExcelEstilo(null, true, System.Drawing.Color.White, "#2980B9", "#DADAD9"));
                foreach (CpaCentralDTO central in listaCentral)
                {
                    listaAlineaHorizontal.Add("right");
                    listaTipo.Add("double");
                }

                List<string> intervalosFormateados = new List<string>();
                intervalosFormateados = ObtenerIntervalosXMesConFormato(dCpaFecha);

                List<string>[] listaRegistros = new List<string>[intervalosFormateados.Count];

                for (int i = 0; i < intervalosFormateados.Count; i++)
                {
                    listaRegistros[i] = new List<string> { };
                    listaRegistros[i].Add(intervalosFormateados[i]);
                }

                CpaExcelCuerpo cuerpo = new CpaExcelCuerpo
                {
                    ListaRegistros = listaRegistros,
                    ListaAlineaHorizontal = listaAlineaHorizontal,
                    ListaTipo = listaTipo,
                    ListaEstilo = listaEstilo
                };
                #endregion

                #region Definir hoja Resumen
                List<CpaExcelHoja> listCpaExcelHoja = new List<CpaExcelHoja>();
                CpaExcelHoja excelHoja = new CpaExcelHoja
                {
                    NombreHoja = "Plantilla",
                    Titulo = titulo,
                    Subtitulo1 = subtitulo1,
                    //Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    CuerpoOculto = cuerpoOculto,
                    Cuerpo = cuerpo
                };
                listCpaExcelHoja.Add(excelHoja);
                #endregion

                #endregion

                return listCpaExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para el archivo Excel con respecto a la opción Descarga de los insumos
        /// </summary>
        /// <param name="cparcodi">Id de la revisión seleccionado</param>
        /// <param name="sTipoInsumo"></param>
        /// <param name="iMeses">Meses seleccionados para realizar la descarga</param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public List<CpaExcelHoja> GenerarArchivoExcelDescarga(int cparcodi, string sTipoInsumo, int[] iMeses, out string nombreArchivo)
        {
            try
            {
                #region Validaciones
                CpaRevisionDTO cpaRevisionDto = GetByIdCpaRevision(cparcodi);
                #endregion

                #region Obtener Insumos
                List<CpaInsumoDiaDTO> listInsumoDiaByAnio = new List<CpaInsumoDiaDTO>();
                ConcurrentDictionary<int, List<CpaInsumoDiaDTO>> cdListaInsumoDiaByMes = new ConcurrentDictionary<int, List<CpaInsumoDiaDTO>>();
                Parallel.ForEach(iMeses, new ParallelOptions { MaxDegreeOfParallelism = 3 }, mes =>
                {
                    DateTime dFecInicio = DateTime.ParseExact(string.Format("01/{0}/{1}", mes.ToString("D2"), cpaRevisionDto.Cpaapanioejercicio), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime dFecFin = dFecInicio.AddMonths(1).AddDays(-1);
                    List<CpaInsumoDiaDTO> listInsumoDiaByMes = FactoryTransferencia.GetCpaInsumoDiaRepository().ListByTipoInsumoByPeriodo(sTipoInsumo, cpaRevisionDto.Cparcodi, dFecInicio, dFecFin);
                    listInsumoDiaByAnio.AddRange(listInsumoDiaByMes);
                    cdListaInsumoDiaByMes.TryAdd(mes, listInsumoDiaByMes);
                });

                if (listInsumoDiaByAnio.Count < 1)
                {
                    throw new Exception("No se puede realizar la descarga debido a que no se encontraron registros en los meses seleccionados.");
                }
                listInsumoDiaByAnio = listInsumoDiaByAnio.OrderBy(x => x.Cpainddia).ToList();
                #endregion

                #region Variables
                bool conBarraCabecera = ContieneFilaNombreBarra(sTipoInsumo);
                int numeroCabeceras = conBarraCabecera ? 2 : 1;
                List<DateTime> listFechasByAnio = listInsumoDiaByAnio.GroupBy(x => x.Cpainddia).Select(x => x.Key).ToList();
                nombreArchivo = ConstantesCPPA.insumosNombArchivoDesc[Int32.Parse(sTipoInsumo) - 1] + "_" + cpaRevisionDto.Cpaapanioejercicio;
                #endregion

                #region Hojas Meses del Año Presupuestal (Año del Ejercicio)

                ConcurrentDictionary<int, CpaExcelHoja> cdExcelHoja = new ConcurrentDictionary<int, CpaExcelHoja>();
                Parallel.ForEach(iMeses, new ParallelOptions { MaxDegreeOfParallelism = 3 }, mes =>
                {
                    #region Hoja Mes

                    #region Obtener datos
                    List<CpaInsumoDiaDTO> listaCentral = cdListaInsumoDiaByMes[mes]
                                            .GroupBy(d => d.Equicodi)      
                                            .Select(g => g.First())        
                                            .OrderBy(d => d.Equinomb)      
                                            .ToList();

                    List<DateTime> listFechasByMes = listFechasByAnio.Where(x => x.Month == mes).ToList();
                    int diasMes = listFechasByMes.Count;
                    #endregion

                    #region Título
                    string tituloMes = ConstantesCPPA.insumosTituloDesc[Int32.Parse(sTipoInsumo) - 1] + " - " + cpaRevisionDto.Cpaapanioejercicio + "_" + ConstantesCPPA.mesesDesc[mes - 1];
                    string subtitulo1Mes = "Presupuesto " + cpaRevisionDto.Cpaapanio + " - " + cpaRevisionDto.Cpaapajuste + (cpaRevisionDto.Cparrevision == "Normal" ? "" : " - " + cpaRevisionDto.Cparrevision);
                    List<CpaExcelEstilo> listaEstilosTitulos = new List<CpaExcelEstilo> { 
                        CrearExcelEstilo(null, true, null, null, null, 12),
                        CrearExcelEstilo(null, true, null, null, null, 12)
                    };
                    string nombreHojaMes = cpaRevisionDto.Cpaapanioejercicio + "-" + ConstantesCPPA.mesesDescNum[mes - 1];
                    #endregion

                    #region Cabecera
                    List<CpaExcelModelo>[] listaCabecerasMes = new List<CpaExcelModelo>[numeroCabeceras];
                    List<CpaExcelModelo> listaCabecera1Mes = new List<CpaExcelModelo> { };
                    List<CpaExcelModelo> listaCabecera2Mes = new List<CpaExcelModelo> { };
                    List<int> listaAnchoColumnaMes = new List<int> { 20 };

                    if (conBarraCabecera)
                    {
                        listaCabecera1Mes.Add(CrearExcelModelo("Central"));
                        listaCabecera2Mes.Add(CrearExcelModelo("S/. /MWh"));
                    }
                    else
                    {
                        listaCabecera1Mes.Add(CrearExcelModelo("Fecha/Central"));
                    }

                    foreach (CpaInsumoDiaDTO central in listaCentral)
                    {
                        listaCabecera1Mes.Add(CrearExcelModelo(central.Equinomb.ToString()));
                        if (conBarraCabecera)
                        {
                            listaCabecera2Mes.Add(CrearExcelModelo(central.Barrbarratransferencia));
                        }
                        listaAnchoColumnaMes.Add(25);
                    }

                    listaCabecerasMes[0] = listaCabecera1Mes;
                    if (conBarraCabecera)
                    {
                        listaCabecerasMes[1] = listaCabecera2Mes;
                    }
                    #endregion

                    #region Cuerpo
                    List<string> listaAlineaHorizontalMes = new List<string> { "center" };
                    List<string> listaTipoMes = new List<string> { "string" };
                    List<CpaExcelEstilo> listaEstiloMes = new List<CpaExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };

                    foreach (CpaInsumoDiaDTO central in listaCentral)
                    {
                        listaAlineaHorizontalMes.Add("right");
                        listaTipoMes.Add("double");
                        listaEstiloMes.Add(CrearExcelEstilo("#,##0.0000"));
                    }

                    List<string>[] listaRegistrosByMes;
                    if (listaCentral.Count < 1 || diasMes < 1)
                    {
                        listaRegistrosByMes = new List<string>[1];
                        listaRegistrosByMes[0] = new List<string>() { "No existen datos" };
                    }
                    else
                    {
                        int j = 0;
                        int cantidadRegistroByMes = diasMes * ConstantesPrimasRER.numero96;
                        listaRegistrosByMes = new List<string>[cantidadRegistroByMes];
                        foreach (DateTime fecha in listFechasByMes)
                        {
                            List<CpaInsumoDiaDTO> listInsumoDiaByFecha = cdListaInsumoDiaByMes[mes].Where(x => x.Cpainddia == fecha).ToList();
                            for (int indexHora = 1; indexHora <= ConstantesPrimasRER.numero96; indexHora++)
                            {
                                listaRegistrosByMes[j] = ObtenerInsumoDiaPorFechaHora(fecha, indexHora, listaCentral, listInsumoDiaByFecha);
                                j++;
                            }
                        }
                    }

                    CpaExcelCuerpo cuerpoMes = new CpaExcelCuerpo
                    {
                        ListaRegistros = listaRegistrosByMes,
                        ListaAlineaHorizontal = listaAlineaHorizontalMes,
                        ListaTipo = listaTipoMes,
                        ListaEstilo = listaEstiloMes
                    };
                    #endregion

                    #region Definir hoja
                    CpaExcelHoja excelHojaMes = new CpaExcelHoja
                    {
                        NombreHoja = nombreHojaMes,
                        Titulo = tituloMes,
                        //TitulosEstandar = true,
                        Subtitulo1 = subtitulo1Mes,
                        ListaEstilosTitulos = listaEstilosTitulos,
                        ListaAnchoColumna = listaAnchoColumnaMes,
                        ListaCabeceras = listaCabecerasMes,
                        Cuerpo = cuerpoMes
                    };
                    cdExcelHoja.TryAdd(mes, excelHojaMes);
                    #endregion

                    #endregion
                });

                #region Return
                List<CpaExcelHoja> listExcelHoja = (from int mes in iMeses
                                                    where cdExcelHoja.ContainsKey(mes)
                                                    select cdExcelHoja[mes]).ToList();
                return listExcelHoja;
                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los datos de los insumos dia de las Centrales RER, para una fecha y hora especifica,
        /// en el orden especificado de la lista de centrales 'listCentral'
        /// </summary>
        /// <param name="rerinddiafecdia"></param>
        /// <param name="indexHora"></param>
        /// <param name="listCentral"></param>
        /// <param name="listInsumoDia">Todos los registros deben corresponder a un mismo rerindtipresultado</param>
        /// <returns></returns>
        private List<string> ObtenerInsumoDiaPorFechaHora(DateTime rerinddiafecdia, int indexHora, List<CpaInsumoDiaDTO> listCentral, List<CpaInsumoDiaDTO> listInsumoDia)
        {
            try
            {
                List<string> listRegistro = new List<string> { ObtenerFechaHora(rerinddiafecdia, indexHora) };
                foreach (CpaInsumoDiaDTO central in listCentral)
                {
                    List<CpaInsumoDiaDTO> listIDia = listInsumoDia.Where(x => x.Cpainddia == rerinddiafecdia && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                    if (listIDia.Count > 0)
                    {
                        var value = listIDia[0].GetType().GetProperty($"Cpaindh{indexHora}").GetValue(listIDia[0], null);
                        listRegistro.Add((value != null) ? value.ToString() : "");
                    }
                    else
                    {
                        listRegistro.Add("");
                    }
                }
                return listRegistro;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Arma una fecha y hora en base a los valores entregados
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="indexHora"></param>
        /// <returns></returns>
        private string ObtenerFechaHora(DateTime fecha, int indexHora)
        {
            int hora = 0;
            int minutos = indexHora * ConstantesCPPA.numero15;
            while (minutos >= ConstantesPrimasRER.numero60)
            {
                hora++;
                minutos -= ConstantesPrimasRER.numero60;
            }

            if (indexHora == ConstantesPrimasRER.numero96)
            {
                fecha = fecha.AddDays(1);
                hora = 0;
            }

            return string.Format("{0}/{1}/{2} {3}:{4}", fecha.Day.ToString("D2"), fecha.Month.ToString("D2"), fecha.Year, hora.ToString("D2"), minutos.ToString("D2"));
        }

        /// <summary>
        /// Obtiene intervalos de 15 minutos para un mes, por ejemplo: 01/05/2024 00:15, 01/05/2024 00:30, ...
        /// </summary>
        /// <param name="sDia">Dia de inicio del mes</param>
        public static List<string> ObtenerIntervalosXMesConFormato(DateTime sDia)
        {
            List<string> intervalosFormateados = new List<string>();

            DateTime inicio = new DateTime(sDia.Year, sDia.Month, sDia.Day, 0, 15, 0);

            while (inicio <= sDia.Date.AddMonths(1))
            {
                intervalosFormateados.Add(inicio.ToString("dd/MM/yyyy HH:mm"));
                inicio = inicio.AddMinutes(15);
            }

            return intervalosFormateados;
        }

        /// <summary>
        /// Obtiene el nombre del archivo Excel descargado para la carga manual
        /// </summary>
        /// <param name="sAnio">Año Presupuestal seleccionado</param>
        /// <param name="sMes">Año Presupuestal seleccionado</param>
        /// <param name="sOpcion"></param>
        public static string ObtenerNombreArchivoPlantillaInsumo(string sAnio, string sMes, string sOpcion) 
        {
            return ConstantesCPPA.insumosNombArchivoDesc[Int32.Parse(sOpcion) - 1] + "_" + (int.Parse(sAnio) - 1).ToString() + "_" + int.Parse(sMes).ToString("D2") + "-" + ConstantesCPPA.mesesDescCorta[int.Parse(sMes) - 1];            
        }

        /// <summary>
        /// Obtiene true si el insumo posee una fila extra con el nombre de de la barra
        /// </summary>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        public static bool ContieneFilaNombreBarra(string sTipoInsumo)
        {
            bool conBarraCabecera = false;
            if (sTipoInsumo == "2" || sTipoInsumo == "3")
            {
                conBarraCabecera = true;
            }
            return conBarraCabecera;
        }

        /// <summary>
        /// Valida los formatos de las celdas del archivo Excel importado.
        /// </summary>
        /// <param name="sAnio">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sNumMes">Mes del año tarifario seleccionado</param>
        /// <param name="sRutaArchivo">Ruta donde se descargará temporalmente el archivo</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <returns></returns>
        public List<string> ValidarExcelImportado15min(string sAnio, string sAjuste, string sRevision, string sNumMes, string sRutaArchivo, string sTipoInsumo)
        {
            #region Variables
            string sMensajeError = "";
            List<string> celdaErrores = new List<string>();
            #endregion

            try
            {
                #region Variables
                List<string> intervalosFormateados = new List<string>();    // Donde se alojarán los intervalos de 15 min: 01/01/2024 00:15, 01/01/2024 00:30, ...
                int desplazamiento = 0;
                List<string> sCeldaErrores = new List<string>();            // Lista de nombres de las celdas que posean errores de sintaxis
                List<string> sCeldaErroresSize = new List<string>();            // Lista de nombres de las celdas que posean errores por cantidad de decimales
                bool conBarraCabecera = ContieneFilaNombreBarra(sTipoInsumo);
                List<CpaCentralDTO> listaCentralImportada = new List<CpaCentralDTO>();
                int numColumCentrales = 0;                                  // Número de centrales encontradas en la hoja
                int columInicio = 2;
                int filaInicioOcula = conBarraCabecera ? 6 : 5;
                int filaInicioCuerpo = conBarraCabecera ? 8 : 7;                                  // Fila donde se encuentra el 1er intervalo 01/01/2024 00:15
                //string pattern = @"^\d{1,5}(\.\d{1,9})?$";
                string pattern = @"^\d{1,5}(\.\d*)?$";
                #endregion

                #region Validamos que exista la revisión
                if (sAnio == "" || sAjuste == "" || sRevision == "")
                {
                    sMensajeError = "Por favor, seleccione un año, ajuste y revisión para ver los datos";
                    throw new Exception(sMensajeError);
                }

                CpaRevisionDTO cpaRevisionDto = GetByIdCpaRevision(Int32.Parse(sRevision));
                if (cpaRevisionDto == null)
                {
                    sMensajeError = "No se encontro una revisión con los datos proporcionados en la Base de Datos.";
                    throw new Exception(sMensajeError);
                }
                #endregion

                DateTime dCpaFecha = DateTime.ParseExact(string.Format("01/{0}/{1}", int.Parse(sNumMes).ToString("D2"), (int.Parse(sAnio) - 1).ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                int cantidadIntervalos15minXMes = DateTime.DaysInMonth(dCpaFecha.Year, dCpaFecha.Month) * 24 * 60 / 15;
                string sNombreArchivo = ObtenerNombreArchivoPlantillaInsumo(sAnio, sNumMes, sTipoInsumo);
                string sRutaNombreArchivo = sRutaArchivo + sNombreArchivo;

                //Obtenemos la lista de centrales
                List<CpaCentralDTO> listaCentral = ListCpaCentralByRevision(cpaRevisionDto.Cparcodi);

                //Obtenemos todos los intervalos de 15 min.
                intervalosFormateados = ObtenerIntervalosXMesConFormato(dCpaFecha);

                #region Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = GeneraDataset(sRutaNombreArchivo, 1);

                int numColumnasXAnalizar = listaCentral.Count;              // Número de columnas que deberian existir
                int numFilasXAnalizar = intervalosFormateados.Count;        // Número de filas que deberian existir

                int numColumnasHoja = ds.Tables[0].Columns.Count;           // Número de columnas encontradas en el archivo
                int numFilasHoja = ds.Tables[0].Rows.Count;                 // Número de filas encontradas en el archivo
                #endregion

                #region Validación de fechas con formato dd/MM/yyyy HH:mm de la columna B del archivo excel
                for (int f = filaInicioCuerpo; f < numFilasXAnalizar + filaInicioCuerpo; f++)
                {
                    string fechaExcel = ds.Tables[0].Rows[f][1].ToString();
                    string fechaEsperada = intervalosFormateados[f- filaInicioCuerpo];
                    if (fechaExcel != fechaEsperada)
                    {
                        throw new Exception("La fecha esperada en la celda 'B" + (f + 2).ToString() + "' es " + fechaEsperada + " y se obtuvo " + fechaExcel + ". Por favor, no manipular las fechas de la columna B. Volver a descargar la plantilla.");
                    }
                }
                #endregion

                #region Obtenemos el número de columnas a analizar
                for (int c = 2; c < numColumnasHoja; c++)
                {
                    string filaOcultaCentral = ds.Tables[0].Rows[filaInicioOcula][c].ToString();
                    string filaOcultaEmpresa = ds.Tables[0].Rows[filaInicioOcula + 1][c].ToString();

                    CpaCentralDTO cpaCentralImportada = new CpaCentralDTO();
                    cpaCentralImportada.Equicodi = Int32.Parse(filaOcultaCentral);
                    cpaCentralImportada.Emprcodi = Int32.Parse(filaOcultaEmpresa);

                    listaCentralImportada.Add(cpaCentralImportada);
                    if (!string.IsNullOrEmpty(filaOcultaEmpresa) && !string.IsNullOrEmpty(filaOcultaCentral))
                    {
                        numColumCentrales++;
                    }
                }
                #endregion

                #region Validación de no duplicados en la listaCentralImportada
                bool existeCodEntregaDuplicados = listaCentralImportada.GroupBy(e => new { e.Emprcodi, e.Equicodi }).Any(group => group.Count() > 1);
                if (existeCodEntregaDuplicados)
                {
                    throw new Exception("Por favor, no duplicar columnas en el archivo Excel. Descargue nuevamente la plantilla.");
                }
                #endregion

                #region Validacion de los datos ingresados
                for (int j = columInicio; j < numColumCentrales + columInicio; j++)        // Recorro las centrales (columnas)
                {
                    string sEquicodi = ds.Tables[0].Rows[filaInicioOcula][j].ToString().Trim();
                    string sEmprcodi = ds.Tables[0].Rows[filaInicioOcula + 1][j].ToString().Trim();

                    #region Validamos que no este vacio ni contenga letras en las filas ocultas
                    if (string.IsNullOrEmpty(sEmprcodi) || !int.TryParse(sEmprcodi, out int emprcodi))
                    {
                        throw new Exception("Por favor, no manipular las filas " + filaInicioOcula.ToString() + " y " + (filaInicioOcula + 1).ToString() + ". Descargar nuevamente la plantilla excel.");
                    }

                    if (string.IsNullOrEmpty(sEquicodi) || !int.TryParse(sEquicodi, out int equicodi))
                    {
                        throw new Exception("Por favor, no manipular las filas " + filaInicioOcula.ToString() + " y " + (filaInicioOcula + 1).ToString() + ". Descargar nuevamente la plantilla excel.");
                    }
                    #endregion

                    #region Validamos que el equicodi se encuentren en listaCentral
                    CpaCentralDTO centralValida = listaCentral.Where(item => item.Emprcodi == emprcodi && item.Equicodi == equicodi).ToList().FirstOrDefault();
                    if (centralValida == null)
                    {
                        throw new Exception("La relación de empresa y central en la columna '" + ConvertirNumeroColumnaALetra(j + 1) + "' no se encuentran vigentes en el mes seleccionado. Por favor, descargar nuevamente la plantilla excel.");
                    }
                    #endregion

                    for (int i = filaInicioCuerpo; i < numFilasXAnalizar + filaInicioCuerpo; i++)        // Recorro los intervalos (filas)
                    {
                        string sSeImporta = ds.Tables[0].Rows[3][j].ToString().Trim();          // Celda donde se encuentra el valor de 'S'
                        string sValorCelda = ds.Tables[0].Rows[i][j].ToString().Trim();
                        decimal dValorIntervalo = 0;
                        if (sSeImporta == "S")
                        {
                            #region Validamos que el Valor total sea un valor decimal
                            if (sValorCelda == "null" || sValorCelda == "")
                            {
                                sCeldaErrores.Add(ConvertirNumeroColumnaALetra(j + 1) + (i + 2).ToString());
                            }
                            else if (!decimal.TryParse(sValorCelda, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out dValorIntervalo))
                            {
                                sCeldaErrores.Add(ConvertirNumeroColumnaALetra(j + 1) + (i + 2).ToString());
                            }
                            else
                            {
                                sValorCelda = dValorIntervalo.ToString("0.########");
                                if (decimal.Parse(sValorCelda) < 0)
                                {
                                    sCeldaErrores.Add(ConvertirNumeroColumnaALetra(j + 1) + (i + 2).ToString());
                                }
                            }
                            #endregion

                            #region Valida la cantidad de enteros y decimales
                            bool isValid = Regex.IsMatch(sValorCelda, pattern);
                            if (!isValid)
                            {
                                sCeldaErroresSize.Add(ConvertirNumeroColumnaALetra(j + 1) + (i + 2).ToString());
                            }
                            #endregion
                        }
                    }
                }
                #endregion

                #region En caso que existan errores, lo muestro en pantalla
                if (sCeldaErrores.Count > 0)
                {
                    celdaErrores = sCeldaErrores.Select(numero => "'" + (numero) + "'").ToList();

                    string sCeldasError = string.Join(", ", celdaErrores);
                    sCeldasError = sCeldasError.Length > 50 ? sCeldasError.Substring(0, 50) + "..." : sCeldasError;
                    throw new Exception("La(s) celdas(s) " + sCeldasError + " ingresada(s) no posee(n) un valor en decimal mayor o igual a cero. Por favor, corregir estos valores.");
                }
                celdaErrores = new List<string>();

                if (sCeldaErroresSize.Count > 0)
                {
                    celdaErrores = sCeldaErroresSize.Select(numero => "'" + (numero) + "'").ToList();

                    string sCeldasErrorSize = string.Join(", ", celdaErrores);
                    sCeldasErrorSize = sCeldasErrorSize.Length > 50 ? sCeldasErrorSize.Substring(0, 50) + "..." : sCeldasErrorSize;
                    throw new Exception("La(s) celdas(s) " + sCeldasErrorSize + " ingresada(s) posee(n) más de 5 dígitos enteros. Por favor, corregir estos valores.");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            return celdaErrores;
        }

        public static List<string> ObtenerIntervalosConFormato(DateTime dia)
        {
            List<string> intervalosFormateados = new List<string>();

            DateTime inicio = new DateTime(dia.Year, dia.Month, dia.Day, 0, 15, 0);

            while (inicio <= dia.Date.AddDays(1))
            {
                intervalosFormateados.Add(inicio.ToString("d/MM/yyyy HH:mm"));
                inicio = inicio.AddMinutes(15);
            }

            return intervalosFormateados;
        }

        public static List<string> ObtenerIntervalosMinutosXDiaConFormato(DateTime dia)
        {
            List<string> intervalosFormateados = new List<string>();

            DateTime inicio = new DateTime(dia.Year, dia.Month, dia.Day, 0, 15, 0);

            while (inicio < dia.Date.AddDays(1))
            {
                intervalosFormateados.Add(inicio.ToString("HH:mm"));
                inicio = inicio.AddMinutes(15);
            }
            intervalosFormateados.Add("24:00");
            return intervalosFormateados;
        }


        #endregion

        #region Procesar archivos
        /// <summary>
        /// Almacena en la DB los registros de los archivos excel importados de los meses seleccionados en los insumos
        /// </summary>
        /// <param name="sAnio">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <param name="iMeses">Meses que se seleccionaron para realizar la importación</param>
        /// <param name="sUser">Usuario que realiza la importación</param>
        /// <returns></returns>
        public void procesarTodosArchivosInsumo15min(string sAnio, string sAjuste, string sRevision, string sTipoInsumo, int[] iMeses, string sUser)
        {
            try
            {
                string sMensajeError = "";
                #region Valido si existe la revisión
                if (sAnio == "" || sAjuste == "" || sRevision == "")
                {
                    sMensajeError = "Por favor, seleccione un año, ajuste y revisión para ver los datos";
                    throw new Exception(sMensajeError);
                }

                CpaRevisionDTO cpaRevisionDto = GetByIdCpaRevision(Int32.Parse(sRevision));
                if (cpaRevisionDto == null)
                {
                    sMensajeError = "No se encontro una revisión con los datos proporcionados en la Base de Datos.";
                    throw new Exception(sMensajeError);
                }
                #endregion

                #region Variables generales
                List<string> sMesesNoImportados = new List<string>();
                string sRutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                string sNombreArchivo = "";
                List<string> listaCeldaErrores = new List<string>();
                List<CpaCentralDTO> listaCentralImportada = new List<CpaCentralDTO>();
                int numColumCentrales = 0;                                  // Número de centrales encontradas en la hoja
                bool conBarraCabecera = ContieneFilaNombreBarra(sTipoInsumo);
                List<int> columnasConS = new List<int>();
                int filaInicioCuerpo = conBarraCabecera ? 7 : 6;                                  // Fila donde se encuentra el 1er intervalo 01/01/2024 00:15
                int filaInicioOcula = conBarraCabecera ? 6 : 5;
                #endregion

                #region Validar los archivos de los meses seleccionados
                foreach (int iMesAValidar in iMeses)
                {
                    // Compruebo que exista los archivos de los meses seleccionados
                    sNombreArchivo = ObtenerNombreArchivoPlantillaInsumo(sAnio, iMesAValidar.ToString("D2"), sTipoInsumo);

                    if (!System.IO.File.Exists(sRutaArchivo + sNombreArchivo))
                    {
                        sMesesNoImportados.Add("'" + ConstantesCPPA.mesesDesc[iMesAValidar - 1] + "'");
                    }
                    else { 
                        listaCeldaErrores = ValidarExcelImportado15min(sAnio, sAjuste, sRevision, iMesAValidar.ToString(), sRutaArchivo, sTipoInsumo);
                        
                        if (listaCeldaErrores.Count > 0)
                        {
                            List<string> celdaErrores = listaCeldaErrores.Select(numero => "'" + (numero) + "'").ToList();

                            string sCeldasError = string.Join(", ", celdaErrores);
                            throw new Exception("La(s) celdas(s) " + sCeldasError + " ingresada(s) no posee(n) un valor decimal.");
                        }
                    }
                }

                if (sMesesNoImportados.Count > 1)
                {
                    throw new Exception("Los archivos Excel de los meses de " + string.Join(", ", sMesesNoImportados) + " no se cargaron previamente.");
                }
                else if(sMesesNoImportados.Count == 1) {
                    throw new Exception("El archivo Excel del mes de " + string.Join(", ", sMesesNoImportados) + " no se cargaró previamente.");
                }
                #endregion

                #region Variables del método
                string sResultado = "";
                List<string> sCeldaErrores = new List<string>();

                //Para cpa_insumo
                int iCpaInsCodi = 0;
                string sCpaTipoProceso = "M";
                string sLog = "Se inició la importación manual del insumo. <br>";
                List<string> logList = new List<string>();
                #endregion

                #region Crea el CPA_INSUMO
                //Asignamos el correlativo de la tabla CPA_INSUMO_DIA
                List<CpaInsumoDiaDTO> listaCpaInsumoDia = new List<CpaInsumoDiaDTO>();
                #endregion

                // Creamos el CPA_INSUMO
                int iCpainscodi = InsertarInsumo(cpaRevisionDto.Cparcodi, sTipoInsumo, sCpaTipoProceso, sLog, sUser);
                int iCpaindcodi = FactoryTransferencia.GetCpaInsumoDiaRepository().GetMaxId();

                #region De los meses seleccionados, obtenemos sus respectivos archivos Excels
                foreach (int iMes in iMeses)
                {
                    decimal dTotalMes = 0;
                    decimal dTotalDia = 0;

                    #region Obtenemos la cantidad de intervalos de 15 minutos que existen en el mes
                    DateTime dCpaFecha = DateTime.ParseExact(string.Format("01/{0}/{1}", iMes.ToString("D2"), sAnio), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    int cantidadDias = DateTime.DaysInMonth(Int32.Parse(sAnio) - 1, iMes);
                    int cantidadIntervalos15minXMes = cantidadDias * 24 * 60 / 15;
                    #endregion

                    // Obtenemos las centrales
                    List<CpaCentralDTO> listaCentral = ListCpaCentralByRevision(cpaRevisionDto.Cparcodi);

                    //Traemos la primera hoja del archivo
                    sNombreArchivo = ObtenerNombreArchivoPlantillaInsumo(sAnio, iMes.ToString("D2"), sTipoInsumo);

                    DataSet ds = new DataSet();
                    ds = GeneraDataset(sRutaArchivo + sNombreArchivo, 1);

                    int numColumnasXAnalizar = listaCentral.Count;
                    int numFilasXAnalizar = cantidadIntervalos15minXMes;

                    int numColumnasHoja = ds.Tables[0].Columns.Count;           // Número de columnas encontradas en el archivo
                    int numFilasHoja = ds.Tables[0].Rows.Count;                 // Número de filas encontradas en el archivo
                    numColumCentrales = 0;

                    #region Obtenemos el número de columnas a analizar
                    for (int c = 2 ; c < numColumnasHoja; c++)
                    {
                        string filaOcultaCentral = ds.Tables[0].Rows[filaInicioOcula][c].ToString();
                        string filaOcultaEmpresa = ds.Tables[0].Rows[filaInicioOcula + 1][c].ToString();

                        CpaCentralDTO cpaCentralImportada = new CpaCentralDTO();
                        cpaCentralImportada.Equicodi = Int32.Parse(filaOcultaCentral);
                        cpaCentralImportada.Emprcodi = Int32.Parse(filaOcultaEmpresa);

                        listaCentralImportada.Add(cpaCentralImportada);
                        numColumCentrales++;
                    }
                    #endregion

                    #region Recorro cada columna de las centrales importadas
                    for (int j = 2; j < numColumCentrales + 2; j++)        // Recorro las centrales (columnas)
                    {
                        string valorS = ds.Tables[0].Rows[3][j].ToString().Trim();
                        if (valorS != "S") {
                            continue;
                        }
                        // Obtenemos el valor de cpainmcodi en caso exista
                        CpaInsumoMesDTO cpaInsumoMesDTO = GetByCriteriaCpaInsumoMes(cpaRevisionDto.Cparcodi, listaCentralImportada[j - 2].Emprcodi, listaCentralImportada[j - 2].Equicodi, sTipoInsumo, iMes);

                        //En caso que exista una central en otro insumo dia o mes, 
                        DeleteCpaInsumoDiaByCentral(cpaRevisionDto.Cparcodi, listaCentralImportada[j - 2].Equicodi, sTipoInsumo, cpaInsumoMesDTO.Cpainmcodi);
                        DeleteCpaInsumoMesByCentral(cpaRevisionDto.Cparcodi, listaCentralImportada[j - 2].Equicodi, sTipoInsumo, cpaInsumoMesDTO.Cpainmcodi);

                        #region Crea el cpa_insumo_mes para una empresa-central
                        int iCpaInsMesCodi = InsertarInsumoMes(iCpainscodi, cpaRevisionDto.Cparcodi, listaCentralImportada[j - 2].Emprcodi, listaCentralImportada[j - 2].Equicodi, sTipoInsumo, sCpaTipoProceso, iMes, 0,  sUser);
                        #endregion

                        dTotalMes = 0;
                        for (int iDia = 1; iDia <= cantidadDias; iDia++)
                        {
                            dTotalDia = 0;

                            //Crea un CPA_INSUMO_DIA_DTO
                            CpaInsumoDiaDTO dtoInsumoDia = new CpaInsumoDiaDTO
                            {
                                Cpainmcodi = iCpaInsMesCodi,
                                Cparcodi = cpaRevisionDto.Cparcodi,
                                Emprcodi = listaCentralImportada[j - 2].Emprcodi,
                                Equicodi = listaCentralImportada[j - 2].Equicodi,
                                Cpaindtipinsumo = sTipoInsumo,
                                Cpaindtipproceso = sCpaTipoProceso,
                                Cpaindtotaldia = 0,
                                Cpainddia = DateTime.ParseExact(string.Format("{0}/{1}/{2}", iDia.ToString("D2"), iMes.ToString("D2"), (int.Parse(sAnio)-1).ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                                Cpaindusucreacion = sUser,
                                Cpaindfeccreacion = DateTime.Now
                            };

                            #region Almacena los valores de H de dtoInsumoDia
                            for (int i = 1; i <= 96; i++)
                            {
                                string sValorIntervalo = ds.Tables[0].Rows[(i + filaInicioCuerpo) + (iDia - 1) * 96][j].ToString().Trim();
                                decimal.TryParse(sValorIntervalo, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out decimal dValorIntervalo);
                                dValorIntervalo = Math.Round(dValorIntervalo, 9);           //Si existen mayor cantidad de decimales, lo redondeamos a 9 decimales
                                dtoInsumoDia.GetType().GetProperty($"Cpaindh{i}").SetValue(dtoInsumoDia, dValorIntervalo);

                                dTotalDia += dValorIntervalo;
                            }

                            dtoInsumoDia.Cpaindtotaldia = dTotalDia;
                            #endregion

                            #region BULKINSERT en cpa_insumo_dia
                            //APILAMOS PARA EL BULKINSERT
                            dtoInsumoDia.Cpaindcodi = iCpaindcodi++;
                            listaCpaInsumoDia.Add(dtoInsumoDia);
                            #endregion

                            #region Insert individual en cpa_insumo_dia
                            //SaveCpaInsumoDia(dtoInsumoDia);
                            #endregion
                            dTotalMes += dTotalDia;
                        }

                        #region Actualizamos el Rerinsmtotal en rer_insumo_mes
                        CpaInsumoMesDTO insumoMes = GetByIdCpaInsumoMes(iCpaInsMesCodi);
                        insumoMes.Cpainmtotal = dTotalMes;
                        UpdateCpaInsumoMes(insumoMes);
                        #endregion

                    }
                    #endregion
                    logList.Add(ConstantesCPPA.mesesDesc[iMes - 1]);
                }
                #endregion
                sLog = sLog + "Los meses de: " + string.Join(", ", logList) + " se cargaron correctamente. <br>";

                #region Subimos todos los registros de CPA_INSUMO_DIA por BulkInsert
                if (listaCpaInsumoDia.Count > 0)
                {
                    FactoryTransferencia.GetCpaInsumoDiaRepository().BulkInsertCpaInsumoDia(listaCpaInsumoDia);
                }
                #endregion

                #region Actualizar el log de RER_INSUMO
                CpaInsumoDTO dtoInsumo = GetByIdCpaInsumo(iCpainscodi);
                sLog = sLog + "Se finalizó la importación manual del insumo.";
                dtoInsumo.Cpainslog = sLog;
                UpdateCpaInsumo(dtoInsumo);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        #endregion

        #region Validación si existe centrales

        /// <summary>
        /// Retorna "S" si existen centrales en la revision seleccionada
        /// </summary>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <returns></returns>
        public string ValidacionCentrales(string sRevision)
        {
            string validacion = "N";
            List<CpaCentralDTO> listaCentral = ListCpaCentralByRevision(int.Parse(sRevision));
            if (listaCentral.Count > 0)
            {
                validacion = "S";
            }
            return validacion;
        }

        /// <summary>
        /// Retorna TRUE si ya se proceso calculo anteriormente en la revisión seleccionada
        /// </summary>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <returns></returns>
        public bool ValidarSiSeProcesoCalculo(string sRevision)
        {
            bool validacion = false;
            return validacion;
        }
        #endregion

        #endregion

        #region Exportación archivo Excel
        /// <summary>
        /// Almacena un archivo en excel en un data set
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="Hoja"></param>
        public DataSet GeneraDataset(string RutaArchivo, int Hoja)
        {
            return UtilCPPA.GeneraDataset(RutaArchivo, Hoja);
        }

        /// <summary>
        /// Genera un objeto CpaExcelModelo (puede ser cabecera o pie)
        /// </summary>
        /// <param name="nombre">Valor de la celda</param>
        /// <param name="alineaHoriz">Alineación de la celda</param>
        /// <param name="numColumnas">Número de columnas de la celda</param>
        /// <param name="numFilas">Número de filas de la cenda</param>
        /// <returns>CpaExcelModelo</returns>
        public CpaExcelModelo CrearExcelModelo(string nombre, string alineaHoriz = "center", int numColumnas = 1, int numFilas = 1)
        {
            CpaExcelModelo modelo = new CpaExcelModelo
            {
                Nombre = nombre,
                AlineaHorizontal = alineaHoriz,
                NumColumnas = numColumnas,
                NumFilas = numFilas
            };
            return modelo;
        }

        /// <summary>
        /// Genera un objeto CpaExcelEstilo
        /// </summary>
        /// <param name="numberformatFormat">Formato numérico. Ej: #,##0.0000</param>
        /// <param name="fontBold">Aplicar negrita al texto. Ej: true</param>
        /// <param name="fontColor">Color del texto. Ej: Color.White</param>
        /// <param name="fillBackgroundColor">Color del fondo. Ej: #2980B9</param>
        /// <param name="borderColor">Color del borde. Ej: #DADAD9</param>
        /// <param name="listRangoFilas">Lista de rango de filas en donde se aplicará el estilo. Si es nulo se aplica a toda la columna. Ej: new List<string> { "0,0", "1,3" } . Donde <filaInicial, filaFinal></param>
        /// <param name="listEstilo">Lista de estilos para filas específicas. Ej: new List<CpaExcelEstilo> { CrearExcelEstilo(null, null, null, "#ffff14", "#DADAD9", new List<string> { "0,0" }, null) })</param>
        /// <returns>CpaExcelModelo</returns>
        public CpaExcelEstilo CrearExcelEstilo(string numberformatFormat = null, bool? fontBold = null, System.Drawing.Color? fontColor = null, string fillBackgroundColor = null, string borderColor = null, float? fontSize = null, List<string> listRangoFilas = null, List<CpaExcelEstilo> listEstilo = null)
        {
            CpaExcelEstilo estilo = new CpaExcelEstilo
            {
                NumberformatFormat = numberformatFormat,
                FontBold = fontBold,
                FontColor = fontColor,
                FillBackgroundColor = fillBackgroundColor,
                BorderColor = borderColor,
                FontSize = fontSize,
                ListaRangoFilas = listRangoFilas,
                ListaEstilo = listEstilo
            };
            return estilo;
        }

        /// <summary>
        /// Genera un objeto CpaExcelCuerpo
        /// </summary>
        /// <param name="listaRegistros">Matriz con el contenido del cuerpo del reporte</param>
        /// <param name="listaAlineaHorizontal">Lista con la alineación de las celdas</param>
        /// <param name="listaTipo">Lista con el tipo de dato de las celdas</param>
        /// <returns>CpaExcelCuerpo</returns>
        public CpaExcelCuerpo CrearExcelCuerpo(List<string>[] listaRegistros, List<string> listaAlineaHorizontal, List<string> listaTipo)
        {
            CpaExcelCuerpo modelo = new CpaExcelCuerpo
            {
                ListaRegistros = listaRegistros,
                ListaAlineaHorizontal = listaAlineaHorizontal,
                ListaTipo = listaTipo
            };
            return modelo;
        }

        /// <summary>
        /// Genera un reporte a excel(.xlsx)
        /// </summary>
        /// <param name="listaExcelHoja">Contenido del archivo a exportar, definido por hojas excel</param>
        /// <param name="rutaArchivo">Ruta donde se generará el reporte</param>
        /// <param name="nombreArchivo">Nombre del reporte. La aplicación colocará al final la extension: .xlsx</param>
        /// <param name="mostrarLogoTitulo">Bool que indica si el excel tendrá logo y titulo</param>
        /// <returns>Retorna el nombre del reporte generado. Nota: En caso de haber error devuelve -1</returns>
        public string ExportarReporteaExcel(List<CpaExcelHoja> listaExcelHoja, string rutaArchivo, string nombreArchivo, bool mostrarLogoTitulo)
        {
            string Reporte = nombreArchivo + ".xlsx";

            try
            {
                ExcelDocumentCPPA.ExportarReporte(listaExcelHoja, rutaArchivo + Reporte, mostrarLogoTitulo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ConstantesCPPA.anInternalApplicationErrorHasOccurred, ex);
            }

            return Reporte;
        }

        /// <summary>
        /// Convierte de un numero de columna, a una letra de columna en formato Excel
        /// </summary>
        /// <param name="numeroColumna">Número al que se desea convertir en letra segun las columnas de un archivo Excel</param>
        public string ConvertirNumeroColumnaALetra(int numeroColumna)
        {
            string letraColumna = "";
            while (numeroColumna > 0)
            {
                int modulo = (numeroColumna - 1) % 26;
                char letra = (char)('A' + modulo);
                letraColumna = letra + letraColumna;
                numeroColumna = (numeroColumna - 1) / 26;
            }
            return letraColumna;
        }
        #endregion

        #endregion

        #region Métodos CRUD de tablas CPPA

        #region CPA_AJUSTEPRESUPUESTAL

        /// <summary>
        /// Inserta un registro de la tabla CPA_AJUSTEPRESUPUESTAL
        /// </summary>
        public void SaveCpaAjustePresupuestal(CpaAjustePresupuestalDTO entity)
            {
                try
                {
                    FactoryTransferencia.GetCpaAjustePresupuestalRepository().Save(entity);
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }

            /// <summary>
            /// Actualiza un registro de la tabla CPA_AJUSTEPRESUPUESTAL
            /// </summary>
            public void UpdateCpaAjustePresupuestal(CpaAjustePresupuestalDTO entity)
            {
                try
                {
                    FactoryTransferencia.GetCpaAjustePresupuestalRepository().Update(entity);
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }

            /// <summary>
            /// Elimina un registro de la tabla CPA_AJUSTEPRESUPUESTAL
            /// <param name="cpaAjusPresId">Identificador de la tabla CPA_AJUSTEPRESUPUESTAL</param>
            /// </summary>
            public void DeleteCpaAjustePresupuestal(int cpaAjusPresId)
            {
                try
                {
                    FactoryTransferencia.GetCpaAjustePresupuestalRepository().Delete(cpaAjusPresId);
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }

            /// <summary>
            /// Permite obtener un registro de la tabla CPA_AJUSTEPRESUPUESTAL
            /// <param name="cpaAjusPresId">Identificador de la tabla CPA_AJUSTEPRESUPUESTAL</param>
            /// </summary>
            public CpaAjustePresupuestalDTO GetByIdCpaAjustePresupuestal(int cpaAjusPresId)
            {
                return FactoryTransferencia.GetCpaAjustePresupuestalRepository().GetById(cpaAjusPresId);
            }

            /// <summary>
            /// Permite listar todos los registros de la tabla CPA_AJUSTEPRESUPUESTAL
            /// </summary>
            public List<CpaAjustePresupuestalDTO> ListCpaAjustePresupuestal()
            {
                return FactoryTransferencia.GetCpaAjustePresupuestalRepository().List();
            }
        #endregion

        #region CPA_CALCULO_CENTRAL
        /// <summary>
        /// Inserta un registro de la tabla CPA_CALCULO_CENTRAL
        /// </summary>
        public void SaveCpaCalculoCentral(CpaCalculoCentralDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCalculoCentralRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_CALCULO_CENTRAL
        /// </summary>
        public void UpdateCpaCalculoCentral(CpaCalculoCentralDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCalculoCentralRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_CALCULO_CENTRAL
        /// <param name="cpaCalculoCentralId">Identificador de la tabla CPA_CALCULO_CENTRAL</param>
        /// </summary>
        public void DeleteCpaCalculoCentral(int cpaCalculoCentralId)
        {
            try
            {
                FactoryTransferencia.GetCpaCalculoCentralRepository().Delete(cpaCalculoCentralId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_CALCULO_CENTRAL
        /// <param name="cpaCalculoCentralId">Identificador de la tabla CPA_CALCULO_CENTRAL</param>
        /// </summary>
        public CpaCalculoCentralDTO GetByIdCpaCalculoCentral(int cpaCalculoCentralId)
        {
            return FactoryTransferencia.GetCpaCalculoCentralRepository().GetById(cpaCalculoCentralId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_CALCULO_CENTRAL
        /// </summary>
        public List<CpaCalculoCentralDTO> ListCpaCalculoCentral()
        {
            return FactoryTransferencia.GetCpaCalculoCentralRepository().List();
        }

        #endregion

        #region CPA_CALCULO_EMPRESA
        /// <summary>
        /// Inserta un registro de la tabla CPA_CALCULO_EMPRESA
        /// </summary>
        public void SaveCpaCalculoEmpresa(CpaCalculoEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCalculoEmpresaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_CALCULO_EMPRESA
        /// </summary>
        public void UpdateCpaCalculoEmpresa(CpaCalculoEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCalculoEmpresaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_CALCULO_EMPRESA
        /// <param name="cpaCalculoEmpresaId">Identificador de la tabla CPA_CALCULO_EMPRESA</param>
        /// </summary>
        public void DeleteCpaCalculoEmpresa(int cpaCalculoEmpresaId)
        {
            try
            {
                FactoryTransferencia.GetCpaCalculoEmpresaRepository().Delete(cpaCalculoEmpresaId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_CALCULO_EMPRESA
        /// <param name="cpaCalculoEmpresaId">Identificador de la tabla CPA_CALCULO_EMPRESA</param>
        /// </summary>
        public CpaCalculoEmpresaDTO GetByIdCpaCalculoEmpresa(int cpaCalculoEmpresaId)
        {
            return FactoryTransferencia.GetCpaCalculoEmpresaRepository().GetById(cpaCalculoEmpresaId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_CALCULO_EMPRESA
        /// </summary>
        public List<CpaCalculoEmpresaDTO> ListCpaCalculoEmpresa()
        {
            return FactoryTransferencia.GetCpaCalculoEmpresaRepository().List();
        }

        #endregion

        #region CPA_CALCULO
        /// <summary>
        /// Inserta un registro de la tabla CPA_CALCULO
        /// </summary>
        public void SaveCpaCalculo(CpaCalculoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCalculoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_CALCULO
        /// </summary>
        public void UpdateCpaCalculo(CpaCalculoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCalculoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_CALCULO
        /// <param name="cpaCalculoId">Identificador de la tabla CPA_CALCULO</param>
        /// </summary>
        public void DeleteCpaCalculo(int cpaCalculoId)
        {
            try
            {
                FactoryTransferencia.GetCpaCalculoRepository().Delete(cpaCalculoId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_CALCULO
        /// <param name="cpaCalculoId">Identificador de la tabla CPA_CALCULO</param>
        /// </summary>
        public CpaCalculoDTO GetByIdCpaCalculo(int cpaCalculoId)
        {
            return FactoryTransferencia.GetCpaCalculoRepository().GetById(cpaCalculoId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_CALCULO
        /// </summary>
        public List<CpaCalculoDTO> ListCpaCalculo()
        {
            return FactoryTransferencia.GetCpaCalculoRepository().List();
        }
        public CpaCalculoDTO GetByCriteriaCpaCalculo(int cpaRevision)
        {
            return FactoryTransferencia.GetCpaCalculoRepository().GetByCriteria(cpaRevision);

        }
        #endregion

        #region CPA_CENTRAL_PMPO
        /// <summary>
        /// Inserta un registro de la tabla CPA_CENTRAL_PMPO
        /// </summary>
        public void SaveCpaCentralPmpo(CpaCentralPmpoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCentralPmpoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_CENTRAL_PMPO
        /// </summary>
        public void UpdateCpaCentralPmpo(CpaCentralPmpoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCentralPmpoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_CENTRAL_PMPO
        /// <param name="cpaCentralPmpoId">Identificador de la tabla CPA_CENTRAL_PMPO</param>
        /// </summary>
        public void DeleteCpaCentralPmpo(int cpaCentralPmpoId)
        {
            try
            {
                FactoryTransferencia.GetCpaCentralPmpoRepository().Delete(cpaCentralPmpoId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_CENTRAL_PMPO
        /// <param name="cpaCentralPmpoId">Identificador de la tabla CPA_CENTRAL_PMPO</param>
        /// </summary>
        public CpaCentralPmpoDTO GetByIdCpaCentralPmpo(int cpaCentralPmpoId)
        {
            return FactoryTransferencia.GetCpaCentralPmpoRepository().GetById(cpaCentralPmpoId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_CENTRAL_PMPO
        /// </summary>
        /// <param name="cpacntcodi">Identificador de la tabla CPA_CENTRAL</param>
        public List<CpaCentralPmpoDTO> ListCpaCentralPmpo(int cpacntcodi)
        {
            return FactoryTransferencia.GetCpaCentralPmpoRepository().List(cpacntcodi);
        }

        #endregion

        #region CPA_CENTRAL
        /// <summary>
        /// Inserta un registro de la tabla CPA_CENTRAL
        /// </summary>
        public int SaveCpaCentral(CpaCentralDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetCpaCentralRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_CENTRAL
        /// </summary>
        public void UpdateCpaCentral(CpaCentralDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaCentralRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_CENTRAL
        /// <param name="cpaCentralId">Identificador de la tabla CPA_CENTRAL</param>
        /// </summary>
        public void DeleteCpaCentral(int cpaCentralId)
        {
            try
            {
                FactoryTransferencia.GetCpaCentralRepository().Delete(cpaCentralId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_CENTRAL
        /// <param name="cpaCentralId">Identificador de la tabla CPA_CENTRAL</param>
        /// </summary>
        public CpaCentralDTO GetByIdCpaCentral(int cpaCentralId)
        {
            return FactoryTransferencia.GetCpaCentralRepository().GetById(cpaCentralId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_CENTRAL
        /// </summary>
        public List<CpaCentralDTO> ListCpaCentral()
        {
            return FactoryTransferencia.GetCpaCentralRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_CENTRAL por Revisión
        /// <param name="cparcodi">Identificador de la tabla CPA_CENTRAL</param>
        /// </summary>
        public List<CpaCentralDTO> ListCpaCentralByRevision(int cparcodi)
        {
            return FactoryTransferencia.GetCpaCentralRepository().ListByRevision(cparcodi);
        }
        #endregion

        #region CPA_EMPRESA
        /// <summary>
        /// Inserta un registro de la tabla CPA_EMPRESA
        /// </summary>
        public int SaveCpaEmpresa(CpaEmpresaDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetCpaEmpresaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_EMPRESA
        /// </summary>
        public void UpdateCpaEmpresa(CpaEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaEmpresaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_EMPRESA
        /// <param name="cpaEmpresaId">Identificador de la tabla CPA_EMPRESA</param>
        /// </summary>
        public void DeleteCpaEmpresa(int cpaEmpresaId)
        {
            try
            {
                FactoryTransferencia.GetCpaEmpresaRepository().Delete(cpaEmpresaId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_EMPRESA
        /// <param name="cpaEmpresaId">Identificador de la tabla CPA_EMPRESA</param>
        /// </summary>
        public CpaEmpresaDTO GetByIdCpaEmpresa(int cpaEmpresaId)
        {
            return FactoryTransferencia.GetCpaEmpresaRepository().GetById(cpaEmpresaId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_EMPRESA
        /// </summary>
        public List<CpaEmpresaDTO> ListCpaEmpresa()
        {
            return FactoryTransferencia.GetCpaEmpresaRepository().List();
        }

        #endregion

        #region CPA_GERCSVDET
        /// <summary>
        /// Inserta un registro de la tabla CPA_GERCSVDET
        /// </summary>
        public int SaveCpaGercsvDet(CpaGercsvDetDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetCpaGercsvDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_GERCSVDET
        /// </summary>
        public void UpdateCpaGercsvDet(CpaGercsvDetDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaGercsvDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_GERCSVDET
        /// <param name="cpaGercsvTmpId">Identificador de la tabla CPA_GERCSVDET</param>
        /// </summary>
        public void DeleteCpaGercsvDet(int cpaGercsvTmpId)
        {
            try
            {
                FactoryTransferencia.GetCpaGercsvDetRepository().Delete(cpaGercsvTmpId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_GERCSVDET
        /// <param name="cpaGercsvTmpId">Identificador de la tabla CPA_GERCSVDET</param>
        /// </summary>
        public CpaGercsvDetDTO GetByIdCpaGercsvDet(int cpaGercsvTmpId)
        {
            return FactoryTransferencia.GetCpaGercsvDetRepository().GetById(cpaGercsvTmpId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_GERCSVDET
        /// <param name="cpagercodi">Identificador de la tabla CPA_GERCSV</param>
        /// <param name="cpagedtipcsv">Tipo de archivo: G=gergnd, H=gerhid, T=gerter y D=duraci.</param>
        /// <paramref name="dFecEjercicio">Fecha de inicio del Ejercicio</paramref>
        /// <paramref name="dFecEjercicioFin">Fecha de final del Ejercicio</paramref>
        /// </summary>
        public List<CpaGercsvDetDTO> ListCpaGercsvDet(int cpagercodi, string cpagedtipcsv, DateTime dFecEjercicio, DateTime dFecEjercicioFin)
        {
            return FactoryTransferencia.GetCpaGercsvDetRepository().List(cpagercodi, cpagedtipcsv, dFecEjercicio, dFecEjercicioFin);
        }

        //Funciones de CPA_GERCSVDET_TMP
        /// <summary>
        /// Trunca la tabla CPA_GERCSVDET_TMP
        /// </summary>
        public void TruncateCpaGercsvDetTmp()
        {
            try
            {
                FactoryTransferencia.GetCpaGercsvDetRepository().TruncateTmp();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region CPA_GERCSV
        /// <summary>
        /// Inserta un registro de la tabla CPA_GERCSV
        /// </summary>
        public int SaveCpaGercsv(CpaGercsvDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetCpaGercsvRepository().Save(entity);
            }
            catch (Exception ex) 
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_GERCSV
        /// </summary>
        public void UpdateCpaGercsv(CpaGercsvDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaGercsvRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_GERCSV
        /// <param name="cpaGercsvId">Identificador de la tabla CPA_GERCSV</param>
        /// </summary>
        public void DeleteCpaGercsv(int cpaGercsvId)
        {
            try
            {
                FactoryTransferencia.GetCpaGercsvRepository().Delete(cpaGercsvId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_GERCSV
        /// <param name="Cpsddpcodi">Identificador de la tabla CPA_SDDP</param>
        /// </summary>
        public CpaGercsvDTO GetByIdCpaGercsv(int Cpsddpcodi)
        {
            return FactoryTransferencia.GetCpaGercsvRepository().GetById(Cpsddpcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_GERCSV
        /// </summary>
        public List<CpaGercsvDTO> ListCpaGercsv()
        {
            return FactoryTransferencia.GetCpaGercsvRepository().List();
        }
        #endregion

        #region CPA_HISTORICO
        /// <summary>
        /// Inserta un registro de la tabla CPA_HISTORICO
        /// </summary>
        public void SaveCpaHistorico(CpaHistoricoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaHistoricoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_HISTORICO
        /// </summary>
        public void UpdateCpaHistorico(CpaHistoricoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaHistoricoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_HISTORICO
        /// <param name="cpaHistId">Identificador de la tabla CPA_HISTORICO</param>
        /// </summary>
        public void DeleteCpaHistorico(int cpaHistId)
        {
            try
            {
                FactoryTransferencia.GetCpaHistoricoRepository().Delete(cpaHistId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_HISTORICO
        /// <param name="cpaHistId">Identificador de la tabla CPA_HISTORICO</param>
        /// </summary>
        public CpaHistoricoDTO GetByIdCpaHistorico(int cpaHistId)
        {
            return FactoryTransferencia.GetCpaHistoricoRepository().GetById(cpaHistId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_HISTORICO
        /// </summary>
        public List<CpaHistoricoDTO> ListCpaHistorico()
        {
            return FactoryTransferencia.GetCpaHistoricoRepository().List();
        }

        #endregion

        #region CPA_INSUMO_DIA
        /// <summary>
        /// Inserta un registro de la tabla CPA_INSUMO_DIA
        /// </summary>
        public void SaveCpaInsumoDia(CpaInsumoDiaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoDiaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_INSUMO_DIA
        /// </summary>
        //public void UpdateCpaInsumoDia(CpaInsumoDiaDTO entity)
        //{
        //    try
        //    {
        //        FactoryTransferencia.GetCpaInsumoDiaRepository().Update(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ConstantesAppServicio.LogError, ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Elimina un registro de la tabla CPA_INSUMO_DIA
        /// <param name="cpaInsumoDiaId">Identificador de la tabla CPA_INSUMO_DIA</param>
        /// </summary>
        public void DeleteCpaInsumoDia(int cpaInsumoDiaId)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoDiaRepository().Delete(cpaInsumoDiaId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_INSUMO_DIA
        /// <param name="cpaInsumoDiaId">Identificador de la tabla CPA_INSUMO_DIA</param>
        /// </summary>
        public CpaInsumoDiaDTO GetByIdCpaInsumoDia(int cpaInsumoDiaId)
        {
            return FactoryTransferencia.GetCpaInsumoDiaRepository().GetById(cpaInsumoDiaId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_INSUMO_DIA
        /// </summary>
        public List<CpaInsumoDiaDTO> ListCpaInsumoDia()
        {
            return FactoryTransferencia.GetCpaInsumoDiaRepository().List();
        }

        /// <summary>
        /// Elimina información de la tabla CPA_INSUMO_DIA por Revisión y Tipo de Insumo
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="equicodi">Identificador de la tabla EQUICODI</param>
        /// <param name="cpaindtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// <param name="cpainmcodi">Identificador de la tabla CPA_INSUMO_MES</param>
        /// </summary>
        public void DeleteCpaInsumoDiaByCentral(int cparcodi,int equicodi, string cpaindtipinsumo, int cpainmcodi)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoDiaRepository().DeleteByCentral(cparcodi, equicodi, cpaindtipinsumo, cpainmcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina información de la tabla CPA_INSUMO_DIA por Revisión y Tipo de Insumo
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="cpaindtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// </summary>
        public void DeleteCpaInsumoDiaByRevision(int cparcodi, string cpaindtipinsumo)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoDiaRepository().DeleteByRevision(cparcodi, cpaindtipinsumo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta la información en la tabla CPA_INSUMO_DIA desde CPA_GERCSVDET_TMP
        /// <param name="cpaindcodi">Identificador de la tabla CPA_INSUMO_DIA</param>
        /// <param name="cpainmcodi">Identificador de la tabla CPA_INSUMO_MES</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="fecinicio">Intervalo de inicio</param>
        /// <param name="fecfin">Intervalo final</param>
        /// </summary>
        private void InsertarInsumoDiaByTMP(int cpaindcodi, int cpainmcodi, int emprcodi, int equicodi, DateTime fecinicio, DateTime fecfin)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoDiaRepository().InsertarInsumoDiaByTMP(cpaindcodi, cpainmcodi, emprcodi, equicodi, fecinicio, fecfin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta la información en la tabla CPA_INSUMO_DIA desde CMg Ejecutado - VTEA
        /// <param name="cpaindcodi">Identificador de la tabla CPA_INSUMO_DIA</param>
        /// <param name="cpainmcodi">Identificador de la tabla CPA_INSUMO_MES</param>
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="fecinicio">Primer dia del mes (periodo, revisión)</param>
        /// <param name="pericodi">Identificador de la tabla TRN_PERIODO</param>
        /// <param name="recacodi">Identificador de la tabla TRN_RECALCULO</param>
        /// <param name="barrcodi">Identificador de la tabla TRN_BARRA -> Barra de Transferencia</param>
        /// <param name="cpaindusucreacion">Usuario que ejecuta el proceso</param>
        /// </summary>
        private void InsertarInsumoDiaByCMg(int cpaindcodi, int cpainmcodi, int cparcodi, int emprcodi, int equicodi, DateTime fecinicio, int pericodi, int recacodi, int barrcodi, string cpaindusucreacion)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoDiaRepository().InsertarInsumoDiaByCMg(cpaindcodi, cpainmcodi, cparcodi, emprcodi, equicodi, fecinicio, pericodi, recacodi, barrcodi, cpaindusucreacion);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta la información en la tabla CPA_INSUMO_DIA desde RER_INSUMO_DIA_TEMP
        /// <param name="cpaindcodi">Identificador de la tabla CPA_INSUMO_DIA</param>
        /// <param name="cpainmcodi">Identificador de la tabla CPA_INSUMO_MES</param>
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="ptomedicodi">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="fecinicio">Intervalo de inicio</param>
        /// <param name="fecfin">Intervalo final</param>
        /// <param name="dTipoCambio">Tipo de Cambio del Mes</param>
        /// <param name="cpaindusucreacion">Usuario que ejecuta el proceso</param>
        /// </summary>
        private void InsertarInsumoDiaByCMgPMPO(int cpaindcodi, int cpainmcodi, int cparcodi, int emprcodi, int equicodi, int ptomedicodi, DateTime fecinicio, DateTime fecfin, decimal dTipoCambio, string cpaindusucreacion)
        {
            try
            {
                DateTime dInicio = fecinicio;
                DateTime dFin = fecfin;
                int iNumAnio = 0;
                int iNumRegistros = 0;
                do
                {
                    //Paso 1: Verificamos si existe información a importar en el inicio de 01/mes/año: dInicio
                    iNumRegistros = FactoryTransferencia.GetCpaInsumoDiaRepository().GetNumRegistrosCMgByFecha(ptomedicodi, equicodi, dInicio);
                    if (iNumRegistros > 0)
                    {
                        //Paso2: si existe importamos la data del mes
                        FactoryTransferencia.GetCpaInsumoDiaRepository().InsertarInsumoDiaByCMgPMPO(cpaindcodi, cpainmcodi, cparcodi, emprcodi, equicodi, ptomedicodi, dInicio, dFin, dTipoCambio, cpaindusucreacion);
                    }
                    else
                    {
                        //Paso3: si no hay data, nos movemosun año adelante en el mismo mes
                        dInicio = dInicio.AddYears(1);
                        dFin = dFin.AddYears(1);
                        iNumAnio++; //Numero de años que se a avanzado para encontrar data
                        if (iNumAnio > 10) iNumRegistros = -1;
                    }
                }
                while (iNumRegistros == 0);

                if (iNumAnio > 0)
                {
                    //Como el numero de años ha avanzado en iNumAnio, actualicemos los registros al año de fecinicio
                    FactoryTransferencia.GetCpaInsumoDiaRepository().UpdateMesEquipo(cpainmcodi, iNumAnio);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta la información en la tabla CPA_INSUMO_DIA desde CPA_GERCSVDET
        /// <param name="cpaindcodi">Identificador de la tabla CPA_INSUMO_DIA</param>
        /// <param name="cpainmcodi">Identificador de la tabla CPA_INSUMO_MES</param>
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="fecinicio">Intervalo de inicio</param>
        /// <param name="fecfin">Intervalo final</param>
        /// </summary>
        private void InsertarInsumoDiaBySddp(int cpaindcodi, int cpainmcodi, int cparcodi, int emprcodi, int equicodi, DateTime fecinicio, DateTime fecfin)
        {
            try
            {
                DateTime dInicio = fecinicio;
                DateTime dFin = fecfin;
                int iNumAnio = 0;
                int iNumRegistros = 0;
                do {
                    //Paso 1: Verificamos si existe información a importar en el inicio de 01/mes/año: dInicio
                    iNumRegistros = FactoryTransferencia.GetCpaInsumoDiaRepository().GetNumRegistrosByFecha(dInicio);
                    if (iNumRegistros > 0)
                    {
                        //Paso2: si existe importamos la data del mes
                        FactoryTransferencia.GetCpaInsumoDiaRepository().InsertarInsumoDiaBySddp(cpaindcodi, cpainmcodi, cparcodi, emprcodi, equicodi, dInicio, dFin);
                    }
                    else
                    {
                        //Paso3: si no hay data, nos movemosun año adelante en el mismo mes
                        dInicio = dInicio.AddYears(1);
                        dFin = dFin.AddYears(1);
                        iNumAnio++; //Numero de años que se a avanzado para encontrar data
                        if (iNumAnio > 10) iNumRegistros = -1;
                    }
                }
                while (iNumRegistros == 0);

                if (iNumAnio > 0) {
                    //Como el numero de años ha avanzado en iNumAnio, actualicemos los registros al año de fecinicio
                    FactoryTransferencia.GetCpaInsumoDiaRepository().UpdateMesEquipo(cpainmcodi, iNumAnio);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region CPA_INSUMO_MES
        /// <summary>
        /// Inserta un registro de la tabla CPA_INSUMO_MES
        /// </summary>
        public int SaveCpaInsumoMes(CpaInsumoMesDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetCpaInsumoMesRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_INSUMO_MES
        /// </summary>
        public void UpdateCpaInsumoMes(CpaInsumoMesDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoMesRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_INSUMO_MES
        /// <param name="cpaInsumoMesId">Identificador de la tabla CPA_INSUMO_MES</param>
        /// </summary>
        public void DeleteCpaInsumoMes(int cpaInsumoMesId)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoMesRepository().Delete(cpaInsumoMesId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_INSUMO_MES
        /// <param name="cpaInsumoMesId">Identificador de la tabla CPA_INSUMO_MES</param>
        /// </summary>
        public CpaInsumoMesDTO GetByIdCpaInsumoMes(int cpaInsumoMesId)
        {
            return FactoryTransferencia.GetCpaInsumoMesRepository().GetById(cpaInsumoMesId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_INSUMO_MES
        /// </summary>
        public List<CpaInsumoMesDTO> ListCpaInsumoMes()
        {
            return FactoryTransferencia.GetCpaInsumoMesRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_INSUMO_MES
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="cpainmtipinsumo">Tipo de insumo</param>
        /// <param name="cpainmes">Identificador de la tabla CPA_INSUMO_MES</param>
        /// </summary>
        public CpaInsumoMesDTO GetByCriteriaCpaInsumoMes(int cparcodi, int emprcodi, int equicodi, string cpainmtipinsumo, int cpainmes)
        {
            return FactoryTransferencia.GetCpaInsumoMesRepository().GetByCriteria(cparcodi, emprcodi, equicodi, cpainmtipinsumo, cpainmes);
        }

        /// <summary>
        /// Elimina información de la tabla CPA_INSUMO_MES por Revisión, emprcodi, equicodi y Tipo de Insumo
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="cpainmtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// <param name="cpainmcodi">Identificador de la tabla CPA_INSUMO_MES</param>
        /// </summary>
        public void DeleteCpaInsumoMesByCentral(int cparcodi, int equicodi, string cpainmtipinsumo, int cpainmcodi)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoMesRepository().DeleteByCentral(cparcodi, equicodi, cpainmtipinsumo, cpainmcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina información de la tabla CPA_INSUMO_MES por Revisión y Tipo de Insumo
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="cpainmtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// </summary>
        public void DeleteCpaInsumoMesByRevision(int cparcodi, string cpainmtipinsumo)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoMesRepository().DeleteByRevision(cparcodi, cpainmtipinsumo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Proceso que se encarga de Insertar un InsumoMes
        /// </summary>
        /// <param name="Cpainscodi">Identificador de la tabla CPA_INSUMO</param>
        /// <param name="Cparcodi">Identificador de la tabla CPA_REVISIÓN</param>
        /// <param name="Emprcodi">Identificador de una Empresa</param>
        /// <param name="Equicodi">Identificador de una Central</param>
        /// <param name="Cpainmtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// <param name="Cpainmtipproceso">M=Manual, A=Automatico.</param>
        /// <param name="Cpainmmes">Mes. Donde: 1=Enero, 2=Febrero, etc.</param>
        /// <param name="Cpainmtotal">Total del mes.</param>
        /// <param name="Cpainmusucreacion">Usuario</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public int InsertarInsumoMes(int Cpainscodi, int Cparcodi, int Emprcodi, int Equicodi, string Cpainmtipinsumo, string Cpainmtipproceso, int Cpainmmes, decimal Cpainmtotal, string Cpainmusucreacion)
        {
            CpaInsumoMesDTO dtoInsumoMes = new CpaInsumoMesDTO();
            //dtoInsumoMes.Cpainmcodi            --PK
            dtoInsumoMes.Cpainscodi = Cpainscodi;
            dtoInsumoMes.Cparcodi = Cparcodi;
            dtoInsumoMes.Emprcodi = Emprcodi;
            dtoInsumoMes.Equicodi = Equicodi;
            dtoInsumoMes.Cpainmtipinsumo = Cpainmtipinsumo;
            dtoInsumoMes.Cpainmtipproceso = Cpainmtipproceso;
            dtoInsumoMes.Cpainmmes = Cpainmmes;
            dtoInsumoMes.Cpainmtotal = Cpainmtotal;
            dtoInsumoMes.Cpainmusucreacion = Cpainmusucreacion;
            dtoInsumoMes.Cpainmfeccreacion = DateTime.Now;
            return SaveCpaInsumoMes(dtoInsumoMes);
        }

        /// <summary>
        /// Actualiza el total en un registro de la tabla CPA_INSUMO_MES a partir de Insumo Día
        /// <param name="Cpainmcodi">Identificador de la tabla CPA_INSUMO_MES</param>
        /// <param name="Equicodi">Identificador de una Central</param>
        /// <param name="Cpainmtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// <param name="dFecInicio">Intervalo de Inicio</param>
        /// <param name="dFecFin">Intervao Fin</param>
        /// </summary>
        public void UpdateInsumoMesTotal(int Cpainmcodi, int Equicodi, string Cpainmtipinsumo, DateTime dFecInicio, DateTime dFecFin)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoMesRepository().UpdateInsumoMesTotal(Cpainmcodi, Equicodi, Cpainmtipinsumo, dFecInicio, dFecFin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        ///// <summary>
        ///// Actualiza el total en un registro de la tabla CPA_INSUMO_MES
        ///// <param name="Cpainmcodi">Identificador de la tabla CPA_INSUMO_MES</param>
        ///// <param name="Cpainmtotal">Valor total del mes</param>
        ///// <param name="sFechaModificacion">Fecha en la que se realizó la modificación</param>
        ///// <param name="sUserModificacion">Usuario que realizo la modificación</param>
        ///// </summary>
        //public void UpdateByInsumoMes(int Cpainmcodi, float Cpainmtotal, DateTime sFechaModificacion, string sUserModificacion)
        //{
        //    try
        //    {
        //        FactoryTransferencia.GetCpaInsumoMesRepository().UpdateByInsumoMes(Cpainmcodi, Cpainmtotal, sFechaModificacion, sUserModificacion);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ConstantesAppServicio.LogError, ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}
        #endregion

        #region CPA_INSUMO
        /// <summary>
        /// Inserta un registro de la tabla CPA_INSUMO
        /// </summary>
        public int SaveCpaInsumo(CpaInsumoDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetCpaInsumoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_INSUMO
        /// </summary>
        public void UpdateCpaInsumo(CpaInsumoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_INSUMO
        /// <param name="cpaInsumoId">Identificador de la tabla CPA_INSUMO</param>
        /// </summary>
        public void DeleteCpaInsumo(int cpaInsumoId)
        {
            try
            {
                FactoryTransferencia.GetCpaInsumoRepository().Delete(cpaInsumoId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_INSUMO
        /// <param name="cpaInsumoId">Identificador de la tabla CPA_INSUMO</param>
        /// </summary>
        public CpaInsumoDTO GetByIdCpaInsumo(int cpaInsumoId)
        {
            return FactoryTransferencia.GetCpaInsumoRepository().GetById(cpaInsumoId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_INSUMO
        /// </summary>
        public List<CpaInsumoDTO> ListCpaInsumo()
        {
            return FactoryTransferencia.GetCpaInsumoRepository().List();
        }

        /// <summary>
        /// Proceso que se encarga de Insertar un Insumo
        /// </summary>
        /// <param name="Cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="Cpainstipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// <param name="Cpainstipproceso">M: Manual, A: Automatico</param>
        /// <param name="Cpainslog">Mensaje para el usuario</param>
        /// <param name="Cpainsusucreacion">Usuario</param>
        /// <returns>cpaInsumoId: Identificador de la tabla CPA_INSUMO</returns>
        public int InsertarInsumo(int Cparcodi, string Cpainstipinsumo, string Cpainstipproceso, string Cpainslog, string Cpainsusucreacion)
        {
            CpaInsumoDTO dtoInsumo = new CpaInsumoDTO();
            //dtoInsumo.Cpainscodi -- PK
            dtoInsumo.Cparcodi = Cparcodi;
            dtoInsumo.Cpainstipinsumo = Cpainstipinsumo;
            dtoInsumo.Cpainstipproceso = Cpainstipproceso;
            dtoInsumo.Cpainslog = Cpainslog;
            dtoInsumo.Cpainsusucreacion = Cpainsusucreacion;
            dtoInsumo.Cpainsfeccreacion = DateTime.Now;
            return SaveCpaInsumo(dtoInsumo);
        }
        #endregion

        #region CPA_PARAMETRO
        /// <summary>
        /// Inserta un registro de la tabla CPA_PARAMETRO
        /// </summary>
        public int SaveCpaParametro(CpaParametroDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetCpaParametroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_PARAMETRO
        /// </summary>
        public void UpdateCpaParametro(CpaParametroDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaParametroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_PARAMETRO
        /// <param name="cpaParametroId">Identificador de la tabla CPA_PARAMETRO</param>
        /// </summary>
        public void DeleteCpaParametro(int cpaParametroId)
        {
            try
            {
                FactoryTransferencia.GetCpaParametroRepository().Delete(cpaParametroId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_PARAMETRO
        /// <param name="cpaParametroId">Identificador de la tabla CPA_PARAMETRO</param>
        /// </summary>
        public CpaParametroDTO GetByIdCpaParametro(int cpaParametroId)
        {
            return FactoryTransferencia.GetCpaParametroRepository().GetById(cpaParametroId);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_PARAMETRO
        /// </summary>
        public List<CpaParametroDTO> ListCpaParametro()
        {
            return FactoryTransferencia.GetCpaParametroRepository().List();
        }

        #endregion

        #region CPA_PARAMETRO_HISTORICO
        /// <summary>
        /// Inserta un registro de la tabla CPA_PARAMETRO_HISTORICO
        /// </summary>
        public void SaveCpaParametroHistorico(CpaParametroHistoricoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaParametroHistoricoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region CPA_REVISION
        /// <summary>
        /// Inserta un registro de la tabla CPA_REVISION
        /// </summary>
        public void SaveCpaRevision(CpaRevisionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaRevisionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_REVISION
        /// </summary>
        public void UpdateCpaRevision(CpaRevisionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaRevisionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_REVISION
        /// <param name="cpaRevId">Identificador de la tabla CPA_REVISION</param>
        /// </summary>
        public void DeleteCpaRevision(int cpaRevId)
        {
            try
            {
                FactoryTransferencia.GetCpaRevisionRepository().Delete(cpaRevId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_REVISION
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// </summary>
        public CpaRevisionDTO GetByIdCpaRevision(int cparcodi)
        {
            try
            {
                CpaRevisionDTO revision = FactoryTransferencia.GetCpaRevisionRepository().GetById(cparcodi);
                FormatearCpaRevision(revision);
                return revision;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception("No existe la Revisión con id = " + cparcodi);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_REVISION
        /// </summary>
        public List<CpaRevisionDTO> ListCpaRevision()
        {
            return FactoryTransferencia.GetCpaRevisionRepository().List();
        }

        #endregion

        #region CPA_SDDP
        /// <summary>
        /// Inserta un registro de la tabla CPA_SDDP
        /// </summary>
        public int SaveCpaSddp(CpaSddpDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetCpaSddpRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla CPA_SDDP
        /// </summary>
        public void UpdateCpaSddp(CpaSddpDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaSddpRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_SDDP
        /// <param name="cpaSddpId">Identificador de la tabla CPA_SDDP</param>
        /// </summary>
        public void DeleteCpaSddp(int cpaSddpId)
        {
            try
            {
                FactoryTransferencia.GetCpaSddpRepository().Delete(cpaSddpId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_SDDP
        /// <param name="Cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// </summary>
        public CpaSddpDTO GetByIdCpaSddp(int Cparcodi)
        {
            return FactoryTransferencia.GetCpaSddpRepository().GetById(Cparcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_SDDP
        /// </summary>
        public List<CpaSddpDTO> ListCpaSddp()
        {
            return FactoryTransferencia.GetCpaSddpRepository().List();
        }

        /// <summary>
        /// Permite obtener el correlativo del Sddp segun Cparcodi
        /// <param name="Cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// </summary>
        public int GetByCorrelativoSddp(int Cparcodi)
        {
            return FactoryTransferencia.GetCpaSddpRepository().GetByCorrelativoSddp(Cparcodi);
        }
        #endregion

        #region CPA_DOCUMENTOS
        /// <summary>
        /// Inserta un registro de la tabla CPA_SDDP
        /// </summary>
        public int SaveCpaDocumentos(CpaDocumentosDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetCpaDocumentosRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }
        #endregion

        #region CPA_DOCUMENTOS_DETALLE
        /// <summary>
        /// Inserta un registro de la tabla CPA_SDDP
        /// </summary>
        public int SaveCpaDocumentosDetalle(CpaDocumentosDetalleDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetCpaDocumentosDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        #endregion


        #region CPA_TOTAL_DEMANDA
        /// <summary>
        /// Inserta un registro de la tabla CPA_TOTAL_DEMANDA
        /// </summary>
        public int SaveCpaTotalDemanda(int Anio, string IdAjuste, int IdRevision, string IdTipoParticipacion, int Mes, string UsuCreacion, DateTime FecCreacion)
        {
            CpaTotalDemandaDTO entity = new CpaTotalDemandaDTO();
            entity.Cpatdanio = Anio;
            entity.Cpatdajuste = IdAjuste;
            entity.Cparcodi = IdRevision;
            entity.Cpatdtipo = IdTipoParticipacion;
            entity.Cpatdmes = Mes;
            entity.Cpatdusucreacion = UsuCreacion;
            entity.Cpatdfeccreacion = FecCreacion;

            try
            {
                return FactoryTransferencia.GetCpaTotalDemandaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_TOTAL_DEMANDA
        /// <param name="cpaTotalDemandaId">Identificador de la tabla CPA_TOTAL_DEMANDA</param>
        /// </summary>
        public void DeleteCpaTotalDemanda(int cpaTotalDemandaId)
        {
            try
            {
                FactoryTransferencia.GetCpaTotalDemandaRepository().Delete(cpaTotalDemandaId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_TOTAL_DEMANDA
        /// </summary>
        public List<CpaTotalDemandaDTO> ListCpaTotalDemanda()
        {
            return FactoryTransferencia.GetCpaTotalDemandaRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_TOTAL_DEMANDA
        /// <param name="cpaTotalDemandaId">Identificador de la tabla CPA_TOTAL_DEMANDA</param>
        /// </summary>
        public CpaTotalDemandaDTO GetByIdCpaTotalDemanda(int cpaTotalDemandaId)
        {
            return FactoryTransferencia.GetCpaTotalDemandaRepository().GetById(cpaTotalDemandaId);
        }
        #endregion

        #region CPA_TOTAL_DEMANDADET
        /// <summary>
        /// Inserta un registro de la tabla CPA_TOTAL_DEMANDADET
        /// </summary>
        public void SaveCpaTotalDemandaDet(CpaTotalDemandaDetDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaTotalDemandaDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_TOTAL_DEMANDADET
        /// <param name="cpaTotalDemandaDetId">Identificador de la tabla CPA_TOTAL_DEMANDADET</param>
        /// </summary>
        public void DeleteCpaTotalDemandaDet(int cpaTotalDemandaDetId)
        {
            try
            {
                FactoryTransferencia.GetCpaTotalDemandaDetRepository().Delete(cpaTotalDemandaDetId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_TOTAL_DEMANDADET
        /// </summary>
        public List<CpaTotalDemandaDetDTO> ListCpaTotalDemandaDet()
        {
            return FactoryTransferencia.GetCpaTotalDemandaDetRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_TOTAL_DEMANDADET
        /// <param name="cpaTotalDemandaDetId">Identificador de la tabla CPA_TOTAL_DEMANDADET</param>
        /// </summary>
        public CpaTotalDemandaDetDTO GetByIdCpaTotalDemandaDet(int cpaTotalDemandaDetId)
        {
            return FactoryTransferencia.GetCpaTotalDemandaDetRepository().GetById(cpaTotalDemandaDetId);
        }
        #endregion

        #region CPA_TOTAL_DEMANDA y CPA_TOTAL_DEMANDADET Adicionales
        /// <summary>
        /// Permite obtener registros de la CPA_TOTAL_DEMANDADET por el Id de la tabla CPA_TOTAL_DEMANDA
        /// </summary>
        /// <param name="cpaTdCodi">Id de la tabla CPA_TOTAL_DEMANDA</param>
        /// <returns>Lista de CpaTotalDemandaDetDTO</returns>
        public List<CpaTotalDemandaDetDTO> GetByIdDemanda(int cpaTdCodi)
        {
            return FactoryTransferencia.GetCpaTotalDemandaDetRepository().GetByIdDemanda(cpaTdCodi);
        }

        /// <summary>
        /// reporte de envios
        /// </summary>
        /// <returns>string</returns>
        public string ReporteHtmlEnviosDemandas(int revision, string tipo, int mes)
        {
            var lstCabecera = ObtenerEnvios(revision, tipo, mes);

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table class='pretty tabla-icono' style='width: 350px;' id='tablaenvio'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>CÓDIGO</th>");
            strHtml.Append("<th>FECHA REGISTRO</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            foreach (var cabecera in lstCabecera)
            {
                strHtml.Append($"<tr style='cursor:pointer;' onClick='obtenerDemandaDemandaDet({cabecera.Cpatdcodi},{cabecera.Cpatdanio},\"{cabecera.Cpatdajuste}\",{cabecera.Cparcodi},\"{cabecera.Cpatdtipo}\",{cabecera.Cpatdmes});'>");
                strHtml.AppendFormat("<td>{0}</td>", cabecera.Cpatdcodi);
                strHtml.AppendFormat("<td>{0}</td>", cabecera.Cpatdfeccreacion.ToString(ConstantesBase.FormatFechaFull));
                strHtml.AppendFormat("<td>{0}</td>", cabecera.Cpatdusucreacion);
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla CPA_TOTAL_DEMANDADET
        /// </summary>
        /// <param name="Anio">Año del calculo total de transmisores detalle</param>
        /// <param name="IdAjuste">Código de ajuste del total de transmisores detalle</param>
        /// <param name="IdRevision">Código de la revision del total de transmisores detalle</param>
        /// <param name="Tipo">Tipo</param>
        /// <param name="Mes">Mes</param>
        /// <param name="NombTipo">Tipo</param>
        /// <param name="NombMes">Nombre Mes</param>
        /// <param name="NombRevision">Nombre de la Versión de Recálculo de Potencia</param>
        /// <param name="Formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoTotalDemandaDetalle(int Anio, string IdAjuste, int IdRevision, string Tipo, int Mes, string NombTipo, string NombMes, string NombRevision, int Formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;

            List<CpaTotalDemandaDetDTO> ListaDemandaDet = null;

            // Selecciono filtros e Indica el boton Consultar y primero verifica si existen envios en la tabla
            if (ObtenerNroRegistrosEnvios() > 0) // Existen envios
            {
                if (ObtenerNroRegistroEnviosFiltros(IdRevision, Tipo, Mes) > 0)
                {
                    // Trae el ultimo envio para mostrarlo por defecto
                    ListaDemandaDet = ObtenerUltimoEnvio(IdRevision, Tipo, Mes);

                    if (ListaDemandaDet.Count == 0)
                    {
                        // Trae envio vacio
                        ListaDemandaDet = EnvioVacio(IdRevision, Tipo, Mes);
                    }
                }
                else
                {
                    // Trae envio vacio
                    ListaDemandaDet = EnvioVacio(IdRevision, Tipo, Mes);
                }
            }
            else // No existen envios
            {
                // Trae envio vacio
                ListaDemandaDet = EnvioVacio(IdRevision, Tipo, Mes);
            }

            if (Formato == 1)
            {
                fileName = "ReporteTotalDemanda_" + Mes.ToString("00") + "_" + Anio + "_" + IdAjuste + ".xlsx";
                ExcelDocumentCPPA.GenerarFormatoTotalDemanda(pathFile + fileName, Anio, NombTipo, NombMes, IdAjuste, NombRevision, ListaDemandaDet);
            }

            return fileName;
        }

        /// <summary>
        /// Permite obtener el ultimo envio de la tabla CPA_TOTAL_DEMANDADET
        /// </summary>
        public List<CpaTotalDemandaDetDTO> ObtenerUltimoEnvio(int IdRevision, string Tipo, int Mes)
        {
            return FactoryTransferencia.GetCpaTotalDemandaDetRepository().GetLastEnvio(IdRevision, Tipo, Mes);
        }

        /// <summary>
        /// Permite obtener el numero de registros de la tabla CPA_TOTAL_DEMANDADET
        /// </summary>
        public int ObtenerNroRegistrosEnvios()
        {
            return FactoryTransferencia.GetCpaTotalDemandaRepository().ObtenerNroRegistrosEnvios();
        }

        /// <summary>
        /// Permite obtener registros de la CPA_TOTAL_DEMANDADET
        /// </summary>
        /// <param name="revision">Id de la revisión/param>
        /// <param name="tipo">tipo</param>
        /// <param name="mes">Mes</param>
        /// <returns>Lista de CpaTotalDemandaDetDTO</returns>
        public List<CpaTotalDemandaDetDTO> EnvioVacio(int revision, string tipo, int mes)
        {
            return FactoryTransferencia.GetCpaTotalDemandaDetRepository().EnvioVacio(revision, tipo, mes);
        }

        /// <summary>
        /// Permite obtener el numero de registros de la tabla CPA_TOTAL_DEMANDA
        /// </summary>
        public int ObtenerNroRegistroEnviosFiltros(int revision, string tipo, int mes)
        {
            return FactoryTransferencia.GetCpaTotalDemandaRepository().ObtenerNroRegistroEnviosFiltros(revision, tipo, mes);
        }

        /// <summary>
        /// Permite obtener registros de la CPA_TOTAL_DEMANDA
        /// </summary>
        /// <param name="revision">Id de la revisión/param>
        /// <param name="tipo">tipo</param>
        /// <param name="mes">Mes</param>
        /// <returns>Lista de CpaTotalDemandaDetDTO</returns>
        public List<CpaTotalDemandaDTO> ObtenerEnvios(int revision, string tipo, int mes)
        {
            return FactoryTransferencia.GetCpaTotalDemandaRepository().ObtenerEnvios(revision, tipo, mes);
        }

        /// <summary>
        /// Permite obtener el estado de la tabla CPA_REVISION
        /// </summary>
        /// <param name="revision">Id de la revisión</param>
        /// <returns>Estado</returns>
        public string ObtenerEstadoRevisionDemanda(int revision)
        {
            return FactoryTransferencia.GetCpaTotalDemandaRepository().ObtenerEstadoRevisionDemanda(revision);
        }

        /// <summary>
        /// Permite obtener el numero de registros de la tabla CPA_PORCENTAJE
        /// </summary>
        /// <param name="revision">Id de la revisión</param>
        /// <returns>Nro de registros de la tabla CPA_PORCENTAJE</returns>
        public int ObtenerNroRegistrosCPPEJDemanda(int revision)
        {
            return FactoryTransferencia.GetCpaTotalDemandaRepository().ObtenerNroRegistrosCPPEJDemanda(revision);
        }

        /// <summary>
        /// Eliminar registros de la tabla CPA_PORCENTAJE
        /// </summary>
        /// <param name="revision">Id de la revisión</param>
        /// <returns></returns>
        public void DeleteCPPEJDemanda(int revision)
        {
            FactoryTransferencia.GetCpaTotalDemandaRepository().DeleteCPPEJDemanda(revision);
        }

        /// <summary>
        /// Permite obtener el tipo de empresa tabla CPA_EMPRESA
        /// </summary>
        /// <param name="cparcodi">Revisión</param>
        /// <param name="cpaemptipo">Tipo empresa</param>
        /// <param name="emprNom">Nombre empresa</param>
        /// <returns>Tipo</returns>
        public string ObtenerTipoEmpresaCPADemandaPorNombre(int cparcodi, string cpaemptipo, string emprNom)
        {
            return FactoryTransferencia.GetCpaTotalDemandaRepository().ObtenerTipoEmpresaCPAPorNombre(cparcodi, cpaemptipo, emprNom);
        }
        #endregion

        #region CPA_TOTAL_TRANSMISORES
        /// <summary>
        /// Inserta un registro de la tabla CPA_TOTAL_TRANSMISORES
        /// </summary>
        public int SaveCpaTotalTransmisores(int Anio, string IdAjuste, int IdRevision, string UsuCreacion, DateTime FecCreacion)
        {
            CpaTotalTransmisoresDTO entity = new CpaTotalTransmisoresDTO();
            entity.Cpattanio = Anio;
            entity.Cpattajuste = IdAjuste;
            entity.Cparcodi = IdRevision;
            entity.Cpattusucreacion = UsuCreacion;
            entity.Cpattfeccreacion = FecCreacion;

            try
            {
                return FactoryTransferencia.GetCpaTotalTransmisoresRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_TOTAL_TRANSMISORES
        /// <param name="cpaTotalTransmisoresId">Identificador de la tabla CPA_TOTAL_TRANSMISORES</param>
        /// </summary>
        public void DeleteCpaTotalTransmisores(int cpaTotalTransmisoresId)
        {
            try
            {
                FactoryTransferencia.GetCpaTotalTransmisoresRepository().Delete(cpaTotalTransmisoresId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_TOTAL_TRANSMISORES
        /// </summary>
        public List<CpaTotalTransmisoresDTO> ListCpaTotalTransmisores()
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_TOTAL_TRANSMISORES
        /// <param name="cpaTotalTransmisoresId">Identificador de la tabla CPA_TOTAL_TRANSMISORES</param>
        /// </summary>
        public CpaTotalTransmisoresDTO GetByIdCpaTotalTransmisores(int cpaTotalTransmisoresId)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresRepository().GetById(cpaTotalTransmisoresId);
        }
        #endregion

        #region CPA_TOTAL_TRANSMISORESDET
        /// <summary>
        /// Inserta un registro de la tabla CPA_TOTAL_TRANSMISORESDET
        /// </summary>
        public void SaveCpaTotalTransmisoresDet(CpaTotalTransmisoresDetDTO entity)
        {
            try
            {
                FactoryTransferencia.GetCpaTotalTransmisoresDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CPA_TOTAL_TRANSMISORESDET
        /// <param name="cpaTotalTransmisoresDetId">Identificador de la tabla CPA_TOTAL_TRANSMISORESDET</param>
        /// </summary>
        public void DeleteCpaTotalTransmisoresDet(int cpaTotalTransmisoresDetId)
        {
            try
            {
                FactoryTransferencia.GetCpaTotalTransmisoresDetRepository().Delete(cpaTotalTransmisoresDetId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CPA_TOTAL_TRANSMISORESDET
        /// </summary>
        public List<CpaTotalTransmisoresDetDTO> ListCpaTotalTransmisoresDet()
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresDetRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_TOTAL_TRANSMISORESDET
        /// <param name="cpaTotalTransmisoresDetId">Identificador de la tabla CPA_TOTAL_TRANSMISORESDET</param>
        /// </summary>
        public CpaTotalTransmisoresDetDTO GetByIdCpaTotalTransmisoresDet(int cpaTotalTransmisoresDetId)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresDetRepository().GetById(cpaTotalTransmisoresDetId);
        }
        #endregion

        #region CPA_TOTAL_TRANSMISORES y CPA_TOTAL_TRANSMISORESDET Adicionales
        /// <summary>
        /// Permite obtener registros de la CPA_TOTAL_TRANSMISORESDET por el Id de la tabla CPA_TOTAL_TRANSMISORES
        /// </summary>
        /// <param name="cpaTdCodi">Id de la tabla CPA_TOTAL_TRANSMISORES</param>
        /// <returns>Lista de CpaTotalTransmisoresDetDTO</returns>
        public List<CpaTotalTransmisoresDetDTO> GetByIdTransmisores(int cpaTdCodi)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresDetRepository().GetByIdTransmisores(cpaTdCodi);
        }
        
        /// <summary>
        /// reporte de envios
        /// </summary>
        /// <param name="cparcodi">Id de la revisión/param>
        /// <returns>string</returns>
        public string ReporteHtmlEnviosTransmisores(int cparcodi)
        {
            var lstCabecera = ObtenerEnviosTransmisores(cparcodi);

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table class='pretty tabla-icono' style='width: 350px;' id='tablaenvio'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>CÓDIGO</th>");
            strHtml.Append("<th>FECHA REGISTRO</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            foreach (var cabecera in lstCabecera)
            {
                strHtml.Append($"<tr style='cursor:pointer;' onClick='obtenerTransmisoresTransmisoresDet({cabecera.Cpattcodi},{cabecera.Cpattanio},\"{cabecera.Cpattajuste}\",{cabecera.Cparcodi});'>");
                strHtml.AppendFormat("<td>{0}</td>", cabecera.Cpattcodi);
                strHtml.AppendFormat("<td>{0}</td>", cabecera.Cpattfeccreacion.ToString(ConstantesBase.FormatFechaFull));
                strHtml.AppendFormat("<td>{0}</td>", cabecera.Cpattusucreacion);
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla CPA_TOTAL_TRANSMISORESDET
        /// </summary>
        /// <param name="Anio">Año del calculo total de transmisores detalle</param>
        /// <param name="IdAjuste">Código de ajuste del total de transmisores detalle</param>
        /// <param name="IdRevision">Código de la revision del total de transmisores detalle</param>
        /// <param name="NombRevision">Nombre de la Versión de Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoTotalTransmisoresDetalle(int Anio, string IdAjuste, int IdRevision, string NombRevision, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;

            List<CpaTotalTransmisoresDetDTO> ListaTransmisoresDet = null;

            // Selecciono filtros e Indica el boton Consultar y primero verifica si existen envios en la tabla
            if (ObtenerNroRegistrosEnviosTransmisores() > 0)
            {
                if (ObtenerNroRegistroEnviosFiltrosTransmisores(IdRevision) > 0)
                {
                    // Trae el ultimo envio para mostrarlo por defecto
                    ListaTransmisoresDet = ObtenerUltimoEnvioTransmisores(IdRevision);

                    if (ListaTransmisoresDet.Count == 0)
                    {
                        // Trae envio vacio
                        ListaTransmisoresDet = EnvioVacioTransmisores(IdRevision);
                    }
                }
                else
                {
                    // Trae envio vacio
                    ListaTransmisoresDet = EnvioVacioTransmisores(IdRevision);
                }
            }
            else  // No existen envios
            {
                // Obtener el envio vacio
                ListaTransmisoresDet = EnvioVacioTransmisores(IdRevision);
            }

            if (formato == 1)
            {
                fileName = "ReporteTotalTransmisores_" + Anio + "_" + IdAjuste + ".xlsx";
                ExcelDocumentCPPA.GenerarFormatoTotalTransmisores(pathFile + fileName, 
                                                                  Anio, 
                                                                  IdAjuste,
                                                                  NombRevision,
                                                                  ListaTransmisoresDet);
            }

            return fileName;
        }

        /// <summary>
        /// Permite obtener el ultimo envio de la tabla CPA_TOTAL_TRANSMISORESDET
        /// </summary>
        public List<CpaTotalTransmisoresDetDTO> ObtenerUltimoEnvioTransmisores(int IdRevision)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresDetRepository().GetLastEnvio(IdRevision);
        }

        /// <summary>
        /// Permite obtener el numero de registros de la tabla CPA_TOTAL_TRANSMISORESDET
        /// </summary>
        public int ObtenerNroRegistrosEnviosTransmisores()
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresRepository().ObtenerNroRegistrosEnvios();
        }

        /// <summary>
        /// Permite obtener registros de la CPA_TOTAL_TRANSMISORESDET
        /// </summary>
        /// <param name="revision">Id de la revisión/param>
        /// <param name="tipo">tipo</param>
        /// <param name="mes">Mes</param>
        /// <returns>Lista de CpaTotalDemandaDetDTO</returns>
        public List<CpaTotalTransmisoresDetDTO> EnvioVacioTransmisores(int revision)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresDetRepository().EnvioVacio(revision);
        }

        /// <summary>
        /// Permite obtener el numero de registros de la tabla CPA_TOTAL_TRANSMISORES
        /// </summary>
        public int ObtenerNroRegistroEnviosFiltrosTransmisores(int revision)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresRepository().ObtenerNroRegistroEnviosFiltros(revision);
        }

        /// <summary>
        /// Permite obtener registros de la CPA_TOTAL_TRANSMISORES
        /// </summary>
        /// <param name="revision">Id de la revisión</param>
        /// <returns>Lista de CpaTotalDemandaDetDTO</returns>
        public List<CpaTotalTransmisoresDTO> ObtenerEnviosTransmisores(int revision)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresRepository().ObtenerEnvios(revision);
        }

        /// <summary>
        /// Permite obtener el estado de la tabla CPA_REVISION
        /// </summary>
        /// <param name="revision">Id de la revisión</param>
        /// <returns>Estado</returns>
        public string ObtenerEstadoRevisionTransmisores(int revision)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresRepository().ObtenerEstadoRevisionTransmisores(revision);
        }

        /// <summary>
        /// Permite obtener el numero de registros de la tabla CPA_PORCENTAJE
        /// </summary>
        /// <param name="revision">Id de la revisión</param>
        /// <returns>Nro de registros de la tabla CPA_PORCENTAJE</returns>
        public int ObtenerNroRegistrosCPPEJTransmisores(int revision)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresRepository().ObtenerNroRegistrosCPPEJTransmisores(revision);
        }

        /// <summary>
        /// Eliminar registros de la tabla CPA_PORCENTAJE
        /// </summary>
        /// <param name="revision">Id de la revisión</param>
        /// <returns></returns>
        public void DeleteCPPEJTransmisores(int revision)
        {
            FactoryTransferencia.GetCpaTotalTransmisoresRepository().DeleteCPPEJTransmisores(revision);
        }

        /// <summary>
        /// Permite obtener el tipo de empresa tabla CPA_EMPRESA
        /// </summary>
        /// <param name="cparcodi">Revisión</param>
        /// <param name="emprNom">Nombre empresa</param>
        /// <returns>Tipo</returns>
        public string ObtenerTipoEmpresaCPATransmisoresNombre(int cparcodi, string emprNom)
        {
            return FactoryTransferencia.GetCpaTotalTransmisoresRepository().ObtenerTipoEmpresaCPAPorNombre(cparcodi, emprNom);
        }
        #endregion
        #endregion

        #region Maestras

        #endregion
    }

    #region Clases Adicionales
    public class MesAnioPresupuestalDTO
    {
        public int Id { get; set; }
        public int Cparcodi { get; set; }
        public string NomMesAnio { get; set; }
        public string Cparcodi_tipoInsumo { get; set; }
        public string TipoInsumo { get; set; }

        public string NomMes { get; set; }
        public string NomArchivo { get; set; }
        public string NomPlantilla { get; set; }
        public string Id_Insumo { get; set; }
    }

    public class MyCustomException: Exception
    {
        public MyCustomException()
        {
        }

        public MyCustomException(string mensaje)
            : base(mensaje)
        {
        }

        public MyCustomException(string mensaje, Exception inner)
            : base(mensaje, inner)
        {
        }
    }
    #endregion

}