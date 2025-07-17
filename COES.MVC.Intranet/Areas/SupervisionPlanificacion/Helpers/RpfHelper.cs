using COES.Dominio.DTO.Sic;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SupervisionPlanificacion.Helpers
{
    public class RpdHelper
    {
        public List<String> Errores = null;

        /// <summary>
        /// Permite obtener la configuracion del RPF
        /// </summary>
        //ConfiguracionRPD configuracion = new ConfiguracionRPD();

        /// <summary>
        /// Lee los artículos desde el formato excel cargado
        /// </summary>
        /// <param name="codCliente"></param>
        /// <returns></returns>
        public List<DesviacionDTO> LeerDesdeFormato(string file, out List<String> errores)
        {
            try
            {
                this.Errores = new List<string>();
                List<DesviacionDTO> list = new List<DesviacionDTO>();

                FileInfo fileInfo = new FileInfo(file);

                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets["DESV-SUB-SOB"];                  

                    int index = 6;
                    int columnIni = 5;
                    int columnFin = 8;
                    list.AddRange(this.LeerDatos(index, columnIni, columnFin, ws));
                    columnIni = 9;
                    columnFin = 12;
                    list.AddRange(this.LeerDatos(index, columnIni, columnFin, ws));
                    columnIni = 13;
                    columnFin = 16;
                    list.AddRange(this.LeerDatos(index, columnIni, columnFin, ws));
                }

                errores = this.Errores;
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite leer los datos
        /// </summary>
        /// <param name="rowIni"></param>
        /// <param name="columnIni"></param>
        /// <param name="columnFin"></param>
        /// <returns></returns>
        private List<DesviacionDTO> LeerDatos(int rowIni, int columnIni, int columnFin, ExcelWorksheet ws)
        {
            List<DesviacionDTO> entitys = new List<DesviacionDTO>();
            while (true)
            {
                object value1 = ws.Cells[rowIni, columnIni].Value;
                object value2 = ws.Cells[rowIni, columnFin].Value;

                if (value1 != null && value2 != null)
                {
                    bool flag = false;
                    int ptoMedicodi = 0;
                    
                    if (int.TryParse(value1.ToString(), out ptoMedicodi))
                    {
                        int origen = 0;
                        if (int.TryParse(value2.ToString(), out origen))
                        {
                            DesviacionDTO entity = new DesviacionDTO();
                            entity.Ptomedicodi = ptoMedicodi;
                            entity.Medorigdesv = origen;
                            entitys.Add(entity);
                            flag = true;
                        }
                    }

                    if (!flag)
                    {
                        this.Errores.Add(String.Format("Error fila: {0}, Columa1: {1}, Columna2 : {2}", rowIni, columnIni, columnFin));
                    }
                }
                else 
                {
                    break;
                }

                rowIni++;
            }

            return entitys;            
        }
    }
}