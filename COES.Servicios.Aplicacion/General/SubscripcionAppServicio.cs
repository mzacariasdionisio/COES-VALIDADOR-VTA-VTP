using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General
{
    /// <summary>
    /// Clase para el manejo de subscripciones del portal web
    /// </summary>
    public class SubscripcionAppServicio
    {
        #region Metodos para la tabla de subscripciones

        /// <summary>
        /// Permite listar las subscripciones por rango de fechas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<WbSubscripcionDTO> ListarSubscripciones(DateTime fechaInicio, DateTime fechaFin, int? idPublicacion)
        {
            return FactorySic.GetWbSubscripcionRepository().GetByCriteria(fechaInicio, fechaFin, idPublicacion);
        }

        /// <summary>
        /// Permite listar las publicaciones por rango de fechas de exportación
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idPublicacion"></param>
        /// <returns></returns>
        public List<WbSubscripcionDTO> ListarSubscripcionesExportar(DateTime fechaInicio, DateTime fechaFin, int? idPublicacion)
        { 
            return FactorySic.GetWbSubscripcionRepository().ObtenerExportacion(fechaInicio, fechaFin, idPublicacion);
        }

        /// <summary>
        /// Permite obtener los datos de la subscripcion
        /// </summary>
        /// <param name="idSubscripcion"></param>
        /// <returns></returns>
        public WbSubscripcionDTO ObtenerSubscripcion(int idSubscripcion)
        {
            WbSubscripcionDTO entity = FactorySic.GetWbSubscripcionRepository().GetById(idSubscripcion);            
            return entity;
        }

        /// <summary>
        /// Permite eliminar una subscripcion
        /// </summary>
        /// <param name="idSubscripcion"></param>
        public void EliminarSubscripcion(int idSubscripcion)
        {
            try
            {
                FactorySic.GetWbSubscripcionitemRepository().Delete(idSubscripcion);
                FactorySic.GetWbSubscripcionRepository().Delete(idSubscripcion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos de las subscripcion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarSubscripcion(WbSubscripcionDTO entity, string detalle)
        {
            try
            {
                bool valido = this.ValidarSubscripcion(entity, detalle);

                if (valido)
                {
                    int id = 0;

                    if (entity.Subscripcodi == 0)
                    {
                        id = FactorySic.GetWbSubscripcionRepository().Save(entity);
                    }
                    else
                    {
                        FactorySic.GetWbSubscripcionRepository().Update(entity);
                        id = entity.Subscripcodi;
                        FactorySic.GetWbSubscripcionitemRepository().Delete(id);
                    }

                    if (!string.IsNullOrEmpty(detalle))
                    {
                        List<int> idsDetalle = detalle.Split(ConstantesAppServicio.CaracterComa).Select(i => int.Parse(i)).ToList();

                        foreach (int idDetalle in idsDetalle)
                        {
                            WbSubscripcionitemDTO item = new WbSubscripcionitemDTO
                            {
                                Publiccodi = idDetalle,
                                Subscripcodi = id
                            };

                            FactorySic.GetWbSubscripcionitemRepository().Save(item);
                        }
                    }

                    if (entity.Subscripcodi == 0)
                    {
                        this.EnviarNotificaciones(id);
                    }

                    return id;
                }
                else 
                {
                    return -1;
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite validar los datos del formulario
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ValidarSubscripcion(WbSubscripcionDTO entity, string detalle)
        {
            if (string.IsNullOrEmpty(entity.Subscripnombres) ||
                string.IsNullOrEmpty(entity.Subscripapellidos) ||
                string.IsNullOrEmpty(entity.Subscripemail) ||
                string.IsNullOrEmpty(detalle)) {

                    return false;
            }
            else if (!string.IsNullOrEmpty(entity.Subscripemail))
            {
                if (!COES.Base.Tools.Util.ValidarEmail(entity.Subscripemail)) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Permite obtener los items de la subscripcion
        /// </summary>
        /// <param name="idSubscripcion"></param>
        /// <returns></returns>
        public List<WbSubscripcionitemDTO> ObtenerItemsSubscripcion(int idSubscripcion)
        {
            return FactorySic.GetWbSubscripcionitemRepository().GetByCriteria(idSubscripcion);
        }
        

        #endregion

        #region Métodos para las tabla de publicaciones

        /// <summary>
        /// Permite realizar el listado de publicaciones
        /// </summary>
        /// <returns></returns>
        public List<WbPublicacionDTO> ListarPublicaciones()
        {
            return FactorySic.GetWbPublicacionRepository().List();
        }

        /// <summary>
        /// Permite obtener 
        /// </summary>
        /// <param name="idPublicacion"></param>
        /// <returns></returns>
        public WbPublicacionDTO ObtenerPublicacion(int idPublicacion)
        {
            return FactorySic.GetWbPublicacionRepository().GetById(idPublicacion);
        }

        /// <summary>
        /// Permite eliminar una publicacion
        /// </summary>
        /// <param name="idPublicacion"></param>
        public void EliminarPublicacion(int idPublicacion)
        {
            try
            {
                FactorySic.GetWbPublicacionRepository().Delete(idPublicacion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar una publicación
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarPublicacion(WbPublicacionDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Publiccodi == 0)
                {
                    id = FactorySic.GetWbPublicacionRepository().Save(entity);
                }
                else 
                {
                    FactorySic.GetWbPublicacionRepository().Update(entity);
                    id = entity.Publiccodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite enviar los correos electrónicos
        /// </summary>
        /// <param name="idSubscripcion"></param>
        private void EnviarNotificaciones(int idSubscripcion)
        {
            WbSubscripcionDTO entity = FactorySic.GetWbSubscripcionRepository().GetById(idSubscripcion);
            List<WbSubscripcionitemDTO> items = FactorySic.GetWbSubscripcionitemRepository().GetByCriteria(idSubscripcion);
            string publicaciones = this.ObtenerPublicacionesEmail(items);
            entity.Detalle = publicaciones;
            string mensajeUsuario = this.ObtenerHtmlUsuario(entity);
            string mensajeAdministrador = this.ObtenerHtmlAdministrador(entity);
            mensajeAdministrador = mensajeAdministrador.Replace("[", "{");
            mensajeAdministrador = mensajeAdministrador.Replace("]", "}");
            mensajeUsuario = mensajeUsuario.Replace("[", "{");
            mensajeUsuario = mensajeUsuario.Replace("]", "}");
            string asuntoUsuario = MensajeSuscripcion.AsuntoUsuario;
            string asuntoAdministrador = MensajeSuscripcion.AsuntoAdministrador;
            string emailUsuario = entity.Subscripemail;
            string emailAdministrador = ConfigurationManager.AppSettings[MensajeSuscripcion.EmailSubscripciones];

            COES.Base.Tools.Util.SendEmail(emailUsuario, asuntoUsuario, mensajeUsuario);
            COES.Base.Tools.Util.SendEmail(emailAdministrador, asuntoAdministrador, mensajeAdministrador);

            foreach (WbSubscripcionitemDTO item in items)
            {
                if (item.Indicador > 0)
                {
                    List<string> mails = new List<string>();

                    WbPublicacionDTO publicacion = FactorySic.GetWbPublicacionRepository().GetById(item.Publiccodi);

                    if (!string.IsNullOrEmpty(publicacion.Publicemail))
                    {
                        if (COES.Base.Tools.Util.ValidarEmail(publicacion.Publicemail))
                        {
                            mails.Add(publicacion.Publicemail);
                        }
                    }

                    if (!string.IsNullOrEmpty(publicacion.Publicemail1))
                    {
                        if (COES.Base.Tools.Util.ValidarEmail(publicacion.Publicemail1))
                        {
                            mails.Add(publicacion.Publicemail1);
                        }
                    }

                    if (!string.IsNullOrEmpty(publicacion.Publicemail2))
                    {
                        if (COES.Base.Tools.Util.ValidarEmail(publicacion.Publicemail2))
                        {
                            mails.Add(publicacion.Publicemail2);
                        }
                    }

                    if (mails.Count > 0)
                    {
                        List<string> mailsCc = new List<string>();

                        string asuntoResponsable = string.Format(MensajeSuscripcion.AsuntoResponsable, publicacion.Publicnombre);
                        string mensajeResponsable = this.ObtenerHtmlResponsable(publicacion, entity);
                        mensajeResponsable = mensajeResponsable.Replace("[", "{");
                        mensajeResponsable = mensajeResponsable.Replace("]", "}");
                        COES.Base.Tools.Util.SendEmail(mails, mailsCc, asuntoResponsable, mensajeResponsable);
                    }
                }
            }
        }

        /// <summary>
        /// Permite obtener el mensaje para enviar al responsable de la publicacion
        /// </summary>
        /// <param name="publicacion"></param>
        /// <param name="subscripcion"></param>
        /// <returns></returns>
        public string ObtenerHtmlResponsable(WbPublicacionDTO publicacion, WbSubscripcionDTO subscripcion)
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
                        -->
                        </style>
                    </head>
                    <body>
                        <table width='605'>
                            <tr>
                                <td><img src='http://www.coes.org.pe/wcoes/fondo_emailapp.png'></td>
                            </tr>
                            <tr>
                                <td>
	                                <table cellspacing='0' border='0' width='100%'>		
		                                <tr>
			                                <td><strong>Estimado Usuario:</string><br /><br /></td>			       
		                                </tr>
		                                <tr>			        
			                                <td>
                                                Se ha registrado una suscripción en el Portal Web a la publicación <strong>{0}</strong> con los siguientes datos:
                                                <table>
                                                    <tr>
                                                        <td>Nombre:</td>
                                                        <td>{1}</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Apellido:</td>
                                                        <td>{2}</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Email:</td>
                                                        <td>{3}</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Teléfono:</td>
                                                        <td>{4}</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Empresa:</td>
                                                        <td>{5}</td>
                                                    </tr>                                                   
                                                </table>       
                                                Por favor actualice la lista de correo correspondiente.                                        
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

            return String.Format(mensaje, publicacion.Publicnombre, subscripcion.Subscripnombres, subscripcion.Subscripapellidos, subscripcion.Subscripemail,
                subscripcion.Subscriptelefono, subscripcion.Subscripempresa);
        }

        /// <summary>
        /// Permite obtener el listado de subscripciones
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string ObtenerPublicacionesEmail(List<WbSubscripcionitemDTO> items)
        {
            StringBuilder str = new StringBuilder();
            List<WbSubscripcionitemDTO> list = items.Where(x => x.Indicador > 0).ToList();

            str.Append("<ul>");
            foreach (WbSubscripcionitemDTO item in list)
            {
                str.AppendFormat("<li>{0}</li>", item.DesPublicacion);
            }
            str.Append("</ul>");

            return str.ToString();
        }

        /// <summary>
        /// Permite obtener la plantilla para el administrador
        /// </summary>
        /// <returns></returns>
        private string ObtenerHtmlAdministrador(WbSubscripcionDTO subscripcion)
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
                        -->
                        </style>
                    </head>
                    <body>
                        <table width='605'>
                            <tr>
                                <td><img src='http://www.coes.org.pe/wcoes/fondo_emailapp.png'></td>
                            </tr>
                            <tr>
                                <td>
	                                <table cellspacing='0' border='0' width='100%'>		
		                                <tr>
			                                <td><strong>Estimado Usuario:</string><br /><br /></td>			       
		                                </tr>
		                                <tr>			        
			                                <td>
                                                Se ha registrado una suscripción en el Portal Web con los siguientes datos:
                                                <table>
                                                    <tr>
                                                        <td>Nombre:</td>
                                                        <td>{0}</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Apellido:</td>
                                                        <td>{1}</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Email:</td>
                                                        <td>{2}</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Teléfono:</td>
                                                        <td>{3}</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Empresa:</td>
                                                        <td>{4}</td>
                                                    </tr>
                                                    <tr>
                                                        <td valign='top'>Publicaciones:</td>
                                                        <td valign='top'>{5}</td>
                                                    </tr>
                                                </table>                                               
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

            return String.Format(mensaje, subscripcion.Subscripnombres, subscripcion.Subscripapellidos, subscripcion.Subscripemail,
                subscripcion.Subscriptelefono, subscripcion.Subscripempresa, subscripcion.Detalle);
        }

        /// <summary>
        /// Permite obtener la plantilla para el usuario
        /// </summary>
        /// <returns></returns>
        private string ObtenerHtmlUsuario(WbSubscripcionDTO subscripcion)
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
                        -->
                        </style>
                    </head>
                    <body>
                        <table width='605'>
                            <tr>
                                <td><img src='http://www.coes.org.pe/wcoes/fondo_emailapp.png'></td>
                            </tr>
                            <tr>
                                <td>
	                                <table cellspacing='0' border='0' width='100%'>		
		                                <tr>
			                                <td><strong>Estimado:</strong> {0}<br /><br /></td>			       
		                                </tr>
		                                <tr>			        
			                                <td>
                                                Su solicitud de suscripción se registró exitósamente. <br />
                                                En adelante usted recibirá por correo electrónico las publicaciones a las cuales se ha suscrito.  
                                            </td>
		                                </tr>      
	                                </table>	        
	                                <br/>	
	                            </td>
	                        </tr>
                        </table>
                    </body>
                </html>";

            return String.Format(mensaje, subscripcion.Subscripnombres + " " + subscripcion.Subscripapellidos);
        }
        
        /// <summary>
        /// Permite generar el archivo
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void GenerarReporteExcel(List<WbSubscripcionDTO> list, string path, string filename)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("SUSCRIPCIONES");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE SUSCRIPCIONES";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "FECHA REGISTRO";
                        ws.Cells[index, 3].Value = "NOMBRES";
                        ws.Cells[index, 4].Value = "APELLIDOS";
                        ws.Cells[index, 5].Value = "CORREO ELECTRÓNICO";
                        ws.Cells[index, 6].Value = "TELÉFONO";
                        ws.Cells[index, 7].Value = "EMPRESA";
                        ws.Cells[index, 8].Value = "PUBLICACIÓN";

                        rg = ws.Cells[index, 2, index, 8];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (WbSubscripcionDTO item in list)
                        {
                            ws.Cells[index, 2].Value = ((DateTime)item.Subscripfecha).ToString("dd/MM/yyyy");
                            ws.Cells[index, 3].Value = item.Subscripnombres;
                            ws.Cells[index, 4].Value = item.Subscripapellidos;
                            ws.Cells[index, 5].Value = item.Subscripemail;
                            ws.Cells[index, 6].Value = item.Subscriptelefono;
                            ws.Cells[index, 7].Value = item.Subscripempresa;
                            ws.Cells[index, 8].Value = item.Publicname;

                            rg = ws.Cells[index, 2, index, 8];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 8];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;
                        ws.Column(3).Width = 30;
                        ws.Column(4).Width = 30;
                        ws.Column(5).Width = 30;
                        ws.Column(6).Width = 30;
                        ws.Column(7).Width = 30;
                        ws.Column(8).Width = 30;

                        rg = ws.Cells[5, 3, index, 8];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        
        #endregion

        /// <summary>
        /// Permite listar las areas del COES
        /// </summary>
        /// <returns></returns>
        public List<FwAreaDTO> ListarAreas()
        {
            return FactorySic.GetFwAreaRepository().GetByCriteria(1);
        }
    }
}
