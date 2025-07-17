using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using System.Linq;

namespace COES.Servicios.Aplicacion.Mediciones
{
    /// <summary>
    /// Clases con métodos del módulo Mediciones
    /// </summary>
    public class IndicadorAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IndicadorAppServicio));

        #region Métodos Tabla F_INDICADOR

        /// <summary>
        /// Inserta un registro de la tabla F_INDICADOR
        /// </summary>
        //public void SaveFIndicador(FIndicadorDTO entity)
        //{
        //    try
        //    {
        //        FactorySic.GetFIndicadorRepository().Save(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ConstantesAppServicio.LogError, ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Actualiza un registro de la tabla F_INDICADOR
        /// </summary>
        public void UpdateFIndicador(FIndicadorDTO entity)
        {
            try
            {
                FactorySic.GetFIndicadorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla F_INDICADOR
        /// </summary>
        public void DeleteFIndicador()
        {
            try
            {
                FactorySic.GetFIndicadorRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla F_INDICADOR
        /// </summary>
        public FIndicadorDTO GetByIdFIndicador()
        {
            return FactorySic.GetFIndicadorRepository().GetById();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla F_INDICADOR
        /// </summary>
        public List<FIndicadorDTO> ListFIndicadors()
        {
            return FactorySic.GetFIndicadorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla FIndicador
        /// </summary>
        public List<FIndicadorDTO> GetByCriteriaFIndicadors()
        {
            return FactorySic.GetFIndicadorRepository().GetByCriteria();
        }

        public string Get_cadena_transgresion(DateTime dtFecha,string ls_dato, int li_gpscodi, string ls_indiccodi, string ls_unimed)
        {
            var oTabla = FactorySic.GetFIndicadorRepository().Get_cadena_transgresion(dtFecha, li_gpscodi, ls_indiccodi);
            string ls_cadena = string.Empty;
            if (oTabla.Rows.Count > 0) //Si existe, seguimos
            {
                foreach (DataRow drow in oTabla.Rows)
                {
                    if (ls_dato == "FECHAHORA")
                    {
                        DateTime ldt_fechahora = Convert.ToDateTime(drow["FECHAHORA"]);
                        ls_cadena = ls_cadena + ldt_fechahora.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 8);
                    }
                    else
                    {
                        double ld_indicador = Convert.ToDouble(drow["INDICVALOR"]);
                        ls_cadena = ls_cadena + Convert.ToString(ld_indicador) + " ";
                    }
                    ls_cadena = ls_cadena + "\n";
                }
            }
            else
                ls_cadena = "---";
            return ls_cadena;
        }

        public int Get_fallaacumulada(DateTime dtFecha, int li_gpscodi, string ls_indiccodi)
        {
            return FactorySic.GetFIndicadorRepository().Get_fallaacumulada(dtFecha, li_gpscodi, ls_indiccodi);
        }

        public string Get_cadena_transgresionFrec(DateTime dtFecha, string ls_dato, int li_gpscodi, string ls_indiccodi, out List<GenericoDTO> listaRangoHora)
        {
            var oTabla = FactorySic.GetFIndicadorRepository().Get_cadena_transgresionFrec(dtFecha, li_gpscodi, ls_indiccodi);
            string ls_cadena = string.Empty;
            listaRangoHora = new List<GenericoDTO>();

            if (oTabla.Rows.Count > 0) //Si existe, seguimos
            {
                foreach (DataRow drow in oTabla.Rows)
                {
                    if (ls_dato == "FECHAHORA")
                    {
                        DateTime ldt_fechahora = Convert.ToDateTime(drow["FECHAHORA"]);


                        var fechaFinStr = string.Empty;
                        if (ls_indiccodi == ConstantesIndicador.VariacionSubita)
                        {
                            fechaFinStr = " - " + ldt_fechahora.AddMinutes(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 8);
                            ls_cadena = ls_cadena + ldt_fechahora.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 8) + fechaFinStr;

                            listaRangoHora.Add(new GenericoDTO() { ValorFecha1 = ldt_fechahora, ValorFecha2 = ldt_fechahora.AddMinutes(1).AddSeconds(-1) });
                        }

                        if (ls_indiccodi == ConstantesIndicador.VariacionSostenida)
                        {
                            if (ls_cadena.Length > 0)
                            {
                                ls_cadena = ls_cadena + " - " + ldt_fechahora.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 8);
                                if (listaRangoHora.Any()) {
                                    listaRangoHora.First().ValorFecha2 = ldt_fechahora;
                                };
                            }
                            else
                            {
                                ls_cadena = ls_cadena + ldt_fechahora.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 8);
                                listaRangoHora.Add(new GenericoDTO() { ValorFecha1 = ldt_fechahora.AddSeconds(1) });
                            }
                            continue;
                        }

                        if (ls_indiccodi == ConstantesIndicador.FrecuenciaMinima || ls_indiccodi == ConstantesIndicador.FrecuenciaMaxima)
                            ls_cadena = ls_cadena + ldt_fechahora.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 8);
                    }
                    else
                    {
                        double ld_indicador = Convert.ToDouble(drow["INDICVALOR"]);
                        if (ls_indiccodi == ConstantesIndicador.VariacionSostenida)
                        {
                            if (ls_cadena.Length > 0)
                                continue;
                            else
                                ls_cadena = ls_cadena + (ld_indicador + 60).ToString("00.000") + " ";

                        }
                        else
                        {
                            if (ls_indiccodi == ConstantesIndicador.VariacionSubita)
                                ls_cadena = ls_cadena + (ld_indicador + 60).ToString("00.000") + " ";
                            else
                                ls_cadena = ls_cadena + (ld_indicador ).ToString("00.000") + " ";
                        }

                    }
                    ls_cadena = ls_cadena + "\n";
                }
            }
            else
                ls_cadena = "---";
            return ls_cadena;
        }

        public int Get_fallaacumuladaFrec(DateTime dtFecha, int li_gpscodi, string ls_indiccodi)
        {
            return FactorySic.GetFIndicadorRepository().Get_fallaacumuladaFrec(dtFecha, li_gpscodi, ls_indiccodi);
        }

        public int Get_fallaacumuladaSEIN(DateTime dtFecha, string ls_dato)
        {
            return FactorySic.ObtenerIeodCuadroDao().Get_fallaacumuladaSEIN(dtFecha, ls_dato);
        }

        #endregion

    }
}
