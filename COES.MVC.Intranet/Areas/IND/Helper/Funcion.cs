using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IND.Helper
{
    public class Funcion
    {
        //CONSTANTES
        public const int iGrabar = 1;
        public const int iEditar = 2;
        public const int iNuevo = 3;
        public const int iEliminar = 5;

        //Paginación
        public const int PageSizeMercadoLibre = 10;
        public const int PageSizePeriodoRentaCongestion = 30;
        public const int PageSize = 20;
        public const int NroPageShow = 10;
        public const int PageSizeCodigoEntrega = 20;
        public const int PageSizeCodigoRetiro = 20;
        public const int PageSizeCodigoRetiroSC = 20;
        public const int PageSizeCodigoInfoBase = 20;
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        
        public bool ValidarPermisoGrabar(object IdOpcion, string sUsuario)
        {
            int id = (IdOpcion != null) ? Convert.ToInt32(IdOpcion) : 0;
            return this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, iGrabar, sUsuario);
            //return true;
        }

        public bool ValidarPermisoEditar(object IdOpcion, string sUsuario)
        {
            int id = (IdOpcion != null) ? Convert.ToInt32(IdOpcion) : 0;
            return this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, iEditar, sUsuario);
            //return true;
        }

        public bool ValidarPermisoNuevo(object IdOpcion, string sUsuario)
        {
            int id = (IdOpcion != null) ? Convert.ToInt32(IdOpcion) : 0;
            return this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, iNuevo, sUsuario);
            //return true;
        }

        public bool ValidarPermisoEliminar(object IdOpcion, string sUsuario)
        {
            int id = (IdOpcion != null) ? Convert.ToInt32(IdOpcion) : 0;
            return this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, iEliminar, sUsuario);
            //return true;
        }

        //Retorna una lista de años tomando como inicio 2014 y finalizando 6 años mas al año actual
        public IEnumerable<SelectListItem> ObtenerAnio()
        {
            var ListaAnio = new List<SelectListItem>();
            int iAnioFinal = DateTime.Today.Year + 6;
            for (int i = 2014; i <= iAnioFinal; i++)
            {
                var list = new SelectListItem();
                list.Text = list.Value = i.ToString();
                ListaAnio.Add(list);
            }
            return ListaAnio;
        }

        //Retorna una lista de meses del año
        public IEnumerable<SelectListItem> ObtenerMes()
        {
            var ListaMes = new List<SelectListItem>();
            var list1 = new SelectListItem();
            list1.Text = "Enero"; list1.Value = "1"; ListaMes.Add(list1);
            var list2 = new SelectListItem();
            list2.Text = "Febrero"; list2.Value = "2"; ListaMes.Add(list2);
            var list3 = new SelectListItem();
            list3.Text = "Marzo"; list3.Value = "3"; ListaMes.Add(list3);
            var list4 = new SelectListItem();
            list4.Text = "Abril"; list4.Value = "4"; ListaMes.Add(list4);
            var list5 = new SelectListItem();
            list5.Text = "Mayo"; list5.Value = "5"; ListaMes.Add(list5);
            var list6 = new SelectListItem();
            list6.Text = "Junio"; list6.Value = "6"; ListaMes.Add(list6);
            var list7 = new SelectListItem();
            list7.Text = "Julio"; list7.Value = "7"; ListaMes.Add(list7);
            var list8 = new SelectListItem();
            list8.Text = "Agosto"; list8.Value = "8"; ListaMes.Add(list8);
            var list9 = new SelectListItem();
            list9.Text = "Setiembre"; list9.Value = "9"; ListaMes.Add(list9);
            var list10 = new SelectListItem();
            list10.Text = "Octubre"; list10.Value = "10"; ListaMes.Add(list10);
            var list11 = new SelectListItem();
            list11.Text = "Noviembre"; list11.Value = "11"; ListaMes.Add(list11);
            var list12 = new SelectListItem();
            list12.Text = "Diciembre"; list12.Value = "12"; ListaMes.Add(list12);
            return ListaMes;
        }

        //Almacena un archivo en excel en un data set
        public DataSet GeneraDataset(string RutaArchivo, int hoja)
        {
            FileInfo archivo = new FileInfo(RutaArchivo);
            ExcelPackage xlPackage = new ExcelPackage(archivo);
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            for (int j = 1; j <= ws.Dimension.End.Column; j = j + 1)
            {
                string Columna = "";
                if (ws.Cells[1, j].Value != null) Columna = ws.Cells[1, j].Value.ToString();
                dt.Columns.Add(Columna);
            }

            for (int i = 2; i <= ws.Dimension.End.Row; i = i + 1)
            {
                DataRow row = dt.NewRow();
                for (int j = 1; j <= ws.Dimension.End.Column; j = j + 1)
                {
                    if (ws.Cells[i, j].Value == null)
                        row[j - 1] = "null";
                    else
                        row[j - 1] = ws.Cells[i, j].Value.ToString().Trim();
                }
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            xlPackage.Dispose();
            return ds;
        }

        //Valida que la información ingresada solo contenga numeros y letras
        public string CorregirCodigo(string str) 
        {   string strLimpio = "";
            str = str.ToUpper();
            for (int i = 0; i < str.Length; i++) 
            {   if ( (str[i] >= '0' && str[i] <= '9') || 
                     (str[i] >= 'A' && str[i] <= 'Z')
                )
                { strLimpio = strLimpio + str[i]; } 
            }
            return strLimpio; 
        }

        //Lista los tipos de tecnología
        public List<TipoTecnologia> listTipoTecnologia()
        {
            List<TipoTecnologia> lista = new List<TipoTecnologia>();

            TipoTecnologia dto = new TipoTecnologia();
            dto.Id = 0; dto.Tipo = "NA";
            lista.Add(dto);

            dto = new TipoTecnologia();
            dto.Id = 1; dto.Tipo = "CICLO COMBINADO";
            lista.Add(dto);

            dto = new TipoTecnologia();
            dto.Id = 2; dto.Tipo = "CICLO SIMPLE";
            lista.Add(dto);

            dto = new TipoTecnologia();
            dto.Id = 3; dto.Tipo = "MOTORES RECIPROCANTES";
            lista.Add(dto);

            return lista;
        }
    }
}

