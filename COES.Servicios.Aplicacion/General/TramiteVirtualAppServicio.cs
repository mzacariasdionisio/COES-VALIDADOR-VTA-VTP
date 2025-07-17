using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General
{
    public class TramiteVirtualAppServicio : AppServicioBase
    {
        /// <summary>
        /// Código de módulo asociado
        /// </summary>
        public int IdModulo = 31;
        public int IdModuloTramite = 34;
         /// <summary>
         /// Permite obtener el listado de proveedores
         /// </summary>
         /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasProveedor(int tipoEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasProveedores(tipoEmpresa);
        }

        /// <summary>
        /// Lista las empresas
        /// </summary>
        /// <param name="tipoAgente"></param>
        /// <param name="tipoEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasPortalTramite(string tipoAgente, int tipoEmpresa, string indicador, string ruc, string razonSocial, int nroPagina, int pageSize)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaPortalTramite(tipoAgente, tipoEmpresa, indicador, ruc, razonSocial, nroPagina, pageSize);
        }

        public int ObtenerNroRegistrosBusquedaTramite(string tipoAgente, int tipoEmpresa, string indicador, string ruc, string razonSocial)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerNroRegistrosBusquedaTramite(tipoAgente, tipoEmpresa, indicador, ruc, razonSocial);
        }

        /// <summary>
        /// Permite registrar los correos por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idModulo"></param>
        /// <returns></returns>
        public List<SiEmpresaCorreoDTO> ObtenerCorreosPorEmpresa(int idEmpresa, int idModulo)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().ObtenerCorreosPorEmpresaModuloAdicional(idEmpresa, idModulo);
        }

        /// <summary>
        /// Permite registrar los correos por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idModulo"></param>
        /// <returns></returns>
        public List<SiEmpresaCorreoDTO> ObtenerCorreosPorPersona(int idEmpresa, int idModulo)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().ObtenerCorreosPorEmpresaModuloAdicional(idEmpresa, idModulo);
        }
        /// <summary>
        /// Permite obtener las personas de contacto
        /// </summary>
        /// <param name="tipoAgente"></param>
        /// <param name="tipoEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaCorreoDTO> ObtenerPesonasContactoExportacion(string tipoAgente, int tipoEmpresa)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().ObtenerPesonasContactoExportacion(tipoAgente, tipoEmpresa);
        }

        /// <summary>
        /// Permite obtener el dato de la cuenta
        /// </summary>
        /// <param name="idEmpresaCorreo"></param>
        /// <returns></returns>
        public string ObtenerCorreo(int idEmpresaCorreo)
        {
            if (idEmpresaCorreo > 0)
            {
                SiEmpresaCorreoDTO entity = FactorySic.GetSiEmpresaCorreoRepository().GetById(idEmpresaCorreo);
                return entity.Empcoremail;
            }
            else
            {
                return string.Empty;
            }
        }

        public SiEmpresaCorreoDTO ObtenerEmpresaCorreo(int idEmpresaCorreo)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().GetById(idEmpresaCorreo);
        }



        /// <summary>
        /// Permite eliminar la cuenta del una empresa
        /// </summary>
        /// <param name="idEmpresaCorreo"></param>
        public void EliminarCuentaCorreoTramite(int idEmpresaCorreo, int idEmpresa, int idModulo)
        {
            try
            {
                FactorySic.GetSiEmpresaCorreoRepository().Delete(idEmpresaCorreo);

                //Verificamos

                List<SiEmpresaCorreoDTO> list = this.ObtenerCorreosPorEmpresa(idEmpresa, idModulo);

                if (list.Count == 0)
                {
                    string indicador = ConstantesAppServicio.NO;
                    FactorySic.GetSiEmpresaRepository().ActualizarDatosUsuarioTramite(idEmpresa, indicador, null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite eliminar la cuenta del una empresa
        /// </summary>
        /// <param name="idRpt"></param>
        public void EliminarRepresentante(int idRpt, string usuario)
        {
            try
            {
                FactorySic.GetSiRepresentanteRepository().DarBajaRepresentante(idRpt, usuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        

        /// <summary>
        /// Permite eliminar la cuenta del una empresa
        /// </summary>
        /// <param name="idEmpresaCorreo"></param>
        public void EliminarCuentaCorreo(int idEmpresaCorreo)
        {
            try
            {
                FactorySic.GetSiEmpresaCorreoRepository().Delete(idEmpresaCorreo);              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los representantes legales
        /// </summary>
        /// <returns></returns>
        public List<SiRepresentanteDTO> ObtenerRepresentantesLegales(int idEmpresa)
        {
            return FactorySic.GetSiRepresentanteRepository().ObtenerRepresentantesTitulares(idEmpresa);
        }

        public SiRepresentanteDTO ObtenerRepresentanteLegal(int idRepresentante)
        {
            return FactorySic.GetSiRepresentanteRepository().GetById(idRepresentante);
        }

        public List<SiEmpresaCorreoDTO> ObtenerCorreosNotificacion(string ruc)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().ObtenerCorreosNotificacion(ruc);
        }

        public List<String> ObtenerListaCorreosNotificacion(string ruc,string tipo)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().ObtenerListaCorreosNotificacion(ruc,tipo);
        }

        public void ActualizarRepresentante(int idRepresentante, string indicador)
        {
            try
            {
                FactorySic.GetSiRepresentanteRepository().ActualizarNotificacion(idRepresentante, indicador);
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void ActualizarRepresentante(SiRepresentanteDTO representante)
        {
            try
            {
                if(representante.Rptecodi != 0)
                {
                    representante.Rptefecmodificacion = DateTime.Now;
                    FactorySic.GetSiRepresentanteRepository().ActualizarRepresentante(representante);
                }
                else
                {
                    representante.Rptefeccreacion = DateTime.Now;
                    representante.Rptetipo = "L";
                    representante.Rptetiprepresentantelegal = "T";
                     representante.Rptebaja = ConstantesAppServicio.NO;
                    FactorySic.GetSiRepresentanteRepository().Save(representante);
                }
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int GrabarSolicitudNoIntegrante(SiEmpresaDTO empresa, SiRepresentanteDTO representante,
            SiEmpresaCorreoDTO contacto1, SiEmpresaCorreoDTO contacto2, SiEmpresaCorreoDTO contacto3, string usuario)
        {
            try
            {
                int resultado = 1;

                SiEmpresaDTO entityEmpresa = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaPorRuc(empresa.Emprruc);

                bool flag = this.ValidarAccesoEmpresaCyDOC(empresa.Emprruc);

                if (!flag)
                {
                    int idEmpresa = 0;

                    if (entityEmpresa != null)
                    {
                        idEmpresa = entityEmpresa.Emprcodi;
                        //entityEmpresa.Emprsein = ConstantesAppServicio.SI;

                        if (string.IsNullOrEmpty(entityEmpresa.Emprrazsocial))
                        {
                            entityEmpresa.Emprrazsocial = empresa.Emprrazsocial;
                        }

                        FactorySic.GetSiEmpresaRepository().Update(entityEmpresa);

                        // Registamos en el caso no exista la empresa en el cydoc
                        this.RegistrarEmpresaCyDOC(entityEmpresa);
                    }
                    else
                    {
                        empresa.Emprcoes = ConstantesAppServicio.NO;
                        empresa.Emprsein = ConstantesAppServicio.NO;
                        empresa.Lastuser = usuario;
                        empresa.Lastdate = DateTime.Now;
                        empresa.Emprusucreacion = usuario;
                        empresa.Emprfeccreacion = DateTime.Now;
                        empresa.Emprestado = ConstantesAppServicio.Activo;
                        empresa.EmpresaEstado = ConstantesAppServicio.Activo;
                        //empresa.Emprsein = ConstantesAppServicio.SI;
                        idEmpresa = FactorySic.GetSiEmpresaRepository().Save(empresa);
                        empresa.Emprcodi = idEmpresa;

                        //- Envio de correo por registro de empresa a amontalva
                        (new EmpresaAppServicio()).EnviarCorreoEmpresa(empresa);

                        //- Integración con CyDOC
                        this.RegistrarEmpresaCyDOC(empresa);
                    }

                    //- Datos del representante legal
                    representante.Emprcodi = idEmpresa;
                    representante.Rpteusucreacion = usuario;
                    representante.Rptefeccreacion = DateTime.Now;
                    representante.Rpteindnotic = ConstantesAppServicio.SI;
                    representante.Rptetipo = "L";
                    representante.Rptetiprepresentantelegal = "T";
                    representante.Rptebaja = ConstantesAppServicio.NO;
                    FactorySic.GetSiRepresentanteRepository().Save(representante);

                    //- Datos de persona de contacto 1

                    contacto1.Emprcodi = idEmpresa;
                    contacto1.Modcodi = this.IdModuloTramite;
                    contacto1.Empcorindnotic = ConstantesAppServicio.SI;
                    contacto1.Empcorestado = ConstantesAppServicio.Activo;
                    contacto1.Lastuser = usuario;
                    FactorySic.GetSiEmpresaCorreoRepository().Save(contacto1);

                    if (contacto2 != null)
                    {
                        //- Datos de persona de contacto 2
                        contacto2.Emprcodi = idEmpresa;
                        contacto2.Modcodi = this.IdModuloTramite;
                        contacto2.Empcorindnotic = ConstantesAppServicio.SI;
                        contacto2.Empcorestado = ConstantesAppServicio.Activo;
                        contacto2.Lastuser = usuario;
                        FactorySic.GetSiEmpresaCorreoRepository().Save(contacto2);
                    }

                    if (contacto3 != null)
                    {
                        //- Datos de persona de contacto 3
                        contacto3.Emprcodi = idEmpresa;
                        contacto3.Modcodi = this.IdModuloTramite;
                        contacto3.Empcorestado = ConstantesAppServicio.Activo;
                        contacto3.Empcorindnotic = ConstantesAppServicio.SI;
                        contacto3.Lastuser = usuario;
                        FactorySic.GetSiEmpresaCorreoRepository().Save(contacto3);
                    }

                    this.GenerarCredencialIntegrante(idEmpresa, this.IdModuloTramite);
                    this.NotificarCreacionCredencial(empresa);
                    //- Notificar a los usuarios acerca de la creación del nuevo usuario

                    resultado = 1;
                }
                else
                {
                    resultado = 2;
                }

                // Llamada a creación de registro de empresa

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void NotificarCreacionCredencial(SiEmpresaDTO entity)
        {
            string mensaje = this.HtmlNotificarCreacionCredencial(entity);

            string sTo = ConfigurationManager.AppSettings["NotificationCredTramVirtListUsuarioBCC"].ToString();
            //string sTo = "amontalva@coes.org.pe;mvelasquez@coes.org.pe;leyla.gomero@coes.org.pe";

            List<string> listTo = new List<string>();
            listTo = sTo.Split(';').ToList();
            List<string> listBCc = new List<string>();
            List<string> listCC = new List<string>();

            Base.Tools.Util.SendEmail(listTo, listCC, listBCc, "Notificación de creación de acceso al Portal de Trámite Virtual", mensaje, "webapp@coes.org.pe");
        }

        public string HtmlNotificarCreacionCredencial(SiEmpresaDTO entity)
        {


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
		                            <td class='titulo'>Creación de credencial para acceso al Portal de Trámite Virtual de empresa no integrante del COES</td>
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
				                            </tr>
				                            <tr>
					                           <td class='tdcelda'>{0}</td>
					                           <td class='tdcelda'>{1}</td>
					                           <td class='tdcelda'>{2}</td>
					              						
				                           </tr>			                          
			                            </table>
                                        <br />
                                        <br />
		                            </td>
	                            </tr>	                          
                            </table>
                            </body>
                            </html>
                            ";

            String mensaje = String.Format(HtmlCuerpo, entity.Emprnomb, entity.Emprrazsocial, entity.Emprruc);
            mensaje = mensaje.Replace("[", "{");
            mensaje = mensaje.Replace("]", "}");


            return mensaje;
        }

        //public void CrearCredencialesFicticias()
        //{

        //    Guid guid = Guid.NewGuid();
        //    string uuid = guid.ToString();
        //    string random = GenerarPassword();
        //    string password = GenerarPassword(random, uuid);

        //    //SiEmpresaDTO entity = new SiEmpresaDTO { Emprrazsocial = "EMPRESA DE PRUEBA 01", Emprruc = "11111111110" };
        //    //this.GrabarUsuario(entity, "cbutron@coes.org.pe", uuid, password);
        //    //SiRepresentanteDTO item = new SiRepresentanteDTO { Rptenombres  = "César", Rpteapellidos = "Butrón", Emprruc = "11111111110",
        //    //    Rptecorreoelectronico = "cbutron@coes.org.pe"
        //    //};

        //    SiEmpresaDTO entity = new SiEmpresaDTO { Emprrazsocial = "EMPRESA DE PRUEBA 03", Emprruc = "01111111111" };
        //    this.GrabarUsuario(entity, "psuclla@coes.org.pe", uuid, password);
        //    SiRepresentanteDTO item = new SiRepresentanteDTO
        //    {
        //        Rptenombres = "Patricia",
        //        Rpteapellidos = "Suclla",
        //        Emprruc = "01111111111",
        //        Rptecorreoelectronico = "psuclla@coes.org.pe"
        //    };

        //    //EnviarCorreoIntegrante(item, random);
        //}

        public List<string> ObtenerCorreosPorEmpresa(int idEmpresa)
        {
            List<string> correos = new List<string>();
            List<SiRepresentanteDTO> representantes = this.ObtenerRepresentantesLegales(idEmpresa).
                Where(x=>x.Rpteindnotic == ConstantesAppServicio.SI).ToList();
            List<SiEmpresaCorreoDTO> cuentas = this.ObtenerCorreosPorEmpresa(idEmpresa, this.IdModuloTramite).
                Where(x=>x.Empcorindnotic == ConstantesAppServicio.SI).ToList();

            foreach (SiRepresentanteDTO entity in representantes)
            {
                correos.Add(entity.Rptecorreoelectronico);
            }

            foreach (SiEmpresaCorreoDTO entity in cuentas)
            {
                correos.Add(entity.Empcoremail);
            }

            return correos;
        }



        //public void GrabarUsuarioCOE()
        //{
        //    Guid guid = Guid.NewGuid();
        //    string uuid = guid.ToString();
        //    string random = GenerarPassword();
        //    string password = GenerarPassword(random, uuid);

        //    //SiEmpresaDTO entity = new SiEmpresaDTO { Emprrazsocial = "EMPRESA DE PRUEBA 01", Emprruc = "11111111110" };
        //    //this.GrabarUsuario(entity, "cbutron@coes.org.pe", uuid, password);

        //    SiEmpresaDTO entity = new SiEmpresaDTO { Emprrazsocial = "EMPRESA DE PRUEBAS 02", Emprruc = "11111111112" };
        //    this.GrabarUsuario(entity, "mgonzales@coes.org.pe", uuid, password);

            
        //}

        /// <summary>
        /// Permite grabar la nueva cuenta
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public void GrabarCuenta(int idEmpresa, int idEmpresaCorreo, string email, string usuario, int IdModulo)
        {
            try
            {
                if (idEmpresaCorreo == 0)
                {
                    SiEmpresaCorreoDTO entity = new SiEmpresaCorreoDTO();
                    entity.Emprcodi = idEmpresa;
                    entity.Empcorestado = ConstantesAppServicio.Activo;
                    entity.Empcoremail = email;
                    entity.Modcodi = IdModulo;
                    entity.Lastuser = usuario;
                    FactorySic.GetSiEmpresaCorreoRepository().Save(entity);
                }
                else
                {
                    SiEmpresaCorreoDTO entity = FactorySic.GetSiEmpresaCorreoRepository().GetById(idEmpresaCorreo);                    
                    entity.Empcoremail = email;
                    entity.Lastuser = usuario;
                    FactorySic.GetSiEmpresaCorreoRepository().Update(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void GrabarContacto(SiEmpresaCorreoDTO entity)
        {
            try
            {
                if (entity.Empcorcodi == 0)
                {

                    List<SiEmpresaCorreoDTO> list = this.ObtenerCorreosPorEmpresa(entity.Emprcodi, IdModulo);
                    
                    FactorySic.GetSiEmpresaCorreoRepository().Save(entity);

                    if (list.Count == 0)
                    {
                        string indicador = ConstantesAppServicio.SI;
                        FactorySic.GetSiEmpresaRepository().ActualizarDatosUsuarioTramite(entity.Emprcodi, indicador, null);
                    }
                }
                else
                {                  
                    FactorySic.GetSiEmpresaCorreoRepository().Update(entity);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #region Creacion de Cuenta

    /// <summary>
    /// Método que permite generar las credenciales para los agentes
    /// </summary>
    public int CrearCredenciales(int idEmpresa)
        {
            try
            {
                Guid guid = Guid.NewGuid();
                string uuid = guid.ToString();
                string random = GenerarPassword();

                string password = GenerarPassword(random, uuid);

                List<SiEmpresaCorreoDTO> listaCorreos = this.ObtenerCorreosPorEmpresa(idEmpresa, IdModulo);
                SiEmpresaDTO entity = FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);

                if (listaCorreos.Count > 0)
                {
                    string email = string.Empty;
                    GrabarUsuario(entity, listaCorreos[0].Empcoremail, uuid, password, IdModulo);
                    EnviarCorreo(entity, random);
                    return 1;
                }
                else
                {
                    return -2;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    /// <summary>
    /// Método para el envío del correo
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="password"></param>
    public void EnviarCorreo(SiEmpresaDTO entity, string password)
        {
            List<SiEmpresaCorreoDTO> list = this.ObtenerCorreosPorEmpresa(entity.Emprcodi, IdModulo); 
            
            string mensaje = ObtenerCuerpoCorreo(entity, password);
            mensaje = mensaje.Replace("[", "{");
            mensaje = mensaje.Replace("}", "]");
            List<string> emailsTo = new List<string>();
            string asunto = "Acceso al Portal de Proveedores del COES";
            string pathImagen = "fondo_proveedores.png";

            foreach (SiEmpresaCorreoDTO entidad in list)
            {
                if (COES.Base.Tools.Util.ValidarEmail(entidad.Empcoremail))
                {
                    emailsTo.Add(entidad.Empcoremail);
                }
            }

            this.EnviarCorreoCredencial(emailsTo, asunto, mensaje, pathImagen,0);
        }
        /// <summary>
        /// Generar la estructura del passoword
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public  String GenerarPassword(String password, String salt)
        {
            String salt1 = SHA256(salt);
            String salt2 = SHA256(salt);

            return SHA512(salt1 + password + salt2);
            //return Encrypt(salt1+ password+salt2 );
        }

        /// <summary>
        /// Encripta un texto en sha256
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public  string SHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Encripta un texto en sha512
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public  string SHA512(string str)
        {
            SHA512 sha512 = SHA512Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha512.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        /// <summary>
        /// Permite generar un password
        /// </summary>
        /// <returns></returns>
        public  string GenerarPassword()
        {
            GeneradorPassword generador = new GeneradorPassword(6, 40, 30, 30);
            return generador.GetNewPassword();
        }     
        /// <summary>
        /// Permite grabar el usuario
        /// </summary>
        public void GrabarUsuario(SiEmpresaDTO entity, string correo, string uuid, string password,int idModulo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["ConnCyDOC"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            SqlCommand command = conn.CreateCommand();

            command.CommandText = "select count(*) from seg_usuario where usuario_numero_documento = '" + entity.Emprruc + "'";
            object resultCount = command.ExecuteScalar();
            int contador = Convert.ToInt32(resultCount);            

            if (contador == 0)
            {
                command.CommandText = "select isnull(max(id_usuario), 0) + 1 from seg_usuario";
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    int id = Convert.ToInt32(result);

                    string tramite = command.CommandText = @"insert into seg_usuario (
                                        id_usuario,
                                        activo,
                                        correo,
                                        password,
                                        usuario_numero_documento,
                                        uuid,
                                        razon_social,
                                        tipo_documento,
                                        tipo_usuario,
                                        tipo_cliente
                                        )
                                        values
                                        (
                                        @id_usuario,
                                        @activo,
                                        @correo,
                                        @password,
                                        @usuario_numero_documento,
                                        @uuid,
                                        @razon_social,
                                        @tipo_documento,
                                        @tipo_usuario,
                                        @tipo_cliente
                                        )";
                    string proveedor = command.CommandText = @"insert into seg_usuario (
                                        id_usuario,
                                        activo,
                                        correo,
                                        password2,
                                        usuario_numero_documento,
                                        uuid2,
                                        razon_social,
                                        tipo_documento,
                                        tipo_usuario,
                                        tipo_cliente
                                        )
                                        values
                                        (
                                        @id_usuario,
                                        @activo,
                                        @correo,
                                        @password,
                                        @usuario_numero_documento,
                                        @uuid,
                                        @razon_social,
                                        @tipo_documento,
                                        @tipo_usuario,
                                        @tipo_cliente
                                        )";
         
                    if (String.IsNullOrEmpty(entity.Emprrazsocial)) command.Parameters.Add(new SqlParameter("@razon_social", DBNull.Value));
                    else 
                        command.Parameters.Add(new SqlParameter("@razon_social", entity.Emprrazsocial));

                    command.Parameters.Add(new SqlParameter("@tipo_documento", "juridica"));
                    command.Parameters.Add(new SqlParameter("@tipo_cliente", "tipo_agente"));

                    string emprindproveedor = entity.Emprindproveedor;
                    string emprcoes = entity.Emprcoes;

                    if (emprcoes == "S" & emprindproveedor=="S")
                    {
                        if (idModulo == 34) command.CommandText = tramite;
                        else command.CommandText = proveedor;

                        command.Parameters.Add(new SqlParameter("@tipo_usuario", "tipo_proveedor_integrante"));
                    }
                    else if (emprcoes == "S")
                    {
                        if (idModulo == 34) command.CommandText = tramite;
                        else command.CommandText = proveedor;

                        command.Parameters.Add(new SqlParameter("@tipo_usuario", "tipo_integrante"));
                    }
                    else if (emprindproveedor == "S")
                    {
                        if (idModulo == 34) command.CommandText = tramite;
                        else command.CommandText = proveedor;
                        command.Parameters.Add(new SqlParameter("@tipo_usuario", "tipo_proveedor"));
                    }
                    else
                    {
                        command.CommandText = tramite;
                        command.Parameters.Add(new SqlParameter("@tipo_usuario", "tipo_integrante"));
                    }

                    command.Parameters.Add(new SqlParameter("@id_usuario", id));
                    command.Parameters.Add(new SqlParameter("@activo", 1));
                    command.Parameters.Add(new SqlParameter("@correo", correo));
                    command.Parameters.Add(new SqlParameter("@password", password));
                    command.Parameters.Add(new SqlParameter("@usuario_numero_documento", entity.Emprruc));
                    command.Parameters.Add(new SqlParameter("@uuid", uuid));

                    int indicador = command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    command.CommandText = "update cliente set correo = '" + correo  + "' where numero_identificacion = '" + entity.Emprruc + "'";
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                string tramite = "update seg_usuario  set password = @password, uuid = @uuid, correo = @correo, razon_social = @razon_social, tipo_usuario = @tipo_usuario,tipo_cliente =@tipo_cliente where usuario_numero_documento = @usuario_numero_documento";
                string proveedor = "update seg_usuario  set password2 = @password, uuid2 = @uuid, correo = @correo, razon_social = @razon_social,tipo_usuario = @tipo_usuario,tipo_cliente =@tipo_cliente where usuario_numero_documento = @usuario_numero_documento";

                string emprindproveedor = entity.Emprindproveedor;
                string emprcoes = entity.Emprcoes;
                
                if (emprcoes == "S" && emprindproveedor == "S")
                {
                    if (idModulo == 34) command.CommandText = tramite;
                    else command.CommandText = proveedor;

                    command.Parameters.Add(new SqlParameter("@tipo_usuario", "tipo_proveedor_integrante"));
                }
                else if (emprcoes == "S")
                {
                    if(idModulo == 34) command.CommandText = tramite;
                    else command.CommandText = proveedor;
                    
                    command.Parameters.Add(new SqlParameter("@tipo_usuario", "tipo_integrante"));
                }
                else if (emprindproveedor == "S")
                {
                    if (idModulo == 34) command.CommandText = tramite;
                    else command.CommandText = proveedor;
                    command.Parameters.Add(new SqlParameter("@tipo_usuario", "tipo_proveedor"));
                }
                else
                {
                    command.CommandText = tramite;
                    command.Parameters.Add(new SqlParameter("@tipo_usuario", "tipo_integrante"));
                }

                command.Parameters.Add(new SqlParameter("@password", password));
                command.Parameters.Add(new SqlParameter("@uuid", uuid));
                command.Parameters.Add(new SqlParameter("@correo", correo));
                command.Parameters.Add(new SqlParameter("@razon_social", entity.Emprrazsocial));
                command.Parameters.Add(new SqlParameter("@tipo_cliente", "tipo_agente"));
                command.Parameters.Add(new SqlParameter("@usuario_numero_documento", entity.Emprruc));

                int indicador = command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.CommandText = "update cliente set correo = '" + correo + "' where numero_identificacion = '" + entity.Emprruc + "'";
                command.ExecuteNonQuery();
            }
        }

        #region Notificaciones


        public void EnviarNotificacionAgente()
        {
            List<EntidadEnvio> entitys = this.ObtenerEmpresasNotificacionAgente();

            foreach (EntidadEnvio entity in entitys)
            {
                string mensaje = ObtenerCuerpoNotificacionAgenteCOES(entity);
                mensaje = mensaje.Replace("[", "{");
                mensaje = mensaje.Replace("}", "]");                
                string asunto = "Documento enviado por el COES - Expediente N° " +  entity.NroExpediente;
                string pathImagen = "fondo_tramitevirtual.png";
                SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaPorRuc(entity.Ruc);
                List<string> correos = this.ObtenerCorreosPorEmpresa(empresa.Emprcodi);
                this.EnviarCorreoCredencial(correos, asunto, mensaje, pathImagen, 0);
                this.ActualizarEstadoNotificacion(entity.Codigo);
            }
        }

        /// <summary>
        /// Permite enviar las notificaciones al Agente
        /// </summary>
        /// <param name="expediente"></param>
        /// <param name="tipo"></param>
        public int EnviarNotificacionPortalTramite(int expediente, int tipo)
        {
            try
            {
                if (expediente >= 83592)
                {
                    EntidadEnvio entity = this.ObtenerDatosExpedienteNotificacion(expediente);
                    SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaPorRuc(entity.Ruc);
                    List<string> correos = this.ObtenerCorreosPorEmpresa(empresa.Emprcodi);
                    entity.Empresa = empresa.Emprrazsocial;
                    string asunto = this.ObtenerAsuntoCorreo(entity, tipo);
                    string cuerpo = this.ObtenerCuerpoNotificacionTramite(entity, tipo);
                    string pathImagen = "fondo_tramitevirtual.png";
                    this.EnviarCorreoCredencial(correos, asunto, cuerpo, pathImagen, 0);

                    return 1;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public void EnviarCorreoIntegrante(SiEmpresaDTO entity, List<string> cuentas, string password)
        {
            string mensaje = ObtenerCuerpoCorreoIntegrante(entity, password);
            mensaje = mensaje.Replace("[", "{");
            mensaje = mensaje.Replace("}", "]");
            List<string> emailsTo = new List<string>();
            string asunto = "Acceso al Portal de Trámite Virtual del COES";
            string pathImagen = "fondo_tramitevirtual.png";

            foreach (string cuenta in cuentas)
            {
                emailsTo.Add(cuenta.Trim());
            }

            this.EnviarCorreoCredencial(emailsTo, asunto, mensaje, pathImagen, 1);
        }
        /// <summary>
        /// Permite enviar un correo electrónico
        /// </summary>
        /// <param name="mailsTo"></param>
        /// <param name="mailsCc"></param>
        /// <param name="mailsBcc"></param>
        /// <param name="subject"></param>
        /// <param name="mensaje"></param>
        /// <param name="sfrom"></param>
        /// <param name="files"></param>
        public void EnviarCorreoCredencial(List<string> mailsTo, string subject, string mensaje, string pathImagen, int indicador)
        {
            MailMessage correo = new MailMessage();

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mensaje, null, "text/html");
            //LinkedResource topImage = null;
            if(ConfigurationManager.AppSettings["FolderDataImages"] != null)
            {
                LinkedResource topImage = new LinkedResource(FileServer.DownloadToStream(pathImagen, ConfigurationManager.AppSettings["FolderDataImages"].ToString()));
                topImage.ContentId = "topImageID";
                htmlView.LinkedResources.Add(topImage);
            }

            correo.From = new MailAddress("webapp@coes.org.pe", "Comunicaciones COES");

            //- Agregamos los destinatarios
            foreach (string mailTo in mailsTo)
            {               
                if (COES.Base.Tools.Util.ValidarEmail(mailTo.Trim()))
                {
                    correo.To.Add(mailTo.Trim());
                }
            }

            string emailsBcc = string.Empty;
            if (indicador == 1)
            {
                emailsBcc = ConfigurationManager.AppSettings["EmailCredTramVirtListUsuarioBCC"].ToString();
                //emailsBcc = "amontalva@coes.org.pe;leyla.gomero@coes.org.pe;webapp@coes.org.pe";
            }
            else
            {
                emailsBcc = "webapp@coes.org.pe";
            }
            
            string[] itemsBcc = emailsBcc.Split(';');

            //- Agregamos los destinatarios con copia oculta
            foreach (string mailBcc in itemsBcc)
                correo.Bcc.Add(mailBcc);

            correo.Subject = subject;
            correo.AlternateViews.Add(htmlView);
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["EmailServer"].ToString();
            smtp.Credentials = new NetworkCredential(string.Empty, string.Empty);

            smtp.Send(correo);
        }


        /// <summary>
        /// Permite obtener los datos de la empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// /// <param name="idModulo"></param>
        public int GenerarCredencialIntegrante(int idEmpresa,int idModulo)
        {
            try
            {
                string email = string.Empty;
                SiRepresentanteDTO representate = this.ObtenerRepresentantesLegales(idEmpresa).Where(x => x.Rptetiprepresentantelegal == "Titular").FirstOrDefault();
                List<string> cuentas = this.ObtenerCorreosPorEmpresa(idEmpresa);
                SiEmpresaDTO entity = (new EmpresaAppServicio()).ObtenerEmpresa(idEmpresa);
                
                if (representate != null)
                {
                    if (!string.IsNullOrEmpty(representate.Rptecorreoelectronico))
                    {
                        email = representate.Rptecorreoelectronico.Trim();
                    }
                }
                if (string.IsNullOrEmpty(email))
                {
                    if (cuentas.Count() > 0)
                    {
                        email = cuentas[0];
                    }
                }

                if (!string.IsNullOrEmpty(email))
                {
                    Guid guid = Guid.NewGuid();
                    string uuid = guid.ToString();
                    string random = GenerarPassword();

                    string password = GenerarPassword(random, uuid);

                    this.GrabarUsuario(entity, email, uuid, password, idModulo);
                    EnviarCorreoIntegrante(entity, cuentas, random);
                    string indicador = ConstantesAppServicio.SI;
                    DateTime fecha = DateTime.Now;
                    FactorySic.GetSiEmpresaRepository().ActualizarDatosUsuarioTramite(entity.Emprcodi, indicador, fecha);

                    return 1;
                }
                else
                {
                    return -2;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public string ObtenerAsuntoCorreo(EntidadEnvio entidad, int tipo)
        {
            string asunto = string.Empty;

            switch (tipo)
            {
                case 1:
                    {
                        asunto = "Cargo de trámite registrado - Expediente N° " + entidad.NroExpediente;
                        break;
                    }
                case 2:
                    {
                        asunto = "Rechazo de trámite - Expediente N° " + entidad.NroExpediente;
                        break;
                    }
                case 3:
                    {
                        asunto = "Finalización de trámite - Expediente N° " + entidad.NroExpediente;
                        break;
                    }
            }
            return asunto;
        }

        public string ObtenerCuerpoNotificacionTramite(EntidadEnvio entidad, int tipo)
        {
            string mensaje = string.Empty;
            string cuerpo = string.Empty;

            if (tipo == 1)
            {
                cuerpo = @"
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
			                <td class='celda' colspan='2'>Estimado(a):,<br /><br /></td>			       
		                </tr>
		                <tr>			        
			                <td class='celdacon' colspan='2'>

Gracias por contactarnos, queremos confirmarle que hemos recibido su trámite, para el seguimiento tome en cuenta los siguientes datos:<br /><br />
                                </td>
		                </tr>	                
                        <tr>
                            <td class='celdacon'  style='width:200px'><strong>Empresa:</strong></td>
                            <td class='celdacon1' style='width:400px'>{0}</td>
                        </tr>       
                        <tr>
                            <td class='celdacon'  style='width:200px'><strong>RUC:</strong></td>
                            <td class='celdacon1' style='width:400px'>{1}</td>
                        </tr>   
                        <tr>
                            <td class='celdacon'><strong>Número de Expediente:</strong></td>
                            <td class='celdacon1'>{2}</td>
                        </tr> 
                         <tr>
                            <td class='celdacon'><strong>Asunto:</strong></td>
                            <td class='celdacon1'>{3}</td>
                        </tr> 
                        <tr>
                            <td class='celdacon'><strong>Fecha de envío:</strong></td>
                            <td class='celdacon1'>{4}</td>
                        </tr> 
                        <tr>
                            <td class='celdacon'><strong>Estado del trámite:</strong></td>
                            <td class='celdacon1'>{5}</td>
                        </tr> 
                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
                                <br />
Estaremos procediendo con la atención de su trámite.
                            </td>
                        </tr>

                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
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

                mensaje = String.Format(cuerpo, entidad.Empresa, entidad.Ruc, entidad.NroExpediente, entidad.Asunto, entidad.Fecha, entidad.Estado);
            }

            if (tipo == 2)
            {

                cuerpo = @"
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
			                <td class='celda' colspan='2'>Estimado(a):,<br /><br /></td>			       
		                </tr>
		                <tr>			        
			                <td class='celdacon' colspan='2'>

Queremos comunicarle que su trámite ha sido rechazado, los datos del trámite se muestran a continuación:<br /><br />
                                </td>
		                </tr>	                
                        <tr>
                            <td class='celdacon'  style='width:200px'><strong>Empresa:</strong></td>
                            <td class='celdacon1' style='width:400px'>{0}</td>
                        </tr>       
                        <tr>
                            <td class='celdacon'  style='width:200px'><strong>RUC:</strong></td>
                            <td class='celdacon1' style='width:400px'>{1}</td>
                        </tr>   
                        <tr>
                            <td class='celdacon'><strong>Número de Expediente:</strong></td>
                            <td class='celdacon1'>{2}</td>
                        </tr> 
                         <tr>
                            <td class='celdacon'><strong>Asunto:</strong></td>
                            <td class='celdacon1'>{3}</td>
                        </tr>                        
                        <tr>
                            <td class='celdacon'><strong>Estado del trámite:</strong></td>
                            <td class='celdacon1'>{4}</td>
                        </tr> 
                        <tr>
                            <td class='celdacon'><strong>Motivo de rechazo:</strong></td>
                            <td class='celdacon1'>{5}</td>
                        </tr> 
                        

                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
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

                mensaje = String.Format(cuerpo, entidad.Empresa, entidad.Ruc, entidad.NroExpediente, entidad.Asunto, entidad.Estado, entidad.MotivoRechazo);
            }

            if (tipo == 3)
            {
                cuerpo = @"
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
			                <td class='celda' colspan='2'>Estimado(a):,<br /><br /></td>			       
		                </tr>
		                <tr>			        
			                <td class='celdacon' colspan='2'>

Queremos comunicarle que su trámite ha sido atendido, los datos del trámite se muestran a continuación:<br /><br />
                                </td>
		                </tr>	                
                        <tr>
                            <td class='celdacon'  style='width:200px'><strong>Empresa:</strong></td>
                            <td class='celdacon1' style='width:400px'>{0}</td>
                        </tr>       
                        <tr>
                            <td class='celdacon'  style='width:200px'><strong>RUC:</strong></td>
                            <td class='celdacon1' style='width:400px'>{1}</td>
                        </tr>   
                        <tr>
                            <td class='celdacon'><strong>Número de Expediente:</strong></td>
                            <td class='celdacon1'>{2}</td>
                        </tr> 
                         <tr>
                            <td class='celdacon'><strong>Asunto:</strong></td>
                            <td class='celdacon1'>{3}</td>
                        </tr>                        
                        <tr>
                            <td class='celdacon'><strong>Estado del trámite:</strong></td>
                            <td class='celdacon1'>{4}</td>
                        </tr> 
                     
                        

                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
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

                mensaje = String.Format(cuerpo, entidad.Empresa, entidad.Ruc, entidad.NroExpediente, entidad.Asunto, entidad.Estado);
            }

            return mensaje;
        }

        #endregion


        public string ObtenerCuerpoCorreo(SiEmpresaDTO empresa, string clave)
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
	                <table cellspacing='0' border='0' width='100%' >		
		                <tr>
			                <td class='celda' colspan='2'>Estimado(a):<br /><br /></td>			       
		                </tr>
		                <tr>			        
			                <td class='celdacon' colspan='2'>
                                Se ha creado un usuario para que pueda acceder a nuestro Portal de Proveedores, desde el cual podrá gestionar los pagos de sus facturas.<br />
                                Los datos de acceso son los siguientes:<br /><br /></td>
		                </tr>	                
                        <tr>
                            <td class='celdacon'  style='width:300px'>Empresa:</td>
                            <td class='celdacon1' style='width:400px'>{0}</td>
                        </tr>       
                        <tr>
                            <td class='celdacon'  style='width:300px'>Usuario:</td>
                            <td class='celdacon1' style='width:400px'>{1}</td>
                        </tr>                       
                        <tr>
                            <td class='celdacon'>Contraseña:</td>
                            <td class='celdacon1'>{2}</td>
                        </tr>                                    

                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
                                <br />
Para cualquier consulta favor de comunicarse con Eliana Naupay (enaupay@coes.org.pe o al 6118585- anexo 631). <br />
<br />

                                <table style='width:500px' cellpadding='4'>
<tr>
<td style='width:45%; background-color:#3074B7; text-align:center;'><a href='https://proveedores.coes.org.pe/seguridad-proveedores/login' style='color:white; text-decoration:none; font-weight:bold'>Acceder al Portal de Proveedores</a></td>
<td style='width:10%'></td>
<td style='width:45% ; background-color:#3074B7; text-align:center;'> <a href='http://www.coes.org.pe/wcoes/manuales/Manual_de_Usuario_Proveedores_v1.0.pdf' style='color:white; text-decoration:none; font-weight:bold'>Descargar Manual de Usuario</a></td>
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

            return String.Format(mensaje, empresa.Emprrazsocial, empresa.Emprruc, clave);
        }
        /// <summary>
        /// Permite generar el correo electronico para el representante legal
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string ObtenerCuerpoCorreoIntegrante(SiEmpresaDTO entity, string password)
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
			                <td class='celda' colspan='2'>Estimado(a): ,<br /><br /></td>			       
		                </tr>
		                <tr>			        
			                <td class='celdacon' colspan='2'>

Se ha creado un usuario para que su empresa pueda acceder a nuestro Portal de Trámite Virtual , en donde podrá iniciar el registro de trámites virtuales.<br />
Los datos de acceso son los siguientes:<br /><br />
                                </td>
		                </tr>	                
                        <tr>
                            <td class='celdacon'  style='width:300px'>Empresa:</td>
                            <td class='celdacon1' style='width:400px'>{0}</td>
                        </tr>       
                        <tr>
                            <td class='celdacon'  style='width:300px'>Usuario:</td>
                            <td class='celdacon1' style='width:400px'>{1}</td>
                        </tr>                       
                        <tr>
                            <td class='celdacon'>Contraseña:</td>
                            <td class='celdacon1'>{2}</td>
                        </tr>                                    

                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
                                <br />
                                

Agradecemos que usted o quien designe realice las pruebas solicitadas de uso de nuestro portal.<br />
Para cualquier consulta favor de comunicarse al Ing. Alfredo Montalva (amontalva@coes.org.pe o al 6118585- anexo 620). <br />
<br />

<table style='width:500px' cellpadding='4'>
<tr>
<td style='width:45%; background-color:#3074B7; text-align:center;'><a href='https://tramitevirtual.coes.org.pe/seguridad/login' style='color:white; text-decoration:none; font-weight:bold'>Acceder al Portal</a></td>
<td style='width:10%'></td>
<td style='width:45% ; background-color:#3074B7; text-align:center;'> <a href='http://www.coes.org.pe/wcoes/manuales/Manual_de_Usuario_Tramite_Virtual_v1.0.pdf' style='color:white; text-decoration:none; font-weight:bold'>Descargar Manual de Usuario</a></td>
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

            return String.Format(mensaje, entity.Emprnomb, entity.Emprruc, password);
        }

        public string ObtenerCuerpoNotificacionAgenteCOES(EntidadEnvio entity)
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
			                <td class='celda' colspan='2'>Estimado(a):,<br /><br /></td>			       
		                </tr>
		                <tr>			        
			                <td class='celdacon' colspan='2'>

Se le notifica por este medio, que el COES le ha enviado documentación a través del Portal de Trámite Virtual con los siguiente datos:<br /><br />
                                </td>
		                </tr>	                
                        <tr>
                            <td class='celdacon'  style='width:200px'><strong>Empresa:</strong></td>
                            <td class='celdacon1' style='width:400px'>{0}</td>
                        </tr>       
                        <tr>
                            <td class='celdacon'  style='width:200px'><strong>RUC:</strong></td>
                            <td class='celdacon1' style='width:400px'>{1}</td>
                        </tr>                       
                        <tr>
                            <td class='celdacon'><strong>Número de Documento:</strong></td>
                            <td class='celdacon1'>{2}</td>
                        </tr>                                    
                        <tr>
                            <td class='celdacon'><strong>Número de Expediente:</strong></td>
                            <td class='celdacon1'>{3}</td>
                        </tr> 
 <tr>
                            <td class='celdacon'><strong>Asunto:</strong></td>
                            <td class='celdacon1'>{4}</td>
                        </tr> 
                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
                                <br />
Para poder consultar y descargar el documento debe ingresar a nuestro Portal de Trámite Virtual, para ello 
                                 haga <a href='https://tramitevirtual.coes.org.pe/seguridad/login'>clic aquí. </a>
                            </td>
                        </tr>

                        <tr>
                            <td colspan='2' class='celdacon1'>
                                <br />
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

            return String.Format(mensaje, entity.Empresa, entity.Ruc, entity.NroCarta, entity.NroExpediente, entity.Asunto);
        }

        public List<EntidadEnvio> ObtenerEmpresasNotificacionAgente()
        {
            List<EntidadEnvio> entitys = new List<EntidadEnvio>();
            string strConn = ConfigurationManager.ConnectionStrings["ConnCyDOC"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            SqlCommand command = conn.CreateCommand();

            command.CommandText = @"select datediff(minute,  men.fecha_envio, getdate()) as diferencia, id_msj_envio_electronica, men.fecha_envio, cli.numero_identificacion, cli.correo, cli.razon_social,
                                    doc.numero as documento, ex.numero as expediente ,
									ex.titulo as asunto
									from envio_mensajeria_electronica men inner join cliente cli
                                    on men.id_destinatario = cli.id_cliente
                                    inner join documento doc on doc.id_documento = men.id_documento
                                    inner join expediente ex on ex.id_expediente = men.id_expediente
                                    where datediff(minute,  men.fecha_envio, getdate()) <= 5 and men.notificado = 0
                                    ";
            
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                EntidadEnvio entity = new EntidadEnvio();

                int iRuc = dr.GetOrdinal("numero_identificacion");
                if (!dr.IsDBNull(iRuc)) entity.Ruc = dr.GetString(iRuc);

                int iRazonSocial = dr.GetOrdinal("razon_social");
                if (!dr.IsDBNull(iRazonSocial)) entity.Empresa = dr.GetString(iRazonSocial);

                int iCorreo = dr.GetOrdinal("correo");
                if (!dr.IsDBNull(iCorreo)) entity.Correo = dr.GetString(iCorreo);

                int iDocumento = dr.GetOrdinal("documento");
                if (!dr.IsDBNull(iDocumento)) entity.NroCarta = dr.GetString(iDocumento);

                int iExpediente = dr.GetOrdinal("expediente");
                if (!dr.IsDBNull(iExpediente)) entity.NroExpediente = dr.GetString(iExpediente);
                
                int iAsunto = dr.GetOrdinal("asunto");
                if (!dr.IsDBNull(iAsunto)) entity.Asunto = dr.GetString(iAsunto);

                int iCodigo = dr.GetOrdinal("id_msj_envio_electronica");
                if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToInt32(dr.GetValue(iCodigo));

                entitys.Add(entity);
            }
                       
            return entitys;
        }

        public void ActualizarEstadoNotificacion(int codigo)
        {

            try
            {
                string strConn = ConfigurationManager.ConnectionStrings["ConnCyDOC"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                command.CommandText = @"update envio_mensajeria_electronica set notificado = 1 where id_msj_envio_electronica = " + codigo;

                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public int RegistrarEmpresaCyDOC(SiEmpresaDTO entity)
        {
            try
            {
                List<EntidadEnvio> entitys = new List<EntidadEnvio>();
                string strConn = ConfigurationManager.ConnectionStrings["ConnCyDOC"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                command.CommandText = @"select count(*) from cliente where numero_identificacion = '" + entity.Emprruc +"'";

                int count = 0;
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    count = Convert.ToInt32(result);
                }

                if (count == 0)
                {
                    command.CommandText = "select isnull(max(id_cliente), 0) + 1 from cliente";
                    result = command.ExecuteScalar();
                    int id = 1;

                    if (result != null)
                    {
                        id = Convert.ToInt32(result);
                    }
                                       
                    command.CommandText = @"
                    insert into cliente(id_cliente,estado, fecha_creacion, numero_identificacion,razon_social, id_tipo_cliente, empresa_codigo)
                    values (@id_cliente, 'A', getdate(), @numero_identificacion, @razon_social, 1, @empresa_codigo)";
                    command.Parameters.Add(new SqlParameter("@id_cliente", id));
                    command.Parameters.Add(new SqlParameter("@numero_identificacion", entity.Emprruc));
                    command.Parameters.Add(new SqlParameter("@razon_social", entity.Emprrazsocial));
                    command.Parameters.Add(new SqlParameter("@empresa_codigo", entity.Emprcodi));
                 

                    int indicador = command.ExecuteNonQuery();
                }

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
               
            }
        }

        public bool ValidarAccesoEmpresaCyDOC(string ruc)
        {
            try
            {              
                string strConn = ConfigurationManager.ConnectionStrings["ConnCyDOC"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                command.CommandText = @"select count(*) from seg_usuario where usuario_numero_documento = '" + ruc + "'";

                int count = 0;
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    count = Convert.ToInt32(result);
                }

                if (count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }

        public EntidadEnvio ObtenerDatosExpedienteNotificacion(int idExpediente)
        {
            EntidadEnvio entity = null;
            string strConn = ConfigurationManager.ConnectionStrings["ConnCyDOC"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            SqlCommand command = conn.CreateCommand();

            command.CommandText = string.Format(@"select cli.numero_identificacion, expe.estado, expe.numero, expe.titulo , expe.fecha_creacion,
                                                (select mot.motivo from rechazo_tramite_web mot where mot.id_documento = doc.id_documento) as motivo
                                                from expediente expe inner join cliente cli on expe.id_cliente = cli.id_cliente 
                                                inner join documento doc on expe.id_expediente = doc.expediente
                                                where id_expediente = {0} ", idExpediente);

            SqlDataReader dr = command.ExecuteReader();

            if (dr.Read())
            {
                 entity = new EntidadEnvio();

                int iRuc = dr.GetOrdinal("numero_identificacion");
                if (!dr.IsDBNull(iRuc)) entity.Ruc = dr.GetString(iRuc);

                int iEstado = dr.GetOrdinal("estado");
                if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

                int iNroExpediente = dr.GetOrdinal("numero");
                if (!dr.IsDBNull(iNroExpediente)) entity.NroExpediente = dr.GetString(iNroExpediente);

                int iAsunto = dr.GetOrdinal("titulo");
                if (!dr.IsDBNull(iAsunto)) entity.Asunto = dr.GetString(iAsunto);

                int iMotivo = dr.GetOrdinal("motivo");
                if (!dr.IsDBNull(iMotivo)) entity.MotivoRechazo = dr.GetString(iMotivo);

                int iFecha = dr.GetOrdinal("fecha_creacion");
                if (!dr.IsDBNull(iFecha))
                {
                    entity.Fecha = dr.GetDateTime(iFecha).ToString("dd/MM/yyyy HH:mm:ss");
                }

                if (entity.Estado == "R")
                {
                    entity.Estado = "Registrado";
                }
                else if (entity.Estado == "X")
                {
                    if (!string.IsNullOrEmpty(entity.MotivoRechazo))
                    {
                        entity.Estado = "Rechazado";
                    }
                    else
                    {
                        entity.Estado = "Finalizado";
                    }
                }
                                             
            }

            return entity;

        }

        public void SincronizarEmpresas(SiEmpresaDTO entity)
        {
            try
            {
                string strConn = ConfigurationManager.ConnectionStrings["ConnCyDOC"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                command.CommandText = string.Format(@"USP_REGISTRAR_EMPRESA");
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RAZON_SOCIAL", entity.Emprrazsocial));
                command.Parameters.Add(new SqlParameter("@RUC", entity.Emprruc));
                command.Parameters.Add(new SqlParameter("@NOMBRE_EMPRESA", entity.Emprnomb));
                command.Parameters.Add(new SqlParameter("@ID_EMPRESA", entity.Emprcodi));
                command.Parameters.Add(new SqlParameter("@IND_ESTADO", (entity.Emprestado == "E" ? "I" : entity.Emprestado)));

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion
    }

    public class GeneradorPassword
    {
        /// <summary>
        /// Enumeración que permite conocer el tipo de juego de carácteres a emplear
        /// para cada carácter
        /// </summary>
        private enum TipoCaracterEnum { Minuscula, Mayuscula, Simbolo, Numero }

        #region Campos

        private int porcentajeMayusculas;
        private int porcentajeSimbolos;
        private int porcentajeNumeros;
        Random semilla;

        // Caracteres que pueden emplearse en la contraseña
        string caracteres = "abcdefghijklmnopqrstuvwxyz";
        string numeros = "0123456789";
        string simbolos = "$";

        // Cadena que contiene el password generado
        private StringBuilder password;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene o establece la longitud en carácteres de la contraseña a obtener
        /// </summary>
        public int LongitudPassword { get; set; }

        /// <summary>
        /// Obtiene o establece el porcentaje de carácteres en mayúsculas que 
        /// contendrá la contraseña
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un valor que no coincida con un porcentaje</exception>
        public int PorcentajeMayusculas
        {
            get { return porcentajeMayusculas; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("El porcentaje es un número entre 0 y 100");
                porcentajeMayusculas = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el porcentaje de símbolos que contendrá la contraseña
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un valor que no coincida con un porcentaje</exception>
        public int PorcentajeSimbolos
        {
            get { return porcentajeSimbolos; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("El porcentaje es un número entre 0 y 100");
                porcentajeSimbolos = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el número de caracteres numéricos que contendrá la contraseña
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un valor que no coincida con un porcentaje</exception>
        public int PorcentajeNumeros
        {
            get { return porcentajeNumeros; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("El porcentaje es un número entre 0 y 100");
                porcentajeNumeros = value;
            }
        }

        #endregion


        #region Constructores
        /// <summary>
        /// Constructor. La contraseña tendrá 8 caracteres, incluyendo una letra mayúscula, 
        /// un número y un símbolo
        /// </summary>
        public GeneradorPassword()
            : this(8)
        { }


        /// <summary>
        /// Constructor. La contraseña tendrá un 20% de caracteres en mayúsculas y otro tanto de 
        /// símbolos
        /// </summary>
        /// <param name="longitudCaracteres">Longitud en carácteres de la contraseña a obtener</param>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un porcentaje de caracteres especiales mayor de 100</exception>
        public GeneradorPassword(int longitudCaracteres)
            : this(longitudCaracteres, 20, 20, 20)
        { }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="longitudCaracteres">Longitud en carácteres de la contraseña a obtener</param>
        /// <param name="porcentajeMayusculas">Porcentaje a aplicar de caracteres en mayúscula</param>
        /// <param name="porcentajeSimbolos">Porcenta a aplicar de símbolos</param>
        /// <param name="porcentajeNumeros">Porcentaje de caracteres numéricos</param>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un porcentaje de caracteres especiales mayor de 100</exception>
        public GeneradorPassword(int longitudCaracteres, int porcentajeMayusculas, int porcentajeSimbolos, int porcentajeNumeros)
        {
            LongitudPassword = longitudCaracteres;
            PorcentajeMayusculas = porcentajeMayusculas;
            PorcentajeSimbolos = porcentajeSimbolos;
            PorcentajeNumeros = porcentajeNumeros;

            if (PorcentajeMayusculas + porcentajeSimbolos + PorcentajeNumeros > 100)
                throw new ArgumentOutOfRangeException(
                "La suma de los porcentajes de caracteres especiales no puede superar el " +
                "100%, es decir, no puede ser superior a la longitud de la contraseña");
            semilla = new Random(DateTime.Now.Millisecond);
        }

        #endregion


        #region Métodos públicos

        /// <summary>
        /// Obtiene el password
        /// </summary>
        /// <returns></returns>
        public string GetNewPassword()
        {
            GeneraPassword();
            return password.ToString();
        }


        /// <summary>
        /// Permite establecer el número de caracteres especiales que se quieren obtener
        /// </summary>
        /// <param name="numeroCaracteresMayuscula">Número de caracteres en mayúscula</param>
        /// <param name="numeroCaracteresNumericos">Número de caracteres numéricos</param>
        /// <param name="numeroCaracteresSimbolos">Número de caracteres de símbolos</param>
        public void SetCaracteresEspeciales(
            int numeroCaracteresMayuscula
            , int numeroCaracteresNumericos
            , int numeroCaracteresSimbolos)
        {
            // Comprobación de errores
            if (numeroCaracteresMayuscula
                    + numeroCaracteresNumericos
                    + numeroCaracteresSimbolos > LongitudPassword)
                throw new ArgumentOutOfRangeException(
                    "El número de caracteres especiales no puede superar la longitud del password");

            PorcentajeMayusculas = numeroCaracteresMayuscula * 100 / LongitudPassword;
            PorcentajeNumeros = numeroCaracteresNumericos * 100 / LongitudPassword;
            PorcentajeSimbolos = numeroCaracteresSimbolos * 100 / LongitudPassword;
        }



        /// <summary>
        /// Constructor. La contraseña tendrá 8 caracteres, incluyendo una letra mayúscula, 
        /// un número y un símbolo
        /// </summary>
        public static string GetPassword()
        {
            // Se crea un método estático para facilitar el uso
            GeneradorPassword gp = new GeneradorPassword();
            return gp.GetNewPassword();
        }


        #endregion


        #region Métodos de cálculo

        /// <summary>
        /// Método que genera el password. Primero crea una cadena de caracteres 
        /// en minúscula y va sustituyendo los caracteres especiales
        /// </summary>
        private void GeneraPassword()
        {
            // Se genera una cadena de caracteres en minúscula con la longitud del 
            // password seleccionado
            password = new StringBuilder(LongitudPassword);
            for (int i = 0; i < LongitudPassword; i++)
            {
                password.Append(GetCaracterAleatorio(TipoCaracterEnum.Minuscula));
            }

            // Se obtiene el número de caracteres especiales (Mayúsculas y caracteres) 
            int numMayusculas = (int)(LongitudPassword * (PorcentajeMayusculas / 100d));
            int numSimbolos = (int)(LongitudPassword * (PorcentajeSimbolos / 100d));
            int numNumeros = (int)(LongitudPassword * (PorcentajeNumeros / 100d));

            // Se obtienen las posiciones en las que irán los caracteres especiales
            int[] caracteresEspeciales =
                    GetPosicionesCaracteresEspeciales(numMayusculas + numSimbolos + numNumeros);
            int posicionInicial = 0;
            int posicionFinal = 0;

            // Se reemplazan las mayúsculas
            posicionFinal += numMayusculas;
            ReemplazaCaracteresEspeciales(caracteresEspeciales,
                 posicionInicial, posicionFinal, TipoCaracterEnum.Mayuscula);

            // Se reemplazan los símbolos
            posicionInicial = posicionFinal;
            posicionFinal += numSimbolos;
            ReemplazaCaracteresEspeciales(caracteresEspeciales,
                 posicionInicial, posicionFinal, TipoCaracterEnum.Simbolo);

            // Se reemplazan los Números
            posicionInicial = posicionFinal;
            posicionFinal += numNumeros;
            ReemplazaCaracteresEspeciales(caracteresEspeciales,
                 posicionInicial, posicionFinal, TipoCaracterEnum.Numero);
        }



        /// <summary>
        /// Reemplaza un caracter especial en la cadena Password
        /// </summary>
        private void ReemplazaCaracteresEspeciales(
                                        int[] posiciones
                                        , int posicionInicial
                                        , int posicionFinal
                                        , TipoCaracterEnum tipoCaracter)
        {
            for (int i = posicionInicial; i < posicionFinal; i++)
            {
                password[posiciones[i]] = GetCaracterAleatorio(tipoCaracter);
            }
        }



        /// <summary>
        /// Obtiene un array con las posiciones en las que deberán colocarse los caracteres
        /// especiales (Mayúsculas o Símbolos). Es importante que no se repitan los números
        /// de posición para poder mantener el porcentaje de dichos carácteres
        /// </summary>
        /// <param name="numeroPosiciones">Valor que representa el número de posiciones
        /// que deberán crearse sin repetir</param>
        private int[] GetPosicionesCaracteresEspeciales(int numeroPosiciones)
        {
            List<int> lista = new List<int>();
            while (lista.Count < numeroPosiciones)
            {
                int posicion = semilla.Next(0, LongitudPassword);
                if (!lista.Contains(posicion))
                {
                    lista.Add(posicion);
                }
            }
            return lista.ToArray();
        }


        /// <summary>
        /// Obtiene un carácter aleatorio en base a la "matriz" del tipo de caracteres
        /// </summary>
        private char GetCaracterAleatorio(TipoCaracterEnum tipoCaracter)
        {
            string juegoCaracteres;
            switch (tipoCaracter)
            {
                case TipoCaracterEnum.Mayuscula:
                    juegoCaracteres = caracteres.ToUpper();
                    break;
                case TipoCaracterEnum.Minuscula:
                    juegoCaracteres = caracteres.ToLower();
                    break;
                case TipoCaracterEnum.Numero:
                    juegoCaracteres = numeros;
                    break;
                default:
                    juegoCaracteres = simbolos;
                    break;
            }

            // índice máximo de la matriz char de caracteres
            int longitudJuegoCaracteres = juegoCaracteres.Length;

            // Obtención de un número aletorio para obtener la posición del carácter
            int numeroAleatorio = semilla.Next(0, longitudJuegoCaracteres);

            // Se devuelve una posición obtenida aleatoriamente
            return juegoCaracteres[numeroAleatorio];
        }

        #endregion

    }

    public class EntidadEnvio
    {
        public string Empresa { get; set; }
        public string NroExpediente { get; set; }
        public string NroCarta { get; set; }
        public string Fecha { get; set; }
        public string Asunto { get; set; }
        public string Ruc { get; set; }
        public string Correo { get; set; }
        public int Codigo { get; set; }
        public string Estado { get; set; }
        public string MotivoRechazo { get; set; }
    
    }
}
