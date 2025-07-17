using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using fwapp;
using System.Data;
using Netx.Mail;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace WScoes
{
    public class AdminService : IAdminService
    {
        public int ii_Version = 21001;
        public n_fw_data[] iL_data = new n_fw_data[2];
        public n_fw_app n_app = new n_fw_app();

        public AdminService()
        {
            iL_data[0] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SICOES"].ToString());
            iL_data[1] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SCADA"].ToString());
        }

        //public Dictionary<int, string> GetEmpresas()
        //{
        //    Dictionary<int, string> Empresas = new Dictionary<int, string>();
        //    DataSet i_ds = new DataSet("dsregister");
        //    // n_app in_app = (n_app)Session["in_app"];
        //    string ls_comando = @" SELECT * FROM FW_COMPANY WHERE COMPCODE NOT IN (-1, 1, 4) ORDER BY COMPNAME";
        //    iL_data[0].Fill(i_ds, "FW_COMPANY", ls_comando);
        //    //in_app.iL_data[0].Fill(i_ds, "EVE_EVENCLASE", ls_comando);
        //    foreach (DataRow dr in i_ds.Tables["FW_COMPANY"].Rows)
        //    {
        //        Empresas[Convert.ToInt32(dr["COMPCODE"])] = dr["COMPNAME"].ToString();
        //    }

        //    return Empresas;
        //}
        public Dictionary<int, string> GetEmpresas()
        {
            Dictionary<int, string> Empresas = new Dictionary<int, string>();
            DataSet i_ds = new DataSet("dsregister");
            // n_app in_app = (n_app)Session["in_app"];
            string ls_comando = @" SELECT * FROM SI_EMPRESA WHERE TIPOEMPRCODI >= 1 AND TIPOEMPRCODI < 5 UNION ALL SELECT * FROM SI_EMPRESA WHERE EMPRCODI = 0";
            iL_data[0].Fill(i_ds, "SI_EMPRESA", ls_comando);
            //in_app.iL_data[0].Fill(i_ds, "EVE_EVENCLASE", ls_comando);
            foreach (DataRow dr in i_ds.Tables["SI_EMPRESA"].Rows)
            {
                Empresas[Convert.ToInt32(dr["EMPRCODI"])] = dr["EMPRNOMB"].ToString();
            }

            return Empresas;
        }


        public bool IsAdmin(int ai_userCode, out string as_userRoles)
        {
            bool lb_existe = false;
            as_userRoles = String.Empty;
            DataSet i_ds = new DataSet("dsuser");
            string ls_comando = @"SELECT DISTINCT(u.usercode), ur.rolcode, u.areacode, u.username, u.userlogin FROM FW_USER u ";
            ls_comando += "INNER JOIN FW_USERROL ur ON u.usercode = ur.usercode ";
            ls_comando += "WHERE ur.userrolcheck IS NOT NULL ";
            ls_comando += "AND ur.userrolvalidate IS NOT NULL ";
            ls_comando += "AND ur.rolcode IN (43, 44, 47, 53, 59, 64, 78)";
            iL_data[0].Fill(i_ds, "FW_USER", ls_comando);

            foreach (DataRow dr in i_ds.Tables["FW_USER"].Rows)
            {
                if (Int32.Parse(dr["USERCODE"].ToString()).Equals(ai_userCode))
                {
                    if (!String.IsNullOrEmpty(dr["ROLCODE"].ToString()))
	                {
                        as_userRoles += dr["ROLCODE"].ToString() + ", ";
	                }
                    
                    lb_existe = true;
                }
            }

            return lb_existe;
        }

        public bool IsUserModulo(int ai_userCode, int ai_codigoModulo)
        {
            bool lb_existe = false;
            DataSet i_ds = new DataSet("dsuser");
            string ls_comando = @"SELECT DISTINCT(u.usercode), u.areacode, u.username, u.userlogin FROM FW_USER u ";
            ls_comando += "INNER JOIN FW_USERROL ur ON u.usercode = ur.usercode ";
            ls_comando += "AND ur.userrolvalidate IS NOT NULL ";
            ls_comando += "AND ur.userrolcheck IS NOT NULL ";
            ls_comando += "AND ur.rolcode IN (" + ai_codigoModulo + " )";
            iL_data[0].Fill(i_ds, "FW_USER", ls_comando);

            foreach (DataRow dr in i_ds.Tables["FW_USER"].Rows)
            {
                if (Int32.Parse(dr["USERCODE"].ToString()).Equals(ai_userCode))
                {
                    lb_existe = true;
                }
            }

            return lb_existe;
        }



        //public DataTable ListarUsuarios(int ai_estado, int ai_codeEmpr, DateTime adt_fechaInicio, DateTime adt_fechaFin, int ai_usercode)
        public DataTable ListarUsuarios(int ai_estado, int ai_codeEmpr, int ai_codeMod, DateTime adt_fechaInicio, DateTime adt_fechaFin)
        {
            DataSet li_ds = new DataSet("dsusers");

            string ls_comando = " SELECT DISTINCT(u.usercode), u.userlogin, u.username, u.usertlf, ur.lastuser, u.userstate, ";
            ls_comando += "CASE WHEN u.arealaboral IS NULL THEN a.areaname ELSE u.arealaboral END AS areaname, ";
            ls_comando += "u.motivocontacto,u.empresas ";
            ls_comando += " FROM FW_USER u";
            ls_comando += " INNER JOIN FW_AREA a ON u.areacode = a.areacode";
            ls_comando += " INNER JOIN FW_USERROL ur ON u.usercode = ur.usercode";
            ls_comando += " INNER JOIN FW_USER_X_EMPRESA ue ON u.usercode = ue.usercode ";

            if (ai_codeEmpr != 0)
                ls_comando += " WHERE ue.emprcodi = " + ai_codeEmpr.ToString();
            //    ls_comando += " WHERE u.empresas LIKE '%' || :param || '%'";


            //filtrando por el estado del usuario
            switch (ai_estado)
            {
                case 1:
                    ls_comando += " AND u.userstate = 'A'";
                    ls_comando += " AND ur.userrolvalidate IS NOT NULL AND ur.userrolcheck IS NOT NULL  ";
                    break;
                case 2:
                    //ls_comando += " AND u.userstate IS NULL";
                    ls_comando += " AND ur.userrolvalidate IS NULL AND ur.userrolcheck IS NULL AND u.userstate <> 'B' ";
                    break;
                case 3:
                    ls_comando += " AND u.userstate = 'B'";
                    break;
                default:
                    ls_comando += " AND u.userstate = 'A'";
                    ls_comando += " AND ur.userrolvalidate IS NOT NULL AND ur.userrolcheck IS NOT NULL  ";
                    break;
            }


            /*filtrar por tipo de administrador*/
            switch (ai_codeMod)
            {
                case 48:
                    ls_comando += " AND ur.rolcode = 48 ";
                    break;
                case 49:
                    ls_comando += " AND ur.rolcode = 49 ";
                    break;
                case 50:
                    ls_comando += " AND ur.rolcode = 50 ";
                    break;
                case 54:
                    ls_comando += " AND ur.rolcode = 54 ";
                    break;
                case 60:
                    ls_comando += " AND ur.rolcode = 60 ";
                    break;
                case 65:
                    ls_comando += " AND ur.rolcode = 65 ";
                    break;
                case 79:
                    ls_comando += " AND ur.rolcode = 79 ";
                    break;
                default:
                    ls_comando += " AND ur.rolcode IN (48, 49, 50, 54, 60, 65, 79) ";
                    break;
            }

            /*filtrando por fecha*/
            if (adt_fechaFin.CompareTo(adt_fechaInicio) >= 0)
            {
                ls_comando += " AND u.userfcreacion BETWEEN " + EPDate.SQLDateTimeOracleString(adt_fechaInicio) + " AND " + EPDate.SQLDateTimeOracleString(adt_fechaFin);
            }

            ls_comando += " ORDER BY u.userlogin";


            iL_data[0].Fill(li_ds, "FW_USER", ls_comando);

            //retornando los usuarios seleccionados;
            return li_ds.Tables["FW_USER"];
        }

        public int InsertUsuario(string as_nomb_apell, int ai_emprcodi, string as_userlogin, string as_usertlf, string as_motivocontact, string as_userCargo, string as_areaLaboral)
        {
            try
            {
                int li_resultado = 0, li_usercodi = 0, li_areacodi = 0;
                try
                {
                    li_usercodi = iL_data[0].nf_ExecuteScalar_GetInteger("SELECT MAX(USERCODE) FROM FW_USER ") + 1;
                    li_areacodi = iL_data[0].nf_ExecuteScalar_GetInteger("SELECT AREACODE FROM FW_AREA WHERE COMPCODE = " + ai_emprcodi);
                }
                catch { }


                if ((as_nomb_apell.Length > 50) && (li_resultado > 0) && (li_areacodi > 0))
                    as_nomb_apell = as_nomb_apell.Substring(0, 50).TrimStart().TrimEnd();

                if ((as_userCargo.Length > 50) && (as_areaLaboral.Length > 50))
                {
                    as_userCargo = as_userCargo.Substring(0, 50).TrimStart().TrimEnd();
                    as_areaLaboral = as_areaLaboral.Substring(0, 50).TrimStart().TrimEnd();
                }
                string ls_comando = "INSERT INTO FW_USER (usercode, areacode, username, userlogin, usercheck, lastuser, usertlf, motivocontacto, usercargo, arealaboral) " +
                    "VALUES (" + li_usercodi.ToString() + "," + li_areacodi.ToString() + ",'" + as_nomb_apell.TrimStart().TrimEnd() + "','" + as_userlogin.ToString() + "', " + nf_get_random_nbr() + ",'userForRegister','" + as_usertlf.ToString() + "','" + as_motivocontact + "','" + as_userCargo + "','" + as_areaLaboral + "')";
                li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);

                if (li_resultado > 0 && li_usercodi > 0)
                {
                    return li_usercodi;
                }
                else
                {
                    return li_resultado;
                }


            }
            catch
            {
                return -1;
            }
        }


        public int ActualizaEstado(string as_usercode, string as_estado, string as_lastuser)
        {
            int li_resultado = 0;
            string ls_fecha_estado = String.Empty, ls_comando = String.Empty;

            switch (as_estado)
            {
                case "Activo"://Pasar a baja
                    ls_fecha_estado = " 'B', USERFBAJA = SYSDATE, USERFACTIVACION = NULL ";
                    break;
                case "Pendiente": //Pasar a activo
                    ls_fecha_estado = " 'A', USERFACTIVACION = SYSDATE ";
                    break;
                case "Baja"://Pasar a activo
                    ls_fecha_estado = " 'A', USERFACTIVACION = SYSDATE, USERFBAJA = NULL ";
                    break;
                default:
                    break;
            }

            try
            {
                if (!String.IsNullOrEmpty(as_estado))
                {
                    ls_comando = "UPDATE FW_USER SET USERSTATE = " + ls_fecha_estado + ", LASTUSER = '" + as_lastuser.Trim() + "' WHERE USERCODE = " + as_usercode.ToString();
                    li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);
                }
                
                return li_resultado;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int AsignaRol(string as_usercode, string as_lastuser, string as_codigorol)
        {
            int li_resultado = 0;
            string ls_comando = String.Empty;

            try
            {
                if (!String.IsNullOrEmpty(as_usercode))
                {
                    ls_comando = "INSERT INTO FW_USERROL (USERCODE, ROLCODE, LASTUSER, LASTDATE) VALUES ( " + as_usercode.ToString() + " , " + as_codigorol + ", '" + as_lastuser.Trim() + "', SYSDATE)";
                    li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);
                }

                return li_resultado;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        //public int EnviaCorreo(string as_mail_dest, string as_nomb_user_dest, string as_login_dest, string as_user_pwd, string as_mail_copia, string as_nomb_user_creado, string as_nomb_emp, string as_mail_server)
        //{
        //    Mail ln_mail;
        //    string as_msg;
        //    int li_mail_result;

        //    as_msg = "";
        //    ln_mail = new Mail(as_mail_server);
        //    as_msg = "";
        //    if (as_nomb_emp != "")
        //    {
        //        as_msg += "<p>Registro de Integrantes</p>";
        //        as_msg += "<p>Datos de acceso de " + as_nomb_emp + "</p>";
        //    }
        //    as_msg += "<p> Estimado " + as_nomb_user_dest + ".</p>";
        //    as_msg += "<p> Fue agregado como representante y/o persona de contacto por el usuario " + as_nomb_user_creado + ".</p>";
        //    as_msg += "<p> Los datos de acceso asignados a su cuenta son:</p>";
        //    as_msg += "<p> Usuario: " + as_login_dest + "</p>";
        //    as_msg += "<p> Contraseña: " + as_user_pwd + "</p>";
        //    as_msg += "<p> Atte</p>";
        //    as_msg += "<p> Registro de Integrantes</p>";
        //    as_msg += "<p> COES-SINAC</p>";
        //    li_mail_result = ln_mail.nf_envia_correo("webapp@coes.org.pe", as_mail_dest, as_mail_copia, "", "COES-SINAC / Datos de Acceso", as_msg);

        //    return li_mail_result;
        //}

        public int SetPassword(int ai_usercode, int ai_usercheck)
        {
            string ls_comando = String.Empty;
            int li_resultado = 0;
            try
            {
                if (( ai_usercode > 0) && (ai_usercheck > 0))
                {
                    ls_comando = "UPDATE FW_USER SET USERVALIDATE = " + ai_usercheck.ToString() + " WHERE USERCODE = " + ai_usercode.ToString();
                    li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);
                }

                return li_resultado;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public DataRow GetInfoUsuario(string as_usercode)
        {
            DataSet i_ds = new DataSet("dsuser");
            string ls_comando = @"SELECT * FROM FW_USER WHERE USERCODE = " + as_usercode;
            iL_data[0].Fill(i_ds, "FW_USER", ls_comando);

            foreach (DataRow dr in i_ds.Tables["FW_USER"].Rows)
            {
                if (Int32.Parse(dr["USERCODE"].ToString()).Equals(Convert.ToInt32(as_usercode)))
                {
                    return dr;
                }
            }

            return null;
        }

        public static int nf_get_random_nbr()
        {
            char[] chArray = new char[] { 
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
            };

            int li_num = 0;
            //string str = "";
            DateTime today = DateTime.Today;
            int introduced10 = DateTime.DaysInMonth(today.Year, today.Month);
            li_num = introduced10 + (((today.Day + today.Hour) + today.Minute) + today.Second);
            Random random = new Random(DateTime.Now.Millisecond + li_num);
            int length = chArray.Length;
            //for (int i = 0; i < li_tam; i++)
            //{
            //    int index = random.Next(0, length);

            //    str = str + chArray[index].ToString();
            //}

            //if (Int32.TryParse(str, out li_num))
            //{
            //    return li_num;
            //}

            //return 0;
            Random rn = new Random();

            return rn.Next(10000, 100000);
        }

        public int EnviaCorreoLog(string as_to_list, string as_subject, string as_message, bool ab_html)
        {
            string as_host = "correo.coes.org.pe";
            MailMessage msg = new MailMessage("webapp@coes.org.pe", as_to_list);

            msg.Subject = as_subject;

            /*Agregando cuenta CC*/
            MailAddress ma_to_bcc = new MailAddress("webapp@coes.org.pe", "Web app");
            msg.Bcc.Add(ma_to_bcc);

            string as_msg = String.Empty;
            as_msg += as_message;

            msg.Body = as_msg;
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = ab_html;

            SmtpClient cliente = new SmtpClient(as_host, 25);
            cliente.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                cliente.Send(msg);
                return 1;
            }

            catch (Exception)
            {
                return -1;
            }

        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int EnviarNotificacionAdministrador(bool ab_inicial, string as_username, string as_admin_emails, string as_userlogin, string as_empresas, string as_modulos, string as_subject)
        {
            string[] ls_array_mail = as_admin_emails.Split(new char[] { ';', ',' });
            List<string> ld_list_mail = new List<string>();
            foreach (string item in ls_array_mail)
            {
                if (IsValidEmail(item))
                {
                    ld_list_mail.Add(item);
                }
                else
                {
                    if (IsValidEmail(item + "@coes.org.pe"))
                    {
                        ld_list_mail.Add(item + "@coes.org.pe");
                    }
                    else
                    {
                        return -1;
                    }
                }
            }

            if (ld_list_mail.Count == 0)
                return -1;

            string as_host = "zimbra.coes.org.pe";


            //MailMessage msg = new MailMessage("registroExtranet@coes.org.pe", as_userlogin);
            MailMessage msg = new MailMessage("registroExtranet@coes.org.pe", ld_list_mail[0]);

            for (int i = 1; i < ld_list_mail.Count; i++)
            {
                msg.Bcc.Add(ld_list_mail[i]);
            }

            msg.Bcc.Add("webapp@coes.org.pe");
            msg.Subject = as_subject;
            string as_msg = "<p>EXTRANET COES</p>";
            if (ab_inicial)
                as_msg += "<p>Se remitieron las credenciales al usuario y s";
            else
                as_msg += "S";
            as_msg += "e habilitaron los accesos a los módulos de la EXTRANET COES del usuario: " + as_userlogin + "</p>";
            as_msg += "<p> Los datos de la cuenta y los Módulos habilitados son:</p>";
            as_msg += "<p> Empresa(s):<br />" + as_empresas + "</p>";
            as_msg += "<p> Usuario: " + as_userlogin + "</p>";
            as_msg += "<p> Módulos habilitados:<br />" + as_modulos + "</p>";
            as_msg += "<p> Atte</p>";
            as_msg += "<p> Extranet COES</p>";
            as_msg += "<p> COES-SINAC</p>";

            msg.Body = as_msg;
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;
            SmtpClient cliente = new SmtpClient(as_host, 25);
            cliente.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                cliente.Send(msg);
                return 1;
            }

            catch (Exception)
            {

                return -1;

            }
        }

        public int EnviarNotificacionUsuario(bool ab_inicial, string as_username, string as_userlogin, string as_empresas, string as_modulos, string as_subject)
        {
            string as_host = "zimbra.coes.org.pe";


            MailMessage msg = new MailMessage("registroExtranet@coes.org.pe", as_userlogin);

            msg.Bcc.Add("webapp@coes.org.pe");
            msg.Subject = as_subject;
            string as_msg = "<p>EXTRANET COES</p>";
            if (ab_inicial)
                as_msg += "<p>Se remitieron las credenciales al usuario y s";
            else
                as_msg += "S";
            as_msg += "e habilitaron los accesos a los módulos de la EXTRANET COES del usuario</p>";
            as_msg += "<p> Los datos de la cuenta y los Módulos habilitados son:</p>";
            as_msg += "<p> Empresa(s):<br />" + as_empresas + "</p>";

            as_msg += "<p> Módulos habilitados:<br />" + as_modulos + "</p>";
            as_msg += "<p> Atte</p>";
            as_msg += "<p> Extranet COES</p>";
            as_msg += "<p> COES-SINAC</p>";

            msg.Body = as_msg;
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;
            SmtpClient cliente = new SmtpClient(as_host, 25);
            cliente.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                cliente.Send(msg);
                return 1;
            }

            catch (Exception)
            {

                return -1;

            }
        }

        public int EnviaCorreoAlta(string as_username, int ai_areacode, string as_userlogin, string as_user_pwd, string as_msg_mail, bool ab_password_visible)
        {
            string as_dominio_coes = "@coes.org.pe";
            string as_host = "zimbra.coes.org.pe";
            
            MailMessage msg = null;
            if (ai_areacode >= 1 && ai_areacode <= 17)
            {
                msg = new MailMessage("registroExtranet@coes.org.pe", as_userlogin + as_dominio_coes);
            }
            else
            {
                msg = new MailMessage("registroExtranet@coes.org.pe", as_userlogin);
            }
            
            msg.Bcc.Add("webapp@coes.org.pe");
            msg.Subject = as_msg_mail;
            string as_msg = "<p>EXTRANET COES</p>";
            as_msg += "<p>Datos de acceso a la EXTRANET COES</p>";
            as_msg += "<p> Estimado Sr. " + as_username + "</p>";
            //as_msg += "<p> Fue agregado como representante y/o persona de contacto por el usuario " + as_nomb_user_creado + ".</p>";
            as_msg += "<p> Los datos de acceso asignados a su cuenta son:</p>";
            if (as_userlogin.EndsWith("@coes.org.pe"))
            {
                as_msg += "<p> Usuario: " + as_userlogin.Replace("@coes.org.pe", String.Empty) + "</p>";
            }
            else
            {
                as_msg += "<p> Usuario: " + as_userlogin + "</p>";
            }
            if (ab_password_visible)
            {
                as_msg += "<p> Contraseña: " + as_user_pwd + "</p>";
            }
            
            as_msg += "<p> Atte</p>";
            as_msg += "<p> Extranet COES</p>";
            as_msg += "<p> COES-SINAC</p>";

            msg.Body = as_msg;
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;
            SmtpClient cliente = new SmtpClient(as_host, 25);
            cliente.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                cliente.Send(msg);
                return 1;
            }

            catch (Exception)
            {

                return -1;

            }

        }

        public int EnviaCorreoAlta(string as_username, string as_userlogin, string as_user_pwd, string as_msg_mail, string as_html_link_manual)
        {

            string as_host = "zimbra.coes.org.pe";
            MailMessage msg = new MailMessage("registroExtranet@coes.org.pe", as_userlogin);
            msg.Bcc.Add("webapp@coes.org.pe");
            msg.Subject = as_msg_mail;
            string as_msg = "<p>EXTRANET COES</p>";
            as_msg += "<p>Datos de acceso a la EXTRANET COES</p>";
            as_msg += "<p> Estimado Sr. " + as_username + "</p>";
            //as_msg += "<p> Fue agregado como representante y/o persona de contacto por el usuario " + as_nomb_user_creado + ".</p>";
            as_msg += "<p> Los datos de acceso asignados a su cuenta son:</p>";
            as_msg += "<p> Usuario: " + as_userlogin + "</p>";
            as_msg += "<p> Contraseña: " + as_user_pwd + "</p>";
            as_msg += "<p> Para descargar el manual haga clic en el siguiente <a href='" + as_html_link_manual + "' target='_blank'>enlace</a></p>";
            as_msg += "<p> Atte</p>";
            as_msg += "<p> Extranet COES</p>";
            as_msg += "<p> COES-SINAC</p>";

            msg.Body = as_msg;
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;
            SmtpClient cliente = new SmtpClient(as_host, 25);
            cliente.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                cliente.Send(msg);
                return 1;
            }

            catch (Exception)
            {

                return -1;

            }

        }

        public int EnviaCorreoBaja(string as_username, string as_userlogin, string as_msg_mail)
        {

            string as_host = "zimbra.coes.org.pe";
            MailMessage msg = new MailMessage("registroExtranet@coes.org.pe", as_userlogin);
            msg.Bcc.Add("webapp@coes.org.pe");
            msg.Subject = as_msg_mail;
            string as_msg = "<p>EXTRANET COES</p>";
            as_msg += "<p> Estimado Sr. " + as_username + "</p>";
            as_msg += "<p> Su usuario ha sido dado de baja:</p>";
            as_msg += "<p> Usuario: " + as_userlogin + "</p>";
            as_msg += "<p> Atte</p>";
            as_msg += "<p> Extranet COES</p>";
            as_msg += "<p> COES-SINAC</p>";

            msg.Body = as_msg;
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;
            SmtpClient cliente = new SmtpClient(as_host, 25);
            cliente.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                cliente.Send(msg);
                return 1;
            }

            catch (Exception)
            {

                return -1;

            }

        }

        public int EnviaCorreoRegistro(string as_username, string as_userlogin, string as_msg_mail)
        {

            string as_host = "zimbra.coes.org.pe";
            MailMessage msg = new MailMessage("registroExtranet@coes.org.pe", as_userlogin);
            msg.Bcc.Add("webapp@coes.org.pe");
            msg.Subject = as_msg_mail;
            string as_msg = "<p> Estimado " + as_username + ", su solicitud ha sido enviada.</p>";
            //as_msg += "<p> Fue agregado como representante y/o persona de contacto por el usuario " + as_nomb_user_creado + ".</p>";
            as_msg += "<p> " + "El administrador de la extranet revisará su solicitud y responderá lo antes posible." + "</p>";
            as_msg += "<p> Atte</p>";
            as_msg += "<p> Extranet COES</p>";
            as_msg += "<p> COES-SINAC</p>";

            msg.Body = as_msg;
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;
            SmtpClient cliente = new SmtpClient(as_host, 25);
            cliente.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                cliente.Send(msg);
                return 1;
            }

            catch (Exception)
            {

                return -1;

            }

        }

        public int EnviaCorreoToAdministrator(string ps_listaToSend, string ps_nombreRegistrado, string ps_apellidoRegistrado, string ps_emailRegistrado, string ps_empresaRegistrado, string ps_motivoRegistrado, string ps_subject)
        {
            string as_host = "zimbra.coes.org.pe";
            MailMessage msg = new MailMessage("registroExtranet@coes.org.pe", ps_listaToSend);
            msg.Bcc.Add("webapp@coes.org.pe");
            msg.Subject = ps_subject;
            string as_msg = "<p>EXTRANET COES</p>";
            as_msg += "<p>Datos de registro a la EXTRANET COES</p>";
            as_msg += "<p> Se envío la solicitud </p>";
            as_msg += "<p> Los datos del usuario pendiente de registro son:</p>";
            as_msg += "<p> Usuario: " + ps_emailRegistrado + "</p>";
            as_msg += "<p> Nombre y Apellido: " + ps_nombreRegistrado + " " + ps_apellidoRegistrado + "</p>";
            as_msg += "<p> Motivo de Contacto: " + ps_motivoRegistrado + "</p>";
            as_msg += "<p> Para acceder a más detalles y registrar al usuario pendiente puede acceder a: http://www.coes.org.pe/extranet/WebForm/Admin/w_adm_listarUsuarios.aspx </p>";
            as_msg += "<p> Atte</p>";
            as_msg += "<p> Extranet COES</p>";
            as_msg += "<p> COES-SINAC</p>";

            msg.Body = as_msg;

            msg.Body = as_msg;
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;
            SmtpClient cliente = new SmtpClient(as_host, 25);
            cliente.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                cliente.Send(msg);
                return 1;
            }

            catch (Exception)
            {

                return -1;

            }
        }


        public Dictionary<int, string> GetEmpresasSEIN()
        {
            Dictionary<int, string> Empresas = new Dictionary<int, string>();
            DataSet i_ds = new DataSet("dsempresas");
            // n_app in_app = (n_app)Session["in_app"];
            //string ls_comando = @" SELECT * FROM FW_COMPANY WHERE COMPCODE NOT IN (-1, 1, 4) ORDER BY COMPNAME";
            //string ls_comando = @" SELECT EMPRNOMB, EMPRCODI FROM SI_EMPRESA WHERE EMPRSEIN='S' AND EMPRCODI>0 ORDER BY 1";
            string ls_comando = @" select EMPRNOMB, EMPRCODI from SI_EMPRESA e where e.tipoemprcodi in (1, 2, 3, 4) AND EMPRCODI>0 ORDER BY 1";
            iL_data[0].Fill(i_ds, "SI_EMPRESA", ls_comando);
            //in_app.iL_data[0].Fill(i_ds, "EVE_EVENCLASE", ls_comando);
            foreach (DataRow dr in i_ds.Tables["SI_EMPRESA"].Rows)
            {
                Empresas[Convert.ToInt32(dr["EMPRCODI"])] = dr["EMPRNOMB"].ToString();
            }

            return Empresas;
        }

        public string GetEmpresasSEIN(string ls_empresaCodigo)
        {
            Dictionary<int, string> Empresas = new Dictionary<int, string>();
            DataSet i_ds = new DataSet("dsempresas");
            // n_app in_app = (n_app)Session["in_app"];
            //string ls_comando = @" SELECT * FROM FW_COMPANY WHERE COMPCODE NOT IN (-1, 1, 4) ORDER BY COMPNAME";
            string ls_comando = @" select EMPRNOMB, EMPRCODI from SI_EMPRESA e WHERE e.EMPRCODI = " + ls_empresaCodigo + " AND e.tipoemprcodi in (1, 2, 3, 4) AND EMPRCODI>0 ORDER BY 1";
            iL_data[0].Fill(i_ds, "SI_EMPRESA", ls_comando);
            //in_app.iL_data[0].Fill(i_ds, "EVE_EVENCLASE", ls_comando);
            if (i_ds.Tables["SI_EMPRESA"].Rows.Count > 0)
            {
                return i_ds.Tables["SI_EMPRESA"].Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        public string GetEmpresasxUsuario(int ai_usercode)
        {
            Dictionary<int, string> Empresa = new Dictionary<int, string>();
            DataSet i_ds = new DataSet("dsempresas");
            // n_app in_app = (n_app)Session["in_app"];
            //string ls_comando = @" SELECT * FROM FW_COMPANY WHERE COMPCODE NOT IN (-1, 1, 4) ORDER BY COMPNAME";
            string ls_comando = @" SELECT EMPRESAS FROM FW_USER WHERE USERCODE = " + ai_usercode.ToString() + " ORDER BY 1";
            iL_data[0].Fill(i_ds, "FW_USER", ls_comando);
            //in_app.iL_data[0].Fill(i_ds, "EVE_EVENCLASE", ls_comando);
            if (i_ds.Tables["FW_USER"].Rows.Count > 0)
            {
                return i_ds.Tables["FW_USER"].Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        public string GetAreaXUsuario(int ai_areacode)
        {
            Dictionary<int, string> Empresa = new Dictionary<int, string>();
            DataSet i_ds = new DataSet("dsempresas");
            // n_app in_app = (n_app)Session["in_app"];
            //string ls_comando = @" SELECT * FROM FW_COMPANY WHERE COMPCODE NOT IN (-1, 1, 4) ORDER BY COMPNAME";
            string ls_comando = @" SELECT AREANAME FROM FW_AREA WHERE COMPCODE = " + ai_areacode.ToString() + " ORDER BY 1";
            iL_data[0].Fill(i_ds, "FW_AREA", ls_comando);
            //in_app.iL_data[0].Fill(i_ds, "EVE_EVENCLASE", ls_comando);
            if (i_ds.Tables["FW_AREA"].Rows.Count > 0)
            {
                return i_ds.Tables["FW_AREA"].Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        public int SetRoles(int ai_usercode, int ai_rolcode)
        {
            int li_resultado = 0;
            string ls_comando = "INSERT INTO FW_USERROL (usercode, rolcode) " +
                    "VALUES (" + ai_usercode.ToString() + "," + ai_rolcode.ToString() + ")";

            li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);

            return li_resultado;
        }

        public int ActualizaRol(int ai_usercode, int ai_rolcode, string as_lastuser)
        {
            if (as_lastuser.Length > 20)
                as_lastuser = as_lastuser.Substring(0, 20);

            string ls_comando = String.Empty;
            int li_resultado = 0;
            try
            {
                if ((ai_usercode > 0) && (!String.IsNullOrEmpty(as_lastuser)))
                {
                    ls_comando = "UPDATE FW_USERROL SET USERROLVALIDATE = 1, USERROLCHECK = 1, LASTUSER = '" + as_lastuser.ToString() + "', LASTDATE = sysdate ";
                    ls_comando += " WHERE USERCODE = " + ai_usercode.ToString() + " AND ROLCODE = " + ai_rolcode.ToString();
                    li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);
                }

                return li_resultado;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int ResetRol(int ai_usercode, int ai_rolcode, string as_lastuser)
        {
            if (as_lastuser.Length > 20)
                as_lastuser = as_lastuser.Substring(0, 20);

            string ls_comando = String.Empty;
            int li_resultado = 0;
            try
            {
                if ((ai_usercode > 0) && (!String.IsNullOrEmpty(as_lastuser)))
                {
                    ls_comando = "UPDATE FW_USERROL SET USERROLVALIDATE = NULL, USERROLCHECK = NULL, LASTUSER = '" + as_lastuser.ToString() + "', LASTDATE = sysdate ";
                    ls_comando += " WHERE USERCODE = " + ai_usercode.ToString() + " AND ROLCODE = " + ai_rolcode.ToString();
                    li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);
                }

                return li_resultado;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public Dictionary<int, int> GetRolesAsigXUsuario(int ai_usercode)
        {
            Dictionary<int, int> Roles = new Dictionary<int, int>();
            DataSet i_ds = new DataSet("Roles");
            string ls_comando = "SELECT DISTINCT(u.usercode), ur.rolcode, u.areacode, u.username, u.userlogin ";
            ls_comando += "FROM FW_USER u INNER JOIN FW_USERROL ur ON u.usercode = ur.usercode ";
            ls_comando += "WHERE u.usercode = " + ai_usercode;
            ls_comando += "AND ur.userrolcheck IS NOT NULL ";
            ls_comando += "AND ur.userrolvalidate IS NOT NULL ";
            ls_comando += "AND ur.rolcode IN (48, 49, 50, 54, 60, 65, 79) ";

            iL_data[0].Fill(i_ds, "FW_ROL", ls_comando);

            foreach (DataRow dr in i_ds.Tables["FW_ROL"].Rows)
            {
                Roles.Add(Convert.ToInt32(dr["ROLCODE"].ToString()), Convert.ToInt32(dr["USERCODE"].ToString()));
            }

            return Roles;
        }

        public Dictionary<int, string> GetRolesSolicitadosXUsuario(int ai_usercode)
        {
            Dictionary<int, string> Roles = new Dictionary<int, string>();
            DataSet i_ds = new DataSet("Roles");
            string ls_cadena = String.Empty;
            DateTime ldt_fecha = new DateTime(2000, 1, 1);
            string ls_comando = "SELECT DISTINCT(u.usercode), ur.rolcode, r.rolname, ur.userrolvalidate, ur.userrolcheck, ur.lastuser, ur.lastdate, u.areacode, u.username, u.userlogin ";
            ls_comando += " FROM FW_USER u INNER JOIN FW_USERROL ur ON u.usercode = ur.usercode ";
            ls_comando += " INNER JOIN FW_ROL r ON r.rolcode = ur.rolcode ";
            ls_comando += " WHERE u.usercode = " + ai_usercode;
            ls_comando += " AND ur.rolcode IN (48, 49, 50, 54, 60, 65, 79) ";

            iL_data[0].Fill(i_ds, "FW_ROL", ls_comando);

            foreach (DataRow dr in i_ds.Tables["FW_ROL"].Rows)
            {
                ls_cadena = String.Empty;
                if (dr["ROLNAME"] != null)
                    ls_cadena += dr["ROLNAME"].ToString();
                //if (dr["LASTUSER"] != null)
                //    ls_cadena += " - " + dr["LASTUSER"].ToString();
                //if (DateTime.TryParse(dr["LASTDATE"].ToString(), out ldt_fecha))
                //    ls_cadena += " - " + ldt_fecha.ToString("dd/MM/yyyy HH:mm");

                Roles.Add(Convert.ToInt32(dr["ROLCODE"].ToString()), ls_cadena);
            }

            return Roles;
        }

        public Dictionary<int, string> GetRolesAsignadosXUsuario(int ai_usercode)
        {
            Dictionary<int, string> Roles = new Dictionary<int, string>();
            DataSet i_ds = new DataSet("Roles");
            string ls_cadena = String.Empty;
            DateTime ldt_fecha = new DateTime(2000, 1, 1);
            string ls_comando = "SELECT DISTINCT(u.usercode), ur.rolcode, r.rolname, ur.userrolvalidate, ur.userrolcheck, ur.lastuser, ur.lastdate, u.areacode, u.username, u.userlogin ";
            ls_comando += " FROM FW_USER u INNER JOIN FW_USERROL ur ON u.usercode = ur.usercode ";
            ls_comando += " INNER JOIN FW_ROL r ON r.rolcode = ur.rolcode ";
            ls_comando += " WHERE u.usercode = " + ai_usercode;
            ls_comando += " AND ur.userrolvalidate IS NOT NULL ";
            ls_comando += " AND ur.userrolcheck IS NOT NULL ";
            ls_comando += " AND ur.rolcode IN (48, 49, 50, 54, 60, 65, 79) ";

            iL_data[0].Fill(i_ds, "FW_ROL", ls_comando);

            foreach (DataRow dr in i_ds.Tables["FW_ROL"].Rows)
            {
                ls_cadena = String.Empty;
                if (dr["ROLNAME"] != null)
                    ls_cadena += dr["ROLNAME"].ToString();
                if (dr["LASTUSER"] != null)
                    ls_cadena += " - " + dr["LASTUSER"].ToString();
                if (DateTime.TryParse(dr["LASTDATE"].ToString(), out ldt_fecha))
                    ls_cadena += " - " + ldt_fecha.ToString("dd/MM/yyyy HH:mm");

                Roles.Add(Convert.ToInt32(dr["ROLCODE"].ToString()), ls_cadena);
            }

            return Roles;
        }

        public Dictionary<int, int> GetRolesNoAsigXUsuario(int ai_usercode)
        {
            Dictionary<int, int> Roles = new Dictionary<int, int>();
            DataSet i_ds = new DataSet("Roles");
            string ls_comando = "SELECT DISTINCT(u.usercode), ur.rolcode, u.areacode, u.username, u.userlogin ";
            ls_comando += "FROM FW_USER u INNER JOIN FW_USERROL ur ON u.usercode = ur.usercode ";
            ls_comando += "WHERE u.usercode = " + ai_usercode;
            ls_comando += "AND ur.userrolcheck IS NULL ";
            ls_comando += "AND ur.userrolvalidate IS NULL ";
            ls_comando += "AND ur.rolcode IN (48, 49, 50, 54, 60, 65, 79) ";

            iL_data[0].Fill(i_ds, "FW_ROL", ls_comando);

            foreach (DataRow dr in i_ds.Tables["FW_ROL"].Rows)
            {
                Roles.Add(Convert.ToInt32(dr["ROLCODE"].ToString()), Convert.ToInt32(dr["USERCODE"].ToString()));
            }

            return Roles;
        }

        public Dictionary<int, int> GetRolesAsigXAdmin(int ai_admincode)
        {
            int li_rolcode = 0;
            Dictionary<int, int> Roles = new Dictionary<int, int>();
            DataSet i_ds = new DataSet("Roles");
            string ls_comando = "SELECT DISTINCT(u.usercode), ur.rolcode ";
            ls_comando += "FROM FW_USER u INNER JOIN FW_USERROL ur ON u.usercode = ur.usercode ";
            ls_comando += "WHERE u.usercode = " + ai_admincode;
            ls_comando += "AND ur.userrolcheck IS NOT NULL ";
            ls_comando += "AND ur.userrolvalidate IS NOT NULL ";
            ls_comando += "AND ur.rolcode IN (43, 44, 47, 53, 59, 64, 78) ";

            iL_data[0].Fill(i_ds, "FW_ROL", ls_comando);

            foreach (DataRow dr in i_ds.Tables["FW_ROL"].Rows)
            {
                if (Int32.TryParse(dr["ROLCODE"].ToString(), out li_rolcode))
                    if (li_rolcode == 43) //Adm. Demanda
                        Roles.Add(49, Convert.ToInt32(dr["USERCODE"].ToString()));//Usuario Demanda
                    else if (li_rolcode == 44)//Adm. Mantenimientos
                        Roles.Add(48, Convert.ToInt32(dr["USERCODE"].ToString()));//Usuario Mantenimientos
                    else if (li_rolcode == 47)//Adm. Hidrologia
                        Roles.Add(50, Convert.ToInt32(dr["USERCODE"].ToString()));//Usuario Hidrologia
                    else if (li_rolcode == 53)//Adm. Reclamos
                        Roles.Add(54, Convert.ToInt32(dr["USERCODE"].ToString()));//Usuario Reclamos
                    else if (li_rolcode == 59)//Adm. Cumplimiento RPF
                        Roles.Add(60, Convert.ToInt32(dr["USERCODE"].ToString()));//Usuario Medidores
                    else if (li_rolcode == 64)//Adm. Cumplimiento RPF
                        Roles.Add(65, Convert.ToInt32(dr["USERCODE"].ToString()));//Usuario Cumplimiento RPF
                    else if (li_rolcode == 78)//Adm. Transferencias
                        Roles.Add(79, Convert.ToInt32(dr["USERCODE"].ToString()));//Usuario Transferencias
                    else//Adm. Medidores
                        Roles.Add(60, Convert.ToInt32(dr["USERCODE"].ToString()));//Usuario Reclamos
            }

            return Roles;
        }



        public int SetEmpresas(int ai_usercode, string as_empresasSEIN, string as_userLogin)
        {
            string ls_comando = String.Empty;
            int li_output, li_resultado;
            li_output = li_resultado = 0;
            List<int> ls_empresas = new List<int>();

            if (as_userLogin != null && as_userLogin.Length > 20)
                as_userLogin = as_userLogin.Substring(0, 20);

            try
            {
                if ((ai_usercode > 0) && (!String.IsNullOrEmpty(as_empresasSEIN)))
                {
                    ls_comando = "UPDATE FW_USER SET EMPRESAS = '" + as_empresasSEIN.ToString() + "' WHERE USERCODE = " + ai_usercode.ToString();
                    li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);

                    if (li_resultado > 0)
                    {
                        if (Int32.TryParse(as_empresasSEIN, out li_output))
                            ls_empresas.Add(li_output);
                        else
                            ls_empresas = as_empresasSEIN.Split(',').Select(int.Parse).ToList();

                        foreach (int item in ls_empresas)
                        {
                            ls_comando = "SELECT ue.usercode from fw_user_x_empresa ue where ue.emprcodi = " + item + " AND ue.usercode = " + ai_usercode;
                            li_resultado = iL_data[0].nf_ExecuteScalar_GetInteger(ls_comando);

                            if (li_resultado == 0)
                            {
                                ls_comando = "INSERT INTO FW_USER_X_EMPRESA VALUES (" + ai_usercode + "," + item + ",'" + as_userLogin + "',sysdate)";
                                li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);
                            }
                        }

                        if (li_resultado > 0)
                            return li_resultado;
                        else
                            return -1;
                    }
                    else
                    {
                        return -1;
                    }
                }

                return li_resultado;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int SetEmpresasByCompcode(int ai_usercode, int ai_empresaCompcode, string as_userlogin)
        {
            string ls_comando = String.Empty;
            int li_output, li_resultado;
            li_output = li_resultado = 0;
            List<int> ls_empresas = new List<int>();

            try
            {
                if ((ai_usercode > 0) && (ai_empresaCompcode > 0))
                {
                    ls_comando = "SELECT e.emprcodi from si_empresa e where e.compcode = " + ai_empresaCompcode.ToString();
                    li_resultado = iL_data[0].nf_ExecuteScalar_GetInteger(ls_comando);

                    ls_comando = "SELECT ue.usercode from fw_user_x_empresa ue where ue.emprcodi = " + li_resultado + " AND ue.usercode = " + ai_usercode;
                    li_output = iL_data[0].nf_ExecuteScalar_GetInteger(ls_comando);

                    if (li_output == 0)
                    {
                        ls_comando = "INSERT INTO FW_USER_X_EMPRESA VALUES (" + ai_usercode + "," + li_resultado + ",'" + as_userlogin + "',sysdate)";
                        li_resultado = iL_data[0].nf_ExecuteNonQuery(ls_comando);
                    }
                }

                return li_resultado;
            }
            catch (Exception)
            {
                return -1;
            }
        }

    }
}
