using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace fwapp
{
   /// <summary>
   /// Summary description for n_fw_global.
   /// </summary>
   public class n_fw_app
   {
      //Manejo de ADO.Net
      public n_fw_data[] iL_data = new n_fw_data[3]; // el item 0 es para la coneccion a las tablas del framework      
      //public List<n_fw_data> L_data = new List<n_fw_data>(); 

      //CONSTANTES SISTEMA

      public string is_SystemName = "System.Net";
      public string is_SystemDescription = "System Name Description";
      public string is_SystemVersion = "00.00.00";
      public int ii_Version = 0;

      //CONSTANTES FRAMEWORK
      public int _ALL = 0;
      public int _NODEF = -1;

      //variables local user
      public string is_UserLogin = "";
      public string is_UserName = "";
      public string is_UserEmail = "";
      public string is_AreaName = "";
      public int ii_UserCode = -1;
      public int ii_AreaCode = -1;
      public bool ib_access = false;
      public bool ib_IsIntranet = false;

      //codigo interno de la aplicacion framework
      public int ii_root = 0;
      public int ii_applcode = 0;
      public long il_maxcode = 10000000;
      public long ii_node = 1;

      //session
      public long il_event = 0;

      //PC interno
      public string is_PCUserName = "";
      public string is_PCComputerName = "";
      public string is_PCHostName = "";
      public string is_PC_IPs = ""; //los IP´s asignados a la maquina
      public string is_Empresas = "";
      public List<string> Ls_emprcodi = new List<string>();
      public string is_PCCurrentDir = ""; //directorio donde esta instalado WINDOWS
      public string is_PCOSVersion = "";

      //TextWriter in_twlog;

      public n_fw_app()
      {        
      }

      public int Version()
      {
         return ii_Version;
      }
            
      public void nf_GetPCProperties()
      {
         this.is_PCUserName = Environment.UserName; //cadena.Substring(cadena.LastIndexOf('\\')+1);
         this.is_PCComputerName = Environment.MachineName;
         this.is_PCHostName = Dns.GetHostName();//cadena.Substring(0,cadena.IndexOf('\\'));			
         this.is_PCOSVersion = Environment.OSVersion.ToString();
         this.is_PCCurrentDir = Environment.CurrentDirectory;
      }

      //public void nf_writelog(string as_mensaje)
      //{
      //   if (in_twlog == null) in_twlog = new StreamWriter(@"c:\online.log", true);
      //   //MessageBox.Show("antes de escribir al log;");
      //   in_twlog.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-" + as_mensaje);
      //   in_twlog.Flush();

      //}

   }


}
