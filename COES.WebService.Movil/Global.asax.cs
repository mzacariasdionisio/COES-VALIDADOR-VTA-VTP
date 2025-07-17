using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace COES.WebService.Movil
{
    public class Global : System.Web.HttpApplication
    {

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            log.Info("Request.Url.AbsolutePath - " + Request.Url.AbsolutePath);
            if (!Request.IsSecureConnection && Request.Url.AbsolutePath.Contains("/COES.WebService.Movil/"))
            {
                log.Info("Request.Url.Host - " + Request.Url.Host);
                log.Info("Request.Url.PathAndQuery - " + Request.Url.PathAndQuery);

                string redirectUrl = "https://10.100.210.39:443" + Request.Url.PathAndQuery;

                log.Info("redirectUrl - " + redirectUrl);
                Response.Redirect(redirectUrl, true);
            }

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}