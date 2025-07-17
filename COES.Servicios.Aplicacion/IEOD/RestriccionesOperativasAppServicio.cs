using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.IEOD
{
    public class RestriccionesOperativasAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RestriccionesOperativasAppServicio));
        #region Métodos Tabla SI_EMPRESA
        /// <summary>
        /// Devuelve lista de empresa por tipo de empresa
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaCriteria(string strTipoempresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetByCriteria(strTipoempresa);
        }

        #endregion

        #region METODOS TABLA EQ_EQUIPO

        /// <summary>
        /// Genera los tipos de equipo para la empresa seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetListaTiposEquiposXEmpresa(int idEmpresa)
        {
            return FactorySic.GetEqEquipoRepository().ListadoXEmpresa(idEmpresa);
        }

        /// <summary>
        /// Lista de Equipos según el tipo de equipo seleccionado
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="famCodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetListaEquiposXFamilia(int idEmpresa, int famCodi)
        {
            return FactorySic.GetEqEquipoRepository().GetByEmprFam(idEmpresa, famCodi);
        }

        /// <summary>
        /// devuelve detalle de equipo seleccionado
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public EqEquipoDTO GetEquipoxId(int idEquipo)
        {
            return FactorySic.GetEqEquipoRepository().GetById(idEquipo);
        }

        #endregion

        #region METODOS TABLA EVE_IEODCUADRO

        /// <summary>
        /// Devuelve la lista de equipos filtrados por empresa y por tipo de central de generacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="iCodFamilias"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralesXEmpresaGener(int idEmpresa)
        {
            return FactorySic.GetEqEquipoRepository().CentralesXEmpresaHorasOperacion(idEmpresa);
        }

        public void UpdateRestriccion(EveIeodcuadroDTO entity)
        {
            FactorySic.GetEveIeodcuadroRepository().Update(entity);
        }

        public int SaveRestriccion(EveIeodcuadroDTO entity)
        {
            return FactorySic.GetEveIeodcuadroRepository().Save(entity);
        }

        public List<EveIeodcuadroDTO> GetListarEveIeodCuadroxEmpresa(DateTime fechaInicio, DateTime FechaFin, int subCausaCodi, int emprcodi)
        {
            return FactorySic.GetEveIeodcuadroRepository().ListarEveIeodCuadroxEmpresa(fechaInicio, FechaFin, subCausaCodi, emprcodi);
        }

        public EveIeodcuadroDTO GetByIdRestriccion(int iccodi)
        {
            return FactorySic.GetEveIeodcuadroRepository().GetById(iccodi);
        }

        public void DeleteLogicoRestriccion(int iccodi)
        {
            FactorySic.GetEveIeodcuadroRepository().BorradoLogico(iccodi);
        }

        public List<EveIeodcuadroDTO> GetListarRestricionesxCodigos(string pkCodis)
        {
            return FactorySic.GetEveIeodcuadroRepository().GetCriteriaxPKCodis(pkCodis);
        }

        #region PR5
        public List<EveIeodcuadroDTO> GetListarEveIeodCuadroxEmpresaxEquipos(DateTime fechaInicio, DateTime FechaFin, int subCausaCodi, string emprcodi, string equipos, int nPagina, int nRegistros)
        {
            return FactorySic.GetEveIeodcuadroRepository().ListarEveIeodCuadroxEmpresaxEquipos(fechaInicio, FechaFin, subCausaCodi, emprcodi, equipos, nPagina, nRegistros);
        }

        public int NroRegistrosConsultaRestricciones(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, string emprcodi, string equipos)
        {
            return FactorySic.GetEveIeodcuadroRepository().ObtenerNroElementosConsultaRestricciones(fechaIni, fechaFin, subcausaCodi, emprcodi, equipos);
        }

        public int ContarEveIeodCuadroxEmpresaxEquipos(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, string emprcodi, string equipos, string areacodi)
        {
            return FactorySic.GetEveIeodcuadroRepository().ContarEveIeodCuadroxEmpresaxEquipos(fechaIni, fechaFin, subcausaCodi, emprcodi, equipos, areacodi);
        }
        #endregion

        #endregion

        #region Métodos Tabla ME_ENVIO

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO
        /// </summary>
        public int SaveMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                return FactorySic.GetMeEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIO
        /// </summary>
        public void UpdateMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIO
        /// </summary>
        public void UpdateMeEnvio1(MeEnvioDTO entity)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().Update1(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_ENVIO
        /// </summary>
        public MeEnvioDTO GetByIdMeEnvio(int idEnvio)
        {
            return FactorySic.GetMeEnvioRepository().GetById(idEnvio);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio
        /// </summary>
        public List<MeEnvioDTO> GetByCriteriaMeEnvios(int idEmpresa, int idFormato, DateTime fecha)
        {
            return FactorySic.GetMeEnvioRepository().GetByCriteria(idEmpresa, idFormato, fecha);
        }


        /// <summary>
        /// Lista de Envios por paginado
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetListaMultipleMeEnvios(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin, int nroPaginas, int pageSize)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().GetListaMultiple(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin, nroPaginas, pageSize);
        }

        /// <summary>
        /// Lista de envios para consulta excel si paginado
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetListaMultipleMeEnviosXLS(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().GetListaMultipleXLS(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin);
        }

        /// <summary>
        /// Devuelve el total de registros para listado de envios por paginado
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public int TotalListaMultipleMeEnvios(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().TotalListaMultiple(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin);
        }

        /// <summary>
        /// Obtiene el maximo id del envio de un formato de todos los periodos
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public int ObtenerIdMaxEnvioFormato(int idFormato, int idEmpresa)
        {
            return FactorySic.GetMeEnvioRepository().GetMaxIdEnvioFormato(idFormato, idEmpresa);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio por rango de fechas
        /// </summary>
        public List<MeEnvioDTO> GetByCriteriaRangoFecha(int idEmpresa, int idFormato, DateTime fechaini, DateTime fechafin)
        {
            return FactorySic.GetMeEnvioRepository().GetByCriteriaRangoFecha(idEmpresa, idFormato, fechaini, fechafin);
        }

        /// <summary>
        /// Obtiene el maximo id del envio de un formato de todos los periodos
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public int ObtenerIdMaxEnvioFormatoPeriodo(int idFormato, int idEmpresa, DateTime periodo)
        {
            return FactorySic.GetMeEnvioRepository().GetByMaxEnvioFormatoPeriodo(idFormato, idEmpresa, periodo);
        }


        #endregion

        #region Métodos Tabla ME_ENVIODET

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIODET
        /// </summary>
        public void SaveMeEnviodet(MeEnviodetDTO entity)
        {
            try
            {
                FactorySic.GetMeEnviodetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIODET
        /// </summary>
        public void UpdateMeEnviodet(MeEnviodetDTO entity)
        {
            try
            {
                FactorySic.GetMeEnviodetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_ENVIODET
        /// </summary>
        public void DeleteMeEnviodet(int enviocodi, int fdatpkcodi)
        {
            try
            {
                FactorySic.GetMeEnviodetRepository().Delete(enviocodi, fdatpkcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_ENVIODET
        /// </summary>
        public MeEnviodetDTO GetByIdMeEnviodet(int enviocodi)
        {
            return FactorySic.GetMeEnviodetRepository().GetById(enviocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ENVIODET
        /// </summary>
        public List<MeEnviodetDTO> ListMeEnviodets()
        {
            return FactorySic.GetMeEnviodetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnviodet
        /// </summary>
        public List<MeEnviodetDTO> GetByCriteriaMeEnviodets(int enviocodi)
        {
            return FactorySic.GetMeEnviodetRepository().GetByCriteria(enviocodi);
        }

        #endregion

        #region METODOS GENERACION DE ARCHIVO EXCEL

        public const int MTU_PER_PIXEL = 9525;

        //Genera archivo excel de listado de archivos enviados
        public void GenerarArchivosEnviados(List<IeodCuadroDTO> lista, string ruta)
        {
            FileInfo template = new FileInfo(ruta + ConstantesRestricc.PlantillaExcelRestricciones);
            FileInfo newFile = new FileInfo(ruta + ConstantesRestricc.NombreReporteArchivosEnviados);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesRestricc.NombreReporteArchivosEnviados);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE RESTRICCIONES");
                ws = xlPackage.Workbook.Worksheets["RPTE RESTRICCIONES"];
                string titulo = "REPORTE HISTORICO RESTRICCIONES";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcel5(ws, lista);
                xlPackage.Save();
            }
        }

        ///****util*****
        /// <summary>
        /// Genera encabezados para reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="ruta"></param>
        public static void ConfigEncabezadoExcel(ExcelWorksheet ws, string titulo, string ruta)
        {
            AddImage(ws, 1, 0, ruta + ConstantesRestricc.NombreLogoCoes);
            ws.Cells[1, 3].Value = titulo;
            var font = ws.Cells[1, 3].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";
            //Borde, font cabecera de Tabla Fecha
            var borderFecha = ws.Cells[3, 2, 3, 3].Style.Border;
            borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[3, 2, 4, 3].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Bold = true;
            ws.Cells[3, 2].Value = "Fecha:";
            ws.Cells[3, 3].Value = DateTime.Now.ToString(ConstantesRestricc.FormatoFechaHora);

        }

        //Configuracion de area para reporte Archivos enviados
        public static void ConfiguracionHojaExcel5(ExcelWorksheet ws, List<IeodCuadroDTO> lista)
        {
            var fill = ws.Cells[5, 2, 5, 13].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 13].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            ws.Cells[5, 2].Value = "EMPRESA";
            ws.Cells[5, 3].Value = "T.EQ.";
            ws.Cells[5, 4].Value = "EQUIPO";
            ws.Cells[5, 5].Value = "INICIO";
            ws.Cells[5, 6].Value = "FINAL";
            ws.Cells[5, 7].Value = "USUARIO";
            ws.Cells[5, 8].Value = "DESCRIPCION";
            ws.Cells[5, 9].Value = "MODIFICACION";


            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 30;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
            ws.Column(6).Width = 30;
            ws.Column(7).Width = 30;
            ws.Column(8).Width = 30;

            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.EMPRENOMB;
                    ws.Cells[row, column + 1].Value = reg.FAMABREV;
                    ws.Cells[row, column + 2].Value = reg.EQUINOMB;
                    ws.Cells[row, column + 3].Value = reg.ICHORINI;
                    ws.Cells[row, column + 4].Value = reg.ICHORFIN;
                    ws.Cells[row, column + 5].Value = reg.LASTUSER;

                    ws.Cells[row, column + 6].Value = reg.ICDESCRIP1;
                    ws.Cells[row, column + 7].Value = reg.LASTDATE;

                    border = ws.Cells[row, 2, row, 13].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 13].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }
        }

        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>
        ///         
        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);

            }
        }


        public static int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        #endregion
    }
}
