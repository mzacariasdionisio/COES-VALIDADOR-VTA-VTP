using System;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using COES.Servicios.Aplicacion.Coordinacion;
using COES.Servicios.Aplicacion.EnviarCorreos;
using COES.Framework.Base.Tools;
using System.Globalization;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using System.Configuration;

namespace COES.Servicios.Aplicacion.CambioTurno
{
    /// <summary>
    /// Clases con métodos del módulo CambioTurno
    /// </summary>
    public class CambioTurnoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CambioTurnoAppServicio));

        /// <summary>
        /// Lista los modos de operación
        /// </summary>
        /// <returns></returns>
        public List<string> ObtenerModosOperacion()
        {
            return FactorySic.GetSiCambioTurnoRepository().ObtenerModosOperacion();
        }
        
        /// <summary>
        /// Inserta un registro de la tabla SI_CAMBIO_TURNO
        /// </summary>
        public void SaveSiCambioTurno(SiCambioTurnoDTO entity, string userName)
        {
            try
            {
                string accion = string.Empty;
                int id = FactorySic.GetSiCambioTurnoRepository().VerificarExistencia((int)entity.Turno, 
                    (DateTime)entity.Fecturno);

                if (id == 0)
                {
                    id = FactorySic.GetSiCambioTurnoRepository().Save(entity);
                    accion = AccionesTabla.Grabar;
                }
                else 
                {
                    FactorySic.GetSiCambioTurnoSubseccionRepository().Delete(id);
                    FactorySic.GetSiCambioTurnoSeccionRepository().Delete(id);

                    entity.Cambioturnocodi = id;
                    FactorySic.GetSiCambioTurnoRepository().Update(entity);
                    accion = AccionesTabla.Editar;
                }

                foreach (SiCambioTurnoSeccionDTO seccion in entity.ListaSeccion)
                {
                    seccion.Cambioturnocodi = id;
                    int idSeccion = FactorySic.GetSiCambioTurnoSeccionRepository().Save(seccion);

                    foreach (SiCambioTurnoSubseccionDTO subSeccion in seccion.ListItems)
                    {
                        subSeccion.Seccioncodi = idSeccion;
                        FactorySic.GetSiCambioTurnoSubseccionRepository().Save(subSeccion);
                    }
                }

                SiCambioTurnoAuditDTO auditoria = new SiCambioTurnoAuditDTO();
                auditoria.Lastuser = userName;
                auditoria.Lastdate = DateTime.Now;
                auditoria.Cambioturnocodi = id;
                auditoria.Desaccion = accion;
                FactorySic.GetSiCambioTurnoAuditRepository().Save(auditoria);
                
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el rango de horas del turno
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="turno"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        private void ObtenerFechaTurno(DateTime fecha, int turno, out DateTime fechaInicio, out DateTime fechaFin)
        {
            fechaInicio = DateTime.Now;
            fechaFin = DateTime.Now;

            /*
             •	Turno 1: de las 23:00 del día anterior hasta las 7:00 
             •	Turno 2: de las 7:00 hasta las 15:00
             •	Turno 3: de las 15:00 hasta las 23:00
            */

            if (turno == 1)
            {
                fechaInicio = fecha.AddDays(-1).AddHours(23).AddSeconds(1);
                fechaFin = fecha.AddHours(7);
            }
            if (turno == 2)
            {
                fechaInicio = fecha.AddHours(7).AddSeconds(1);
                fechaFin = fecha.AddHours(15);
            }
            if (turno == 3)
            {
                fechaInicio = fecha.AddHours(15).AddSeconds(1);
                fechaFin = fecha.AddHours(23);
            }
        }

        /// <summary>
        /// Permite obtener el contenido de las secciones
        /// </summary>
        /// <returns></returns>
        public List<SiCambioTurnoSubseccionDTO> ObtenerContenidoSecciones(DateTime fecha, int turno, out SiCambioTurnoDTO datos,
            out int indicador, string userName)
        {
            int id = FactorySic.GetSiCambioTurnoRepository().VerificarExistencia(turno, fecha);
            List<SiCambioTurnoSubseccionDTO> list = new List<SiCambioTurnoSubseccionDTO>();
            indicador = id;

            if (id == 0)
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                this.ObtenerFechaTurno(fecha, turno, out fechaInicio, out fechaFin);


                list.AddRange(FactorySic.GetSiCambioTurnoSubseccionRepository().ObtenerRSF(fechaFin, fechaFin, SubSeccionesCambio.Seccion11));
                //list.AddRange(FactorySic.GetSiCambioTurnoSubseccionRepository().ObtenerReprogramas(fechaInicio, fechaFin, SubSeccionesCambio.Seccion12));
                list.AddRange(ObtenerReprogramasEveEmails(turno, fechaInicio, fechaFin, SubSeccionesCambio.Seccion12));
                list.AddRange(FactorySic.GetSiCambioTurnoSubseccionRepository().ObtenerMantenimientos(fechaInicio, fechaFin, SubSeccionesCambio.Seccion21));

                DateTime fechaAnterior = fecha;
                int turnoAnterior = 0;
                if (turno == 3 || turno == 2)
                {
                    turnoAnterior = turno - 1;
                }
                else if (turno == 1)
                {
                    fechaAnterior = fecha.AddDays(-1);
                    turnoAnterior = 3;
                }

                list.AddRange(FactorySic.GetSiCambioTurnoSubseccionRepository().ObtenerMantenimientoComentario(fechaAnterior, turnoAnterior, SubSeccionesCambio.Seccion22));
                list.AddRange(FactorySic.GetSiCambioTurnoSubseccionRepository().ObtenerSuministros(fechaInicio, fechaFin, SubSeccionesCambio.Seccion31));
                list.AddRange(FactorySic.GetSiCambioTurnoSubseccionRepository().ObtenerOperacionCentrales(fechaInicio, fechaFin, SubSeccionesCambio.Seccion41));
                list.AddRange(FactorySic.GetSiCambioTurnoSubseccionRepository().ObtenerLineasDesconectadas(fechaInicio, fechaFin, SubSeccionesCambio.Seccion42));
                list.AddRange(FactorySic.GetSiCambioTurnoSubseccionRepository().ObtenerEventosImportantes(fechaInicio, fechaFin, SubSeccionesCambio.Seccion44));
                list.AddRange(FactorySic.GetSiCambioTurnoSubseccionRepository().ObtenerInformeFalla(fechaInicio, fechaFin, SubSeccionesCambio.Seccion45));

                for (int i = 0; i < 2; i++)
                {
                    SiCambioTurnoSubseccionDTO entity43 = new SiCambioTurnoSubseccionDTO();
                    entity43.Subseccionnumber = SubSeccionesCambio.Seccion43;
                    list.Add(entity43);

                    //- Linea agregada SCO
                    SiCambioTurnoSubseccionDTO entity51 = new SiCambioTurnoSubseccionDTO();
                    entity51.Subseccionnumber = SubSeccionesCambio.Seccion51;
                    list.Add(entity51);
                    //- Fin de lineas agregadas
                }

                datos = new SiCambioTurnoDTO();
                datos.ListaSeccion = new List<SiCambioTurnoSeccionDTO>();
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion11 });
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion12 });
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion21 });
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion22 });
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion31 });
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion41 });
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion42 });
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion43 });
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion44 });
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion45 });

                //- Linea agregada
                datos.ListaSeccion.Add(new SiCambioTurnoSeccionDTO { Nroseccion = SubSeccionesCambio.Seccion51 });
            }
            else
            {
                SiCambioTurnoDTO entity = FactorySic.GetSiCambioTurnoRepository().GetById(id);
                List<SiCambioTurnoSeccionDTO> secciones = FactorySic.GetSiCambioTurnoSeccionRepository().GetByCriteria(id);
                List<SiCambioTurnoSubseccionDTO> subsecciones = FactorySic.GetSiCambioTurnoSubseccionRepository().GetByCriteria(id);
                //var listaOne = ObtenerReprogramasEveEmails(turno, fechaInicio, fechaFin, SubSeccionesCambio.Seccion12);
                //subsecciones.AddRange(listaOne);
                entity.ListaSeccion = secciones;
                list = subsecciones;
                datos = entity;
            }

            return list;
        }


        /// <summary>
        /// Devuelve la lista de reprogramas  guardados
        /// </summary>
        /// <param name="turno"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="seccion12"></param>
        /// <returns></returns>
        private List<SiCambioTurnoSubseccionDTO> ObtenerReprogramasEveEmails(int turno, DateTime fechaInicio, DateTime fechaFin, int seccion12)
        {
            List<SiCambioTurnoSubseccionDTO> lstReprog = new List<SiCambioTurnoSubseccionDTO>();

            var fecIni = fechaInicio.Date;
            var fecFin = fechaFin.Date;
            List<EveMailsDTO> lstReprogramas = ObtenerReprogramasEveEmail(fecIni, fecFin);

            if (lstReprogramas.Any())
            {
                List<EveMailsDTO> listaDiaAnterior = new List<EveMailsDTO>();
                List<EveMailsDTO> listaDiaActual = new List<EveMailsDTO>();
                List<EveMailsDTO> listaFinalConEdiciones = new List<EveMailsDTO>();
                List<EveMailsDTO> listaFinalSinEdiciones = new List<EveMailsDTO>();

                if (turno == 1)
                {
                    listaDiaAnterior = lstReprogramas.Where(x => x.Mailfecha.Date == fecIni && x.Mailbloquehorario >= 47).ToList(); //No seria necesario ya que mailFecha ya es el adecuado
                    listaDiaActual = lstReprogramas.Where(x => x.Mailfecha.Date == fecFin && x.Mailbloquehorario <= 14).ToList();
                }
                else
                {
                    if (turno == 2)
                    {
                        listaDiaActual = lstReprogramas.Where(x => x.Mailfecha.Date == fecFin && x.Mailbloquehorario >= 15 && x.Mailbloquehorario <= 30).ToList();
                    }
                    else
                    {
                        if (turno == 3)
                        {
                            listaDiaActual = lstReprogramas.Where(x => x.Mailfecha.Date == fecFin && x.Mailbloquehorario >= 31 && x.Mailbloquehorario <= 46).ToList();
                        }

                    }
                }


                listaFinalConEdiciones.AddRange(listaDiaAnterior);
                listaFinalConEdiciones.AddRange(listaDiaActual);

                //Solo se muestra los ultimos editados por cada tipo de reprograma (A,B,C,D,...)
                var lstTiposReprog = listaFinalConEdiciones.GroupBy(x => x.Mailhoja).ToList();
                foreach (var item in lstTiposReprog)
                {
                    var tipo = item.Key;
                    var regUnico = listaFinalConEdiciones.Where(x => x.Mailhoja == tipo).OrderByDescending(x => x.Lastdate).First();

                    listaFinalSinEdiciones.Add(regUnico);
                }

                foreach (var regEveEmail in listaFinalSinEdiciones)
                {
                    SiCambioTurnoSubseccionDTO obj = new SiCambioTurnoSubseccionDTO();
                    obj.Despreprogramas = "Reprograma de la operación SCO°" + fecFin.DayOfYear + "-" + regEveEmail.Mailhoja;
                    obj.Desphorareprog = ObtenerHora(regEveEmail.Mailbloquehorario);
                    obj.Despmotivorepro = regEveEmail.Mailreprogcausa;
                    obj.Desptiporeprog = regEveEmail.Mailhoja;
                    obj.Subseccionnumber = seccion12;

                    lstReprog.Add(obj);
                }
            }

            //Ordeno la lista segun la hora
            lstReprog = lstReprog.OrderBy(x => x.Desphorareprog).ToList();


            return lstReprog;
        }

        /// <summary>
        /// Deveulve la hora (hh:mm) que corresponde al bloque dado por parametro (1 al 48)
        /// </summary>
        /// <param name="mailbloquehorario"></param>
        /// <returns></returns>
        private string ObtenerHora(int? mailbloquehorario)
        {
            string hora = "";
            if (mailbloquehorario.HasValue)
            {
                hora = string.Format("{0:D2}", (mailbloquehorario / 2)) + ":" + (mailbloquehorario % 2 == 0 ? "00" : "30");
            }

            return hora;
        }

        /// <summary>
        /// Devuelve lista de reprogramas de Envio Correos (tabla EveMails)
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private List<EveMailsDTO> ObtenerReprogramasEveEmail(DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveMailsDTO> listaReprogramas = new List<EveMailsDTO>();

            var numPage = 1;
            var numRegistros = 1000;
            var subCausaCodi = 322;
            listaReprogramas = new EnviarCorreosAppServicio().BuscarOperaciones(subCausaCodi, fechaInicio, fechaFin, numPage, numRegistros);

            return listaReprogramas;
        }

        /// <summary>
        /// Funcion que verifica existencia del archivo atr y completa la celda respectiva
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="agrupados"></param>
        /// <returns></returns>
        public int VerificarExistenciaArchivosATR(string fechaConsulta, string agrupados, out List<string> lstEnvio)
        {
            lstEnvio = new List<string>();

            List<string> lstAnadir = new List<string>();

            int resultado = 1;

            try
            {
                bool mostrarSIoNO = true;
                resultado = VerificarArchivoEnServidorArchivos(fechaConsulta, agrupados, lstAnadir, ref lstEnvio, mostrarSIoNO);
            }
            catch (Exception e)
            {
                resultado = -1;
            }


            return resultado;
        }

        /// <summary>
        /// Ingresa al servidor de archivos para verificar archivos atr
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="agrupados"></param>
        /// <param name="lstAnadir"></param>
        /// <param name="lstEnvio"></param>
        /// <param name="mostrarSIoNO"></param>
        /// <returns></returns>
        private int VerificarArchivoEnServidorArchivos(string fechaConsulta, string agrupados, List<string> lstAnadir, ref List<string> lstEnvio, bool mostrarSIoNO)
        {
            int resultado = 1; //todos tienen su archivo atr
            int estado = 0;
            string cadArchivo = "";
            List<int> listaResultados = new List<int>();

            DateTime fecConsulta = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            string dia = string.Format("{0:D2}", fecConsulta.Day);
            string numMes = string.Format("{0:D2}", fecConsulta.Month);
            string mes = EPDate.f_NombreMes(fecConsulta.Month).ToUpper();


            string[] lstgrupos = agrupados.Split(',');

            foreach (var grupo in lstgrupos)
            {
                cadArchivo = "NO";
                string[] duo = grupo.Split('/');

                var tipo = duo[0];
                var subseccodi = "";

                if (mostrarSIoNO)
                    subseccodi = duo[1];

                estado = -1;

                //- Definimos la carpeta de trabajo temporal
                string pathTemporal = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathTempCostosMarginales];

                //- Verificamos que exista reprograma en la ultima media hora
                string folderReprograma = @"Operación\Programa de Operación\Reprograma Diario Operación\" + fecConsulta.Year + @"\" + fecConsulta.Month.ToString().PadLeft(2, '0') + @"_" +
                    COES.Base.Tools.Util.ObtenerNombreMes(fecConsulta.Month) + @"\" + ConstantesCortoPlazo.TextoDia + fecConsulta.Day.ToString().PadLeft(2, '0') +
                    @"\Reprog " + dia + numMes + tipo;

                //- Lista de directorios de reprogramas
                string pathRootNCP = @"\\coes.org.pe\archivosapp\web\";

                //List<FileData> listDirectories = FileServer.ObtenerFolderPorCriterio(folderReprograma, fecConsulta, fecConsulta.AddHours(24), pathRootNCP);
                List<FileData> listArchivos = FileServer.ObtenerArchivosPorCriterio(folderReprograma, ConstantesCortoPlazo.ExtensionZip, fecConsulta, fecConsulta.AddHours(24), pathRootNCP);

                foreach (var archivoZip in listArchivos)
                {
                    var nombreZip = archivoZip.FileName;
                    if (nombreZip.Contains("ATR"))
                    {
                        cadArchivo = "SI";
                        estado = 1;
                    }

                }

                listaResultados.Add(estado);

                if (mostrarSIoNO)
                {
                    //Colocar 'Si' 'No' a la celda ATR PUBLICADO
                    SiCambioTurnoSubseccionDTO objRegistro = new SiCambioTurnoSubseccionDTO();
                    objRegistro = FactorySic.GetSiCambioTurnoSubseccionRepository().GetById(int.Parse(subseccodi));

                    objRegistro.Desparchivoatr = cadArchivo;
                    FactorySic.GetSiCambioTurnoSubseccionRepository().Update(objRegistro);

                    string regEnviar = subseccodi + "/" + tipo + "/" + cadArchivo;
                    lstAnadir.Add(regEnviar);
                }

            }

            if (mostrarSIoNO)
                lstEnvio = lstAnadir;

            if (listaResultados.Contains(-1))
                resultado = 0;  //Existe almenos un reporgrama sin su archivo atr

            return resultado;
        }

        /// <summary>
        /// Verifica la existencia de archivos ATR  
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="agrupados"></param>
        /// <returns></returns>
        public int VerificarSoloExistenciaArchivos(string fechaConsulta, string agrupados)
        {
            List<string> lstEnvio = new List<string>();
            List<string> lstAnadir = new List<string>();
            int resultado = 1; //todos tienen su archivo atr

            try
            {
                bool mostrarSIoNO = false;
                resultado = VerificarArchivoEnServidorArchivos(fechaConsulta, agrupados, lstAnadir, ref lstEnvio, mostrarSIoNO);

            }
            catch (Exception e)
            {

                resultado = -1; //ocuriio error
            }

            return resultado;
        }
        /// <summary>
        /// Permite obtener los responsables 
        /// </summary>
        /// <returns></returns>
        public List<SiCambioTurnoDTO> ObtenerResponsables()
        {
            return FactorySic.GetSiCambioTurnoRepository().ObtenerResponsables();
        }

        /// <summary>
        /// Permite obtener la auditoria del registro de cambio de turnos
        /// </summary>
        /// <param name="idCambioTurno"></param>
        /// <returns></returns>
        public List<SiCambioTurnoAuditDTO> ObtenerAuditoria(int idCambioTurno)
        {
            return FactorySic.GetSiCambioTurnoAuditRepository().GetByCriteria(idCambioTurno);
        }

        /// <summary>
        /// Permite exportar el formato a pdf y excel
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="turno"></param>
        /// <param name="formato"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public int ExportarFormato(DateTime fechaProceso, int turno, int formato, string path, string filename, string userName)
        {
            int id = FactorySic.GetSiCambioTurnoRepository().VerificarExistencia(turno, fechaProceso);

            if (id > 0)
            {
                SiCambioTurnoDTO entity = FactorySic.GetSiCambioTurnoRepository().GetById(id);
                List<SiCambioTurnoSeccionDTO> secciones = FactorySic.GetSiCambioTurnoSeccionRepository().GetByCriteria(id);
                List<SiCambioTurnoSubseccionDTO> subsecciones = FactorySic.GetSiCambioTurnoSubseccionRepository().GetByCriteria(id);
                List<SiCambioTurnoDTO> responsables = FactorySic.GetSiCambioTurnoRepository().ObtenerResponsables();

                string responsable = string.Empty;

                if (entity.Coordinadorresp != null)
                {
                    SiCambioTurnoDTO resp = responsables.Where(x => x.Percodi == (int)entity.Coordinadorresp).FirstOrDefault();
                    if (resp != null)
                    {
                        responsable = resp.Pernomb;
                    }
                }

                foreach (SiCambioTurnoSeccionDTO item in secciones)
                {
                    item.ListItems = subsecciones.Where(x => x.Subseccionnumber == item.Nroseccion).ToList();
                }                
                
                entity.ListaSeccion = secciones;

                if (formato == 1)
                {
                    this.ExportarExcel(entity, responsable, path, filename);

                    SiCambioTurnoAuditDTO auditoria = new SiCambioTurnoAuditDTO();
                    auditoria.Lastuser = userName;
                    auditoria.Lastdate = DateTime.Now;
                    auditoria.Cambioturnocodi = id;
                    auditoria.Desaccion = AccionesTabla.ExportarExcel;
                    FactorySic.GetSiCambioTurnoAuditRepository().Save(auditoria);
                }
                else if (formato == 2)
                {
                    this.ExportarPDF(entity, responsable, path, filename);

                    SiCambioTurnoAuditDTO auditoria = new SiCambioTurnoAuditDTO();
                    auditoria.Lastuser = userName;
                    auditoria.Lastdate = DateTime.Now;
                    auditoria.Cambioturnocodi = id;
                    auditoria.Desaccion = AccionesTabla.ExportarPDF;
                    FactorySic.GetSiCambioTurnoAuditRepository().Save(auditoria);
                }

                return 1;
            }
            else 
            {
                return 2;
            }           
        }

        /// <summary>
        /// Permite exportar los datos a formato excel
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="responsable"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        protected void ExportarExcel(SiCambioTurnoDTO entity, string responsable, string path, string fileName)
        {
            fileName = path + fileName;
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CAMBIOTURNO");

                if (ws != null)
                {
                    int index = 10;
                    int lastcolumn = 10;

                    ws.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C4C4C4"));

                    ExcelRange rg = ws.Cells[1, 1, index, 10];
                    rg = this.ObtenerEstiloCelda(rg, 5);


                    ws.Cells[6, 2].Value = "ENTREGA DE TURNO DEL CENTRO COORDINADOR DE LA OPERACIÓN";
                    rg = ws.Cells[6, 2, 6, 8];
                    rg.Merge = true;
                    rg = this.ObtenerEstiloCelda(rg, 8);


                    ws.Cells[7, 3].Value = "DIRECCIÓN DE OPERACIONES";
                    rg = ws.Cells[7, 3, 7, 7];
                    rg.Merge = true;
                    rg = this.ObtenerEstiloCelda(rg, 9);

                    ws.Cells[8, 3].Value = "SUB DIRECCIÓN DE COORDINACIÓN";
                    rg = ws.Cells[8, 3, 8, 7];
                    rg.Merge = true;
                    rg = this.ObtenerEstiloCelda(rg, 10);

                    ws.Cells[index, 1].Value = "COORDINADOR RESPONSABLE:";
                    ws.Cells[index, 2].Value = responsable;
                    ws.Cells[index, 7].Value = "FECHA:";
                    ws.Cells[index, 8].Value = (entity.Fecturno != null) ? ((DateTime)entity.Fecturno).ToString("dd/MM/yyyy") : string.Empty;
                    ws.Cells[index, 9].Value = "TURNO:";
                    ws.Cells[index, 10].Value = "TURNO " + entity.Turno;

                    rg = ws.Cells[index, 2, index, 6];
                    rg.Merge = true;
                    rg = this.ObtenerEstiloCelda(rg, 7);

                    rg = ws.Cells[index, 1, index, 1];
                    rg = this.ObtenerEstiloCelda(rg, 6);
                    rg = ws.Cells[index, 7, index, 9];
                    rg = this.ObtenerEstiloCelda(rg, 6);
                    rg = ws.Cells[index, 9, index, 9];
                    rg = this.ObtenerEstiloCelda(rg, 6);
                    rg = ws.Cells[index, 8, index, 8];
                    rg = this.ObtenerEstiloCelda(rg, 7);
                    rg = ws.Cells[index, 10, index, 10];
                    rg = this.ObtenerEstiloCelda(rg, 7);

                    index = index + 1;
                    ws.Cells[index, 1].Value = "1. DESPACHO";

                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 1);
                    rg.Merge = true;


                    ws.Cells[index + 1, 1].Value = "CENTRAL MARGINAL";
                    ws.Cells[index + 1, 2].Value = "RSF-AUTOMÁTICA SEIN";
                    ws.Cells[index + 1, 5].Value = "RSF-MANUAL  SEIN";
                    ws.Cells[index + 1, 8].Value = "RSF-SISTEMAS AISLADOS";
                    ws.Cells[index + 2, 2].Value = "URS";
                    ws.Cells[index + 2, 4].Value = "MAGNITUD";
                    ws.Cells[index + 2, 5].Value = "URS";
                    ws.Cells[index + 2, 7].Value = "MAGNITUD";
                    ws.Cells[index + 2, 8].Value = "URS";
                    ws.Cells[index + 2, 10].Value = "MAGNITUD";
                    rg = ws.Cells[index + 1, 1, index + 2, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 1, index + 2, 1].Merge = true;
                    ws.Cells[index + 1, 2, index + 1, 4].Merge = true;
                    ws.Cells[index + 1, 5, index + 1, 7].Merge = true;
                    ws.Cells[index + 1, 8, index + 1, 10].Merge = true;
                    ws.Cells[index + 2, 2, index + 2, 3].Merge = true;
                    ws.Cells[index + 2, 5, index + 2, 6].Merge = true;
                    ws.Cells[index + 2, 8, index + 2, 9].Merge = true;

                    index = index + 3;

                    SiCambioTurnoSeccionDTO seccion11 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion11).FirstOrDefault();

                    int indexInicio = index;
                    foreach (SiCambioTurnoSubseccionDTO item in seccion11.ListItems)
                    {
                        ws.Cells[index, 1].Value = item.Despcentromarginal;
                        ws.Cells[index, 2].Value = item.Despursautomatica;
                        ws.Cells[index, 4].Value = (item.Despmagautomatica != null) ? item.Despmagautomatica.ToString() : string.Empty;
                        ws.Cells[index, 5].Value = item.Despursmanual;
                        ws.Cells[index, 7].Value = (item.Despmagmanual != null) ? item.Despmagmanual.ToString() : string.Empty;
                        ws.Cells[index, 8].Value = item.Despcentralaislado;
                        ws.Cells[index, 10].Value = (item.Despmagaislado != null) ? item.Despmagaislado.ToString() : string.Empty;

                        ws.Cells[index, 2, index, 3].Merge = true;
                        ws.Cells[index, 5, index, 6].Merge = true;
                        ws.Cells[index, 8, index, 9].Merge = true;

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    ws.Cells[index, 1].Value = "REPROGRAMAS";
                    ws.Cells[index, 2].Value = "HORA";
                    ws.Cells[index, 3].Value = "MOTIVO PRINCIPAL";
                    ws.Cells[index, 6].Value = "ATR PUBLICADO";
                    ws.Cells[index, 7].Value = "PREMISAS IMPORTANTES";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index, 3, index, 5].Merge = true;
                    ws.Cells[index, 7, index, 10].Merge = true;

                    index = index + 1;

                    SiCambioTurnoSeccionDTO seccion12 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion12).FirstOrDefault();

                    indexInicio = index;
                    foreach (SiCambioTurnoSubseccionDTO item in seccion12.ListItems)
                    {
                        ws.Cells[index, 1].Value = item.Despreprogramas;
                        ws.Cells[index, 2].Value = item.Desphorareprog;
                        ws.Cells[index, 3].Value = item.Despmotivorepro;
                        ws.Cells[index, 6].Value = item.Desparchivoatr;
                        ws.Cells[index, 7].Value = item.Desppremisasreprog;

                        ws.Cells[index, 3, index, 5].Merge = true;
                        ws.Cells[index, 7, index, 10].Merge = true;

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    ws.Cells[index, 1].Value = (string.IsNullOrEmpty(seccion12.Descomentario)) ? "Comentarios adicionales" : seccion12.Descomentario;
                    rg = ws.Cells[index, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 3);
                    rg.Merge = true;

                    index = index + 2;

                    ws.Cells[index, 1].Value = "2.  MANTENIMIENTOS RELEVANTES";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 1);
                    rg.Merge = true;

                    ws.Cells[index + 1, 1].Value = "EQUIPO";
                    ws.Cells[index + 1, 2].Value = "TIPO";
                    ws.Cells[index + 1, 4].Value = "HORA CONEXÓN";
                    ws.Cells[index + 1, 5].Value = "CONSIDERACIONES IMPORTANTES PARA LA MANIOBRA";
                    ws.Cells[index + 2, 2].Value = "PROGR";
                    ws.Cells[index + 2, 3].Value = "CORREC";
                    rg = ws.Cells[index + 1, 1, index + 2, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 1, index + 2, 1].Merge = true;
                    ws.Cells[index + 1, 2, index + 1, 3].Merge = true;
                    ws.Cells[index + 1, 4, index + 2, 4].Merge = true;
                    ws.Cells[index + 1, 5, index + 2, 10].Merge = true;

                    index = index + 3;

                    List<int> listIndexCero = new List<int>();
                    SiCambioTurnoSeccionDTO seccion21 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion21).FirstOrDefault();

                    indexInicio = index;

                    List<SiCambioTurnoSubseccionDTO> list21 = seccion21.ListItems.OrderBy(x => x.Manhoraconex).ToList();
                    foreach (SiCambioTurnoSubseccionDTO item in list21)
                    {
                        ws.Cells[index, 1].Value = item.Manequipo;
                        if (item.Mantipo == "P") ws.Cells[index, 2].Value = "X";
                        if (item.Mantipo == "C") ws.Cells[index, 3].Value = "X";
                        ws.Cells[index, 4].Value = item.Manhoraconex;
                        ws.Cells[index, 5].Value = item.Manconsideraciones;
                        ws.Cells[index, 5, index, 10].Merge = true;

                        if (item.Manhoraconex == "00:00")
                        {
                            listIndexCero.Add(index);
                            //rg = ws.Cells[index, 1, index, lastcolumn];
                            //rg = this.ObtenerEstiloCelda(rg, 11);
                        }

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    ws.Cells[index, 1].Value = (string.IsNullOrEmpty(seccion21.Descomentario)) ? "Comentarios adicionales" : seccion21.Descomentario;
                    rg = ws.Cells[index, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 3);
                    rg.Merge = true;



                    index = index + 1;


                    ws.Cells[index + 1, 1].Value = "EQUIPO";
                    ws.Cells[index + 1, 2].Value = "TIPO";
                    ws.Cells[index + 1, 4].Value = "HORA CONEXÓN";
                    ws.Cells[index + 1, 5].Value = "CONSIDERACIONES IMPORTANTES PARA LA MANIOBRA";
                    ws.Cells[index + 2, 2].Value = "PROGR";
                    ws.Cells[index + 2, 3].Value = "CORREC";
                    rg = ws.Cells[index + 1, 1, index + 2, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 1, index + 2, 1].Merge = true;
                    ws.Cells[index + 1, 2, index + 1, 3].Merge = true;
                    ws.Cells[index + 1, 4, index + 2, 4].Merge = true;
                    ws.Cells[index + 1, 5, index + 2, 10].Merge = true;

                    index = index + 3;

                    SiCambioTurnoSeccionDTO seccion22 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion22).FirstOrDefault();

                    indexInicio = index;
                    List<SiCambioTurnoSubseccionDTO> list22 = seccion22.ListItems.OrderBy(x => x.Manhoraconex).ToList();
                    foreach (SiCambioTurnoSubseccionDTO item in list22)
                    {
                        ws.Cells[index, 1].Value = item.Manequipo;
                        if (item.Mantipo == "P") ws.Cells[index, 2].Value = "X";
                        if (item.Mantipo == "C") ws.Cells[index, 3].Value = "X";
                        ws.Cells[index, 4].Value = item.Manhoraconex;
                        ws.Cells[index, 5].Value = item.Manconsideraciones;
                        ws.Cells[index, 5, index, 10].Merge = true;

                        if (item.Manhoraconex == "00:00")
                        {
                            listIndexCero.Add(index);
                        }

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    foreach (int i in listIndexCero)
                    {
                        rg = ws.Cells[i, 1, i, lastcolumn];
                        rg = this.ObtenerEstiloCelda(rg, 11);
                    }

                    //index = index + 2;

                    ws.Cells[index, 1].Value = "3.  SUMINISTRO DE ENERGIA";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 1);
                    rg.Merge = true;

                    ws.Cells[index + 1, 1].Value = "SUBESTACIÓN";
                    ws.Cells[index + 1, 2].Value = "MOTIVO DEL CORTE";
                    ws.Cells[index + 1, 6].Value = "HORA INICIO";
                    ws.Cells[index + 1, 7].Value = "REPOSICIÓN";
                    ws.Cells[index + 1, 8].Value = "CONSIDERACIONES IMPORTANTES";
                    ws.Cells[index + 2, 2].Value = "Deficit/Sobrecarga";
                    ws.Cells[index + 2, 3].Value = "FALLA";
                    ws.Cells[index + 2, 4].Value = "TENSIÓN";
                    ws.Cells[index + 2, 5].Value = "MANTTO";
                    rg = ws.Cells[index + 1, 1, index + 2, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 1, index + 2, 1].Merge = true;
                    ws.Cells[index + 1, 2, index + 1, 5].Merge = true;
                    ws.Cells[index + 1, 6, index + 2, 6].Merge = true;
                    ws.Cells[index + 1, 7, index + 2, 7].Merge = true;
                    ws.Cells[index + 1, 8, index + 2, 10].Merge = true;


                    index = index + 3;

                    SiCambioTurnoSeccionDTO seccion31 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion31).FirstOrDefault();

                    indexInicio = index;
                    foreach (SiCambioTurnoSubseccionDTO item in seccion31.ListItems)
                    {
                        ws.Cells[index, 1].Value = item.Sumsubestacion;
                        if (item.Summotivocorte == "D") ws.Cells[index, 2].Value = "X";
                        if (item.Summotivocorte == "F") ws.Cells[index, 3].Value = "X";
                        if (item.Summotivocorte == "T") ws.Cells[index, 4].Value = "X";
                        if (item.Summotivocorte == "M") ws.Cells[index, 5].Value = "X";
                        ws.Cells[index, 6].Value = item.Sumhorainicio;
                        ws.Cells[index, 7].Value = item.Sumreposicion;
                        ws.Cells[index, 8].Value = item.Sumconsideraciones;

                        ws.Cells[index, 8, index, 10].Merge = true;

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    ws.Cells[index, 1].Value = (string.IsNullOrEmpty(seccion31.Descomentario)) ? "Comentarios adicionales" : seccion31.Descomentario;
                    rg = ws.Cells[index, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 3);
                    rg.Merge = true;

                    index = index + 2;

                    ws.Cells[index, 1].Value = "4.  OTROS ASPECTOS RELEVANTES PARA LA OPERACIÓN";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 1);
                    rg.Merge = true;

                    ws.Cells[index + 1, 1].Value = "Regulación de tensión";
                    rg = ws.Cells[index + 1, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 4);
                    rg.Merge = true;

                    ws.Cells[index + 2, 1].Value = "OPERACIÓN DE CENTRALES";
                    ws.Cells[index + 2, 4].Value = "SUBESTACIÓN";
                    ws.Cells[index + 2, 8].Value = "HORA FIN APROX";

                    rg = ws.Cells[index + 2, 1, index + 2, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 2, 1, index + 2, 3].Merge = true;
                    ws.Cells[index + 2, 4, index + 2, 7].Merge = true;
                    ws.Cells[index + 2, 8, index + 2, 10].Merge = true;

                    index = index + 3;

                    SiCambioTurnoSeccionDTO seccion41 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion41).FirstOrDefault();

                    indexInicio = index;
                    foreach (SiCambioTurnoSubseccionDTO item in seccion41.ListItems)
                    {
                        ws.Cells[index, 1].Value = item.Regopecentral;
                        ws.Cells[index, 4].Value = item.Regcentralsubestacion;
                        ws.Cells[index, 8].Value = item.Regcentralhorafin;

                        ws.Cells[index, 1, index, 3].Merge = true;
                        ws.Cells[index, 4, index, 7].Merge = true;
                        ws.Cells[index, 8, index, 10].Merge = true;

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    ws.Cells[index, 1].Value = (string.IsNullOrEmpty(seccion41.Descomentario)) ? "Comentarios adicionales" : seccion41.Descomentario;
                    rg = ws.Cells[index, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 3);
                    rg.Merge = true;


                    index = index + 2;

                    ws.Cells[index, 1].Value = "Lineas desconectadas";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 4);
                    rg.Merge = true;


                    ws.Cells[index + 1, 1].Value = "LÍNEA";
                    ws.Cells[index + 1, 4].Value = "SUBESTACIÓN";
                    ws.Cells[index + 1, 8].Value = "HORA FIN APROX";
                    rg = ws.Cells[index + 1, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 1, index + 1, 3].Merge = true;
                    ws.Cells[index + 1, 4, index + 1, 7].Merge = true;
                    ws.Cells[index + 1, 8, index + 1, 10].Merge = true;

                    index = index + 2;

                    SiCambioTurnoSeccionDTO seccion42 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion42).FirstOrDefault();

                    indexInicio = index;

                    foreach (SiCambioTurnoSubseccionDTO item in seccion42.ListItems)
                    {
                        ws.Cells[index, 1].Value = item.Reglineas;
                        ws.Cells[index, 4].Value = item.Reglineasubestacion;
                        ws.Cells[index, 8].Value = item.Reglineahorafin;

                        ws.Cells[index, 1, index, 3].Merge = true;
                        ws.Cells[index, 4, index, 7].Merge = true;
                        ws.Cells[index, 8, index, 10].Merge = true;

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    ws.Cells[index, 1].Value = (string.IsNullOrEmpty(seccion42.Descomentario)) ? "Comentarios adicionales" : seccion42.Descomentario;
                    rg = ws.Cells[index, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 3);
                    rg.Merge = true;

                    index = index + 2;

                    ws.Cells[index, 1].Value = "Gestión de Mantenimientos fuera del PDO";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 4);
                    rg.Merge = true;

                    ws.Cells[index + 1, 1].Value = "EQUIPO";
                    ws.Cells[index + 1, 2].Value = "ACEPTADO";
                    ws.Cells[index + 1, 3].Value = "RECHAZADO";
                    ws.Cells[index + 1, 4].Value = "DETALLE (descripción, fecha, hora inicio, hora fin) (motivo de rechazo)";
                    rg = ws.Cells[index + 1, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 4, index + 1, 10].Merge = true;

                    index = index + 2;

                    SiCambioTurnoSeccionDTO seccion43 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion43).FirstOrDefault();

                    indexInicio = index;

                    foreach (SiCambioTurnoSubseccionDTO item in seccion43.ListItems)
                    {
                        ws.Cells[index, 1].Value = item.Gesequipo;

                        if (item.Gesaceptado == "A") ws.Cells[index, 2].Value = "X";
                        if (item.Gesaceptado == "R") ws.Cells[index, 3].Value = "X";
                        ws.Cells[index, 4].Value = item.Gesdetalle;

                        ws.Cells[index, 4, index, 10].Merge = true;

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    ws.Cells[index, 1].Value = (string.IsNullOrEmpty(seccion43.Descomentario)) ? "Comentarios adicionales" : seccion43.Descomentario;
                    rg = ws.Cells[index, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 3);
                    rg.Merge = true;

                    index = index + 2;

                    ws.Cells[index, 1].Value = "Eventos Importantes";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 4);
                    rg.Merge = true;

                    ws.Cells[index + 1, 1].Value = "EQUIPO";
                    ws.Cells[index + 1, 3].Value = "HORA INICIO";
                    ws.Cells[index + 1, 4].Value = "REPOSICIÓN";
                    ws.Cells[index + 1, 5].Value = "RESUMEN (hora falla, carga interrumpida,reposición del equipo)";
                    rg = ws.Cells[index + 1, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 1, index + 1, 2].Merge = true;
                    ws.Cells[index + 1, 5, index + 1, 10].Merge = true;

                    index = index + 2;

                    SiCambioTurnoSeccionDTO seccion44 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion44).FirstOrDefault();

                    indexInicio = index;

                    foreach (SiCambioTurnoSubseccionDTO item in seccion44.ListItems)
                    {
                        ws.Cells[index, 1].Value = item.Eveequipo;
                        ws.Cells[index, 3].Value = item.Evehorainicio;
                        ws.Cells[index, 4].Value = item.Evereposicion;
                        ws.Cells[index, 5].Value = item.Everesumen;

                        ws.Cells[index, 1, index, 2].Merge = true;
                        ws.Cells[index, 5, index, 10].Merge = true;

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    ws.Cells[index, 1].Value = (string.IsNullOrEmpty(seccion44.Descomentario)) ? "Comentarios adicionales" : seccion44.Descomentario;
                    rg = ws.Cells[index, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 3);
                    rg.Merge = true;

                    index = index + 2;

                    ws.Cells[index, 1].Value = "Informes Finales de Falla próximos a vencer (Tipo N1 dentro de las 24h)";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 4);
                    rg.Merge = true;

                    ws.Cells[index + 1, 1].Value = "EQUIPO";
                    ws.Cells[index + 1, 4].Value = "ENVIADO";
                    ws.Cells[index + 1, 6].Value = "PENDIENTE";
                    ws.Cells[index + 1, 8].Value = "PLAZO (h)";
                    rg = ws.Cells[index + 1, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 1, index + 1, 3].Merge = true;
                    ws.Cells[index + 1, 4, index + 1, 5].Merge = true;
                    ws.Cells[index + 1, 6, index + 1, 7].Merge = true;
                    ws.Cells[index + 1, 8, index + 1, 10].Merge = true;

                    index = index + 2;

                    SiCambioTurnoSeccionDTO seccion45 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion45).FirstOrDefault();

                    indexInicio = index;

                    foreach (SiCambioTurnoSubseccionDTO item in seccion45.ListItems)
                    {
                        ws.Cells[index, 1].Value = item.Infequipo;
                        if (item.Infestado == "E") ws.Cells[index, 4].Value = "X";
                        if (item.Infestado == "P") ws.Cells[index, 6].Value = "X";
                        ws.Cells[index, 8].Value = item.Infplazo;

                        ws.Cells[index, 1, index, 3].Merge = true;
                        ws.Cells[index, 4, index, 5].Merge = true;
                        ws.Cells[index, 6, index, 7].Merge = true;
                        ws.Cells[index, 8, index, 10].Merge = true;

                        index++;
                    }

                    rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 0);

                    //- Inicio modificado                                      

                    ws.Cells[index, 1].Value = "5.  VISUALIZACIÓN DE SORTEO DE PRUEBAS ALEATORIAS";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 1);
                    rg.Merge = true;

                    ws.Cells[index + 1, 1].Value = "FECHA DE SORTEO";
                    ws.Cells[index + 1, 3].Value = "SORTEO";
                    ws.Cells[index + 1, 4].Value = "RESULTADO";
                    ws.Cells[index + 1, 7].Value = "GENERADOR";
                    ws.Cells[index + 1, 9].Value = "PRUEBA";
                    rg = ws.Cells[index + 1, 1, index + 1, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 1, index + 1, 2].Merge = true;
                    ws.Cells[index + 1, 3, index + 1, 4].Merge = true;
                    ws.Cells[index + 1, 5, index + 1, 6].Merge = true;
                    ws.Cells[index + 1, 7, index + 1, 8].Merge = true;
                    ws.Cells[index + 1, 9, index + 1, 10].Merge = true;

                    index = index + 2;

                    if (entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion51).Count() > 0)
                    {

                        SiCambioTurnoSeccionDTO seccion51 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion51).FirstOrDefault();

                        indexInicio = index;

                        foreach (SiCambioTurnoSubseccionDTO item in seccion51.ListItems)
                        {
                            ws.Cells[index, 1].Value = item.Pafecha;
                            ws.Cells[index, 3].Value = item.Pasorteo;
                            ws.Cells[index, 5].Value = item.Paresultado;
                            ws.Cells[index, 6].Value = item.Pagenerador;
                            ws.Cells[index, 9].Value = item.Paprueba;

                            ws.Cells[index, 1, index, 2].Merge = true;
                            ws.Cells[index, 3, index, 4].Merge = true;
                            ws.Cells[index, 5, index, 6].Merge = true;
                            ws.Cells[index, 7, index, 8].Merge = true;
                            ws.Cells[index, 9, index, 10].Merge = true;

                            index++;
                        }


                        rg = ws.Cells[indexInicio, 1, index - 1, lastcolumn];
                        rg = this.ObtenerEstiloCelda(rg, 0);

                    }

                    //- Fin modificado





                    ws.Cells[index, 1].Value = "Otros";
                    rg = ws.Cells[index, 1, index, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 4);
                    rg.Merge = true;

                    ws.Cells[index + 1, 1].Value = "CASOS SIN RESERVA";
                    ws.Cells[index + 1, 3].Value = "EMS OPERATIVO";
                    ws.Cells[index + 1, 9].Value = "HORA DE ENTREGA DE TURNO";
                    ws.Cells[index + 2, 1].Value = "Se entrega todos los casos SIN RESERVA";
                    ws.Cells[index + 2, 3].Value = "SI";
                    ws.Cells[index + 2, 4].Value = "NO";
                    ws.Cells[index + 2, 5].Value = "OBSERVACIONES";
                    rg = ws.Cells[index + 1, 1, index + 2, lastcolumn];
                    rg = this.ObtenerEstiloCelda(rg, 2);

                    ws.Cells[index + 1, 1, index + 1, 2].Merge = true;
                    ws.Cells[index + 1, 3, index + 1, 8].Merge = true;
                    ws.Cells[index + 1, 9, index + 2, 10].Merge = true;
                    ws.Cells[index + 2, 1, index + 2, 2].Merge = true;
                    ws.Cells[index + 2, 5, index + 2, 8].Merge = true;

                    ws.Cells[index + 3, 1].Value = entity.CasoSinReserva;
                    ws.Cells[index + 3, 3].Value = (entity.Emsoperativo == ConstantesAppServicio.SI) ? "X" : "";
                    ws.Cells[index + 3, 4].Value = (entity.Emsoperativo == ConstantesAppServicio.NO) ? "X" : "";
                    ws.Cells[index + 3, 5].Value = entity.Emsobservaciones;
                    ws.Cells[index + 3, 9].Value = entity.Horaentregaturno;

                    ws.Cells[index + 3, 1, index + 3, 2].Merge = true;
                    ws.Cells[index + 3, 5, index + 3, 8].Merge = true;
                    ws.Cells[index + 3, 9, index + 3, 10].Merge = true;

                    index = index + 4;

                    ws.Cells[index, 1].Value = "";
                    ws.Cells[index, 1, index, 10].Merge = true;

                    rg = ws.Cells[index - 1, 1, index, 10];
                    rg = this.ObtenerEstiloCelda(rg, 0);


                    ws.Cells[index + 1, 1].Value = entity.Coordinadorrecibe;
                    ws.Cells[index + 1, 3].Value = entity.Especialistarecibe;
                    ws.Cells[index + 1, 7].Value = entity.Analistarecibe;

                    ws.Cells[index + 2, 1].Value = "COORDINADOR QUE RECIBE EL TURNO";
                    ws.Cells[index + 2, 3].Value = "ESPECIALISTA QUE RECIBE EL TURNO";
                    ws.Cells[index + 2, 7].Value = "ANALISTA QUE RECIBE EL TURNO";


                    ws.Cells[index + 1, 1, index + 1, 2].Merge = true;
                    ws.Cells[index + 1, 3, index + 1, 6].Merge = true;
                    ws.Cells[index + 1, 7, index + 1, 10].Merge = true;

                    rg = ws.Cells[index + 1, 1, index + 1, 10];
                    rg = this.ObtenerEstiloCelda(rg, 7);

                    ws.Cells[index + 2, 1, index + 2, 2].Merge = true;
                    ws.Cells[index + 2, 3, index + 2, 6].Merge = true;
                    ws.Cells[index + 2, 7, index + 2, 10].Merge = true;

                    rg = ws.Cells[index + 2, 1, index + 2, 10];
                    rg = this.ObtenerEstiloCelda(rg, 6);



                    ws.Column(1).Width = 30;
                    ws.Column(2).Width = 15;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;


                    rg = ws.Cells[1, 1, index + 2, 1];
                    string colorborder = "#000000";

                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));

                    rg = ws.Cells[1, 10, index + 2, 10];
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));

                    rg = ws.Cells[1, 1, 1, 10];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));

                    rg = ws.Cells[index + 2, 1, index + 2, 10];
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));




                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture("lOGO", img);
                    picture.SetPosition(20, 490);
                    //picture.From.Column = 4;
                    //picture.From.Row = 1;
                    //picture.To.Column = 4;
                    //picture.To.Row = 2;
                    picture.SetSize(180, 60);

                }


                xlPackage.Save();
            }
        }


        /// <summary>
        /// Permite exportar los datos a formato PDF
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="responsable"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        protected void ExportarPDF(SiCambioTurnoDTO entity, string responsable, string path, string fileName)
        {
            PdfDocument pdf = new PdfDocument();
            pdf.GenerarReporteInforme(entity, responsable, path, fileName);
        }

        /// <summary>
        /// Estilo del excel 
        /// 0: Celdas
        /// 1: Titulos
        /// 2: Subtitulos
        /// 3: Comentarios
        /// 4: agrupaciones      
        /// 5: bloque cabecera
        /// 6: datos
        /// 7: Contenido de datos
        /// 8: Primera de fila titulo
        /// 9: segunda fila titulo
        /// 10: tercera fila titulo
        /// 11: Mantenimientos con hora 00:00
        /// </summary>
        /// <param name="rango"></param>
        /// <param name="seccion"></param>
        /// <returns></returns>
        private ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#1C91AE"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;

                string colorborder = "#E7E7E7";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1) 
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#1991B5"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;

                string colorborder = "#DADAD9";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D7EFEF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#1C91AE"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            if (seccion == 3)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#EA9140"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;
                string colorborder = "#DADAD9";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));                
            
            }

            if (seccion == 4)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFEB9C"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#AD6500"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            }
            

            if (seccion == 5)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#1C91AE"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                
            }

            if (seccion == 6)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F2F2F2"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FA7D00"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;

                string colorborder = "#DADAD9";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            }

            if (seccion == 7)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F2F2F2"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#1C91AE"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;

                string colorborder = "#DADAD9";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            }


            if (seccion == 8)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#1991B5"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 13;
                rango.Style.Font.Bold = true;
            }

            if (seccion == 9)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#1991B5"));
                rango.Style.Font.Color.SetColor(Color.Black);
                rango.Style.Font.Size = 12;
                rango.Style.Font.Bold = true;
            }
            if (seccion == 10)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#1991B5"));
                rango.Style.Font.Color.SetColor(Color.Black);
                rango.Style.Font.Size = 11;
                rango.Style.Font.Bold = true;
            }
            if (seccion == 11)
            {               
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAFFEA"));               
                rango.Style.Font.Size = 10;
               
            }
            

            return rango;
        }
        
    }

    /// <summary>
    /// Secciones del informe
    /// </summary>
    public class SubSeccionesCambio
    {
        /// <summary>
        /// Regulación secundaria de frecuencia
        /// </summary>
        public const int Seccion11 = 11;

        /// <summary>
        /// Reprogramas
        /// </summary>
        public const int Seccion12 = 12;

        /// <summary>
        /// Mantenimientos relevantes
        /// </summary>
        public const int Seccion21 = 21;

        /// <summary>
        /// Observaciones de los mantenimientos relevantes
        /// </summary>
        public const int Seccion22 = 22;

        /// <summary>
        /// Suministro
        /// </summary>
        public const int Seccion31 = 31;

        /// <summary>
        /// Operación de centrales
        /// </summary>
        public const int Seccion41 = 41;

        /// <summary>
        /// Lineas desconectadas
        /// </summary>
        public const int Seccion42 = 42;

        /// <summary>
        /// Mantenimientos fuera del pdo
        /// </summary>
        public const int Seccion43 = 43;

        /// <summary>
        /// Eventos importantes
        /// </summary>
        public const int Seccion44 = 44;

        /// <summary>
        /// Informes 
        /// </summary>
        public const int Seccion45 = 45;

        /// <summary>
        /// Sortep
        /// </summary>
        public const int Seccion51 = 51;
    }
}
