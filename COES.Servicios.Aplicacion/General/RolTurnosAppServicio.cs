using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Linq;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Globalization;
using COES.Servicios.Aplicacion.General.Helper;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace COES.Servicios.Aplicacion.General
{
    /// <summary>
    /// Clases con métodos del módulo Programacion
    /// </summary>
    public class RolTurnosAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RolTurnosAppServicio));

        #region Métodos Configuración

        /// <summary>
        /// Inserta un registro de la tabla RTU_ACTIVIDAD
        /// </summary>
        public int SaveRtuActividad(RtuActividadDTO entity)
        {
            try
            {
                if (entity.Rtuactcodi == 0)
                {
                    entity.Rtuactusucreacion = entity.Rtuactusumodificacion;
                    entity.Rtuactfeccreacion = entity.Rtuactfecmodificacion = DateTime.Now;
                    FactorySic.GetRtuActividadRepository().Save(entity);
                }
                else
                {
                    entity.Rtuactfecmodificacion = DateTime.Now;
                    FactorySic.GetRtuActividadRepository().Update(entity);
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
        /// Elimina un registro de la tabla RTU_ACTIVIDAD
        /// </summary>
        public int DeleteRtuActividad(int rtuactcodi, string username)
        {
            try
            {
                FactorySic.GetRtuActividadRepository().Delete(rtuactcodi, username);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RTU_ACTIVIDAD
        /// </summary>
        public RtuActividadDTO GetByIdRtuActividad(int rtuactcodi)
        {
            return FactorySic.GetRtuActividadRepository().GetById(rtuactcodi);
        }

        /// <summary>
        /// Permite obtener los tipos de responsabilidad
        /// </summary>
        /// <returns></returns>
        public List<RtuActividadDTO> ObtenerTiposResponsabilidad()
        {
            return FactorySic.GetRtuActividadRepository().ObtenerTipoResponsables();
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla RtuActividad
        /// </summary>
        public List<RtuActividadDTO> GetByCriteriaRtuActividads()
        {
            return FactorySic.GetRtuActividadRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener la configuración del rol de turnos
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public EstructuraConfiguracionRolTurno ObtenerConfiguracionRolTurno(int anio, int mes)
        {
            EstructuraConfiguracionRolTurno result = new EstructuraConfiguracionRolTurno();
            List<SiPersonaDTO> listPersona = FactorySic.GetSiPersonaRepository().GetByCriteriaArea(7);
            List<RtuConfiguracionDTO> listaConfiguracion = FactorySic.GetRtuConfiguracionRepository().ObtenerConfguracion(anio, mes);
            int anioAuditoria = anio;
            int mesAuditoria = mes;           
            if(listaConfiguracion.Count == 0)
            {
                List<RtuConfiguracionDTO> configuracionAnterior = FactorySic.GetRtuConfiguracionRepository().ObtenerConfiguracionReciente(anio, mes);
                if(configuracionAnterior.Count > 0)
                {
                    int anioAnterior = (int)configuracionAnterior[0].Rtuconanio;
                    int mesAnterior = (int)configuracionAnterior[0].Rtuconmes;
                    listaConfiguracion = FactorySic.GetRtuConfiguracionRepository().ObtenerConfguracion(anioAnterior, mesAnterior);
                    anioAuditoria = anioAnterior;
                    mesAuditoria = mesAnterior;                   
                }
            }

            if (listaConfiguracion.Count != 0)
            {
                RtuConfiguracionDTO entity = FactorySic.GetRtuConfiguracionRepository().GetByAnioMes(anioAuditoria, mesAuditoria);
                result.Usuario = entity.Rtuconusumodificacion;
                result.Fecha = (entity.Rtuconfecmodificacion != null) ? ((DateTime)entity.Rtuconfecmodificacion).ToString(ConstantesAppServicio.FormatoFechaFullSeg) : string.Empty;
                result.AnioMesConfiguracion = "Configuración del " + mesAuditoria.ToString().PadLeft(2, '0') + "/" + anioAuditoria;
            }

            result.ListaPersonas = listPersona.Where(x => !listaConfiguracion.Any(y => x.Percodi == y.Percodi)).ToList();

            var listGrupo = listaConfiguracion.Select(x => new
            {
                x.Grupocodi,
                Grupotipo = x.Gruptipo,
                x.Grupoorden
            }).Distinct().OrderBy(x => x.Grupoorden).ToList();

            List<Grupoconfiguracion> listaGrupos = new List<Grupoconfiguracion>();
            foreach (var grupo in listGrupo)
            {
                List<Personaconfiguracion> listaPersona = listaConfiguracion.Where(x => x.Grupocodi == grupo.Grupocodi).Select(x => new Personaconfiguracion
                {
                    Personacodi = x.Percodi,
                    Personanombre = x.Pernomb,
                    Personaorden = x.Perorden
                }).OrderBy(x => x.Personaorden).ToList();

                Grupoconfiguracion itemGrupo = new Grupoconfiguracion
                {
                    Grupocodi = grupo.Grupocodi,
                    Grupotipo = grupo.Grupotipo,
                    Grupoorden = grupo.Grupoorden,
                    ListaPersonas = listaPersona
                };
                listaGrupos.Add(itemGrupo);
            }

            result.ListaGrupos = listaGrupos;
            return result;
        }

        /// <summary>
        /// Permite grabar la configuracion del rol de turnos
        /// </summary>
        /// <param name="data"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarConfiguracion(List<DataConfiguracionItem> data, int anio, int mes, string username)
        {
            try
            {
                RtuConfiguracionDTO entity = FactorySic.GetRtuConfiguracionRepository().GetByAnioMes(anio, mes);
                int id = 0;
                if (entity == null)
                {

                    entity = new RtuConfiguracionDTO
                    {
                        Rtuconanio = anio,
                        Rtuconmes = mes
                    };
                    entity.Rtuconusucreacion = entity.Rtuconusumodificacion = username;
                    entity.Rtuconfeccreacion = entity.Rtuconfecmodificacion = DateTime.Now;
                    id = FactorySic.GetRtuConfiguracionRepository().Save(entity);
                }
                else
                {
                    entity.Rtuconusumodificacion = username;
                    entity.Rtuconfecmodificacion = DateTime.Now;
                    FactorySic.GetRtuConfiguracionRepository().Update(entity);
                    id = entity.Rtuconcodi;
                    FactorySic.GetRtuConfiguracionRepository().Delete(id);
                }

                int ordenGrupo = 1;
                foreach (DataConfiguracionItem item in data)
                {
                    RtuConfiguracionGrupoDTO itemGrupo = new RtuConfiguracionGrupoDTO
                    {
                        Rtuconcodi = id,
                        Rtugrutipo = item.TipoGrupo,
                        Rtugruorden = ordenGrupo
                    };
                    int idGrupo = FactorySic.GetRtuConfiguracionGrupoRepository().Save(itemGrupo);
                    int ordenPersona = 1;
                    foreach (int idPersona in item.ListaPersonas)
                    {
                        RtuConfiguracionPersonaDTO itemPersona = new RtuConfiguracionPersonaDTO
                        {
                            Percodi = idPersona,
                            Rtugrucodi = idGrupo,
                            Rtuperorden = ordenPersona
                        };
                        FactorySic.GetRtuConfiguracionPersonaRepository().Save(itemPersona);

                        ordenPersona++;
                    }

                    ordenGrupo++;
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

        #region Rol de Turnos

        /// <summary>
        /// Permite obtener la estructura de rol de turnos
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="flagExcel"></param>
        /// <returns></returns>
        public EstructuraRolTurno ObtenerEstructuraRolTurno(int anio, int mes, bool flagExcel)
        {
            EstructuraRolTurno result = new EstructuraRolTurno();

            try
            {
                List<string[]> data = new List<string[]>();
                List<string[]> modalidadTrabajo = new List<string[]>();
                EstructuraConfiguracionRolTurno configuracion = this.ObtenerConfiguracionRolTurno(anio, mes);
                List<RtuRolturnoDTO> entitys = FactorySic.GetRtuRolturnoRepository().ObtenerEstructura(anio, mes);
                int nroDias = DateTime.DaysInMonth(anio, mes);
                CultureInfo culture = new CultureInfo("es-PE");
                //- Feriados
                List<DocDiaEspDTO> listaFeriados = FactorySic.GetDocDiaEspRepository().List();
                List<int> titulosGrupo = new List<int>();
                int[] feriados = new int[2 + nroDias];
                int[] finessemana = new int[2 + nroDias];
                int[] anchocolumnas = new int[2 + nroDias];
                string[] firstRow = new string[2 + nroDias];
                string[] secondRow = new string[2 + nroDias];
                firstRow[1] = "DIA:";
                secondRow[1] = "RESPONSABLE:";
                anchocolumnas[0] = 1;
                anchocolumnas[1] = 200;

                for (int i = 0; i < nroDias; i++)
                {
                    DateTime fecha = new DateTime(anio, mes, i + 1);
                    firstRow[i + 2] = culture.DateTimeFormat.GetDayName(fecha.DayOfWeek)[0].ToString().ToUpper();
                    secondRow[i + 2] = (i + 1).ToString();
                    feriados[i + 2] = listaFeriados.Where(x => ((DateTime)x.Diafecha).Subtract(fecha).TotalDays == 0).Count() > 0 ? 1 : 0;
                    finessemana[i + 2] = fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday ? 1 : 0;
                    anchocolumnas[i + 2] = 80;
                }

                data.Add(firstRow);
                data.Add(secondRow);
                modalidadTrabajo.Add(firstRow);
                modalidadTrabajo.Add(secondRow);

                int indexGrupo = 2;
                foreach (var itemGrupo in configuracion.ListaGrupos)
                {
                    string[] grupo = new string[2 + nroDias];
                    grupo[1] = RolTurnoHelper.ObtenerTipoGrupo(itemGrupo.Grupotipo);
                    string[] itemModalidad = new string[2 + nroDias];
                    data.Add(grupo);
                    titulosGrupo.Add(indexGrupo);
                    modalidadTrabajo.Add(itemModalidad);
                    indexGrupo = indexGrupo + itemGrupo.ListaPersonas.Count + 1;

                    foreach (var itemPersona in itemGrupo.ListaPersonas)
                    {
                        string[] persona = new string[2 + nroDias];
                        persona[0] = itemPersona.Personacodi.ToString();
                        persona[1] = itemPersona.Personanombre;

                        string[] modalidad = new string[2 + nroDias];

                        for (int nroDia = 1; nroDia <= nroDias; nroDia++)
                        {
                            List<RtuRolturnoDTO> actividades = entitys.Where(x => x.Rtunrodia == nroDia && x.Percodi == itemPersona.Personacodi).Distinct().ToList();

                            if (!flagExcel)
                                persona[1 + nroDia] = string.Join<int>(",", actividades.Select(x => x.Actcodi).ToList());
                            else
                                persona[1 + nroDia] = string.Join<string>(",", actividades.Select(x => x.Actnombre).ToList());

                            if (actividades.Count > 0)
                            {
                                modalidad[1 + nroDia] = actividades.First().Rtumodtrabajo;
                            }
                            else
                            {
                                modalidad[1 + nroDia] = RolTurnoHelper.ModalidadRemota;
                            }
                        }

                        data.Add(persona);
                        modalidadTrabajo.Add(modalidad);
                    }
                }

                List<RtuActividadDTO> listaActividad = this.GetByCriteriaRtuActividads();
                RtuRolturnoDTO entity = FactorySic.GetRtuRolturnoRepository().GetById(anio, mes);

                result.Data = data.ToArray();
                result.Feriados = feriados;
                result.FinesSemana = finessemana;
                result.ModalidadTrabajo = modalidadTrabajo.ToArray();
                result.ListaActividad = listaActividad.Select(x => new EstructuraActividad { id = x.Rtuactcodi.ToString(), label = x.Rtuactabreviatura, descripcion = x.Rtuactdescripcion }).ToList();
                result.Usuario = (entity != null) ? entity.Rturolusumodificacion : string.Empty;
                result.Fecha = (entity != null) ? ((DateTime)entity.Rturolfecmodificacion).ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;
                result.AnchoColumnas = anchocolumnas;
                result.TitulosGrupo = titulosGrupo.ToArray();
                result.Configuracion = configuracion.AnioMesConfiguracion;

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
        /// Permite almacenar los datos del rol de turnos
        /// </summary>
        /// <param name="data"></param>
        /// <param name="modalidad"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarRolTurno(string[][] data, string[][] modalidad, int anio, int mes, string username)
        {
            try
            {
                RtuRolturnoDTO entity = FactorySic.GetRtuRolturnoRepository().GetById(anio, mes);
                int id = 0;
                if (entity == null)
                {
                    entity = new RtuRolturnoDTO
                    {
                        Rturolanio = anio,
                        Rturolmes = mes
                    };
                    entity.Rturolusucreacion = entity.Rturolusumodificacion = username;
                    entity.Rturolfeccreacion = entity.Rturolfecmodificacion = DateTime.Now;
                    id = FactorySic.GetRtuRolturnoRepository().Save(entity);
                }
                else
                {
                    entity.Rturolusumodificacion = username;
                    entity.Rturolfecmodificacion = DateTime.Now;
                    FactorySic.GetRtuRolturnoRepository().Update(entity);
                    id = entity.Rturolcodi;
                    FactorySic.GetRtuRolturnoRepository().Delete(id);
                }

                for (int i = 2; i < data.Length; i++)
                {
                    int idPersona = !string.IsNullOrEmpty(data[i][0]) ? int.Parse(data[i][0]) : 0;
                    if (idPersona != 0)
                    {
                        for (int j = 2; j < data[0].Length; j++)
                        {
                            RtuRolturnoDetalleDTO itemDetalle = new RtuRolturnoDetalleDTO
                            {
                                Percodi = idPersona,
                                Rtudetnrodia = int.Parse(data[1][j]),
                                Rturolcodi = id,
                                Rtudetmodtrabajo = !string.IsNullOrEmpty(modalidad[i][j]) ? modalidad[i][j] : "R"
                            };
                            List<int> actividades = !string.IsNullOrEmpty(data[i][j]) ? data[i][j].Split(',').Select(int.Parse).ToList() : new List<int>();

                            if (actividades.Count > 0)
                            {
                                int idDetalle = FactorySic.GetRtuRolturnoDetalleRepository().Save(itemDetalle);

                                foreach (int actividad in actividades)
                                {
                                    RtuRolturnoActividadDTO itemActividad = new RtuRolturnoActividadDTO
                                    {
                                        Rtuactcodi = actividad,
                                        Rtudetcodi = idDetalle
                                    };
                                    FactorySic.GetRtuRolturnoActividadRepository().Save(itemActividad);
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

        /// <summary>
        /// Permite generar el formato de rol de turnos
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public int GenerarFormatoRolTurno(string path, string file, int anio, int mes)
        {
            try
            {
                EstructuraRolTurno estructura = this.ObtenerEstructuraRolTurno(anio, mes, true);
                ReporteRolTurno reporte = this.ObtenerReporteRolTurno(anio, mes);

                FileInfo newFile = new FileInfo(path + file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + file);
                }

                int index = 3;

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RolTurno");

                    ws.Cells[1, 2].Value = "SRP: Rol " + COES.Base.Tools.Util.ObtenerNombreMes(mes) + " " + anio;
                    ws.Cells[1, 2].Style.Font.Bold = true;
                    ws.Cells[1, 2].Style.Font.Size = 13;
                    ws.Cells[1, 3].Value = "Leyenda:";
                    ws.Cells[1, 3].Style.Font.Bold = true;
                    ws.Cells[1, 4].Value = "Trabajo remoto";
                    ws.Cells[1, 4].Style.Font.Bold = true;
                    ws.Cells[1, 4].Style.Font.Color.SetColor(Color.Red);
                    ws.Cells[1, 5].Value = "Trabajo presencial";
                    ws.Cells[1, 5].Style.Font.Bold = true;

                    //- Pintado los datos
                    for (int i = 0; i < estructura.Data.Length; i++)
                    {
                        for (int j = 0; j < estructura.Data[0].Length; j++)
                        {
                            ws.Cells[i + index, j + 1].Value = estructura.Data[i][j];
                        }
                    }

                    int maxColumn = estructura.Data[0].Length;
                    int maxRow = estructura.Data.Length + 2;

                    //- Estilos cabecera
                    ExcelRange rg = ws.Cells[index, 1, index + 1, maxColumn];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#3D8AB8"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    //- Estilos personal
                    rg = ws.Cells[index + 2, 1, maxRow, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CFE4FA"));
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    //- Estilos celdas y dias especiales
                    for (int i = 3; i <= maxColumn; i++)
                    {
                        rg = ws.Cells[index + 2, i, maxRow, i];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Font.Size = 10;

                        if (estructura.FinesSemana[i - 1] == 1)
                        {
                            rg.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        }
                        else if (estructura.Feriados[i - 1] == 1)
                        {
                            rg.Style.Fill.BackgroundColor.SetColor(Color.Green);
                        }
                        else
                        {
                            rg.Style.Fill.BackgroundColor.SetColor(Color.White);
                        }
                    }

                    //- Color de texto
                    for (int i = 3; i <= maxRow - 2; i++)
                    {
                        for (int j = 3; j <= maxColumn; j++)
                        {
                            rg = ws.Cells[i + 2, j, i + 2, j];
                            string modalidad = estructura.ModalidadTrabajo[i - 1][j - 1];

                            rg.Style.Font.Color.SetColor(Color.Red);
                            if (modalidad == RolTurnoHelper.ModalidadPresencial)
                            {
                                rg.Style.Font.Color.SetColor(Color.Black);
                            }
                        }
                    }

                    //- Estilos agrupaciones
                    for (int k = 1; k <= estructura.TitulosGrupo.Length; k++)
                    {
                        rg = ws.Cells[estructura.TitulosGrupo[k - 1] + 3, 1, estructura.TitulosGrupo[k - 1] + 3, maxColumn];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#000000"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                    }

                    rg = ws.Cells[index, 1, maxRow, maxColumn];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    //- Seteando los tamaños
                    ws.Column(1).Width = 1;
                    ws.Column(1).Hidden = true;
                    ws.Column(2).Width = 32;

                    for (int i = 3; i <= maxColumn; i++)
                    {
                        ws.Column(i).Width = 16;
                    }

                    //- Pintando reporte

                    int rowReporte = 1;
                    int columnReporte = maxColumn + 2;

                    ws.Cells[rowReporte, columnReporte + 3].Value = "SDF";
                    ws.Cells[rowReporte, columnReporte + 4].Value = "DC";
                    ws.Cells[rowReporte, columnReporte + 5].Value = "NETO";

                    ws.Cells[rowReporte + 1, columnReporte + 3].Value = reporte.TotalSDF;
                    ws.Cells[rowReporte + 1, columnReporte + 4].Value = reporte.TotalDC;
                    ws.Cells[rowReporte + 1, columnReporte + 5].Value = reporte.TotalNeto;

                    ws.Cells[rowReporte + 2, columnReporte].Value = "PDO/PDI";
                    rg = ws.Cells[rowReporte + 2, columnReporte, rowReporte + 2, columnReporte + 3];
                    rg.Merge = true;
                    ws.Cells[rowReporte + 2, columnReporte + 4].Value = "DESCANSOS";
                    ws.Cells[rowReporte + 2, columnReporte + 5].Value = "NETO";

                    ws.Cells[rowReporte + 3, columnReporte].Value = "SAB";
                    ws.Cells[rowReporte + 3, columnReporte + 1].Value = "DOM";
                    ws.Cells[rowReporte + 3, columnReporte + 2].Value = "FER";
                    ws.Cells[rowReporte + 3, columnReporte + 3].Value = "TOTAL";
                    ws.Cells[rowReporte + 3, columnReporte + 4].Value = "DC";

                    rg = ws.Cells[1, columnReporte + 3, maxRow, columnReporte + 3];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00B0F0"));
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    rg = ws.Cells[3, columnReporte, 4, columnReporte + 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00B0F0"));
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    rg = ws.Cells[1, columnReporte + 4, maxRow, columnReporte + 4];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF00"));
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    rg = ws.Cells[1, columnReporte + 5, maxRow, columnReporte + 5];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00FF00"));
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int countReporte = 4;
                    bool flagReporte = true;
                    foreach(var itemReporte in reporte.ListaReporte)
                    {
                        if(itemReporte.Cabecera == 1 || itemReporte.Cabecera == 2)
                        {
                            //ws.Cells[rowReporte + countReporte, columnReporte].Value = itemReporte.Grupo;
                            rg = ws.Cells[rowReporte + countReporte, columnReporte, rowReporte + countReporte, columnReporte + 5];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#000000"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;
                            flagReporte = (itemReporte.Cabecera == 1);
                        }
                        else
                        {
                            if (flagReporte)
                            {
                                ws.Cells[rowReporte + countReporte, columnReporte].Value = itemReporte.Sabado;
                                ws.Cells[rowReporte + countReporte, columnReporte + 1].Value = itemReporte.Domingo;
                                ws.Cells[rowReporte + countReporte, columnReporte + 2].Value = itemReporte.Feriado;
                                ws.Cells[rowReporte + countReporte, columnReporte + 3].Value = itemReporte.Total;
                                ws.Cells[rowReporte + countReporte, columnReporte + 4].Value = itemReporte.Descanso;
                                ws.Cells[rowReporte + countReporte, columnReporte + 5].Value = itemReporte.Neto;
                            }
                        }
                        countReporte++;
                    }

                    rg = ws.Cells[1, columnReporte, countReporte, columnReporte + 5];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    int indexLeyenda = maxRow + 3;
                    ws.Cells[indexLeyenda, 2].Value = "LEYENDA";
                    ws.Cells[indexLeyenda, 2].Style.Font.Bold = true;
                    ws.Cells[indexLeyenda + 1, 2].Value = "DESCRIPCIÓN";
                    ws.Cells[indexLeyenda + 1, 3].Value = "ABREVIATURA";

                    rg = ws.Cells[indexLeyenda + 1, 2, indexLeyenda + 1, 3];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#3D8AB8"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int countLeyenda = 2;
                    foreach(var itemLeyenda in estructura.ListaActividad)
                    {
                        ws.Cells[indexLeyenda + countLeyenda, 2].Value = itemLeyenda.descripcion;
                        ws.Cells[indexLeyenda + countLeyenda, 3].Value = itemLeyenda.label;
                        countLeyenda++;
                    }

                    rg = ws.Cells[indexLeyenda + 1, 2, indexLeyenda + countLeyenda - 1, 3];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    ws.View.ShowGridLines = false;
                    ws.View.ZoomScale = 90;
                    ws.View.FreezePanes(5, 3);

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
        /// Permite importar a pantalla los datos de excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public EstructuraRolTurno CargarRolTurnoExcel(string path, string file, int anio, int mes)
        {
            EstructuraRolTurno result = new EstructuraRolTurno();

            try
            {
                FileInfo fileInfo = new FileInfo(path + file);

                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets["RolTurno"];

                    if (ws != null)
                    {
                        //-- Validar la estructura correcta
                        EstructuraRolTurno estructura = this.ObtenerEstructuraRolTurno(anio, mes, false);
                        List<ErrorRolTurno> errores = new List<ErrorRolTurno>();

                        int rowMax = estructura.Data.Length;
                        int colMax = estructura.Data[0].Length;
                        bool flagEstructura = true;
                        for (int j = 2; j <= colMax; j++)
                        {
                            if (ws.Cells[3, j].Value != null && ws.Cells[4, j].Value != null)
                            {
                                if (ws.Cells[3, j].Value.ToString() != estructura.Data[0][j - 1] ||
                                    ws.Cells[4, j].Value.ToString() != estructura.Data[1][j - 1])
                                {
                                    flagEstructura = false;
                                }
                            }
                            else
                            {
                                flagEstructura = false;
                            }
                        }

                        for (int i = 3; i <= rowMax; i++)
                        {
                            if (!estructura.TitulosGrupo.Contains(i - 1))
                            {
                                if (ws.Cells[i + 2, 1].Value != null && ws.Cells[i +2, 2].Value != null)
                                {
                                    if (ws.Cells[i + 2, 1].Value.ToString() != estructura.Data[i - 1][0] ||
                                        ws.Cells[i + 2, 2].Value.ToString() != estructura.Data[i - 1][1])
                                    {
                                        flagEstructura = false;
                                    }
                                }
                                else
                                {
                                    flagEstructura = false;
                                }
                            }
                        }

                        if (!flagEstructura)
                        {
                            result.Result = 3; // Estructura incorrecta
                        }
                        else
                        {
                            for (int k = 0; k < estructura.TitulosGrupo.Length; k++)
                            {
                                int rowIni = estructura.TitulosGrupo[k] + 1;
                                int rowFin = (k < estructura.TitulosGrupo.Length - 1) ? estructura.TitulosGrupo[k + 1] - 1 : rowMax - 1;

                                for (int row = rowIni; row <= rowFin; row++)
                                {
                                    for (int t = 2; t < colMax; t++)
                                    {
                                        string actividad = (ws.Cells[row + 3, t + 1].Value != null) ? ws.Cells[row + 3, t + 1].Value.ToString() : string.Empty;

                                        if (!string.IsNullOrEmpty(actividad))
                                        {
                                            List<string> arrActividad = actividad.Split(',').Select(p => p.Trim()).ToList();
                                            List<int> validIds = new List<int>();
                                            List<string> invalidos = new List<string>();
                                            bool flagActividad = true;
                                            foreach (string itemActividad in arrActividad)
                                            {
                                                EstructuraActividad act = estructura.ListaActividad.Where(x => x.label == itemActividad).FirstOrDefault();
                                                if (act != null)
                                                {
                                                    validIds.Add(int.Parse(act.id));
                                                }
                                                else
                                                {
                                                    invalidos.Add(itemActividad);
                                                    flagActividad = false;
                                                }
                                            }
                                            if (flagActividad)
                                            {
                                                estructura.Data[row][t] = string.Join(",", validIds.Select(n => n.ToString()).ToArray());
                                            }
                                            else
                                            {
                                                ErrorRolTurno error = new ErrorRolTurno
                                                {
                                                    Responsable = estructura.Data[row][1],
                                                    NroDia = int.Parse(estructura.Data[1][t]),
                                                    Valor = string.Join(",", invalidos.Select(n => n.ToString()).ToArray()),
                                                    Motivo = RolTurnoHelper.MotivoErrorActividad
                                                };
                                                errores.Add(error);

                                            }
                                        }
                                        else
                                        {
                                            estructura.Data[row][t] = string.Empty;
                                        }

                                        string color = ws.Cells[row + 3, t + 1].Style.Font.Color.Rgb;
                                        estructura.ModalidadTrabajo[row][t] = color == RolTurnoHelper.ColorPresencial ? RolTurnoHelper.ModalidadPresencial : RolTurnoHelper.ModalidadRemota;
                                    }
                                }
                            }

                            if (errores.Count > 0)
                            {
                                result.Result = 2;
                                result.Errores = errores;
                            }
                            else
                            {
                                result = estructura;
                                result.Result = 1;

                            }
                        }
                    }
                    else
                    {
                        result.Result = 4;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result.Result = 1;
            }
            return result;
        }

        /// <summary>
        /// Permite obtener el reporte
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public ReporteRolTurno ObtenerReporteRolTurno(int anio, int mes)
        {
            ReporteRolTurno reporte = new ReporteRolTurno();
            List<EstructuraReporte> result = new List<EstructuraReporte>();

            EstructuraRolTurno estructura = this.ObtenerEstructuraRolTurno(anio, mes, false);
            List<RtuActividadDTO> actividades = FactorySic.GetRtuActividadRepository().List();
            List<int> pdopdi = actividades.Where(x => x.Rtuactreporte == RolTurnoHelper.ReportePDOPDI).Select(x => x.Rtuactcodi).ToList();
            List<int> descansos = actividades.Where(x => x.Rtuactreporte == RolTurnoHelper.ReporteDescanso).Select(x => x.Rtuactcodi).ToList();

            int rowMax = estructura.Data.Length;
            int colMax = estructura.Data[0].Length;

            for (int k = 0; k < estructura.TitulosGrupo.Length; k++)
            {
                int rowIni = estructura.TitulosGrupo[k] + 1;
                int rowFin = (k < estructura.TitulosGrupo.Length - 1) ? estructura.TitulosGrupo[k + 1] - 1 : rowMax - 1;
                int countPd = 0;
                List<EstructuraReporte> subListReporte = new List<EstructuraReporte>();

                for (int row = rowIni; row <= rowFin; row++)
                {
                    int countPdSabado = 0;
                    int countPdDomingo = 0;
                    int countPdFeriado = 0;
                    int countDescanso = 0;

                    for (int t = 2; t < colMax; t++)
                    {
                        List<int> ids = !string.IsNullOrEmpty(estructura.Data[row][t]) ?
                            estructura.Data[row][t].Split(',').Select(int.Parse).ToList() : new List<int>();

                        if (ids.Count > 0)
                        {
                            DateTime fecha = new DateTime(anio, mes, t - 1);

                            foreach (int id in ids)
                            {
                                if (pdopdi.Contains(id))
                                {
                                    if (estructura.FinesSemana[t] == 1)
                                    {
                                        if (fecha.DayOfWeek == DayOfWeek.Saturday)
                                            countPdSabado++;

                                        if (fecha.DayOfWeek == DayOfWeek.Sunday)
                                            countPdDomingo++;
                                    }
                                    else if (estructura.Feriados[t] == 1)
                                        countPdFeriado++;
                                }
                                if (descansos.Contains(id))
                                    countDescanso++;
                            }
                        }
                    }

                    countPd = countPd + countPdSabado + countPdDomingo + countPdFeriado;

                    EstructuraReporte itemReporte = new EstructuraReporte
                    {
                        Cabecera = 0,
                        Responsable = estructura.Data[row][1],
                        Sabado = countPdSabado,
                        Domingo = countPdDomingo,
                        Feriado = countPdFeriado,
                        Total = countPdSabado + countPdDomingo + countPdFeriado,
                        Descanso = countDescanso
                    };
                    itemReporte.Neto = itemReporte.Total - itemReporte.Descanso;
                    subListReporte.Add(itemReporte);
                }

                if (countPd > 0)
                {
                    EstructuraReporte cabReporte = new EstructuraReporte
                    {
                        Cabecera = 1,
                        Grupo = estructura.Data[estructura.TitulosGrupo[k]][1]
                    };
                    result.Add(cabReporte);
                    result.AddRange(subListReporte);
                }
                else
                {
                    //- Para subgrupos sin datos
                    EstructuraReporte cabReporte = new EstructuraReporte
                    {
                        Cabecera = 2,
                        Grupo = estructura.Data[estructura.TitulosGrupo[k]][1]
                    };
                    result.Add(cabReporte);
                    result.AddRange(subListReporte);
                }
            }

            reporte.ListaReporte = result;
            reporte.TotalSDF = result.Sum(x => x.Total);
            reporte.TotalDC = result.Sum(x => x.Descanso);
            reporte.TotalNeto = result.Sum(x => x.Neto);

            return reporte;
        }

        #endregion
    }

    #region Clases

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class DataConfiguracionItem
    {
        public string TipoGrupo { get; set; }
        public List<int> ListaPersonas { get; set; }
    }

    public class EstructuraConfiguracionRolTurno
    {
        public List<SiPersonaDTO> ListaPersonas { get; set; }
        public List<Grupoconfiguracion> ListaGrupos { get; set; }
        public string Usuario { get; set; }
        public string Fecha { get; set; }
        public string AnioMesConfiguracion { get; set; }
    }

    public class Grupoconfiguracion
    {
        public int Grupocodi { get; set; }
        public string Grupotipo { get; set; }
        public int Grupoorden { get; set; }
        public List<Personaconfiguracion> ListaPersonas { get; set; }
    }

    public class Personaconfiguracion
    {
        public int Personacodi { get; set; }
        public string Personanombre { get; set; }
        public int Personaorden { get; set; }
    }

    public class EstructuraRolTurno
    {
        public string[][] Data { get; set; }
        public int[] Feriados { get; set; }
        public int[] FinesSemana { get; set; }
        public string[][] ModalidadTrabajo { get; set; }
        public List<EstructuraActividad> ListaActividad { get; set; }
        public string Usuario { get; set; }
        public string Fecha { get; set; }
        public int[] AnchoColumnas { get; set; }
        public int Result { get; set; }
        public int[] TitulosGrupo { get; set; }
        public List<ErrorRolTurno> Errores { get; set; }
        public string Configuracion { get; set; }
    }

    public class EstructuraActividad
    {
        public string id { get; set; }
        public string label { get; set; }
        public string descripcion { get; set; }
    }

    public class ErrorRolTurno
    {
        public string Responsable { get; set; }
        public int NroDia { get; set; }
        public string Valor { get; set; }
        public string Motivo { get; set; }
    }

    public class ReporteRolTurno
    {
        public int TotalSDF { get; set; }
        public int TotalDC { get; set; }
        public int TotalNeto { get; set; }
        public List<EstructuraReporte> ListaReporte { get; set; }

    }

    public class EstructuraReporte
    {
        public int Cabecera { get; set; }
        public string Grupo { get; set; }
        public string Responsable { get; set; }
        public int Sabado { get; set; }
        public int Domingo { get; set; }
        public int Feriado { get; set; }
        public int Total { get; set; }
        public int Descanso { get; set; }
        public int Neto { get; set; }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    #endregion

}
