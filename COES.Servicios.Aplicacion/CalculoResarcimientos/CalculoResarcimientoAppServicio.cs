using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DocumentFormat.OpenXml.Bibliography;
using log4net;
using Novacode;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paragraph = Novacode.Paragraph;
using Table = Novacode.Table;




namespace COES.Servicios.Aplicacion.CalculoResarcimientos
{

    public class CalculoResarcimientoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CalculoResarcimientoAppServicio));

        /// <summary>
        /// Permite listar todos los registros de la tabla RE_NIVEL_TENSION
        /// </summary>
        public List<ReNivelTensionDTO> ListReNivelTensions()
        {
            return FactorySic.GetReNivelTensionRepository().List();
        }

        #region Métodos Tabla RE_ENVIO

        /// <summary>
        /// Inserta un registro de la tabla RE_ENVIO
        /// </summary>
        public void SaveReEnvio(ReEnvioDTO entity)
        {
            try
            {
                FactorySic.GetReEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RE_ENVIO
        /// </summary>
        public ReEnvioDTO GetByIdReEnvio(int reenvcodi)
        {
            return FactorySic.GetReEnvioRepository().GetById(reenvcodi);
        }
        #endregion

        #region Métodos Tabla RE_PUNTO_ENTREGA

        /// <summary>
        /// Inserta un registro de la tabla RE_PUNTO_ENTREGA
        /// </summary>
        public void SaveRePuntoEntrega(RePuntoEntregaDTO entity)
        {
            try
            {
                FactorySic.GetRePuntoEntregaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RE_PUNTO_ENTREGA
        /// </summary>
        public void UpdateRePuntoEntrega(RePuntoEntregaDTO entity)
        {
            try
            {
                FactorySic.GetRePuntoEntregaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RE_PUNTO_ENTREGA
        /// </summary>
        public void DeleteRePuntoEntrega(int repentcodi)
        {
            try
            {
                FactorySic.GetRePuntoEntregaRepository().Delete(repentcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RE_PUNTO_ENTREGA
        /// </summary>
        public RePuntoEntregaDTO GetByIdRePuntoEntrega(int repentcodi)
        {
            return FactorySic.GetRePuntoEntregaRepository().GetById(repentcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RE_PUNTO_ENTREGA
        /// </summary>
        public List<RePuntoEntregaDTO> ListRePuntoEntregas()
        {
            return FactorySic.GetRePuntoEntregaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RePuntoEntrega
        /// </summary>
        public List<RePuntoEntregaDTO> GetByCriteriaRePuntoEntregas()
        {
            return FactorySic.GetRePuntoEntregaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla RE_PERIODO

        /// <summary>
        /// Inserta un registro de la tabla RE_PERIODO
        /// </summary>
        public int GrabarPeriodo(RePeriodoDTO entity)
        {
            try
            {
                int count = FactorySic.GetRePeriodoRepository().ValidarNombre(entity.Repernombre, entity.Repercodi);

                if (count == 0)
                {

                    int id = 0;
                    entity.Reperfecmodificacion = DateTime.Now;
                    entity.Reperusumodificacion = entity.Reperusucreacion;

                    if (entity.Repercodi == 0)
                    {
                        entity.Reperfeccreacion = DateTime.Now;
                        id = FactorySic.GetRePeriodoRepository().Save(entity);

                        if (entity.Repertipo == ConstantesCalculoResarcimiento.IdPeriodoSemestral && entity.Reperpadre == 0)
                        {
                            this.ImportarInsumos(id, (int)entity.Reperanio, entity.Reperusucreacion);
                        }
                    }
                    else
                    {
                        FactorySic.GetRePeriodoRepository().Update(entity);
                        id = entity.Repercodi;
                    }

                    FactorySic.GetRePeriodoEtapaRepository().Delete(id);

                    List<string> periodos = entity.Data.Split(ConstantesAppServicio.CaracterGuion).ToList();

                    foreach (string periodo in periodos)
                    {
                        if (!string.IsNullOrEmpty(periodo))
                        {
                            string[] item = periodo.Split(ConstantesAppServicio.CaracterArroba);

                            RePeriodoEtapaDTO etapa = new RePeriodoEtapaDTO();
                            etapa.Reetacodi = int.Parse(item[0]);
                            etapa.Repercodi = id;

                            if (!string.IsNullOrEmpty(item[1]))
                            {
                                DateTime fecha = DateTime.ParseExact(item[1], ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                                etapa.Repeetfecha = fecha;
                                if (fecha.Subtract(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).TotalDays > 0)
                                    etapa.Repeetestado = ConstantesCalculoResarcimiento.EstadoProceso;
                            }

                            etapa.Repeetestado = ConstantesCalculoResarcimiento.EstadoCulminado;

                            etapa.Repeetusucreacion = entity.Reperusucreacion;
                            etapa.Repeetusumodificacion = entity.Reperusumodificacion;
                            etapa.Repeetfeccreacion = DateTime.Now;
                            etapa.Repeetfecmodificacion = DateTime.Now;

                            FactorySic.GetRePeriodoEtapaRepository().Save(etapa);
                        }
                    }

                    return 1;
                }
                else
                    return 2;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite imporar los insumos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="anio"></param>
        /// <param name="usuario"></param>
        private void ImportarInsumos(int id, int anio, string usuario)
        {
            List<RePeriodoDTO> entitys = FactorySic.GetRePeriodoRepository().ObtenerPeriodosCercanos(anio, id);

            if (entitys.Count > 0)
            {
                int idPeriodo = entitys[0].Repercodi;

                //- Realizamos importación de insumos
                List<RePtoentregaPeriodoDTO> ptoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodo);

                foreach (RePtoentregaPeriodoDTO item in ptoEntrega)
                {
                    item.Repercodi = id;
                    item.Reptopusucreacion = usuario;
                    item.Reptopusumodificacion = usuario;
                    item.Reptopfeccreacion = DateTime.Now;
                    item.Reptopfecmodificacion = DateTime.Now;
                    FactorySic.GetRePtoentregaPeriodoRepository().Save(item);
                }


                //- Listado de indicadores
                List<ReIndicadorPeriodoDTO> indicadores = FactorySic.GetReIndicadorPeriodoRepository().ObtenerParaImportar(idPeriodo);

                foreach (ReIndicadorPeriodoDTO item in indicadores)
                {
                    item.Repercodi = id;
                    item.Reindusucreacion = usuario;
                    item.Reindusumodificacion = usuario;
                    item.Reindfeccreacion = DateTime.Now;
                    item.Reindfecmodificacion = DateTime.Now;
                    FactorySic.GetReIndicadorPeriodoRepository().Save(item);
                }

                ///- Listado de niveles de tolerancia
                List<ReToleranciaPeriodoDTO> tolerancia = FactorySic.GetReToleranciaPeriodoRepository().ObtenerParaImportar(idPeriodo);

                foreach (ReToleranciaPeriodoDTO item in tolerancia)
                {
                    item.Repercodi = id;
                    item.Retolusucreacion = usuario;
                    item.Retolusumodificacion = usuario;
                    item.Retolfeccreacion = DateTime.Now;
                    item.Retolfecmodificacion = DateTime.Now;
                    FactorySic.GetReToleranciaPeriodoRepository().Save(item);
                }
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RE_PERIODO
        /// </summary>
        public int DeleteRePeriodo(int repercodi)
        {
            try
            {
                FactorySic.GetRePeriodoRepository().Delete(repercodi);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RE_PERIODO
        /// </summary>
        public RePeriodoDTO GetByIdRePeriodo(int repercodi)
        {
            return FactorySic.GetRePeriodoRepository().GetById(repercodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RePeriodo
        /// </summary>
        public List<RePeriodoDTO> GetByCriteriaRePeriodos(int anioDesde, int anioHasta, string estado)
        {
            return FactorySic.GetRePeriodoRepository().GetByCriteria(anioDesde, anioHasta, estado);
        }

        /// <summary>
        /// Permite obtener los periodos del año seleccionado
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<RePeriodoDTO> ObtenerPeriodosPorAnio(int anio)
        {
            return this.GetByCriteriaRePeriodos(anio, anio, ConstantesAppServicio.Activo);
        }

        /// <summary>
        /// Permite editar las etapas del periodo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[][] ObtenerEtapasPorPeriodo(int id)
        {
            List<ReMaestroEtapaDTO> entitys = FactorySic.GetReMaestroEtapaRepository().GetByCriteria(id);

            string[][] arreglo = new string[entitys.Count][];

            int index = 0;
            foreach (ReMaestroEtapaDTO item in entitys)
            {
                arreglo[index] = new string[5];
                arreglo[index][0] = item.Reetacodi.ToString();
                arreglo[index][1] = item.Reetanombre;
                arreglo[index][2] = (item.FechaFinal != null) ? ((DateTime)item.FechaFinal).ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                arreglo[index][3] = item.Reetaregistro;

                if (item.FechaFinal != null)
                {
                    DateTime fecha = ((DateTime)item.FechaFinal);
                    string estado = ConstantesCalculoResarcimiento.TextoEstadoCulminado;
                    if ((int)fecha.Subtract(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).TotalDays >= 0)
                        estado = ConstantesCalculoResarcimiento.TextoEstadoProceso;
                    arreglo[index][4] = estado;
                }

                index++;
            }

            return arreglo;
        }


        /// <summary>
        /// Permite obtener los periodos padre
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<RePeriodoDTO> ObtenerPeriodosPadre(int anio)
        {
            return FactorySic.GetRePeriodoRepository().ObtenerPeriodosPadre(anio);
        }

        /// <summary>
        /// Permite obtener los periodos semestrales
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<RePeriodoDTO> ObtenerPeriodosSemestrales(int anio)
        {
            return this.ObtenerPeriodosPorAnio(anio).Where(x => x.Repertipo == ConstantesCalculoResarcimiento.PeriodoSemestral).ToList();
        }

        /// <summary>
        /// Otiene periodos que no son de revision
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<RePeriodoDTO> ObtenerPeriodosSemestralesSinRevision(int anio)
        {
            return this.ObtenerPeriodosPorAnio(anio).Where(x => x.Repertipo == ConstantesCalculoResarcimiento.PeriodoSemestral &&
            x.Reperrevision != ConstantesAppServicio.SI).ToList();
        }


        #endregion

        #region Puntos Entrega

        /// <summary>
        /// Guarda los datos de punto de entrega
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="niveltension"></param>
        /// <param name="estado"></param>
        /// <param name="usuario"></param>
        /// <param name="accion"></param>
        /// <param name="repentcodi"></param>
        public void GuardarDatosPuntoEntrega(string nombre, int niveltension, string estado, string usuario, int accion, int? repentcodi)
        {
            RePuntoEntregaDTO pe = new RePuntoEntregaDTO();

            //validamos duplicados
            if (accion == ConstantesCalculoResarcimiento.AccionNuevo)
            {
                List<RePuntoEntregaDTO> lstPuntoEntrega = ListarPuntosDeEntrega();
                RePuntoEntregaDTO objRepetido = lstPuntoEntrega.Find(x => x.Repentnombre.ToUpper().Trim() == nombre.ToUpper().Trim());
                if (objRepetido != null)
                {
                    throw new Exception("Ya existe un punto de entrega registrado con el mismo nombre.");
                }

            }
            else
            {
                if (accion == ConstantesCalculoResarcimiento.AccionEditar)
                {
                    RePuntoEntregaDTO objEditado = GetByIdRePuntoEntrega(repentcodi.Value);
                    List<RePuntoEntregaDTO> lstPuntoEntrega = ListarPuntosDeEntrega();
                    List<RePuntoEntregaDTO> lstPuntoEntregaSinEditado = lstPuntoEntrega.Where(x => x.Repentcodi != objEditado.Repentcodi).ToList();
                    RePuntoEntregaDTO objRepetido = lstPuntoEntregaSinEditado.Find(x => x.Repentnombre.ToUpper().Trim() == nombre.ToUpper().Trim() && x.Rentcodi == niveltension);
                    if (objRepetido != null)
                    {
                        throw new Exception("Ya existe un punto de entrega registrado con el mismo nombre y nivel de tensión.");
                    }
                }
            }

            //preparamos el punto de entrega a guardar
            pe.Repentnombre = nombre;
            pe.Rentcodi = niveltension;
            pe.Repentestado = estado;

            if (accion == ConstantesCalculoResarcimiento.AccionNuevo)
            {
                pe.Repentusucreacion = usuario;
                pe.Repentfeccreacion = DateTime.Now;
            }
            else
            {
                pe.Repentcodi = repentcodi.Value;
            }

            pe.Repentusumodificacion = usuario;
            pe.Repentfecmodificacion = DateTime.Now;

            if (accion == ConstantesCalculoResarcimiento.AccionNuevo)
                SaveRePuntoEntrega(pe);
            else
                UpdateRePuntoEntrega(pe);

        }

        /// <summary>
        /// Lista los puntos de entrega
        /// </summary>
        /// <returns></returns>
        public List<RePuntoEntregaDTO> ListarPuntosDeEntrega()
        {
            List<RePuntoEntregaDTO> lstPE = ListRePuntoEntregas();
            List<ReNivelTensionDTO> lstNT = ListReNivelTensions();

            foreach (var puntoEntrega in lstPE)
            {
                puntoEntrega.Rentabrev = lstNT.Find(x => x.Rentcodi == puntoEntrega.Rentcodi).Rentabrev;
                puntoEntrega.UsuarioCreaciónModificacion = puntoEntrega.Repentusumodificacion != "" ? puntoEntrega.Repentusumodificacion : puntoEntrega.Repentusucreacion;
                puntoEntrega.FechaCreaciónModificacionDesc = puntoEntrega.Repentfecmodificacion != null ? puntoEntrega.Repentfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : puntoEntrega.Repentfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                puntoEntrega.EstadoDesc = puntoEntrega.Repentestado == "A" ? "Activo" : (puntoEntrega.Repentestado == "B" ? "Baja" : "");
            }

            return lstPE.OrderBy(x => x.Repentnombre).ToList();
        }

        /// <summary>
        /// Genera el archivo a exportar con el listado de puntos de entrega
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="nameFile"></param>
        public void GenerarExportacionPE(string ruta, string pathLogo, string nameFile)
        {
            List<RePuntoEntregaDTO> listaPETotales = ListarPuntosDeEntrega();

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
                GenerarArchivoExcelPE(xlPackage, pathLogo, listaPETotales);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera la estructura de la tabla a exportar
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="listaPETotales"></param>
        private void GenerarArchivoExcelPE(ExcelPackage xlPackage, string pathLogo, List<RePuntoEntregaDTO> listaPETotales)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            string nameWS = "REPORTE";
            string titulo = "MAESTRO PUNTOS DE ENTREGA Y NIVEL DE TENSIÓN";

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

            int colPE = colIniTable;
            int colNT = colIniTable + 1;
            int colEstado = colIniTable + 2;
            int colUsuario = colIniTable + 3;
            int colFecha = colIniTable + 4;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFecha, "Calibri", 16);

            ws.Row(rowIniTabla).Height = 25;
            ws.Cells[rowIniTabla, colPE].Value = "Punto de Entrega";
            ws.Cells[rowIniTabla, colNT].Value = "Nivel de Tensión";
            ws.Cells[rowIniTabla, colEstado].Value = "Estado";
            ws.Cells[rowIniTabla, colUsuario].Value = "Usuario Creación / Modificación";
            ws.Cells[rowIniTabla, colFecha].Value = "Fecha de Creación / Modificación";

            //Estilos cabecera
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colPE, rowIniTabla, colFecha, "Calibri", 11);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colPE, rowIniTabla, colFecha, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colPE, rowIniTabla, colFecha, "Centro");
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colPE, rowIniTabla, colFecha, "#2980B9");
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colPE, rowIniTabla, colFecha, "#FFFFFF");
            servFormato.BorderCeldas2(ws, rowIniTabla, colPE, rowIniTabla, colFecha);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colPE, rowIniTabla, colFecha);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;
            string fondoEnBaja = "#FFD6D6";

            foreach (var item in listaPETotales)
            {
                ws.Cells[rowData, colPE].Value = item.Repentnombre;
                ws.Cells[rowData, colNT].Value = item.Rentabrev.ToString().Trim();
                ws.Cells[rowData, colEstado].Value = item.EstadoDesc.Trim();
                ws.Cells[rowData, colUsuario].Value = item.UsuarioCreaciónModificacion.ToString().Trim();
                ws.Cells[rowData, colFecha].Value = item.FechaCreaciónModificacionDesc.Trim();

                if (item.Repentestado == "B")
                    servFormato.CeldasExcelColorFondo(ws, rowData, colPE, rowData, colFecha, fondoEnBaja);

                rowData++;
            }

            if (!listaPETotales.Any()) rowData++;

            //Estilos registros
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colPE, rowData - 1, colFecha, "Calibri", 8);
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colPE, rowData - 1, colFecha, "Centro");
            servFormato.BorderCeldas2(ws, rowIniTabla + 1, colPE, rowData - 1, colFecha);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colPE, rowData - 1, colFecha, "Centro");

            #endregion

            //filter           
            ws.Cells[rowIniTabla, colPE, rowData, colFecha].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        /// <summary>
        /// Elimina un registro de punto de entrega
        /// </summary>
        /// <param name="repentcodi"></param>
        /// <param name="usuario"></param>
        public void EliminarPuntoEntrega(int repentcodi, string usuario)
        {
            try
            {
                RePuntoEntregaDTO pe = GetByIdRePuntoEntrega(repentcodi);
                pe.Repentestado = "E";
                pe.Repentusumodificacion = usuario;
                pe.Repentfecmodificacion = DateTime.Now;

                UpdateRePuntoEntrega(pe);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }



        #endregion

        #region Insumos por Periodo

        /// <summary>
        /// Permite obtener el listado de puntos de entrega por periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<RePtoentregaPeriodoDTO> ObtenerPtoEntregaPorPeriodo(int periodo)
        {
            return FactorySic.GetRePtoentregaPeriodoRepository().GetByCriteria(periodo);
        }

        /// <summary>
        /// Elimina un registro de la tabla RE_PTOENTREGA_PERIODO
        /// </summary>
        public int DeleteRePtoentregaPeriodo(int reptopcodi, int periodo)
        {
            try
            {
                FactorySic.GetRePtoentregaPeriodoRepository().Delete(reptopcodi, periodo);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite agregar el punto de entrega al periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="ptoEntregaCodi"></param>
        /// <returns></returns>
        public int GrabarPtoEntregaPeriodo(int periodo, int ptoEntregaCodi, string username)
        {
            try
            {
                int result = 1;
                int validacion = FactorySic.GetRePtoentregaPeriodoRepository().ObtenerPorPtoEntrega(ptoEntregaCodi, periodo);

                if (validacion == 0)
                {
                    RePtoentregaPeriodoDTO entity = new RePtoentregaPeriodoDTO();
                    entity.Repentcodi = ptoEntregaCodi;
                    entity.Repercodi = periodo;
                    entity.Reptopusucreacion = username;
                    entity.Reptopusumodificacion = username;
                    entity.Reptopfeccreacion = DateTime.Now;
                    entity.Reptopfecmodificacion = DateTime.Now;

                    FactorySic.GetRePtoentregaPeriodoRepository().Save(entity);
                }
                else
                {
                    result = 2;
                }


                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite almacenar los puntos de entrega
        /// </summary>
        /// <param name="entitys"></param>
        public void CargarMasivoPtoEntrega(List<RePtoentregaPeriodoDTO> entitys, int periodo, string username, out List<string> validaciones)
        {
            List<RePuntoEntregaDTO> maestroPE = this.ListarPuntosDeEntrega();
            List<string> errores = new List<string>();
            bool flag = true;
            List<RePtoentregaPeriodoDTO> result = new List<RePtoentregaPeriodoDTO>();

            foreach (RePtoentregaPeriodoDTO entity in entitys)
            {
                RePuntoEntregaDTO maestro = maestroPE.Where(x => x.Repentnombre == entity.Repentnombre).FirstOrDefault();

                if (maestro != null)
                {
                    if (maestro.Repentestado == ConstantesAppServicio.Baja)
                    {
                        errores.Add("El punto de entrega " + entity.Repentnombre + " se encuentra en estado BAJA.");
                        flag = false;
                    }
                    else
                    {
                        int validacion = FactorySic.GetRePtoentregaPeriodoRepository().ObtenerPorPtoEntrega(maestro.Repentcodi, periodo);

                        if (validacion == 0)
                        {
                            RePtoentregaPeriodoDTO item = new RePtoentregaPeriodoDTO();
                            item.Repentcodi = maestro.Repentcodi;
                            item.Repercodi = periodo;
                            item.Reptopusucreacion = username;
                            item.Reptopusumodificacion = username;
                            item.Reptopfeccreacion = DateTime.Now;
                            item.Reptopfecmodificacion = DateTime.Now;
                            result.Add(item);
                        }
                        else
                        {
                            errores.Add("El punto de entrega " + entity.Repentnombre + " ya se encuentra registrado.");
                            flag = false;
                        }
                    }
                }
                else
                {
                    errores.Add("El punto de entrega " + entity.Repentnombre + " no se encuentra en el maestro de PE.");
                    flag = false;
                }
            }

            if (flag)
            {
                foreach (RePtoentregaPeriodoDTO item in result)
                {
                    FactorySic.GetRePtoentregaPeriodoRepository().Save(item);
                }
            }

            validaciones = errores;
        }

        /// <summary>
        /// Permite obtener la estructura de la tabla de indicadores
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public string[][] ObtenerEstructuraIndicadores(int periodo, out int[] rowspans)
        {
            List<ReCausaInterrupcionDTO> entitys = FactorySic.GetReCausaInterrupcionRepository().GetByCriteria(periodo);
            List<string[]> result = new List<string[]>();
            List<int> idsTipo = entitys.Select(x => (int)x.Retintcodi).Distinct().ToList();
            rowspans = new int[idsTipo.Count];
            int count = 0;
            foreach (int tipo in idsTipo)
            {
                List<ReCausaInterrupcionDTO> items = entitys.Where(x => x.Retintcodi == tipo).ToList();
                int rowspan = items.Count;
                rowspans[count] = rowspan;
                int index = 0;
                foreach (ReCausaInterrupcionDTO item in items)
                {
                    string[] itemData = new string[5];
                    itemData[0] = item.Recintcodi.ToString();
                    itemData[1] = (index == 0) ? item.Retintnombre : string.Empty;
                    itemData[2] = item.Recintnombre;
                    itemData[3] = item.Reindni.ToString();
                    itemData[4] = item.Reindki.ToString();
                    result.Add(itemData);
                    index++;
                }
                count++;
            }
            return result.ToArray();
        }

        /// <summary>
        /// Permite grabar los indicadores por periodo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarIndicadoresPorPeriodo(string[][] data, int periodo, string username)
        {
            try
            {
                FactorySic.GetReIndicadorPeriodoRepository().Delete(periodo);

                foreach (string[] item in data)
                {
                    ReIndicadorPeriodoDTO entity = new ReIndicadorPeriodoDTO();
                    entity.Repercodi = periodo;
                    entity.Reindusucreacion = username;
                    entity.Reindusumodificacion = username;
                    entity.Reindfeccreacion = DateTime.Now;
                    entity.Reindfecmodificacion = DateTime.Now;
                    entity.Recintcodi = int.Parse(item[0]);
                    entity.Reindni = decimal.Parse(item[3]);
                    entity.Reindki = decimal.Parse(item[4]);
                    FactorySic.GetReIndicadorPeriodoRepository().Save(entity);
                }

                return 1;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener el listado de tolerancia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public string[][] ObtenerEstructuraTolerancia(int periodo)
        {
            List<ReToleranciaPeriodoDTO> entitys = FactorySic.GetReToleranciaPeriodoRepository().GetByCriteria(periodo);

            List<string[]> result = new List<string[]>();

            foreach (ReToleranciaPeriodoDTO entity in entitys)
            {
                string[] data = new string[6];
                data[0] = entity.Rentcodi.ToString();
                data[1] = entity.Rentabrev;
                data[2] = entity.Retolninf.ToString();
                data[3] = entity.Retoldinf.ToString();
                data[4] = entity.Retolnsup.ToString();
                data[5] = entity.Retoldsup.ToString();
                result.Add(data);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Permite grabar los indicadores por periodo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarToleranciaPorPeriodo(string[][] data, int periodo, string username)
        {
            try
            {
                FactorySic.GetReToleranciaPeriodoRepository().Delete(periodo);

                for (int i = 2; i < data.Length; i++)
                {
                    ReToleranciaPeriodoDTO entity = new ReToleranciaPeriodoDTO();
                    entity.Repercodi = periodo;
                    entity.Retolusucreacion = username;
                    entity.Retolusumodificacion = username;
                    entity.Retolfeccreacion = DateTime.Now;
                    entity.Retolfecmodificacion = DateTime.Now;
                    entity.Rentcodi = int.Parse(data[i][0]);
                    entity.Retolninf = int.Parse(data[i][2]);
                    entity.Retoldinf = int.Parse(data[i][3]);
                    entity.Retolnsup = int.Parse(data[i][4]);
                    entity.Retoldsup = int.Parse(data[i][5]);
                    FactorySic.GetReToleranciaPeriodoRepository().Save(entity);
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Permite obtener el listado de ingresos por transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<ReIngresoTransmisionDTO> ListIngresosPorTransmision(int periodo)
        {
            List<ReIngresoTransmisionDTO> entitys = FactorySic.GetReIngresoTransmisionRepository().GetByCriteria(periodo);

            foreach (ReIngresoTransmisionDTO entity in entitys)
            {
                if (!string.IsNullOrEmpty(entity.Reingsustento))
                {
                    String fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoIngreso, entity.Reingcodi, entity.Reingsustento);

                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileName,
                      ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                    {
                        entity.TieneArchivo = true;
                    }
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener los ingreso de transmision
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReIngresoTransmisionDTO ObtenerIngresoPorId(int id)
        {
            return FactorySic.GetReIngresoTransmisionRepository().GetById(id);
        }

        /// <summary>
        /// Permite obtener las empresas de equipamiento
        /// </summary>
        /// <returns></returns>
        public List<ReEmpresaDTO> ObtenerEmpresas()
        {
            return FactorySic.GetReIngresoTransmisionRepository().ObtenerEmpresas();
        }

        /// <summary>
        /// Permite grabar el ingreso de transmision
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="empresa"></param>
        /// <param name="moneda"></param>
        /// <param name="ingreso"></param>
        /// <param name="periodo"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarIngreso(int codigo, int empresa, string moneda, decimal ingreso, int periodo, string username)
        {
            try
            {
                ReIngresoTransmisionDTO entity = new ReIngresoTransmisionDTO();
                entity.Reingcodi = codigo;
                entity.Emprcodi = empresa;
                entity.Reingmoneda = moneda;
                entity.Reingvalor = ingreso;
                entity.Repercodi = periodo;
                entity.Reingfecmodificacion = DateTime.Now;
                entity.Reingusumodificacion = username;
                entity.Reingfuente = ConstantesCalculoResarcimiento.FuenteIngresoIntranet;

                if (codigo == 0)
                {
                    List<ReIngresoTransmisionDTO> list = FactorySic.GetReIngresoTransmisionRepository().GetByCriteria(periodo);
                    if (list.Where(x => x.Emprcodi == empresa).Count() > 0)
                    {
                        return 2;
                    }


                    entity.Reingfeccreacion = DateTime.Now;
                    entity.Reingusucreacion = username;
                    FactorySic.GetReIngresoTransmisionRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetReIngresoTransmisionRepository().Update(entity);
                }

                ReEnvioDTO envio = new ReEnvioDTO();
                envio.Repercodi = periodo;
                envio.Reenvtipo = ConstantesCalculoResarcimiento.EnvioIngresoTransmision;
                envio.Emprcodi = empresa;
                envio.Reenvfecha = DateTime.Now;
                envio.Reenvestado = ConstantesAppServicio.Activo;
                envio.Reenvusucreacion = username;
                envio.Reenvfeccreacion = DateTime.Now;
                FactorySic.GetReEnvioRepository().Save(envio);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite eliminar el ingreso
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public int EliminarIngreso(int codigo)
        {
            try
            {
                FactorySic.GetReIngresoTransmisionRepository().Delete(codigo);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite exportar los datos de ingresos de transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int ExportarIngresos(int periodo, string path, string filename)
        {
            try
            {
                List<ReIngresoTransmisionDTO> entitys = this.ListIngresosPorTransmision(periodo);
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("INGRESOS POR TRANSMISIÓN");

                    if (ws != null)
                    {
                        int index = 1;

                        ws.Cells[index, 1].Value = "EMPRESA";
                        ws.Cells[index, 2].Value = "MONEDA";
                        ws.Cells[index, 3].Value = "INGRESOS POR TRANSMISIÓN";
                        ws.Cells[index, 4].Value = "USUARIO";
                        ws.Cells[index, 5].Value = "FECHA";

                        ExcelRange rg = ws.Cells[index, 1, index, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 2;
                        foreach (ReIngresoTransmisionDTO item in entitys)
                        {
                            ws.Cells[index, 1].Value = item.Emprnomb;
                            ws.Cells[index, 2].Value = item.Reingmoneda;
                            ws.Cells[index, 3].Value = item.Reingvalor;
                            ws.Cells[index, 4].Value = item.Reingusucreacion;
                            ws.Cells[index, 5].Value = (item.Reingfecmodificacion != null) ?
                                ((DateTime)item.Reingfecmodificacion).ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                            rg = ws.Cells[index, 3, index, 3];
                            rg.Style.Numberformat.Format = "#,##0.00";
                            rg = ws.Cells[index, 1, index, 5];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[2, 1, index - 1, 5];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        rg = ws.Cells[1, 1, index, 5];
                        rg.AutoFitColumns();
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener los ingresos por periodo
        /// </summary>
        /// <returns></returns>
        public EstructuraIngresoTransmision ObtenerPorEmpresaPeriodo(int idEmpresa, int idPeriodo)
        {
            EstructuraIngresoTransmision result = new EstructuraIngresoTransmision();
            result.Existe = -1;
            ReIngresoTransmisionDTO entity = FactorySic.GetReIngresoTransmisionRepository().ObtenerPorEmpresaPeriodo(idEmpresa, idPeriodo);
            if (entity != null) result.Existe = 1;
            result.Entidad = entity;
            result.Plazo = (this.ValidarPlazoEtapa(idPeriodo, 9) == ConstantesAppServicio.SI) ? -1 : 1;

            return result;
        }

        /// <summary>
        /// Permite grabar el archivo de interrupción
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extension"></param>
        public void GrabarArchivoIngreso(int id, string extension)
        {
            FactorySic.GetReIngresoTransmisionRepository().ActualizarArchivo(id, extension);
        }


        /// <summary>
        /// Permite grabar el ingreso de transmision
        /// </summary>
        /// <param name="path"></param>
        /// <param name="codigo"></param>
        /// <param name="empresa"></param>
        /// <param name="moneda"></param>
        /// <param name="ingreso"></param>
        /// <param name="periodo"></param>
        /// <param name="username"></param>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public int GrabarIngresoExtranet(string path, int codigo, int empresa, string moneda, decimal ingreso, int periodo, string archivo, string username)
        {
            try
            {
                ReIngresoTransmisionDTO entity = new ReIngresoTransmisionDTO();
                entity.Reingcodi = codigo;
                entity.Emprcodi = empresa;
                entity.Reingmoneda = moneda;
                entity.Reingvalor = ingreso;
                entity.Repercodi = periodo;
                entity.Reingfecmodificacion = DateTime.Now;
                entity.Reingusumodificacion = username;
                entity.Reingfuente = ConstantesCalculoResarcimiento.FuenteIngresoExtranet;
                entity.Reingsustento = archivo;

                if (codigo == 0)
                {
                    entity.Reingfeccreacion = DateTime.Now;
                    entity.Reingusucreacion = username;
                    int id = FactorySic.GetReIngresoTransmisionRepository().Save(entity);

                    if (!string.IsNullOrEmpty(archivo))
                    {
                        string fileTemporal = string.Format(ConstantesCalculoResarcimiento.TemporalIngreso, archivo);
                        string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoIngreso, id, archivo);
                        /*if (File.Exists(path + fileTemporal))
                        {
                            if (File.Exists(path + fileFinal)) File.Delete(path + fileFinal);

                            File.Move(path + fileTemporal, path + fileFinal);
                        }*/

                        if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                   ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                        {
                            FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                        }
                    }
                }
                else
                {
                    FactorySic.GetReIngresoTransmisionRepository().Update(entity);
                }

                ReEnvioDTO envio = new ReEnvioDTO();
                envio.Repercodi = periodo;
                envio.Reenvtipo = ConstantesCalculoResarcimiento.EnvioIngresoTransmision;
                envio.Emprcodi = empresa;
                envio.Reenvfecha = DateTime.Now;
                envio.Reenvestado = ConstantesAppServicio.Activo;
                envio.Reenvusucreacion = username;
                envio.Reenvfeccreacion = DateTime.Now;
                FactorySic.GetReEnvioRepository().Save(envio);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite exportar los datos de ingresos de transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int ExportarPuntoEntregaPeriodo(int periodo, string path, string filename)
        {
            try
            {
                List<RePtoentregaPeriodoDTO> entitys = this.ObtenerPtoEntregaPorPeriodo(periodo);
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("PUNTOS DE ENTREGA");

                    if (ws != null)
                    {
                        int index = 1;

                        ws.Cells[index, 1].Value = "PUNTO DE ENTREGA";
                        ws.Cells[index, 2].Value = "NIVEL DE TENSION";
                        ws.Cells[index, 3].Value = "USUARIO CREACIÓN";
                        ws.Cells[index, 4].Value = "FECHA CREACIÓN";

                        ExcelRange rg = ws.Cells[index, 1, index, 4];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 2;
                        foreach (RePtoentregaPeriodoDTO item in entitys)
                        {
                            ws.Cells[index, 1].Value = item.Repentnombre;
                            ws.Cells[index, 2].Value = item.Rentabrev;
                            ws.Cells[index, 3].Value = item.Reptopusumodificacion;
                            ws.Cells[index, 4].Value = (item.Reptopfecmodificacion != null) ?
                                ((DateTime)item.Reptopfecmodificacion).ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                            rg = ws.Cells[index, 1, index, 4];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[2, 1, index - 1, 4];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        rg = ws.Cells[1, 1, index, 5];
                        rg.AutoFitColumns();
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }


        /// <summary>
        /// Permite obtener los eventos por periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<ReEventoPeriodoDTO> ObtenerEventosPorPeriodo(int periodo)
        {
            List<ReEventoPeriodoDTO> entitys = FactorySic.GetReEventoPeriodoRepository().GetByCriteria(periodo);

            foreach (ReEventoPeriodoDTO entity in entitys)
            {
                entity.FechaEvento = ((DateTime)entity.Reevefecha).ToString(ConstantesAppServicio.FormatoFecha);
            }

            return entitys;
        }

        /// <summary>
        /// Pemite otener el evento en base al id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReEventoPeriodoDTO ObtenerEvento(int id)
        {
            return FactorySic.GetReEventoPeriodoRepository().GetById(id);
        }

        /// <summary>
        /// Permite grabar los eventos
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarEvento(ReEventoPeriodoDTO entity, string username)
        {
            try
            {
                RePeriodoDTO entityPeriodo = FactorySic.GetRePeriodoRepository().GetById((int)entity.Repercodi);
                DateTime fechaInicio = (DateTime)entityPeriodo.Reperfecinicio;
                DateTime fechaFin = (DateTime)entityPeriodo.Reperfecfin;
                DateTime fecha = (DateTime)entity.Reevefecha;

                if (!(fecha.Subtract(fechaInicio).TotalDays < 0 || fecha.Subtract(fechaFin).TotalDays > 0))
                {
                    if (entity.Reevecodi == 0)
                    {
                        entity.Reeveusucreacion = username;
                        entity.Reeveusumodificacion = username;
                        entity.Reevefeccreacion = DateTime.Now;
                        entity.Reevefecmodificacion = DateTime.Now;
                        FactorySic.GetReEventoPeriodoRepository().Save(entity);
                    }
                    else
                    {
                        entity.Reeveusumodificacion = username;
                        entity.Reevefecmodificacion = DateTime.Now;
                        FactorySic.GetReEventoPeriodoRepository().Update(entity);
                    }

                    return 1;
                }
                else
                {
                    return 2;
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }


        /// <summary>
        /// Permite eliminar eventos
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public int EliminarEvento(int idEvento)
        {
            try
            {
                FactorySic.GetReEventoPeriodoRepository().Delete(idEvento);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite exportar los datos de ingresos de transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int ExportarEventos(int periodo, string path, string filename)
        {
            try
            {
                List<ReEventoPeriodoDTO> entitys = this.ObtenerEventosPorPeriodo(periodo);
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("EVENTOS COES");

                    if (ws != null)
                    {
                        int index = 1;

                        ws.Cells[index, 1].Value = "EVENTO COES";
                        ws.Cells[index, 2].Value = "FECHA";
                        ws.Cells[index, 3].Value = "RESPONSABLE 1";
                        ws.Cells[index, 4].Value = "% RESP. 1";
                        ws.Cells[index, 5].Value = "RESPONSABLE 2";
                        ws.Cells[index, 6].Value = "% RESP. 2";
                        ws.Cells[index, 7].Value = "RESPONSABLE 3";
                        ws.Cells[index, 8].Value = "% RESP. 3";
                        ws.Cells[index, 9].Value = "RESPONSABLE 4";
                        ws.Cells[index, 10].Value = "% RESP. 4";
                        ws.Cells[index, 11].Value = "RESPONSABLE 5";
                        ws.Cells[index, 12].Value = "% RESP. 5";
                        ws.Cells[index, 13].Value = "COMENTARIO";

                        ExcelRange rg = ws.Cells[index, 1, index, 13];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 2;
                        foreach (ReEventoPeriodoDTO item in entitys)
                        {
                            ws.Cells[index, 1].Value = item.Reevedescripcion;
                            ws.Cells[index, 2].Value = (item.Reevefecha != null) ?
                                ((DateTime)item.Reevefecha).ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[index, 3].Value = item.Responsablenomb1;
                            ws.Cells[index, 4].Value = item.Reeveporc1 / 100;
                            ws.Cells[index, 5].Value = item.Responsablenomb2;
                            ws.Cells[index, 6].Value = item.Reeveporc2 / 100;
                            ws.Cells[index, 7].Value = item.Responsablenomb3;
                            ws.Cells[index, 8].Value = item.Reeveporc3 / 100;
                            ws.Cells[index, 9].Value = item.Responsablenomb4;
                            ws.Cells[index, 10].Value = item.Reeveporc4 / 100;
                            ws.Cells[index, 11].Value = item.Responsablenomb5;
                            ws.Cells[index, 12].Value = item.Reeveporc5 / 100;
                            ws.Cells[index, 13].Value = item.Reevecomentario;

                            rg = ws.Cells[index, 4, index, 4];
                            rg.Style.Numberformat.Format = "#0.00%";
                            rg = ws.Cells[index, 6, index, 6];
                            rg.Style.Numberformat.Format = "#0.00%";
                            rg = ws.Cells[index, 8, index, 8];
                            rg.Style.Numberformat.Format = "#0.00%";
                            rg = ws.Cells[index, 10, index, 10];
                            rg.Style.Numberformat.Format = "#0.00%";
                            rg = ws.Cells[index, 12, index, 12];
                            rg.Style.Numberformat.Format = "#0.00%";

                            rg = ws.Cells[index, 1, index, 13];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[2, 1, index - 1, 13];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        rg = ws.Cells[1, 1, index, 13];
                        rg.AutoFitColumns();
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite almacenar los puntos de entrega
        /// </summary>
        /// <param name="entitys"></param>
        public void CargarMasivoEvento(List<ReEventoPeriodoDTO> entitys, int periodo, string username, out List<string> validaciones)
        {
            try
            {
                List<ReEmpresaDTO> empresas = this.ObtenerEmpresas();
                RePeriodoDTO entityPeriodo = FactorySic.GetRePeriodoRepository().GetById(periodo);
                List<string> errores = new List<string>();
                List<ReEventoPeriodoDTO> result = new List<ReEventoPeriodoDTO>();

                int index = 2;
                foreach (ReEventoPeriodoDTO entity in entitys)
                {
                    bool flag = true;
                    DateTime fecha;
                    decimal porcentaje1 = 0;
                    decimal? porcentaje2 = null;
                    decimal? porcentaje3 = null;
                    decimal? porcentaje4 = null;
                    decimal? porcentaje5 = null;
                    int? emprcodi2 = null;
                    int? emprcodi3 = null;
                    int? emprcodi4 = null;
                    int? emprcodi5 = null;
                    ReEmpresaDTO entityEmpresa = null;

                    if (string.IsNullOrEmpty(entity.Reevedescripcion) ||
                       string.IsNullOrEmpty(entity.FechaEvento) ||
                       string.IsNullOrEmpty(entity.Responsablenomb1) ||
                       string.IsNullOrEmpty(entity.Porcentaje1) ||
                       string.IsNullOrEmpty(entity.Reevecomentario))
                    {
                        errores.Add("En la fila " + index + " debe ingresar la descripción, la fecha, el responsable 1, el porcentaje 1 y el comentario.");
                        flag = false;
                    }
                    else
                    {
                        entity.FechaEvento = entity.FechaEvento.Replace("00:00:00", "");
                        entity.FechaEvento = entity.FechaEvento.Trim();

                        if (!DateTime.TryParseExact(entity.FechaEvento, "dd/MM/yyyy",
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None,
                                                   out fecha))
                        {
                            errores.Add("La fecha de la fila " + index + " no tiene el formato de dd/mm/yyyy.");
                            flag = false;
                        }
                        else
                        {
                            DateTime fechaInicio = (DateTime)entityPeriodo.Reperfecinicio;
                            DateTime fechaFin = (DateTime)entityPeriodo.Reperfecfin;

                            if (fecha.Subtract(fechaInicio).TotalDays < 0 || fecha.Subtract(fechaFin).TotalDays > 0)
                            {
                                errores.Add("La fecha de la fila " + index + " debe estar dentro del periodo seleccionado.");
                                flag = false;
                            }
                            //- Validamos que la fecha se encuentre dentro del periodo
                        }

                        entityEmpresa = empresas.Where(x => x.Emprnomb == entity.Responsablenomb1).FirstOrDefault();
                        if (entityEmpresa == null)
                        {
                            errores.Add("El responsable 1 de la fila " + index + " no existe en la base de datos.");
                            flag = false;
                        }

                        if (!decimal.TryParse(entity.Porcentaje1, out porcentaje1))
                        {
                            errores.Add("El porcentaje del responsable 1 de la fila " + index + " no tiene formato numérico.");
                            flag = false;
                        }

                        bool flagEmpresa2 = true;
                        bool flagEmpresa3 = true;
                        bool flagEmpresa4 = true;
                        bool flagEmpresa5 = true;

                        errores.AddRange(UtilCalculoResarcimiento.ValidarEvento(empresas, index, entity.Responsablenomb2, entity.Porcentaje2,
                            out flagEmpresa2, out porcentaje2, out emprcodi2, 2));
                        errores.AddRange(UtilCalculoResarcimiento.ValidarEvento(empresas, index, entity.Responsablenomb3, entity.Porcentaje3,
                            out flagEmpresa3, out porcentaje3, out emprcodi3, 3));
                        errores.AddRange(UtilCalculoResarcimiento.ValidarEvento(empresas, index, entity.Responsablenomb4, entity.Porcentaje4,
                            out flagEmpresa4, out porcentaje4, out emprcodi4, 4));
                        errores.AddRange(UtilCalculoResarcimiento.ValidarEvento(empresas, index, entity.Responsablenomb5, entity.Porcentaje5,
                            out flagEmpresa5, out porcentaje5, out emprcodi5, 5));

                        if (!flagEmpresa2) flag = false;
                        if (!flagEmpresa3) flag = false;
                        if (!flagEmpresa4) flag = false;
                        if (!flagEmpresa5) flag = false;
                    }
                    decimal por2 = (porcentaje2 != null) ? (decimal)porcentaje2 : 0;
                    decimal por3 = (porcentaje3 != null) ? (decimal)porcentaje3 : 0;
                    decimal por4 = (porcentaje4 != null) ? (decimal)porcentaje4 : 0;
                    decimal por5 = (porcentaje5 != null) ? (decimal)porcentaje5 : 0;

                    if (porcentaje1 + por2 + por3 + por4 + por5 != 100)
                    {
                        errores.Add("La suma de porcentajes de la fila " + index + " debe ser 100%");
                        flag = false;
                    }

                    if (flag)
                    {
                        ReEventoPeriodoDTO itemEvento = new ReEventoPeriodoDTO();
                        itemEvento.Reevedescripcion = entity.Reevedescripcion;
                        itemEvento.Repercodi = periodo;
                        itemEvento.Reeveestado = ConstantesAppServicio.Activo;
                        itemEvento.Reevefecha = DateTime.ParseExact(entity.FechaEvento, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        itemEvento.Reeveempr1 = entityEmpresa.Emprcodi;
                        itemEvento.Reeveporc1 = porcentaje1;
                        itemEvento.Reeveempr2 = emprcodi2;
                        itemEvento.Reeveporc2 = porcentaje2;
                        itemEvento.Reeveempr3 = emprcodi3;
                        itemEvento.Reeveporc3 = porcentaje3;
                        itemEvento.Reeveempr4 = emprcodi4;
                        itemEvento.Reeveporc4 = porcentaje4;
                        itemEvento.Reeveempr5 = emprcodi5;
                        itemEvento.Reeveporc5 = porcentaje5;
                        itemEvento.Reevecomentario = entity.Reevecomentario;
                        itemEvento.Reeveusucreacion = username;
                        itemEvento.Reeveusumodificacion = username;
                        itemEvento.Reevefeccreacion = DateTime.Now;
                        itemEvento.Reevefecmodificacion = DateTime.Now;
                        result.Add(itemEvento);
                    }

                    index++;
                }

                validaciones = errores;

                if (validaciones.Count == 0)
                {
                    foreach (ReEventoPeriodoDTO eventoPeriodo in result)
                    {
                        FactorySic.GetReEventoPeriodoRepository().Save(eventoPeriodo);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                validaciones = new List<string>();
            }
        }

        #endregion

        #region Reportes

        #region Reportes en archivo Excel

        #region General

        /// <summary>
        /// Devuelve formato numerico en celdas excel
        /// </summary>
        /// <param name="numDecimales"></param>
        /// <returns></returns>
        public string FormatoNumDecimales(int numDecimales)
        {
            string salida = "";
            switch (numDecimales)
            {
                case 1: salida = "#,#0.0"; break;
                case 2: salida = "#,##0.00"; break;
                case 3: salida = "#,###0.000"; break;
                case 4: salida = "#,####0.0000"; break;
                case 5: salida = "#,#####0.00000"; break;
                case 6: salida = "#,######0.000000"; break;
                case 7: salida = "#,#######0.0000000"; break;
                case 8: salida = "#,########0.00000000"; break;
                default:
                    break;
            }
            return salida;
        }

        /// <summary>
        /// Devuelve formato numerico porcentaje en celdas excel
        /// </summary>
        /// <param name="numDecimales"></param>
        /// <returns></returns>
        public string FormatoNumDecimalesPorcentaje(int numDecimales)
        {
            string salida = "";
            switch (numDecimales)
            {
                case 1: salida = "#,#0.0%"; break;
                case 2: salida = "#,##0.00%"; break;
                case 3: salida = "#,###0.000%"; break;
                case 4: salida = "#,####0.0000%"; break;
                case 5: salida = "#,#####0.00000%"; break;
                case 6: salida = "#,######0.000000%"; break;
                case 7: salida = "#,#######0.0000000%"; break;
                case 8: salida = "#,########0.00000000%"; break;
                default:
                    break;
            }
            return salida;
        }

        /// <summary>
        /// Devuelve el nombre de cierto reporte excel
        /// </summary>
        /// <param name="codigoReporte"></param>
        /// <returns></returns>
        public string ObtenerNombreReporteEnExcel(int codigoReporte)
        {
            string salida = "";
            switch (codigoReporte)
            {
                case ConstantesCalculoResarcimiento.ReporteExcelConsolidadoInterrupcionesSuministro:
                    salida = "Reporte Consolidado de Interrupciones.xlsx";
                    break;
                case ConstantesCalculoResarcimiento.ReporteExcelCumplimientoEnvíoInterrupciónSuministros:
                    salida = "Cumplimiento de envío de interrupción de suministros.xlsx";
                    break;
                case ConstantesCalculoResarcimiento.ReporteExcelCumplimientoEnvíoObservaciones:
                    salida = "Cumplimiento de envío de observaciones.xlsx";
                    break;
                case ConstantesCalculoResarcimiento.ReporteExcelCumplimientoEnvíoRespuestasObservaciones:
                    salida = "Cumplimiento de envío de respuesta a observaciones.xlsx";
                    break;
                case ConstantesCalculoResarcimiento.ReporteExcelInterrupcionesFuerzaMayor:
                    salida = "Interrupciones en Fuerza Mayor.xlsx";
                    break;
                case ConstantesCalculoResarcimiento.ReporteExcelContrasteInterrupcionesSuministro:
                    salida = "Contraste de interrupciones de suministro.xlsx";
                    break;
                case ConstantesCalculoResarcimiento.ReporteExcelInterrupcionesEnControversia:
                    salida = "Interrupciones en controversia.xlsx";
                    break;
                case ConstantesCalculoResarcimiento.ReporteExcelComparativoSemestralTrimestral:
                    salida = "Comparativo semestral y trimestral.xlsx";
                    break;
            }
            return salida;
        }

        /// <summary>
        /// Genera el reporte excel de cierto codigo
        /// </summary>
        /// <param name="codigoReporte"></param>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="nameFile"></param>
        public void GenerarReporteEnExcel(int periodoId, int periodoTrimestral, int codigoReporte, string ruta, string pathLogo, string nameFile)
        {
            RePeriodoDTO periodo = GetByIdRePeriodo(periodoId);
            RePeriodoDTO periodoTrim = GetByIdRePeriodo(periodoTrimestral);


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
                switch (codigoReporte)
                {
                    case ConstantesCalculoResarcimiento.ReporteExcelConsolidadoInterrupcionesSuministro:

                        //Para pestaña EventosRC. Obtengo el listado de eventos para cierto periodo
                        int idPeriodoUsar = this.ObtenerIdPeriodoPadre(periodoId);
                        RePeriodoDTO periodoUsar = GetByIdRePeriodo(idPeriodoUsar);
                        List<EventoRC> listaEventosRC = ObtenerListaEventosPorPeriodo(periodoUsar);

                        //Para pestaña MT
                        //Obtengo la parte necesaria del listado de la pestaña PE (interrupciones por punto de entrega)
                        List<InterrupcionSuministroPE> listaRegistrosPE_F = ObtenerListaInterrupcionesPEPorPeriodo(periodo, false, null);
                        //Obtengo la parte necesaria del listado de la pestaña RC (interrupciones por rechazo de carga)
                        List<InterrupcionSuministroRC> listaRegistrosRC_F = ObtenerListaInterrupcionesRCPorPeriodo(periodo, false, null);

                        //Para pestaña LIMTX
                        List<LimiteIngreso> lista_Limtx = ObtenerListaLimiteTransmisionPorPeriodo(periodo, listaRegistrosPE_F, listaRegistrosRC_F, listaEventosRC);

                        //Para pestaña PE_FINAL
                        List<InterrupcionSuministroPE> listaRegistrosPE_Final = ObtenerListaInterrupcionesPEPorPeriodo(periodo, true, lista_Limtx);

                        //Para pestaña RC_FINAL
                        List<InterrupcionSuministroRC> listaRegistrosRC_Final = ObtenerListaInterrupcionesRCPorPeriodo(periodo, true, lista_Limtx);

                        GenerarArchivoExcelConsolidadoInterrupcionesSuministro_PE(xlPackage, pathLogo, listaRegistrosPE_Final);
                        GenerarArchivoExcelConsolidadoInterrupcionesSuministro_RC(xlPackage, pathLogo, periodo, listaRegistrosRC_Final);
                        GenerarArchivoExcelConsolidadoInterrupcionesSuministro_EventoRC(xlPackage, pathLogo, periodo, listaEventosRC);
                        //GenerarArchivoExcelConsolidadoInterrupcionesSuministro_MT(xlPackage, pathLogo, periodo, listaRegistrosPE_F, listaRegistrosRC_F, listaEventosRC, out List<MontoTotalResarcimiento> lista);
                        GenerarArchivoExcelConsolidadoInterrupcionesSuministro_MT(xlPackage, pathLogo, periodo, listaRegistrosPE_Final, listaRegistrosRC_Final, listaEventosRC, out List<MontoTotalResarcimiento> lista);
                        GenerarArchivoExcelConsolidadoInterrupcionesSuministro_Limtx(xlPackage, pathLogo, periodo, listaRegistrosPE_F, listaRegistrosRC_F, listaEventosRC);
                        break;
                    case ConstantesCalculoResarcimiento.ReporteExcelCumplimientoEnvíoInterrupciónSuministros:
                        GenerarArchivoExcelCumplimientoEnvíoInterrupciónSuministros(xlPackage, pathLogo, periodo);
                        break;
                    case ConstantesCalculoResarcimiento.ReporteExcelCumplimientoEnvíoObservaciones:
                        GenerarArchivoExcelCumplimientoEnvíoObservaciones(xlPackage, pathLogo, periodo);
                        break;
                    case ConstantesCalculoResarcimiento.ReporteExcelCumplimientoEnvíoRespuestasObservaciones:
                        GenerarArchivoExcelCumplimientoEnvíoRespuestaAObservaciones(xlPackage, pathLogo, periodo);
                        break;
                    case ConstantesCalculoResarcimiento.ReporteExcelInterrupcionesFuerzaMayor:
                        GenerarArchivoExcelInterrupcionesEnFuerzaMayor(xlPackage, pathLogo, periodo);
                        break;
                    case ConstantesCalculoResarcimiento.ReporteExcelContrasteInterrupcionesSuministro:
                        GenerarArchivoExcelContrasteInterrupcionesSuministro(xlPackage, pathLogo, periodo, null);
                        break;
                    case ConstantesCalculoResarcimiento.ReporteExcelInterrupcionesEnControversia:
                        GenerarArchivoExcelInterrupcionesEnControversia(xlPackage, pathLogo, periodo);
                        break;
                    case ConstantesCalculoResarcimiento.ReporteExcelComparativoSemestralTrimestral:

                        //Obtenemos las interrupciones para cada uno de ellos
                        List<InterrupcionSuministroPE> listaRegistrosPE_Semestral = ObtenerListaInterrupcionesPEPorPeriodo(periodo, false, null);
                        List<InterrupcionSuministroPE> listaRegistrosPE_Trimestral = ObtenerListaInterrupcionesPEPorPeriodo(periodoTrim, false, null);

                        GenerarArchivoExcelComparativoSemestralTrimestral(xlPackage, pathLogo, periodo, periodoTrim, listaRegistrosPE_Semestral, listaRegistrosPE_Trimestral, ConstantesCalculoResarcimiento.ComparativoSemestralRespectoTrimestral);
                        GenerarArchivoExcelComparativoSemestralTrimestral(xlPackage, pathLogo, periodo, periodoTrim, listaRegistrosPE_Semestral, listaRegistrosPE_Trimestral, ConstantesCalculoResarcimiento.ComparativoTrimestralRespectoSemestral);
                        break;
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Devuelve el listado total de suministradores
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerListadoGeneralSuministradores()
        {
            List<SiEmpresaDTO> lstSalida = new List<SiEmpresaDTO>();

            GeneralAppServicio appGeneral = new GeneralAppServicio();
            List<SiEmpresaDTO> empresasGeneradorasActivas = appGeneral.ListadoComboEmpresasPorTipo(ConstantesCalculoResarcimiento.TipoEmpresasGeneradoras).Where(t => t.Emprestado.Trim() == "A").ToList();

            lstSalida.AddRange(empresasGeneradorasActivas);

            return lstSalida.OrderBy(x => x.Emprnomb).ToList();
        }
        #endregion

        #region reporte Interrupciones Pendientes reportar


        #endregion

        #region Reporte Consolidado de Interrupciones de Suministro

        /// <summary>
        /// Genera el reporte de la pestaña PE
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        public void GenerarArchivoExcelConsolidadoInterrupcionesSuministro_PE(ExcelPackage xlPackage, string pathLogo, List<InterrupcionSuministroPE> listaRegistrosPE_Final)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroPE> listaRegistrosPE = new List<InterrupcionSuministroPE>();
            listaRegistrosPE = listaRegistrosPE_Final;
            //listaRegistrosPE = new List<InterrupcionSuministroPE>();  //para hacer evidencias de listado sin registros

            string nameWS = "PE";
            string titulo = "";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 2;

            int colIniTable = colIniTitulo - 1;
            int rowIniTabla = rowIniTitulo + 2;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;

            int colSuministrador = colIniTable;
            int colCorrelativo = colIniTable + 1;
            int colTipoCliente = colIniTable + 2;
            int colNombreCliente = colIniTable + 3;
            int colPuntoEntregaBarraNombre = colIniTable + 4;
            int colNumSuministroClienteLibre = colIniTable + 5;
            int colNivelTensionNombre = colIniTable + 6;
            int colAplicacionLiteral = colIniTable + 7;
            int colEnergiaPeriodo = colIniTable + 8;
            int colIncrementoTolerancia = colIniTable + 9;
            int colTipoNombre = colIniTable + 10;
            int colCausaNombre = colIniTable + 11;
            int colNi = colIniTable + 12;
            int colKi = colIniTable + 13;
            int colTiempoEjecutadoIni = colIniTable + 14;
            int colTiempoEjecutadoFin = colIniTable + 15;
            int colTiempoProgramadoIni = colIniTable + 16;
            int colTiempoProgramadoFin = colIniTable + 17;
            int colResponsable1Nombre = colIniTable + 18;
            int colResponsable1Porcentaje = colIniTable + 19;
            int colResponsable1DispFinal = colIniTable + 20;
            int colResponsable1CompCero = colIniTable + 21;
            int colResponsable2Nombre = colIniTable + 22;
            int colResponsable2Porcentaje = colIniTable + 23;
            int colResponsable2DispFinal = colIniTable + 24;
            int colResponsable2CompCero = colIniTable + 25;
            int colResponsable3Nombre = colIniTable + 26;
            int colResponsable3Porcentaje = colIniTable + 27;
            int colResponsable3DispFinal = colIniTable + 28;
            int colResponsable3CompCero = colIniTable + 29;
            int colResponsable4Nombre = colIniTable + 30;
            int colResponsable4Porcentaje = colIniTable + 31;
            int colResponsable4DispFinal = colIniTable + 32;
            int colResponsable4CompCero = colIniTable + 33;
            int colResponsable5Nombre = colIniTable + 34;
            int colResponsable5Porcentaje = colIniTable + 35;
            int colResponsable5DispFinal = colIniTable + 36;
            int colResponsable5CompCero = colIniTable + 37;
            int colCausaResumida = colIniTable + 38;
            int colEiE = colIniTable + 39;
            int colResarcimiento = colIniTable + 40;
            int colAgenteResp1 = colIniTable + 41;
            int colAgenteResp2 = colIniTable + 42;
            int colAgenteResp3 = colIniTable + 43;
            int colAgenteResp4 = colIniTable + 44;
            int colAgenteResp5 = colIniTable + 45;
            int colAplicacionAResp1 = colIniTable + 46;
            int colAplicacionAResp2 = colIniTable + 47;
            int colAplicacionAResp3 = colIniTable + 48;
            int colAplicacionAResp4 = colIniTable + 49;
            int colAplicacionAResp5 = colIniTable + 50;

            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colCorrelativo].Value = "Correlativo por Punto de Entrega";
            ws.Cells[rowIniTabla, colTipoCliente].Value = "Tipo de Cliente";
            ws.Cells[rowIniTabla, colNombreCliente].Value = "Cliente";
            ws.Cells[rowIniTabla, colPuntoEntregaBarraNombre].Value = "Punto de Entrega/Barra";
            ws.Cells[rowIniTabla, colNumSuministroClienteLibre].Value = "N° de suministro cliente libre";
            ws.Cells[rowIniTabla, colNivelTensionNombre].Value = "Nivel de Tensión";
            ws.Cells[rowIniTabla, colAplicacionLiteral].Value = "Aplicación literal e) de numeral 5.2.4 Base Metodológica (meses de suministro en el semestre)";
            ws.Cells[rowIniTabla, colEnergiaPeriodo].Value = "Energía Semestral (kWh)";
            ws.Cells[rowIniTabla, colIncrementoTolerancia].Value = "Incremento de tolerancias Sector Distribución Típico 2 (Mercado regulado)";
            ws.Cells[rowIniTabla, colTipoNombre].Value = "Tipo";
            ws.Cells[rowIniTabla, colCausaNombre].Value = "Causa";
            ws.Cells[rowIniTabla, colNi].Value = "Ni";
            ws.Cells[rowIniTabla, colKi].Value = "Ki";
            ws.Cells[rowIniTabla, colTiempoEjecutadoIni].Value = "Tiempo Ejecutado";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colTiempoProgramadoIni].Value = "Tiempo Programado";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colResponsable1Nombre].Value = "Responsable 1";
            ws.Cells[rowIniTabla + 1, colResponsable1Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable1Porcentaje].Value = "%";
            ws.Cells[rowIniTabla + 1, colResponsable1DispFinal].Value = "Aplica 1DF NTCSE";
            ws.Cells[rowIniTabla + 1, colResponsable1CompCero].Value = "Compensación Cero";
            ws.Cells[rowIniTabla, colResponsable2Nombre].Value = "Responsable 2";
            ws.Cells[rowIniTabla + 1, colResponsable2Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable2Porcentaje].Value = "%";
            ws.Cells[rowIniTabla + 1, colResponsable2DispFinal].Value = "Aplica 1DF NTCSE";
            ws.Cells[rowIniTabla + 1, colResponsable2CompCero].Value = "Compensación Cero";
            ws.Cells[rowIniTabla, colResponsable3Nombre].Value = "Responsable 3";
            ws.Cells[rowIniTabla + 1, colResponsable3Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable3Porcentaje].Value = "%";
            ws.Cells[rowIniTabla + 1, colResponsable3DispFinal].Value = "Aplica 1DF NTCSE";
            ws.Cells[rowIniTabla + 1, colResponsable3CompCero].Value = "Compensación Cero";
            ws.Cells[rowIniTabla, colResponsable4Nombre].Value = "Responsable 4";
            ws.Cells[rowIniTabla + 1, colResponsable4Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable4Porcentaje].Value = "%";
            ws.Cells[rowIniTabla + 1, colResponsable4DispFinal].Value = "Aplica 1DF NTCSE";
            ws.Cells[rowIniTabla + 1, colResponsable4CompCero].Value = "Compensación Cero";
            ws.Cells[rowIniTabla, colResponsable5Nombre].Value = "Responsable 5";
            ws.Cells[rowIniTabla + 1, colResponsable5Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable5Porcentaje].Value = "%";
            ws.Cells[rowIniTabla + 1, colResponsable5DispFinal].Value = "Aplica 1DF NTCSE";
            ws.Cells[rowIniTabla + 1, colResponsable5CompCero].Value = "Compensación Cero";
            ws.Cells[rowIniTabla, colCausaResumida].Value = "Causa resumida de interrupción";
            ws.Cells[rowIniTabla, colEiE].Value = "Ei / E";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento (US$)";
            ws.Cells[rowIniTabla, colAgenteResp1].Value = "Agente Resp. 1 (US$) - PE";
            ws.Cells[rowIniTabla, colAgenteResp2].Value = "Agente Resp. 2 (US$) - PE";
            ws.Cells[rowIniTabla, colAgenteResp3].Value = "Agente Resp. 3 (US$) - PE";
            ws.Cells[rowIniTabla, colAgenteResp4].Value = "Agente Resp. 4 (US$) - PE";
            ws.Cells[rowIniTabla, colAgenteResp5].Value = "Agente Resp. 5 (US$) - PE";
            ws.Cells[rowIniTabla, colAplicacionAResp1].Value = "Aplicación Disposición Final Agent Resp 1 (US$) - PE";
            ws.Cells[rowIniTabla, colAplicacionAResp2].Value = "Aplicación Disposición Final Agent Resp 2 (US$) - PE";
            ws.Cells[rowIniTabla, colAplicacionAResp3].Value = "Aplicación Disposición Final Agent Resp 3 (US$) - PE";
            ws.Cells[rowIniTabla, colAplicacionAResp4].Value = "Aplicación Disposición Final Agent Resp 4 (US$) - PE";
            ws.Cells[rowIniTabla, colAplicacionAResp5].Value = "Aplicación Disposición Final Agent Resp 5 (US$) - PE";


            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCorrelativo, rowIniTabla + 1, colCorrelativo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoCliente, rowIniTabla + 1, colTipoCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNombreCliente, rowIniTabla + 1, colNombreCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPuntoEntregaBarraNombre, rowIniTabla + 1, colPuntoEntregaBarraNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNumSuministroClienteLibre, rowIniTabla + 1, colNumSuministroClienteLibre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNivelTensionNombre, rowIniTabla + 1, colNivelTensionNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionLiteral, rowIniTabla + 1, colAplicacionLiteral);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEnergiaPeriodo, rowIniTabla + 1, colEnergiaPeriodo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colIncrementoTolerancia, rowIniTabla + 1, colIncrementoTolerancia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoNombre, rowIniTabla + 1, colTipoNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaNombre, rowIniTabla + 1, colCausaNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNi, rowIniTabla + 1, colNi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colKi, rowIniTabla + 1, colKi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoIni, rowIniTabla, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoProgramadoIni, rowIniTabla, colTiempoProgramadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable1Nombre, rowIniTabla, colResponsable1CompCero);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable2Nombre, rowIniTabla, colResponsable2CompCero);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable3Nombre, rowIniTabla, colResponsable3CompCero);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable4Nombre, rowIniTabla, colResponsable4CompCero);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable5Nombre, rowIniTabla, colResponsable5CompCero);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaResumida, rowIniTabla + 1, colCausaResumida);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEiE, rowIniTabla + 1, colEiE);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResarcimiento, rowIniTabla + 1, colResarcimiento);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResp1, rowIniTabla + 1, colAgenteResp1);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResp2, rowIniTabla + 1, colAgenteResp2);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResp3, rowIniTabla + 1, colAgenteResp3);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResp4, rowIniTabla + 1, colAgenteResp4);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResp5, rowIniTabla + 1, colAgenteResp5);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionAResp1, rowIniTabla + 1, colAplicacionAResp1);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionAResp2, rowIniTabla + 1, colAplicacionAResp2);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionAResp3, rowIniTabla + 1, colAplicacionAResp3);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionAResp4, rowIniTabla + 1, colAplicacionAResp4);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionAResp5, rowIniTabla + 1, colAplicacionAResp5);

            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colAplicacionAResp5, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colAplicacionAResp5, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colAplicacionAResp5, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "#8EA9DB");
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colAgenteResp1, rowIniTabla + 1, colAplicacionAResp5, "#F2F2F2");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colAplicacionAResp5);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in listaRegistrosPE)
            {
                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colCorrelativo].Value = item.Correlativo != null ? item.Correlativo.Value : item.Correlativo;
                ws.Cells[rowData, colTipoCliente].Value = item.TipoCliente != null ? item.TipoCliente.Trim() : null;
                ws.Cells[rowData, colNombreCliente].Value = item.NombreCliente != null ? item.NombreCliente.Trim() : null;
                ws.Cells[rowData, colPuntoEntregaBarraNombre].Value = item.PuntoEntregaBarraNombre != null ? item.PuntoEntregaBarraNombre.Trim() : null;
                ws.Cells[rowData, colNumSuministroClienteLibre].Value = item.NumSuministroClienteLibre != null ? item.NumSuministroClienteLibre : item.NumSuministroClienteLibre;
                ws.Cells[rowData, colNivelTensionNombre].Value = item.NivelTensionNombre != null ? item.NivelTensionNombre.Trim() : null;
                ws.Cells[rowData, colAplicacionLiteral].Value = item.AplicacionLiteral != null ? item.AplicacionLiteral.Value : item.AplicacionLiteral;
                ws.Cells[rowData, colEnergiaPeriodo].Value = item.EnergiaPeriodo != null ? item.EnergiaPeriodo.Value : item.EnergiaPeriodo;
                ws.Cells[rowData, colEnergiaPeriodo].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colIncrementoTolerancia].Value = item.IncrementoTolerancia != null ? item.IncrementoTolerancia.Trim() : null;
                ws.Cells[rowData, colTipoNombre].Value = item.TipoNombre != null ? item.TipoNombre.Trim() : null;
                ws.Cells[rowData, colCausaNombre].Value = item.CausaNombre != null ? item.CausaNombre.Trim() : null;
                ws.Cells[rowData, colNi].Value = item.Ni != null ? item.Ni.Value : item.Ni;
                ws.Cells[rowData, colNi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colKi].Value = item.Ki != null ? item.Ki.Value : item.Ki;
                ws.Cells[rowData, colKi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colTiempoEjecutadoIni].Value = item.TiempoEjecutadoIni != null ? item.TiempoEjecutadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoIni].Value = item.TiempoProgramadoIni != null ? item.TiempoProgramadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoFin].Value = item.TiempoProgramadoFin != null ? item.TiempoProgramadoFin.Trim() : null;
                ws.Cells[rowData, colResponsable1Nombre].Value = item.Responsable1Nombre != null ? item.Responsable1Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable1Porcentaje].Value = item.Responsable1Porcentaje != null ? item.Responsable1Porcentaje.Value / 100 : item.Responsable1Porcentaje;
                ws.Cells[rowData, colResponsable1Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable1DispFinal].Value = item.Responsable1DispFinal != null ? (item.Responsable1DispFinal.Trim() == "S" ? "Si" : (item.Responsable1DispFinal.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colResponsable1CompCero].Value = item.Responsable1CompCero != null ? (item.Responsable1CompCero.Trim() == "S" ? "Si" : (item.Responsable1CompCero.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colResponsable2Nombre].Value = item.Responsable2Nombre != null ? item.Responsable2Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable2Porcentaje].Value = item.Responsable2Porcentaje != null ? item.Responsable2Porcentaje.Value / 100 : item.Responsable2Porcentaje;
                ws.Cells[rowData, colResponsable2Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable2DispFinal].Value = item.Responsable2DispFinal != null ? (item.Responsable2DispFinal.Trim() == "S" ? "Si" : (item.Responsable2DispFinal.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colResponsable2CompCero].Value = item.Responsable2CompCero != null ? (item.Responsable2CompCero.Trim() == "S" ? "Si" : (item.Responsable2CompCero.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colResponsable3Nombre].Value = item.Responsable3Nombre != null ? item.Responsable3Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable3Porcentaje].Value = item.Responsable3Porcentaje != null ? item.Responsable3Porcentaje.Value / 100 : item.Responsable3Porcentaje;
                ws.Cells[rowData, colResponsable3Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable3DispFinal].Value = item.Responsable3DispFinal != null ? (item.Responsable3DispFinal.Trim() == "S" ? "Si" : (item.Responsable3DispFinal.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colResponsable3CompCero].Value = item.Responsable3CompCero != null ? (item.Responsable3CompCero.Trim() == "S" ? "Si" : (item.Responsable3CompCero.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colResponsable4Nombre].Value = item.Responsable4Nombre != null ? item.Responsable4Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable4Porcentaje].Value = item.Responsable4Porcentaje != null ? item.Responsable4Porcentaje.Value / 100 : item.Responsable4Porcentaje;
                ws.Cells[rowData, colResponsable4Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable4DispFinal].Value = item.Responsable4DispFinal != null ? (item.Responsable4DispFinal.Trim() == "S" ? "Si" : (item.Responsable4DispFinal.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colResponsable4CompCero].Value = item.Responsable4CompCero != null ? (item.Responsable4CompCero.Trim() == "S" ? "Si" : (item.Responsable4CompCero.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colResponsable5Nombre].Value = item.Responsable5Nombre != null ? item.Responsable5Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable5Porcentaje].Value = item.Responsable5Porcentaje != null ? item.Responsable5Porcentaje.Value / 100 : item.Responsable5Porcentaje;
                ws.Cells[rowData, colResponsable5Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable5DispFinal].Value = item.Responsable5DispFinal != null ? (item.Responsable5DispFinal.Trim() == "S" ? "Si" : (item.Responsable5DispFinal.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colResponsable5CompCero].Value = item.Responsable5CompCero != null ? (item.Responsable5CompCero.Trim() == "S" ? "Si" : (item.Responsable5CompCero.Trim() == "N" ? "No" : "")) : "";
                ws.Cells[rowData, colCausaResumida].Value = item.CausaResumida != null ? item.CausaResumida.Trim() : null;
                ws.Cells[rowData, colEiE].Value = item.EiE != null ? item.EiE.Value : item.EiE;
                ws.Cells[rowData, colEiE].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResarcimiento].Value = item.Resarcimiento != null ? item.Resarcimiento.Value : item.Resarcimiento;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colAgenteResp1].Value = item.AgenteResp1 != null ? item.AgenteResp1.Value : item.AgenteResp1;
                ws.Cells[rowData, colAgenteResp1].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAgenteResp2].Value = item.AgenteResp2 != null ? item.AgenteResp2.Value : item.AgenteResp2;
                ws.Cells[rowData, colAgenteResp2].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAgenteResp3].Value = item.AgenteResp3 != null ? item.AgenteResp3.Value : item.AgenteResp3;
                ws.Cells[rowData, colAgenteResp3].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAgenteResp4].Value = item.AgenteResp4 != null ? item.AgenteResp4.Value : item.AgenteResp4;
                ws.Cells[rowData, colAgenteResp4].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAgenteResp5].Value = item.AgenteResp5 != null ? item.AgenteResp5.Value : item.AgenteResp5;
                ws.Cells[rowData, colAgenteResp5].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAplicacionAResp1].Value = item.AplicacionAResp1 != null ? item.AplicacionAResp1.Value : item.AplicacionAResp1;
                ws.Cells[rowData, colAplicacionAResp1].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAplicacionAResp2].Value = item.AplicacionAResp2 != null ? item.AplicacionAResp2.Value : item.AplicacionAResp2;
                ws.Cells[rowData, colAplicacionAResp2].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAplicacionAResp3].Value = item.AplicacionAResp3 != null ? item.AplicacionAResp3.Value : item.AplicacionAResp3;
                ws.Cells[rowData, colAplicacionAResp3].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAplicacionAResp4].Value = item.AplicacionAResp4 != null ? item.AplicacionAResp4.Value : item.AplicacionAResp4;
                ws.Cells[rowData, colAplicacionAResp4].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAplicacionAResp5].Value = item.AplicacionAResp5 != null ? item.AplicacionAResp5.Value : item.AplicacionAResp5;
                ws.Cells[rowData, colAplicacionAResp5].Style.Numberformat.Format = FormatoNumDecimales(4);


                rowData++;
            }

            if (!listaRegistrosPE.Any()) rowData++;

            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colSuministrador, rowData - 1, colAplicacionAResp5, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministrador, rowData - 1, colAplicacionAResp5);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 2, colResponsable1DispFinal, rowData - 1, colResponsable1CompCero, "Centro");
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 2, colResponsable2DispFinal, rowData - 1, colResponsable2CompCero, "Centro");
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 2, colResponsable3DispFinal, rowData - 1, colResponsable3CompCero, "Centro");
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 2, colResponsable4DispFinal, rowData - 1, colResponsable4CompCero, "Centro");
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 2, colResponsable5DispFinal, rowData - 1, colResponsable5CompCero, "Centro");

            #endregion

            if (listaRegistrosPE.Any())
            {
                ws.Cells[rowIniTabla, colSuministrador, rowData, colAplicacionAResp5].AutoFitColumns();
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 14;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 18;
                ws.Column(colTiempoEjecutadoFin).Width = 18;
                ws.Column(colTiempoProgramadoIni).Width = 18;
                ws.Column(colTiempoProgramadoFin).Width = 18;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
                ws.Column(colAgenteResp1).Width = 10;
                ws.Column(colAgenteResp2).Width = 10;
                ws.Column(colAgenteResp3).Width = 10;
                ws.Column(colAgenteResp4).Width = 10;
                ws.Column(colAgenteResp5).Width = 10;
                ws.Column(colAplicacionAResp1).Width = 18;
                ws.Column(colAplicacionAResp2).Width = 18;
                ws.Column(colAplicacionAResp3).Width = 18;
                ws.Column(colAplicacionAResp4).Width = 18;
                ws.Column(colAplicacionAResp5).Width = 18;
            }
            else
            {
                ws.Column(colSuministrador).Width = 20;
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNombreCliente).Width = 12;
                ws.Column(colPuntoEntregaBarraNombre).Width = 22;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 12;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colTipoNombre).Width = 10;
                ws.Column(colCausaNombre).Width = 10;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 13;
                ws.Column(colTiempoEjecutadoFin).Width = 12;
                ws.Column(colTiempoProgramadoIni).Width = 13;
                ws.Column(colTiempoProgramadoFin).Width = 12;
                ws.Column(colResponsable1Nombre).Width = 8;
                ws.Column(colResponsable1Porcentaje).Width = 5;
                ws.Column(colResponsable2Nombre).Width = 8;
                ws.Column(colResponsable2Porcentaje).Width = 5;
                ws.Column(colResponsable3Nombre).Width = 8;
                ws.Column(colResponsable3Porcentaje).Width = 5;
                ws.Column(colResponsable4Nombre).Width = 8;
                ws.Column(colResponsable4Porcentaje).Width = 5;
                ws.Column(colResponsable5Nombre).Width = 8;
                ws.Column(colResponsable5Porcentaje).Width = 5;
                ws.Column(colCausaResumida).Width = 27;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
                ws.Column(colAgenteResp1).Width = 10;
                ws.Column(colAgenteResp2).Width = 10;
                ws.Column(colAgenteResp3).Width = 10;
                ws.Column(colAgenteResp4).Width = 10;
                ws.Column(colAgenteResp5).Width = 10;
                ws.Column(colAplicacionAResp1).Width = 18;
                ws.Column(colAplicacionAResp2).Width = 18;
                ws.Column(colAplicacionAResp3).Width = 18;
                ws.Column(colAplicacionAResp4).Width = 18;
                ws.Column(colAplicacionAResp5).Width = 18;
            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }

        /// <summary>
        /// Devuelve el listado de interrupciones por punto de entrega
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="InfoCompleta"></param>
        /// <param name="lista_Limtx"></param>
        /// <returns></returns>
        public List<InterrupcionSuministroPE> ObtenerListaInterrupcionesPEPorPeriodo(RePeriodoDTO periodo, bool InfoCompleta, List<LimiteIngreso> lista_Limtx)
        {
            List<InterrupcionSuministroPE> lstSalida = new List<InterrupcionSuministroPE>();

            //Obtengo las interrupciones por periodo
            List<ReInterrupcionSuministroDTO> lstInterrupcionesPE = FactorySic.GetReInterrupcionSuministroRepository().ObtenerPorEmpresaPeriodo(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi);

            //Obtenemos los detalles de las interrupciones
            List<ReInterrupcionSuministroDetDTO> lstDetallesInterrupcionesPE = FactorySic.GetReInterrupcionSuministroDetRepository().ObtenerPorEmpresaPeriodo(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi);

            int idPeriodoUsar = this.ObtenerIdPeriodoPadre(periodo.Repercodi);

            ////- Aca debemos cambiar porque las interrupciones son distintas
            EstructuraInterrupcion maestros = new EstructuraInterrupcion();
            maestros.ListaCliente = this.ObtenerEmpresas();
            maestros.ListaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
            maestros.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().List();
            maestros.ListaNivelTension = FactorySic.GetReNivelTensionRepository().List();
            maestros.ListaEmpresa = this.ObtenerEmpresasSuministradorasTotal();
            maestros.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoUsar);

            //generar Listado
            foreach (var interrupcion in lstInterrupcionesPE)
            {
                int interrupcionId = interrupcion.Reintcodi;

                List<ReInterrupcionSuministroDetDTO> lstDetalles = lstDetallesInterrupcionesPE.Where(x => x.Reintcodi == interrupcionId).ToList();

                List<ReEmpresaDTO> lstEmpresasPorCliente = maestros.ListaCliente.Where(x => x.Emprcodi == interrupcion.Reintcliente).ToList();
                RePtoentregaPeriodoDTO ptoERegulado = maestros.ListaPuntoEntrega.Find(x => x.Repentcodi == interrupcion.Repentcodi);
                ReNivelTensionDTO nivTension = maestros.ListaNivelTension.Find(x => x.Rentcodi == interrupcion.Rentcodi);
                ReTipoInterrupcionDTO tipoInt = maestros.ListaTipoInterrupcion.Find(x => x.Retintcodi == interrupcion.Retintcodi);
                ReCausaInterrupcionDTO causaInt = maestros.ListaCausaInterrupcion.Find(x => x.Recintcodi == interrupcion.Recintcodi);

                InterrupcionSuministroPE objInt = new InterrupcionSuministroPE();
                objInt.SuministradorId = interrupcion.Emprcodi.Value;
                objInt.PuntoEntregaId = interrupcion.Repentcodi; //para regulados
                objInt.NivelTensionId = interrupcion.Rentcodi;
                objInt.CausaId = interrupcion.Recintcodi;

                objInt.InterrupcionTipoId = interrupcion.Retintcodi; //contraste
                ReEmpresaDTO sum = maestros.ListaEmpresa.Find(x => x.Emprcodi == interrupcion.Emprcodi);
                objInt.Suministrador = sum != null ? (sum.Emprnomb != null ? sum.Emprnomb.Trim() : "") : "";
                objInt.Correlativo = interrupcion.Reintcorrelativo;
                objInt.TipoCliente = interrupcion.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado ? ConstantesCalculoResarcimiento.TextoClienteRegulado : (interrupcion.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteLibre ? ConstantesCalculoResarcimiento.TextoClienteLibre : "");
                objInt.NombreCliente = lstEmpresasPorCliente.Any() ? (lstEmpresasPorCliente.First().Emprnomb != null ? (lstEmpresasPorCliente.First().Emprnomb.Trim()) : "") : "";
                objInt.PuntoEntregaBarraNombre = (interrupcion.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado) ? (ptoERegulado != null ? (ptoERegulado.Repentnombre != null ? ptoERegulado.Repentnombre.Trim() : "") : "") : (interrupcion.Reintptoentrega != null ? interrupcion.Reintptoentrega.Trim() : "");
                objInt.NumSuministroClienteLibre = (interrupcion.Reintnrosuministro != null) ? interrupcion.Reintnrosuministro.ToString() : null;
                objInt.NivelTensionNombre = interrupcion.Rentcodi != null ? (nivTension != null ? (nivTension.Rentnombre != null ? nivTension.Rentnombre.Trim() : "") : "") : "";

                objInt.AplicacionLiteral = interrupcion.Reintaplicacionnumeral;
                objInt.EnergiaPeriodo = interrupcion.Reintenergiasemestral;
                objInt.IncrementoTolerancia = interrupcion.Reintinctolerancia == "S" ? ConstantesCalculoResarcimiento.TextoSi : (interrupcion.Reintinctolerancia == "N" ? ConstantesCalculoResarcimiento.TextoNo : "");
                objInt.TipoNombre = interrupcion.Retintcodi != null ? (tipoInt != null ? (tipoInt.Retintnombre != null ? tipoInt.Retintnombre.Trim() : "") : "") : "";
                objInt.CausaNombre = interrupcion.Recintcodi != null ? (causaInt != null ? (causaInt.Recintnombre != null ? causaInt.Recintnombre.Trim() : "") : "") : "";

                objInt.Ni = interrupcion.Reintni;
                objInt.Ki = interrupcion.Reintki;
                objInt.TiempoEjecutadoIni = interrupcion.Reintfejeinicio != null ? interrupcion.Reintfejeinicio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                objInt.TiempoEjecutadoFin = interrupcion.Reintfejefin != null ? interrupcion.Reintfejefin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                objInt.TiempoProgramadoIni = interrupcion.Reintfproginicio != null ? interrupcion.Reintfproginicio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                objInt.TiempoProgramadoFin = interrupcion.Reintfprogfin != null ? interrupcion.Reintfprogfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";

                ReInterrupcionSuministroDetDTO r1 = lstDetalles.Find(x => x.Reintdorden == 1);
                ReInterrupcionSuministroDetDTO r2 = lstDetalles.Find(x => x.Reintdorden == 2);
                ReInterrupcionSuministroDetDTO r3 = lstDetalles.Find(x => x.Reintdorden == 3);
                ReInterrupcionSuministroDetDTO r4 = lstDetalles.Find(x => x.Reintdorden == 4);
                ReInterrupcionSuministroDetDTO r5 = lstDetalles.Find(x => x.Reintdorden == 5);
                ReEmpresaDTO resp1 = r1 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r1.Emprcodi) : null;
                ReEmpresaDTO resp2 = r2 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r2.Emprcodi) : null;
                ReEmpresaDTO resp3 = r3 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r3.Emprcodi) : null;
                ReEmpresaDTO resp4 = r4 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r4.Emprcodi) : null;
                ReEmpresaDTO resp5 = r5 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r5.Emprcodi) : null;

                objInt.Responsable1CompCero = r1 != null ? (r1.Reintdcompcero != null ? r1.Reintdcompcero.Trim() : "N") : "";
                objInt.Responsable2CompCero = r2 != null ? (r2.Reintdcompcero != null ? r2.Reintdcompcero.Trim() : "N") : "";
                objInt.Responsable3CompCero = r3 != null ? (r3.Reintdcompcero != null ? r3.Reintdcompcero.Trim() : "N") : "";
                objInt.Responsable4CompCero = r4 != null ? (r4.Reintdcompcero != null ? r4.Reintdcompcero.Trim() : "N") : "";
                objInt.Responsable5CompCero = r5 != null ? (r5.Reintdcompcero != null ? r5.Reintdcompcero.Trim() : "N") : "";

                int factorCompCero1 = objInt.Responsable1CompCero == "S" ? 0 : 1;
                int factorCompCero2 = objInt.Responsable2CompCero == "S" ? 0 : 1;
                int factorCompCero3 = objInt.Responsable3CompCero == "S" ? 0 : 1;
                int factorCompCero4 = objInt.Responsable4CompCero == "S" ? 0 : 1;
                int factorCompCero5 = objInt.Responsable5CompCero == "S" ? 0 : 1;

                objInt.Responsable1DispFinal = r1 != null ? (r1.Reintddisposicion != null ? r1.Reintddisposicion.Trim() : "N") : "";
                objInt.Responsable2DispFinal = r2 != null ? (r2.Reintddisposicion != null ? r2.Reintddisposicion.Trim() : "N") : "";
                objInt.Responsable3DispFinal = r3 != null ? (r3.Reintddisposicion != null ? r3.Reintddisposicion.Trim() : "N") : "";
                objInt.Responsable4DispFinal = r4 != null ? (r4.Reintddisposicion != null ? r4.Reintddisposicion.Trim() : "N") : "";
                objInt.Responsable5DispFinal = r5 != null ? (r5.Reintddisposicion != null ? r5.Reintddisposicion.Trim() : "N") : "";
                objInt.ARDF1 = objInt.Responsable1DispFinal.ToUpper() == "S" ? "S" : "N";
                objInt.ARDF2 = objInt.Responsable2DispFinal.ToUpper() == "S" ? "S" : "N";
                objInt.ARDF3 = objInt.Responsable3DispFinal.ToUpper() == "S" ? "S" : "N";
                objInt.ARDF4 = objInt.Responsable4DispFinal.ToUpper() == "S" ? "S" : "N";
                objInt.ARDF5 = objInt.Responsable5DispFinal.ToUpper() == "S" ? "S" : "N";

                objInt.Responsable1Id = r1 != null ? r1.Emprcodi : null;
                objInt.Responsable2Id = r2 != null ? r2.Emprcodi : null;
                objInt.Responsable3Id = r3 != null ? r3.Emprcodi : null;
                objInt.Responsable4Id = r4 != null ? r4.Emprcodi : null;
                objInt.Responsable5Id = r5 != null ? r5.Emprcodi : null;

                objInt.Responsable1Nombre = resp1 != null ? (resp1.Emprnomb != null ? resp1.Emprnomb.Trim() : "") : "";
                objInt.Responsable1Porcentaje = r1 != null ? r1.Reintdorcentaje : null;
                objInt.Responsable2Nombre = resp2 != null ? (resp2.Emprnomb != null ? resp2.Emprnomb.Trim() : "") : "";
                objInt.Responsable2Porcentaje = r2 != null ? r2.Reintdorcentaje : null;
                objInt.Responsable3Nombre = resp3 != null ? (resp3.Emprnomb != null ? resp3.Emprnomb.Trim() : "") : "";
                objInt.Responsable3Porcentaje = r3 != null ? r3.Reintdorcentaje : null;
                objInt.Responsable4Nombre = resp4 != null ? (resp4.Emprnomb != null ? resp4.Emprnomb.Trim() : "") : "";
                objInt.Responsable4Porcentaje = r4 != null ? r4.Reintdorcentaje : null;
                objInt.Responsable5Nombre = resp5 != null ? (resp5.Emprnomb != null ? resp5.Emprnomb.Trim() : "") : "";
                objInt.Responsable5Porcentaje = r5 != null ? r5.Reintdorcentaje : null;
                objInt.CausaResumida = interrupcion.Reintcausaresumida != null ? interrupcion.Reintcausaresumida.Trim() : "";
                objInt.EiE = interrupcion.Reinteie;
                objInt.Resarcimiento = interrupcion.Reintresarcimiento;
                objInt.AgenteResp1 = (interrupcion.Reintresarcimiento != null && r1 != null) ? (r1.Reintdorcentaje != null ? (factorCompCero1 * interrupcion.Reintresarcimiento * r1.Reintdorcentaje / 100) : null) : null;
                objInt.AgenteResp2 = (interrupcion.Reintresarcimiento != null && r2 != null) ? (r2.Reintdorcentaje != null ? (factorCompCero2 * interrupcion.Reintresarcimiento * r2.Reintdorcentaje / 100) : null) : null;
                objInt.AgenteResp3 = (interrupcion.Reintresarcimiento != null && r3 != null) ? (r3.Reintdorcentaje != null ? (factorCompCero3 * interrupcion.Reintresarcimiento * r3.Reintdorcentaje / 100) : null) : null;
                objInt.AgenteResp4 = (interrupcion.Reintresarcimiento != null && r4 != null) ? (r4.Reintdorcentaje != null ? (factorCompCero4 * interrupcion.Reintresarcimiento * r4.Reintdorcentaje / 100) : null) : null;
                objInt.AgenteResp5 = (interrupcion.Reintresarcimiento != null && r5 != null) ? (r5.Reintdorcentaje != null ? (factorCompCero5 * interrupcion.Reintresarcimiento * r5.Reintdorcentaje / 100) : null) : null;

                if (InfoCompleta)
                {
                    LimiteIngreso objLimtx1 = objInt.Responsable1Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable1Id.Value) : null;
                    LimiteIngreso objLimtx2 = objInt.Responsable2Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable2Id.Value) : null;
                    LimiteIngreso objLimtx3 = objInt.Responsable3Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable3Id.Value) : null;
                    LimiteIngreso objLimtx4 = objInt.Responsable4Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable4Id.Value) : null;
                    LimiteIngreso objLimtx5 = objInt.Responsable5Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable5Id.Value) : null;

                    objInt.AplicacionAResp1 = objLimtx1 != null ? (objInt.Responsable1DispFinal == "S" ? (objLimtx1.Limite != "" ? (objLimtx1.Limite == "Si" ? ((objInt.AgenteResp1 != null && objLimtx1.Ajuste != null) ? (objInt.AgenteResp1 * objLimtx1.Ajuste / 100) : null) : null) : null) : null) : null;
                    objInt.AplicacionAResp2 = objLimtx2 != null ? (objInt.Responsable2DispFinal == "S" ? (objLimtx2.Limite != "" ? (objLimtx2.Limite == "Si" ? ((objInt.AgenteResp2 != null && objLimtx2.Ajuste != null) ? (objInt.AgenteResp2 * objLimtx2.Ajuste / 100) : null) : null) : null) : null) : null;
                    objInt.AplicacionAResp3 = objLimtx3 != null ? (objInt.Responsable3DispFinal == "S" ? (objLimtx3.Limite != "" ? (objLimtx3.Limite == "Si" ? ((objInt.AgenteResp3 != null && objLimtx3.Ajuste != null) ? (objInt.AgenteResp3 * objLimtx3.Ajuste / 100) : null) : null) : null) : null) : null;
                    objInt.AplicacionAResp4 = objLimtx4 != null ? (objInt.Responsable4DispFinal == "S" ? (objLimtx4.Limite != "" ? (objLimtx4.Limite == "Si" ? ((objInt.AgenteResp4 != null && objLimtx4.Ajuste != null) ? (objInt.AgenteResp4 * objLimtx4.Ajuste / 100) : null) : null) : null) : null) : null;
                    objInt.AplicacionAResp5 = objLimtx5 != null ? (objInt.Responsable5DispFinal == "S" ? (objLimtx5.Limite != "" ? (objLimtx5.Limite == "Si" ? ((objInt.AgenteResp5 != null && objLimtx5.Ajuste != null) ? (objInt.AgenteResp5 * objLimtx5.Ajuste / 100) : null) : null) : null) : null) : null;

                }
                DateTime? fecEjecIni = interrupcion.Reintfejeinicio;
                DateTime? fecProgIni = interrupcion.Reintfproginicio;
                DateTime? fecProgFin = interrupcion.Reintfprogfin;

                objInt.FechaEjecIniMinuto = fecEjecIni != null ? new DateTime(fecEjecIni.Value.Year, fecEjecIni.Value.Month, fecEjecIni.Value.Day, fecEjecIni.Value.Hour, fecEjecIni.Value.Minute, 0) : fecEjecIni; //contraste
                objInt.FechaEjecIniMinutoDesc = objInt.FechaEjecIniMinuto != null ? (objInt.FechaEjecIniMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : ""; //contraste                
                objInt.FechaEjecFin = interrupcion.Reintfejefin; //contraste
                objInt.FechaProgramadoIniMinuto = fecProgIni != null ? new DateTime(fecProgIni.Value.Year, fecProgIni.Value.Month, fecProgIni.Value.Day, fecProgIni.Value.Hour, fecProgIni.Value.Minute, 0) : fecProgIni; //contraste
                objInt.FechaProgramadoFinMinuto = fecProgFin != null ? new DateTime(fecProgFin.Value.Year, fecProgFin.Value.Month, fecProgFin.Value.Day, fecProgFin.Value.Hour, fecProgFin.Value.Minute, 0) : fecProgFin; //contraste

                objInt.InterrupcionId = interrupcionId;

                lstSalida.Add(objInt);
            }

            return lstSalida;
        }

        /// <summary>
        /// Genera el reporte de la pestaña RC
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="periodo"></param>
        /// <param name="listaRegistrosRC_Final"></param>
        private void GenerarArchivoExcelConsolidadoInterrupcionesSuministro_RC(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo, List<InterrupcionSuministroRC> listaRegistrosRC_Final)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroRC> listaRegistrosRC = new List<InterrupcionSuministroRC>();

            listaRegistrosRC = listaRegistrosRC_Final;
            //listaRegistrosRC = new List<InterrupcionSuministroRC>();  //para hacer evidencias de listado sin registros

            string nameWS = "RC";
            string titulo = "";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 2;

            int colIniTable = colIniTitulo - 1;
            int rowIniTabla = rowIniTitulo + 2;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;

            int colSuministrador = colIniTable;
            int colCorrelativo = colIniTable + 1;
            int colTipoCliente = colIniTable + 2;
            int colNombreCliente = colIniTable + 3;
            int colBarraNombre = colIniTable + 4;
            int colAlimentador = colIniTable + 5;
            int colEnst = colIniTable + 6;
            int ColEvento = colIniTable + 7;
            int colComentario = colIniTable + 8;
            int colTiempoEjecutadoIni = colIniTable + 9;
            int colTiempoEjecutadoFin = colIniTable + 10;
            int colPk = colIniTable + 11;
            int colCompensable = colIniTable + 12;
            int colEns = colIniTable + 13;
            int colResarcimiento = colIniTable + 14;
            int colAgenteResponsable1Nombre = colIniTable + 15;
            int colAgenteResponsable2Nombre = colIniTable + 16;
            int colAgenteResponsable3Nombre = colIniTable + 17;
            int colAgenteResponsable4Nombre = colIniTable + 18;
            int colAgenteResponsable5Nombre = colIniTable + 19;
            int colAgenteResponsable1Porcentaje = colIniTable + 20;
            int colAgenteResponsable2Porcentaje = colIniTable + 21;
            int colAgenteResponsable3Porcentaje = colIniTable + 22;
            int colAgenteResponsable4Porcentaje = colIniTable + 23;
            int colAgenteResponsable5Porcentaje = colIniTable + 24;
            int colAgenteResponsable1Usd = colIniTable + 25;
            int colAgenteResponsable2Usd = colIniTable + 26;
            int colAgenteResponsable3Usd = colIniTable + 27;
            int colAgenteResponsable4Usd = colIniTable + 28;
            int colAgenteResponsable5Usd = colIniTable + 29;
            int colAplicacionDFAgenteResp1 = colIniTable + 30;
            int colAplicacionDFAgenteResp2 = colIniTable + 31;
            int colAplicacionDFAgenteResp3 = colIniTable + 32;
            int colAplicacionDFAgenteResp4 = colIniTable + 33;
            int colAplicacionDFAgenteResp5 = colIniTable + 34;


            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colCorrelativo].Value = "Correlativo por Rechazo de Carga";
            ws.Cells[rowIniTabla, colTipoCliente].Value = "Tipo de Cliente";
            ws.Cells[rowIniTabla, colNombreCliente].Value = "Cliente";
            ws.Cells[rowIniTabla, colBarraNombre].Value = "Barra";
            ws.Cells[rowIniTabla, colAlimentador].Value = "Alimentador/SED";
            ws.Cells[rowIniTabla, colEnst].Value = "ENST f,k (kWh) ";
            ws.Cells[rowIniTabla, ColEvento].Value = "Evento COES";
            ws.Cells[rowIniTabla, colComentario].Value = "Comentario";
            ws.Cells[rowIniTabla, colTiempoEjecutadoIni].Value = "Tiempo Ejecutado";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoIni].Value = "Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoFin].Value = "Fin";
            ws.Cells[rowIniTabla, colPk].Value = "Pk (kW)";
            ws.Cells[rowIniTabla, colCompensable].Value = "Compensable";
            ws.Cells[rowIniTabla, colEns].Value = "ENS f,k";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento (US$)";
            ws.Cells[rowIniTabla, colAgenteResponsable1Nombre].Value = "Agente Responsable 1";
            ws.Cells[rowIniTabla, colAgenteResponsable2Nombre].Value = "Agente Responsable 2";
            ws.Cells[rowIniTabla, colAgenteResponsable3Nombre].Value = "Agente Responsable 3";
            ws.Cells[rowIniTabla, colAgenteResponsable4Nombre].Value = "Agente Responsable 4";
            ws.Cells[rowIniTabla, colAgenteResponsable5Nombre].Value = "Agente Responsable 5";
            ws.Cells[rowIniTabla, colAgenteResponsable1Porcentaje].Value = "% Resp. 1";
            ws.Cells[rowIniTabla, colAgenteResponsable2Porcentaje].Value = "% Resp. 2";
            ws.Cells[rowIniTabla, colAgenteResponsable3Porcentaje].Value = "% Resp. 3";
            ws.Cells[rowIniTabla, colAgenteResponsable4Porcentaje].Value = "% Resp. 4";
            ws.Cells[rowIniTabla, colAgenteResponsable5Porcentaje].Value = "% Resp. 5";
            ws.Cells[rowIniTabla, colAgenteResponsable1Usd].Value = "Agente Resp. 1 (US$)";
            ws.Cells[rowIniTabla, colAgenteResponsable2Usd].Value = "Agente Resp. 2 (US$)";
            ws.Cells[rowIniTabla, colAgenteResponsable3Usd].Value = "Agente Resp. 3 (US$)";
            ws.Cells[rowIniTabla, colAgenteResponsable4Usd].Value = "Agente Resp. 4 (US$)";
            ws.Cells[rowIniTabla, colAgenteResponsable5Usd].Value = "Agente Resp. 5 (US$)";
            ws.Cells[rowIniTabla, colAplicacionDFAgenteResp1].Value = "Aplicación Disposición Final Agent Resp 1 (US$)";
            ws.Cells[rowIniTabla, colAplicacionDFAgenteResp2].Value = "Aplicación Disposición Final Agent Resp 2 (US$)";
            ws.Cells[rowIniTabla, colAplicacionDFAgenteResp3].Value = "Aplicación Disposición Final Agent Resp 3 (US$)";
            ws.Cells[rowIniTabla, colAplicacionDFAgenteResp4].Value = "Aplicación Disposición Final Agent Resp 4 (US$)";
            ws.Cells[rowIniTabla, colAplicacionDFAgenteResp5].Value = "Aplicación Disposición Final Agent Resp 5 (US$)";


            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCorrelativo, rowIniTabla + 1, colCorrelativo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoCliente, rowIniTabla + 1, colTipoCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNombreCliente, rowIniTabla + 1, colNombreCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colBarraNombre, rowIniTabla + 1, colBarraNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAlimentador, rowIniTabla + 1, colAlimentador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEnst, rowIniTabla + 1, colEnst);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, ColEvento, rowIniTabla + 1, ColEvento);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colComentario, rowIniTabla + 1, colComentario);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoIni, rowIniTabla, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPk, rowIniTabla + 1, colPk);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCompensable, rowIniTabla + 1, colCompensable);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEns, rowIniTabla + 1, colEns);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResarcimiento, rowIniTabla + 1, colResarcimiento);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable1Nombre, rowIniTabla + 1, colAgenteResponsable1Nombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable2Nombre, rowIniTabla + 1, colAgenteResponsable2Nombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable3Nombre, rowIniTabla + 1, colAgenteResponsable3Nombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable4Nombre, rowIniTabla + 1, colAgenteResponsable4Nombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable5Nombre, rowIniTabla + 1, colAgenteResponsable5Nombre);

            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable1Porcentaje, rowIniTabla + 1, colAgenteResponsable1Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable2Porcentaje, rowIniTabla + 1, colAgenteResponsable2Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable3Porcentaje, rowIniTabla + 1, colAgenteResponsable3Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable4Porcentaje, rowIniTabla + 1, colAgenteResponsable4Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable5Porcentaje, rowIniTabla + 1, colAgenteResponsable5Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable1Usd, rowIniTabla + 1, colAgenteResponsable1Usd);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable2Usd, rowIniTabla + 1, colAgenteResponsable2Usd);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable3Usd, rowIniTabla + 1, colAgenteResponsable3Usd);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable4Usd, rowIniTabla + 1, colAgenteResponsable4Usd);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAgenteResponsable5Usd, rowIniTabla + 1, colAgenteResponsable5Usd);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionDFAgenteResp1, rowIniTabla + 1, colAplicacionDFAgenteResp1);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionDFAgenteResp2, rowIniTabla + 1, colAplicacionDFAgenteResp2);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionDFAgenteResp3, rowIniTabla + 1, colAplicacionDFAgenteResp3);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionDFAgenteResp4, rowIniTabla + 1, colAplicacionDFAgenteResp4);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionDFAgenteResp5, rowIniTabla + 1, colAplicacionDFAgenteResp5);


            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colAplicacionDFAgenteResp5, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colAplicacionDFAgenteResp5, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colAplicacionDFAgenteResp5, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "#8EA9DB");
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colAgenteResponsable1Nombre, rowIniTabla + 1, colAplicacionDFAgenteResp5, "#F2F2F2");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colAplicacionDFAgenteResp5);


            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in listaRegistrosRC)
            {

                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colCorrelativo].Value = item.Correlativo != null ? item.Correlativo.Value : item.Correlativo;
                ws.Cells[rowData, colTipoCliente].Value = item.TipoCliente != null ? item.TipoCliente.Trim() : null;
                ws.Cells[rowData, colNombreCliente].Value = item.NombreCliente != null ? item.NombreCliente.Trim() : null;
                ws.Cells[rowData, colBarraNombre].Value = item.BarraNombre != null ? item.BarraNombre.Trim() : null;
                ws.Cells[rowData, colAlimentador].Value = item.AlimentadorNombre != null ? item.AlimentadorNombre.Trim() : null;
                ws.Cells[rowData, colEnst].Value = item.Enst != null ? item.Enst.Value : item.Enst;
                ws.Cells[rowData, colEnst].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, ColEvento].Value = item.EventoNombre != null ? item.EventoNombre.Trim() : null;
                ws.Cells[rowData, colComentario].Value = item.Comentario != null ? item.Comentario.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoIni].Value = item.TiempoEjecutadoIni != null ? item.TiempoEjecutadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colPk].Value = item.Pk != null ? item.Pk.Value : item.Pk;
                ws.Cells[rowData, colPk].Style.Numberformat.Format = FormatoNumDecimales(1);
                ws.Cells[rowData, colCompensable].Value = item.Compensable != null ? item.Compensable.Trim() : null;

                //Ens
                if (item.Ens != null)
                {
                    if (item.Ens.Value == 0)
                        ws.Cells[rowData, colEns].Value = "-";
                    else
                        ws.Cells[rowData, colEns].Value = item.Ens.Value;
                }
                else
                    ws.Cells[rowData, colEns].Value = item.Ens;

                ws.Cells[rowData, colEns].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colResarcimiento].Value = item.Resarcimiento != null ? item.Resarcimiento.Value : item.Resarcimiento;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = FormatoNumDecimales(3);
                ws.Cells[rowData, colAgenteResponsable1Nombre].Value = item.AgResponsable1Nombre != null ? item.AgResponsable1Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResponsable2Nombre].Value = item.AgResponsable2Nombre != null ? item.AgResponsable2Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResponsable3Nombre].Value = item.AgResponsable3Nombre != null ? item.AgResponsable3Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResponsable4Nombre].Value = item.AgResponsable4Nombre != null ? item.AgResponsable4Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResponsable5Nombre].Value = item.AgResponsable5Nombre != null ? item.AgResponsable5Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResponsable1Porcentaje].Value = item.AgResponsable1Porcentaje != null ? item.AgResponsable1Porcentaje.Value / 100 : item.AgResponsable1Porcentaje;
                ws.Cells[rowData, colAgenteResponsable1Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colAgenteResponsable2Porcentaje].Value = item.AgResponsable2Porcentaje != null ? item.AgResponsable2Porcentaje.Value / 100 : item.AgResponsable2Porcentaje;
                ws.Cells[rowData, colAgenteResponsable2Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colAgenteResponsable3Porcentaje].Value = item.AgResponsable3Porcentaje != null ? item.AgResponsable3Porcentaje.Value / 100 : item.AgResponsable3Porcentaje;
                ws.Cells[rowData, colAgenteResponsable3Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colAgenteResponsable4Porcentaje].Value = item.AgResponsable4Porcentaje != null ? item.AgResponsable4Porcentaje.Value / 100 : item.AgResponsable4Porcentaje;
                ws.Cells[rowData, colAgenteResponsable4Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colAgenteResponsable5Porcentaje].Value = item.AgResponsable5Porcentaje != null ? item.AgResponsable5Porcentaje.Value / 100 : item.AgResponsable5Porcentaje;
                ws.Cells[rowData, colAgenteResponsable5Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colAgenteResponsable1Usd].Value = item.AgResponsable1USD != null ? item.AgResponsable1USD.Value : item.AgResponsable1USD;
                ws.Cells[rowData, colAgenteResponsable1Usd].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colAgenteResponsable2Usd].Value = item.AgResponsable2USD != null ? item.AgResponsable2USD.Value : item.AgResponsable2USD;
                ws.Cells[rowData, colAgenteResponsable2Usd].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colAgenteResponsable3Usd].Value = item.AgResponsable3USD != null ? item.AgResponsable3USD.Value : item.AgResponsable3USD;
                ws.Cells[rowData, colAgenteResponsable3Usd].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colAgenteResponsable4Usd].Value = item.AgResponsable4USD != null ? item.AgResponsable4USD.Value : item.AgResponsable4USD;
                ws.Cells[rowData, colAgenteResponsable4Usd].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colAgenteResponsable5Usd].Value = item.AgResponsable5USD != null ? item.AgResponsable5USD.Value : item.AgResponsable5USD;
                ws.Cells[rowData, colAgenteResponsable5Usd].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colAplicacionDFAgenteResp1].Value = item.AplicacionAResp1 != null ? item.AplicacionAResp1.Value : item.AplicacionAResp1;
                ws.Cells[rowData, colAplicacionDFAgenteResp1].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAplicacionDFAgenteResp2].Value = item.AplicacionAResp2 != null ? item.AplicacionAResp2.Value : item.AplicacionAResp2;
                ws.Cells[rowData, colAplicacionDFAgenteResp2].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAplicacionDFAgenteResp3].Value = item.AplicacionAResp3 != null ? item.AplicacionAResp3.Value : item.AplicacionAResp3;
                ws.Cells[rowData, colAplicacionDFAgenteResp3].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAplicacionDFAgenteResp4].Value = item.AplicacionAResp4 != null ? item.AplicacionAResp4.Value : item.AplicacionAResp4;
                ws.Cells[rowData, colAplicacionDFAgenteResp4].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colAplicacionDFAgenteResp5].Value = item.AplicacionAResp5 != null ? item.AplicacionAResp5.Value : item.AplicacionAResp5;
                ws.Cells[rowData, colAplicacionDFAgenteResp5].Style.Numberformat.Format = FormatoNumDecimales(4);

                rowData++;
            }

            if (!listaRegistrosRC.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colSuministrador, rowData - 1, colAplicacionDFAgenteResp5, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministrador, rowData - 1, colAplicacionDFAgenteResp5);
            #endregion

            //filter                       
            if (listaRegistrosRC.Any())
            {
                ws.Cells[rowIniTabla, colSuministrador, rowData, colAplicacionDFAgenteResp5].AutoFitColumns();
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colEnst).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 18;
                ws.Column(colTiempoEjecutadoFin).Width = 18;
                ws.Column(colPk).Width = 8;
                ws.Column(colEns).Width = 8;
                ws.Column(colAplicacionDFAgenteResp1).Width = 18;
                ws.Column(colAplicacionDFAgenteResp2).Width = 18;
                ws.Column(colAplicacionDFAgenteResp3).Width = 18;
                ws.Column(colAplicacionDFAgenteResp4).Width = 18;
                ws.Column(colAplicacionDFAgenteResp5).Width = 18;
            }
            else
            {
                ws.Column(colSuministrador).Width = 20;
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNombreCliente).Width = 12;
                ws.Column(colBarraNombre).Width = 15;
                ws.Column(colAlimentador).Width = 16;
                ws.Column(colEnst).Width = 8;
                ws.Column(ColEvento).Width = 18;
                ws.Column(colComentario).Width = 15;
                ws.Column(colTiempoEjecutadoIni).Width = 10;
                ws.Column(colTiempoEjecutadoFin).Width = 10;
                ws.Column(colPk).Width = 8;
                ws.Column(colCompensable).Width = 12;
                ws.Column(colEns).Width = 8;
                ws.Column(colResarcimiento).Width = 13;
                ws.Column(colAgenteResponsable1Nombre).Width = 12;
                ws.Column(colAgenteResponsable2Nombre).Width = 12;
                ws.Column(colAgenteResponsable3Nombre).Width = 12;
                ws.Column(colAgenteResponsable4Nombre).Width = 12;
                ws.Column(colAgenteResponsable5Nombre).Width = 12;
                ws.Column(colAgenteResponsable1Porcentaje).Width = 10;
                ws.Column(colAgenteResponsable2Porcentaje).Width = 10;
                ws.Column(colAgenteResponsable3Porcentaje).Width = 10;
                ws.Column(colAgenteResponsable4Porcentaje).Width = 10;
                ws.Column(colAgenteResponsable5Porcentaje).Width = 10;
                ws.Column(colAgenteResponsable1Usd).Width = 11;
                ws.Column(colAgenteResponsable2Usd).Width = 11;
                ws.Column(colAgenteResponsable3Usd).Width = 11;
                ws.Column(colAgenteResponsable4Usd).Width = 11;
                ws.Column(colAgenteResponsable5Usd).Width = 11;
                ws.Column(colAplicacionDFAgenteResp1).Width = 18;
                ws.Column(colAplicacionDFAgenteResp2).Width = 18;
                ws.Column(colAplicacionDFAgenteResp3).Width = 18;
                ws.Column(colAplicacionDFAgenteResp4).Width = 18;
                ws.Column(colAplicacionDFAgenteResp5).Width = 18;
            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }

        /// <summary>
        /// Devuelve el listado de interrupciones por rechazo de carga
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="infoCompleta"></param>
        /// <param name="lista_Limtx"></param>
        /// <returns></returns>
        public List<InterrupcionSuministroRC> ObtenerListaInterrupcionesRCPorPeriodo(RePeriodoDTO periodo, bool infoCompleta, List<LimiteIngreso> lista_Limtx)
        {
            List<InterrupcionSuministroRC> lstSalida = new List<InterrupcionSuministroRC>();

            //Obtengo las interrupciones por periodo
            List<ReRechazoCargaDTO> lstInterrupcionesRC = FactorySic.GetReRechazoCargaRepository().ObtenerPorEmpresaPeriodo(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi);

            int idPeriodoUsar = this.ObtenerIdPeriodoPadre(periodo.Repercodi);

            EstructuraRechazoCarga maestros = new EstructuraRechazoCarga();
            maestros.ListaCliente = this.ObtenerEmpresas();
            maestros.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoUsar);
            maestros.ListaEvento = this.ObtenerEventosPorPeriodo(idPeriodoUsar);
            List<ReEmpresaDTO> listSuministrador = this.ObtenerEmpresasSuministradorasTotal();

            foreach (var interrupcion in lstInterrupcionesRC)
            {
                int interrupcionId = interrupcion.Rerccodi;

                ReEventoPeriodoDTO objEvento = maestros.ListaEvento.Find(x => x.Reevecodi == interrupcion.Reevecodi);
                List<ReEmpresaDTO> lstEmpresasPorCliente = maestros.ListaCliente.Where(x => x.Emprcodi == interrupcion.Rerccliente).ToList();
                RePtoentregaPeriodoDTO ptoERegulado = maestros.ListaPuntoEntrega.Find(x => x.Repentcodi == interrupcion.Repentcodi);

                InterrupcionSuministroRC objInt = new InterrupcionSuministroRC();

                objInt.SuministradorId = interrupcion.Emprcodi.Value;
                objInt.BarraId = interrupcion.Repentcodi; //Solo cuando es regulado
                objInt.EventoId = interrupcion.Reevecodi;

                ReEmpresaDTO sum = listSuministrador.Find(x => x.Emprcodi == interrupcion.Emprcodi);
                objInt.Suministrador = sum != null ? sum.Emprnomb.Trim() : "";
                objInt.Correlativo = interrupcion.Rerccorrelativo;
                objInt.TipoCliente = interrupcion.Rerctipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado ? ConstantesCalculoResarcimiento.TextoClienteRegulado : (interrupcion.Rerctipcliente == ConstantesCalculoResarcimiento.TipoClienteLibre ? ConstantesCalculoResarcimiento.TextoClienteLibre : "");

                objInt.NombreCliente = lstEmpresasPorCliente.Any() ? (lstEmpresasPorCliente.First().Emprnomb != null ? (lstEmpresasPorCliente.First().Emprnomb.Trim()) : "") : "";
                objInt.BarraNombre = (interrupcion.Rerctipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado) ? (ptoERegulado != null ? (ptoERegulado.Repentnombre != null ? ptoERegulado.Repentnombre.Trim() : "") : "") : (interrupcion.Rercptoentrega != null ? interrupcion.Rercptoentrega.Trim() : "");
                objInt.AlimentadorNombre = interrupcion.Rercalimentadorsed != null ? interrupcion.Rercalimentadorsed.Trim() : null;
                objInt.Enst = interrupcion.Rercenst;
                objInt.EventoNombre = interrupcion.Reevecodi != null ? (objEvento != null ? objEvento.Reevedescripcion : null) : null;
                objInt.Comentario = interrupcion.Rerccomentario != null ? interrupcion.Rerccomentario.Trim() : "";
                objInt.TiempoEjecutadoIni = interrupcion.Rerctejecinicio != null ? interrupcion.Rerctejecinicio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                objInt.TiempoEjecutadoFin = interrupcion.Rerctejecfin != null ? interrupcion.Rerctejecfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                objInt.Pk = interrupcion.Rercpk;
                objInt.Compensable = interrupcion.Rerccompensable != null ? (interrupcion.Rerccompensable == "S" ? ConstantesCalculoResarcimiento.TextoSi : (interrupcion.Rerccompensable == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "";
                objInt.Ens = interrupcion.Rercens;
                objInt.Resarcimiento = interrupcion.Rercresarcimiento;

                objInt.Responsable1Id = objEvento != null ? objEvento.Reeveempr1 : null;
                objInt.Responsable2Id = objEvento != null ? objEvento.Reeveempr2 : null;
                objInt.Responsable3Id = objEvento != null ? objEvento.Reeveempr3 : null;
                objInt.Responsable4Id = objEvento != null ? objEvento.Reeveempr4 : null;
                objInt.Responsable5Id = objEvento != null ? objEvento.Reeveempr5 : null;

                objInt.DispFinalAResp1 = interrupcion.Rercdisposicion1 != null ? interrupcion.Rercdisposicion1.Trim() : "";
                objInt.DispFinalAResp2 = interrupcion.Rercdisposicion2 != null ? interrupcion.Rercdisposicion2.Trim() : "";
                objInt.DispFinalAResp3 = interrupcion.Rercdisposicion3 != null ? interrupcion.Rercdisposicion3.Trim() : "";
                objInt.DispFinalAResp4 = interrupcion.Rercdisposicion4 != null ? interrupcion.Rercdisposicion4.Trim() : "";
                objInt.DispFinalAResp5 = interrupcion.Rercdisposicion5 != null ? interrupcion.Rercdisposicion5.Trim() : "";
                objInt.DFAR1 = objInt.DispFinalAResp1.ToUpper() == "S" ? "S" : "N";
                objInt.DFAR2 = objInt.DispFinalAResp2.ToUpper() == "S" ? "S" : "N";
                objInt.DFAR3 = objInt.DispFinalAResp3.ToUpper() == "S" ? "S" : "N";
                objInt.DFAR4 = objInt.DispFinalAResp4.ToUpper() == "S" ? "S" : "N";
                objInt.DFAR5 = objInt.DispFinalAResp5.ToUpper() == "S" ? "S" : "N";

                objInt.AgResponsable1Nombre = objEvento != null ? (objEvento.Responsablenomb1 != null ? objEvento.Responsablenomb1.Trim() : "") : "";
                objInt.AgResponsable2Nombre = objEvento != null ? (objEvento.Responsablenomb2 != null ? objEvento.Responsablenomb2.Trim() : "") : "";
                objInt.AgResponsable3Nombre = objEvento != null ? (objEvento.Responsablenomb3 != null ? objEvento.Responsablenomb3.Trim() : "") : "";
                objInt.AgResponsable4Nombre = objEvento != null ? (objEvento.Responsablenomb4 != null ? objEvento.Responsablenomb4.Trim() : "") : "";
                objInt.AgResponsable5Nombre = objEvento != null ? (objEvento.Responsablenomb5 != null ? objEvento.Responsablenomb5.Trim() : "") : "";
                //objInt.AgResponsable1Porcentaje = objEvento != null ? objEvento.Reeveporc1 : null;
                //objInt.AgResponsable2Porcentaje = objEvento != null ? objEvento.Reeveporc2 : null;
                //objInt.AgResponsable3Porcentaje = objEvento != null ? objEvento.Reeveporc3 : null;
                //objInt.AgResponsable4Porcentaje = objEvento != null ? objEvento.Reeveporc4 : null;
                //objInt.AgResponsable5Porcentaje = objEvento != null ? objEvento.Reeveporc5 : null;
                objInt.AgResponsable1Porcentaje = interrupcion.Rercporcentaje1;
                objInt.AgResponsable2Porcentaje = interrupcion.Rercporcentaje2;
                objInt.AgResponsable3Porcentaje = interrupcion.Rercporcentaje3;
                objInt.AgResponsable4Porcentaje = interrupcion.Rercporcentaje4;
                objInt.AgResponsable5Porcentaje = interrupcion.Rercporcentaje5;
                //objInt.AgResponsable1USD = (interrupcion.Rercresarcimiento != null && objEvento != null) ? (objEvento.Reeveporc1 != null ? (interrupcion.Rercresarcimiento * objEvento.Reeveporc1 / 100) : null) : null;
                //objInt.AgResponsable2USD = (interrupcion.Rercresarcimiento != null && objEvento != null) ? (objEvento.Reeveporc2 != null ? (interrupcion.Rercresarcimiento * objEvento.Reeveporc2 / 100) : null) : null;
                //objInt.AgResponsable3USD = (interrupcion.Rercresarcimiento != null && objEvento != null) ? (objEvento.Reeveporc3 != null ? (interrupcion.Rercresarcimiento * objEvento.Reeveporc3 / 100) : null) : null;
                //objInt.AgResponsable4USD = (interrupcion.Rercresarcimiento != null && objEvento != null) ? (objEvento.Reeveporc4 != null ? (interrupcion.Rercresarcimiento * objEvento.Reeveporc4 / 100) : null) : null;
                //objInt.AgResponsable5USD = (interrupcion.Rercresarcimiento != null && objEvento != null) ? (objEvento.Reeveporc5 != null ? (interrupcion.Rercresarcimiento * objEvento.Reeveporc5 / 100) : null) : null;
                objInt.AgResponsable1USD = (interrupcion.Rercresarcimiento != null && interrupcion.Rercporcentaje1 != null) ? (interrupcion.Rercresarcimiento * interrupcion.Rercporcentaje1 / 100) : null;
                objInt.AgResponsable2USD = (interrupcion.Rercresarcimiento != null && interrupcion.Rercporcentaje2 != null) ? (interrupcion.Rercresarcimiento * interrupcion.Rercporcentaje2 / 100) : null;
                objInt.AgResponsable3USD = (interrupcion.Rercresarcimiento != null && interrupcion.Rercporcentaje3 != null) ? (interrupcion.Rercresarcimiento * interrupcion.Rercporcentaje3 / 100) : null;
                objInt.AgResponsable4USD = (interrupcion.Rercresarcimiento != null && interrupcion.Rercporcentaje4 != null) ? (interrupcion.Rercresarcimiento * interrupcion.Rercporcentaje4 / 100) : null;
                objInt.AgResponsable5USD = (interrupcion.Rercresarcimiento != null && interrupcion.Rercporcentaje5 != null) ? (interrupcion.Rercresarcimiento * interrupcion.Rercporcentaje5 / 100) : null;

                if (infoCompleta)
                {
                    LimiteIngreso objLimtx1 = objInt.Responsable1Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable1Id.Value) : null;
                    LimiteIngreso objLimtx2 = objInt.Responsable2Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable2Id.Value) : null;
                    LimiteIngreso objLimtx3 = objInt.Responsable3Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable3Id.Value) : null;
                    LimiteIngreso objLimtx4 = objInt.Responsable4Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable4Id.Value) : null;
                    LimiteIngreso objLimtx5 = objInt.Responsable5Id != null ? lista_Limtx.Find(x => x.EmpresaId == objInt.Responsable5Id.Value) : null;

                    objInt.AplicacionAResp1 = objLimtx1 != null ? (objInt.DispFinalAResp1 == "S" ? (objLimtx1.Limite != "" ? (objLimtx1.Limite == "Si" ? ((objInt.AgResponsable1USD != null && objLimtx1.Ajuste != null) ? (objInt.AgResponsable1USD * objLimtx1.Ajuste / 100) : null) : null) : null) : null) : null;
                    objInt.AplicacionAResp2 = objLimtx2 != null ? (objInt.DispFinalAResp2 == "S" ? (objLimtx2.Limite != "" ? (objLimtx2.Limite == "Si" ? ((objInt.AgResponsable2USD != null && objLimtx2.Ajuste != null) ? (objInt.AgResponsable2USD * objLimtx2.Ajuste / 100) : null) : null) : null) : null) : null;
                    objInt.AplicacionAResp3 = objLimtx3 != null ? (objInt.DispFinalAResp3 == "S" ? (objLimtx3.Limite != "" ? (objLimtx3.Limite == "Si" ? ((objInt.AgResponsable3USD != null && objLimtx3.Ajuste != null) ? (objInt.AgResponsable3USD * objLimtx3.Ajuste / 100) : null) : null) : null) : null) : null;
                    objInt.AplicacionAResp4 = objLimtx4 != null ? (objInt.DispFinalAResp4 == "S" ? (objLimtx4.Limite != "" ? (objLimtx4.Limite == "Si" ? ((objInt.AgResponsable4USD != null && objLimtx4.Ajuste != null) ? (objInt.AgResponsable4USD * objLimtx4.Ajuste / 100) : null) : null) : null) : null) : null;
                    objInt.AplicacionAResp5 = objLimtx5 != null ? (objInt.DispFinalAResp5 == "S" ? (objLimtx5.Limite != "" ? (objLimtx5.Limite == "Si" ? ((objInt.AgResponsable5USD != null && objLimtx5.Ajuste != null) ? (objInt.AgResponsable5USD * objLimtx5.Ajuste / 100) : null) : null) : null) : null) : null;

                }

                lstSalida.Add(objInt);
            }

            return lstSalida;
        }

        /// <summary>
        /// Genera el reporte de la pestaña RC
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="periodo"></param>
        /// <param name="listaEventosRC"></param>
        private void GenerarArchivoExcelConsolidadoInterrupcionesSuministro_EventoRC(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo, List<EventoRC> listaEventosRC)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //listaEventosRC = new List<EventoRC>();  //para hacer evidencias de listado sin registros

            string nameWS = "Eventos RC";
            string titulo = "";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 2;

            int colIniTable = colIniTitulo - 1;
            int rowIniTabla = rowIniTitulo + 2;

            ws.Row(rowIniTabla).Height = 30;

            int colEvento = colIniTable;
            int colFecha = colIniTable + 1;
            int colAgenteResp1 = colIniTable + 2;
            int colAgenteResp2 = colIniTable + 3;
            int colAgenteResp3 = colIniTable + 4;
            int colAgenteResp4 = colIniTable + 5;
            int colAgenteResp5 = colIniTable + 6;
            int colAgenteResp1Porcentaje = colIniTable + 7;
            int colAgenteResp2Porcentaje = colIniTable + 8;
            int colAgenteResp3Porcentaje = colIniTable + 9;
            int colAgenteResp4Porcentaje = colIniTable + 10;
            int colAgenteResp5Porcentaje = colIniTable + 11;
            int colComentario = colIniTable + 12;



            ws.Cells[rowIniTabla, colEvento].Value = "Evento COES";
            ws.Cells[rowIniTabla, colFecha].Value = "Fecha de evento";
            ws.Cells[rowIniTabla, colAgenteResp1].Value = "Agente Resp. 1";
            ws.Cells[rowIniTabla, colAgenteResp2].Value = "Agente Resp. 2";
            ws.Cells[rowIniTabla, colAgenteResp3].Value = "Agente Resp. 3";
            ws.Cells[rowIniTabla, colAgenteResp4].Value = "Agente Resp. 4";
            ws.Cells[rowIniTabla, colAgenteResp5].Value = "Agente Resp. 5";
            ws.Cells[rowIniTabla, colAgenteResp1Porcentaje].Value = "% Resp. 1";
            ws.Cells[rowIniTabla, colAgenteResp2Porcentaje].Value = "% Resp. 2";
            ws.Cells[rowIniTabla, colAgenteResp3Porcentaje].Value = "% Resp. 3";
            ws.Cells[rowIniTabla, colAgenteResp4Porcentaje].Value = "% Resp. 4";
            ws.Cells[rowIniTabla, colAgenteResp5Porcentaje].Value = "% Resp. 5";
            ws.Cells[rowIniTabla, colComentario].Value = "Comentario";


            //Estilos cabecera            
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colEvento, rowIniTabla, colComentario, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colEvento, rowIniTabla, colComentario, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colEvento, rowIniTabla, colComentario, "Calibri", 11);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colEvento, rowIniTabla, colComentario, "#8EA9DB");
            //servFormato.CeldasExcelWrapText(ws, rowIniTabla, colEvento, rowIniTabla + 1, colComentario);


            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in listaEventosRC)
            {

                ws.Cells[rowData, colEvento].Value = item.EventoDescripcion != null ? item.EventoDescripcion.Trim() : null;
                ws.Cells[rowData, colFecha].Value = item.Fecha != null ? item.Fecha.Trim() : null;
                ws.Cells[rowData, colAgenteResp1].Value = item.AgResponsable1Nombre != null ? item.AgResponsable1Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResp2].Value = item.AgResponsable2Nombre != null ? item.AgResponsable2Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResp3].Value = item.AgResponsable3Nombre != null ? item.AgResponsable3Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResp4].Value = item.AgResponsable4Nombre != null ? item.AgResponsable4Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResp5].Value = item.AgResponsable5Nombre != null ? item.AgResponsable5Nombre.Trim() : null;
                ws.Cells[rowData, colAgenteResp1Porcentaje].Value = item.AgResponsable1Porcentaje != null ? item.AgResponsable1Porcentaje.Value / 100 : item.AgResponsable1Porcentaje;
                ws.Cells[rowData, colAgenteResp1Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colAgenteResp2Porcentaje].Value = item.AgResponsable2Porcentaje != null ? item.AgResponsable2Porcentaje.Value / 100 : item.AgResponsable2Porcentaje;
                ws.Cells[rowData, colAgenteResp2Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colAgenteResp3Porcentaje].Value = item.AgResponsable3Porcentaje != null ? item.AgResponsable3Porcentaje.Value / 100 : item.AgResponsable3Porcentaje;
                ws.Cells[rowData, colAgenteResp3Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colAgenteResp4Porcentaje].Value = item.AgResponsable4Porcentaje != null ? item.AgResponsable4Porcentaje.Value / 100 : item.AgResponsable4Porcentaje;
                ws.Cells[rowData, colAgenteResp4Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colAgenteResp5Porcentaje].Value = item.AgResponsable5Porcentaje != null ? item.AgResponsable5Porcentaje.Value / 100 : item.AgResponsable5Porcentaje;
                ws.Cells[rowData, colAgenteResp5Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colComentario].Value = item.Comentario != null ? item.Comentario.Trim() : null;

                rowData++;
            }

            if (!listaEventosRC.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colEvento, rowData - 1, colComentario, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colEvento, rowData - 1, colComentario);
            #endregion

            ws.Cells[rowIniTabla, colEvento, rowData, colComentario].AutoFitColumns();

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        /// <summary>
        /// Devuelve el listado de eventos por periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<EventoRC> ObtenerListaEventosPorPeriodo(RePeriodoDTO periodo)
        {
            List<EventoRC> lstSalida = new List<EventoRC>();

            //Obtengo los eventos por periodo
            List<ReEventoPeriodoDTO> lstEventos = ObtenerEventosPorPeriodo(periodo.Repercodi).OrderBy(x => x.Reevefecha).ToList();

            foreach (var evento in lstEventos)
            {
                EventoRC objEvento = new EventoRC();

                objEvento.EventoId = evento.Reevecodi;

                objEvento.AgResponsable1Id = evento.Reeveempr1;
                objEvento.AgResponsable2Id = evento.Reeveempr2;
                objEvento.AgResponsable3Id = evento.Reeveempr3;
                objEvento.AgResponsable4Id = evento.Reeveempr4;
                objEvento.AgResponsable5Id = evento.Reeveempr5;

                objEvento.EventoDescripcion = evento.Reevedescripcion != null ? evento.Reevedescripcion.Trim() : "";
                objEvento.Fecha = evento.FechaEvento != null ? evento.FechaEvento.Trim() : "";
                objEvento.AgResponsable1Nombre = evento.Responsablenomb1 != null ? evento.Responsablenomb1.Trim() : "";
                objEvento.AgResponsable2Nombre = evento.Responsablenomb2 != null ? evento.Responsablenomb2.Trim() : "";
                objEvento.AgResponsable3Nombre = evento.Responsablenomb3 != null ? evento.Responsablenomb3.Trim() : "";
                objEvento.AgResponsable4Nombre = evento.Responsablenomb4 != null ? evento.Responsablenomb4.Trim() : "";
                objEvento.AgResponsable5Nombre = evento.Responsablenomb5 != null ? evento.Responsablenomb5.Trim() : "";
                objEvento.AgResponsable1Porcentaje = evento.Reeveporc1;
                objEvento.AgResponsable2Porcentaje = evento.Reeveporc2;
                objEvento.AgResponsable3Porcentaje = evento.Reeveporc3;
                objEvento.AgResponsable4Porcentaje = evento.Reeveporc4;
                objEvento.AgResponsable5Porcentaje = evento.Reeveporc5;
                objEvento.Comentario = evento.Reevecomentario != null ? evento.Reevecomentario.Trim() : "";

                lstSalida.Add(objEvento);
            }

            return lstSalida;
        }

        /// <summary>
        /// Genera el reporte de la pestaña MT
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="periodo"></param>
        /// <param name="listaRegistrosPE"></param>
        /// <param name="listaRegistrosRC"></param>
        /// <param name="listaEventosRC"></param>
        /// <param name="lista"></param>
        private void GenerarArchivoExcelConsolidadoInterrupcionesSuministro_MT(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo, List<InterrupcionSuministroPE> listaRegistrosPE,
            List<InterrupcionSuministroRC> listaRegistrosRC, List<EventoRC> listaEventosRC, out List<MontoTotalResarcimiento> lista)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            lista = new List<MontoTotalResarcimiento>();

            lista = ObtenerListaMontoTotalResarcimientoPorPeriodo(listaRegistrosPE, listaRegistrosRC, listaEventosRC);

            string nameWS = "MT";
            string titulo = "";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 2;

            int colIniTable = colIniTitulo - 1;
            int rowIniTabla = rowIniTitulo + 2;

            ws.Row(rowIniTabla).Height = 30;

            int colEmpresaTipo = colIniTable;
            int colEmpresaNombre = colIniTable + 1;
            int colResarcimientoIS = colIniTable + 2;
            int colResarcimientoRC = colIniTable + 3;
            int colResarcimientoFallatTx = colIniTable + 4;
            int colResarcimientoTotal = colIniTable + 5;
            int colResarcimientoTotalDF = colIniTable + 6;




            ws.Cells[rowIniTabla, colEmpresaTipo].Value = "Tipo de Empresa";
            ws.Cells[rowIniTabla, colEmpresaNombre].Value = "Nombre Empresa";
            ws.Cells[rowIniTabla, colResarcimientoIS].Value = "Resarcimiento (US$) – Interrupción de suministro";
            ws.Cells[rowIniTabla, colResarcimientoRC].Value = "Resarcimiento (US$) – Rechazo de carga";
            ws.Cells[rowIniTabla, colResarcimientoFallatTx].Value = "Resarcimiento por fallas en Transmisión (US$)";
            ws.Cells[rowIniTabla, colResarcimientoTotal].Value = "Resarcimiento total (US$)";
            ws.Cells[rowIniTabla, colResarcimientoTotalDF].Value = "Resarcimiento Total aplicando 1ra Disposición Final (US$)";


            //Estilos cabecera            
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colEmpresaTipo, rowIniTabla, colResarcimientoTotalDF, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colEmpresaTipo, rowIniTabla, colResarcimientoTotalDF, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colEmpresaTipo, rowIniTabla, colResarcimientoTotalDF, "Calibri", 11);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colEmpresaTipo, rowIniTabla, colResarcimientoTotalDF, "#8EA9DB");


            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in lista)
            {

                ws.Cells[rowData, colEmpresaTipo].Value = item.EmpresaTipo != null ? item.EmpresaTipo.Trim() : null;
                ws.Cells[rowData, colEmpresaNombre].Value = item.EmpresaNombre != null ? item.EmpresaNombre.Trim() : null;
                ws.Cells[rowData, colResarcimientoIS].Value = item.ResarcimientoIS != null ? item.ResarcimientoIS.Value : item.ResarcimientoIS;
                ws.Cells[rowData, colResarcimientoIS].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colResarcimientoRC].Value = item.ResarcimientoRC != null ? item.ResarcimientoRC.Value : item.ResarcimientoRC;
                ws.Cells[rowData, colResarcimientoRC].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colResarcimientoFallatTx].Value = item.ResarcimientoFallasTx != null ? item.ResarcimientoFallasTx.Value : item.ResarcimientoFallasTx;
                ws.Cells[rowData, colResarcimientoFallatTx].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colResarcimientoTotal].Value = item.ResarcimientoTotal != null ? item.ResarcimientoTotal.Value : item.ResarcimientoTotal;
                ws.Cells[rowData, colResarcimientoTotal].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colResarcimientoTotalDF].Value = item.ResarcimientoTotalDF != null ? item.ResarcimientoTotalDF.Value : item.ResarcimientoTotalDF;
                ws.Cells[rowData, colResarcimientoTotalDF].Style.Numberformat.Format = FormatoNumDecimales(4);



                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colEmpresaTipo, rowData - 1, colResarcimientoTotalDF, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colEmpresaTipo, rowData - 1, colResarcimientoTotalDF);
            #endregion

            ws.Cells[rowIniTabla, colEmpresaTipo, rowData, colResarcimientoTotalDF].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        /// <summary>
        /// Devuelve el listado de monto total de resarcimiento por periodo
        /// </summary>
        /// <param name="listaRegistrosPE"></param>
        /// <param name="listaRegistrosRC"></param>
        /// <param name="listaEventosRC"></param>
        /// <returns></returns>
        public List<MontoTotalResarcimiento> ObtenerListaMontoTotalResarcimientoPorPeriodo(List<InterrupcionSuministroPE> listaRegistrosPE, List<InterrupcionSuministroRC> listaRegistrosRC, List<EventoRC> listaEventosRC)
        {
            List<MontoTotalResarcimiento> lstSalida = new List<MontoTotalResarcimiento>();

            //Obtengo el total de agentes responsables
            List<int> lstEmpresas2 = ObtenerTotalEmpresasReporteMontoTotalResarcimiento(listaRegistrosPE, listaEventosRC);

            //Obtengo info para "Resarcimiento por fallas en Transmisión (US$)":             
            Dictionary<int, decimal?> listaRFallaTx = ObtenerListaMontoTotalResarcimientoFallasTxPorPeriodo(listaRegistrosPE, listaRegistrosRC, lstEmpresas2);

            //Obtengo tipo de empresas
            EventoAppServicio eventoServicio = new EventoAppServicio();
            List<SiTipoempresaDTO> lstTipoEmpresas = eventoServicio.ListarTipoEmpresas();

            //Obtengo lista General de empresas
            EstructuraInterrupcion maestros = new EstructuraInterrupcion();
            maestros.ListaEmpresa = this.ObtenerEmpresas();

            //foreach (int idEmpresa in lstEmpresas.Keys)
            foreach (int idEmpresa in lstEmpresas2)
            {
                ReEmpresaDTO empr = maestros.ListaEmpresa.Find(x => x.Emprcodi == idEmpresa);
                SiTipoempresaDTO tipoE = empr != null ? lstTipoEmpresas.Find(x => x.Tipoemprcodi == empr.Tipoemprcodi) : null;

                MontoTotalResarcimiento objMT = new MontoTotalResarcimiento();

                objMT.EmpresaId = idEmpresa;

                objMT.EmpresaTipo = tipoE != null ? (tipoE.Tipoemprdesc != null ? tipoE.Tipoemprdesc.Trim() : "") : "";
                objMT.EmpresaNombre = empr != null ? (empr.Emprnomb != null ? empr.Emprnomb.Trim() : "") : "";
                objMT.ResarcimientoIS = ObtenerResarcimientoTotalPE(idEmpresa, listaRegistrosPE);
                objMT.ResarcimientoRC = ObtenerResarcimientoTotalRC(idEmpresa, listaRegistrosRC);
                decimal? resarcimiento1DF_PE = ObtenerResarcimientoTotalDFPE(idEmpresa, listaRegistrosPE);
                decimal? resarcimiento1DF_RC = ObtenerResarcimientoTotalDFRC(idEmpresa, listaRegistrosRC);

                decimal? total = null;

                if (objMT.ResarcimientoIS != null)
                {
                    if (objMT.ResarcimientoRC != null)
                        total = objMT.ResarcimientoIS.Value + objMT.ResarcimientoRC.Value;
                    else
                        total = objMT.ResarcimientoIS.Value;
                }
                else
                {
                    if (objMT.ResarcimientoRC != null)
                        total = objMT.ResarcimientoRC.Value;
                }

                objMT.ResarcimientoFallasTx = listaRFallaTx[idEmpresa];
                objMT.ResarcimientoTotal = total;

                //Aplicando DF
                decimal? totalDF = null;
                if (resarcimiento1DF_PE != null)
                {
                    if (resarcimiento1DF_RC != null)
                        totalDF = resarcimiento1DF_PE.Value + resarcimiento1DF_RC.Value;
                    else
                        totalDF = resarcimiento1DF_PE.Value;
                }
                else
                {
                    if (resarcimiento1DF_RC != null)
                        totalDF = resarcimiento1DF_RC.Value;
                }
                objMT.ResarcimientoTotalDF = totalDF;

                lstSalida.Add(objMT);
            }

            return lstSalida.OrderBy(x => x.EmpresaNombre).ToList();
        }

        /// <summary>
        /// Obtiene el valor de resarcimiento por fallas de transmision
        /// </summary>
        /// <param name="listaRegistrosPE"></param>
        /// <param name="listaRegistrosRC"></param>
        /// <param name="empresasPresentes"></param>
        /// <returns></returns>
        public Dictionary<int, decimal?> ObtenerListaMontoTotalResarcimientoFallasTxPorPeriodo(List<InterrupcionSuministroPE> listaRegistrosPE, List<InterrupcionSuministroRC> listaRegistrosRC, List<int> empresasPresentes)
        {
            Dictionary<int, decimal?> lstSalida = new Dictionary<int, decimal?>();

            foreach (int idEmpresa in empresasPresentes)
            {
                int emprcodiResponsable = idEmpresa;

                //Datos pestaña PE
                List<InterrupcionSuministroPE> listaRegistrosPEResp1 = listaRegistrosPE.Where(x => x.Responsable1Id == emprcodiResponsable && x.Responsable1DispFinal == "S").ToList();
                List<InterrupcionSuministroPE> listaRegistrosPEResp2 = listaRegistrosPE.Where(x => x.Responsable2Id == emprcodiResponsable && x.Responsable2DispFinal == "S").ToList();
                List<InterrupcionSuministroPE> listaRegistrosPEResp3 = listaRegistrosPE.Where(x => x.Responsable3Id == emprcodiResponsable && x.Responsable3DispFinal == "S").ToList();
                List<InterrupcionSuministroPE> listaRegistrosPEResp4 = listaRegistrosPE.Where(x => x.Responsable4Id == emprcodiResponsable && x.Responsable4DispFinal == "S").ToList();
                List<InterrupcionSuministroPE> listaRegistrosPEResp5 = listaRegistrosPE.Where(x => x.Responsable5Id == emprcodiResponsable && x.Responsable5DispFinal == "S").ToList();

                decimal? sumaAgResponsablesPE = listaRegistrosPEResp1.Sum(x => x.AgenteResp1) + listaRegistrosPEResp2.Sum(x => x.AgenteResp2) + listaRegistrosPEResp3.Sum(x => x.AgenteResp3) +
                    listaRegistrosPEResp4.Sum(x => x.AgenteResp4) + listaRegistrosPEResp5.Sum(x => x.AgenteResp5);

                //Datos pestaña RC
                List<InterrupcionSuministroRC> listaRegistrosRCResp1 = listaRegistrosRC.Where(x => x.Responsable1Id == emprcodiResponsable && x.DispFinalAResp1 == "S").ToList();
                List<InterrupcionSuministroRC> listaRegistrosRCResp2 = listaRegistrosRC.Where(x => x.Responsable2Id == emprcodiResponsable && x.DispFinalAResp2 == "S").ToList();
                List<InterrupcionSuministroRC> listaRegistrosRCResp3 = listaRegistrosRC.Where(x => x.Responsable3Id == emprcodiResponsable && x.DispFinalAResp3 == "S").ToList();
                List<InterrupcionSuministroRC> listaRegistrosRCResp4 = listaRegistrosRC.Where(x => x.Responsable4Id == emprcodiResponsable && x.DispFinalAResp4 == "S").ToList();
                List<InterrupcionSuministroRC> listaRegistrosRCResp5 = listaRegistrosRC.Where(x => x.Responsable5Id == emprcodiResponsable && x.DispFinalAResp5 == "S").ToList();

                decimal? sumaAgResponsablesRC = listaRegistrosRCResp1.Sum(x => x.AgResponsable1USD) + listaRegistrosRCResp2.Sum(x => x.AgResponsable2USD) + listaRegistrosRCResp3.Sum(x => x.AgResponsable3USD) +
                    listaRegistrosRCResp4.Sum(x => x.AgResponsable4USD) + listaRegistrosRCResp5.Sum(x => x.AgResponsable5USD);

                decimal? sumaTotal = sumaAgResponsablesPE + sumaAgResponsablesRC;

                if (listaRegistrosPEResp1.Any() || listaRegistrosPEResp2.Any() || listaRegistrosPEResp3.Any() || listaRegistrosPEResp4.Any() || listaRegistrosPEResp5.Any()
                    || listaRegistrosRCResp1.Any() || listaRegistrosRCResp2.Any() || listaRegistrosRCResp3.Any() || listaRegistrosRCResp4.Any() || listaRegistrosRCResp5.Any())
                {
                    lstSalida.Add(emprcodiResponsable, sumaTotal);
                }
                else
                {
                    lstSalida.Add(emprcodiResponsable, null);
                }

            }


            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado de empresas del reporte Monto Total de Resarcimiento
        /// </summary>
        /// <param name="listaRegistrosPE"></param>
        /// <param name="listaEventosRC"></param>
        /// <returns></returns>
        public List<int> ObtenerTotalEmpresasReporteMontoTotalResarcimiento(List<InterrupcionSuministroPE> listaRegistrosPE, List<EventoRC> listaEventosRC)
        {
            List<int> lstSalida = new List<int>();
            List<int> lstTemp = new List<int>();
            List<int> lstEmpresasPE = new List<int>();
            List<int> lstEmpresasRC = new List<int>();

            //obtengo todos los responsables de PE
            foreach (var interrupcionPE in listaRegistrosPE)
            {
                if (interrupcionPE.Responsable1Id != null) lstEmpresasPE.Add(interrupcionPE.Responsable1Id.Value);
                if (interrupcionPE.Responsable2Id != null) lstEmpresasPE.Add(interrupcionPE.Responsable2Id.Value);
                if (interrupcionPE.Responsable3Id != null) lstEmpresasPE.Add(interrupcionPE.Responsable3Id.Value);
                if (interrupcionPE.Responsable4Id != null) lstEmpresasPE.Add(interrupcionPE.Responsable4Id.Value);
                if (interrupcionPE.Responsable5Id != null) lstEmpresasPE.Add(interrupcionPE.Responsable5Id.Value);
            }

            //obtengo todos los responsables de RC (para ello usamos listado de eventos)
            foreach (var eventoRC in listaEventosRC)
            {
                if (eventoRC.AgResponsable1Id != null) lstEmpresasRC.Add(eventoRC.AgResponsable1Id.Value);
                if (eventoRC.AgResponsable2Id != null) lstEmpresasRC.Add(eventoRC.AgResponsable2Id.Value);
                if (eventoRC.AgResponsable3Id != null) lstEmpresasRC.Add(eventoRC.AgResponsable3Id.Value);
                if (eventoRC.AgResponsable4Id != null) lstEmpresasRC.Add(eventoRC.AgResponsable4Id.Value);
                if (eventoRC.AgResponsable5Id != null) lstEmpresasRC.Add(eventoRC.AgResponsable5Id.Value);


            }

            //juntamos ambos y filtramos sin repeticiones                        
            lstTemp.AddRange(lstEmpresasPE);
            lstTemp.AddRange(lstEmpresasRC);

            lstSalida = lstTemp.Distinct().ToList();

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el total de resarcimiento PE para cierta empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="listaRegistrosPE"></param>
        /// <returns></returns>
        public decimal? ObtenerResarcimientoTotalPE(int idEmpresa, List<InterrupcionSuministroPE> listaRegistrosPE)
        {
            decimal? salida;
            decimal temporalReg = 0; //suma los resarcimientos de la emrpesa de un registro
            decimal temporalGrupo = 0;//suma los resarcimientos de la emrpesa de todos los registros de la empresa
            List<InterrupcionSuministroPE> lstPEEmpresa = listaRegistrosPE.Where(x => x.Responsable1Id == idEmpresa || x.Responsable2Id == idEmpresa || x.Responsable3Id == idEmpresa || x.Responsable4Id == idEmpresa || x.Responsable5Id == idEmpresa).ToList();

            bool hayDato = false;
            foreach (var interrupcion in lstPEEmpresa)
            {
                temporalReg = 0;

                if (interrupcion.Responsable1Id == idEmpresa)
                {
                    if (interrupcion.AgenteResp1 != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgenteResp1.Value;
                        hayDato = true;
                    }
                }

                if (interrupcion.Responsable2Id == idEmpresa)
                {
                    if (interrupcion.AgenteResp2 != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgenteResp2.Value;
                        hayDato = true;
                    }
                }

                if (interrupcion.Responsable3Id == idEmpresa)
                {
                    if (interrupcion.AgenteResp3 != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgenteResp3.Value;
                        hayDato = true;
                    }
                }

                if (interrupcion.Responsable4Id == idEmpresa)
                {
                    if (interrupcion.AgenteResp4 != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgenteResp4.Value;
                        hayDato = true;
                    }
                }

                if (interrupcion.Responsable5Id == idEmpresa)
                {
                    if (interrupcion.AgenteResp5 != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgenteResp5.Value;
                        hayDato = true;
                    }
                }

                temporalGrupo = temporalGrupo + temporalReg;
            }

            if (temporalGrupo != 0)
                salida = temporalGrupo;
            else
            {
                if (hayDato)
                    salida = 0;
                else
                    salida = null;
            }


            return salida;
        }

        /// <summary>
        /// Devuelve el total de resarcimiento PE aplicando la 1ra disposicion final para cierta empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="listaRegistrosPE"></param>
        /// <returns></returns>
        public decimal? ObtenerResarcimientoTotalDFPE(int idEmpresa, List<InterrupcionSuministroPE> listaRegistrosPE)
        {
            decimal? salida;
            decimal temporalReg = 0; //suma los resarcimientos de la emrpesa de un registro
            decimal temporalGrupo = 0;//suma los resarcimientos de la emrpesa de todos los registros de la empresa
            List<InterrupcionSuministroPE> lstPEEmpresa = listaRegistrosPE.Where(x => x.Responsable1Id == idEmpresa || x.Responsable2Id == idEmpresa || x.Responsable3Id == idEmpresa || x.Responsable4Id == idEmpresa || x.Responsable5Id == idEmpresa).ToList();

            bool hayDato = false;
            foreach (var interrupcion in lstPEEmpresa)
            {
                temporalReg = 0;

                if (interrupcion.Responsable1Id == idEmpresa)
                {
                    if (interrupcion.ARDF1 != "S")
                    {
                        if (interrupcion.AgenteResp1 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgenteResp1.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp1 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp1.Value;
                            hayDato = true;
                        }
                    }
                }

                if (interrupcion.Responsable2Id == idEmpresa)
                {
                    //if (interrupcion.AgenteResp2 != null)
                    //{
                    //    temporalReg = temporalReg + interrupcion.AgenteResp2.Value;
                    //    hayDato = true;
                    //}
                    if (interrupcion.ARDF2 != "S")
                    {
                        if (interrupcion.AgenteResp2 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgenteResp2.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp2 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp2.Value;
                            hayDato = true;
                        }
                    }
                }

                if (interrupcion.Responsable3Id == idEmpresa)
                {
                    //if (interrupcion.AgenteResp3 != null)
                    //{
                    //    temporalReg = temporalReg + interrupcion.AgenteResp3.Value;
                    //    hayDato = true;
                    //}
                    if (interrupcion.ARDF3 != "S")
                    {
                        if (interrupcion.AgenteResp3 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgenteResp3.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp3 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp3.Value;
                            hayDato = true;
                        }
                    }
                }

                if (interrupcion.Responsable4Id == idEmpresa)
                {
                    //if (interrupcion.AgenteResp4 != null)
                    //{
                    //    temporalReg = temporalReg + interrupcion.AgenteResp4.Value;
                    //    hayDato = true;
                    //}
                    if (interrupcion.ARDF4 != "S")
                    {
                        if (interrupcion.AgenteResp4 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgenteResp4.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp4 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp4.Value;
                            hayDato = true;
                        }
                    }
                }

                if (interrupcion.Responsable5Id == idEmpresa)
                {
                    //if (interrupcion.AgenteResp5 != null)
                    //{
                    //    temporalReg = temporalReg + interrupcion.AgenteResp5.Value;
                    //    hayDato = true;
                    //}
                    if (interrupcion.ARDF5 != "S")
                    {
                        if (interrupcion.AgenteResp5 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgenteResp5.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp5 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp5.Value;
                            hayDato = true;
                        }
                    }
                }

                temporalGrupo = temporalGrupo + temporalReg;
            }

            if (temporalGrupo != 0)
                salida = temporalGrupo;
            else
            {
                if (hayDato)
                    salida = 0;
                else
                    salida = null;
            }


            return salida;
        }

        /// <summary>
        /// Devuelve el total de resarcimiento RC aplicando la 1ra disposicion final para cierta empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="listaRegistrosPE"></param>
        /// <returns></returns>
        public decimal? ObtenerResarcimientoTotalDFRC(int idEmpresa, List<InterrupcionSuministroRC> listaRegistrosRC)
        {
            decimal? salida;
            decimal temporalReg = 0; //suma los resarcimientos de la emrpesa de un registro
            decimal temporalGrupo = 0;//suma los resarcimientos de la emrpesa de todos los registros de la empresa
            List<InterrupcionSuministroRC> lstRCEmpresa = listaRegistrosRC.Where(x => x.Responsable1Id == idEmpresa || x.Responsable2Id == idEmpresa || x.Responsable3Id == idEmpresa || x.Responsable4Id == idEmpresa || x.Responsable5Id == idEmpresa).ToList();
            //List<InterrupcionSuministroPE> lstPEEmpresa = listaRegistrosRC.Where(x => x.Responsable1Id == idEmpresa || x.Responsable2Id == idEmpresa || x.Responsable3Id == idEmpresa || x.Responsable4Id == idEmpresa || x.Responsable5Id == idEmpresa).ToList();

            bool hayDato = false;
            foreach (var interrupcion in lstRCEmpresa)
            {
                temporalReg = 0;

                if (interrupcion.Responsable1Id == idEmpresa)
                {
                    if (interrupcion.DFAR1 != "S")
                    {
                        if (interrupcion.AgResponsable1USD != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgResponsable1USD.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp1 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp1.Value;
                            hayDato = true;
                        }
                    }
                }

                if (interrupcion.Responsable2Id == idEmpresa)
                {
                    //if (interrupcion.AgenteResp2 != null)
                    //{
                    //    temporalReg = temporalReg + interrupcion.AgenteResp2.Value;
                    //    hayDato = true;
                    //}
                    if (interrupcion.DFAR2 != "S")
                    {
                        if (interrupcion.AgResponsable2USD != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgResponsable2USD.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp2 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp2.Value;
                            hayDato = true;
                        }
                    }
                }

                if (interrupcion.Responsable3Id == idEmpresa)
                {
                    //if (interrupcion.AgenteResp3 != null)
                    //{
                    //    temporalReg = temporalReg + interrupcion.AgenteResp3.Value;
                    //    hayDato = true;
                    //}
                    if (interrupcion.DFAR3 != "S")
                    {
                        if (interrupcion.AgResponsable3USD != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgResponsable3USD.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp3 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp3.Value;
                            hayDato = true;
                        }
                    }
                }

                if (interrupcion.Responsable4Id == idEmpresa)
                {
                    //if (interrupcion.AgenteResp4 != null)
                    //{
                    //    temporalReg = temporalReg + interrupcion.AgenteResp4.Value;
                    //    hayDato = true;
                    //}
                    if (interrupcion.DFAR4 != "S")
                    {
                        if (interrupcion.AgResponsable4USD != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgResponsable4USD.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp4 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp4.Value;
                            hayDato = true;
                        }
                    }
                }

                if (interrupcion.Responsable5Id == idEmpresa)
                {
                    //if (interrupcion.AgenteResp5 != null)
                    //{
                    //    temporalReg = temporalReg + interrupcion.AgenteResp5.Value;
                    //    hayDato = true;
                    //}
                    if (interrupcion.DFAR5 != "S")
                    {
                        if (interrupcion.AgResponsable5USD != null)
                        {
                            temporalReg = temporalReg + interrupcion.AgResponsable5USD.Value;
                            hayDato = true;
                        }
                    }
                    else
                    {
                        if (interrupcion.AplicacionAResp5 != null)
                        {
                            temporalReg = temporalReg + interrupcion.AplicacionAResp5.Value;
                            hayDato = true;
                        }
                    }
                }

                temporalGrupo = temporalGrupo + temporalReg;
            }

            if (temporalGrupo != 0)
                salida = temporalGrupo;
            else
            {
                if (hayDato)
                    salida = 0;
                else
                    salida = null;
            }


            return salida;
        }

        /// <summary>
        /// Devuelve el total de resarcimiento RC para cierta empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="listaRegistrosRC"></param>
        /// <returns></returns>
        public decimal? ObtenerResarcimientoTotalRC(int idEmpresa, List<InterrupcionSuministroRC> listaRegistrosRC)
        {
            decimal? salida;
            decimal temporalReg = 0; //suma los resarcimientos de la emrpesa de un registro
            decimal temporalGrupo = 0;//suma los resarcimientos de la emrpesa de todos los registros de la empresa
            List<InterrupcionSuministroRC> lstRCEmpresa = listaRegistrosRC.Where(x => x.Responsable1Id == idEmpresa || x.Responsable2Id == idEmpresa || x.Responsable3Id == idEmpresa || x.Responsable4Id == idEmpresa || x.Responsable5Id == idEmpresa).ToList();

            bool hayDato = false;
            foreach (var interrupcion in lstRCEmpresa)
            {
                temporalReg = 0;

                if (interrupcion.Responsable1Id == idEmpresa)
                {
                    if (interrupcion.AgResponsable1USD != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgResponsable1USD.Value;
                        hayDato = true;
                    }
                }

                if (interrupcion.Responsable2Id == idEmpresa)
                {
                    if (interrupcion.AgResponsable2USD != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgResponsable2USD.Value;
                        hayDato = true;
                    }
                }

                if (interrupcion.Responsable3Id == idEmpresa)
                {
                    if (interrupcion.AgResponsable3USD != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgResponsable3USD.Value;
                        hayDato = true;
                    }
                }

                if (interrupcion.Responsable4Id == idEmpresa)
                {
                    if (interrupcion.AgResponsable4USD != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgResponsable4USD.Value;
                        hayDato = true;
                    }
                }

                if (interrupcion.Responsable5Id == idEmpresa)
                {
                    if (interrupcion.AgResponsable5USD != null)
                    {
                        temporalReg = temporalReg + interrupcion.AgResponsable5USD.Value;
                        hayDato = true;
                    }
                }

                temporalGrupo = temporalGrupo + temporalReg;
            }

            if (temporalGrupo != 0)
                salida = temporalGrupo;
            else
            {
                if (hayDato)
                    salida = 0;
                else
                    salida = null;
            }

            return salida;
        }

        /// <summary>
        /// Genera el reporte de la pestaña LIMTX
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="periodo"></param>
        /// <param name="listaRegistrosPE"></param>
        /// <param name="listaRegistrosRC"></param>
        /// <param name="listaEventosRC"></param>
        private void GenerarArchivoExcelConsolidadoInterrupcionesSuministro_Limtx(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo, List<InterrupcionSuministroPE> listaRegistrosPE, List<InterrupcionSuministroRC> listaRegistrosRC, List<EventoRC> listaEventosRC)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<LimiteIngreso> lista = new List<LimiteIngreso>();

            lista = ObtenerListaLimiteTransmisionPorPeriodo(periodo, listaRegistrosPE, listaRegistrosRC, listaEventosRC);
            //lista = new List<LimiteIngreso>();  //para hacer evidencias de listado sin registros

            string nameWS = "LIMTX";
            string titulo = "APLICACIÓN DISPOSICIÓN FINAL";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 1;
            int rowIniTitulo = 2;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 2;

            ws.Row(rowIniTabla).Height = 30;

            int colEmpresaNombre = colIniTable;
            int colIngresoTxSoles = colIniTable + 1;
            int colIngresoTxDolares = colIniTable + 2;
            int colTope = colIniTable + 3;
            int colResarcimiento = colIniTable + 4;
            int colResarcimientoFallatTx = colIniTable + 5;
            int colLimite = colIniTable + 6;
            int colAjuste = colIniTable + 7;


            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colAjuste);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colAjuste, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colAjuste, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colAjuste, "Calibri", 12);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colAjuste);

            ws.Cells[rowIniTabla, colEmpresaNombre].Value = "Empresa";
            ws.Cells[rowIniTabla, colIngresoTxSoles].Value = "Ingreso transmisión (S./.)";
            ws.Cells[rowIniTabla, colIngresoTxDolares].Value = "Ingreso transmisión (USD)";
            ws.Cells[rowIniTabla, colTope].Value = "Tope (10%) US$";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento total (US$)";
            ws.Cells[rowIniTabla, colResarcimientoFallatTx].Value = "Resarcimiento por fallas en Transmisión (US$)";
            ws.Cells[rowIniTabla, colLimite].Value = "Límite";
            ws.Cells[rowIniTabla, colAjuste].Value = "% Ajuste";


            //Estilos cabecera            
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colEmpresaNombre, rowIniTabla, colAjuste, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colEmpresaNombre, rowIniTabla, colAjuste, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colEmpresaNombre, rowIniTabla, colAjuste, "Calibri", 11);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colEmpresaNombre, rowIniTabla, colAjuste, "#8EA9DB");


            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in lista)
            {

                ws.Cells[rowData, colEmpresaNombre].Value = item.EmpresaNombre != null ? item.EmpresaNombre.Trim() : null;
                ws.Cells[rowData, colIngresoTxSoles].Value = item.IngresoTransmisionSoles != null ? item.IngresoTransmisionSoles.Value : item.IngresoTransmisionSoles;
                ws.Cells[rowData, colIngresoTxSoles].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colIngresoTxDolares].Value = item.IngresoTransmisionDolares != null ? item.IngresoTransmisionDolares.Value : item.IngresoTransmisionDolares;
                ws.Cells[rowData, colIngresoTxDolares].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colTope].Value = item.Tope != null ? item.Tope.Value : item.Tope;
                ws.Cells[rowData, colTope].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colResarcimiento].Value = item.ResarcimientoTotal != null ? item.ResarcimientoTotal.Value : item.ResarcimientoTotal;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colResarcimientoFallatTx].Value = item.ResarcimientoFallasTx != null ? item.ResarcimientoFallasTx.Value : item.ResarcimientoFallasTx;
                ws.Cells[rowData, colResarcimientoFallatTx].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colLimite].Value = item.Limite != null ? item.Limite.Trim() : null;
                ws.Cells[rowData, colAjuste].Value = item.Ajuste != null ? item.Ajuste.Value / 100 : item.Ajuste;
                ws.Cells[rowData, colAjuste].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);

                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colEmpresaNombre, rowData - 1, colAjuste, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colEmpresaNombre, rowData - 1, colAjuste);
            #endregion

            ws.Cells[rowIniTabla, colEmpresaNombre, rowData, colAjuste].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        /// <summary>
        /// Devuelve el listado de ingresos de transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="listaRegistrosPE"></param>
        /// <param name="listaRegistrosRC"></param>
        /// <param name="listaEventosRC"></param>
        /// <returns></returns>
        public List<LimiteIngreso> ObtenerListaLimiteTransmisionPorPeriodo(RePeriodoDTO periodo, List<InterrupcionSuministroPE> listaRegistrosPE, List<InterrupcionSuministroRC> listaRegistrosRC, List<EventoRC> listaEventosRC)
        {
            List<LimiteIngreso> lstSalida = new List<LimiteIngreso>();

            List<ReIngresoTransmisionDTO> lstIngresos = ObtenerIngresosTx(periodo);

            if (lstIngresos.Any())
            {
                decimal tipocambioDolares = periodo.Repertcambio != null ? periodo.Repertcambio.Value : 0;

                //Obtenemos lista de Montos totales de resarcimiento para el periodo
                List<MontoTotalResarcimiento> listaMT = ObtenerListaMontoTotalResarcimientoPorPeriodo(listaRegistrosPE, listaRegistrosRC, listaEventosRC);

                //Obtengo info para "Resarcimiento por fallas en Transmisión (US$)": 
                List<int> empresasPresentes = lstIngresos.Where(x => x.Emprcodi != null).Select(x => x.Emprcodi.Value).ToList();
                Dictionary<int, decimal?> listaRFallaTx = ObtenerListaMontoTotalResarcimientoFallasTxPorPeriodo(listaRegistrosPE, listaRegistrosRC, empresasPresentes);

                foreach (var ingresoTx in lstIngresos)
                {
                    MontoTotalResarcimiento objMT = listaMT.Find(x => x.EmpresaId == ingresoTx.Emprcodi.Value);

                    LimiteIngreso objIngreso = new LimiteIngreso();


                    objIngreso.IngresoId = ingresoTx.Reingcodi;
                    objIngreso.EmpresaId = ingresoTx.Emprcodi != null ? ingresoTx.Emprcodi.Value : -2;

                    objIngreso.EmpresaNombre = ingresoTx.Emprnomb != null ? ingresoTx.Emprnomb.Trim() : "";
                    objIngreso.IngresoTransmisionSoles = ingresoTx.Reingvalor != null ? (ingresoTx.Reingmoneda != null ? (ingresoTx.Reingmoneda == "SOLES" ? ingresoTx.Reingvalor : ingresoTx.Reingvalor * tipocambioDolares) : null) : null;
                    objIngreso.IngresoTransmisionDolares = ingresoTx.Reingvalor != null ? (ingresoTx.Reingmoneda != null ? (ingresoTx.Reingmoneda == "SOLES" ? ingresoTx.Reingvalor / tipocambioDolares : ingresoTx.Reingvalor) : null) : null;
                    objIngreso.Tope = objIngreso.IngresoTransmisionDolares != null ? objIngreso.IngresoTransmisionDolares / 10 : null;
                    objIngreso.ResarcimientoFallasTx = listaRFallaTx[ingresoTx.Emprcodi.Value];
                    objIngreso.ResarcimientoTotal = objMT != null ? (objMT.ResarcimientoTotal) : null;
                    //objIngreso.Limite = (objIngreso.ResarcimientoTotal != null && objIngreso.Tope != null) ? ((objIngreso.ResarcimientoTotal > objIngreso.Tope) ? "Si" : "No") : "";
                    objIngreso.Limite = (objIngreso.ResarcimientoFallasTx != null && objIngreso.Tope != null) ? ((objIngreso.ResarcimientoFallasTx > objIngreso.Tope) ? "Si" : "No") : "";
                    //objIngreso.Ajuste = objIngreso.Limite == "Si" ? ((objIngreso.Tope / objIngreso.ResarcimientoTotal * 100)) : null;
                    objIngreso.Ajuste = objIngreso.Limite == "Si" ? ((objIngreso.Tope / objIngreso.ResarcimientoFallasTx * 100)) : null;

                    lstSalida.Add(objIngreso);
                }
            }

            return lstSalida.OrderBy(x => x.EmpresaNombre).ToList();
        }

        /// <summary>
        /// Devuelve el listado de ingresos de transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private List<ReIngresoTransmisionDTO> ObtenerIngresosTx(RePeriodoDTO periodo)
        {
            //Si el periodo es trimestral se busca el padre de este y se trabaja con ese
            int idPeriodoUsar = 0;

            idPeriodoUsar = this.ObtenerIdPeriodoPadre(periodo.Repercodi);

            //Obtengo los ingresos por periodo (sea trimestral o semestral)
            List<ReIngresoTransmisionDTO> lstIngresos = ListIngresosPorTransmision(idPeriodoUsar);

            return lstIngresos;
        }



        #endregion

        #region Reporte Cumplimiento Envío Interrupción de Suministro

        /// <summary>
        /// Genera el reporte Cumplimiento Envío Interrupción de Suministros
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="periodo"></param>
        private void GenerarArchivoExcelCumplimientoEnvíoInterrupciónSuministros(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<CumplimientoEnvioInterrupcion> lista = new List<CumplimientoEnvioInterrupcion>();

            lista = ObtenerListaCumplimientoEnvioInterrupcionPorPeriodo(periodo);
            //lista = new List<CumplimientoEnvioInterrupcion>();  //para hacer evidencias de listado sin registros

            string nameWS = "Listado";
            string titulo = "Cumplimiento de envío de interrupción de suministros";
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 1;
            int rowIniTitulo = 3;


            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;



            ws.Row(rowIniTabla).Height = 30;

            int colSuministradorNombre = colIniTable;
            int colFechaUltimoEnvio = colIniTable + 1;
            int colNumInterrupcionesEnPlazo = colIniTable + 2;
            int colNumInterrupcionesFueraPlazo = colIniTable + 3;


            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colNumInterrupcionesFueraPlazo, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colNumInterrupcionesFueraPlazo);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colNumInterrupcionesFueraPlazo, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colNumInterrupcionesFueraPlazo);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colNumInterrupcionesFueraPlazo, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colNumInterrupcionesFueraPlazo);

            ws.Cells[rowIniTabla, colSuministradorNombre].Value = "Suministrador";
            ws.Cells[rowIniTabla, colFechaUltimoEnvio].Value = "Fecha del último envío de información";
            ws.Cells[rowIniTabla, colNumInterrupcionesEnPlazo].Value = "Total interrupciones en plazo";
            ws.Cells[rowIniTabla, colNumInterrupcionesFueraPlazo].Value = "Total interrupciones fuera de plazo";

            //Estilos cabecera            
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumInterrupcionesFueraPlazo, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumInterrupcionesFueraPlazo, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumInterrupcionesFueraPlazo, "Calibri", 11);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumInterrupcionesFueraPlazo, "#1659AA");
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumInterrupcionesFueraPlazo, "#FFFFFF");
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumInterrupcionesFueraPlazo);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in lista)
            {

                ws.Cells[rowData, colSuministradorNombre].Value = item.SuministradorNombre != null ? item.SuministradorNombre.Trim() : null;

                if (!item.ConMensaje)
                {
                    ws.Cells[rowData, colFechaUltimoEnvio].Value = item.FechaUltimoEnvio != null ? item.FechaUltimoEnvio.Trim() : null;
                    ws.Cells[rowData, colNumInterrupcionesEnPlazo].Value = item.NumInterrupcionesEnPlazo != null ? item.NumInterrupcionesEnPlazo.Value : item.NumInterrupcionesEnPlazo;
                    ws.Cells[rowData, colNumInterrupcionesFueraPlazo].Value = item.NumInterrupcionesFueraPlazo != null ? item.NumInterrupcionesFueraPlazo.Value : item.NumInterrupcionesFueraPlazo;
                }
                else
                {
                    ws.Cells[rowData, colFechaUltimoEnvio].Value = item.Mensaje;
                    servFormato.CeldasExcelAgrupar(ws, rowData, colFechaUltimoEnvio, rowData, colNumInterrupcionesFueraPlazo);
                }

                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colSuministradorNombre, rowData - 1, colNumInterrupcionesFueraPlazo, "Calibri", 9);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colFechaUltimoEnvio, rowData - 1, colNumInterrupcionesFueraPlazo, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colFechaUltimoEnvio, rowData - 1, colNumInterrupcionesFueraPlazo, "Centro");
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministradorNombre, rowData - 1, colNumInterrupcionesFueraPlazo);
            #endregion

            ws.Cells[rowIniTabla, colSuministradorNombre, rowData, colNumInterrupcionesFueraPlazo].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        /// <summary>
        /// Devuelve el listado de registro de cumplimiento de envio de interrupcion por periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<CumplimientoEnvioInterrupcion> ObtenerListaCumplimientoEnvioInterrupcionPorPeriodo(RePeriodoDTO periodo)
        {
            List<CumplimientoEnvioInterrupcion> lstSalida = new List<CumplimientoEnvioInterrupcion>();

            //Obtenemos todos los suministraores
            List<ReEmpresaDTO> lstSuministradores = this.ObtenerEmpresasSuministradorasTotal();

            //Obtenemos todos los envios de interrupcion, declarados sin interrupcion
            List<ReEnvioDTO> listadoEnviosConEnvios = ListarEnvios(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi, ConstantesCalculoResarcimiento.EnvioTipoInterrupcion);
            List<ReEnvioDTO> listadoEnviosDeclaradosSinInterrupcion = ListarEnvios(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi, ConstantesCalculoResarcimiento.EnvioTipoDeclaracion);

            foreach (var suministrador in lstSuministradores)
            {
                List<ReEnvioDTO> listadoEnviosPorSuministrador = listadoEnviosConEnvios.Where(x => x.Emprcodi == suministrador.Emprcodi).OrderByDescending(c => c.Reenvfecha).ToList();
                List<ReEnvioDTO> objSinInterrupcion =
                    listadoEnviosDeclaradosSinInterrupcion.Where(x => x.Emprcodi == suministrador.Emprcodi).OrderByDescending(c => c.Reenvfecha).ToList();

                //Interrupciones en plazo
                int? numEnPlazo = 0;
                if (listadoEnviosPorSuministrador.Any())
                {
                    List<ReEnvioDTO> listaEnPlazo = listadoEnviosPorSuministrador.Where(x => x.Reenvplazo == "S").ToList();
                    numEnPlazo = listaEnPlazo.Count;
                }
                else
                    numEnPlazo = null;

                //Interrupciones fuera plazo
                int? numFueraPlazo = 0;
                if (listadoEnviosPorSuministrador.Any())
                {
                    List<ReEnvioDTO> listaFueraPlazo = listadoEnviosPorSuministrador.Where(x => x.Reenvplazo == "N").ToList();
                    numFueraPlazo = listaFueraPlazo.Count;
                }
                else
                    numFueraPlazo = null;

                //Completo los registros
                CumplimientoEnvioInterrupcion cumplObj = new CumplimientoEnvioInterrupcion();

                cumplObj.SuministradorId = suministrador.Emprcodi;
                cumplObj.SuministradorNombre = suministrador.Emprnomb != null ? suministrador.Emprnomb.Trim() : "";

                if (listadoEnviosPorSuministrador.Any())
                {
                    cumplObj.ConMensaje = false;
                    cumplObj.FechaUltimoEnvio = listadoEnviosPorSuministrador.Any() ? (listadoEnviosPorSuministrador.First().ReenvfechaDesc != null ? listadoEnviosPorSuministrador.First().ReenvfechaDesc : "") : "";
                    cumplObj.NumInterrupcionesEnPlazo = numEnPlazo;
                    cumplObj.NumInterrupcionesFueraPlazo = numFueraPlazo;
                }
                else
                {
                    cumplObj.ConMensaje = true;

                    if (objSinInterrupcion.Count > 0 && objSinInterrupcion[0].Reenvindicador == "S")
                        cumplObj.Mensaje = "REPORTÓ QUE NO TIENE INTERRUPCIONES";
                    else
                        cumplObj.Mensaje = "NO CUMPLIÓ";
                }

                lstSalida.Add(cumplObj);

            }

            return lstSalida;
        }

        #endregion

        #region Reporte Cumplimiento de envío de observaciones 

        /// <summary>
        /// Genera el reporte Cumplimiento Envío Interrupción de Suministros
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="periodo"></param>
        private void GenerarArchivoExcelCumplimientoEnvíoObservaciones(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<CumplimientoEnvioObservacion> lista = new List<CumplimientoEnvioObservacion>();

            lista = ObtenerListaCumplimientoEnvioObservacionesPorPeriodo(periodo);
            //lista = new List<CumplimientoEnvioObservacion>();

            string nameWS = "Listado";
            string titulo = "Cumplimiento de envío de observaciones";
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 1;
            int rowIniTitulo = 3;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;

            ws.Row(rowIniTabla).Height = 30;

            int colAgenteNombre = colIniTable;
            int colFechaUltimoEnvio = colIniTable + 1;
            int colNumInterrupcionesConformidadRespNo = colIniTable + 2;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colNumInterrupcionesConformidadRespNo, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colNumInterrupcionesConformidadRespNo);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colNumInterrupcionesConformidadRespNo, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colNumInterrupcionesConformidadRespNo);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colNumInterrupcionesConformidadRespNo, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colNumInterrupcionesConformidadRespNo);

            ws.Cells[rowIniTabla, colAgenteNombre].Value = "Agente responsable";
            ws.Cells[rowIniTabla, colFechaUltimoEnvio].Value = "Fecha del último envío de información";
            ws.Cells[rowIniTabla, colNumInterrupcionesConformidadRespNo].Value = "Total interrupciones conformidad responsable 'No'";

            //Estilos cabecera            
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colAgenteNombre, rowIniTabla, colNumInterrupcionesConformidadRespNo, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colAgenteNombre, rowIniTabla, colNumInterrupcionesConformidadRespNo, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colAgenteNombre, rowIniTabla, colNumInterrupcionesConformidadRespNo, "Calibri", 11);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colAgenteNombre, rowIniTabla, colNumInterrupcionesConformidadRespNo, "#1659AA");
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colAgenteNombre, rowIniTabla, colNumInterrupcionesConformidadRespNo, "#FFFFFF");
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colAgenteNombre, rowIniTabla, colNumInterrupcionesConformidadRespNo);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in lista)
            {
                ws.Cells[rowData, colAgenteNombre].Value = item.AgenteResponsableNombre != null ? item.AgenteResponsableNombre.Trim() : null;
                ws.Cells[rowData, colFechaUltimoEnvio].Value = item.FechaUltimoEnvio != null ? item.FechaUltimoEnvio.Trim() : null;
                ws.Cells[rowData, colNumInterrupcionesConformidadRespNo].Value = item.NumInterrupcionesConformidadRespNo != null ? item.NumInterrupcionesConformidadRespNo.Value : item.NumInterrupcionesConformidadRespNo;

                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colAgenteNombre, rowData - 1, colNumInterrupcionesConformidadRespNo, "Calibri", 9);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colFechaUltimoEnvio, rowData - 1, colNumInterrupcionesConformidadRespNo, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colFechaUltimoEnvio, rowData - 1, colNumInterrupcionesConformidadRespNo, "Centro");
            servFormato.BorderCeldas2(ws, rowIniTabla, colAgenteNombre, rowData - 1, colNumInterrupcionesConformidadRespNo);
            #endregion

            ws.Cells[rowIniTabla, colAgenteNombre, rowData, colNumInterrupcionesConformidadRespNo].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        /// <summary>
        /// Devuelve el listado de registro de cumplimiento de envio de observaciones por periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<CumplimientoEnvioObservacion> ObtenerListaCumplimientoEnvioObservacionesPorPeriodo(RePeriodoDTO periodo)
        {
            List<CumplimientoEnvioObservacion> lstSalida = new List<CumplimientoEnvioObservacion>();

            //Obtenemos todos los responsables finales 
            List<ReEmpresaDTO> lstResponsablesFinales = ObtenerListadoGeneralAgentesResponsablesReporteCumplimiento(periodo);

            //Obtenemos todos los envios de Observacion
            List<ReEnvioDTO> listadoEnviosConEnviosObservacion = ListarEnvios(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi, ConstantesCalculoResarcimiento.EnvioTipoObservacion);

            //Obtenemos interrupciones con Conformidad responsable en NO
            List<ReInterrupcionSuministroDetDTO> listadoInterrupcionesConformidadRespNo = ObtenerListadoInterrupcionesConformidadRespEnNO(periodo);

            foreach (var empresaResponsable in lstResponsablesFinales)
            {
                //Obtenemos los envios para el responsable, ordenado desde el ultimo al inicial
                List<ReEnvioDTO> enviosPorResponsable = listadoEnviosConEnviosObservacion.Where(x => x.Emprcodi == empresaResponsable.Emprcodi).OrderByDescending(x => x.Reenvfecha).ToList();

                //Obtenemos los registros de conformidad NO para el responsable
                List<ReInterrupcionSuministroDetDTO> interrupcionEnConfNOParaResponsable = listadoInterrupcionesConformidadRespNo.Where(x => x.Emprcodi == empresaResponsable.Emprcodi).ToList();

                //Obtengo la cantidad de interrupciones con Conformidad responsable en NO
                int? num = 0;

                if (interrupcionEnConfNOParaResponsable.Any())
                {
                    var lstAgrupadaPorInterrupcion = interrupcionEnConfNOParaResponsable.GroupBy(x => x.Reintcodi).ToList();
                    num = lstAgrupadaPorInterrupcion.Count;
                }
                else
                {
                    if (enviosPorResponsable.Any())
                        num = 0;
                    else
                        num = null;
                }

                //armamos el listado
                CumplimientoEnvioObservacion objCO = new CumplimientoEnvioObservacion();
                objCO.AgenteResponsableId = empresaResponsable.Emprcodi;
                objCO.AgenteResponsableNombre = empresaResponsable.Emprnomb;
                objCO.FechaUltimoEnvio = enviosPorResponsable.Any() ? (enviosPorResponsable.First().ReenvfechaDesc != null ? enviosPorResponsable.First().ReenvfechaDesc : "") : "";
                objCO.NumInterrupcionesConformidadRespNo = num;

                lstSalida.Add(objCO);
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve listado de interrupciones cuyos responsables tienen NO en su campo Conformidad Responsable 
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<ReInterrupcionSuministroDetDTO> ObtenerListadoInterrupcionesConformidadRespEnNO(RePeriodoDTO periodo)
        {
            List<ReInterrupcionSuministroDetDTO> lstSalida = new List<ReInterrupcionSuministroDetDTO>();

            lstSalida = FactorySic.GetReInterrupcionSuministroDetRepository().GetConformidadResponsableNO(periodo.Repercodi);

            return lstSalida;
        }

        /// <summary>
        /// Obtiene los responsables finales para un periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<ReEmpresaDTO> ObtenerListadoGeneralAgentesResponsablesReporteCumplimiento(RePeriodoDTO periodo)
        {
            List<ReEmpresaDTO> lstSalida = new List<ReEmpresaDTO>();

            EstructuraInterrupcion maestros = new EstructuraInterrupcion();
            maestros.ListaEmpresa = this.ObtenerEmpresas();

            List<ReEmpresaDTO> listaResponsablesFinales = FactorySic.GetReInterrupcionSuministroDetRepository().ObtenerResponsablesFinalesPorPeriodo(periodo.Repercodi);

            foreach (var responsable in listaResponsablesFinales)
            {
                ReEmpresaDTO respEmp = maestros.ListaEmpresa.Find(x => x.Emprcodi == responsable.Emprcodi);

                responsable.Emprnomb = respEmp != null ? (respEmp.Emprnomb != null ? (respEmp.Emprnomb.Trim()) : "") : "";

                lstSalida.Add(responsable);
            }

            return lstSalida.OrderBy(x => x.Emprnomb).ToList();
        }
        #endregion

        #region Reporte Cumplimiento de envío de respuesta a observaciones 

        /// <summary>
        /// Genera el reporte Cumplimiento Envío Interrupción de Suministros
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="periodo"></param>
        private void GenerarArchivoExcelCumplimientoEnvíoRespuestaAObservaciones(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<CumplimientoEnvioRespuestaObservacion> lista = new List<CumplimientoEnvioRespuestaObservacion>();

            lista = ObtenerListaCumplimientoEnvioRespuestaObsPorPeriodo(periodo);
            //lista = new List<CumplimientoEnvioRespuestaObservacion>();  //para hacer evidencias de listado sin registros

            string nameWS = "Listado";
            string titulo = "Cumplimiento de envío de respuesta a observaciones ";
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 1;
            int rowIniTitulo = 3;


            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;



            ws.Row(rowIniTabla).Height = 30;

            int colSuministradorNombre = colIniTable;
            int colFechaUltimoEnvio = colIniTable + 1;
            int colNumObservacionesTotales = colIniTable + 2;
            int colNumObservacionesSinResponder = colIniTable + 3;


            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colNumObservacionesSinResponder, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colNumObservacionesSinResponder);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colNumObservacionesSinResponder, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colNumObservacionesSinResponder);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colNumObservacionesSinResponder, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colNumObservacionesSinResponder);

            ws.Cells[rowIniTabla, colSuministradorNombre].Value = "Suministrador";
            ws.Cells[rowIniTabla, colFechaUltimoEnvio].Value = "Fecha del último envío de información";
            ws.Cells[rowIniTabla, colNumObservacionesTotales].Value = "Total de observaciones";
            ws.Cells[rowIniTabla, colNumObservacionesSinResponder].Value = "Total de observaciones sin responder";

            //Estilos cabecera            
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumObservacionesSinResponder, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumObservacionesSinResponder, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumObservacionesSinResponder, "Calibri", 11);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumObservacionesSinResponder, "#1659AA");
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumObservacionesSinResponder, "#FFFFFF");
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colSuministradorNombre, rowIniTabla, colNumObservacionesSinResponder);


            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in lista)
            {
                ws.Cells[rowData, colSuministradorNombre].Value = item.SuministradorNombre != null ? item.SuministradorNombre.Trim() : null;

                if (item.FechaUltimoEnvio != "No Envío")
                {
                    ws.Cells[rowData, colFechaUltimoEnvio].Value = item.FechaUltimoEnvio != null ? item.FechaUltimoEnvio.Trim() : null;
                    ws.Cells[rowData, colNumObservacionesTotales].Value = item.NumObservacionesTotales != null ? item.NumObservacionesTotales.Value : item.NumObservacionesTotales;
                    ws.Cells[rowData, colNumObservacionesSinResponder].Value = item.NumObservacionesSinResponder != null ? item.NumObservacionesSinResponder.Value : item.NumObservacionesSinResponder;
                }
                else
                {
                    ws.Cells[rowData, colFechaUltimoEnvio].Value = item.FechaUltimoEnvio != null ? item.FechaUltimoEnvio.Trim() : null;
                    servFormato.CeldasExcelAgrupar(ws, rowData, colFechaUltimoEnvio, rowData, colNumObservacionesSinResponder);
                }

                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colSuministradorNombre, rowData - 1, colNumObservacionesSinResponder, "Calibri", 9);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colFechaUltimoEnvio, rowData - 1, colNumObservacionesSinResponder, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colFechaUltimoEnvio, rowData - 1, colNumObservacionesSinResponder, "Centro");
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministradorNombre, rowData - 1, colNumObservacionesSinResponder);
            #endregion


            ws.Cells[rowIniTabla, colSuministradorNombre, rowData, colNumObservacionesSinResponder].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        /// <summary>
        /// Devuelve el listado de registro de cumplimiento de envio de respuesta a observaciones por periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<CumplimientoEnvioRespuestaObservacion> ObtenerListaCumplimientoEnvioRespuestaObsPorPeriodo(RePeriodoDTO periodo)
        {
            List<CumplimientoEnvioRespuestaObservacion> lstSalida = new List<CumplimientoEnvioRespuestaObservacion>();

            //Obtenemos todos los suministraores
            List<SiEmpresaDTO> lstSuministradores = ObtenerListadoGeneralSuministradores();

            //Obtenemos todos los envios de respuesta a observaciones
            List<ReEnvioDTO> listadoEnviosConEnviosRespObs = ListarEnvios(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi, ConstantesCalculoResarcimiento.EnvioTipoRespuesta);

            //Obtengo las interrupciones por periodo
            List<ReInterrupcionSuministroDTO> lstInterrupcionesPE = FactorySic.GetReInterrupcionSuministroRepository().ObtenerPorEmpresaPeriodo(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi);

            //Obtenemos los detalles de las interrupciones por periodo
            List<ReInterrupcionSuministroDetDTO> lstDetallesInterrupcionesPE = FactorySic.GetReInterrupcionSuministroDetRepository().ObtenerPorEmpresaPeriodo(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi);
            int saaa = 0;
            foreach (var suministrador in lstSuministradores)
            {

                //Obtengo lista envios de respuesta de obs por suministrador
                List<ReEnvioDTO> lstEnviosPorSuministrador = listadoEnviosConEnviosRespObs.Where(x => x.Emprcodi == suministrador.Emprcodi).OrderByDescending(x => x.Reenvfecha).ToList();

                //Obtengo lista de reintcodis para el suministrador
                List<int> lstInterrupcionesIdPorSuministrador = lstInterrupcionesPE.Where(x => x.Emprcodi == suministrador.Emprcodi).Select(x => x.Reintcodi).Distinct().ToList();

                //Obtengo lista de interrupcionesDetalle para los reintcodis anteriores
                List<ReInterrupcionSuministroDetDTO> lstDetallesInterrupcionesPorSuministrador = lstDetallesInterrupcionesPE.Where(x => lstInterrupcionesIdPorSuministrador.Contains(x.Reintcodi.Value)).ToList();

                //Obtener Total de observaciones
                List<ReInterrupcionSuministroDetDTO> lstDetNo = lstDetallesInterrupcionesPorSuministrador.Where(x => x.Reintdconformidadresp == "N").ToList();
                List<ReInterrupcionSuministroDetDTO> lstDetSi = lstDetallesInterrupcionesPorSuministrador.Where(x => x.Reintdconformidadresp == "S").ToList();
                int numObservaciones = lstDetNo.Count + lstDetSi.Count;

                //Obtener Total obs sin responder
                List<ReInterrupcionSuministroDetDTO> lstDetObsSinResponder = lstDetNo.Where(x => x.Reintdconformidadsumi == null).ToList();
                int numObsSinResponder = lstDetObsSinResponder.Count;

                //Completamos registros
                CumplimientoEnvioRespuestaObservacion cumplObj = new CumplimientoEnvioRespuestaObservacion();

                cumplObj.SuministradorId = suministrador.Emprcodi;
                cumplObj.SuministradorNombre = suministrador.Emprnomb;
                cumplObj.FechaUltimoEnvio = lstEnviosPorSuministrador.Any() ? (lstEnviosPorSuministrador.First().ReenvfechaDesc != null ? lstEnviosPorSuministrador.First().ReenvfechaDesc : "") : "No Envío";
                cumplObj.NumObservacionesTotales = numObservaciones;
                cumplObj.NumObservacionesSinResponder = numObsSinResponder;

                lstSalida.Add(cumplObj);

            }

            return lstSalida;
        }

        #endregion

        #region Reporte Interrupciones en Fuerza Mayor

        /// <summary>
        /// Genera el reporte Interrupciones en Fuerza Mayor
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        private void GenerarArchivoExcelInterrupcionesEnFuerzaMayor(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroPE> lista = new List<InterrupcionSuministroPE>();

            //Obtengo la lista General de interrupciones por punto entrega
            List<InterrupcionSuministroPE> listaRegistrosPE = ObtenerListaInterrupcionesPEPorPeriodo(periodo, false, null);
            //listaRegistrosPE = new List<InterrupcionSuministroPE>();  //para hacer evidencias de listado sin registros

            //Filtro aquellas que tengan causa igual a FM TRAMITE
            lista = listaRegistrosPE.Where(x => x.CausaNombre.ToUpper().Trim() == "FM TRAMITE").ToList();

            string nameWS = "Listado";
            string titulo = "Interrupciones en Fuerza Mayor";
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 1;
            int rowIniTitulo = 3;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;

            int colSuministrador = colIniTable;
            int colCorrelativo = colIniTable + 1;
            int colTipoCliente = colIniTable + 2;
            int colNombreCliente = colIniTable + 3;
            int colPuntoEntregaBarraNombre = colIniTable + 4;
            int colNumSuministroClienteLibre = colIniTable + 5;
            int colNivelTensionNombre = colIniTable + 6;
            int colAplicacionLiteral = colIniTable + 7;
            int colEnergiaPeriodo = colIniTable + 8;
            int colIncrementoTolerancia = colIniTable + 9;
            int colTipoNombre = colIniTable + 10;
            int colCausaNombre = colIniTable + 11;
            int colNi = colIniTable + 12;
            int colKi = colIniTable + 13;
            int colTiempoEjecutadoIni = colIniTable + 14;
            int colTiempoEjecutadoFin = colIniTable + 15;
            int colTiempoProgramadoIni = colIniTable + 16;
            int colTiempoProgramadoFin = colIniTable + 17;
            int colResponsable1Nombre = colIniTable + 18;
            int colResponsable1Porcentaje = colIniTable + 19;
            int colResponsable2Nombre = colIniTable + 20;
            int colResponsable2Porcentaje = colIniTable + 21;
            int colResponsable3Nombre = colIniTable + 22;
            int colResponsable3Porcentaje = colIniTable + 23;
            int colResponsable4Nombre = colIniTable + 24;
            int colResponsable4Porcentaje = colIniTable + 25;
            int colResponsable5Nombre = colIniTable + 26;
            int colResponsable5Porcentaje = colIniTable + 27;
            int colCausaResumida = colIniTable + 28;
            int colEiE = colIniTable + 29;
            int colResarcimiento = colIniTable + 30;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento);

            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colCorrelativo].Value = "Correlativo por Punto de Entrega";
            ws.Cells[rowIniTabla, colTipoCliente].Value = "Tipo de Cliente";
            ws.Cells[rowIniTabla, colNombreCliente].Value = "Cliente";
            ws.Cells[rowIniTabla, colPuntoEntregaBarraNombre].Value = "Punto de Entrega/Barra";
            ws.Cells[rowIniTabla, colNumSuministroClienteLibre].Value = "N° de suministro cliente libre";
            ws.Cells[rowIniTabla, colNivelTensionNombre].Value = "Nivel de Tensión";
            ws.Cells[rowIniTabla, colAplicacionLiteral].Value = "Aplicación literal e) de numeral 5.2.4 Base Metodológica (meses de suministro en el semestre)";
            ws.Cells[rowIniTabla, colEnergiaPeriodo].Value = "Energía Semestral (kWh)";
            ws.Cells[rowIniTabla, colIncrementoTolerancia].Value = "Incremento de tolerancias Sector Distribución Típico 2 (Mercado regulado)";
            ws.Cells[rowIniTabla, colTipoNombre].Value = "Tipo";
            ws.Cells[rowIniTabla, colCausaNombre].Value = "Causa";
            ws.Cells[rowIniTabla, colNi].Value = "Ni";
            ws.Cells[rowIniTabla, colKi].Value = "Ki";
            ws.Cells[rowIniTabla, colTiempoEjecutadoIni].Value = "Tiempo Ejecutado";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colTiempoProgramadoIni].Value = "Tiempo Programado";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colResponsable1Nombre].Value = "Responsable 1";
            ws.Cells[rowIniTabla + 1, colResponsable1Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable1Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable2Nombre].Value = "Responsable 2";
            ws.Cells[rowIniTabla + 1, colResponsable2Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable2Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable3Nombre].Value = "Responsable 3";
            ws.Cells[rowIniTabla + 1, colResponsable3Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable3Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable4Nombre].Value = "Responsable 4";
            ws.Cells[rowIniTabla + 1, colResponsable4Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable4Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable5Nombre].Value = "Responsable 5";
            ws.Cells[rowIniTabla + 1, colResponsable5Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable5Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colCausaResumida].Value = "Causa resumida de interrupción";
            ws.Cells[rowIniTabla, colEiE].Value = "Ei / E";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento (US$)";



            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCorrelativo, rowIniTabla + 1, colCorrelativo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoCliente, rowIniTabla + 1, colTipoCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNombreCliente, rowIniTabla + 1, colNombreCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPuntoEntregaBarraNombre, rowIniTabla + 1, colPuntoEntregaBarraNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNumSuministroClienteLibre, rowIniTabla + 1, colNumSuministroClienteLibre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNivelTensionNombre, rowIniTabla + 1, colNivelTensionNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionLiteral, rowIniTabla + 1, colAplicacionLiteral);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEnergiaPeriodo, rowIniTabla + 1, colEnergiaPeriodo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colIncrementoTolerancia, rowIniTabla + 1, colIncrementoTolerancia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoNombre, rowIniTabla + 1, colTipoNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaNombre, rowIniTabla + 1, colCausaNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNi, rowIniTabla + 1, colNi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colKi, rowIniTabla + 1, colKi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoIni, rowIniTabla, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoProgramadoIni, rowIniTabla, colTiempoProgramadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable1Nombre, rowIniTabla, colResponsable1Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable2Nombre, rowIniTabla, colResponsable2Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable3Nombre, rowIniTabla, colResponsable3Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable4Nombre, rowIniTabla, colResponsable4Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable5Nombre, rowIniTabla, colResponsable5Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaResumida, rowIniTabla + 1, colCausaResumida);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEiE, rowIniTabla + 1, colEiE);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResarcimiento, rowIniTabla + 1, colResarcimiento);

            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "#1659AA");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento);
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "#FFFFFF");
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento);


            #endregion


            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in lista)
            {
                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colCorrelativo].Value = item.Correlativo != null ? item.Correlativo.Value : item.Correlativo;
                ws.Cells[rowData, colTipoCliente].Value = item.TipoCliente != null ? item.TipoCliente.Trim() : null;
                ws.Cells[rowData, colNombreCliente].Value = item.NombreCliente != null ? item.NombreCliente.Trim() : null;
                ws.Cells[rowData, colPuntoEntregaBarraNombre].Value = item.PuntoEntregaBarraNombre != null ? item.PuntoEntregaBarraNombre.Trim() : null;
                ws.Cells[rowData, colNumSuministroClienteLibre].Value = item.NumSuministroClienteLibre != null ? item.NumSuministroClienteLibre : item.NumSuministroClienteLibre;
                ws.Cells[rowData, colNivelTensionNombre].Value = item.NivelTensionNombre != null ? item.NivelTensionNombre.Trim() : null;
                ws.Cells[rowData, colAplicacionLiteral].Value = item.AplicacionLiteral != null ? item.AplicacionLiteral.Value : item.AplicacionLiteral;
                ws.Cells[rowData, colEnergiaPeriodo].Value = item.EnergiaPeriodo != null ? item.EnergiaPeriodo.Value : item.EnergiaPeriodo;
                ws.Cells[rowData, colEnergiaPeriodo].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colIncrementoTolerancia].Value = item.IncrementoTolerancia != null ? item.IncrementoTolerancia.Trim() : null;
                ws.Cells[rowData, colTipoNombre].Value = item.TipoNombre != null ? item.TipoNombre.Trim() : null;
                ws.Cells[rowData, colCausaNombre].Value = item.CausaNombre != null ? item.CausaNombre.Trim() : null;
                ws.Cells[rowData, colNi].Value = item.Ni != null ? item.Ni.Value : item.Ni;
                ws.Cells[rowData, colNi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colKi].Value = item.Ki != null ? item.Ki.Value : item.Ki;
                ws.Cells[rowData, colKi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colTiempoEjecutadoIni].Value = item.TiempoEjecutadoIni != null ? item.TiempoEjecutadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoIni].Value = item.TiempoProgramadoIni != null ? item.TiempoProgramadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoFin].Value = item.TiempoProgramadoFin != null ? item.TiempoProgramadoFin.Trim() : null;
                ws.Cells[rowData, colResponsable1Nombre].Value = item.Responsable1Nombre != null ? item.Responsable1Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable1Porcentaje].Value = item.Responsable1Porcentaje != null ? item.Responsable1Porcentaje.Value / 100 : item.Responsable1Porcentaje;
                ws.Cells[rowData, colResponsable1Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable2Nombre].Value = item.Responsable2Nombre != null ? item.Responsable2Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable2Porcentaje].Value = item.Responsable2Porcentaje != null ? item.Responsable2Porcentaje.Value / 100 : item.Responsable2Porcentaje;
                ws.Cells[rowData, colResponsable2Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable3Nombre].Value = item.Responsable3Nombre != null ? item.Responsable3Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable3Porcentaje].Value = item.Responsable3Porcentaje != null ? item.Responsable3Porcentaje.Value / 100 : item.Responsable3Porcentaje;
                ws.Cells[rowData, colResponsable3Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable4Nombre].Value = item.Responsable4Nombre != null ? item.Responsable4Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable4Porcentaje].Value = item.Responsable4Porcentaje != null ? item.Responsable4Porcentaje.Value / 100 : item.Responsable4Porcentaje;
                ws.Cells[rowData, colResponsable4Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable5Nombre].Value = item.Responsable5Nombre != null ? item.Responsable5Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable5Porcentaje].Value = item.Responsable5Porcentaje != null ? item.Responsable5Porcentaje.Value / 100 : item.Responsable5Porcentaje;
                ws.Cells[rowData, colResponsable5Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colCausaResumida].Value = item.CausaResumida != null ? item.CausaResumida.Trim() : null;
                ws.Cells[rowData, colEiE].Value = item.EiE != null ? item.EiE.Value : item.EiE;
                ws.Cells[rowData, colEiE].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResarcimiento].Value = item.Resarcimiento != null ? item.Resarcimiento.Value : item.Resarcimiento;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = FormatoNumDecimales(2);

                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colSuministrador, rowData - 1, colResarcimiento, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministrador, rowData - 1, colResarcimiento);
            #endregion

            //filter                       

            if (lista.Any())
            {
                ws.Cells[rowIniTabla, colSuministrador, rowData, colResarcimiento].AutoFitColumns();
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 14;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 18;
                ws.Column(colTiempoEjecutadoFin).Width = 18;
                ws.Column(colTiempoProgramadoIni).Width = 18;
                ws.Column(colTiempoProgramadoFin).Width = 18;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
            }
            else
            {
                ws.Column(colSuministrador).Width = 20;
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNombreCliente).Width = 12;
                ws.Column(colPuntoEntregaBarraNombre).Width = 22;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 12;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colTipoNombre).Width = 10;
                ws.Column(colCausaNombre).Width = 10;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 13;
                ws.Column(colTiempoEjecutadoFin).Width = 12;
                ws.Column(colTiempoProgramadoIni).Width = 13;
                ws.Column(colTiempoProgramadoFin).Width = 12;
                ws.Column(colResponsable1Nombre).Width = 8;
                ws.Column(colResponsable1Porcentaje).Width = 5;
                ws.Column(colResponsable2Nombre).Width = 8;
                ws.Column(colResponsable2Porcentaje).Width = 5;
                ws.Column(colResponsable3Nombre).Width = 8;
                ws.Column(colResponsable3Porcentaje).Width = 5;
                ws.Column(colResponsable4Nombre).Width = 8;
                ws.Column(colResponsable4Porcentaje).Width = 5;
                ws.Column(colResponsable5Nombre).Width = 8;
                ws.Column(colResponsable5Porcentaje).Width = 5;
                ws.Column(colCausaResumida).Width = 27;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }

        #endregion

        #region Reporte Contraste de interrupciones de suministro

        /// <summary>
        /// Genera el reporte  Contraste de interrupciones de suministro
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="periodo"></param>
        public void GenerarArchivoExcelContrasteInterrupcionesSuministro(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo, List<InterrupcionSuministroPE> listaDatos)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroPE> lista = new List<InterrupcionSuministroPE>();

            lista = listaDatos == null ? ObtenerListadoContrasteInterrupcionesPorSuministrador(periodo, null) : listaDatos;

            string nameWS = "Listado";
            string titulo = "Contraste de Interrupciones de Suministro y Pendientes de Reportar";
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 1;
            int rowIniTitulo = 3;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;

            int colPuntoEntrega = colIniTable;
            int colFechaIniInterrupcion = colIniTable + 1;
            int colSuministrador = colIniTable + 2;
            int colTipoInterrupcion = colIniTable + 3;
            int colCausaInterrupcion = colIniTable + 4;
            int colTiempoEjecutadoFin = colIniTable + 5;
            int colTiempoProgramadoIni = colIniTable + 6;
            int colTiempoProgramadoFin = colIniTable + 7;
            int colResponsable1Nombre = colIniTable + 8;
            int colResponsable1Porcentaje = colIniTable + 9;
            int colResponsable2Nombre = colIniTable + 10;
            int colResponsable2Porcentaje = colIniTable + 11;
            int colResponsable3Nombre = colIniTable + 12;
            int colResponsable3Porcentaje = colIniTable + 13;
            int colResponsable4Nombre = colIniTable + 14;
            int colResponsable4Porcentaje = colIniTable + 15;
            int colResponsable5Nombre = colIniTable + 16;
            int colResponsable5Porcentaje = colIniTable + 17;
            int colObs = colIniTable + 18;
            int colContraste = colIniTable + 19;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colContraste, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colContraste);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colContraste, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colContraste);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colContraste, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colContraste);

            ws.Cells[rowIniTabla, colPuntoEntrega].Value = "Punto de Entrega";
            ws.Cells[rowIniTabla, colFechaIniInterrupcion].Value = "Fecha hora Inicio de interrupción";
            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colTipoInterrupcion].Value = "Tipo de Interrupción";
            ws.Cells[rowIniTabla, colCausaInterrupcion].Value = "Causa de Interrupción";
            ws.Cells[rowIniTabla, colTiempoEjecutadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colTiempoProgramadoIni].Value = "Tiempo Programado";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoIni].Value = "Fecha hora de inicio";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoFin].Value = "Fecha hora de fin";
            ws.Cells[rowIniTabla, colResponsable1Nombre].Value = "Responsable 1";
            ws.Cells[rowIniTabla + 1, colResponsable1Nombre].Value = "Responsable";
            ws.Cells[rowIniTabla + 1, colResponsable1Porcentaje].Value = "% Resp";
            ws.Cells[rowIniTabla, colResponsable2Nombre].Value = "Responsable 2";
            ws.Cells[rowIniTabla + 1, colResponsable2Nombre].Value = "Responsable";
            ws.Cells[rowIniTabla + 1, colResponsable2Porcentaje].Value = "% Resp";
            ws.Cells[rowIniTabla, colResponsable3Nombre].Value = "Responsable 3";
            ws.Cells[rowIniTabla + 1, colResponsable3Nombre].Value = "Responsable";
            ws.Cells[rowIniTabla + 1, colResponsable3Porcentaje].Value = "% Resp";
            ws.Cells[rowIniTabla, colResponsable4Nombre].Value = "Responsable 4";
            ws.Cells[rowIniTabla + 1, colResponsable4Nombre].Value = "Responsable";
            ws.Cells[rowIniTabla + 1, colResponsable4Porcentaje].Value = "% Resp";
            ws.Cells[rowIniTabla, colResponsable5Nombre].Value = "Responsable 5";
            ws.Cells[rowIniTabla + 1, colResponsable5Nombre].Value = "Responsable";
            ws.Cells[rowIniTabla + 1, colResponsable5Porcentaje].Value = "% Resp";
            ws.Cells[rowIniTabla, colObs].Value = "Observación";
            ws.Cells[rowIniTabla, colContraste].Value = "Campos Diferentes";

            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPuntoEntrega, rowIniTabla + 1, colPuntoEntrega);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colFechaIniInterrupcion, rowIniTabla + 1, colFechaIniInterrupcion);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoInterrupcion, rowIniTabla + 1, colTipoInterrupcion);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoFin, rowIniTabla + 1, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaInterrupcion, rowIniTabla + 1, colCausaInterrupcion);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colObs, rowIniTabla + 1, colObs);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colContraste, rowIniTabla + 1, colContraste);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoProgramadoIni, rowIniTabla, colTiempoProgramadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable1Nombre, rowIniTabla, colResponsable1Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable2Nombre, rowIniTabla, colResponsable2Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable3Nombre, rowIniTabla, colResponsable3Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable4Nombre, rowIniTabla, colResponsable4Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable5Nombre, rowIniTabla, colResponsable5Porcentaje);

            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colPuntoEntrega, rowIniTabla + 1, colContraste, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colPuntoEntrega, rowIniTabla + 1, colContraste, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colPuntoEntrega, rowIniTabla + 1, colContraste, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colPuntoEntrega, rowIniTabla + 1, colContraste, "#1659AA");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colPuntoEntrega, rowIniTabla + 1, colContraste);
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colPuntoEntrega, rowIniTabla + 1, colContraste, "#FFFFFF");
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colPuntoEntrega, rowIniTabla + 1, colContraste);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in lista)
            {
                ws.Cells[rowData, colPuntoEntrega].Value = item.PuntoEntregaBarraNombre != null ? item.PuntoEntregaBarraNombre.Trim() : null;
                ws.Cells[rowData, colFechaIniInterrupcion].Value = item.FechaEjecIniMinuto != null ? item.FechaEjecIniMinutoDesc.Trim() : null;
                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colTipoInterrupcion].Value = item.TipoNombre != null ? item.TipoNombre.Trim() : null;
                ws.Cells[rowData, colCausaInterrupcion].Value = item.CausaNombre != null ? item.CausaNombre.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoIni].Value = item.FechaProgramadoIniMinutoDesc != null ? item.FechaProgramadoIniMinutoDesc.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoFin].Value = item.FechaProgramadoFinMinutoDesc != null ? item.FechaProgramadoFinMinutoDesc.Trim() : null;

                ws.Cells[rowData, colResponsable1Nombre].Value = item.Responsable1Nombre != null ? item.Responsable1Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable1Porcentaje].Value = item.Responsable1Porcentaje != null ? item.Responsable1Porcentaje.Value / 100 : item.Responsable1Porcentaje;
                ws.Cells[rowData, colResponsable1Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable2Nombre].Value = item.Responsable2Nombre != null ? item.Responsable2Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable2Porcentaje].Value = item.Responsable2Porcentaje != null ? item.Responsable2Porcentaje.Value / 100 : item.Responsable2Porcentaje;
                ws.Cells[rowData, colResponsable2Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable3Nombre].Value = item.Responsable3Nombre != null ? item.Responsable3Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable3Porcentaje].Value = item.Responsable3Porcentaje != null ? item.Responsable3Porcentaje.Value / 100 : item.Responsable3Porcentaje;
                ws.Cells[rowData, colResponsable3Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable4Nombre].Value = item.Responsable4Nombre != null ? item.Responsable4Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable4Porcentaje].Value = item.Responsable4Porcentaje != null ? item.Responsable4Porcentaje.Value / 100 : item.Responsable4Porcentaje;
                ws.Cells[rowData, colResponsable4Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable5Nombre].Value = item.Responsable5Nombre != null ? item.Responsable5Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable5Porcentaje].Value = item.Responsable5Porcentaje != null ? item.Responsable5Porcentaje.Value / 100 : item.Responsable5Porcentaje;
                ws.Cells[rowData, colResponsable5Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colObs].Value = item.Observacion != null ? item.Observacion.Trim() : null;
                ws.Cells[rowData, colContraste].Value = item.CamposContraste != null ? item.CamposContraste.Trim() : null;

                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colPuntoEntrega, rowData - 1, colContraste, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colPuntoEntrega, rowData - 1, colContraste);
            #endregion

            //filter                       
            if (lista.Any())
            {
                ws.Cells[rowIniTabla, colPuntoEntrega, rowData, colContraste].AutoFitColumns();

            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }



        /// <summary>
        /// Devuelve el listado del reporte contraste de interrupciones para un periodo y suministrador
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="idSuministrador"></param>
        /// <returns></returns>
        public List<InterrupcionSuministroPE> ObtenerListadoContrasteInterrupcionesPorSuministrador(RePeriodoDTO periodo, int? idSuministrador)
        {
            List<InterrupcionSuministroPE> lstSalida = new List<InterrupcionSuministroPE>();
            List<ReEmpresaDTO> lstSumTotales = ObtenerEmpresasSuministradorasTotal();

            #region CASO 1 contraste sobre lo reportado por agentes (PE)
            //Obtengo las interrupciones por periodo
            List<InterrupcionSuministroPE> listaRegistrosPETotal = ObtenerListaInterrupcionesPEPorPeriodo(periodo, false, null);
            List<InterrupcionSuministroPE> listaRegistrosPEXSum = idSuministrador == null ? listaRegistrosPETotal : listaRegistrosPETotal.Where(x => x.SuministradorId == idSuministrador).ToList();

            //Para el caso 1, SIEMPRE se usa todas las interrupciones asi haya idSuministrador o no, el filtro se hace al final
            List<InterrupcionSuministroPE> listaRegistrosPERegulado = listaRegistrosPETotal.Where(x => x.TipoCliente.Trim() == "Regulado").ToList();

            //agrupo por punto entrega y fecIni (hasta minuto)
            var lista = listaRegistrosPERegulado.GroupBy(x => new { x.PuntoEntregaBarraNombre, x.FechaEjecIniMinuto }).ToList();

            foreach (var item in lista)
            {
                //agrupo por punto entrega y fecIni (hasta minuto)
                var lista2 = item.GroupBy(x => new
                {
                    x.InterrupcionTipoId,
                    x.FechaEjecFin,
                    x.Responsable1Id,
                    x.Responsable1Porcentaje,
                    x.Responsable2Id,
                    x.Responsable2Porcentaje,
                    x.Responsable3Id,
                    x.Responsable3Porcentaje,
                    x.Responsable4Id,
                    x.Responsable4Porcentaje,
                    x.Responsable5Id,
                    x.Responsable5Porcentaje,
                    x.CausaId,
                    x.FechaProgramadoIniMinuto,
                    x.FechaProgramadoFinMinuto
                }).ToList();

                //Si hay almenos 2 filas diferentes, TODOS deben mostrarse en el listado contraste
                if (lista2.Count > 1)
                { //Si no son iguales, Muestro todos los registros 

                    //Para obtener columna Observacion, veo el motivo por el cual se contrasta
                    List<InterrupcionSuministroPE> lstComparacion = new List<InterrupcionSuministroPE>();
                    foreach (var reg2 in lista2)
                    {
                        InterrupcionSuministroPE newReg = new InterrupcionSuministroPE();
                        newReg.InterrupcionTipoId = reg2.Key.InterrupcionTipoId;
                        newReg.FechaEjecFin = reg2.Key.FechaEjecFin;
                        newReg.Responsable1Id = reg2.Key.Responsable1Id;
                        newReg.Responsable1Porcentaje = reg2.Key.Responsable1Porcentaje;
                        newReg.Responsable2Id = reg2.Key.Responsable2Id;
                        newReg.Responsable2Porcentaje = reg2.Key.Responsable2Porcentaje;
                        newReg.Responsable3Id = reg2.Key.Responsable3Id;
                        newReg.Responsable3Porcentaje = reg2.Key.Responsable3Porcentaje;
                        newReg.Responsable4Id = reg2.Key.Responsable4Id;
                        newReg.Responsable4Porcentaje = reg2.Key.Responsable4Porcentaje;
                        newReg.Responsable5Id = reg2.Key.Responsable5Id;
                        newReg.Responsable5Porcentaje = reg2.Key.Responsable5Porcentaje;
                        newReg.CausaId = reg2.Key.CausaId;
                        newReg.FechaProgramadoIniMinuto = reg2.Key.FechaProgramadoIniMinuto;
                        newReg.FechaProgramadoFinMinuto = reg2.Key.FechaProgramadoFinMinuto;

                        lstComparacion.Add(newReg);
                    }

                    List<int?> lstTipoInterrupcion = lstComparacion.Select(x => x.InterrupcionTipoId).Distinct().ToList();
                    List<DateTime?> lstFechaFin = lstComparacion.Select(x => x.FechaEjecFin).Distinct().ToList();
                    List<int?> lstResp1Id = lstComparacion.Select(x => x.Responsable1Id).Distinct().ToList();
                    List<int?> lstResp2Id = lstComparacion.Select(x => x.Responsable2Id).Distinct().ToList();
                    List<int?> lstResp3Id = lstComparacion.Select(x => x.Responsable3Id).Distinct().ToList();
                    List<int?> lstResp4Id = lstComparacion.Select(x => x.Responsable4Id).Distinct().ToList();
                    List<int?> lstResp5Id = lstComparacion.Select(x => x.Responsable5Id).Distinct().ToList();
                    List<decimal?> lstPorcentaje1 = lstComparacion.Select(x => x.Responsable1Porcentaje).Distinct().ToList();
                    List<decimal?> lstPorcentaje2 = lstComparacion.Select(x => x.Responsable2Porcentaje).Distinct().ToList();
                    List<decimal?> lstPorcentaje3 = lstComparacion.Select(x => x.Responsable3Porcentaje).Distinct().ToList();
                    List<decimal?> lstPorcentaje4 = lstComparacion.Select(x => x.Responsable4Porcentaje).Distinct().ToList();
                    List<decimal?> lstPorcentaje5 = lstComparacion.Select(x => x.Responsable5Porcentaje).Distinct().ToList();
                    List<int?> lstCausaId = lstComparacion.Select(x => x.CausaId).Distinct().ToList();
                    List<DateTime?> lstProgIni = lstComparacion.Select(x => x.FechaProgramadoIniMinuto).Distinct().ToList();
                    List<DateTime?> lstProgFin = lstComparacion.Select(x => x.FechaProgramadoFinMinuto).Distinct().ToList();

                    List<string> lstamposDiferentes = new List<string>();
                    if (lstTipoInterrupcion.Count > 1) lstamposDiferentes.Add("Tipo Interrupción");
                    if (lstFechaFin.Count > 1) lstamposDiferentes.Add("Tiempo ejecutado - Fecha hora de fin ");
                    if (lstResp1Id.Count > 1) lstamposDiferentes.Add("Responsable 1");
                    if (lstResp2Id.Count > 1) lstamposDiferentes.Add("Responsable 2");
                    if (lstResp3Id.Count > 1) lstamposDiferentes.Add("Responsable 3");
                    if (lstResp4Id.Count > 1) lstamposDiferentes.Add("Responsable 4");
                    if (lstResp5Id.Count > 1) lstamposDiferentes.Add("Responsable 5");
                    if (lstPorcentaje1.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 1");
                    if (lstPorcentaje2.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 2");
                    if (lstPorcentaje3.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 3");
                    if (lstPorcentaje4.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 4");
                    if (lstPorcentaje5.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 5");
                    if (lstCausaId.Count > 1) lstamposDiferentes.Add("Causa de Interrupción");
                    if (lstProgIni.Count > 1) lstamposDiferentes.Add("Tiempo Programado - Fecha hora de inicio");
                    if (lstProgFin.Count > 1) lstamposDiferentes.Add("Tiempo Programado - Fecha hora de fin");

                    //Aqui hago el filtro si existe o no idSuministrador
                    if (idSuministrador == null)
                    {
                        foreach (var reg in item)
                        {
                            reg.FechaProgramadoIniMinutoDesc = reg.FechaProgramadoIniMinuto != null ? (reg.FechaProgramadoIniMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";
                            reg.FechaProgramadoFinMinutoDesc = reg.FechaProgramadoFinMinuto != null ? (reg.FechaProgramadoFinMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";
                            reg.Observacion = "Contraste de interrupciones regulados, existen diferencias en la calificación en los siguientes campos:";
                            reg.CamposContraste = lstamposDiferentes.Any() ? (String.Join(", ", lstamposDiferentes)) : "";
                            lstSalida.Add(reg);
                        }
                    }
                    else
                    {
                        bool existeSuministradorEnGrupo = item.Where(x => x.SuministradorId == idSuministrador).Any();

                        if (existeSuministradorEnGrupo)
                        {
                            foreach (var reg in item)
                            {
                                reg.FechaProgramadoIniMinutoDesc = reg.FechaProgramadoIniMinuto != null ? (reg.FechaProgramadoIniMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";
                                reg.FechaProgramadoFinMinutoDesc = reg.FechaProgramadoFinMinuto != null ? (reg.FechaProgramadoFinMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";
                                reg.Observacion = "Contraste de interrupciones regulados, existen diferencias en la calificación en los siguientes campos:";
                                reg.CamposContraste = lstamposDiferentes.Any() ? (String.Join(", ", lstamposDiferentes)) : "";
                                lstSalida.Add(reg);
                            }
                        }
                    }

                }
            }
            #endregion

            #region CASO 2: Si la interrupcion fue o no reportado
            //Para el CASO 2: Busco los no reportados por los agentes, para ello busco lo de osinerming en lo reportado por agentes (en base a 3 campos: "Suministrador", "PE" y "Tiempo Ejec Inicio" (hasta minutos) )

            //Agrupo por PE y FecIniEjecutado a los reportado en insumos (intrrupciones osinerming)
            List<ReInterrupcionInsumoDTO> lstIntOsin = FactorySic.GetReInterrupcionInsumoRepository().ObtenerPorPeriodo(periodo.Repercodi);
            List<ReInterrupcionInsumoDTO> lstIntOsinXSum = idSuministrador == null ? lstIntOsin : lstIntOsin.Where(x => x.Reininsuministrador == idSuministrador).ToList();
            List<ReInterrupcionInsumoDTO> lstInterrupcionesOsinermingFinal = FormatearInterrupcionesOsinerming(lstIntOsinXSum, periodo.Repercodi);

            //Obtengo lo reportado por agentes para todos los suministradores
            List<InterrupcionSuministroPE> listaRegPEXSum = idSuministrador == null ? listaRegistrosPEXSum : listaRegistrosPEXSum.Where(x => x.SuministradorId == idSuministrador).ToList();
            //var listaPETotGruposSumPEyFI = listaRegPEXSum.GroupBy(x => new { x.PuntoEntregaBarraNombre, x.FechaEjecIniMinuto }).ToList();

            List<InterrupcionSuministroPE> listadoNoReportados = new List<InterrupcionSuministroPE>();
            if (idSuministrador == null) //si busca para todas las emrpesas
            {
                var listaOsiGruposSumPEyFI = lstInterrupcionesOsinermingFinal.GroupBy(x => new { x.Reininsuministrador, x.PuntoEntregaBarraNombre, x.ReininifecinicioMinuto }).ToList();
                var listaPETotGruposSumPEyFI = listaRegPEXSum.GroupBy(x => new { x.SuministradorId, x.PuntoEntregaBarraNombre, x.FechaEjecIniMinuto }).ToList();

                //Ahora comparo
                foreach (var reportadoInsumo in listaOsiGruposSumPEyFI)
                {
                    //ReInterrupcionInsumoDTO registro = lstInterrupcionesOsinermingXSum.First();

                    int? idSum = reportadoInsumo.Key.Reininsuministrador;
                    string barra = reportadoInsumo.Key.PuntoEntregaBarraNombre;
                    DateTime? horaIniEjec = reportadoInsumo.Key.ReininifecinicioMinuto;
                    ReInterrupcionInsumoDTO regS = lstInterrupcionesOsinermingFinal.Find(x => x.Reininsuministrador == idSum.Value);

                    var regExisteEnRepAgente = listaPETotGruposSumPEyFI.Where(x => x.Key.SuministradorId == idSum && x.Key.PuntoEntregaBarraNombre == barra && x.Key.FechaEjecIniMinuto == horaIniEjec).ToList();

                    //si no esta reportado por los agentesm entran al reporte como NO REPORTADOS
                    if (!regExisteEnRepAgente.Any())
                    {
                        foreach (var regO1 in reportadoInsumo)
                        {
                            InterrupcionSuministroPE newReg = new InterrupcionSuministroPE();
                            newReg.SuministradorId = regO1.Reininsuministrador.Value;
                            newReg.InterrupcionTipoId = regO1.InterrupcionTipoId;
                            newReg.FechaEjecFin = regO1.ReininfecfinMinuto;
                            newReg.Responsable1Id = regO1.Reininresponsable1;
                            newReg.Responsable1Porcentaje = regO1.Reininporcentaje1;
                            newReg.Responsable2Id = regO1.Reininresponsable2;
                            newReg.Responsable2Porcentaje = regO1.Reininporcentaje2;
                            newReg.Responsable3Id = regO1.Reininresponsable3;
                            newReg.Responsable3Porcentaje = regO1.Reininporcentaje3;
                            newReg.Responsable4Id = regO1.Reininresponsable4;
                            newReg.Responsable4Porcentaje = regO1.Reininporcentaje4;
                            newReg.Responsable5Id = regO1.Reininresponsable5;
                            newReg.Responsable5Porcentaje = regO1.Reininporcentaje5;
                            newReg.CausaId = regO1.Recintcodi;
                            newReg.FechaProgramadoIniMinuto = regO1.ReininprogifecinicioMinuto;
                            newReg.FechaProgramadoFinMinuto = regO1.ReininprogfecfinMinuto;

                            newReg.PuntoEntregaBarraNombre = barra;
                            newReg.FechaEjecIniMinuto = regO1.ReininifecinicioMinuto;
                            newReg.FechaEjecIniMinutoDesc = regO1.ReininifecinicioMinutoDesc;
                            newReg.Suministrador = regS.Suministrador;
                            newReg.TipoNombre = regO1.TipoNombre;
                            newReg.CausaNombre = regO1.Reninicausa;
                            newReg.TiempoEjecutadoFin = regO1.ReininfecfinMinuto != null ? regO1.ReininfecfinMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : "";
                            newReg.Responsable1Nombre = regO1.Responsable1Nombre;
                            newReg.Responsable1Porcentaje = regO1.Responsable1Porcentaje;
                            newReg.Responsable2Nombre = regO1.Responsable2Nombre;
                            newReg.Responsable2Porcentaje = regO1.Responsable2Porcentaje;
                            newReg.Responsable3Nombre = regO1.Responsable3Nombre;
                            newReg.Responsable3Porcentaje = regO1.Responsable3Porcentaje;
                            newReg.Responsable4Nombre = regO1.Responsable4Nombre;
                            newReg.Responsable4Porcentaje = regO1.Responsable4Porcentaje;
                            newReg.Responsable5Nombre = regO1.Responsable5Nombre;
                            newReg.Responsable5Porcentaje = regO1.Responsable5Porcentaje;
                            newReg.FechaProgramadoIniMinutoDesc = regO1.ReininprogifecinicioMinuto != null ? (regO1.ReininprogifecinicioMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";
                            newReg.FechaProgramadoFinMinutoDesc = regO1.ReininprogfecfinMinuto != null ? (regO1.ReininprogfecfinMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";

                            newReg.Observacion = "Interrupción Osinergmin no reportada por los agentes.";

                            listadoNoReportados.Add(newReg);

                            lstSalida.AddRange(listadoNoReportados);
                        }

                    }
                    else //si existe simulitudes de los reportado en insumos en lo reportado por los agentes, hago mas comparaciones para buscar los constrastados
                    {
                        //Obtengo todos los suministradores PE y OSI
                        List<int> lstSumPE = listaRegPEXSum.Select(X => X.SuministradorId).ToList();
                        List<int> lstSumOSI = lstInterrupcionesOsinermingFinal.Where(x => x.Reininsuministrador != null).Select(X => X.Reininsuministrador.Value).ToList();
                        List<int> lstSumFinal = new List<int>();
                        lstSumFinal.AddRange(lstSumOSI);
                        lstSumFinal.AddRange(lstSumPE);
                        lstSumFinal = lstSumFinal.Distinct().ToList();

                        foreach (int idSumi in lstSumFinal)
                        {
                            //busco los grupos por todos los campos que se deben comparar para lo reportado por agentes
                            List<InterrupcionSuministroPE> lstPE_S_PE_FI = listaRegPEXSum.Where(x => x.SuministradorId == idSumi && x.PuntoEntregaBarraNombre == barra && x.FechaEjecIniMinuto == horaIniEjec).ToList();
                            var gruposPEXCamposCompletos = lstPE_S_PE_FI.GroupBy(x => new
                            {
                                x.InterrupcionTipoId,
                                x.FechaEjecFin,
                                x.Responsable1Id,
                                x.Responsable1Porcentaje,
                                x.Responsable2Id,
                                x.Responsable2Porcentaje,
                                x.Responsable3Id,
                                x.Responsable3Porcentaje,
                                x.Responsable4Id,
                                x.Responsable4Porcentaje,
                                x.Responsable5Id,
                                x.Responsable5Porcentaje,
                                x.CausaId,
                                x.FechaProgramadoIniMinuto,
                                x.FechaProgramadoFinMinuto
                            }).ToList();


                            //busco los grupos por todos los campos que se deben comparar para lo reportado en insumos
                            List<ReInterrupcionInsumoDTO> lstOSI_S_PE_FI = lstInterrupcionesOsinermingFinal.Where(x => x.Reininsuministrador == idSumi && x.PuntoEntregaBarraNombre == barra && x.ReininifecinicioMinuto == horaIniEjec).ToList();
                            var gruposOSIXCamposCompletos = lstOSI_S_PE_FI.GroupBy(x => new
                            {
                                x.InterrupcionTipoId,
                                x.ReininfecfinMinuto,
                                x.Reininresponsable1,
                                x.Reininporcentaje1,
                                x.Reininresponsable2,
                                x.Reininporcentaje2,
                                x.Reininresponsable3,
                                x.Reininporcentaje3,
                                x.Reininresponsable4,
                                x.Reininporcentaje4,
                                x.Reininresponsable5,
                                x.Reininporcentaje5,
                                x.Recintcodi,
                                x.ReininprogifecinicioMinuto,
                                x.ReininprogfecfinMinuto
                            }).ToList();

                            foreach (var reportadoPE in gruposPEXCamposCompletos)
                            {
                                var regExisteEnOSI = gruposOSIXCamposCompletos.Where(x =>
                                   x.Key.InterrupcionTipoId == reportadoPE.Key.InterrupcionTipoId &&
                                   x.Key.ReininfecfinMinuto == reportadoPE.Key.FechaEjecFin &&
                                   x.Key.Reininresponsable1 == reportadoPE.Key.Responsable1Id &&
                                   x.Key.Reininporcentaje1 == reportadoPE.Key.Responsable1Porcentaje &&
                                   x.Key.Reininresponsable2 == reportadoPE.Key.Responsable2Id &&
                                   x.Key.Reininporcentaje2 == reportadoPE.Key.Responsable2Porcentaje &&
                                   x.Key.Reininresponsable3 == reportadoPE.Key.Responsable3Id &&
                                   x.Key.Reininporcentaje3 == reportadoPE.Key.Responsable3Porcentaje &&
                                   x.Key.Reininresponsable4 == reportadoPE.Key.Responsable4Id &&
                                   x.Key.Reininporcentaje4 == reportadoPE.Key.Responsable4Porcentaje &&
                                   x.Key.Reininresponsable5 == reportadoPE.Key.Responsable5Id &&
                                   x.Key.Reininporcentaje5 == reportadoPE.Key.Responsable5Porcentaje &&
                                   x.Key.Recintcodi == reportadoPE.Key.CausaId &&
                                   x.Key.ReininprogifecinicioMinuto == reportadoPE.Key.FechaProgramadoIniMinuto &&
                                   x.Key.ReininprogfecfinMinuto == reportadoPE.Key.FechaProgramadoFinMinuto
                                   ).ToList();

                                //si lo de osinerming no coincide en lo reportado por agentes, busco las diferencias
                                if (!regExisteEnOSI.Any())
                                {
                                    List<InterrupcionSuministroPE> lstComparacion = new List<InterrupcionSuministroPE>();
                                    foreach (var newReg in reportadoPE)
                                    {
                                        //newReg.TipoObservacion = "Interrupción Osinerming que fue reportado por los agentes pero existe diferencias con otros";
                                        newReg.Observacion = "Interrupción reportada por agentes pero existe contraste dado que hay diferencias en la calificación con respecto a las interrupciones Osinegmin en los siguientes campos:";
                                        newReg.FechaProgramadoIniMinutoDesc = newReg.FechaProgramadoIniMinuto != null ? (newReg.FechaProgramadoIniMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";
                                        newReg.FechaProgramadoFinMinutoDesc = newReg.FechaProgramadoFinMinuto != null ? (newReg.FechaProgramadoFinMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";

                                        lstComparacion.Add(newReg);
                                    }

                                    List<int?> lstTipoInterrupcion = lstComparacion.Select(x => x.InterrupcionTipoId).Distinct().ToList();
                                    List<DateTime?> lstFechaFin = lstComparacion.Select(x => x.FechaEjecFin).Distinct().ToList();
                                    List<int?> lstResp1Id = lstComparacion.Select(x => x.Responsable1Id).Distinct().ToList();
                                    List<int?> lstResp2Id = lstComparacion.Select(x => x.Responsable2Id).Distinct().ToList();
                                    List<int?> lstResp3Id = lstComparacion.Select(x => x.Responsable3Id).Distinct().ToList();
                                    List<int?> lstResp4Id = lstComparacion.Select(x => x.Responsable4Id).Distinct().ToList();
                                    List<int?> lstResp5Id = lstComparacion.Select(x => x.Responsable5Id).Distinct().ToList();
                                    List<decimal?> lstPorcentaje1 = lstComparacion.Select(x => x.Responsable1Porcentaje).Distinct().ToList();
                                    List<decimal?> lstPorcentaje2 = lstComparacion.Select(x => x.Responsable2Porcentaje).Distinct().ToList();
                                    List<decimal?> lstPorcentaje3 = lstComparacion.Select(x => x.Responsable3Porcentaje).Distinct().ToList();
                                    List<decimal?> lstPorcentaje4 = lstComparacion.Select(x => x.Responsable4Porcentaje).Distinct().ToList();
                                    List<decimal?> lstPorcentaje5 = lstComparacion.Select(x => x.Responsable5Porcentaje).Distinct().ToList();
                                    List<int?> lstCausaId = lstComparacion.Select(x => x.CausaId).Distinct().ToList();
                                    List<DateTime?> lstProgIni = lstComparacion.Select(x => x.FechaProgramadoIniMinuto).Distinct().ToList();
                                    List<DateTime?> lstProgFin = lstComparacion.Select(x => x.FechaProgramadoFinMinuto).Distinct().ToList();

                                    List<string> lstamposDiferentes = new List<string>();
                                    if (lstTipoInterrupcion.Count > 1) lstamposDiferentes.Add("Tipo Interrupción");
                                    if (lstFechaFin.Count > 1) lstamposDiferentes.Add("Tiempo ejecutado - Fecha hora de fin ");
                                    if (lstResp1Id.Count > 1) lstamposDiferentes.Add("Responsable 1");
                                    if (lstResp2Id.Count > 1) lstamposDiferentes.Add("Responsable 2");
                                    if (lstResp3Id.Count > 1) lstamposDiferentes.Add("Responsable 3");
                                    if (lstResp4Id.Count > 1) lstamposDiferentes.Add("Responsable 4");
                                    if (lstResp5Id.Count > 1) lstamposDiferentes.Add("Responsable 5");
                                    if (lstPorcentaje1.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 1");
                                    if (lstPorcentaje2.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 2");
                                    if (lstPorcentaje3.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 3");
                                    if (lstPorcentaje4.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 4");
                                    if (lstPorcentaje5.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 5");
                                    if (lstCausaId.Count > 1) lstamposDiferentes.Add("Causa de Interrupción");
                                    if (lstProgIni.Count > 1) lstamposDiferentes.Add("Tiempo Programado - Fecha hora de inicio");
                                    if (lstProgFin.Count > 1) lstamposDiferentes.Add("Tiempo Programado - Fecha hora de fin");


                                    foreach (var reg in lstComparacion)
                                    {
                                        reg.CamposContraste = lstamposDiferentes.Any() ? (String.Join(", ", lstamposDiferentes)) : "";
                                    }

                                    lstSalida.AddRange(lstComparacion);
                                }

                            }
                        }

                    }
                }
            }
            else //si es solo para cierto suministrador
            {
                var listaOsiGruposSumPEyFI = lstInterrupcionesOsinermingFinal.GroupBy(x => new { x.PuntoEntregaBarraNombre, x.ReininifecinicioMinuto }).ToList();
                var listaPETotGruposSumPEyFI = listaRegPEXSum.GroupBy(x => new { x.PuntoEntregaBarraNombre, x.FechaEjecIniMinuto }).ToList();
                ReEmpresaDTO empSum = lstSumTotales.Find(x => x.Emprcodi == idSuministrador);

                //Ahora comparo
                foreach (var reportadoInsumo in listaOsiGruposSumPEyFI)
                {
                    //ReInterrupcionInsumoDTO registro = lstInterrupcionesOsinermingXSum.First();
                    string barra = reportadoInsumo.Key.PuntoEntregaBarraNombre;
                    DateTime? horaIniEjec = reportadoInsumo.Key.ReininifecinicioMinuto;

                    var regExisteEnRepAgente = listaPETotGruposSumPEyFI.Where(x => x.Key.PuntoEntregaBarraNombre == barra && x.Key.FechaEjecIniMinuto == horaIniEjec).ToList();

                    //si no esta reportado por los agentesm entran al reporte como NO REPORTADOS
                    if (!regExisteEnRepAgente.Any())
                    {
                        foreach (var regO1 in reportadoInsumo)
                        {
                            InterrupcionSuministroPE newReg = new InterrupcionSuministroPE();
                            newReg.InterrupcionTipoId = regO1.InterrupcionTipoId;
                            newReg.FechaEjecFin = regO1.ReininfecfinMinuto;
                            newReg.Responsable1Id = regO1.Reininresponsable1;
                            newReg.Responsable1Porcentaje = regO1.Reininporcentaje1;
                            newReg.Responsable2Id = regO1.Reininresponsable2;
                            newReg.Responsable2Porcentaje = regO1.Reininporcentaje2;
                            newReg.Responsable3Id = regO1.Reininresponsable3;
                            newReg.Responsable3Porcentaje = regO1.Reininporcentaje3;
                            newReg.Responsable4Id = regO1.Reininresponsable4;
                            newReg.Responsable4Porcentaje = regO1.Reininporcentaje4;
                            newReg.Responsable5Id = regO1.Reininresponsable5;
                            newReg.Responsable5Porcentaje = regO1.Reininporcentaje5;
                            newReg.CausaId = regO1.Recintcodi;
                            newReg.FechaProgramadoIniMinuto = regO1.ReininprogifecinicioMinuto;
                            newReg.FechaProgramadoFinMinuto = regO1.ReininprogfecfinMinuto;
                            newReg.CausaNombre = regO1.Reninicausa;
                            //newReg.FechaProgramadoIniMinutoDesc = newReg.FechaProgramadoIniMinuto != null ? (newReg.FechaProgramadoIniMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";
                            //newReg.FechaProgramadoFinMinutoDesc = newReg.FechaProgramadoFinMinuto != null ? (newReg.FechaProgramadoFinMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";

                            newReg.PuntoEntregaBarraNombre = barra;
                            newReg.FechaEjecIniMinuto = regO1.ReininifecinicioMinuto;
                            newReg.FechaEjecIniMinutoDesc = regO1.ReininifecinicioMinutoDesc;
                            newReg.Suministrador = empSum.Emprnomb;
                            newReg.TipoNombre = regO1.TipoNombre;
                            newReg.CausaNombre = regO1.Reninicausa;
                            newReg.TiempoEjecutadoFin = regO1.ReininfecfinMinuto != null ? regO1.ReininfecfinMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : "";
                            newReg.Responsable1Nombre = regO1.Responsable1Nombre;
                            newReg.Responsable1Porcentaje = regO1.Responsable1Porcentaje;
                            newReg.Responsable2Nombre = regO1.Responsable2Nombre;
                            newReg.Responsable2Porcentaje = regO1.Responsable2Porcentaje;
                            newReg.Responsable3Nombre = regO1.Responsable3Nombre;
                            newReg.Responsable3Porcentaje = regO1.Responsable3Porcentaje;
                            newReg.Responsable4Nombre = regO1.Responsable4Nombre;
                            newReg.Responsable4Porcentaje = regO1.Responsable4Porcentaje;
                            newReg.Responsable5Nombre = regO1.Responsable5Nombre;
                            newReg.Responsable5Porcentaje = regO1.Responsable5Porcentaje;
                            newReg.FechaProgramadoIniMinutoDesc = regO1.ReininprogifecinicioMinuto != null ? (regO1.ReininprogifecinicioMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";
                            newReg.FechaProgramadoFinMinutoDesc = regO1.ReininprogfecfinMinuto != null ? (regO1.ReininprogfecfinMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";



                            //newReg.TipoObservacion = "Interrupción Osinerming que no fue reportado por los agentes";
                            newReg.Observacion = "Interrupción Osinergmin no reportada por los agentes.";

                            listadoNoReportados.Add(newReg);

                            lstSalida.AddRange(listadoNoReportados);
                        }

                    }
                    else //si existe simulitudes de los reportado en insumos en lo reportado por los agentes, hago mas comparaciones para buscar los constrastados
                    {
                        //busco los grupos por todos los campos que se deben comparar para lo reportado por agentes
                        List<InterrupcionSuministroPE> lstPE_S_PE_FI = listaRegPEXSum.Where(x => x.PuntoEntregaBarraNombre == barra && x.FechaEjecIniMinuto == horaIniEjec).ToList();
                        var gruposPEXCamposCompletos = lstPE_S_PE_FI.GroupBy(x => new
                        {
                            x.InterrupcionTipoId,
                            x.FechaEjecFin,
                            x.Responsable1Id,
                            x.Responsable1Porcentaje,
                            x.Responsable2Id,
                            x.Responsable2Porcentaje,
                            x.Responsable3Id,
                            x.Responsable3Porcentaje,
                            x.Responsable4Id,
                            x.Responsable4Porcentaje,
                            x.Responsable5Id,
                            x.Responsable5Porcentaje,
                            x.CausaId,
                            x.FechaProgramadoIniMinuto,
                            x.FechaProgramadoFinMinuto
                        }).ToList();


                        //busco los grupos por todos los campos que se deben comparar para lo reportado en insumos
                        List<ReInterrupcionInsumoDTO> lstOSI_S_PE_FI = lstInterrupcionesOsinermingFinal.Where(x => x.PuntoEntregaBarraNombre == barra && x.ReininifecinicioMinuto == horaIniEjec).ToList();
                        var gruposOSIXCamposCompletos = lstOSI_S_PE_FI.GroupBy(x => new
                        {
                            x.InterrupcionTipoId,
                            x.ReininfecfinMinuto,
                            x.Reininresponsable1,
                            x.Reininporcentaje1,
                            x.Reininresponsable2,
                            x.Reininporcentaje2,
                            x.Reininresponsable3,
                            x.Reininporcentaje3,
                            x.Reininresponsable4,
                            x.Reininporcentaje4,
                            x.Reininresponsable5,
                            x.Reininporcentaje5,
                            x.Recintcodi,
                            x.ReininprogifecinicioMinuto,
                            x.ReininprogfecfinMinuto
                        }).ToList();

                        foreach (var reportadoPE in gruposPEXCamposCompletos)
                        {
                            var regExisteEnOSI = gruposOSIXCamposCompletos.Where(x =>
                               x.Key.InterrupcionTipoId == reportadoPE.Key.InterrupcionTipoId &&
                               x.Key.ReininfecfinMinuto == reportadoPE.Key.FechaEjecFin &&
                               x.Key.Reininresponsable1 == reportadoPE.Key.Responsable1Id &&
                               x.Key.Reininporcentaje1 == reportadoPE.Key.Responsable1Porcentaje &&
                               x.Key.Reininresponsable2 == reportadoPE.Key.Responsable2Id &&
                               x.Key.Reininporcentaje2 == reportadoPE.Key.Responsable2Porcentaje &&
                               x.Key.Reininresponsable3 == reportadoPE.Key.Responsable3Id &&
                               x.Key.Reininporcentaje3 == reportadoPE.Key.Responsable3Porcentaje &&
                               x.Key.Reininresponsable4 == reportadoPE.Key.Responsable4Id &&
                               x.Key.Reininporcentaje4 == reportadoPE.Key.Responsable4Porcentaje &&
                               x.Key.Reininresponsable5 == reportadoPE.Key.Responsable5Id &&
                               x.Key.Reininporcentaje5 == reportadoPE.Key.Responsable5Porcentaje &&
                               x.Key.Recintcodi == reportadoPE.Key.CausaId &&
                               x.Key.ReininprogifecinicioMinuto == reportadoPE.Key.FechaProgramadoIniMinuto &&
                               x.Key.ReininprogfecfinMinuto == reportadoPE.Key.FechaProgramadoFinMinuto
                               ).ToList();

                            //si lo de osinerming no coincide en lo reportado por agentes, busco las diferencias
                            if (!regExisteEnOSI.Any())
                            {
                                List<InterrupcionSuministroPE> lstComparacion = new List<InterrupcionSuministroPE>();
                                foreach (var newReg in reportadoPE)
                                {
                                    //newReg.TipoObservacion = "Interrupción Osinerming que fue reportado por los agentes pero existe diferencias con otros";
                                    newReg.Observacion = "Interrupción reportada por agentes pero existe contraste dado que hay diferencias en la calificación con respecto a las interrupciones Osinegmin en los siguientes campos:";
                                    newReg.FechaProgramadoIniMinutoDesc = newReg.FechaProgramadoIniMinuto != null ? (newReg.FechaProgramadoIniMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";
                                    newReg.FechaProgramadoFinMinutoDesc = newReg.FechaProgramadoFinMinuto != null ? (newReg.FechaProgramadoFinMinuto.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : "";

                                    lstComparacion.Add(newReg);
                                }

                                List<int?> lstTipoInterrupcion = lstComparacion.Select(x => x.InterrupcionTipoId).Distinct().ToList();
                                List<DateTime?> lstFechaFin = lstComparacion.Select(x => x.FechaEjecFin).Distinct().ToList();
                                List<int?> lstResp1Id = lstComparacion.Select(x => x.Responsable1Id).Distinct().ToList();
                                List<int?> lstResp2Id = lstComparacion.Select(x => x.Responsable2Id).Distinct().ToList();
                                List<int?> lstResp3Id = lstComparacion.Select(x => x.Responsable3Id).Distinct().ToList();
                                List<int?> lstResp4Id = lstComparacion.Select(x => x.Responsable4Id).Distinct().ToList();
                                List<int?> lstResp5Id = lstComparacion.Select(x => x.Responsable5Id).Distinct().ToList();
                                List<decimal?> lstPorcentaje1 = lstComparacion.Select(x => x.Responsable1Porcentaje).Distinct().ToList();
                                List<decimal?> lstPorcentaje2 = lstComparacion.Select(x => x.Responsable2Porcentaje).Distinct().ToList();
                                List<decimal?> lstPorcentaje3 = lstComparacion.Select(x => x.Responsable3Porcentaje).Distinct().ToList();
                                List<decimal?> lstPorcentaje4 = lstComparacion.Select(x => x.Responsable4Porcentaje).Distinct().ToList();
                                List<decimal?> lstPorcentaje5 = lstComparacion.Select(x => x.Responsable5Porcentaje).Distinct().ToList();
                                List<int?> lstCausaId = lstComparacion.Select(x => x.CausaId).Distinct().ToList();
                                List<DateTime?> lstProgIni = lstComparacion.Select(x => x.FechaProgramadoIniMinuto).Distinct().ToList();
                                List<DateTime?> lstProgFin = lstComparacion.Select(x => x.FechaProgramadoFinMinuto).Distinct().ToList();

                                List<string> lstamposDiferentes = new List<string>();
                                if (lstTipoInterrupcion.Count > 1) lstamposDiferentes.Add("Tipo Interrupción");
                                if (lstFechaFin.Count > 1) lstamposDiferentes.Add("Tiempo ejecutado - Fecha hora de fin ");
                                if (lstResp1Id.Count > 1) lstamposDiferentes.Add("Responsable 1");
                                if (lstResp2Id.Count > 1) lstamposDiferentes.Add("Responsable 2");
                                if (lstResp3Id.Count > 1) lstamposDiferentes.Add("Responsable 3");
                                if (lstResp4Id.Count > 1) lstamposDiferentes.Add("Responsable 4");
                                if (lstResp5Id.Count > 1) lstamposDiferentes.Add("Responsable 5");
                                if (lstPorcentaje1.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 1");
                                if (lstPorcentaje2.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 2");
                                if (lstPorcentaje3.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 3");
                                if (lstPorcentaje4.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 4");
                                if (lstPorcentaje5.Count > 1) lstamposDiferentes.Add("Porcentaje Responsable 5");
                                if (lstCausaId.Count > 1) lstamposDiferentes.Add("Causa de Interrupción");
                                if (lstProgIni.Count > 1) lstamposDiferentes.Add("Tiempo Programado - Fecha hora de inicio");
                                if (lstProgFin.Count > 1) lstamposDiferentes.Add("Tiempo Programado - Fecha hora de fin");


                                foreach (var reg in lstComparacion)
                                {
                                    reg.CamposContraste = lstamposDiferentes.Any() ? (String.Join(", ", lstamposDiferentes)) : "";
                                }

                                lstSalida.AddRange(lstComparacion);
                            }

                        }
                    }
                }
            }



            #endregion

            #region CASO 3: Verificacion de interrupciones de suministradores por cada punto de entrega

            //Obtengo la relacion de ptos de entrega con suministrador en la seccion INSUMOS
            List<RePtoentregaSuministradorDTO> lstPtosEntregaSumTotal = FactorySic.GetRePtoentregaSuministradorRepository().GetByCriteria(periodo.Repercodi);
            List<RePtoentregaSuministradorDTO> lstPtosEntregaSum = idSuministrador == null ? lstPtosEntregaSumTotal : lstPtosEntregaSumTotal.Where(x => x.Emprcodi == idSuministrador).ToList();

            //Obtengo todos los ids de suministradores presentes en la relacion ptoEntrega-suministrador en INSUMOS
            List<int> lstSuministradoresInsumo = lstPtosEntregaSum.Any() ? (lstPtosEntregaSum.Where(x => x.Emprcodi != null).Select(x => x.Emprcodi.Value).Distinct().ToList()) : new List<int>();

            List<int> lst = listaRegistrosPEXSum.Where(x => x.PuntoEntregaId != null).Select(x => x.PuntoEntregaId.Value).Distinct().OrderBy(x => x).ToList();

            //busco por cada suministrador (de insumos) si fue reportado por los agentes o no
            foreach (int idSuminInsumo in lstSuministradoresInsumo)
            {
                string nombSuministrador = lstPtosEntregaSum.Find(x => x.Emprcodi == idSuminInsumo).Emprnomb;
                List<RePtoentregaSuministradorDTO> lstRelacionSumPEPorSuministrador = lstPtosEntregaSum.Where(x => x.Emprcodi == idSuminInsumo).ToList();

                //Agrupo por punto entrega el listado anterior
                var lstGrupoPEPorSum = lstRelacionSumPEPorSuministrador.GroupBy(x => new { x.Repentcodi }).ToList();

                //Obtengo lo reportado por agentes pero solo para el suministrador
                var lstGrupoPESumRepAgente = listaRegistrosPEXSum.GroupBy(x => new { x.PuntoEntregaId, x.SuministradorId }).ToList();

                //Ahora comparo
                //foreach (var relSumPtoE in listaRelGruposSumPE)
                foreach (var ptoEnt in lstGrupoPEPorSum)
                {
                    RePtoentregaSuministradorDTO registroS = lstRelacionSumPEPorSuministrador.First();
                    int idBarraPtoE = ptoEnt.Key.Repentcodi.Value;
                    RePtoentregaSuministradorDTO registroPE = lstPtosEntregaSum.Find(x => x.Repentcodi == idBarraPtoE);
                    string barra = registroPE != null ? (registroPE.Repentnombre != null ? registroPE.Repentnombre.Trim() : "") : "";

                    //lo busco por pe y sum
                    var regExisteEnRepAgente = lstGrupoPESumRepAgente.Where(x => x.Key.PuntoEntregaId == idBarraPtoE && x.Key.SuministradorId == idSuminInsumo).ToList();
                    if (!regExisteEnRepAgente.Any())
                    {
                        //agrego al listado que aparecera en el reporte exportado
                        InterrupcionSuministroPE reg = new InterrupcionSuministroPE();
                        reg.PuntoEntregaBarraNombre = barra;
                        reg.Suministrador = registroS.Emprnomb != null ? registroS.Emprnomb.Trim() : "";
                        reg.SuministradorId = idSuminInsumo;
                        reg.Observacion = "Pendiente de reportar, el suministrador debió reportar interrupciones para el punto de entrega";

                        lstSalida.Add(reg);
                    }
                }
            }

            #endregion


            return lstSalida;
        }

        /// <summary>
        /// Vambia el formato de la interrupcion para reporte contraste
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        public InterrupcionSuministroPE FormatearInterrupcionConstraste(ReInterrupcionInsumoDTO registro)
        {
            InterrupcionSuministroPE salida = new InterrupcionSuministroPE();

            salida.PuntoEntregaBarraNombre = registro.PuntoEntregaBarraNombre;
            salida.FechaEjecIniMinuto = registro.Reininifecinicio;
            salida.FechaEjecIniMinutoDesc = registro.ReininifecinicioMinutoDesc;
            salida.Suministrador = registro.Suministrador;
            salida.TipoNombre = registro.TipoNombre;
            salida.TiempoEjecutadoFin = registro.TiempoEjecutadoFin;
            salida.Responsable1Nombre = registro.Responsable1Nombre;
            salida.Responsable1Porcentaje = registro.Responsable1Porcentaje;
            salida.Responsable2Nombre = registro.Responsable2Nombre;
            salida.Responsable2Porcentaje = registro.Responsable2Porcentaje;
            salida.Responsable3Nombre = registro.Responsable3Nombre;
            salida.Responsable3Porcentaje = registro.Responsable3Porcentaje;
            salida.Responsable4Nombre = registro.Responsable4Nombre;
            salida.Responsable4Porcentaje = registro.Responsable4Porcentaje;
            salida.Responsable5Nombre = registro.Responsable5Nombre;
            salida.Responsable5Porcentaje = registro.Responsable5Porcentaje;
            salida.Observacion = registro.Observacion;


            return salida;
        }

        /// <summary>
        /// Devuelve el listado de interrupciones osinerming formateado
        /// </summary>
        /// <param name="listado"></param>
        /// <returns></returns>
        public List<ReInterrupcionInsumoDTO> FormatearInterrupcionesOsinerming(List<ReInterrupcionInsumoDTO> listado, int idPeriodo)
        {
            List<RePtoentregaPeriodoDTO> lstPtoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodo);
            List<ReEmpresaDTO> listaEmpresa = this.ObtenerEmpresasSuministradorasTotal();
            List<ReTipoInterrupcionDTO> listaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
            List<ReEmpresaDTO> listaCliente = this.ObtenerEmpresas();

            foreach (ReInterrupcionInsumoDTO reg in listado)
            {
                RePtoentregaPeriodoDTO ptoE = lstPtoEntrega.Find(x => x.Repentcodi == reg.Repentcodi);
                reg.PuntoEntregaBarraNombre = ptoE != null ? (ptoE.Repentnombre != null ? ptoE.Repentnombre.Trim() : "") : "";
                reg.InterrupcionTipoId = reg.Retintcodi; //contraste
                reg.ReininifecinicioMinutoDesc = reg.Reininifecinicio != null ? (reg.Reininifecinicio.Value.ToString(ConstantesAppServicio.FormatoFechaFull)) : ""; //contraste                
                ReEmpresaDTO sum = listaEmpresa.Find(x => x.Emprcodi == reg.Reininsuministrador);
                reg.Suministrador = sum != null ? (sum.Emprnomb != null ? sum.Emprnomb.Trim() : "") : "";
                ReTipoInterrupcionDTO tipoInt = listaTipoInterrupcion.Find(x => x.Retintcodi == reg.Retintcodi);
                reg.TipoNombre = reg.Retintcodi != null ? (tipoInt != null ? (tipoInt.Retintnombre != null ? tipoInt.Retintnombre.Trim() : "") : "") : "";
                reg.TiempoEjecutadoFin = reg.Reininfecfin != null ? reg.Reininfecfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";

                DateTime? fecEI = reg.Reininifecinicio;
                DateTime? fecEF = reg.Reininfecfin;
                DateTime? fecPI = reg.Reininprogifecinicio;
                DateTime? fecPF = reg.Reininprogfecfin;

                reg.ReininifecinicioMinuto = fecEI != null ? new DateTime(fecEI.Value.Year, fecEI.Value.Month, fecEI.Value.Day, fecEI.Value.Hour, fecEI.Value.Minute, 0) : fecEI; //contraste
                reg.ReininfecfinMinuto = fecEF != null ? new DateTime(fecEF.Value.Year, fecEF.Value.Month, fecEF.Value.Day, fecEF.Value.Hour, fecEF.Value.Minute, 0) : fecEF; //contraste
                reg.ReininprogifecinicioMinuto = fecPI != null ? new DateTime(fecPI.Value.Year, fecPI.Value.Month, fecPI.Value.Day, fecPI.Value.Hour, fecPI.Value.Minute, 0) : fecPI; //contraste
                reg.ReininprogfecfinMinuto = fecPF != null ? new DateTime(fecPF.Value.Year, fecPF.Value.Month, fecPF.Value.Day, fecPF.Value.Hour, fecPF.Value.Minute, 0) : fecPF; //contraste


                ReEmpresaDTO resp1 = reg.Reininresponsable1 != null ? listaCliente.Find(x => x.Emprcodi == reg.Reininresponsable1) : null;
                ReEmpresaDTO resp2 = reg.Reininresponsable2 != null ? listaCliente.Find(x => x.Emprcodi == reg.Reininresponsable2) : null;
                ReEmpresaDTO resp3 = reg.Reininresponsable3 != null ? listaCliente.Find(x => x.Emprcodi == reg.Reininresponsable3) : null;
                ReEmpresaDTO resp4 = reg.Reininresponsable4 != null ? listaCliente.Find(x => x.Emprcodi == reg.Reininresponsable4) : null;
                ReEmpresaDTO resp5 = reg.Reininresponsable5 != null ? listaCliente.Find(x => x.Emprcodi == reg.Reininresponsable5) : null;
                reg.Responsable1Nombre = resp1 != null ? (resp1.Emprnomb != null ? resp1.Emprnomb.Trim() : "") : "";
                reg.Responsable1Porcentaje = reg.Reininresponsable1 != null ? reg.Reininporcentaje1 : null;
                reg.Responsable2Nombre = resp2 != null ? (resp2.Emprnomb != null ? resp2.Emprnomb.Trim() : "") : "";
                reg.Responsable2Porcentaje = reg.Reininresponsable2 != null ? reg.Reininporcentaje2 : null;
                reg.Responsable3Nombre = resp3 != null ? (resp3.Emprnomb != null ? resp3.Emprnomb.Trim() : "") : "";
                reg.Responsable3Porcentaje = reg.Reininresponsable3 != null ? reg.Reininporcentaje3 : null;
                reg.Responsable4Nombre = resp4 != null ? (resp4.Emprnomb != null ? resp4.Emprnomb.Trim() : "") : "";
                reg.Responsable4Porcentaje = reg.Reininresponsable4 != null ? reg.Reininporcentaje4 : null;
                reg.Responsable5Nombre = resp5 != null ? (resp5.Emprnomb != null ? resp5.Emprnomb.Trim() : "") : "";
                reg.Responsable5Porcentaje = reg.Reininresponsable5 != null ? reg.Reininporcentaje5 : null;
            }

            return listado;
        }

        #endregion

        #region Reporte Interrupciones en Controversia

        /// <summary>
        /// Genera el reporte Interrupciones en Controversia
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        private void GenerarArchivoExcelInterrupcionesEnControversia(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroIntranet> lista = new List<InterrupcionSuministroIntranet>();

            //Obtengo el listado general 
            List<InterrupcionSuministroIntranet> listaGeneral = ObtenerListaInterrupcionIntranetPorPeriodo(periodo);
            //List<InterrupcionSuministroIntranet> listaGeneral = new List<InterrupcionSuministroIntranet>();  // para evidencias cuando no haya registros

            //Obtengo los registros en controversia
            foreach (var regInterrupcion in listaGeneral)
            {
                if (regInterrupcion.RegistroEnControversiaSum)
                    lista.Add(regInterrupcion);
            }

            int numResponsables = ObtenerNumeroResponsables(lista);

            string nameWS = "Listado";
            string titulo = "Interrupciones en controversia";
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 1;
            int rowIniTitulo = 3;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;

            int colSuministrador = colIniTable;
            int colCorrelativo = colIniTable + 1;
            int colTipoCliente = colIniTable + 2;
            int colNombreCliente = colIniTable + 3;
            int colPuntoEntregaBarraNombre = colIniTable + 4;
            int colNumSuministroClienteLibre = colIniTable + 5;
            int colNivelTensionNombre = colIniTable + 6;
            int colAplicacionLiteral = colIniTable + 7;
            int colEnergiaPeriodo = colIniTable + 8;
            int colIncrementoTolerancia = colIniTable + 9;
            int colTipoNombre = colIniTable + 10;
            int colCausaNombre = colIniTable + 11;
            int colNi = colIniTable + 12;
            int colKi = colIniTable + 13;
            int colTiempoEjecutadoIni = colIniTable + 14;
            int colTiempoEjecutadoFin = colIniTable + 15;
            int colTiempoProgramadoIni = colIniTable + 16;
            int colTiempoProgramadoFin = colIniTable + 17;
            int colResponsable1Nombre = colIniTable + 18;
            int colResponsable1Porcentaje = colIniTable + 19;
            int colResponsable2Nombre = colIniTable + 20;
            int colResponsable2Porcentaje = colIniTable + 21;
            int colResponsable3Nombre = colIniTable + 22;
            int colResponsable3Porcentaje = colIniTable + 23;
            int colResponsable4Nombre = colIniTable + 24;
            int colResponsable4Porcentaje = colIniTable + 25;
            int colResponsable5Nombre = colIniTable + 26;
            int colResponsable5Porcentaje = colIniTable + 27;
            int colCausaResumida = colIniTable + 28;
            int colEiE = colIniTable + 29;
            int colResarcimiento = colIniTable + 30;

            int colResp1ConformidadResponsable = colIniTable + 31;
            int colResp1Observacion = colIniTable + 32;
            int colResp1DetalleObservacion = colIniTable + 33;
            int colResp1Comentario1 = colIniTable + 34;
            int colResp1ConformidadSuministrador = colIniTable + 35;
            int colResp1Comentario2 = colIniTable + 36;

            int colResp2ConformidadResponsable = colIniTable + 37;
            int colResp2Observacion = colIniTable + 38;
            int colResp2DetalleObservacion = colIniTable + 39;
            int colResp2Comentario1 = colIniTable + 40;
            int colResp2ConformidadSuministrador = colIniTable + 41;
            int colResp2Comentario2 = colIniTable + 42;

            int colResp3ConformidadResponsable = colIniTable + 43;
            int colResp3Observacion = colIniTable + 44;
            int colResp3DetalleObservacion = colIniTable + 45;
            int colResp3Comentario1 = colIniTable + 46;
            int colResp3ConformidadSuministrador = colIniTable + 47;
            int colResp3Comentario2 = colIniTable + 48;

            int colResp4ConformidadResponsable = colIniTable + 49;
            int colResp4Observacion = colIniTable + 50;
            int colResp4DetalleObservacion = colIniTable + 51;
            int colResp4Comentario1 = colIniTable + 52;
            int colResp4ConformidadSuministrador = colIniTable + 53;
            int colResp4Comentario2 = colIniTable + 54;

            int colResp5ConformidadResponsable = colIniTable + 55;
            int colResp5Observacion = colIniTable + 56;
            int colResp5DetalleObservacion = colIniTable + 57;
            int colResp5Comentario1 = colIniTable + 58;
            int colResp5ConformidadSuministrador = colIniTable + 59;
            int colResp5Comentario2 = colIniTable + 60;

            int colDesicionControversia = colIniTable + 61;
            int colComentarioFinal = colIniTable + 62;

            switch (numResponsables)
            {
                case 1:
                    colDesicionControversia = colIniTable + 37;
                    colComentarioFinal = colIniTable + 38;
                    break;
                case 2:
                    colDesicionControversia = colIniTable + 43;
                    colComentarioFinal = colIniTable + 44;
                    break;
                case 3:
                    colDesicionControversia = colIniTable + 49;
                    colComentarioFinal = colIniTable + 50;
                    break;
                case 4:
                    colDesicionControversia = colIniTable + 55;
                    colComentarioFinal = colIniTable + 56;
                    break;
                case 5:
                    colDesicionControversia = colIniTable + 61;
                    colComentarioFinal = colIniTable + 62;
                    break;
            }


            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento);

            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colCorrelativo].Value = "Correlativo por Punto de Entrega";
            ws.Cells[rowIniTabla, colTipoCliente].Value = "Tipo de Cliente";
            ws.Cells[rowIniTabla, colNombreCliente].Value = "Cliente";
            ws.Cells[rowIniTabla, colPuntoEntregaBarraNombre].Value = "Punto de Entrega/Barra";
            ws.Cells[rowIniTabla, colNumSuministroClienteLibre].Value = "N° de suministro cliente libre";
            ws.Cells[rowIniTabla, colNivelTensionNombre].Value = "Nivel de Tensión";
            ws.Cells[rowIniTabla, colAplicacionLiteral].Value = "Aplicación literal e) de numeral 5.2.4 Base Metodológica (meses de suministro en el semestre)";
            ws.Cells[rowIniTabla, colEnergiaPeriodo].Value = "Energía Semestral (kWh)";
            ws.Cells[rowIniTabla, colIncrementoTolerancia].Value = "Incremento de tolerancias Sector Distribución Típico 2 (Mercado regulado)";
            ws.Cells[rowIniTabla, colTipoNombre].Value = "Tipo";
            ws.Cells[rowIniTabla, colCausaNombre].Value = "Causa";
            ws.Cells[rowIniTabla, colNi].Value = "Ni";
            ws.Cells[rowIniTabla, colKi].Value = "Ki";
            ws.Cells[rowIniTabla, colTiempoEjecutadoIni].Value = "Tiempo Ejecutado";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colTiempoProgramadoIni].Value = "Tiempo Programado";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colResponsable1Nombre].Value = "Responsable 1";
            ws.Cells[rowIniTabla + 1, colResponsable1Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable1Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable2Nombre].Value = "Responsable 2";
            ws.Cells[rowIniTabla + 1, colResponsable2Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable2Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable3Nombre].Value = "Responsable 3";
            ws.Cells[rowIniTabla + 1, colResponsable3Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable3Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable4Nombre].Value = "Responsable 4";
            ws.Cells[rowIniTabla + 1, colResponsable4Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable4Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable5Nombre].Value = "Responsable 5";
            ws.Cells[rowIniTabla + 1, colResponsable5Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable5Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colCausaResumida].Value = "Causa resumida de interrupción";
            ws.Cells[rowIniTabla, colEiE].Value = "Ei / E";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento (US$)";

            if (numResponsables >= 1)
            {
                ws.Cells[rowIniTabla, colResp1ConformidadResponsable].Value = "Responsable 1";
                ws.Cells[rowIniTabla + 1, colResp1ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp1Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp1DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp1Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp1ConformidadSuministrador].Value = "Conformidad Responsable 1";
                ws.Cells[rowIniTabla + 1, colResp1ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp1Comentario2].Value = "Comentarios";
            }

            if (numResponsables >= 2)
            {
                ws.Cells[rowIniTabla, colResp2ConformidadResponsable].Value = "Responsable 2";
                ws.Cells[rowIniTabla + 1, colResp2ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp2Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp2DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp2Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp2ConformidadSuministrador].Value = "Conformidad Responsable 2";
                ws.Cells[rowIniTabla + 1, colResp2ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp2Comentario2].Value = "Comentarios";
            }

            if (numResponsables >= 3)
            {
                ws.Cells[rowIniTabla, colResp3ConformidadResponsable].Value = "Responsable 3";
                ws.Cells[rowIniTabla + 1, colResp3ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp3Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp3DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp3Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp3ConformidadSuministrador].Value = "Conformidad Responsable 3";
                ws.Cells[rowIniTabla + 1, colResp3ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp3Comentario2].Value = "Comentarios";
            }

            if (numResponsables >= 4)
            {
                ws.Cells[rowIniTabla, colResp4ConformidadResponsable].Value = "Responsable 4";
                ws.Cells[rowIniTabla + 1, colResp4ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp4Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp4DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp4Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp4ConformidadSuministrador].Value = "Conformidad Responsable 4";
                ws.Cells[rowIniTabla + 1, colResp4ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp4Comentario2].Value = "Comentarios";
            }

            if (numResponsables == 5)
            {
                ws.Cells[rowIniTabla, colResp5ConformidadResponsable].Value = "Responsable 5";
                ws.Cells[rowIniTabla + 1, colResp5ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp5Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp5DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp5Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp5ConformidadSuministrador].Value = "Conformidad Responsable 5";
                ws.Cells[rowIniTabla + 1, colResp5ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp5Comentario2].Value = "Comentarios";
            }

            ws.Cells[rowIniTabla, colDesicionControversia].Value = "Decisión de Controveria";
            ws.Cells[rowIniTabla, colComentarioFinal].Value = "Comentarios";

            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCorrelativo, rowIniTabla + 1, colCorrelativo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoCliente, rowIniTabla + 1, colTipoCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNombreCliente, rowIniTabla + 1, colNombreCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPuntoEntregaBarraNombre, rowIniTabla + 1, colPuntoEntregaBarraNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNumSuministroClienteLibre, rowIniTabla + 1, colNumSuministroClienteLibre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNivelTensionNombre, rowIniTabla + 1, colNivelTensionNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionLiteral, rowIniTabla + 1, colAplicacionLiteral);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEnergiaPeriodo, rowIniTabla + 1, colEnergiaPeriodo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colIncrementoTolerancia, rowIniTabla + 1, colIncrementoTolerancia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoNombre, rowIniTabla + 1, colTipoNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaNombre, rowIniTabla + 1, colCausaNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNi, rowIniTabla + 1, colNi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colKi, rowIniTabla + 1, colKi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoIni, rowIniTabla, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoProgramadoIni, rowIniTabla, colTiempoProgramadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable1Nombre, rowIniTabla, colResponsable1Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable2Nombre, rowIniTabla, colResponsable2Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable3Nombre, rowIniTabla, colResponsable3Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable4Nombre, rowIniTabla, colResponsable4Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable5Nombre, rowIniTabla, colResponsable5Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaResumida, rowIniTabla + 1, colCausaResumida);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEiE, rowIniTabla + 1, colEiE);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResarcimiento, rowIniTabla + 1, colResarcimiento);

            if (numResponsables >= 1)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp1ConformidadResponsable, rowIniTabla, colResp1Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp1ConformidadSuministrador, rowIniTabla, colResp1Comentario2);
            }

            if (numResponsables >= 2)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp2ConformidadResponsable, rowIniTabla, colResp2Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp2ConformidadSuministrador, rowIniTabla, colResp2Comentario2);
            }

            if (numResponsables >= 3)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp3ConformidadResponsable, rowIniTabla, colResp3Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp3ConformidadSuministrador, rowIniTabla, colResp3Comentario2);
            }

            if (numResponsables >= 4)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp4ConformidadResponsable, rowIniTabla, colResp4Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp4ConformidadSuministrador, rowIniTabla, colResp4Comentario2);
            }

            if (numResponsables == 5)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp5ConformidadResponsable, rowIniTabla, colResp5Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp5ConformidadSuministrador, rowIniTabla, colResp5Comentario2);
            }


            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colDesicionControversia, rowIniTabla + 1, colDesicionControversia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colComentarioFinal, rowIniTabla + 1, colComentarioFinal);


            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "#1659AA");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal);
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "#FFFFFF");
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal);

            if (numResponsables >= 1)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp1ConformidadResponsable, rowIniTabla + 1, colResp1Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp1ConformidadResponsable, rowIniTabla + 1, colResp1Comentario1, "#000000");
            }

            if (numResponsables >= 2)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp2ConformidadResponsable, rowIniTabla + 1, colResp2Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp2ConformidadResponsable, rowIniTabla + 1, colResp2Comentario1, "#000000");
            }

            if (numResponsables >= 3)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp3ConformidadResponsable, rowIniTabla + 1, colResp3Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp3ConformidadResponsable, rowIniTabla + 1, colResp3Comentario1, "#000000");
            }

            if (numResponsables >= 4)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp4ConformidadResponsable, rowIniTabla + 1, colResp4Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp4ConformidadResponsable, rowIniTabla + 1, colResp4Comentario1, "#000000");
            }

            if (numResponsables >= 5)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp5ConformidadResponsable, rowIniTabla + 1, colResp5Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp5ConformidadResponsable, rowIniTabla + 1, colResp5Comentario1, "#000000");
            }

            #endregion


            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in lista)
            {
                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colCorrelativo].Value = item.Correlativo != null ? item.Correlativo.Value : item.Correlativo;
                ws.Cells[rowData, colTipoCliente].Value = item.TipoCliente != null ? item.TipoCliente.Trim() : null;
                ws.Cells[rowData, colNombreCliente].Value = item.NombreCliente != null ? item.NombreCliente.Trim() : null;
                ws.Cells[rowData, colPuntoEntregaBarraNombre].Value = item.PuntoEntregaBarraNombre != null ? item.PuntoEntregaBarraNombre.Trim() : null;
                ws.Cells[rowData, colNumSuministroClienteLibre].Value = item.NumSuministroClienteLibre != null ? item.NumSuministroClienteLibre : item.NumSuministroClienteLibre;
                ws.Cells[rowData, colNivelTensionNombre].Value = item.NivelTensionNombre != null ? item.NivelTensionNombre.Trim() : null;
                ws.Cells[rowData, colAplicacionLiteral].Value = item.AplicacionLiteral != null ? item.AplicacionLiteral.Value : item.AplicacionLiteral;
                ws.Cells[rowData, colEnergiaPeriodo].Value = item.EnergiaPeriodo != null ? item.EnergiaPeriodo.Value : item.EnergiaPeriodo;
                ws.Cells[rowData, colEnergiaPeriodo].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colIncrementoTolerancia].Value = item.IncrementoTolerancia != null ? item.IncrementoTolerancia.Trim() : null;
                ws.Cells[rowData, colTipoNombre].Value = item.TipoNombre != null ? item.TipoNombre.Trim() : null;
                ws.Cells[rowData, colCausaNombre].Value = item.CausaNombre != null ? item.CausaNombre.Trim() : null;
                ws.Cells[rowData, colNi].Value = item.Ni != null ? item.Ni.Value : item.Ni;
                ws.Cells[rowData, colNi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colKi].Value = item.Ki != null ? item.Ki.Value : item.Ki;
                ws.Cells[rowData, colKi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colTiempoEjecutadoIni].Value = item.TiempoEjecutadoIni != null ? item.TiempoEjecutadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoIni].Value = item.TiempoProgramadoIni != null ? item.TiempoProgramadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoFin].Value = item.TiempoProgramadoFin != null ? item.TiempoProgramadoFin.Trim() : null;
                ws.Cells[rowData, colResponsable1Nombre].Value = item.Responsable1Nombre != null ? item.Responsable1Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable1Porcentaje].Value = item.Responsable1Porcentaje != null ? item.Responsable1Porcentaje.Value / 100 : item.Responsable1Porcentaje;
                ws.Cells[rowData, colResponsable1Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable2Nombre].Value = item.Responsable2Nombre != null ? item.Responsable2Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable2Porcentaje].Value = item.Responsable2Porcentaje != null ? item.Responsable2Porcentaje.Value / 100 : item.Responsable2Porcentaje;
                ws.Cells[rowData, colResponsable2Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable3Nombre].Value = item.Responsable3Nombre != null ? item.Responsable3Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable3Porcentaje].Value = item.Responsable3Porcentaje != null ? item.Responsable3Porcentaje.Value / 100 : item.Responsable3Porcentaje;
                ws.Cells[rowData, colResponsable3Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable4Nombre].Value = item.Responsable4Nombre != null ? item.Responsable4Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable4Porcentaje].Value = item.Responsable4Porcentaje != null ? item.Responsable4Porcentaje.Value / 100 : item.Responsable4Porcentaje;
                ws.Cells[rowData, colResponsable4Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable5Nombre].Value = item.Responsable5Nombre != null ? item.Responsable5Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable5Porcentaje].Value = item.Responsable5Porcentaje != null ? item.Responsable5Porcentaje.Value / 100 : item.Responsable5Porcentaje;
                ws.Cells[rowData, colResponsable5Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colCausaResumida].Value = item.CausaResumida != null ? item.CausaResumida.Trim() : null;
                ws.Cells[rowData, colEiE].Value = item.EiE != null ? item.EiE.Value : item.EiE;
                ws.Cells[rowData, colEiE].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResarcimiento].Value = item.Resarcimiento != null ? item.Resarcimiento.Value : item.Resarcimiento;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = FormatoNumDecimales(2);

                if (numResponsables >= 1)
                {
                    ws.Cells[rowData, colResp1ConformidadResponsable].Value = item.Resp1ConformidadResponsable != null ? item.Resp1ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp1Observacion].Value = item.Resp1Observacion != null ? item.Resp1Observacion.Trim() : null;
                    ws.Cells[rowData, colResp1DetalleObservacion].Value = item.Resp1DetalleObservacion != null ? item.Resp1DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp1Comentario1].Value = item.Resp1Comentario1 != null ? item.Resp1Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp1ConformidadSuministrador].Value = item.Resp1ConformidadSuministrador != null ? item.Resp1ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp1Comentario2].Value = item.Resp1Comentario2 != null ? item.Resp1Comentario2.Trim() : null;
                }

                if (numResponsables >= 2)
                {
                    ws.Cells[rowData, colResp2ConformidadResponsable].Value = item.Resp2ConformidadResponsable != null ? item.Resp2ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp2Observacion].Value = item.Resp2Observacion != null ? item.Resp2Observacion.Trim() : null;
                    ws.Cells[rowData, colResp2DetalleObservacion].Value = item.Resp2DetalleObservacion != null ? item.Resp2DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp2Comentario1].Value = item.Resp2Comentario1 != null ? item.Resp2Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp2ConformidadSuministrador].Value = item.Resp2ConformidadSuministrador != null ? item.Resp2ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp2Comentario2].Value = item.Resp2Comentario2 != null ? item.Resp2Comentario2.Trim() : null;
                }

                if (numResponsables >= 3)
                {
                    ws.Cells[rowData, colResp3ConformidadResponsable].Value = item.Resp3ConformidadResponsable != null ? item.Resp3ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp3Observacion].Value = item.Resp3Observacion != null ? item.Resp3Observacion.Trim() : null;
                    ws.Cells[rowData, colResp3DetalleObservacion].Value = item.Resp3DetalleObservacion != null ? item.Resp3DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp3Comentario1].Value = item.Resp3Comentario1 != null ? item.Resp3Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp3ConformidadSuministrador].Value = item.Resp3ConformidadSuministrador != null ? item.Resp3ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp3Comentario2].Value = item.Resp3Comentario2 != null ? item.Resp3Comentario2.Trim() : null;
                }

                if (numResponsables >= 4)
                {
                    ws.Cells[rowData, colResp4ConformidadResponsable].Value = item.Resp4ConformidadResponsable != null ? item.Resp4ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp4Observacion].Value = item.Resp4Observacion != null ? item.Resp4Observacion.Trim() : null;
                    ws.Cells[rowData, colResp4DetalleObservacion].Value = item.Resp4DetalleObservacion != null ? item.Resp4DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp4Comentario1].Value = item.Resp4Comentario1 != null ? item.Resp4Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp4ConformidadSuministrador].Value = item.Resp4ConformidadSuministrador != null ? item.Resp4ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp4Comentario2].Value = item.Resp4Comentario2 != null ? item.Resp4Comentario2.Trim() : null;
                }

                if (numResponsables == 5)
                {
                    ws.Cells[rowData, colResp5ConformidadResponsable].Value = item.Resp5ConformidadResponsable != null ? item.Resp5ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp5Observacion].Value = item.Resp5Observacion != null ? item.Resp5Observacion.Trim() : null;
                    ws.Cells[rowData, colResp5DetalleObservacion].Value = item.Resp5DetalleObservacion != null ? item.Resp5DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp5Comentario1].Value = item.Resp5Comentario1 != null ? item.Resp5Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp5ConformidadSuministrador].Value = item.Resp5ConformidadSuministrador != null ? item.Resp5ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp5Comentario2].Value = item.Resp5Comentario2 != null ? item.Resp5Comentario2.Trim() : null;
                }

                ws.Cells[rowData, colDesicionControversia].Value = item.DesicionControversia != null ? item.DesicionControversia.Trim() : null;
                ws.Cells[rowData, colComentarioFinal].Value = item.ComentarioFinal != null ? item.ComentarioFinal.Trim() : null;


                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colSuministrador, rowData - 1, colComentarioFinal, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministrador, rowData - 1, colComentarioFinal);
            #endregion

            //filter                       

            if (lista.Any())
            {

                ws.Cells[rowIniTabla, colSuministrador, rowData, colComentarioFinal].AutoFitColumns(12.5);
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 14;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 18;
                ws.Column(colTiempoEjecutadoFin).Width = 18;
                ws.Column(colTiempoProgramadoIni).Width = 18;
                ws.Column(colTiempoProgramadoFin).Width = 18;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadResponsable).Width = 12;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadSuministrador).Width = 13;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadResponsable).Width = 12;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadSuministrador).Width = 13;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadResponsable).Width = 12;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadSuministrador).Width = 13;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadResponsable).Width = 12;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadSuministrador).Width = 13;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadResponsable).Width = 12;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadSuministrador).Width = 13;
            }
            else
            {
                ws.Column(colSuministrador).Width = 20;
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNombreCliente).Width = 12;
                ws.Column(colPuntoEntregaBarraNombre).Width = 22;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 12;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colTipoNombre).Width = 10;
                ws.Column(colCausaNombre).Width = 10;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 13;
                ws.Column(colTiempoEjecutadoFin).Width = 12;
                ws.Column(colTiempoProgramadoIni).Width = 13;
                ws.Column(colTiempoProgramadoFin).Width = 12;
                ws.Column(colResponsable1Nombre).Width = 8;
                ws.Column(colResponsable1Porcentaje).Width = 5;
                ws.Column(colResponsable2Nombre).Width = 8;
                ws.Column(colResponsable2Porcentaje).Width = 5;
                ws.Column(colResponsable3Nombre).Width = 8;
                ws.Column(colResponsable3Porcentaje).Width = 5;
                ws.Column(colResponsable4Nombre).Width = 8;
                ws.Column(colResponsable4Porcentaje).Width = 5;
                ws.Column(colResponsable5Nombre).Width = 8;
                ws.Column(colResponsable5Porcentaje).Width = 5;
                ws.Column(colCausaResumida).Width = 27;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadResponsable).Width = 12;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadSuministrador).Width = 13;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadResponsable).Width = 12;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadSuministrador).Width = 13;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadResponsable).Width = 12;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadSuministrador).Width = 13;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadResponsable).Width = 12;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadSuministrador).Width = 13;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadResponsable).Width = 12;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadSuministrador).Width = 13;
            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }


        #endregion

        #region Reporte comparativo semestral y trimestral

        /// <summary>
        /// Devuelve el listado de peridos trimestrales asociados al periodo semestral
        /// </summary>
        /// <param name="periodoSemestral"></param>
        /// <returns></returns>
        public List<RePeriodoDTO> ObtenerPeriodosTrimestralesAsociados(RePeriodoDTO periodoSemestral)
        {
            List<RePeriodoDTO> lstSalida = new List<RePeriodoDTO>();

            List<RePeriodoDTO> lstPeriodos = FactorySic.GetRePeriodoRepository().List();
            lstSalida = lstPeriodos.Where(x => x.Repertipo == "T" && x.Reperestado == "A" && x.Reperpadre == periodoSemestral.Repercodi).OrderBy(x => x.Reperorden).ToList();

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado de peridos trimestrales asociados al periodo semestral
        /// </summary>
        /// <param name="periodoSemestral"></param>
        /// <returns></returns>
        public List<RePeriodoDTO> ObtenerPeriodosTrimestrales(int idPeriodo)
        {
            List<RePeriodoDTO> lstSalida = new List<RePeriodoDTO>();

            List<RePeriodoDTO> lstPeriodos = FactorySic.GetRePeriodoRepository().List();
            lstSalida = lstPeriodos.Where(x => x.Repertipo == "T" && x.Reperestado == "A" && x.Reperpadre == idPeriodo).OrderBy(x => x.Reperorden).ToList();

            return lstSalida;
        }

        /// <summary>
        /// Genera el reporte comparativo semestral y trimestral
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="periodoSemestral"></param>
        /// <param name="periodoTrimestral"></param>
        /// <param name="listaRegistrosPE_Semestral"></param>
        /// <param name="listaRegistrosPE_Trimestral"></param>
        /// <param name="tipo"></param>
        private void GenerarArchivoExcelComparativoSemestralTrimestral(ExcelPackage xlPackage, string pathLogo, RePeriodoDTO periodoSemestral, RePeriodoDTO periodoTrimestral, List<InterrupcionSuministroPE> listaRegistrosPE_Semestral, List<InterrupcionSuministroPE> listaRegistrosPE_Trimestral, int tipo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroPE> lista = new List<InterrupcionSuministroPE>();
            lista = ObtenerListadoComparativoSemestralTrimestral(listaRegistrosPE_Semestral, listaRegistrosPE_Trimestral, tipo);

            string nameWS = tipo == ConstantesCalculoResarcimiento.ComparativoSemestralRespectoTrimestral ? "Semestral Trimestral" : (tipo == ConstantesCalculoResarcimiento.ComparativoTrimestralRespectoSemestral ? "Trimestral Semestral" : "Comparativo");
            string titulo = tipo == ConstantesCalculoResarcimiento.ComparativoSemestralRespectoTrimestral ? "Comparativo Semestral respecto al Trimestral" : (tipo == ConstantesCalculoResarcimiento.ComparativoTrimestralRespectoSemestral ? "Comparativo Trimestral respecto al Semestral" : "");
            string strPeriodo = "Semestral " + periodoSemestral.Reperanio + " ( " + periodoSemestral.Repernombre + ")  vs Trimestral " + periodoTrimestral.Reperanio + " ( " + periodoTrimestral.Repernombre + ")";
            string strNota = tipo == ConstantesCalculoResarcimiento.ComparativoSemestralRespectoTrimestral ? "Interrupciones registradas en el periodo Semestral y NO en el periodo Trimestral" : (tipo == ConstantesCalculoResarcimiento.ComparativoTrimestralRespectoSemestral ? "Interrupciones registradas en el periodo Trimestral y NO en el periodo Semestral" : "");

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 1;
            int rowIniTitulo = 3;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 4;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;

            int colSuministrador = colIniTable;
            int colCorrelativo = colIniTable + 1;
            int colTipoCliente = colIniTable + 2;
            int colNombreCliente = colIniTable + 3;
            int colPuntoEntregaBarraNombre = colIniTable + 4;
            int colNumSuministroClienteLibre = colIniTable + 5;
            int colNivelTensionNombre = colIniTable + 6;
            int colAplicacionLiteral = colIniTable + 7;
            int colEnergiaPeriodo = colIniTable + 8;
            int colIncrementoTolerancia = colIniTable + 9;
            int colTipoNombre = colIniTable + 10;
            int colCausaNombre = colIniTable + 11;
            int colNi = colIniTable + 12;
            int colKi = colIniTable + 13;
            int colTiempoEjecutadoIni = colIniTable + 14;
            int colTiempoEjecutadoFin = colIniTable + 15;
            int colTiempoProgramadoIni = colIniTable + 16;
            int colTiempoProgramadoFin = colIniTable + 17;
            int colResponsable1Nombre = colIniTable + 18;
            int colResponsable1Porcentaje = colIniTable + 19;
            int colResponsable2Nombre = colIniTable + 20;
            int colResponsable2Porcentaje = colIniTable + 21;
            int colResponsable3Nombre = colIniTable + 22;
            int colResponsable3Porcentaje = colIniTable + 23;
            int colResponsable4Nombre = colIniTable + 24;
            int colResponsable4Porcentaje = colIniTable + 25;
            int colResponsable5Nombre = colIniTable + 26;
            int colResponsable5Porcentaje = colIniTable + 27;
            int colCausaResumida = colIniTable + 28;
            int colEiE = colIniTable + 29;
            int colResarcimiento = colIniTable + 30;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            ws.Cells[rowIniTitulo + 2, colIniTitulo].Value = strNota;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 2, colResarcimiento, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Calibri", 11);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 2, colIniTitulo, rowIniTitulo + 2, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 2, colIniTitulo, rowIniTitulo + 2, colResarcimiento, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 2, colResarcimiento);

            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colCorrelativo].Value = "Correlativo por Punto de Entrega";
            ws.Cells[rowIniTabla, colTipoCliente].Value = "Tipo de Cliente";
            ws.Cells[rowIniTabla, colNombreCliente].Value = "Cliente";
            ws.Cells[rowIniTabla, colPuntoEntregaBarraNombre].Value = "Punto de Entrega/Barra";
            ws.Cells[rowIniTabla, colNumSuministroClienteLibre].Value = "N° de suministro cliente libre";
            ws.Cells[rowIniTabla, colNivelTensionNombre].Value = "Nivel de Tensión";
            ws.Cells[rowIniTabla, colAplicacionLiteral].Value = "Aplicación literal e) de numeral 5.2.4 Base Metodológica (meses de suministro en el semestre)";
            ws.Cells[rowIniTabla, colEnergiaPeriodo].Value = "Energía Semestral (kWh)";
            ws.Cells[rowIniTabla, colIncrementoTolerancia].Value = "Incremento de tolerancias Sector Distribución Típico 2 (Mercado regulado)";
            ws.Cells[rowIniTabla, colTipoNombre].Value = "Tipo";
            ws.Cells[rowIniTabla, colCausaNombre].Value = "Causa";
            ws.Cells[rowIniTabla, colNi].Value = "Ni";
            ws.Cells[rowIniTabla, colKi].Value = "Ki";
            ws.Cells[rowIniTabla, colTiempoEjecutadoIni].Value = "Tiempo Ejecutado";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colTiempoProgramadoIni].Value = "Tiempo Programado";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colResponsable1Nombre].Value = "Responsable 1";
            ws.Cells[rowIniTabla + 1, colResponsable1Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable1Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable2Nombre].Value = "Responsable 2";
            ws.Cells[rowIniTabla + 1, colResponsable2Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable2Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable3Nombre].Value = "Responsable 3";
            ws.Cells[rowIniTabla + 1, colResponsable3Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable3Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable4Nombre].Value = "Responsable 4";
            ws.Cells[rowIniTabla + 1, colResponsable4Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable4Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable5Nombre].Value = "Responsable 5";
            ws.Cells[rowIniTabla + 1, colResponsable5Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable5Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colCausaResumida].Value = "Causa resumida de interrupción";
            ws.Cells[rowIniTabla, colEiE].Value = "Ei / E";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento (US$)";


            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCorrelativo, rowIniTabla + 1, colCorrelativo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoCliente, rowIniTabla + 1, colTipoCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNombreCliente, rowIniTabla + 1, colNombreCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPuntoEntregaBarraNombre, rowIniTabla + 1, colPuntoEntregaBarraNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNumSuministroClienteLibre, rowIniTabla + 1, colNumSuministroClienteLibre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNivelTensionNombre, rowIniTabla + 1, colNivelTensionNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionLiteral, rowIniTabla + 1, colAplicacionLiteral);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEnergiaPeriodo, rowIniTabla + 1, colEnergiaPeriodo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colIncrementoTolerancia, rowIniTabla + 1, colIncrementoTolerancia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoNombre, rowIniTabla + 1, colTipoNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaNombre, rowIniTabla + 1, colCausaNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNi, rowIniTabla + 1, colNi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colKi, rowIniTabla + 1, colKi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoIni, rowIniTabla, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoProgramadoIni, rowIniTabla, colTiempoProgramadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable1Nombre, rowIniTabla, colResponsable1Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable2Nombre, rowIniTabla, colResponsable2Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable3Nombre, rowIniTabla, colResponsable3Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable4Nombre, rowIniTabla, colResponsable4Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable5Nombre, rowIniTabla, colResponsable5Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaResumida, rowIniTabla + 1, colCausaResumida);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEiE, rowIniTabla + 1, colEiE);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResarcimiento, rowIniTabla + 1, colResarcimiento);

            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "#1659AA");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento);
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "#FFFFFF");
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento);


            #endregion


            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in lista)
            {
                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colCorrelativo].Value = item.Correlativo != null ? item.Correlativo.Value : item.Correlativo;
                ws.Cells[rowData, colTipoCliente].Value = item.TipoCliente != null ? item.TipoCliente.Trim() : null;
                ws.Cells[rowData, colNombreCliente].Value = item.NombreCliente != null ? item.NombreCliente.Trim() : null;
                ws.Cells[rowData, colPuntoEntregaBarraNombre].Value = item.PuntoEntregaBarraNombre != null ? item.PuntoEntregaBarraNombre.Trim() : null;
                ws.Cells[rowData, colNumSuministroClienteLibre].Value = item.NumSuministroClienteLibre != null ? item.NumSuministroClienteLibre : item.NumSuministroClienteLibre;
                ws.Cells[rowData, colNivelTensionNombre].Value = item.NivelTensionNombre != null ? item.NivelTensionNombre.Trim() : null;
                ws.Cells[rowData, colAplicacionLiteral].Value = item.AplicacionLiteral != null ? item.AplicacionLiteral.Value : item.AplicacionLiteral;
                ws.Cells[rowData, colEnergiaPeriodo].Value = item.EnergiaPeriodo != null ? item.EnergiaPeriodo.Value : item.EnergiaPeriodo;
                ws.Cells[rowData, colEnergiaPeriodo].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colIncrementoTolerancia].Value = item.IncrementoTolerancia != null ? item.IncrementoTolerancia.Trim() : null;
                ws.Cells[rowData, colTipoNombre].Value = item.TipoNombre != null ? item.TipoNombre.Trim() : null;
                ws.Cells[rowData, colCausaNombre].Value = item.CausaNombre != null ? item.CausaNombre.Trim() : null;
                ws.Cells[rowData, colNi].Value = item.Ni != null ? item.Ni.Value : item.Ni;
                ws.Cells[rowData, colNi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colKi].Value = item.Ki != null ? item.Ki.Value : item.Ki;
                ws.Cells[rowData, colKi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colTiempoEjecutadoIni].Value = item.TiempoEjecutadoIni != null ? item.TiempoEjecutadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoIni].Value = item.TiempoProgramadoIni != null ? item.TiempoProgramadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoFin].Value = item.TiempoProgramadoFin != null ? item.TiempoProgramadoFin.Trim() : null;
                ws.Cells[rowData, colResponsable1Nombre].Value = item.Responsable1Nombre != null ? item.Responsable1Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable1Porcentaje].Value = item.Responsable1Porcentaje != null ? item.Responsable1Porcentaje.Value / 100 : item.Responsable1Porcentaje;
                ws.Cells[rowData, colResponsable1Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable2Nombre].Value = item.Responsable2Nombre != null ? item.Responsable2Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable2Porcentaje].Value = item.Responsable2Porcentaje != null ? item.Responsable2Porcentaje.Value / 100 : item.Responsable2Porcentaje;
                ws.Cells[rowData, colResponsable2Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable3Nombre].Value = item.Responsable3Nombre != null ? item.Responsable3Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable3Porcentaje].Value = item.Responsable3Porcentaje != null ? item.Responsable3Porcentaje.Value / 100 : item.Responsable3Porcentaje;
                ws.Cells[rowData, colResponsable3Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable4Nombre].Value = item.Responsable4Nombre != null ? item.Responsable4Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable4Porcentaje].Value = item.Responsable4Porcentaje != null ? item.Responsable4Porcentaje.Value / 100 : item.Responsable4Porcentaje;
                ws.Cells[rowData, colResponsable4Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable5Nombre].Value = item.Responsable5Nombre != null ? item.Responsable5Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable5Porcentaje].Value = item.Responsable5Porcentaje != null ? item.Responsable5Porcentaje.Value / 100 : item.Responsable5Porcentaje;
                ws.Cells[rowData, colResponsable5Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colCausaResumida].Value = item.CausaResumida != null ? item.CausaResumida.Trim() : null;
                ws.Cells[rowData, colEiE].Value = item.EiE != null ? item.EiE.Value : item.EiE;
                ws.Cells[rowData, colEiE].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResarcimiento].Value = item.Resarcimiento != null ? item.Resarcimiento.Value : item.Resarcimiento;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = FormatoNumDecimales(2);

                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colSuministrador, rowData - 1, colResarcimiento, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministrador, rowData - 1, colResarcimiento);
            #endregion

            //filter                       

            if (lista.Any())
            {
                ws.Cells[rowIniTabla, colSuministrador, rowData, colResarcimiento].AutoFitColumns();
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 14;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 18;
                ws.Column(colTiempoEjecutadoFin).Width = 18;
                ws.Column(colTiempoProgramadoIni).Width = 18;
                ws.Column(colTiempoProgramadoFin).Width = 18;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
            }
            else
            {
                ws.Column(colSuministrador).Width = 20;
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNombreCliente).Width = 12;
                ws.Column(colPuntoEntregaBarraNombre).Width = 22;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 12;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colTipoNombre).Width = 10;
                ws.Column(colCausaNombre).Width = 10;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 13;
                ws.Column(colTiempoEjecutadoFin).Width = 12;
                ws.Column(colTiempoProgramadoIni).Width = 13;
                ws.Column(colTiempoProgramadoFin).Width = 12;
                ws.Column(colResponsable1Nombre).Width = 8;
                ws.Column(colResponsable1Porcentaje).Width = 5;
                ws.Column(colResponsable2Nombre).Width = 8;
                ws.Column(colResponsable2Porcentaje).Width = 5;
                ws.Column(colResponsable3Nombre).Width = 8;
                ws.Column(colResponsable3Porcentaje).Width = 5;
                ws.Column(colResponsable4Nombre).Width = 8;
                ws.Column(colResponsable4Porcentaje).Width = 5;
                ws.Column(colResponsable5Nombre).Width = 8;
                ws.Column(colResponsable5Porcentaje).Width = 5;
                ws.Column(colCausaResumida).Width = 27;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }

        /// <summary>
        /// Devuelve el listado comparativo semestral y trimestral para cierto tipo (S/T o T/S)
        /// </summary>
        /// <param name="listaRegistrosPE_Semestral"></param>
        /// <param name="listaRegistrosPE_Trimestral"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<InterrupcionSuministroPE> ObtenerListadoComparativoSemestralTrimestral(List<InterrupcionSuministroPE> listaRegistrosPE_Semestral, List<InterrupcionSuministroPE> listaRegistrosPE_Trimestral, int tipo)
        {
            List<InterrupcionSuministroPE> lstSalida = new List<InterrupcionSuministroPE>();


            if (tipo == ConstantesCalculoResarcimiento.ComparativoSemestralRespectoTrimestral)
            {
                lstSalida = ListarComparativoDosPeriodos(listaRegistrosPE_Semestral, listaRegistrosPE_Trimestral);
            }
            else
            {
                if (tipo == ConstantesCalculoResarcimiento.ComparativoTrimestralRespectoSemestral)
                {
                    lstSalida = ListarComparativoDosPeriodos(listaRegistrosPE_Trimestral, listaRegistrosPE_Semestral);
                }
            }

            return lstSalida;
        }

        /// <summary>
        /// Deveuvele la comparacion de interrupciones de dos periodos
        /// </summary>
        /// <param name="lstInterrupciones1"></param>
        /// <param name="lstInterrupciones2"></param>
        /// <returns></returns>
        public List<InterrupcionSuministroPE> ListarComparativoDosPeriodos(List<InterrupcionSuministroPE> lstInterrupciones1, List<InterrupcionSuministroPE> lstInterrupciones2)
        {
            List<InterrupcionSuministroPE> lstSalida = new List<InterrupcionSuministroPE>();

            foreach (var interrupcion1 in lstInterrupciones1)
            {
                string cliente = interrupcion1.NombreCliente.Trim();
                string ptoEntrega = interrupcion1.PuntoEntregaBarraNombre.Trim();
                string nroSuministro = interrupcion1.NumSuministroClienteLibre;
                string nivelT = interrupcion1.NivelTensionNombre.Trim();
                string tipo = interrupcion1.TipoNombre.Trim();
                string causa = interrupcion1.CausaNombre.Trim();
                int? aplicacionLit = interrupcion1.AplicacionLiteral;
                string incrementoToler = interrupcion1.IncrementoTolerancia.Trim();
                string ejecFechaIni = interrupcion1.TiempoEjecutadoIni.Trim();
                string ejecFechaFin = interrupcion1.TiempoEjecutadoFin.Trim();
                string progFechaIni = interrupcion1.TiempoProgramadoIni.Trim();
                string progFechaFin = interrupcion1.TiempoProgramadoFin.Trim();
                string resp1Nomb = interrupcion1.Responsable1Nombre.Trim();
                decimal? resp1Porcen = interrupcion1.Responsable1Porcentaje;
                string resp2Nomb = interrupcion1.Responsable2Nombre.Trim();
                decimal? resp2Porcen = interrupcion1.Responsable2Porcentaje;
                string resp3Nomb = interrupcion1.Responsable3Nombre.Trim();
                decimal? resp3Porcen = interrupcion1.Responsable3Porcentaje;
                string resp4Nomb = interrupcion1.Responsable4Nombre.Trim();
                decimal? resp4Porcen = interrupcion1.Responsable4Porcentaje;
                string resp5Nomb = interrupcion1.Responsable5Nombre.Trim();
                decimal? resp5Porcen = interrupcion1.Responsable5Porcentaje;

                List<InterrupcionSuministroPE> listaIgualesEnPeriodo2 = lstInterrupciones2.Where(x => x.NombreCliente == cliente && x.PuntoEntregaBarraNombre == ptoEntrega &&
                x.NumSuministroClienteLibre == nroSuministro && x.NivelTensionNombre == nivelT && x.TipoNombre == tipo && x.CausaNombre == causa &&
                x.AplicacionLiteral == aplicacionLit && x.IncrementoTolerancia == incrementoToler && x.TiempoEjecutadoIni == ejecFechaIni &&
                x.TiempoEjecutadoFin == ejecFechaFin && x.TiempoProgramadoIni == progFechaIni && x.TiempoProgramadoFin == progFechaFin &&
                x.Responsable1Nombre == resp1Nomb && x.Responsable1Porcentaje == resp1Porcen && x.Responsable2Nombre == resp2Nomb &&
                x.Responsable2Porcentaje == resp2Porcen && x.Responsable3Nombre == resp3Nomb && x.Responsable3Porcentaje == resp3Porcen &&
                x.Responsable4Nombre == resp4Nomb && x.Responsable4Porcentaje == resp4Porcen && x.Responsable5Nombre == resp5Nomb &&
                x.Responsable5Porcentaje == resp5Porcen).ToList();

                //lo tomo en cuenta si no existe en el 2do periodo
                if (!listaIgualesEnPeriodo2.Any())
                {
                    lstSalida.Add(interrupcion1);
                }
            }

            return lstSalida;
        }
        #endregion

        #endregion

        #region Reportes en word

        #region General

        /// <summary>
        /// Devuelve el nombre de cierto reporte word
        /// </summary>
        /// <param name="codigoReporte"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public string ObtenerNombreReporteEnWord(RePeriodoDTO periodo)
        {
            bool esPeriodoRevision = periodo.Reperrevision == "S" ? true : false;
            string salida = "";

            if (esPeriodoRevision)
                salida = "Informe Resarcimiento Revisión.docx";
            else
                salida = "Informe Resarcimiento.docx";

            return salida;
        }

        /// <summary>
        /// Genera el reporte word
        /// </summary>
        /// <param name="codigoReporte"></param>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="nameFile"></param>
        public void GenerarReporteEnWord(RePeriodoDTO periodo, int codigoReporte, string rutaSalida, string pathLogo, string nameFile, string pathPlantilla)
        {
            bool esPeriodoRevision = periodo.Reperrevision == "S" ? true : false;
            ////Descargo archivo segun requieran
            string rutaFileOutput = rutaSalida + nameFile;
            string rutaArchivoPlantilla = pathPlantilla + nameFile;

            FileInfo newFile = new FileInfo(rutaFileOutput);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFileOutput);
            }

            if (esPeriodoRevision)
            {
                GenerarArchivoWordInformeResarcimientoRevisionDevExp(periodo, rutaArchivoPlantilla, rutaFileOutput);
            }
            else
            {
                GenerarArchivoWordInformeResarcimientoDevExp(periodo, rutaArchivoPlantilla, rutaFileOutput);
            }


        }

        #endregion

        #region Reporte Word Informe Resarcimiento

        /// <summary>
        /// Devuelve el listado de registros para el reporte Informe Final 
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<DatoPorSuministrador> ObtenerListadoInformeFinal(RePeriodoDTO periodo)
        {
            List<DatoPorSuministrador> lstSalida = new List<DatoPorSuministrador>();

            List<int> lstSuministradoresID = new List<int>();

            //Para obtener nombres
            EstructuraInterrupcion maestros = new EstructuraInterrupcion();
            maestros.ListaEmpresa = this.ObtenerEmpresasSuministradorasTotal();


            //Para pestaña EventosRC. Obtengo el listado de eventos para cierto periodo
            List<EventoRC> listaEventosRC = ObtenerListaEventosPorPeriodo(periodo);

            //Para pestaña MT
            //Obtengo la parte necesaria del listado de la pestaña PE (interrupciones por punto de entrega)
            List<InterrupcionSuministroPE> listaRegistrosPE_F = ObtenerListaInterrupcionesPEPorPeriodo(periodo, false, null);
            //Obtengo la parte necesaria del listado de la pestaña RC (interrupciones por rechazo de carga)
            List<InterrupcionSuministroRC> listaRegistrosRC_F = ObtenerListaInterrupcionesRCPorPeriodo(periodo, false, null);

            //Para pestaña LIMTX
            List<LimiteIngreso> lista_Limtx = ObtenerListaLimiteTransmisionPorPeriodo(periodo, listaRegistrosPE_F, listaRegistrosRC_F, listaEventosRC);

            //Para pestaña PE_FINAL
            List<InterrupcionSuministroPE> listaRegistrosPE = ObtenerListaInterrupcionesPEPorPeriodo(periodo, true, lista_Limtx);
            List<int> lstPESuministradorID = listaRegistrosPE.Select(x => x.SuministradorId).Distinct().ToList();

            //Para pestaña RC_FINAL
            List<InterrupcionSuministroRC> listaRegistrosRC = ObtenerListaInterrupcionesRCPorPeriodo(periodo, true, lista_Limtx);
            List<int> lstRCSuministradorID = listaRegistrosRC.Select(x => x.SuministradorId).Distinct().ToList();

            lstSuministradoresID.AddRange(lstPESuministradorID);
            lstSuministradoresID.AddRange(lstRCSuministradorID);
            lstSuministradoresID = lstSuministradoresID.Distinct().ToList();


            #region Datos 

            //foreach (var itemPE in lstPEAgrupadaPorSuministrador)
            foreach (var suministradorID in lstSuministradoresID)
            {
                ReEmpresaDTO sum = maestros.ListaEmpresa.Find(x => x.Emprcodi == suministradorID);

                InterrupcionSuministroPE objPE = listaRegistrosPE.Find(x => x.SuministradorId == suministradorID);
                InterrupcionSuministroRC objRC = listaRegistrosRC.Find(x => x.SuministradorId == suministradorID);

                List<RegistroWord> lstPEWord = new List<RegistroWord>();
                List<RegistroWord> lstRCWord = new List<RegistroWord>();

                List<RegistroWord> lstPEHorizontalXSum = new List<RegistroWord>();
                List<RegistroWord> lstPEVerticalXSum = new List<RegistroWord>();
                List<RegistroWord> lstRCHorizontalXSum = new List<RegistroWord>();
                List<RegistroWord> lstRCVerticalXSum = new List<RegistroWord>();

                #region Datos para Punto de Entrega

                List<InterrupcionSuministroPE> lstPEPorSuministrador = listaRegistrosPE.Where(x => x.SuministradorId == suministradorID).ToList();

                //Agrupamos por cliente y barra
                //var lstPEAgrupadaPorSuministradorBarraClienteYResponsables = lstPEPorSuministrador.GroupBy(x => new { x.NombreCliente, x.PuntoEntregaBarraNombre, x.Responsable1Id,
                //    x.Responsable2Id, x.Responsable3Id, x.Responsable4Id, x.Responsable5Id }).ToList();

                var lstPEAgrupadaPorSuministradorBarraClienteYResponsables = lstPEPorSuministrador.GroupBy(x => new
                {
                    x.NombreCliente,
                    x.PuntoEntregaBarraNombre,
                    x.Responsable1Id,
                    x.ARDF1,
                    x.Responsable2Id,
                    x.ARDF2,
                    x.Responsable3Id,
                    x.ARDF3,
                    x.Responsable4Id,
                    x.ARDF4,
                    x.Responsable5Id,
                    x.ARDF5
                }).ToList();

                foreach (var itemPE_SBC in lstPEAgrupadaPorSuministradorBarraClienteYResponsables)
                {
                    //grupo por cliente, barra y responsables
                    //List<InterrupcionSuministroPE> lstPEPorSuministradorBarraClienteResp = lstPEPorSuministrador.Where(x => x.NombreCliente == itemPE_SBC.Key.NombreCliente && x.PuntoEntregaBarraNombre == itemPE_SBC.Key.PuntoEntregaBarraNombre &&
                    //                                                x.Responsable1Id == itemPE_SBC.Key.Responsable1Id && x.Responsable2Id == itemPE_SBC.Key.Responsable2Id && x.Responsable3Id == itemPE_SBC.Key.Responsable3Id &&
                    //                                                x.Responsable4Id == itemPE_SBC.Key.Responsable4Id && x.Responsable5Id == itemPE_SBC.Key.Responsable5Id).ToList();

                    List<InterrupcionSuministroPE> lstPEPorSuministradorBarraClienteResp = lstPEPorSuministrador.Where(x => x.NombreCliente == itemPE_SBC.Key.NombreCliente && x.PuntoEntregaBarraNombre == itemPE_SBC.Key.PuntoEntregaBarraNombre &&
                                                                    x.Responsable1Id == itemPE_SBC.Key.Responsable1Id && x.ARDF1 == itemPE_SBC.Key.ARDF1 &&
                                                                    x.Responsable2Id == itemPE_SBC.Key.Responsable2Id && x.ARDF2 == itemPE_SBC.Key.ARDF2 &&
                                                                    x.Responsable3Id == itemPE_SBC.Key.Responsable3Id && x.ARDF3 == itemPE_SBC.Key.ARDF3 &&
                                                                    x.Responsable4Id == itemPE_SBC.Key.Responsable4Id && x.ARDF4 == itemPE_SBC.Key.ARDF4 &&
                                                                    x.Responsable5Id == itemPE_SBC.Key.Responsable5Id && x.ARDF5 == itemPE_SBC.Key.ARDF5).ToList();

                    if (lstPEPorSuministradorBarraClienteResp.Any())
                    {
                        RegistroWord regWordPE = new RegistroWord();

                        regWordPE.NombreCliente = itemPE_SBC.Key.NombreCliente;
                        regWordPE.BarraNombre = itemPE_SBC.Key.PuntoEntregaBarraNombre;

                        regWordPE.Responsable1Id = lstPEPorSuministradorBarraClienteResp.First().Responsable1Id;
                        regWordPE.Responsable1Nombre = lstPEPorSuministradorBarraClienteResp.First().Responsable1Nombre;
                        regWordPE.SumAgenteResp1 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AgenteResp1);
                        regWordPE.SumAplicacionAResp1 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp1);
                        //regWordPE.ListaDisposicionFinalR1 = regWordPE.Responsable1Id != null ? (lstPEPorSuministradorBarraClienteResp.Where(x => x.Responsable1DispFinal != null && x.Responsable1DispFinal != "").Select(x => x.Responsable1DispFinal.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordPE.FactorR1 = regWordPE.Responsable1Id != null ? (regWordPE.ListaDisposicionFinalR1.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable1Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable1Id).Ajuste): null) : null) : null;
                        bool tieneDFResp1 = regWordPE.Responsable1Id != null ? (lstPEPorSuministradorBarraClienteResp.Find(x => x.Responsable1DispFinal == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp1 = lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable1Id);
                        regWordPE.FactorR1 = (regWordPE.SumAgenteResp1 != null && regWordPE.SumAgenteResp1 != 0) ? (regWordPE.Responsable1Id != null ? (tieneDFResp1 ? (limIngResp1 != null ? limIngResp1.Ajuste : null) : null) : null) : null;

                        regWordPE.Responsable2Id = lstPEPorSuministradorBarraClienteResp.First().Responsable2Id;
                        regWordPE.Responsable2Nombre = lstPEPorSuministradorBarraClienteResp.First().Responsable2Nombre;
                        regWordPE.SumAgenteResp2 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AgenteResp2);
                        regWordPE.SumAplicacionAResp2 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp2);
                        //regWordPE.ListaDisposicionFinalR2 = regWordPE.Responsable2Id != null ? (lstPEPorSuministradorBarraClienteResp.Where(x => x.Responsable2DispFinal != null && x.Responsable2DispFinal != "").Select(x => x.Responsable2DispFinal.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordPE.FactorR2 = regWordPE.Responsable2Id != null ? (regWordPE.ListaDisposicionFinalR2.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable2Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable2Id).Ajuste): null) : null) : null;
                        bool tieneDFResp2 = regWordPE.Responsable2Id != null ? (lstPEPorSuministradorBarraClienteResp.Find(x => x.Responsable2DispFinal == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp2 = lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable2Id);
                        regWordPE.FactorR2 = (regWordPE.SumAgenteResp2 != null && regWordPE.SumAgenteResp2 != 0) ? (regWordPE.Responsable2Id != null ? (tieneDFResp2 ? (limIngResp2 != null ? limIngResp2.Ajuste : null) : null) : null) : null;


                        regWordPE.Responsable3Id = lstPEPorSuministradorBarraClienteResp.First().Responsable3Id;
                        regWordPE.Responsable3Nombre = lstPEPorSuministradorBarraClienteResp.First().Responsable3Nombre;
                        regWordPE.SumAgenteResp3 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AgenteResp3);
                        regWordPE.SumAplicacionAResp3 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp3);
                        //regWordPE.ListaDisposicionFinalR3 = regWordPE.Responsable3Id != null ? (lstPEPorSuministradorBarraClienteResp.Where(x => x.Responsable3DispFinal != null && x.Responsable3DispFinal != "").Select(x => x.Responsable3DispFinal.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordPE.FactorR3 = regWordPE.Responsable3Id != null ? (regWordPE.ListaDisposicionFinalR3.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable3Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable3Id).Ajuste) : null) : null) : null;
                        bool tieneDFResp3 = regWordPE.Responsable3Id != null ? (lstPEPorSuministradorBarraClienteResp.Find(x => x.Responsable3DispFinal == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp3 = lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable3Id);
                        regWordPE.FactorR3 = (regWordPE.SumAgenteResp3 != null && regWordPE.SumAgenteResp3 != 0) ? (regWordPE.Responsable3Id != null ? (tieneDFResp3 ? (limIngResp3 != null ? limIngResp3.Ajuste : null) : null) : null) : null;

                        regWordPE.Responsable4Id = lstPEPorSuministradorBarraClienteResp.First().Responsable4Id;
                        regWordPE.Responsable4Nombre = lstPEPorSuministradorBarraClienteResp.First().Responsable4Nombre;
                        regWordPE.SumAgenteResp4 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AgenteResp4);
                        regWordPE.SumAplicacionAResp4 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp4);
                        //regWordPE.ListaDisposicionFinalR4 = regWordPE.Responsable4Id != null ? (lstPEPorSuministradorBarraClienteResp.Where(x => x.Responsable4DispFinal != null && x.Responsable4DispFinal != "").Select(x => x.Responsable4DispFinal.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordPE.FactorR4 = regWordPE.Responsable4Id != null ? (regWordPE.ListaDisposicionFinalR4.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable4Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable4Id).Ajuste) : null) : null) : null;
                        bool tieneDFResp4 = regWordPE.Responsable4Id != null ? (lstPEPorSuministradorBarraClienteResp.Find(x => x.Responsable4DispFinal == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp4 = lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable4Id);
                        regWordPE.FactorR4 = (regWordPE.SumAgenteResp4 != null && regWordPE.SumAgenteResp4 != 0) ? (regWordPE.Responsable4Id != null ? (tieneDFResp4 ? (limIngResp4 != null ? limIngResp4.Ajuste : null) : null) : null) : null;

                        regWordPE.Responsable5Id = lstPEPorSuministradorBarraClienteResp.First().Responsable5Id;
                        regWordPE.Responsable5Nombre = lstPEPorSuministradorBarraClienteResp.First().Responsable5Nombre;
                        regWordPE.SumAgenteResp5 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AgenteResp5);
                        regWordPE.SumAplicacionAResp5 = lstPEPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp5);
                        //regWordPE.ListaDisposicionFinalR5 = regWordPE.Responsable5Id != null ? (lstPEPorSuministradorBarraClienteResp.Where(x => x.Responsable5DispFinal != null && x.Responsable5DispFinal != "").Select(x => x.Responsable5DispFinal.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordPE.FactorR5 = regWordPE.Responsable2Id != null ? (regWordPE.ListaDisposicionFinalR5.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable5Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable5Id).Ajuste) : null) : null) : null;
                        bool tieneDFResp5 = regWordPE.Responsable5Id != null ? (lstPEPorSuministradorBarraClienteResp.Find(x => x.Responsable5DispFinal == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp5 = lista_Limtx.Find(x => x.EmpresaId == regWordPE.Responsable5Id);
                        regWordPE.FactorR5 = (regWordPE.SumAgenteResp5 != null && regWordPE.SumAgenteResp5 != 0) ? (regWordPE.Responsable5Id != null ? (tieneDFResp5 ? (limIngResp5 != null ? limIngResp5.Ajuste : null) : null) : null) : null;

                        //Obtengo numero de responsables 
                        int numResp = 0;
                        if (regWordPE.Responsable1Id != null) numResp++;
                        if (regWordPE.Responsable2Id != null) numResp++;
                        if (regWordPE.Responsable3Id != null) numResp++;
                        if (regWordPE.Responsable4Id != null) numResp++;
                        if (regWordPE.Responsable5Id != null) numResp++;

                        regWordPE.numResponsables = numResp;

                        lstPEWord.Add(regWordPE);
                    }
                }

                //Separamos por cantidad de responsables
                List<RegistroWord> lstPEMenos4Responsables = new List<RegistroWord>();
                List<RegistroWord> lstPEMas3Responsables = new List<RegistroWord>();

                foreach (var item in lstPEWord)
                {
                    if (item.numResponsables <= 3)
                    {
                        lstPEMenos4Responsables.Add(item);
                    }
                    else
                    {
                        if (item.numResponsables > 3)
                        {
                            lstPEMas3Responsables.Add(item);
                        }
                    }
                }

                lstPEHorizontalXSum = lstPEMenos4Responsables;
                lstPEVerticalXSum = lstPEMas3Responsables;

                //Hallamos las sumas totales para las horizontales
                decimal? SumTAgResp1 = lstPEHorizontalXSum.Sum(x => x.SumAgenteResp1);
                decimal? SumTAplicAgResp1 = lstPEHorizontalXSum.Sum(x => x.SumAplicacionAResp1);
                decimal? SumTAgResp2 = lstPEHorizontalXSum.Sum(x => x.SumAgenteResp2);
                decimal? SumTAplicAgResp2 = lstPEHorizontalXSum.Sum(x => x.SumAplicacionAResp2);
                decimal? SumTAgResp3 = lstPEHorizontalXSum.Sum(x => x.SumAgenteResp3);
                decimal? SumTAplicAgResp3 = lstPEHorizontalXSum.Sum(x => x.SumAplicacionAResp3);

                Totales sumasTotalesHorizonta = new Totales();
                sumasTotalesHorizonta.SumTotalAgenteResp1 = SumTAgResp1;
                sumasTotalesHorizonta.SumTotalAplicacionAResp1 = SumTAplicAgResp1;
                sumasTotalesHorizonta.SumTotalAgenteResp2 = SumTAgResp2;
                sumasTotalesHorizonta.SumTotalAplicacionAResp2 = SumTAplicAgResp2;
                sumasTotalesHorizonta.SumTotalAgenteResp3 = SumTAgResp3;
                sumasTotalesHorizonta.SumTotalAplicacionAResp3 = SumTAplicAgResp3;

                #endregion

                #region Datos para Rechazo carga

                List<InterrupcionSuministroRC> lstRCPorSuministrador = listaRegistrosRC.Where(x => x.SuministradorId == suministradorID).ToList();

                //Agrupamos por cliente y barra
                var lstRCAgrupadaPorSuministradorBarraClienteYResponsables = lstRCPorSuministrador.GroupBy(x => new
                {
                    x.NombreCliente,
                    x.BarraNombre,
                    x.Responsable1Id,
                    x.DFAR1,
                    x.Responsable2Id,
                    x.DFAR2,
                    x.Responsable3Id,
                    x.DFAR3,
                    x.Responsable4Id,
                    x.DFAR4,
                    x.Responsable5Id,
                    x.DFAR5
                }).ToList();

                foreach (var itemRC_SBC in lstRCAgrupadaPorSuministradorBarraClienteYResponsables)
                {
                    //grupo por cliente, barra y responsables
                    //List<InterrupcionSuministroRC> lstRCPorSuministradorBarraClienteResp = lstRCPorSuministrador.Where(x => x.NombreCliente == itemRC_SBC.Key.NombreCliente && x.BarraNombre == itemRC_SBC.Key.BarraNombre &&
                    //                                                x.Responsable1Id == itemRC_SBC.Key.Responsable1Id && x.Responsable2Id == itemRC_SBC.Key.Responsable2Id && x.Responsable3Id == itemRC_SBC.Key.Responsable3Id &&
                    //                                                x.Responsable4Id == itemRC_SBC.Key.Responsable4Id && x.Responsable5Id == itemRC_SBC.Key.Responsable5Id).ToList();

                    List<InterrupcionSuministroRC> lstRCPorSuministradorBarraClienteResp = lstRCPorSuministrador.Where(x => x.NombreCliente == itemRC_SBC.Key.NombreCliente && x.BarraNombre == itemRC_SBC.Key.BarraNombre &&
                                                                    x.Responsable1Id == itemRC_SBC.Key.Responsable1Id && x.DFAR1 == itemRC_SBC.Key.DFAR1 &&
                                                                    x.Responsable2Id == itemRC_SBC.Key.Responsable2Id && x.DFAR2 == itemRC_SBC.Key.DFAR2 &&
                                                                    x.Responsable3Id == itemRC_SBC.Key.Responsable3Id && x.DFAR3 == itemRC_SBC.Key.DFAR3 &&
                                                                    x.Responsable4Id == itemRC_SBC.Key.Responsable4Id && x.DFAR4 == itemRC_SBC.Key.DFAR4 &&
                                                                    x.Responsable5Id == itemRC_SBC.Key.Responsable5Id && x.DFAR5 == itemRC_SBC.Key.DFAR5).ToList();


                    if (lstRCPorSuministradorBarraClienteResp.Any())
                    {
                        //Separo los que tienen disposicion Final y los que no lo tienen
                        //List<InterrupcionSuministroRC> lstRCPorSuministradorBarraClienteRespSi = lstRCPorSuministradorBarraClienteResp.Where()

                        RegistroWord regWordRC = new RegistroWord();

                        regWordRC.NombreCliente = itemRC_SBC.Key.NombreCliente;
                        regWordRC.BarraNombre = itemRC_SBC.Key.BarraNombre;

                        regWordRC.Responsable1Id = lstRCPorSuministradorBarraClienteResp.First().Responsable1Id;
                        regWordRC.Responsable1Nombre = lstRCPorSuministradorBarraClienteResp.First().AgResponsable1Nombre;
                        regWordRC.SumAgenteResp1 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AgResponsable1USD);
                        regWordRC.SumAplicacionAResp1 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp1);
                        //regWordRC.ListaDisposicionFinalR1 = regWordRC.Responsable1Id != null ? (lstRCPorSuministradorBarraClienteResp.Where(x => x.DispFinalAResp1 != null && x.DispFinalAResp1 != "").Select(x => x.DispFinalAResp1.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordRC.FactorR1 = regWordRC.Responsable1Id != null ? (regWordRC.ListaDisposicionFinalR1.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable1Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable1Id).Ajuste) : null) : null) : null;
                        bool tieneDFResp1 = regWordRC.Responsable1Id != null ? (lstRCPorSuministradorBarraClienteResp.Find(x => x.DispFinalAResp1 == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp1 = lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable1Id);
                        regWordRC.FactorR1 = (regWordRC.SumAgenteResp1 != null && regWordRC.SumAgenteResp1 != 0) ? (regWordRC.Responsable1Id != null ? (tieneDFResp1 ? (limIngResp1 != null ? limIngResp1.Ajuste : null) : null) : null) : null;

                        regWordRC.Responsable2Id = lstRCPorSuministradorBarraClienteResp.First().Responsable2Id;
                        regWordRC.Responsable2Nombre = lstRCPorSuministradorBarraClienteResp.First().AgResponsable2Nombre;
                        regWordRC.SumAgenteResp2 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AgResponsable2USD);
                        regWordRC.SumAplicacionAResp2 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp2);
                        //regWordRC.ListaDisposicionFinalR2 = regWordRC.Responsable2Id != null ? (lstRCPorSuministradorBarraClienteResp.Where(x => x.DispFinalAResp2 != null && x.DispFinalAResp2 != "").Select(x => x.DispFinalAResp2.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordRC.FactorR2 = regWordRC.Responsable2Id != null ? (regWordRC.ListaDisposicionFinalR2.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable2Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable2Id).Ajuste) : null) : null) : null;
                        bool tieneDFResp2 = regWordRC.Responsable2Id != null ? (lstRCPorSuministradorBarraClienteResp.Find(x => x.DispFinalAResp2 == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp2 = lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable2Id);
                        regWordRC.FactorR2 = (regWordRC.SumAgenteResp2 != null && regWordRC.SumAgenteResp2 != 0) ? (regWordRC.Responsable2Id != null ? (tieneDFResp2 ? (limIngResp2 != null ? limIngResp2.Ajuste : null) : null) : null) : null;

                        regWordRC.Responsable3Id = lstRCPorSuministradorBarraClienteResp.First().Responsable3Id;
                        regWordRC.Responsable3Nombre = lstRCPorSuministradorBarraClienteResp.First().AgResponsable3Nombre;
                        regWordRC.SumAgenteResp3 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AgResponsable3USD);
                        regWordRC.SumAplicacionAResp3 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp3);
                        //regWordRC.ListaDisposicionFinalR3 = regWordRC.Responsable3Id != null ? (lstRCPorSuministradorBarraClienteResp.Where(x => x.DispFinalAResp3 != null && x.DispFinalAResp3 != "").Select(x => x.DispFinalAResp3.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordRC.FactorR3 = regWordRC.Responsable3Id != null ? (regWordRC.ListaDisposicionFinalR3.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable3Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable3Id).Ajuste) : null) : null) : null;
                        bool tieneDFResp3 = regWordRC.Responsable3Id != null ? (lstRCPorSuministradorBarraClienteResp.Find(x => x.DispFinalAResp3 == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp3 = lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable3Id);
                        regWordRC.FactorR3 = (regWordRC.SumAgenteResp3 != null && regWordRC.SumAgenteResp3 != 0) ? (regWordRC.Responsable3Id != null ? (tieneDFResp3 ? (limIngResp3 != null ? limIngResp3.Ajuste : null) : null) : null) : null;

                        regWordRC.Responsable4Id = lstRCPorSuministradorBarraClienteResp.First().Responsable4Id;
                        regWordRC.Responsable4Nombre = lstRCPorSuministradorBarraClienteResp.First().AgResponsable4Nombre;
                        regWordRC.SumAgenteResp4 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AgResponsable4USD);
                        regWordRC.SumAplicacionAResp4 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp4);
                        //regWordRC.ListaDisposicionFinalR4 = regWordRC.Responsable4Id != null ? (lstRCPorSuministradorBarraClienteResp.Where(x => x.DispFinalAResp4 != null && x.DispFinalAResp4 != "").Select(x => x.DispFinalAResp4.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordRC.FactorR4 = regWordRC.Responsable4Id != null ? (regWordRC.ListaDisposicionFinalR4.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable4Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable4Id).Ajuste) : null) : null) : null;
                        bool tieneDFResp4 = regWordRC.Responsable4Id != null ? (lstRCPorSuministradorBarraClienteResp.Find(x => x.DispFinalAResp4 == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp4 = lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable4Id);
                        regWordRC.FactorR4 = (regWordRC.SumAgenteResp4 != null && regWordRC.SumAgenteResp4 != 0) ? (regWordRC.Responsable4Id != null ? (tieneDFResp4 ? (limIngResp4 != null ? limIngResp4.Ajuste : null) : null) : null) : null;

                        regWordRC.Responsable5Id = lstRCPorSuministradorBarraClienteResp.First().Responsable5Id;
                        regWordRC.Responsable5Nombre = lstRCPorSuministradorBarraClienteResp.First().AgResponsable5Nombre;
                        regWordRC.SumAgenteResp5 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AgResponsable5USD);
                        regWordRC.SumAplicacionAResp5 = lstRCPorSuministradorBarraClienteResp.Sum(x => x.AplicacionAResp5);
                        //regWordRC.ListaDisposicionFinalR5 = regWordRC.Responsable5Id != null ? (lstRCPorSuministradorBarraClienteResp.Where(x => x.DispFinalAResp5 != null && x.DispFinalAResp5 != "").Select(x => x.DispFinalAResp5.Trim()).Distinct().ToList()) : new List<string>();
                        //regWordRC.FactorR5 = regWordRC.Responsable5Id != null ? (regWordRC.ListaDisposicionFinalR5.Where(x => x == "S").ToList().Any() ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable5Id) != null ? (lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable5Id).Ajuste) : null) : null) : null;
                        bool tieneDFResp5 = regWordRC.Responsable5Id != null ? (lstRCPorSuministradorBarraClienteResp.Find(x => x.DispFinalAResp5 == "S") != null ? true : false) : false;
                        LimiteIngreso limIngResp5 = lista_Limtx.Find(x => x.EmpresaId == regWordRC.Responsable5Id);
                        regWordRC.FactorR5 = (regWordRC.SumAgenteResp5 != null && regWordRC.SumAgenteResp5 != 0) ? (regWordRC.Responsable5Id != null ? (tieneDFResp5 ? (limIngResp5 != null ? limIngResp5.Ajuste : null) : null) : null) : null;


                        //Obtengo numero de responsables
                        int numResp = 0;
                        if (regWordRC.Responsable1Id != null) numResp++;
                        if (regWordRC.Responsable2Id != null) numResp++;
                        if (regWordRC.Responsable3Id != null) numResp++;
                        if (regWordRC.Responsable4Id != null) numResp++;
                        if (regWordRC.Responsable5Id != null) numResp++;

                        regWordRC.numResponsables = numResp;

                        lstRCWord.Add(regWordRC);
                    }
                }

                //Separamos por cantidad de responsables
                List<RegistroWord> lstRCMenos4Responsables = new List<RegistroWord>();
                List<RegistroWord> lstRCMas3Responsables = new List<RegistroWord>();

                foreach (var item in lstRCWord)
                {
                    if (item.numResponsables <= 3)
                    {
                        lstRCMenos4Responsables.Add(item);
                    }
                    else
                    {
                        if (item.numResponsables > 3)
                        {
                            lstRCMas3Responsables.Add(item);
                        }
                    }
                }

                lstRCHorizontalXSum = lstRCMenos4Responsables;
                lstRCVerticalXSum = lstRCMas3Responsables;

                //Hallamos las sumas totales para las horizontales
                decimal? SumTRCAgResp1 = lstRCHorizontalXSum.Sum(x => x.SumAgenteResp1);
                decimal? SumTRCAplicAgResp1 = lstRCHorizontalXSum.Sum(x => x.SumAplicacionAResp1);
                decimal? SumTRCAgResp2 = lstRCHorizontalXSum.Sum(x => x.SumAgenteResp2);
                decimal? SumTRCAplicAgResp2 = lstRCHorizontalXSum.Sum(x => x.SumAplicacionAResp2);
                decimal? SumTRCAgResp3 = lstRCHorizontalXSum.Sum(x => x.SumAgenteResp3);
                decimal? SumTRCAplicAgResp3 = lstRCHorizontalXSum.Sum(x => x.SumAplicacionAResp3);

                Totales sumasTotalesHorizontaRC = new Totales();
                sumasTotalesHorizontaRC.SumTotalAgenteResp1 = SumTRCAgResp1;
                sumasTotalesHorizontaRC.SumTotalAplicacionAResp1 = SumTRCAplicAgResp1;
                sumasTotalesHorizontaRC.SumTotalAgenteResp2 = SumTRCAgResp2;
                sumasTotalesHorizontaRC.SumTotalAplicacionAResp2 = SumTRCAplicAgResp2;
                sumasTotalesHorizontaRC.SumTotalAgenteResp3 = SumTRCAgResp3;
                sumasTotalesHorizontaRC.SumTotalAplicacionAResp3 = SumTRCAplicAgResp3;


                #endregion

                //LLenamos data General
                DatoPorSuministrador objDatoXSuminitrador = new DatoPorSuministrador();

                objDatoXSuminitrador.SuministradorId = suministradorID;
                objDatoXSuminitrador.SuministradorNombre = sum != null ? sum.Emprnomb.Trim() : (objPE != null ? objPE.Suministrador : (objRC != null ? objRC.Suministrador : ""));
                objDatoXSuminitrador.ListaPEHorizontal = lstPEHorizontalXSum.OrderBy(x => x.NombreCliente).ThenBy(x => x.BarraNombre).ToList();
                objDatoXSuminitrador.ListaPEVertical = lstPEVerticalXSum.OrderBy(x => x.NombreCliente).ThenBy(x => x.BarraNombre).ToList();
                objDatoXSuminitrador.ListaRCHorizontal = lstRCHorizontalXSum.OrderBy(x => x.NombreCliente).ThenBy(x => x.BarraNombre).ToList();
                objDatoXSuminitrador.ListaRCVertical = lstRCVerticalXSum.OrderBy(x => x.NombreCliente).ThenBy(x => x.BarraNombre).ToList();
                objDatoXSuminitrador.NumResponsablesTablaHorizontalPE = lstPEHorizontalXSum.Any() ? lstPEHorizontalXSum.Max(x => x.numResponsables) : 0;
                objDatoXSuminitrador.NumResponsablesTablaHorizontalRC = lstRCHorizontalXSum.Any() ? lstRCHorizontalXSum.Max(x => x.numResponsables) : 0;
                objDatoXSuminitrador.NumFilasTablaHorizontalPE = lstPEHorizontalXSum.Count;
                objDatoXSuminitrador.NumFilasTablaHorizontalRC = lstRCHorizontalXSum.Count;
                objDatoXSuminitrador.TotalPE3Menos = sumasTotalesHorizonta;
                objDatoXSuminitrador.TotalRC3Menos = sumasTotalesHorizontaRC;

                lstSalida.Add(objDatoXSuminitrador);
            }

            #endregion

            return lstSalida.OrderBy(x => x.SuministradorNombre).ToList();
        }


        /// <summary>
        /// Devuelve el numero del periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public string ObtenerNumeroPeriodo(RePeriodoDTO periodo)
        {
            string salida = "";
            DateTime fecIniPeriodo = periodo.Reperfecinicio.Value;

            if (periodo.Repertipo == "S")
            {
                if (fecIniPeriodo.Month == 1)
                    salida = "Primer";
                if (fecIniPeriodo.Month == 7)
                    salida = "Segundo";
            }
            else
            {
                if (periodo.Repertipo == "T")
                {
                    if (fecIniPeriodo.Month == 1)
                        salida = "Primer";
                    if (fecIniPeriodo.Month == 4)
                        salida = "Segundo";
                    if (fecIniPeriodo.Month == 7)
                        salida = "Tercer";
                    if (fecIniPeriodo.Month == 10)
                        salida = "Cuarto";
                }
            }
            return salida;
        }

        /// <summary>
        /// Devuelve el numero de revision para un periodo con revision
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public int ObtenerNumeroRevision(RePeriodoDTO periodo)
        {
            int salida = -1;

            int? periodoPadreId = periodo.Reperpadre;

            if (periodoPadreId != null && periodoPadreId != 0)
            {
                List<RePeriodoDTO> periodosTotales = FactorySic.GetRePeriodoRepository().List();
                List<RePeriodoDTO> periodosSemestralesActivosDelPadre = periodosTotales.Where(x => x.Reperpadre == periodoPadreId && x.Repertipo == "S" && x.Reperrevision == "S"
                                                                                                   && x.Reperestado == "A").OrderBy(x => x.Reperfeccreacion).ToList();
                int orden = 1;
                foreach (var item in periodosSemestralesActivosDelPadre)
                {
                    if (item.Repercodi == periodo.Repercodi)
                    {
                        return orden;
                    }
                    else
                    {
                        orden++;
                    }
                }
            }
            else
            {
                return -1;
            }

            return salida;
        }

        /// <summary>
        /// Central una celda horizontal y verticalmente
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="fila"></param>
        /// <param name="col"></param>
        public void CentralCelda(Table tabla, int fila, int col)
        {
            tabla.Rows[fila].Cells[col].VerticalAlignment = VerticalAlignment.Center;
            tabla.Rows[fila].Cells[col].Paragraphs[0].Alignment = Alignment.center;
        }

        /// <summary>
        /// Rellena una celda con un texto
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="fila"></param>
        /// <param name="col"></param>
        /// <param name="tamLetra"></param>
        /// <param name="tipoTexto"></param>
        /// <param name="enNegrita"></param>
        /// <param name="texto"></param>
        public void LlenarCeldaTabla(Table tabla, int fila, int col, int tamLetra, string tipoTexto, bool enNegrita, string texto)
        {
            if (enNegrita)
                tabla.Rows[fila].Cells[col].Paragraphs[0].Append(texto).FontSize(tamLetra).Font(new FontFamily(tipoTexto)).Bold();
            else
                tabla.Rows[fila].Cells[col].Paragraphs[0].Append(texto).FontSize(tamLetra).Font(new FontFamily(tipoTexto));
        }


        /// <summary>
        /// Genera el archivo word de Informe de resarcimiento Revision
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="rutaArchivoPlantilla"></param>
        /// <param name="rutaFileOutput"></param>
        /// <exception cref="Exception"></exception>
        public void GenerarArchivoWordInformeResarcimientoRevisionDevExp(RePeriodoDTO periodo, string rutaArchivoPlantilla, string rutaFileOutput)
        {
            // Ruta de la plantilla a usar
            string templatePath = rutaArchivoPlantilla;

            // Ruta para guardar el nuevo documento
            string outputPath = rutaFileOutput;

            #region Datos generales
            string numPeriodo = ObtenerNumeroPeriodo(periodo);
            string tipoPeriodo = periodo.Repertipo == "T" ? "Trimestre" : (periodo.Repertipo == "S" ? "Semestre" : "");
            int anioPeriodo = periodo.Reperanio.Value;
            int numRevision = ObtenerNumeroRevision(periodo);
            string nLink = periodo.Repertipo == "T" ? "03" : (numPeriodo == "Primer" ? "01" : (numPeriodo == "Segundo" ? "02" : ""));

            if (numRevision == -1)
                throw new Exception("El periodo seleccionado no es un periodo con revisión dado que no cuenta con un periodo padre.");


            List<DatoPorSuministrador> data = ObtenerListadoInformeFinal(periodo);
            #endregion

            // Cargar la plantilla
            // Crear un servidor de documentos
            using (RichEditDocumentServer wordProcessor = new RichEditDocumentServer())
            {
                // Cargar la plantilla
                wordProcessor.LoadDocument(templatePath);

                // Acceder al documento
                Document document = wordProcessor.Document;

                string fontArial = "Arial";

                //Reemplazamos texto en contenido principal
                ReemplazarVariablesEnWord(document, numPeriodo, tipoPeriodo, anioPeriodo, nLink, numRevision, null);

                // Insertar una nueva sección después del contenido actual
                document.InsertSection(document.Range.End);
                DevExpress.XtraRichEdit.API.Native.Section newSection = document.Sections[document.Sections.Count - 1];  // Última sección
                newSection.Page.Landscape = true;


                //Muestro tablas por cada suministrador
                int numS = 1;
                foreach (var regPorSuministrador in data)
                {

                    // Insertar un subtítulo (Heading 2)
                    string subItemSuministrador = string.Format("5.{0}.   {1}", numS, regPorSuministrador.SuministradorNombre.ToUpper());

                    // Insertar el texto en la posición final del documento
                    DocumentPosition subtitlePosition = document.Range.End;
                    DocumentRange subItemRange = document.InsertText(subtitlePosition, subItemSuministrador + "\n");

                    // Actualizar las propiedades de los caracteres (fuente, tamaño, negrita)
                    CharacterProperties subItemProperties = document.BeginUpdateCharacters(subItemRange);
                    subItemProperties.FontSize = 14;
                    subItemProperties.Bold = true;
                    document.EndUpdateCharacters(subItemProperties);

                    // Actualizar las propiedades del párrafo
                    ParagraphProperties subtitleParagraph = document.BeginUpdateParagraphs(subItemRange);
                    subtitleParagraph.Style = document.ParagraphStyles["Heading 2"]; // Estilo de nivel 2 (asegúrate de que exista en tu documento)
                    //subtitleParagraph.Style = document.ParagraphStyles["Estilo2"]; //NO Quitar comentario: En algunos archivos funciona con esto
                    subtitleParagraph.Alignment = ParagraphAlignment.Left; // Puedes especificar la alineación si es necesario
                    document.EndUpdateParagraphs(subtitleParagraph);

                    #region Tablas por Punto de Entrega

                    CrearTituloConVinieta("Resarcimientos puntos de entrega", document);

                    if (regPorSuministrador.ListaPEHorizontal.Any() || regPorSuministrador.ListaPEVertical.Any())
                    {

                    }
                    else
                    {
                        // Insertar un subtítulo (Heading 2)
                        string noHayPE = "No se presentaron interrupciones por punto de entrega.";

                        // Insertar el texto en la posición final del documento
                        DocumentPosition subtitlePosition4 = document.Range.End;
                        DocumentRange subItemRange4 = document.InsertText(subtitlePosition4, noHayPE + "\n");

                        // Actualizar las propiedades de los caracteres (fuente, tamaño, negrita)
                        CharacterProperties subItemProperties4 = document.BeginUpdateCharacters(subItemRange4);
                        subItemProperties4.FontSize = 11;
                        //subItemProperties.Bold = true;
                        document.EndUpdateCharacters(subItemProperties4);
                    }

                    //Si tiene hasta 3 suministradores, se muestra tabla horizontal, la hoja despues del punto 5 es horizontal
                    if (regPorSuministrador.ListaPEHorizontal.Any())
                    {
                        CrearTablaHorizontalNuevo(document, fontArial, regPorSuministrador, ConstantesCalculoResarcimiento.TablasPE);
                    }

                    if (regPorSuministrador.ListaPEVertical.Any())
                    {
                        foreach (var regPEVertical in regPorSuministrador.ListaPEVertical)
                        {
                            CrearTablaVerticalNuevo(document, fontArial, regPEVertical);
                        }
                    }


                    #endregion


                    #region Tablas por Rechazo de carga

                    CrearTituloConVinieta("Resarcimientos rechazo de carga", document);

                    if (regPorSuministrador.ListaRCHorizontal.Any() || regPorSuministrador.ListaRCVertical.Any())
                    {

                    }
                    else
                    {
                        // Insertar un subtítulo (Heading 2)
                        string noHayRC = "No se presentaron interrupciones por rechazo de carga.";

                        // Insertar el texto en la posición final del documento
                        DocumentPosition subtitlePosition3 = document.Range.End;
                        DocumentRange subItemRange3 = document.InsertText(subtitlePosition3, noHayRC + "\n");

                        // Actualizar las propiedades de los caracteres (fuente, tamaño, negrita)
                        CharacterProperties subItemProperties3 = document.BeginUpdateCharacters(subItemRange3);
                        subItemProperties3.FontSize = 11;
                        //subItemProperties.Bold = true;
                        document.EndUpdateCharacters(subItemProperties3);
                    }

                    //Si muestra tabla horizontal cambio de modo de hoja a Horizontal
                    if (regPorSuministrador.ListaRCHorizontal.Any())
                    {
                        #region Tabla Horizontal

                        CrearTablaHorizontalNuevo(document, fontArial, regPorSuministrador, ConstantesCalculoResarcimiento.TablasRC);

                        #endregion

                    }

                    if (regPorSuministrador.ListaRCVertical.Any())
                    {
                        foreach (var regRCVertical in regPorSuministrador.ListaRCVertical)
                        {
                            #region Tabla Vertical

                            CrearTablaVerticalNuevo(document, fontArial, regRCVertical);

                            #endregion
                        }
                    }


                    #endregion

                    numS++;

                }

                /******************************************************************************************************************************************/




                // Actualizar todos los campos del documento (esto incluye las tablas de contenido)
                wordProcessor.Document.Fields.Update();

                // Guardar el documento
                wordProcessor.SaveDocument(outputPath, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);

                //Console.WriteLine($"Documento generado exitosamente en: {outputPath}");
            }
        }


        /// <summary>
        /// Devuelve el listado de emrpesas de ventas del reporte Informe resarcimiento
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public string ObtenerEmpresasInformaronVentas(RePeriodoDTO periodo)
        {
            List<ReIngresoTransmisionDTO> lstIngresos = ObtenerIngresosTx(periodo);
            List<string> lstEmpresas = lstIngresos.Select(x => x.Emprnomb).Distinct().ToList();
            List<string> lstEmpresasClean = new List<string>();

            foreach (var item in lstEmpresas)
            {
                lstEmpresasClean.Add(item.Trim());
            }

            string strEmpresas = "";

            if (lstEmpresasClean.Any())
                strEmpresas = String.Join(", ", lstEmpresasClean);

            return strEmpresas;
        }


        public void GenerarArchivoWordInformeResarcimientoDevExp(RePeriodoDTO periodo, string rutaArchivoPlantilla, string rutaFileOutput)
        {
            // Ruta de la plantilla existente
            string templatePath = rutaArchivoPlantilla;

            // Ruta para guardar el nuevo documento
            string outputPath = rutaFileOutput;

            #region Datos generales
            string numPeriodo = ObtenerNumeroPeriodo(periodo);
            string tipoPeriodo = periodo.Repertipo == "T" ? "Trimestre" : (periodo.Repertipo == "S" ? "Semestre" : "");
            int anioPeriodo = periodo.Reperanio.Value;
            string empresasVentas = ObtenerEmpresasInformaronVentas(periodo);

            string nLink = periodo.Repertipo == "T" ? "03" : (numPeriodo == "Primer" ? "01" : (numPeriodo == "Segundo" ? "02" : ""));

            List<ReIngresoTransmisionDTO> listaIngresos = ListIngresosPorTransmision(periodo.Repercodi);
            List<DatoPorSuministrador> data = ObtenerListadoInformeFinal(periodo);
            #endregion

            // Cargar la plantilla
            // Crear un servidor de documentos
            using (RichEditDocumentServer wordProcessor = new RichEditDocumentServer())
            {
                // Cargar la plantilla
                wordProcessor.LoadDocument(templatePath);

                // Acceder al documento
                Document document = wordProcessor.Document;

                string fontArial = "Arial";

                //Reemplazamos texto en contenido principal
                ReemplazarVariablesEnWord(document, numPeriodo, tipoPeriodo, anioPeriodo, nLink, -1, listaIngresos);

                // Insertar una nueva sección después del contenido actual
                document.InsertSection(document.Range.End);
                DevExpress.XtraRichEdit.API.Native.Section newSection = document.Sections[document.Sections.Count - 1];  // Última sección
                newSection.Page.Landscape = true;


                //Muestro tablas por cada suministrador
                int numS = 1;
                foreach (var regPorSuministrador in data)
                {

                    // Insertar un subtítulo (Heading 2)
                    string subItemSuministrador = string.Format("5.{0}.   {1}", numS, regPorSuministrador.SuministradorNombre.ToUpper());

                    // Insertar el texto en la posición final del documento
                    DocumentPosition subtitlePosition = document.Range.End;
                    DocumentRange subItemRange = document.InsertText(subtitlePosition, subItemSuministrador + "\n");

                    // Actualizar las propiedades de los caracteres (fuente, tamaño, negrita)
                    CharacterProperties subItemProperties = document.BeginUpdateCharacters(subItemRange);
                    subItemProperties.FontSize = 14;
                    subItemProperties.Bold = true;
                    document.EndUpdateCharacters(subItemProperties);

                    // Actualizar las propiedades del párrafo
                    ParagraphProperties subtitleParagraph = document.BeginUpdateParagraphs(subItemRange);
                    subtitleParagraph.Style = document.ParagraphStyles["Heading 2"]; // Estilo de nivel 2 (asegúrate de que exista en tu documento)
                    //subtitleParagraph.Style = document.ParagraphStyles["Estilo2"]; //NO Quitar comentario: En algunos archivos funciona con esto
                    subtitleParagraph.Alignment = ParagraphAlignment.Left; // Puedes especificar la alineación si es necesario
                    document.EndUpdateParagraphs(subtitleParagraph);

                    // Crear el texto adicional sin el estilo "Estilo2"
                    string resarcimientosTexto = "Resarcimientos puntos de entrega:";


                    #region Tablas por Punto de Entrega

                    CrearTituloConVinieta("Resarcimientos puntos de entrega", document);

                    if (regPorSuministrador.ListaPEHorizontal.Any() || regPorSuministrador.ListaPEVertical.Any())
                    {

                    }
                    else
                    {
                        // Insertar un subtítulo (Heading 2)
                        string noHayPE = "No se presentaron interrupciones por punto de entrega.";

                        // Insertar el texto en la posición final del documento
                        DocumentPosition subtitlePosition4 = document.Range.End;
                        DocumentRange subItemRange4 = document.InsertText(subtitlePosition4, noHayPE + "\n");

                        // Actualizar las propiedades de los caracteres (fuente, tamaño, negrita)
                        CharacterProperties subItemProperties4 = document.BeginUpdateCharacters(subItemRange4);
                        subItemProperties4.FontSize = 11;
                        //subItemProperties.Bold = true;
                        document.EndUpdateCharacters(subItemProperties4);
                    }

                    //Si tiene hasta 3 suministradores, se muestra tabla horizontal, la hoja despues del punto 5 es horizontal
                    if (regPorSuministrador.ListaPEHorizontal.Any())
                    {
                        CrearTablaHorizontalNuevo(document, fontArial, regPorSuministrador, ConstantesCalculoResarcimiento.TablasPE);
                    }

                    if (regPorSuministrador.ListaPEVertical.Any())
                    {
                        foreach (var regPEVertical in regPorSuministrador.ListaPEVertical)
                        {
                            CrearTablaVerticalNuevo(document, fontArial, regPEVertical);
                        }
                    }


                    #endregion

                    #region Tablas por Rechazo de carga

                    CrearTituloConVinieta("Resarcimientos rechazo de carga", document);

                    if (regPorSuministrador.ListaRCHorizontal.Any() || regPorSuministrador.ListaRCVertical.Any())
                    {

                    }
                    else
                    {
                        // Insertar un subtítulo (Heading 2)
                        string noHayRC = "No se presentaron interrupciones por rechazo de carga.";

                        // Insertar el texto en la posición final del documento
                        DocumentPosition subtitlePosition3 = document.Range.End;
                        DocumentRange subItemRange3 = document.InsertText(subtitlePosition3, noHayRC + "\n");

                        // Actualizar las propiedades de los caracteres (fuente, tamaño, negrita)
                        CharacterProperties subItemProperties3 = document.BeginUpdateCharacters(subItemRange3);
                        subItemProperties3.FontSize = 11;
                        //subItemProperties.Bold = true;
                        document.EndUpdateCharacters(subItemProperties3);
                    }

                    //Si muestra tabla horizontal cambio de modo de hoja a Horizontal
                    if (regPorSuministrador.ListaRCHorizontal.Any())
                    {
                        #region Tabla Horizontal

                        CrearTablaHorizontalNuevo(document, fontArial, regPorSuministrador, ConstantesCalculoResarcimiento.TablasRC);

                        #endregion

                    }

                    if (regPorSuministrador.ListaRCVertical.Any())
                    {
                        foreach (var regRCVertical in regPorSuministrador.ListaRCVertical)
                        {
                            #region Tabla Vertical

                            //CrearTablaVertical(document, fontArial, regRCVertical);
                            CrearTablaVerticalNuevo(document, fontArial, regRCVertical);

                            #endregion
                        }
                    }


                    #endregion

                    numS++;

                }

                /******************************************************************************************************************************************/




                // Actualizar todos los campos del documento (esto incluye las tablas de contenido)
                wordProcessor.Document.Fields.Update();

                // Guardar el documento
                wordProcessor.SaveDocument(outputPath, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);

                //Console.WriteLine($"Documento generado exitosamente en: {outputPath}");
            }
        }

        /// <summary>
        ///  Arma la tabla horizontal
        /// </summary>
        /// <param name="document"></param>
        /// <param name="fontArial"></param>
        /// <param name="regPorSuministrador"></param>
        /// <param name="tipoTabla"></param>
        private void CrearTablaHorizontalNuevo(Document document, string fontArial, DatoPorSuministrador regPorSuministrador, int tipoTabla)
        {
            int nResp = 3;
            int numColumnasH = 2 + 4 * nResp;
            int numFilasH = tipoTabla == ConstantesCalculoResarcimiento.TablasPE ? (regPorSuministrador.NumFilasTablaHorizontalPE.Value + 2) : (regPorSuministrador.NumFilasTablaHorizontalRC.Value + 2);

            DevExpress.XtraRichEdit.API.Native.Table table = document.Tables.Create(document.Range.End, numFilasH, numColumnasH); // 30 filas, 5 columnas 

            // Formatear la tabla
            table.TableLayout = TableLayoutType.Autofit; // Ancho fijo

            #region Cabecera

            table.FirstRow.Height = 100;
            table.BeginUpdate();

            // Configurar la primera fila como encabezado
            TableRow headerRow = table.Rows[0];
            headerRow.RepeatAsHeaderRow = true;

            //Central cabecera
            headerRow.Cells[0].VerticalAlignment = TableCellVerticalAlignment.Center;
            headerRow.Cells[1].VerticalAlignment = TableCellVerticalAlignment.Center;

            for (int i = 0; i < nResp; i++)
            {
                headerRow.Cells[2 + 4 * i].VerticalAlignment = TableCellVerticalAlignment.Center;
                headerRow.Cells[3 + 4 * i].VerticalAlignment = TableCellVerticalAlignment.Center;
                headerRow.Cells[4 + 4 * i].VerticalAlignment = TableCellVerticalAlignment.Center;
                headerRow.Cells[5 + 4 * i].VerticalAlignment = TableCellVerticalAlignment.Center;
            }

            //colores cabecera
            headerRow.Cells[0].BackgroundColor = ColorTranslator.FromHtml("#8DB4E2");
            headerRow.Cells[1].BackgroundColor = ColorTranslator.FromHtml("#8DB4E2");
            for (int i = 0; i < nResp; i++)
            {
                headerRow.Cells[2 + 4 * i].BackgroundColor = ColorTranslator.FromHtml("#8DB4E2");
                headerRow.Cells[3 + 4 * i].BackgroundColor = ColorTranslator.FromHtml("#8DB4E2");
                headerRow.Cells[4 + 4 * i].BackgroundColor = ColorTranslator.FromHtml("#1F497D");
                headerRow.Cells[5 + 4 * i].BackgroundColor = ColorTranslator.FromHtml("#1F497D");

            }

            //Textos
            IngresarTextoCeldaCabecera(document, headerRow.Cells[0], "Cliente");
            IngresarTextoCeldaCabecera(document, headerRow.Cells[1], "Barra");

            for (int i = 0; i < nResp; i++)
            {
                IngresarTextoCeldaCabecera(document, headerRow.Cells[2 + 4 * i], string.Format("Agente Resp. {0}", (1 + i)));
                IngresarTextoCeldaCabecera(document, headerRow.Cells[3 + 4 * i], string.Format("Resarcimiento Agente Resp. {0} (USD)", (1 + i)));
                IngresarTextoCeldaGeneral(document, headerRow.Cells[4 + 4 * i], null, string.Format("(*) Resarcimiento Final Agente Resp. {0} (USD)", (1 + i)), ParagraphAlignment.Center, TableCellVerticalAlignment.Center, false, "#FFFFFF", "#1F497D");
                IngresarTextoCeldaGeneral(document, headerRow.Cells[5 + 4 * i], null, string.Format("Factor de Aplicación de la Primera Disposición Final", (1 + i)), ParagraphAlignment.Center, TableCellVerticalAlignment.Center, false, "#FFFFFF", "#1F497D");

            }

            #endregion

            #region Cuerpo

            //Ingresar data en el cuerpo
            int tamCampoTexto = 7;
            int fila = 1;
            List<RegistroWord> listaUsar = tipoTabla == ConstantesCalculoResarcimiento.TablasPE ? regPorSuministrador.ListaPEHorizontal : regPorSuministrador.ListaRCHorizontal;
            foreach (var reg in listaUsar)
            {
                TableRow filaActual = table.Rows[fila];

                IngresarTextoCeldaCuerpo(document, filaActual.Cells[0], reg.NombreCliente.ToUpper(), "", false);
                IngresarTextoCeldaCuerpo(document, filaActual.Cells[1], reg.BarraNombre.ToUpper(), "", false);

                for (int i = 0; i < nResp; i++)
                {
                    string nombRX = "";
                    string resAgRespX = "";
                    string aplicAgRespX = "";
                    string fapdf = "";

                    if (i == 0)
                    {
                        nombRX = reg.Responsable1Nombre.Trim();
                        resAgRespX = reg.SumAgenteResp1 != null ? (reg.SumAgenteResp1.Value != 0 ? reg.SumAgenteResp1.Value.ToString("#,##0.00") : "-") : "-";
                        aplicAgRespX = reg.SumAplicacionAResp1 != null ? (reg.SumAplicacionAResp1.Value != 0 ? reg.SumAplicacionAResp1.Value.ToString("#,##0.00") : "-") : "-";
                        fapdf = reg.FactorR1 != null ? (reg.FactorR1.Value != 0 ? (reg.FactorR1 / 100).Value.ToString("#,##0.00%") : "-") : "-";
                    }
                    else
                    {
                        if (i == 1)
                        {
                            nombRX = reg.Responsable2Nombre.Trim();
                            resAgRespX = reg.SumAgenteResp2 != null ? (reg.SumAgenteResp2.Value != 0 ? reg.SumAgenteResp2.Value.ToString("#,##0.00") : "-") : "-";
                            aplicAgRespX = reg.SumAplicacionAResp2 != null ? (reg.SumAplicacionAResp2.Value != 0 ? reg.SumAplicacionAResp2.Value.ToString("#,##0.00") : "-") : "-";
                            fapdf = reg.FactorR2 != null ? (reg.FactorR2.Value != 0 ? (reg.FactorR2 / 100).Value.ToString("#,##0.00%") : "-") : "-";
                        }
                        else
                        {
                            if (i == 2)
                            {
                                nombRX = reg.Responsable3Nombre.Trim();
                                resAgRespX = reg.SumAgenteResp3 != null ? (reg.SumAgenteResp3.Value != 0 ? reg.SumAgenteResp3.Value.ToString("#,##0.00") : "-") : "-";
                                aplicAgRespX = reg.SumAplicacionAResp3 != null ? (reg.SumAplicacionAResp3.Value != 0 ? reg.SumAplicacionAResp3.Value.ToString("#,##0.00") : "-") : "-";
                                fapdf = reg.FactorR3 != null ? (reg.FactorR3.Value != 0 ? (reg.FactorR3 / 100).Value.ToString("#,##0.00%") : "-") : "-";
                            }

                        }
                    }

                    IngresarTextoCeldaCuerpo(document, filaActual.Cells[2 + 4 * i], nombRX, "", false);
                    IngresarTextoCeldaCuerpo(document, filaActual.Cells[3 + 4 * i], resAgRespX, "", false);
                    IngresarTextoCeldaCuerpo(document, filaActual.Cells[4 + 4 * i], aplicAgRespX, "", true);

                    string colorCellfapdf = fapdf == "-" ? "#D9D9D9" : "";
                    IngresarTextoCeldaCuerpo(document, filaActual.Cells[5 + 4 * i], fapdf, colorCellfapdf, true);

                    //Si son guion los centro
                    if (resAgRespX == "-")
                        document.BeginUpdateParagraphs(filaActual.Cells[3 + 4 * i].Range).Alignment = ParagraphAlignment.Center;
                    else
                        document.BeginUpdateParagraphs(filaActual.Cells[3 + 4 * i].Range).Alignment = ParagraphAlignment.Right;

                    if (aplicAgRespX == "-")
                        document.BeginUpdateParagraphs(filaActual.Cells[4 + 4 * i].Range).Alignment = ParagraphAlignment.Center;
                    else
                        document.BeginUpdateParagraphs(filaActual.Cells[4 + 4 * i].Range).Alignment = ParagraphAlignment.Right;

                    if (fapdf == "-")
                        document.BeginUpdateParagraphs(filaActual.Cells[5 + 4 * i].Range).Alignment = ParagraphAlignment.Center;
                    else
                        document.BeginUpdateParagraphs(filaActual.Cells[5 + 4 * i].Range).Alignment = ParagraphAlignment.Right;
                }

                fila++;
            }

            #endregion

            #region Pie de Pagina

            TableRow filaFinal = table.Rows[fila];

            IngresarTextoCeldaUltimaFila(document, filaFinal.Cells[0], "TOTAL GENERAL", "#8DB4E2");
            IngresarTextoCeldaUltimaFila(document, filaFinal.Cells[1], "", "");

            Totales objTotal3Menos = tipoTabla == ConstantesCalculoResarcimiento.TablasPE ? regPorSuministrador.TotalPE3Menos : regPorSuministrador.TotalRC3Menos;

            for (int dx = 0; dx < nResp; dx++)
            {
                string sumResAgRespX = "";
                string sumAplicAgRespX = "";
                string sumFapdf = "";

                if (dx == 0)
                {
                    sumResAgRespX = objTotal3Menos.SumTotalAgenteResp1 != null ? (objTotal3Menos.SumTotalAgenteResp1.Value != 0 ? objTotal3Menos.SumTotalAgenteResp1.Value.ToString("#,##0.00") : "-") : "-";
                    sumAplicAgRespX = objTotal3Menos.SumTotalAplicacionAResp1 != null ? (objTotal3Menos.SumTotalAplicacionAResp1.Value != 0 ? objTotal3Menos.SumTotalAplicacionAResp1.Value.ToString("#,##0.00") : "-") : "-";
                    sumFapdf = "-";
                }
                else
                {
                    if (dx == 1)
                    {
                        sumResAgRespX = objTotal3Menos.SumTotalAgenteResp2 != null ? (objTotal3Menos.SumTotalAgenteResp2.Value != 0 ? objTotal3Menos.SumTotalAgenteResp2.Value.ToString("#,##0.00") : "-") : "-";
                        sumAplicAgRespX = objTotal3Menos.SumTotalAplicacionAResp2 != null ? (objTotal3Menos.SumTotalAplicacionAResp2.Value != 0 ? objTotal3Menos.SumTotalAplicacionAResp2.Value.ToString("#,##0.00") : "-") : "-";
                        sumFapdf = "-";
                    }
                    else
                    {
                        if (dx == 2)
                        {
                            sumResAgRespX = objTotal3Menos.SumTotalAgenteResp3 != null ? (objTotal3Menos.SumTotalAgenteResp3.Value != 0 ? objTotal3Menos.SumTotalAgenteResp3.Value.ToString("#,##0.00") : "-") : "-";
                            sumAplicAgRespX = objTotal3Menos.SumTotalAplicacionAResp3 != null ? (objTotal3Menos.SumTotalAplicacionAResp3.Value != 0 ? objTotal3Menos.SumTotalAplicacionAResp3.Value.ToString("#,##0.00") : "-") : "-";
                            sumFapdf = "-";
                        }
                    }
                }

                IngresarTextoCeldaUltimaFila(document, filaFinal.Cells[2 + 4 * dx], "", "");
                IngresarTextoCeldaUltimaFila(document, filaFinal.Cells[3 + 4 * dx], sumResAgRespX, "");
                IngresarTextoCeldaUltimaFila(document, filaFinal.Cells[4 + 4 * dx], sumAplicAgRespX, "");
                IngresarTextoCeldaUltimaFila(document, filaFinal.Cells[5 + 4 * dx], sumFapdf, "#D9D9D9");

                //Si son guion los centro
                if (sumResAgRespX == "-")
                    document.BeginUpdateParagraphs(filaFinal.Cells[3 + 4 * dx].Range).Alignment = ParagraphAlignment.Center;
                else
                    document.BeginUpdateParagraphs(filaFinal.Cells[3 + 4 * dx].Range).Alignment = ParagraphAlignment.Right;


                if (sumAplicAgRespX == "-")
                    document.BeginUpdateParagraphs(filaFinal.Cells[4 + 4 * dx].Range).Alignment = ParagraphAlignment.Center;
                else
                    document.BeginUpdateParagraphs(filaFinal.Cells[4 + 4 * dx].Range).Alignment = ParagraphAlignment.Right;

                if (sumFapdf == "-")
                    document.BeginUpdateParagraphs(filaFinal.Cells[5 + 4 * dx].Range).Alignment = ParagraphAlignment.Center;
                else
                    document.BeginUpdateParagraphs(filaFinal.Cells[5 + 4 * dx].Range).Alignment = ParagraphAlignment.Right;
            }

            #endregion

            //Al final hago merge
            table.MergeCells(table[fila, 0], table[fila, 1]);

            table.EndUpdate();

            document.AppendText("\n"); // Esto agrega un salto de línea después de la tabla. Sirve para que no se mezclen las tablas vertical y horizontal
        }

        /// <summary>
        /// Ingresa valor a una celda final, usando devexpress
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cell"></param>
        /// <param name="texto"></param>
        /// <param name="colorFondo"></param>
        public void IngresarTextoCeldaUltimaFila(Document document, TableCell cell, string texto, string colorFondo)
        {
            cell.Height = 110;

            // Agregar texto al encabezado
            DocumentRange cellRange = cell.Range;
            document.InsertText(cellRange.Start, texto);

            // Configurar alineación y estilo
            ParagraphProperties paragraphProperties = document.BeginUpdateParagraphs(cellRange);
            paragraphProperties.Alignment = ParagraphAlignment.Center;
            paragraphProperties.Style.FontSize = 7;
            paragraphProperties.Style.FontName = "Arial";
            paragraphProperties.Style.Bold = true;
            document.EndUpdateParagraphs(paragraphProperties);

            // Alinear verticalmente
            cell.VerticalAlignment = TableCellVerticalAlignment.Center;

            //color fondo
            if (colorFondo != "")
                cell.BackgroundColor = ColorTranslator.FromHtml(colorFondo);

        }

        /// <summary>
        /// metodo general para ingresar valor a una celda de la tabla usando devexpress
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cell"></param>
        /// <param name="altura"></param>
        /// <param name="texto"></param>
        /// <param name="alineacionHor"></param>
        /// <param name="alineacionVert"></param>
        /// <param name="esNegrita"></param>
        /// <param name="colorTexto"></param>
        /// <param name="colorFondo"></param>
        public void IngresarTextoCeldaGeneral(Document document, TableCell cell, float? altura, string texto, ParagraphAlignment alineacionHor, TableCellVerticalAlignment alineacionVert, bool esNegrita, string colorTexto, string colorFondo)
        {
            //cell.Height = 110;
            if (altura != null)
                cell.Height = altura.Value;

            // Alinear verticalmente
            //cell.VerticalAlignment = TableCellVerticalAlignment.Center;
            cell.VerticalAlignment = alineacionVert;

            // Agregar texto al encabezado
            DocumentRange cellRange = cell.Range;
            DocumentRange subItemRange = document.InsertText(cellRange.Start, texto);
            CharacterProperties characterProperties = document.BeginUpdateCharacters(subItemRange);
            characterProperties.ForeColor = ColorTranslator.FromHtml(colorTexto); // Aplicar el color de texto
            characterProperties.Bold = esNegrita;

            document.EndUpdateCharacters(characterProperties);

            // Configurar alineación y estilo
            ParagraphProperties paragraphProperties = document.BeginUpdateParagraphs(cellRange);
            //paragraphProperties.Alignment = ParagraphAlignment.Center;
            paragraphProperties.Alignment = alineacionHor;
            paragraphProperties.Style.FontSize = 7;
            paragraphProperties.Style.FontName = "Arial";
            //paragraphProperties.Style.Bold = true;
            //paragraphProperties.Style.Bold = esNegrita;
            document.EndUpdateParagraphs(paragraphProperties);

            //color fondo
            if (colorFondo != "")
                cell.BackgroundColor = ColorTranslator.FromHtml(colorFondo);

        }

        /// <summary>
        /// Ingresa valores a las celdas cabeceras del reporte word
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cell"></param>
        /// <param name="texto"></param>
        public void IngresarTextoCeldaCabecera(Document document, TableCell cell, string texto)
        {
            // Agregar texto al encabezado
            DocumentRange cellRange = cell.Range;
            document.InsertText(cellRange.Start, texto);

            // Configurar alineación y estilo
            ParagraphProperties paragraphProperties = document.BeginUpdateParagraphs(cellRange);
            paragraphProperties.Alignment = ParagraphAlignment.Center;
            paragraphProperties.Style.FontSize = 7;
            paragraphProperties.Style.FontName = "Arial";
            paragraphProperties.Style.Bold = true;
            document.EndUpdateParagraphs(paragraphProperties);

        }

        /// <summary>
        /// Da formato al texto dentro de las celdas de la tabla, especificamente para el cuerpo
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cell"></param>
        /// <param name="texto"></param>
        /// <param name="colorFondo"></param>
        public void IngresarTextoCeldaCuerpo(Document document, TableCell cell, string texto, string colorFondo, bool enNegrita)
        {
            // Agregar texto al encabezado
            //DocumentRange cellRange = cell.Range;
            //document.InsertText(cellRange.Start, texto);

            DocumentRange cellRange = cell.Range;
            DocumentRange subItemRange = document.InsertText(cellRange.Start, texto);
            CharacterProperties characterProperties = document.BeginUpdateCharacters(subItemRange);
            //characterProperties.ForeColor = ColorTranslator.FromHtml(colorTexto); // Aplicar el color de texto
            characterProperties.Bold = enNegrita;
            document.EndUpdateCharacters(characterProperties);

            // Configurar alineación y estilo
            ParagraphProperties paragraphProperties = document.BeginUpdateParagraphs(cellRange);
            paragraphProperties.Alignment = ParagraphAlignment.Center;
            paragraphProperties.Style.FontSize = 7;
            paragraphProperties.Style.FontName = "Arial";
            //paragraphProperties.Style.Bold = enNegrita;
            document.EndUpdateParagraphs(paragraphProperties);

            // Alinear verticalmente
            cell.VerticalAlignment = TableCellVerticalAlignment.Center;

            //color fondo
            if (colorFondo != "")
                cell.BackgroundColor = ColorTranslator.FromHtml(colorFondo);
        }

        /// <summary>
        /// Genera un parrafo con el subitem
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="document"></param>
        public void CrearTituloConVinieta(string texto, Document document)
        {
            // Insertar el texto en la posición final del documento
            DocumentPosition subtitlePosition = document.Range.End;
            //DocumentRange subItemRange = document.InsertText(subtitlePosition, "•\tResarcimientos puntos de entrega" + "\n");
            DocumentRange subItemRange = document.AppendText("\n" + "•\t" + texto + "\n");

            // Actualizar las propiedades de los caracteres (fuente, tamaño, negrita)
            CharacterProperties subItemProperties = document.BeginUpdateCharacters(subItemRange);
            subItemProperties.FontSize = 14;
            subItemProperties.Bold = true;
            document.EndUpdateCharacters(subItemProperties);

            string msg = "(*) Incluye aplicación de la Primera Disposición Final de la NTCSE";
            // Insertar el texto en la posición final del documento
            DocumentPosition subtitlePosition2 = document.Range.End;
            DocumentRange subItemRange2 = document.AppendText("\t" + msg + "\n");

            // Actualizar las propiedades de los caracteres (fuente, tamaño, negrita)
            CharacterProperties subItemProperties2 = document.BeginUpdateCharacters(subItemRange2);
            subItemProperties2.FontSize = 8;
            subItemProperties2.ForeColor = ColorTranslator.FromHtml("#0209f9");
            subItemProperties2.Italic = true;
            document.EndUpdateCharacters(subItemProperties2);
        }

        /// <summary>
        /// Rellena las variables de la plantilla para ambos tipos de reportes word
        /// </summary>
        /// <param name="document"></param>
        /// <param name="numeroPeriodo"></param>
        /// <param name="tipoDelPeriodo"></param>
        /// <param name="anioPeriodo"></param>
        /// <param name="nLink"></param>
        /// <param name="numRevision"></param>
        /// <param name="listaIngresos"></param>
        public void ReemplazarVariablesEnWord(Document document, string numeroPeriodo, string tipoDelPeriodo, int anioPeriodo, string nLink, int numRevision, List<ReIngresoTransmisionDTO> listaIngresos)
        {
            string fechaActual = string.Format("{0} de {1} de {2}", string.Format("{0:D2}", DateTime.Now.Day), EPDate.f_NombreMes(DateTime.Now.Month), anioPeriodo);

            string numPeriodo = numeroPeriodo.ToUpper();
            string NumPeriodoM = numeroPeriodo.ToUpper();
            string tipoPeriodo = tipoDelPeriodo.ToUpper();
            string TipoPeriodoM = tipoDelPeriodo.ToUpper();

            string strListaEmpresas = "";
            if (listaIngresos != null)
            {
                List<string> strEmpresas = listaIngresos.Where(x => x.Emprnomb != null).Select(x => x.Emprnomb.Trim().ToUpper()).OrderBy(x => x).Distinct().ToList();
                strListaEmpresas = listaIngresos != null ? (listaIngresos.Any() ? string.Join(", ", strEmpresas) : "") : "";
            }
            string nRev = "";
            string nRevM = "";
            if (numRevision != -1)
            {
                nRev = "Revisión " + numRevision;
                nRevM = "REVISIÓN " + numRevision;
            }


            document.ReplaceAll("{anioPeriodo}", anioPeriodo.ToString(), SearchOptions.None);
            document.ReplaceAll("{numPeriodo}", numPeriodo, SearchOptions.None);
            document.ReplaceAll("{NUMPERIODO}", NumPeriodoM, SearchOptions.None);
            document.ReplaceAll("{tipoPeriodo}", tipoPeriodo, SearchOptions.None);
            document.ReplaceAll("{TIPOPERIODO}", TipoPeriodoM, SearchOptions.None);
            document.ReplaceAll("{fechaActual}", fechaActual, SearchOptions.None);
            document.ReplaceAll("{numPerTipo}", nLink, SearchOptions.None);
            document.ReplaceAll("{listaEmpresasIngresoTx}", strListaEmpresas, SearchOptions.None);
            document.ReplaceAll("{NumRevision}", nRev, SearchOptions.None);
            document.ReplaceAll("{NUMREVISION}", nRevM, SearchOptions.None);


            // Buscar y reemplazar texto dentro de las cajas de texto
            foreach (Shape shape in document.Shapes)
            {
                if (shape.ShapeFormat.TextBox != null) // Verifica que sea una caja de texto
                {
                    SubDocument textoCaja = shape.TextBox.Document; // Obtén el subdocumento de la caja de texto

                    textoCaja.ReplaceAll("{anioPeriodo}", anioPeriodo.ToString(), SearchOptions.None);
                    textoCaja.ReplaceAll("{numPeriodo}", numPeriodo, SearchOptions.None);
                    textoCaja.ReplaceAll("{NUMPERIODO}", NumPeriodoM, SearchOptions.None);
                    textoCaja.ReplaceAll("{tipoPeriodo}", tipoPeriodo, SearchOptions.None);
                    textoCaja.ReplaceAll("{TIPOPERIODO}", TipoPeriodoM, SearchOptions.None);
                    textoCaja.ReplaceAll("{fechaActual}", fechaActual, SearchOptions.None);
                    textoCaja.ReplaceAll("{numPerTipo}", nLink, SearchOptions.None);
                    textoCaja.ReplaceAll("{listaEmpresasIngresoTx}", strListaEmpresas, SearchOptions.None);
                    textoCaja.ReplaceAll("{NumRevision}", nRev, SearchOptions.None);
                    textoCaja.ReplaceAll("{NUMREVISION}", nRevM, SearchOptions.None);
                }
            }


            foreach (DevExpress.XtraRichEdit.API.Native.Section seccion in document.Sections)
            {
                // Acceder al encabezado (Header)
                SubDocument encabezado = seccion.BeginUpdateHeader();

                encabezado.ReplaceAll("{anioPeriodo}", anioPeriodo.ToString(), SearchOptions.None);
                encabezado.ReplaceAll("{numPeriodo}", numPeriodo, SearchOptions.None);
                encabezado.ReplaceAll("{NUMPERIODO}", NumPeriodoM, SearchOptions.None);
                encabezado.ReplaceAll("{tipoPeriodo}", tipoPeriodo, SearchOptions.None);
                encabezado.ReplaceAll("{TIPOPERIODO}", TipoPeriodoM, SearchOptions.None);
                encabezado.ReplaceAll("{fechaActual}", fechaActual, SearchOptions.None);
                encabezado.ReplaceAll("{numPerTipo}", nLink, SearchOptions.None);
                encabezado.ReplaceAll("{listaEmpresasIngresoTx}", strListaEmpresas, SearchOptions.None);
                encabezado.ReplaceAll("{NumRevision}", nRev, SearchOptions.None);
                encabezado.ReplaceAll("{NUMREVISION}", nRevM, SearchOptions.None);

                seccion.EndUpdateHeader(encabezado);

            }

        }


        /// <summary>
        /// Genera una tabla vertical usando devexcpress
        /// </summary>
        /// <param name="document"></param>
        /// <param name="fontArial"></param>
        /// <param name="regVertical"></param>
        private void CrearTablaVerticalNuevo(Document document, string fontArial, RegistroWord regVertical)
        {
            document.AppendText("\n"); // Salto de línea para separar las tablas y no se mezclen vertical con horizontal

            int nResp = regVertical.numResponsables;
            int numColumnasV = 5;
            int numFilasV = 3 + nResp;

            //Table tableV = document.InsertTable(numFilasV, numColumnasV);
            DevExpress.XtraRichEdit.API.Native.Table table = document.Tables.Create(document.Range.End, numFilasV, numColumnasV); // 30 filas, 5 columnas
            // Formatear la tabla
            table.TableLayout = TableLayoutType.Autofit; // Ancho fijo
            table.FirstRow.Height = 80;
            table.BeginUpdate();



            // Configurar la primera fila como encabezado
            TableRow primeraFila = table.Rows[0];


            //Textos
            IngresarTextoCeldaGeneral(document, table.Rows[0].Cells[0], null, "Agente Responsable", ParagraphAlignment.Center, TableCellVerticalAlignment.Center, true, "#000000", "#8DB4E2");
            IngresarTextoCeldaGeneral(document, table.Rows[0].Cells[2], null, "Resarcimiento Agente Resp. (USD)", ParagraphAlignment.Center, TableCellVerticalAlignment.Center, true, "#000000", "#8DB4E2");
            IngresarTextoCeldaGeneral(document, table.Rows[0].Cells[3], null, "(*) Resarcimiento Final Agente Resp. (USD)", ParagraphAlignment.Center, TableCellVerticalAlignment.Center, true, "#FFFFFF", "#1F497D");
            IngresarTextoCeldaGeneral(document, table.Rows[0].Cells[4], null, "Factor de Aplicación de la Primera Disposición Final", ParagraphAlignment.Center, TableCellVerticalAlignment.Center, true, "#FFFFFF", "#1F497D");

            IngresarTextoCeldaGeneral(document, table.Rows[1].Cells[0], null, "Cliente", ParagraphAlignment.Center, TableCellVerticalAlignment.Center, true, "#000000", "#8DB4E2");
            IngresarTextoCeldaGeneral(document, table.Rows[1].Cells[2], null, regVertical.NombreCliente, ParagraphAlignment.Center, TableCellVerticalAlignment.Center, false, "#000000", "#8DB4E2");

            IngresarTextoCeldaGeneral(document, table.Rows[2].Cells[0], null, "Barra", ParagraphAlignment.Center, TableCellVerticalAlignment.Center, true, "#000000", "#8DB4E2");
            IngresarTextoCeldaGeneral(document, table.Rows[2].Cells[2], null, regVertical.BarraNombre, ParagraphAlignment.Center, TableCellVerticalAlignment.Center, false, "#000000", "#8DB4E2");



            for (int dx = 0; dx < nResp; dx++)
            {
                string responsableX = "";
                string sumResAgRespX = "";
                string sumAplicAgRespX = "";
                string fapDf = "";

                if (dx == 0)
                {
                    responsableX = regVertical.Responsable1Nombre;
                    sumResAgRespX = regVertical.SumAgenteResp1 != null ? (regVertical.SumAgenteResp1.Value != 0 ? regVertical.SumAgenteResp1.Value.ToString("#,##0.00") : "-") : "-";
                    sumAplicAgRespX = regVertical.SumAplicacionAResp1 != null ? (regVertical.SumAplicacionAResp1.Value != 0 ? regVertical.SumAplicacionAResp1.Value.ToString("#,##0.00") : "-") : "-";
                    fapDf = regVertical.FactorR1 != null ? (regVertical.FactorR1.Value != 0 ? (regVertical.FactorR1 / 100).Value.ToString("#,##0.00%") : "-") : "-";
                }
                else
                {
                    if (dx == 1)
                    {
                        responsableX = regVertical.Responsable2Nombre;
                        sumResAgRespX = regVertical.SumAgenteResp2 != null ? (regVertical.SumAgenteResp2.Value != 0 ? regVertical.SumAgenteResp2.Value.ToString("#,##0.00") : "-") : "-";
                        sumAplicAgRespX = regVertical.SumAplicacionAResp2 != null ? (regVertical.SumAplicacionAResp2.Value != 0 ? regVertical.SumAplicacionAResp2.Value.ToString("#,##0.00") : "-") : "-";
                        fapDf = regVertical.FactorR2 != null ? (regVertical.FactorR2.Value != 0 ? (regVertical.FactorR2 / 100).Value.ToString("#,##0.00%") : "-") : "-";
                    }
                    else
                    {
                        if (dx == 2)
                        {
                            responsableX = regVertical.Responsable3Nombre;
                            sumResAgRespX = regVertical.SumAgenteResp3 != null ? (regVertical.SumAgenteResp3.Value != 0 ? regVertical.SumAgenteResp3.Value.ToString("#,##0.00") : "-") : "-";
                            sumAplicAgRespX = regVertical.SumAplicacionAResp3 != null ? (regVertical.SumAplicacionAResp3.Value != 0 ? regVertical.SumAplicacionAResp3.Value.ToString("#,##0.00") : "-") : "-";
                            fapDf = regVertical.FactorR3 != null ? (regVertical.FactorR3.Value != 0 ? (regVertical.FactorR3 / 100).Value.ToString("#,##0.00%") : "-") : "-";
                        }
                        else
                        {
                            if (dx == 3)
                            {
                                responsableX = regVertical.Responsable4Nombre;
                                sumResAgRespX = regVertical.SumAgenteResp4 != null ? (regVertical.SumAgenteResp4.Value != 0 ? regVertical.SumAgenteResp4.Value.ToString("#,##0.00") : "-") : "-";
                                sumAplicAgRespX = regVertical.SumAplicacionAResp4 != null ? (regVertical.SumAplicacionAResp4.Value != 0 ? regVertical.SumAplicacionAResp4.Value.ToString("#,##0.00") : "-") : "-";
                                fapDf = regVertical.FactorR4 != null ? (regVertical.FactorR4.Value != 0 ? (regVertical.FactorR4 / 100).Value.ToString("#,##0.00%") : "-") : "-";
                            }
                            else
                            {
                                if (dx == 4)
                                {
                                    responsableX = regVertical.Responsable5Nombre;
                                    sumResAgRespX = regVertical.SumAgenteResp5 != null ? (regVertical.SumAgenteResp5.Value != 0 ? regVertical.SumAgenteResp5.Value.ToString("#,##0.00") : "-") : "-";
                                    sumAplicAgRespX = regVertical.SumAplicacionAResp5 != null ? (regVertical.SumAplicacionAResp5.Value != 0 ? regVertical.SumAplicacionAResp5.Value.ToString("#,##0.00") : "-") : "-";
                                    fapDf = regVertical.FactorR5 != null ? (regVertical.FactorR5.Value != 0 ? (regVertical.FactorR5 / 100).Value.ToString("#,##0.00%") : "-") : "-";
                                }
                            }
                        }
                    }
                }

                IngresarTextoCeldaGeneral(document, table.Rows[3 + dx].Cells[0], null, string.Format("Agente Resp. {0}", (1 + dx)), ParagraphAlignment.Center, TableCellVerticalAlignment.Center, true, "#000000", "#8DB4E2");
                IngresarTextoCeldaGeneral(document, table.Rows[3 + dx].Cells[1], null, responsableX, ParagraphAlignment.Center, TableCellVerticalAlignment.Center, false, "#000000", "");
                IngresarTextoCeldaGeneral(document, table.Rows[3 + dx].Cells[2], null, sumResAgRespX, ParagraphAlignment.Center, TableCellVerticalAlignment.Center, false, "#000000", "");
                IngresarTextoCeldaGeneral(document, table.Rows[3 + dx].Cells[3], null, sumAplicAgRespX, ParagraphAlignment.Center, TableCellVerticalAlignment.Center, true, "#000000", "");

                string colorCellfapdf = fapDf == "-" ? "#D9D9D9" : "";
                IngresarTextoCeldaGeneral(document, table.Rows[3 + dx].Cells[4], null, fapDf, ParagraphAlignment.Center, TableCellVerticalAlignment.Center, true, "#000000", colorCellfapdf);

                //Si son guion los centro
                if (sumResAgRespX == "-")
                    document.BeginUpdateParagraphs(table.Rows[3 + dx].Cells[2].Range).Alignment = ParagraphAlignment.Center;
                else
                    document.BeginUpdateParagraphs(table.Rows[3 + dx].Cells[2].Range).Alignment = ParagraphAlignment.Right;

                if (sumAplicAgRespX == "-")
                    document.BeginUpdateParagraphs(table.Rows[3 + dx].Cells[3].Range).Alignment = ParagraphAlignment.Center;
                else
                    document.BeginUpdateParagraphs(table.Rows[3 + dx].Cells[3].Range).Alignment = ParagraphAlignment.Right;

                if (fapDf == "-")
                    document.BeginUpdateParagraphs(table.Rows[3 + dx].Cells[4].Range).Alignment = ParagraphAlignment.Center;
                else
                    document.BeginUpdateParagraphs(table.Rows[3 + dx].Cells[4].Range).Alignment = ParagraphAlignment.Right;

            }



            //Merge Celdas
            //Al final hago merge
            table.MergeCells(table[0, 0], table[0, 1]);
            table.MergeCells(table[1, 0], table[1, 1]);
            table.MergeCells(table[2, 0], table[2, 1]);

            table.MergeCells(table[0, 2], table[2, 2]);
            table.MergeCells(table[0, 3], table[2, 3]);


            table.EndUpdate();

        }
        /// <summary>
        /// Arma tabla vertical
        /// </summary>
        /// <param name="document"></param>
        /// <param name="fontArial"></param>
        /// <param name="regVertical"></param>
        /// <param name="tipoTabla"></param>
        private void CrearTablaVertical(DocX document, string fontArial, RegistroWord regVertical)
        {
            int nResp = regVertical.numResponsables;
            int numColumnasV = 4;
            int numFilasV = 3 + nResp;

            Table tableV = document.InsertTable(numFilasV, numColumnasV);
            tableV.Design = TableDesign.TableGrid;

            tableV.AutoFit = AutoFit.Contents;
            //tableV.AutoFit = AutoFit.Window;

            tableV.Rows[0].Height = 80;

            //Verticalmente centro
            tableV.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[0].Cells[3].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[1].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[1].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[1].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[1].Cells[1].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[1].Cells[2].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[1].Cells[2].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[1].Cells[3].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[1].Cells[3].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[2].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[2].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[2].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[2].Cells[1].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[2].Cells[2].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[2].Cells[2].Paragraphs[0].Alignment = Alignment.center;
            tableV.Rows[2].Cells[3].VerticalAlignment = VerticalAlignment.Center;
            tableV.Rows[2].Cells[3].Paragraphs[0].Alignment = Alignment.center;

            for (int i = 0; i < nResp; i++)
            {
                tableV.Rows[3 + i].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                tableV.Rows[3 + i].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                tableV.Rows[3 + i].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                tableV.Rows[3 + i].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                tableV.Rows[3 + i].Cells[2].VerticalAlignment = VerticalAlignment.Center;
                tableV.Rows[3 + i].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                tableV.Rows[3 + i].Cells[3].VerticalAlignment = VerticalAlignment.Center;
                tableV.Rows[3 + i].Cells[3].Paragraphs[0].Alignment = Alignment.center;
            }


            //Textos
            tableV.Rows[0].Cells[0].Paragraphs[0].Append("Agente Responsable").FontSize(7).Font(new FontFamily(fontArial)).Bold();
            tableV.Rows[0].Cells[2].Paragraphs[0].Append("Resarcimiento Agente Resp. (USD)").FontSize(7).Font(new FontFamily(fontArial)).Bold();
            tableV.Rows[0].Cells[3].Paragraphs[0].Append("(*) Resarcimiento Final Agente Resp. (USD)").FontSize(7).Font(new FontFamily(fontArial)).Bold().Color(ColorTranslator.FromHtml("#FFFFFF"));

            tableV.Rows[1].Cells[0].Paragraphs[0].Append("Cliente").FontSize(7).Font(new FontFamily(fontArial)).Bold();
            tableV.Rows[1].Cells[2].Paragraphs[0].Append(regVertical.NombreCliente).FontSize(7).Font(new FontFamily(fontArial));
            tableV.Rows[1].Cells[0].MarginTop = 10;

            tableV.Rows[2].Cells[0].Paragraphs[0].Append("Barra").FontSize(7).Font(new FontFamily(fontArial)).Bold();
            tableV.Rows[2].Cells[2].Paragraphs[0].Append(regVertical.BarraNombre).FontSize(7).Font(new FontFamily(fontArial));
            tableV.Rows[2].Cells[0].MarginTop = 10;

            for (int dx = 0; dx < nResp; dx++)
            {
                string responsableX = "";
                string sumResAgRespX = "";
                string sumAplicAgRespX = "";

                if (dx == 0)
                {
                    responsableX = regVertical.Responsable1Nombre;
                    sumResAgRespX = regVertical.SumAgenteResp1 != null ? (regVertical.SumAgenteResp1.Value != 0 ? regVertical.SumAgenteResp1.Value.ToString("#,##0.00") : "-") : "-";
                    sumAplicAgRespX = regVertical.SumAplicacionAResp1 != null ? (regVertical.SumAplicacionAResp1.Value != 0 ? regVertical.SumAplicacionAResp1.Value.ToString("#,##0.00") : "-") : "-";
                }
                else
                {
                    if (dx == 1)
                    {
                        responsableX = regVertical.Responsable2Nombre;
                        sumResAgRespX = regVertical.SumAgenteResp2 != null ? (regVertical.SumAgenteResp2.Value != 0 ? regVertical.SumAgenteResp2.Value.ToString("#,##0.00") : "-") : "-";
                        sumAplicAgRespX = regVertical.SumAplicacionAResp2 != null ? (regVertical.SumAplicacionAResp2.Value != 0 ? regVertical.SumAplicacionAResp2.Value.ToString("#,##0.00") : "-") : "-";
                    }
                    else
                    {
                        if (dx == 2)
                        {
                            responsableX = regVertical.Responsable3Nombre;
                            sumResAgRespX = regVertical.SumAgenteResp3 != null ? (regVertical.SumAgenteResp3.Value != 0 ? regVertical.SumAgenteResp3.Value.ToString("#,##0.00") : "-") : "-";
                            sumAplicAgRespX = regVertical.SumAplicacionAResp3 != null ? (regVertical.SumAplicacionAResp3.Value != 0 ? regVertical.SumAplicacionAResp3.Value.ToString("#,##0.00") : "-") : "-";
                        }
                        else
                        {
                            if (dx == 3)
                            {
                                responsableX = regVertical.Responsable4Nombre;
                                sumResAgRespX = regVertical.SumAgenteResp4 != null ? (regVertical.SumAgenteResp4.Value != 0 ? regVertical.SumAgenteResp4.Value.ToString("#,##0.00") : "-") : "-";
                                sumAplicAgRespX = regVertical.SumAplicacionAResp4 != null ? (regVertical.SumAplicacionAResp4.Value != 0 ? regVertical.SumAplicacionAResp4.Value.ToString("#,##0.00") : "-") : "-";
                            }
                            else
                            {
                                if (dx == 4)
                                {
                                    responsableX = regVertical.Responsable5Nombre;
                                    sumResAgRespX = regVertical.SumAgenteResp5 != null ? (regVertical.SumAgenteResp5.Value != 0 ? regVertical.SumAgenteResp5.Value.ToString("#,##0.00") : "-") : "-";
                                    sumAplicAgRespX = regVertical.SumAplicacionAResp5 != null ? (regVertical.SumAplicacionAResp5.Value != 0 ? regVertical.SumAplicacionAResp5.Value.ToString("#,##0.00") : "-") : "-";
                                }
                            }
                        }
                    }
                }

                tableV.Rows[3 + dx].Cells[0].Paragraphs[0].Append(string.Format("Agente Resp. {0}", (1 + dx))).FontSize(7).Font(new FontFamily(fontArial)).Bold();
                tableV.Rows[3 + dx].Cells[1].Paragraphs[0].Append(responsableX).FontSize(7).Font(new FontFamily(fontArial));
                tableV.Rows[3 + dx].Cells[2].Paragraphs[0].Append(sumResAgRespX).FontSize(7).Font(new FontFamily(fontArial));
                tableV.Rows[3 + dx].Cells[3].Paragraphs[0].Append(sumAplicAgRespX).FontSize(7).Font(new FontFamily(fontArial));

                //Si son guion los centro
                if (sumResAgRespX == "-")
                    tableV.Rows[3 + dx].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                else
                    tableV.Rows[3 + dx].Cells[2].Paragraphs[0].Alignment = Alignment.right;

                if (sumAplicAgRespX == "-")
                    tableV.Rows[3 + dx].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                else
                    tableV.Rows[3 + dx].Cells[3].Paragraphs[0].Alignment = Alignment.right;
            }

            //Colores
            tableV.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#8DB4E2");
            tableV.Rows[0].Cells[2].FillColor = ColorTranslator.FromHtml("#8DB4E2");
            tableV.Rows[0].Cells[3].FillColor = ColorTranslator.FromHtml("#1F497D");

            tableV.Rows[1].Cells[0].FillColor = ColorTranslator.FromHtml("#8DB4E2");

            tableV.Rows[2].Cells[0].FillColor = ColorTranslator.FromHtml("#8DB4E2");

            for (int dx = 0; dx < nResp; dx++)
            {
                tableV.Rows[3 + dx].Cells[0].FillColor = ColorTranslator.FromHtml("#8DB4E2");
            }

            //Merge Celdas
            tableV.Rows[0].MergeCells(0, 1);
            tableV.Rows[1].MergeCells(0, 1);
            tableV.Rows[1].MergeCells(1, 2);
            tableV.Rows[2].MergeCells(0, 1);
            tableV.Rows[2].MergeCells(1, 2);


            Paragraph pSbPE = document.InsertParagraph();
            pSbPE.AppendLine("");
        }

        /// <summary>
        /// Arma la tabla horizontal
        /// </summary>
        /// <param name="document"></param>
        /// <param name="fontArial"></param>
        /// <param name="regPorSuministrador"></param>
        /// <param name="tipoTabla"></param>
        private void CrearTablaHorizontal(DocX document, string fontArial, DatoPorSuministrador regPorSuministrador, int tipoTabla)
        {
            int nResp = tipoTabla == ConstantesCalculoResarcimiento.TablasPE ? regPorSuministrador.NumResponsablesTablaHorizontalPE.Value : regPorSuministrador.NumResponsablesTablaHorizontalRC.Value;
            int numColumnasH = 2 + 3 * nResp;
            int numFilasH = tipoTabla == ConstantesCalculoResarcimiento.TablasPE ? (regPorSuministrador.NumFilasTablaHorizontalPE.Value + 2) : (regPorSuministrador.NumFilasTablaHorizontalRC.Value + 2);

            Table tableHorizontal = document.InsertTable(numFilasH, numColumnasH);
            tableHorizontal.Design = TableDesign.TableGrid;
            //tableHorizontal.AutoFit = AutoFit.Contents;
            tableHorizontal.AutoFit = AutoFit.Window;

            tableHorizontal.Rows[0].Height = 100;

            //Verticalmente centro
            tableHorizontal.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            tableHorizontal.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            tableHorizontal.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            tableHorizontal.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;

            for (int i = 0; i < nResp; i++)
            {
                tableHorizontal.Rows[0].Cells[2 + 3 * i].VerticalAlignment = VerticalAlignment.Center;
                tableHorizontal.Rows[0].Cells[2 + 3 * i].Paragraphs[0].Alignment = Alignment.center;
                tableHorizontal.Rows[0].Cells[3 + 3 * i].VerticalAlignment = VerticalAlignment.Center;
                tableHorizontal.Rows[0].Cells[3 + 3 * i].Paragraphs[0].Alignment = Alignment.center;
                tableHorizontal.Rows[0].Cells[4 + 3 * i].VerticalAlignment = VerticalAlignment.Center;
                tableHorizontal.Rows[0].Cells[4 + 3 * i].Paragraphs[0].Alignment = Alignment.center;
            }


            //Colores de celdas cabecera
            tableHorizontal.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#8DB4E2");
            tableHorizontal.Rows[0].Cells[1].FillColor = ColorTranslator.FromHtml("#8DB4E2");

            for (int i = 0; i < nResp; i++)
            {
                tableHorizontal.Rows[0].Cells[2 + 3 * i].FillColor = ColorTranslator.FromHtml("#8DB4E2");
                tableHorizontal.Rows[0].Cells[3 + 3 * i].FillColor = ColorTranslator.FromHtml("#8DB4E2");
                tableHorizontal.Rows[0].Cells[4 + 3 * i].FillColor = ColorTranslator.FromHtml("#1F497D");
            }

            //Textos
            tableHorizontal.Rows[0].Cells[0].Paragraphs[0].Append("Cliente").FontSize(7).Font(new FontFamily(fontArial)).Bold();
            tableHorizontal.Rows[0].Cells[1].Paragraphs[0].Append("Barra").FontSize(7).Font(new FontFamily(fontArial)).Bold();

            for (int i = 0; i < nResp; i++)
            {
                tableHorizontal.Rows[0].Cells[2 + 3 * i].Paragraphs[0].Append(string.Format("Agente Resp. {0}", (1 + i))).FontSize(7).Font(new FontFamily(fontArial)).Bold();
                tableHorizontal.Rows[0].Cells[3 + 3 * i].Paragraphs[0].Append(string.Format("Resarcimiento\r\nAgente Resp. {0} (USD)", (1 + i))).FontSize(7).Font(new FontFamily(fontArial)).Bold();
                tableHorizontal.Rows[0].Cells[4 + 3 * i].Paragraphs[0].Append(string.Format("(*) Resarcimiento Final\r\nAgente Resp. {0} (USD) ", (1 + i))).FontSize(7).Font(new FontFamily(fontArial)).Bold().Color(ColorTranslator.FromHtml("#FFFFFF"));
            }


            //Ingresar data en el cuerpo
            //Tamaño texto campos texto
            int tamCampoTexto = tipoTabla == ConstantesCalculoResarcimiento.TablasPE ? (regPorSuministrador.NumResponsablesTablaHorizontalPE == 3 ? 6 : 7) : (regPorSuministrador.NumResponsablesTablaHorizontalRC == 3 ? 6 : 7);
            int fila = 1;
            List<RegistroWord> listaUsar = tipoTabla == ConstantesCalculoResarcimiento.TablasPE ? regPorSuministrador.ListaPEHorizontal : regPorSuministrador.ListaRCHorizontal;
            foreach (var reg in listaUsar)
            {
                tableHorizontal.Rows[fila].Cells[0].Paragraphs[0].Append(reg.NombreCliente.ToUpper()).FontSize(tamCampoTexto).Font(new FontFamily(fontArial));
                tableHorizontal.Rows[fila].Cells[1].Paragraphs[0].Append(reg.BarraNombre.ToUpper()).FontSize(tamCampoTexto).Font(new FontFamily(fontArial));

                for (int i = 0; i < nResp; i++)
                {
                    string nombRX = "";
                    string resAgRespX = "";
                    string aplicAgRespX = "";

                    if (i == 0)
                    {
                        nombRX = reg.Responsable1Nombre.Trim();
                        resAgRespX = reg.SumAgenteResp1 != null ? (reg.SumAgenteResp1.Value != 0 ? reg.SumAgenteResp1.Value.ToString("#,##0.00") : "-") : "-";
                        aplicAgRespX = reg.SumAplicacionAResp1 != null ? (reg.SumAplicacionAResp1.Value != 0 ? reg.SumAplicacionAResp1.Value.ToString("#,##0.00") : "-") : "-";
                    }
                    else
                    {
                        if (i == 1)
                        {
                            nombRX = reg.Responsable2Nombre.Trim();
                            resAgRespX = reg.SumAgenteResp2 != null ? (reg.SumAgenteResp2.Value != 0 ? reg.SumAgenteResp2.Value.ToString("#,##0.00") : "-") : "-";
                            aplicAgRespX = reg.SumAplicacionAResp2 != null ? (reg.SumAplicacionAResp2.Value != 0 ? reg.SumAplicacionAResp2.Value.ToString("#,##0.00") : "-") : "-";
                        }
                        else
                        {
                            if (i == 2)
                            {
                                nombRX = reg.Responsable3Nombre.Trim();
                                resAgRespX = reg.SumAgenteResp3 != null ? (reg.SumAgenteResp3.Value != 0 ? reg.SumAgenteResp3.Value.ToString("#,##0.00") : "-") : "-";
                                aplicAgRespX = reg.SumAplicacionAResp3 != null ? (reg.SumAplicacionAResp3.Value != 0 ? reg.SumAplicacionAResp3.Value.ToString("#,##0.00") : "-") : "-";
                            }
                        }
                    }


                    tableHorizontal.Rows[fila].Cells[2 + 3 * i].Paragraphs[0].Append(nombRX).FontSize(tamCampoTexto).Font(new FontFamily(fontArial));
                    tableHorizontal.Rows[fila].Cells[3 + 3 * i].Paragraphs[0].Append(resAgRespX).FontSize(7).Font(new FontFamily(fontArial));
                    tableHorizontal.Rows[fila].Cells[4 + 3 * i].Paragraphs[0].Append(aplicAgRespX).FontSize(7).Font(new FontFamily(fontArial));



                    //Si son guion los centro
                    if (resAgRespX == "-")
                        tableHorizontal.Rows[fila].Cells[3 + 3 * i].Paragraphs[0].Alignment = Alignment.center;
                    else
                        tableHorizontal.Rows[fila].Cells[3 + 3 * i].Paragraphs[0].Alignment = Alignment.right;

                    if (aplicAgRespX == "-")
                        tableHorizontal.Rows[fila].Cells[4 + 3 * i].Paragraphs[0].Alignment = Alignment.center;
                    else
                        tableHorizontal.Rows[fila].Cells[4 + 3 * i].Paragraphs[0].Alignment = Alignment.right;
                }

                //Verticalmente centro
                tableHorizontal.Rows[fila].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                tableHorizontal.Rows[fila].Cells[1].VerticalAlignment = VerticalAlignment.Center;

                for (int i = 0; i < nResp; i++)
                {
                    tableHorizontal.Rows[fila].Cells[2 + 3 * i].VerticalAlignment = VerticalAlignment.Center;
                    tableHorizontal.Rows[fila].Cells[3 + 3 * i].VerticalAlignment = VerticalAlignment.Center;
                    tableHorizontal.Rows[fila].Cells[4 + 3 * i].VerticalAlignment = VerticalAlignment.Center;
                }

                fila++;
            }

            //Pie de tabla
            tableHorizontal.Rows[fila].Cells[0].Paragraphs[0].Append("TOTAL GENERAL").FontSize(7).Font(new FontFamily(fontArial)).Bold();
            tableHorizontal.Rows[fila].Cells[1].Paragraphs[0].Append("").FontSize(7).Font(new FontFamily(fontArial)).Bold();
            tableHorizontal.Rows[fila].Cells[0].FillColor = ColorTranslator.FromHtml("#8DB4E2");
            tableHorizontal.Rows[fila].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            tableHorizontal.Rows[fila].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            tableHorizontal.Rows[fila].Cells[0].MarginTop = 10;
            //
            Totales objTotal3Menos = tipoTabla == ConstantesCalculoResarcimiento.TablasPE ? regPorSuministrador.TotalPE3Menos : regPorSuministrador.TotalRC3Menos;

            for (int dx = 0; dx < nResp; dx++)
            {
                string sumResAgRespX = "";
                string sumAplicAgRespX = "";

                if (dx == 0)
                {
                    sumResAgRespX = objTotal3Menos.SumTotalAgenteResp1 != null ? (objTotal3Menos.SumTotalAgenteResp1.Value != 0 ? objTotal3Menos.SumTotalAgenteResp1.Value.ToString("#,##0.00") : "-") : "-";
                    sumAplicAgRespX = objTotal3Menos.SumTotalAplicacionAResp1 != null ? (objTotal3Menos.SumTotalAplicacionAResp1.Value != 0 ? objTotal3Menos.SumTotalAplicacionAResp1.Value.ToString("#,##0.00") : "-") : "-";
                }
                else
                {
                    if (dx == 1)
                    {
                        sumResAgRespX = objTotal3Menos.SumTotalAgenteResp2 != null ? (objTotal3Menos.SumTotalAgenteResp2.Value != 0 ? objTotal3Menos.SumTotalAgenteResp2.Value.ToString("#,##0.00") : "-") : "-";
                        sumAplicAgRespX = objTotal3Menos.SumTotalAplicacionAResp2 != null ? (objTotal3Menos.SumTotalAplicacionAResp2.Value != 0 ? objTotal3Menos.SumTotalAplicacionAResp2.Value.ToString("#,##0.00") : "-") : "-";
                    }
                    else
                    {
                        if (dx == 2)
                        {
                            sumResAgRespX = objTotal3Menos.SumTotalAgenteResp3 != null ? (objTotal3Menos.SumTotalAgenteResp3.Value != 0 ? objTotal3Menos.SumTotalAgenteResp3.Value.ToString("#,##0.00") : "-") : "-";
                            sumAplicAgRespX = objTotal3Menos.SumTotalAplicacionAResp3 != null ? (objTotal3Menos.SumTotalAplicacionAResp3.Value != 0 ? objTotal3Menos.SumTotalAplicacionAResp3.Value.ToString("#,##0.00") : "-") : "-";
                        }
                    }
                }

                tableHorizontal.Rows[fila].Cells[2 + 3 * dx].Paragraphs[0].Append("").FontSize(7).Font(new FontFamily(fontArial)).Bold();
                tableHorizontal.Rows[fila].Cells[3 + 3 * dx].Paragraphs[0].Append(sumResAgRespX).FontSize(7).Font(new FontFamily(fontArial)).Bold();
                tableHorizontal.Rows[fila].Cells[4 + 3 * dx].Paragraphs[0].Append(sumAplicAgRespX).FontSize(7).Font(new FontFamily(fontArial)).Bold();

                //centro verticalmente
                tableHorizontal.Rows[fila].Cells[2 + 3 * dx].VerticalAlignment = VerticalAlignment.Center;
                tableHorizontal.Rows[fila].Cells[3 + 3 * dx].VerticalAlignment = VerticalAlignment.Center;
                tableHorizontal.Rows[fila].Cells[4 + 3 * dx].VerticalAlignment = VerticalAlignment.Center;

                //Si son guion los centro
                if (sumResAgRespX == "-")
                    tableHorizontal.Rows[fila].Cells[3 + 3 * dx].Paragraphs[0].Alignment = Alignment.center;
                else
                    tableHorizontal.Rows[fila].Cells[3 + 3 * dx].Paragraphs[0].Alignment = Alignment.right;

                if (sumAplicAgRespX == "-")
                    tableHorizontal.Rows[fila].Cells[4 + 3 * dx].Paragraphs[0].Alignment = Alignment.center;
                else
                    tableHorizontal.Rows[fila].Cells[4 + 3 * dx].Paragraphs[0].Alignment = Alignment.right;
            }

            //Al final hago merge
            tableHorizontal.Rows[fila].MergeCells(0, 1);


            Paragraph paSbCX = document.InsertParagraph();
            paSbCX.AppendLine();
            paSbCX.AppendLine();
        }


        #endregion

        #endregion

        #region Reportes en Zip

        #region General        

        /// <summary>
        /// Devuelve el nombre de cierto reporte excel
        /// </summary>
        /// <param name="codigoReporte"></param>
        /// <returns></returns>
        public string ObtenerNombreReporteEnZip(int codigoReporte)
        {
            string salida = "";
            switch (codigoReporte)
            {
                case ConstantesCalculoResarcimiento.ReporteZipInterrupcionesPorAgenteResponsable:
                    salida = "Interrupciones por Agente Responsable.zip";
                    break;
                case ConstantesCalculoResarcimiento.ReporteZipInterrupcionesPorSuministrador:
                    salida = "Interrupciones por suministrador.zip";
                    break;

            }
            return salida;
        }

        /// <summary>
        /// Genera reportes en zip
        /// </summary>
        /// <param name="periodoId"></param>
        /// <param name="codigoReporte"></param>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="nameFile"></param>
        public void GenerarReporteZip(int periodoId, int codigoReporte, string ruta, string pathLogo, string nameFile)
        {
            RePeriodoDTO periodo = GetByIdRePeriodo(periodoId);

            switch (codigoReporte)
            {
                case ConstantesCalculoResarcimiento.ReporteZipInterrupcionesPorAgenteResponsable:
                    GenerarArchivosExcelPorAgente(periodo, pathLogo, ruta);
                    break;
                case ConstantesCalculoResarcimiento.ReporteZipInterrupcionesPorSuministrador:
                    GenerarArchivosExcelPorSuministrador(periodo, pathLogo, ruta);
                    break;
            }

            GenerarArchivosZipReporte(ruta, codigoReporte, nameFile);
        }

        /// <summary>
        /// Zipear archivos .xlsx generados 
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="codigoReporte"></param>
        /// <param name="nameFile"></param>
        private void GenerarArchivosZipReporte(string ruta, int codigoReporte, string nameFile)
        {
            try
            {
                string path = "";
                if (codigoReporte == ConstantesCalculoResarcimiento.ReporteZipInterrupcionesPorAgenteResponsable)
                    path = ruta + "ReportePorAgente\\";
                if (codigoReporte == ConstantesCalculoResarcimiento.ReporteZipInterrupcionesPorSuministrador)
                    path = ruta + "ReportePorSuministrador\\";
                var nombreCarpeta = "SALIDA";
                var rutaZip = path + nameFile;


                if (File.Exists(rutaZip)) File.Delete(rutaZip);
                FileServer.CreateZipFromDirectory(nombreCarpeta, rutaZip, path);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Reporte Interrupciones por Agente Responsable

        /// <summary>
        /// Genera los archivos excel por cada agente
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="pathLogo"></param>
        /// <param name="ruta"></param>
        private void GenerarArchivosExcelPorAgente(RePeriodoDTO periodo, string pathLogo, string ruta)
        {
            List<InterrupcionSuministroIntranet> listaGeneral = new List<InterrupcionSuministroIntranet>();

            listaGeneral = ObtenerListaInterrupcionIntranetPorPeriodo(periodo);
            //listaGeneral = new List<InterrupcionSuministroIntranet>();  //para hacer evidencias de listado sin registros


            //Crear carpeta para generar los excel
            var patTrabajo = "ReportePorAgente\\";
            string nombreCarpeta = "SALIDA";
            FileServer.DeleteFolderAlter(patTrabajo, ruta);
            FileServer.CreateFolder(patTrabajo, nombreCarpeta, ruta);
            //Generar los .dat
            var rutaFinalArchivoDat = ruta + patTrabajo + nombreCarpeta + "\\";

            //Obtenemos el total de agentes responsables
            List<int> lstAgentesResponsablesTotales = ObtenerTotalAgentesResponsables(listaGeneral);

            //Obtenemos los nombres de los suministrdores
            Dictionary<int, string> lstNombreResponsables = ObtenerRelacionResponsables(lstAgentesResponsablesTotales, listaGeneral);

            if (!lstAgentesResponsablesTotales.Any())
            {
                throw new Exception("No se encontraron agentes responsables para el periodo seleccionado.");
            }

            foreach (int agenteId in lstAgentesResponsablesTotales)
            {
                string nombreAgente = lstNombreResponsables[agenteId];

                List<InterrupcionSuministroIntranet> listaPorAgente = listaGeneral.Where(x => x.Responsable1Id == agenteId || x.Responsable2Id == agenteId
                                                                     || x.Responsable3Id == agenteId || x.Responsable4Id == agenteId || x.Responsable5Id == agenteId).ToList();
                GenerarExcelParaAgente(periodo, listaPorAgente, nombreAgente, rutaFinalArchivoDat, pathLogo);
            }
        }

        /// <summary>
        /// Devuelve la lista de agentes responsables
        /// </summary>
        /// <param name="lstAgentesResponsablesTotales"></param>
        /// <param name="listaGeneral"></param>
        /// <returns></returns>
        public Dictionary<int, string> ObtenerRelacionResponsables(List<int> lstAgentesResponsablesTotales, List<InterrupcionSuministroIntranet> listaGeneral)
        {
            Dictionary<int, string> lstSalida = new Dictionary<int, string>();

            ////- Aca debemos cambiar porque las interrupciones son distintas
            EstructuraInterrupcion maestros = new EstructuraInterrupcion();
            maestros.ListaEmpresa = this.ObtenerEmpresas();

            int num = 1;
            foreach (var agenteId in lstAgentesResponsablesTotales)
            {

                ReEmpresaDTO agente = maestros.ListaEmpresa.Find(x => x.Emprcodi == agenteId);

                string nombAgente = "";

                if (agente != null)
                {
                    if (agente.Emprnomb != null)
                    {
                        if (agente.Emprnomb != "")
                        {
                            nombAgente = agente.Emprnomb.Trim();
                        }
                        else
                        {
                            nombAgente = "AgenteResponsable_" + num;
                            num++;
                        }
                    }
                    else
                    {
                        nombAgente = "AgenteResp_" + num;
                        num++;
                    }

                }
                else
                {
                    nombAgente = "AgenteResp_" + num;
                    num++;
                }

                lstSalida.Add(agenteId, nombAgente);
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado de interrupciones intranet por periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<InterrupcionSuministroIntranet> ObtenerListaInterrupcionIntranetPorPeriodo(RePeriodoDTO periodo)
        {
            List<InterrupcionSuministroIntranet> lstSalida = new List<InterrupcionSuministroIntranet>();

            //Obtengo las interrupciones por periodo
            List<ReInterrupcionSuministroDTO> lstInterrupciones = FactorySic.GetReInterrupcionSuministroRepository().ObtenerPorEmpresaPeriodo(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi);

            //Obtenemos los detalles de las interrupciones
            List<ReInterrupcionSuministroDetDTO> lstDetallesInterrupcionesIntranet = FactorySic.GetReInterrupcionSuministroDetRepository().ObtenerPorEmpresaPeriodo(ConstantesCalculoResarcimiento.ParametroDefecto, periodo.Repercodi);

            int idPeriodoUsar = this.ObtenerIdPeriodoPadre(periodo.Repercodi);

            ////- Aca debemos cambiar porque las interrupciones son distintas
            EstructuraInterrupcion maestros = new EstructuraInterrupcion();
            maestros.ListaCliente = this.ObtenerEmpresas();
            maestros.ListaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
            maestros.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().List();
            maestros.ListaNivelTension = FactorySic.GetReNivelTensionRepository().List();
            maestros.ListaEmpresa = this.ObtenerEmpresasSuministradorasTotal();
            maestros.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoUsar);

            //generar Listado
            foreach (var interrupcion in lstInterrupciones)
            {
                int interrupcionId = interrupcion.Reintcodi;

                List<ReInterrupcionSuministroDetDTO> lstDetalles = lstDetallesInterrupcionesIntranet.Where(x => x.Reintcodi == interrupcionId).ToList();
                List<ReEmpresaDTO> lstEmpresasPorCliente = maestros.ListaCliente.Where(x => x.Emprcodi == interrupcion.Reintcliente).ToList();
                RePtoentregaPeriodoDTO ptoERegulado = maestros.ListaPuntoEntrega.Find(x => x.Repentcodi == interrupcion.Repentcodi);
                ReNivelTensionDTO nivTension = maestros.ListaNivelTension.Find(x => x.Rentcodi == interrupcion.Rentcodi);
                ReTipoInterrupcionDTO tipoInt = maestros.ListaTipoInterrupcion.Find(x => x.Retintcodi == interrupcion.Retintcodi);
                ReCausaInterrupcionDTO causaInt = maestros.ListaCausaInterrupcion.Find(x => x.Recintcodi == interrupcion.Recintcodi);

                ReInterrupcionSuministroDetDTO r1 = lstDetalles.Find(x => x.Reintdorden == 1);
                ReInterrupcionSuministroDetDTO r2 = lstDetalles.Find(x => x.Reintdorden == 2);
                ReInterrupcionSuministroDetDTO r3 = lstDetalles.Find(x => x.Reintdorden == 3);
                ReInterrupcionSuministroDetDTO r4 = lstDetalles.Find(x => x.Reintdorden == 4);
                ReInterrupcionSuministroDetDTO r5 = lstDetalles.Find(x => x.Reintdorden == 5);
                ReEmpresaDTO resp1 = r1 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r1.Emprcodi) : null;
                ReEmpresaDTO resp2 = r2 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r2.Emprcodi) : null;
                ReEmpresaDTO resp3 = r3 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r3.Emprcodi) : null;
                ReEmpresaDTO resp4 = r4 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r4.Emprcodi) : null;
                ReEmpresaDTO resp5 = r5 != null ? maestros.ListaCliente.Find(x => x.Emprcodi == r5.Emprcodi) : null;

                InterrupcionSuministroIntranet objInt = new InterrupcionSuministroIntranet();
                objInt.SuministradorId = interrupcion.Emprcodi.Value;
                objInt.PuntoEntregaId = interrupcion.Repentcodi; //para regulados
                objInt.NivelTensionId = interrupcion.Rentcodi;
                objInt.CausaId = interrupcion.Recintcodi;

                int? intNulo = null;
                objInt.Responsable1Id = resp1 != null ? resp1.Emprcodi : intNulo;
                objInt.Responsable2Id = resp2 != null ? resp2.Emprcodi : intNulo;
                objInt.Responsable3Id = resp3 != null ? resp3.Emprcodi : intNulo;
                objInt.Responsable4Id = resp4 != null ? resp4.Emprcodi : intNulo;
                objInt.Responsable5Id = resp5 != null ? resp5.Emprcodi : intNulo;

                //numero agentes responsables
                int numAgentes = 0;
                if (objInt.Responsable1Id != null) numAgentes++;
                if (objInt.Responsable2Id != null) numAgentes++;
                if (objInt.Responsable3Id != null) numAgentes++;
                if (objInt.Responsable4Id != null) numAgentes++;
                if (objInt.Responsable5Id != null) numAgentes++;
                objInt.NumeroAgentesResponsables = numAgentes;

                ReEmpresaDTO sum = maestros.ListaEmpresa.Find(x => x.Emprcodi == interrupcion.Emprcodi);
                objInt.Suministrador = sum != null ? sum.Emprnomb.Trim() : "";
                objInt.Correlativo = interrupcion.Reintcorrelativo;
                objInt.TipoCliente = interrupcion.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado ? ConstantesCalculoResarcimiento.TextoClienteRegulado : (interrupcion.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteLibre ? ConstantesCalculoResarcimiento.TextoClienteLibre : "");

                objInt.NombreCliente = lstEmpresasPorCliente.Any() ? (lstEmpresasPorCliente.First().Emprnomb != null ? (lstEmpresasPorCliente.First().Emprnomb.Trim()) : "") : "";
                objInt.PuntoEntregaBarraNombre = (interrupcion.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado) ? (ptoERegulado != null ? (ptoERegulado.Repentnombre != null ? ptoERegulado.Repentnombre.Trim() : "") : "") : (interrupcion.Reintptoentrega != null ? interrupcion.Reintptoentrega.Trim() : "");
                objInt.NumSuministroClienteLibre = (interrupcion.Reintnrosuministro != null) ? interrupcion.Reintnrosuministro.ToString() : null;
                objInt.NivelTensionNombre = interrupcion.Rentcodi != null ? (nivTension != null ? (nivTension.Rentnombre != null ? nivTension.Rentnombre.Trim() : "") : "") : "";

                objInt.AplicacionLiteral = interrupcion.Reintaplicacionnumeral;
                objInt.EnergiaPeriodo = interrupcion.Reintenergiasemestral;
                objInt.IncrementoTolerancia = interrupcion.Reintinctolerancia == "S" ? ConstantesCalculoResarcimiento.TextoSi : (interrupcion.Reintinctolerancia == "N" ? ConstantesCalculoResarcimiento.TextoNo : "");
                objInt.TipoNombre = interrupcion.Retintcodi != null ? (tipoInt != null ? (tipoInt.Retintnombre != null ? tipoInt.Retintnombre.Trim() : "") : "") : "";
                objInt.CausaNombre = interrupcion.Recintcodi != null ? (causaInt != null ? (causaInt.Recintnombre != null ? causaInt.Recintnombre.Trim() : "") : "") : "";
                objInt.Ni = interrupcion.Reintni;
                objInt.Ki = interrupcion.Reintki;
                objInt.TiempoEjecutadoIni = interrupcion.Reintfejeinicio != null ? interrupcion.Reintfejeinicio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                objInt.TiempoEjecutadoFin = interrupcion.Reintfejefin != null ? interrupcion.Reintfejefin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                objInt.TiempoProgramadoIni = interrupcion.Reintfproginicio != null ? interrupcion.Reintfproginicio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                objInt.TiempoProgramadoFin = interrupcion.Reintfprogfin != null ? interrupcion.Reintfprogfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                objInt.Responsable1Nombre = resp1 != null ? (resp1.Emprnomb != null ? resp1.Emprnomb.Trim() : "") : "";
                objInt.Responsable1Porcentaje = r1 != null ? r1.Reintdorcentaje : null;
                objInt.Responsable2Nombre = resp2 != null ? (resp2.Emprnomb != null ? resp2.Emprnomb.Trim() : "") : "";
                objInt.Responsable2Porcentaje = r2 != null ? r2.Reintdorcentaje : null;
                objInt.Responsable3Nombre = resp3 != null ? (resp3.Emprnomb != null ? resp3.Emprnomb.Trim() : "") : "";
                objInt.Responsable3Porcentaje = r3 != null ? r3.Reintdorcentaje : null;
                objInt.Responsable4Nombre = resp4 != null ? (resp4.Emprnomb != null ? resp4.Emprnomb.Trim() : "") : "";
                objInt.Responsable4Porcentaje = r4 != null ? r4.Reintdorcentaje : null;
                objInt.Responsable5Nombre = resp5 != null ? (resp5.Emprnomb != null ? resp5.Emprnomb.Trim() : "") : "";
                objInt.Responsable5Porcentaje = r5 != null ? r5.Reintdorcentaje : null;
                objInt.CausaResumida = interrupcion.Reintcausaresumida != null ? interrupcion.Reintcausaresumida.Trim() : "";
                objInt.EiE = interrupcion.Reinteie;
                objInt.Resarcimiento = interrupcion.Reintresarcimiento;

                //seteo en controversia a false
                objInt.RegistroEnControversiaSum = false;
                objInt.RegistroEnControversiaResp = false;

                objInt.Resp1ConformidadResponsable = r1 != null ? (r1.Reintdconformidadresp != null ? (r1.Reintdconformidadresp.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r1.Reintdconformidadresp.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaResp = objInt.RegistroEnControversiaResp != true ? (objInt.Resp1ConformidadResponsable == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp1Observacion = r1 != null ? (r1.Reintdobservacionresp != null ? r1.Reintdobservacionresp.Trim() : "") : "";
                objInt.Resp1DetalleObservacion = r1 != null ? (r1.Reintddetalleresp != null ? r1.Reintddetalleresp.Trim() : "") : "";
                objInt.Resp1Comentario1 = r1 != null ? (r1.Reintdcomentarioresp != null ? r1.Reintdcomentarioresp.Trim() : "") : "";
                objInt.Resp1ConformidadSuministrador = r1 != null ? (r1.Reintdconformidadsumi != null ? (r1.Reintdconformidadsumi.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r1.Reintdconformidadsumi.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaSum = objInt.RegistroEnControversiaSum != true ? (objInt.Resp1ConformidadSuministrador == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp1Comentario2 = r1 != null ? (r1.Reintdcomentariosumi != null ? r1.Reintdcomentariosumi.Trim() : "") : "";

                objInt.Resp2ConformidadResponsable = r2 != null ? (r2.Reintdconformidadresp != null ? (r2.Reintdconformidadresp.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r2.Reintdconformidadresp.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaResp = objInt.RegistroEnControversiaResp != true ? (objInt.Resp2ConformidadResponsable == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp2Observacion = r2 != null ? (r2.Reintdobservacionresp != null ? r2.Reintdobservacionresp.Trim() : "") : "";
                objInt.Resp2DetalleObservacion = r2 != null ? (r2.Reintddetalleresp != null ? r2.Reintddetalleresp.Trim() : "") : "";
                objInt.Resp2Comentario1 = r2 != null ? (r2.Reintdcomentarioresp != null ? r2.Reintdcomentarioresp.Trim() : "") : "";
                objInt.Resp2ConformidadSuministrador = r2 != null ? (r2.Reintdconformidadsumi != null ? (r2.Reintdconformidadsumi.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r2.Reintdconformidadsumi.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaSum = objInt.RegistroEnControversiaSum != true ? (objInt.Resp2ConformidadSuministrador == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp2Comentario2 = r2 != null ? (r2.Reintdcomentariosumi != null ? r2.Reintdcomentariosumi.Trim() : "") : "";

                objInt.Resp3ConformidadResponsable = r3 != null ? (r3.Reintdconformidadresp != null ? (r3.Reintdconformidadresp.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r3.Reintdconformidadresp.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaResp = objInt.RegistroEnControversiaResp != true ? (objInt.Resp3ConformidadResponsable == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp3Observacion = r3 != null ? (r3.Reintdobservacionresp != null ? r3.Reintdobservacionresp.Trim() : "") : "";
                objInt.Resp3DetalleObservacion = r3 != null ? (r3.Reintddetalleresp != null ? r3.Reintddetalleresp.Trim() : "") : "";
                objInt.Resp3Comentario1 = r3 != null ? (r3.Reintdcomentarioresp != null ? r3.Reintdcomentarioresp.Trim() : "") : "";
                objInt.Resp3ConformidadSuministrador = r3 != null ? (r3.Reintdconformidadsumi != null ? (r3.Reintdconformidadsumi.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r3.Reintdconformidadsumi.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaSum = objInt.RegistroEnControversiaSum != true ? (objInt.Resp3ConformidadSuministrador == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp3Comentario2 = r3 != null ? (r3.Reintdcomentariosumi != null ? r3.Reintdcomentariosumi.Trim() : "") : "";

                objInt.Resp4ConformidadResponsable = r4 != null ? (r4.Reintdconformidadresp != null ? (r4.Reintdconformidadresp.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r4.Reintdconformidadresp.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaResp = objInt.RegistroEnControversiaResp != true ? (objInt.Resp4ConformidadResponsable == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp4Observacion = r4 != null ? (r4.Reintdobservacionresp != null ? r4.Reintdobservacionresp.Trim() : "") : "";
                objInt.Resp4DetalleObservacion = r4 != null ? (r4.Reintddetalleresp != null ? r4.Reintddetalleresp.Trim() : "") : "";
                objInt.Resp4Comentario1 = r4 != null ? (r4.Reintdcomentarioresp != null ? r4.Reintdcomentarioresp.Trim() : "") : "";
                objInt.Resp4ConformidadSuministrador = r4 != null ? (r4.Reintdconformidadsumi != null ? (r4.Reintdconformidadsumi.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r4.Reintdconformidadsumi.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaSum = objInt.RegistroEnControversiaSum != true ? (objInt.Resp4ConformidadSuministrador == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp4Comentario2 = r4 != null ? (r4.Reintdcomentariosumi != null ? r4.Reintdcomentariosumi.Trim() : "") : "";

                objInt.Resp5ConformidadResponsable = r5 != null ? (r5.Reintdconformidadresp != null ? (r5.Reintdconformidadresp.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r5.Reintdconformidadresp.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaResp = objInt.RegistroEnControversiaResp != true ? (objInt.Resp5ConformidadResponsable == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp5Observacion = r5 != null ? (r5.Reintdobservacionresp != null ? r5.Reintdobservacionresp.Trim() : "") : "";
                objInt.Resp5DetalleObservacion = r5 != null ? (r5.Reintddetalleresp != null ? r5.Reintddetalleresp.Trim() : "") : "";
                objInt.Resp5Comentario1 = r5 != null ? (r5.Reintdcomentarioresp != null ? r5.Reintdcomentarioresp.Trim() : "") : "";
                objInt.Resp5ConformidadSuministrador = r5 != null ? (r5.Reintdconformidadsumi != null ? (r5.Reintdconformidadsumi.Trim() == "S" ? ConstantesCalculoResarcimiento.TextoSi : (r5.Reintdconformidadsumi.Trim() == "N" ? ConstantesCalculoResarcimiento.TextoNo : "")) : "") : "";
                objInt.RegistroEnControversiaSum = objInt.RegistroEnControversiaSum != true ? (objInt.Resp5ConformidadSuministrador == ConstantesCalculoResarcimiento.TextoNo ? true : false) : true;
                objInt.Resp5Comentario2 = r5 != null ? (r5.Reintdcomentariosumi != null ? r5.Reintdcomentariosumi.Trim() : "") : "";

                objInt.DesicionControversia = interrupcion.Reintdescontroversia != null ? interrupcion.Reintdescontroversia.Trim() : "";
                objInt.ComentarioFinal = interrupcion.Reintcomentario != null ? interrupcion.Reintcomentario.Trim() : "";

                lstSalida.Add(objInt);
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve los codigos de todos los agentes responsables 
        /// </summary>
        /// <param name="listaGeneral"></param>
        /// <returns></returns>
        private List<int> ObtenerTotalAgentesResponsables(List<InterrupcionSuministroIntranet> listaGeneral)
        {
            List<int> lstSalida = new List<int>();

            List<int> lstResponsables1 = listaGeneral.Where(x => x.Responsable1Id != null).Select(x => x.Responsable1Id.Value).Distinct().ToList();
            List<int> lstResponsables2 = listaGeneral.Where(x => x.Responsable2Id != null).Select(x => x.Responsable2Id.Value).Distinct().ToList();
            List<int> lstResponsables3 = listaGeneral.Where(x => x.Responsable3Id != null).Select(x => x.Responsable3Id.Value).Distinct().ToList();
            List<int> lstResponsables4 = listaGeneral.Where(x => x.Responsable4Id != null).Select(x => x.Responsable4Id.Value).Distinct().ToList();
            List<int> lstResponsables5 = listaGeneral.Where(x => x.Responsable5Id != null).Select(x => x.Responsable5Id.Value).Distinct().ToList();

            lstSalida.AddRange(lstResponsables1);
            lstSalida.AddRange(lstResponsables2);
            lstSalida.AddRange(lstResponsables3);
            lstSalida.AddRange(lstResponsables4);
            lstSalida.AddRange(lstResponsables5);

            lstSalida = lstSalida.Distinct().ToList();

            return lstSalida;
        }

        /// <summary>
        /// Genera un archivo excel del reporte
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="listaPorAgente"></param>
        /// <param name="nombreAgente"></param>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="lstGeneral"></param>
        public void GenerarExcelParaAgente(RePeriodoDTO periodo, List<InterrupcionSuministroIntranet> listaPorAgente, string nombreAgente, string ruta, string pathLogo)
        {
            ////Descargo archivo segun requieran
            string rutaFile = ruta + nombreAgente + ".xlsx";

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarArchivoExcelInterrupcionesAgenteResponsable(xlPackage, pathLogo, listaPorAgente, nombreAgente, periodo);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera el reporte Interrupciones en Controversia
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        private void GenerarArchivoExcelInterrupcionesAgenteResponsable(ExcelPackage xlPackage, string pathLogo, List<InterrupcionSuministroIntranet> listaPorAgente, string nombreAgente, RePeriodoDTO periodo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroIntranet> lista = new List<InterrupcionSuministroIntranet>();

            lista = listaPorAgente;

            int numResponsables = ObtenerNumeroResponsables(lista);

            string nameWS = "Listado";
            string titulo = "Interrupciones por Agente Responsable (" + nombreAgente + ")";
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera 

            int colIniTitulo = 1;
            int rowIniTitulo = 3;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;

            int colSuministrador = colIniTable;
            int colCorrelativo = colIniTable + 1;
            int colTipoCliente = colIniTable + 2;
            int colNombreCliente = colIniTable + 3;
            int colPuntoEntregaBarraNombre = colIniTable + 4;
            int colNumSuministroClienteLibre = colIniTable + 5;
            int colNivelTensionNombre = colIniTable + 6;
            int colAplicacionLiteral = colIniTable + 7;
            int colEnergiaPeriodo = colIniTable + 8;
            int colIncrementoTolerancia = colIniTable + 9;
            int colTipoNombre = colIniTable + 10;
            int colCausaNombre = colIniTable + 11;
            int colNi = colIniTable + 12;
            int colKi = colIniTable + 13;
            int colTiempoEjecutadoIni = colIniTable + 14;
            int colTiempoEjecutadoFin = colIniTable + 15;
            int colTiempoProgramadoIni = colIniTable + 16;
            int colTiempoProgramadoFin = colIniTable + 17;
            int colResponsable1Nombre = colIniTable + 18;
            int colResponsable1Porcentaje = colIniTable + 19;
            int colResponsable2Nombre = colIniTable + 20;
            int colResponsable2Porcentaje = colIniTable + 21;
            int colResponsable3Nombre = colIniTable + 22;
            int colResponsable3Porcentaje = colIniTable + 23;
            int colResponsable4Nombre = colIniTable + 24;
            int colResponsable4Porcentaje = colIniTable + 25;
            int colResponsable5Nombre = colIniTable + 26;
            int colResponsable5Porcentaje = colIniTable + 27;
            int colCausaResumida = colIniTable + 28;
            int colEiE = colIniTable + 29;
            int colResarcimiento = colIniTable + 30;

            int colResp1ConformidadResponsable = colIniTable + 31;
            int colResp1Observacion = colIniTable + 32;
            int colResp1DetalleObservacion = colIniTable + 33;
            int colResp1Comentario1 = colIniTable + 34;
            int colResp1ConformidadSuministrador = colIniTable + 35;
            int colResp1Comentario2 = colIniTable + 36;

            int colResp2ConformidadResponsable = colIniTable + 37;
            int colResp2Observacion = colIniTable + 38;
            int colResp2DetalleObservacion = colIniTable + 39;
            int colResp2Comentario1 = colIniTable + 40;
            int colResp2ConformidadSuministrador = colIniTable + 41;
            int colResp2Comentario2 = colIniTable + 42;

            int colResp3ConformidadResponsable = colIniTable + 43;
            int colResp3Observacion = colIniTable + 44;
            int colResp3DetalleObservacion = colIniTable + 45;
            int colResp3Comentario1 = colIniTable + 46;
            int colResp3ConformidadSuministrador = colIniTable + 47;
            int colResp3Comentario2 = colIniTable + 48;

            int colResp4ConformidadResponsable = colIniTable + 49;
            int colResp4Observacion = colIniTable + 50;
            int colResp4DetalleObservacion = colIniTable + 51;
            int colResp4Comentario1 = colIniTable + 52;
            int colResp4ConformidadSuministrador = colIniTable + 53;
            int colResp4Comentario2 = colIniTable + 54;

            int colResp5ConformidadResponsable = colIniTable + 55;
            int colResp5Observacion = colIniTable + 56;
            int colResp5DetalleObservacion = colIniTable + 57;
            int colResp5Comentario1 = colIniTable + 58;
            int colResp5ConformidadSuministrador = colIniTable + 59;
            int colResp5Comentario2 = colIniTable + 60;

            int colDesicionControversia = colIniTable + 61;
            int colComentarioFinal = colIniTable + 62;

            switch (numResponsables)
            {
                case 1:
                    colDesicionControversia = colIniTable + 37;
                    colComentarioFinal = colIniTable + 38;
                    break;
                case 2:
                    colDesicionControversia = colIniTable + 43;
                    colComentarioFinal = colIniTable + 44;
                    break;
                case 3:
                    colDesicionControversia = colIniTable + 49;
                    colComentarioFinal = colIniTable + 50;
                    break;
                case 4:
                    colDesicionControversia = colIniTable + 55;
                    colComentarioFinal = colIniTable + 56;
                    break;
                case 5:
                    colDesicionControversia = colIniTable + 61;
                    colComentarioFinal = colIniTable + 62;
                    break;
            }


            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento);

            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colCorrelativo].Value = "Correlativo por Punto de Entrega";
            ws.Cells[rowIniTabla, colTipoCliente].Value = "Tipo de Cliente";
            ws.Cells[rowIniTabla, colNombreCliente].Value = "Cliente";
            ws.Cells[rowIniTabla, colPuntoEntregaBarraNombre].Value = "Punto de Entrega/Barra";
            ws.Cells[rowIniTabla, colNumSuministroClienteLibre].Value = "N° de suministro cliente libre";
            ws.Cells[rowIniTabla, colNivelTensionNombre].Value = "Nivel de Tensión";
            ws.Cells[rowIniTabla, colAplicacionLiteral].Value = "Aplicación literal e) de numeral 5.2.4 Base Metodológica (meses de suministro en el semestre)";
            ws.Cells[rowIniTabla, colEnergiaPeriodo].Value = "Energía Semestral (kWh)";
            ws.Cells[rowIniTabla, colIncrementoTolerancia].Value = "Incremento de tolerancias Sector Distribución Típico 2 (Mercado regulado)";
            ws.Cells[rowIniTabla, colTipoNombre].Value = "Tipo";
            ws.Cells[rowIniTabla, colCausaNombre].Value = "Causa";
            ws.Cells[rowIniTabla, colNi].Value = "Ni";
            ws.Cells[rowIniTabla, colKi].Value = "Ki";
            ws.Cells[rowIniTabla, colTiempoEjecutadoIni].Value = "Tiempo Ejecutado";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colTiempoProgramadoIni].Value = "Tiempo Programado";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colResponsable1Nombre].Value = "Responsable 1";
            ws.Cells[rowIniTabla + 1, colResponsable1Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable1Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable2Nombre].Value = "Responsable 2";
            ws.Cells[rowIniTabla + 1, colResponsable2Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable2Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable3Nombre].Value = "Responsable 3";
            ws.Cells[rowIniTabla + 1, colResponsable3Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable3Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable4Nombre].Value = "Responsable 4";
            ws.Cells[rowIniTabla + 1, colResponsable4Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable4Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable5Nombre].Value = "Responsable 5";
            ws.Cells[rowIniTabla + 1, colResponsable5Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable5Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colCausaResumida].Value = "Causa resumida de interrupción";
            ws.Cells[rowIniTabla, colEiE].Value = "Ei / E";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento (US$)";

            if (numResponsables >= 1)
            {
                ws.Cells[rowIniTabla, colResp1ConformidadResponsable].Value = "Responsable 1";
                ws.Cells[rowIniTabla + 1, colResp1ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp1Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp1DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp1Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp1ConformidadSuministrador].Value = "Conformidad Responsable 1";
                ws.Cells[rowIniTabla + 1, colResp1ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp1Comentario2].Value = "Comentarios";
            }

            if (numResponsables >= 2)
            {
                ws.Cells[rowIniTabla, colResp2ConformidadResponsable].Value = "Responsable 2";
                ws.Cells[rowIniTabla + 1, colResp2ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp2Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp2DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp2Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp2ConformidadSuministrador].Value = "Conformidad Responsable 2";
                ws.Cells[rowIniTabla + 1, colResp2ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp2Comentario2].Value = "Comentarios";
            }

            if (numResponsables >= 3)
            {
                ws.Cells[rowIniTabla, colResp3ConformidadResponsable].Value = "Responsable 3";
                ws.Cells[rowIniTabla + 1, colResp3ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp3Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp3DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp3Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp3ConformidadSuministrador].Value = "Conformidad Responsable 3";
                ws.Cells[rowIniTabla + 1, colResp3ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp3Comentario2].Value = "Comentarios";
            }

            if (numResponsables >= 4)
            {
                ws.Cells[rowIniTabla, colResp4ConformidadResponsable].Value = "Responsable 4";
                ws.Cells[rowIniTabla + 1, colResp4ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp4Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp4DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp4Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp4ConformidadSuministrador].Value = "Conformidad Responsable 4";
                ws.Cells[rowIniTabla + 1, colResp4ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp4Comentario2].Value = "Comentarios";
            }

            if (numResponsables == 5)
            {
                ws.Cells[rowIniTabla, colResp5ConformidadResponsable].Value = "Responsable 5";
                ws.Cells[rowIniTabla + 1, colResp5ConformidadResponsable].Value = "Conformidad Responsable";
                ws.Cells[rowIniTabla + 1, colResp5Observacion].Value = "Observación";
                ws.Cells[rowIniTabla + 1, colResp5DetalleObservacion].Value = "Detalle del campo Observado";
                ws.Cells[rowIniTabla + 1, colResp5Comentario1].Value = "Comentarios";
                ws.Cells[rowIniTabla, colResp5ConformidadSuministrador].Value = "Conformidad Responsable 5";
                ws.Cells[rowIniTabla + 1, colResp5ConformidadSuministrador].Value = "Conformidad Suministrador";
                ws.Cells[rowIniTabla + 1, colResp5Comentario2].Value = "Comentarios";
            }

            ws.Cells[rowIniTabla, colDesicionControversia].Value = "Decisión de Controveria";
            ws.Cells[rowIniTabla, colComentarioFinal].Value = "Comentarios";

            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCorrelativo, rowIniTabla + 1, colCorrelativo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoCliente, rowIniTabla + 1, colTipoCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNombreCliente, rowIniTabla + 1, colNombreCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPuntoEntregaBarraNombre, rowIniTabla + 1, colPuntoEntregaBarraNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNumSuministroClienteLibre, rowIniTabla + 1, colNumSuministroClienteLibre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNivelTensionNombre, rowIniTabla + 1, colNivelTensionNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionLiteral, rowIniTabla + 1, colAplicacionLiteral);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEnergiaPeriodo, rowIniTabla + 1, colEnergiaPeriodo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colIncrementoTolerancia, rowIniTabla + 1, colIncrementoTolerancia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoNombre, rowIniTabla + 1, colTipoNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaNombre, rowIniTabla + 1, colCausaNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNi, rowIniTabla + 1, colNi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colKi, rowIniTabla + 1, colKi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoIni, rowIniTabla, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoProgramadoIni, rowIniTabla, colTiempoProgramadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable1Nombre, rowIniTabla, colResponsable1Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable2Nombre, rowIniTabla, colResponsable2Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable3Nombre, rowIniTabla, colResponsable3Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable4Nombre, rowIniTabla, colResponsable4Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable5Nombre, rowIniTabla, colResponsable5Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaResumida, rowIniTabla + 1, colCausaResumida);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEiE, rowIniTabla + 1, colEiE);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResarcimiento, rowIniTabla + 1, colResarcimiento);

            if (numResponsables >= 1)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp1ConformidadResponsable, rowIniTabla, colResp1Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp1ConformidadSuministrador, rowIniTabla, colResp1Comentario2);
            }

            if (numResponsables >= 2)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp2ConformidadResponsable, rowIniTabla, colResp2Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp2ConformidadSuministrador, rowIniTabla, colResp2Comentario2);
            }

            if (numResponsables >= 3)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp3ConformidadResponsable, rowIniTabla, colResp3Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp3ConformidadSuministrador, rowIniTabla, colResp3Comentario2);
            }

            if (numResponsables >= 4)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp4ConformidadResponsable, rowIniTabla, colResp4Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp4ConformidadSuministrador, rowIniTabla, colResp4Comentario2);
            }

            if (numResponsables == 5)
            {
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp5ConformidadResponsable, rowIniTabla, colResp5Comentario1);
                servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResp5ConformidadSuministrador, rowIniTabla, colResp5Comentario2);
            }


            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colDesicionControversia, rowIniTabla + 1, colDesicionControversia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colComentarioFinal, rowIniTabla + 1, colComentarioFinal);


            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "#1659AA");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal);
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal, "#FFFFFF");
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colComentarioFinal);

            if (numResponsables >= 1)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp1ConformidadResponsable, rowIniTabla + 1, colResp1Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp1ConformidadResponsable, rowIniTabla + 1, colResp1Comentario1, "#000000");
            }

            if (numResponsables >= 2)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp2ConformidadResponsable, rowIniTabla + 1, colResp2Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp2ConformidadResponsable, rowIniTabla + 1, colResp2Comentario1, "#000000");
            }

            if (numResponsables >= 3)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp3ConformidadResponsable, rowIniTabla + 1, colResp3Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp3ConformidadResponsable, rowIniTabla + 1, colResp3Comentario1, "#000000");
            }

            if (numResponsables >= 4)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp4ConformidadResponsable, rowIniTabla + 1, colResp4Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp4ConformidadResponsable, rowIniTabla + 1, colResp4Comentario1, "#000000");
            }

            if (numResponsables >= 5)
            {
                servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colResp5ConformidadResponsable, rowIniTabla + 1, colResp5Comentario1, "#FDE9D9");
                servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colResp5ConformidadResponsable, rowIniTabla + 1, colResp5Comentario1, "#000000");
            }
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in lista)
            {
                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colCorrelativo].Value = item.Correlativo != null ? item.Correlativo.Value : item.Correlativo;
                ws.Cells[rowData, colTipoCliente].Value = item.TipoCliente != null ? item.TipoCliente.Trim() : null;
                ws.Cells[rowData, colNombreCliente].Value = item.NombreCliente != null ? item.NombreCliente.Trim() : null;
                ws.Cells[rowData, colPuntoEntregaBarraNombre].Value = item.PuntoEntregaBarraNombre != null ? item.PuntoEntregaBarraNombre.Trim() : null;
                ws.Cells[rowData, colNumSuministroClienteLibre].Value = item.NumSuministroClienteLibre != null ? item.NumSuministroClienteLibre : item.NumSuministroClienteLibre;
                ws.Cells[rowData, colNivelTensionNombre].Value = item.NivelTensionNombre != null ? item.NivelTensionNombre.Trim() : null;
                ws.Cells[rowData, colAplicacionLiteral].Value = item.AplicacionLiteral != null ? item.AplicacionLiteral.Value : item.AplicacionLiteral;
                ws.Cells[rowData, colEnergiaPeriodo].Value = item.EnergiaPeriodo != null ? item.EnergiaPeriodo.Value : item.EnergiaPeriodo;
                ws.Cells[rowData, colEnergiaPeriodo].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colIncrementoTolerancia].Value = item.IncrementoTolerancia != null ? item.IncrementoTolerancia.Trim() : null;
                ws.Cells[rowData, colTipoNombre].Value = item.TipoNombre != null ? item.TipoNombre.Trim() : null;
                ws.Cells[rowData, colCausaNombre].Value = item.CausaNombre != null ? item.CausaNombre.Trim() : null;
                ws.Cells[rowData, colNi].Value = item.Ni != null ? item.Ni.Value : item.Ni;
                ws.Cells[rowData, colNi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colKi].Value = item.Ki != null ? item.Ki.Value : item.Ki;
                ws.Cells[rowData, colKi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colTiempoEjecutadoIni].Value = item.TiempoEjecutadoIni != null ? item.TiempoEjecutadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoIni].Value = item.TiempoProgramadoIni != null ? item.TiempoProgramadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoFin].Value = item.TiempoProgramadoFin != null ? item.TiempoProgramadoFin.Trim() : null;
                ws.Cells[rowData, colResponsable1Nombre].Value = item.Responsable1Nombre != null ? item.Responsable1Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable1Porcentaje].Value = item.Responsable1Porcentaje != null ? item.Responsable1Porcentaje.Value / 100 : item.Responsable1Porcentaje;
                ws.Cells[rowData, colResponsable1Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable2Nombre].Value = item.Responsable2Nombre != null ? item.Responsable2Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable2Porcentaje].Value = item.Responsable2Porcentaje != null ? item.Responsable2Porcentaje.Value / 100 : item.Responsable2Porcentaje;
                ws.Cells[rowData, colResponsable2Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable3Nombre].Value = item.Responsable3Nombre != null ? item.Responsable3Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable3Porcentaje].Value = item.Responsable3Porcentaje != null ? item.Responsable3Porcentaje.Value / 100 : item.Responsable3Porcentaje;
                ws.Cells[rowData, colResponsable3Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable4Nombre].Value = item.Responsable4Nombre != null ? item.Responsable4Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable4Porcentaje].Value = item.Responsable4Porcentaje != null ? item.Responsable4Porcentaje.Value / 100 : item.Responsable4Porcentaje;
                ws.Cells[rowData, colResponsable4Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable5Nombre].Value = item.Responsable5Nombre != null ? item.Responsable5Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable5Porcentaje].Value = item.Responsable5Porcentaje != null ? item.Responsable5Porcentaje.Value / 100 : item.Responsable5Porcentaje;
                ws.Cells[rowData, colResponsable5Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colCausaResumida].Value = item.CausaResumida != null ? item.CausaResumida.Trim() : null;
                ws.Cells[rowData, colEiE].Value = item.EiE != null ? item.EiE.Value : item.EiE;
                ws.Cells[rowData, colEiE].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResarcimiento].Value = item.Resarcimiento != null ? item.Resarcimiento.Value : item.Resarcimiento;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = FormatoNumDecimales(2);

                if (numResponsables >= 1)
                {
                    ws.Cells[rowData, colResp1ConformidadResponsable].Value = item.Resp1ConformidadResponsable != null ? item.Resp1ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp1Observacion].Value = item.Resp1Observacion != null ? item.Resp1Observacion.Trim() : null;
                    ws.Cells[rowData, colResp1DetalleObservacion].Value = item.Resp1DetalleObservacion != null ? item.Resp1DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp1Comentario1].Value = item.Resp1Comentario1 != null ? item.Resp1Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp1ConformidadSuministrador].Value = item.Resp1ConformidadSuministrador != null ? item.Resp1ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp1Comentario2].Value = item.Resp1Comentario2 != null ? item.Resp1Comentario2.Trim() : null;
                }

                if (numResponsables >= 2)
                {
                    ws.Cells[rowData, colResp2ConformidadResponsable].Value = item.Resp2ConformidadResponsable != null ? item.Resp2ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp2Observacion].Value = item.Resp2Observacion != null ? item.Resp2Observacion.Trim() : null;
                    ws.Cells[rowData, colResp2DetalleObservacion].Value = item.Resp2DetalleObservacion != null ? item.Resp2DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp2Comentario1].Value = item.Resp2Comentario1 != null ? item.Resp2Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp2ConformidadSuministrador].Value = item.Resp2ConformidadSuministrador != null ? item.Resp2ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp2Comentario2].Value = item.Resp2Comentario2 != null ? item.Resp2Comentario2.Trim() : null;
                }

                if (numResponsables >= 3)
                {
                    ws.Cells[rowData, colResp3ConformidadResponsable].Value = item.Resp3ConformidadResponsable != null ? item.Resp3ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp3Observacion].Value = item.Resp3Observacion != null ? item.Resp3Observacion.Trim() : null;
                    ws.Cells[rowData, colResp3DetalleObservacion].Value = item.Resp3DetalleObservacion != null ? item.Resp3DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp3Comentario1].Value = item.Resp3Comentario1 != null ? item.Resp3Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp3ConformidadSuministrador].Value = item.Resp3ConformidadSuministrador != null ? item.Resp3ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp3Comentario2].Value = item.Resp3Comentario2 != null ? item.Resp3Comentario2.Trim() : null;
                }

                if (numResponsables >= 4)
                {
                    ws.Cells[rowData, colResp4ConformidadResponsable].Value = item.Resp4ConformidadResponsable != null ? item.Resp4ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp4Observacion].Value = item.Resp4Observacion != null ? item.Resp4Observacion.Trim() : null;
                    ws.Cells[rowData, colResp4DetalleObservacion].Value = item.Resp4DetalleObservacion != null ? item.Resp4DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp4Comentario1].Value = item.Resp4Comentario1 != null ? item.Resp4Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp4ConformidadSuministrador].Value = item.Resp4ConformidadSuministrador != null ? item.Resp4ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp4Comentario2].Value = item.Resp4Comentario2 != null ? item.Resp4Comentario2.Trim() : null;
                }

                if (numResponsables == 5)
                {
                    ws.Cells[rowData, colResp5ConformidadResponsable].Value = item.Resp5ConformidadResponsable != null ? item.Resp5ConformidadResponsable.Trim() : null;
                    ws.Cells[rowData, colResp5Observacion].Value = item.Resp5Observacion != null ? item.Resp5Observacion.Trim() : null;
                    ws.Cells[rowData, colResp5DetalleObservacion].Value = item.Resp5DetalleObservacion != null ? item.Resp5DetalleObservacion.Trim() : null;
                    ws.Cells[rowData, colResp5Comentario1].Value = item.Resp5Comentario1 != null ? item.Resp5Comentario1.Trim() : null;
                    ws.Cells[rowData, colResp5ConformidadSuministrador].Value = item.Resp5ConformidadSuministrador != null ? item.Resp5ConformidadSuministrador.Trim() : null;
                    ws.Cells[rowData, colResp5Comentario2].Value = item.Resp5Comentario2 != null ? item.Resp5Comentario2.Trim() : null;
                }

                ws.Cells[rowData, colDesicionControversia].Value = item.DesicionControversia != null ? item.DesicionControversia.Trim() : null;
                ws.Cells[rowData, colComentarioFinal].Value = item.ComentarioFinal != null ? item.ComentarioFinal.Trim() : null;


                rowData++;
            }
            if (!lista.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colSuministrador, rowData - 1, colComentarioFinal, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministrador, rowData - 1, colComentarioFinal);
            #endregion

            //filter                       

            if (lista.Any())
            {

                ws.Cells[rowIniTabla, colSuministrador, rowData, colComentarioFinal].AutoFitColumns(12.5);
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 14;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 18;
                ws.Column(colTiempoEjecutadoFin).Width = 18;
                ws.Column(colTiempoProgramadoIni).Width = 18;
                ws.Column(colTiempoProgramadoFin).Width = 18;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadResponsable).Width = 12;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadSuministrador).Width = 13;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadResponsable).Width = 12;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadSuministrador).Width = 13;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadResponsable).Width = 12;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadSuministrador).Width = 13;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadResponsable).Width = 12;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadSuministrador).Width = 13;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadResponsable).Width = 12;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadSuministrador).Width = 13;
            }
            else
            {
                ws.Column(colSuministrador).Width = 20;
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNombreCliente).Width = 12;
                ws.Column(colPuntoEntregaBarraNombre).Width = 22;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 12;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colTipoNombre).Width = 10;
                ws.Column(colCausaNombre).Width = 10;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 13;
                ws.Column(colTiempoEjecutadoFin).Width = 12;
                ws.Column(colTiempoProgramadoIni).Width = 13;
                ws.Column(colTiempoProgramadoFin).Width = 12;
                ws.Column(colResponsable1Nombre).Width = 8;
                ws.Column(colResponsable1Porcentaje).Width = 5;
                ws.Column(colResponsable2Nombre).Width = 8;
                ws.Column(colResponsable2Porcentaje).Width = 5;
                ws.Column(colResponsable3Nombre).Width = 8;
                ws.Column(colResponsable3Porcentaje).Width = 5;
                ws.Column(colResponsable4Nombre).Width = 8;
                ws.Column(colResponsable4Porcentaje).Width = 5;
                ws.Column(colResponsable5Nombre).Width = 8;
                ws.Column(colResponsable5Porcentaje).Width = 5;
                ws.Column(colCausaResumida).Width = 27;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;

                if (numResponsables >= 1) ws.Column(colResp1ConformidadResponsable).Width = 12;
                if (numResponsables >= 1) ws.Column(colResp1ConformidadSuministrador).Width = 13;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadResponsable).Width = 12;
                if (numResponsables >= 2) ws.Column(colResp2ConformidadSuministrador).Width = 13;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadResponsable).Width = 12;
                if (numResponsables >= 3) ws.Column(colResp3ConformidadSuministrador).Width = 13;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadResponsable).Width = 12;
                if (numResponsables >= 4) ws.Column(colResp4ConformidadSuministrador).Width = 13;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadResponsable).Width = 12;
                if (numResponsables >= 5) ws.Column(colResp5ConformidadSuministrador).Width = 13;

            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }

        /// <summary>
        /// Devuelve el numero de agentes responsables existentes en las interrupciones
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public int ObtenerNumeroResponsables(List<InterrupcionSuministroIntranet> lista) //Mejorar para que muestren los agentes que son visibles
        {
            int salida = 0;

            //Mejorar para que muestren los agentes que son visibles, por eso se comento lo de abajo
            salida = 5;

            //int temp = 0;
            //foreach (var interrupcion in lista)
            //{
            //    int numAgentesResponsables = interrupcion.NumeroAgentesResponsables;                

            //    if (numAgentesResponsables > temp)
            //    {
            //        temp = numAgentesResponsables;
            //    }
            //}

            //if (temp == 0)            
            //    salida = 5;            
            //else
            //    salida = temp;

            return salida;
        }
        #endregion

        #region Reporte Interrupciones por Suministrador

        /// <summary>
        /// Genera los archivos excel por cada agente
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="pathLogo"></param>
        /// <param name="ruta"></param>
        private void GenerarArchivosExcelPorSuministrador(RePeriodoDTO periodo, string pathLogo, string ruta)
        {
            List<InterrupcionSuministroPE> listaGeneralPE = new List<InterrupcionSuministroPE>();
            List<InterrupcionSuministroRC> listaGeneralRC = new List<InterrupcionSuministroRC>();

            listaGeneralPE = ObtenerListaInterrupcionesPEPorPeriodo(periodo, false, null);
            listaGeneralRC = ObtenerListaInterrupcionesRCPorPeriodo(periodo, false, null);

            //Crear carpeta para generar los excel
            var patTrabajo = "ReportePorSuministrador\\";
            string nombreCarpeta = "SALIDA";
            FileServer.DeleteFolderAlter(patTrabajo, ruta);
            FileServer.CreateFolder(patTrabajo, nombreCarpeta, ruta);
            //Generar los .dat
            var rutaFinalArchivoDat = ruta + patTrabajo + nombreCarpeta + "\\";

            //Obtenemos el total de agentes responsables
            List<int> lstSuministradoresPETotales = ObtenerTotalSuministradoresPE(listaGeneralPE);
            List<int> lstSuministradoresRCTotales = ObtenerTotalSuministradoresRC(listaGeneralRC);

            //Obtengo todos los suministradores
            List<int> lstSuministradoresTotales = new List<int>();
            lstSuministradoresTotales.AddRange(lstSuministradoresPETotales);
            lstSuministradoresTotales.AddRange(lstSuministradoresRCTotales);
            lstSuministradoresTotales = lstSuministradoresTotales.Distinct().ToList();

            //Obtenemos los nombres de los suministradores
            Dictionary<int, string> lstNombreSuministradores = ObtenerRelacionSuministradores(lstSuministradoresTotales, listaGeneralPE, listaGeneralRC);

            if (!lstSuministradoresTotales.Any())
            {
                throw new Exception("No se encontraron suministradores para el periodo seleccionado.");
            }

            foreach (int suministradorId in lstSuministradoresTotales)
            {
                string nombSuministrador = lstNombreSuministradores[suministradorId];

                List<InterrupcionSuministroPE> listaPEPorSuministrador = listaGeneralPE.Where(x => x.SuministradorId == suministradorId).ToList();
                List<InterrupcionSuministroRC> listaRCPorSuministrador = listaGeneralRC.Where(x => x.SuministradorId == suministradorId).ToList();

                GenerarExcelParaSuministrador(periodo, listaPEPorSuministrador, listaRCPorSuministrador, nombSuministrador, rutaFinalArchivoDat, pathLogo);
            }


        }

        /// <summary>
        /// Devuelve la lista de suministradores
        /// </summary>
        /// <param name="listaGeneralPE"></param>
        /// <param name="listaGeneralRC"></param>
        /// <returns></returns>
        public Dictionary<int, string> ObtenerRelacionSuministradores(List<int> lstSuministradoresTotales, List<InterrupcionSuministroPE> listaGeneralPE, List<InterrupcionSuministroRC> listaGeneralRC)
        {
            Dictionary<int, string> lstSalida = new Dictionary<int, string>();

            int num = 1;
            foreach (var suministradorId in lstSuministradoresTotales)
            {
                InterrupcionSuministroPE ope = listaGeneralPE.Find(x => x.SuministradorId == suministradorId);
                InterrupcionSuministroRC orc = listaGeneralRC.Find(x => x.SuministradorId == suministradorId);

                string nombSum = "";

                //si el suministador esta en pestaña PE
                if (ope != null)
                {
                    if (ope.Suministrador != null)
                    {
                        if (ope.Suministrador.Trim() != "")

                            nombSum = ope.Suministrador.Trim();
                        else
                        {
                            nombSum = "Sum_" + num;
                            num++;
                        }
                    }
                    else
                    {
                        nombSum = "Sum_X" + num;
                        num++;
                    }
                }
                else
                {
                    //si el suministador esta en pestaña RC
                    if (orc != null)
                    {
                        if (orc.Suministrador != null)
                        {
                            if (orc.Suministrador.Trim() != "")

                                nombSum = orc.Suministrador.Trim();
                            else
                            {
                                nombSum = "Sum_" + num;
                                num++;
                            }
                        }
                        else
                        {
                            nombSum = "Sum_X" + num;
                            num++;
                        }
                    }
                    else
                    {
                        nombSum = "Suministrador_" + num;
                        num++;
                    }
                }

                lstSalida.Add(suministradorId, nombSum);
            }

            return lstSalida;
        }


        /// <summary>
        /// Devuelve los codigos de todos los Suministradores PE
        /// </summary>
        /// <param name="listaGeneral"></param>
        /// <returns></returns>
        private List<int> ObtenerTotalSuministradoresPE(List<InterrupcionSuministroPE> listaGeneral)
        {
            List<int> lstSalida = new List<int>();

            lstSalida = listaGeneral.Select(x => x.SuministradorId).Distinct().ToList();

            lstSalida = lstSalida.Distinct().ToList();

            return lstSalida;
        }

        /// <summary>
        /// Devuelve los codigos de todos los Suministradores RC
        /// </summary>
        /// <param name="listaGeneral"></param>
        /// <returns></returns>
        private List<int> ObtenerTotalSuministradoresRC(List<InterrupcionSuministroRC> listaGeneral)
        {
            List<int> lstSalida = new List<int>();

            lstSalida = listaGeneral.Select(x => x.SuministradorId).Distinct().ToList();

            lstSalida = lstSalida.Distinct().ToList();

            return lstSalida;
        }

        /// <summary>
        /// Genera un archivo excel del reporte
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="listaPEPorSuministrador"></param>
        /// <param name="listaRCPorSuministrador"></param>
        /// <param name="nombreSuministrador"></param>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        private void GenerarExcelParaSuministrador(RePeriodoDTO periodo, List<InterrupcionSuministroPE> listaPEPorSuministrador, List<InterrupcionSuministroRC> listaRCPorSuministrador, string nombreSuministrador, string ruta, string pathLogo)
        {
            ////Descargo archivo segun requieran
            string rutaFile = ruta + nombreSuministrador + ".xlsx";

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarArchivoExcelInterrupcionesPorSuministrador_PE(xlPackage, pathLogo, listaPEPorSuministrador, nombreSuministrador, periodo);
                GenerarArchivoExcelInterrupcionesPorSuministrador_RC(xlPackage, pathLogo, listaRCPorSuministrador, nombreSuministrador, periodo);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera archivo excel para interrupciones por suministrador pestaña PE
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="listaPEPorSuministrador"></param>
        /// <param name="nombreSuministrador"></param>
        /// <param name="periodo"></param>
        private void GenerarArchivoExcelInterrupcionesPorSuministrador_PE(ExcelPackage xlPackage, string pathLogo, List<InterrupcionSuministroPE> listaPEPorSuministrador, string nombreSuministrador, RePeriodoDTO periodo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroPE> listaRegistrosPE = new List<InterrupcionSuministroPE>();

            listaRegistrosPE = listaPEPorSuministrador;

            string nameWS = "PE";
            string titulo = "Interrupciones por Suministrador (" + nombreSuministrador + ")";
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera 



            int colIniTitulo = 1;
            int rowIniTitulo = 3;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;



            int colSuministrador = colIniTable;
            int colCorrelativo = colIniTable + 1;
            int colTipoCliente = colIniTable + 2;
            int colNombreCliente = colIniTable + 3;
            int colPuntoEntregaBarraNombre = colIniTable + 4;
            int colNumSuministroClienteLibre = colIniTable + 5;
            int colNivelTensionNombre = colIniTable + 6;
            int colAplicacionLiteral = colIniTable + 7;
            int colEnergiaPeriodo = colIniTable + 8;
            int colIncrementoTolerancia = colIniTable + 9;
            int colTipoNombre = colIniTable + 10;
            int colCausaNombre = colIniTable + 11;
            int colNi = colIniTable + 12;
            int colKi = colIniTable + 13;
            int colTiempoEjecutadoIni = colIniTable + 14;
            int colTiempoEjecutadoFin = colIniTable + 15;
            int colTiempoProgramadoIni = colIniTable + 16;
            int colTiempoProgramadoFin = colIniTable + 17;
            int colResponsable1Nombre = colIniTable + 18;
            int colResponsable1Porcentaje = colIniTable + 19;
            int colResponsable2Nombre = colIniTable + 20;
            int colResponsable2Porcentaje = colIniTable + 21;
            int colResponsable3Nombre = colIniTable + 22;
            int colResponsable3Porcentaje = colIniTable + 23;
            int colResponsable4Nombre = colIniTable + 24;
            int colResponsable4Porcentaje = colIniTable + 25;
            int colResponsable5Nombre = colIniTable + 26;
            int colResponsable5Porcentaje = colIniTable + 27;
            int colCausaResumida = colIniTable + 28;
            int colEiE = colIniTable + 29;
            int colResarcimiento = colIniTable + 30;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento);

            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colCorrelativo].Value = "Correlativo por Punto de Entrega";
            ws.Cells[rowIniTabla, colTipoCliente].Value = "Tipo de Cliente";
            ws.Cells[rowIniTabla, colNombreCliente].Value = "Cliente";
            ws.Cells[rowIniTabla, colPuntoEntregaBarraNombre].Value = "Punto de Entrega/Barra";
            ws.Cells[rowIniTabla, colNumSuministroClienteLibre].Value = "N° de suministro cliente libre";
            ws.Cells[rowIniTabla, colNivelTensionNombre].Value = "Nivel de Tensión";
            ws.Cells[rowIniTabla, colAplicacionLiteral].Value = "Aplicación literal e) de numeral 5.2.4 Base Metodológica (meses de suministro en el semestre)";
            ws.Cells[rowIniTabla, colEnergiaPeriodo].Value = "Energía Semestral (kWh)";
            ws.Cells[rowIniTabla, colIncrementoTolerancia].Value = "Incremento de tolerancias Sector Distribución Típico 2 (Mercado regulado)";
            ws.Cells[rowIniTabla, colTipoNombre].Value = "Tipo";
            ws.Cells[rowIniTabla, colCausaNombre].Value = "Causa";
            ws.Cells[rowIniTabla, colNi].Value = "Ni";
            ws.Cells[rowIniTabla, colKi].Value = "Ki";
            ws.Cells[rowIniTabla, colTiempoEjecutadoIni].Value = "Tiempo Ejecutado";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colTiempoProgramadoIni].Value = "Tiempo Programado";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoIni].Value = "Fecha Hora Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoProgramadoFin].Value = "Fecha Hora Fin";
            ws.Cells[rowIniTabla, colResponsable1Nombre].Value = "Responsable 1";
            ws.Cells[rowIniTabla + 1, colResponsable1Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable1Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable2Nombre].Value = "Responsable 2";
            ws.Cells[rowIniTabla + 1, colResponsable2Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable2Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable3Nombre].Value = "Responsable 3";
            ws.Cells[rowIniTabla + 1, colResponsable3Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable3Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable4Nombre].Value = "Responsable 4";
            ws.Cells[rowIniTabla + 1, colResponsable4Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable4Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colResponsable5Nombre].Value = "Responsable 5";
            ws.Cells[rowIniTabla + 1, colResponsable5Nombre].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colResponsable5Porcentaje].Value = "%";
            ws.Cells[rowIniTabla, colCausaResumida].Value = "Causa resumida de interrupción";
            ws.Cells[rowIniTabla, colEiE].Value = "Ei / E";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento (US$)";



            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCorrelativo, rowIniTabla + 1, colCorrelativo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoCliente, rowIniTabla + 1, colTipoCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNombreCliente, rowIniTabla + 1, colNombreCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPuntoEntregaBarraNombre, rowIniTabla + 1, colPuntoEntregaBarraNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNumSuministroClienteLibre, rowIniTabla + 1, colNumSuministroClienteLibre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNivelTensionNombre, rowIniTabla + 1, colNivelTensionNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAplicacionLiteral, rowIniTabla + 1, colAplicacionLiteral);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEnergiaPeriodo, rowIniTabla + 1, colEnergiaPeriodo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colIncrementoTolerancia, rowIniTabla + 1, colIncrementoTolerancia);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoNombre, rowIniTabla + 1, colTipoNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaNombre, rowIniTabla + 1, colCausaNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNi, rowIniTabla + 1, colNi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colKi, rowIniTabla + 1, colKi);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoIni, rowIniTabla, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoProgramadoIni, rowIniTabla, colTiempoProgramadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable1Nombre, rowIniTabla, colResponsable1Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable2Nombre, rowIniTabla, colResponsable2Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable3Nombre, rowIniTabla, colResponsable3Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable4Nombre, rowIniTabla, colResponsable4Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResponsable5Nombre, rowIniTabla, colResponsable5Porcentaje);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCausaResumida, rowIniTabla + 1, colCausaResumida);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEiE, rowIniTabla + 1, colEiE);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResarcimiento, rowIniTabla + 1, colResarcimiento);

            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "#8EA9DB");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in listaRegistrosPE)
            {
                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colCorrelativo].Value = item.Correlativo != null ? item.Correlativo.Value : item.Correlativo;
                ws.Cells[rowData, colTipoCliente].Value = item.TipoCliente != null ? item.TipoCliente.Trim() : null;
                ws.Cells[rowData, colNombreCliente].Value = item.NombreCliente != null ? item.NombreCliente.Trim() : null;
                ws.Cells[rowData, colPuntoEntregaBarraNombre].Value = item.PuntoEntregaBarraNombre != null ? item.PuntoEntregaBarraNombre.Trim() : null;
                ws.Cells[rowData, colNumSuministroClienteLibre].Value = item.NumSuministroClienteLibre != null ? item.NumSuministroClienteLibre : item.NumSuministroClienteLibre;
                ws.Cells[rowData, colNivelTensionNombre].Value = item.NivelTensionNombre != null ? item.NivelTensionNombre.Trim() : null;
                ws.Cells[rowData, colAplicacionLiteral].Value = item.AplicacionLiteral != null ? item.AplicacionLiteral.Value : item.AplicacionLiteral;
                ws.Cells[rowData, colEnergiaPeriodo].Value = item.EnergiaPeriodo != null ? item.EnergiaPeriodo.Value : item.EnergiaPeriodo;
                ws.Cells[rowData, colEnergiaPeriodo].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colIncrementoTolerancia].Value = item.IncrementoTolerancia != null ? item.IncrementoTolerancia.Trim() : null;
                ws.Cells[rowData, colTipoNombre].Value = item.TipoNombre != null ? item.TipoNombre.Trim() : null;
                ws.Cells[rowData, colCausaNombre].Value = item.CausaNombre != null ? item.CausaNombre.Trim() : null;
                ws.Cells[rowData, colNi].Value = item.Ni != null ? item.Ni.Value : item.Ni;
                ws.Cells[rowData, colNi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colKi].Value = item.Ki != null ? item.Ki.Value : item.Ki;
                ws.Cells[rowData, colKi].Style.Numberformat.Format = FormatoNumDecimales(2);
                ws.Cells[rowData, colTiempoEjecutadoIni].Value = item.TiempoEjecutadoIni != null ? item.TiempoEjecutadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoIni].Value = item.TiempoProgramadoIni != null ? item.TiempoProgramadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoProgramadoFin].Value = item.TiempoProgramadoFin != null ? item.TiempoProgramadoFin.Trim() : null;
                ws.Cells[rowData, colResponsable1Nombre].Value = item.Responsable1Nombre != null ? item.Responsable1Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable1Porcentaje].Value = item.Responsable1Porcentaje != null ? item.Responsable1Porcentaje.Value / 100 : item.Responsable1Porcentaje;
                ws.Cells[rowData, colResponsable1Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable2Nombre].Value = item.Responsable2Nombre != null ? item.Responsable2Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable2Porcentaje].Value = item.Responsable2Porcentaje != null ? item.Responsable2Porcentaje.Value / 100 : item.Responsable2Porcentaje;
                ws.Cells[rowData, colResponsable2Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable3Nombre].Value = item.Responsable3Nombre != null ? item.Responsable3Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable3Porcentaje].Value = item.Responsable3Porcentaje != null ? item.Responsable3Porcentaje.Value / 100 : item.Responsable3Porcentaje;
                ws.Cells[rowData, colResponsable3Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable4Nombre].Value = item.Responsable4Nombre != null ? item.Responsable4Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable4Porcentaje].Value = item.Responsable4Porcentaje != null ? item.Responsable4Porcentaje.Value / 100 : item.Responsable4Porcentaje;
                ws.Cells[rowData, colResponsable4Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResponsable5Nombre].Value = item.Responsable5Nombre != null ? item.Responsable5Nombre.Trim() : null;
                ws.Cells[rowData, colResponsable5Porcentaje].Value = item.Responsable5Porcentaje != null ? item.Responsable5Porcentaje.Value / 100 : item.Responsable5Porcentaje;
                ws.Cells[rowData, colResponsable5Porcentaje].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colCausaResumida].Value = item.CausaResumida != null ? item.CausaResumida.Trim() : null;
                ws.Cells[rowData, colEiE].Value = item.EiE != null ? item.EiE.Value : item.EiE;
                ws.Cells[rowData, colEiE].Style.Numberformat.Format = FormatoNumDecimalesPorcentaje(2);
                ws.Cells[rowData, colResarcimiento].Value = item.Resarcimiento != null ? item.Resarcimiento.Value : item.Resarcimiento;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = FormatoNumDecimales(2);


                rowData++;
            }
            if (!listaRegistrosPE.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colSuministrador, rowData - 1, colResarcimiento, "Calibri", 9);

            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministrador, rowData - 1, colResarcimiento);
            #endregion


            if (listaRegistrosPE.Any())
            {
                ws.Cells[rowIniTabla, colSuministrador, rowData, colResarcimiento].AutoFitColumns();
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 14;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 18;
                ws.Column(colTiempoEjecutadoFin).Width = 18;
                ws.Column(colTiempoProgramadoIni).Width = 18;
                ws.Column(colTiempoProgramadoFin).Width = 18;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
            }
            else
            {
                ws.Column(colSuministrador).Width = 20;
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNombreCliente).Width = 12;
                ws.Column(colPuntoEntregaBarraNombre).Width = 22;
                ws.Column(colNumSuministroClienteLibre).Width = 11;
                ws.Column(colNivelTensionNombre).Width = 16;
                ws.Column(colAplicacionLiteral).Width = 18;
                ws.Column(colEnergiaPeriodo).Width = 12;
                ws.Column(colIncrementoTolerancia).Width = 18;
                ws.Column(colTipoNombre).Width = 10;
                ws.Column(colCausaNombre).Width = 10;
                ws.Column(colNi).Width = 8;
                ws.Column(colKi).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 13;
                ws.Column(colTiempoEjecutadoFin).Width = 12;
                ws.Column(colTiempoProgramadoIni).Width = 13;
                ws.Column(colTiempoProgramadoFin).Width = 12;
                ws.Column(colResponsable1Nombre).Width = 8;
                ws.Column(colResponsable1Porcentaje).Width = 5;
                ws.Column(colResponsable2Nombre).Width = 8;
                ws.Column(colResponsable2Porcentaje).Width = 5;
                ws.Column(colResponsable3Nombre).Width = 8;
                ws.Column(colResponsable3Porcentaje).Width = 5;
                ws.Column(colResponsable4Nombre).Width = 8;
                ws.Column(colResponsable4Porcentaje).Width = 5;
                ws.Column(colResponsable5Nombre).Width = 8;
                ws.Column(colResponsable5Porcentaje).Width = 5;
                ws.Column(colCausaResumida).Width = 27;
                ws.Column(colEiE).Width = 8;
                ws.Column(colResarcimiento).Width = 20;
            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }

        /// <summary>
        /// Genera archivo excel para interrupciones por suministrador pestaña RC
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="listaRCPorSuministrador"></param>
        /// <param name="nombreSuministrador"></param>
        /// <param name="periodo"></param>
        private void GenerarArchivoExcelInterrupcionesPorSuministrador_RC(ExcelPackage xlPackage, string pathLogo, List<InterrupcionSuministroRC> listaRCPorSuministrador, string nombreSuministrador, RePeriodoDTO periodo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            //Obtengo el listado
            List<InterrupcionSuministroRC> listaRegistrosRC = new List<InterrupcionSuministroRC>();

            listaRegistrosRC = listaRCPorSuministrador;

            string nameWS = "RC";
            string titulo = "Interrupciones por Suministrador (" + nombreSuministrador + ")";
            string strPeriodo = (periodo.Repertipo == "T" ? "Trimestral" : (periodo.Repertipo == "S" ? "Semestral" : "")) + " " + periodo.Reperanio + " ( " + periodo.Repernombre + ")";


            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;


            UtilExcel.AddImageLocal(ws, 0, 0, pathLogo, 120, 70);

            #region  Cabecera

            int colIniTitulo = 1;
            int rowIniTitulo = 3;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 3;

            ws.Row(rowIniTabla).Height = 30;
            ws.Row(rowIniTabla + 1).Height = 30;


            int colSuministrador = colIniTable;
            int colCorrelativo = colIniTable + 1;
            int colTipoCliente = colIniTable + 2;
            int colNombreCliente = colIniTable + 3;
            int colBarraNombre = colIniTable + 4;
            int colAlimentador = colIniTable + 5;
            int colEnst = colIniTable + 6;
            int ColEvento = colIniTable + 7;
            int colComentario = colIniTable + 8;
            int colTiempoEjecutadoIni = colIniTable + 9;
            int colTiempoEjecutadoFin = colIniTable + 10;
            int colPk = colIniTable + 11;
            int colCompensable = colIniTable + 12;
            int colEns = colIniTable + 13;
            int colResarcimiento = colIniTable + 14;


            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniTitulo + 1, colIniTitulo].Value = strPeriodo;
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colResarcimiento, "Calibri", 13);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento);
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo + 1, colIniTitulo, rowIniTitulo + 1, colResarcimiento, "Calibri", 11);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo + 1, colResarcimiento);

            ws.Cells[rowIniTabla, colSuministrador].Value = "Suministrador";
            ws.Cells[rowIniTabla, colCorrelativo].Value = "Correlativo por Rechazo de Carga";
            ws.Cells[rowIniTabla, colTipoCliente].Value = "Tipo de Cliente";
            ws.Cells[rowIniTabla, colNombreCliente].Value = "Cliente";
            ws.Cells[rowIniTabla, colBarraNombre].Value = "Barra";
            ws.Cells[rowIniTabla, colAlimentador].Value = "Alimentador/SED";
            ws.Cells[rowIniTabla, colEnst].Value = "ENST f,k (kWh) ";
            ws.Cells[rowIniTabla, ColEvento].Value = "Evento COES";
            ws.Cells[rowIniTabla, colComentario].Value = "Comentario";
            ws.Cells[rowIniTabla, colTiempoEjecutadoIni].Value = "Tiempo Ejecutado";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoIni].Value = "Inicio";
            ws.Cells[rowIniTabla + 1, colTiempoEjecutadoFin].Value = "Fin";
            ws.Cells[rowIniTabla, colPk].Value = "Pk (kW)";
            ws.Cells[rowIniTabla, colCompensable].Value = "Compensable";
            ws.Cells[rowIniTabla, colEns].Value = "ENS f,k";
            ws.Cells[rowIniTabla, colResarcimiento].Value = "Resarcimiento (US$)";


            //Estilos cabecera
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colSuministrador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCorrelativo, rowIniTabla + 1, colCorrelativo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTipoCliente, rowIniTabla + 1, colTipoCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colNombreCliente, rowIniTabla + 1, colNombreCliente);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colBarraNombre, rowIniTabla + 1, colBarraNombre);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colAlimentador, rowIniTabla + 1, colAlimentador);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEnst, rowIniTabla + 1, colEnst);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, ColEvento, rowIniTabla + 1, ColEvento);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colComentario, rowIniTabla + 1, colComentario);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colTiempoEjecutadoIni, rowIniTabla, colTiempoEjecutadoFin);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colPk, rowIniTabla + 1, colPk);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colCompensable, rowIniTabla + 1, colCompensable);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colEns, rowIniTabla + 1, colEns);
            servFormato.CeldasExcelAgrupar(ws, rowIniTabla, colResarcimiento, rowIniTabla + 1, colResarcimiento);


            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "Calibri", 9);
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento, "#8EA9DB");
            servFormato.CeldasExcelWrapText(ws, rowIniTabla, colSuministrador, rowIniTabla + 1, colResarcimiento);


            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            foreach (var item in listaRegistrosRC)
            {

                ws.Cells[rowData, colSuministrador].Value = item.Suministrador != null ? item.Suministrador.Trim() : null;
                ws.Cells[rowData, colCorrelativo].Value = item.Correlativo != null ? item.Correlativo.Value : item.Correlativo;
                ws.Cells[rowData, colTipoCliente].Value = item.TipoCliente != null ? item.TipoCliente.Trim() : null;
                ws.Cells[rowData, colNombreCliente].Value = item.NombreCliente != null ? item.NombreCliente.Trim() : null;
                ws.Cells[rowData, colBarraNombre].Value = item.BarraNombre != null ? item.BarraNombre.Trim() : null;
                ws.Cells[rowData, colAlimentador].Value = item.AlimentadorNombre != null ? item.AlimentadorNombre.Trim() : null;
                ws.Cells[rowData, colEnst].Value = item.Enst != null ? item.Enst.Value : item.Enst;
                ws.Cells[rowData, colEnst].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, ColEvento].Value = item.EventoNombre != null ? item.EventoNombre.Trim() : null;
                ws.Cells[rowData, colComentario].Value = item.Comentario != null ? item.Comentario.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoIni].Value = item.TiempoEjecutadoIni != null ? item.TiempoEjecutadoIni.Trim() : null;
                ws.Cells[rowData, colTiempoEjecutadoFin].Value = item.TiempoEjecutadoFin != null ? item.TiempoEjecutadoFin.Trim() : null;
                ws.Cells[rowData, colPk].Value = item.Pk != null ? item.Pk.Value : item.Pk;
                ws.Cells[rowData, colPk].Style.Numberformat.Format = FormatoNumDecimales(1);
                ws.Cells[rowData, colCompensable].Value = item.Compensable != null ? item.Compensable.Trim() : null;

                //Ens
                if (item.Ens != null)
                {
                    if (item.Ens.Value == 0)
                        ws.Cells[rowData, colEns].Value = "-";
                    else
                        ws.Cells[rowData, colEns].Value = item.Ens.Value;
                }
                else
                    ws.Cells[rowData, colEns].Value = item.Ens;

                ws.Cells[rowData, colEns].Style.Numberformat.Format = FormatoNumDecimales(4);
                ws.Cells[rowData, colResarcimiento].Value = item.Resarcimiento != null ? item.Resarcimiento.Value : item.Resarcimiento;
                ws.Cells[rowData, colResarcimiento].Style.Numberformat.Format = FormatoNumDecimales(3);

                rowData++;
            }
            if (!listaRegistrosRC.Any()) rowData++;
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 2, colSuministrador, rowData - 1, colResarcimiento, "Calibri", 9);
            servFormato.BorderCeldas2(ws, rowIniTabla, colSuministrador, rowData - 1, colResarcimiento);
            #endregion

            //filter                       
            if (listaRegistrosRC.Any())
            {
                ws.Cells[rowIniTabla, colSuministrador, rowData, colResarcimiento].AutoFitColumns();
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colEnst).Width = 8;
                ws.Column(colTiempoEjecutadoIni).Width = 10;
                ws.Column(colTiempoEjecutadoFin).Width = 10;
                ws.Column(colPk).Width = 8;
                ws.Column(colEns).Width = 8;
            }
            else
            {
                ws.Column(colSuministrador).Width = 20;
                ws.Column(colCorrelativo).Width = 10;
                ws.Column(colTipoCliente).Width = 10;
                ws.Column(colNombreCliente).Width = 12;
                ws.Column(colBarraNombre).Width = 15;
                ws.Column(colAlimentador).Width = 16;
                ws.Column(colEnst).Width = 8;
                ws.Column(ColEvento).Width = 18;
                ws.Column(colComentario).Width = 15;
                ws.Column(colTiempoEjecutadoIni).Width = 10;
                ws.Column(colTiempoEjecutadoFin).Width = 10;
                ws.Column(colPk).Width = 8;
                ws.Column(colCompensable).Width = 12;
                ws.Column(colEns).Width = 8;
                ws.Column(colResarcimiento).Width = 13;
            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 90;
            ws.View.FreezePanes(rowIniTabla + 2, 1);
        }

        #endregion

        #endregion

        #endregion

        #region Carga de Interrupciones

        /// <summary>
        /// Valida si cierta empresa tiene habilitada o no la carga de informacion para cierto periodo
        /// </summary>
        /// <param name="repercodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public string ValidarHabilitacionCargaDatosParaEmpresaPeriodo(int repercodi, int emprcodi, int tipo)
        {
            string salida = "";

            List<ReInterrupcionAccesoDTO> listaHabilitacionPorPeriodo = FactorySic.GetReInterrupcionAccesoRepository().ListByPeriodo(repercodi);
            ReInterrupcionAccesoDTO habilitacionParaEmpresa = listaHabilitacionPorPeriodo.Find(x => x.Emprcodi == emprcodi);

            string valPE = "N";
            string valRC = "N";
            switch (tipo)
            {
                case ConstantesCalculoResarcimiento.HabilitacionIngresoDatosPuntoEntrega:
                    if (habilitacionParaEmpresa != null)
                        valPE = habilitacionParaEmpresa.Reinacptoentrega;
                    if (valPE == "N")
                    {
                        salida = "Esta empresa no tiene habilitada la carga de información para el periodo seleccionado.";

                        List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                         ObtenerPorEmpresaPeriodo(emprcodi, repercodi);

                        if (entitys.Any())
                        {
                            salida = "TIENE_DATOS";
                        }
                    }

                    break;
                case ConstantesCalculoResarcimiento.HabilitacionIngresoDatosRechazoCarga:
                    if (habilitacionParaEmpresa != null)
                        valRC = habilitacionParaEmpresa.Reinacrechazocarga;
                    if (valRC == "N")
                    {
                        salida = "Esta empresa no tiene habilitada la carga de información para el periodo seleccionado.";

                        List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().
                            ObtenerPorEmpresaPeriodo(emprcodi, repercodi);

                        if (entitys.Any())
                        {
                            salida = "TIENE_DATOS";
                        }
                    }
                    break;
            }


            return salida;
        }

        /// <summary>
        /// Permite obtener el formato de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public EstructuraInterrupcion ObtenerEstructuraInterrupciones(int idEmpresa, int idPeriodo, bool flag)
        {
            EstructuraInterrupcion result = new EstructuraInterrupcion();

            bool conDatos;
            result.Data = this.ObtenerDataInterrupciones(idEmpresa, idPeriodo, flag, out conDatos);
            result.Result = 1;
            result.ListaCliente = this.ObtenerEmpresas();
            result.ListaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
            result.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().List();
            result.ListaNivelTension = FactorySic.GetReNivelTensionRepository().List();
            result.ListaEmpresa = this.ObtenerEmpresas();
            int idPeriodoPadre = this.ObtenerIdPeriodoPadre(idPeriodo);
            result.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
            int[] rowspans = new int[result.ListaCausaInterrupcion.Count];
            result.Indicadores = this.ObtenerEstructuraIndicadores(idPeriodoPadre, out rowspans);
            result.PlazoEnvio = this.ValidarPlazoEtapa(idPeriodo, 1);
            result.PlazoEnergia = this.ValidarPlazoEtapa(idPeriodo, 5);
            result.ConDatos = conDatos;

            return result;
        }

        /// <summary>
        /// Obtiene el id del periodo padre
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public int ObtenerIdPeriodoPadre(int periodo)
        {
            int idPeriodo = periodo;
            RePeriodoDTO entityPeriodo = FactorySic.GetRePeriodoRepository().GetById(idPeriodo);
            if ((entityPeriodo.Repertipo == ConstantesCalculoResarcimiento.IdPeriodoTrimestral
                || (entityPeriodo.Repertipo == ConstantesCalculoResarcimiento.IdPeriodoSemestral && entityPeriodo.Reperrevision == ConstantesAppServicio.SI))
                && entityPeriodo.Reperpadre != null)
            {
                if ((int)entityPeriodo.Reperpadre != 0) idPeriodo = (int)entityPeriodo.Reperpadre;
            }

            return idPeriodo;
        }

        /// <summary>
        /// Permite vaidar los plazoa
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="etapa"></param>
        /// <returns></returns>
        private string ValidarPlazoEtapa(int idPeriodo, int etapa)
        {
            List<ReMaestroEtapaDTO> entitys = FactorySic.GetReMaestroEtapaRepository().GetByCriteria(idPeriodo);
            ReMaestroEtapaDTO entity = entitys.Where(x => x.Reetacodi == etapa).First();
            if (entity != null)
            {
                if (entity.FechaFinal != null)
                {
                    int days = (int)DateTime.Now.Subtract((DateTime)entity.FechaFinal).TotalDays;
                    if (days > 0)
                    {
                        return ConstantesAppServicio.SI;
                    }
                }
            }
            return ConstantesAppServicio.NO;
        }


        /// <summary>
        /// Permite obtener la data de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public string[][] ObtenerDataInterrupciones(int idEmpresa, int idPeriodo, bool flag, out bool conDatos)
        {
            List<string[]> result = new List<string[]>();
            List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                ObtenerPorEmpresaPeriodo(idEmpresa, idPeriodo);
            List<ReInterrupcionSuministroDetDTO> detalle = FactorySic.GetReInterrupcionSuministroDetRepository().
                ObtenerPorEmpresaPeriodo(idEmpresa, idPeriodo);

            //Si tiene datos previamente guardados 
            conDatos = false;
            if (entitys.Any())
            {
                string acceso = this.ValidarHabilitacionCargaDatosParaEmpresaPeriodo(idPeriodo, idEmpresa,
                    ConstantesCalculoResarcimiento.HabilitacionIngresoDatosPuntoEntrega);
                if (acceso != string.Empty)
                {
                    conDatos = true;
                }
            }

            foreach (ReInterrupcionSuministroDTO entity in entitys)
            {
                List<ReInterrupcionSuministroDetDTO> subList = detalle.
                    Where(x => x.Reintcodi == entity.Reintcodi).OrderBy(x => x.Reintdorden).ToList();
                ReInterrupcionSuministroDetDTO det1 = subList.Where(x => x.Reintdorden == 1).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det2 = subList.Where(x => x.Reintdorden == 2).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det3 = subList.Where(x => x.Reintdorden == 3).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det4 = subList.Where(x => x.Reintdorden == 4).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det5 = subList.Where(x => x.Reintdorden == 5).FirstOrDefault();

                string[] data = new string[33];
                data[0] = entity.Reintcodi.ToString(); //- Id
                data[1] = string.Empty;                //- Eliminar
                data[2] = entity.Reintcorrelativo.ToString(); //- Correlativo
                data[3] = entity.Reinttipcliente; //- Tipo de cliente
                data[4] = entity.Reintcliente.ToString(); //- Cliente
                data[5] = (entity.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado) ?
                    entity.Repentcodi.ToString() : entity.Reintptoentrega; //- Punto de entrega
                data[6] = (entity.Reintnrosuministro != null) ? entity.Reintnrosuministro.ToString() : string.Empty; //- Nro suministro
                data[7] = entity.Rentcodi.ToString(); //- Nivel de tension
                data[8] = entity.Reintaplicacionnumeral.ToString(); //- Aplicacion literal
                data[9] = (entity.Reintenergiasemestral != null) ? ((decimal)entity.Reintenergiasemestral).ToString() : string.Empty; //- Energia semestral
                data[10] = entity.Reintinctolerancia; //- Incremento de tolerancias
                data[11] = entity.Retintcodi.ToString(); //- Tipo de interrupcion
                data[12] = entity.Recintcodi.ToString(); //- Causa de interrupcion
                data[13] = entity.Reintni.ToString(); //- Indicador NI
                data[14] = entity.Reintki.ToString(); //- Indicador KI
                data[15] = ((DateTime)entity.Reintfejeinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado inicio
                data[16] = ((DateTime)entity.Reintfejefin).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado fin
                data[17] = (entity.Reintfproginicio != null) ?
                    ((DateTime)entity.Reintfproginicio).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado inicio
                data[18] = (entity.Reintfprogfin != null) ?
                    ((DateTime)entity.Reintfprogfin).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado fin
                data[19] = det1.Emprcodi.ToString(); //- Empresa 1
                data[20] = det1.Reintdorcentaje.ToString();
                data[21] = (det2 != null) ? det2.Emprcodi.ToString() : string.Empty; //- Empresa 2
                data[22] = (det2 != null) ? det2.Reintdorcentaje.ToString() : string.Empty;
                data[23] = (det3 != null) ? det3.Emprcodi.ToString() : string.Empty; //- Empresa 3
                data[24] = (det3 != null) ? det3.Reintdorcentaje.ToString() : string.Empty;
                data[25] = (det4 != null) ? det4.Emprcodi.ToString() : string.Empty; //- Empresa 4
                data[26] = (det4 != null) ? det4.Reintdorcentaje.ToString() : string.Empty;
                data[27] = (det5 != null) ? det5.Emprcodi.ToString() : string.Empty; //- Empresa 5
                data[28] = (det5 != null) ? det5.Reintdorcentaje.ToString() : string.Empty;
                data[29] = (entity.Reintcausaresumida != null) ? entity.Reintcausaresumida : string.Empty; //- Causa resumida
                data[30] = entity.Reinteie.ToString(); //- Ei/E
                data[31] = entity.Reintresarcimiento.ToString(); //- Resarcimiento
                data[32] = entity.Reintevidencia; //- Evidencia
                result.Add(data);
            }

            if (entitys.Count == 0 && flag)
            {
                for (int i = 0; i <= 20; i++)
                {
                    string[] data = new string[33];
                    for (int j = 0; j < 33; j++)
                    {
                        data[j] = string.Empty;
                    }
                    result.Add(data);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <param name="template"></param>
        /// <param name="file"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public int GenerarFormatoInterrupciones(string path, string plantilla, string file, int idEmpresa, int idPeriodo)
        {
            try
            {
                EstructuraInterrupcion result = this.ObtenerEstructuraInterrupciones(idEmpresa, idPeriodo, false);
                RePeriodoDTO periodo = FactorySic.GetRePeriodoRepository().GetById(idPeriodo);

                int idPeriodoPadre = this.ObtenerIdPeriodoPadre(idPeriodo);
                RePeriodoDTO periodoPadre = FactorySic.GetRePeriodoRepository().GetById(idPeriodoPadre);

                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + file);
                }


                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet wsData = xlPackage.Workbook.Worksheets["Data"];
                    ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];

                    int index = 2;
                    foreach (ReEmpresaDTO item in result.ListaCliente)
                    {
                        wsData.Cells[index, 2].Value = item.Emprnomb;
                        index++;
                    }

                    index = 2;
                    foreach (RePtoentregaPeriodoDTO item in result.ListaPuntoEntrega)
                    {
                        wsData.Cells[index, 3].Value = item.Repentnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReNivelTensionDTO item in result.ListaNivelTension)
                    {
                        wsData.Cells[index, 4].Value = item.Rentabrev;
                        index++;
                    }

                    index = 2;
                    foreach (ReTipoInterrupcionDTO item in result.ListaTipoInterrupcion)
                    {
                        wsData.Cells[index, 6].Value = item.Retintnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReCausaInterrupcionDTO item in result.ListaCausaInterrupcion)
                    {
                        wsData.Cells[index, 7].Value = item.Recintnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReEmpresaDTO item in result.ListaEmpresa)
                    {
                        wsData.Cells[index, 8].Value = item.Emprnomb;
                        index++;
                    }

                    index = 2;
                    wsData.Cells[index, 9].Value = ((DateTime)periodoPadre.Reperfecinicio).ToString(ConstantesAppServicio.FormatoFecha);
                    wsData.Cells[index, 10].Value = (((DateTime)periodoPadre.Reperfecfin).AddDays(1)).ToString(ConstantesAppServicio.FormatoFecha);

                    index = 5;
                    foreach (string[] data in result.Data)
                    {
                        wsInterrupcion.Cells[index, 1].Value = data[0]; //- Id
                        wsInterrupcion.Cells[index, 2].Value = data[2]; //- Correlativo
                        wsInterrupcion.Cells[index, 3].Value = (data[3] == ConstantesCalculoResarcimiento.TipoClienteLibre) ?
                            ConstantesCalculoResarcimiento.TextoClienteLibre : ConstantesCalculoResarcimiento.TextoClienteRegulado; //- Tipo de cliente
                        wsInterrupcion.Cells[index, 4].Value = result.ListaCliente.Where(x => x.Emprcodi == int.Parse(data[4])).First().Emprnomb; //- Cliente
                        wsInterrupcion.Cells[index, 5].Value = (data[3] == ConstantesCalculoResarcimiento.TipoClienteRegulado) ?
                            result.ListaPuntoEntrega.Where(x => x.Repentcodi == int.Parse(data[5])).First().Repentnombre : data[5]; //- Punto de entrega
                        wsInterrupcion.Cells[index, 6].Value = data[6]; //- Nro de suministro
                        wsInterrupcion.Cells[index, 7].Value = result.ListaNivelTension.Where(x => x.Rentcodi == int.Parse(data[7])).First().Rentabrev; //- Nivel de tension
                        wsInterrupcion.Cells[index, 8].Value = data[8]; //- Correlativo
                        wsInterrupcion.Cells[index, 9].Value = data[9]; //- Energia
                        wsInterrupcion.Cells[index, 10].Value = (data[10] == ConstantesAppServicio.SI) ?
                            ConstantesCalculoResarcimiento.TextoSi : ConstantesCalculoResarcimiento.TextoNo; //- Incremento de tolerancia
                        wsInterrupcion.Cells[index, 11].Value = result.ListaTipoInterrupcion.Where(x => x.Retintcodi == int.Parse(data[11])).First().Retintnombre; //- Tipo de interrupcion
                        wsInterrupcion.Cells[index, 12].Value = result.ListaCausaInterrupcion.Where(x => x.Recintcodi == int.Parse(data[12])).First().Recintnombre; //- Causa de interrupcion
                        wsInterrupcion.Cells[index, 13].Value = data[13]; //- Ni
                        wsInterrupcion.Cells[index, 14].Value = data[14]; //- Ki
                        wsInterrupcion.Cells[index, 15].Value = data[15]; //- Tiempo inicio ejecutado
                        wsInterrupcion.Cells[index, 16].Value = data[16]; //- Tiempo fin ejecutado
                        wsInterrupcion.Cells[index, 17].Value = data[17]; //- Tiempo inicio programado
                        wsInterrupcion.Cells[index, 18].Value = data[18]; //- Tiempo fin programado

                        ReEmpresaDTO empresa1 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[19])).FirstOrDefault();
                        wsInterrupcion.Cells[index, 19].Value = (empresa1 != null) ? empresa1.Emprnomb : data[19]; //- Empresa 1
                        wsInterrupcion.Cells[index, 20].Value = data[20]; //- Porcentaje 1

                        if (!string.IsNullOrEmpty(data[21]))
                        {
                            ReEmpresaDTO empresa2 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[21])).FirstOrDefault();
                            wsInterrupcion.Cells[index, 21].Value = (empresa2 != null) ? empresa2.Emprnomb : data[21]; //- Empresa 2
                        }
                        wsInterrupcion.Cells[index, 22].Value = data[22]; //- Porcentaje 2


                        if (!string.IsNullOrEmpty(data[23]))
                        {
                            ReEmpresaDTO empresa3 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[23])).FirstOrDefault();
                            wsInterrupcion.Cells[index, 23].Value = (empresa3 != null) ? empresa3.Emprnomb : data[23]; //- Empresa 3
                        }
                        wsInterrupcion.Cells[index, 24].Value = data[24]; //- Porcentaje 3


                        if (!string.IsNullOrEmpty(data[25]))
                        {
                            ReEmpresaDTO empresa4 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[25])).FirstOrDefault();
                            wsInterrupcion.Cells[index, 25].Value = (empresa4 != null) ? empresa4.Emprnomb : data[25]; //- Empresa 4
                        }
                        wsInterrupcion.Cells[index, 26].Value = data[26]; //- Porcentaje 4


                        if (!string.IsNullOrEmpty(data[27]))
                        {
                            ReEmpresaDTO empresa5 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[27])).FirstOrDefault();
                            wsInterrupcion.Cells[index, 27].Value = (empresa5 != null) ? empresa5.Emprnomb : data[27]; //- Empresa 5
                        }
                        wsInterrupcion.Cells[index, 28].Value = data[28]; //- Porcentaje 5


                        wsInterrupcion.Cells[index, 29].Value = data[29]; //- Causa resuminda
                        wsInterrupcion.Cells[index, 30].Value = data[30]; //- Ei
                        wsInterrupcion.Cells[index, 31].Value = data[31]; //- Resarcimientos
                        index++;
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Carga los datos a partir de excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="validaciones"></param>
        /// <returns></returns>
        public string[][] CargarInterrupcionesExcel(string path, string file, int empresa, int periodo, out List<string> validaciones, out List<EstructuraCargaFile> listadoArchivos)
        {
            List<string[]> result = new List<string[]>();
            FileInfo fileInfo = new FileInfo(path + file);
            List<string> errores = new List<string>();
            List<EstructuraCargaFile> listFiles = new List<EstructuraCargaFile>();

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];

                if (wsInterrupcion != null)
                {
                    //-- Verificar columnas 
                    List<string> columnas = UtilCalculoResarcimiento.ObtenerEstructuraPlantillaInterrupcuones(path);
                    bool flag = true;
                    for (int i = 1; i <= 32; i++)
                    {
                        string header = (wsInterrupcion.Cells[1, i].Value != null) ? wsInterrupcion.Cells[1, i].Value.ToString() : string.Empty;
                        if (header != columnas[i - 1])
                        {
                            errores.Add("La plantilla ha sido modificada. No puede cambiar o eliminar columnas del Excel.");
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        //- Verificar eliminacion de filas
                        List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                                     ObtenerPorEmpresaPeriodo(empresa, periodo);
                        List<int> idsTotal = entitys.Select(x => x.Reintcodi).Distinct().ToList();
                        List<int> ids = new List<int>();

                        EstructuraInterrupcion maestros = new EstructuraInterrupcion();
                        maestros.ListaCliente = this.ObtenerEmpresas();
                        maestros.ListaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
                        maestros.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().List();
                        maestros.ListaNivelTension = FactorySic.GetReNivelTensionRepository().List();
                        maestros.ListaEmpresa = this.ObtenerEmpresas();
                        int idPeriodoPadre = this.ObtenerIdPeriodoPadre(periodo);
                        maestros.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
                        maestros.PlazoEnergia = this.ValidarPlazoEtapa(periodo, 5);
                        bool flagPlazo = true;

                        for (int index = 5; index <= ConstantesCalculoResarcimiento.CantidadRegistrosFormato; index++)
                        {
                            if (
                                (wsInterrupcion.Cells[index, 2].Value != null && wsInterrupcion.Cells[index, 2].Value != string.Empty) ||
                                (wsInterrupcion.Cells[index, 3].Value != null && wsInterrupcion.Cells[index, 3].Value != string.Empty) ||
                                (wsInterrupcion.Cells[index, 4].Value != null && wsInterrupcion.Cells[index, 4].Value != string.Empty) ||
                                (wsInterrupcion.Cells[index, 5].Value != null && wsInterrupcion.Cells[index, 5].Value != string.Empty))
                            {
                                string[] data = new string[33];

                                if (wsInterrupcion.Cells[index, 1].Value != null && wsInterrupcion.Cells[index, 1].Value != string.Empty)
                                {
                                    ids.Add(int.Parse(wsInterrupcion.Cells[index, 1].Value.ToString()));
                                }

                                data[0] = (wsInterrupcion.Cells[index, 1].Value != null) ? wsInterrupcion.Cells[index, 1].Value.ToString() : string.Empty; //- Id

                                ReInterrupcionSuministroDTO entity = null;
                                int valorId = 0;
                                if (!string.IsNullOrEmpty(data[0]))
                                {
                                    if (int.TryParse(data[0], out valorId))
                                    {
                                        entity = entitys.Where(x => x.Reintcodi == int.Parse(data[0])).FirstOrDefault();
                                    }
                                }

                                data[1] = string.Empty;
                                data[2] = (wsInterrupcion.Cells[index, 2].Value != null) ? wsInterrupcion.Cells[index, 2].Value.ToString() : string.Empty; //- Correlativo

                                string tipoCliente = string.Empty;
                                if (wsInterrupcion.Cells[index, 3].Value != null)
                                {
                                    tipoCliente = (wsInterrupcion.Cells[index, 3].Value.ToString() == ConstantesCalculoResarcimiento.TextoClienteRegulado) ?
                                             ConstantesCalculoResarcimiento.TipoClienteRegulado : ConstantesCalculoResarcimiento.TipoClienteLibre;
                                }
                                data[3] = tipoCliente; //- Tipo de cliente

                                string cliente = string.Empty;
                                if (wsInterrupcion.Cells[index, 4].Value != null)
                                {
                                    ReEmpresaDTO reCliente = maestros.ListaCliente.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 4].Value.ToString()).FirstOrDefault();
                                    if (reCliente != null) cliente = reCliente.Emprcodi.ToString();
                                }
                                data[4] = cliente; //- Cliente

                                string ptoEntrega = string.Empty;
                                if (wsInterrupcion.Cells[index, 5].Value != null)
                                {
                                    if (tipoCliente == ConstantesCalculoResarcimiento.TipoClienteRegulado)
                                    {
                                        RePtoentregaPeriodoDTO rePtoEntrega = maestros.ListaPuntoEntrega.Where(x => x.Repentnombre == wsInterrupcion.Cells[index, 5].Value.ToString()).FirstOrDefault();
                                        if (rePtoEntrega != null) ptoEntrega = rePtoEntrega.Repentcodi.ToString();
                                    }
                                    else
                                    {
                                        ptoEntrega = wsInterrupcion.Cells[index, 5].Value.ToString();
                                    }
                                }

                                data[5] = ptoEntrega; //- Punto de entrega
                                data[6] = (wsInterrupcion.Cells[index, 6].Value != null) ? wsInterrupcion.Cells[index, 6].Value.ToString() : string.Empty; //- Nro de suministro

                                string nivelTension = string.Empty;
                                if (wsInterrupcion.Cells[index, 7].Value != null)
                                {
                                    ReNivelTensionDTO reNivelTension = maestros.ListaNivelTension.Where(x => x.Rentabrev == wsInterrupcion.Cells[index, 7].Value.ToString()).FirstOrDefault();
                                    if (reNivelTension != null) nivelTension = reNivelTension.Rentcodi.ToString();
                                }
                                data[7] = nivelTension; //- Nivel de tension
                                data[8] = (wsInterrupcion.Cells[index, 8].Value != null) ? wsInterrupcion.Cells[index, 8].Value.ToString() : string.Empty; //- Correlativo
                                data[9] = (wsInterrupcion.Cells[index, 9].Value != null) ? wsInterrupcion.Cells[index, 9].Value.ToString() : string.Empty; //- Energia

                                //- Validando plazo de la etapa 5
                                if (entity != null && maestros.PlazoEnergia == ConstantesAppServicio.SI)
                                {
                                    if (data[9] != string.Empty && decimal.Parse(data[9]) != entity.Reintenergiasemestral)
                                        flagPlazo = false;
                                }

                                string incremento = string.Empty;
                                if (wsInterrupcion.Cells[index, 10].Value != null)
                                {
                                    incremento = (wsInterrupcion.Cells[index, 10].Value.ToString() == ConstantesCalculoResarcimiento.TextoSi) ?
                                        ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                                }
                                data[10] = incremento; //- Incremento de tolerancia

                                string tipoInterrupcion = string.Empty;
                                if (wsInterrupcion.Cells[index, 11].Value != null)
                                {
                                    ReTipoInterrupcionDTO reTipoInterrupcion = maestros.ListaTipoInterrupcion.Where(x => x.Retintnombre == wsInterrupcion.Cells[index, 11].Value.ToString()).FirstOrDefault();
                                    if (reTipoInterrupcion != null) tipoInterrupcion = reTipoInterrupcion.Retintcodi.ToString();
                                }
                                data[11] = tipoInterrupcion; //- Tipo de interrupcion

                                string causaInterrupcion = string.Empty;
                                if (wsInterrupcion.Cells[index, 12].Value != null)
                                {
                                    int idTipoInterrupcion = 0;
                                    if (!string.IsNullOrEmpty(tipoInterrupcion))
                                    {
                                       idTipoInterrupcion = int.Parse(tipoInterrupcion);
                                    }

                                    ReCausaInterrupcionDTO reCausaInterrupcion = maestros.ListaCausaInterrupcion.Where(x => x.Recintnombre == wsInterrupcion.Cells[index, 12].Value.ToString() &&
                                    (x.Retintcodi == idTipoInterrupcion || idTipoInterrupcion == 0)).FirstOrDefault();
                                                                     
                                    if (reCausaInterrupcion != null) causaInterrupcion = reCausaInterrupcion.Recintcodi.ToString();
                                }
                                data[12] = causaInterrupcion; //- Causa de interrupcion


                                data[13] = (wsInterrupcion.Cells[index, 13].Value != null) ? wsInterrupcion.Cells[index, 13].Value.ToString() : string.Empty; //- Ni
                                data[14] = (wsInterrupcion.Cells[index, 14].Value != null) ? wsInterrupcion.Cells[index, 14].Value.ToString() : string.Empty; //- Ki
                                data[15] = (wsInterrupcion.Cells[index, 15].Value != null) ? wsInterrupcion.Cells[index, 15].Value.ToString() : string.Empty; //- Tiempo inicio ejecutado
                                data[16] = (wsInterrupcion.Cells[index, 16].Value != null) ? wsInterrupcion.Cells[index, 16].Value.ToString() : string.Empty; //- Tiempo fin ejecutado
                                data[17] = (wsInterrupcion.Cells[index, 17].Value != null) ? wsInterrupcion.Cells[index, 17].Value.ToString() : string.Empty; //- Tiempo inicio programado
                                data[18] = (wsInterrupcion.Cells[index, 18].Value != null) ? wsInterrupcion.Cells[index, 18].Value.ToString() : string.Empty; //- Tiempo fin programado

                                string emprcodi1 = string.Empty;
                                if (wsInterrupcion.Cells[index, 19].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 19].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi1 = reEmpresa.Emprcodi.ToString();
                                }

                                data[19] = emprcodi1; //- Empresa 1
                                data[20] = (wsInterrupcion.Cells[index, 20].Value != null) ? wsInterrupcion.Cells[index, 20].Value.ToString() : string.Empty; //- Porcentaje 1


                                string emprcodi2 = string.Empty;
                                if (wsInterrupcion.Cells[index, 21].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 21].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi2 = reEmpresa.Emprcodi.ToString();
                                }
                                data[21] = emprcodi2;//- Empresa 2
                                data[22] = (wsInterrupcion.Cells[index, 22].Value != null) ? wsInterrupcion.Cells[index, 22].Value.ToString() : string.Empty; //- Porcentaje 2


                                string emprcodi3 = string.Empty;
                                if (wsInterrupcion.Cells[index, 23].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 23].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi3 = reEmpresa.Emprcodi.ToString();
                                }
                                data[23] = emprcodi3; //- Empresa 3
                                data[24] = (wsInterrupcion.Cells[index, 24].Value != null) ? wsInterrupcion.Cells[index, 24].Value.ToString() : string.Empty; //- Porcentaje 3


                                string emprcodi4 = string.Empty;
                                if (wsInterrupcion.Cells[index, 25].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 25].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi4 = reEmpresa.Emprcodi.ToString();
                                }
                                data[25] = emprcodi4; //- Empresa 4
                                data[26] = (wsInterrupcion.Cells[index, 26].Value != null) ? wsInterrupcion.Cells[index, 26].Value.ToString() : string.Empty; //- Porcentaje 4


                                string emprcodi5 = string.Empty;
                                if (wsInterrupcion.Cells[index, 27].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 27].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi5 = reEmpresa.Emprcodi.ToString();
                                }
                                data[27] = emprcodi5; //- Empresa 5
                                data[28] = (wsInterrupcion.Cells[index, 28].Value != null) ? wsInterrupcion.Cells[index, 28].Value.ToString() : string.Empty; //- Porcentaje 5
                                data[29] = (wsInterrupcion.Cells[index, 29].Value != null) ? wsInterrupcion.Cells[index, 29].Value.ToString() : string.Empty; //- Causa resuminda
                                data[30] = (wsInterrupcion.Cells[index, 30].Value != null) ? wsInterrupcion.Cells[index, 30].Value.ToString() : string.Empty; //- Ei
                                data[31] = (wsInterrupcion.Cells[index, 31].Value != null) ? wsInterrupcion.Cells[index, 31].Value.ToString() : string.Empty; //- Resarcimientos

                                //- Validando plazo de la etapa 5
                                if (entity != null && maestros.PlazoEnergia == ConstantesAppServicio.SI)
                                {
                                    if (data[30] != string.Empty && decimal.Parse(data[30]) != entity.Reinteie)
                                        flagPlazo = false;
                                    if (data[31] != string.Empty && decimal.Parse(data[31]) != entity.Reintresarcimiento)
                                        flagPlazo = false;
                                }

                                if (entity != null)
                                {
                                    data[32] = entity.Reintevidencia; //- Para saber si tiene archivo
                                }
                                else
                                {
                                    data[32] = string.Empty;
                                }

                                result.Add(data);


                                EstructuraCargaFile itemFile = new EstructuraCargaFile();
                                itemFile.Id = valorId;
                                itemFile.Fila = index;
                                itemFile.FileName = (wsInterrupcion.Cells[index, 32].Value != null && wsInterrupcion.Cells[index, 32].Value != string.Empty) ?
                                    wsInterrupcion.Cells[index, 32].Value.ToString() : string.Empty;

                                listFiles.Add(itemFile);
                            }
                        }

                        if (idsTotal.Where(x => ids.Any(y => x == y)).Count() != idsTotal.Count)
                        {
                            errores.Add("Si desea editar registros, primero debe descargar la plantilla nuevamente.");
                            flag = false;
                        }

                        if (!flagPlazo)
                        {
                            errores.Add("No puede cambiar los datos Energía Semestral (kWh), Ei/E y Resarcimiento (US$) dado que ha superado el plazo de la etapa 5.");
                            flag = false;
                        }
                    }
                }
                else
                {
                    errores.Add("No existe la hoja 'Interrupciones' en el libro Excel.");
                }
            }
            validaciones = errores;
            listadoArchivos = listFiles;
            return result.ToArray();
        }

        /// <summary>
        /// Permite grabar las interrupciones
        /// </summary>
        /// <param name="data"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="username"></param>
        /// <param name="comentario"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public EstructuraGrabado GrabarInterrupciones(string[][] data, int empresa, int periodo, string username, string comentario, string path, string fuente)
        {
            EstructuraGrabado result = new EstructuraGrabado();
            try
            {
                bool flagCalculo = true;
                string[][] calculo = this.CalcularResarcimientoInterrupcion(data, periodo, out flagCalculo, false);
                if (flagCalculo)
                {
                    List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                             ObtenerPorEmpresaPeriodo(empresa, periodo);
                    List<ReInterrupcionSuministroDetDTO> detalle = FactorySic.GetReInterrupcionSuministroDetRepository().
                             ObtenerPorEmpresaPeriodo(empresa, periodo);

                    List<int> idsNuevos = new List<int>();
                    List<EstructuraNotificacion> idsActualizados = new List<EstructuraNotificacion>();

                    for (int i = 4; i < data.Length; i++)
                    {
                        if (data[i][2] == string.Empty || data[i][5] == string.Empty) break;

                        ReInterrupcionSuministroDTO entity = new ReInterrupcionSuministroDTO();
                        entity.Reintcodi = (!string.IsNullOrEmpty(data[i][0])) ? int.Parse(data[i][0]) : 0;

                        if (entity.Reintcodi > 0)
                            if (entitys.Where(x => x.Reintcodi == entity.Reintcodi).Count() == 0) entity.Reintcodi = 0;


                        entity.Repercodi = periodo;
                        entity.Reintestado = ConstantesAppServicio.Activo;
                        entity.Reintpadre = -1; //- Para padres que se visualizan
                        entity.Reintfinal = ConstantesAppServicio.SI;
                        entity.Emprcodi = empresa;
                        entity.Reintcorrelativo = int.Parse(data[i][2]);
                        entity.Reinttipcliente = data[i][3];
                        entity.Reintcliente = int.Parse(data[i][4]);
                        if (entity.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado)
                            entity.Repentcodi = int.Parse(data[i][5]);
                        else
                            entity.Reintptoentrega = data[i][5];
                        entity.Reintnrosuministro = data[i][6];
                        entity.Rentcodi = int.Parse(data[i][7]);
                        entity.Reintaplicacionnumeral = int.Parse(data[i][8]);
                        entity.Reintenergiasemestral = decimal.Parse(data[i][9]);
                        entity.Reintinctolerancia = data[i][10];
                        entity.Retintcodi = int.Parse(data[i][11]);
                        entity.Recintcodi = int.Parse(data[i][12]);
                        entity.Reintni = decimal.Parse(data[i][13]);
                        entity.Reintki = decimal.Parse(data[i][14]);
                        entity.Reintfejeinicio = DateTime.ParseExact(data[i][15], ConstantesAppServicio.FormatoFechaFull2,
                            CultureInfo.InvariantCulture);
                        entity.Reintfejefin = DateTime.ParseExact(data[i][16], ConstantesAppServicio.FormatoFechaFull2,
                            CultureInfo.InvariantCulture);
                        entity.Reintfproginicio = (!string.IsNullOrEmpty(data[i][17])) ?
                            (DateTime?)DateTime.ParseExact(data[i][17], ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture) : null;
                        entity.Reintfprogfin = (!string.IsNullOrEmpty(data[i][18])) ?
                            (DateTime?)DateTime.ParseExact(data[i][18], ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture) : null;
                        entity.Emprcodi1 = int.Parse(data[i][19]);
                        entity.Porcentaje1 = decimal.Parse(data[i][20]);
                        entity.Emprcodi2 = (!string.IsNullOrEmpty(data[i][21])) ? (int?)int.Parse(data[i][21]) : null;
                        entity.Porcentaje2 = (!string.IsNullOrEmpty(data[i][22])) ? (decimal?)decimal.Parse(data[i][22]) : null;
                        entity.Emprcodi3 = (!string.IsNullOrEmpty(data[i][23])) ? (int?)int.Parse(data[i][23]) : null;
                        entity.Porcentaje3 = (!string.IsNullOrEmpty(data[i][24])) ? (decimal?)decimal.Parse(data[i][24]) : null;
                        entity.Emprcodi4 = (!string.IsNullOrEmpty(data[i][25])) ? (int?)int.Parse(data[i][25]) : null;
                        entity.Porcentaje4 = (!string.IsNullOrEmpty(data[i][26])) ? (decimal?)decimal.Parse(data[i][26]) : null;
                        entity.Emprcodi5 = (!string.IsNullOrEmpty(data[i][27])) ? (int?)int.Parse(data[i][27]) : null;
                        entity.Porcentaje5 = (!string.IsNullOrEmpty(data[i][28])) ? (decimal?)decimal.Parse(data[i][28]) : null;
                        entity.Reintcausaresumida = data[i][29];
                        entity.Reinteie = decimal.Parse(data[i][30]);
                        entity.Reintresarcimiento = decimal.Parse(data[i][31]);
                        entity.Reintevidencia = data[i][32];
                        entity.Reintusucreacion = username;
                        entity.Reintfeccreacion = DateTime.Now;

                        if (entity.Reintcodi == 0)
                        {
                            int id = this.GrabarInterrupcion(entity, null, null, null, null, null, path);
                            //- Marcamos los ids de los nuevos
                            idsNuevos.Add(id);

                            if (!string.IsNullOrEmpty(data[i][32]))
                            {
                                string fileTemporal = string.Format(ConstantesCalculoResarcimiento.TemporalInterrupcion, i, data[i][32]);
                                string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id, data[i][32]);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }

                                /*if (File.Exists(path + fileTemporal))
                                {
                                    File.Move(path + fileTemporal, path + fileFinal);
                                }*/
                            }
                        }
                        else
                        {
                            ReInterrupcionSuministroDTO original = entitys.Where(x => x.Reintcodi == entity.Reintcodi).First();
                            List<ReInterrupcionSuministroDetDTO> subList = detalle.Where(x => x.Reintcodi == entity.Reintcodi).ToList();
                            ReInterrupcionSuministroDetDTO det1 = subList.Where(x => x.Reintdorden == 1).FirstOrDefault();
                            ReInterrupcionSuministroDetDTO det2 = subList.Where(x => x.Reintdorden == 2).FirstOrDefault();
                            ReInterrupcionSuministroDetDTO det3 = subList.Where(x => x.Reintdorden == 3).FirstOrDefault();
                            ReInterrupcionSuministroDetDTO det4 = subList.Where(x => x.Reintdorden == 4).FirstOrDefault();
                            ReInterrupcionSuministroDetDTO det5 = subList.Where(x => x.Reintdorden == 5).FirstOrDefault();
                            original.Emprcodi1 = det1.Emprcodi; //- Empresa 1
                            original.Porcentaje1 = det1.Reintdorcentaje;
                            original.Emprcodi2 = (det2 != null) ? det2.Emprcodi : null; //- Empresa 2
                            original.Porcentaje2 = (det2 != null) ? det2.Reintdorcentaje : null;
                            original.Emprcodi3 = (det3 != null) ? det3.Emprcodi : null; //- Empresa 3
                            original.Porcentaje3 = (det3 != null) ? det3.Reintdorcentaje : null;
                            original.Emprcodi4 = (det4 != null) ? det4.Emprcodi : null; //- Empresa 4
                            original.Porcentaje4 = (det4 != null) ? det4.Reintdorcentaje : null;
                            original.Emprcodi5 = (det5 != null) ? det5.Emprcodi : null; //- Empresa 5
                            original.Porcentaje5 = (det5 != null) ? det5.Reintdorcentaje : null;

                            bool comparar = UtilCalculoResarcimiento.CompararRegistroInterrupcion(entity, original);
                            if (!comparar)
                            {
                                int id = this.GrabarInterrupcion(entity, det1, det2, det3, det4, det5, path);
                                idsActualizados.Add(new EstructuraNotificacion { IdNuevo = id, IdAnterior = entity.Reintcodi });

                                original.Reintfinal = ConstantesAppServicio.NO;
                                original.Reintpadre = id;
                                FactorySic.GetReInterrupcionSuministroRepository().Update(original);

                                if (!string.IsNullOrEmpty(entity.Reintevidencia) && !string.IsNullOrEmpty(original.Reintevidencia))
                                {
                                    string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, original.Reintcodi, original.Reintevidencia);
                                    string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id, entity.Reintevidencia);

                                    /*if (File.Exists(path + fileTemporal))
                                    {
                                        File.Move(path + fileTemporal, path + fileFinal);
                                    }*/
                                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                    {
                                        FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                    }
                                }
                            }
                        }
                    }

                    ReEnvioDTO envio = new ReEnvioDTO();
                    envio.Repercodi = periodo;
                    envio.Reenvtipo = ConstantesCalculoResarcimiento.EnvioTipoInterrupcion;
                    envio.Emprcodi = empresa;
                    envio.Reenvfecha = DateTime.Now;
                    envio.Reenvplazo = (!string.IsNullOrEmpty(comentario)) ? ConstantesAppServicio.NO : ConstantesAppServicio.SI;
                    envio.Reenvcomentario = comentario;
                    envio.Reenvestado = ConstantesAppServicio.Activo;
                    envio.Reenvusucreacion = username;
                    envio.Reenvfeccreacion = DateTime.Now;
                    FactorySic.GetReEnvioRepository().Save(envio);

                    if (fuente == ConstantesCalculoResarcimiento.FuenteIngresoExtranet)
                    {
                        if (idsNuevos.Count > 0 || idsActualizados.Count > 0)
                        {
                            this.EnviarNotificacion(ConstantesCalculoResarcimiento.EnvioTipoInterrupcion, ConstantesCalculoResarcimiento.TipoNotificacionNuevoActualizacion,
                                0, idsNuevos, idsActualizados, periodo, empresa);
                        }
                    }

                    result.Result = 1;
                }
                else
                {
                    result.Result = 2; // Existen error en el calculo
                    result.Data = calculo;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result.Result = -1;
            }

            return result;
        }

        /// <summary>
        /// Permite realizar el cálculo del resarcimiento por interrupciones de suministro
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <param name="flagCalculo"></param>
        /// <param name="flagAnulacion"></param>
        public string[][] CalcularResarcimientoInterrupcion(string[][] data, int periodo, out bool flagCalculo, bool flagAnulacion)
        {
            //- Datos del periodo
            RePeriodoDTO entityPeriodo = FactorySic.GetRePeriodoRepository().GetById(periodo);
            decimal factorE = (decimal)entityPeriodo.Reperfactorcomp;
            flagCalculo = true;
            //- Tolerancia de indicadores
            int idPeriodoPadre = this.ObtenerIdPeriodoPadre(periodo);
            RePeriodoDTO entityPeriodoPadre = FactorySic.GetRePeriodoRepository().GetById(idPeriodoPadre);
            decimal horasSemestre = (decimal)(((DateTime)entityPeriodoPadre.Reperfecfin).AddDays(1)).Subtract((DateTime)entityPeriodoPadre.Reperfecinicio).TotalHours;

            List<ReToleranciaPeriodoDTO> tolerancias = FactorySic.GetReToleranciaPeriodoRepository().ObtenerParaImportar(idPeriodoPadre);

            //- Obteniendo la data
            List<ReInterrupcionSuministroDTO> entitys = new List<ReInterrupcionSuministroDTO>();
            for (int i = 4; i < data.Length; i++)
            {
                if (data[i][2] == string.Empty || data[i][5] == string.Empty) break;
                ReInterrupcionSuministroDTO entity = new ReInterrupcionSuministroDTO();
                entity.Reintcorrelativo = int.Parse(data[i][2]); //- Correlativo
                entity.Reinttipcliente = data[i][3];  //- Tipo de cliente
                entity.Reintcliente = int.Parse(data[i][4]);
                entity.Reintptoentrega = data[i][5];  //- Punto de entrega               
                entity.Rentcodi = int.Parse(data[i][7]); // nivel de tesnion
                entity.Reintaplicacionnumeral = int.Parse(data[i][8]); //  meses
                entity.Reintenergiasemestral = decimal.Parse(data[i][9]); // energia semestral
                entity.Reintinctolerancia = data[i][10]; //- incremento tolerancia
                entity.Retintcodi = int.Parse(data[i][11]);
                entity.Recintcodi = int.Parse(data[i][12]);
                entity.Reintni = decimal.Parse(data[i][13]); // ni
                entity.Reintki = decimal.Parse(data[i][14]); // ki
                entity.Reintfejeinicio = DateTime.ParseExact(data[i][15], ConstantesAppServicio.FormatoFechaFull2,
                    CultureInfo.InvariantCulture);
                entity.Reintfejefin = DateTime.ParseExact(data[i][16], ConstantesAppServicio.FormatoFechaFull2,
                    CultureInfo.InvariantCulture);
                entity.Horasdiferencia = (decimal)((DateTime)entity.Reintfejefin).Subtract((DateTime)entity.Reintfejeinicio).TotalHours; // horas
                entity.Reinteie = decimal.Parse(data[i][30]);
                entity.Reintresarcimiento = decimal.Parse(data[i][31]);
                if (flagAnulacion)
                    entity.OrdenRegistro = int.Parse(data[i][1]); //- Artificio para grabar el orden para anulacion

                entitys.Add(entity);
            }

            //- Agrupamos los puntos de entrega
            var listPtoEntrega = entitys.Select(x => new { x.Reintptoentrega, x.Reintcliente }).Distinct().ToList();
            foreach (var ptoEntrega in listPtoEntrega)
            {
                List<ReInterrupcionSuministroDTO> subList = entitys.Where(x => x.Reintptoentrega == ptoEntrega.Reintptoentrega && x.Reintcliente == ptoEntrega.Reintcliente).ToList();
                int meses = (int)subList[0].Reintaplicacionnumeral;
                decimal energia = (decimal)subList[0].Reintenergiasemestral;
                int nivelTension = (int)subList[0].Rentcodi;
                string incremento = subList[0].Reintinctolerancia;
                int tolN = 0;
                int tolD = 0;
                ReToleranciaPeriodoDTO entityTolerancia = tolerancias.Where(x => x.Rentcodi == nivelTension).First();

                if (incremento == ConstantesAppServicio.NO)
                {
                    tolN = (int)entityTolerancia.Retolninf;
                    tolD = (int)entityTolerancia.Retoldinf;
                }
                else
                {
                    tolN = (int)entityTolerancia.Retolnsup;
                    tolD = (int)entityTolerancia.Retoldsup;
                }

                if (meses != 6)
                {
                    tolN = (int)(Math.Ceiling((double)(tolN * meses / 6M)));
                    tolD = (int)(Math.Ceiling((double)(tolD * meses / 6M)));
                }

                //- Calculo a nivel de registro
                decimal n = 0;
                decimal d = 0;
                decimal dreal = 0;
                bool formulaAdicional = false;

                foreach (ReInterrupcionSuministroDTO item in subList)
                {
                    n = n + (decimal)item.Reintni;
                    d = d + (decimal)item.Reintki * item.Horasdiferencia;
                    dreal = dreal + item.Horasdiferencia;
                    if (item.Horasdiferencia > 34)
                    {
                        if (item.Recintcodi == 6 || item.Recintcodi == 10 || item.Recintcodi == 11)
                            formulaAdicional = true;
                    }
                }

                decimal energianosuministrada = energia * d / (horasSemestre - dreal);
                decimal factor = 0;


                if (n <= tolN && d <= tolD)
                {
                    factor = 0;
                }
                else
                {
                    factor = 1;
                    if (n > tolN)
                    {
                        factor = factor + (n - tolN) / tolN;
                    }
                    if (d > tolD)
                    {
                        if (formulaAdicional)
                        {
                            factor = factor + (decimal)((24m - tolD) / tolD) + (decimal)((d - tolD) / (tolD * 3m));
                        }
                        else
                        {
                            factor = factor + (d - tolD) / tolD;
                        }
                    }
                }

                decimal resarcimiento = factorE * energianosuministrada * factor;

                //- Hacemos la reparticion
                decimal di = 0;
                decimal factorparcial = 0;
                foreach (ReInterrupcionSuministroDTO item in subList)
                {
                    di = (decimal)item.Reintki * item.Horasdiferencia;

                    if (factor != 0)
                    {
                        if (n == 0)
                        {
                            factorparcial = di / d;
                        }
                        if (d == 0)
                        {
                            factorparcial = (decimal)item.Reintni / n;
                        }
                        if (n != 0 && d != 0)
                        {
                            factorparcial = 0.5M * ((decimal)item.Reintni / n + di / d);
                        }
                        if (n > tolN)
                        {
                            factorparcial = factorparcial + ((decimal)item.Reintni / n) * (n - tolN) / tolN;
                        }
                        if (d > tolD)
                        {
                            if (formulaAdicional)
                            {
                                factorparcial = factorparcial + (di / d) * ((24m - tolD) / tolD + (decimal)((d - tolD) / (tolD * 3m)));
                            }
                            else
                            {
                                factorparcial = factorparcial + (di / d) * (d - tolD) / tolD;
                            }
                        }
                        factorparcial = factorparcial / factor;
                    }
                    else
                        factorparcial = 0;

                    item.Factorcalculado = factorparcial;
                    item.Resarcimientocalculado = factorparcial * resarcimiento;

                    if ((decimal)Math.Abs((decimal)item.Reintresarcimiento - item.Resarcimientocalculado) > 0.01M)
                    {
                        item.DiferenciaCalculo = ConstantesAppServicio.SI;
                        flagCalculo = false;
                    }

                }
            }

            int orden = 0;
            string[][] result = new string[entitys.Count][];
            foreach (ReInterrupcionSuministroDTO item in entitys)
            {
                string[] itemData = new string[4];
                itemData[0] = orden.ToString();
                itemData[1] = Math.Round(item.Factorcalculado, 4).ToString();
                itemData[2] = Math.Round(item.Resarcimientocalculado, 4).ToString();
                if (!flagAnulacion)
                    itemData[3] = item.DiferenciaCalculo;
                else
                {
                    itemData[3] = item.OrdenRegistro.ToString(); // artificio para pasar el orden
                }
                result[orden] = itemData;
                orden++;
            }
            return result;
        }

        /// <summary>
        /// Permite anular una interrupcion
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idInterrupcion"></param>
        /// <param name="comentario"></param>
        /// <param name="username"></param>
        /// <param name="fuente"></param>
        /// <param name="recalculo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int AnularInterrupcion(int idPeriodo, int idInterrupcion, string comentario, string username, string fuente,
            string recalculo, string[][] data)
        {
            try
            {
                string plazo = this.ValidarPlazoEtapa(idPeriodo, 7);
                if (plazo != ConstantesAppServicio.SI)
                {
                    FactorySic.GetReInterrupcionSuministroRepository().AnularInterrupcion(idInterrupcion, comentario, username);

                    if (recalculo == ConstantesAppServicio.SI)
                    {                        
                        foreach (string[] item in data)
                        {
                            int id = int.Parse(item[0]);
                            decimal ei = decimal.Parse(item[1]);
                            decimal resarcimiento = decimal.Parse(item[2]);

                            FactorySic.GetReInterrupcionSuministroRepository().ActualizarResarcimiento(id, ei, resarcimiento);
                        }
                    }

                    if (fuente == ConstantesCalculoResarcimiento.FuenteIngresoExtranet)
                    {
                        this.EnviarNotificacion(ConstantesCalculoResarcimiento.EnvioTipoInterrupcion,
                            ConstantesCalculoResarcimiento.TipoNotificacionEliminacion, idInterrupcion, new List<int>(), new List<EstructuraNotificacion>(), idPeriodo, 0);
                    }

                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener los datos de anulacion de una interrupcion
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public EstructuraGrabado ObtenerDatosAnulacion(string[][] data, int periodo)
        {
            EstructuraGrabado result = new EstructuraGrabado();
            bool flagCalculo = false;
            string[][] calculo = this.CalcularResarcimientoInterrupcion(data, periodo, out flagCalculo, true);

            if (!flagCalculo)
            {
                result.Data = calculo;
                result.Result = 1; // - Existen diferencias en el cálculo
            }
            else
            {
                result.Result = 2; //  - No existen diferencias en el cálculo
            }

            return result;

        }

        /// <summary>
        /// Permite obtener los datos de anulacion de una interrupcion
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public EstructuraGrabado ObtenerDatosAnulacionRC(string[][] data, int periodo)
        {
            EstructuraGrabado result = new EstructuraGrabado();
            bool flagCalculo = false;
            bool flagEnergia = false;
            string[][] calculo = this.CalcularRechazoCarga(data, periodo, out flagCalculo, out flagEnergia, true);

            if (!flagCalculo)
            {
                result.Data = calculo;
                result.Result = 1; // - Existen diferencias en el cálculo
            }
            else
            {
                result.Result = 2; //  - No existen diferencias en el cálculo
            }

            return result;

        }

        /// <summary>
        /// Permie aliminar el archivo de una interrupcion
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int EliminarArchivoInterrupcion(string path)
        {
            try
            {
                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, path,
                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                {
                    FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos + path,
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                }

                /*if (File.Exists(path))
                {
                    File.Delete(path);
                }*/
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener los envios de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<ReEnvioDTO> ObtenerEnviosInterrupciones(int idEmpresa, int idPeriodo, string tipo)
        {
            return this.FormatearEnvio(FactorySic.GetReEnvioRepository().GetByCriteria(idEmpresa, idPeriodo, tipo));
        }

        /// <summary>
        /// Permite obtener el path para los archivos
        /// </summary>
        /// <returns></returns>
        public string ObtenerPathArchivosResarcimiento()
        {
            string pathBase = ConfigurationManager.AppSettings[ConstantesCalculoResarcimiento.RutaBaseResarcimientos].ToString();
            return pathBase + ConstantesCalculoResarcimiento.FolderResarcimientos;
        }

        /// <summary>
        /// Permite grabar el archivo de interrupción
        /// </summary>
        /// <param name="id"></param>
        /// <param name="interrupcion"></param>
        public void GrabarArchivoInterrupcion(int id, string extension)
        {
            FactorySic.GetReInterrupcionSuministroRepository().ActualizarArchivo(id, extension);
        }

        /// <summary>
        /// Permite actualizar el archivo de observacion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extension"></param>
        public void ActualizarArchivoObservacion(int id, string extension)
        {
            FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarArchivoObservacion(id, extension);
        }

        /// <summary>
        /// Permite actualizar el archivo de observacion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extension"></param>
        public void ActualizarArchivoRespuesta(int id, string extension)
        {
            FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarArchivoRespuesta(id, extension);
        }

        /// <summary>
        /// Permite obtener el detalle de interrupcion
        /// </summary>
        /// <param name="itemDetalle"></param>
        /// <param name="det"></param>
        public ReInterrupcionSuministroDetDTO ObtenerDetalleInterrupcion(ReInterrupcionSuministroDetDTO itemDetalle, ReInterrupcionSuministroDetDTO det)
        {
            if (det != null && itemDetalle.Emprcodi == det.Emprcodi)
            {
                itemDetalle.Reintdcomentarioresp = det.Reintdcomentarioresp;
                itemDetalle.Reintdcomentariosumi = det.Reintdcomentariosumi;
                itemDetalle.Reintdconformidadresp = det.Reintdconformidadresp;
                itemDetalle.Reintdconformidadsumi = det.Reintdconformidadsumi;
                itemDetalle.Reintddetalleresp = det.Reintddetalleresp;
                itemDetalle.Reintdevidenciaresp = det.Reintdevidenciaresp;
                itemDetalle.Reintdevidenciasumi = det.Reintdevidenciasumi;
                itemDetalle.Reintdobservacionresp = det.Reintdobservacionresp;
            }

            return itemDetalle;
        }

        public void MoverImagenDetalle(ReInterrupcionSuministroDetDTO itemDetalle, ReInterrupcionSuministroDetDTO det1, int idDet, string path)
        {
            if (det1 != null)
            {
                if (!string.IsNullOrEmpty(det1.Reintdevidenciasumi) && !string.IsNullOrEmpty(itemDetalle.Reintdevidenciasumi))
                {
                    string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, det1.Reintdcodi, det1.Reintdevidenciasumi);
                    string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDet, itemDetalle.Reintdevidenciasumi);

                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                    {
                        FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                    }
                }
            }
        }

        /// <summary>
        /// Permite grabar una interrupcion y su detalle
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private int GrabarInterrupcion(ReInterrupcionSuministroDTO entity, ReInterrupcionSuministroDetDTO det1,
            ReInterrupcionSuministroDetDTO det2, ReInterrupcionSuministroDetDTO det3, ReInterrupcionSuministroDetDTO det4,
            ReInterrupcionSuministroDetDTO det5, string path)
        {
            try
            {
                int id = FactorySic.GetReInterrupcionSuministroRepository().Save(entity);

                for (int i = 1; i <= 5; i++)
                {
                    ReInterrupcionSuministroDetDTO itemDetalle = new ReInterrupcionSuministroDetDTO();
                    itemDetalle.Reintcodi = id;
                    itemDetalle.Reintdestado = ConstantesAppServicio.Activo;
                    itemDetalle.Reintdorden = i;

                    if (i == 1)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi1;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje1;
                        //- aqui debe ser la comparacion
                        itemDetalle = this.ObtenerDetalleInterrupcion(itemDetalle, det1);

                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);

                        this.MoverImagenDetalle(itemDetalle, det1, idDet, path);


                    }
                    if (i == 2 && entity.Emprcodi2 != null && entity.Porcentaje2 != null)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi2;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje2;
                        itemDetalle = this.ObtenerDetalleInterrupcion(itemDetalle, det2);
                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);
                        this.MoverImagenDetalle(itemDetalle, det2, idDet, path);

                    }
                    if (i == 3 && entity.Emprcodi3 != null && entity.Porcentaje3 != null)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi3;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje3;
                        itemDetalle = this.ObtenerDetalleInterrupcion(itemDetalle, det3);
                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);
                        this.MoverImagenDetalle(itemDetalle, det3, idDet, path);
                    }
                    if (i == 4 && entity.Emprcodi4 != null && entity.Porcentaje4 != null)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi4;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje4;
                        itemDetalle = this.ObtenerDetalleInterrupcion(itemDetalle, det4);
                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);
                        this.MoverImagenDetalle(itemDetalle, det4, idDet, path);
                    }
                    if (i == 5 && entity.Emprcodi5 != null && entity.Porcentaje5 != null)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi5;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje5;
                        itemDetalle = this.ObtenerDetalleInterrupcion(itemDetalle, det5);
                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);
                        this.MoverImagenDetalle(itemDetalle, det5, idDet, path);
                    }
                }

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite realizar el copiado de las interrupciones
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idPeriodoTrimestral"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int CopiarInterrupciones(int idPeriodo, int idPeriodoTrimestral, string username)
        {
            try
            {
                RePeriodoDTO periodo = FactorySic.GetRePeriodoRepository().GetById(idPeriodo);
                RePeriodoDTO periodoTrimestral = FactorySic.GetRePeriodoRepository().GetById(idPeriodoTrimestral);
                List<InterrupcionSuministroPE> listaRegistrosPE_Semestral = ObtenerListaInterrupcionesPEPorPeriodo(periodo, false, null);
                List<InterrupcionSuministroPE> listaRegistrosPE_Trimestral = ObtenerListaInterrupcionesPEPorPeriodo(periodoTrimestral, false, null);
                string path = this.ObtenerPathArchivosResarcimiento();
                List<InterrupcionSuministroPE> lstSalida = new List<InterrupcionSuministroPE>();

                foreach (var interrupcion1 in listaRegistrosPE_Trimestral)
                {
                    string cliente = interrupcion1.NombreCliente.Trim();
                    string ptoEntrega = interrupcion1.PuntoEntregaBarraNombre.Trim();
                    string nroSuministro = interrupcion1.NumSuministroClienteLibre;
                    string nivelT = interrupcion1.NivelTensionNombre.Trim();
                    string tipo = interrupcion1.TipoNombre.Trim();
                    string causa = interrupcion1.CausaNombre.Trim();
                    int? aplicacionLit = interrupcion1.AplicacionLiteral;
                    string incrementoToler = interrupcion1.IncrementoTolerancia.Trim();
                    string ejecFechaIni = interrupcion1.TiempoEjecutadoIni.Trim();
                    string ejecFechaFin = interrupcion1.TiempoEjecutadoFin.Trim();
                    string progFechaIni = interrupcion1.TiempoProgramadoIni.Trim();
                    string progFechaFin = interrupcion1.TiempoProgramadoFin.Trim();
                    string resp1Nomb = interrupcion1.Responsable1Nombre.Trim();
                    decimal? resp1Porcen = interrupcion1.Responsable1Porcentaje;
                    string resp2Nomb = interrupcion1.Responsable2Nombre.Trim();
                    decimal? resp2Porcen = interrupcion1.Responsable2Porcentaje;
                    string resp3Nomb = interrupcion1.Responsable3Nombre.Trim();
                    decimal? resp3Porcen = interrupcion1.Responsable3Porcentaje;
                    string resp4Nomb = interrupcion1.Responsable4Nombre.Trim();
                    decimal? resp4Porcen = interrupcion1.Responsable4Porcentaje;
                    string resp5Nomb = interrupcion1.Responsable5Nombre.Trim();
                    decimal? resp5Porcen = interrupcion1.Responsable5Porcentaje;

                    List<InterrupcionSuministroPE> listaIgualesEnPeriodo2 = listaRegistrosPE_Semestral.Where(x => x.NombreCliente == cliente && x.PuntoEntregaBarraNombre == ptoEntrega &&
                    x.NumSuministroClienteLibre == nroSuministro && x.NivelTensionNombre == nivelT && x.TipoNombre == tipo && x.CausaNombre == causa &&
                    x.AplicacionLiteral == aplicacionLit && x.IncrementoTolerancia == incrementoToler && x.TiempoEjecutadoIni == ejecFechaIni &&
                    x.TiempoEjecutadoFin == ejecFechaFin && x.TiempoProgramadoIni == progFechaIni && x.TiempoProgramadoFin == progFechaFin &&
                    x.Responsable1Nombre == resp1Nomb && x.Responsable1Porcentaje == resp1Porcen && x.Responsable2Nombre == resp2Nomb &&
                    x.Responsable2Porcentaje == resp2Porcen && x.Responsable3Nombre == resp3Nomb && x.Responsable3Porcentaje == resp3Porcen &&
                    x.Responsable4Nombre == resp4Nomb && x.Responsable4Porcentaje == resp4Porcen && x.Responsable5Nombre == resp5Nomb &&
                    x.Responsable5Porcentaje == resp5Porcen).ToList();

                    //Si el trimestral existe en el semestral
                    if (listaIgualesEnPeriodo2.Count() == 1)
                    {
                        int idInterrupcionTrimestral = interrupcion1.InterrupcionId;
                        int idInterrupcionSemestral = listaIgualesEnPeriodo2[0].InterrupcionId;

                        //- Copiamos las observaciones y respuestas
                        FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarDesdeTrimestral(idInterrupcionSemestral, idInterrupcionTrimestral);

                        List<ReInterrupcionSuministroDetDTO> entitys = FactorySic.GetReInterrupcionSuministroDetRepository().ObtenerRegistrosConSustento(idInterrupcionSemestral, idInterrupcionTrimestral);
                        List<ReInterrupcionSuministroDetDTO> entitysTrimestral = entitys.Where(x => x.Reintcodi == idInterrupcionTrimestral).ToList();
                        List<ReInterrupcionSuministroDetDTO> entitysSemestral = entitys.Where(x => x.Reintcodi == idInterrupcionSemestral).ToList();

                        foreach (ReInterrupcionSuministroDetDTO entityTrimestral in entitysTrimestral)
                        {
                            ReInterrupcionSuministroDetDTO entitySemestral = entitysSemestral.Where(x => x.Emprcodi == entityTrimestral.Emprcodi).FirstOrDefault();

                            if (!string.IsNullOrEmpty(entityTrimestral.Reintdevidenciaresp))
                            {
                                string fileResponsable = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, entityTrimestral.Reintdcodi, entityTrimestral.Reintdevidenciaresp);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileResponsable,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    string newFileResponsable = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, entitySemestral.Reintdcodi, entitySemestral.Reintdevidenciaresp);

                                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, newFileResponsable,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                    {
                                        FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos + newFileResponsable,
                                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                    }
                                    FileServer.CopiarFileDirectory(ConstantesCalculoResarcimiento.RutaResarcimientos, fileResponsable, newFileResponsable,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                                }
                                /*

                                if (File.Exists(fileResponsable))
                                {
                                    string newFileResponsable = path + string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, entitySemestral.Reintdcodi, entitySemestral.Reintdevidenciaresp);

                                    if (File.Exists(newFileResponsable))
                                    {
                                        File.Delete(newFileResponsable);
                                    }
                                    File.Copy(fileResponsable, newFileResponsable);
                                }*/
                            }

                            if (!string.IsNullOrEmpty(entityTrimestral.Reintdevidenciasumi))
                            {
                                string fileSuministrador = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, entityTrimestral.Reintdcodi, entityTrimestral.Reintdevidenciasumi);
                                /*if (File.Exists(fileSuministrador))
                                {
                                    string newFileSuministrador = path + string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, entitySemestral.Reintdcodi, entitySemestral.Reintdevidenciasumi);

                                    if (File.Exists(newFileSuministrador))
                                    {
                                        File.Delete(newFileSuministrador);
                                    }
                                    File.Copy(fileSuministrador, newFileSuministrador);
                                }*/
                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileSuministrador,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    string newFileSuministrador = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, entitySemestral.Reintdcodi, entitySemestral.Reintdevidenciasumi);

                                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, newFileSuministrador,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                    {
                                        FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos + newFileSuministrador,
                                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                    }
                                    FileServer.CopiarFileDirectory(ConstantesCalculoResarcimiento.RutaResarcimientos, fileSuministrador, newFileSuministrador,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                            }
                        }
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        #endregion

        #region Declaracion de Interrupciones

        /// <summary>
        /// Guarda las declaraciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="valor"></param>
        /// <param name="usuario"></param>
        public void EnviarDatosDeclaracion(int empresa, int periodo, string valor, string usuario)
        {

            ReEnvioDTO envio = new ReEnvioDTO();

            envio.Repercodi = periodo;
            envio.Reenvtipo = "D";
            envio.Emprcodi = empresa;
            envio.Reenvfecha = DateTime.Now;
            envio.Reenvestado = "A";
            envio.Reenvindicador = valor;
            envio.Reenvusucreacion = usuario;
            envio.Reenvfeccreacion = DateTime.Now;

            SaveReEnvio(envio);
        }

        /// <summary>
        /// Lista los envios para una empresa y periodo
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<ReEnvioDTO> ListarEnvios(int empresa, int periodo, string tipo)
        {
            List<ReEnvioDTO> lstSalida = new List<ReEnvioDTO>();
            List<ReEnvioDTO> lstSalidaTemp = new List<ReEnvioDTO>();

            lstSalidaTemp = FactorySic.GetReEnvioRepository().GetByPeriodoYEmpresa(empresa, periodo, tipo).OrderByDescending(x => x.Reenvcodi).ToList();

            if (lstSalidaTemp.Any())
                lstSalida = FormatearEnvio(lstSalidaTemp);

            return lstSalida;
        }

        /// <summary>
        /// Da formato a los envios de declaración
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public List<ReEnvioDTO> FormatearEnvio(List<ReEnvioDTO> lista)
        {
            List<ReEnvioDTO> lstSalida = new List<ReEnvioDTO>();

            foreach (var item in lista)
            {
                item.ReenvfechaDesc = item.Reenvfecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                item.ReenvindicadorDesc = item.Reenvindicador == "S" ? "Si" : (item.Reenvindicador == "N" ? "No" : "");
                lstSalida.Add(item);
            }
            return lstSalida;
        }


        #endregion

        #region Rechazo de Carga

        /// <summary>
        /// Permite obtener el formato de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public EstructuraRechazoCarga ObtenerEstructuraRechazoCarga(int idEmpresa, int idPeriodo, bool flag)
        {
            bool conDatos;

            EstructuraRechazoCarga result = new EstructuraRechazoCarga();
            result.Data = this.ObtenerDataRechazoCarga(idEmpresa, idPeriodo, flag, out conDatos);
            result.Result = 1;
            result.ListaCliente = this.ObtenerEmpresas();
            int idPeriodoPadre = this.ObtenerIdPeriodoPadre(idPeriodo);
            result.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
            result.ListaEvento = this.ObtenerEventosPorPeriodo(idPeriodoPadre);
            result.PlazoEnvio = this.ValidarPlazoEtapa(idPeriodo, 4);
            result.ConDatos = conDatos;

            return result;
        }


        /// <summary>
        /// Permite obtener la data de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public string[][] ObtenerDataRechazoCarga(int idEmpresa, int idPeriodo, bool flag, out bool conDatos)
        {
            List<string[]> result = new List<string[]>();
            List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().
                ObtenerPorEmpresaPeriodo(idEmpresa, idPeriodo);

            //Si tiene datos previamente guardados
            conDatos = false;
            if (entitys.Any())
            {
                string acceso = this.ValidarHabilitacionCargaDatosParaEmpresaPeriodo(idPeriodo, idEmpresa,
                    ConstantesCalculoResarcimiento.HabilitacionIngresoDatosRechazoCarga);
                if (acceso != string.Empty)
                {
                    conDatos = true;
                }
            }

            foreach (ReRechazoCargaDTO entity in entitys)
            {
                string[] data = new string[16];
                data[0] = entity.Rerccodi.ToString(); //- Id
                data[1] = string.Empty;                //- Eliminar
                data[2] = entity.Rerccorrelativo.ToString(); //- Correlativo
                data[3] = entity.Rerctipcliente; //- Tipo de cliente
                data[4] = entity.Rerccliente.ToString(); //- Cliente
                data[5] = (entity.Rerctipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado) ?
                    entity.Repentcodi.ToString() : entity.Rercptoentrega; //- Punto de entrega
                data[6] = entity.Rercalimentadorsed; //- Alimentador SED
                data[7] = entity.Rercenst.ToString(); //- Enst
                data[8] = entity.Reevecodi.ToString(); //- Evento COES
                data[9] = (entity.Rerccomentario != null) ? entity.Rerccomentario : string.Empty; //- Comentario              
                data[10] = ((DateTime)entity.Rerctejecinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado inicio
                data[11] = ((DateTime)entity.Rerctejecfin).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado fin
                data[12] = entity.Rercpk.ToString(); //- Pk
                data[13] = entity.Rerccompensable; //- Compensable
                data[14] = entity.Rercens.ToString(); //- ENS
                data[15] = entity.Rercresarcimiento.ToString(); //- Resarcimiento

                result.Add(data);
            }

            if (entitys.Count == 0 && flag)
            {
                for (int i = 0; i <= 20; i++)
                {
                    string[] data = new string[16];
                    for (int j = 0; j < 16; j++)
                    {
                        data[j] = string.Empty;
                    }
                    result.Add(data);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Permite generar el formato de carga de rechazo de carga
        /// </summary>
        /// <param name="path"></param>
        /// <param name="template"></param>
        /// <param name="file"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public int GenerarFormatoRechazoCarga(string path, string plantilla, string file, int idEmpresa, int idPeriodo)
        {
            try
            {
                EstructuraRechazoCarga result = this.ObtenerEstructuraRechazoCarga(idEmpresa, idPeriodo, false);

                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet wsData = xlPackage.Workbook.Worksheets["Data"];
                    ExcelWorksheet wsRechazoCarga = xlPackage.Workbook.Worksheets["RechazoCarga"];

                    int index = 2;
                    foreach (ReEmpresaDTO item in result.ListaCliente)
                    {
                        wsData.Cells[index, 2].Value = item.Emprnomb;
                        index++;
                    }

                    index = 2;
                    foreach (RePtoentregaPeriodoDTO item in result.ListaPuntoEntrega)
                    {
                        wsData.Cells[index, 3].Value = item.Repentnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReEventoPeriodoDTO item in result.ListaEvento)
                    {
                        wsData.Cells[index, 4].Value = item.Reevedescripcion;
                        index++;
                    }

                    index = 3;
                    foreach (string[] data in result.Data)
                    {
                        wsRechazoCarga.Cells[index, 1].Value = data[0]; //- Id
                        wsRechazoCarga.Cells[index, 2].Value = data[2]; //- Correlativo
                        wsRechazoCarga.Cells[index, 3].Value = (data[3] == ConstantesCalculoResarcimiento.TipoClienteLibre) ?
                            ConstantesCalculoResarcimiento.TextoClienteLibre : ConstantesCalculoResarcimiento.TextoClienteRegulado; //- Tipo de cliente
                        wsRechazoCarga.Cells[index, 4].Value = result.ListaCliente.Where(x => x.Emprcodi == int.Parse(data[4])).First().Emprnomb; //- Cliente
                        wsRechazoCarga.Cells[index, 5].Value = (data[3] == ConstantesCalculoResarcimiento.TipoClienteRegulado) ?
                            result.ListaPuntoEntrega.Where(x => x.Repentcodi == int.Parse(data[5])).First().Repentnombre : data[5]; //- Punto de entrega
                        wsRechazoCarga.Cells[index, 6].Value = data[6]; //- Alimentador SED
                        wsRechazoCarga.Cells[index, 7].Value = data[7]; //- ENTS       
                        wsRechazoCarga.Cells[index, 8].Value = result.ListaEvento.Where(x => x.Reevecodi == int.Parse(data[8])).First().Reevedescripcion; //- Evento COES
                        wsRechazoCarga.Cells[index, 9].Value = data[9]; //- Comentario
                        wsRechazoCarga.Cells[index, 10].Value = data[10]; //- Tiempo inicio ejecutado
                        wsRechazoCarga.Cells[index, 11].Value = data[11]; //- Tiempo fin ejecutado
                        wsRechazoCarga.Cells[index, 12].Value = data[12]; //- Pk
                        wsRechazoCarga.Cells[index, 13].Value = (data[13] == ConstantesAppServicio.SI) ?
                            ConstantesCalculoResarcimiento.TextoSi : ConstantesCalculoResarcimiento.TextoNo; //- Compensable
                        wsRechazoCarga.Cells[index, 14].Value = data[14]; //- ENS
                        wsRechazoCarga.Cells[index, 15].Value = data[15]; //- Resarcimiento

                        index++;
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Carga los datos de rechazos de carga a partir de excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="validaciones"></param>
        /// <returns></returns>
        public string[][] CargarRechazoCargaExcel(string path, string file, int empresa, int periodo, out List<string> validaciones)
        {
            List<string[]> result = new List<string[]>();
            FileInfo fileInfo = new FileInfo(path + file);
            List<string> errores = new List<string>();

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet wsRechazoCarga = xlPackage.Workbook.Worksheets["RechazoCarga"];

                if (wsRechazoCarga != null)
                {
                    //-- Verificar columnas 
                    List<string> columnas = UtilCalculoResarcimiento.ObtenerEstructuraPlantillaRechazoCarga(path);
                    bool flag = true;
                    for (int i = 1; i <= 15; i++)
                    {
                        string header = (wsRechazoCarga.Cells[1, i].Value != null) ? wsRechazoCarga.Cells[1, i].Value.ToString() : string.Empty;
                        if (header != columnas[i - 1])
                        {
                            errores.Add("La plantilla ha sido modificada. No puede cambiar o eliminar columnas del Excel.");
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        //- Verificar eliminacion de filas
                        List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().
                                     ObtenerPorEmpresaPeriodo(empresa, periodo);
                        List<int> idsTotal = entitys.Select(x => x.Rerccodi).Distinct().ToList();
                        List<int> ids = new List<int>();

                        EstructuraRechazoCarga maestros = new EstructuraRechazoCarga();
                        maestros.ListaCliente = this.ObtenerEmpresas();
                        int idPeriodoPadre = this.ObtenerIdPeriodoPadre(periodo);
                        maestros.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
                        maestros.ListaEvento = this.ObtenerEventosPorPeriodo(idPeriodoPadre);

                        for (int index = 2; index <= 500; index++)
                        {
                            if (wsRechazoCarga.Cells[index, 5].Value != null && wsRechazoCarga.Cells[index, 5].Value != string.Empty)
                            {
                                string[] data = new string[16];

                                if (wsRechazoCarga.Cells[index, 1].Value != null && wsRechazoCarga.Cells[index, 1].Value != string.Empty)
                                {
                                    ids.Add(int.Parse(wsRechazoCarga.Cells[index, 1].Value.ToString()));
                                }

                                data[0] = (wsRechazoCarga.Cells[index, 1].Value != null) ? wsRechazoCarga.Cells[index, 1].Value.ToString() : string.Empty; //- Id

                                ReRechazoCargaDTO entity = null;
                                if (!string.IsNullOrEmpty(data[0]))
                                {
                                    int valorId = 0;
                                    if (int.TryParse(data[0], out valorId))
                                    {
                                        entity = entitys.Where(x => x.Rerccodi == int.Parse(data[0])).FirstOrDefault();
                                    }
                                }

                                data[1] = string.Empty;
                                data[2] = (wsRechazoCarga.Cells[index, 2].Value != null) ? wsRechazoCarga.Cells[index, 2].Value.ToString() : string.Empty; //- Correlativo

                                string tipoCliente = string.Empty;
                                if (wsRechazoCarga.Cells[index, 3].Value != null)
                                {
                                    tipoCliente = (wsRechazoCarga.Cells[index, 3].Value.ToString() == ConstantesCalculoResarcimiento.TextoClienteRegulado) ?
                                             ConstantesCalculoResarcimiento.TipoClienteRegulado : ConstantesCalculoResarcimiento.TipoClienteLibre;
                                }
                                data[3] = tipoCliente; //- Tipo de cliente

                                string cliente = string.Empty;
                                if (wsRechazoCarga.Cells[index, 4].Value != null)
                                {
                                    ReEmpresaDTO reCliente = maestros.ListaCliente.Where(x => x.Emprnomb == wsRechazoCarga.Cells[index, 4].Value.ToString()).FirstOrDefault();
                                    if (reCliente != null) cliente = reCliente.Emprcodi.ToString();
                                }
                                data[4] = cliente; //- Cliente

                                string ptoEntrega = string.Empty;
                                if (wsRechazoCarga.Cells[index, 5].Value != null)
                                {
                                    if (tipoCliente == ConstantesCalculoResarcimiento.TipoClienteRegulado)
                                    {
                                        RePtoentregaPeriodoDTO rePtoEntrega = maestros.ListaPuntoEntrega.Where(x => x.Repentnombre == wsRechazoCarga.Cells[index, 5].Value.ToString()).FirstOrDefault();
                                        if (rePtoEntrega != null) ptoEntrega = rePtoEntrega.Repentcodi.ToString();
                                    }
                                    else
                                    {
                                        ptoEntrega = wsRechazoCarga.Cells[index, 5].Value.ToString();
                                    }
                                }

                                data[5] = ptoEntrega; //- Punto de entrega
                                data[6] = (wsRechazoCarga.Cells[index, 6].Value != null) ? wsRechazoCarga.Cells[index, 6].Value.ToString() : string.Empty; //- Alimentador SED
                                data[7] = (wsRechazoCarga.Cells[index, 7].Value != null) ? wsRechazoCarga.Cells[index, 7].Value.ToString() : string.Empty; //- ENST 

                                string evento = string.Empty;
                                if (wsRechazoCarga.Cells[index, 8].Value != null)
                                {
                                    ReEventoPeriodoDTO reEvento = maestros.ListaEvento.Where(x => x.Reevedescripcion == wsRechazoCarga.Cells[index, 8].Value.ToString()).FirstOrDefault();
                                    if (reEvento != null) evento = reEvento.Reevecodi.ToString();
                                }
                                data[8] = evento; //- Evento COES
                                data[9] = (wsRechazoCarga.Cells[index, 9].Value != null) ? wsRechazoCarga.Cells[index, 9].Value.ToString() : string.Empty; //- Comentario
                                data[10] = (wsRechazoCarga.Cells[index, 10].Value != null) ? wsRechazoCarga.Cells[index, 10].Value.ToString() : string.Empty; //- Tiempo inicio ejecutado
                                data[11] = (wsRechazoCarga.Cells[index, 11].Value != null) ? wsRechazoCarga.Cells[index, 11].Value.ToString() : string.Empty; //- Tiempo fin ejecutado
                                data[12] = (wsRechazoCarga.Cells[index, 12].Value != null) ? wsRechazoCarga.Cells[index, 12].Value.ToString() : string.Empty; //- Pk

                                string compensable = string.Empty;
                                if (wsRechazoCarga.Cells[index, 13].Value != null)
                                {
                                    compensable = (wsRechazoCarga.Cells[index, 13].Value.ToString() == ConstantesCalculoResarcimiento.TextoSi) ?
                                        ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                                }
                                data[13] = compensable; //- Compensable                                
                                data[14] = (wsRechazoCarga.Cells[index, 14].Value != null) ? wsRechazoCarga.Cells[index, 14].Value.ToString() : string.Empty; //- Ens
                                data[15] = (wsRechazoCarga.Cells[index, 15].Value != null) ? wsRechazoCarga.Cells[index, 15].Value.ToString() : string.Empty; //- Resarcimiento

                                result.Add(data);
                            }
                        }

                        if (idsTotal.Where(x => ids.Any(y => x == y)).Count() != idsTotal.Count)
                        {
                            errores.Add("Si desea editar registros, primero debe descargar la plantilla nuevamente.");
                            flag = false;
                        }
                    }
                }
                else
                {
                    errores.Add("No existe la hoja 'RechazoCarga' en el libro Excel.");
                }
            }
            validaciones = errores;
            return result.ToArray();
        }

        /// <summary>
        /// Permite grabar las interrupciones
        /// </summary>
        /// <param name="data"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="username"></param>
        /// <param name="comentario"></param>       
        /// <returns></returns>
        public EstructuraGrabado GrabarRechazosCarga(string[][] data, int empresa, int periodo, string username, string comentario, string fuente)
        {
            EstructuraGrabado result = new EstructuraGrabado();
            try
            {
                bool flagCalculo = true;
                bool flagEnergia = true;
                string[][] dataRC = this.CalcularRechazoCarga(data, periodo, out flagCalculo, out flagEnergia, false);

                if (flagCalculo && flagEnergia)
                {
                    int idPeriodoPadre = this.ObtenerIdPeriodoPadre(periodo);
                    List<ReEventoPeriodoDTO> eventos = this.ObtenerEventosPorPeriodo(idPeriodoPadre);

                    List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().
                             ObtenerPorEmpresaPeriodo(empresa, periodo);

                    List<int> idsNuevos = new List<int>();
                    List<EstructuraNotificacion> idsActualizados = new List<EstructuraNotificacion>();

                    for (int i = 2; i < data.Length; i++)
                    {
                        if (data[i][5] == string.Empty) break;

                        ReRechazoCargaDTO entity = new ReRechazoCargaDTO();
                        entity.Rerccodi = (!string.IsNullOrEmpty(data[i][0])) ? int.Parse(data[i][0]) : 0;

                        if (entity.Rerccodi > 0)
                            if (entitys.Where(x => x.Rerccodi == entity.Rerccodi).Count() == 0) entity.Rerccodi = 0;


                        entity.Repercodi = periodo;
                        entity.Rercestado = ConstantesAppServicio.Activo;
                        entity.Rercpadre = -1; //- Para padres que se visualizan
                        entity.Rercfinal = ConstantesAppServicio.SI;
                        entity.Emprcodi = empresa;
                        entity.Rerccorrelativo = int.Parse(data[i][2]);
                        entity.Rerctipcliente = data[i][3];
                        entity.Rerccliente = int.Parse(data[i][4]);
                        if (entity.Rerctipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado)
                            entity.Repentcodi = int.Parse(data[i][5]);
                        else
                            entity.Rercptoentrega = data[i][5];

                        entity.Rercalimentadorsed = data[i][6];
                        entity.Rercenst = decimal.Parse(data[i][7]);
                        entity.Reevecodi = int.Parse(data[i][8]);
                        entity.Rerccomentario = data[i][9];
                        entity.Rerctejecinicio = DateTime.ParseExact(data[i][10], ConstantesAppServicio.FormatoFechaFull2,
                           CultureInfo.InvariantCulture);
                        entity.Rerctejecfin = DateTime.ParseExact(data[i][11], ConstantesAppServicio.FormatoFechaFull2,
                            CultureInfo.InvariantCulture);
                        entity.Rercpk = decimal.Parse(data[i][12]);
                        entity.Rerccompensable = data[i][13];
                        entity.Rercens = decimal.Parse(data[i][14]);
                        entity.Rercresarcimiento = decimal.Parse(data[i][15]);
                        entity.Rercusucreacion = username;
                        entity.Rercfeccreacion = DateTime.Now;

                        ReEventoPeriodoDTO evento = eventos.Where(x => x.Reevecodi == entity.Reevecodi).First();
                        entity.Rercporcentaje1 = evento.Reeveporc1;
                        entity.Rercporcentaje2 = evento.Reeveporc2;
                        entity.Rercporcentaje3 = evento.Reeveporc3;
                        entity.Rercporcentaje4 = evento.Reeveporc4;
                        entity.Rercporcentaje5 = evento.Reeveporc5;

                        if (entity.Rerccodi == 0)
                        {
                            int id = FactorySic.GetReRechazoCargaRepository().Save(entity);
                            idsNuevos.Add(id);
                        }
                        else
                        {
                            ReRechazoCargaDTO original = entitys.Where(x => x.Rerccodi == entity.Rerccodi).First();
                            bool comparar = UtilCalculoResarcimiento.CompararRegistroRechazoCarga(entity, original);
                            if (!comparar)
                            {
                                int id = FactorySic.GetReRechazoCargaRepository().Save(entity);
                                idsActualizados.Add(new EstructuraNotificacion { IdNuevo = id, IdAnterior = entity.Rerccodi });

                                original.Rercfinal = ConstantesAppServicio.NO;
                                original.Rercpadre = id;
                                FactorySic.GetReRechazoCargaRepository().Update(original);
                            }
                        }
                    }

                    ReEnvioDTO envio = new ReEnvioDTO();
                    envio.Repercodi = periodo;
                    envio.Reenvtipo = ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga;
                    envio.Emprcodi = empresa;
                    envio.Reenvfecha = DateTime.Now;
                    envio.Reenvplazo = (!string.IsNullOrEmpty(comentario)) ? ConstantesAppServicio.NO : ConstantesAppServicio.SI;
                    envio.Reenvcomentario = comentario;
                    envio.Reenvestado = ConstantesAppServicio.Activo;
                    envio.Reenvusucreacion = username;
                    envio.Reenvfeccreacion = DateTime.Now;
                    FactorySic.GetReEnvioRepository().Save(envio);

                    if (fuente == ConstantesCalculoResarcimiento.FuenteIngresoExtranet)
                    {
                        if (idsNuevos.Count > 0 || idsActualizados.Count > 0)
                        {
                            this.EnviarNotificacion(ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga,
                                ConstantesCalculoResarcimiento.TipoNotificacionNuevoActualizacion, 0, idsNuevos, idsActualizados, periodo, empresa);
                        }
                    }

                    result.Result = 1;
                }
                else
                {
                    if (flagCalculo && !flagEnergia)
                        result.Result = 2;
                    if (!flagCalculo && flagEnergia)
                        result.Result = 3;
                    if (!flagCalculo && !flagEnergia)
                        result.Result = 4;

                    result.Data = dataRC;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result.Result = -1;
            }

            return result;
        }

        /// <summary>
        /// Permite calcular los resarcimientos por rechazo de carga
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="flagCalculo"></param>
        /// <returns></returns>
        public string[][] CalcularRechazoCarga(string[][] data, int idPeriodo, out bool flagCalculo, out bool flagEnergia, bool flagAnulacion)
        {
            RePeriodoDTO entityPeriodo = FactorySic.GetRePeriodoRepository().GetById(idPeriodo);
            decimal factorE = (decimal)entityPeriodo.Reperfactorcomp;
            flagCalculo = true;
            flagEnergia = true;
            List<ReRechazoCargaDTO> entitys = new List<ReRechazoCargaDTO>();
            for (int i = 2; i < data.Length; i++)
            {
                if (data[i][5] == string.Empty) break;
                ReRechazoCargaDTO entity = new ReRechazoCargaDTO();
                entity.Rerctipcliente = data[i][3];
                entity.Rerccliente = int.Parse(data[i][4]);
                entity.Rercptoentrega = data[i][5];
                entity.Rercalimentadorsed = data[i][6];
                entity.Rercenst = decimal.Parse(data[i][7]);
                entity.Reevecodi = int.Parse(data[i][8]);
                entity.Rerctejecinicio = DateTime.ParseExact(data[i][10], ConstantesAppServicio.FormatoFechaFull2,
                   CultureInfo.InvariantCulture);
                entity.Rerctejecfin = DateTime.ParseExact(data[i][11], ConstantesAppServicio.FormatoFechaFull2,
                    CultureInfo.InvariantCulture);
                entity.Rercpk = decimal.Parse(data[i][12]);
                entity.Rerccompensable = data[i][13];
                entity.Rercens = decimal.Parse(data[i][14]);
                entity.Rercresarcimiento = decimal.Parse(data[i][15]);
                entity.Horasdiferencia = (decimal)((DateTime)entity.Rerctejecfin).Subtract((DateTime)entity.Rerctejecinicio).TotalHours; // horas

                if (flagAnulacion)
                    entity.OrdenRegistro = int.Parse(data[i][1]); //- Artificio para grabar el orden para anulacion

                entitys.Add(entity);
            }

            var groups = entitys.Select(x => new { x.Rerccliente, x.Rercptoentrega, x.Reevecodi }).Distinct().ToList();

            foreach (var group in groups)
            {
                List<ReRechazoCargaDTO> subList = entitys.Where(x => x.Rerccliente == group.Rerccliente &&
                    x.Rercptoentrega == group.Rercptoentrega && x.Reevecodi == group.Reevecodi).ToList();

                double energia = (double)subList[0].Rercenst;
                double pkdk = 0;
                double dk = 0;
                double pk;
                int index = 0;
                bool flagPk = false;
                foreach (ReRechazoCargaDTO item in subList)
                {
                    dk = (double)item.Horasdiferencia;
                    pk = (double)item.Rercpk;
                    pkdk = pkdk + dk * pk;

                    if (pk != 0) flagPk = true;

                    if (index > 0)
                    {
                        if (energia != (double)item.Rercenst)
                        {
                            flagEnergia = false;
                            item.Indicadorenergia = ConstantesAppServicio.SI;
                        }
                    }

                    index++;
                }

                if (!flagPk)
                {
                    foreach (ReRechazoCargaDTO item in subList)
                    {
                        dk = (double)item.Horasdiferencia;
                        pk = (double)((double)1 / Math.Pow(10, 100));
                        pkdk = pkdk + dk * pk;
                        item.ValorPkModificado = pk;
                    }
                }

                double ens = 0;
                foreach (ReRechazoCargaDTO item in subList)
                {
                    dk = (double)item.Horasdiferencia;
                    pk = (flagPk) ? (double)item.Rercpk : item.ValorPkModificado;
                    ens = dk * pk * energia / pkdk;
                    item.Enscalculada = (decimal)ens;
                }
            }

            //- Obtenemos los alimentadores distintos
            List<string> alimentadores = entitys.Select(x => x.Rercalimentadorsed).Distinct().ToList();

            foreach (string alimentador in alimentadores)
            {
                List<ReRechazoCargaDTO> detalle = entitys.Where(x => x.Rercalimentadorsed == alimentador).ToList();

                int n = 0;
                decimal d = 0M;
                foreach (ReRechazoCargaDTO item in detalle)
                {
                    if (item.Rerccompensable == ConstantesAppServicio.SI && item.Horasdiferencia * 60 >= 3)
                    {
                        n = n + 1;
                        d = d + item.Horasdiferencia;
                    }
                }
                decimal ef = 0M;
                if (n <= 2) ef = 1;
                else
                {
                    ef = 1M + ((decimal)n - 2M) / 4M;
                    if (d > 0.15M)
                    {
                        ef = ef + (d - 0.15M) / 0.15M;
                    }
                }
                ef = Math.Round(ef, 2);

                foreach (ReRechazoCargaDTO item in detalle)
                {
                    if (item.Rerccompensable == ConstantesAppServicio.SI && item.Enscalculada > 0 && item.Horasdiferencia * 60 >= 3)
                    {
                        item.Resarcimientocalculado = factorE * item.Enscalculada * ef;
                    }
                    else
                    {
                        item.Resarcimientocalculado = 0;
                    }

                    if ((decimal)Math.Abs((decimal)item.Rercresarcimiento - item.Resarcimientocalculado) > 0.01M)
                    {
                        item.Diferenciacalculo = ConstantesAppServicio.SI;
                        flagCalculo = false;
                    }
                }
            }

            int orden = 0;
            string[][] result = new string[entitys.Count][];
            foreach (ReRechazoCargaDTO item in entitys)
            {
                string[] itemData = new string[5];
                itemData[0] = orden.ToString();
                itemData[1] = Math.Round((decimal)item.Rercresarcimiento, 4).ToString();
                itemData[2] = Math.Round(item.Resarcimientocalculado, 4).ToString();
                
                if (!flagAnulacion)
                    itemData[3] = item.Diferenciacalculo;
                else
                {
                    itemData[3] = item.OrdenRegistro.ToString(); // artificio para pasar el orden
                }

                itemData[4] = item.Indicadorenergia;
                result[orden] = itemData;
                orden++;
            }
            return result;
        }

        /// <summary>
        /// Permite anular una interrupcion
        /// </summary>
        /// <param name="idInterrupcion"></param>
        /// <param name="comentario"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int AnularRechazoCarga(int idPeriodo, int idInterrupcion, string comentario, string username, string fuente,
            string recalculo, string[][] data)
        {
            try
            {
                string plazo = this.ValidarPlazoEtapa(idPeriodo, 7);
                if (plazo != ConstantesAppServicio.SI)
                {
                    FactorySic.GetReRechazoCargaRepository().AnularRechazoCarga(idInterrupcion, comentario, username);

                    if (recalculo == ConstantesAppServicio.SI)
                    {
                        foreach (string[] item in data)
                        {
                            int id = int.Parse(item[0]);                           
                            decimal resarcimiento = decimal.Parse(item[1]);

                            FactorySic.GetReRechazoCargaRepository().ActualizarResarcimiento(id, resarcimiento);
                        }
                    }

                    if (fuente == ConstantesCalculoResarcimiento.FuenteIngresoExtranet)
                    {
                        this.EnviarNotificacion(ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga,
                            ConstantesCalculoResarcimiento.TipoNotificacionEliminacion, idInterrupcion, new List<int>(), new List<EstructuraNotificacion>(),
                            idPeriodo, 0);
                    }

                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }


        #endregion

        #region Observaciones

        /// <summary>
        /// Permite obtener el formato de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public EstructuraInterrupcion ObtenerEstructuraObservaciones(int idEmpresa, int idPeriodo, bool flag)
        {
            EstructuraInterrupcion result = new EstructuraInterrupcion();
            result.Data = this.ObtenerDataObservaciones(idEmpresa, idPeriodo, flag);
            result.Result = 1;
            result.PlazoEnvio = this.ValidarPlazoEtapa(idPeriodo, 2);
            result.PlazoObservacion = this.ValidarPlazoEtapa(idPeriodo, 1);
            result.ListaTiposObservacion = UtilCalculoResarcimiento.ListarTiposObservacion();

            return result;
        }

        /// <summary>
        /// Permite obtener la data de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public string[][] ObtenerDataObservaciones(int idEmpresa, int idPeriodo, bool flag)
        {
            List<string[]> result = new List<string[]>();

            //- Aca debemos cambiar porque las interrupciones son distintas
            EstructuraInterrupcion maestros = new EstructuraInterrupcion();
            maestros.ListaCliente = this.ObtenerEmpresas();
            maestros.ListaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
            maestros.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().List();
            maestros.ListaNivelTension = FactorySic.GetReNivelTensionRepository().List();
            maestros.ListaEmpresa = this.ObtenerEmpresas();
            int idPeriodoPadre = this.ObtenerIdPeriodoPadre(idPeriodo);
            maestros.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);

            List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                ObtenerInterrupcionesPorResponsable(idEmpresa, idPeriodo);
            List<ReInterrupcionSuministroDetDTO> detalle = FactorySic.GetReInterrupcionSuministroDetRepository().
                ObtenerInterrupcionesPorResponsable(idEmpresa, idPeriodo);

            foreach (ReInterrupcionSuministroDTO entity in entitys)
            {
                List<ReInterrupcionSuministroDetDTO> subList = detalle.
                    Where(x => x.Reintcodi == entity.Reintcodi).OrderBy(x => x.Reintdorden).ToList();
                ReInterrupcionSuministroDetDTO det1 = subList.Where(x => x.Reintdorden == 1).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det2 = subList.Where(x => x.Reintdorden == 2).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det3 = subList.Where(x => x.Reintdorden == 3).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det4 = subList.Where(x => x.Reintdorden == 4).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det5 = subList.Where(x => x.Reintdorden == 5).FirstOrDefault();

                string[] data = new string[38];
                data[0] = entity.Reintcodi.ToString() + ConstantesAppServicio.CaracterGuion + entity.Emprresponsable.ToString() +
                    ConstantesAppServicio.CaracterGuion + entity.Reintreftrimestral; //- Id interrupcion y detalle
                data[1] = entity.Emprnomb;                //- Suministador
                data[2] = entity.Reintcorrelativo.ToString(); //- Correlativo
                data[3] = (entity.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado) ?
                                             ConstantesCalculoResarcimiento.TextoClienteRegulado : ConstantesCalculoResarcimiento.TextoClienteLibre; ; //- Tipo de cliente

                if (maestros.ListaCliente.Where(x => x.Emprcodi == entity.Reintcliente).Any())
                {
                    data[4] = maestros.ListaCliente.Where(x => x.Emprcodi == entity.Reintcliente).First().Emprnomb; //- Cliente 
                }
                else
                {
                    data[4] = (new EmpresaAppServicio()).ObtenerEmpresa((int)entity.Reintcliente).Emprnomb + " (EN BAJA)";
                }


                data[5] = (entity.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado) ?
                        maestros.ListaPuntoEntrega.Where(x => x.Repentcodi == entity.Repentcodi).First().Repentnombre : entity.Reintptoentrega; //- Punto de entrega
                data[6] = (entity.Reintnrosuministro != null) ? entity.Reintnrosuministro.ToString() : string.Empty; //- Nro suministro
                data[7] = maestros.ListaNivelTension.Where(x => x.Rentcodi == entity.Rentcodi).First().Rentabrev; //- Nivel de tension
                data[8] = entity.Reintaplicacionnumeral.ToString(); //- Aplicacion literal
                data[9] = (entity.Reintenergiasemestral != null) ? entity.Reintenergiasemestral.ToString() : string.Empty; //- Energia semestral
                data[10] = (entity.Reintinctolerancia == ConstantesAppServicio.SI) ? ConstantesCalculoResarcimiento.TextoSi : ConstantesCalculoResarcimiento.TextoNo; //- Incremento de tolerancias
                data[11] = maestros.ListaTipoInterrupcion.Where(x => x.Retintcodi == entity.Retintcodi).First().Retintnombre; //- Tipo de interrupcion
                data[12] = maestros.ListaCausaInterrupcion.Where(x => x.Recintcodi == entity.Recintcodi).First().Recintnombre; //- Causa de interrupcion
                data[13] = entity.Reintni.ToString(); //- Indicador NI
                data[14] = entity.Reintki.ToString(); //- Indicador KI
                data[15] = ((DateTime)entity.Reintfejeinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado inicio
                data[16] = ((DateTime)entity.Reintfejefin).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado fin
                data[17] = (entity.Reintfproginicio != null) ?
                    ((DateTime)entity.Reintfproginicio).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado inicio
                data[18] = (entity.Reintfprogfin != null) ?
                    ((DateTime)entity.Reintfprogfin).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado fin

                //data[19] = maestros.ListaEmpresa.Where(x => x.Emprcodi == det1.Emprcodi).First().Emprnomb; //- Empresa 1

                if (maestros.ListaEmpresa.Where(x => x.Emprcodi == det1.Emprcodi).Any())
                {
                    data[19] = maestros.ListaEmpresa.Where(x => x.Emprcodi == det1.Emprcodi).First().Emprnomb; //- Empresa 1 
                }
                else
                {
                    data[19] = (new EmpresaAppServicio()).ObtenerEmpresa((int)det1.Emprcodi).Emprnomb + " (EN BAJA)"; //- Empresa 1
                }


                data[20] = det1.Reintdorcentaje.ToString();

                if (det2 != null)
                {
                    //data[21] = maestros.ListaEmpresa.Where(x => x.Emprcodi == det2.Emprcodi).First().Emprnomb;

                    if (maestros.ListaEmpresa.Where(x => x.Emprcodi == det2.Emprcodi).Any())
                    {
                        data[21] = maestros.ListaEmpresa.Where(x => x.Emprcodi == det2.Emprcodi).First().Emprnomb; //- Empresa 1 
                    }
                    else
                    {
                        data[21] = (new EmpresaAppServicio()).ObtenerEmpresa((int)det2.Emprcodi).Emprnomb + " (EN BAJA)"; //- Empresa 1
                    }
                }
                else
                {
                    data[21] = string.Empty;
                }

                data[22] = (det2 != null) ? det2.Reintdorcentaje.ToString() : string.Empty;

                if (det3 != null)
                {
                    //data[23] = (det3 != null) ? maestros.ListaEmpresa.Where(x => x.Emprcodi == det3.Emprcodi).First().Emprnomb : string.Empty; //- Empresa 3

                    if (maestros.ListaEmpresa.Where(x => x.Emprcodi == det3.Emprcodi).Any())
                    {
                        data[23] = maestros.ListaEmpresa.Where(x => x.Emprcodi == det3.Emprcodi).First().Emprnomb; //- Empresa 1 
                    }
                    else
                    {
                        data[23] = (new EmpresaAppServicio()).ObtenerEmpresa((int)det3.Emprcodi).Emprnomb + " (EN BAJA)"; //- Empresa 1
                    }
                }
                else
                {
                    data[23] = string.Empty;
                }

                data[24] = (det3 != null) ? det3.Reintdorcentaje.ToString() : string.Empty;

                if (det4 != null)
                {
                    //data[25] = (det4 != null) ? maestros.ListaEmpresa.Where(x => x.Emprcodi == det4.Emprcodi).First().Emprnomb : string.Empty; //- Empresa 4

                    if (maestros.ListaEmpresa.Where(x => x.Emprcodi == det4.Emprcodi).Any())
                    {
                        data[25] = maestros.ListaEmpresa.Where(x => x.Emprcodi == det4.Emprcodi).First().Emprnomb; //- Empresa 1 
                    }
                    else
                    {
                        data[25] = (new EmpresaAppServicio()).ObtenerEmpresa((int)det4.Emprcodi).Emprnomb + " (EN BAJA)"; //- Empresa 1
                    }
                }
                else
                {
                    data[25] = string.Empty;

                }


                data[26] = (det4 != null) ? det4.Reintdorcentaje.ToString() : string.Empty;

                if (det5 != null)
                {
                    //data[27] = (det5 != null) ? maestros.ListaEmpresa.Where(x => x.Emprcodi == det5.Emprcodi).First().Emprnomb : string.Empty; //- Empresa 5

                    if (maestros.ListaEmpresa.Where(x => x.Emprcodi == det5.Emprcodi).Any())
                    {
                        data[27] = maestros.ListaEmpresa.Where(x => x.Emprcodi == det5.Emprcodi).First().Emprnomb; //- Empresa 1 
                    }
                    else
                    {
                        data[27] = (new EmpresaAppServicio()).ObtenerEmpresa((int)det5.Emprcodi).Emprnomb + " (EN BAJA)"; //- Empresa 1
                    }
                }
                else
                {
                    data[27] = string.Empty;
                }


                data[28] = (det5 != null) ? det5.Reintdorcentaje.ToString() : string.Empty;
                data[29] = (entity.Reintcausaresumida != null) ? entity.Reintcausaresumida : string.Empty; //- Causa resumida
                data[30] = entity.Reinteie.ToString(); //- Ei/E
                data[31] = entity.Reintresarcimiento.ToString(); //- Resarcimiento
                data[32] = entity.Reintevidencia; //- Evidencia

                ReInterrupcionSuministroDetDTO entityDetalle = detalle.Where(x => x.Reintdcodi == entity.Emprresponsable).First();
                data[33] = (entityDetalle.Reintdconformidadresp != null) ? entityDetalle.Reintdconformidadresp : string.Empty; //- Conformidad responsable
                data[34] = (entityDetalle.Reintdobservacionresp != null) ? entityDetalle.Reintdobservacionresp : string.Empty; //- Observacion responsable
                data[35] = (entityDetalle.Reintddetalleresp != null) ? entityDetalle.Reintddetalleresp : string.Empty; //- Detalle campo observado
                data[36] = (entityDetalle.Reintdcomentarioresp != null) ? entityDetalle.Reintdcomentarioresp : string.Empty; // Comentarios responsable
                data[37] = (entityDetalle.Reintdevidenciaresp != null) ? entityDetalle.Reintdevidenciaresp : string.Empty; //- Evidencia responsable               


                result.Add(data);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Permite generar el formato de carga de observaciones
        /// </summary>
        /// <param name="path"></param>
        /// <param name="template"></param>
        /// <param name="file"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public int GenerarFormatoObservacion(string path, string plantilla, string file, int idEmpresa, int idPeriodo)
        {
            try
            {
                EstructuraInterrupcion result = new EstructuraInterrupcion();
                result.Data = this.ObtenerDataObservaciones(idEmpresa, idPeriodo, false);
                result.ListaTiposObservacion = UtilCalculoResarcimiento.ListarTiposObservacion();

                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];

                    int index = 5;
                    foreach (string[] data in result.Data)
                    {
                        wsInterrupcion.Cells[index, 1].Value = data[0]; //- Id
                        wsInterrupcion.Cells[index, 2].Value = data[1]; //-Suministrador
                        wsInterrupcion.Cells[index, 3].Value = data[2]; //- Correlativo
                        wsInterrupcion.Cells[index, 4].Value = data[3];//- Tipo de cliente
                        wsInterrupcion.Cells[index, 5].Value = data[4]; //- Cliente
                        wsInterrupcion.Cells[index, 6].Value = data[5]; //- Punto de entrega
                        wsInterrupcion.Cells[index, 7].Value = data[6]; //- Nro de suministro
                        wsInterrupcion.Cells[index, 8].Value = data[7]; //- Nivel de tension
                        wsInterrupcion.Cells[index, 9].Value = data[8]; //- Correlativo
                        wsInterrupcion.Cells[index, 10].Value = data[9]; //- Energia
                        wsInterrupcion.Cells[index, 11].Value = data[10]; //- Incremento de tolerancia
                        wsInterrupcion.Cells[index, 12].Value = data[11]; //- Tipo de interrupcion
                        wsInterrupcion.Cells[index, 13].Value = data[12]; //- Causa de interrupcion
                        wsInterrupcion.Cells[index, 14].Value = data[13]; //- Ni
                        wsInterrupcion.Cells[index, 15].Value = data[14]; //- Ki
                        wsInterrupcion.Cells[index, 16].Value = data[15]; //- Tiempo inicio ejecutado
                        wsInterrupcion.Cells[index, 17].Value = data[16]; //- Tiempo fin ejecutado
                        wsInterrupcion.Cells[index, 18].Value = data[17]; //- Tiempo inicio programado
                        wsInterrupcion.Cells[index, 19].Value = data[18]; //- Tiempo fin programado
                        wsInterrupcion.Cells[index, 20].Value = data[19]; //- Empresa 1
                        wsInterrupcion.Cells[index, 21].Value = data[20]; //- Porcentaje 1                       
                        wsInterrupcion.Cells[index, 22].Value = data[21]; //- Empresa 2
                        wsInterrupcion.Cells[index, 23].Value = data[22]; //- Porcentaje 2                        
                        wsInterrupcion.Cells[index, 24].Value = data[23]; //- Empresa 3
                        wsInterrupcion.Cells[index, 25].Value = data[24]; //- Porcentaje 3                       
                        wsInterrupcion.Cells[index, 26].Value = data[25]; //- Empresa 4
                        wsInterrupcion.Cells[index, 27].Value = data[26]; //- Porcentaje 4                      
                        wsInterrupcion.Cells[index, 28].Value = data[27]; //- Empresa 5
                        wsInterrupcion.Cells[index, 29].Value = data[28]; //- Porcentaje 5
                        wsInterrupcion.Cells[index, 30].Value = data[29]; //- Causa resuminda
                        wsInterrupcion.Cells[index, 31].Value = data[30]; //- Ei
                        wsInterrupcion.Cells[index, 32].Value = data[31]; //- Resarcimientos

                        if (!string.IsNullOrEmpty(data[33]))
                            wsInterrupcion.Cells[index, 33].Value = (data[33] == ConstantesAppServicio.SI) ?
                                ConstantesCalculoResarcimiento.TextoSi : ConstantesCalculoResarcimiento.TextoNo;  //- Conformidad responsable
                        if (!string.IsNullOrEmpty(data[34]))
                        {
                            List<int> idsObservaciones = data[34].Split(',').Select(int.Parse).ToList();
                            List<string> textosObservaciones = result.ListaTiposObservacion.Where(x => idsObservaciones.Any(y => int.Parse(x.Id) == y)).Select(x => x.Texto).ToList();

                            wsInterrupcion.Cells[index, 34].Value = string.Join(",", textosObservaciones); //- Observacion
                        }


                        wsInterrupcion.Cells[index, 35].Value = data[35]; //- Detalle de campo observado
                        wsInterrupcion.Cells[index, 36].Value = data[36]; //- Comentarios

                        index++;
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Carga los datos a partir de excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="validaciones"></param>
        /// <returns></returns>
        public string[][] CargarObservacionesExcel(string path, string file, int empresa, int periodo, out List<string> validaciones, out List<EstructuraCargaFile> listadoArchivos)
        {
            List<string[]> result = new List<string[]>();
            FileInfo fileInfo = new FileInfo(path + file);
            List<string> errores = new List<string>();
            List<EstructuraCargaFile> listFiles = new List<EstructuraCargaFile>();

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];

                if (wsInterrupcion != null)
                {
                    //-- Verificar columnas 
                    List<string> columnas = UtilCalculoResarcimiento.ObtenerEstructuraPlantillaObservacion(path);
                    bool flag = true;
                    for (int i = 1; i <= 36; i++)
                    {
                        string header = (wsInterrupcion.Cells[1, i].Value != null) ? wsInterrupcion.Cells[1, i].Value.ToString() : string.Empty;
                        if (header != columnas[i - 1])
                        {
                            errores.Add("La plantilla ha sido modificada. No puede cambiar o eliminar columnas del Excel.");
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        //- Verificar eliminacion de filas
                        List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                                    ObtenerInterrupcionesPorResponsable(empresa, periodo);
                        List<ReInterrupcionSuministroDetDTO> detalle = FactorySic.GetReInterrupcionSuministroDetRepository().
                                    ObtenerInterrupcionesPorResponsable(empresa, periodo);
                        List<string> idsTotal = entitys.Select(x => x.Reintcodi + ConstantesAppServicio.CaracterGuion + x.Emprresponsable.ToString()).Distinct().ToList();
                        List<string> ids = new List<string>();

                        EstructuraInterrupcion maestros = new EstructuraInterrupcion();
                        maestros.ListaTiposObservacion = UtilCalculoResarcimiento.ListarTiposObservacion();
                        bool flagPlazo = true;

                        for (int index = 5; index <= ConstantesCalculoResarcimiento.CantidadRegistrosFormato; index++)
                        {
                            if (wsInterrupcion.Cells[index, 5].Value != null && wsInterrupcion.Cells[index, 5].Value != string.Empty)
                            {
                                ReInterrupcionSuministroDetDTO itemDetalle = null;
                                string[] data = new string[6];

                                if (wsInterrupcion.Cells[index, 1].Value != null && wsInterrupcion.Cells[index, 1].Value != string.Empty)
                                {
                                    string idRegistro = wsInterrupcion.Cells[index, 1].Value.ToString();
                                    ids.Add(idRegistro);

                                    string[] parts = idRegistro.Split(ConstantesAppServicio.CaracterGuion);
                                    if (parts.Length == 3)
                                    {
                                        int valorId = 0;
                                        if (int.TryParse(parts[1], out valorId))
                                        {
                                            itemDetalle = detalle.Where(x => x.Reintdcodi == valorId).FirstOrDefault();
                                        }
                                    }
                                }

                                data[0] = (wsInterrupcion.Cells[index, 1].Value != null) ? wsInterrupcion.Cells[index, 1].Value.ToString() : string.Empty; //- Id

                                string conformidad = string.Empty;
                                if (wsInterrupcion.Cells[index, 33].Value != null)
                                {
                                    conformidad = (wsInterrupcion.Cells[index, 33].Value.ToString() == ConstantesCalculoResarcimiento.TextoSi) ?
                                        ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                                }
                                data[1] = conformidad; //- Conformidad responsable

                                string observacion = string.Empty;
                                if (wsInterrupcion.Cells[index, 34].Value != null)
                                {
                                    List<string> observaciones = wsInterrupcion.Cells[index, 34].Value.ToString().Split(',').ToList();
                                    List<int> idsObservacion = maestros.ListaTiposObservacion.Where(x => observaciones.Any(y => x.Texto == y.Trim())).Select(x => int.Parse(x.Id)).ToList();
                                    observacion = string.Join(",", idsObservacion.Select(n => n.ToString()).ToArray());
                                    /*
                                    TipoObservacion tipoObservacion = maestros.ListaTiposObservacion.Where(x => x.Texto == wsInterrupcion.Cells[index, 34].Value.ToString()).FirstOrDefault();
                                    if (tipoObservacion != null)
                                        observacion = tipoObservacion.Id;*/
                                }
                                data[2] = observacion; //- Observacion responsable
                                data[3] = (wsInterrupcion.Cells[index, 35].Value != null) ? wsInterrupcion.Cells[index, 35].Value.ToString() : string.Empty; //- Detalle campo observado
                                data[4] = (wsInterrupcion.Cells[index, 36].Value != null) ? wsInterrupcion.Cells[index, 36].Value.ToString() : string.Empty; //- Comentarios
                                data[5] = (itemDetalle != null) ? (itemDetalle.Reintdevidenciaresp != null ? itemDetalle.Reintdevidenciaresp : string.Empty) : string.Empty;

                                result.Add(data);

                                EstructuraCargaFile itemFile = new EstructuraCargaFile();
                                itemFile.Id = itemDetalle.Reintdcodi;
                                itemFile.Fila = index;
                                itemFile.FileName = (wsInterrupcion.Cells[index, 37].Value != null && wsInterrupcion.Cells[index, 37].Value != string.Empty) ?
                                    wsInterrupcion.Cells[index, 37].Value.ToString() : string.Empty;

                                listFiles.Add(itemFile);

                            }
                        }

                        if (idsTotal.Select(x => ids.Any(y => x == y)).Count() != idsTotal.Count)
                        {
                            errores.Add("Si desea editar registros, primero debe descargar la plantilla nuevamente.");
                            flag = false;
                        }

                    }
                }
                else
                {
                    errores.Add("No existe la hoja 'Interrupciones' en el libro Excel.");
                }
            }
            validaciones = errores;
            listadoArchivos = listFiles;
            return result.ToArray();
        }


        /// <summary>
        /// Permite grabar las interrupciones
        /// </summary>
        /// <param name="data"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="username"></param>
        /// <param name="comentario"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public int GrabarObservaciones(string[][] data, int empresa, int periodo, string username, string comentario)
        {
            try
            {
                List<ReInterrupcionSuministroDetDTO> actualizados = new List<ReInterrupcionSuministroDetDTO>();

                for (int i = 4; i < data.Length; i++)
                {
                    if (data[i][0] == string.Empty) break;

                    string[] parts = data[i][0].Split(ConstantesAppServicio.CaracterGuion);
                    if (parts.Length == 3)
                    {
                        int valorId = 0;
                        if (int.TryParse(parts[1], out valorId))
                        {
                            ReInterrupcionSuministroDetDTO entity = FactorySic.GetReInterrupcionSuministroDetRepository().GetById(valorId);

                            bool flagCambioObservacion = this.ValidarCambioObservacion(entity, data[i]);

                            if (flagCambioObservacion)
                            {
                                entity.Reintdconformidadresporiginal = entity.Reintdconformidadresp;
                                entity.Reintdobservacionresporiginal = entity.Reintdobservacionresp;
                                entity.Reintddetalleresporiginal = entity.Reintddetalleresp;
                                entity.Reintdcomentarioresporiginal = entity.Reintdcomentarioresp;
                                entity.Reintdevidenciaresporiginal = entity.Reintdevidenciaresp;
                            }

                            entity.Reintdconformidadresp = data[i][33];
                            entity.Reintdobservacionresp = data[i][34];
                            entity.Reintddetalleresp = data[i][35];
                            entity.Reintdcomentarioresp = data[i][36];
                            entity.Reintdevidenciaresp = data[i][37];

                            FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarObservacion(entity);

                            if (flagCambioObservacion)
                                actualizados.Add(entity);

                        }
                    }
                }

                ReEnvioDTO envio = new ReEnvioDTO();
                envio.Repercodi = periodo;
                envio.Reenvtipo = ConstantesCalculoResarcimiento.EnvioTipoObservacion;
                envio.Emprcodi = empresa;
                envio.Reenvfecha = DateTime.Now;
                envio.Reenvplazo = (!string.IsNullOrEmpty(comentario)) ? ConstantesAppServicio.NO : ConstantesAppServicio.SI;
                envio.Reenvcomentario = comentario;
                envio.Reenvestado = ConstantesAppServicio.Activo;
                envio.Reenvusucreacion = username;
                envio.Reenvfeccreacion = DateTime.Now;
                FactorySic.GetReEnvioRepository().Save(envio);

                if (actualizados.Count > 0)
                    this.EnviarNotificacionObservacion(actualizados, periodo, empresa);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        #endregion

        #region Respuesta a Observaciones

        /// <summary>
        /// Permite obtener el formato de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public EstructuraInterrupcion ObtenerEstructuraRespuestas(int idEmpresa, int idPeriodo, bool flag)
        {
            EstructuraInterrupcion result = new EstructuraInterrupcion();
            result.Data = this.ObtenerDataRespuesta(idEmpresa, idPeriodo, flag);
            result.Result = 1;
            result.ListaCliente = this.ObtenerEmpresas();
            result.ListaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
            result.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().List();
            result.ListaNivelTension = FactorySic.GetReNivelTensionRepository().List();
            result.ListaEmpresa = this.ObtenerEmpresas();
            int idPeriodoPadre = this.ObtenerIdPeriodoPadre(idPeriodo);
            result.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
            int[] rowspans = new int[result.ListaCausaInterrupcion.Count];
            result.Indicadores = this.ObtenerEstructuraIndicadores(idPeriodoPadre, out rowspans);
            result.PlazoEnvio = this.ValidarPlazoEtapa(idPeriodo, 3);
            result.PlazoEnergia = this.ValidarPlazoEtapa(idPeriodo, 5);
            result.PlazoRespuesta = this.ValidarPlazoEtapa(idPeriodo, 2);
            result.ListaTiposObservacion = UtilCalculoResarcimiento.ListarTiposObservacion();

            return result;
        }


        /// <summary>
        /// Permite obtener la data de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public string[][] ObtenerDataRespuesta(int idEmpresa, int idPeriodo, bool flag)
        {
            List<string[]> result = new List<string[]>();
            List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                ObtenerPorEmpresaPeriodo(idEmpresa, idPeriodo);
            List<ReInterrupcionSuministroDetDTO> detalle = FactorySic.GetReInterrupcionSuministroDetRepository().
                ObtenerPorEmpresaPeriodo(idEmpresa, idPeriodo);

            foreach (ReInterrupcionSuministroDTO entity in entitys)
            {
                List<ReInterrupcionSuministroDetDTO> subList = detalle.
                    Where(x => x.Reintcodi == entity.Reintcodi).OrderBy(x => x.Reintdorden).ToList();
                ReInterrupcionSuministroDetDTO det1 = subList.Where(x => x.Reintdorden == 1).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det2 = subList.Where(x => x.Reintdorden == 2).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det3 = subList.Where(x => x.Reintdorden == 3).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det4 = subList.Where(x => x.Reintdorden == 4).FirstOrDefault();
                ReInterrupcionSuministroDetDTO det5 = subList.Where(x => x.Reintdorden == 5).FirstOrDefault();

                string[] data = new string[33 + 40];
                data[0] = entity.Reintcodi.ToString() + ConstantesAppServicio.CaracterGuion + entity.Reintreftrimestral; //- Id (Haremos una jugada para el id)
                data[1] = string.Empty;                //- Eliminar
                data[2] = entity.Reintcorrelativo.ToString(); //- Correlativo
                data[3] = entity.Reinttipcliente; //- Tipo de cliente
                data[4] = entity.Reintcliente.ToString(); //- Cliente
                data[5] = (entity.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado) ?
                    entity.Repentcodi.ToString() : entity.Reintptoentrega; //- Punto de entrega
                data[6] = (entity.Reintnrosuministro != null) ? entity.Reintnrosuministro.ToString() : string.Empty; //- Nro suministro
                data[7] = entity.Rentcodi.ToString(); //- Nivel de tension
                data[8] = entity.Reintaplicacionnumeral.ToString(); //- Aplicacion literal
                data[9] = (entity.Reintenergiasemestral != null) ? ((decimal)entity.Reintenergiasemestral).ToString() : string.Empty; //- Energia semestral
                data[10] = entity.Reintinctolerancia; //- Incremento de tolerancias
                data[11] = entity.Retintcodi.ToString(); //- Tipo de interrupcion
                data[12] = entity.Recintcodi.ToString(); //- Causa de interrupcion
                data[13] = entity.Reintni.ToString(); //- Indicador NI
                data[14] = entity.Reintki.ToString(); //- Indicador KI
                data[15] = ((DateTime)entity.Reintfejeinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado inicio
                data[16] = ((DateTime)entity.Reintfejefin).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado fin
                data[17] = (entity.Reintfproginicio != null) ?
                    ((DateTime)entity.Reintfproginicio).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado inicio
                data[18] = (entity.Reintfprogfin != null) ?
                    ((DateTime)entity.Reintfprogfin).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado fin
                data[19] = det1.Emprcodi.ToString(); //- Empresa 1
                data[20] = det1.Reintdorcentaje.ToString();
                data[21] = (det2 != null) ? det2.Emprcodi.ToString() : string.Empty; //- Empresa 2
                data[22] = (det2 != null) ? det2.Reintdorcentaje.ToString() : string.Empty;
                data[23] = (det3 != null) ? det3.Emprcodi.ToString() : string.Empty; //- Empresa 3
                data[24] = (det3 != null) ? det3.Reintdorcentaje.ToString() : string.Empty;
                data[25] = (det4 != null) ? det4.Emprcodi.ToString() : string.Empty; //- Empresa 4
                data[26] = (det4 != null) ? det4.Reintdorcentaje.ToString() : string.Empty;
                data[27] = (det5 != null) ? det5.Emprcodi.ToString() : string.Empty; //- Empresa 5
                data[28] = (det5 != null) ? det5.Reintdorcentaje.ToString() : string.Empty;
                data[29] = (entity.Reintcausaresumida != null) ? entity.Reintcausaresumida : string.Empty; //- Causa resumida
                data[30] = entity.Reinteie.ToString(); //- Ei/E
                data[31] = entity.Reintresarcimiento.ToString(); //- Resarcimiento
                data[32] = entity.Reintevidencia; //- Evidencia

                string[] camposObservacion = UtilCalculoResarcimiento.ObtenerCamposObservacion(det1, det2, det3, det4, det5,
                    UtilCalculoResarcimiento.ListarTiposObservacion());
                for (int k = 0; k < camposObservacion.Length; k++)
                    data[33 + k] = camposObservacion[k];

                result.Add(data);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <param name="template"></param>
        /// <param name="file"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public int GenerarFormatoRespuestas(string path, string plantilla, string file, int idEmpresa, int idPeriodo)
        {
            try
            {
                EstructuraInterrupcion result = this.ObtenerEstructuraRespuestas(idEmpresa, idPeriodo, false);
                RePeriodoDTO periodo = FactorySic.GetRePeriodoRepository().GetById(idPeriodo);

                int idPeriodoPadre = this.ObtenerIdPeriodoPadre(idPeriodo);
                RePeriodoDTO periodoPadre = FactorySic.GetRePeriodoRepository().GetById(idPeriodoPadre);

                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + file);
                }


                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet wsData = xlPackage.Workbook.Worksheets["Data"];
                    ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];

                    int index = 2;
                    foreach (ReEmpresaDTO item in result.ListaCliente)
                    {
                        wsData.Cells[index, 2].Value = item.Emprnomb;
                        index++;
                    }

                    index = 2;
                    foreach (RePtoentregaPeriodoDTO item in result.ListaPuntoEntrega)
                    {
                        wsData.Cells[index, 3].Value = item.Repentnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReNivelTensionDTO item in result.ListaNivelTension)
                    {
                        wsData.Cells[index, 4].Value = item.Rentabrev;
                        index++;
                    }

                    index = 2;
                    foreach (ReTipoInterrupcionDTO item in result.ListaTipoInterrupcion)
                    {
                        wsData.Cells[index, 6].Value = item.Retintnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReCausaInterrupcionDTO item in result.ListaCausaInterrupcion)
                    {
                        wsData.Cells[index, 7].Value = item.Recintnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReEmpresaDTO item in result.ListaEmpresa)
                    {
                        wsData.Cells[index, 8].Value = item.Emprnomb;
                        index++;
                    }

                    index = 2;
                    wsData.Cells[index, 9].Value = ((DateTime)periodoPadre.Reperfecinicio).ToString(ConstantesAppServicio.FormatoFecha);
                    wsData.Cells[index, 10].Value = (((DateTime)periodoPadre.Reperfecfin).AddDays(1)).ToString(ConstantesAppServicio.FormatoFecha);

                    index = 5;
                    foreach (string[] data in result.Data)
                    {
                        wsInterrupcion.Cells[index, 1].Value = data[0]; //- Id
                        wsInterrupcion.Cells[index, 2].Value = data[2]; //- Correlativo
                        wsInterrupcion.Cells[index, 3].Value = (data[3] == ConstantesCalculoResarcimiento.TipoClienteLibre) ?
                            ConstantesCalculoResarcimiento.TextoClienteLibre : ConstantesCalculoResarcimiento.TextoClienteRegulado; //- Tipo de cliente
                        wsInterrupcion.Cells[index, 4].Value = result.ListaCliente.Where(x => x.Emprcodi == int.Parse(data[4])).First().Emprnomb; //- Cliente
                        wsInterrupcion.Cells[index, 5].Value = (data[3] == ConstantesCalculoResarcimiento.TipoClienteRegulado) ?
                            result.ListaPuntoEntrega.Where(x => x.Repentcodi == int.Parse(data[5])).First().Repentnombre : data[5]; //- Punto de entrega
                        wsInterrupcion.Cells[index, 6].Value = data[6]; //- Nro de suministro
                        wsInterrupcion.Cells[index, 7].Value = result.ListaNivelTension.Where(x => x.Rentcodi == int.Parse(data[7])).First().Rentabrev; //- Nivel de tension
                        wsInterrupcion.Cells[index, 8].Value = data[8]; //- Correlativo
                        wsInterrupcion.Cells[index, 9].Value = data[9]; //- Energia
                        wsInterrupcion.Cells[index, 10].Value = (data[10] == ConstantesAppServicio.SI) ?
                            ConstantesCalculoResarcimiento.TextoSi : ConstantesCalculoResarcimiento.TextoNo; //- Incremento de tolerancia
                        wsInterrupcion.Cells[index, 11].Value = result.ListaTipoInterrupcion.Where(x => x.Retintcodi == int.Parse(data[11])).First().Retintnombre; //- Tipo de interrupcion
                        wsInterrupcion.Cells[index, 12].Value = result.ListaCausaInterrupcion.Where(x => x.Recintcodi == int.Parse(data[12])).First().Recintnombre; //- Causa de interrupcion
                        wsInterrupcion.Cells[index, 13].Value = data[13]; //- Ni
                        wsInterrupcion.Cells[index, 14].Value = data[14]; //- Ki
                        wsInterrupcion.Cells[index, 15].Value = data[15]; //- Tiempo inicio ejecutado
                        wsInterrupcion.Cells[index, 16].Value = data[16]; //- Tiempo fin ejecutado
                        wsInterrupcion.Cells[index, 17].Value = data[17]; //- Tiempo inicio programado
                        wsInterrupcion.Cells[index, 18].Value = data[18]; //- Tiempo fin programado

                        ReEmpresaDTO empresa1 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[19])).FirstOrDefault();
                        wsInterrupcion.Cells[index, 19].Value = (empresa1 != null) ? empresa1.Emprnomb : data[19]; //- Empresa 1
                        wsInterrupcion.Cells[index, 20].Value = data[20]; //- Porcentaje 1

                        if (!string.IsNullOrEmpty(data[21]))
                        {
                            ReEmpresaDTO empresa2 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[21])).FirstOrDefault();
                            wsInterrupcion.Cells[index, 21].Value = (empresa2 != null) ? empresa2.Emprnomb : data[21]; //- Empresa 2
                        }
                        wsInterrupcion.Cells[index, 22].Value = data[22]; //- Porcentaje 2

                        if (!string.IsNullOrEmpty(data[23]))
                        {
                            ReEmpresaDTO empresa3 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[23])).FirstOrDefault();
                            wsInterrupcion.Cells[index, 23].Value = (empresa3 != null) ? empresa3.Emprnomb : data[23]; //- Empresa 3
                        }
                        wsInterrupcion.Cells[index, 24].Value = data[24]; //- Porcentaje 3

                        if (!string.IsNullOrEmpty(data[25]))
                        {
                            ReEmpresaDTO empresa4 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[25])).FirstOrDefault();
                            wsInterrupcion.Cells[index, 25].Value = (empresa4 != null) ? empresa4.Emprnomb : data[25]; //- Empresa 4
                        }
                        wsInterrupcion.Cells[index, 26].Value = data[26]; //- Porcentaje 4

                        if (!string.IsNullOrEmpty(data[27]))
                        {
                            ReEmpresaDTO empresa5 = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[27])).FirstOrDefault();
                            wsInterrupcion.Cells[index, 27].Value = (empresa5 != null) ? empresa5.Emprnomb : data[27]; //- Empresa 5
                        }
                        wsInterrupcion.Cells[index, 28].Value = data[28]; //- Porcentaje 5

                        wsInterrupcion.Cells[index, 29].Value = data[29]; //- Causa resuminda
                        wsInterrupcion.Cells[index, 30].Value = data[30]; //- Ei
                        wsInterrupcion.Cells[index, 31].Value = data[31]; //- Resarcimientos

                        //- Campos de observaciones y respuestas
                        // 32 es de evidencia
                        int count = 0;
                        int col = 33;
                        for (int k = 33; k < data.Length; k++)
                        {
                            count++;
                            if (count != 5)
                            {
                                if (count == 6)
                                {
                                    if (!string.IsNullOrEmpty(data[k]))
                                    {
                                        wsInterrupcion.Cells[index, col].Value = (data[k] == ConstantesAppServicio.SI) ?
                                        ConstantesCalculoResarcimiento.TextoSi : ConstantesCalculoResarcimiento.TextoNo;
                                    }
                                }
                                else if (count == 8)
                                {
                                    wsInterrupcion.Cells[index, col].Value = string.Empty;
                                }
                                else wsInterrupcion.Cells[index, col].Value = data[k];
                                col++;
                            }
                            if (count == 8) count = 0;
                        }

                        // 33
                        // 34
                        // 35
                        // 36
                        // 37 es de evidencia
                        // 38
                        // 39
                        // 40 es de evidencia
                        // 41
                        // 42
                        // 43
                        // 44
                        // 45 es de evidencia
                        // 46
                        // 47 
                        // 48 es de evidencia

                        index++;

                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Carga los datos a partir de excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="validaciones"></param>
        /// <returns></returns>
        public string[][] CargarRespuestasExcel(string path, string file, int empresa, int periodo, out List<string> validaciones, out List<EstructuraCargaFile> listadoArchivos)
        {
            List<string[]> result = new List<string[]>();
            FileInfo fileInfo = new FileInfo(path + file);
            List<string> errores = new List<string>();
            List<EstructuraCargaFile> listFiles = new List<EstructuraCargaFile>();

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];

                if (wsInterrupcion != null)
                {
                    //-- Verificar columnas 
                    List<string> columnas = UtilCalculoResarcimiento.ObtenerEstructuraPlantillaRespuestas(path);
                    bool flag = true;
                    for (int i = 1; i <= 72; i++)
                    {
                        string header = (wsInterrupcion.Cells[1, i].Value != null) ? wsInterrupcion.Cells[1, i].Value.ToString() : string.Empty;
                        if (header != columnas[i - 1])
                        {
                            errores.Add("La plantilla ha sido modificada. No puede cambiar o eliminar columnas del Excel.");
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        //- Verificar eliminacion de filas
                        List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                                     ObtenerPorEmpresaPeriodo(empresa, periodo);
                        List<ReInterrupcionSuministroDetDTO> detalle = FactorySic.GetReInterrupcionSuministroDetRepository().
                                     ObtenerPorEmpresaPeriodo(empresa, periodo);
                        List<int> idsTotal = entitys.Select(x => x.Reintcodi).Distinct().ToList();
                        List<int> ids = new List<int>();

                        EstructuraInterrupcion maestros = new EstructuraInterrupcion();
                        maestros.ListaCliente = this.ObtenerEmpresas();
                        maestros.ListaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
                        maestros.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().List();
                        maestros.ListaNivelTension = FactorySic.GetReNivelTensionRepository().List();
                        maestros.ListaEmpresa = this.ObtenerEmpresas();
                        int idPeriodoPadre = this.ObtenerIdPeriodoPadre(periodo);
                        maestros.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
                        maestros.PlazoEnergia = this.ValidarPlazoEtapa(periodo, 5);
                        maestros.ListaTiposObservacion = UtilCalculoResarcimiento.ListarTiposObservacion();
                        bool flagPlazo = true;

                        for (int index = 5; index <= ConstantesCalculoResarcimiento.CantidadRegistrosFormato; index++)
                        {
                            if (wsInterrupcion.Cells[index, 5].Value != null && wsInterrupcion.Cells[index, 5].Value != string.Empty)
                            {
                                string[] data = new string[33 + 40];

                                if (wsInterrupcion.Cells[index, 1].Value != null && wsInterrupcion.Cells[index, 1].Value != string.Empty)
                                {
                                    ids.Add(int.Parse(wsInterrupcion.Cells[index, 1].Value.ToString().Split('-')[0]));
                                }

                                string id = (wsInterrupcion.Cells[index, 1].Value != null) ? wsInterrupcion.Cells[index, 1].Value.ToString() : string.Empty; //- Id
                                data[0] = string.Empty;

                                if (id.Split(ConstantesAppServicio.CaracterGuion).Length == 2)
                                {
                                    data[0] = id;
                                }
                                int valorId = 0;

                                ReInterrupcionSuministroDTO entity = null;
                                if (!string.IsNullOrEmpty(data[0]))
                                {
                                    if (int.TryParse(data[0].Split(ConstantesAppServicio.CaracterGuion)[0], out valorId))
                                    {
                                        entity = entitys.Where(x => x.Reintcodi == valorId).FirstOrDefault();
                                    }
                                }

                                data[1] = string.Empty;
                                data[2] = (wsInterrupcion.Cells[index, 2].Value != null) ? wsInterrupcion.Cells[index, 2].Value.ToString() : string.Empty; //- Correlativo

                                string tipoCliente = string.Empty;
                                if (wsInterrupcion.Cells[index, 3].Value != null)
                                {
                                    tipoCliente = (wsInterrupcion.Cells[index, 3].Value.ToString() == ConstantesCalculoResarcimiento.TextoClienteRegulado) ?
                                             ConstantesCalculoResarcimiento.TipoClienteRegulado : ConstantesCalculoResarcimiento.TipoClienteLibre;
                                }
                                data[3] = tipoCliente; //- Tipo de cliente

                                string cliente = string.Empty;
                                if (wsInterrupcion.Cells[index, 4].Value != null)
                                {
                                    ReEmpresaDTO reCliente = maestros.ListaCliente.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 4].Value.ToString()).FirstOrDefault();
                                    if (reCliente != null) cliente = reCliente.Emprcodi.ToString();
                                }
                                data[4] = cliente; //- Cliente

                                string ptoEntrega = string.Empty;
                                if (wsInterrupcion.Cells[index, 5].Value != null)
                                {
                                    if (tipoCliente == ConstantesCalculoResarcimiento.TipoClienteRegulado)
                                    {
                                        RePtoentregaPeriodoDTO rePtoEntrega = maestros.ListaPuntoEntrega.Where(x => x.Repentnombre == wsInterrupcion.Cells[index, 5].Value.ToString()).FirstOrDefault();
                                        if (rePtoEntrega != null) ptoEntrega = rePtoEntrega.Repentcodi.ToString();
                                    }
                                    else
                                    {
                                        ptoEntrega = wsInterrupcion.Cells[index, 5].Value.ToString();
                                    }
                                }

                                data[5] = ptoEntrega; //- Punto de entrega
                                data[6] = (wsInterrupcion.Cells[index, 6].Value != null) ? wsInterrupcion.Cells[index, 6].Value.ToString() : string.Empty; //- Nro de suministro

                                string nivelTension = string.Empty;
                                if (wsInterrupcion.Cells[index, 7].Value != null)
                                {
                                    ReNivelTensionDTO reNivelTension = maestros.ListaNivelTension.Where(x => x.Rentabrev == wsInterrupcion.Cells[index, 7].Value.ToString()).FirstOrDefault();
                                    if (reNivelTension != null) nivelTension = reNivelTension.Rentcodi.ToString();
                                }
                                data[7] = nivelTension; //- Nivel de tension
                                data[8] = (wsInterrupcion.Cells[index, 8].Value != null) ? wsInterrupcion.Cells[index, 8].Value.ToString() : string.Empty; //- Correlativo
                                data[9] = (wsInterrupcion.Cells[index, 9].Value != null) ? wsInterrupcion.Cells[index, 9].Value.ToString() : string.Empty; //- Energia

                                //- Validando plazo de la etapa 5
                                if (entity != null && maestros.PlazoEnergia == ConstantesAppServicio.SI)
                                {
                                    if (data[9] != string.Empty && decimal.Parse(data[9]) != entity.Reintenergiasemestral)
                                        flagPlazo = false;
                                }

                                string incremento = string.Empty;
                                if (wsInterrupcion.Cells[index, 10].Value != null)
                                {
                                    incremento = (wsInterrupcion.Cells[index, 10].Value.ToString() == ConstantesCalculoResarcimiento.TextoSi) ?
                                        ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                                }
                                data[10] = incremento; //- Incremento de tolerancia

                                string tipoInterrupcion = string.Empty;
                                if (wsInterrupcion.Cells[index, 11].Value != null)
                                {
                                    ReTipoInterrupcionDTO reTipoInterrupcion = maestros.ListaTipoInterrupcion.Where(x => x.Retintnombre == wsInterrupcion.Cells[index, 11].Value.ToString()).FirstOrDefault();
                                    if (reTipoInterrupcion != null) tipoInterrupcion = reTipoInterrupcion.Retintcodi.ToString();
                                }
                                data[11] = tipoInterrupcion; //- Tipo de interrupcion

                                string causaInterrupcion = string.Empty;
                                if (wsInterrupcion.Cells[index, 12].Value != null)
                                {
                                    int idTipoInterrupcion = 0;
                                    if (!string.IsNullOrEmpty(tipoInterrupcion))
                                    {
                                        idTipoInterrupcion = int.Parse(tipoInterrupcion);
                                    }

                                    ReCausaInterrupcionDTO reCausaInterrupcion = maestros.ListaCausaInterrupcion.Where(x => x.Recintnombre == wsInterrupcion.Cells[index, 12].Value.ToString() &&
                                    (x.Retintcodi == idTipoInterrupcion || idTipoInterrupcion == 0)).FirstOrDefault();

                                    if (reCausaInterrupcion != null) causaInterrupcion = reCausaInterrupcion.Recintcodi.ToString();
                                }
                                data[12] = causaInterrupcion; //- Causa de interrupcion
                                data[13] = (wsInterrupcion.Cells[index, 13].Value != null) ? wsInterrupcion.Cells[index, 13].Value.ToString() : string.Empty; //- Ni
                                data[14] = (wsInterrupcion.Cells[index, 14].Value != null) ? wsInterrupcion.Cells[index, 14].Value.ToString() : string.Empty; //- Ki
                                data[15] = (wsInterrupcion.Cells[index, 15].Value != null) ? wsInterrupcion.Cells[index, 15].Value.ToString() : string.Empty; //- Tiempo inicio ejecutado
                                data[16] = (wsInterrupcion.Cells[index, 16].Value != null) ? wsInterrupcion.Cells[index, 16].Value.ToString() : string.Empty; //- Tiempo fin ejecutado
                                data[17] = (wsInterrupcion.Cells[index, 17].Value != null) ? wsInterrupcion.Cells[index, 17].Value.ToString() : string.Empty; //- Tiempo inicio programado
                                data[18] = (wsInterrupcion.Cells[index, 18].Value != null) ? wsInterrupcion.Cells[index, 18].Value.ToString() : string.Empty; //- Tiempo fin programado

                                string emprcodi1 = string.Empty;
                                if (wsInterrupcion.Cells[index, 19].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 19].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi1 = reEmpresa.Emprcodi.ToString();
                                }

                                data[19] = emprcodi1; //- Empresa 1
                                data[20] = (wsInterrupcion.Cells[index, 20].Value != null) ? wsInterrupcion.Cells[index, 20].Value.ToString() : string.Empty; //- Porcentaje 1


                                string emprcodi2 = string.Empty;
                                if (wsInterrupcion.Cells[index, 21].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 21].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi2 = reEmpresa.Emprcodi.ToString();
                                }
                                data[21] = emprcodi2;//- Empresa 2
                                data[22] = (wsInterrupcion.Cells[index, 22].Value != null) ? wsInterrupcion.Cells[index, 22].Value.ToString() : string.Empty; //- Porcentaje 2


                                string emprcodi3 = string.Empty;
                                if (wsInterrupcion.Cells[index, 23].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 23].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi3 = reEmpresa.Emprcodi.ToString();
                                }
                                data[23] = emprcodi3; //- Empresa 3
                                data[24] = (wsInterrupcion.Cells[index, 24].Value != null) ? wsInterrupcion.Cells[index, 24].Value.ToString() : string.Empty; //- Porcentaje 3


                                string emprcodi4 = string.Empty;
                                if (wsInterrupcion.Cells[index, 25].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 25].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi4 = reEmpresa.Emprcodi.ToString();
                                }
                                data[25] = emprcodi4; //- Empresa 4
                                data[26] = (wsInterrupcion.Cells[index, 26].Value != null) ? wsInterrupcion.Cells[index, 26].Value.ToString() : string.Empty; //- Porcentaje 4


                                string emprcodi5 = string.Empty;
                                if (wsInterrupcion.Cells[index, 27].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 27].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi5 = reEmpresa.Emprcodi.ToString();
                                }
                                data[27] = emprcodi5; //- Empresa 5
                                data[28] = (wsInterrupcion.Cells[index, 28].Value != null) ? wsInterrupcion.Cells[index, 28].Value.ToString() : string.Empty; //- Porcentaje 5
                                data[29] = (wsInterrupcion.Cells[index, 29].Value != null) ? wsInterrupcion.Cells[index, 29].Value.ToString() : string.Empty; //- Causa resuminda
                                data[30] = (wsInterrupcion.Cells[index, 30].Value != null) ? wsInterrupcion.Cells[index, 30].Value.ToString() : string.Empty; //- Ei
                                data[31] = (wsInterrupcion.Cells[index, 31].Value != null) ? wsInterrupcion.Cells[index, 31].Value.ToString() : string.Empty; //- Resarcimientos

                                //- Validando plazo de la etapa 5
                                if (entity != null && maestros.PlazoEnergia == ConstantesAppServicio.SI)
                                {
                                    if (data[30] != string.Empty && decimal.Parse(data[30]) != entity.Reinteie)
                                        flagPlazo = false;
                                    if (data[31] != string.Empty && decimal.Parse(data[31]) != entity.Reintresarcimiento)
                                        flagPlazo = false;
                                }

                                if (entity != null)
                                {
                                    data[32] = entity.Reintevidencia; //- Para saber si tiene archivo
                                }
                                else
                                {
                                    data[32] = string.Empty;
                                }

                                ReInterrupcionSuministroDetDTO det1 = null;
                                ReInterrupcionSuministroDetDTO det2 = null;
                                ReInterrupcionSuministroDetDTO det3 = null;
                                ReInterrupcionSuministroDetDTO det4 = null;
                                ReInterrupcionSuministroDetDTO det5 = null;

                                if (entity != null)
                                {
                                    List<ReInterrupcionSuministroDetDTO> subList = detalle.
                                         Where(x => x.Reintcodi == entity.Reintcodi).OrderBy(x => x.Reintdorden).ToList();
                                    det1 = subList.Where(x => x.Reintdorden == 1).FirstOrDefault();
                                    det2 = subList.Where(x => x.Reintdorden == 2).FirstOrDefault();
                                    det3 = subList.Where(x => x.Reintdorden == 3).FirstOrDefault();
                                    det4 = subList.Where(x => x.Reintdorden == 4).FirstOrDefault();
                                    det5 = subList.Where(x => x.Reintdorden == 5).FirstOrDefault();
                                }
                                List<EstructuraCargaFileItem> filesItems = new List<EstructuraCargaFileItem>();
                                List<TipoObservacion> listaTipoObservacion = UtilCalculoResarcimiento.ListarTiposObservacion();

                                for (int k = 0; k < 5; k++)
                                {
                                    ReInterrupcionSuministroDetDTO itemDetalle = (k == 0) ? det1 : (k == 1) ? det2 : (k == 2) ? det3 : (k == 3) ? det4 : det5;

                                    EstructuraCargaFileItem itemFileDet = new EstructuraCargaFileItem();
                                    itemFileDet.Orden = k + 1;
                                    itemFileDet.Indicador = 0;


                                    data[33 + k * 8] = (itemDetalle != null) ?
                                        (!string.IsNullOrEmpty(itemDetalle.Reintdconformidadresp)) ? (itemDetalle.Reintdconformidadresp == ConstantesAppServicio.SI) ?
                                        ConstantesCalculoResarcimiento.TextoSi : ConstantesCalculoResarcimiento.TextoNo : string.Empty : string.Empty;

                                    if (itemDetalle != null && !string.IsNullOrEmpty(itemDetalle.Reintdobservacionresp))
                                    {
                                        List<int> idsObservaciones = itemDetalle.Reintdobservacionresp.Split(',').Select(int.Parse).ToList();
                                        List<string> textosObservaciones = listaTipoObservacion.Where(x => idsObservaciones.Any(y => int.Parse(x.Id) == y)).Select(x => x.Texto).ToList();
                                        data[34 + k * 8] = string.Join(",", textosObservaciones);
                                    }
                                    else
                                    {
                                        data[34 + k * 8] = string.Empty;
                                    }

                                    //data[34 + k * 8] = (itemDetalle != null) ? (itemDetalle.Reintdobservacionresp != null) ? itemDetalle.Reintdobservacionresp : string.Empty : string.Empty;//(wsInterrupcion.Cells[index, 33 + k * 6].Value != null) ? wsInterrupcion.Cells[index, 33 + k * 6].Value.ToString() : string.Empty; //- observacion resp
                                    data[35 + k * 8] = (itemDetalle != null) ? (itemDetalle.Reintddetalleresp != null) ? itemDetalle.Reintddetalleresp : string.Empty : string.Empty;//(wsInterrupcion.Cells[index, 34 + k * 6].Value != null) ? wsInterrupcion.Cells[index, 34 + k * 6].Value.ToString() : string.Empty; ; //- detalle campo obs resp
                                    data[36 + k * 8] = (itemDetalle != null) ? (itemDetalle.Reintdcomentarioresp != null) ? itemDetalle.Reintdcomentarioresp : string.Empty : string.Empty;//(wsInterrupcion.Cells[index, 35 + k * 6].Value != null) ? wsInterrupcion.Cells[index, 35 + k * 6].Value.ToString() : string.Empty; ; //- comentario resp
                                    data[37 + k * 8] = (itemDetalle != null) ? (itemDetalle.Reintdevidenciaresp != null) ? itemDetalle.Reintdevidenciaresp : string.Empty : string.Empty; //- evidencia resp

                                    if (itemDetalle != null && itemDetalle.Reintdconformidadresp == ConstantesAppServicio.NO)
                                    {
                                        string conformidadSuministrador = string.Empty;
                                        if (wsInterrupcion.Cells[index, 37 + k * 7].Value != null)
                                        {
                                            conformidadSuministrador = (wsInterrupcion.Cells[index, 37 + k * 7].Value.ToString() == ConstantesCalculoResarcimiento.TextoSi) ?
                                                ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                                        }
                                        data[38 + k * 8] = conformidadSuministrador; //- conformidad sumistrador
                                        data[39 + k * 8] = (wsInterrupcion.Cells[index, 38 + k * 7].Value != null) ? wsInterrupcion.Cells[index, 38 + k * 7].Value.ToString() : string.Empty; //- comentario suministrador
                                        data[40 + k * 8] = (itemDetalle != null) ? itemDetalle.Reintdevidenciasumi : string.Empty; //- sustento

                                        //- Agregamos
                                        itemFileDet.Indicador = 1;
                                        itemFileDet.FileName = (wsInterrupcion.Cells[index, 39 + k * 7].Value != null) ? wsInterrupcion.Cells[index, 39 + k * 7].Value.ToString() : string.Empty; //- archivo sustento suministrador

                                    }
                                    else
                                    {
                                        data[38 + k * 8] = string.Empty;
                                        data[39 + k * 8] = string.Empty;
                                        data[40 + k * 8] = string.Empty;
                                    }

                                    filesItems.Add(itemFileDet);

                                }

                                //- Cargamos las observaciones y respuestas
                                result.Add(data);

                                EstructuraCargaFile itemFile = new EstructuraCargaFile();
                                itemFile.Id = valorId;
                                itemFile.Fila = index;
                                itemFile.FileName = (wsInterrupcion.Cells[index, 32].Value != null && wsInterrupcion.Cells[index, 32].Value.ToString() != string.Empty) ?
                                    wsInterrupcion.Cells[index, 32].Value.ToString() : string.Empty;
                                itemFile.ListaItems = filesItems;

                                listFiles.Add(itemFile);
                            }
                        }

                        if (idsTotal.Where(x => ids.Any(y => x == y)).Count() != idsTotal.Count)
                        {
                            errores.Add("Si desea editar registros, primero debe descargar la plantilla nuevamente.");
                            flag = false;
                        }

                        if (!flagPlazo)
                        {
                            errores.Add("No puede cambiar los datos Energía Semestral (kWh), Ei/E y Resarcimiento (US$) dado que ha superado el plazo de la etapa 5.");
                            flag = false;
                        }
                    }
                }
                else
                {
                    errores.Add("No existe la hoja 'Interrupciones' en el libro Excel.");
                }
            }
            listadoArchivos = listFiles;
            validaciones = errores;
            return result.ToArray();
        }

        /// <summary>
        /// Permite grabar las interrupciones
        /// </summary>
        /// <param name="data"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="username"></param>
        /// <param name="comentario"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public EstructuraGrabado GrabarRespuestas(string[][] data, int empresa, int periodo, string username, string comentario,
            string path)
        {
            EstructuraGrabado result = new EstructuraGrabado();
            try
            {
                bool flagCalculo = true;
                string[][] calculo = this.CalcularResarcimientoInterrupcion(data, periodo, out flagCalculo, false);
                if (flagCalculo)
                {
                    List<TipoError> listaTipoError = new List<TipoError>();
                    List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                             ObtenerPorEmpresaPeriodo(empresa, periodo);
                    List<ReInterrupcionSuministroDetDTO> detalle = FactorySic.GetReInterrupcionSuministroDetRepository().
                             ObtenerPorEmpresaPeriodo(empresa, periodo);
                    bool flagEnvio = true;

                    List<int> idsNuevos = new List<int>();
                    List<EstructuraNotificacion> idsActualizados = new List<EstructuraNotificacion>();

                    List<EstructuraGrabadoRespuesta> estructuraGrabar = new List<EstructuraGrabadoRespuesta>();

                    for (int i = 4; i < data.Length; i++)
                    {
                        if (data[i][5] == string.Empty) break;

                        ReInterrupcionSuministroDTO entity = new ReInterrupcionSuministroDTO();

                        string strid = data[i][0]; //- Id
                        string dataid = string.Empty;
                        string trimestral = string.Empty;

                        if (strid.Split(ConstantesAppServicio.CaracterGuion).Length == 2)
                        {
                            dataid = strid.Split(ConstantesAppServicio.CaracterGuion)[0];
                            trimestral = strid.Split(ConstantesAppServicio.CaracterGuion)[1];
                        }

                        entity.Reintcodi = (!string.IsNullOrEmpty(dataid)) ? int.Parse(dataid) : 0;
                        entity.Repercodi = periodo;
                        entity.Reintestado = ConstantesAppServicio.Activo;
                        entity.Reintpadre = -1; //- Para padres que se visualizan
                        entity.Reintfinal = ConstantesAppServicio.SI;
                        entity.Emprcodi = empresa;
                        entity.Reintcorrelativo = int.Parse(data[i][2]);
                        entity.Reinttipcliente = data[i][3];
                        entity.Reintcliente = int.Parse(data[i][4]);
                        if (entity.Reinttipcliente == ConstantesCalculoResarcimiento.TipoClienteRegulado)
                            entity.Repentcodi = int.Parse(data[i][5]);
                        else
                            entity.Reintptoentrega = data[i][5];
                        entity.Reintnrosuministro = data[i][6];
                        entity.Rentcodi = int.Parse(data[i][7]);
                        entity.Reintaplicacionnumeral = int.Parse(data[i][8]);
                        entity.Reintenergiasemestral = decimal.Parse(data[i][9]);
                        entity.Reintinctolerancia = data[i][10];
                        entity.Retintcodi = int.Parse(data[i][11]);
                        entity.Recintcodi = int.Parse(data[i][12]);
                        entity.Reintni = decimal.Parse(data[i][13]);
                        entity.Reintki = decimal.Parse(data[i][14]);
                        entity.Reintfejeinicio = DateTime.ParseExact(data[i][15], ConstantesAppServicio.FormatoFechaFull2,
                            CultureInfo.InvariantCulture);
                        entity.Reintfejefin = DateTime.ParseExact(data[i][16], ConstantesAppServicio.FormatoFechaFull2,
                            CultureInfo.InvariantCulture);
                        entity.Reintfproginicio = (!string.IsNullOrEmpty(data[i][17])) ?
                            (DateTime?)DateTime.ParseExact(data[i][17], ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture) : null;
                        entity.Reintfprogfin = (!string.IsNullOrEmpty(data[i][18])) ?
                            (DateTime?)DateTime.ParseExact(data[i][18], ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture) : null;
                        entity.Emprcodi1 = int.Parse(data[i][19]);
                        entity.Porcentaje1 = decimal.Parse(data[i][20]);
                        entity.Emprcodi2 = (!string.IsNullOrEmpty(data[i][21])) ? (int?)int.Parse(data[i][21]) : null;
                        entity.Porcentaje2 = (!string.IsNullOrEmpty(data[i][22])) ? (decimal?)decimal.Parse(data[i][22]) : null;
                        entity.Emprcodi3 = (!string.IsNullOrEmpty(data[i][23])) ? (int?)int.Parse(data[i][23]) : null;
                        entity.Porcentaje3 = (!string.IsNullOrEmpty(data[i][24])) ? (decimal?)decimal.Parse(data[i][24]) : null;
                        entity.Emprcodi4 = (!string.IsNullOrEmpty(data[i][25])) ? (int?)int.Parse(data[i][25]) : null;
                        entity.Porcentaje4 = (!string.IsNullOrEmpty(data[i][26])) ? (decimal?)decimal.Parse(data[i][26]) : null;
                        entity.Emprcodi5 = (!string.IsNullOrEmpty(data[i][27])) ? (int?)int.Parse(data[i][27]) : null;
                        entity.Porcentaje5 = (!string.IsNullOrEmpty(data[i][28])) ? (decimal?)decimal.Parse(data[i][28]) : null;
                        entity.Reintcausaresumida = data[i][29];
                        entity.Reinteie = decimal.Parse(data[i][30]);
                        entity.Reintresarcimiento = decimal.Parse(data[i][31]);
                        entity.Reintevidencia = data[i][32];
                        entity.Reintusucreacion = username;
                        entity.Reintfeccreacion = DateTime.Now;

                        ReInterrupcionSuministroDTO original = entitys.Where(x => x.Reintcodi == entity.Reintcodi).First();
                        List<ReInterrupcionSuministroDetDTO> subList = detalle.Where(x => x.Reintcodi == entity.Reintcodi).ToList();
                        ReInterrupcionSuministroDetDTO det1 = subList.Where(x => x.Reintdorden == 1).FirstOrDefault();
                        ReInterrupcionSuministroDetDTO det2 = subList.Where(x => x.Reintdorden == 2).FirstOrDefault();
                        ReInterrupcionSuministroDetDTO det3 = subList.Where(x => x.Reintdorden == 3).FirstOrDefault();
                        ReInterrupcionSuministroDetDTO det4 = subList.Where(x => x.Reintdorden == 4).FirstOrDefault();
                        ReInterrupcionSuministroDetDTO det5 = subList.Where(x => x.Reintdorden == 5).FirstOrDefault();
                        original.Emprcodi1 = det1.Emprcodi; //- Empresa 1
                        original.Porcentaje1 = det1.Reintdorcentaje;
                        original.Emprcodi2 = (det2 != null) ? det2.Emprcodi : null; //- Empresa 2
                        original.Porcentaje2 = (det2 != null) ? det2.Reintdorcentaje : null;
                        original.Emprcodi3 = (det3 != null) ? det3.Emprcodi : null; //- Empresa 3
                        original.Porcentaje3 = (det3 != null) ? det3.Reintdorcentaje : null;
                        original.Emprcodi4 = (det4 != null) ? det4.Emprcodi : null; //- Empresa 4
                        original.Porcentaje4 = (det4 != null) ? det4.Reintdorcentaje : null;
                        original.Emprcodi5 = (det5 != null) ? det5.Emprcodi : null; //- Empresa 5
                        original.Porcentaje5 = (det5 != null) ? det5.Reintdorcentaje : null;
                        bool flagActualizacion = false;
                        bool flagActualizacionRespuesta = false;
                        List<string> mensajesCambios = new List<string>();

                        if (det1 != null)
                        {
                            flagActualizacionRespuesta = this.ValidarCambioRespuesta(det1, data[i], 0);
                            det1.Reintdconformidadsumioriginal = det1.Reintdconformidadsumi;
                            det1.Reintdcomentariosumioriginal = det1.Reintdcomentariosumi;
                            det1.Reintdconformidadsumi = data[i][38];
                            det1.Reintdcomentariosumi = data[i][39];
                            det1.Reintdevidenciasumi = data[i][40];

                            string mensajeCambio = this.ValidarCompletadoObservacion(det1, entity, original);
                            if (!string.IsNullOrEmpty(mensajeCambio)) mensajesCambios.Add(mensajeCambio);

                            if (det1.Reintdconformidadresp == ConstantesAppServicio.NO && det1.Reintdconformidadsumi == ConstantesAppServicio.SI)
                                flagActualizacion = true;
                        }
                        if (det2 != null)
                        {
                            flagActualizacionRespuesta = this.ValidarCambioRespuesta(det2, data[i], 1);
                            det2.Reintdconformidadsumioriginal = det2.Reintdconformidadsumi;
                            det2.Reintdcomentariosumioriginal = det2.Reintdcomentariosumi;
                            det2.Reintdconformidadsumi = data[i][46];
                            det2.Reintdcomentariosumi = data[i][47];
                            det2.Reintdevidenciasumi = data[i][48];

                            string mensajeCambio = this.ValidarCompletadoObservacion(det2, entity, original);
                            if (!string.IsNullOrEmpty(mensajeCambio)) mensajesCambios.Add(mensajeCambio);

                            if (det2.Reintdconformidadresp == ConstantesAppServicio.NO && det2.Reintdconformidadsumi == ConstantesAppServicio.SI)
                                flagActualizacion = true;
                        }
                        if (det3 != null)
                        {
                            flagActualizacionRespuesta = this.ValidarCambioRespuesta(det3, data[i], 2);
                            det3.Reintdconformidadsumioriginal = det3.Reintdconformidadsumi;
                            det3.Reintdcomentariosumioriginal = det3.Reintdcomentariosumi;
                            det3.Reintdconformidadsumi = data[i][54];
                            det3.Reintdcomentariosumi = data[i][55];
                            det3.Reintdevidenciasumi = data[i][56];

                            string mensajeCambio = this.ValidarCompletadoObservacion(det3, entity, original);
                            if (!string.IsNullOrEmpty(mensajeCambio)) mensajesCambios.Add(mensajeCambio);

                            if (det3.Reintdconformidadresp == ConstantesAppServicio.NO && det3.Reintdconformidadsumi == ConstantesAppServicio.SI)
                                flagActualizacion = true;
                        }
                        if (det4 != null)
                        {
                            flagActualizacionRespuesta = this.ValidarCambioRespuesta(det4, data[i], 3);
                            det4.Reintdconformidadsumioriginal = det4.Reintdconformidadsumi;
                            det4.Reintdcomentariosumioriginal = det4.Reintdcomentariosumi;
                            det4.Reintdconformidadsumi = data[i][62];
                            det4.Reintdcomentariosumi = data[i][63];
                            det4.Reintdevidenciasumi = data[i][64];

                            string mensajeCambio = this.ValidarCompletadoObservacion(det4, entity, original);
                            if (!string.IsNullOrEmpty(mensajeCambio)) mensajesCambios.Add(mensajeCambio);

                            if (det4.Reintdconformidadresp == ConstantesAppServicio.NO && det4.Reintdconformidadsumi == ConstantesAppServicio.SI)
                                flagActualizacion = true;
                        }
                        if (det5 != null)
                        {
                            flagActualizacionRespuesta = this.ValidarCambioRespuesta(det5, data[i], 4);
                            det5.Reintdconformidadsumioriginal = det5.Reintdconformidadsumi;
                            det5.Reintdcomentariosumioriginal = det5.Reintdcomentariosumi;
                            det5.Reintdconformidadsumi = data[i][70];
                            det5.Reintdcomentariosumi = data[i][71];
                            det5.Reintdevidenciasumi = data[i][72];

                            string mensajeCambio = this.ValidarCompletadoObservacion(det5, entity, original);
                            if (!string.IsNullOrEmpty(mensajeCambio)) mensajesCambios.Add(mensajeCambio);

                            if (det5.Reintdconformidadresp == ConstantesAppServicio.NO && det5.Reintdconformidadsumi == ConstantesAppServicio.SI)
                                flagActualizacion = true;
                        }


                        bool comparar = UtilCalculoResarcimiento.CompararRegistroInterrupcion(entity, original);
                        bool compararRegistro = UtilCalculoResarcimiento.CompararRegistroInterrupcionRegistro(entity, original);

                        bool cambioConformidadSuministrador = UtilCalculoResarcimiento.CompararConformidadSuministrador(det1, det2, det3, det4, det5, data[i]);

                        bool actualizar = true;
                        int caso = 0;

                        if (flagActualizacion == false && compararRegistro == false)
                        {
                            actualizar = false;
                            caso = 1;
                            //- No debe haber cambio
                        }
                        if (flagActualizacion == true && comparar == true)
                        {
                            actualizar = false;
                            caso = 2;

                            if (cambioConformidadSuministrador)
                            {
                                actualizar = true;
                            }

                            //- Debe hacer cambio
                        }

                        if (trimestral == ConstantesAppServicio.SI)
                        {
                            actualizar = true;
                        }

                        if (actualizar)
                        {
                            if (mensajesCambios.Count() == 0)
                            {
                                if (!comparar)
                                {
                                    estructuraGrabar.Add(new EstructuraGrabadoRespuesta
                                    {
                                        Entidad = entity,
                                        Original = original,
                                        Det1 = det1,
                                        Det2 = det2,
                                        Det3 = det3,
                                        Det4 = det4,
                                        Det5 = det5,
                                        Grabar = true
                                    });
                                }
                                else
                                {
                                    estructuraGrabar.Add(new EstructuraGrabadoRespuesta
                                    {
                                        Entidad = entity,
                                        Original = original,
                                        Det1 = det1,
                                        Det2 = det2,
                                        Det3 = det3,
                                        Det4 = det4,
                                        Det5 = det5,
                                        Grabar = false,
                                        FlagActualizacionRespuesta = flagActualizacionRespuesta
                                    });
                                }
                            }
                            else
                            {
                                flagEnvio = false;
                                foreach (string itemMensaje in mensajesCambios)
                                {
                                    listaTipoError.Add(new TipoError { Fila = i, Mensaje = itemMensaje });
                                }
                            }
                        }
                        else
                        {
                            flagEnvio = false;
                            string mensaje = (caso == 1) ? ConstantesCalculoResarcimiento.ValidacionNoActualizacionInterrupcion :
                                ConstantesCalculoResarcimiento.ValidarActualizacionInterrupcion;
                            listaTipoError.Add(new TipoError { Fila = i, Mensaje = mensaje });
                        }
                    }

                    if (listaTipoError.Count == 0)
                    {
                        int id = 0;
                        foreach (EstructuraGrabadoRespuesta itemRespuesta in estructuraGrabar)
                        {
                            if (itemRespuesta.Grabar)
                            {
                                id = this.GrabarInterrupcionObservacion(itemRespuesta.Entidad, path, itemRespuesta.Det1, itemRespuesta.Det2, itemRespuesta.Det3, itemRespuesta.Det4, itemRespuesta.Det5);
                                itemRespuesta.Original.Reintfinal = ConstantesAppServicio.NO;
                                itemRespuesta.Original.Reintpadre = id;
                                FactorySic.GetReInterrupcionSuministroRepository().Update(itemRespuesta.Original);
                                idsActualizados.Add(new EstructuraNotificacion { IdNuevo = id, IdAnterior = itemRespuesta.Entidad.Reintcodi, TipoCambio = 0 });

                                if (!string.IsNullOrEmpty(itemRespuesta.Entidad.Reintevidencia) && !string.IsNullOrEmpty(itemRespuesta.Original.Reintevidencia))
                                {
                                    string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, itemRespuesta.Original.Reintcodi, itemRespuesta.Original.Reintevidencia);
                                    string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id, itemRespuesta.Entidad.Reintevidencia);


                                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                    {
                                        FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                    }
                                }
                            }
                            else
                            {
                                this.GrabarInterrpcionDetalle(itemRespuesta.Det1, itemRespuesta.Det2, itemRespuesta.Det3, itemRespuesta.Det4, itemRespuesta.Det5);

                                if (itemRespuesta.FlagActualizacionRespuesta)
                                {
                                    idsActualizados.Add(new EstructuraNotificacion { IdNuevo = 0, IdAnterior = itemRespuesta.Entidad.Reintcodi, TipoCambio = 1, Det1 = itemRespuesta.Det1, Det2 = itemRespuesta.Det2, Det3 = itemRespuesta.Det3, Det4 = itemRespuesta.Det4, Det5 = itemRespuesta.Det5 });
                                }
                            }

                        }
                    }

                    if (flagEnvio)
                    {
                        ReEnvioDTO envio = new ReEnvioDTO();
                        envio.Repercodi = periodo;
                        envio.Reenvtipo = ConstantesCalculoResarcimiento.EnvioTipoRespuesta;
                        envio.Emprcodi = empresa;
                        envio.Reenvfecha = DateTime.Now;
                        envio.Reenvplazo = (!string.IsNullOrEmpty(comentario)) ? ConstantesAppServicio.NO : ConstantesAppServicio.SI;
                        envio.Reenvcomentario = comentario;
                        envio.Reenvestado = ConstantesAppServicio.Activo;
                        envio.Reenvusucreacion = username;
                        envio.Reenvfeccreacion = DateTime.Now;
                        FactorySic.GetReEnvioRepository().Save(envio);

                        if (idsActualizados.Count > 0)
                            this.EnviarNotificacionRespuesta(idsActualizados, periodo, empresa);

                        result.Result = 1;
                    }
                    else
                    {
                        result.Result = 3;
                        result.ListaMensaje = listaTipoError;
                    }
                }
                else
                {
                    result.Result = 2; // Existen error en el calculo
                    result.Data = calculo;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result.Result = -1;
            }
            return result;
        }

        /// <summary>
        /// Validamos los cambios de datos en base a las observaciones
        /// </summary>
        /// <param name="detalle"></param>
        /// <param name="entity"></param>
        /// <param name="original"></param>
        /// <returns></returns>
        public string ValidarCompletadoObservacion(ReInterrupcionSuministroDetDTO detalle, ReInterrupcionSuministroDTO entity, ReInterrupcionSuministroDTO original)
        {
            string result = string.Empty;
            if (detalle != null && detalle.Reintdconformidadresp == ConstantesAppServicio.NO &&
                detalle.Reintdconformidadsumi == ConstantesAppServicio.SI)
            {
                string tipoObservacion = detalle.Reintdobservacionresp;

                switch (tipoObservacion)
                {
                    case "1": //En trámite de FM o FM Fundada
                        {
                            if (!(entity.Recintcodi == 7 || entity.Recintcodi == 8))
                            {
                                result = "La causa de la interrupción debe ser: FM FUNDADO o FM TRAMITE";
                            }
                            break;
                        }
                    case "2": //Exonerado por Expansión o Reforzamiento
                        {
                            if (!(entity.Recintcodi == 4))
                            {
                                result = "La causa de la interrupción debe ser: EXONERACION POR EXP O REF";
                            }
                            break;
                        }
                    case "3": //No se tiene registro de interrupción
                        {
                            break;
                        }
                    case "4": //Campo observado Nivel de Tensión
                        {
                            if (entity.Rentcodi == original.Rentcodi)
                            {
                                result = "Debe modificar el campo NIVEL DE TENSIÓN";
                            }
                            break;
                        }
                    case "5": //Campo observado Causa o Tipo
                        {
                            if (entity.Retintcodi == original.Retintcodi && entity.Recintcodi == original.Recintcodi)
                            {
                                result = "Debe modificar el campo TIPO O CAUSA DE INTERRUPCIÓN";
                            }
                            break;
                        }
                    case "6": //Campo observado fecha y hora
                        {
                            if (entity.Reintfejeinicio == original.Reintfejeinicio && entity.Reintfejefin == original.Reintfejefin &&
                                entity.Reintfproginicio == original.Reintfproginicio && entity.Reintfprogfin == original.Reintfprogfin)
                            {
                                result = "Debe modificar alguna fecha de inicio o fin programada o ejecutada.";
                            }
                            break;
                        }
                    case "7": //Campo observado responsable o porcentaje de responsabilidad
                        {
                            if (entity.Emprcodi1 == original.Emprcodi1 && entity.Porcentaje1 == original.Porcentaje1 &&
                                entity.Emprcodi2 == original.Emprcodi2 && entity.Porcentaje2 == original.Porcentaje2 &&
                                entity.Emprcodi3 == original.Emprcodi3 && entity.Porcentaje3 == original.Porcentaje3 &&
                                entity.Emprcodi4 == original.Emprcodi4 && entity.Porcentaje4 == original.Porcentaje4 &&
                                entity.Emprcodi5 == original.Emprcodi5 && entity.Porcentaje5 == original.Porcentaje5
                                )
                            {
                                result = "Debe modificar algún responsable o porcentaje de responsabilidad.";
                            }
                            break;
                        }
                    case "8": //Incremento de Tolerancias Sector Típico de Distribución
                        {
                            if (entity.Reintinctolerancia == original.Reintinctolerancia)
                            {
                                result = "Debe modificar el campo Incremento de Tolerancias Sector Típico de Distribución.";
                            }
                            break;
                        }
                }
            }

            //return result;
            return string.Empty;
        }


        /// <summary>
        /// Valida si existen cambios en las respuestas
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ValidarCambioRespuesta(ReInterrupcionSuministroDetDTO entity, string[] data, int pos)
        {
            if (entity != null)
            {
                if ((!string.IsNullOrEmpty(entity.Reintdconformidadsumi) ? entity.Reintdconformidadsumi : string.Empty) != data[38 + pos * 8] ||
                    (!string.IsNullOrEmpty(entity.Reintdcomentariosumi) ? entity.Reintdcomentariosumi : string.Empty) != data[39 + pos * 8])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Valida si existen cambios en las respuestas
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ValidarCambioObservacion(ReInterrupcionSuministroDetDTO entity, string[] data)
        {
            if (entity != null)
            {
                if ((!string.IsNullOrEmpty(entity.Reintdconformidadresp) ? entity.Reintdconformidadresp : string.Empty) != data[33] ||
                    (!string.IsNullOrEmpty(entity.Reintdobservacionresp) ? entity.Reintdobservacionresp : string.Empty) != data[34] ||
                    (!string.IsNullOrEmpty(entity.Reintddetalleresp) ? entity.Reintddetalleresp : string.Empty) != data[35] ||
                    (!string.IsNullOrEmpty(entity.Reintdcomentarioresp) ? entity.Reintdcomentarioresp : string.Empty) != data[36] ||
                    (!string.IsNullOrEmpty(entity.Reintdevidenciaresp) ? entity.Reintdevidenciaresp : string.Empty) != data[37])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Permite actualizar las respuestas
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        /// <param name="entity3"></param>
        /// <param name="entity4"></param>
        /// <param name="entity5"></param>
        private void GrabarInterrpcionDetalle(ReInterrupcionSuministroDetDTO entity1,
            ReInterrupcionSuministroDetDTO entity2,
            ReInterrupcionSuministroDetDTO entity3,
            ReInterrupcionSuministroDetDTO entity4,
            ReInterrupcionSuministroDetDTO entity5)
        {
            if (entity1 != null) FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarRespuesta(entity1);
            if (entity2 != null) FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarRespuesta(entity2);
            if (entity3 != null) FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarRespuesta(entity3);
            if (entity4 != null) FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarRespuesta(entity4);
            if (entity5 != null) FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarRespuesta(entity5);
        }

        /// <summary>
        /// Permite grabar una interrupcion y su detalle
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private int GrabarInterrupcionObservacion(ReInterrupcionSuministroDTO entity, string path,
            ReInterrupcionSuministroDetDTO entity1,
            ReInterrupcionSuministroDetDTO entity2,
            ReInterrupcionSuministroDetDTO entity3,
            ReInterrupcionSuministroDetDTO entity4,
            ReInterrupcionSuministroDetDTO entity5)
        {
            try
            {
                int id = FactorySic.GetReInterrupcionSuministroRepository().Save(entity);

                List<EstructuraArchivoSustento> listFiles = new List<EstructuraArchivoSustento>();

                for (int i = 1; i <= 5; i++)
                {
                    ReInterrupcionSuministroDetDTO itemDetalle = new ReInterrupcionSuministroDetDTO();
                    itemDetalle.Reintcodi = id;
                    itemDetalle.Reintdestado = ConstantesAppServicio.Activo;
                    itemDetalle.Reintdorden = i;

                    if (i == 1)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi1;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje1;
                        UtilCalculoResarcimiento.setaerDatosRespuesta(itemDetalle, entity1);
                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);

                        if (!string.IsNullOrEmpty(entity1.Reintdevidenciasumi) && !string.IsNullOrEmpty(itemDetalle.Reintdevidenciasumi))
                        {
                            string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, entity1.Reintdcodi, entity1.Reintdevidenciasumi);
                            string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDet, itemDetalle.Reintdevidenciasumi);

                            if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                            {
                                FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                            }
                        }

                        if (!string.IsNullOrEmpty(entity1.Reintdevidenciaresp))
                        {
                            string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, entity1.Reintdcodi, entity1.Reintdevidenciaresp);
                            string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, idDet, entity1.Reintdevidenciaresp);

                            if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                            {
                                FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                            }
                        }

                        string fileObservacion = ValidarTipoInterrupcion(entity, itemDetalle, idDet);
                        if (!string.IsNullOrEmpty(fileObservacion)) listFiles.Add(new EstructuraArchivoSustento { FileName = fileObservacion, Extension = itemDetalle.Reintdevidenciaresp });

                    }
                    if (i == 2 && entity.Emprcodi2 != null && entity.Porcentaje2 != null)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi2;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje2;
                        UtilCalculoResarcimiento.setaerDatosRespuesta(itemDetalle, entity2);
                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);

                        if (entity2 != null)
                        {
                            if (!string.IsNullOrEmpty(entity2.Reintdevidenciasumi) && !string.IsNullOrEmpty(itemDetalle.Reintdevidenciasumi))
                            {
                                string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, entity2.Reintdcodi, entity2.Reintdevidenciasumi);
                                string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDet, itemDetalle.Reintdevidenciasumi);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                            }

                            if (!string.IsNullOrEmpty(entity2.Reintdevidenciaresp))
                            {
                                string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, entity2.Reintdcodi, entity2.Reintdevidenciaresp);
                                string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, idDet, entity2.Reintdevidenciaresp);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                            }

                            string fileObservacion = ValidarTipoInterrupcion(entity, itemDetalle, idDet);
                            if (!string.IsNullOrEmpty(fileObservacion)) listFiles.Add(new EstructuraArchivoSustento { FileName = fileObservacion, Extension = itemDetalle.Reintdevidenciaresp });
                        }
                    }
                    if (i == 3 && entity.Emprcodi3 != null && entity.Porcentaje3 != null)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi3;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje3;
                        UtilCalculoResarcimiento.setaerDatosRespuesta(itemDetalle, entity3);
                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);
                        if (entity3 != null)
                        {
                            if (!string.IsNullOrEmpty(entity3.Reintdevidenciasumi) && !string.IsNullOrEmpty(itemDetalle.Reintdevidenciasumi))
                            {
                                string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, entity3.Reintdcodi, entity3.Reintdevidenciasumi);
                                string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDet, itemDetalle.Reintdevidenciasumi);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                            }

                            if (!string.IsNullOrEmpty(entity3.Reintdevidenciaresp))
                            {
                                string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, entity3.Reintdcodi, entity3.Reintdevidenciaresp);
                                string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, idDet, entity3.Reintdevidenciaresp);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                            }

                            string fileObservacion = ValidarTipoInterrupcion(entity, itemDetalle, idDet);
                            if (!string.IsNullOrEmpty(fileObservacion)) listFiles.Add(new EstructuraArchivoSustento { FileName = fileObservacion, Extension = itemDetalle.Reintdevidenciaresp });
                        }
                    }
                    if (i == 4 && entity.Emprcodi4 != null && entity.Porcentaje4 != null)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi4;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje4;
                        UtilCalculoResarcimiento.setaerDatosRespuesta(itemDetalle, entity4);
                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);
                        if (entity4 != null)
                        {
                            if (!string.IsNullOrEmpty(entity4.Reintdevidenciasumi) && !string.IsNullOrEmpty(itemDetalle.Reintdevidenciasumi))
                            {
                                string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, entity4.Reintdcodi, entity4.Reintdevidenciasumi);
                                string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDet, itemDetalle.Reintdevidenciasumi);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                            }
                            if (!string.IsNullOrEmpty(entity4.Reintdevidenciaresp))
                            {
                                string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, entity4.Reintdcodi, entity4.Reintdevidenciaresp);
                                string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, idDet, entity4.Reintdevidenciaresp);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                            }
                            string fileObservacion = ValidarTipoInterrupcion(entity, itemDetalle, idDet);
                            if (!string.IsNullOrEmpty(fileObservacion)) listFiles.Add(new EstructuraArchivoSustento { FileName = fileObservacion, Extension = itemDetalle.Reintdevidenciaresp });
                        }
                    }
                    if (i == 5 && entity.Emprcodi5 != null && entity.Porcentaje5 != null)
                    {
                        itemDetalle.Emprcodi = entity.Emprcodi5;
                        itemDetalle.Reintdorcentaje = entity.Porcentaje5;
                        UtilCalculoResarcimiento.setaerDatosRespuesta(itemDetalle, entity5);
                        int idDet = FactorySic.GetReInterrupcionSuministroDetRepository().Save(itemDetalle);
                        if (entity5 != null)
                        {
                            if (!string.IsNullOrEmpty(entity5.Reintdevidenciasumi) && !string.IsNullOrEmpty(itemDetalle.Reintdevidenciasumi))
                            {
                                string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, entity5.Reintdcodi, entity5.Reintdevidenciasumi);
                                string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDet, itemDetalle.Reintdevidenciasumi);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                            }
                            if (!string.IsNullOrEmpty(entity5.Reintdevidenciaresp))
                            {
                                string fileTemporal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, entity5.Reintdcodi, entity5.Reintdevidenciaresp);
                                string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, idDet, entity5.Reintdevidenciaresp);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.MoveFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileTemporal, fileFinal,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                            }
                            string fileObservacion = ValidarTipoInterrupcion(entity, itemDetalle, idDet);
                            if (!string.IsNullOrEmpty(fileObservacion)) listFiles.Add(new EstructuraArchivoSustento { FileName = fileObservacion, Extension = itemDetalle.Reintdevidenciaresp });
                        }
                    }
                }

                //- Verificamos si debemos reemplezar el archivo de la interrupcion
                if (listFiles.Any())
                {
                    if (listFiles.Count == 1)
                    {

                        string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id, listFiles[0].Extension);

                        if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, listFiles[0].FileName,
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                        {
                            FileServer.CopiarArchivo(ConstantesCalculoResarcimiento.RutaResarcimientos, listFiles[0].FileName, fileFinal,
                                ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                            this.GrabarArchivoInterrupcion((int)id, listFiles[0].Extension);
                        }
                    }
                    else
                    {
                        //- Debemos zipear el archivo de sustentos de las observaciones
                        string fileFinal = ConstantesCalculoResarcimiento.RutaResarcimientos +
                            string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id, ConstantesCalculoResarcimiento.ExtensionZip);
                        foreach (EstructuraArchivoSustento item in listFiles)
                        {
                            item.FileName = ConstantesCalculoResarcimiento.RutaResarcimientos + item.FileName;
                        }
                        FileServer.CreateZipFromFiles(listFiles.Select(x => x.FileName).ToList(),
                            fileFinal, ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                        this.GrabarArchivoInterrupcion((int)id, ConstantesCalculoResarcimiento.ExtensionZip);
                    }
                }

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Validar si el tipo de observaciones es FM
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="itemDetalle"></param>
        /// <returns></returns>
        public string ValidarTipoInterrupcion(ReInterrupcionSuministroDTO entity, ReInterrupcionSuministroDetDTO itemDetalle, int idDet)
        {
            string file = string.Empty;
            var observaciones = (itemDetalle.Reintdobservacionresp ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            if (itemDetalle.Reintdconformidadsumi == ConstantesAppServicio.SI &&
                itemDetalle.Reintdconformidadresp == ConstantesAppServicio.NO &&
                (observaciones.Contains("1") || observaciones.Contains("2")) &&
                (entity.Recintcodi == 4 || entity.Recintcodi == 7 || entity.Recintcodi == 8) &&
                string.IsNullOrEmpty(entity.Reintevidencia))
            {
                if (!string.IsNullOrEmpty(itemDetalle.Reintdevidenciaresp))
                {
                    string fileFinal = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, idDet,
                       itemDetalle.Reintdevidenciaresp);

                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileFinal,
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                    {
                        file = fileFinal;
                    }

                }
            }
            return file;
        }

        /// <summary>
        /// Permite obtener el id del detalle 
        /// </summary>
        /// <param name="idInterrupcion"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        public int ObtenerIdDetalleInterrupcion(int idInterrupcion, int orden)
        {
            ReInterrupcionSuministroDetDTO detalle = FactorySic.GetReInterrupcionSuministroDetRepository().
                ObtenerPorOrden(idInterrupcion, orden);

            if (detalle != null) return detalle.Reintdcodi;
            return -1;
        }

        #endregion

        #region Consolidado

        /// <summary>
        /// Permite obtener las causas de las interrupciones
        /// </summary>
        /// <returns></returns>
        public List<ReCausaInterrupcionDTO> ObtenerCausasInterrupcion()
        {
            return FactorySic.GetReCausaInterrupcionRepository().List();
        }

        /// <summary>
        /// Permite obtener el listado de empresas suministradoras
        /// </summary>
        /// <returns></returns>
        public List<ReEmpresaDTO> ObtenerEmpresasSuministradoras()
        {
            return FactorySic.GetReIngresoTransmisionRepository().ObtenerEmpresasSuministradoras();
        }

        public List<ReEmpresaDTO> ObtenerEmpresasSuministradorasTotal()
        {
            return FactorySic.GetReIngresoTransmisionRepository().ObtenerEmpresasSuministradorasTotal();
        }

        /// <summary>
        /// Permite obtener obtener los datos para la consulta de interrupciones
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public EstructuraConsolidado HabilitarConsolidado(int idPeriodo, string tipo)
        {
            EstructuraConsolidado entity = new EstructuraConsolidado();
            entity.ListaEvento = this.ObtenerEventosPorPeriodo(idPeriodo);
            int idPeriodoPadre = this.ObtenerIdPeriodoPadre(idPeriodo);
            entity.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
            entity.MuestraReporteComparativo = VerificarVisualizacionReporteComparativo(idPeriodo);
            entity.ListaSuministrador = FactorySic.GetReIngresoTransmisionRepository().ObtenerSuministradoresPorPeriodo(idPeriodo, tipo);
            entity.ListaResponsables = FactorySic.GetReIngresoTransmisionRepository().ObtenerResponsablesPorPeriodo(idPeriodo, tipo);
            entity.ListaCausaInterrupcion = this.ObtenerCausasInterrupcion();
            return entity;
        }

        /// <summary>
        /// Permite actualizar los filtros
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public EstructuraConsolidado ObtenerDatosPorSuministrador(int idSuministrador, int idPeriodo, string tipo)
        {
            EstructuraConsolidado entity = new EstructuraConsolidado();

            if (idSuministrador == -1)
            {
                entity.ListaEvento = this.ObtenerEventosPorPeriodo(idPeriodo);
                int idPeriodoPadre = this.ObtenerIdPeriodoPadre(idPeriodo);
                entity.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
                entity.ListaCausaInterrupcion = this.ObtenerCausasInterrupcion();
            }
            else
            {
                entity.ListaEvento = FactorySic.GetReEventoPeriodoRepository().ObtenerEventosUtilizadosPorPeriodo(idPeriodo, idSuministrador);
                entity.ListaPuntoEntrega = FactorySic.GetRePtoentregaPeriodoRepository().ObtenerPtoEntregaUtilizadosPorPeriodo(idPeriodo, idSuministrador, tipo);
                entity.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().ObtenerCausasInterrupcionUtilizadosPorPeriodo(idPeriodo, idSuministrador);
            }


            return entity;
        }

        /// <summary>
        /// Verifica si el reporte de comparativo semestral y trimestral debe mostrarse o no
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public int VerificarVisualizacionReporteComparativo(int idPeriodo)
        {
            int salida = 0;

            List<RePeriodoDTO> lstPeriodos = FactorySic.GetRePeriodoRepository().List();
            RePeriodoDTO periodo = GetByIdRePeriodo(idPeriodo);
            if (periodo.Repertipo == "S")
            {
                if (periodo.Reperrevision == "N")
                {
                    salida = 1;
                }
            }

            return salida;
        }

        /// <summary>
        /// Permite obtener el consolidado de interrupciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="suministrador"></param>
        /// <param name="barra"></param>
        /// <param name="causaInterrupcion"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="evento"></param>
        /// <param name="alimentadorSED"></param>
        /// <param name="estado"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        public EstructuraConsolidado ObtenerConsultaConsolidado(int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
            string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
            string estado, int responsable, string disposicion, string compensacion, string buscar)
        {
            EstructuraConsolidado entity = new EstructuraConsolidado();

            entity.ListaEvento = this.ObtenerEventosPorPeriodo(periodo);
            int idPeriodoPadre = this.ObtenerIdPeriodoPadre(periodo);
            entity.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
            entity.ListaCausaInterrupcion = this.ObtenerCausasInterrupcion();

            if (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion)
            {
                List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                    ObtenerConsolidado(periodo, suministrador, causaInterrupcion, estado, barra, ConstantesAppServicio.SI, responsable, disposicion, compensacion);
                string[][] data = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupciones(entitys, conformidadResponsable, conformidadSuministrador, buscar);
                entity.Data = data;

                //entity.ListaPuntoEntrega = entity.ListaPuntoEntrega.Where(x => entitys.Any(y => x.Repentcodi == y.Repentcodi)).ToList();
                //entity.ListaCausaInterrupcion = entity.ListaCausaInterrupcion.Where(x => entitys.Any(y => x.Recintcodi == y.Recintcodi)).ToList();

            }
            if (tipo == ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga)
            {
                List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().ObtenerConsolidado(periodo,
                    suministrador, barra, estado, evento, alimentadorSED, ConstantesAppServicio.SI, responsable, disposicion);
                string[][] data = UtilCalculoResarcimiento.ObtenerConsolidadoRechazoCarga(entitys, buscar);
                entity.Data = data;

                //entity.ListaPuntoEntrega = entity.ListaPuntoEntrega.Where(x => entitys.Any(y => x.Repentcodi == y.Repentcodi)).ToList();
                //entity.ListaEvento = entity.ListaEvento.Where(x => entitys.Any(y => x.Reevecodi == y.Reevecodi)).ToList();
            }

            return entity;
        }

        /// <summary>
        /// Permite grabar los datos adicionales
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="suministrador"></param>
        /// <param name="barra"></param>
        /// <param name="causaInterrupcion"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="evento"></param>
        /// <param name="alimentadorSED"></param>
        /// <param name="estado"></param>
        /// <param name="responsable"></param>
        /// <param name="disposicion"></param>
        /// <param name="compensacion"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        public int GrabarDatosAdicionalesInterrupciones(string[][] data, int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
            string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
            string estado, int responsable, string disposicion, string compensacion, string buscar)
        {
            try
            {
                if (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion)
                {
                    List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                        ObtenerConsolidado(periodo, suministrador, causaInterrupcion, estado, barra, ConstantesAppServicio.SI, responsable, disposicion, compensacion);
                    foreach (string[] item in data)
                    {
                        int id = int.Parse(item[0]);
                        string decision = item[11];
                        string comentario = item[12];

                        if (!string.IsNullOrEmpty(decision) || !string.IsNullOrEmpty(comentario))
                            FactorySic.GetReInterrupcionSuministroRepository().ActualizarDecisionControveria(id, decision, comentario);

                        for (int i = 1; i <= 5; i++)
                        {
                            if (entitys.Where(x => x.Reintcodi == id && x.OrdenDetalle == i).Count() > 0)
                            {
                                ReInterrupcionSuministroDetDTO detalle = new ReInterrupcionSuministroDetDTO();
                                detalle.Reintcodi = id;
                                detalle.Reintddisposicion = item[1 + (i - 1) * 2];
                                detalle.Reintdcompcero = item[2 + (i - 1) * 2];
                                detalle.Reintdorden = i;

                                if (!string.IsNullOrEmpty(detalle.Reintddisposicion) || !string.IsNullOrEmpty(detalle.Reintdcompcero))
                                {
                                    FactorySic.GetReInterrupcionSuministroDetRepository().ActualizarDatosAdicionales(detalle);
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().ObtenerConsolidado(periodo,
                        suministrador, barra, estado, evento, alimentadorSED, ConstantesAppServicio.SI, responsable, disposicion);

                    foreach (string[] item in data)
                    {
                        ReRechazoCargaDTO entity = new ReRechazoCargaDTO();
                        entity.Rerccodi = int.Parse(item[0]);
                        entity.Rercporcentaje1 = !string.IsNullOrEmpty(item[1]) ? (decimal?)decimal.Parse(item[1]) : null;
                        entity.Rercdisposicion1 = item[2];
                        entity.Rercporcentaje2 = !string.IsNullOrEmpty(item[3]) ? (decimal?)decimal.Parse(item[3]) : null;
                        entity.Rercdisposicion2 = item[4];
                        entity.Rercporcentaje3 = !string.IsNullOrEmpty(item[5]) ? (decimal?)decimal.Parse(item[5]) : null;
                        entity.Rercdisposicion3 = item[6];
                        entity.Rercporcentaje4 = !string.IsNullOrEmpty(item[7]) ? (decimal?)decimal.Parse(item[7]) : null;
                        entity.Rercdisposicion4 = item[8];
                        entity.Rercporcentaje5 = !string.IsNullOrEmpty(item[9]) ? (decimal?)decimal.Parse(item[9]) : null;
                        entity.Rercdisposicion5 = item[10];

                        ReRechazoCargaDTO itemRechazo = entitys.Where(x => x.Rerccodi == entity.Rerccodi).First();

                        if (entity.Rercporcentaje1 != itemRechazo.Rercporcentaje1 || entity.Rercdisposicion1 != itemRechazo.Rercdisposicion1 ||
                            entity.Rercporcentaje2 != itemRechazo.Rercporcentaje2 || entity.Rercdisposicion2 != itemRechazo.Rercdisposicion2 ||
                            entity.Rercporcentaje3 != itemRechazo.Rercporcentaje3 || entity.Rercdisposicion3 != itemRechazo.Rercdisposicion3 ||
                            entity.Rercporcentaje4 != itemRechazo.Rercporcentaje4 || entity.Rercdisposicion4 != itemRechazo.Rercdisposicion4 ||
                            entity.Rercporcentaje5 != itemRechazo.Rercporcentaje5 || entity.Rercdisposicion5 != itemRechazo.Rercdisposicion5)
                        {
                            FactorySic.GetReRechazoCargaRepository().ActualizarPorcentajes(entity);
                        }
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite generar el formato Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="suministrador"></param>
        /// <param name="barra"></param>
        /// <param name="causaInterrupcion"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="evento"></param>
        /// <param name="alimentadorSED"></param>
        /// <param name="estado"></param>
        /// <param name="buscar"></param>
        public void ExportarConsolidado(string path, string filename, int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
           string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
           string estado, int responsable, string disposicion, string compensacion, string buscar)
        {
            if (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion)
            {
                List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                    ObtenerConsolidado(periodo, suministrador, causaInterrupcion, estado, barra, ConstantesAppServicio.SI, responsable, disposicion, compensacion);
                string[][] data = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupciones(entitys, conformidadResponsable, conformidadSuministrador, buscar);
                string plantilla = ConstantesCalculoResarcimiento.PlantillaConsolidadoInterrupcion;
                UtilCalculoResarcimiento.GenerarExcelConsolidadoInterrupcion(path, filename, plantilla, data);
            }
            if (tipo == ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga)
            {
                List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().ObtenerConsolidado(periodo,
                    suministrador, barra, estado, evento, alimentadorSED, ConstantesAppServicio.SI, responsable, disposicion);
                string[][] data = UtilCalculoResarcimiento.ObtenerConsolidadoRechazoCarga(entitys, buscar);
                string plantilla = ConstantesCalculoResarcimiento.PlantilaaConsolidadoRechazoCarga;
                UtilCalculoResarcimiento.GenerarExcelConsolidadoRechazoCarga(path, filename, plantilla, data);
            }
        }

        /// <summary>
        /// Permite leer los datos desde Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="suministrador"></param>
        /// <param name="barra"></param>
        /// <param name="causaInterrupcion"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="evento"></param>
        /// <param name="alimentadorSED"></param>
        /// <param name="estado"></param>
        /// <param name="responsable"></param>
        /// <param name="disposicion"></param>
        /// <param name="compensacion"></param>
        /// <param name="buscar"></param>
        /// <param name="validaciones"></param>
        /// <returns></returns>
        public string[][] CargarConsolidadoExcel(string path, string file, int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
           string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
           string estado, int responsable, string disposicion, string compensacion, string buscar, out List<string> validaciones)
        {
            List<string[]> result = new List<string[]>();
            FileInfo fileInfo = new FileInfo(path + file);
            List<string> errores = new List<string>();

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];

                if (wsInterrupcion != null)
                {
                    //-- Verificar columnas 
                    List<string> columnas = (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ?
                        UtilCalculoResarcimiento.ObtenerEstructuraPlantillaConsolidadoInterrupcuones(path) : UtilCalculoResarcimiento.ObtenerEstructuraPlantillaConsolidadoRechazoCarga(path);
                    bool flag = true;
                    int nroColumnas = (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ? 74 : 31;
                    int filaInicial = (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ? 5 : 3;
                    for (int i = 1; i <= nroColumnas; i++)
                    {
                        string header = (wsInterrupcion.Cells[1, i].Value != null) ? wsInterrupcion.Cells[1, i].Value.ToString() : string.Empty;
                        if (header != columnas[i - 1])
                        {
                            errores.Add("La plantilla ha sido modificada. No puede cambiar o eliminar columnas del Excel.");
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        string[][] data = null;
                        if (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion)
                        {
                            List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                                ObtenerConsolidado(periodo, suministrador, causaInterrupcion, estado, barra, ConstantesAppServicio.SI, responsable, disposicion, compensacion);
                            data = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupciones(entitys, conformidadResponsable, conformidadSuministrador, buscar);

                        }
                        if (tipo == ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga)
                        {
                            List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().ObtenerConsolidado(periodo,
                                suministrador, barra, estado, evento, alimentadorSED, ConstantesAppServicio.SI, responsable, disposicion);
                            data = UtilCalculoResarcimiento.ObtenerConsolidadoRechazoCarga(entitys, buscar);
                        }

                        bool flagRegistros = true;
                        //- Validación de eliminación de filas
                        for (int i = 0; i < data.Length; i++)
                        {
                            string id = (wsInterrupcion.Cells[i + filaInicial, 1].Value != null) ? wsInterrupcion.Cells[i + filaInicial, 1].Value.ToString() : string.Empty;

                            if (id != data[i][0])
                            {
                                flagRegistros = false;
                                break;
                            }
                        }

                        if (flagRegistros)
                        {
                            int[] posicionesExcel = { 22, 23, 26, 27, 30, 31, 34, 35, 38, 39, 73, 74 };
                            int[] posicionesWeb = { 22, 23, 26, 27, 30, 31, 34, 35, 38, 39, 84, 85 };
                            int[] posicioensSino = { 22, 23, 26, 27, 30, 31, 34, 35, 38, 39 };

                            if (tipo == ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga)
                            {
                                posicionesExcel = new int[] { 18, 19, 21, 22, 24, 25, 27, 28, 30, 31 };
                                posicionesWeb = new int[] { 18, 19, 21, 22, 24, 25, 27, 28, 30, 31 };
                                posicioensSino = new int[] { 19, 22, 25, 28, 31 };
                            }

                            for (int i = 0; i < data.Length; i++)
                            {
                                int posWeb = 0;
                                for (int j = 0; j < data[i].Length; j++)
                                {
                                    if (posicionesWeb.Contains(j))
                                    {
                                        string valor = string.Empty;
                                        int posExcel = posicionesExcel[posWeb];

                                        if (wsInterrupcion.Cells[i + filaInicial, posExcel].Value != null)
                                        {
                                            valor = wsInterrupcion.Cells[i + filaInicial, posExcel].Value.ToString();

                                            if (posicioensSino.Contains(j) && !string.IsNullOrEmpty(valor))
                                            {
                                                valor = (valor == ConstantesCalculoResarcimiento.TextoSi) ?
                                                ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                                            }
                                        }

                                        data[i][j] = valor;
                                        posWeb++;
                                    }
                                }
                            }


                            result = data.ToList();
                        }
                        else
                        {
                            errores.Add("Existen registros eliminados. No puede continuar.");
                        }
                    }
                }
                else
                {
                    errores.Add("No existe la hoja 'Interrupciones' en el libro Excel.");
                }
            }
            validaciones = errores;
            return result.ToArray();

        }

        /// <summary>
        /// Permite obtener los datos de trazabilidad de interrupciones de suministro
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="idInterrupcion"></param>
        /// <returns></returns>
        public string[][] ObtenerTrazabilidad(string tipo, int idInterrupcion)
        {
            if (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion)
            {
                UtilCalculoResarcimiento util = new UtilCalculoResarcimiento();
                ReInterrupcionSuministroDTO entity = FactorySic.GetReInterrupcionSuministroRepository().GetById(idInterrupcion);
                List<ReInterrupcionSuministroDTO> list = FactorySic.GetReInterrupcionSuministroRepository().ObtenerTrazabilidad((int)entity.Repercodi, (int)entity.Emprcodi);
                util.ObtenerIdInterrupcionRecursivo(idInterrupcion, list);
                List<int> ids = util.IdsInterrupcion;
                DateTime plazo = ObtenerPlazoEtapa((int)entity.Repercodi, 1);
                List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                        ObtenerConsolidado((int)entity.Repercodi, (int)entity.Emprcodi, -1, ConstantesCalculoResarcimiento.Todos, -1, ConstantesCalculoResarcimiento.Todos, -1, string.Empty, string.Empty);
                List<ReInterrupcionSuministroDTO> result = entitys.Where(x => ids.Any(y => y == x.Reintcodi) &&
                ((DateTime)x.Reintfeccreacion).Subtract(plazo).TotalDays >= 0
                ).OrderByDescending(x => x.Reintcodi).ToList();
                string[][] data = UtilCalculoResarcimiento.ObtenerTrazabilidadInterrupcion(result);
                return data;
            }
            if (tipo == ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga)
            {
                UtilCalculoResarcimiento util = new UtilCalculoResarcimiento();
                ReRechazoCargaDTO entity = FactorySic.GetReRechazoCargaRepository().GetById(idInterrupcion);
                List<ReRechazoCargaDTO> list = FactorySic.GetReRechazoCargaRepository().ObtenerTrazabilidad((int)entity.Repercodi, (int)entity.Emprcodi);
                util.ObtenerIdRechazoCargaRecursivo(idInterrupcion, list);
                List<int> ids = util.IdsRechazoCarga;
                DateTime plazo = ObtenerPlazoEtapa((int)entity.Repercodi, 4);
                List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().ObtenerConsolidado(
                    (int)entity.Repercodi, (int)entity.Emprcodi, -1, ConstantesCalculoResarcimiento.Todos, -1, string.Empty, ConstantesCalculoResarcimiento.Todos, -1, string.Empty);
                List<ReRechazoCargaDTO> result = entitys.Where(x => ids.Any(y => y == x.Rerccodi) &&
                ((DateTime)x.Rercfeccreacion).Subtract(plazo).TotalDays >= 0
                ).OrderByDescending(x => x.Rerccodi).ToList();
                string[][] data = UtilCalculoResarcimiento.ObtenerTrazabilidadRechazoCarga(result);
                return data;
            }
            return null;
        }

        /// <summary>
        /// Permite vaidar los plazoa
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="etapa"></param>
        /// <returns></returns>
        private DateTime ObtenerPlazoEtapa(int idPeriodo, int etapa)
        {
            List<ReMaestroEtapaDTO> entitys = FactorySic.GetReMaestroEtapaRepository().GetByCriteria(idPeriodo);
            ReMaestroEtapaDTO entity = entitys.Where(x => x.Reetacodi == etapa).First();
            if (entity != null)
            {
                if (entity.FechaFinal != null)
                {
                    return (DateTime)entity.FechaFinal;
                }
            }
            return DateTime.Now;
        }

        /// <summary>
        /// Permite obtener el motivo de anulacion
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="idInterrupcion"></param>
        /// <returns></returns>
        public MotivoAnulacion ObtenerMotivoAnulacion(string tipo, int idInterrupcion)
        {
            MotivoAnulacion result = new MotivoAnulacion();

            if (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion)
            {
                ReInterrupcionSuministroDTO entity = FactorySic.GetReInterrupcionSuministroRepository().GetById(idInterrupcion);
                result.Fecha = (entity.Reintfecanulacion != null) ? ((DateTime)entity.Reintfecanulacion).ToString(ConstantesAppServicio.FormatoFechaFull2) :
                    string.Empty;
                result.Usuario = entity.Reintusueliminacion;
                result.Motivo = entity.Reintmotivoanulacion;
            }
            if (tipo == ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga)
            {
                ReRechazoCargaDTO entity = FactorySic.GetReRechazoCargaRepository().GetById(idInterrupcion);
                result.Fecha = (entity.Rercfecanulacion != null) ? ((DateTime)entity.Rercfecanulacion).ToString(ConstantesAppServicio.FormatoFechaFull2) :
                    string.Empty;
                result.Usuario = entity.Rercusueliminacion;
                result.Motivo = entity.Rercmotivoanulacion;
            }

            return result;
        }

        #endregion

        #region Envio de notificaciones
        /// <summary>
        /// Permite enviar las notificaciones
        /// </summary>
        /// <param name="fuente"></param>
        /// <param name="tipoNotificacion"></param>
        /// <param name="idInterrupcion"></param>
        /// <param name="idsNuevos"></param>
        /// <param name="idsActualizados"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="idEmpresa"></param>
        public void EnviarNotificacion(string fuente, int tipoNotificacion, int idInterrupcion, List<int> idsNuevos, List<EstructuraNotificacion> idsActualizados, int idPeriodo, int idEmpresa)
        {

            int etapa = (fuente == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ? 1 : 4;
            string plazo = this.ValidarPlazoEtapa(idPeriodo, etapa);

            if (plazo == ConstantesAppServicio.SI)
            {
                List<int> ids = new List<int>();
                ids.Add(idInterrupcion);
                ids.AddRange(idsNuevos);
                ids.AddRange(idsActualizados.Select(x => x.IdNuevo));
                ids.AddRange(idsActualizados.Select(x => x.IdAnterior));
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string asunto = string.Empty;
                string cuerpo = string.Empty;
                string destinatario = ConfigurationManager.AppSettings["DestinatarioNotificacionResarcimientos"];
                List<string> emails = destinatario.Split(';').ToList();
                string archivos = string.Empty;
                StringBuilder strCuerpo = new StringBuilder();
                RePeriodoDTO periodo = FactorySic.GetRePeriodoRepository().GetById(idPeriodo);
                string nombreArchivo = string.Empty;
                if (tipoNotificacion == ConstantesCalculoResarcimiento.TipoNotificacionEliminacion)
                {
                    if (fuente == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion)
                    {
                        ReInterrupcionSuministroDTO interrupcion = FactorySic.GetReInterrupcionSuministroRepository().GetById(idInterrupcion);
                        idEmpresa = (int)interrupcion.Emprcodi;
                    }
                    else
                    {
                        ReRechazoCargaDTO interrupcion = FactorySic.GetReRechazoCargaRepository().GetById(idInterrupcion);
                        idEmpresa = (int)interrupcion.Emprcodi;
                    }
                }
                SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
                strCuerpo.Append("<ul>");
                if (tipoNotificacion == ConstantesCalculoResarcimiento.TipoNotificacionEliminacion)
                    strCuerpo.Append("<li>" + empresa.Emprnomb + " realizó anulación de interrupciones. </li>");
                if (idsNuevos.Count > 0) strCuerpo.Append("<li>" + empresa.Emprnomb + " registró nuevas interrupciones. </li>");
                if (idsActualizados.Count > 0) strCuerpo.Append("<li>" + empresa.Emprnomb + " actualizó interrupciones. </li>");
                strCuerpo.Append("</ul>");
                cuerpo = strCuerpo.ToString();

                if (fuente == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion)
                {
                    List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().ObtenerNotificacionInterrupcion(ids);
                    List<ReInterrupcionSuministroDTO> entitysNuevos = entitys.Where(x => idsNuevos.Any(y => x.Reintcodi == y)).ToList();

                    //- Colocarlos en el mismo orden

                    List<ReInterrupcionSuministroDTO> entitysActualizadosTotal = entitys.Where(x => idsActualizados.Any(y => x.Reintcodi == y.IdNuevo)).ToList();

                    foreach (ReInterrupcionSuministroDTO itemAct in entitysActualizadosTotal)
                    {
                        EstructuraNotificacion itemEstructura = idsActualizados.Where(x => x.IdNuevo == itemAct.Reintcodi).FirstOrDefault();
                        if (itemEstructura != null)
                            itemAct.CampoOrden = itemEstructura.IdAnterior;
                    }
                    List<ReInterrupcionSuministroDTO> entitysActualizados = entitysActualizadosTotal.OrderBy(x => x.CampoOrden).ToList();

                    List<ReInterrupcionSuministroDTO> entitysOriginales = entitys.Where(x => idsActualizados.Any(y => x.Reintcodi == y.IdAnterior)).OrderBy(x => x.Reintcodi).ToList();

                    string[][] dataAnulados = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupcionesComparativo(entitys.Where(x => x.Reintcodi == idInterrupcion).ToList(), string.Empty, string.Empty, string.Empty);
                    string[][] dataNuevos = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupcionesComparativo(entitysNuevos, string.Empty, string.Empty, string.Empty);
                    string[][] dataActualizados = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupcionesComparativo(entitysActualizados, string.Empty, string.Empty, string.Empty);
                    string[][] dataOriginales = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupcionesComparativo(entitysOriginales, string.Empty, string.Empty, string.Empty);

                    archivos = this.GenerarArchivoExcelNotificacionInterrupcion(fuente, path, dataAnulados, dataNuevos, dataActualizados, dataOriginales, out nombreArchivo);
                    asunto = "Notificación Resarcimiento – Interrupciones por Punto de Entrega – " + periodo.Repernombre + " - " + empresa.Emprnomb;

                }
                else if (fuente == ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga)
                {
                    List<ReRechazoCargaDTO> entitys = FactorySic.GetReRechazoCargaRepository().ObtenerNotificacionInterrupcion(ids);
                    List<ReRechazoCargaDTO> entitysNuevos = entitys.Where(x => idsNuevos.Any(y => x.Rerccodi == y)).ToList();
                    List<ReRechazoCargaDTO> entitysActualizadosTotal = entitys.Where(x => idsActualizados.Any(y => x.Rerccodi == y.IdNuevo)).ToList();

                    foreach (ReRechazoCargaDTO itemAct in entitysActualizadosTotal)
                    {
                        EstructuraNotificacion itemEstructura = idsActualizados.Where(x => x.IdNuevo == itemAct.Rerccodi).FirstOrDefault();
                        if (itemEstructura != null)
                            itemAct.CampoOrden = itemEstructura.IdAnterior;
                    }
                    List<ReRechazoCargaDTO> entitysActualizados = entitysActualizadosTotal.OrderBy(x => x.CampoOrden).ToList();

                    List<ReRechazoCargaDTO> entitysOriginales = entitys.Where(x => idsActualizados.Any(y => x.Rerccodi == y.IdAnterior)).OrderBy(x => x.Rerccodi).ToList();

                    string[][] dataAnulados = UtilCalculoResarcimiento.ObtenerConsolidadoRechazoCarga(entitys.Where(x => x.Rerccodi == idInterrupcion).ToList(), string.Empty);
                    string[][] dataNuevos = UtilCalculoResarcimiento.ObtenerConsolidadoRechazoCarga(entitysNuevos, string.Empty);
                    string[][] dataActualizados = UtilCalculoResarcimiento.ObtenerConsolidadoRechazoCarga(entitysActualizados, string.Empty);
                    string[][] dataOriginales = UtilCalculoResarcimiento.ObtenerConsolidadoRechazoCarga(entitysOriginales, string.Empty);

                    archivos = this.GenerarArchivoExcelNotificacionInterrupcion(fuente, path, dataAnulados, dataNuevos, dataActualizados, dataOriginales, out nombreArchivo);
                    asunto = "Notificación Resarcimiento – Interrupciones por Rechazo Carga – " + periodo.Repernombre + " - " + empresa.Emprnomb;
                }

                //Enviamos Correo
                COES.Base.Tools.Util.SendEmailNotificacion(emails, asunto, cuerpo, string.Empty, archivos, nombreArchivo);
            }
        }

        /// <summary>
        /// Permite genear el archivo para notificacion
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dataAnulados"></param>
        /// <param name="dataNuevos"></param>
        /// <param name="dataActualizados"></param>
        /// <param name="dataOriginales"></param>
        private string GenerarArchivoExcelNotificacionInterrupcion(string tipo, string path, string[][] dataAnulados, string[][] dataNuevos,
            string[][] dataActualizados, string[][] dataOriginales, out string nombreArchivo)
        {
            string result = string.Empty;
            nombreArchivo = string.Empty;
            try
            {
                string plantilla = (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ? ConstantesCalculoResarcimiento.PlantillaNotificacionInterrupcion :
                    ConstantesCalculoResarcimiento.PlantillaNotificacionRechazoCarga;
                string fileName = (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ?
                    String.Format(ConstantesCalculoResarcimiento.ArchivoNotificacionInterrupcion, DateTime.Now.ToString("ddMMyyyyHHmmss")) :
                    String.Format(ConstantesCalculoResarcimiento.ArchivoNotificacionRechazoCarga, DateTime.Now.ToString("ddMMyyyyHHmmss"));

                nombreArchivo = fileName;

                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + fileName);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (dataAnulados.Length > 0)
                    {
                        ExcelWorksheet wsAnuladas = xlPackage.Workbook.Worksheets["Interrupciones anuladas"];
                        this.GenerarHojaInterrupcionNotificacion(tipo, wsAnuladas, dataAnulados, null);
                    }
                    else
                        xlPackage.Workbook.Worksheets.Delete("Interrupciones anuladas");

                    if (dataNuevos.Length > 0)
                    {
                        ExcelWorksheet wsNuevos = xlPackage.Workbook.Worksheets["Nuevas interrupciones"];
                        this.GenerarHojaInterrupcionNotificacion(tipo, wsNuevos, dataNuevos, null);
                    }
                    else
                        xlPackage.Workbook.Worksheets.Delete("Nuevas interrupciones");

                    if (dataActualizados.Length > 0)
                    {
                        ExcelWorksheet wsAQctualizados = xlPackage.Workbook.Worksheets["Interrupciones actualizadas"];
                        this.GenerarHojaInterrupcionNotificacion(tipo, wsAQctualizados, dataActualizados, dataOriginales);
                    }
                    else
                        xlPackage.Workbook.Worksheets.Delete("Interrupciones actualizadas");

                    xlPackage.Save();

                    result = path + fileName;
                }

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        /// <summary>
        /// Permite generar la hoja para las notificaciones
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="result"></param>
        /// <param name="originales"></param>
        public void GenerarHojaInterrupcionNotificacion(string tipo, ExcelWorksheet ws, string[][] result, string[][] originales)
        {
            try
            {
                ExcelRange rg = null;
                int index = (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ? 4 : 3;
                int colmax = (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ? 33 : 17;
                int filaOrigen = index;
                if (originales != null)
                {
                    rg = ws.Cells[filaOrigen, 1, filaOrigen, colmax];
                    rg.Merge = true;
                    ws.Cells[filaOrigen, 1].Value = "Interrupciones Actualizadas";
                    rg.Style.Font.Size = 9;
                    rg.Style.Font.Bold = true;
                    index++;
                }
                int t = 0;
                foreach (string[] data in result)
                {
                    int col = 1;
                    for (int k = 2; k < colmax; k++)
                    {
                        ws.Cells[index, col].Value = data[k];

                        if (originales != null)
                        {
                            if (data[k] != originales[t][k])
                            {
                                rg = ws.Cells[index, col, index, col];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));
                            }
                        }

                        col++;
                    }
                    t++;
                    index++;
                }

                rg = ws.Cells[filaOrigen, 1, index - 1, colmax - 2];
                rg.Style.Font.Size = 9;
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                if (originales != null)
                {
                    int indexNuevo = index + 1;
                    index++;
                    rg = ws.Cells[index, 1, index, colmax];
                    rg.Merge = true;
                    ws.Cells[index, 1].Value = "Interrupciones Originales";

                    rg.Style.Font.Size = 9;
                    rg.Style.Font.Bold = true;
                    index++;


                    foreach (string[] data in originales)
                    {
                        int col = 1;
                        for (int k = 2; k < colmax; k++)
                        {
                            ws.Cells[index, col].Value = data[k];

                            col++;
                        }

                        index++;
                    }

                    rg = ws.Cells[indexNuevo, 1, index - 1, colmax - 2];
                    rg.Style.Font.Size = 9;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite enviar las notificaciones
        /// </summary>
        /// <param name="fuente"></param>
        /// <param name="tipoNotificacion"></param>
        /// <param name="idInterrupcion"></param>
        /// <param name="idsNuevos"></param>
        /// <param name="idsActualizados"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="idEmpresa"></param>
        public void EnviarNotificacionRespuesta(List<EstructuraNotificacion> idsActualizados, int idPeriodo, int idEmpresa)
        {
            string plazo = this.ValidarPlazoEtapa(idPeriodo, 3);

            if (plazo == ConstantesAppServicio.SI)
            {
                List<int> ids = new List<int>();
                ids.AddRange(idsActualizados.Select(x => x.IdNuevo));
                ids.AddRange(idsActualizados.Select(x => x.IdAnterior));
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string asunto = string.Empty;
                string cuerpo = string.Empty;
                string destinatario = ConfigurationManager.AppSettings["DestinatarioNotificacionResarcimientos"];
                List<string> emails = destinatario.Split(';').ToList();
                string archivos = string.Empty;
                StringBuilder strCuerpo = new StringBuilder();
                RePeriodoDTO periodo = FactorySic.GetRePeriodoRepository().GetById(idPeriodo);
                string nombreArchivo = string.Empty;

                SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
                strCuerpo.Append("<ul>");
                if (idsActualizados.Count > 0) strCuerpo.Append("<li>" + empresa.Emprnomb + " actualizó interrupciones y/o respuestas. </li>");
                strCuerpo.Append("</ul>");
                cuerpo = strCuerpo.ToString();

                //- Todos los registros
                List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().ObtenerNotificacionInterrupcion(ids);

                List<ReInterrupcionSuministroDTO> entitysActualizados = UtilCalculoResarcimiento.ClonarListaInterrupcion(entitys.Where(x => idsActualizados.Any(y => x.Reintcodi == y.IdNuevo && y.TipoCambio == 0)).ToList());
                entitysActualizados.AddRange(UtilCalculoResarcimiento.ClonarListaInterrupcion(entitys.Where(x => idsActualizados.Any(y => x.Reintcodi == y.IdAnterior && y.TipoCambio == 1)).ToList()));
                List<ReInterrupcionSuministroDTO> entitysOriginales = UtilCalculoResarcimiento.ClonarListaInterrupcion(entitys.Where(x => idsActualizados.Any(y => x.Reintcodi == y.IdAnterior && y.TipoCambio == 0)).ToList());
                entitysOriginales.AddRange(UtilCalculoResarcimiento.ClonarListaInterrupcion(entitys.Where(x => idsActualizados.Any(y => x.Reintcodi == y.IdAnterior && y.TipoCambio == 1)).ToList()));

                List<EstructuraNotificacion> subList = idsActualizados.Where(x => x.TipoCambio == 1).ToList();
                foreach (EstructuraNotificacion item in subList)
                {
                    List<ReInterrupcionSuministroDTO> iniciales = entitysOriginales.Where(x => x.Reintcodi == item.IdAnterior).ToList();
                    ReInterrupcionSuministroDTO det1 = iniciales.Where(x => x.OrdenDetalle == 1).FirstOrDefault();
                    if (det1 != null)
                    {
                        det1.Reintdconformidadsumi = item.Det1.Reintdconformidadsumioriginal;
                        det1.Reintdcomentariosumi = item.Det1.Reintdcomentariosumioriginal;
                    }
                    ReInterrupcionSuministroDTO det2 = iniciales.Where(x => x.OrdenDetalle == 2).FirstOrDefault();
                    if (det2 != null)
                    {
                        det2.Reintdconformidadsumi = item.Det2.Reintdconformidadsumioriginal;
                        det2.Reintdcomentariosumi = item.Det2.Reintdcomentariosumioriginal;
                    }
                    ReInterrupcionSuministroDTO det3 = iniciales.Where(x => x.OrdenDetalle == 3).FirstOrDefault();
                    if (det3 != null)
                    {
                        det3.Reintdconformidadsumi = item.Det3.Reintdconformidadsumioriginal;
                        det3.Reintdcomentariosumi = item.Det3.Reintdcomentariosumioriginal;
                    }
                    ReInterrupcionSuministroDTO det4 = iniciales.Where(x => x.OrdenDetalle == 4).FirstOrDefault();
                    if (det4 != null)
                    {
                        det4.Reintdconformidadsumi = item.Det4.Reintdconformidadsumioriginal;
                        det4.Reintdcomentariosumi = item.Det4.Reintdcomentariosumioriginal;
                    }
                    ReInterrupcionSuministroDTO det5 = iniciales.Where(x => x.OrdenDetalle == 5).FirstOrDefault();
                    if (det5 != null)
                    {
                        det5.Reintdconformidadsumi = item.Det5.Reintdconformidadsumioriginal;
                        det5.Reintdcomentariosumi = item.Det5.Reintdcomentariosumioriginal;
                    }
                }
                //- Cambiamos con los valores originales

                string[][] dataActualizados = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupcionesComparativo(entitysActualizados, string.Empty, string.Empty, string.Empty);
                string[][] dataOriginales = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupcionesComparativo(entitysOriginales, string.Empty, string.Empty, string.Empty);

                archivos = this.GenerarArchivoExcelNotificacionRespuestaInterrupcion(path, dataActualizados, dataOriginales, out nombreArchivo);
                asunto = "Notificación Resarcimiento – Respuesta a las observaciones de interrupciones  – " + periodo.Repernombre + " - " + empresa.Emprnomb;

                //Enviamos Correo
                COES.Base.Tools.Util.SendEmailNotificacion(emails, asunto, cuerpo, string.Empty, archivos, nombreArchivo);
            }
        }


        /// <summary>
        /// Permite genear el archivo para notificacion
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dataAnulados"></param>
        /// <param name="dataNuevos"></param>
        /// <param name="dataActualizados"></param>
        /// <param name="dataOriginales"></param>
        private string GenerarArchivoExcelNotificacionRespuestaInterrupcion(string path,
            string[][] result, string[][] originales, out string nombreArchivo)
        {
            string resultado = string.Empty;
            nombreArchivo = string.Empty;
            try
            {
                string plantilla = ConstantesCalculoResarcimiento.PlantillaNotificacionRespuesta;
                string fileName = String.Format(ConstantesCalculoResarcimiento.ArchivoNotificacionRepuesta, DateTime.Now.ToString("ddMMyyyyHHmmss"));

                nombreArchivo = fileName;

                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + fileName);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Interrupciones Actualizadas"];
                    ExcelRange rg = null;
                    int index = 4;
                    int colmax = 73;
                    int filaOrigen = index;
                    if (originales != null)
                    {
                        rg = ws.Cells[filaOrigen, 1, filaOrigen, colmax];
                        rg.Merge = true;
                        ws.Cells[filaOrigen, 1].Value = "Interrupciones Actualizadas";
                        rg.Style.Font.Size = 9;
                        rg.Style.Font.Bold = true;
                        index++;
                    }
                    int t = 0;
                    foreach (string[] data in result)
                    {
                        int col = 1;
                        for (int k = 2; k < colmax; k++)
                        {
                            ws.Cells[index, col].Value = data[k];

                            if (originales != null)
                            {
                                if (data[k] != originales[t][k])
                                {
                                    rg = ws.Cells[index, col, index, col];
                                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));
                                }
                            }

                            col++;
                        }
                        t++;
                        index++;
                    }

                    rg = ws.Cells[filaOrigen, 1, index - 1, colmax - 2];
                    rg.Style.Font.Size = 9;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    if (originales != null)
                    {
                        int indexNuevo = index + 1;
                        index++;
                        rg = ws.Cells[index, 1, index, colmax];
                        rg.Merge = true;
                        ws.Cells[index, 1].Value = "Interrupciones Originales";

                        rg.Style.Font.Size = 9;
                        rg.Style.Font.Bold = true;
                        index++;


                        foreach (string[] data in originales)
                        {
                            int col = 1;
                            for (int k = 2; k < colmax; k++)
                            {
                                ws.Cells[index, col].Value = data[k];

                                col++;
                            }

                            index++;
                        }

                        rg = ws.Cells[indexNuevo, 1, index - 1, colmax - 2];
                        rg.Style.Font.Size = 9;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    }

                    xlPackage.Save();

                    resultado = path + fileName;
                }

                return resultado;
            }
            catch (Exception)
            {
                return resultado;
            }
        }


        /// <summary>
        /// Permite enviar las notificaciones
        /// </summary>
        /// <param name="fuente"></param>
        /// <param name="tipoNotificacion"></param>
        /// <param name="idInterrupcion"></param>
        /// <param name="idsNuevos"></param>
        /// <param name="idsActualizados"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="idEmpresa"></param>
        public void EnviarNotificacionObservacion(List<ReInterrupcionSuministroDetDTO> idsActualizados, int idPeriodo, int idEmpresa)
        {
            string plazo = this.ValidarPlazoEtapa(idPeriodo, 2);

            if (plazo == ConstantesAppServicio.SI)
            {
                List<int> ids = new List<int>();
                ids.AddRange(idsActualizados.Select(x => (int)x.Reintcodi));
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string asunto = string.Empty;
                string cuerpo = string.Empty;
                string destinatario = ConfigurationManager.AppSettings["DestinatarioNotificacionResarcimientos"];
                List<string> emails = destinatario.Split(';').ToList();
                string archivos = string.Empty;
                StringBuilder strCuerpo = new StringBuilder();
                RePeriodoDTO periodo = FactorySic.GetRePeriodoRepository().GetById(idPeriodo);
                string nombreArchivo = string.Empty;

                SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
                strCuerpo.Append("<ul>");
                if (idsActualizados.Count > 0) strCuerpo.Append("<li>" + empresa.Emprnomb + " realizó modificaciones a las observaciones. </li>");
                strCuerpo.Append("</ul>");
                cuerpo = strCuerpo.ToString();

                List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().ObtenerNotificacionInterrupcion(ids);

                List<ReInterrupcionSuministroDTO> entitysActualizados = UtilCalculoResarcimiento.ClonarListaInterrupcion(entitys);

                foreach (ReInterrupcionSuministroDetDTO item in idsActualizados)
                {
                    List<ReInterrupcionSuministroDTO> subList = entitysActualizados.Where(x => x.Reintcodi == item.Reintcodi).ToList();
                    ReInterrupcionSuministroDTO itemPrincipal = subList.Where(x => x.Reintdcodi == item.Reintdcodi).FirstOrDefault();

                    if (itemPrincipal != null)
                    {
                        List<ReInterrupcionSuministroDTO> subListNoPrinical = subList.Where(x => x.Reintdcodi != item.Reintdcodi).ToList();
                        foreach (ReInterrupcionSuministroDTO itemNoPrinicipal in subListNoPrinical)
                        {
                            itemNoPrinicipal.Reintdconformidadresp = itemPrincipal.Reintdconformidadresp;
                            itemNoPrinicipal.Reintdobservacionresp = itemPrincipal.Reintdobservacionresp;
                            itemNoPrinicipal.Reintddetalleresp = itemPrincipal.Reintddetalleresp;
                            itemNoPrinicipal.Reintdcomentarioresp = itemPrincipal.Reintdcomentarioresp;
                            itemNoPrinicipal.Reintdevidenciaresp = itemPrincipal.Reintdevidenciaresp;
                        }
                    }
                }

                List<ReInterrupcionSuministroDTO> entitysOriginales = UtilCalculoResarcimiento.ClonarListaInterrupcion(entitys);

                foreach (ReInterrupcionSuministroDTO item in entitysOriginales)
                {
                    ReInterrupcionSuministroDetDTO detalle = idsActualizados.Where(x => x.Reintcodi == item.Reintcodi).FirstOrDefault();
                    if (detalle != null)
                    {
                        item.Reintdconformidadresp = detalle.Reintdconformidadresporiginal;
                        item.Reintdobservacionresp = detalle.Reintdobservacionresporiginal;
                        item.Reintddetalleresp = detalle.Reintddetalleresporiginal;
                        item.Reintdcomentarioresp = detalle.Reintdcomentarioresporiginal;
                        item.Reintdevidenciaresp = detalle.Reintdevidenciaresporiginal;
                    }
                }

                //- Cambiamos con los valores originales
                string[][] dataActualizados = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupcionesComparativo(entitysActualizados, string.Empty, string.Empty, string.Empty);
                string[][] dataOriginales = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupcionesComparativo(entitysOriginales, string.Empty, string.Empty, string.Empty);

                archivos = this.GenerarArchivoExcelNotificacionObservacion(path, dataActualizados, dataOriginales, out nombreArchivo);
                asunto = "Notificación Resarcimiento – Observaciones a las Interrupciones   – " + periodo.Repernombre + " - " + empresa.Emprnomb;

                //Enviamos Correo
                COES.Base.Tools.Util.SendEmailNotificacion(emails, asunto, cuerpo, string.Empty, archivos, nombreArchivo);
            }
        }

        /// <summary>
        /// Permite genear el archivo para notificacion
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dataAnulados"></param>
        /// <param name="dataNuevos"></param>
        /// <param name="dataActualizados"></param>
        /// <param name="dataOriginales"></param>
        private string GenerarArchivoExcelNotificacionObservacion(string path,
            string[][] result, string[][] originales, out string nombreArchivo)
        {
            string resultado = string.Empty;
            nombreArchivo = string.Empty;
            try
            {
                string plantilla = ConstantesCalculoResarcimiento.PlantillaNotificacionObservacion;
                string fileName = String.Format(ConstantesCalculoResarcimiento.ArchivoNotificacionObservacion, DateTime.Now.ToString("ddMMyyyyHHmmss"));

                nombreArchivo = fileName;

                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + fileName);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Interrupciones Actualizadas"];
                    ExcelRange rg = null;
                    int index = 4;
                    int colmax = 38;
                    int filaOrigen = index;
                    if (originales != null)
                    {
                        rg = ws.Cells[filaOrigen, 1, filaOrigen, colmax];
                        rg.Merge = true;
                        ws.Cells[filaOrigen, 1].Value = "Interrupciones Actualizadas";
                        rg.Style.Font.Size = 9;
                        rg.Style.Font.Bold = true;
                        index++;
                    }
                    int t = 0;
                    foreach (string[] data in result)
                    {
                        int col = 1;
                        for (int k = 2; k < colmax; k++)
                        {
                            ws.Cells[index, col].Value = data[k];

                            if (originales != null)
                            {
                                if (data[k] != originales[t][k])
                                {
                                    rg = ws.Cells[index, col, index, col];
                                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));
                                }
                            }

                            col++;
                        }
                        t++;
                        index++;
                    }

                    rg = ws.Cells[filaOrigen, 1, index - 1, colmax - 2];
                    rg.Style.Font.Size = 9;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    if (originales != null)
                    {
                        int indexNuevo = index + 1;
                        index++;
                        rg = ws.Cells[index, 1, index, colmax];
                        rg.Merge = true;
                        ws.Cells[index, 1].Value = "Interrupciones Originales";

                        rg.Style.Font.Size = 9;
                        rg.Style.Font.Bold = true;
                        index++;


                        foreach (string[] data in originales)
                        {
                            int col = 1;
                            for (int k = 2; k < colmax; k++)
                            {
                                ws.Cells[index, col].Value = data[k];

                                col++;
                            }

                            index++;
                        }

                        rg = ws.Cells[indexNuevo, 1, index - 1, colmax - 2];
                        rg.Style.Font.Size = 9;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    }

                    xlPackage.Save();

                    resultado = path + fileName;
                }

                return resultado;
            }
            catch (Exception)
            {
                return resultado;
            }
        }

        #endregion

        /// <summary>
        /// Permite generar el archivo comprimido con todos los archivos de la interrupcion
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="suministrador"></param>
        /// <param name="barra"></param>
        /// <param name="causaInterrupcion"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="evento"></param>
        /// <param name="alimentadorSED"></param>
        /// <param name="estado"></param>
        /// <param name="responsable"></param>
        /// <param name="disposicion"></param>
        /// <param name="compensacion"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        public Stream GenerarArchivoConsolidadoComprimido(int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
           string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
           string estado, int responsable, string disposicion, string compensacion, string buscar, out string archivoResultado)
        {
            archivoResultado = string.Empty;
            try
            {
                if (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion)
                {
                    RePeriodoDTO entityPeriodo = FactorySic.GetRePeriodoRepository().GetById(periodo);
                    List<ReInterrupcionSuministroDTO> entitys = FactorySic.GetReInterrupcionSuministroRepository().
                        ObtenerConsolidado(periodo, suministrador, causaInterrupcion, estado, barra, ConstantesAppServicio.SI, responsable, disposicion, compensacion);
                    string[][] data = UtilCalculoResarcimiento.ObtenerConsolidadoInterrupciones(entitys, conformidadResponsable, conformidadSuministrador, buscar);

                    string folderPrincipal = entityPeriodo.Repernombre;

                    if (FileServer.VerificarExistenciaDirectorio(ConstantesCalculoResarcimiento.RutaResarcimientos + folderPrincipal,
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                    {
                        FileServer.DeleteFolderAlter(ConstantesCalculoResarcimiento.RutaResarcimientos + folderPrincipal,
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                    }

                    FileServer.CreateFolder(ConstantesCalculoResarcimiento.RutaResarcimientos, folderPrincipal,
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                    // Generamos el archivo comprimido

                    foreach (string[] item in data)
                    {
                        //- Creamos el folder
                        string folderName = item[0].Split('-')[0];
                        bool flagCreacion = false;
                        //- Evidencia interrupcion
                        if (!string.IsNullOrEmpty(item[43]))
                        {
                            string fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, folderName, item[43]);

                            if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileName,
                                ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                            {
                                flagCreacion = true;
                            }
                        }


                        for (int i = 1; i <= 5; i++)
                        {
                            //- Existe sustento observacion o respuesta
                            if (!string.IsNullOrEmpty(item[48 + (i - 1) * 8]) || !string.IsNullOrEmpty(item[51 + (i - 1) * 8]))
                                flagCreacion = true;
                        }


                        if (flagCreacion)
                        {

                            FileServer.CreateFolder(ConstantesCalculoResarcimiento.RutaResarcimientos + "/" + folderPrincipal, folderName,
                                ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                            string folderInterrupcion = folderPrincipal + "/" + folderName + "/";

                            if (!string.IsNullOrEmpty(item[43]))
                            {
                                //- Creamos el archivo sustento
                                string fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, folderName, item[43]);
                                FileServer.CopiarFileDirectory(ConstantesCalculoResarcimiento.RutaResarcimientos, fileName, folderInterrupcion + fileName,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                            }
                            for (int i = 1; i <= 5; i++)
                            {
                                //- Existe sustento observacion o respuesta
                                if (!string.IsNullOrEmpty(item[48 + (i - 1) * 8]) || !string.IsNullOrEmpty(item[51 + (i - 1) * 8]))
                                {
                                    int idDetalle = this.ObtenerIdDetalleInterrupcion(int.Parse(folderName), i);

                                    string empresa = item[20 + (i - 1) * 4];
                                    //- Creamos el folder del resposable
                                    string folderResponsable = "RES" + i + "_" + (empresa.Length > 10 ? empresa.Substring(0, 10) : empresa);
                                    FileServer.CreateFolder(ConstantesCalculoResarcimiento.RutaResarcimientos + "/" + folderInterrupcion, folderResponsable, ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                                    string folderEmpresa = folderInterrupcion + "/" + folderResponsable + "/";

                                    if (!string.IsNullOrEmpty(item[48 + (i - 1) * 8]))
                                    {
                                        //- Creamos el archivo de la observación
                                        string fileObservacion = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, idDetalle.ToString(), item[48 + (i - 1) * 8]);
                                        FileServer.CopiarFileDirectory(ConstantesCalculoResarcimiento.RutaResarcimientos, fileObservacion, folderEmpresa + fileObservacion,
                                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                    }

                                    if (!string.IsNullOrEmpty(item[51 + (i - 1) * 8]))
                                    {
                                        string fileRespuesta = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDetalle.ToString(), item[51 + (i - 1) * 8]);
                                        //- Creamos el archivo de la respuesta
                                        FileServer.CopiarFileDirectory(ConstantesCalculoResarcimiento.RutaResarcimientos, fileRespuesta, folderEmpresa + fileRespuesta,
                                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                    }
                                }
                            }

                        }
                    }

                    //- Debemos eliminar el archivo zip generado

                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, folderPrincipal + ".zip",
                          ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                    {
                        FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos + folderPrincipal + ".zip",
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                    }

                    //- Verificamos si el folder tiene contenido
                    FileServer.ComprimirDirectorio(ConstantesCalculoResarcimiento.RutaResarcimientos, folderPrincipal, folderPrincipal + ".zip",
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                    archivoResultado = folderPrincipal + ".zip";
                    return FileServer.DownloadToStream(ConstantesCalculoResarcimiento.RutaResarcimientos + folderPrincipal + ".zip",
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite exportar el listado de errores
        /// </summary>
        /// <param name="errores"></param>
        /// <returns></returns>
        public int ExportarErroresInterrupcion(string[][] errores, string path, string filename)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("ERRORES");

                    if (ws != null)
                    {
                        int index = 1;

                        ws.Cells[index, 1].Value = "FILA";
                        ws.Cells[index, 2].Value = "COLUMNA";
                        ws.Cells[index, 3].Value = "ERROR";

                        ExcelRange rg = ws.Cells[index, 1, index, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 2;
                        foreach (string[] item in errores)
                        {
                            ws.Cells[index, 1].Value = item[0];
                            ws.Cells[index, 2].Value = item[1];
                            ws.Cells[index, 3].Value = item[2];
                            rg = ws.Cells[index, 1, index, 3];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[2, 1, index - 1, 3];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        rg = ws.Cells[1, 1, index, 3];
                        rg.AutoFitColumns();
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        #region Suministradores por punto de entrega y interrupciones osinergmin

        /// <summary>
        /// Permite obtener los suministradores por punto de entrega
        /// </summary>
        /// <param name="idPtoEntrega"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public List<RePtoentregaSuministradorDTO> ObtenerSuministradoresPorPuntoEntrega(int idPtoEntrega, int idPeriodo)
        {
            return FactorySic.GetRePtoentregaSuministradorRepository().ObtenerPorPuntoEntregaPeriodo(idPtoEntrega, idPeriodo);
        }

        /// <summary>
        /// Permite grabar los puntos de entrega
        /// </summary>
        /// <param name="idPtoEntrega"></param>
        /// <param name="periodo"></param>
        /// <param name="suministradores"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarPtoEntregaSuministrador(int idPtoEntrega, int periodo, string suministradores, string username)
        {
            try
            {
                FactorySic.GetRePtoentregaSuministradorRepository().Delete(idPtoEntrega, periodo);
                if (!string.IsNullOrEmpty(suministradores))
                {
                    List<int> ids = suministradores.Split(',').Select(int.Parse).ToList();

                    foreach (int id in ids)
                    {
                        RePtoentregaSuministradorDTO entity = new RePtoentregaSuministradorDTO();
                        entity.Emprcodi = id;
                        entity.Repentcodi = idPtoEntrega;
                        entity.Repercodi = periodo;
                        entity.Reptsmfeccreacion = entity.Reptsmfecmodificacion = DateTime.Now;
                        entity.Reptsmusucreacion = entity.Reptsmusumodificacion = username;
                        FactorySic.GetRePtoentregaSuministradorRepository().Save(entity);
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite exportar el formato de carga de suministradores por punto de entrega
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int ExportarSuministradorPorPuntoEntregaPeriodo(int idPeriodo, string path, string filename)
        {
            try
            {
                string plantilla = ConstantesCalculoResarcimiento.PlantillaCargaPtoEntregaSuministrador;
                List<RePtoentregaPeriodoDTO> puntosEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodo);
                List<ReEmpresaDTO> suministradores = this.ObtenerEmpresasSuministradorasTotal();
                List<RePtoentregaSuministradorDTO> entitys = FactorySic.GetRePtoentregaSuministradorRepository().GetByCriteria(idPeriodo);

                string file = path + filename;
                FileInfo newFile = new FileInfo(file);
                FileInfo template = new FileInfo(path + plantilla);


                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets["PtoEntregaSuministrador"];

                    if (ws != null)
                    {
                        int index = 2;
                        ExcelRange rg = null;

                        foreach (RePtoentregaSuministradorDTO item in entitys)
                        {
                            ws.Cells[index, 1].Value = item.Repentnombre;
                            ws.Cells[index, 2].Value = item.Emprnomb;

                            index++;
                        }

                        if (index == 2) index++;

                        rg = ws.Cells[2, 1, index - 1, 2];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    }

                    ExcelWorksheet wsData = xlPackage.Workbook.Worksheets["Data"];

                    if (wsData != null)
                    {
                        int index = 2;

                        foreach (ReEmpresaDTO item in suministradores)
                        {
                            wsData.Cells[index, 1].Value = item.Emprnomb;
                            index++;
                        }

                        index = 2;

                        foreach (RePtoentregaPeriodoDTO item in puntosEntrega)
                        {
                            wsData.Cells[index, 2].Value = item.Repentnombre;
                            index++;
                        }
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite almacenar los puntos de entrega
        /// </summary>
        /// <param name="entitys"></param>
        public void CargarMasivoSuministradoresPorPuntoEntrega(List<RePtoentregaSuministradorDTO> entitys, int periodo, string username, out List<string> validaciones)
        {
            List<RePtoentregaPeriodoDTO> puntosEntrega = this.ObtenerPtoEntregaPorPeriodo(periodo);
            List<ReEmpresaDTO> suministradores = this.ObtenerEmpresasSuministradorasTotal();

            List<string> errores = new List<string>();
            bool flag = true;
            List<RePtoentregaSuministradorDTO> result = new List<RePtoentregaSuministradorDTO>();

            int fila = 2;
            foreach (RePtoentregaSuministradorDTO entity in entitys)
            {
                RePtoentregaPeriodoDTO puntoEntrega = puntosEntrega.Where(x => x.Repentnombre == entity.Repentnombre).FirstOrDefault();
                ReEmpresaDTO suministrador = suministradores.Where(x => x.Emprnomb == entity.Emprnomb).FirstOrDefault();

                if (puntoEntrega != null && suministrador != null)
                {
                    RePtoentregaSuministradorDTO item = new RePtoentregaSuministradorDTO();
                    item.Emprcodi = suministrador.Emprcodi;
                    item.Repentcodi = puntoEntrega.Repentcodi;
                    item.Repercodi = periodo;
                    item.Reptsmusucreacion = item.Reptsmusumodificacion = username;
                    item.Reptsmfeccreacion = item.Reptsmfecmodificacion = DateTime.Now;
                    result.Add(item);
                }
                else
                {
                    if (puntoEntrega == null)
                        errores.Add("El punto de entrega " + entity.Repentnombre + " de la fila " + fila + " no se encuentra en la configuración del periodo.");
                    if (suministrador == null)
                        errores.Add("El suministrador " + entity.Emprnomb + " de la fila " + fila + " no existe en la base de datos.");

                    flag = false;
                }

                fila++;
            }

            if (flag)
            {
                FactorySic.GetRePtoentregaSuministradorRepository().EliminarPorPeriodo(periodo);
                foreach (RePtoentregaSuministradorDTO item in result)
                {
                    FactorySic.GetRePtoentregaSuministradorRepository().Save(item);
                }
            }

            validaciones = errores;
        }

        /// <summary>
        /// Permite obtener la estructura de insumo de interrupciones
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public EstructuraInterrupcionInsumo ObtenerEstructuraInsumoInterrupciones(int idPeriodo, bool flag)
        {
            EstructuraInterrupcionInsumo result = new EstructuraInterrupcionInsumo();

            result.Data = this.ObtenerDataInterrupcionesInsumo(idPeriodo, flag);
            result.Result = 1;
            result.ListaCliente = this.ObtenerEmpresas();
            result.ListaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
            result.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().List();
            result.ListaEmpresa = this.ObtenerEmpresas();
            int idPeriodoPadre = this.ObtenerIdPeriodoPadre(idPeriodo);
            result.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);
            result.ListaSuministradores = this.ObtenerEmpresasSuministradorasTotal();

            return result;
        }

        /// <summary>
        /// Permite obtener la data de interrupciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public string[][] ObtenerDataInterrupcionesInsumo(int idPeriodo, bool flag)
        {
            List<string[]> result = new List<string[]>();

            List<ReInterrupcionInsumoDTO> entitys = FactorySic.GetReInterrupcionInsumoRepository().ObtenerPorPeriodo(idPeriodo);

            foreach (ReInterrupcionInsumoDTO entity in entitys)
            {
                string[] data = new string[24];

                data[0] = entity.Reinincodi.ToString(); //-0 Id    
                data[1] = entity.Repentcodi.ToString(); //-1 Punto Entrega
                data[2] = ((DateTime)entity.Reininifecinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //-2 Fecha ejecutado ini  --
                data[3] = ((DateTime)entity.Reininfecfin).ToString(ConstantesAppServicio.FormatoFechaFull2); //-3 Fecha ejecutado fin
                data[4] = entity.Reninitipo;//-4 Tipo de Interrupción
                data[5] = entity.Reninicausa;//-5 Causa Interrupcion
                data[6] = entity.Reinincodosi;//-6 Codigo OSI
                data[7] = entity.Reinincliente.ToString();//-7 Cliente
                data[8] = entity.Reininsuministrador.ToString();//-8 Suministrador
                data[9] = entity.Reininobservacion;//-9 Observacion
                data[10] = (entity.Reininresponsable1 != null) ? entity.Reininresponsable1.ToString() : string.Empty; //-10 Empresa 1  --
                data[11] = (entity.Reininporcentaje1 != null) ? entity.Reininporcentaje1.ToString() : string.Empty;//-11 Porcentaje 1
                data[12] = (entity.Reininresponsable2 != null) ? entity.Reininresponsable2.ToString() : string.Empty; //-12 Empresa 2   --
                data[13] = (entity.Reininporcentaje2 != null) ? entity.Reininporcentaje2.ToString() : string.Empty;//-13 Porcentaje 2 
                data[14] = (entity.Reininresponsable3 != null) ? entity.Reininresponsable3.ToString() : string.Empty; //-14 Empresa 3   --
                data[15] = (entity.Reininporcentaje3 != null) ? entity.Reininporcentaje3.ToString() : string.Empty;//-15 Porcentaje 3
                data[16] = (entity.Reininresponsable4 != null) ? entity.Reininresponsable4.ToString() : string.Empty; //-16 Empresa 4   --
                data[17] = (entity.Reininporcentaje4 != null) ? entity.Reininporcentaje4.ToString() : string.Empty;//-17 Porcentaje 4
                data[18] = (entity.Reininresponsable5 != null) ? entity.Reininresponsable5.ToString() : string.Empty; //-18 Empresa 5   --
                data[19] = (entity.Reininporcentaje5 != null) ? entity.Reininporcentaje5.ToString() : string.Empty;//-19 Porcentaje 5
                data[20] = (entity.Reininprogifecinicio != null) ?
                  ((DateTime)entity.Reininprogifecinicio).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;//-20 Fecha programado ini  --
                data[21] = (entity.Reininprogfecfin != null) ?
                  ((DateTime)entity.Reininprogfecfin).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;//-21 Fecha programado fin
                data[22] = entity.Retintcodi.ToString();//-22 Tipo interrupcion
                data[23] = entity.Recintcodi.ToString();//-23 Causa interrupcion

                result.Add(data);
            }

            if (flag)
            {
                if (entitys.Count == 0)
                {
                    for (int i = 0; i <= 20; i++)
                    {
                        string[] data = new string[24];
                        for (int j = 0; j < 24; j++)
                        {
                            data[j] = string.Empty;
                        }
                        result.Add(data);
                    }
                }
            }

            return result.ToArray();
        }


        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <param name="template"></param>
        /// <param name="file"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public int GenerarFormatoInterrupcionesInsumo(string path, string plantilla, string file, int idPeriodo)
        {
            try
            {
                EstructuraInterrupcionInsumo result = this.ObtenerEstructuraInsumoInterrupciones(idPeriodo, false);
                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + file);
                }


                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet wsData = xlPackage.Workbook.Worksheets["Data"];
                    ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];

                    int index = 2;
                    foreach (ReEmpresaDTO item in result.ListaCliente)
                    {
                        wsData.Cells[index, 1].Value = item.Emprnomb;
                        index++;
                    }

                    index = 2;
                    foreach (RePtoentregaPeriodoDTO item in result.ListaPuntoEntrega)
                    {
                        wsData.Cells[index, 4].Value = item.Repentnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReTipoInterrupcionDTO item in result.ListaTipoInterrupcion)
                    {
                        wsData.Cells[index, 5].Value = item.Retintnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReCausaInterrupcionDTO item in result.ListaCausaInterrupcion)
                    {
                        wsData.Cells[index, 6].Value = item.Recintnombre;
                        index++;
                    }

                    index = 2;
                    foreach (ReEmpresaDTO item in result.ListaEmpresa)
                    {
                        wsData.Cells[index, 2].Value = item.Emprnomb;
                        index++;
                    }

                    index = 2;
                    foreach (ReEmpresaDTO item in result.ListaSuministradores)
                    {
                        wsData.Cells[index, 3].Value = item.Emprnomb;
                        index++;
                    }

                    index = 3;
                    foreach (string[] data in result.Data)
                    {
                        wsInterrupcion.Cells[index, 1].Value = data[0]; //- Id
                        wsInterrupcion.Cells[index, 2].Value = result.ListaPuntoEntrega.Where(x => x.Repentcodi == int.Parse(data[1])).First().Repentnombre;
                        wsInterrupcion.Cells[index, 3].Value = data[2]; //- Tiempo inicio ejecutado
                        wsInterrupcion.Cells[index, 4].Value = data[3]; //- Tiempo fin ejecutado
                        wsInterrupcion.Cells[index, 5].Value = data[4]; //- Tipo 
                        wsInterrupcion.Cells[index, 6].Value = data[5]; //- Causa
                        wsInterrupcion.Cells[index, 7].Value = data[6]; //- Codosi                    
                        wsInterrupcion.Cells[index, 8].Value = result.ListaCliente.Where(x => x.Emprcodi == int.Parse(data[7])).First().Emprnomb; // cliente
                        wsInterrupcion.Cells[index, 9].Value = result.ListaSuministradores.Where(x => x.Emprcodi == int.Parse(data[8])).First().Emprnomb; //- Suministrador
                        wsInterrupcion.Cells[index, 10].Value = data[9]; //- observacion
                        if (!string.IsNullOrEmpty(data[10]))
                            wsInterrupcion.Cells[index, 11].Value = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[10])).First().Emprnomb; //- Empresa 1
                        wsInterrupcion.Cells[index, 12].Value = data[11]; //- Porcentaje 1
                        if (!string.IsNullOrEmpty(data[12]))
                            wsInterrupcion.Cells[index, 13].Value = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[12])).First().Emprnomb; //- Empresa 2
                        wsInterrupcion.Cells[index, 14].Value = data[13]; //- Porcentaje 2
                        if (!string.IsNullOrEmpty(data[14]))
                            wsInterrupcion.Cells[index, 15].Value = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[14])).First().Emprnomb; //- Empresa 3
                        wsInterrupcion.Cells[index, 16].Value = data[15]; //- Porcentaje 2
                        if (!string.IsNullOrEmpty(data[16]))
                            wsInterrupcion.Cells[index, 17].Value = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[16])).First().Emprnomb; //- Empresa 4
                        wsInterrupcion.Cells[index, 18].Value = data[17]; //- Porcentaje 4
                        if (!string.IsNullOrEmpty(data[18]))
                            wsInterrupcion.Cells[index, 19].Value = result.ListaEmpresa.Where(x => x.Emprcodi == int.Parse(data[18])).First().Emprnomb; //- Empresa 5
                        wsInterrupcion.Cells[index, 20].Value = data[19]; //- Porcentaje 5
                        wsInterrupcion.Cells[index, 21].Value = data[20]; //- Tiempo inicio programado
                        wsInterrupcion.Cells[index, 22].Value = data[21]; //- Tiempo fin programado
                        wsInterrupcion.Cells[index, 23].Value = result.ListaTipoInterrupcion.Where(x => x.Retintcodi == int.Parse(data[22])).First().Retintnombre; //- Tipo de interrupcion
                        wsInterrupcion.Cells[index, 24].Value = result.ListaCausaInterrupcion.Where(x => x.Recintcodi == int.Parse(data[23])).First().Recintnombre; //- Causa de interrupcion


                        index++;
                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }


        /// <summary>
        /// Carga los datos a partir de excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>        
        /// <param name="periodo"></param>
        /// <param name="validaciones"></param>
        /// <returns></returns>
        public string[][] CargarInterrupcionesInsumosExcel(string path, string file, int periodo, out List<string> validaciones)
        {
            List<string[]> result = new List<string[]>();
            FileInfo fileInfo = new FileInfo(path + file);
            List<string> errores = new List<string>();

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];

                if (wsInterrupcion != null)
                {
                    //-- Verificar columnas 
                    List<string> columnas = UtilCalculoResarcimiento.ObtenerEstructuraPlantillaInterrupcionesInsumo(path);
                    bool flag = true;
                    for (int i = 1; i <= 24; i++)
                    {
                        string header = (wsInterrupcion.Cells[1, i].Value != null) ? wsInterrupcion.Cells[1, i].Value.ToString() : string.Empty;
                        if (header != columnas[i - 1])
                        {
                            errores.Add("La plantilla ha sido modificada. No puede cambiar o eliminar columnas del Excel.");
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        EstructuraInterrupcionInsumo maestros = new EstructuraInterrupcionInsumo();
                        maestros.ListaCliente = this.ObtenerEmpresas();
                        maestros.ListaTipoInterrupcion = FactorySic.GetReTipoInterrupcionRepository().List();
                        maestros.ListaCausaInterrupcion = FactorySic.GetReCausaInterrupcionRepository().List();
                        maestros.ListaSuministradores = this.ObtenerEmpresasSuministradorasTotal();
                        maestros.ListaEmpresa = this.ObtenerEmpresas();
                        int idPeriodoPadre = this.ObtenerIdPeriodoPadre(periodo);
                        maestros.ListaPuntoEntrega = this.ObtenerPtoEntregaPorPeriodo(idPeriodoPadre);


                        for (int index = 2; index <= 1200; index++)
                        {
                            if (
                                (wsInterrupcion.Cells[index, 1].Value != null && wsInterrupcion.Cells[index, 1].Value != string.Empty) ||
                                (wsInterrupcion.Cells[index, 2].Value != null && wsInterrupcion.Cells[index, 2].Value != string.Empty))
                            {
                                string[] data = new string[24];


                                data[0] = (wsInterrupcion.Cells[index, 1].Value != null) ? wsInterrupcion.Cells[index, 1].Value.ToString() : string.Empty; //- Id

                                string ptoEntrega = string.Empty;
                                if (wsInterrupcion.Cells[index, 2].Value != null)
                                {
                                    RePtoentregaPeriodoDTO rePtoEntrega = maestros.ListaPuntoEntrega.Where(x => x.Repentnombre == wsInterrupcion.Cells[index, 2].Value.ToString()).FirstOrDefault();
                                    if (rePtoEntrega != null) ptoEntrega = rePtoEntrega.Repentcodi.ToString();
                                }
                                data[1] = ptoEntrega; //- Punto de entrega
                                data[2] = (wsInterrupcion.Cells[index, 3].Value != null) ? wsInterrupcion.Cells[index, 3].Value.ToString() : string.Empty; //- Tiempo inicio ejecutado
                                data[3] = (wsInterrupcion.Cells[index, 4].Value != null) ? wsInterrupcion.Cells[index, 4].Value.ToString() : string.Empty; //- Tiempo fin ejecutado
                                data[4] = (wsInterrupcion.Cells[index, 5].Value != null) ? wsInterrupcion.Cells[index, 5].Value.ToString() : string.Empty; //- Tipo
                                data[5] = (wsInterrupcion.Cells[index, 6].Value != null) ? wsInterrupcion.Cells[index, 6].Value.ToString() : string.Empty; //- Causa
                                data[6] = (wsInterrupcion.Cells[index, 7].Value != null) ? wsInterrupcion.Cells[index, 7].Value.ToString() : string.Empty; //- Codosi

                                string cliente = string.Empty;
                                if (wsInterrupcion.Cells[index, 8].Value != null)
                                {
                                    ReEmpresaDTO reCliente = maestros.ListaCliente.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 8].Value.ToString()).FirstOrDefault();
                                    if (reCliente != null) cliente = reCliente.Emprcodi.ToString();
                                }
                                data[7] = cliente; //- Cliente

                                string suministrador = string.Empty;
                                if (wsInterrupcion.Cells[index, 9].Value != null)
                                {
                                    ReEmpresaDTO reSuministrador = maestros.ListaSuministradores.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 9].Value.ToString()).FirstOrDefault();
                                    if (reSuministrador != null) suministrador = reSuministrador.Emprcodi.ToString();
                                }
                                data[8] = suministrador; //- Suministrador
                                data[9] = (wsInterrupcion.Cells[index, 10].Value != null) ? wsInterrupcion.Cells[index, 10].Value.ToString() : string.Empty; //- Observacion


                                string emprcodi1 = string.Empty;
                                if (wsInterrupcion.Cells[index, 11].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 11].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi1 = reEmpresa.Emprcodi.ToString();
                                }

                                data[10] = emprcodi1; //- Empresa 1
                                data[11] = (wsInterrupcion.Cells[index, 12].Value != null) ? wsInterrupcion.Cells[index, 12].Value.ToString() : string.Empty; //- Porcentaje 1


                                string emprcodi2 = string.Empty;
                                if (wsInterrupcion.Cells[index, 13].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 13].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi2 = reEmpresa.Emprcodi.ToString();
                                }
                                data[12] = emprcodi2;//- Empresa 2
                                data[13] = (wsInterrupcion.Cells[index, 14].Value != null) ? wsInterrupcion.Cells[index, 14].Value.ToString() : string.Empty; //- Porcentaje 2


                                string emprcodi3 = string.Empty;
                                if (wsInterrupcion.Cells[index, 15].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 15].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi3 = reEmpresa.Emprcodi.ToString();
                                }
                                data[14] = emprcodi3; //- Empresa 3
                                data[15] = (wsInterrupcion.Cells[index, 16].Value != null) ? wsInterrupcion.Cells[index, 16].Value.ToString() : string.Empty; //- Porcentaje 3


                                string emprcodi4 = string.Empty;
                                if (wsInterrupcion.Cells[index, 17].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 17].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi4 = reEmpresa.Emprcodi.ToString();
                                }
                                data[16] = emprcodi4; //- Empresa 4
                                data[17] = (wsInterrupcion.Cells[index, 18].Value != null) ? wsInterrupcion.Cells[index, 18].Value.ToString() : string.Empty; //- Porcentaje 4


                                string emprcodi5 = string.Empty;
                                if (wsInterrupcion.Cells[index, 19].Value != null)
                                {
                                    ReEmpresaDTO reEmpresa = maestros.ListaEmpresa.Where(x => x.Emprnomb == wsInterrupcion.Cells[index, 19].Value.ToString()).FirstOrDefault();
                                    if (reEmpresa != null) emprcodi5 = reEmpresa.Emprcodi.ToString();
                                }
                                data[18] = emprcodi5; //- Empresa 5
                                data[19] = (wsInterrupcion.Cells[index, 20].Value != null) ? wsInterrupcion.Cells[index, 20].Value.ToString() : string.Empty; //- Porcentaje 5
                                data[20] = (wsInterrupcion.Cells[index, 21].Value != null) ? wsInterrupcion.Cells[index, 21].Value.ToString() : string.Empty; //- Tiempo inicio programado
                                data[21] = (wsInterrupcion.Cells[index, 22].Value != null) ? wsInterrupcion.Cells[index, 22].Value.ToString() : string.Empty; //- Tiempo fin programado

                                string tipoInterrupcion = string.Empty;
                                if (wsInterrupcion.Cells[index, 23].Value != null)
                                {
                                    ReTipoInterrupcionDTO reTipoInterrupcion = maestros.ListaTipoInterrupcion.Where(x => x.Retintnombre == wsInterrupcion.Cells[index, 23].Value.ToString()).FirstOrDefault();
                                    if (reTipoInterrupcion != null) tipoInterrupcion = reTipoInterrupcion.Retintcodi.ToString();
                                }
                                data[22] = tipoInterrupcion; //- Tipo de interrupcion

                                string causaInterrupcion = string.Empty;
                                if (wsInterrupcion.Cells[index, 24].Value != null)
                                {
                                    ReCausaInterrupcionDTO reCausaInterrupcion = maestros.ListaCausaInterrupcion.Where(x => x.Recintnombre == wsInterrupcion.Cells[index, 24].Value.ToString()).FirstOrDefault();
                                    if (reCausaInterrupcion != null) causaInterrupcion = reCausaInterrupcion.Recintcodi.ToString();
                                }
                                data[23] = causaInterrupcion; //- Causa de interrupcion


                                result.Add(data);
                            }
                        }
                    }
                }
                else
                {
                    errores.Add("No existe la hoja 'Interrupciones' en el libro Excel.");
                }
            }
            validaciones = errores;
            return result.ToArray();
        }


        /// <summary>
        /// Permite grabar las interrupciones
        /// </summary>
        /// <param name="data"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="username"></param>
        /// <param name="comentario"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public EstructuraGrabado GrabarInterrupcionesInsumo(string[][] data, int periodo, string username)
        {
            EstructuraGrabado result = new EstructuraGrabado();
            try
            {
                List<ReInterrupcionInsumoDTO> entitys = new List<ReInterrupcionInsumoDTO>();

                for (int i = 2; i < data.Length; i++)
                {
                    if (data[i][0] == string.Empty || data[i][1] == string.Empty) break;

                    ReInterrupcionInsumoDTO entity = new ReInterrupcionInsumoDTO();
                    entity.Reinincodi = (!string.IsNullOrEmpty(data[i][0])) ? int.Parse(data[i][0]) : 0;
                    entity.Repentcodi = int.Parse(data[i][1]);
                    entity.Repercodi = periodo;
                    entity.Reininifecinicio = (!string.IsNullOrEmpty(data[i][2])) ?
                        (DateTime?)DateTime.ParseExact(data[i][2], ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture) : null;
                    entity.Reininfecfin = (!string.IsNullOrEmpty(data[i][3])) ?
                        (DateTime?)DateTime.ParseExact(data[i][3], ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture) : null;
                    entity.Reninitipo = data[i][4];
                    entity.Reninicausa = data[i][5];
                    entity.Reinincodosi = data[i][6];
                    entity.Reinincliente = int.Parse(data[i][7]);
                    entity.Reininsuministrador = int.Parse(data[i][8]);
                    entity.Reininobservacion = data[i][9];
                    entity.Reininresponsable1 = (!string.IsNullOrEmpty(data[i][10])) ? (int?)int.Parse(data[i][10]) : null;
                    entity.Reininporcentaje1 = (!string.IsNullOrEmpty(data[i][11])) ? (decimal?)decimal.Parse(data[i][11]) : null;
                    entity.Reininresponsable2 = (!string.IsNullOrEmpty(data[i][12])) ? (int?)int.Parse(data[i][12]) : null;
                    entity.Reininporcentaje2 = (!string.IsNullOrEmpty(data[i][13])) ? (decimal?)decimal.Parse(data[i][13]) : null;
                    entity.Reininresponsable3 = (!string.IsNullOrEmpty(data[i][14])) ? (int?)int.Parse(data[i][14]) : null;
                    entity.Reininporcentaje3 = (!string.IsNullOrEmpty(data[i][15])) ? (decimal?)decimal.Parse(data[i][15]) : null;
                    entity.Reininresponsable4 = (!string.IsNullOrEmpty(data[i][16])) ? (int?)int.Parse(data[i][16]) : null;
                    entity.Reininporcentaje4 = (!string.IsNullOrEmpty(data[i][17])) ? (decimal?)decimal.Parse(data[i][17]) : null;
                    entity.Reininresponsable5 = (!string.IsNullOrEmpty(data[i][18])) ? (int?)int.Parse(data[i][18]) : null;
                    entity.Reininporcentaje5 = (!string.IsNullOrEmpty(data[i][19])) ? (decimal?)decimal.Parse(data[i][19]) : null;
                    entity.Reininprogifecinicio = (!string.IsNullOrEmpty(data[i][20])) ?
                            (DateTime?)DateTime.ParseExact(data[i][20], ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture) : null;
                    entity.Reininprogfecfin = (!string.IsNullOrEmpty(data[i][21])) ?
                        (DateTime?)DateTime.ParseExact(data[i][21], ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture) : null;
                    entity.Retintcodi = int.Parse(data[i][22]);
                    entity.Recintcodi = int.Parse(data[i][23]);

                    entitys.Add(entity);
                }

                FactorySic.GetReInterrupcionInsumoRepository().Delete(periodo);

                foreach (ReInterrupcionInsumoDTO entity in entitys)
                {
                    entity.Reininusucreacion = entity.Reininusumodificacion = username;
                    entity.Reininfeccreacion = entity.Reininfecmodificacion = DateTime.Now;
                    FactorySic.GetReInterrupcionInsumoRepository().Save(entity);
                }

                result.Result = 1;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result.Result = -1;
            }

            return result;
        }

        /// <summary>
        /// Permite obtener la configuración de los tipos de interrupcion
        /// </summary>
        /// <returns></returns>
        public List<ReTipoInterrupcionDTO> ObtenerConfiguracionTiposInterrupcion()
        {
            return FactorySic.GetReTipoInterrupcionRepository().ObtenerConfiguracion();
        }

        /// <summary>
        /// Lista la configuracion de causas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ReCausaInterrupcionDTO> ObtenerConfiguracionCausasInterrupcion(int id)
        {
            return FactorySic.GetReCausaInterrupcionRepository().ObtenerConfiguracion(id);
        }

        /// <summary>
        /// Obtiene el registro de un tipo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReTipoInterrupcionDTO ObtenerRegistroTipoInterrupcion(int id)
        {
            return FactorySic.GetReTipoInterrupcionRepository().GetById(id);
        }

        /// <summary>
        /// Obtiene el registro de una causa de int
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReCausaInterrupcionDTO ObtenerRegistroCausaInterrupcion(int id)
        {
            return FactorySic.GetReCausaInterrupcionRepository().GetById(id);
        }

        /// <summary>
        /// Permite eliminar un tipo de interruipcion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int EliminarTipoInterrupcion(int id)
        {
            try
            {
                FactorySic.GetReTipoInterrupcionRepository().Delete(id);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite eliminar una causa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int EliminarCausaInterrupcion(int id)
        {
            try
            {
                FactorySic.GetReCausaInterrupcionRepository().Delete(id);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite grabar o editar un tipo de interrupcion
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarTipoInterrupcion(ReTipoInterrupcionDTO entity, string username)
        {
            try
            {
                if (entity.Retintcodi == 0)
                {
                    entity.Retintusucreacion = entity.Retintusumodificacion = username;
                    entity.Retintfeccreacion = entity.Retintfecmodificacion = DateTime.Now;
                    FactorySic.GetReTipoInterrupcionRepository().Save(entity);
                }
                else
                {
                    entity.Retintusumodificacion = username;
                    entity.Retintfecmodificacion = DateTime.Now;
                    FactorySic.GetReTipoInterrupcionRepository().Update(entity);
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite grabar o editar una causa de interrupcion
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarCausaInterrupcion(ReCausaInterrupcionDTO entity, string username)
        {
            try
            {
                if (entity.Recintcodi == 0)
                {
                    entity.Recintusucreacion = entity.Recintusumodificacion = username;
                    entity.Recintfeccreacion = entity.Recintfecmodificacion = DateTime.Now;
                    FactorySic.GetReCausaInterrupcionRepository().Save(entity);
                }
                else
                {
                    entity.Recintusumodificacion = username;
                    entity.Recintfecmodificacion = DateTime.Now;
                    FactorySic.GetReCausaInterrupcionRepository().Update(entity);
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        #endregion


    }
}

