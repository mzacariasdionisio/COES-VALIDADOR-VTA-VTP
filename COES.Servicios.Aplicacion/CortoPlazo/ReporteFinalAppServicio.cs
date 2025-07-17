using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using System.Globalization;

namespace COES.Servicios.Aplicacion.CortoPlazo
{
    /// <summary>
    /// Clases con métodos del módulo ReporteFinal
    /// </summary>
    public class ReporteFinalAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReporteFinalAppServicio));

        #region Configuracion de parámetros

        /// <summary>
        /// Elimina un registro de la tabla CM_BARRA_RELACION
        /// </summary>
        public int DeleteCmBarraRelacion(int cmbarecodi)
        {
            try
            {
                FactorySic.GetCmBarraRelacionRepository().Delete(cmbarecodi);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_BARRA_RELACION
        /// </summary>
        public CmBarraRelacionDTO GetByIdCmBarraRelacion(int cmbarecodi)
        {
            return FactorySic.GetCmBarraRelacionRepository().GetById(cmbarecodi);
        }  

        /// <summary>
        /// Elimina un registro de la tabla CM_PERIODO
        /// </summary>
        public int DeleteCmPeriodo(int cmpercodi)
        {
            try
            {
                FactorySic.GetCmPeriodoRepository().Delete(cmpercodi);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_PERIODO
        /// </summary>
        public CmPeriodoDTO GetByIdCmPeriodo(int cmpercodi)
        {
            return FactorySic.GetCmPeriodoRepository().GetById(cmpercodi);
        }              

        /// <summary>
        /// Elimina un registro de la tabla CM_UMBRALREPORTE
        /// </summary>
        public int DeleteCmUmbralreporte(int cmurcodi)
        {
            try
            {
                FactorySic.GetCmUmbralreporteRepository().Delete(cmurcodi);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_UMBRALREPORTE
        /// </summary>
        public CmUmbralreporteDTO GetByIdCmUmbralreporte(int cmurcodi)
        {
            return FactorySic.GetCmUmbralreporteRepository().GetById(cmurcodi);
        }      

        /// <summary>
        /// Elimina un registro de la tabla CM_EQUIPOBARRA
        /// </summary>
        public int DeleteCmEquipobarra(int cmeqbacodi)
        {
            try
            {
                FactorySic.GetCmEquipobarraRepository().Delete(cmeqbacodi);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_EQUIPOBARRA
        /// </summary>
        public CmEquipobarraDTO GetByIdCmEquipobarra(int cmeqbacodi)
        {
            CmEquipobarraDTO entity = FactorySic.GetCmEquipobarraRepository().GetById(cmeqbacodi);
            entity.ListaDetalle = FactorySic.GetCmEquipobarraDetRepository().GetByCriteria(entity.Cmeqbacodi);
            return entity;
        }        

        /// <summary>
        /// Permite obtener las barras EMS
        /// </summary>
        /// <returns></returns>
        public List<CmConfigbarraDTO> ObtenerBarrasEMS()
        {
            return FactorySic.GetCmConfigbarraRepository().GetByCriteria((-1).ToString(), (-1).ToString()).ToList();
        }

        /// <summary>
        /// Permite obtener la relacion de barras ems adicionales
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        public List<CmBarraRelacionDetDTO> ObtenerBarrasEMSAdicionales(int idRelacion)
        {
            return FactorySic.GetCmBarraRelacionDetRepository().GetByCriteria(idRelacion);
        }

        /// <summary>
        /// Permite obtener las barras de transferencia
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<BarraDTO> ObtenerBarrasTransferencia()
        {
            return (new Transferencias.BarraAppServicio()).ListaBarrasActivas();
        }

        /// <summary>
        /// Permite obtener las barras desconocidas
        /// </summary>
        /// <returns></returns>
        public List<BarraDTO> ObtenerBarrasTransferenciaDesconocida(DateTime fecha)
        {
            List<BarraDTO> entitys = this.ObtenerBarrasTransferencia();

            List<CmBarraRelacionDTO> barras = FactorySic.GetCmBarraRelacionRepository().GetByCriteria(fecha)
                .Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraDesconocida).ToList();

            return entitys.Where(x => barras.Any(y => y.Barrcodi == x.BarrCodi)).ToList();
        }

        /// <summary>
        /// Permite obtener la configuración por fecha
        /// </summary>
        /// <param name="fecha"></param>
        public ConfiguracionReporteFinal ObtenerConfiguracionPorFecha(DateTime fecha)
        {
            ConfiguracionReporteFinal result = new ConfiguracionReporteFinal();

            List<CmBarraRelacionDTO> barras = FactorySic.GetCmBarraRelacionRepository().GetByCriteria(fecha);
            result.ListaBarraDesconocida = barras.Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraDesconocida).ToList();
            result.ListaBarraEMS = barras.Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraEMS).ToList();
            result.ListaBarraTransferencia = barras.Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraTransferencia).ToList();
            result.ListaUmbral = FactorySic.GetCmUmbralreporteRepository().GetByCriteria(fecha);
            result.ListaPeriodo = FactorySic.GetCmPeriodoRepository().GetByCriteria(fecha);
            result.ListaRelacionEquipo = FactorySic.GetCmEquipobarraRepository().GetByCriteria(fecha);

            foreach (CmEquipobarraDTO entity in result.ListaRelacionEquipo)
            {
                entity.ListaDetalle = FactorySic.GetCmEquipobarraDetRepository().GetByCriteria(entity.Cmeqbacodi);
            }

            return result;
        }

        /// <summary>
        /// Permite grabar la configuracion de la barras
        /// </summary>
        /// <param name="entity"></param>
        public ResultadoConfiguracion GrabarConfiguracionBarra(CmBarraRelacionDTO entity)
        {
            ResultadoConfiguracion result = new ResultadoConfiguracion();
            try
            {
                result.Resultado = 1;
                List<CmBarraRelacionDTO> list = FactorySic.GetCmBarraRelacionRepository().GetByCriteria(entity.Cmbaretipreg, (int)entity.Barrcodi).
                    OrderBy(x => x.Cmbarevigencia).ToList();

                if (list.Count > 0)
                {
                    if (entity.Cmbarecodi == 0)
                    {
                        if (((DateTime)entity.Cmbarevigencia).Subtract(((DateTime)list[list.Count - 1].Cmbarevigencia)).TotalSeconds < 0)
                        {
                            result.Mensaje = "La fecha de vigencia debe ser superior a la fecha de vigencia de la última configuración realizada.";
                            result.Resultado = 2;
                        }
                    }
                    else
                    {
                        int selected = 0;
                        for (int index = 0; index < list.Count; index++)
                        {
                            if (list[index].Cmbarecodi == entity.Cmbarecodi)
                            {
                                selected = index;
                            }
                        }

                        int anterior = selected - 1;
                        int posterior = selected + 1;
                        DateTime fechaMin = DateTime.MinValue;
                        DateTime fechaMax = DateTime.MaxValue;

                        if (anterior >= 0)
                        {
                            fechaMin = ((DateTime)list[anterior].Cmbarevigencia);
                        }

                        if (posterior < list.Count)
                        {
                            fechaMax = ((DateTime)list[posterior].Cmbarevigencia);
                        }

                        if (!(((DateTime)entity.Cmbarevigencia).Subtract(fechaMin).TotalSeconds > 0 &&
                            ((DateTime)entity.Cmbarevigencia).Subtract(fechaMax).TotalSeconds < 0))
                        {
                            result.Mensaje = "La fecha de vigencia debe estar en el rango de vigencia de la configuración anterior y superior.";
                            result.Resultado = 2;
                        }
                    }
                }

                if (entity.Cmbareexpira != null)
                {
                    if (((DateTime)entity.Cmbarevigencia).Subtract(((DateTime)entity.Cmbareexpira)).TotalSeconds > 0)
                    {
                        result.Mensaje = "La fecha de vigencia debe ser menor a la fecha de expiración.";
                        result.Resultado = 2;
                    }
                }                

                if (result.Resultado == 1)
                {
                    if (entity.Barrcodi2 == -1 || entity.Barrcodi2 == 0) entity.Barrcodi2 = null;
                    if (entity.Cnfbarcodi == -1 || entity.Cnfbarcodi == 0) entity.Cnfbarcodi = null;

                    if (entity.Cmbarecodi == 0)
                    {
                        entity.Cmbareusumodificacion = entity.Cmbareusucreacion;
                        entity.Cmbarefeccreacion = DateTime.Now;
                        entity.Cmbarefecmodificacion = DateTime.Now;
                        FactorySic.GetCmBarraRelacionRepository().Save(entity);
                    }
                    else
                    {
                        entity.Cmbareusumodificacion = entity.Cmbareusucreacion;                        
                        entity.Cmbarefecmodificacion = DateTime.Now;
                        FactorySic.GetCmBarraRelacionRepository().Update(entity);
                    }

                    result.Mensaje = "La operación se realizó correctamente.";
                }
                
            }
            catch (Exception ex)
            {
                result.Resultado = -1;
            }

            return result;
        }

        /// <summary>
        /// Permite obtener el historico de cambios de la barra
        /// </summary>
        /// <param name="idBarra"></param>
        /// <returns></returns>
        public List<CmBarraRelacionDTO> ObtenerHistoricoBarra(int idBarra, string tipoRegistro)
        {
            return FactorySic.GetCmBarraRelacionRepository().ObtenerHistorico(idBarra, tipoRegistro);
        }

        /// <summary>
        /// Permite grabar los datos de periodos
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoConfiguracion GrabarConfiguracionPeriodo(CmPeriodoDTO entity)
        {
            ResultadoConfiguracion result = new ResultadoConfiguracion();
            try
            {
                result.Resultado = 1;
                List<CmPeriodoDTO> list = FactorySic.GetCmPeriodoRepository().ObtenerHistoricoPeriodo().OrderBy(x => x.Cmpervigencia).ToList();

                if (list.Count > 0)
                {
                    if (entity.Cmpercodi == 0)
                    {
                        if (((DateTime)entity.Cmpervigencia).Subtract(((DateTime)list[list.Count - 1].Cmpervigencia)).TotalSeconds < 0)
                        {
                            result.Mensaje = "La fecha de vigencia debe ser superior a la fecha de vigencia de la última configuración realizada.";
                            result.Resultado = 2;
                        }
                    }
                    else
                    {
                        int selected = 0;
                        for (int index = 0; index < list.Count; index++)
                        {
                            if (list[index].Cmpercodi == entity.Cmpercodi)
                            {
                                selected = index;
                            }
                        }

                        int anterior = selected - 1;
                        int posterior = selected + 1;
                        DateTime fechaMin = DateTime.MinValue;
                        DateTime fechaMax = DateTime.MaxValue;

                        if (anterior >= 0)
                        {
                            fechaMin = ((DateTime)list[anterior].Cmpervigencia);
                        }

                        if (posterior < list.Count)
                        {
                            fechaMax = ((DateTime)list[posterior].Cmpervigencia);
                        }

                        if (!(((DateTime)entity.Cmpervigencia).Subtract(fechaMin).TotalSeconds > 0 &&
                            ((DateTime)entity.Cmpervigencia).Subtract(fechaMax).TotalSeconds < 0))
                        {
                            result.Mensaje = "La fecha de vigencia debe estar en el rango de vigencia de la configuración anterior y superior.";
                            result.Resultado = 2;
                        }
                    }
                }

                if (entity.Cmperexpira != null)
                {
                    if (((DateTime)entity.Cmpervigencia).Subtract(((DateTime)entity.Cmperexpira)).TotalSeconds > 0)
                    {
                        result.Mensaje = "La fecha de vigencia debe ser menor a la fecha de expiración.";
                        result.Resultado = 2;
                    }
                }

                //- Validación de Periodos

                string[] horas = { "00:30", "01:00" , "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30",
                                            "06:00" , "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30",
                                            "11:00" , "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30",
                                            "16:00" , "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30",
                                            "21:00" , "21:30", "22:00", "22:30", "23:00", "23:30", "23:59"};

                string horaIniBase = entity.Cmperbase.Split('-')[0].Trim();
                string horaFinBase = entity.Cmperbase.Split('-')[1].Trim();
                string horaIniMedia = entity.Cmpermedia.Split('-')[0].Trim();
                string horaFinMedia = entity.Cmpermedia.Split('-')[1].Trim();
                string horaIniPunta = entity.Cmperpunta.Split('-')[0].Trim();
                string horaFinPunta = entity.Cmperpunta.Split('-')[1].Trim();

                string[] datos = { horaIniBase, horaFinBase, horaIniMedia, horaFinMedia, horaIniPunta, horaFinPunta };
               
                if (horas.Where(x => datos.Any(y => x == y)).Count() != 6)
                {
                    result.Mensaje = "El formato de horas debe ser hh:mm y deben ser consecutivos. El inicio debe ser 00:30 y el final 23:59";
                    result.Resultado = 2;
                }
                else
                {
                    if (!(datos[0] == "00:30" && datos[5] == "23:59"))
                    {
                        result.Mensaje = "La hora inicio debe ser 00:30 y la última hora debe ser 23:59";
                        result.Resultado = 2;
                    }

                    else
                    {
                        int index = -1;
                        bool flagIndex = true;
                        for(int k = 0; k < datos.Length; k++)
                        {
                            int newIndex = horas.ToList().IndexOf(datos[k]);

                            if (newIndex > index) {
                                index = newIndex;
                            }
                            else 
                            {
                                flagIndex = false;
                            }
                        }

                        if (!flagIndex)
                        {
                            result.Mensaje = "Las horas deben estar ordenados ascendentemente.";
                            result.Resultado = 2;
                        }
                        else
                        {

                            int postMedia = horas.ToList().IndexOf(horaFinBase);
                            if (postMedia < horas.Length - 1)
                            {
                                postMedia = postMedia + 1;

                                if (horaIniMedia != horas[postMedia])
                                {
                                    result.Mensaje = "Los periodos deben ser consecutivos.";
                                    result.Resultado = 2;
                                }
                            }

                            int postPunta = horas.ToList().IndexOf(horaFinMedia);

                            if (postPunta < horas.Length - 1)
                            {
                                postPunta = postPunta + 1;

                                if (horaIniPunta != horas[postPunta])
                                {
                                    result.Mensaje = "Los periodos deben ser consecutivos.";
                                    result.Resultado = 2;
                                }
                            }
                        }
                    }
                }


                if (result.Resultado == 1)
                {                    

                    if (entity.Cmpercodi == 0)
                    {
                        entity.Cmperusumodificacion = entity.Cmperusucreacion;
                        entity.Cmperfeccreacion = DateTime.Now;
                        entity.Cmperfecmodificacion = DateTime.Now;
                        FactorySic.GetCmPeriodoRepository().Save(entity);
                    }
                    else
                    {
                        entity.Cmperusumodificacion = entity.Cmperusucreacion;
                        entity.Cmperfecmodificacion = DateTime.Now;
                        FactorySic.GetCmPeriodoRepository().Update(entity);
                    }

                    result.Mensaje = "La operación se realizó correctamente.";
                }

            }
            catch (Exception ex)
            {
                result.Resultado = -1;
            }

            return result;
        }

        /// <summary>
        /// Permite obtener el historico de cambios de la barra
        /// </summary>
        /// <param name="idBarra"></param>
        /// <returns></returns>
        public List<CmPeriodoDTO> ObtenerHistoricoPeriodo()
        {
            return FactorySic.GetCmPeriodoRepository().ObtenerHistoricoPeriodo();
        }

        public ResultadoConfiguracion GrabarConfiguracionUmbral(CmUmbralreporteDTO entity)
        {
            ResultadoConfiguracion result = new ResultadoConfiguracion();
            try
            {
                result.Resultado = 1;
                List<CmUmbralreporteDTO> list = FactorySic.GetCmUmbralreporteRepository().ObtenerHistorico().OrderBy(x => x.Cmurvigencia).ToList();

                if (list.Count > 0)
                {
                    if (entity.Cmurcodi == 0)
                    {
                        if (((DateTime)entity.Cmurvigencia).Subtract(((DateTime)list[list.Count - 1].Cmurvigencia)).TotalSeconds < 0)
                        {
                            result.Mensaje = "La fecha de vigencia debe ser superior a la fecha de vigencia de la última configuración realizada.";
                            result.Resultado = 2;
                        }
                    }
                    else
                    {
                        int selected = 0;
                        for (int index = 0; index < list.Count; index++)
                        {
                            if (list[index].Cmurcodi == entity.Cmurcodi)
                            {
                                selected = index;
                            }
                        }

                        int anterior = selected - 1;
                        int posterior = selected + 1;
                        DateTime fechaMin = DateTime.MinValue;
                        DateTime fechaMax = DateTime.MaxValue;

                        if (anterior >= 0)
                        {
                            fechaMin = ((DateTime)list[anterior].Cmurvigencia);
                        }

                        if (posterior < list.Count)
                        {
                            fechaMax = ((DateTime)list[posterior].Cmurvigencia);
                        }

                        if (!(((DateTime)entity.Cmurvigencia).Subtract(fechaMin).TotalSeconds > 0 &&
                            ((DateTime)entity.Cmurvigencia).Subtract(fechaMax).TotalSeconds < 0))
                        {
                            result.Mensaje = "La fecha de vigencia debe estar en el rango de vigencia de la configuración anterior y superior.";
                            result.Resultado = 2;
                        }
                    }
                }

                if (entity.Cmurexpira != null)
                {
                    if (((DateTime)entity.Cmurvigencia).Subtract(((DateTime)entity.Cmurexpira)).TotalSeconds > 0)
                    {
                        result.Mensaje = "La fecha de vigencia debe ser menor a la fecha de expiración.";
                        result.Resultado = 2;
                    }
                }

                if((decimal)entity.Cmurmaxbarra <= (decimal)entity.Cmurminbarra ||
                    (decimal)entity.Cmurmaxenergia <= (decimal)entity.Cmurminenergia ||
                    (decimal)entity.Cmurmaxconges <= (decimal)entity.Cmurminconges)
                {
                    result.Mensaje = "El umbral maximo debe ser mayor al mínimo";
                    result.Resultado = 2;
                }

                if (result.Resultado == 1)
                {

                    if (entity.Cmurcodi == 0)
                    {
                        entity.Cmurusumodificacion = entity.Cmurusucreacion;
                        entity.Cmurfeccreacion = DateTime.Now;
                        entity.Cmurfecmodificacion = DateTime.Now;
                        FactorySic.GetCmUmbralreporteRepository().Save(entity);
                    }
                    else
                    {
                        entity.Cmurusumodificacion = entity.Cmurusucreacion;
                        entity.Cmurfecmodificacion = DateTime.Now;
                        FactorySic.GetCmUmbralreporteRepository().Update(entity);
                    }

                    result.Mensaje = "La operación se realizó correctamente.";
                }

            }
            catch (Exception ex)
            {
                result.Resultado = -1;
            }

            return result;
        }

        /// <summary>
        /// Permite obtener el historico de umbrales
        /// </summary>
        /// <returns></returns>
        public List<CmUmbralreporteDTO> ObtenerHistoricoUmbral()
        {
            return FactorySic.GetCmUmbralreporteRepository().ObtenerHistorico();
        }
        
        /// <summary>
        /// Permite obtener el historico de cambios de un equipo
        /// </summary>
        /// <param name="idConfig"></param>
        /// <returns></returns>
        public List<CmEquipobarraDTO> ObtenerHistoricoEquipo(int idConfig)
        {
            List<CmEquipobarraDTO> list = FactorySic.GetCmEquipobarraRepository().ObtenerHistorico(idConfig);

            foreach (CmEquipobarraDTO entity in list)
            {
                entity.ListaDetalle = FactorySic.GetCmEquipobarraDetRepository().GetByCriteria(entity.Cmeqbacodi);
            }

            return list;
        }

        /// <summary>
        /// Permite obtener los trafos 3D
        /// </summary>
        /// <returns></returns>
        public List<EqCongestionConfigDTO> ObtenerEquiposCongestion()
        {
            return FactorySic.GetEqCongestionConfigRepository().GetByCriteria(-1, ConstantesAppServicio.ActivoDesc.ToUpper(), -1,
                ConstantesCortoPlazo.IdTrafo3D);
        }

        /// <summary>
        /// Permite obtener las barras adicionales
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CmBarraRelacionDTO> ObtenerBarrasAdicionales(DateTime fecha)
        {
            List<CmBarraRelacionDTO> barras = FactorySic.GetCmBarraRelacionRepository().GetByCriteria(fecha);
            return barras.Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraEMS && x.Cmbaretiprel ==
                ConfiguracionReporteFinal.BarraAdicional).ToList();
        }


        /// <summary>
        /// Permite grabar la configuracion de equipos
        /// </summary>
        /// <param name="entity"></param>
        public ResultadoConfiguracion GrabarConfiguracionEquipo(CmEquipobarraDTO entity)
        {
            ResultadoConfiguracion result = new ResultadoConfiguracion();
            try
            {
                result.Resultado = 1;
                List<CmEquipobarraDTO> list = FactorySic.GetCmEquipobarraRepository().ObtenerHistorico((int)entity.Configcodi).
                    OrderBy(x => x.Cmeqbavigencia).ToList();

                if (list.Count > 0)
                {
                    if (entity.Cmeqbacodi == 0)
                    {
                        if (((DateTime)entity.Cmeqbavigencia).Subtract(((DateTime)list[list.Count - 1].Cmeqbavigencia)).TotalSeconds < 0)
                        {
                            result.Mensaje = "La fecha de vigencia debe ser superior a la fecha de vigencia de la última configuración realizada.";
                            result.Resultado = 2;
                        }
                    }
                    else
                    {
                        int selected = 0;
                        for (int index = 0; index < list.Count; index++)
                        {
                            if (list[index].Cmeqbacodi == entity.Cmeqbacodi)
                            {
                                selected = index;
                            }
                        }

                        int anterior = selected - 1;
                        int posterior = selected + 1;
                        DateTime fechaMin = DateTime.MinValue;
                        DateTime fechaMax = DateTime.MaxValue;

                        if (anterior >= 0)
                        {
                            fechaMin = ((DateTime)list[anterior].Cmeqbavigencia);
                        }

                        if (posterior < list.Count)
                        {
                            fechaMax = ((DateTime)list[posterior].Cmeqbavigencia);
                        }

                        if (!(((DateTime)entity.Cmeqbavigencia).Subtract(fechaMin).TotalSeconds > 0 &&
                            ((DateTime)entity.Cmeqbavigencia).Subtract(fechaMax).TotalSeconds < 0))
                        {
                            result.Mensaje = "La fecha de vigencia debe estar en el rango de vigencia de la configuración anterior y superior.";
                            result.Resultado = 2;
                        }
                    }
                }

                if (entity.Cmeqbaexpira != null)
                {
                    if (((DateTime)entity.Cmeqbavigencia).Subtract(((DateTime)entity.Cmeqbaexpira)).TotalSeconds > 0)
                    {
                        result.Mensaje = "La fecha de vigencia debe ser menor a la fecha de expiración.";
                        result.Resultado = 2;
                    }
                }

                if (result.Resultado == 1)
                {
                    int id = 0;
                    if (entity.Cmeqbacodi == 0)
                    {
                        entity.Cmeqbausumodificacion = entity.Cmeqbausucreacion;
                        entity.Cmeqbafeccreacion = DateTime.Now;
                        entity.Cmeqbafecmodificacion = DateTime.Now;
                        id = FactorySic.GetCmEquipobarraRepository().Save(entity);
                    }
                    else
                    {
                        FactorySic.GetCmEquipobarraDetRepository().Delete(entity.Cmeqbacodi);
                        entity.Cmeqbausumodificacion = entity.Cmeqbausucreacion;
                        entity.Cmeqbafecmodificacion = DateTime.Now;
                        FactorySic.GetCmEquipobarraRepository().Update(entity);
                        id = entity.Cmeqbacodi;
                    }

                    List<int> idsBarras = entity.Barras.Split(',').Select(int.Parse).ToList();

                    foreach(int idBarra in idsBarras)
                    {
                        CmEquipobarraDetDTO detalle = new CmEquipobarraDetDTO()
                        {
                            Barrcodi = idBarra,
                            Cmeqbacodi = id,
                            Cmebdefeccreacion = DateTime.Now,
                            Cmebdefecmodificacion = DateTime.Now,
                            Cmebdeusucreacion = entity.Cmeqbausucreacion,
                            Cmebdeusumodificacion = entity.Cmeqbausucreacion
                        };

                        FactorySic.GetCmEquipobarraDetRepository().Save(detalle);
                    }

                    result.Mensaje = "La operación se realizó correctamente.";
                }

            }
            catch (Exception ex)
            {
                result.Resultado = -1;
            }

            return result;
        }


        /// <summary>
        /// Permite grabar la configuracion de equipos
        /// </summary>
        /// <param name="entity"></param>
        public ResultadoConfiguracion GraberConfiguracionBarraEMSAdicional(CmBarraRelacionDTO entity)
        {
            ResultadoConfiguracion result = new ResultadoConfiguracion();
            try
            {
                result.Resultado = 1;
              
                if (result.Resultado == 1)
                {
                    int id = entity.Cmbarecodi;

                    List<int> idsBarras = (!string.IsNullOrEmpty(entity.Barras)) ? entity.Barras.Split(',').Select(int.Parse).ToList() : new List<int>();

                    FactorySic.GetCmBarraRelacionDetRepository().Delete(id);

                    foreach (int idBarra in idsBarras)
                    {
                        CmBarraRelacionDetDTO detalle = new CmBarraRelacionDetDTO()
                        {
                            Cnfbarcodi = idBarra,
                            Cmbarecodi = id,
                            Cmbadeestado = ConstantesAppServicio.Activo,
                            Cmbadefeccreacion = DateTime.Now,
                            Cmbadefecmodificacion = DateTime.Now,
                            Cmbadeusucreacion = entity.Cmbareusucreacion,
                            Cmbadeusumodificacion = entity.Cmbareusucreacion
                        };

                        FactorySic.GetCmBarraRelacionDetRepository().Save(detalle);
                    }

                    result.Mensaje = "La operación se realizó correctamente.";
                }

            }
            catch (Exception ex)
            {
                result.Resultado = -1;
            }

            return result;
        }

        #endregion

        #region Factor de Pérdida Marginal

        /// <summary>
        /// Permite cargar los factores de périda marginal
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public ResultadoFactorPerdida CargarFactoresPerdida(DateTime fecha)
        {
            ResultadoFactorPerdida entity = new ResultadoFactorPerdida();
            entity.FechaDatos = string.Empty;

            try
            {
                List<CmBarraRelacionDTO> barras = FactorySic.GetCmBarraRelacionRepository().GetByCriteria(fecha);
                List<CmBarraRelacionDTO> listaBarraDesconocida = barras.Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraDesconocida).ToList();
                List<CmBarraRelacionDTO> listaBarraAdicional = barras.Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraEMS &&
                    x.Cmbaretiprel == ConfiguracionReporteFinal.BarraAdicional).ToList();
                List<CmPeriodoDTO> listaPeriodo = FactorySic.GetCmPeriodoRepository().GetByCriteria(fecha);

                listaBarraDesconocida.AddRange(listaBarraAdicional);

                if (listaBarraDesconocida.Count > 0)
                {
                    if (listaPeriodo.Count > 0)
                    {
                        string[][] data = new string[listaBarraDesconocida.Count + 2][];

                        CmPeriodoDTO periodo = listaPeriodo.FirstOrDefault();
                        entity.ListaHistorico = FactorySic.GetCmFactorperdidaRepository().GetByCriteria(fecha);
                        List<CmFpmdetalleDTO> listaDetalle = FactorySic.GetCmFpmdetalleRepository().ObtenerPorFecha(fecha);

                        if (listaDetalle.Count > 0)
                        {
                            CmFpmdetalleDTO itemDetalle = listaDetalle.FirstOrDefault();
                            entity.FechaDatos = "Los datos cargados corresponden a la fecha de " + itemDetalle.FechaDatos.ToString(ConstantesAppServicio.FormatoFecha);
                        }
                        
                        string[] header1 = { "","Barra", "Factor de pérdida marginal (FPM)", "", "" };
                        string[] header2 = { "", "", "Base (" + periodo.Cmperbase + ")", "Media (" + periodo.Cmpermedia + ")", "Punta (" + periodo.Cmperpunta + ")" };

                        data[0] = header1;
                        data[1] = header2;

                        int index = 2;
                        foreach (CmBarraRelacionDTO barra in listaBarraDesconocida)
                        {
                            CmFpmdetalleDTO itemDetalle = listaDetalle.Where(x => x.Barrcodi == barra.Barrcodi).FirstOrDefault();                            

                            string pbase = string.Empty;
                            string pmedia = string.Empty;
                            string ppunta = string.Empty;

                            if (itemDetalle != null)
                            {
                                pbase = (itemDetalle.Cmfpmdbase != null) ? itemDetalle.Cmfpmdbase.ToString() : string.Empty;
                                pmedia = (itemDetalle.Cmfpmdmedia != null) ? itemDetalle.Cmfpmdmedia.ToString() : string.Empty;
                                ppunta = (itemDetalle.Cmfpmdpunta != null) ? itemDetalle.Cmfpmdpunta.ToString() : string.Empty;
                            }

                            string[] row = { barra.Barrcodi.ToString(), barra.Barrnombre, pbase, pmedia, ppunta };
                            data[index] = row;
                            index++;
                        }

                        entity.Resultado = 1;
                        entity.Datos = data;
                    }
                    else
                    {
                        entity.Resultado = 2;
                        entity.Mensaje = "No existe configuración de periodos.";
                    }
                }
                else
                {
                    entity.Resultado = 2;
                    entity.Mensaje = "No existen barras desconocidas ni adicionales.";
                }
            }
            catch (Exception ex)
            {
                entity.Resultado = -1;
            }

            return entity;
        }

        /// <summary>
        /// Permite cargar la grilla a partir del excel
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ResultadoFactorPerdida CargarFactorPerdidaFormato(string path)
        {
            ResultadoFactorPerdida entity = new ResultadoFactorPerdida();

            try 
            {
                List<string[]> result = new List<string[]>();
                FileInfo fileInfo = new FileInfo(path);
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {               
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                    int cantidad = 200;

                    for(int k = 1; k <= 2; k++)
                    {
                        string code = (ws.Cells[k, 1].Value != null) ? ws.Cells[k, 1].Value.ToString() : string.Empty;
                        string barra = (ws.Cells[k, 2].Value != null) ? ws.Cells[k, 2].Value.ToString() : string.Empty;
                        string fbase = (ws.Cells[k, 3].Value != null) ? ws.Cells[k, 3].Value.ToString() : string.Empty;
                        string fmedia = (ws.Cells[k, 4].Value != null) ? ws.Cells[k, 4].Value.ToString() : string.Empty;
                        string fpunta = (ws.Cells[k, 5].Value != null) ? ws.Cells[k, 5].Value.ToString() : string.Empty;

                        string[] data = { code, barra, fbase, fmedia, fpunta };
                        result.Add(data);
                    }

                    for (int i = 3; i <= cantidad; i++)
                    {
                        if (ws.Cells[i, 1].Value != null && ws.Cells[i, 1].Value != string.Empty)
                        {
                            string code = (ws.Cells[i, 1].Value!=null)? ws.Cells[i, 1].Value.ToString():string.Empty;
                            string barra = (ws.Cells[i, 2].Value != null) ? ws.Cells[i, 2].Value.ToString() : string.Empty;
                            string fbase = (ws.Cells[i, 3].Value != null) ? ws.Cells[i, 3].Value.ToString() : string.Empty;
                            string fmedia = (ws.Cells[i, 4].Value != null) ? ws.Cells[i, 4].Value.ToString() : string.Empty;
                            string fpunta = (ws.Cells[i, 5].Value != null) ? ws.Cells[i, 5].Value.ToString() : string.Empty;

                            string[] data = { code, barra, fbase, fmedia, fpunta };
                            result.Add(data);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                entity.Resultado = 1;
                entity.Datos = result.ToArray();
                entity.FechaDatos = string.Empty;
            }
            catch(Exception ex)
            {
                entity.Resultado = -1;
            }

            return entity;
        }

        /// <summary>
        /// Permite grabar los datos de factores de perdida
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResultadoFactorPerdida GrabarFactoresPerdida(DateTime fecha, string user, string[][] data)
        {
            ResultadoFactorPerdida entity = new ResultadoFactorPerdida();

            try 
            {
                if(data.Length > 2)
                {
                    CmFactorperdidaDTO header = new CmFactorperdidaDTO();
                    header.Cmfpmestado = ConstantesAppServicio.Activo;
                    header.Cmfpmfeccreacion = DateTime.Now;
                    header.Cmfpmfecha = fecha;
                    header.Cmfpmfecmodificacion = DateTime.Now;
                    header.Cmfpmusucreacion = user;
                    header.Cmfpmusumodificacion = user;

                    int id = FactorySic.GetCmFactorperdidaRepository().Save(header);

                    FactorySic.GetCmFpmdetalleRepository().Delete(fecha);

                    for (int i = 2; i < data.Length; i++)
                    {
                        CmFpmdetalleDTO item = new CmFpmdetalleDTO();
                        item.Cmfpmcodi = id;
                        item.Barrcodi = int.Parse(data[i][0]);
                        item.Cmfpmdbase = decimal.Parse(data[i][2]);
                        item.Cmfpmdmedia = decimal.Parse(data[i][3]);
                        item.Cmfpmdpunta = decimal.Parse(data[i][4]);
                        FactorySic.GetCmFpmdetalleRepository().Save(item);
                    }
                }

                entity.Resultado = 1;
            }
            catch(Exception ex)
            {
                entity.Resultado = -1;
            }

            return entity;
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="data"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public ResultadoFactorPerdida GenerarFormatoFPM(string[][] data, string path, string filename, DateTime fecha)
        {
            ResultadoFactorPerdida entity = new ResultadoFactorPerdida();
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DATOS");

                    if (ws != null)
                    {
                        int index = 1;
                        for(int i = 0; i< data.Length; i++)
                        {
                            ws.Cells[index, 1].Value = data[i][0];
                            ws.Cells[index, 2].Value = data[i][1];
                            ws.Cells[index, 3].Value = data[i][2];
                            ws.Cells[index, 4].Value = data[i][3];
                            ws.Cells[index, 5].Value = data[i][4];                           
                            index++;
                        }

                        ExcelRange rg = ws.Cells[1, 1, 2, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[3, 1, index - 1, 5];

                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        rg = ws.Cells[1, 1, 2, 1];
                        rg.Merge = true;

                        rg = ws.Cells[1, 2, 2, 2];
                        rg.Merge = true;

                        rg = ws.Cells[1, 3, 1, 5];
                        rg.Merge = true;

                        ws.Column(1).Width = 30;
                        rg = ws.Cells[1, 1, index - 1, 5];
                        rg.AutoFitColumns();

                    }

                    xlPackage.Save();
                }



                entity.Resultado = 1;
                entity.Fecha = fecha.ToString("ddMMyyyy");
            }
            catch(Exception ex)
            {
                entity.Resultado = -1;
            }

            return entity;
        }

        #endregion

        #region Reporte Final

        public CmCostomarginalDTO CalcularCostoMarginal(int idPeriodo, List<CmCostomarginalDTO> entitysCM, List<CmCostomarginalDTO> entitysCMAdicionales)
        {
            CmCostomarginalDTO entity = null;

            if (entitysCMAdicionales.Count == 0)
            {
                entity = entitysCM.Where(x => x.Periodo == idPeriodo).FirstOrDefault();
            }
            else 
            {
                CmCostomarginalDTO mainCM = entitysCM.Where(x => x.Periodo == idPeriodo).FirstOrDefault();
                List<CmCostomarginalDTO> entitys = entitysCMAdicionales.Where(x => x.Periodo == idPeriodo).ToList();

                if (mainCM != null)
                {
                    entitys.Add(mainCM);
                }

                if (entitys.Count > 0)
                {
                    entity = new CmCostomarginalDTO();
                    entity.Cmgnenergia = entitys.Sum(x=>x.Cmgnenergia)/entitys.Count;
                    entity.Cmgntotal = entitys.Sum(x => x.Cmgntotal) / entitys.Count;
                    entity.Cmgncongestion = entitys.Sum(x => x.Cmgncongestion) / entitys.Count;
                }
            }

            return entity;
        }

        /// <summary>
        /// Permite generar el reporte final
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public ResultadoConfiguracion GenerarReporteFinal(DateTime fecha, string username)
        {
            ResultadoConfiguracion result = new ResultadoConfiguracion();

            try
            {
                #region Lectura de insumos

                #region CMgCP_PR07
                int version = ConstantesCortoPlazo.VersionCMOriginal;
                DateTime fechaVigenciaPR07 = DateTime.ParseExact(
                    ConfigurationManager.AppSettings[ConstantesCortoPlazo.FechaVigenciaPR07], ConstantesAppServicio.FormatoFecha, 
                    CultureInfo.InvariantCulture);

                if (fecha.Subtract(fechaVigenciaPR07).TotalDays >= 0) version = ConstantesCortoPlazo.VersionCMPR07;

                List<CmCostomarginalDTO> listCostoMarginal = FactorySic.GetCmCostomarginalRepository().ObtenerReporteCostosMarginales(fecha, fecha,
                    ConstantesCortoPlazo.EstimadorTNA, ConstantesCortoPlazo.TipoMDCOES, version);

                List<int> listTIE = this.ValidarExistenciaTIE(fecha);

                #endregion

                List<CmFpmdetalleDTO> listaDetalle = FactorySic.GetCmFpmdetalleRepository().ObtenerPorFecha(fecha);

                List<CmBarraRelacionDTO> barras = FactorySic.GetCmBarraRelacionRepository().GetByCriteria(fecha);
                List<CmBarraRelacionDTO> barrasEMS = barras.Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraEMS).ToList();
                List<CmBarraRelacionDTO> barrasConocidas = barrasEMS.Where(x => x.Cmbaretiprel == ConfiguracionReporteFinal.BarraConocida).ToList();
                List<CmBarraRelacionDTO> barrasDesconocidas = barrasEMS.Where(x => x.Cmbaretiprel == ConfiguracionReporteFinal.BarraNoConocida).ToList();
                List<CmBarraRelacionDTO> barrasConocidaDesconocida = barrasEMS.Where(x => x.Cmbaretiprel == ConfiguracionReporteFinal.BarraConocidaDesconocida).ToList();

                List<CmPeriodoDTO> listaPeriodo = FactorySic.GetCmPeriodoRepository().GetByCriteria(fecha);
                CmPeriodoDTO periodo = null;

                if (listaPeriodo.Count > 0)
                {
                    periodo = listaPeriodo.FirstOrDefault();
                }

                #endregion

                #region Reporte de revisión

                List<CmReportedetalleDTO> reporteRevision = new List<CmReportedetalleDTO>();

                foreach (CmBarraRelacionDTO entity in barrasConocidas)
                {
                    List<CmCostomarginalDTO> entitysCM = listCostoMarginal.Where(x => x.Cnfbarcodi == (int)entity.Cnfbarcodi).ToList();

                    List<CmBarraRelacionDetDTO> barrasEMSAdicionales = FactorySic.GetCmBarraRelacionDetRepository().GetByCriteria(entity.Cmbarecodi);

                    List<CmCostomarginalDTO> entitysCMAdicional = listCostoMarginal.Where(x => barrasEMSAdicionales.Any(y=> x.Cnfbarcodi == y.Cnfbarcodi)).ToList();

                    for (int i = 1; i <= 48; i++)
                    {

                        CmCostomarginalDTO cmItem = this.CalcularCostoMarginal(i, entitysCM, entitysCMAdicional);

                        //CmCostomarginalDTO cmItem = entitysCM.Where(x => x.Periodo == i).FirstOrDefault();

                        CmReportedetalleDTO itemReporte = new CmReportedetalleDTO();
                        itemReporte.Barrcodi = entity.Barrcodi;
                        itemReporte.Cmredefecha = fecha;
                        itemReporte.Cmredeperiodo = i;

                        if (cmItem != null)
                        {
                            itemReporte.Cmredecmenergia = cmItem.Cmgnenergia;
                            itemReporte.Cmredecmtotal = cmItem.Cmgntotal;
                            itemReporte.Cmredecongestion = cmItem.Cmgncongestion;
                        }
                        else
                        {
                            itemReporte.Cmredevalenergia = -1;
                            itemReporte.Cmredevalcongestion = -1;
                            itemReporte.Cmredevaltotal = -1;
                        }

                        reporteRevision.Add(itemReporte);
                    }
                }

                if (periodo != null)
                {
                    foreach (CmBarraRelacionDTO entity in barrasDesconocidas)
                    {
                        List<CmCostomarginalDTO> entitysCM = listCostoMarginal.Where(x => x.Cnfbarcodi == (int)entity.Cnfbarcodi).ToList();

                        List<CmBarraRelacionDetDTO> barrasEMSAdicionales = FactorySic.GetCmBarraRelacionDetRepository().GetByCriteria(entity.Cmbarecodi);

                        List<CmCostomarginalDTO> entitysCMAdicional = listCostoMarginal.Where(x => barrasEMSAdicionales.Any(y => x.Cnfbarcodi == y.Cnfbarcodi)).ToList();

                        for (int i = 1; i <= 48; i++)
                        {
                            //CmCostomarginalDTO cmItem = entitysCM.Where(x => x.Periodo == i).FirstOrDefault();

                            CmCostomarginalDTO cmItem = this.CalcularCostoMarginal(i, entitysCM, entitysCMAdicional);

                            decimal? fpm = this.ObtenerValorFPM(periodo, listaDetalle, i, (int)entity.Barrcodi);

                            CmReportedetalleDTO itemReporte = new CmReportedetalleDTO();
                            itemReporte.Barrcodi = entity.Barrcodi;
                            itemReporte.Cmredefecha = fecha;
                            itemReporte.Cmredeperiodo = i;

                            if (cmItem != null && fpm != null)
                            {
                                if (version == ConstantesCortoPlazo.VersionCMPR07)
                                {
                                    itemReporte.Cmredecmenergia = cmItem.Cmgnenergia * fpm;
                                    itemReporte.Cmredecongestion = cmItem.Cmgncongestion;
                                    itemReporte.Cmredecmtotal = itemReporte.Cmredecmenergia + itemReporte.Cmredecongestion;
                                }
                                else
                                {
                                    itemReporte.Cmredecmtotal = cmItem.Cmgntotal * fpm;
                                    itemReporte.Cmredecongestion = cmItem.Cmgncongestion;
                                    itemReporte.Cmredecmenergia = itemReporte.Cmredecmtotal - itemReporte.Cmredecongestion;
                                }
                            }
                            else
                            {
                                itemReporte.Cmredevalenergia = -1;
                                itemReporte.Cmredevalcongestion = -1;
                                itemReporte.Cmredevaltotal = -1;
                            }

                            reporteRevision.Add(itemReporte);
                        }
                    }
                }

                #region CMgCP_PR07

                if(periodo != null)
                {
                    foreach (CmBarraRelacionDTO entity in barrasConocidaDesconocida)
                    {
                        if (entity.Cnfbarcodi != null && entity.Cnfbarcodi2 != null)
                        {

                            List<CmCostomarginalDTO> entitysCMConocida = listCostoMarginal.Where(x => x.Cnfbarcodi == (int)entity.Cnfbarcodi).ToList();
                            List<CmCostomarginalDTO> entitysCMDesconocida = listCostoMarginal.Where(x => x.Cnfbarcodi == (int)entity.Cnfbarcodi2).ToList();
                            List<CmBarraRelacionDetDTO> barrasEMSAdicionales = FactorySic.GetCmBarraRelacionDetRepository().GetByCriteria(entity.Cmbarecodi);
                            List<CmCostomarginalDTO> entitysCMAdicional = listCostoMarginal.Where(x => barrasEMSAdicionales.Any(y => x.Cnfbarcodi == y.Cnfbarcodi)).ToList();

                            for (int i = 1; i <= 48; i++)
                            {
                                int flagTIE = listTIE[i - 1];

                                if (flagTIE == 0)
                                {
                                    CmCostomarginalDTO cmItem = this.CalcularCostoMarginal(i, entitysCMConocida, entitysCMAdicional);

                                    CmReportedetalleDTO itemReporte = new CmReportedetalleDTO();
                                    itemReporte.Barrcodi = entity.Barrcodi;
                                    itemReporte.Cmredefecha = fecha;
                                    itemReporte.Cmredeperiodo = i;

                                    if (cmItem != null)
                                    {
                                        itemReporte.Cmredecmenergia = cmItem.Cmgnenergia;
                                        itemReporte.Cmredecmtotal = cmItem.Cmgntotal;
                                        itemReporte.Cmredecongestion = cmItem.Cmgncongestion;
                                    }
                                    else
                                    {
                                        itemReporte.Cmredevalenergia = -1;
                                        itemReporte.Cmredevalcongestion = -1;
                                        itemReporte.Cmredevaltotal = -1;
                                    }

                                    reporteRevision.Add(itemReporte);
                                }
                                else
                                {
                                    CmCostomarginalDTO cmItem = this.CalcularCostoMarginal(i, entitysCMDesconocida, entitysCMAdicional);

                                    decimal? fpm = this.ObtenerValorFPM(periodo, listaDetalle, i, (int)entity.Barrcodi);

                                    CmReportedetalleDTO itemReporte = new CmReportedetalleDTO();
                                    itemReporte.Barrcodi = entity.Barrcodi;
                                    itemReporte.Cmredefecha = fecha;
                                    itemReporte.Cmredeperiodo = i;

                                    if (cmItem != null && fpm != null)
                                    {
                                        if (version == ConstantesCortoPlazo.VersionCMPR07)
                                        {
                                            itemReporte.Cmredecmenergia = cmItem.Cmgnenergia * fpm;
                                            itemReporte.Cmredecongestion = cmItem.Cmgncongestion;
                                            itemReporte.Cmredecmtotal = itemReporte.Cmredecmenergia + itemReporte.Cmredecongestion;
                                        }
                                        else
                                        {
                                            itemReporte.Cmredecmtotal = cmItem.Cmgntotal * fpm;
                                            itemReporte.Cmredecongestion = cmItem.Cmgncongestion;
                                            itemReporte.Cmredecmenergia = itemReporte.Cmredecmtotal - itemReporte.Cmredecongestion;
                                        }
                                    }
                                    else
                                    {
                                        itemReporte.Cmredevalenergia = -1;
                                        itemReporte.Cmredevalcongestion = -1;
                                        itemReporte.Cmredevaltotal = -1;
                                    }

                                    reporteRevision.Add(itemReporte);
                                }
                            }
                        }
                    }
                }

                #endregion

                //- Debemos mandar a grabar

                #endregion

                #region Reporte final

                List<CmReportedetalleDTO> reporteRevisionCopia = reporteRevision.Select(x => new CmReportedetalleDTO(x)).ToList();// new List<CmReportedetalleDTO>(reporteRevision);

                List<CmReportedetalleDTO> reporteFinal = reporteRevision.Select(x => new CmReportedetalleDTO(x)).ToList(); //new List<CmReportedetalleDTO>(reporteRevision);

                List<CmBarraRelacionDTO> barrasSTR = barras.Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraTransferencia).ToList();

                foreach (CmReportedetalleDTO item in reporteFinal)
                {
                    if (item.Cmredevaltotal == -1)
                    {
                        CmReportedetalleDTO cmReemplazo = this.BuscarCMRecursivo(barrasSTR, reporteRevisionCopia, (int)item.Barrcodi, (int)item.Cmredeperiodo);

                        if (cmReemplazo != null)
                        {
                            item.Cmredecmtotal = cmReemplazo.Cmredecmtotal;
                            item.Cmredecmenergia = cmReemplazo.Cmredecmenergia;
                            item.Cmredecongestion = cmReemplazo.Cmredecongestion;
                            item.Barrcodi2 = cmReemplazo.Barrcodi;
                            item.Cmredevaltotal = null;
                            item.Cmredevalenergia = null;
                            item.Cmredevalcongestion = null;
                            
                        }
                    }
                }

                //- Verificamos las congestiones
                List<CmEquipobarraDTO> listEquipos = FactorySic.GetCmEquipobarraRepository().GetByCriteria(fecha);
                List<CmBarraRelacionDTO> barrasAdicionales = barrasEMS.Where(x => x.Cmbaretiprel == ConfiguracionReporteFinal.BarraAdicional).ToList();

                foreach (CmEquipobarraDTO entity in listEquipos)
                {
                    bool flag = FactorySic.GetPrCongestionRepository().VerificarExistenciaCongestion((int)entity.Configcodi, fecha);

                    if (flag)
                    {
                        List<PrCongestionDTO> listCongestionEquipo = FactorySic.GetPrCongestionRepository().ObtenerCongestion(fecha, fecha).Where(x => x.Configcodi == (int)entity.Configcodi).ToList();
                        entity.ListaDetalle = FactorySic.GetCmEquipobarraDetRepository().GetByCriteria(entity.Cmeqbacodi);

                        foreach (CmEquipobarraDetDTO barraDetalle in entity.ListaDetalle)
                        {
                            CmBarraRelacionDTO barraRelacion = barrasAdicionales.Where(x => x.Barrcodi == barraDetalle.Barrcodi).FirstOrDefault();

                            if (barraRelacion != null)
                            {
                                List<CmCostomarginalDTO> entitysCM = listCostoMarginal.Where(x => x.Cnfbarcodi == (int)barraRelacion.Cnfbarcodi).ToList();

                                for (int i = 1; i <= 48; i++)
                                {
                                    bool validCongestion = this.ValidarCongestionPeriodo(listCongestionEquipo, i, fecha);

                                    if (validCongestion)
                                    {

                                        CmCostomarginalDTO cmItem = entitysCM.Where(x => x.Periodo == i).FirstOrDefault();

                                        decimal? fpm = this.ObtenerValorFPM(periodo, listaDetalle, i, (int)barraDetalle.Barrcodi);

                                        CmReportedetalleDTO itemReporte = new CmReportedetalleDTO();

                                        itemReporte.Barrcodi = barraDetalle.Barrcodi;
                                        itemReporte.Cmredefecha = fecha;
                                        itemReporte.Cmredeperiodo = i;

                                        if (cmItem != null && fpm != null)
                                        {
                                            if (version == ConstantesCortoPlazo.VersionCMPR07)
                                            {
                                                itemReporte.Cmredecmenergia = cmItem.Cmgnenergia * fpm;
                                                itemReporte.Cmredecongestion = cmItem.Cmgncongestion;
                                                itemReporte.Cmredecmtotal = itemReporte.Cmredecmenergia + itemReporte.Cmredecongestion;
                                            }
                                            else 
                                            {
                                                itemReporte.Cmredecmtotal = cmItem.Cmgntotal * fpm;
                                                itemReporte.Cmredecongestion = cmItem.Cmgncongestion;
                                                itemReporte.Cmredecmenergia = itemReporte.Cmredecmtotal - itemReporte.Cmredecongestion;
                                            }                                            
                                        }
                                        else
                                        {
                                            itemReporte.Cmredevalenergia = -1;
                                            itemReporte.Cmredevalcongestion = -1;
                                            itemReporte.Cmredevaltotal = -1;
                                        }
                                        reporteFinal.Add(itemReporte);

                                    }
                                    else 
                                    {
                                        CmReportedetalleDTO itemReporte = new CmReportedetalleDTO();
                                        itemReporte.Barrcodi = barraDetalle.Barrcodi;
                                        itemReporte.Cmredefecha = fecha;
                                        itemReporte.Cmredeperiodo = i;
                                        reporteFinal.Add(itemReporte);
                                    }
                                    
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Reporte de verificación

                List<CmReportedetalleDTO> reporteValidacion = new List<CmReportedetalleDTO>();
                List<CmUmbralreporteDTO> umbrales = FactorySic.GetCmUmbralreporteRepository().GetByCriteria(fecha);

                if (umbrales.Count() > 0)
                {
                    CmUmbralreporteDTO umbral = umbrales.FirstOrDefault();

                    foreach (CmReportedetalleDTO itemFinal in reporteFinal)
                    {
                        if (itemFinal.Cmredevaltotal != -1)
                        {
                            if (itemFinal.Cmredecmtotal != null)                            
                                itemFinal.Cmredecmtotal = (decimal)itemFinal.Cmredecmtotal;
                            if (itemFinal.Cmredecmenergia != null)
                                itemFinal.Cmredecmenergia = (decimal)itemFinal.Cmredecmenergia;
                            if (itemFinal.Cmredecongestion != null)
                                itemFinal.Cmredecongestion = (decimal)itemFinal.Cmredecongestion;

                            if (!(itemFinal.Cmredecmtotal >= umbral.Cmurminbarra && itemFinal.Cmredecmtotal <= umbral.Cmurmaxbarra))
                            {
                                CmReportedetalleDTO itemTotal = new CmReportedetalleDTO();
                                itemTotal.Cmredeperiodo = itemFinal.Cmredeperiodo;
                                itemTotal.Cmredefecha = itemFinal.Cmredefecha;
                                itemTotal.Barrcodi = itemFinal.Barrcodi;
                                itemTotal.Cmredevaltotal = 2;
                                reporteValidacion.Add(itemTotal);
                            }
                            if (!(itemFinal.Cmredecmenergia >= umbral.Cmurminenergia && itemFinal.Cmredecmenergia <= umbral.Cmurmaxenergia))
                            {
                                CmReportedetalleDTO itemEnergia = new CmReportedetalleDTO();
                                itemEnergia.Cmredeperiodo = itemFinal.Cmredeperiodo;
                                itemEnergia.Cmredefecha = itemFinal.Cmredefecha;
                                itemEnergia.Barrcodi = itemFinal.Barrcodi;
                                itemEnergia.Cmredevalenergia = 2;
                                reporteValidacion.Add(itemEnergia);
                            }
                            if (!(itemFinal.Cmredecongestion >= umbral.Cmurminconges && itemFinal.Cmredecongestion <= umbral.Cmurmaxconges))
                            {
                                CmReportedetalleDTO itemCongestion = new CmReportedetalleDTO();
                                itemCongestion.Cmredeperiodo = itemFinal.Cmredeperiodo;
                                itemCongestion.Cmredefecha = itemFinal.Cmredefecha;
                                itemCongestion.Barrcodi = itemFinal.Barrcodi;
                                itemCongestion.Cmredevalcongestion = 2;
                                reporteValidacion.Add(itemCongestion);
                            }

                            if (itemFinal.Cmredecmtotal - itemFinal.Cmredecmenergia - itemFinal.Cmredecongestion > umbral.Cmurdiferencia)
                            {
                                CmReportedetalleDTO itemDiferencia = new CmReportedetalleDTO();
                                itemDiferencia.Cmredeperiodo = itemFinal.Cmredeperiodo;
                                itemDiferencia.Cmredefecha = itemFinal.Cmredefecha;
                                itemDiferencia.Barrcodi = itemFinal.Barrcodi;
                                itemDiferencia.Cmredevaltotal = 3;
                                reporteValidacion.Add(itemDiferencia);
                            }
                        }
                    }
                }

                #endregion

                CmReporteDTO reporte = new CmReporteDTO();
                reporte.Cmrepfecha = fecha;
                reporte.Cmurcodi = (umbrales.Count() > 0) ? (int?)umbrales.FirstOrDefault().Cmurcodi : null;
                reporte.Cmpercodi = (listaPeriodo.Count > 0) ? (int?)listaPeriodo.FirstOrDefault().Cmpercodi : null;
                reporte.Cmrepestado = ConstantesAppServicio.Activo;
                reporte.Cmrepfeccreacion = DateTime.Now;
                reporte.Cmrepusucreacion = username;
                reporte.Cmrepfecmodificacion = DateTime.Now;
                reporte.Cmrepusumodificacion = username;
                reporte.Cmrepversion = FactorySic.GetCmReporteRepository().ObtenerNroVersion(fecha);

                int idReporte = FactorySic.GetCmReporteRepository().Save(reporte);

                this.AlmacenarReporteDetalle(idReporte, 1, reporteRevision);
                this.AlmacenarReporteDetalle(idReporte, 2, reporteFinal);
                this.AlmacenarReporteDetalle(idReporte, 3, reporteValidacion);

                result.Resultado = 1;
            }
            catch(Exception ex)
            {
                result.Resultado = -1;
            }

            return result;
        }

        /// <summary>
        /// Permite validar la existencia de TIE por cada media hora
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<int> ValidarExistenciaTIE(DateTime fecha)
        {
            List<int> indexes = new List<int>();

            for (int i = 0; i < 48; i++)
            {
                indexes.Add(0);
            }

            List<EveIeodcuadroDTO> listTIE = (new ReprocesoAppServicio()).ObtenerTransacciones(fecha);

            foreach (EveIeodcuadroDTO entity in listTIE)
            {
                DateTime fechaIni = (DateTime)entity.Ichorini;
                DateTime fechaFin = (DateTime)entity.Ichorfin;
                int periodoInicial = Convert.ToInt32(Math.Ceiling(((decimal)(fechaIni.Hour * 60 + fechaIni.Minute) / 30.0M)));
                int periodoFinal = Convert.ToInt32(Math.Floor(((decimal)(fechaFin.Hour * 60 + fechaFin.Minute) / 30.0M)));

                if (fechaIni.Hour == 0 && fechaIni.Minute == 0) periodoInicial = 1;
                if (fechaFin.Year == fecha.Year && fechaFin.Month == fecha.Month && fechaFin.Day == fecha.Day + 1) periodoFinal = 48;

                for (int i = periodoInicial; i <= periodoFinal; i++)
                {
                    indexes[i - 1] = 1;
                }
            }

            return indexes;
        }

        /// <summary>
        /// Permite validar si hay congestión en el periodo
        /// </summary>
        /// <param name="list"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public bool ValidarCongestionPeriodo(List<PrCongestionDTO> list, int periodo, DateTime fecha)
        {
            foreach (PrCongestionDTO item in list)
            {
                DateTime fecIni = (DateTime)item.Congesfecinicio;
                int periodoInicio = UtilCortoPlazoTna.CalcularPeriodo((DateTime)item.Congesfecinicio);
                int periodoFin = UtilCortoPlazoTna.CalcularPeriodo((DateTime)item.Congesfecfin);
                if (!(((DateTime)item.Congesfecfin).Minute == 0 || ((DateTime)item.Congesfecfin).Minute == 30)) periodoFin = periodoFin - 1;

                if (periodoFin == 0) periodoFin = 48;

                if (periodo >= periodoInicio && periodo <= periodoFin)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Permite almacenar el detalle del reporte
        /// </summary>
        /// <param name="idReporte"></param>
        /// <param name="listDetalle"></param>
        protected void AlmacenarReporteDetalle(int idReporte, int tipo, List<CmReportedetalleDTO> listDetalle)
        {
            int id = FactorySic.GetCmReportedetalleRepository().ObtenerMaxId();
            foreach (CmReportedetalleDTO item in listDetalle)
            {
                item.Cmrepcodi = idReporte;
                item.Cmredecodi = id;
                item.Cmredetiporeporte = tipo.ToString();
                id++;
                //FactorySic.GetCmReportedetalleRepository().Save(item);
            }
            FactorySic.GetCmReportedetalleRepository().GrabarDatosBulkResult(listDetalle);
        }

        /// <summary>
        /// Permite obtener el factor de pérdida marginal
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="detalle"></param>
        /// <param name="index"></param>
        /// <param name="barracodi"></param>
        /// <returns></returns>
        public decimal? ObtenerValorFPM(CmPeriodoDTO periodo, List<CmFpmdetalleDTO> detalle, int index, int barracodi)
        {
            decimal? valor = null;
            string[] horas = { "00:30", "01:00" , "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30",
                               "06:00" , "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30",
                               "11:00" , "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30",
                               "16:00" , "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30",
                               "21:00" , "21:30", "22:00", "22:30", "23:00", "23:30", "23:59"};


            string[] bloques = periodo.Cmperbase.Split('-');
            int inicio = horas.ToList().IndexOf(bloques[0]) + 1;
            int fin = horas.ToList().IndexOf(bloques[1]) + 1;

            if (index >= inicio && index <= fin)
            {
                CmFpmdetalleDTO item = detalle.Where(x => x.Barrcodi == barracodi).FirstOrDefault();
                if (item != null) valor = item.Cmfpmdbase;
            }
            else {
                bloques = periodo.Cmpermedia.Split('-');
                inicio = horas.ToList().IndexOf(bloques[0]) + 1;
                fin = horas.ToList().IndexOf(bloques[1]) + 1;

                if (index >= inicio && index <= fin)
                {
                    CmFpmdetalleDTO item = detalle.Where(x => x.Barrcodi == barracodi).FirstOrDefault();
                    if (item != null) valor = item.Cmfpmdmedia;
                }
                else
                {
                    bloques = periodo.Cmperpunta.Split('-');
                    inicio = horas.ToList().IndexOf(bloques[0]) + 1;
                    fin = horas.ToList().IndexOf(bloques[1]) + 1;

                    if (index >= inicio && index <= fin)
                    {
                        CmFpmdetalleDTO item = detalle.Where(x => x.Barrcodi == barracodi).FirstOrDefault();
                        if (item != null) valor = item.Cmfpmdpunta;
                    }
                }
            }

            return valor;
        }

        /// <summary>
        /// Permite obtener el costo marginal de la barras cercanas
        /// </summary>
        /// <param name="barrasSTR"></param>
        /// <param name="reporteRevision"></param>
        /// <param name="idBarra"></param>
        /// <returns></returns>
        public CmReportedetalleDTO BuscarCMRecursivo(List<CmBarraRelacionDTO> barrasSTR, List<CmReportedetalleDTO> reporteRevision, int idBarra, int idPeriodo)
        {
            if (barrasSTR.Where(x => x.Barrcodi == idBarra).Count() > 0)
            {
                int barra2 = (int)barrasSTR.Where(x => x.Barrcodi == idBarra).FirstOrDefault().Barrcodi2;

                if (reporteRevision.Where(x => x.Barrcodi == barra2).Count() > 0) 
                {
                    CmReportedetalleDTO entity = reporteRevision.Where(x => x.Barrcodi == barra2 && x.Cmredeperiodo == idPeriodo).FirstOrDefault();

                    if (entity.Cmredevaltotal != -1)
                    {
                        return entity;
                    }
                    else
                    {
                        return this.BuscarCMRecursivo(barrasSTR, reporteRevision, barra2, idPeriodo);
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Permite obtener los reportes en el rango de fechas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<CmReporteDTO> ObtenerBusquedaReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetCmReporteRepository().GetByCriteria(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite generar los archivos de los reportes
        /// </summary>
        /// <param name="idReporte"></param>
        /// <param name="tipo"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public ResultadoConfiguracion GenerarArchivoReporte(DateTime fecha, int idReporte, int tipo, string path)
        {
            ResultadoConfiguracion result = new ResultadoConfiguracion();

            try
            {
                string file = string.Empty;
                CmReporteDTO reporte = FactorySic.GetCmReporteRepository().GetById(idReporte);
                fecha = (DateTime)reporte.Cmrepfecha;
                List<CmReportedetalleDTO> listDetalle = FactorySic.GetCmReportedetalleRepository().GetByCriteria(idReporte, tipo.ToString());
                #region Ticket_6245
                List<CmBarraRelacionDTO> barras = FactorySic.GetCmBarraRelacionRepository().GetByCriteria(fecha)
                        .Where(x => x.Cmbaretipreg == ConfiguracionReporteFinal.BarraEMS && x.Cmbarereporte == ConstantesAppServicio.SI).ToList();

                #endregion

                if (barras.Count > 0)
                {


                    if (tipo == 1)
                    {
                        file = string.Format("CMgsTR_{0}.xlsx", fecha.ToString("yyyy.MM.dd"));

                        string rutaFile = path + file;
                        FileInfo newFile = new FileInfo(rutaFile);

                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(rutaFile);
                        }

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            GenerarHojaCostoMarginal(xlPackage, "Formato_Total", fecha, listDetalle, barras, 1);
                            xlPackage.Save();

                            GenerarHojaCostoMarginal(xlPackage, "Formato_Energía", fecha, listDetalle, barras, 2);
                            xlPackage.Save();

                            GenerarHojaCostoMarginal(xlPackage, "Formato_Congestión", fecha, listDetalle, barras, 3);
                            xlPackage.Save();
                        }
                    }
                    else if (tipo == 2)
                    {
                        file = string.Format("CMgCP_{0}.xlsx", fecha.ToString("yyyy.MM.dd"));

                        string rutaFile = path + file;
                        FileInfo newFile = new FileInfo(rutaFile);

                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(rutaFile);
                        }

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            bool flagFS = false;
                            GenerarHojaCostoMarginalFinal(xlPackage, "Formato_Total", fecha, listDetalle, barras, 1, out flagFS);
                            xlPackage.Save();

                            GenerarHojaCostoMarginalFinal(xlPackage, "Formato_Energía", fecha, listDetalle, barras, 2, out flagFS);
                            xlPackage.Save();

                            GenerarHojaCostoMarginalFinal(xlPackage, "Formato_Congestión", fecha, listDetalle, barras, 3, out flagFS);

                            if (flagFS) result.IndicadorFS = 1;
                            xlPackage.Save();
                        }
                    }
                    else
                    {
                        file = string.Format("VerificaciónCMgCP_{0}.xlsx", fecha.ToString("yyyy.MM.dd"));
                        this.GenerarReporteVerificacion(listDetalle, fecha, path, file);
                    }

                    result.Resultado = 1;
                    result.Filename = file;
                }
                else 
                {
                    result.Resultado = 2;
                }
            }
            catch (Exception)
            {
                result.Resultado = -1;
            }

            return result;
        }

        /// <summary>
        /// Generar detalle por modo de operación
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="obj"></param>
        /// <param name="fechaConsulta"></param>
        private void GenerarHojaCostoMarginal(ExcelPackage xlPackage, string nameWS, DateTime fecha, List<CmReportedetalleDTO> detalle,
            List<CmBarraRelacionDTO> barras, int tipo)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Cells[1, 1].Value = "EMS";
            ws.Cells[2, 1].Value = "S/./MWh";

            var listBarras = detalle.Select(x => new { x.Barrcodi, x.Barrnombre }).Distinct().OrderBy(x => x.Barrnombre).ToList();

            int col = 2;
            foreach (var item in listBarras)
            {
                string ems = string.Empty;
                CmBarraRelacionDTO itemBarra = barras.Where(x => x.Barrcodi == item.Barrcodi).FirstOrDefault();
                if (itemBarra != null) ems = itemBarra.Cnfbarnombre;

                ws.Cells[1, col].Value = ems;
                ws.Cells[2, col].Value = item.Barrnombre;
                col++;
            }           

            ExcelRange rg = ws.Cells[1, 1, 2, col - 1];
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
            rg.Style.Font.Color.SetColor(Color.White);
            rg.Style.Font.Size = 10;
            rg.Style.Font.Bold = true;

            int index = 3;
            col = 2;
            for (int i = 1; i <= 48; i++)
            {
                string periodo = fecha.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoHora);
                if (i == 48) periodo = "23:59";

                ws.Cells[index, 1].Value = fecha.ToString(ConstantesAppServicio.FormatoFecha) + " " + periodo;
                col = 2;
                foreach (var item in listBarras)
                {
                    CmReportedetalleDTO dat = detalle.Where(x => x.Barrcodi == item.Barrcodi && x.Cmredeperiodo == i).FirstOrDefault();

                    if (dat != null)
                    {
                        string valor = string.Empty;
                        decimal? val = null;
                        bool flag = false;
                        if (tipo == 1)
                        {
                            if (dat.Cmredevaltotal == -1)
                            {
                                valor = "FS";
                                flag = true;
                            }
                            else
                                val = dat.Cmredecmtotal;
                        }
                        else if (tipo == 2)
                        {
                            if (dat.Cmredevalenergia == -1)
                            {
                                valor = "FS";
                                flag = true;
                            }
                            else
                                val = dat.Cmredecmenergia;
                        }
                        else if (tipo == 3)
                        {
                            if (dat.Cmredevalcongestion == -1)
                            {
                                valor = "FS";
                                flag = true;
                            }
                            else
                                val = dat.Cmredecongestion;
                        }                        

                        if (flag)
                        {
                            ws.Cells[index, col].Value = valor;
                            rg = ws.Cells[index, col, index, col];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(Color.Red);
                        }
                        else 
                        {
                            ws.Cells[index, col].Value = val;
                            rg = ws.Cells[index, col, index, col];
                            rg.Style.Numberformat.Format = "#,##0.00";
                        }
                    }

                    col++;
                }

                index++;
            }

            for (int i = 1; i < col; i++)
            {
                ws.Column(i).Width = 20;
            }

            rg = ws.Cells[1, 1, index - 1, col - 1];
            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Font.Size = 10;
           
        }

        /// <summary>
        /// Generar detalle por modo de operación
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="obj"></param>
        /// <param name="fechaConsulta"></param>
        private void GenerarHojaCostoMarginalFinal(ExcelPackage xlPackage, string nameWS, DateTime fecha, List<CmReportedetalleDTO> detalle,
            List<CmBarraRelacionDTO> barras, int tipo, out bool flagFS)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            flagFS = false;

            ws.Cells[3, 2].Value = "S/./MWh";

            var listBarras = detalle.Select(x => new { x.Barrcodi, x.Barrnombre }).Distinct().OrderBy(x => x.Barrnombre).ToList();

            //- Jugada para ordenamiento de barras de transferencia
            List<CmBarraRelacionDTO> listaBarraOrdenada = new List<CmBarraRelacionDTO>();
            foreach (var item in listBarras)
            {
                CmBarraRelacionDTO itemBarraDTO = new CmBarraRelacionDTO();
                itemBarraDTO.Barrcodi = item.Barrcodi;
                itemBarraDTO.Barrnombre = item.Barrnombre;
                string tipoRel = string.Empty;
                CmBarraRelacionDTO itemBarra = barras.Where(x => x.Barrcodi == item.Barrcodi).FirstOrDefault();
                if (itemBarra != null)
                {
                    tipoRel = itemBarra.Cmbaretiprel;

                    if (tipoRel == 0.ToString() || tipoRel == 1.ToString() || tipoRel == 3.ToString())
                    {
                        itemBarraDTO.Barrcodi2 = 0;
                    }
                    else
                    {
                        itemBarraDTO.Barrcodi2 = 1;
                    }

                    if (itemBarra.Cmbarereporte == ConstantesAppServicio.SI)
                    listaBarraOrdenada.Add(itemBarraDTO);
                }

                
            }

            var list = listaBarraOrdenada.OrderBy(x => x.Barrcodi2).ThenBy(x => x.Barrnombre).ToList();


            int col = 3;
            foreach (var item in list)
            {               
                ws.Cells[3, col].Value = item.Barrnombre;
                col++;
            }

            ExcelRange rg = ws.Cells[3, 2, 3, col - 1];
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
            rg.Style.Font.Color.SetColor(Color.White);
            rg.Style.Font.Size = 10;
            rg.Style.Font.Bold = true;

            int index = 4;
            col = 3;
            for (int i = 1; i <= 48; i++)
            {
                string periodo = fecha.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoHora);
                if (i == 48) periodo = "23:59";

                ws.Cells[index, 2].Value = fecha.ToString(ConstantesAppServicio.FormatoFecha) + " " + periodo;

                rg = ws.Cells[index, 2, index, 2];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BDD7EE"));
                rg.Style.Font.Name = "Arial";
                rg.Style.Font.Size = 8;               

                col = 3;
                foreach (var item in list)
                {
                    CmReportedetalleDTO dat = detalle.Where(x => x.Barrcodi == item.Barrcodi && x.Cmredeperiodo == i).FirstOrDefault();

                    if (dat != null)
                    {
                        string valor = string.Empty;
                        decimal? val = null; 
                        bool flag = false;
                        if (tipo == 1)
                        {
                            if (dat.Cmredevaltotal == -1)
                            {
                                valor = "FS";
                                flag = true;
                            }
                            else
                                val = dat.Cmredecmtotal;
                        }
                        else if (tipo == 2)
                        {
                            if (dat.Cmredevalenergia == -1)
                            {
                                valor = "FS";
                                flag = true;
                            }
                            else
                                val = dat.Cmredecmenergia;
                        }
                        else if (tipo == 3)
                        {
                            if (dat.Cmredevalcongestion == -1)
                            {
                                valor = "FS";
                                flag = true;
                            }
                            else
                                val = dat.Cmredecongestion;
                        }                       

                        if (flag)
                        {
                            ws.Cells[index, col].Value = valor;
                            rg = ws.Cells[index, col, index, col];
                            rg.Style.Font.Size = 10;
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Font.Color.SetColor(Color.Red);
                            rg.Style.Font.Name = "Arial";
                            flagFS = true;
                        }
                        else
                        {
                            ws.Cells[index, col].Value = val;
                            rg = ws.Cells[index, col, index, col];
                            rg.Style.Font.Name = "Arial";
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Numberformat.Format = "#,##0.00";
                        }
                    }

                    col++;
                }

                index++;
            }

            for (int i = 2; i < col; i++)
            {
                ws.Column(i).Width = 20;
            }

            rg = ws.Cells[3, 2, index - 1, col - 1];
            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Font.Size = 8;


            ws.Cells[index + 1, 2].Value = "Nota:";
            rg = ws.Cells[index + 1, 2, index + 1, 2];
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BDD7EE"));
            rg.Style.Font.Name = "Arial";
            rg.Style.Font.Size = 8;
            rg.Style.Font.Bold = true;

            rg = ws.Cells[index + 1, 3, index + 1, 11];
            rg.Merge = true;

            rg = ws.Cells[index + 1, 2, index + 1, 11];          
            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Font.Size = 8;

            ws.View.FreezePanes(4, 3);
           
        }

        /// <summary>
        /// Permite obtener el libro de verificación
        /// </summary>
        /// <param name="detalle"></param>
        /// <param name="barras"></param>
        /// <param name="fecha"></param>
        protected void GenerarReporteVerificacion (List<CmReportedetalleDTO> detalle, DateTime fecha, string path, string filename)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE VERIFICACIÓN DE CMGCP FINAL DEL " + fecha.ToString("dd/MM/yyyy");

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;
                        
                        ws.Cells[index, 2].Value = "COMPONENTE";
                        ws.Cells[index, 3].Value = "BARRA";
                        ws.Cells[index, 4].Value = "PERIODO";
                        //ws.Cells[index, 5].Value = "VALOR";
                        ws.Cells[index, 5].Value = "INCUMPLIMIENTO";

                        rg = ws.Cells[index, 2, index, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (CmReportedetalleDTO item in detalle)
                        {
                            string componente = string.Empty;
                            string mensaje = string.Empty;
                            string periodo = fecha.AddMinutes(((int)item.Cmredeperiodo) * 30).ToString(ConstantesAppServicio.FormatoHora);
                            if ((int)item.Cmredeperiodo == 48) periodo = "23:59";
                            string barra = item.Barrnombre;
                            string valor = string.Empty;

                            if(item.Cmredevaltotal == 2)
                            {
                                componente = "CM Total";
                                mensaje = "El CMgCP Total se encuentra fuera de los umbrales permitidos.";
                                valor = item.Cmredecmtotal.ToString();
                            }
                            else if (item.Cmredevalenergia == 2)
                            {
                                componente = "CM Energía";
                                mensaje = "El CMgCP Energía se encuentra fuera de los umbrales permitidos.";
                                valor = item.Cmredecmenergia.ToString();
                            }
                            else if (item.Cmredevalcongestion == 2)
                            {
                                componente = "CM Congestión";
                                mensaje = "El CMgCP Congestión se encuentra fuera de los umbrales permitidos.";
                                valor = item.Cmredecongestion.ToString();
                            }
                            else if (item.Cmredevaltotal == 3)
                            {
                                componente = "CM Total";
                                mensaje = "El CMgCP Total - CMgCP Energía - CMgCP Congestión supera el umbral permitido.";
                                valor = (item.Cmredecmtotal - item.Cmredecmenergia - item.Cmredecongestion).ToString();
                            }

                            ws.Cells[index, 2].Value = componente;
                            ws.Cells[index, 3].Value = barra;
                            ws.Cells[index, 4].Value = periodo;
                            //ws.Cells[index, 5].Value = valor;
                            ws.Cells[index, 5].Value = mensaje;

                            rg = ws.Cells[index, 2, index, 5];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));                           

                            //rg = ws.Cells[index, 5, index, 5];
                            //rg.Style.Numberformat.Format = "#,##0.00";

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 5];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));


                        ws.Column(2).Width = 30;
                        rg = ws.Cells[5, 3, index, 5];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el reporte de barras desenergizadas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public List<ReporteBarraDesenergizada> ObtenerDataReporteBarra(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteBarraDesenergizada> result = new List<ReporteBarraDesenergizada>();
            List<CmReportedetalleDTO> entitys = FactorySic.GetCmReportedetalleRepository().ObtenerReporte(fechaInicio, fechaFin);

            int dias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

            for(int i = 0; i <= dias; i++)
            {
                DateTime fecha = fechaInicio.AddDays(i);

                List<CmReportedetalleDTO> list = entitys.Where(x => x.Cmredefecha == fecha).ToList();

                if(list.Count > 0)
                {              
                    var listBarra = list.Select(x => new { Barrcodi = x.Barrcodi, Barrnombre = x.Barrnombre }).Distinct().ToList();

                    foreach(var barra in listBarra)
                    {
                        List<CmReportedetalleDTO> detalle = list.Where(x => x.Barrcodi == barra.Barrcodi).OrderBy(x => x.Cmredeperiodo).ToList();

                        int periodo = (int)detalle[0].Cmredeperiodo;                       
                        int bloque = 1;
                        detalle[0].Bloque = bloque;
                            

                        if (detalle.Count > 0)
                        {
                            for (int k = 1; k < detalle.Count; k++)
                            {
                                if (periodo + 1 == (int)detalle[k].Cmredeperiodo)
                                {
                                    periodo = periodo + 1;
                                }
                                else 
                                {                                  
                                    periodo = (int)detalle[k].Cmredeperiodo;
                                    bloque = bloque + 1;
                                }
                                detalle[k].Bloque = bloque;
                            }
                        }

                        List<int> bloques = detalle.Select(x => x.Bloque).Distinct().ToList();

                        foreach (int id in bloques)
                        {
                            List<CmReportedetalleDTO> data = detalle.Where(x=>x.Bloque == id).ToList().OrderBy(x=>x.Cmredeperiodo).ToList();

                            int? indicador = data[0].Cmredevaltotal;
                            int? idBarra = data[0].Barrcodi2;                          
                            int subBloque = 1;

                            for(int k = 0; k < data.Count; k++)
                            {
                                if (indicador == data[k].Cmredevaltotal && idBarra == data[k].Barrcodi2)
                                {                                    
                                    data[k].SubBloque = subBloque;
                                }
                                else 
                                {                                    
                                    indicador = data[k].Cmredevaltotal;                                    
                                    idBarra = data[k].Barrcodi2;
                                    subBloque = subBloque + 1;
                                    data[k].SubBloque = subBloque;
                                }
                            }

                            List<int> subBloques = data.Select(x => x.SubBloque).Distinct().ToList();

                            foreach (int itemSubBloque in subBloques)
                            {
                                List<CmReportedetalleDTO> dataSubBloque = data.Where(x => x.SubBloque == itemSubBloque).ToList().OrderBy(x => x.Cmredeperiodo).ToList();

                                ReporteBarraDesenergizada itemReporte = new ReporteBarraDesenergizada();
                                itemReporte.Fecha = fecha;
                                itemReporte.Inicio = this.ObtenerPeriodo((int)dataSubBloque[0].Cmredeperiodo);
                                itemReporte.Fin = this.ObtenerPeriodo((int)dataSubBloque[dataSubBloque.Count - 1].Cmredeperiodo);
                                itemReporte.BarraDesenergizada = dataSubBloque[0].Barrnombre;

                                string barraReferencia = string.Empty;
                                if (dataSubBloque[0].Cmredevaltotal == -1) barraReferencia = "FS";
                                else if (dataSubBloque[0].Barrcodi2 != null) barraReferencia = dataSubBloque[0].Barrnombre2;

                                itemReporte.BarraReferencia = barraReferencia;

                                result.Add(itemReporte);
                            }
                            
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Permite generar el reporte de barras desenergizadas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public int GenerarReporteBarraDesenergizada(DateTime fechaInicio, DateTime fechaFin, string path, string fileName)
        {
            int result = 0;

            try
            {
                result = 1;
                List<ReporteBarraDesenergizada> list = this.ObtenerDataReporteBarra(fechaInicio, fechaFin);
                string rutaFile = path + fileName;
                FileInfo newFile = new FileInfo(rutaFile);


                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(rutaFile);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("BARRAS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE BARRAS DESENERGIZADAS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "DIA";
                        ws.Cells[index, 3].Value = "PERIODO INICIAL";
                        ws.Cells[index, 4].Value = "PERIODO FINAL";
                        ws.Cells[index, 5].Value = "BARRA DESENERGIZADA";
                        ws.Cells[index, 6].Value = "BARRA DE REFERENCIA PARA EL CMgCP";


                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (ReporteBarraDesenergizada item in list)
                        {
                            ws.Cells[index, 2].Value = item.Fecha.ToString("dd/MM/yyyy HH:mm");
                            ws.Cells[index, 3].Value = item.Inicio;
                            ws.Cells[index, 4].Value = item.Fin;
                            ws.Cells[index, 5].Value = item.BarraDesenergizada;
                            ws.Cells[index, 6].Value = item.BarraReferencia;

                            if(item.BarraReferencia == "FS")
                            {
                                result = 2;
                            }


                            rg = ws.Cells[index, 2, index, 6];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 6];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 6];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
            }
            catch
            {
                result = -1;
            }

            return result;
        }
        
        /// <summary>
        /// Permite obtener los periodos 
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public string ObtenerPeriodo(int indice)
        {
            string[] periodos = { "00:30", "01:00", "01:30", "02:00", "02:30" , "03:00", "03:30" , "04:00", "04:30" , "05:00", "05:30", "06:00", "06:30" , "07:00", "07:30" , "08:00", "08:30" , "09:00", "09:30",
                                 "10:00", "10:30", "11:00", "11:30", "12:00", "12:30" , "13:00", "13:30" , "14:00", "14:30" , "15:00", "15:30", "16:00", "16:30" , "17:00", "17:30" , "18:00", "18:30" , "19:00", "19:30",
                                 "20:00", "20:30", "21:00", "21:30", "22:00", "22:30" , "23:00", "23:30" , "23:59" };
            return periodos[indice - 1];
        }

        #endregion
    }

    /// <summary>
    /// Estructura para el pintado de parámetros
    /// </summary>
    public class ConfiguracionReporteFinal
    {
        public const string BarraDesconocida = "0";
        public const string BarraEMS = "1";
        public const string BarraTransferencia = "2";
        public const string BarraConocida = "0";
        public const string BarraNoConocida = "1";
        public const string BarraAdicional = "2";
        public const string BarraConocidaDesconocida = "3";

        public List<CmPeriodoDTO> ListaPeriodo { get; set; }
        public List<CmUmbralreporteDTO> ListaUmbral { get; set; }
        public List<CmBarraRelacionDTO> ListaBarraDesconocida { get; set; }
        public List<CmBarraRelacionDTO> ListaBarraEMS { get; set; }
        public List<CmBarraRelacionDTO> ListaBarraTransferencia { get; set; }
        public List<CmEquipobarraDTO> ListaRelacionEquipo { get; set; }
    }

    public class ResultadoConfiguracion
    {
        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Filename { get; set; }
        public int IndicadorFS { get; set; }
    }

    public class ResultadoFactorPerdida
    {
        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public string[][] Datos { get; set; }
        public List<CmFactorperdidaDTO> ListaHistorico { get; set; }
        public string Fecha { get; set; }

        public string FechaDatos { get; set; }
    }

    public class ReporteBarraDesenergizada
    {
        public DateTime Fecha { get; set; }
        public String Inicio { get; set; }
        public string Fin { get; set; }
        public string BarraDesenergizada { get; set; }
        public string BarraReferencia { get; set; }
    }
}
