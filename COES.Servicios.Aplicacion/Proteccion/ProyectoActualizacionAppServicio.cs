using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Proteccion.Helper;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace COES.Servicios.Aplicacion.Equipamiento
{
    /// <summary>
    /// Clases con métodos del módulo Equipamiento
    /// </summary>
    public class ProyectoActualizacionAppServicio : AppServicioBase
    {
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();

        /// <summary>
        /// Propiedades de las coordenadas
        /// </summary>
        private const int PropiedadCoorX = 1814;
        private const int PropiedadCoorY = 1815;


        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ProyectoActualizacionAppServicio));


        #region GESPROTEC-20241031
        /// <summary>
        /// Devuelve el listado de equipos COES
        /// </summary>
        /// <returns></returns>
        public List<EprProyectoActEqpDTO> EprProyectoList(int epproysgcodi, string epproynomb, string epproyfecregistroIni, string epproyfecregistroFin)
        {
            return FactorySic.GetEprProyectoActEqpRepository().List(epproysgcodi, epproynomb, epproyfecregistroIni, epproyfecregistroFin);
        }

        public EprProyectoActEqpDTO EprProyectoGetById(int Epproycodi)
        {
            return FactorySic.GetEprProyectoActEqpRepository().GetById(Epproycodi);
        }

        public int EprProyectoSave(EprProyectoActEqpDTO entity)
        {
            return FactorySic.GetEprProyectoActEqpRepository().Save(entity);
        }

        public void EprProyectoUpdate(EprProyectoActEqpDTO entity)
        {
            FactorySic.GetEprProyectoActEqpRepository().Update(entity);
        }

        public void EprProyectoDelete_UpdateAuditoria(EprProyectoActEqpDTO entity)
        {
            FactorySic.GetEprProyectoActEqpRepository().Delete_UpdateAuditoria(entity);
        }


        public EprAreaDTO EprAreaGetById(int Epareacodi)
        {
            return FactorySic.GetEprAreaRepository().GetById(Epareacodi);
        }

        public int EprAreaSave(EprAreaDTO entity)
        {
            return FactorySic.GetEprAreaRepository().Save(entity);
        }

        public void EprAreaUpdate(EprAreaDTO entity)
        {
            FactorySic.GetEprAreaRepository().Update(entity);
        }

        public void EprAreaDelete_UpdateAuditoria(EprAreaDTO entity)
        {
            FactorySic.GetEprAreaRepository().Delete_UpdateAuditoria(entity);
        }


        public EprEquipoDTO EprEquipoGetById(int epequicodi)
        {
            return FactorySic.GetEprEquipoRepository().GetById(epequicodi);
        }

        public int EprEquipoSave(EprEquipoDTO entity)
        {
            return FactorySic.GetEprEquipoRepository().Save(entity);
        }

        public void EprEquipoUpdate(EprEquipoDTO entity)
        {
            FactorySic.GetEprEquipoRepository().Update(entity);
        }

        public void EprEquipoDelete_UpdateAuditoria(EprEquipoDTO entity)
        {
            FactorySic.GetEprEquipoRepository().Delete_UpdateAuditoria(entity);
        }

        public List<EprEquipoDTO> ListEquipamientoModificado(int epproycodi)
        {
            return FactorySic.GetEprEquipoRepository().ListEquipamientoModificado(epproycodi);
        }

        public EprAreaDTO GetCantidadUbicacionSGOCOESEliminar(int epproycodi)
        {
            return FactorySic.GetEprAreaRepository().GetCantidadUbicacionSGOCOESEliminar(epproycodi);
        }

        public EprEquipoDTO GetCantidadEquipoSGOCOESEliminar(int epequicodi)
        {
            return FactorySic.GetEprEquipoRepository().GetCantidadEquipoSGOCOESEliminar(epequicodi);
        }

        #region Metodos Carga Masiva
        /// <summary>
        /// Metodo para obtener un registro de las cargas masivas realizadas
        /// </summary>
        /// <param name="tipoUso"></param>
        /// <param name="usuario"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        public List<EprCargaMasivaDTO> ListaCargaMasiva(int tipoUso, string usuario, string fecIni, string fecFin)
        {
            return FactorySic.GetEprCargaMasivaRepository().ListarCargaMasivaFiltro(tipoUso, usuario, fecIni, fecFin);
        }

        /// <summary>
        /// Registro de Carga Masiva
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public int SaveCargaMasiva(EprCargaMasivaDTO entidad)
        {
            return FactorySic.GetEprCargaMasivaRepository().Save(entidad);
        }

        /// <summary>
        /// Metodo para validar información de cada registro del archivo de carga
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string ValidarProteccionesUsoGeneral(EprCargaMasivaDetalleDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().ValidarProteccionesUsoGeneral(entity);
        }

        /// <summary>
        /// Metodo para grabar cada registro del srchivo de carga
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveProteccionesUsoGeneral(EprCargaMasivaDetalleDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().SaveProteccionesUsoGeneral(entity);
        }

        public string ValidarProteccionesMandoSincronizado(EprCargaMasivaDetalleDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().ValidarProteccionesMandoSincronizado(entity);
        }

        public string SaveProteccionesMandoSincronizado(EprCargaMasivaDetalleDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().SaveProteccionesMandoSincronizado(entity);
        }

        public string ValidarProteccionesTorsional(EprCargaMasivaDetalleDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().ValidarProteccionesTorsional(entity);
        }

        public string SaveProteccionesTorsional(EprCargaMasivaDetalleDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().SaveProteccionesTorsional(entity);
        }

        public string ValidarProteccionesPmu(EprCargaMasivaDetalleDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().ValidarProteccionesPmu(entity);
        }

        public string SaveProteccionesPmu(EprCargaMasivaDetalleDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().SaveProteccionesPmu(entity);
        }

        /// <summary>
        /// Generar Excel de reporte plantilla
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void GenerarExcelPlantilla(string path, string fileName, int tipoUso)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesProteccionAppServicio.HojaEmpresa);
                hojas.Add(ConstantesProteccionAppServicio.HojaCelda);
                hojas.Add(ConstantesProteccionAppServicio.HojaRele);
                hojas.Add(ConstantesProteccionAppServicio.HojaInterruptor);
                hojas.Add(ConstantesProteccionAppServicio.HojaMarca);
                hojas.Add(ConstantesProteccionAppServicio.HojaProyecto);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + fileName + " No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesProteccionAppServicio.HojaPlantillaExcel];

                    xlPackage.Save();

                    foreach (var item in hojas)
                    {
                        GenerarFileExcelHoja(xlPackage, item, tipoUso);
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

        private void GenerarFileExcelHoja(ExcelPackage xlPackage, string hoja, int tipoUso)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];
            int row = 2;
            int columnIniData = 1;
            switch (hoja)
            {
                case ConstantesProteccionAppServicio.HojaEmpresa:
                    //obtener empresas
                    var listaempresas = FactorySic.GetSiEmpresaRepository().ListarMaestroEmpresasProteccion();
                    var listaFormatoEmpresa = listaempresas.Select(x => new { x.Emprabrev, x.Emprnomb, x.Emprrazsocial, x.Tipoemprdesc });

                    ws.Cells[2, 1].LoadFromCollection(listaFormatoEmpresa, false);

                    break;
                case ConstantesProteccionAppServicio.HojaCelda:
                    //obtener familias
                    var listaFamilias = FactorySic.GetEqEquipoRepository().ListarMaestroCeldaProteccion();

                    var listaFormatoCelda = listaFamilias.Select(x => new { x.Emprabrev, x.Areanomb, x.Equinomb, x.Emprnomb});

                    ws.Cells[2,1].LoadFromCollection(listaFormatoCelda,false);

                    break;
                case ConstantesProteccionAppServicio.HojaRele:
                    //obtener Ubicaciones
                    var listaUbicaciones = FactorySic.GetEqEquipoRepository().ListarMaestroReleProteccion(tipoUso);

                    var listaFormatoRele = listaUbicaciones.Select(x => new { x.Emprabrev, x.Areanomb, x.Equinomb, x.Emprnomb });
                    ws.Cells[2, 1].LoadFromCollection(listaFormatoRele, false);

                    break;
                case ConstantesProteccionAppServicio.HojaInterruptor:
                    //obtener Subestación
                    var listaSubestaciones = FactorySic.GetEqEquipoRepository().ListarMaestroInterruptorProteccion();

                    var listaFormatoInterruptor = listaSubestaciones.Select(x => new { x.Emprabrev, x.Areanomb, x.Equinomb, x.Emprnomb });
                    ws.Cells[2, 1].LoadFromCollection(listaFormatoInterruptor, false);

                    break;

                case ConstantesProteccionAppServicio.HojaMarca:
                    //obtener Subestación
                    var listaMarcaProteccion = FactorySic.GetEprPropCatalogoDataRepository().ListMaestroMarcaProteccion();

                    var listaFormatoMarca = listaMarcaProteccion.Select(x => new { x.Eqcatdabrev, x.Eqcatddescripcion});
                    ws.Cells[2, 1].LoadFromCollection(listaFormatoMarca, false);

                    break;
                case ConstantesProteccionAppServicio.HojaProyecto:
                    //obtener Subestación
                    var listaMaestroProyecto = FactorySic.GetEprProyectoActEqpRepository().ListMaestroProyecto();

                    var listaFormatoProyecto = listaMaestroProyecto.Select(x => new { x.Epproydescripcion, x.Epproynemotecnico, x.Epproynomb, x.Emprnomb, x.Epproyfecregistro });
                    ws.Cells[2, 1].LoadFromCollection(listaFormatoProyecto, false);

                    break;
            }

        }




        #endregion

        #endregion

        public void GenerarExportacionProyectoActualizacion(string ruta, string pathLogo, string nameFile, List<EprProyectoActEqpDTO> lProyecto)
        {
            ////Descargo archivo segun requieran
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarArchivoExcelProyectoActualizacion(xlPackage, pathLogo, lProyecto);

                xlPackage.Save();
            }
        }

        private void GenerarArchivoExcelProyectoActualizacion(ExcelPackage xlPackage, string pathLogo, List<EprProyectoActEqpDTO> lProyecto)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            string nameWS = "REPORTE";
            string titulo = "LISTADO DE PROYECTO ACTUALIZACION";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;

            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo, 120, 70);

            #region  Filtros y Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 4;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 2;

            int colArea = colIniTable;
            int colCodigo = colIniTable + 1;
            int colNombre = colIniTable + 2;
            int colFecha = colIniTable + 3;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha, "Calibri", 16);

            ws.Row(rowIniTabla).Height = 25;
            ws.Cells[rowIniTabla, colArea].Value = "Área";
            ws.Cells[rowIniTabla, colCodigo].Value = "Código";
            ws.Cells[rowIniTabla, colNombre].Value = "Nombre";
            ws.Cells[rowIniTabla, colFecha].Value = "Fecha";

            //Estilos cabecera
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colArea, rowIniTabla, colFecha, "Calibri", 11);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colArea, rowIniTabla, colFecha, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colArea, rowIniTabla, colFecha, "Centro");
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colArea, rowIniTabla, colFecha, "#2980B9");
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colArea, rowIniTabla, colFecha, "#FFFFFF");
            servFormato.BorderCeldas2(ws, rowIniTabla, colArea, rowIniTabla, colFecha);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colArea, rowIniTabla, colFecha);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in lProyecto)
            {
                ws.Cells[rowData, colArea].Value = item.Area;
                ws.Cells[rowData, colCodigo].Value = item.Epproynemotecnico;
                ws.Cells[rowData, colNombre].Value = item.Epproynomb;
                ws.Cells[rowData, colFecha].Value = item.Epproyfecregistro;

                rowData++;
            }

            if (!lProyecto.Any()) rowData++;

            //Estilos registros
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colArea, rowData - 1, colFecha, "Calibri", 8);
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colArea, rowData - 1, colFecha, "Centro");
            servFormato.BorderCeldas2(ws, rowIniTabla + 1, colArea, rowData - 1, colFecha);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colArea, rowData - 1, colFecha, "Centro");

            #endregion

            //filter           
            ws.Cells[rowIniTabla, colArea, rowData, colFecha].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        public EprProyectoActEqpDTO ValidarProyectoActualizacionPorRele(int epproycodi)
        {
            return FactorySic.GetEprProyectoActEqpRepository().ValidarProyectoActualizacionPorRele(epproycodi);
        }
    }
}
