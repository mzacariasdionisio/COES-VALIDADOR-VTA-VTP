using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Net;

namespace WSIC2010.Util
{
    public class CCorreo
    {
        /*public string stmsgerror;
        public string stmsglog;

        public CCorreo();
        public CCorreo(string stHostCorreo);
        public CCorreo(XmlDocument xmlDoc);
        public CCorreo(string stHostCorreo, int inPuerto);
        public CCorreo(string stHostCorreo, int inPuerto, NetworkCredential ncCredenciales);
        public CCorreo(string stHostCorreo, int inPuerto, string stUser, string stPassw);

        public bool sendCorreo(string stFrom, string stTo, string stSubject, string stMessage, string[] stAtachado);
        public bool sendCorreo(string stFrom, string stTo, string stCC, string stSubject, string stMessage, string[] stAtachado);
        public bool sendCorreo(string stTo, string stCC, string stSubject, string stMessage, string[] stAtachado, bool boHTML);
        public bool sendCorreoHTML(string stTitulo, string stFrom, string stTo, string stSubject, string stMessage, string[] stAtachado, string stRutaCSS, string stRutaLink);
        public bool sendCorreoHTML(string stTitulo, string stFrom, string stTo, string stCC, string stSubject, string stMessage, string[] stAtachado, string stRutaCSS, string stRutaLink);*/
    }

    public static class Correo
    {
        public static bool IsValidEmail(string email)
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
    }
}