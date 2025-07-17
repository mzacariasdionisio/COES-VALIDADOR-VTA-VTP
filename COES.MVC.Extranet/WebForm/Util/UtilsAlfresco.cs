using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WSIC2010.Util
{
    public static class UtilsAlfresco
    {
        public static string GetEnlace(object ao_id)
        {
            string ls_id;
            ls_id = Convert.ToString(ao_id);
            //srGestor.CMSManagerClient gcCOES = new srGestor.CMSManagerClient();
            //wsGC.CMSManager gcCOES = new wsGC.CMSManager();

            string ls_link;
            ls_link = "http://sicoes.coes.org.pe/wsic2010/webform/download/download.aspx?nodeId=" + ls_id;
            return ls_link;
        }

        public static string GetFile(object ao_id)
        {
            string ls_id;
            ls_id = Convert.ToString(ao_id);

            string ls_link;
            ls_link = "http://www.coes.org.pe/wsic2010/webform/download/download.aspx?nodeId=" + ls_id;

            return ls_link;

        }
        
        public static string GetIcon(object ao_name)
        {
            string ls_name_in, ls_format;
            ls_name_in = Convert.ToString(ao_name);
            string[] ls_array_cadenas = ls_name_in.Split(new char[] { '.' });
            int li_longitud = ls_array_cadenas.Length;
            string BASE_PATH = "http://sicoes.coes.org.pe/wsic2010/webform/images/icon_gc/";
            ls_format = ls_array_cadenas[li_longitud - 1];

            switch (ls_format.ToLower())
            {
                case "pdf":
                    return BASE_PATH + ls_format + ".gif";
                case "xls":
                    return BASE_PATH + ls_format + ".gif";
                case "doc":
                    return BASE_PATH + ls_format + ".gif";
                case "ppt":
                    return BASE_PATH + ls_format + ".gif";
                case "xlsx":
                    return BASE_PATH + ls_format + ".gif";
                case "docx":
                    return BASE_PATH + ls_format + ".gif";
                case "pptx":
                    return BASE_PATH + ls_format + ".gif";
                case "zip":
                    return BASE_PATH + ls_format + ".gif";
                case "rar":
                    return BASE_PATH + ls_format + ".gif";
                case "jpg":
                    return BASE_PATH + ls_format + ".gif";
                case "gif":
                    return BASE_PATH + ls_format + ".gif";
                case "html":
                    return BASE_PATH + ls_format + ".gif";
                default:
                    return BASE_PATH + "default.gif";
            }

        }

        public static string GetFecha(object ao_fecha)
        {
            string ls_cadena;
            DateTime ldt_fecha = new DateTime();
            ls_cadena = Convert.ToString(ao_fecha);

            if (ls_cadena != null && !String.IsNullOrWhiteSpace(ls_cadena))
            {
                ldt_fecha = Convert.ToDateTime(ls_cadena);
                return ldt_fecha.ToString("dd-MM-yyyy");
            }
            else
            {
                return String.Empty;
            }
        }

        public static List<int> CreateTableDiasAtencion(DataTable adt_tabla)
        {
            List<int> listaDias = new List<int>();
            int li_dia;
            foreach (DataRow row in adt_tabla.Rows)
            {
                if (Int32.TryParse(Convert.ToString(row["diasTotales"]), out li_dia))
                {
                    listaDias.Add(li_dia);
                }
            }

            return listaDias;

        }

        public static string GetResumenSumilla(object ao_sumilla)
        {
            string ls_resumen;
            ls_resumen = Convert.ToString(ao_sumilla);

            if (ls_resumen.Length > 100)
            {
                return ls_resumen.Substring(0, 100) + " <a href='#' tooltip='casos de prueba'>Leáse más ...</a>";
            }
            else
            {
                return ls_resumen;
            }
        }

        public static string GetSumilla(object ao_sumilla)
        {
            string ls_sumilla;
            ls_sumilla = Convert.ToString(ao_sumilla);

            return ls_sumilla;
        }

        public static DataTable GetDataTableOrdered(DataTable adt_Data, string ls_sortExp, string ls_sortDir)
        {
            if (adt_Data.Rows.Count > 0)
            {
                //Convertir DataTable a DataView  
                DataView dv = adt_Data.DefaultView;
                //Aplicar ordenamiento en la columna
                dv.Sort = string.Format("{0} {1}", ls_sortExp, ls_sortDir);
                //Se devuelve el ordenamiento a dt_Data
                adt_Data = dv.ToTable();
            }

            return adt_Data;
        }

    }
}