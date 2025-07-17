using System;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.General.Helper;
using System.Text;
using System.Configuration;

namespace COES.Servicios.Aplicacion.General
{
    public class EmpresaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EmpresaAppServicio));

        #region Métodos Tabla SI_CONCEPTO

        /// <summary>
        /// Inserta un registro de la tabla SI_CONCEPTO
        /// </summary>
        public void SaveSiConcepto(SiConceptoDTO entity)
        {
            try
            {
                FactorySic.GetSiConceptoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_CONCEPTO
        /// </summary>
        public void UpdateSiConcepto(SiConceptoDTO entity)
        {
            try
            {
                FactorySic.GetSiConceptoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_CONCEPTO
        /// </summary>
        public void DeleteSiConcepto(int consiscodi)
        {
            try
            {
                FactorySic.GetSiConceptoRepository().Delete(consiscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_CONCEPTO
        /// </summary>
        public SiConceptoDTO GetByIdSiConcepto(int consiscodi)
        {
            return FactorySic.GetSiConceptoRepository().GetById(consiscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_CONCEPTO
        /// </summary>
        public List<SiConceptoDTO> ListSiConceptos()
        {
            return FactorySic.GetSiConceptoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiConcepto
        /// </summary>
        public List<SiConceptoDTO> GetByCriteriaSiConceptos()
        {
            return FactorySic.GetSiConceptoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_EMPRESADAT

        /// <summary>
        /// Inserta un registro de la tabla SI_EMPRESADAT
        /// </summary>
        public void SaveSiEmpresadat(SiEmpresadatDTO entity)
        {
            try
            {
                FactorySic.GetSiEmpresadatRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_EMPRESADAT
        /// </summary>
        public void UpdateSiEmpresadat(SiEmpresadatDTO entity)
        {
            try
            {
                FactorySic.GetSiEmpresadatRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_EMPRESADAT
        /// </summary>
        public void DeleteSiEmpresadat(DateTime empdatfecha, int consiscodi, int emprcodi, string username)
        {
            try
            {
                FactorySic.GetSiEmpresadatRepository().Delete(empdatfecha, consiscodi, emprcodi);
                FactorySic.GetSiEmpresadatRepository().Delete_UpdateAuditoria(empdatfecha, consiscodi, emprcodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_EMPRESADAT
        /// </summary>
        public SiEmpresadatDTO GetByIdSiEmpresadat(DateTime empdatfecha, int consiscodi, int emprcodi)
        {
            return FactorySic.GetSiEmpresadatRepository().GetById(empdatfecha, consiscodi, emprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_EMPRESADAT
        /// </summary>
        public List<SiEmpresadatDTO> ListSiEmpresadats()
        {
            return FactorySic.GetSiEmpresadatRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiEmpresadat
        /// </summary>
        public List<SiEmpresadatDTO> GetByCriteriaSiEmpresadats()
        {
            return FactorySic.GetSiEmpresadatRepository().GetByCriteria();
        }

        /// <summary>
        /// Listar datos de empresa por fecha
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresas"></param>
        /// <param name="conceptos"></param>
        /// <returns></returns>
        public List<SiEmpresadatDTO> ListSiEmmpresadatsByEmpresaYConcepto(DateTime fechaInicio, DateTime fechaFin, string empresas, string conceptos)
        {
            return FactorySic.GetSiEmpresadatRepository().ListByEmpresaYConcepto(fechaInicio, fechaFin, empresas, conceptos);
        }

        #endregion

        /// <summary>
        /// Permite listar los tipos de empresas
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListarTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite hacer una consulta de los datos de la empresa
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        public BeanEmpresa ConsultarPorRUC(string ruc)
        {
            return (new ServicioSunat()).ObtenerDatosSunat(ruc);
        }

        /// <summary>
        /// Obtener empresa por ruc
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        public SiEmpresaDTO ObtenerEmpresaPorRUC(string ruc)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaPorRuc(ruc);
        }

        public SiEmpresaMMEDTO ObtenerEmpresaMME(int idEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetByIdMME(idEmpresa);
        }

        /// <summary>
        /// Permite realizar una búsqueda de empresas
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="nroRuc"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="empresaSein"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> BuscarEmpresas(string nombre, string nroRuc, int idTipoEmpresa, string empresaSein,
            string estado, int nroPagina, int nroFilas)
        {
            return FactorySic.GetSiEmpresaRepository().BuscarEmpresas(nombre, nroRuc, idTipoEmpresa, empresaSein, estado,
                nroPagina, nroFilas);
        }

        public List<SiEmpresaMMEDTO> BuscarEmpresasMME(int idTipoEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().BuscarEmpresasMME(idTipoEmpresa);
        }

        public List<SiEmpresaDTO> BuscarEmpresasporParticipacion(int tipo)
        {
            return FactorySic.GetSiEmpresaRepository().BuscarEmpresasporParticipacion(tipo);
        }

        public List<SiEmpresaMMEDTO> BuscarHistorialEmpresasMME(int Emprcodi)
        {
            return FactorySic.GetSiEmpresaRepository().BuscarHistorialEmpresasMME(Emprcodi);
        }


        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de empresas
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="nroRuc"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="empresaSein"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public int ObtenerNroRegistrosBusqueda(string nombre, string nroRuc, int idTipoEmpresa, string empresaSein,
            string estado)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerNroRegistrosBusqueda(nombre, nroRuc, idTipoEmpresa, empresaSein,
                estado);
        }

        /// <summary>
        /// Permite obtener el listado de empresas para exportars
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="nroRuc"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="empresaSein"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ExportarEmpresas(string nombre, string nroRuc, int idTipoEmpresa, string empresaSein,
            string estado)
        {
            return FactorySic.GetSiEmpresaRepository().ExportarEmpresas(nombre, nroRuc, idTipoEmpresa, empresaSein, estado);
        }

        /// <summary>
        /// Permite obtener una empresa en base al ID
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public SiEmpresaDTO ObtenerEmpresa(int idEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
        }

        /// <summary>
        /// Permite listar los tipos de comportamiento por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<SiTipoComportamientoDTO> ObtenerTipoComportamiento(int idEmpresa)
        {
            return FactorySic.GetSiTipoComportamientoRepository().ListByEmprcodi(idEmpresa);
        }

        /// <summary>
        /// Permite dar de baja a una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        public void DarBajaEmpresa(int idEmpresa, string usuario)
        {
            try
            {
                FactorySic.GetSiEmpresaRepository().DarBajaEmpresa(idEmpresa, usuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar la empresa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool GrabarEmpresa(SiEmpresaDTO entity, out int id, out List<string> listValidaciones, out string ruc)
        {
            try
            {
                bool flag = false;
                ruc = string.Empty;
                List<string> validaciones = new List<string>();
                id = 0;

                if (string.IsNullOrEmpty(entity.Emprruc) && entity.Emprdomiciliada.Equals("N")) //Modificado por sts 18 oct
                {
                    validaciones.Add(MensajesEmpresa.RucRequerido);
                }
                else
                {
                    if (!this.ValidarDocumentoIdentificacion(entity.Emprruc) && entity.Emprdomiciliada.Equals("N")) //Modificado por sts 18 oct
                    {
                        //validaciones.Add(MensajesEmpresa.RucRequerido);
                        validaciones.Add(MensajesEmpresa.RucInvalido);
                    }
                }

                if (entity.Emprcodi == 0)
                {
                    if (validaciones.Count == 0)
                    {
                        bool flagEmpresa = FactorySic.GetSiEmpresaRepository().VerificarExistenciaPorNombre(entity.Emprnomb);
                        bool flagRuc = FactorySic.GetSiEmpresaRepository().VerificarExistenciaPorRuc(entity.Emprruc);

                        if (!flagEmpresa && !flagRuc)
                        {
                            entity.Emprcoes = ConstantesAppServicio.NO;

                            entity.Emprusucreacion = entity.Lastuser;
                            entity.Emprfeccreacion = entity.Lastdate;

                            id = FactorySic.GetSiEmpresaRepository().Save(entity);

                            
                            //- Insertamos los tipos de comportamiento

                            foreach (SiTipoComportamientoDTO item in entity.ListaTipoComportamiento)
                            {
                                item.Emprcodi = id;
                                FactorySic.GetSiTipoComportamientoRepository().Save(item);
                            }

                            //- Fin inserción de los tipos de comportamiento

                            if (entity.Emprdomiciliada == MensajesEmpresa.DomiciliadaExtranjera && entity.Emprruc == null)
                            {
                                string rucGenerado = "ESR" + id.ToString().PadLeft(8, '0');
                                entity.Emprruc = rucGenerado;                              
                                FactorySic.GetSiEmpresaRepository().ActualizarRucFicticio(id, rucGenerado);
                            }

                            ruc = entity.Emprruc;

                            #region Sincronizar Empresa en base de datos CYDOC
                            try
                            {
                                entity.Emprcodi = id;
                                (new TramiteVirtualAppServicio()).SincronizarEmpresas(entity);
                            }
                            catch (Exception ex) { throw new Exception(ex.Message, ex); }
                            #endregion

                            //- Envio de correo de creación de nueva empresa
                            this.EnviarCorreoEmpresa(entity);

                            //- Fin de envio de correo de creación de nueva empresa

                            flag = true;
                        }
                        else
                        {
                            if (flagEmpresa) validaciones.Add(MensajesEmpresa.NombreEmpresaExiste);
                            if (flagRuc) validaciones.Add(MensajesEmpresa.RucEmpresaExiste);
                        }
                    }
                }
                else
                {
                    SiEmpresaDTO entidad = FactorySic.GetSiEmpresaRepository().GetById(entity.Emprcodi);
                    if (entidad.Emprruc == null) entidad.Emprruc = string.Empty;
                    if (entity.Emprruc == null) entity.Emprruc = string.Empty;  //Agregado por sts 18 oct

                    if (entity.Emprnomb.ToUpper() != entidad.Emprnomb.ToUpper())
                    {
                        bool flagEmpresa = FactorySic.GetSiEmpresaRepository().VerificarExistenciaPorNombre(entity.Emprnomb);
                        if (flagEmpresa)
                            validaciones.Add(MensajesEmpresa.NombreEmpresaExiste);
                    }

                    if (entity.Emprruc.Trim() != entidad.Emprruc.Trim())
                    {
                        bool flagRuc = FactorySic.GetSiEmpresaRepository().VerificarExistenciaPorRuc(entity.Emprruc);
                        if (flagRuc)
                            validaciones.Add(MensajesEmpresa.RucEmpresaExiste);
                    }

                    if (validaciones.Count == 0)
                    {
                        //entity.Emprcoes = entidad.Emprcoes;
                        entity.Emprusumodificacion = entity.Lastuser;
                        entity.Emprfecmodificacion = entity.Lastdate;
                        FactorySic.GetSiEmpresaRepository().Update(entity);
                        id = entity.Emprcodi;

                        try
                        {                            
                            (new TramiteVirtualAppServicio()).SincronizarEmpresas(entity);
                        }
                        catch (Exception ex) { throw new Exception(ex.Message, ex); }


                        //- Grabamos los tipos de comportamiento
                        List<SiTipoComportamientoDTO> listTipo = FactorySic.GetSiTipoComportamientoRepository().ListByEmprcodi(id);

                        foreach (SiTipoComportamientoDTO item in entity.ListaTipoComportamiento)
                        {
                            item.Emprcodi = id;

                            SiTipoComportamientoDTO tipoAnterior = listTipo.Where(x => x.Tipoemprcodi == item.Tipoemprcodi).FirstOrDefault();

                            if (tipoAnterior == null)
                            {
                                FactorySic.GetSiTipoComportamientoRepository().Save(item);
                            }
                            else
                            {
                                tipoAnterior.Tipoprincipal = item.Tipoprincipal;
                                FactorySic.GetSiTipoComportamientoRepository().Update(tipoAnterior);
                            }
                        }

                        //-Debemos eliminar los que no corresponden en la lista

                        List<int> idsEliminar = listTipo.Where(x => !entity.ListaTipoComportamiento.Any(y => x.Tipoemprcodi == y.Tipoemprcodi)).
                            Select(x => (int)x.Tipocodi).ToList();

                        foreach (int idTipo in idsEliminar)
                        {
                            FactorySic.GetSiTipoComportamientoRepository().Delete(idTipo);
                        }

                        //- Finalización del grabado de los tipos de comportamiento                        

                        //- Envio de correo de actualización de datos de la empresa

                        //- Fin de envio de correo de actualizacion de datos de la empresa

                        flag = true;
                    }
                }

                listValidaciones = validaciones;
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public bool GrabarEmpresaMME(SiEmpresaMMEDTO entity, out int id)
        {
            try
            {
                bool flag = false;
                id = 0;

                SiEmpresaMMEDTO existe = FactorySic.GetSiEmpresaRepository().GetByIdMME(entity.Emprcodi);
                entity.Emprmmecodi = (existe != null ? existe.Emprmmecodi : 0);

                if (entity.Emprmmecodi == 0)
                {
                    id = FactorySic.GetSiEmpresaRepository().SaveMME(entity);
                    flag = true;

                }
                else
                {
                    FactorySic.GetSiEmpresaRepository().UpdateMME(entity);

                    id = FactorySic.GetSiEmpresaRepository().SaveMME(entity);

                    flag = true;

                }
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        /// <summary>
        /// Notificacion de registro de nueva empresa
        /// </summary>
        /// <param name="entity"></param>
        public void EnviarCorreoEmpresa(SiEmpresaDTO entity)
        {
            string mensaje = this.ObtenerCuerpoCorreo(entity);

            string sTo = ConfigurationManager.AppSettings["RegEmprListUsuarioTo"].ToString();            
            //string sTo = NotificacionEmpresa.ListUsuarioTo;
            List<string> listTo = new List<string>();
            listTo = sTo.Split(';').ToList();
            List<string> listBCc = new List<string>();

            string sCC = ConfigurationManager.AppSettings["RegEmprListUsuarioCC"].ToString();
            //string sCC = NotificacionEmpresa.ListUsuarioCC;            
            List<string> listCC = new List<string>();
            listCC = sCC.Split(';').ToList();

            Base.Tools.Util.SendEmail(listTo, listCC, listBCc, "Notificación de registro de nueva Empresa", mensaje, "webapp@coes.org.pe"); 
        }

        /// <summary>
        /// Permite obtener el cuerpo del correo a enviar
        /// Permite obtener el cuerpo del correo a enviaReporteEmpresasr
        /// </summary>
        /// <param name="listaEquipo"></param>
        /// <param name="listaPropiedad"></param>
        /// <param name="listaGrupo"></param>
        protected string ObtenerCuerpoCorreo(SiEmpresaDTO entity)
        {
            StringBuilder strHtml = new StringBuilder();

            string HtmlEmpresa = @" <tr>
					               <td class='tdcelda'>{0}</td>
					               <td class='tdcelda'>{1}</td>
					               <td class='tdcelda'>{2}</td>
					               <td class='tdcelda'>{3}</td>
					               <td class='tdcelda'>{4}</td>
					               <td class='tdcelda'>{5}</td>							
				               </tr>";


            string sUsuario = entity.Lastuser;
            DateTime? dFecha = entity.Lastdate;
            string emprcoes = (entity.Emprcoes == ConstantesAppServicio.SI) ? "Si" : "No";

            strHtml.Append(String.Format(HtmlEmpresa, entity.Emprnomb, entity.Emprrazsocial, entity.Emprruc, emprcoes, sUsuario, dFecha));

            #region Cuerpo

            string HtmlCuerpo = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                            <html xmlns='http://www.w3.org/1999/xhtml'>
                            <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />
                            <title>Informe equipamiento</title>
                            <style type='text/css'>
                            body[
	                            background-color:#EEF0F2;
	                            top:0;
	                            left:0;
	                            margin:0;
	                            font-family:Arial, Helvetica, sans-serif;
	                            font-size:12px;
	                            color:#333333;
                            ]
                            .content[
	                            width:80%;
	                            margin:auto;
                            ]

                            .titulo[
	                            font-size:16px;
	                            color:#004080;
	                            font-weight:bold;
	                            text-align:center;
	                            padding:20px;
	                            text-transform:uppercase;
                            ]

                            .subtitulo[
	                            font-size:13px;
	                            color:#004080;
	                            font-weight:bold;
	                            text-transform:uppercase;		
                            ]

                            .table[
	
	                            margin-bottom:20px;
                            ]

                            .trtitulo[
	                            background-color:#506DBE;
	                            color:#fff;
	                            font-weight:bold;
	                            text-align:center;
	                            line-height:20px;
	                            font-size:10px;
	                            text-transform:uppercase;
                            ]

                            .tdcelda[
	                            background-color:#fff;
	                            text-align:center;
	                            border:1px solid #DBDCDD;
	                            border-top:1px none;
	                            line-height:18px;
                                font-size:11px;
                            ]

                            </style>
                            </head>

                            <body>
                            <table class='content'>
	                            <tr>
		                            <td class='titulo'>Registro de nueva Empresa</td>
	                            </tr>
	                            <tr>
		                            <td class='subtitulo'></td>
	                            </tr>
	                            <tr>
		                            <td>
			                            <table cellspacing='0' cellpadding='0' width='100%' border='0' class='table'>
				                            <tr class='trtitulo'>
					                            <td>Nombre</td>
					                            <td>Razón Social</td>
					                            <td>RUC</td>
					                            <td>Es Agente</td>
					                            <td>Usuario</td>
					                            <td>Fecha</td>		
				                            </tr>
				                            {0}				                          
			                            </table>
                                        <br />
                                        <br />
		                            </td>
	                            </tr>	                          
                            </table>
                            </body>
                            </html>
                            ";

            #endregion


            String mensaje = String.Format(HtmlCuerpo, strHtml.ToString());
            mensaje = mensaje.Replace("[", "{");
            mensaje = mensaje.Replace("]", "}");


            return mensaje;
        }

        /// <summary>
        /// Permite actualizar el estado de la empresa
        /// </summary>
        /// <param name="entity"></param>
        public void ActualizarEstadoEmpresa(SiEmpresaDTO entity)
        {
            try
            {
                FactorySic.GetSiEmpresaRepository().ActualizarEstado(entity);

                try
                {
                    SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(entity.Emprcodi);
                    (new TramiteVirtualAppServicio()).SincronizarEmpresas(empresa);
                }
                catch { }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite validar la existencia de RUC
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        public bool ValidarDocumentoIdentificacion(string identificationDocument)
        {
            if (!string.IsNullOrEmpty(identificationDocument))
            {
                if (identificationDocument.Trim().Length == 11) 
                {
                    return true;
                }

            }

            return false;
        }

        public List<SiEmpresaDTO> GetEmpresaSistemaPorTipo(string tipos)
        {
            return FactorySic.GetSiEmpresaRepository().GetEmpresaSistemaPorTipo(tipos);
        }

        /// <summary>
        /// Validar RUC
        /// </summary>
        /// <param name="identificationDocument"></param>
        /// <returns></returns>
        public bool ValidarRuc(string identificationDocument)
        {

            return true;
            //int addition = 0;
            //int[] hash = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            //int identificationDocumentLength = identificationDocument.Length;

            //string identificationComponent = identificationDocument.Substring(0, identificationDocumentLength - 1);

            //int identificationComponentLength = identificationComponent.Length;

            //int diff = hash.Length - identificationComponentLength;

            //for (int i = identificationComponentLength - 1; i >= 0; i--)
            //{
            //    addition += (identificationComponent[i] - '0') * hash[i + diff];
            //}

            //addition = 11 - (addition % 11);

            //if (addition == 11)
            //{
            //    addition = 0;
            //}

            //char last = char.ToUpperInvariant(identificationDocument[identificationDocumentLength - 1]);

            //if (identificationDocumentLength == 11)
            //{
            //    // The identification document corresponds to a RUC.
            //    return addition.Equals(last - '0');
            //}
            //return false;
            //else if (char.IsDigit(last))
            //{
            //    // The identification document corresponds to a DNI with a number as verification digit.
            //    char[] hashNumbers = { '6', '7', '8', '9', '0', '1', '1', '2', '3', '4', '5' };
            //    return last.Equals(hashNumbers[addition]);
            //}
            //else if (char.IsLetter(last))
            //{
            //    // The identification document corresponds to a DNI with a letter as verification digit.
            //    char[] hashLetters = { 'K', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            //    return last.Equals(hashLetters[addition]);
            //}

        }

        #region VIGENCIA DE EMPRESAS

        /// <summary>
        /// Permite listar [ ID+Nombre ] de empresas con estado Activas y Bajas del SEIN
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarIdNameEmpresasActivasBajas()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerIdNameEmpresasActivasBajas();
        }

        /// <summary>
        /// Listar el historico de estados de la empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<SiEmpresadatDTO> ListarEstadoEmpresaHistorico(string emprcodi, DateTime fechaConsulta)
        {
            List<SiEmpresadatDTO> lista = this.ListSiEmmpresadatsByEmpresaYConcepto(DateTime.MinValue, fechaConsulta, emprcodi, ConstantesAppServicio.ConsiscodiActivoEmpresa.ToString());

            foreach (var reg in lista)
            {
                reg.EmprestadoDesc = Util.EstadoDescripcion(reg.Emprestado);
                reg.EmprestadoFechaDesc = reg.EmprestadoFecha.ToString(ConstantesAppServicio.FormatoFechaHora);
                reg.EmpdatfeccreacionDesc = reg.Empdatfeccreacion != null ? reg.Empdatfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.EmpdatfecmodificacionDesc = reg.Empdatfecmodificacion != null ? reg.Empdatfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.UltimaModificacionUsuarioDesc = reg.Empdatfecmodificacion != null ? reg.Empdatusumodificacion : reg.Empdatusucreacion;
                reg.UltimaModificacionFechaDesc = reg.Empdatfecmodificacion != null ? reg.EmpdatfecmodificacionDesc : reg.EmpdatfeccreacionDesc;
            }

            return lista;
        }

        /// <summary>
        /// Indica si esta vigente una empresa (segun las transferencias de equipos)
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public bool EsEmpresaVigente(int emprcodi, DateTime fechaConsulta)
        {
            List<SiEmpresaDTO> lista = FactorySic.GetSiEmpresaRepository().ListarEmpresaEstadoActual(fechaConsulta);
            return lista.Find(x => x.Emprcodi == emprcodi && x.Emprestado == ConstantesAppServicio.Activo) != null;
        }

        #endregion
    }
}
