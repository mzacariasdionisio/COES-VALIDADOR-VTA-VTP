using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Evaluacion.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace COES.Servicios.Aplicacion.Evaluacion
{
    /// <summary>
    /// Clases con métodos del módulo Equipamiento
    /// </summary>
    public class ReleAppServicio : AppServicioBase
    {

    

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReleAppServicio));


        #region GESPROTEC
        /// <summary>
        /// Devuelve el listado de relés de sncronismo
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaReleSincronismo(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            return FactorySic.GetEprEquipoRepository().ListaReleSincronismo(codigoId, codigo, subestacion, celda,
            empresa, area, estado);
        }

        /// <summary>
        /// Devuelve el listado de relés de sobretension
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaReleSobretension(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            return FactorySic.GetEprEquipoRepository().ListaReleSobretension(codigoId, codigo, subestacion, celda,
            empresa, area, estado);
        }

        /// <summary>
        /// Devuelve el listado de relés de mando sincronizado
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaReleMandoSincronizado(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            return FactorySic.GetEprEquipoRepository().ListaReleMandoSincronizado(codigoId, codigo, subestacion, celda,
            empresa, area, estado);
        }

        /// <summary>
        /// Devuelve el listado de relés torcional
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaReleTorcional(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            return FactorySic.GetEprEquipoRepository().ListaReleTorcional(codigoId, codigo, subestacion, celda,
            empresa, area, estado);
        }

        /// <summary>
        /// Devuelve el listado de relés PMU
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaRelePMU(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            return FactorySic.GetEprEquipoRepository().ListaRelePMU(codigoId, codigo, subestacion, celda,
            empresa, area, estado);
        }
        #endregion

        #region Exxportacion Reles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="codigoId"></param>
        /// <param name="codigo"></param>
        /// <param name="subestacion"></param>
        /// <param name="celda"></param>
        /// <param name="empresa"></param>
        /// <param name="area"></param>
        /// <param name="estado"></param>
        /// <exception cref="Exception"></exception>
        public void GenerarExcelExportarReleSincronismo(string path, string fileName, string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesEvaluacionAppServicio.HojaListado);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + fileName + " No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {

                    foreach (var item in hojas)
                    {

                        switch (item)
                        {

                            case ConstantesEvaluacionAppServicio.HojaListado:
                             
                                var lista = FactorySic.GetEprEquipoRepository().ListaExportarReleSincronismo(codigoId, codigo, subestacion, celda, empresa, area, estado);
                                GenerarFileExcelHojaExportarReleSincronismo(xlPackage, item, lista);

                                break;
                        }

                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private void GenerarFileExcelHojaExportarReleSincronismo(ExcelPackage xlPackage, string hoja, List<EprEquipoDTO> listaEmpresas)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            int index = 11;            

            foreach (var item in listaEmpresas)
            {               
                ws.Cells[index, 1].Value = item.Equicodi;
                ws.Cells[index, 2].Value = item.Equinomb;
                ws.Cells[index, 3].Value = item.Area;
                ws.Cells[index, 4].Value = item.Empresa;
                ws.Cells[index, 5].Value = item.Subestacion;               

                ws.Cells[index, 6].Value = item.Celda;                
                ws.Cells[index, 7].Value = item.CodigoInterruptor;               
                ws.Cells[index, 8].Value = item.DeltaTension;               
                ws.Cells[index, 9].Value = item.DeltaAngulo;
                ws.Cells[index, 10].Value = item.DeltaFrecuencia;

                index++;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="codigoId"></param>
        /// <param name="codigo"></param>
        /// <param name="subestacion"></param>
        /// <param name="celda"></param>
        /// <param name="empresa"></param>
        /// <param name="area"></param>
        /// <param name="estado"></param>
        /// <exception cref="Exception"></exception>
        public void GenerarExcelExportarReleSobreTension(string path, string fileName, string codigoId, string codigo, int subestacion, int celda,
           int empresa, int area, string estado)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesEvaluacionAppServicio.HojaListado);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + fileName + " No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {

                    foreach (var item in hojas)
                    {

                        switch (item)
                        {

                            case ConstantesEvaluacionAppServicio.HojaListado:

                                var lista = FactorySic.GetEprEquipoRepository().ListaExportarReleSobreTension(codigoId, codigo, subestacion, celda, empresa, area, estado);
                                GenerarFileExcelHojaExportarReleSobreTension(xlPackage, item, lista);

                                break;
                        }

                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private void GenerarFileExcelHojaExportarReleSobreTension(ExcelPackage xlPackage, string hoja, List<EprEquipoDTO> listaEmpresas)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            int index = 11;

            foreach (var item in listaEmpresas)
            {
                ws.Cells[index, 1].Value = item.Equicodi;
                ws.Cells[index, 2].Value = item.Equinomb;
                ws.Cells[index, 3].Value = item.Area;
                ws.Cells[index, 4].Value = item.Empresa;
                ws.Cells[index, 5].Value = item.Subestacion;
                ws.Cells[index, 6].Value = item.Celda;

                ws.Cells[index, 7].Value = item.NivelTension;
                ws.Cells[index, 8].Value = item.SobreTU;
                ws.Cells[index, 9].Value = item.SobreTT;
                ws.Cells[index, 10].Value = item.SobreTUU;
                ws.Cells[index, 11].Value = item.SobreTTT;

                index++;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="codigoId"></param>
        /// <param name="codigo"></param>
        /// <param name="subestacion"></param>
        /// <param name="celda"></param>
        /// <param name="empresa"></param>
        /// <param name="area"></param>
        /// <param name="estado"></param>
        /// <exception cref="Exception"></exception>
        public void GenerarExcelExportarReleMandoSincronizado(string path, string fileName, string codigoId, string codigo, int subestacion, int celda,
         int empresa, int area, string estado)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesEvaluacionAppServicio.HojaListado);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + fileName + " No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {

                    foreach (var item in hojas)
                    {

                        switch (item)
                        {

                            case ConstantesEvaluacionAppServicio.HojaListado:

                                var lista = FactorySic.GetEprEquipoRepository().ListaExportarReleMandoSincronizado(codigoId, codigo, subestacion, celda, empresa, area, estado);
                                GenerarFileExcelHojaExportarReleMandoSincronizado(xlPackage, item, lista);

                                break;
                        }

                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private void GenerarFileExcelHojaExportarReleMandoSincronizado(ExcelPackage xlPackage, string hoja, List<EprEquipoDTO> listaEmpresas)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            int index = 11;

            foreach (var item in listaEmpresas)
            {
                ws.Cells[index, 1].Value = item.Equicodi;
                ws.Cells[index, 2].Value = item.Equinomb;
                ws.Cells[index, 3].Value = item.Area;
                ws.Cells[index, 4].Value = item.Empresa;
                ws.Cells[index, 5].Value = item.Subestacion;

                ws.Cells[index, 6].Value = item.Celda;
                ws.Cells[index, 7].Value = item.MandoSincronizado;               

                index++;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="codigoId"></param>
        /// <param name="codigo"></param>
        /// <param name="subestacion"></param>
        /// <param name="celda"></param>
        /// <param name="empresa"></param>
        /// <param name="area"></param>
        /// <param name="estado"></param>
        /// <exception cref="Exception"></exception>
        public void GenerarExcelExportarReleTorsional(string path, string fileName, string codigoId, string codigo, int subestacion, int celda,
       int empresa, int area, string estado)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesEvaluacionAppServicio.HojaListado);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + fileName + " No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {

                    foreach (var item in hojas)
                    {

                        switch (item)
                        {

                            case ConstantesEvaluacionAppServicio.HojaListado:

                                var lista = FactorySic.GetEprEquipoRepository().ListaExportarReleTorsional(codigoId, codigo, subestacion, celda, empresa, area, estado);
                                GenerarFileExcelHojaExportarReleTorsional(xlPackage, item, lista);

                                break;
                        }

                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private void GenerarFileExcelHojaExportarReleTorsional(ExcelPackage xlPackage, string hoja, List<EprEquipoDTO> listaEmpresas)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            int index = 11;

            foreach (var item in listaEmpresas)
            {
                ws.Cells[index, 1].Value = item.Equicodi;
                ws.Cells[index, 2].Value = item.Equinomb;
                ws.Cells[index, 3].Value = item.Area;
                ws.Cells[index, 4].Value = item.Empresa;
                ws.Cells[index, 5].Value = item.Subestacion;

                ws.Cells[index, 6].Value = item.Celda;
                ws.Cells[index, 7].Value = item.MedidaMitigacion;
                ws.Cells[index, 8].Value = item.ReleTorsionalImplementadoDsc;
               

                index++;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="codigoId"></param>
        /// <param name="codigo"></param>
        /// <param name="subestacion"></param>
        /// <param name="celda"></param>
        /// <param name="empresa"></param>
        /// <param name="area"></param>
        /// <param name="estado"></param>
        /// <exception cref="Exception"></exception>
        public void GenerarExcelExportarRelePmu(string path, string fileName, string codigoId, string codigo, int subestacion, int celda,
     int empresa, int area, string estado)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesEvaluacionAppServicio.HojaListado);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + fileName + " No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {

                    foreach (var item in hojas)
                    {

                        switch (item)
                        {

                            case ConstantesEvaluacionAppServicio.HojaListado:

                                var lista = FactorySic.GetEprEquipoRepository().ListaExportarRelePmu(codigoId, codigo, subestacion, celda, empresa, area, estado);
                                GenerarFileExcelHojaExportarRelePmu(xlPackage, item, lista);

                                break;
                        }

                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private void GenerarFileExcelHojaExportarRelePmu(ExcelPackage xlPackage, string hoja, List<EprEquipoDTO> listaEmpresas)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            int index = 11;

            foreach (var item in listaEmpresas)
            {
                ws.Cells[index, 1].Value = item.Equicodi;
                ws.Cells[index, 2].Value = item.Equinomb;
                ws.Cells[index, 3].Value = item.Area;
                ws.Cells[index, 4].Value = item.Empresa;
                ws.Cells[index, 5].Value = item.Subestacion;

                ws.Cells[index, 6].Value = item.Celda;
                ws.Cells[index, 7].Value = item.Accion;
                ws.Cells[index, 8].Value = item.Estado;
               

                index++;
            }

        }

        #endregion
    }
}
