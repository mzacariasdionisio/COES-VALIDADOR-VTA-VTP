using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.ServicioRPF.Helper;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.ServicioRPF
{
    public class DatosAGCAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DatosAGCAppServicio));
        private readonly RsfAppServicio rsfServicio = new RsfAppServicio();
        private readonly RpfAppServicio rpfServicio = new RpfAppServicio();
        private readonly CostoOportunidadAppServicio costoOpServicio = new CostoOportunidadAppServicio();


        #region Funciones

        /// <summary>
        /// Permite obtener la configuracion para las señales
        /// </summary>
        /// <param name="idUrs"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CoConfiguracionGenDTO> ObtenerConfiguracionGenerador(int idUrs, DateTime fecha)
        {
            List<CoConfiguracionGenDTO> entitys = new List<CoConfiguracionGenDTO>();
            List<EveRsfdetalleDTO> configuracion = rsfServicio.ObtenerConfiguracion(fecha).Where(x => x.Grupocodi == idUrs).ToList();

            CoVersionDTO version = FactorySic.GetCoVersionRepository().ObtenerVersionPorFecha(fecha);
            if (version == null) return entitys;

            List<CoConfiguracionDetDTO> configuracionExtranet = (FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracion((int)version.Copercodi, version.Covercodi)).Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoReporteExtranet).
                    OrderBy(x => x.Courdevigenciadesde).ToList();
            List<CoConfiguracionDetDTO> configExtranet = configuracionExtranet.Where(x => x.Grupocodi == idUrs).ToList();

            if (configExtranet.Any())
            {
                CoConfiguracionDetDTO configuracionReporte = (new CostoOportunidadAppServicio()).ObtenerConfiguracionPorDia(configExtranet, version, fecha);

                if (configuracionReporte.Courdereporte == ConstantesCostoOportunidad.TipoCentral.ToString())
                {
                    List<EveRsfdetalleDTO> datos = configuracion.Where(x => x.Grupotipo == ConstantesAppServicio.SI).ToList();

                    foreach (EveRsfdetalleDTO item in datos)
                    {
                        CoConfiguracionGenDTO entity = new CoConfiguracionGenDTO();
                        entity.Equinomb = item.Ursnomb;
                        entity.Equicodi = item.Equicodi;
                        entitys.Add(entity);
                    }
                }
                else
                {
                    List<EveRsfdetalleDTO> datos = configuracion.Where(x => x.Grupotipo != ConstantesAppServicio.SI).ToList();
                    List<CoConfiguracionGenDTO> detalle = FactorySic.GetCoConfiguracionGenRepository().GetByCriteria(configuracionReporte.Courdecodi);

                    foreach (EveRsfdetalleDTO item in datos)
                    {
                        if (detalle.Where(x => x.Equicodi == item.Equicodi).Count() > 0)
                        {
                            CoConfiguracionGenDTO entity = new CoConfiguracionGenDTO();
                            entity.Equinomb = item.Ursnomb;
                            entity.Equicodi = item.Equicodi;
                            entitys.Add(entity);
                        }
                    }
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener las empresas que son dueñas de alguna URS
        /// </summary>
        /// <returns></returns>
        public List<int> ObtenerEmpresasURS()
        {
            return (new RsfAppServicio()).ObtenerConfiguracion(DateTime.Now).Select(x => x.Emprcodi).Distinct().ToList();
        }

        /// <summary>
        /// Permite obtener las empresas que son dueñas de alguna URS
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasPropietariasURS()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            List<SiEmpresaDTO> result = (new RsfAppServicio()).ObtenerConfiguracion(DateTime.Now).Select(x => new SiEmpresaDTO { Emprcodi = x.Emprcodi, Emprnomb = x.Emprnomb }).ToList();

            foreach(SiEmpresaDTO item in result)
            {
                if(entitys.Where(x=>x.Emprcodi == item.Emprcodi).Count() == 0)
                {
                    entitys.Add(item);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener las URS por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EveRsfdetalleDTO> ObtenerUrsPorEmpresa(int idEmpresa)
        {
            return (new RsfAppServicio()).ObtenerConfiguracion(DateTime.Now).Where(x => x.Emprcodi == idEmpresa && x.Grupotipo == ConstantesAppServicio.SI).ToList();
        }

        /// <summary>
        /// Permite obtener la configuracion de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<CoConfiguracionGenDTO> ObtenerConfiguracionPorEmpresa(int idEmpresa, DateTime fecha, out int result, out List<ResultFormatoAGC> mensajes)
        {
            try
            {
                List<ResultFormatoAGC> resultMensaje = new List<ResultFormatoAGC>();
                result = 1;
                List<CoConfiguracionGenDTO> entitys = new List<CoConfiguracionGenDTO>();
                List<EveRsfdetalleDTO> configuracion = (new RsfAppServicio()).ObtenerConfiguracion(DateTime.Now).Where(x => x.Emprcodi == idEmpresa).ToList();

                CoVersionDTO version = FactorySic.GetCoVersionRepository().ObtenerVersionPorFecha(fecha);

                if (version != null)
                {
                    //- Obtenemos la configuración de las URS
                    List<CoConfiguracionDetDTO> configuracionURS = FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracion((int)version.Copercodi, version.Covercodi);

                    List<CoConfiguracionDetDTO> configuracionExtranet = configuracionURS.Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoReporteExtranet).
                            OrderBy(x => x.Courdevigenciadesde).ToList();

                    //- Listado de URS con formulación especial
                    List<CoUrsEspecialDTO> listURSEspecial = FactorySic.GetCoUrsEspecialRepository().GetByCriteria(version.Covercodi);

                    var idUrs = configuracion.Select(x => new { x.Grupocodi}).Distinct().ToList();

                    foreach (var item in idUrs)
                    {
                        EveRsfdetalleDTO itemConfigURS = configuracion.Where(x => x.Grupocodi == item.Grupocodi).FirstOrDefault();
                        List<CoConfiguracionDetDTO> configURS = configuracionURS.Where(x => x.Grupocodi == item.Grupocodi).ToList();
                        DateTime fecInicioHab = DateTime.MinValue;
                        DateTime fecFinHab = DateTime.MaxValue;
                        if (configURS.Count > 0)
                        {
                            fecInicioHab = (configURS[0].FecInicioHabilitacion != null) ? (DateTime)configURS[0].FecInicioHabilitacion : DateTime.MinValue;
                            fecFinHab = (configURS[0].FecFinHabilitacion != null) ? (DateTime)configURS[0].FecFinHabilitacion : DateTime.MaxValue;
                        }

                        bool flagHabilitacion = true;
                        if (!(fecFinHab.Subtract(fecha).TotalSeconds >= 0 && fecha.Subtract(fecInicioHab).TotalSeconds >= 0))
                            flagHabilitacion = false;

                        if (flagHabilitacion)
                        {
                            if (configURS.Count > 0)
                            {
                                if (listURSEspecial.Where(x => x.Grupocodi == item.Grupocodi).Count() == 0)
                                {

                                    List<CoConfiguracionDetDTO> configExtranet = configuracionExtranet.Where(x => x.Grupocodi == item.Grupocodi).ToList();

                                    CoConfiguracionDetDTO configuracionReporte = (new CostoOportunidadAppServicio()).ObtenerConfiguracionPorDia(configExtranet, version, fecha);

                                    if (configuracionReporte != null)
                                    {

                                        if (configuracionReporte.Courdereporte == ConstantesCostoOportunidad.TipoCentral.ToString())
                                        {
                                            List<EveRsfdetalleDTO> datos = configuracion.Where(x => x.Grupotipo == ConstantesAppServicio.SI && x.Grupocodi == item.Grupocodi).ToList();

                                            foreach (EveRsfdetalleDTO itemConfig in datos)
                                            {
                                                CoConfiguracionGenDTO entity = new CoConfiguracionGenDTO();
                                                entity.Equinomb = itemConfig.Ursnomb;
                                                entity.Equicodi = itemConfig.Equicodi;
                                                entity.Grupocodi = itemConfig.Grupocodi;
                                                entity.Centralnomb = itemConfig.Gruponomb;
                                                entity.Emprnomb = itemConfig.Emprnomb;
                                                entity.Codigo = itemConfig.Grupocodi + "-" + itemConfig.Equicodi;

                                                entitys.Add(entity);
                                            }
                                        }
                                        else
                                        {
                                            List<EveRsfdetalleDTO> datos = configuracion.Where(x => x.Grupotipo != ConstantesAppServicio.SI && x.Grupocodi == item.Grupocodi).ToList();
                                            List<CoConfiguracionGenDTO> detalle = FactorySic.GetCoConfiguracionGenRepository().GetByCriteria(configuracionReporte.Courdecodi);

                                            if (detalle.Count > 0)
                                            {

                                                foreach (EveRsfdetalleDTO itemConfig in datos)
                                                {
                                                    if (detalle.Where(x => x.Equicodi == itemConfig.Equicodi).Count() > 0)
                                                    {
                                                        CoConfiguracionGenDTO entity = new CoConfiguracionGenDTO();
                                                        entity.Equinomb = itemConfig.Ursnomb;
                                                        entity.Equicodi = itemConfig.Equicodi;
                                                        entity.Grupocodi = itemConfig.Grupocodi;
                                                        entity.Centralnomb = itemConfig.Gruponomb;
                                                        entity.Emprnomb = itemConfig.Emprnomb;
                                                        entity.Codigo = itemConfig.Grupocodi + "-" + itemConfig.Equicodi;
                                                        entitys.Add(entity);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ResultFormatoAGC itemMensaje = new ResultFormatoAGC();
                                                itemMensaje.NombreURS = itemConfigURS.Ursnomb;
                                                itemMensaje.Mensaje = "La URS " + itemConfigURS.Ursnomb + " no tiene unidades configuradas";
                                                resultMensaje.Add(itemMensaje);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ResultFormatoAGC itemMensaje = new ResultFormatoAGC();
                                        itemMensaje.NombreURS = itemConfigURS.Ursnomb;
                                        itemMensaje.Mensaje = "La URS " + itemConfigURS.Ursnomb + " no tiene configuración para la fecha seleccionada";
                                        resultMensaje.Add(itemMensaje);
                                    }
                                }
                                else
                                {
                                    List<EveRsfdetalleDTO> datos = configuracion.Where(x => x.Grupotipo != ConstantesAppServicio.SI && x.Grupocodi == item.Grupocodi).ToList();

                                    foreach (EveRsfdetalleDTO itemConfig in datos)
                                    {
                                        CoConfiguracionGenDTO entity = new CoConfiguracionGenDTO();
                                        entity.Equinomb = itemConfig.Ursnomb;
                                        entity.Equicodi = itemConfig.Equicodi;
                                        entity.Grupocodi = itemConfig.Grupocodi;
                                        entity.Centralnomb = itemConfig.Gruponomb;
                                        entity.Emprnomb = itemConfig.Emprnomb;
                                        entity.Codigo = itemConfig.Grupocodi + "-" + itemConfig.Equicodi;
                                        entitys.Add(entity);
                                    }
                                }
                            }
                            else
                            {
                                ResultFormatoAGC itemMensaje = new ResultFormatoAGC();
                                itemMensaje.NombreURS = itemConfigURS.Ursnomb;
                                itemMensaje.Mensaje = "La URS " + itemConfigURS.Ursnomb + " no tiene configuración";
                                resultMensaje.Add(itemMensaje);
                            }
                        }
                        else
                        {
                            ResultFormatoAGC itemMensaje = new ResultFormatoAGC();
                            itemMensaje.NombreURS = itemConfigURS.Ursnomb;
                            itemMensaje.Mensaje = "La URS " + itemConfigURS.Ursnomb + " no se encuentra habilitada";
                            resultMensaje.Add(itemMensaje);
                        }
                    }
                }
                else
                {
                    result = 2;
                }
                mensajes = resultMensaje;
                return entitys;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result = -1;
                mensajes = new List<ResultFormatoAGC>();
                return new List<CoConfiguracionGenDTO>();
            }

        }
       

        /// <summary>
        /// Permite generar el formato de carga de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public int GenerarArchivoCargaAGC(int idEmpresa, DateTime fecha, string path, out string file, out List<ResultFormatoAGC> mensajesResult)
        {
            try
            {
                int result = 1;
                file = string.Empty;
                List<ResultFormatoAGC> mensajes = new List<ResultFormatoAGC>();
                List<CoConfiguracionGenDTO> entitys = this.ObtenerConfiguracionPorEmpresa(idEmpresa, fecha, out result, out mensajes);

                if (result == 1)
                {
                    if (entitys.Count > 0)
                    {
                        string[][] data = new string[6 + 24 * 60 * 60][];

                        int row = 0;
                        int column = 1;
                        data[0] = new string[1 + entitys.Count * 2];
                        data[1] = new string[4];
                        data[2] = new string[1 + entitys.Count * 2];
                        data[3] = new string[1 + entitys.Count * 2];
                        data[4] = new string[1 + entitys.Count * 2];
                        data[5] = new string[1 + entitys.Count * 2];

                        data[0][0] = "ID";
                        data[1][0] = "Fecha";
                        data[2][0] = "Empresa";
                        data[3][0] = "Central";
                        data[4][0] = "Equipo";
                        data[5][0] = "Fecha / Hora";
                        row = 6;

                        for (int j = 1; j <= 24; j++)
                        {
                            for (int m = 1; m <= 60; m++)
                            {
                                for (int k = 1; k <= 60; k++)
                                {
                                    data[row] = new string[1 + entitys.Count * 2];
                                    data[row][0] = fecha.AddMinutes((j - 1) * 60 + (m - 1)).AddSeconds(k - 1).ToString(ConstantesAppServicio.FormatoHHmmss);
                                    row++;
                                }
                            }
                        }

                        column = 1;

                        data[1][column] = fecha.Year.ToString();
                        data[1][column + 1] = fecha.Month.ToString();
                        data[1][column + 2] = fecha.Day.ToString();

                        foreach (CoConfiguracionGenDTO entity in entitys)
                        {
                            data[0][column] = data[0][column + 1] = entity.Grupocodi + "-" + entity.Equicodi;
                            data[2][column] = data[2][column + 1] = entity.Emprnomb.Trim();
                            data[3][column] = data[3][column + 1] = entity.Centralnomb.Trim();
                            data[4][column] = data[4][column + 1] = entity.Equinomb.Trim();
                            data[5][column] = "SetPoint";
                            data[5][column + 1] = "Estado";
                            column = column + 2;
                        }

                        SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
                        string fileName = empresa.Emprabrev + "_" + fecha.ToString("ddMMyyyy") + ".csv";

                        if (System.IO.File.Exists(path + fileName))
                        {
                            System.IO.File.Delete(path + fileName);
                        }

                        using (var outFile = System.IO.File.CreateText(path + fileName))
                        {
                            foreach (string[] itemResult in data)
                            {
                                outFile.WriteLine(string.Join(",", itemResult));
                            }
                        }
                        file = fileName;
                    }
                    else
                    {
                        result = 3; // no existen datos
                    }
                }

                mensajesResult = mensajes;
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                file = string.Empty;
                mensajesResult = new List<ResultFormatoAGC>();
                return -1;
            }
        }        


        /// <summary>
        /// Permite validar el archivo de carga
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="file"></param>
        /// <param name="validaciones"></param>
        /// <param name="indicador"></param>
        /// <param name="fechaSeleccion"></param>
        /// <returns></returns>
        public List<RpfMedicion60DTO> ProcesarArchivo(int idEmpresa, string file, out List<string> validaciones, out bool indicador,
            DateTime fechaSeleccion)
        {
            List<String> errors = new List<String>();
            List<RpfMedicion60DTO> list = new List<RpfMedicion60DTO>();
            int result = 0;
            List<ResultFormatoAGC> mensajes = new List<ResultFormatoAGC>();
            List<CoConfiguracionGenDTO> estructura = this.ObtenerConfiguracionPorEmpresa(idEmpresa, fechaSeleccion, out result, out mensajes);

            bool flag = true;

            #region Lectura completa del archivo

            List<string[]> arreglo = new List<string[]>();
            string[] ptoMedicion = null;
            string[] tipoInfo = null;
            string[] fecha = null;
            char[] separer = new char[2];
            separer[0] = ',';
            separer[1] = ';';
            char[] separerRaya = new char[1];
            separerRaya[0] = '-';

            string line = null;
            using (System.IO.StreamReader sr = System.IO.File.OpenText(file))
            {
                ptoMedicion = sr.ReadLine().Split(separer, StringSplitOptions.RemoveEmptyEntries);
                fecha = sr.ReadLine().Split(separer, StringSplitOptions.RemoveEmptyEntries);
                for (int t = 1; t <= 3; t++) sr.ReadLine();
                tipoInfo = sr.ReadLine().Split(separer, StringSplitOptions.RemoveEmptyEntries);

                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (i == 0)
                    {
                        string[] verificacion = line.Split(separer, StringSplitOptions.RemoveEmptyEntries);
                        bool flagInicio = false;
                        if (verificacion.Length > 0)
                        {
                            if (verificacion[0].Trim() == ConstantesDatosAGC.HoraCero || verificacion[0].Trim() == ConstantesDatosAGC.HoraCeroAlter)
                                flagInicio = true;
                        }
                        if (!flagInicio)
                            errors.Add(ConstantesDatosAGC.DatosInicio);
                    }
                    arreglo.Add(line.Split(separer, StringSplitOptions.RemoveEmptyEntries));
                    i++;
                }
            }

            #endregion

            #region Validacion de estructura de códigos

            if (ptoMedicion.Length == tipoInfo.Length)
            {
                if (ptoMedicion.Length > 2 && (ptoMedicion.Length - 1) % 2 == 0)
                {
                    //- Validamos códigos duplicados
                    List<string> codigos = ptoMedicion.Distinct().ToList();

                    foreach(string codigo in codigos)
                    {
                        if(ptoMedicion.Where(x=>x== codigo).Count() > 2)
                        {
                            errors.Add(String.Format(ConstantesDatosAGC.FormatoCodigosDuplicados, codigo.ToString()));
                        }
                    }


                    for (int i = 1; i <= ptoMedicion.Length - 1; i++)
                    {
                        int codigoPunto = 0;

                        string[] dataPto = ptoMedicion[i].Split(separerRaya, StringSplitOptions.RemoveEmptyEntries);

                        if (dataPto.Length == 2)
                        {
                            if (!int.TryParse(dataPto[0], out codigoPunto) || !int.TryParse(dataPto[1], out codigoPunto))
                                errors.Add(String.Format(ConstantesDatosAGC.FormatoIncorrectoPtoMedicion, (i + 1).ToString()));
                        }

                        if (i % 2 == 0)
                        {
                            if (ptoMedicion[i] != ptoMedicion[i - 1])
                                errors.Add(String.Format(ConstantesDatosAGC.CodigosPuntosIguales, (i + 1).ToString()));
                            if (tipoInfo[i] != ConstantesDatosAGC.Estado)
                                errors.Add(String.Format(ConstantesDatosAGC.CodigosValorEstado, (i + 1).ToString()));
                        }
                        else
                        {
                            if (codigoPunto > 0)
                            {
                                if (!(estructura.Where(x => x.Codigo == ptoMedicion[i]).Count() > 0))
                                    errors.Add(String.Format(ConstantesDatosAGC.CodigoInvalido, (i + 1).ToString()));
                            }
                            if (tipoInfo[i] != ConstantesDatosAGC.SetPoint)
                                errors.Add(String.Format(ConstantesDatosAGC.CodigosValorSetPoint, (i + 1).ToString()));
                        }
                    }
                }
                else
                {
                    errors.Add(ConstantesDatosAGC.VerificarCantidadCodigos);
                }
            }
            else
            {
                errors.Add(ConstantesDatosAGC.CantidadPtoMedicionVSTipoInfo);
            }

            if (errors.Count > 0) flag = false;

            #endregion

            #region Validacion de fechas

            DateTime today = DateTime.Now;

            if (fecha.Length == 4)
            {
                int anio = 0; 
                int mes = 0; 
                int dia = 0; 

                if (!int.TryParse(fecha[1], out anio))
                {
                    errors.Add(ConstantesDatosAGC.AnioIncorrecto);
                }
                if (!int.TryParse(fecha[2], out mes))
                {
                    errors.Add(ConstantesDatosAGC.MesIncorrecto);
                }
                if (!int.TryParse(fecha[3], out dia))
                {
                    errors.Add(ConstantesDatosAGC.DiaIncorrecto);
                }

                today = DateTime.ParseExact(dia.ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero) + ConstantesDatosAGC.CaracterSlash +
                    mes.ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero) + ConstantesDatosAGC.CaracterSlash + anio, ConstantesAppServicio.FormatoFecha,
                    CultureInfo.InvariantCulture);

                if (!(fechaSeleccion.Year == today.Year && fechaSeleccion.Month == today.Month && fechaSeleccion.Day == today.Day))
                {
                    errors.Add(ConstantesDatosAGC.FechasNoCoinciden);
                }
            }
            else
            {
                errors.Add(ConstantesDatosAGC.FechaNoValida);
            }

            if (errors.Count > 0) flag = false;


            #endregion

            #region Valicion de secuencia de datos

            int index = 0;
            bool flagFila = true;
            for (int i = 1; i <= 24; i++)
            {
                for (int j = 1; j <= 60; j++)
                {
                    for (int k = 1; k <= 60; k++)
                    {
                        if (arreglo[index].Length == ptoMedicion.Length)
                        {
                            if (arreglo[index][0].Split(':').Length == 3)
                            {
                                string cad = arreglo[index][0].Split(':')[2];

                                if ((k - 1) != int.Parse(cad))
                                {
                                    errors.Add(String.Format(ConstantesDatosAGC.NoExisteRegistro, (i - 1).ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero) +
                                        ConstantesDatosAGC.CaracterPuntos + (j - 1).ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero) + ConstantesDatosAGC.CaracterPuntos +
                                        (k - 1).ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero)));
                                    flagFila = false;
                                    break;
                                }
                            }
                            else
                            {
                                errors.Add(String.Format(ConstantesDatosAGC.FechaNoTieneFormato, (i - 1).ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero) +
                                        ConstantesDatosAGC.CaracterPuntos + (j - 1).ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero) + ConstantesDatosAGC.CaracterPuntos +
                                        (k - 1).ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero)));
                                flagFila = false;
                                break;
                            }
                        }
                        else
                        {
                            errors.Add(String.Format(ConstantesDatosAGC.FechaNoDatosCompleto, (i - 1).ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero) +
                                       ConstantesDatosAGC.CaracterPuntos + (j - 1).ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero) + ConstantesDatosAGC.CaracterPuntos +
                                       (k - 1).ToString().PadLeft(2, ConstantesDatosAGC.CaracterCero)));
                            flagFila = false;
                            break;
                        }
                        index++;
                    }
                    if (!flagFila) break;
                }
                if (!flagFila) break;
            }

            if (!flagFila) flag = false;

            #endregion

            #region Cargado de datos

            bool flagNegativo = true;

            if (flag)
            {
                string[] dataPto = null;
                string senial = string.Empty;
                decimal valor = 0;
                int indice = 0;

                for (int i = 1; i < ptoMedicion.Length; i++)
                {
                    dataPto = ptoMedicion[i].Split(separerRaya, StringSplitOptions.RemoveEmptyEntries);
                    senial = tipoInfo[i];

                    for (int j = 1; j <= 24; j++)
                    {
                        for (int k = 1; k <= 60; k++)
                        {
                            RpfMedicion60DTO entity = new RpfMedicion60DTO();
                            entity.Grupocodi = int.Parse(dataPto[0]);
                            entity.Equicodi = int.Parse(dataPto[1]);
                            entity.Cotidacodi = (senial == ConstantesDatosAGC.SetPoint) ? ConstantesDatosAGC.CodigoSetPoint : ConstantesDatosAGC.CodigoEstado;
                            entity.Rpfmedfecha = today.AddHours(j - 1).AddMinutes(k - 1);

                            for (int t = 1; t <= 60; t++)
                            {
                                indice = (j - 1) * 60 * 60 + (k - 1) * 60 + t - 1;
                                valor = 0;
                                if (decimal.TryParse(arreglo[indice][i], NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
                                {
                                    if (valor >= 0)
                                    {
                                        decimal valor5decimales = MathHelper.TruncateDecimal(valor, 5);
                                        entity.GetType().GetProperty("H" + t).SetValue(entity, valor5decimales);
                                    }
                                    else
                                    {
                                        if (flagNegativo)
                                        {
                                            errors.Add(ConstantesDatosAGC.ValoresNegativosNoPermitidos);
                                            flagNegativo = false;
                                            flag = false;
                                        }
                                    }
                                }
                                else
                                {
                                    errors.Add(String.Format(ConstantesDatosAGC.ValidacionFormatoNumero, (indice + 7).ToString(), (i + 1).ToString()));
                                    flag = false;
                                }
                            }

                            list.Add(entity);
                        }
                    }
                }
            }

            #endregion

            validaciones = errors;
            indicador = flag;

            return list;
        }

        /// <summary>
        /// Permite almacenar los datos del AGC
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="fuente"></param>
        /// <param name="usuario"></param>
        /// <param name="errores"></param>
        /// <returns></returns>
        public int GrabarDatos(string fileName, int idEmpresa, DateTime fecha, string fuente, string usuario, out List<string> errores)
        {
            try
            {
                int resultado = 1;
                List<string> validaciones = new List<string>();
                bool flag = false;
                string estado = string.Empty;

                if (fuente == ConstantesDatosAGC.FuenteExtranet)
                {
                    flag = true;
                    DateTime fechaActual = DateTime.Now;
                    decimal paramValor = (new ParametroAppServicio()).ObtenerValorParametro(ConstantesDatosAGC.IdParametroPlazo, DateTime.Now);

                    if (paramValor != 0)
                    {
                        string textoValor = paramValor.ToString().PadLeft(4, '0');
                        string horaPermitida = textoValor.Substring(0, 2) + ":" + textoValor.Substring(2, 2);

                        string[] strHoraPermitida = horaPermitida.Split(ConstantesDatosAGC.SeparadorPuntos);
                        int hora = int.Parse(strHoraPermitida[0]);
                        int minuto = int.Parse(strHoraPermitida[1]);
                        int dias = (int)fechaActual.Subtract(fecha).TotalDays;

                        if (dias == 1)
                        {
                            if (fechaActual.Hour > hora || (fechaActual.Hour == hora && fechaActual.Minute > 0))
                            {
                                validaciones.Add(String.Format(ConstantesDatosAGC.HoraNoPermitida, horaPermitida));
                                resultado = 3; // Se ha superado el placo de envio
                            }
                            else
                            {
                                estado = ConstantesDatosAGC.EnviadoEnPlazo;
                            }
                        }
                        else
                        {
                            resultado = 4; // Solo se permiten fechas del dia anterior
                        }
                    }
                    else
                    {
                        resultado = 5; //- No existe plazo configurado
                    }
                }
                else if (fuente == ConstantesDatosAGC.FuenteIntranet)
                {
                    estado = ConstantesDatosAGC.EnviadoPorCOES;
                    flag = true;
                }

                if (resultado == 1)
                {

                    List<RpfMedicion60DTO> data = this.ProcesarArchivo(idEmpresa, fileName, out validaciones, out flag, fecha);

                    if (flag)
                    {
                        string tableName = "RPF_MEDICION60_" + fecha.Year + string.Format("{0:D2}", fecha.Month);

                        RpfEnvioDTO envio = new RpfEnvioDTO();
                        envio.Rpfenvestado = estado;
                        envio.Rpfenvfeccreacion = DateTime.Now;
                        envio.Rpfenvfecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        envio.Rpfenvfecmodificacion = DateTime.Now;
                        envio.Rpfenvusucreacion = usuario;
                        envio.Rpfenvusumodificacion = usuario;
                        envio.Emprcodi = idEmpresa;

                        RpfEnvioDTO entity = FactorySic.GetRpfEnvioRepository().ObtenerPorFecha(fecha, idEmpresa);

                        int idEnvio = 0;

                        if (entity != null)
                        {
                            idEnvio = envio.Rpfenvcodi = entity.Rpfenvcodi;
                            FactorySic.GetRpfEnvioRepository().Update(envio);
                        }
                        else
                        {
                            idEnvio = FactorySic.GetRpfEnvioRepository().Save(envio);
                        }

                        //- Hacemos una eliminación previa
                        FactorySic.GetRpfMedicion60Repository().Delete(idEnvio, tableName);

                        int maxId = FactorySic.GetRpfMedicion60Repository().GetMaxId(tableName);

                        //- Llamamos a la inserción masiva
                        foreach (RpfMedicion60DTO item in data)
                        {
                            item.Rpfmedcodi = maxId;
                            item.Rpfenvcodi = idEnvio;
                            maxId++;
                        }

                        FactorySic.GetRpfMedicion60Repository().GrabarMasivo(data, tableName);
                    }

                    if (!flag)
                    {
                        resultado = 7;
                    }
                }

                errores = validaciones;
                return resultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                errores = new List<string>();
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener la consulta de datos reportados por el agente
        /// </summary>
        /// <param name="idUrs"></param>
        /// <param name="idEquipo"></param>
        /// <param name="fecha"></param>
        /// <param name="tipoDato"></param>
        /// <param name="minutoInicio"></param>
        /// <param name="minutoFin"></param>
        /// <returns></returns>
        public string[][] ObtenerConsultaDatos(int idUrs, int idEquipo, DateTime fecha, int tipoDato, int minutoInicio, int minutoFin)
        {
            //- Aca nos falta verificar existencia de tabla
            string tableName = "RPF_MEDICION60_" + fecha.Year + string.Format("{0:D2}", fecha.Month);

            DateTime fechaInicio = fecha.AddHours(minutoInicio);
            DateTime fechaFin = fecha.AddHours(minutoFin);

            List<RpfMedicion60DTO> data = FactorySic.GetRpfMedicion60Repository().ObtenerConsulta(idUrs, idEquipo, tipoDato,
                fechaInicio, fechaFin, tableName);

            string[][] result = new string[data.Count + 1][];
            string[] header = new string[1 + 60];
            header[0] = "Hora";
            for (int i = 1; i <= 60; i++)
            {
                header[i] = i + "''";
            }
            result[0] = header;
            int index = 1;
            foreach (RpfMedicion60DTO item in data)
            {
                string[] itemData = new string[1 + 60];
                itemData[0] = ((DateTime)item.Rpfmedfecha).ToString(ConstantesAppServicio.FormatoHora);
                for (int i = 1; i <= 60; i++)
                {
                    var obj = item.GetType().GetProperty("H" + i).GetValue(item, null);
                    decimal? valor = (obj != null) ? (decimal?)Convert.ToDecimal(obj) : null;
                    itemData[i] = (valor != null) ? valor.ToString() : string.Empty;
                }
                result[index] = itemData;
                index++;
            }
            return result;
        }

        /// <summary>
        /// Permite obtener el reporte de cumplimiento
        /// </summary>
        /// <param name="fecha"></param>
        public List<CoConfiguracionGenDTO> ObtenerReporteCumplimiento(DateTime fecha)
        {
            List<CoConfiguracionGenDTO> entitys = new List<CoConfiguracionGenDTO>();

            //- Obtenemos los envios del dia
            List<RpfEnvioDTO> listaEnvios = FactorySic.GetRpfEnvioRepository().ObtenerEnviosPorFecha(fecha);

            //- Listamos todas las URS
            List<EveRsfdetalleDTO> configuracion = rsfServicio.ObtenerConfiguracion(fecha);

            //- Obtenemos la versión correspondiente
            CoVersionDTO version = FactorySic.GetCoVersionRepository().ObtenerVersionPorFecha(fecha);
           
            if (version == null) return entitys;

            //- Obtenermos las configuraciones de reporte extranet para la fecha seleccionada
            List<CoConfiguracionDetDTO> configuracionExtranet = (FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracion(
                (int)version.Copercodi, version.Covercodi)).Where(x => x.Courdetipo == ConstantesCostoOportunidad.TipoReporteExtranet).
                    OrderBy(x => x.Courdevigenciadesde).ToList();

            //- Obtenemos el listado de URS
            List<int> idsURS = configuracion.Select(x => (int)x.Grupocodi).Distinct().ToList();

            foreach (int idUrs in idsURS)
            {
                List<CoConfiguracionDetDTO> configExtranet = configuracionExtranet.Where(x => x.Grupocodi == idUrs).ToList();

                if (configExtranet.Any())
                {
                    CoConfiguracionDetDTO configuracionReporte = (new CostoOportunidadAppServicio()).ObtenerConfiguracionPorDia(configExtranet, version, fecha);

                    if (configuracionReporte.Courdereporte == ConstantesCostoOportunidad.TipoCentral.ToString())
                    {
                        List<EveRsfdetalleDTO> datos = configuracion.Where(x => x.Grupocodi == idUrs && x.Grupotipo == ConstantesAppServicio.SI).ToList();

                        foreach (EveRsfdetalleDTO item in datos)
                        {
                            CoConfiguracionGenDTO entity = this.ObtenerItemReporte(item, listaEnvios);
                            entitys.Add(entity);
                        }
                    }
                    else
                    {
                        List<EveRsfdetalleDTO> datos = configuracion.Where(x => x.Grupocodi == idUrs && x.Grupotipo != ConstantesAppServicio.SI).ToList();
                        List<CoConfiguracionGenDTO> detalle = FactorySic.GetCoConfiguracionGenRepository().GetByCriteria(configuracionReporte.Courdecodi);

                        foreach (EveRsfdetalleDTO item in datos)
                        {
                            if (detalle.Where(x => x.Equicodi == item.Equicodi).Count() > 0)
                            {
                                CoConfiguracionGenDTO entity = this.ObtenerItemReporte(item, listaEnvios);
                                entitys.Add(entity);
                            }
                        }
                    }
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite genear el formato excel con los datos exportados
        /// </summary>
        /// <param name="puntos"></param>
        /// <param name="fecha"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void GenerarReporteCumplimiento(string puntos, DateTime fecha, string path, out string fileName)
        {
            try
            {
                List<EveRsfdetalleDTO> entitys = new List<EveRsfdetalleDTO>();

                //- Obtenemos los datos de urs seleccionadas
                string[] ids = puntos.Split(',');
                foreach (string id in ids)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        string[] codes = id.Split('-');
                        int grupocodi = int.Parse(codes[0]);
                        int equicodi = int.Parse(codes[1]);
                        entitys.Add(new EveRsfdetalleDTO { Grupocodi = grupocodi, Equicodi = equicodi });
                    }
                }

                //- Listamos todas las URS con propiedades par el pintado del reporte
                List<EveRsfdetalleDTO> configuracion = rsfServicio.ObtenerConfiguracion(fecha);

                //- Obtenemos los datos para la consulta
                string tableName = "RPF_MEDICION60_" + fecha.Year + string.Format("{0:D2}", fecha.Month);
                string grupos = string.Join(",", entitys.Select(n => n.Grupocodi.ToString()).Distinct().ToArray());
                List<RpfMedicion60DTO> result = FactorySic.GetRpfMedicion60Repository().ObtenerReporteCumplimiento(fecha, grupos, tableName);

                //- Obteniendo la estructura del reporte
                string[][] data = new string[6 + 24 * 60 * 60][];

                int row = 0;
                int column = 1;
                data[0] = new string[1 + entitys.Count * 2];
                data[1] = new string[4];
                data[2] = new string[1 + entitys.Count * 2];
                data[3] = new string[1 + entitys.Count * 2];
                data[4] = new string[1 + entitys.Count * 2];
                data[5] = new string[1 + entitys.Count * 2];

                data[0][0] = "ID";
                data[1][0] = "Fecha";
                data[2][0] = "Empresa";
                data[3][0] = "Central";
                data[4][0] = "Equipo";
                data[5][0] = "Fecha / Hora";
                row = 6;

                for (int j = 1; j <= 24; j++)
                {
                    for (int m = 1; m <= 60; m++)
                    {
                        for (int k = 1; k <= 60; k++)
                        {
                            data[row] = new string[1 + entitys.Count * 2];
                            data[row][0] = fecha.AddMinutes((j - 1) * 60 + (m - 1)).AddSeconds(k - 1).ToString(ConstantesAppServicio.FormatoHHmmss);
                            row++;
                        }
                    }
                }

                column = 1;

                data[1][column] = fecha.Year.ToString(); 
                data[1][column + 1] = fecha.Month.ToString(); 
                data[1][column + 2] = fecha.Day.ToString();

                foreach (EveRsfdetalleDTO item in entitys)
                {
                    EveRsfdetalleDTO entity = configuracion.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();

                    data[0][column] = data[0][column + 1] = entity.Grupocodi + "-" + entity.Equicodi;
                    data[2][column] = data[2][column + 1] = entity.Emprnomb.Trim();
                    data[3][column] = data[3][column + 1] = entity.Gruponomb.Trim();
                    data[4][column] = data[4][column + 1] = entity.Ursnomb.Trim();
                    data[5][column] = "SetPoint";
                    data[5][column + 1] = "Estado";

                    List<RpfMedicion60DTO> subData = result.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).ToList();
                    List<RpfMedicion60DTO> subDataSetPoint = subData.Where(x => x.Cotidacodi == ConstantesDatosAGC.CodigoSetPoint).ToList();
                    List<RpfMedicion60DTO> subDataEstado = subData.Where(x => x.Cotidacodi == ConstantesDatosAGC.CodigoEstado).ToList();

                    row = 6;

                    if (subDataSetPoint.Count > 0)
                    {
                        for (int i = 0; i < subDataSetPoint.Count; i++)
                        {
                            for (int k = 1; k <= 60; k++)
                            {
                                data[row][column] = subDataSetPoint[i].GetType().GetProperty("H" + k).GetValue(subDataSetPoint[i]).ToString();
                                data[row][column + 1] = subDataEstado[i].GetType().GetProperty("H" + k).GetValue(subDataEstado[i]).ToString();
                                row++;
                            }
                        }
                    }
                  

                    column = column + 2;
                }

                string file = "Reporte_" + fecha.ToString("ddMMyyyy") + ".csv";

                if (System.IO.File.Exists(path + file))
                {
                    System.IO.File.Delete(path + file);
                }

                using (var outFile = System.IO.File.CreateText(path + file))
                {
                    foreach (string[] itemResult in data)
                    {
                        outFile.WriteLine(string.Join(",", itemResult));
                    }
                }

                fileName = file;
            }
            catch(Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                fileName = string.Empty;
            }
        }

        /// <summary>
        /// Permite obtener un item del reporte de cumplimiento
        /// </summary>
        /// <param name="item"></param>
        /// <param name="envios"></param>
        /// <returns></returns>
        private CoConfiguracionGenDTO ObtenerItemReporte(EveRsfdetalleDTO item, List<RpfEnvioDTO> envios)
        {
            CoConfiguracionGenDTO entity = new CoConfiguracionGenDTO();
            entity.Equinomb = item.Ursnomb;
            entity.Equicodi = item.Equicodi;
            entity.Grupocodi = item.Grupocodi;
            entity.Emprnomb = item.Emprnomb;
            entity.Centralnomb = item.Gruponomb;
            entity.Codigo = item.Grupocodi + "-" + item.Equicodi;

            if(envios.Where(x=>x.Emprcodi == item.Emprcodi).Count() > 0)
            {
                RpfEnvioDTO envio = envios.Where(x => x.Emprcodi == item.Emprcodi).FirstOrDefault();
                entity.Estadoenvio = (envio.Rpfenvestado == ConstantesDatosAGC.EnviadoEnPlazo) ? ConstantesDatosAGC.TextoEnPlazo :
                    ConstantesDatosAGC.TextoEnviadoPorCOES;
                entity.Fechaenvio = (envio.Rpfenvfeccreacion != null) ? ((DateTime)envio.Rpfenvfeccreacion).ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;
                entity.Usuarioenvio = envio.Rpfenvusucreacion;
            }
            else
            {
                entity.Estadoenvio = ConstantesDatosAGC.TextoNoEnvio;
            }

            return entity;
        }

        #region Comparativo Extranet vs SP7

        /// <summary>
        /// Devuelve el listado de URS y todos sus equipos para cierta fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EveRsfdetalleDTO> ObtenerListadoURSConEquiposPorFecha(DateTime fecha)
        {
            List<EveRsfdetalleDTO> lstSalida = new List<EveRsfdetalleDTO>();

            List<EveRsfdetalleDTO> lstConfiguracion = rsfServicio.ObtenerConfiguracion(fecha);
            lstSalida = lstConfiguracion;

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado de URS por fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EveRsfdetalleDTO> ObtenerListadoURSPorFecha(DateTime fecha)
        {
            List<EveRsfdetalleDTO> lstSalida = new List<EveRsfdetalleDTO>();

            List<EveRsfdetalleDTO> lstConfiguracion = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracion(fecha);

            lstSalida = lstConfiguracion.OrderBy(x=>x.Ursnomb).ToList();

            return lstSalida;
        }


        /// <summary>
        /// Devuelve el listado comparativo de la informacion de extranet con los datos sp7
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idUrs"></param>
        /// <param name="idEquipo"></param>
        /// <param name="senial"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        public List<ComparativoExtranetConSP7> ObtenerListadoComparativoExtranetConSP7(DateTime fechaConsulta, int idEmpresa, int idUrs, int idEquipo, int senial, int resolucion)
        {
            List<ComparativoExtranetConSP7> lstSalida = new List<ComparativoExtranetConSP7>();

            string strIdsUrs = idUrs.ToString();
            string strIdsEquipos = idEquipo.ToString();
            string strTipoDato = senial.ToString();            

            List<RpfMedicion60DTO> dataExtranet = ObtenerInformacionAGCExtranet(fechaConsulta, strIdsUrs, strIdsEquipos, strTipoDato);
            List<CoMedicion60DTO> dataSP7 = ObtenerInformacionSP7(fechaConsulta, strIdsUrs, strIdsEquipos, strTipoDato);

            List<ComparativoExtranetConSP7> lstComparacion = CompararInformacionAGC(dataExtranet, dataSP7, resolucion);

            lstSalida = lstComparacion;
            return lstSalida;
        }


        /// <summary>
        /// Devuelve la informacion AGC del histórico SP7
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="strGrupocodis"></param>
        /// <param name="strEquicodis"></param>
        /// <param name="strCotidacodis"></param>
        /// <returns></returns>
        private List<CoMedicion60DTO> ObtenerInformacionSP7(DateTime fechaConsulta, string strGrupocodis, string strEquicodis, string strCotidacodis)
        {
            List<CoMedicion60DTO> lstSalida = new List<CoMedicion60DTO>();
            string strFecha = fechaConsulta.ToString(ConstantesAppServicio.FormatoFechaWS);

            string nombMesTabla = fechaConsulta.Year.ToString() + fechaConsulta.Month.ToString("00");
            int existeTablaCoMedicion60 = FactorySic.GetCoMedicion60Repository().VerificarExistenciaTabla(nombMesTabla);

            lstSalida = existeTablaCoMedicion60 == 1 ? FactorySic.GetCoMedicion60Repository().GetInformacionAGC(fechaConsulta, strFecha, strGrupocodis, strEquicodis, strCotidacodis) : new List<CoMedicion60DTO>();

            return lstSalida;
        }

        /// <summary>
        /// Devuelve la informacion AGC reportada por agentes (extranet)
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="strIdsUrs"></param>
        /// <param name="strIdsEquipos"></param>
        /// <param name="strTipoDato"></param>
        /// <returns></returns>
        public List<RpfMedicion60DTO> ObtenerInformacionAGCExtranet(DateTime fechaConsulta, string strIdsUrs, string strIdsEquipos, string strTipoDato)
        {
            List<RpfMedicion60DTO> lstSalida = new List<RpfMedicion60DTO>();
            string strFecha = fechaConsulta.ToString(ConstantesAppServicio.FormatoFechaWS);

            string nombMesTabla = fechaConsulta.Year.ToString() + fechaConsulta.Month.ToString("00");
            int existeTablaRpfMedicion60 = FactorySic.GetRpfMedicion60Repository().VerificarExistenciaTabla(nombMesTabla);

            lstSalida = existeTablaRpfMedicion60 == 1 ? FactorySic.GetRpfMedicion60Repository().GetInformacionAGCExtranet(fechaConsulta, strFecha, strIdsUrs, strIdsEquipos, strTipoDato) : new List<RpfMedicion60DTO>();

            return lstSalida;
        }

        /// <summary>
        /// Devuelve un listado con la informacion comparada
        /// </summary>
        /// <param name="dataExtranet"></param>
        /// <param name="dataSP7"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        private List<ComparativoExtranetConSP7> CompararInformacionAGC(List<RpfMedicion60DTO> dataExtranet, List<CoMedicion60DTO> dataSP7, int resolucion)
        {
            List<ComparativoExtranetConSP7> lstSalida = new List<ComparativoExtranetConSP7>();
            DateTime horaX = new DateTime(2000, 1, 1, 0, 0, 0);

            int numReg = 0;
            int minAdd = 0;
            if (resolucion == ConstantesDatosAGC.Resolucion15min)
            {
                numReg = 96;
                minAdd = 15;
            }
            if (resolucion == ConstantesDatosAGC.Resolucion30min)
            {
                numReg = 48;
                minAdd = 30;
            }

            for (int i = 1; i <= numReg; i++)
            {
                ComparativoExtranetConSP7 obj = new ComparativoExtranetConSP7();
                horaX = horaX.AddMinutes(minAdd);

                obj.Hora = horaX.Hour.ToString("00") + ":" + horaX.Minute.ToString("00");
                obj.ValorExtranet = ObtenerDatoExtranet(dataExtranet, horaX.Hour, horaX.Minute);
                obj.ValorSP7 = ObtenerDatoSP7(dataSP7, horaX.Hour, horaX.Minute);
                obj.Diferencia = (obj.ValorSP7 != null ? (obj.ValorExtranet != null ? (obj.ValorSP7 - obj.ValorExtranet) : obj.ValorSP7) : (obj.ValorExtranet != null ? (obj.ValorExtranet) : null));

                decimal? num = obj.Diferencia;
                decimal? den = obj.ValorSP7 != null ? (obj.ValorExtranet != null ? ((Math.Max(obj.ValorSP7.Value, obj.ValorExtranet.Value))) : (obj.ValorSP7.Value)) : (obj.ValorExtranet != null ? obj.ValorExtranet : null);
                obj.Desviacion = den != 0 ? ((num != null && den != null) ? (num / den * 100) : null) : null;

                lstSalida.Add(obj);
            }            

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el registro SP7 para cierta hora y minuto
        /// </summary>
        /// <param name="dataSP7"></param>
        /// <param name="hora"></param>
        /// <param name="minuto"></param>
        /// <returns></returns>
        private decimal? ObtenerDatoSP7(List<CoMedicion60DTO> dataSP7, int hora, int minuto)
        {
            decimal? salida = null;

            CoMedicion60DTO reg = dataSP7.Find(x => x.Comedihora == hora && x.Comediminuto == minuto);

            if (reg != null)
                salida = reg.H1;

            return salida;
        }

        /// <summary>
        /// Devuelve el registro reportado por el agente para cierta hora y minuto
        /// </summary>
        /// <param name="dataExtranet"></param>
        /// <param name="hora"></param>
        /// <param name="minuto"></param>
        /// <returns></returns>
        private decimal? ObtenerDatoExtranet(List<RpfMedicion60DTO> dataExtranet, int hora, int minuto)
        {
            decimal? salida = null;

            RpfMedicion60DTO reg = dataExtranet.Find(x => x.Rpfmedfecha.Value.Hour == hora && x.Rpfmedfecha.Value.Minute == minuto);

            if (reg != null)
                salida = reg.H1;

            return salida;
        }           

        /// <summary>
        /// Devuelve el listado comparativo en html
        /// </summary>
        /// <param name="listadoComparativo"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoComparativo(List<ComparativoExtranetConSP7> listadoComparativo)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tablaComparativa' style='width:380px'>");
            str.Append("<thead>");

            #region cabecera            
            str.Append("<tr>");
            str.Append("<th style='max-width: 50px;'>Hora</th>");
            str.Append("<th style='max-width: 80px;'>Extranet</th>");
            str.Append("<th style='max-width: 80px;'>SP7</th>");
            str.Append("<th style='max-width: 80px;'>Diferencia</th>");
            str.Append("<th style='max-width: 80px;'>Desviación (%)</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");

            foreach (var item in listadoComparativo)
            {                
                str.Append("<tr>");
                
                str.AppendFormat("<td  style='max-width: 50px; text-align: center'>{0}</td>", item.Hora);
                str.AppendFormat("<td  style='max-width: 80px; text-align: center'>{0}</td>", item.ValorExtranet != null ? item.ValorExtranet.Value.ToString("#,##0.00") : "");
                str.AppendFormat("<td  style='max-width: 80px; text-align: center'>{0}</td>", item.ValorSP7 != null ? item.ValorSP7.Value.ToString("#,##0.00"): "");
                str.AppendFormat("<td  style='max-width: 80px; text-align: center'>{0}</td>", item.Diferencia != null ? item.Diferencia.Value.ToString("#,##0.00"): "");
                str.AppendFormat("<td  style='max-width: 80px; text-align: center'>{0}</td>", item.Desviacion != null ? item.Desviacion.Value.ToString("#,##0.00"): "");

                str.Append("</tr>");
            }

            str.Append("</tbody>");

            return str.ToString();
        }

        /// <summary>
        /// Devuelve un objeto con toda la información del gráfico lineal a mostrar en pantalla
        /// </summary>
        /// <param name="listadoComparativo"></param>
        /// <param name="lstUrs"></param>
        /// <param name="idUrs"></param>
        /// <returns></returns>
        public GraficoWeb ObtenerGraficoComparativo(List<ComparativoExtranetConSP7> listadoComparativo, List<EveRsfdetalleDTO> lstUrs, int idUrs)
        {

            EveRsfdetalleDTO urs = lstUrs.Find(x => x.Grupocodi == idUrs);

            #region Datos del grafico
            string nombUrs = urs != null ? urs.Ursnomb.Trim() : "";
            GraficoWeb graficoWeb = new GraficoWeb
            {                
                NameGrafico = "graficoCompExtSP7",
                XAxisCategories = new List<string>(),                
                SerieData = new DatosSerie[listadoComparativo.Count()],                
                TitleText = "Comparativo Extranet VS Histórico SP7 - "  + nombUrs,
                YAxixTitle = new List<string> { "Potencia (MW)" },
                XAxisTitle = "Hora",
                TooltipValueSuffix = " ",
                YaxixLabelsFormat = " ",
                LegendLayout = "horizontal",
                LegendAlign = "center",
                LegendVerticalAlign = "bottom",
                Subtitle = ""
            };

            graficoWeb.XAxisCategories = listadoComparativo.Select(x => x.Hora).ToList();
            #endregion

            #region Cuerpo del grafico
            var indexSerie = 0;

            List<string> lstInformacion = new List<string>();
            lstInformacion.Add("Extranet");
            lstInformacion.Add("Histórico SP7");


            foreach (string dato in lstInformacion)
            {
                List<decimal?> lstDatos = new List<decimal?>();

                var dataReg = listadoComparativo;
                var maxSem = dataReg.Any() ? dataReg.Count() : 1;

                graficoWeb.SerieData[indexSerie] = new DatosSerie()
                {
                    Name = dato,
                    Data = new decimal?[maxSem]
                };
                switch (indexSerie)
                {
                    case 0:

                        graficoWeb.SerieData[indexSerie].Color = "#0E5BBC";
                        lstDatos = listadoComparativo.Select(x => x.ValorExtranet).ToList();
                        break;
                    case 1:

                        graficoWeb.SerieData[indexSerie].Color = "#099B21";
                        lstDatos = listadoComparativo.Select(x => x.ValorSP7).ToList();
                        break;
                }

                int numr = 0;
                foreach (var datito in lstDatos)
                {
                    graficoWeb.SerieData[indexSerie].Data[numr] = datito;
                    numr++;
                }
                indexSerie++;
            }
            #endregion

            return graficoWeb;
        }

        /// <summary>
        /// Devuelve la informacion de la tabla comparativa a exportar
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="listadoComparativo"></param>
        /// <param name="nameFile"></param>
        public void GenerarReporteTabla(string ruta, List<ComparativoExtranetConSP7> listadoComparativo, string nameFile)
        {
            //Descargo archivo segun requieran
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarExcelTabla(xlPackage, listadoComparativo);

                xlPackage.Save();

            }
        }

        /// <summary>
        /// Devuelve la estructura del reporte de la tabla comparativa
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="listadoComparativo"></param>
        private void GenerarExcelTabla(ExcelPackage xlPackage, List<ComparativoExtranetConSP7> listadoComparativo)
        {
            ExcelWorksheet ws = null;

            string nameWS = "vs";

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 11);


            int colIniTable = 4;
            int rowIniTabla = 5;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 10, 13, 13, 13, 17 })//columna A-I 
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            var colFechaHora = colIniTable;
            int colExtranet = colIniTable + 1;
            int colSp7 = colIniTable + 2;
            int colDiferencia = colIniTable + 3;
            int colDesviacion = colIniTable + 4;

            ws.Cells[rowIniTabla - 2, colIniTable].Value = "Comparativo Extranet VS Histórico SP7";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colDesviacion, "Arial Narrow", 14);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colDesviacion);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colDesviacion);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla - 2, colIniTable, rowIniTabla - 2, colDesviacion, "Centro");

            ws.Row(rowIniTabla).Height = 25;
            ws.Cells[rowIniTabla, colFechaHora].Value = "Hora";
            ws.Cells[rowIniTabla, colExtranet].Value = "Extranet";
            ws.Cells[rowIniTabla, colSp7].Value = "Sp7";
            ws.Cells[rowIniTabla, colDiferencia].Value = "Diferencia";
            ws.Cells[rowIniTabla, colDesviacion].Value = "Desviación (%)";

            var rangoCab = ws.Cells[rowIniTabla, colFechaHora, rowIniTabla, colDesviacion];
            rangoCab.SetAlignment();

            UtilExcel.CeldasExcelColorTexto(ws, rowIniTabla, colFechaHora, rowIniTabla, colDesviacion, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniTabla, colFechaHora, rowIniTabla, colDesviacion, "#0D43A5");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colFechaHora, rowIniTabla, colDesviacion, "Arial", 11);
            UtilExcel.BorderCeldas2(ws, rowIniTabla, colFechaHora, rowIniTabla, colDesviacion);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTabla, colFechaHora, rowIniTabla, colDesviacion);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;


            foreach (var item in listadoComparativo)
            {
                ws.Cells[rowData, colFechaHora].Value = item.Hora;
                ws.Cells[rowData, colExtranet].Value = item.ValorExtranet;
                ws.Cells[rowData, colExtranet].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colSp7].Value = item.ValorSP7;
                ws.Cells[rowData, colSp7].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colDiferencia].Value = item.Diferencia;
                ws.Cells[rowData, colDiferencia].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colDesviacion].Value = item.Desviacion;
                ws.Cells[rowData, colDesviacion].Style.Numberformat.Format = "#,##0.00";

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colFechaHora, rowData, colFechaHora, "Centro");
                UtilExcel.BorderCeldas2(ws, rowData, colFechaHora, rowData, colDesviacion);

                rowData++;
            }

            var rangotabla = ws.Cells[rowIniTabla + 1, colFechaHora, rowData, colDesviacion];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);

        }

        /// <summary>
        /// Genera data del reporte registrada por agentes
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="fecha"></param>
        /// <param name="strIdUrs"></param>
        /// <param name="nameFile"></param>
        /// <param name="lstUrsSinPtomedicion"></param>
        public void GenerarCSVReporteExtranet(string ruta, DateTime fecha, string strIdUrs, string nameFile, out string lstUrsSinPtomedicion)
        {
            try
            {
                lstUrsSinPtomedicion = "";

                //Descargo archivo segun requieran
                string rutaFile = ruta + nameFile;

                FileInfo newFile = new FileInfo(rutaFile);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(rutaFile);
                }

                #region Informacion necesaria
                List<RpfMedicion60DTO> listadoDataExtranet = ObtenerInformacionAGCExtranet(fecha, strIdUrs, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);
                if (!listadoDataExtranet.Any()) throw new ArgumentException("No se encontró información reportada por agentes para la fecha");

                List<int> lstEquicodis = listadoDataExtranet.GroupBy(x => x.Equicodi.Value).Select(n => n.Key).ToList();
                string lstStrEquicodis = String.Join(",", lstEquicodis);

                //Informacion de equipos
                List<EqEquipoDTO> equipos = ObtenerDatosEquipos(lstStrEquicodis);

                //Informacion de Ptos de Medicion
                List<ServicioRpfDTO> listadoPtosMedicion = ObtenerPtosMedicionCentralUnidadesURS(fecha);

                List<EveRsfdetalleDTO> lstUrs = ObtenerListadoURSPorFecha(fecha);
                #endregion

                int numFilasTotales = 86400;

                List<DatoCsv> lstPintar = ConvertirDataExtranetParaCSV(fecha, numFilasTotales, listadoDataExtranet, listadoPtosMedicion, lstUrs, equipos, out List<int> lstEquiposSinPtomedicion);

                //Obtengo las URS Sin ptomedicion para mostrar
                List<string> strLstUrsSinPtomed = new List<string>();
                List<int> lstEquiposSPM = lstEquiposSinPtomedicion.Distinct().ToList();
                foreach (var equicodi in lstEquiposSPM)
                {
                    EveRsfdetalleDTO urs = lstUrs.Find(x => x.Equicodi == equicodi);
                    string nomburs = (urs != null ? urs.Ursnomb.Trim() : "");
                    strLstUrsSinPtomed.Add(nomburs);
                }
                lstUrsSinPtomedicion = String.Join(", ", strLstUrsSinPtomed);

                //Pinto la informacion            
                using (var dbProviderWriter = new StreamWriter(rutaFile))
                {
                    int filasTotales = lstPintar.Max(x => x.Fila);


                    List<string> lineas = ObtenerLineasArchivoCSV(lstPintar);

                    foreach (var linea in lineas)
                    {
                        dbProviderWriter.WriteLine(linea);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Obtiene lineas que deben pintarse en los archivos .csv
        /// </summary>
        /// <param name="lstPintar"></param>
        /// <returns></returns>
        private List<string> ObtenerLineasArchivoCSV(List<DatoCsv> lstPintar)
        {
            List<string> lstSalida = new List<string>();

            var lista = lstPintar.GroupBy(x => new { x.Fila}).ToList();

            foreach (var item in lista)
            {
                string sLine = string.Empty;
                var lstOrdenadaCol = item.Distinct().OrderBy(x => x.Columna).ToList();
                foreach (var reg in lstOrdenadaCol)
                {
                    string val = reg != null ? reg.Valor : "X";
                    sLine += val + ",";
                    
                }
                lstSalida.Add(sLine);
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el archivo csv con las filas
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="fecha"></param>
        /// <param name="strIdsUrs"></param>
        /// <param name="nameFile"></param>
        public void GenerarCSVReporteKumpliy(string ruta, DateTime fecha, string strIdsUrs, string nameFile, out string lstUrsSinPtomedicion)
        {
            try
            {
                lstUrsSinPtomedicion = "";

                //Obtenemos la informacion a usar
                #region Informacion a usar
                List<Urs> ursSeleccionados = ObtenerInformacionUrsSeleccionados(fecha, strIdsUrs);

                //valido datos de las URS
                List<string> lstUrsSinUnidSleccionadas = new List<string>();

                foreach (var urs in ursSeleccionados)
                {
                    if (urs.DatoReporte == ConstantesDatosAGC.PorUnidad)
                    {
                        if (!urs.LstUnidSeleccionadas.Any())
                        {
                            lstUrsSinUnidSleccionadas.Add(urs.NombreUrs.Trim());
                        }
                    }
                }
                string lstUrsSinUnidadesSeleccionadas = "";
                lstUrsSinUnidadesSeleccionadas = String.Join(", ", lstUrsSinUnidSleccionadas);
                if (lstUrsSinUnidadesSeleccionadas != "")
                {
                    string msg = "";
                    if (lstUrsSinUnidSleccionadas.Count() == 1) msg = "La siguiente URS no tiene unidades seleccionadas en su configuración (Reporte Extranet): " + lstUrsSinUnidadesSeleccionadas;
                    if (lstUrsSinUnidSleccionadas.Count() > 1) msg = "Las siguientes URS no tienen unidades seleccionadas en su configuración (Reporte Extranet):" + lstUrsSinUnidadesSeleccionadas;

                    throw new ArgumentException(msg);
                }

                //Data a usar
                List<RpfMedicion60DTO> listadoDataExtranet = ObtenerInformacionAGCExtranet(fecha, strIdsUrs, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);
                List<CoMedicion60DTO> dataSP7 = ObtenerInformacionSP7(fecha, strIdsUrs, ConstantesAppServicio.ParametroDefecto, ConstantesDatosAGC.CodigoBasePoint.ToString());
                #endregion

                //Armamos el archivo a exportar
                string sLine = string.Empty;

                //Descargo archivo segun requieran
                string rutaFile = ruta + nameFile;

                FileInfo newFile = new FileInfo(rutaFile);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(rutaFile);
                }

                int numSegundosAnalizados = 86400;

                List<RegKumpliy> lstKumpliy = ObtenerDataInfoKumpliy(listadoDataExtranet, dataSP7, ursSeleccionados, numSegundosAnalizados);

                #region Tricolumnas sin ptomedicion
                //Obtengo los equicodis de las tricolumnas sin ptomedicion
                var lstGruposTriColumnas = lstKumpliy.GroupBy(x => new { x.OrdenColumna, x.Ptomedicion, x.Equicodi }).ToList();
                List<int> lstTricolumnasSinPtomedicion = new List<int>();
                foreach (var triColumna in lstGruposTriColumnas)
                {
                    int num = 0;

                    var objTriCol = triColumna.First();
                    if (objTriCol != null)
                    {
                        if (objTriCol.Ptomedicion == null)
                        {
                            lstTricolumnasSinPtomedicion.Add(objTriCol.Equicodi);
                        }
                    }

                }

                //obtengo nombres de urs de las que no tienen ptomedicion
                List<EveRsfdetalleDTO> lstUrs = ObtenerListadoURSPorFecha(fecha);
                List<string> strLstUrsSinPtomed = new List<string>();
                List<int> lstEquiposSPM = lstTricolumnasSinPtomedicion.Distinct().ToList();
                foreach (var equicodi in lstEquiposSPM)
                {
                    EveRsfdetalleDTO urs = lstUrs.Find(x => x.Equicodi == equicodi);
                    string nomburs = (urs != null ? urs.Ursnomb.Trim() : "");
                    strLstUrsSinPtomed.Add(nomburs);
                }
                lstUrsSinPtomedicion = String.Join(", ", strLstUrsSinPtomed);

                #endregion

                //Limpio listas para evitar usar memoria innecesariamente
                listadoDataExtranet = new List<RpfMedicion60DTO>();
                dataSP7 = new List<CoMedicion60DTO>();

                List<DatoCsv> lstPintar = ConvertirDataKumpliyParaCSV(lstKumpliy, numSegundosAnalizados);

                //Limpio listas para evitar usar memoria innecesariamente                
                lstKumpliy = new List<RegKumpliy>();


                using (var dbProviderWriter = new StreamWriter(rutaFile))
                {                    
                    List<string> lineas = ObtenerLineasArchivoCSV(lstPintar);
                    
                    foreach (var linea in lineas)
                    {
                        dbProviderWriter.WriteLine(linea);
                    }                                    
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            
        }

        

        /// <summary>
        /// Deveulve toda la informacion de las urs seleccionadas en el popup
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="strIdsUrs"></param>
        /// <returns></returns>
        private List<Urs> ObtenerInformacionUrsSeleccionados(DateTime fecha, string strIdsUrs)
        {
            List<Urs> lstSalida = new List<Urs>();

            //obtengo Informacion de todas las configuraciones urs para la fecha seleccionada
            List<CoConfiguracionDetDTO> dataTotalDetallesConfigUrs = ObtenerInformacionConfigUrs(fecha.ToString(ConstantesAppServicio.FormatoFecha));
            if (!dataTotalDetallesConfigUrs.Any()) throw new ArgumentException("No existe ninguna configuración URS para la fecha seleccionada");

            List<int> lstCourdecodisTotales = dataTotalDetallesConfigUrs.GroupBy(n => n.Courdecodi).Select(x => x.Key).ToList();
            string lstStrCourdecodis = String.Join(",", lstCourdecodisTotales);

            //Obtengo la ultima version de todas
            int ultCovercodi = dataTotalDetallesConfigUrs.First().Covercodi;

            List<CoConfiguracionDetDTO> dataDetallesUrsParaFecha = dataTotalDetallesConfigUrs.Where(x => x.Covercodi == ultCovercodi).ToList();

            //obtengo informacion de urs especiales
            List<CoUrsEspecialDTO> lstUrsEspecialesParaFecha = costoOpServicio.GetByCriteriaCoUrsEspecials(ultCovercodi);

            //Obtengo el listado de unidades seleccionadas
            List<CoConfiguracionGenDTO> lstUnidadesSeleccionadas = FactorySic.GetCoConfiguracionGenRepository().GetUnidadesSeleccionadas(lstStrCourdecodis);

            //Obtengo datos de todas las urs para la fecha
            List<EveRsfdetalleDTO> lstDataUrs = ObtenerListadoURSConEquiposPorFecha(fecha).ToList();

            //Informacion de Ptos de Medicion
            List<ServicioRpfDTO> listadoPtosMedicion = ObtenerPtosMedicionCentralUnidadesURS(fecha);

            //valido que todas tengan configuracion URS y vigentes
            List<CoConfiguracionUrsDTO> lstUrsVigencias = FactorySic.GetCoConfiguracionUrsRepository().GetPorVersion(ultCovercodi);
            List<string> lstUrsSinConfiguracion = new List<string>();
            List<int> lstGrupocodisUrsSeleccionadas = strIdsUrs.Split(',').Select(int.Parse).ToList();
            List<int> lstUrsConConfiguracionYHabilitacion = new List<int>();
            foreach (int grupoco in lstGrupocodisUrsSeleccionadas)
            {
                List<CoConfiguracionDetDTO> detallesUrs = dataDetallesUrsParaFecha.Where(x => x.Grupocodi == grupoco).ToList();                                

                //Tiene configuracion URS
                if (detallesUrs.Any())
                {
                    //verifico que esten vigentes
                    CoConfiguracionUrsDTO ursX = lstUrsVigencias.Find(x => x.Grupocodi == grupoco);
                    if(ursX!= null)
                    {
                        //si son nulos se asume que estan en vigencia
                        DateTime? iniVigencia = ursX.Conursfecinicio;
                        DateTime? finVigencia = ursX.Conursfecfin;

                        //si tienen datos, se verifica
                        if (iniVigencia != null)
                        {
                            if (finVigencia != null)
                            {
                                int result = DateTime.Compare(iniVigencia.Value, fecha);
                                if (result <= 0) //si la fecha consultada es mayor o igual al inicio de vigencia
                                {
                                    int result2 = DateTime.Compare(fecha, finVigencia.Value);
                                    if (result2 <= 0) //si la fecha consultada es menor o igual al fin de vigencia
                                    {
                                        //se toma la urs
                                        lstUrsConConfiguracionYHabilitacion.Add(detallesUrs.First().Grupocodi);
                                    }                                    
                                }
                            }
                            else
                            { 
                                //asumo que no tiene fin vigencia
                                int result = DateTime.Compare(iniVigencia.Value, fecha);
                                if (result <= 0) //si la fecha consultada es mayor o igual al inicio de vigencia
                                {
                                    //se toma la urs
                                    lstUrsConConfiguracionYHabilitacion.Add(detallesUrs.First().Grupocodi);
                                }
                            }
                        }
                        else
                        {
                            if (finVigencia != null)
                            {
                                //asumo que no tiene inicio vigencia
                                int result = DateTime.Compare(fecha, finVigencia.Value);
                                if (result <= 0) //si la fecha consultada es mayor o igual al inicio de vigencia
                                {
                                    //se toma la urs
                                    lstUrsConConfiguracionYHabilitacion.Add(detallesUrs.First().Grupocodi);
                                }
                            }
                            else
                            {
                                //se toma la urs
                                lstUrsConConfiguracionYHabilitacion.Add(detallesUrs.First().Grupocodi);
                            }
                        }                        
                    }
                    
                }
                else  //si no tiene configuracion URS
                {
                    lstUrsSinConfiguracion.Add(lstDataUrs.Find(x => x.Grupocodi == grupoco).Ursnomb.Trim());
                }                              
            }
            //muestro los q no tiene configuracion urs
            string strUrsSinConfiguracion = "";
            strUrsSinConfiguracion = String.Join(", ", lstUrsSinConfiguracion);
            if (strUrsSinConfiguracion != "")
            {                
                //throw new ArgumentException("No se encontró configuración URS para: " + strUrsSinConfiguracion); //Inhabilitado 
            }

            //filtro las que tienen configuracion urs y vigentes para la fecha
            string strUrsConConfiguracion = String.Join(",", lstUrsConConfiguracionYHabilitacion);

            //Obtenemos listado urs seleccionados
            List<Urs> lstUrsSeleccionadas = ObtenerDetallesUrsSeleccionadas(strUrsConConfiguracion, lstDataUrs, dataDetallesUrsParaFecha, lstUrsEspecialesParaFecha, lstUnidadesSeleccionadas, listadoPtosMedicion);

            lstSalida = lstUrsSeleccionadas;
            return lstSalida;
        }

        /// <summary>
        /// Devuelve la informacion de las Urs seleccionadas
        /// </summary>
        /// <param name="strIdsUrs"></param>
        /// <param name="lstDatayEquiposUrs"></param>
        /// <param name="dataDetallesUrsParaFecha"></param>
        /// <param name="lstUrsEspecialesParaFecha"></param>
        /// <param name="lstUnidadesSeleccionadas"></param>
        /// <param name="listadoPtosMedicion"></param>
        /// <returns></returns>
        private List<Urs> ObtenerDetallesUrsSeleccionadas(string strIdsUrs, List<EveRsfdetalleDTO> lstDatayEquiposUrs, List<CoConfiguracionDetDTO> dataDetallesUrsParaFecha, List<CoUrsEspecialDTO> lstUrsEspecialesParaFecha, List<CoConfiguracionGenDTO> lstUnidadesSeleccionadas, List<ServicioRpfDTO> listadoPtosMedicion)
        {
            List<Urs> lstSalida = new List<Urs>();
            List<EveRsfdetalleDTO> lstDataUrs = lstDatayEquiposUrs.Where(x => x.Grupotipo == ConstantesAppServicio.SI).ToList();
            List<int> lstGrupocodisUrsSeleccionadas = strIdsUrs != "" ? strIdsUrs.Split(',').Select(int.Parse).ToList() : new List<int>();

            foreach (var grupocodiUrs in lstGrupocodisUrsSeleccionadas)
            {
                #region Informacion a usar
                EveRsfdetalleDTO dataUrs = lstDataUrs.Find(x => x.Grupocodi == grupocodiUrs);
                List<CoConfiguracionDetDTO> detallesUrs = dataDetallesUrsParaFecha.Where(x => x.Grupocodi == grupocodiUrs).ToList();
                if(!detallesUrs.Any()) throw new ArgumentException("No se encontró configuración URS para " + lstDatayEquiposUrs.Find(x=>x.Grupocodi==grupocodiUrs).Ursnomb);
                List<CoUrsEspecialDTO> ursEspeciales = lstUrsEspecialesParaFecha.Where(x => x.Grupocodi == grupocodiUrs).ToList();
                List<CoConfiguracionGenDTO> unidadesSeleccionadas = lstUnidadesSeleccionadas.Where(x => x.Grupocodi == grupocodiUrs).ToList();
                ServicioRpfDTO datoPtoCentral = listadoPtosMedicion.Find(x => x.EQUICODI == dataUrs.Equicodi.Value);

                //datos operacion y reporte
                CoConfiguracionDetDTO datoOperacion = detallesUrs.Find(x => x.Courdetipo == "1");
                CoConfiguracionDetDTO datoReporte = detallesUrs.Find(x => x.Courdetipo == "2");

                //lista unidades de la urs
                List<EveRsfdetalleDTO> lstUnidadesTotales = lstDatayEquiposUrs.Where(x => x.Grupocodi == grupocodiUrs && x.Grupotipo != ConstantesAppServicio.SI).ToList();
                List<int> equicodisUnidadesTotales = lstUnidadesTotales.GroupBy(x => x.Equicodi.Value).Select(x => x.Key).ToList();
                List<int> equicodisUnidadesSeleccionadas = unidadesSeleccionadas.GroupBy(x => x.Equicodi.Value).Select(x => x.Key).ToList();
                List<int> equicodisUnidadesNoSeleccionadas = (List<int>)equicodisUnidadesTotales.Except(equicodisUnidadesSeleccionadas).ToList();
                #endregion

                #region Listado de unidades
                List<EquipoGen> listaUnidadesTotales = new List<EquipoGen>();
                List<EquipoGen> listaUnidadesSeleccionadas = new List<EquipoGen>();
                List<EquipoGen> listaUnidadesNoSeleccionadas= new List<EquipoGen>();

                foreach (var equicodi in equicodisUnidadesTotales)
                {
                    ServicioRpfDTO datoPtoUnidad = listadoPtosMedicion.Find(x => x.EQUICODI == equicodi);
                    EveRsfdetalleDTO u = lstDatayEquiposUrs.Find(x => x.Equicodi.Value == equicodi);

                    EquipoGen obj = new EquipoGen();
                    obj.Equicodi = equicodi;
                    obj.Ptomedicion = datoPtoUnidad != null ? datoPtoUnidad.PTOMEDICODI : -1;
                    obj.NombreUrs = u != null ? u.Ursnomb.Trim() : "";

                    listaUnidadesTotales.Add(obj);
                }

                foreach (var equicodi in equicodisUnidadesSeleccionadas)
                {
                    ServicioRpfDTO datoPtoUnidad = listadoPtosMedicion.Find(x => x.EQUICODI == equicodi);
                    EveRsfdetalleDTO u = lstDatayEquiposUrs.Find(x => x.Equicodi.Value == equicodi);

                    EquipoGen obj = new EquipoGen();
                    obj.Equicodi = equicodi;
                    obj.Ptomedicion = datoPtoUnidad != null ? datoPtoUnidad.PTOMEDICODI : -1;
                    obj.NombreUrs = u != null ? u.Ursnomb.Trim() : "";

                    listaUnidadesSeleccionadas.Add(obj);
                }

                foreach (var equicodi in equicodisUnidadesNoSeleccionadas)
                {
                    ServicioRpfDTO datoPtoUnidad = listadoPtosMedicion.Find(x => x.EQUICODI == equicodi);
                    EveRsfdetalleDTO u = lstDatayEquiposUrs.Find(x => x.Equicodi.Value == equicodi);

                    EquipoGen obj = new EquipoGen();
                    obj.Equicodi = equicodi;
                    obj.Ptomedicion = datoPtoUnidad != null ? datoPtoUnidad.PTOMEDICODI : -1;
                    obj.NombreUrs = u != null ? u.Ursnomb.Trim() : "";

                    listaUnidadesNoSeleccionadas.Add(obj);
                }
                #endregion

                int? intNulo = null;
                Urs miUrs = new Urs();

                miUrs.Grupocodi = grupocodiUrs;
                miUrs.Equicodi = dataUrs.Equicodi.Value;
                miUrs.NombreUrs = dataUrs.Ursnomb.Trim();
                miUrs.NombreCentral = dataUrs.Gruponomb.Trim();
                miUrs.PtomedicionCentral = datoPtoCentral != null ? datoPtoCentral.PTOMEDICODI : intNulo; //luego se cambia para que pinte como _
                miUrs.EsEspecial = ursEspeciales.Any() ? true : false;
                miUrs.DatoOperacion = datoOperacion.Courdeoperacion; //'1':UNIDAD, '2':CENTRAL
                miUrs.DatoReporte = datoReporte.Courdereporte; //'1':UNIDAD, '2':CENTRAL
                miUrs.LstUnidTotales = listaUnidadesTotales.OrderBy(x=>x.Ptomedicion).ToList();
                miUrs.LstUnidSeleccionadas = listaUnidadesSeleccionadas.OrderBy(x => x.Ptomedicion).ToList();
                miUrs.LstUnidNoSeleccionadas = listaUnidadesNoSeleccionadas.OrderBy(x => x.Ptomedicion).ToList();
                miUrs.TodasUnidadesSeleccionadas = equicodisUnidadesNoSeleccionadas.Any() ? false : true;

                lstSalida.Add(miUrs);
            }

            return lstSalida;
        }

        /// <summary>
        /// Obtiene los ptos de medicion para las urs (central y unidades)
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<ServicioRpfDTO> ObtenerPtosMedicionCentralUnidadesURS(DateTime fechaConsulta)
        {
            List<ServicioRpfDTO> lstSalida = new List<ServicioRpfDTO>();

            List<EveRsfdetalleDTO> lstDataUrs = ObtenerListadoURSConEquiposPorFecha(fechaConsulta).ToList();
            List<ServicioRpfDTO> listadoPtosMedicionOriginal = rpfServicio.ObtenerUnidadesCarga(fechaConsulta);
            int? valNulo = null;

            foreach (var reg in lstDataUrs)
            {
                int? PtoMedUnidad = null;

                //Verifico si existe el ptomedicion para la urs (central o unidad)
                ServicioRpfDTO dato = listadoPtosMedicionOriginal.Find(x => x.EQUICODI == reg.Equicodi);

                //si la urs (central/unidad) no existe en el listado original
                if(dato == null)
                {
                    #region Completo con el ptomedicodi de su unica unidad
                    //verifico que sea una central
                    if (reg.Grupotipo == ConstantesAppServicio.SI)
                    {
                        //verifico la cantidad de unidades de la central
                        List<EveRsfdetalleDTO> lstMismoGrupo = lstDataUrs.Where(x => x.Grupocodi == reg.Grupocodi).ToList();

                        //si tiene solo una unidad (central + 1 unidad = 2 registros)
                        if (lstMismoGrupo.Count() == 2)
                        {
                            EveRsfdetalleDTO objUnidad = lstMismoGrupo.Where(x => x.Equicodi != reg.Equicodi).First();

                            if(objUnidad!= null)
                            {
                                //Obtengo informacion (ptomedicion) de esa unidad, si existiese
                                ServicioRpfDTO datoUnidad = listadoPtosMedicionOriginal.Find(x => x.EQUICODI == objUnidad.Equicodi);

                                PtoMedUnidad = datoUnidad != null ? datoUnidad.PTOMEDICODI : valNulo;
                            }
                            
                        }
                    }
                    #endregion
                }
                else
                {
                    PtoMedUnidad = dato.PTOMEDICODI;
                }

                if(PtoMedUnidad != null)
                {
                    ServicioRpfDTO obj = new ServicioRpfDTO();
                    obj.EQUICODI = reg.Equicodi.Value;
                    obj.PTOMEDICODI = PtoMedUnidad.Value;

                    lstSalida.Add(obj);
                }                
            }


            return lstSalida;
        }
        /// <summary>
        /// Devuelve los detalles de las URS configuradas (todas las versiones) para la fecha seleccionada
        /// </summary>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        private List<CoConfiguracionDetDTO> ObtenerInformacionConfigUrs(string strFecha)
        {
            List<CoConfiguracionDetDTO> lstSalida = FactorySic.GetCoConfiguracionDetRepository().ObtenerConfiguracionDetalle(strFecha);

            return lstSalida;
        }

        /// <summary>
        /// Devuelve informacion de equipos
        /// </summary>
        /// <param name="lstStrEquicodis"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerDatosEquipos(string lstStrEquicodis)
        {
            return FactorySic.GetEqEquipoRepository().ListByIdEquipo(lstStrEquicodis);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listadoDataExtranet"></param>
        /// <param name="dataSP7"></param>
        /// <param name="ursSeleccionados"></param>
        /// <param name="numSegundosAnalizados"></param>
        /// <returns></returns>
        private List<RegKumpliy> ObtenerDataInfoKumpliy(List<RpfMedicion60DTO> listadoDataExtranet, List<CoMedicion60DTO> dataSP7, List<Urs> ursSeleccionados, int numSegundosAnalizados)
        {
            try
            {
                List<RegKumpliy> lstSalida = new List<RegKumpliy>();

                //lstUrs esta ordenado por nombre de central
                List<Urs> lstUrsOrdenada = ursSeleccionados.OrderBy(x => x.NombreCentral).ToList();
                List<RpfMedicion60DTO> listadoDataSetpointExtranet = listadoDataExtranet.Where(x => x.Cotidacodi == ConstantesDatosAGC.CodigoSetPoint).ToList();
                List<RpfMedicion60DTO> listadoDataEstadoExtranet = listadoDataExtranet.Where(x => x.Cotidacodi == ConstantesDatosAGC.CodigoEstado).ToList();

                int ordenCol = 1;
                foreach (var urs in lstUrsOrdenada)
                {
                    List<EquipoGen> unidadesTotales = urs.LstUnidTotales;
                    List<int> equicodisUnidadesTotales = unidadesTotales.GroupBy(x => x.Equicodi).Select(x => x.Key).ToList();
                    List<int> equicodisUnidadesYCentralTotales = new List<int>();
                    equicodisUnidadesYCentralTotales.Add(urs.Equicodi);
                    equicodisUnidadesYCentralTotales.AddRange(equicodisUnidadesTotales);

                    List<int> equicodisUnidadesSeleccionadas = urs.LstUnidSeleccionadas.GroupBy(x => x.Equicodi).Select(x => x.Key).ToList();

                    List<RpfMedicion60DTO> lstDataSetpointExtranetPorUrs = listadoDataSetpointExtranet.Where(d => equicodisUnidadesYCentralTotales.Contains(d.Equicodi.Value)).ToList();
                    List<RpfMedicion60DTO> lstDataEstadoExtranetPorUrs = listadoDataEstadoExtranet.Where(d => equicodisUnidadesYCentralTotales.Contains(d.Equicodi.Value)).ToList();
                    List<CoMedicion60DTO> lstDataSP7PorUrs = dataSP7.Where(d => equicodisUnidadesYCentralTotales.Contains(d.Equicodi.Value)).ToList();

                    //Evaluar si la URS es ESPECIAL o NO
                    if (urs.EsEspecial)
                    {
                        //Escogió POR UNIDAD en sección EXTRANET
                        if (urs.DatoReporte == ConstantesDatosAGC.PorUnidad)
                        {                            
                            ObtenerDataKumpliy(ConstantesDatosAGC.CasoUrsEspecial_RepUnidad, urs.PtomedicionCentral, urs.Equicodi, numSegundosAnalizados, lstSalida, ordenCol, urs,
                                                    equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrs, lstDataEstadoExtranetPorUrs, lstDataSP7PorUrs, new EquipoGen());

                            ordenCol++;

                        }
                        else  //consultar qué pasa (sos)
                        {

                        }
                    }
                    else //No es especal
                    {                        
                        //Es por Central o Unidad (todas unidades seleccionadas)
                        if ((urs.DatoReporte == ConstantesDatosAGC.PorCentral) || (urs.DatoReporte == ConstantesDatosAGC.PorUnidad && urs.TodasUnidadesSeleccionadas))
                        {
                            if (urs.DatoOperacion == ConstantesDatosAGC.PorUnidad)
                            {
                                if (urs.DatoReporte == ConstantesDatosAGC.PorCentral) // TODAS SELECCIONADAS: Operacion:Unidad / Reporte:Central
                                {                                    

                                    ObtenerDataKumpliy(ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpUnidadRepCentral, urs.PtomedicionCentral, urs.Equicodi, numSegundosAnalizados, lstSalida, ordenCol, urs,
                                                    equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrs, lstDataEstadoExtranetPorUrs, lstDataSP7PorUrs, new EquipoGen());

                                    ordenCol++;
                                }
                                else
                                {
                                    if (urs.DatoReporte == ConstantesDatosAGC.PorUnidad) // TODAS SELECCIONADAS: Operacion:Unidad / Reporte:Unidad
                                    {
                                        List<EquipoGen> lstUnidadesTotales = urs.LstUnidTotales; //las unidades ya estan ordenadas por ptomedicion

                                        foreach (var unidad in lstUnidadesTotales)
                                        {                                            

                                            ObtenerDataKumpliy(ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpUnidadRepUnidad, unidad.Ptomedicion, unidad.Equicodi, numSegundosAnalizados, lstSalida, ordenCol, urs,
                                                    equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrs, lstDataEstadoExtranetPorUrs, lstDataSP7PorUrs, unidad);

                                            ordenCol++;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (urs.DatoOperacion == ConstantesDatosAGC.PorCentral)
                                {
                                    if (urs.DatoReporte == ConstantesDatosAGC.PorUnidad) // TODAS SELECCIONADAS: Operacion:Central / Reporte:Unidad
                                    {
                                        List<EquipoGen> lstUnidadesTotales = urs.LstUnidTotales; //las unidades ya estan ordenadas por ptomedicion

                                        foreach (var unidad in lstUnidadesTotales)
                                        {                                            

                                            ObtenerDataKumpliy(ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpCentralRepUnidad, unidad.Ptomedicion, unidad.Equicodi, numSegundosAnalizados, lstSalida, ordenCol, urs,
                                                    equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrs, lstDataEstadoExtranetPorUrs, lstDataSP7PorUrs, unidad);

                                            ordenCol++;
                                        }
                                    }
                                    else
                                    {
                                        if (urs.DatoReporte == ConstantesDatosAGC.PorCentral) // TODAS SELECCIONADAS: Operacion:Central / Reporte:Central
                                        {                                            

                                            ObtenerDataKumpliy(ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpCentralRepCentral, urs.PtomedicionCentral, urs.Equicodi, numSegundosAnalizados, lstSalida, ordenCol, urs,
                                                    equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrs, lstDataEstadoExtranetPorUrs, lstDataSP7PorUrs, new EquipoGen());

                                            ordenCol++;
                                        }
                                    }
                                }
                            }
                        }
                        else //Caso especial 1 (solo algunas unidades han sido seleccionadas)
                        {
                            //Obtengo el orden de las cabeceras (entre ptomedicodis[la primera en nombre de las seleccionads] y [las no selecionadas])
                            List<EquipoGen> lstTemporal = new List<EquipoGen>();
                            List<EquipoGen> lstNuevaOrdenada = new List<EquipoGen>();

                            //ordeno por nombre las seleccionadas
                            EquipoGen unidadRepresentativa = urs.LstUnidSeleccionadas.OrderBy(x => x.NombreUrs).First();
                            unidadRepresentativa.Bandera = true; //Representa a todo el grupo de selecconados

                            lstTemporal.Add(unidadRepresentativa);
                            lstTemporal.AddRange(urs.LstUnidNoSeleccionadas);

                            //ordeno por ptomedicion el listado final
                            lstNuevaOrdenada = lstTemporal.OrderBy(x => x.Ptomedicion).ToList();

                            foreach (var unidad in lstNuevaOrdenada)
                            {
                                if (unidad.Bandera == true) //Representa a las SELECCIONADAS
                                {
                                    if (urs.DatoOperacion == ConstantesDatosAGC.PorUnidad)
                                    {
                                        if (urs.DatoReporte == ConstantesDatosAGC.PorUnidad) // CE_1 (SELECCIONADAS) : Operacion:Unidad / Reporte:Unidad
                                        {                                            

                                            ObtenerDataKumpliy(ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_Seleccionadas, unidad.Ptomedicion, unidad.Equicodi, numSegundosAnalizados, lstSalida, ordenCol, urs,
                                                    equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrs, lstDataEstadoExtranetPorUrs, lstDataSP7PorUrs, new EquipoGen());

                                            ordenCol++;
                                        }
                                    }
                                    else
                                    {
                                        if (urs.DatoOperacion == ConstantesDatosAGC.PorCentral)
                                        {
                                            if (urs.DatoReporte == ConstantesDatosAGC.PorUnidad) // CE_1 (SELECCIONADAS) : Operacion:Central / Reporte:Unidad
                                            {                                                

                                                ObtenerDataKumpliy(ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_Seleccionadas, unidad.Ptomedicion, unidad.Equicodi, numSegundosAnalizados, lstSalida, ordenCol, urs,
                                                    equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrs, lstDataEstadoExtranetPorUrs, lstDataSP7PorUrs, new EquipoGen());

                                                ordenCol++;
                                            }
                                        }
                                    }
                                }
                                else //representa a los NO SELECCIONADAS
                                {
                                    if (urs.DatoOperacion == ConstantesDatosAGC.PorUnidad)
                                    {
                                        if (urs.DatoReporte == ConstantesDatosAGC.PorUnidad) // CE_1 (NO SELECCIONADAS): Operacion:Unidad / Reporte:Unidad
                                        {                                            

                                            ObtenerDataKumpliy(ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_NoSeleccionadas, unidad.Ptomedicion, unidad.Equicodi, numSegundosAnalizados, lstSalida, ordenCol, urs,
                                                   equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrs, lstDataEstadoExtranetPorUrs, lstDataSP7PorUrs, unidad);

                                            ordenCol++;
                                        }
                                    }
                                    else
                                    {
                                        if (urs.DatoOperacion == ConstantesDatosAGC.PorCentral)
                                        {
                                            if (urs.DatoReporte == ConstantesDatosAGC.PorUnidad) //  CE_1 (NO SELECCIONADAS): Operacion:Central / Reporte:Unidad
                                            {
                                                
                                                ObtenerDataKumpliy(ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_NoSeleccionadas, unidad.Ptomedicion, unidad.Equicodi, numSegundosAnalizados, lstSalida, ordenCol, urs, 
                                                    equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrs, lstDataEstadoExtranetPorUrs, lstDataSP7PorUrs, unidad); 


                                                ordenCol++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return lstSalida;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Devuelve la informacion de los 86400 registros para una tricolumna 
        /// </summary>
        /// <param name="casoTipo"></param>
        /// <param name="ptomedicion"></param>
        /// <param name="numSegundosAnalizados"></param>
        /// <param name="lstSalida"></param>
        /// <param name="ordenColumna"></param>
        /// <param name="urs"></param>
        /// <param name="equicodisUnidadesTotales"></param>
        /// <param name="equicodisUnidadesSeleccionadas"></param>
        /// <param name="lstDataSetpointExtranetPorUrs"></param>
        /// <param name="lstDataEstadoExtranetPorUrs"></param>
        /// <param name="lstDataSP7PorUrs"></param>
        /// <param name="unidad"></param>
        public void ObtenerDataKumpliy(int casoTipo, int? ptomedicion, int equicodi, int numSegundosAnalizados, List<RegKumpliy> lstSalida, int ordenColumna, Urs urs, List<int> equicodisUnidadesTotales, List<int> equicodisUnidadesSeleccionadas, List<RpfMedicion60DTO> lstDataSetpointExtranetPorUrs, List<RpfMedicion60DTO> lstDataEstadoExtranetPorUrs, List<CoMedicion60DTO> lstDataSP7PorUrs, EquipoGen unidad)
        {
            int minutoAntiguo = -1;
            List<RpfMedicion60DTO> lstDataSetpointExtranetPorUrsResumido = new List<RpfMedicion60DTO>();
            List<CoMedicion60DTO> lstDataSP7PorUrsResumido = new List<CoMedicion60DTO>();
            List<RpfMedicion60DTO> lstDataEstadoExtranetPorUrsResumido = new List<RpfMedicion60DTO>();

            int numSeg = numSegundosAnalizados;
            for (int seg = 0; seg < numSeg; seg++)
            {
                RegKumpliy objKumpliy = new RegKumpliy();

                objKumpliy.OrdenColumna = ordenColumna;
                objKumpliy.Ptomedicion = ptomedicion;
                objKumpliy.Equicodi = equicodi;
                objKumpliy.NumSegundo = seg;
                
                TimeSpan t = TimeSpan.FromSeconds(seg);
                int horaY = t.Hours;
                int minutoY = t.Minutes;

                if(minutoAntiguo != minutoY)
                {
                    //setpoint
                    lstDataSetpointExtranetPorUrsResumido = lstDataSetpointExtranetPorUrs.Where(x => x.Rpfmedfecha.Value.Hour == horaY && x.Rpfmedfecha.Value.Minute == minutoY).ToList();
                    lstDataSP7PorUrsResumido = lstDataSP7PorUrs.Where(x => x.Comedihora == horaY && x.Comediminuto == minutoY).ToList();

                    //estado
                    lstDataEstadoExtranetPorUrsResumido = lstDataEstadoExtranetPorUrs.Where(x => x.Rpfmedfecha.Value.Hour == horaY && x.Rpfmedfecha.Value.Minute == minutoY).ToList();

                }

                objKumpliy.Setpoint = ObtenerValorSetpointKumpliy(casoTipo, urs, seg, equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrsResumido, lstDataSP7PorUrsResumido, unidad.Equicodi);
                objKumpliy.Estado = ObtenerValorEstadoKumpliy(casoTipo, urs, seg, equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataEstadoExtranetPorUrsResumido, unidad.Equicodi);
                objKumpliy.Basepoint = ObtenerValorBasepointKumpliy(casoTipo, urs, seg, equicodisUnidadesTotales, equicodisUnidadesSeleccionadas, lstDataSetpointExtranetPorUrsResumido, lstDataEstadoExtranetPorUrsResumido, lstDataSP7PorUrsResumido, unidad.Equicodi, objKumpliy.Setpoint);

                lstSalida.Add(objKumpliy);

                minutoAntiguo = minutoY;
            }
            
        }

        /// <summary>
        /// Devuelve el valor de Bsepoint para el caso especial 2
        /// </summary>
        /// <param name="segundo"></param>
        /// <param name="valSetpoint"></param>
        /// <param name="lstaExtranetSetpointEnMinuto"></param>
        /// <param name="lstaExtranetEstadoEnMinuto"></param>
        /// <param name="lstaSp7tEnMinuto"></param>
        /// <returns></returns>
        public decimal? ObtenerBasepointCasoEspecial2(int segundo, decimal? valSetpoint, List<RpfMedicion60DTO>  lstaExtranetSetpointEnMinuto, List<RpfMedicion60DTO> lstaExtranetEstadoEnMinuto, List<CoMedicion60DTO> lstaSp7tEnMinuto)
        {
            decimal? salida = null;
            List<DatoCE2> lstDatosExtranet = new List<DatoCE2>();
            List<DatoCE2> lstDatosSP7 = new List<DatoCE2>();

            if (lstaExtranetSetpointEnMinuto.Count() <= 2 && lstaExtranetEstadoEnMinuto.Count() <= 2) //a lo mucho debe tener 2 generadores
            {
                #region listado de data
                //Setpoint
                foreach (var item in lstaExtranetSetpointEnMinuto)
                {
                    DatoCE2 obj = new DatoCE2();
                    obj.DatoSetpoint = (decimal?)Convert.ToDecimal(item.GetType().GetProperty("H" + (segundo + 1)).GetValue(item, null));
                    obj.DatoEstado = null;
                    obj.DatoBasepoint = null;
                    obj.Equicodi = item.Equicodi.Value;
                    obj.Tipo = "S";
                    lstDatosExtranet.Add(obj);
                }

                //Estado
                foreach (var item in lstaExtranetEstadoEnMinuto)
                {
                    DatoCE2 obj = new DatoCE2();
                    obj.DatoSetpoint = null;
                    obj.DatoEstado = (decimal?)Convert.ToDecimal(item.GetType().GetProperty("H" + (segundo + 1)).GetValue(item, null));
                    obj.DatoBasepoint = null;
                    obj.Equicodi = item.Equicodi.Value;
                    obj.Tipo = "E";
                    lstDatosExtranet.Add(obj);
                }

                //Basepoint
                foreach (var item in lstaSp7tEnMinuto)
                {
                    DatoCE2 obj = new DatoCE2();
                    obj.DatoSetpoint = null;
                    obj.DatoEstado = null;
                    obj.DatoBasepoint = (decimal?)Convert.ToDecimal(item.GetType().GetProperty("H" + (segundo + 1)).GetValue(item, null));
                    obj.Equicodi = item.Equicodi.Value;
                    obj.Tipo = "B";
                    lstDatosSP7.Add(obj);
                }
                #endregion

                #region Valor segun condicion
                DatoCE2 datoUnidadEstado0 = lstDatosExtranet.Find(x => x.DatoEstado == 0);
                DatoCE2 datoUnidadEstado1 = lstDatosExtranet.Find(x => x.DatoEstado == 1);


                //hay uno con estado 1 y otro con estado 0, valor = sepoint (de la unidad con estado 0 ) + basepoint (de la unidad con estado 1)
                if (datoUnidadEstado0 != null && datoUnidadEstado1 != null)
                {
                    int equicodiEstado0 = datoUnidadEstado0.Equicodi;
                    int equicodiEstado1 = datoUnidadEstado1.Equicodi;

                    var d = lstDatosExtranet.Find(x => x.Equicodi == equicodiEstado0 && x.Tipo == "S");
                    decimal? datoSetpoint = d != null ? d.DatoSetpoint : null;

                    var d2 = lstDatosSP7.Find(x => x.Equicodi == equicodiEstado1 );
                    decimal? datoBasepoint = d2 != null ? d2.DatoBasepoint : null;

                    salida = datoSetpoint + datoBasepoint;
                }

                List<DatoCE2> lstDatosExtranetEstados = lstDatosExtranet.Where(x => x.DatoEstado != null).ToList();

                //si ambos son ceros, valor = setpoint
                if (lstDatosExtranetEstados.Where(x => x.DatoEstado == 0).ToList().Count() > 1)
                {
                    salida = valSetpoint;
                }

                //si ambos son 1, valor = suma de sus basepoint
                if (lstDatosExtranetEstados.Where(x => x.DatoEstado == 1).ToList().Count() > 1)
                {
                    List<decimal?> lstBsepointUnidades = new List<decimal?>();
                    
                    foreach (var item in lstDatosExtranetEstados)
                    {
                        var datoBasepointPorUnidad = lstDatosSP7.Find(x => x.Equicodi == item.Equicodi);

                        if (datoBasepointPorUnidad != null)
                            lstBsepointUnidades.Add(datoBasepointPorUnidad.DatoBasepoint);                        
                    }

                    salida = lstBsepointUnidades.Sum(x => x);
                }
                #endregion
            }

            return salida;
        }
        

        /// <summary>
        /// Devuelve valor basepoint para Kumpliy
        /// </summary>
        /// <param name="caso"></param>
        /// <param name="urs"></param>
        /// <param name="segundo"></param>
        /// <param name="equicodisUnidadesTotales"></param>
        /// <param name="equicodisUnidadesSeleccionadas"></param>
        /// <param name="lstDataSetpointExtranetPorUrs"></param>
        /// <param name="lstDataEstadoExtranetPorUrs"></param>
        /// <param name="lstDataSP7PorUrs"></param>
        /// <param name="equicodiEquipo"></param>
        /// <param name="valSetpoint"></param>
        /// <returns></returns>
        private decimal? ObtenerValorBasepointKumpliy(int caso, Urs urs, int segundo, List<int> equicodisUnidadesTotales, List<int> equicodisUnidadesSeleccionadas, List<RpfMedicion60DTO> lstDataSetpointExtranetPorUrs, List<RpfMedicion60DTO> lstDataEstadoExtranetPorUrs, List<CoMedicion60DTO> lstDataSP7PorUrs, int? equicodiEquipo, decimal? valSetpoint)
        {
            decimal? salida = null;

            TimeSpan t = TimeSpan.FromSeconds(segundo);            
            int segundoY = t.Seconds;

            //Escojo el caso
            if (caso == ConstantesDatosAGC.CasoUrsEspecial_RepUnidad)
            {
                decimal? valBP = null;

                List<RpfMedicion60DTO> lstaExtranetSetpointEnMinuto = lstDataSetpointExtranetPorUrs.Where(x => equicodisUnidadesTotales.Contains(x.Equicodi.Value)).ToList();

                List<RpfMedicion60DTO> lstaExtranetEstadoEnMinuto = lstDataEstadoExtranetPorUrs.Where(x => equicodisUnidadesTotales.Contains(x.Equicodi.Value)).ToList();

                List<CoMedicion60DTO> lstaSp7tEnMinuto = lstDataSP7PorUrs.Where(x => equicodisUnidadesTotales.Contains(x.Equicodi.Value)).ToList();

                valBP = ObtenerBasepointCasoEspecial2(segundoY, valSetpoint, lstaExtranetSetpointEnMinuto, lstaExtranetEstadoEnMinuto, lstaSp7tEnMinuto);
                salida = valBP;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpUnidadRepCentral)
            {
                decimal? valBP = null;

                List<CoMedicion60DTO> lstaSp7tEnMinuto = lstDataSP7PorUrs.Where(x => equicodisUnidadesTotales.Contains(x.Equicodi.Value)).ToList();

                valBP = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7tEnMinuto);
                salida = valBP;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpUnidadRepUnidad)
            {
                decimal? valBP = null;

                List<CoMedicion60DTO> lstaSp7tEnMinuto = lstDataSP7PorUrs.Where(x => x.Equicodi == equicodiEquipo).ToList();

                valBP = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7tEnMinuto);
                salida = valBP;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpCentralRepCentral)
            {
                decimal? valBP = null;

                List<CoMedicion60DTO> lstaSp7tEnMinuto = lstDataSP7PorUrs.Where(x => x.Equicodi == urs.Equicodi).ToList();

                valBP = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7tEnMinuto);
                salida = valBP;
            }


            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpCentralRepUnidad)
            {
                decimal? valBP = null;

                List<CoMedicion60DTO> lstaSp7tEnMinuto = lstDataSP7PorUrs.Where(x => x.Equicodi == urs.Equicodi).ToList();

                valBP = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7tEnMinuto);
                salida = valBP != null ? (valBP / equicodisUnidadesTotales.Count()) : valBP;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_Seleccionadas)
            {
                decimal? valBP = null;

                List<CoMedicion60DTO> lstaSp7tEnMinuto = lstDataSP7PorUrs.Where(x => equicodisUnidadesSeleccionadas.Contains(x.Equicodi.Value)).ToList();

                valBP = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7tEnMinuto);
                salida = valBP;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_Seleccionadas) //  Basepoint sp7 respecto a la central 
            {
                decimal? valBP = null;

                List<CoMedicion60DTO> lstaSp7EnMinuto = lstDataSP7PorUrs.Where(x => x.Equicodi == urs.Equicodi).ToList();

                decimal? basepoint = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7EnMinuto);

                int numUnidadesTotales = equicodisUnidadesTotales.Count();
                int numUnidadesSeleccionadas = equicodisUnidadesSeleccionadas.Count();
                valBP = (basepoint != null && numUnidadesTotales != 0) ? (basepoint / numUnidadesTotales) * numUnidadesSeleccionadas : null;

                salida = valBP;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_NoSeleccionadas)
            {
                decimal? valBP = null;

                List<CoMedicion60DTO> lstaSp7EnMinuto = lstDataSP7PorUrs.Where(x => x.Equicodi == urs.Equicodi).ToList(); 

                decimal? basepoint = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7EnMinuto);

                int numUnidadesTotales = equicodisUnidadesTotales.Count();
                valBP = (basepoint != null && numUnidadesTotales != 0) ? (basepoint / numUnidadesTotales) : null;

                salida = valBP;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_NoSeleccionadas)
            {
                decimal? valBP = null;

                List<CoMedicion60DTO> lstaSp7EnMinuto = lstDataSP7PorUrs.Where(x => x.Equicodi == equicodiEquipo).ToList();

                decimal? basepoint = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7EnMinuto);

                valBP = basepoint;

                salida = valBP;
            }

            //Trunco a máximo 5 decimales
            if (salida != null)
                salida = truncarNumeroAEneDecimales(salida.Value, 5);

            return salida;
        }

        
        /// <summary>
        /// Devuelve valor Estado para Kumpliy
        /// </summary>
        /// <param name="caso"></param>
        /// <param name="urs"></param>
        /// <param name="segundo"></param>
        /// <param name="equicodisUnidadesTotales"></param>
        /// <param name="equicodisUnidadesSeleccionadas"></param>
        /// <param name="lstDataEstadoExtranetPorUrs"></param>
        /// <param name="equicodiEquipo"></param>
        /// <returns></returns>
        private decimal? ObtenerValorEstadoKumpliy(int caso, Urs urs, int segundo, List<int> equicodisUnidadesTotales, List<int> equicodisUnidadesSeleccionadas, List<RpfMedicion60DTO> lstDataEstadoExtranetPorUrs, int? equicodiEquipo)
        {
            decimal? salida = null;

            TimeSpan t = TimeSpan.FromSeconds(segundo);
            //int horaY = t.Hours;
            //int minutoY = t.Minutes;
            int segundoY = t.Seconds;

            //Escojo el caso
            if (caso == ConstantesDatosAGC.CasoUrsEspecial_RepUnidad)
            {
                decimal? sumaEstado = null;

                List<RpfMedicion60DTO> lstaExtranetEnMinuto = lstDataEstadoExtranetPorUrs.Where(x => equicodisUnidadesTotales.Contains(x.Equicodi.Value)).ToList();

                sumaEstado = SumarValoresRpfDelSegundo(segundoY, lstaExtranetEnMinuto);
                salida = sumaEstado != null ? (sumaEstado != 0 ? 1 : 0) : sumaEstado;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpUnidadRepCentral || caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpCentralRepCentral)
            {
                decimal? valEstado = null;

                List<RpfMedicion60DTO> lstaExtranetEnMinuto = lstDataEstadoExtranetPorUrs.Where(x => x.Equicodi == urs.Equicodi).ToList();

                valEstado = SumarValoresRpfDelSegundo(segundoY, lstaExtranetEnMinuto);
                salida = valEstado;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpUnidadRepUnidad || caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpCentralRepUnidad)
            {
                decimal? valEstado = null;

                List<RpfMedicion60DTO> lstaExtranetEnMinuto = lstDataEstadoExtranetPorUrs.Where(x => x.Equicodi == equicodiEquipo).ToList();

                valEstado = SumarValoresRpfDelSegundo(segundoY, lstaExtranetEnMinuto);
                salida = valEstado;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_Seleccionadas || caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_Seleccionadas
                || caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_NoSeleccionadas)
            {
                decimal? sumaEstado = null;

                List<RpfMedicion60DTO> lstaExtranetEnMinuto = lstDataEstadoExtranetPorUrs.Where(x => equicodisUnidadesSeleccionadas.Contains(x.Equicodi.Value)).ToList();

                sumaEstado = SumarValoresRpfDelSegundo(segundoY, lstaExtranetEnMinuto);
                salida = sumaEstado != null ? (sumaEstado != 0 ? 1 : 0) : sumaEstado;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_NoSeleccionadas)
            {
                //siempre será cero
                salida = 0;
            }

            return salida;
        }
        

        /// <summary>
        /// Devuelve valor setpoint para kumpliy
        /// </summary>
        /// <param name="caso"></param>
        /// <param name="urs"></param>
        /// <param name="segundo"></param>
        /// <param name="equicodisUnidadesTotales"></param>
        /// <param name="equicodisUnidadesSeleccionadas"></param>
        /// <param name="listadoDataSetpointExtranet"></param>
        /// <param name="dataSP7"></param>
        /// <param name="equicodiEquipo"></param>
        /// <returns></returns>
        private decimal? ObtenerValorSetpointKumpliy(int caso, Urs urs, int segundo, List<int> equicodisUnidadesTotales, List<int> equicodisUnidadesSeleccionadas, List<RpfMedicion60DTO> listadoDataSetpointExtranet, List<CoMedicion60DTO> dataSP7, int? equicodiEquipo)
        {
            decimal? salida = null;

            TimeSpan t = TimeSpan.FromSeconds(segundo);            
            int segundoY = t.Seconds;

            //Escojo el caso
            if (caso == ConstantesDatosAGC.CasoUrsEspecial_RepUnidad)
            {
                //CASO ESPECIAL 2 (solo habra 1 ptomedicodi), no es necesario ordenar las unidades
                decimal? sumaSetpoint = null;

                List<RpfMedicion60DTO> lstaExtranetEnMinuto = listadoDataSetpointExtranet.Where(x => equicodisUnidadesTotales.Contains(x.Equicodi.Value)).ToList();

                sumaSetpoint = SumarValoresRpfDelSegundo(segundoY, lstaExtranetEnMinuto);
                salida = sumaSetpoint;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpUnidadRepCentral || caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpCentralRepCentral)
            {
                decimal? valSetpoint = null;

                List<RpfMedicion60DTO> lstaExtranetEnMinuto = listadoDataSetpointExtranet.Where(x =>  x.Equicodi == urs.Equicodi).ToList();

                valSetpoint = SumarValoresRpfDelSegundo(segundoY, lstaExtranetEnMinuto);
                salida = valSetpoint;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpUnidadRepUnidad || caso == ConstantesDatosAGC.CasoUrsNoEspecial_TodasElegidas_OpCentralRepUnidad)
            {
                decimal? valSetpoint = null;

                List<RpfMedicion60DTO> lstaExtranetEnMinuto = listadoDataSetpointExtranet.Where(x => x.Equicodi == equicodiEquipo).ToList();

                valSetpoint = SumarValoresRpfDelSegundo(segundoY, lstaExtranetEnMinuto);
                salida = valSetpoint;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_Seleccionadas || caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_Seleccionadas)
            {
                decimal? valSetpoint = null;

                List<RpfMedicion60DTO> lstaExtranetEnMinuto = listadoDataSetpointExtranet.Where(x => equicodisUnidadesSeleccionadas.Contains(x.Equicodi.Value)).ToList();

                valSetpoint = SumarValoresRpfDelSegundo(segundoY, lstaExtranetEnMinuto);
                salida = valSetpoint;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_NoSeleccionadas)
            {
                decimal? valSetpoint = null;

                List<CoMedicion60DTO> lstaSp7EnMinuto = dataSP7.Where(x => x.Equicodi == equicodiEquipo).ToList();
                decimal? basepoint = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7EnMinuto);

                valSetpoint = basepoint;

                salida = valSetpoint;
            }

            if (caso == ConstantesDatosAGC.CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_NoSeleccionadas)
            {
                decimal? valSetpoint = null;

                List<CoMedicion60DTO> lstaSp7EnMinuto = dataSP7.Where(x => x.Equicodi == urs.Equicodi).ToList();
                decimal? basepoint = SumarValoresCoMed60DelSegundo(segundoY, lstaSp7EnMinuto);

                int numUnidadesTotales = equicodisUnidadesTotales.Count();
                valSetpoint = (basepoint != null && numUnidadesTotales != 0) ? basepoint / numUnidadesTotales : null;

                salida = valSetpoint;
            }

            //Trunco a máximo 5 decimales
            if (salida != null)
                salida = truncarNumeroAEneDecimales(salida.Value, 5);

            return salida;
        }

        /// <summary>
        /// Trunca el numero a N decimales
        /// </summary>
        /// <param name="numeroOriginal"></param>
        /// <param name="numDecimales"></param>
        /// <returns></returns>
        private decimal? truncarNumeroAEneDecimales(decimal numeroOriginal, int numDecimales)
        {            
            decimal? salida = null;
            decimal num = Convert.ToDecimal(Math.Pow(10.0, (double)numDecimales));
            decimal temp = numeroOriginal * num;
            salida = Math.Truncate(temp) / num;
            
            return salida;
        }

        /// <summary>
        /// Suma valores del segundo Hx de todos los elementos del listado
        /// </summary>
        /// <param name="segundo"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        private decimal? SumarValoresCoMed60DelSegundo(int segundo, List<CoMedicion60DTO> lista)
        {
            decimal? val = null;
            decimal? salidaX = null;

            switch (segundo)
            {
                case 0: salidaX = lista.Sum(x => x.H1); break;
                case 1: salidaX = lista.Sum(x => x.H2); break;
                case 2: salidaX = lista.Sum(x => x.H3); break;
                case 3: salidaX = lista.Sum(x => x.H4); break;
                case 4: salidaX = lista.Sum(x => x.H5); break;
                case 5: salidaX = lista.Sum(x => x.H6); break;
                case 6: salidaX = lista.Sum(x => x.H7); break;
                case 7: salidaX = lista.Sum(x => x.H8); break;
                case 8: salidaX = lista.Sum(x => x.H9); break;
                case 9: salidaX = lista.Sum(x => x.H10); break;
                case 10: salidaX = lista.Sum(x => x.H11); break;
                case 11: salidaX = lista.Sum(x => x.H12); break;
                case 12: salidaX = lista.Sum(x => x.H13); break;
                case 13: salidaX = lista.Sum(x => x.H14); break;
                case 14: salidaX = lista.Sum(x => x.H15); break;
                case 15: salidaX = lista.Sum(x => x.H16); break;
                case 16: salidaX = lista.Sum(x => x.H17); break;
                case 17: salidaX = lista.Sum(x => x.H18); break;
                case 18: salidaX = lista.Sum(x => x.H19); break;
                case 19: salidaX = lista.Sum(x => x.H20); break;
                case 20: salidaX = lista.Sum(x => x.H21); break;
                case 21: salidaX = lista.Sum(x => x.H22); break;
                case 22: salidaX = lista.Sum(x => x.H23); break;
                case 23: salidaX = lista.Sum(x => x.H24); break;
                case 24: salidaX = lista.Sum(x => x.H25); break;
                case 25: salidaX = lista.Sum(x => x.H26); break;
                case 26: salidaX = lista.Sum(x => x.H27); break;
                case 27: salidaX = lista.Sum(x => x.H28); break;
                case 28: salidaX = lista.Sum(x => x.H29); break;
                case 29: salidaX = lista.Sum(x => x.H30); break;
                case 30: salidaX = lista.Sum(x => x.H31); break;
                case 31: salidaX = lista.Sum(x => x.H32); break;
                case 32: salidaX = lista.Sum(x => x.H33); break;
                case 33: salidaX = lista.Sum(x => x.H34); break;
                case 34: salidaX = lista.Sum(x => x.H35); break;
                case 35: salidaX = lista.Sum(x => x.H36); break;
                case 36: salidaX = lista.Sum(x => x.H37); break;
                case 37: salidaX = lista.Sum(x => x.H38); break;
                case 38: salidaX = lista.Sum(x => x.H39); break;
                case 39: salidaX = lista.Sum(x => x.H40); break;
                case 40: salidaX = lista.Sum(x => x.H41); break;
                case 41: salidaX = lista.Sum(x => x.H42); break;
                case 42: salidaX = lista.Sum(x => x.H43); break;
                case 43: salidaX = lista.Sum(x => x.H44); break;
                case 44: salidaX = lista.Sum(x => x.H45); break;
                case 45: salidaX = lista.Sum(x => x.H46); break;
                case 46: salidaX = lista.Sum(x => x.H47); break;
                case 47: salidaX = lista.Sum(x => x.H48); break;
                case 48: salidaX = lista.Sum(x => x.H49); break;
                case 49: salidaX = lista.Sum(x => x.H50); break;
                case 50: salidaX = lista.Sum(x => x.H51); break;
                case 51: salidaX = lista.Sum(x => x.H52); break;
                case 52: salidaX = lista.Sum(x => x.H53); break;
                case 53: salidaX = lista.Sum(x => x.H54); break;
                case 54: salidaX = lista.Sum(x => x.H55); break;
                case 55: salidaX = lista.Sum(x => x.H56); break;
                case 56: salidaX = lista.Sum(x => x.H57); break;
                case 57: salidaX = lista.Sum(x => x.H58); break;
                case 58: salidaX = lista.Sum(x => x.H59); break;
                case 59: salidaX = lista.Sum(x => x.H60); break;

            }

            val = salidaX;

            return val;
        }

        /// <summary>
        /// Suma valores del segundo Hx de todos los elementos del listado
        /// </summary>
        /// <param name="segundo"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        private decimal? SumarValoresRpfDelSegundo(int segundo, List<RpfMedicion60DTO> lista)
        {
            decimal? val = null;
            decimal? salidaX = null;

            switch (segundo)
            {
                case 0: salidaX = lista.Sum(x => x.H1); break;
                case 1: salidaX = lista.Sum(x => x.H2); break;
                case 2: salidaX = lista.Sum(x => x.H3); break;
                case 3: salidaX = lista.Sum(x => x.H4); break;
                case 4: salidaX = lista.Sum(x => x.H5); break;
                case 5: salidaX = lista.Sum(x => x.H6); break;
                case 6: salidaX = lista.Sum(x => x.H7); break;
                case 7: salidaX = lista.Sum(x => x.H8); break;
                case 8: salidaX = lista.Sum(x => x.H9); break;
                case 9: salidaX = lista.Sum(x => x.H10); break;
                case 10: salidaX = lista.Sum(x => x.H11); break;
                case 11: salidaX = lista.Sum(x => x.H12); break;
                case 12: salidaX = lista.Sum(x => x.H13); break;
                case 13: salidaX = lista.Sum(x => x.H14); break;
                case 14: salidaX = lista.Sum(x => x.H15); break;
                case 15: salidaX = lista.Sum(x => x.H16); break;
                case 16: salidaX = lista.Sum(x => x.H17); break;
                case 17: salidaX = lista.Sum(x => x.H18); break;
                case 18: salidaX = lista.Sum(x => x.H19); break;
                case 19: salidaX = lista.Sum(x => x.H20); break;
                case 20: salidaX = lista.Sum(x => x.H21); break;
                case 21: salidaX = lista.Sum(x => x.H22); break;
                case 22: salidaX = lista.Sum(x => x.H23); break;
                case 23: salidaX = lista.Sum(x => x.H24); break;
                case 24: salidaX = lista.Sum(x => x.H25); break;
                case 25: salidaX = lista.Sum(x => x.H26); break;
                case 26: salidaX = lista.Sum(x => x.H27); break;
                case 27: salidaX = lista.Sum(x => x.H28); break;
                case 28: salidaX = lista.Sum(x => x.H29); break;
                case 29: salidaX = lista.Sum(x => x.H30); break;
                case 30: salidaX = lista.Sum(x => x.H31); break;
                case 31: salidaX = lista.Sum(x => x.H32); break;
                case 32: salidaX = lista.Sum(x => x.H33); break;
                case 33: salidaX = lista.Sum(x => x.H34); break;
                case 34: salidaX = lista.Sum(x => x.H35); break;
                case 35: salidaX = lista.Sum(x => x.H36); break;
                case 36: salidaX = lista.Sum(x => x.H37); break;
                case 37: salidaX = lista.Sum(x => x.H38); break;
                case 38: salidaX = lista.Sum(x => x.H39); break;
                case 39: salidaX = lista.Sum(x => x.H40); break;
                case 40: salidaX = lista.Sum(x => x.H41); break;
                case 41: salidaX = lista.Sum(x => x.H42); break;
                case 42: salidaX = lista.Sum(x => x.H43); break;
                case 43: salidaX = lista.Sum(x => x.H44); break;
                case 44: salidaX = lista.Sum(x => x.H45); break;
                case 45: salidaX = lista.Sum(x => x.H46); break;
                case 46: salidaX = lista.Sum(x => x.H47); break;
                case 47: salidaX = lista.Sum(x => x.H48); break;
                case 48: salidaX = lista.Sum(x => x.H49); break;
                case 49: salidaX = lista.Sum(x => x.H50); break;
                case 50: salidaX = lista.Sum(x => x.H51); break;
                case 51: salidaX = lista.Sum(x => x.H52); break;
                case 52: salidaX = lista.Sum(x => x.H53); break;
                case 53: salidaX = lista.Sum(x => x.H54); break;
                case 54: salidaX = lista.Sum(x => x.H55); break;
                case 55: salidaX = lista.Sum(x => x.H56); break;
                case 56: salidaX = lista.Sum(x => x.H57); break;
                case 57: salidaX = lista.Sum(x => x.H58); break;
                case 58: salidaX = lista.Sum(x => x.H59); break;
                case 59: salidaX = lista.Sum(x => x.H60); break;

            }

            val = salidaX;

            return val;
        }

        /// <summary>
        /// Obtiene lista datos (matriz) a pintar
        /// </summary>
        /// <param name="lstKumpliy"></param>
        /// <param name="numSegundosAnalizados"></param>
        /// <returns></returns>
        private List<DatoCsv> ConvertirDataKumpliyParaCSV(List<RegKumpliy> lstKumpliy, int numSegundosAnalizados)
        {
            List<DatoCsv> lstSalida = new List<DatoCsv>();

            #region primera columna
            int columna = 1;
            int numFilasTotales = numSegundosAnalizados;

            for (int i = 0; i <= numFilasTotales; i++)
            {
                DatoCsv dato = new DatoCsv();
                dato.Columna = columna;
                dato.Fila = i;
                dato.Valor = i.ToString();
                lstSalida.Add(dato);
            }

            #endregion

            #region demas columnas
            //Armamos las siguientes columnas (> 2)
            int columnaX = 2;
            
            var lstGruposTriColumnas = lstKumpliy.GroupBy(x => new { x.OrdenColumna }).ToList();
            int numColumnasDe3 = lstGruposTriColumnas.Count();

            foreach (var triColumna in lstGruposTriColumnas)
            {
                int numGC = triColumna.Count(); //86400 
                
                foreach (var reg in triColumna)
                {                    
                    int numDatosXColumna = 3;
                    for (int numDato = 1; numDato <= numDatosXColumna; numDato++)
                    {
                        //pinto el ptomedicion del segundo 0 (es igual que los de los otros segundos)
                        if (reg.NumSegundo == 0) 
                        {
                            DatoCsv datoCab = new DatoCsv();

                            datoCab.Columna = columnaX + numDato - 1;
                            datoCab.Fila = 0;
                            datoCab.Valor = reg.Ptomedicion != null ? reg.Ptomedicion.ToString() : "_";
                            lstSalida.Add(datoCab);
                        }

                        decimal? valdato = null;

                        switch (numDato)
                        {
                            case 1: valdato = reg.Setpoint != null ? reg.Setpoint.Value : 0; break;
                            case 2: valdato = reg.Estado != null ? reg.Estado.Value : 0; break;
                            case 3: valdato = reg.Basepoint != null ? reg.Basepoint.Value : 0; break;
                        }

                        DatoCsv dato = new DatoCsv();

                        dato.Columna = columnaX + numDato - 1;
                        dato.Fila = reg.NumSegundo + 1;
                        dato.Valor = valdato.ToString();
                        lstSalida.Add(dato);
                    }
                }
                columnaX = columnaX + 3;
            }

            #endregion

            return lstSalida;
        }

        /// <summary>
        /// Consolida la informacion reportada por agentes en una lista 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="numFilasTotales"></param>
        /// <param name="listadoDataExtranet"></param>
        /// <param name="listadoPtosMedicion"></param>
        /// <param name="lstUrs"></param>
        /// <param name="equipos"></param>
        /// <param name="lstEquiposSinPtomedicion"></param>
        /// <returns></returns>
        private List<DatoCsv> ConvertirDataExtranetParaCSV(DateTime fecha, int numFilasTotales, List<RpfMedicion60DTO> listadoDataExtranet, List<ServicioRpfDTO> listadoPtosMedicion, List<EveRsfdetalleDTO> lstUrs, List<EqEquipoDTO> equipos, out List<int> lstEquiposSinPtomedicion)
        {
            List<DatoCsv> lstSalida = new List<DatoCsv>();
            lstEquiposSinPtomedicion = new List<int>();

            List<int> lstSenial = new List<int>();
            lstSenial.Add(ConstantesDatosAGC.CodigoSetPoint);
            lstSenial.Add(ConstantesDatosAGC.CodigoEstado);

            #region primera columna
            int columna = 1;

            for (int i = 1; i <= 8; i++)
            {
                DatoCsv dato = new DatoCsv();
                dato.Columna = columna;
                dato.Fila = i;
                string valorC = "";

                switch (i)
                {
                    case 1: valorC = "PTOMEDICODI"; break;
                    case 2: valorC = "TIPOINFOCODI"; break;
                    case 3: valorC = "FECHA (AAAA-MM-DD)"; break;
                    case 4: valorC = "EMPRESA"; break;
                    case 5: valorC = "URS"; break;
                    case 6: valorC = "CENTRAL"; break;
                    case 7: valorC = "EQUIPO"; break;
                    case 8: valorC = "fecha hora"; break;
                }
                dato.Valor = valorC;
                lstSalida.Add(dato);
            }

            for (int f = 1 + 8; f <= numFilasTotales + 8; f++)
            {


                DatoCsv dato = new DatoCsv();
                dato.Columna = columna;
                dato.Fila = f;

                int fx = f - 1 - 8;
                TimeSpan t = TimeSpan.FromSeconds(fx);

                int horaT = t.Hours;
                int minutoT = t.Minutes;
                int segundoT = t.Seconds;

                dato.Valor = horaT.ToString("00") + ":" + minutoT.ToString("00") + ":" + segundoT.ToString("00");
                lstSalida.Add(dato);
            }


            #endregion

            #region demas columnas
            //Armamos las siguientes columnas (> 2)
            int columnaX = 2;
            //lstUrs esta ordenado por nombre de central
            foreach (var urs in lstUrs)
            {
                string nombreCentral = urs.Gruponomb.Trim();
                string nombreURS = ((urs.Ursnomb.Replace("-", "_")).Replace("URS_","")).Trim();
                string nombreEmpresa = urs.Emprnomb.Trim();

                //Data por cada URS
                List<RpfMedicion60DTO> lstExtranetPorUrs = listadoDataExtranet.Where(x => x.Grupocodi == urs.Grupocodi).ToList();
                if (lstExtranetPorUrs.Any())
                {
                    //ordeno por equiabrev (Equipo)
                    List<int> lstEquicodis = lstExtranetPorUrs.GroupBy(x => x.Equicodi.Value).Select(n => n.Key).ToList();

                    List<EqEquipoDTO> listaEquiposOrdenados = equipos.Where(d => lstEquicodis.Contains(d.Equicodi)).OrderBy(v=>v.Equiabrev).ToList();

                    foreach (var equipo in listaEquiposOrdenados)
                    {
                        string nombreEquipo = equipo.Equiabrev.Trim();

                        List<RpfMedicion60DTO> lstExtranetPorUrsYEquipo = lstExtranetPorUrs.Where(x => x.Equicodi == equipo.Equicodi).ToList();

                        //Ordeno por tipo de senial
                        foreach (var senial in lstSenial)
                        {
                            List<RpfMedicion60DTO> lstExtranetPorUrsYEquipoYSenial = lstExtranetPorUrsYEquipo.Where(x => x.Cotidacodi == senial).ToList();
                            RpfMedicion60DTO objM = lstExtranetPorUrsYEquipoYSenial.First();
                            ServicioRpfDTO objPto = listadoPtosMedicion.Find(x => x.EQUICODI == equipo.Equicodi);
                            if (objPto == null)
                            {
                                lstEquiposSinPtomedicion.Add(equipo.Equicodi);
                            }

                            int numFilasCab = 8;
                            int numFilasData = numFilasTotales;

                            //armo la data cabecera
                            for (int i = 1; i <= numFilasCab; i++)
                            {
                                DatoCsv dato = new DatoCsv();
                                dato.Columna = columnaX;
                                dato.Fila = i;
                                string val = "";
                                switch (i)
                                {
                                    case 1: val = objPto != null ? objPto.PTOMEDICODI.ToString() : "_"; break; //ptomedicodi
                                    case 2: val = objM != null ? (objM.Cotidacodi.Value == ConstantesDatosAGC.CodigoSetPoint ? "1":(objM.Cotidacodi.Value == ConstantesDatosAGC.CodigoEstado ? "3" : "_")) : "_"; break; //tipoinfocodi
                                    case 3: val = dato.Columna == 2 ? fecha.Year.ToString() : (dato.Columna == 3 ? fecha.Month.ToString() : (dato.Columna == 4 ? fecha.Day.ToString() : "")); break; //fecha
                                    case 4: val = nombreEmpresa; break; //empresa
                                    case 5: val = nombreURS; break; //urs
                                    case 6: val = nombreCentral; break; //central
                                    case 7: val = nombreEquipo; break; //equipo
                                    case 8: val = senial == ConstantesDatosAGC.CodigoSetPoint ? "MW" : (senial == ConstantesDatosAGC.CodigoEstado ? "Estado" : "_"); break; //tipo Senial
                                    //default:
                                    //    break;
                                }
                                dato.Valor = val;
                                lstSalida.Add(dato);
                            }

                            int horaYTemp = -100;
                            int minutoYTemp = -100;
                            RpfMedicion60DTO datTemp = new RpfMedicion60DTO();

                            //armo data cuerpo
                            for (int f = 1 + numFilasCab; f <= numFilasData + numFilasCab; f++)
                            {
                                DatoCsv dato = new DatoCsv();
                                dato.Columna = columnaX;
                                dato.Fila = f;

                                int filaParaHora = f - 1 - numFilasCab;

                                TimeSpan t = TimeSpan.FromSeconds(filaParaHora);

                                int horaY = t.Hours;
                                int minutoY = t.Minutes;
                                int segundoY = t.Seconds;

                                string Hx = "H" + (segundoY + 1);

                                if (horaY != horaYTemp || minutoY != minutoYTemp)
                                {
                                    datTemp = lstExtranetPorUrsYEquipoYSenial.Find(x => x.Rpfmedfecha.Value.Hour == horaY && x.Rpfmedfecha.Value.Minute == minutoY);
                                }

                                if (datTemp == null)
                                    dato.Valor = "";
                                else
                                {
                                    decimal? v = (decimal?)datTemp.GetType().GetProperty(Hx).GetValue(datTemp, null);
                                    
                                    if(v != null)
                                    {
                                        decimal valor5decimales = MathHelper.TruncateDecimal(v.Value, 5);
                                        dato.Valor = valor5decimales.ToString();
                                    }
                                    else
                                    {
                                        dato.Valor = "";
                                    }                                        
                                }                                                               

                                lstSalida.Add(dato);

                                horaYTemp = horaY;
                                minutoYTemp = minutoY;
                            }
                            columnaX++;
                        }
                    }
                }

            }

            #endregion

            return lstSalida;
        }

        #endregion

        #endregion
    }

    public class ResultFormatoAGC
    {
        public string NombreURS { get; set; }
        public string Mensaje { get; set; }
    }
}

