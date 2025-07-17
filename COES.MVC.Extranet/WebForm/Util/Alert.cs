using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSIC2010.Util
{
    public static class Alert
    {
        /// <summary> 
        /// Shows a client-side JavaScript alert in the browser. 
        /// </summary> 
        /// <param name="message">The message to appear in the alert.</param> 
        public static void Show(string message)
        {
            // Cleans the message to allow single quotation marks 
            string cleanMessage = message.Replace("'", "\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page 
            System.Web.UI.Page page = HttpContext.Current.CurrentHandler as System.Web.UI.Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page 
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
            }
        }
    }
}