using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using log4net;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Text;
using System.Globalization;

namespace COES.Servicios.Aplicacion.IntercambioOsinergmin
{
    /// <summary>
    /// Flujo de Remision de datos a Osinergmin
    /// </summary>
    public class RemisionAppServicio : AppServicioBase
    {
        struct ExcelStruct
        {
            public int indice;
            public string nombre;
            public string tipo;
        }

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RemisionAppServicio));

        #region Métodos para tabla ControlCarga

        /// <summary>
        /// Guardar contorl de carga
        /// </summary>
        /// <param name="scoControlCargaDto"></param>
        /// <returns></returns>
        public int ControlCargaSave(IioControlCargaDTO scoControlCargaDto)
        {
            scoControlCargaDto.RccaEstRegistro = "1";

            try
            {
                int id;
                if (scoControlCargaDto.RccaCodi == 0)
                {
                    id = FactorySic.GetControlCargaRepository().Save(scoControlCargaDto);
                }
                else
                {
                    FactorySic.GetControlCargaRepository().Update(scoControlCargaDto);
                    id = scoControlCargaDto.RccaCodi;
                }
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener el control de carga
        /// </summary>
        /// <param name="rccaCodi"></param>
        /// <returns></returns>
        public IioControlCargaDTO ControlCargaGetById(int rccaCodi)
        {
            try
            {
                return FactorySic.GetControlCargaRepository().GetById(rccaCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener el control de carga
        /// </summary>
        /// <param name="scoControlCargaDto"></param>
        /// <returns></returns>
        public IioControlCargaDTO ControlCargaGetByCriteria(IioControlCargaDTO scoControlCargaDto)
        {
            try
            {
                return FactorySic.GetControlCargaRepository().GetByCriteria(scoControlCargaDto);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos para tabla LogRemision

        /// <summary>
        /// Eliminar control de carga
        /// </summary>
        /// <param name="rccacodi"></param>
        public void ControlCargaDelete(int rccacodi)
        {
            try
            {
                FactorySic.GetLogRemisionRepository().Delete(rccacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listar log de remisión por control de carga
        /// </summary>
        /// <param name="scoControlCargaDto"></param>
        /// <param name="pagina"></param>
        /// <param name="registrosPorPagina"></param>
        /// <returns></returns>
        public List<IioLogRemisionDTO> List(IioControlCargaDTO scoControlCargaDto, int pagina, int registrosPorPagina)
        {
            try
            {
                int minRowToFetch = (pagina - 1) * registrosPorPagina + 1;
                int maxRowToFetch = pagina * registrosPorPagina;

                return FactorySic.GetLogRemisionRepository().List(scoControlCargaDto, minRowToFetch, maxRowToFetch);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar log de remisión
        /// </summary>
        /// <param name="scoLogRemisionDto"></param>
        /// <returns></returns>
        public int SaveLogRemision(IioLogRemisionDTO scoLogRemisionDto)
        {
            try
            {
                if (scoLogRemisionDto.RlogCodi == 0)
                {
                    return FactorySic.GetLogRemisionRepository().Save(scoLogRemisionDto);
                }

                throw new InvalidParameterException();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener log de remision
        /// </summary>
        /// <param name="scoLogRemisionDto"></param>
        /// <returns></returns>
        public IioLogRemisionDTO GetById(IioLogRemisionDTO scoLogRemisionDto)
        {
            try
            {
                if (scoLogRemisionDto.RlogCodi > 0)
                {
                    return FactorySic.GetLogRemisionRepository().GetById(scoLogRemisionDto);
                }

                throw new InvalidParameterException();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos para tabla SCO PeriodoRemision

        /// <summary>
        /// Obtener periodos de sein
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<IioPeriodoSeinDTO> PeriodoGetByCriteria(string anio)
        {
            var lista = FactorySic.GetPeriodoSeinRepository().GetByCriteria(anio);

            DateTime fechaMin = new DateTime(2010, 1, 1);
            foreach (var obj in lista)
            {
                obj.PseinAnioMesPerremDesc = DateTime.ParseExact(obj.PseinAnioMesPerrem, ConstantesAppServicio.FormatoAnioMes, CultureInfo.InvariantCulture).ToString("MM yyyy");

                obj.PseinFecPriEnvioDesc = string.Empty;
                obj.PseinFecUltEnvioDesc= string.Empty;
                if (obj.PseinFecPriEnvio != null && obj.PseinFecPriEnvio > fechaMin) obj.PseinFecPriEnvioDesc = obj.PseinFecPriEnvio.ToString(ConstantesAppServicio.FormatoFechaFull);
                if (obj.PseinFecUltEnvio != null && obj.PseinFecUltEnvio > fechaMin) obj.PseinFecUltEnvioDesc = obj.PseinFecUltEnvio.ToString(ConstantesAppServicio.FormatoFechaFull);
            }

            return lista;
        }

        /// <summary>
        /// Obtener un periodo del sein
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public IioPeriodoSeinDTO PeriodoGetById(string periodo)
        {
            return FactorySic.GetPeriodoSeinRepository().GetById(periodo);
        }

        /// <summary>
        /// Guardar periodos del sein
        /// </summary>
        /// <param name="scoPeriodoRemisionDto"></param>
        /// <returns></returns>
        public string PeriodoSave(IioPeriodoSeinDTO scoPeriodoRemisionDto)
        {
            try
            {
                FactorySic.GetPeriodoSeinRepository().Save(scoPeriodoRemisionDto);
                return scoPeriodoRemisionDto.PseinAnioMesPerrem;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar periodo del sein
        /// </summary>
        /// <param name="scoPeriodoRemisionDto"></param>
        /// <returns></returns>
        public string PeriodoUpdate(IioPeriodoSeinDTO scoPeriodoRemisionDto)
        {
            try
            {
                FactorySic.GetPeriodoSeinRepository().Update(scoPeriodoRemisionDto);
                return scoPeriodoRemisionDto.PseinAnioMesPerrem;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listar anios 
        /// </summary>
        /// <returns></returns>
        public List<string> PeriodoListAnios()
        {
            try
            {
                var anios = FactorySic.GetPeriodoSeinRepository().ListAnios();
                if (!anios.Contains(DateTime.Today.Year.ToString()))
                    anios.Add(DateTime.Today.Year.ToString());

                return anios;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos para tabla TablaSync

        /// <summary>
        /// Obtener la lsita de tablas Sync
        /// </summary>
        /// <returns></returns>
        public List<IioTablaSyncDTO> List(int pseincodi)
        {
            try
            {
                return FactorySic.GeTablaSyncRepository().List(pseincodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generar data 
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="query"></param>
        /// <param name="column"></param>
        /// <param name="delimitador"></param>
        /// <param name="path"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public string GenerateDataTable(String periodo, String query, String delimitador, String path, String tabla, String nombreCortoTabla, int idEnvio)
        {
            try
            {
                return FactorySic.GeTablaSyncRepository().GetPath(periodo, query, delimitador, path, tabla, nombreCortoTabla, idEnvio);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="path"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarFormato(int formato, string path, string pathLogo, int periodo, string tabla, int idEnvio)
        {

            string fileName = string.Empty;

            //Obtenenos los datos del control de carga
            IioControlCargaDTO iioControlCargaDTO = new IioControlCargaDTO();
            iioControlCargaDTO.PseinCodi = periodo;
            iioControlCargaDTO.RtabCodi = tabla;
            IioControlCargaDTO controlCarga = this.ControlCargaGetByCriteria(iioControlCargaDTO);

            if (idEnvio > 0) {
                controlCarga.Enviocodi = idEnvio;
            }
            //Obtenemos los logs de carga
            List<IioLogRemisionDTO> listaLogErrores = this.List(controlCarga, 1, 10000);

            fileName = "Log de errores.xlsx";
            ExcelDocument.GenerarFormatoExcelLogErrores(path + fileName, listaLogErrores);

            return fileName;
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO
        /// </summary>
        public int SaveMeEnvio(MeEnvioDTO entity)
        {
            return FactorySic.GetMeEnvioRepository().Save(entity);
        }

        /// <summary>
        /// Obtener el archivo generado a cargar
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="query"></param>
        /// <param name="path"></param>
        /// <param name="tabla"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public string GenerateDataReader(String periodo, String query, String path, String tabla, String tipo)
        {
            string ruta = "";
            IDataReader dr = FactorySic.GeTablaSyncRepository().GetDataReader(periodo, query);

            if (tipo.Equals("x"))//Excel
            {
                string xlsPath = path + periodo + "_" + tabla + ".xlsx";
                ExcelDocument.GenerarFormatoDataReader(xlsPath, dr);
                ruta = xlsPath;
            }
            else if (tipo.Equals("c"))//Csv
            {
                StringBuilder builder = new StringBuilder();
                string csvPath = path + periodo + "_" + tabla + ".csv";
                //Eliminamos el archivo si existe
                File.Delete(csvPath);

                string data = "";
                string value = "";
                using (dr)
                {

                    List<ExcelStruct> columns = new List<ExcelStruct>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        ExcelStruct reg = new ExcelStruct();
                        reg.indice = i;
                        reg.nombre = dr.GetName(i).Trim();
                        reg.tipo = dr.GetDataTypeName(i).Trim();
                        if (reg.tipo.Contains("Double") || reg.tipo.Contains("Float"))
                        {
                            reg.tipo = "Decimal";
                        }
                        else if (reg.tipo.Contains("Int"))
                        {
                            reg.tipo = "Int";
                        }
                        else if (reg.tipo.Contains("Date"))
                        {
                            reg.tipo = "Date";
                        }
                        else if (reg.tipo.Contains("Varchar"))
                        {
                            reg.tipo = "String";
                        }

                        columns.Add(reg);

                        if (data == "")
                        {
                            data = data + dr.GetName(i);
                        }
                        else
                        {
                            data = data + "," + dr.GetName(i);
                        }
                    }
                    builder.AppendLine(data);

                    while (dr.Read())
                    {
                        data = "";
                        foreach (ExcelStruct item in columns)
                        {
                            if (item.tipo.Equals("String"))
                            {
                                value = "\"" + dr[item.nombre].ToString() + "\"";
                            }
                            else
                            {
                                value = dr[item.nombre].ToString();
                            }

                            if (data == "")
                            {
                                data = data + value;
                            }
                            else
                            {
                                data = data + "," + value;
                            }
                        }
                        builder.AppendLine(data);
                    }
                }
                File.AppendAllText(csvPath, builder.ToString());
                ruta = csvPath;
            }
            else//Zip
            {
                string zipPath = path + "\\" + periodo + "_" + tabla + ".zip";
                //Eliminamos el archivo si existe
                System.IO.File.Delete(zipPath);
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        var file = archive.CreateEntry(tabla + ".txt");
                        using (var entryStream = file.Open())
                        using (var streamWriter = new StreamWriter(entryStream))
                        {
                            string data = "";
                            string value = "";
                            using (dr)
                            {
                                var columns = new List<string>();
                                for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    columns.Add(dr.GetName(i));
                                    if (data == "")
                                    {
                                        data = data + dr.GetName(i);
                                    }
                                    else
                                    {
                                        data = data + " " + dr.GetName(i);
                                    }
                                }
                                streamWriter.WriteLine(data);

                                while (dr.Read())
                                {
                                    data = "";
                                    foreach (var item in columns)
                                    {
                                        value = dr[item.Trim()].ToString();

                                        if (data == "")
                                        {
                                            data = data + value;
                                        }
                                        else
                                        {
                                            data = data + " " + value;
                                        }
                                    }
                                    streamWriter.WriteLine(data);
                                }
                            }
                        }
                    }
                    using (var fileStream = new FileStream(zipPath, FileMode.Create))
                    {
                        memoryStream.Position = 0;
                        memoryStream.WriteTo(fileStream);
                    }
                }
                ruta = zipPath;
            }
            return ruta;
        }

        #endregion

        #region SIOSEIN-PRIE-2021
        /// <summary>
        /// Obtener listado de control carga por periodo
        /// </summary>
        /// <param name="PseinCodi"></param>
        /// <returns></returns>
        public List<IioControlCargaDTO> ControlCargaGetByPeriodo(int PseinCodi)
        {
            var lista = FactorySic.GetControlCargaRepository().GetByPeriodo(PseinCodi);

            foreach (var obj in lista)
            {
                FormatearIioControlCarga(obj);
            }

            return lista;
        }

        /// <summary>
        /// historial por tabla
        /// </summary>
        /// <param name="pseinCodi"></param>
        /// <param name="rtabcodi"></param>
        /// <returns></returns>
        public List<IioControlCargaDTO> ControlCargaGetByPeriodoXTabla(int pseinCodi, string rtabcodi)
        {
            var lista = FactorySic.GetControlCargaRepository().GetByCriteriaXTabla(pseinCodi, rtabcodi);

            foreach (var obj in lista) 
            {
                FormatearIioControlCarga(obj);
            }

            return lista;
        }

        private void FormatearIioControlCarga(IioControlCargaDTO obj)
        {
            obj.RccaFecCreacionDesc = string.Empty;
            obj.RccaFecModificacionDesc = string.Empty;
            obj.RccaEstadoEnvioDesc = string.Empty;

            if (obj.RccaFecModificacion == null)
            {
                obj.RccaFecModificacion = obj.RccaFecCreacion;
                obj.RccaUsuModificacion = obj.RccaUsuCreacion;
            }
            if (obj.RccaUsuModificacion == null)
            {
                obj.RccaUsuModificacion = obj.RccaUsuCreacion;
            }

            obj.RccaUsuModificacion = obj.RccaUsuModificacion ?? "";
            if (obj.RccaFecCreacion != null) obj.RccaFecCreacionDesc = obj.RccaFecCreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            if (obj.RccaFecModificacion != null) obj.RccaFecModificacionDesc = obj.RccaFecModificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            switch (obj.RccaEstadoEnvio)
            {
                case "0": obj.RccaEstadoEnvioDesc = "Remisión fallida"; break;
                case "1": obj.RccaEstadoEnvioDesc = "Remisión exitosa"; break;
                case "2": obj.RccaEstadoEnvioDesc = "No se encontraron registros"; break;
            }
        }

        public void GuardarRemisionOsinergmin(IioControlCargaDTO iioControlCargaDto, List<IioLogRemisionDTO> listaLog)
        {
            iioControlCargaDto.RccaCodi = 0;
            iioControlCargaDto.RccaCodi = ControlCargaSave(iioControlCargaDto);

            int idControl = iioControlCargaDto.RccaCodi;
            foreach (var obj in listaLog) 
            {
                obj.RccaCodi = idControl;
                SaveLogRemision(obj);
            }
        }

        public void ActualizarEstadoRemisionOsinergmin(IioControlCargaDTO iioControlCargaDto, IioPeriodoSeinDTO iioPeriodoSeinDTO, string usuario)
        {
            DateTime fechaActualizacion = DateTime.Now;

            iioControlCargaDto.RccaFecHorEnvio = fechaActualizacion;
            iioControlCargaDto.RccaFecModificacion = fechaActualizacion;
            iioControlCargaDto.RccaUsuModificacion = usuario;

            //el RccaCodi es mayor a 0
            ControlCargaSave(iioControlCargaDto);

            //Verificar si es el primer envío
            DateTime fechaMin = new DateTime(2010, 1, 1);
            if (iioPeriodoSeinDTO.PseinFecPriEnvio == null || iioPeriodoSeinDTO.PseinFecPriEnvio < fechaMin)
            {
                iioPeriodoSeinDTO.PseinFecPriEnvio = fechaActualizacion;
            }
            iioPeriodoSeinDTO.PseinFecUltEnvio = fechaActualizacion;
            iioPeriodoSeinDTO.PseinEstRegistro = "1";
            iioPeriodoSeinDTO.PseinConfirmado = "1";
            iioPeriodoSeinDTO.PseinEstado = "1";
            iioPeriodoSeinDTO.PseinUsuModificacion = usuario;

            PeriodoUpdate(iioPeriodoSeinDTO);
        }

        #endregion

    }
}