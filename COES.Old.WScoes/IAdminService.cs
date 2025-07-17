using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;

namespace WScoes
{
    [ServiceContract]
    public interface IAdminService
    {
        [OperationContract]
        Dictionary<int, string> GetEmpresas();

        [OperationContract]
        bool IsAdmin(int ai_userCode, out string as_userRoles);

        [OperationContract]
        bool IsUserModulo(int ai_userCode, int ai_codigoModulo);

        [OperationContract]
        DataTable ListarUsuarios(int ai_estado, int ai_codeEmpr, int ai_codeMod, DateTime adt_fechaInicio, DateTime adt_fechaFin);

        [OperationContract]
        int InsertUsuario(string as_username, int ai_code_empresa, string as_userlogin, string as_usertlf, string as_motivocontact, string as_userCargo, string as_areaLaboral);

        [OperationContract]
        int ActualizaEstado(string as_usercode, string ls_estado, string as_lastuser);

        [OperationContract]
        int AsignaRol(string as_usercode, string as_lastuser, string as_codigorol);

        [OperationContract]
        int ActualizaRol(int ai_usercode, int ai_rolcode, string as_lastuser);

        [OperationContract]
        int ResetRol(int ai_usercode, int ai_rolcode, string as_lastuser);

        [OperationContract]
        int SetPassword(int ai_usercode, int ai_usercheck);

        [OperationContract]
        int EnviaCorreoLog(string as_to_list, string as_subject, string as_message, bool ab_html);

        [OperationContract]
        int EnviaCorreoAlta(string as_username, int ai_areacode, string as_userlogin, string as_user_pwd, string as_msg_mail, bool ab_password_visible);

        [OperationContract]
        int EnviaCorreoBaja(string as_username, string as_userlogin, string as_msg_mail);

        [OperationContract]
        int EnviaCorreoRegistro(string as_username, string as_userlogin, string as_msg_mail);

        [OperationContract]
        int EnviaCorreoToAdministrator(string ps_listaToSend, string ps_nombreRegistrado, string ps_apellidoRegistrado, string ps_emailRegistrado, string ps_empresaRegistrado, string ps_motivoRegistrado, string ps_subject);

        [OperationContract]
        int EnviarNotificacionUsuario(bool ab_inicial, string as_username, string as_userlogin, string as_empresas, string as_modulos, string as_subject);

        [OperationContract]
        int EnviarNotificacionAdministrador(bool ab_isInicial, string as_username, string as_admin_emails, string as_userlogin, string as_empresas, string as_modulos, string as_subject);

        [OperationContract]
        Dictionary<int, string> GetEmpresasSEIN();

        [OperationContract]
        string GetEmpresasSEIN(string as_empresaCodigo);

        [OperationContract]
        int SetEmpresas(int ai_usercode, string as_empresaSEIN, string as_userlogin);

        [OperationContract]
        int SetEmpresasByCompcode(int ai_usercode, int ai_empresaCompcode, string as_userlogin);

        [OperationContract]
        string GetEmpresasxUsuario(int ai_usercode);

        [OperationContract]
        int SetRoles(int ai_usercode, int ai_rol);

        [OperationContract]
        Dictionary<int, int> GetRolesAsigXUsuario(int ai_usercode);

        [OperationContract]
        Dictionary<int, string> GetRolesAsignadosXUsuario(int ai_usercode);

        [OperationContract]
        Dictionary<int, int> GetRolesAsigXAdmin(int ai_admincode);

        [OperationContract]
        Dictionary<int, int> GetRolesNoAsigXUsuario(int ai_usercode);

        [OperationContract]
        string GetAreaXUsuario(int ai_areacode);


    }
}
