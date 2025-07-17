using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Infraestructura.Datos.Repositorio.Sic;
using System.Configuration;
using System.Text;

namespace COES.Servicios.Aplicacion.GestionEoEpo
{
    /// <summary>
    /// Clases con métodos del módulo GestionEoEpo
    /// </summary>
    public class GestionEoEpoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GestionEoEpoAppServicio));

        #region Métodos Tabla EPO_CONFIGURA

        /// <summary>
        /// Inserta un registro de la tabla EPO_CONFIGURA
        /// </summary>
        public void SaveEpoConfigura(EpoConfiguraDTO entity)
        {
            try
            {
                FactorySic.GetEpoConfiguraRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_CONFIGURA
        /// </summary>
        public void UpdateEpoConfigura(EpoConfiguraDTO entity)
        {
            try
            {
                FactorySic.GetEpoConfiguraRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_CONFIGURA
        /// </summary>
        public void DeleteEpoConfigura(int confcodi)
        {
            try
            {
                FactorySic.GetEpoConfiguraRepository().Delete(confcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_CONFIGURA
        /// </summary>
        public EpoConfiguraDTO GetByIdEpoConfigura(int confcodi)
        {
            return FactorySic.GetEpoConfiguraRepository().GetById(confcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_CONFIGURA
        /// </summary>
        public List<EpoConfiguraDTO> ListEpoConfiguras()
        {
            return FactorySic.GetEpoConfiguraRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoConfigura
        /// </summary>
        public List<EpoConfiguraDTO> GetByCriteriaEpoConfiguras()
        {
            return FactorySic.GetEpoConfiguraRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EPO_ESTUDIO_EO

        /// <summary>
        /// Inserta un registro de la tabla EPO_ESTUDIO_EO
        /// </summary>
        public int SaveEpoEstudioEo(EpoEstudioEoDTO entity)
        {
            try
            {
                entity.Esteoacumdiascoes = entity.Esteoacumdiascoes.HasValue ? entity.Esteoacumdiascoes : 0;
                return FactorySic.GetEpoEstudioEoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_ESTUDIO_EO
        /// </summary>
        public void UpdateEpoEstudioEo(EpoEstudioEoDTO entity)
        {
            try
            {
                entity.Esteoacumdiascoes = entity.Esteoacumdiascoes.HasValue ? entity.Esteoacumdiascoes : 0;
                FactorySic.GetEpoEstudioEoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_ESTUDIO_EO
        /// </summary>
        public void DeleteEpoEstudioEo(int esteocodi)
        {
            try
            {
                FactorySic.GetEpoEstudioEoRepository().Delete(esteocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private int ObtenerNroDiasRangoFechas(DateTime FechaInicial, DateTime FechaFinal)
        {
            int iResult = 0;
            if (FechaInicial > FechaFinal)
            {
                iResult = (FechaInicial - FechaFinal).Days;
            }
            else
            {
                iResult = (FechaFinal - FechaInicial).Days;
            }
            return iResult;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_ESTUDIO_EO
        /// </summary>
        public EpoEstudioEoDTO GetByIdEpoEstudioEo(int esteocodi)
        {
            EpoEstudioEoDTO estudioEO = FactorySic.GetEpoEstudioEoRepository().GetById(esteocodi);

            if (estudioEO != null)
            {
                List<EpoRevisionEoDTO> listadoRevisionEO = FactorySic.GetEpoRevisionEoRepository().GetByCriteria(esteocodi);

                string[] sEstadoColores = { "#51a351", "#f89406", "#bd362f" };
                string[] sEstado = { "En Tiempo", "En Limite", "Vencido" };

                int iEnvioAlcances = Convert.ToInt32(estudioEO.Esteoplazoalcancesvenc);
                int iVerificacionEstudio = Convert.ToInt32(estudioEO.Esteoplazoverificacionvenc);
                int iVerificacionAbsolucion = Convert.ToInt32(estudioEO.Esteoplazoverificacionvencabs);

                DateTime fVencer;
                DateTime fNoConclu;

                if (estudioEO.Esteoalcancefechaini.HasValue)
                {
                    if (estudioEO.Esteoalcancefechaini.Value.ToShortDateString() != "1/01/0001")
                    {

                        DateTime fFinal = DateTime.Now;

                        if (estudioEO.Esteoalcancefechafin.HasValue)
                        {
                            if (estudioEO.Esteoalcancefechafin.Value.ToShortDateString() != "1/01/0001")
                            {
                                fFinal = estudioEO.Esteoalcancefechafin.Value;
                            }
                        }

                        fVencer = estudioEO.Esteoalcancefechaini.Value;

                        var DiaInicio = estudioEO.Esteoalcancefechaini.Value;

                        var diasHabiles = FactorySic.GetDocDiaEspRepository().NumDiasHabiles(DiaInicio, fFinal);
                        if (diasHabiles > iEnvioAlcances)
                        {
                            estudioEO.Esteoalcanceestadocolor = sEstadoColores[2]; //rojo #FF0000
                            estudioEO.Esteoalcanceestado = sEstado[2];
                        }
                        else
                        {
                            if (diasHabiles <= iEnvioAlcances)
                            {
                                estudioEO.Esteoalcanceestadocolor = sEstadoColores[0]; //verde #00FF00
                                estudioEO.Esteoalcanceestado = sEstado[0];
                            }
                            if (diasHabiles > (iEnvioAlcances - 2) && diasHabiles <= iEnvioAlcances)
                            {
                                estudioEO.Esteoalcanceestadocolor = sEstadoColores[1]; //ambar    FFBF00
                                estudioEO.Esteoalcanceestado = sEstado[1];
                            }
                        }





                    }
                }

                if (estudioEO.Esteoverifechaini.HasValue)
                {
                    if (estudioEO.Esteoverifechaini.Value.ToShortDateString() != "1/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalObs = DateTime.Now;
                        if (estudioEO.Esteoverifechafin.HasValue)
                        {
                            if (estudioEO.Esteoverifechafin.Value.ToShortDateString() != "1/01/0001")
                            {
                                fFinal = estudioEO.Esteoverifechafin.Value;
                                if(estudioEO.EsteoAbsFFin.HasValue)
                                    fFinalObs = estudioEO.EsteoAbsFFin.Value;
                            }
                        }

                        fVencer = estudioEO.Esteoverifechaini.Value;

                        #region Mejoras EO-EPO-II

                        //DateTime fechaAdd = estudioEO.Esteoverifechaini.Value.AddDays(iVerificacionEstudio);
                        //iVerificacionEstudio = FactorySic.GetDocDiaEspRepository().NumDiasHabiles(estudioEO.Esteoverifechaini.Value, fechaAdd);

                        fNoConclu = estudioEO.Esteoverifechaini.Value.AddDays(iVerificacionEstudio);

                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(estudioEO.Esteoverifechaini.Value, iVerificacionEstudio);

                        if (fFinal <= fVencer)
                        {
                            estudioEO.Esteoveriestadocolor = sEstadoColores[0];
                            estudioEO.Esteoveriestado = sEstado[0];
                        }
                        else
                        {
                            estudioEO.Esteoveriestadocolor = sEstadoColores[2];
                            estudioEO.Esteoveriestado = sEstado[2];
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            //if (fFinal <= fNoConclu && DateTime.Now >= fVencer)
                            //{
                            //    estudioEO.Esteoveriestadocolor = sEstadoColores[1];
                            //    estudioEO.Esteoveriestado = sEstado[1];
                            //}
                            //else
                            //{
                            //    estudioEO.Esteoveriestadocolor = sEstadoColores[2];
                            //    estudioEO.Esteoveriestado = sEstado[2];
                            //}
                        }
                        #endregion

                        #region Absolución Verificación

                        //DateTime fechaAddAbs = estudioEO.Esteoverifechaini.Value.AddDays(iVerificacionAbsolucion);
                        //iVerificacionAbsolucion = FactorySic.GetDocDiaEspRepository().NumDiasHabiles(estudioEO.Esteoverifechaini.Value, fechaAddAbs);

                        //fNoConclu = estudioEO.Esteoverifechaini.Value.AddDays(iVerificacionAbsolucion);
                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(estudioEO.Esteoverifechafin.Value, iVerificacionAbsolucion);

                        if (fFinalObs <= fVencer)
                        {
                            estudioEO.EsteoAbsestadocolor = sEstadoColores[0];
                            estudioEO.EsteoAbsestado = sEstado[0];
                        }
                        else
                        {
                            estudioEO.EsteoAbsestadocolor = sEstadoColores[2];
                            estudioEO.EsteoAbsestado = sEstado[2];
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            //if (fFinalObs <= fNoConclu && DateTime.Now >= fVencer)
                            //{
                            //    estudioEO.EsteoAbsestadocolor = sEstadoColores[1];
                            //    estudioEO.EsteoAbsestado = sEstado[1];
                            //}
                            //else
                            //{
                            //    estudioEO.EsteoAbsestadocolor = sEstadoColores[2];
                            //    estudioEO.EsteoAbsestado = sEstado[2];
                            //}
                        }

                        #endregion
                    }
                }

                int iNumRevisiones = 0;
                int iTotNroDiasCoes = 0;
                int iDiasHabilesTotales = 0;
                int iTotNroDiasTercerInv = 0;
                int iTotNroDiasTitProyect = 0;

                foreach (EpoRevisionEoDTO item in listadoRevisionEO)
                {
                    //Revision y Conformidad del Estudio (COES) y Envio Estudio del Tercero Involucrado (COES)
                    if (item.Reveorevcoesfechaini.HasValue && item.Reveocoesfechafin.HasValue)
                    {
                        iTotNroDiasCoes += ObtenerNroDiasRangoFechas(item.Reveorevcoesfechaini.Value, item.Reveocoesfechafin.Value);
                        //-------------Días Habiles de cada revision ---------Edison Bardález
                        iNumRevisiones++;
                        iDiasHabilesTotales += FactorySic.GetDocDiaEspRepository().NumDiasHabiles(item.Reveorevcoesfechaini.Value, item.Reveocoesfechafin.Value);
                        //------------------------------------------------------------------
                    }

                    //Revision del Estudio (Tercero Involucrado)
                    if (item.Reveorevterinvfechaini.HasValue && item.Reveorevterinvfechafin.HasValue)
                    {
                        iTotNroDiasTercerInv += ObtenerNroDiasRangoFechas(item.Reveorevterinvfechaini.Value, item.Reveorevterinvfechafin.Value);

                    }

                    //Levantamiento de Observaciones (Titular del Proyecto)

                    if (item.Reveolevobsfechaini.HasValue && item.Reveolevobsfechafin.HasValue)
                    {
                        iTotNroDiasTitProyect += this.ObtenerNroDiasRangoFechas(item.Reveolevobsfechaini.Value, item.Reveolevobsfechafin.Value);

                    }

                }

                estudioEO.TotNroDiasCoes = iTotNroDiasCoes;
                estudioEO.DiasHabilesTotales = iDiasHabilesTotales;
                estudioEO.TotNroDiasTercerInv = iTotNroDiasTercerInv;
                estudioEO.TotNroDiasTitProyect = iTotNroDiasTitProyect;
                estudioEO.PromNroDiasHabilesCoes = iNumRevisiones == 0 ? "0" : ((float)iDiasHabilesTotales / iNumRevisiones).ToString();
            }

            return estudioEO;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_ESTUDIO_EO
        /// </summary>
        public List<EpoEstudioEoDTO> ListEpoEstudioEos()
        {
            return FactorySic.GetEpoEstudioEoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoEstudioEo
        /// </summary>
        public List<EpoEstudioEoDTO> GetByCriteriaEpoEstudioEos(EpoEstudioEoDTO estudioeo)
        {
            return FactorySic.GetEpoEstudioEoRepository().GetByCriteria(estudioeo);
        }

        /// <summary>
        /// Obtiene el numero de registros de la busqueda
        /// </summary>
        public int ObtenerNroRegistroBusquedaEpoEstudioEos(EpoEstudioEoDTO estudioeo)
        {
            return FactorySic.GetEpoEstudioEoRepository().ObtenerNroRegistroBusqueda(estudioeo);
        }

        #endregion

        #region Métodos Tabla EPO_ESTUDIO_EPO

        /// <summary>
        /// Inserta un registro de la tabla EPO_ESTUDIO_EPO
        /// </summary>
        public int SaveEpoEstudioEpo(EpoEstudioEpoDTO entity)
        {
            try
            {
                entity.Estepoacumdiascoes = entity.Estepoacumdiascoes.HasValue ? entity.Estepoacumdiascoes : 0;
                return FactorySic.GetEpoEstudioEpoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_ESTUDIO_EPO
        /// </summary>
        public void UpdateEpoEstudioEpo(EpoEstudioEpoDTO entity)
        {
            try
            {
                entity.Estepoacumdiascoes = entity.Estepoacumdiascoes.HasValue ? entity.Estepoacumdiascoes : 0;
                FactorySic.GetEpoEstudioEpoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_ESTUDIO_EPO
        /// </summary>
        public void DeleteEpoEstudioEpo(int estepocodi)
        {
            try
            {
                FactorySic.GetEpoEstudioEpoRepository().Delete(estepocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_ESTUDIO_EPO
        /// </summary>
        public EpoEstudioEpoDTO GetByIdEpoEstudioEpo(int estepocodi)
        {
            EpoEstudioEpoDTO estudioEPO = FactorySic.GetEpoEstudioEpoRepository().GetById(estepocodi);


            if (estudioEPO != null)
            {


                List<EpoRevisionEpoDTO> listadoRevisionEO = FactorySic.GetEpoRevisionEpoRepository().GetByCriteria(estepocodi);

                string[] sEstadoColores = { "#51a351", "#f89406", "#bd362f" };
                string[] sEstado = { "En Tiempo", "En Limite", "Vencido" };

                int iEnvioAlcances = Convert.ToInt32(estudioEPO.Estepoplazoalcancesvenc);
                int iVerificacionEstudio = Convert.ToInt32(estudioEPO.Estepoplazoverificacionvenc);
                int iVerificacionAbsolucion = Convert.ToInt32(estudioEPO.Estepoplazoverificacionvencabs);


                DateTime fVencer;
                DateTime fNoConclu;

                if (estudioEPO.Estepoalcancefechaini.HasValue)
                {
                    if (estudioEPO.Estepoalcancefechaini.Value.ToShortDateString() != "1/01/0001")
                    {

                        DateTime fFinal = DateTime.Now;

                        if (estudioEPO.Estepoalcancefechafin.HasValue)
                        {
                            if (estudioEPO.Estepoalcancefechafin.Value.ToShortDateString() != "1/01/0001")
                            {
                                fFinal = estudioEPO.Estepoalcancefechafin.Value;
                            }
                        }

                        fVencer = estudioEPO.Estepoalcancefechaini.Value;

                        var DiaInicio = estudioEPO.Estepoalcancefechaini.Value;

                        var diasHabiles = FactorySic.GetDocDiaEspRepository().NumDiasHabiles(DiaInicio, fFinal);
                        if (diasHabiles > iEnvioAlcances)
                        {
                            estudioEPO.Estepoalcanceestadocolor = sEstadoColores[2]; //rojo #FF0000
                            estudioEPO.Estepoalcanceestado = sEstado[2];
                        }
                        else
                        {
                            if (diasHabiles <= iEnvioAlcances)
                            {
                                estudioEPO.Estepoalcanceestadocolor = sEstadoColores[0]; //verde #00FF00
                                estudioEPO.Estepoalcanceestado = sEstado[0];
                            }
                            if (diasHabiles > (iEnvioAlcances - 2) && diasHabiles <= iEnvioAlcances)
                            {
                                estudioEPO.Estepoalcanceestadocolor = sEstadoColores[1]; //ambar    FFBF00
                                estudioEPO.Estepoalcanceestado = sEstado[1];
                            }
                        }
                    }
                }

                if (estudioEPO.Estepoverifechaini.HasValue)
                {
                    if (estudioEPO.Estepoverifechaini.Value.ToShortDateString() != "1/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalObs = DateTime.Now;

                        if (estudioEPO.Estepoverifechafin.HasValue)
                        {
                            if (estudioEPO.Estepoverifechafin.Value.ToShortDateString() != "1/01/0001")
                            {
                                fFinal = estudioEPO.Estepoverifechafin.Value;
                                if(estudioEPO.EstepoAbsFFin.HasValue)
                                    fFinalObs = estudioEPO.EstepoAbsFFin.Value;
                            }
                        }

                        fVencer = estudioEPO.Estepoverifechaini.Value;

                        #region Mejoras EO-EPO-II
                        //DateTime fechaAdd = estudioEPO.Estepoverifechaini.Value.AddDays(iVerificacionEstudio);
                        //iVerificacionEstudio = FactorySic.GetDocDiaEspRepository().NumDiasHabiles(estudioEPO.Estepoverifechaini.Value, fechaAdd);

                        fNoConclu = estudioEPO.Estepoverifechaini.Value.AddDays(iVerificacionEstudio);

                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(estudioEPO.Estepoverifechaini.Value, iVerificacionEstudio);

                        if (fFinal <= fVencer)
                        {
                            estudioEPO.Estepoveriestadocolor = sEstadoColores[0];
                            estudioEPO.Estepoveriestado = sEstado[0];
                        }
                        else
                        {
                            estudioEPO.Estepoveriestadocolor = sEstadoColores[2];
                            estudioEPO.Estepoveriestado = sEstado[2];
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            //if (fFinal <= fNoConclu && DateTime.Now >= fVencer)
                            //{
                            //    estudioEPO.Estepoveriestadocolor = sEstadoColores[1];
                            //    estudioEPO.Estepoveriestado = sEstado[1];
                            //}
                            //else
                            //{
                            //    estudioEPO.Estepoveriestadocolor = sEstadoColores[2];
                            //    estudioEPO.Estepoveriestado = sEstado[2];
                            //}
                        }
                        #endregion
                        #region Absolución Verificación

                        //DateTime fechaAddAbs = estudioEPO.Estepoverifechaini.Value.AddDays(iVerificacionAbsolucion);
                        //iVerificacionAbsolucion = FactorySic.GetDocDiaEspRepository().NumDiasHabiles(estudioEPO.Estepoverifechaini.Value, fechaAddAbs);

                        //fNoConclu = estudioEPO.Estepoverifechaini.Value.AddDays(iVerificacionAbsolucion);
                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(estudioEPO.Estepoverifechafin.Value, iVerificacionAbsolucion);

                        if (fFinalObs <= fVencer)
                        {
                            estudioEPO.EstepoAbsestadocolor = sEstadoColores[0];
                            estudioEPO.EstepoAbsestado = sEstado[0];
                        }
                        else
                        {
                            estudioEPO.EstepoAbsestadocolor = sEstadoColores[2];
                            estudioEPO.EstepoAbsestado = sEstado[2];

                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            //if (fFinalObs <= fNoConclu && DateTime.Now >= fVencer)
                            //{
                            //    estudioEPO.EstepoAbsestadocolor = sEstadoColores[1];
                            //    estudioEPO.EstepoAbsestado = sEstado[1];
                            //}
                            //else
                            //{
                            //    estudioEPO.EstepoAbsestadocolor = sEstadoColores[2];
                            //    estudioEPO.EstepoAbsestado = sEstado[2];
                            //}
                        }

                        #endregion


                    }
                }

                int iNumRevisiones = 0;
                int iTotNroDiasCoes = 0;
                int iDiasHabilesTotales = 0;
                int iTotNroDiasTercerInv = 0;
                int iTotNroDiasTitProyect = 0;

                foreach (EpoRevisionEpoDTO item in listadoRevisionEO)
                {
                    //Revision y Conformidad del Estudio (COES) y Envio Estudio del Tercero Involucrado (COES)
                    if (item.Reveporevcoesfechaini.HasValue && item.Revepocoesfechafin.HasValue)
                    {
                        iTotNroDiasCoes += ObtenerNroDiasRangoFechas(item.Reveporevcoesfechaini.Value, item.Revepocoesfechafin.Value);
                        //-------------Días Habiles de cada revision ---------Edison Bardález
                        iNumRevisiones++;
                        iDiasHabilesTotales += FactorySic.GetDocDiaEspRepository().NumDiasHabiles(item.Reveporevcoesfechaini.Value, item.Revepocoesfechafin.Value);
                        //------------------------------------------------------------------
                    }

                    //Revision del Estudio (Tercero Involucrado)
                    if (item.Reveporevterinvfechaini.HasValue && item.Reveporevterinvfechafin.HasValue)
                    {
                        iTotNroDiasTercerInv += ObtenerNroDiasRangoFechas(item.Reveporevterinvfechaini.Value, item.Reveporevterinvfechafin.Value);

                    }

                    //Levantamiento de Observaciones (Titular del Proyecto)

                    if (item.Revepolevobsfechaini.HasValue && item.Revepolevobsfechafin.HasValue)
                    {
                        iTotNroDiasTitProyect += this.ObtenerNroDiasRangoFechas(item.Revepolevobsfechaini.Value, item.Revepolevobsfechafin.Value);

                    }

                }

                estudioEPO.TotNroDiasCoes = iTotNroDiasCoes;
                estudioEPO.DiasHabilesTotales = iDiasHabilesTotales;
                estudioEPO.TotNroDiasTercerInv = iTotNroDiasTercerInv;
                estudioEPO.TotNroDiasTitProyect = iTotNroDiasTitProyect;
                estudioEPO.PromNroDiasHabilesCoes = iNumRevisiones == 0 ? "0" : ((float)iDiasHabilesTotales / iNumRevisiones).ToString();
            }

            return estudioEPO;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_ESTUDIO_EPO
        /// </summary>
        public List<EpoEstudioEpoDTO> ListEpoEstudioEpos()
        {
            return FactorySic.GetEpoEstudioEpoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoEstudioEpo
        /// </summary>
        public List<EpoEstudioEpoDTO> GetByCriteriaEpoEstudioEpos(EpoEstudioEpoDTO estudioepo)
        {
            return FactorySic.GetEpoEstudioEpoRepository().GetByCriteria(estudioepo);
        }

        /// <summary>
        /// Obtiene el numero de registros de la busqueda
        /// </summary>
        public int ObtenerNroRegistroBusquedaEpoEstudioEpos(EpoEstudioEpoDTO estudioepo)
        {
            return FactorySic.GetEpoEstudioEpoRepository().ObtenerNroRegistroBusqueda(estudioepo);
        }


        #endregion

        #region Métodos Tabla EPO_ESTUDIO_ESTADO

        /// <summary>
        /// Inserta un registro de la tabla EPO_ESTUDIO_ESTADO
        /// </summary>
        public void SaveEpoEstudioEstado(EpoEstudioEstadoDTO entity)
        {
            try
            {
                FactorySic.GetEpoEstudioEstadoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_ESTUDIO_ESTADO
        /// </summary>
        public void UpdateEpoEstudioEstado(EpoEstudioEstadoDTO entity)
        {
            try
            {
                FactorySic.GetEpoEstudioEstadoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_ESTUDIO_ESTADO
        /// </summary>
        public void DeleteEpoEstudioEstado(int estacodi)
        {
            try
            {
                FactorySic.GetEpoEstudioEstadoRepository().Delete(estacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_ESTUDIO_ESTADO
        /// </summary>
        public EpoEstudioEstadoDTO GetByIdEpoEstudioEstado(int estacodi)
        {
            return FactorySic.GetEpoEstudioEstadoRepository().GetById(estacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_ESTUDIO_ESTADO
        /// </summary>
        public List<EpoEstudioEstadoDTO> ListEpoEstudioEstados()
        {
            return FactorySic.GetEpoEstudioEstadoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoEstudioEstado
        /// </summary>
        public List<EpoEstudioEstadoDTO> GetByCriteriaEpoEstudioEstados()
        {
            return FactorySic.GetEpoEstudioEstadoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EPO_HISTORICO_INDICADOR

        /// <summary>
        /// Inserta un registro de la tabla EPO_HISTORICO_INDICADOR
        /// </summary>
        public void SaveEpoHistoricoIndicador(EpoHistoricoIndicadorDTO entity)
        {
            try
            {
                FactorySic.GetEpoHistoricoIndicadorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_HISTORICO_INDICADOR
        /// </summary>
        public void UpdateEpoHistoricoIndicador(EpoHistoricoIndicadorDTO entity)
        {
            try
            {
                FactorySic.GetEpoHistoricoIndicadorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_HISTORICO_INDICADOR
        /// </summary>
        public void DeleteEpoHistoricoIndicador(int hincodi)
        {
            try
            {
                FactorySic.GetEpoHistoricoIndicadorRepository().Delete(hincodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_HISTORICO_INDICADOR
        /// </summary>
        public EpoHistoricoIndicadorDTO GetByIdEpoHistoricoIndicador(int hincodi)
        {
            return FactorySic.GetEpoHistoricoIndicadorRepository().GetById(hincodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_HISTORICO_INDICADOR
        /// </summary>
        public List<EpoHistoricoIndicadorDTO> ListEpoHistoricoIndicadors()
        {
            return FactorySic.GetEpoHistoricoIndicadorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoHistoricoIndicador
        /// </summary>
        public List<EpoHistoricoIndicadorDTO> GetByCriteriaEpoHistoricoIndicadors()
        {
            return FactorySic.GetEpoHistoricoIndicadorRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EPO_HISTORICO_INDICADOR_DET

        /// <summary>
        /// Inserta un registro de la tabla EPO_HISTORICO_INDICADOR_DET
        /// </summary>
        public void SaveEpoHistoricoIndicadorDet(EpoHistoricoIndicadorDetDTO entity)
        {
            try
            {
                FactorySic.GetEpoHistoricoIndicadorDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_HISTORICO_INDICADOR_DET
        /// </summary>
        public void UpdateEpoHistoricoIndicadorDet(EpoHistoricoIndicadorDetDTO entity)
        {
            try
            {
                FactorySic.GetEpoHistoricoIndicadorDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_HISTORICO_INDICADOR_DET
        /// </summary>
        public void DeleteEpoHistoricoIndicadorDet(int hincodi, int percodi)
        {
            try
            {
                FactorySic.GetEpoHistoricoIndicadorDetRepository().Delete(hincodi, percodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_HISTORICO_INDICADOR_DET
        /// </summary>
        public EpoHistoricoIndicadorDetDTO GetByIdEpoHistoricoIndicadorDet(int hincodi, int percodi)
        {
            return FactorySic.GetEpoHistoricoIndicadorDetRepository().GetById(hincodi, percodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_HISTORICO_INDICADOR_DET
        /// </summary>
        public List<EpoHistoricoIndicadorDetDTO> ListEpoHistoricoIndicadorDets()
        {
            return FactorySic.GetEpoHistoricoIndicadorDetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoHistoricoIndicadorDet
        /// </summary>
        public List<EpoHistoricoIndicadorDetDTO> GetByCriteriaEpoHistoricoIndicadorDets()
        {
            return FactorySic.GetEpoHistoricoIndicadorDetRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EPO_INDICADOR

        /// <summary>
        /// Inserta un registro de la tabla EPO_INDICADOR
        /// </summary>
        public void SaveEpoIndicador(EpoIndicadorDTO entity)
        {
            try
            {
                FactorySic.GetEpoIndicadorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_INDICADOR
        /// </summary>
        public void UpdateEpoIndicador(EpoIndicadorDTO entity)
        {
            try
            {
                FactorySic.GetEpoIndicadorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_INDICADOR
        /// </summary>
        public void DeleteEpoIndicador(int indcodi)
        {
            try
            {
                FactorySic.GetEpoIndicadorRepository().Delete(indcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_INDICADOR
        /// </summary>
        public EpoIndicadorDTO GetByIdEpoIndicador(int indcodi)
        {
            return FactorySic.GetEpoIndicadorRepository().GetById(indcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_INDICADOR
        /// </summary>
        public List<EpoIndicadorDTO> ListEpoIndicadors()
        {
            return FactorySic.GetEpoIndicadorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoIndicador
        /// </summary>
        public List<EpoIndicadorDTO> GetByCriteriaEpoIndicadors()
        {
            return FactorySic.GetEpoIndicadorRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EPO_PERIODO

        /// <summary>
        /// Inserta un registro de la tabla EPO_PERIODO
        /// </summary>
        public void SaveEpoPeriodo(EpoPeriodoDTO entity)
        {
            try
            {
                FactorySic.GetEpoPeriodoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_PERIODO
        /// </summary>
        public void UpdateEpoPeriodo(EpoPeriodoDTO entity)
        {
            try
            {
                FactorySic.GetEpoPeriodoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_PERIODO
        /// </summary>
        public void DeleteEpoPeriodo(int percodi)
        {
            try
            {
                FactorySic.GetEpoPeriodoRepository().Delete(percodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_PERIODO
        /// </summary>
        public EpoPeriodoDTO GetByIdEpoPeriodo(int percodi)
        {
            return FactorySic.GetEpoPeriodoRepository().GetById(percodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_PERIODO
        /// </summary>
        public List<EpoPeriodoDTO> ListEpoPeriodos()
        {
            return FactorySic.GetEpoPeriodoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoPeriodo
        /// </summary>
        public List<EpoPeriodoDTO> GetByCriteriaEpoPeriodos()
        {
            return FactorySic.GetEpoPeriodoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EPO_REVISION_EO

        /// <summary>
        /// Inserta un registro de la tabla EPO_REVISION_EO
        /// </summary>
        public void SaveEpoRevisionEo(EpoRevisionEoDTO entity)
        {
            try
            {
                FactorySic.GetEpoRevisionEoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_REVISION_EO
        /// </summary>
        public void UpdateEpoRevisionEo(EpoRevisionEoDTO entity)
        {
            try
            {
                FactorySic.GetEpoRevisionEoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_REVISION_EO
        /// </summary>
        public void DeleteEpoRevisionEo(int reveocodi)
        {
            try
            {
                FactorySic.GetEpoRevisionEoRepository().Delete(reveocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_REVISION_EO
        /// </summary>
        public EpoRevisionEoDTO GetByIdEpoRevisionEo(int reveocodi)
        {
            return FactorySic.GetEpoRevisionEoRepository().GetById(reveocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_REVISION_EO
        /// </summary>
        public List<EpoRevisionEoDTO> ListEpoRevisionEos()
        {
            return FactorySic.GetEpoRevisionEoRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_REVISION_EO Revision COES que esta en vencimiento
        /// </summary>
        public List<EpoRevisionEoDTO> GetByCriteriaEpoRevisionEstudioEos(int diautil, int diautilvenc)
        {
            return FactorySic.GetEpoRevisionEoRepository().GetByCriteriaRevisionEstudio(diautil, diautilvenc);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_REVISION_EO Revision Estudio Tercer Involucrado COES que esta en vencimiento
        /// </summary>
        public List<EpoRevisionEoDTO> GetByCriteriaEpoEnvioTerceroInvEos(int diautil, int diautilvenc)
        {
            return FactorySic.GetEpoRevisionEoRepository().GetByCriteriaEnvioTerceroInv(diautil, diautilvenc);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoRevisionEo
        /// </summary>
        public List<EpoRevisionEoDTO> GetByCriteriaEpoRevisionEos(int esteocodi)
        {
            List<EpoRevisionEoDTO> listadoRevisionEO = FactorySic.GetEpoRevisionEoRepository().GetByCriteria(esteocodi);

            EpoEstudioEoDTO estudioEO = FactorySic.GetEpoEstudioEoRepository().GetById(esteocodi);

            string[] sEstadoColores = { "#51a351", "#f89406", "#bd362f" };
            string[] sEstado = { "En Tiempo", "En Limite", "Vencido" };

            foreach (EpoRevisionEoDTO item in listadoRevisionEO)
            {
                int nDiasRevCoes = estudioEO.Esteoplazorevcoesporv.HasValue ? estudioEO.Esteoplazorevcoesporv.Value : 0;
                int nDiasEnvioTer = estudioEO.Esteoplazoenvestterinvporv.HasValue ? estudioEO.Esteoplazoenvestterinvporv.Value : 0;
                int nDiasRevTerInv = estudioEO.Esteoplazorevterinvporv.HasValue ? estudioEO.Esteoplazorevterinvporv.Value : 0;
                

                int nDiasRevCoesVenc = estudioEO.Esteoplazorevcoesvenc.HasValue ? estudioEO.Esteoplazorevcoesvenc.Value : 0;
                int nDiasEnvioTerVenc = estudioEO.Esteoplazoenvestterinvvenc.HasValue ? estudioEO.Esteoplazoenvestterinvvenc.Value : 0;
                int nDiasRevTerInvVenc = estudioEO.Esteoplazorevterinvvenc.HasValue ? estudioEO.Esteoplazorevterinvvenc.Value : 0;

                int nDiasLevObs;
                int nDiasLevObsVenc;
                if (estudioEO.TipoConfig == 1)
                {
                    nDiasLevObs = estudioEO.Esteoplazolevobsporv.HasValue ? estudioEO.Esteoplazolevobsporv.Value : 0;
                    nDiasLevObsVenc = estudioEO.Esteoplazolevobsvenc.HasValue ? estudioEO.Esteoplazolevobsvenc.Value : 0;
                }
                else
                {
                    int nMesesLevObs;
                    int nMesesLevObsVenc;
                    

                    nMesesLevObs = estudioEO.Esteoplazolevobsporv.HasValue ? estudioEO.Esteoplazolevobsporv.Value : 0;
                    nMesesLevObsVenc = estudioEO.Esteoplazolevobsvenc.HasValue ? estudioEO.Esteoplazolevobsvenc.Value : 0;
                    
                    DateTime fFinLevObs= item.Reveolevobsfechaini.Value.AddMonths(nMesesLevObs);
                    DateTime fFinLevObsVenc = item.Reveolevobsfechaini.Value.AddMonths(nMesesLevObsVenc);

                    nDiasLevObs = FactorySic.GetDocDiaEspRepository().NumDiasHabiles(item.Reveolevobsfechaini.Value,fFinLevObs);
                    nDiasLevObsVenc = FactorySic.GetDocDiaEspRepository().NumDiasHabiles(item.Reveolevobsfechaini.Value, fFinLevObsVenc);
                }
                

                DateTime fVencer;
                DateTime fNoConclu;
                DateTime fVencerNoHabil;
                DateTime fNoConcluNoHabil;
                if (item.Reveorevcoesfechaini.HasValue)
                {
                    if (item.Reveorevcoesfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalNoHabil = DateTime.Now;

                        if (item.Reveocoesfechafin.HasValue)
                        {
                            if (item.Reveocoesfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                fFinal = item.Reveocoesfechafin.Value;
                                fFinalNoHabil = item.Reveocoesfechafin.Value;
                            }
                        }

                        if (item.Reveorevcoesampl > 0)
                        {
                            nDiasRevCoes += item.Reveorevcoesampl;
                            nDiasRevCoesVenc += item.Reveorevcoesampl;
                        }

                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveorevcoesfechaini.Value, nDiasRevCoes);
                        fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveorevcoesfechaini.Value, nDiasRevCoes);

                        fNoConclu = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveorevcoesfechaini.Value, nDiasRevCoesVenc);
                        fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveorevcoesfechaini.Value, nDiasRevCoesVenc);

                        //if (DateTime.Now <= fVencer)
                        if (fFinal <= fVencer || fFinalNoHabil <= fVencerNoHabil)
                        {
                            item.RevisionConfirmidadEstudioEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.RevisionConfirmidadEstudioEstado = sEstado[0];//Vigente
                        }
                        else
                        {
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            if ((fFinal <= fNoConclu && fFinal >= fVencer) || (fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                            {
                                item.RevisionConfirmidadEstudioEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.RevisionConfirmidadEstudioEstado = sEstado[1]; // Por Vencer
                            }
                            else
                            {
                                item.RevisionConfirmidadEstudioEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.RevisionConfirmidadEstudioEstado = sEstado[2];  //Vencido
                            }
                        }
                    }
                }

                if (item.Reveoenvesttercinvfechaini.HasValue)
                {
                    if (item.Reveoenvesttercinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalNoHabil = DateTime.Now;
                        if (item.Reveoenvesttercinvinvfechafin.HasValue)
                        {
                            if (item.Reveoenvesttercinvinvfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                fFinal = item.Reveoenvesttercinvinvfechafin.Value;
                                fFinalNoHabil = item.Reveoenvesttercinvinvfechafin.Value;
                            }
                        }
                        //por vencer
                        //fechaAdd = item.EnvioEstudioTerceroInvolucradoFechaInicio.AddDays(nDiasEnvioTer);
                        //nDiasEnvioTer = this.docdiaRepository.DiasCalEntreFechas(item.EnvioEstudioTerceroInvolucradoFechaInicio, fechaAdd);
                        //fVencer = item.EnvioEstudioTerceroInvolucradoFechaInicio.AddDays(nDiasEnvioTer);
                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveoenvesttercinvfechaini.Value, nDiasEnvioTer);
                        fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveoenvesttercinvfechaini.Value, nDiasEnvioTer);

                        // vecido
                        //fechaAdd = item.EnvioEstudioTerceroInvolucradoFechaInicio.AddDays(nDiasLevInfo);
                        //nDiasLevInfo = this.docdiaRepository.DiasCalEntreFechas(item.EnvioEstudioTerceroInvolucradoFechaInicio, fechaAdd);
                        //fNoConclu = item.EnvioEstudioTerceroInvolucradoFechaInicio.AddDays(nDiasLevInfo);
                        fNoConclu = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveoenvesttercinvfechaini.Value, nDiasEnvioTerVenc);//nDiasLevInfo);
                        fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveoenvesttercinvfechaini.Value, nDiasEnvioTerVenc);//nDiasLevInfo);

                        //if (DateTime.Now <= fVencer)
                        if (fFinal < fVencer || fFinalNoHabil < fVencerNoHabil)
                        {
                            item.EnvioEstudioTercerInvolucradoEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.EnvioEstudioTercerInvolucradoEstado = sEstado[0];
                        }
                        else
                        {
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            if ((fFinal <= fNoConclu && fFinal >= fVencer) || (fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                            {
                                item.EnvioEstudioTercerInvolucradoEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.EnvioEstudioTercerInvolucradoEstado = sEstado[1];
                            }
                            else
                            {
                                item.EnvioEstudioTercerInvolucradoEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.EnvioEstudioTercerInvolucradoEstado = sEstado[2];
                            }
                        }
                    }
                }

                if (item.Reveorevterinvfechaini.HasValue)
                {
                    if (item.Reveorevterinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalNoHabil = DateTime.Now;

                        if (item.Reveorevterinvfechafin.HasValue)
                        {
                            if (item.Reveorevterinvfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                fFinal = item.Reveorevterinvfechafin.Value;
                                fFinalNoHabil = item.Reveorevterinvfechafin.Value;
                            }
                        }

                        if (item.Reveorevterinvampl > 0)
                        {
                            nDiasRevTerInv += item.Reveorevterinvampl;
                            nDiasRevTerInvVenc += item.Reveorevterinvampl;
                        }


                        // por vencer
                        //fechaAdd = item.RevisionTerceroInvolucradoFechaInicio.AddDays(nDiasRevTer);
                        //nDiasRevTer = this.docdiaRepository.DiasCalEntreFechas(item.RevisionTerceroInvolucradoFechaInicio, fechaAdd);
                        //fVencer = item.RevisionTerceroInvolucradoFechaInicio.AddDays(nDiasRevTer);
                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveorevterinvfechaini.Value, nDiasRevTerInv);
                        fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveorevterinvfechaini.Value, nDiasRevTerInv);

                        // vencido
                        //fVencido = item.RevisionTerceroInvolucradoFechaFin.AddDays(nDiasRev3erInv);
                        //fechaAdd = item.RevisionTerceroInvolucradoFechaInicio.AddDays(nDiasRev3erInv);
                        //nDiasRev3erInv = this.docdiaRepository.DiasCalEntreFechas(item.RevisionTerceroInvolucradoFechaInicio, fechaAdd);
                        //fNoConclu = item.RevisionTerceroInvolucradoFechaInicio.AddDays(nDiasRev3erInv);
                        fNoConclu = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveorevterinvfechaini.Value, nDiasRevTerInvVenc);
                        fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveorevterinvfechaini.Value, nDiasRevTerInvVenc);

                        //if (DateTime.Now <= fVencer)
                        if (fFinal <= fVencer || fFinalNoHabil <= fVencerNoHabil)
                        {
                            item.RevisionEstudioEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.RevisionEstudioEstado = sEstado[0];
                        }
                        else
                        {
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            if ((fFinal <= fNoConclu && fFinal >= fVencer)||(fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                            {
                                item.RevisionEstudioEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.RevisionEstudioEstado = sEstado[1];
                            }
                            else
                            {
                                item.RevisionEstudioEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.RevisionEstudioEstado = sEstado[2];
                            }
                        }

                    }
                }

                #region Absolución de observaciones
                if (item.Reveolevobsfechaini.HasValue && estudioEO.TipoConfig == 1) // Cálculo en base a PR20 (antiguo)
                {
                    nDiasLevObs = estudioEO.Esteoplazolevobsporv.HasValue ? estudioEO.Esteoplazolevobsporv.Value : 0;
                    nDiasLevObsVenc = estudioEO.Esteoplazolevobsvenc.HasValue ? estudioEO.Esteoplazolevobsvenc.Value : 0;

                    if (item.Reveolevobsfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalNoHabil = DateTime.Now;

                        if (item.Reveolevobsfechafin.HasValue)
                        {
                            if (item.Reveolevobsfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                fFinal = item.Reveolevobsfechafin.Value;
                                fFinalNoHabil = item.Reveolevobsfechafin.Value;
                            }
                        }

                        //vencido
                        //fechaAdd = item.LevantamientoObservacionFechaInicio.AddDays(nDiasLevObs);
                        //nDiasLevObs = this.docdiaRepository.DiasCalEntreFechas(item.LevantamientoObservacionFechaInicio, fechaAdd);
                        //fVencer = item.LevantamientoObservacionFechaInicio.AddDays(nDiasLevObs);
                        fVencer = item.Reveolevobsfechaini.Value.AddDays(nDiasLevObs);
                        fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabSinFS(item.Reveolevobsfechaini.Value, nDiasLevObs);

                        //por vencer
                        //fVencido = item.LevantamientoObservacionFechaFin.AddDays(nDiasEst3erInv);
                        //fechaAdd = item.LevantamientoObservacionFechaInicio.AddDays(nDiasEst3erInv);
                        //nDiasEst3erInv = this.docdiaRepository.DiasCalEntreFechas(item.LevantamientoObservacionFechaFin, fechaAdd);
                        //fNoConclu = item.LevantamientoObservacionFechaFin.AddDays(nDiasEst3erInv);
                        fNoConclu = item.Reveolevobsfechaini.Value.AddDays(nDiasLevObsVenc);
                        fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabSinFS(item.Reveolevobsfechaini.Value, nDiasLevObsVenc);

                        //if (DateTime.Now <= fVencer)
                        if (fFinal <= fVencer || fFinalNoHabil <= fVencerNoHabil)
                        {
                            item.LevantamientObservacionEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.LevantamientObservacionEstado = sEstado[0];
                        }
                        else
                        {
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            if ((fFinal <= fNoConclu && fFinal >= fVencer) || (fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                            {
                                item.LevantamientObservacionEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.LevantamientObservacionEstado = sEstado[1];
                            }
                            else
                            {
                                item.LevantamientObservacionEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.LevantamientObservacionEstado = sEstado[2];
                            }
                        }
                    }
                }
                else if (item.Reveolevobsfechaini.HasValue) // Cálculo en base a PR20 (nuevo)
                {
                    int nMesesLevObs;
                    int nMesesLevObsVenc;
                    DateTime fFinal = DateTime.Now;

                    nMesesLevObs = estudioEO.Esteoplazolevobsporv.HasValue ? estudioEO.Esteoplazolevobsporv.Value : 0;
                    nMesesLevObsVenc = estudioEO.Esteoplazolevobsvenc.HasValue ? estudioEO.Esteoplazolevobsvenc.Value : 0;

                    DateTime fFinLevObs = item.Reveolevobsfechaini.Value.AddMonths(nMesesLevObs);
                    DateTime fFinLevObsVenc = item.Reveolevobsfechaini.Value.AddMonths(nMesesLevObsVenc);

                    if (item.Reveolevobsfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        if (item.Reveolevobsfechafin.HasValue)
                            fFinal = item.Reveolevobsfechafin.Value; //Fecha Fin ingresada
                       
                        //Validar y obtener día útil de F.Lev.Obs y F.Lev.Obs.Vencido
                        fFinLevObs = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObs.AddDays(-1), 1);
                        fFinLevObsVenc = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObsVenc.AddDays(-1), 1);

                        //Validamos si tiene días de ampliación
                        if (item.Reveopreampl > 0)
                        {
                            fFinLevObs = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObs, item.Reveopreampl);
                            fFinLevObsVenc = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObsVenc, item.Reveopreampl);
                        }

                        if (fFinal <= fFinLevObs)
                        {
                            item.LevantamientObservacionEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.LevantamientObservacionEstado = sEstado[0];
                        }
                        else
                        {
                            if (fFinal > fFinLevObs && fFinal <= fFinLevObsVenc)
                            {
                                item.LevantamientObservacionEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.LevantamientObservacionEstado = sEstado[1];
                            }
                            else
                            {
                                item.LevantamientObservacionEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.LevantamientObservacionEstado = sEstado[2];
                            }
                        }
                    }
                }

                #endregion
            }

            return listadoRevisionEO;
        }

        #endregion

        #region Métodos Tabla EPO_REVISION_EPO

        /// <summary>
        /// Inserta un registro de la tabla EPO_REVISION_EPO
        /// </summary>
        public void SaveEpoRevisionEpo(EpoRevisionEpoDTO entity)
        {
            try
            {
                FactorySic.GetEpoRevisionEpoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EPO_REVISION_EPO
        /// </summary>
        public void UpdateEpoRevisionEpo(EpoRevisionEpoDTO entity)
        {
            try
            {
                FactorySic.GetEpoRevisionEpoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_REVISION_EPO
        /// </summary>
        public void DeleteEpoRevisionEpo(int revepocodi)
        {
            try
            {
                FactorySic.GetEpoRevisionEpoRepository().Delete(revepocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EPO_REVISION_EPO
        /// </summary>
        public EpoRevisionEpoDTO GetByIdEpoRevisionEpo(int revepocodi)
        {
            return FactorySic.GetEpoRevisionEpoRepository().GetById(revepocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_REVISION_EPO
        /// </summary>
        public List<EpoRevisionEpoDTO> ListEpoRevisionEpos()
        {
            return FactorySic.GetEpoRevisionEpoRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_REVISION_EPO Revision COES que esta en vencimiento
        /// </summary>
        public List<EpoRevisionEpoDTO> GetByCriteriaEpoRevisionEstudioEpos(int diautil, int diautilvenc)
        {
            return FactorySic.GetEpoRevisionEpoRepository().GetByCriteriaRevisionEstudio(diautil, diautilvenc);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_REVISION_EO Revision Estudio Tercer Involucrado COES que esta en vencimiento
        /// </summary>
        public List<EpoRevisionEpoDTO> GetByCriteriaEpoEnvioTerceroInvEpos(int diautil, int diautilvenc)
        {
            return FactorySic.GetEpoRevisionEpoRepository().GetByCriteriaEnvioTerceroInv(diautil, diautilvenc);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EpoRevisionEpo
        /// </summary>
        public List<EpoRevisionEpoDTO> GetByCriteriaEpoRevisionEpos(int estepocodi)
        {
            List<EpoRevisionEpoDTO> listadoRevisionEPO = FactorySic.GetEpoRevisionEpoRepository().GetByCriteria(estepocodi);

            EpoEstudioEpoDTO estudioEPO = FactorySic.GetEpoEstudioEpoRepository().GetById(estepocodi);

            string[] sEstadoColores = { "#51a351", "#f89406", "#bd362f" };
            string[] sEstado = { "En Tiempo", "En Limite", "Vencido" };

            foreach (EpoRevisionEpoDTO item in listadoRevisionEPO)
            {
                int nDiasRevCoes = estudioEPO.Estepoplazorevcoesporv.HasValue ? estudioEPO.Estepoplazorevcoesporv.Value : 0;
                int nDiasEnvioTer = estudioEPO.Estepoplazoenvestterinvporv.HasValue ? estudioEPO.Estepoplazoenvestterinvporv.Value : 0;
                int nDiasRevTerInv = estudioEPO.Estepoplazorevterinvporv.HasValue ? estudioEPO.Estepoplazorevterinvporv.Value : 0;

                int nDiasRevCoesVenc = estudioEPO.Estepoplazorevcoesvenc.HasValue ? estudioEPO.Estepoplazorevcoesvenc.Value : 0;
                int nDiasEnvioTerVenc = estudioEPO.Estepoplazoenvestterinvvenc.HasValue ? estudioEPO.Estepoplazoenvestterinvvenc.Value : 0;
                int nDiasRevTerInvVenc = estudioEPO.Estepoplazorevterinvvenc.HasValue ? estudioEPO.Estepoplazorevterinvvenc.Value : 0;

                int nDiasLevObs;
                int nDiasLevObsVenc;

                DateTime fVencer;
                DateTime fNoConclu;
                DateTime fVencerNoHabil;
                DateTime fNoConcluNoHabil;

                if (item.Reveporevcoesfechaini.HasValue)
                {
                    if (item.Reveporevcoesfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalNoHabil = DateTime.Now;

                        if (item.Revepocoesfechafin.HasValue)
                        {
                            if (item.Revepocoesfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                fFinal = item.Revepocoesfechafin.Value;
                                fFinalNoHabil = item.Revepocoesfechafin.Value;
                            }
                        }

                        if (item.Reveporevcoesampl > 0)
                        {
                            nDiasRevCoes += item.Reveporevcoesampl;
                            nDiasRevCoesVenc += item.Reveporevcoesampl;
                        }

                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveporevcoesfechaini.Value, nDiasRevCoes);
                        fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveporevcoesfechaini.Value, nDiasRevCoes);

                        fNoConclu = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveporevcoesfechaini.Value, nDiasRevCoesVenc);
                        fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveporevcoesfechaini.Value, nDiasRevCoesVenc);

                        //if (DateTime.Now <= fVencer)
                        if (fFinal <= fVencer || fFinalNoHabil <= fVencerNoHabil)
                        {
                            item.RevisionConfirmidadEstudioEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.RevisionConfirmidadEstudioEstado = sEstado[0];//Vigente
                        }
                        else
                        {
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            if ((fFinal <= fNoConclu && fFinal >= fVencer) || (fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                            {
                                item.RevisionConfirmidadEstudioEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.RevisionConfirmidadEstudioEstado = sEstado[1]; // Por Vencer
                            }
                            else
                            {
                                item.RevisionConfirmidadEstudioEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.RevisionConfirmidadEstudioEstado = sEstado[2];  //Vencido
                            }
                        }
                    }
                }

                if (item.Revepoenvesttercinvfechaini.HasValue)
                {
                    if (item.Revepoenvesttercinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalNoHabil = DateTime.Now;
                        if (item.Revepoenvesttercinvinvfechafin.HasValue)
                        {
                            if (item.Revepoenvesttercinvinvfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                fFinal = item.Revepoenvesttercinvinvfechafin.Value;
                                fFinalNoHabil = item.Revepoenvesttercinvinvfechafin.Value;
                            }
                        }
                        //por vencer
                        //fechaAdd = item.EnvioEstudioTerceroInvolucradoFechaInicio.AddDays(nDiasEnvioTer);
                        //nDiasEnvioTer = this.docdiaRepository.DiasCalEntreFechas(item.EnvioEstudioTerceroInvolucradoFechaInicio, fechaAdd);
                        //fVencer = item.EnvioEstudioTerceroInvolucradoFechaInicio.AddDays(nDiasEnvioTer);
                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Revepoenvesttercinvfechaini.Value, nDiasEnvioTer);
                        fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Revepoenvesttercinvfechaini.Value, nDiasEnvioTer);

                        // vecido
                        //fechaAdd = item.EnvioEstudioTerceroInvolucradoFechaInicio.AddDays(nDiasLevInfo);
                        //nDiasLevInfo = this.docdiaRepository.DiasCalEntreFechas(item.EnvioEstudioTerceroInvolucradoFechaInicio, fechaAdd);
                        //fNoConclu = item.EnvioEstudioTerceroInvolucradoFechaInicio.AddDays(nDiasLevInfo);
                        fNoConclu = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Revepoenvesttercinvfechaini.Value, nDiasEnvioTerVenc);//nDiasLevInfo);
                        fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Revepoenvesttercinvfechaini.Value, nDiasEnvioTerVenc);//nDiasLevInfo);

                        //if (DateTime.Now <= fVencer)
                        if (fFinal < fVencer || fFinalNoHabil < fVencerNoHabil)
                        {
                            item.EnvioEstudioTercerInvolucradoEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.EnvioEstudioTercerInvolucradoEstado = sEstado[0];
                        }
                        else
                        {
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            if ((fFinal <= fNoConclu && fFinal >= fVencer) || (fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                            {
                                item.EnvioEstudioTercerInvolucradoEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.EnvioEstudioTercerInvolucradoEstado = sEstado[1];
                            }
                            else
                            {
                                item.EnvioEstudioTercerInvolucradoEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.EnvioEstudioTercerInvolucradoEstado = sEstado[2];
                            }
                        }
                    }
                }

                if (item.Reveporevterinvfechaini.HasValue)
                {
                    if (item.Reveporevterinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalNoHabil = DateTime.Now;

                        if (item.Reveporevterinvfechafin.HasValue)
                        {
                            if (item.Reveporevterinvfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                fFinal = item.Reveporevterinvfechafin.Value;
                                fFinalNoHabil = item.Reveporevterinvfechafin.Value;
                            }
                        }

                        if (item.Reveporevterinvampl > 0)
                        {
                            nDiasRevTerInv += item.Reveporevterinvampl;
                            nDiasRevTerInvVenc += item.Reveporevterinvampl;
                        }

                        // por vencer
                        //fechaAdd = item.RevisionTerceroInvolucradoFechaInicio.AddDays(nDiasRevTer);
                        //nDiasRevTer = this.docdiaRepository.DiasCalEntreFechas(item.RevisionTerceroInvolucradoFechaInicio, fechaAdd);
                        //fVencer = item.RevisionTerceroInvolucradoFechaInicio.AddDays(nDiasRevTer);
                        fVencer = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveporevterinvfechaini.Value, nDiasRevTerInv);
                        fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveporevterinvfechaini.Value, nDiasRevTerInv);

                        // vencido
                        //fVencido = item.RevisionTerceroInvolucradoFechaFin.AddDays(nDiasRev3erInv);
                        //fechaAdd = item.RevisionTerceroInvolucradoFechaInicio.AddDays(nDiasRev3erInv);
                        //nDiasRev3erInv = this.docdiaRepository.DiasCalEntreFechas(item.RevisionTerceroInvolucradoFechaInicio, fechaAdd);
                        //fNoConclu = item.RevisionTerceroInvolucradoFechaInicio.AddDays(nDiasRev3erInv);
                        fNoConclu = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(item.Reveporevterinvfechaini.Value, nDiasRevTerInvVenc);
                        fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabiles(item.Reveporevterinvfechaini.Value, nDiasRevTerInvVenc);

                        //if (DateTime.Now <= fVencer)
                        if (fFinal <= fVencer || fFinalNoHabil <= fVencerNoHabil)
                        {
                            item.RevisionEstudioEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.RevisionEstudioEstado = sEstado[0];
                        }
                        else
                        {
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            if ((fFinal <= fNoConclu && fFinal >= fVencer) || (fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                            {
                                item.RevisionEstudioEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.RevisionEstudioEstado = sEstado[1];
                            }
                            else
                            {
                                item.RevisionEstudioEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.RevisionEstudioEstado = sEstado[2];
                            }
                        }

                    }
                }

                #region Absolución de observaciones

                if (item.Revepolevobsfechaini.HasValue && estudioEPO.TipoConfig == 1) // Cálculo en base a PR20 (antiguo)
                {
                    nDiasLevObs = estudioEPO.Estepoplazolevobsporv.HasValue ? estudioEPO.Estepoplazolevobsporv.Value : 0;
                    nDiasLevObsVenc = estudioEPO.Estepoplazolevobsvenc.HasValue ? estudioEPO.Estepoplazolevobsvenc.Value : 0;

                    if (item.Revepolevobsfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        DateTime fFinal = DateTime.Now;
                        DateTime fFinalNoHabil = DateTime.Now;

                        if (item.Revepolevobsfechafin.HasValue)
                        {
                            if (item.Revepolevobsfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                fFinal = item.Revepolevobsfechafin.Value;
                                fFinalNoHabil = item.Revepolevobsfechafin.Value;
                            }
                        }

                        //vencido
                        //fechaAdd = item.LevantamientoObservacionFechaInicio.AddDays(nDiasLevObs);
                        //nDiasLevObs = this.docdiaRepository.DiasCalEntreFechas(item.LevantamientoObservacionFechaInicio, fechaAdd);
                        //fVencer = item.LevantamientoObservacionFechaInicio.AddDays(nDiasLevObs);
                        fVencer = item.Revepolevobsfechaini.Value.AddDays(nDiasLevObs);
                        fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabSinFS(item.Revepolevobsfechaini.Value, nDiasLevObs);

                        //por vencer
                        //fVencido = item.LevantamientoObservacionFechaFin.AddDays(nDiasEst3erInv);
                        //fechaAdd = item.LevantamientoObservacionFechaInicio.AddDays(nDiasEst3erInv);
                        //nDiasEst3erInv = this.docdiaRepository.DiasCalEntreFechas(item.LevantamientoObservacionFechaFin, fechaAdd);
                        //fNoConclu = item.LevantamientoObservacionFechaFin.AddDays(nDiasEst3erInv);
                        fNoConclu = item.Revepolevobsfechaini.Value.AddDays(nDiasLevObsVenc);
                        fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabSinFS(item.Revepolevobsfechaini.Value, nDiasLevObsVenc);

                        //if (DateTime.Now <= fVencer)
                        if (fFinal <= fVencer || fFinalNoHabil <= fVencerNoHabil)
                        {
                            item.LevantamientObservacionEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.LevantamientObservacionEstado = sEstado[0];
                        }
                        else
                        {
                            //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                            if ((fFinal <= fNoConclu && fFinal >= fVencer) || (fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                            {
                                item.LevantamientObservacionEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.LevantamientObservacionEstado = sEstado[1];
                            }
                            else
                            {
                                item.LevantamientObservacionEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.LevantamientObservacionEstado = sEstado[2];
                            }
                        }
                    }
                }
                else if(item.Revepolevobsfechaini.HasValue) // Cálculo en base a PR20 (nuevo)
                {
                    int nMesesLevObs;
                    int nMesesLevObsVenc;
                    DateTime fFinal = DateTime.Now;

                    nMesesLevObs = estudioEPO.Estepoplazolevobsporv.HasValue ? estudioEPO.Estepoplazolevobsporv.Value : 0;
                    nMesesLevObsVenc = estudioEPO.Estepoplazolevobsvenc.HasValue ? estudioEPO.Estepoplazolevobsvenc.Value : 0;

                    DateTime fFinLevObs = item.Revepolevobsfechaini.Value.AddMonths(nMesesLevObs);
                    DateTime fFinLevObsVenc = item.Revepolevobsfechaini.Value.AddMonths(nMesesLevObsVenc);

                    if (item.Revepolevobsfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        if(item.Revepolevobsfechafin.HasValue)
                            fFinal = item.Revepolevobsfechafin.Value; //Fecha Fin ingresada

                        //Validar y obtener día útil de F.Lev.Obs y F.Lev.Obs.Vencido
                        fFinLevObs = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObs.AddDays(-1), 1);
                        fFinLevObsVenc = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObsVenc.AddDays(-1), 1);

                        //Validamos si tiene días de ampliación
                        if (item.Revepopreampl > 0)
                        {
                            fFinLevObs = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObs, item.Revepopreampl);
                            fFinLevObsVenc = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObsVenc, item.Revepopreampl);
                        }

                        if (fFinal <= fFinLevObs)
                        {
                            item.LevantamientObservacionEstadoColor = sEstadoColores[0]; //verde #00FF00
                            item.LevantamientObservacionEstado = sEstado[0];
                        }
                        else
                        {
                            if (fFinal > fFinLevObs && fFinal <= fFinLevObsVenc)
                            {
                                item.LevantamientObservacionEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                item.LevantamientObservacionEstado = sEstado[1];
                            }
                            else
                            {
                                item.LevantamientObservacionEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                item.LevantamientObservacionEstado = sEstado[2];
                            }
                        }
                    }
                }

                #endregion
            }

            return listadoRevisionEPO;
        }

        #endregion

        #region Métodos Tabla EPO_ESTUDIOTERCEROINV_EO

        /// <summary>
        /// Inserta un registro de la tabla EPO_ESTUDIOTERCEROINV_EO
        /// </summary>
        public void SaveEpoEstudioTercerInvEo(EpoEstudioTerceroInvEoDTO entity)
        {
            try
            {
                FactorySic.GetEpoEstudioTerceroInvEoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_ESTUDIOTERCEROINV_EO
        /// </summary>
        public void DeleteEpoEstudioTercerInvEo(int esteocodi)
        {
            try
            {
                FactorySic.GetEpoEstudioTerceroInvEoRepository().Delete(esteocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_REVISION_EO
        /// </summary>
        public List<EpoEstudioTerceroInvEoDTO> GetByCriteriaEpoEstudioTercerInvEo(int esteocodi)
        {
            return FactorySic.GetEpoEstudioTerceroInvEoRepository().GetByCriteria(esteocodi);
        }

        public List<EpoEstudioEoDTO> ListFwUser()
        {
            return FactorySic.GetEpoEstudioEoRepository().ListFwUser();
        }

        #endregion

        #region Métodos Tabla EPO_ESTUDIOTERCEROINV_EPO

        /// <summary>
        /// Inserta un registro de la tabla EPO_ESTUDIOTERCEROINV_EPO
        /// </summary>
        public void SaveEpoEstudioTercerInvEpo(EpoEstudioTerceroInvEpoDTO entity)
        {
            try
            {
                FactorySic.GetEpoEstudioTerceroInvEpoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EPO_ESTUDIOTERCEROINV_EPO
        /// </summary>
        public void DeleteEpoEstudioTercerInvEpo(int estepocodi)
        {
            try
            {
                FactorySic.GetEpoEstudioTerceroInvEpoRepository().Delete(estepocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EPO_REVISION_EPO
        /// </summary>
        public List<EpoEstudioTerceroInvEpoDTO> GetByCriteriaEpoEstudioTercerInvEpo(int estepocodi)
        {
            return FactorySic.GetEpoEstudioTerceroInvEpoRepository().GetByCriteria(estepocodi);
        }

        #endregion

        public List<SiEmpresaDTO> ListarEmpresaTodo()
        {
            return FactorySic.GetSiEmpresaRepository().ListGeneral();
        }


        public void Procesar()
        {
            try
            {

                SiPlantillacorreoDTO siPlantillaCorreo = FactorySic.GetSiPlantillacorreoRepository().GetById(1);
                DateTime periodo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

                string mailTo = "";
                string empresa = "";
                int emprcodi = 0;

                mailTo = ConfigurationManager.AppSettings["SPNMailTo"];
                string fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString(ConstantesAppServicio.FormatoFecha);
                string asunto = string.Format(siPlantillaCorreo.Plantasunto);
                string contenido = this.ObtenerContenido(siPlantillaCorreo.Plantcontenido);

                COES.Base.Tools.Util.SendEmail(mailTo, asunto, contenido);//Enviar correo sin CCO

                SiCorreoDTO correo = new SiCorreoDTO();
                correo.Corrasunto = asunto;
                correo.Corrcontenido = contenido;
                correo.Corrfechaenvio = DateTime.Now;
                correo.Corrto = mailTo;
                correo.Emprcodi = emprcodi;
                correo.Corrfechaperiodo = periodo;
                correo.Plantcodi = siPlantillaCorreo.Plantcodi;
                FactorySic.GetSiCorreoRepository().Save(correo);
            }
            catch (Exception ex)
            {
                //log.Error("El proceso Alerta Equipamiento ejecutado a las  {0} ha sido fallido. "+DateTime.Now,ex);
                throw ex;
            }
        }

        public string ObtenerContenido(string plantilla)
        {
            int DiasHabilesRevisionCoes = Convert.ToInt32(ConfigurationManager.AppSettings["DiasHabilesRevisionCoes"]);
            int DiasHabilesVencRevisionCoes = Convert.ToInt32(ConfigurationManager.AppSettings["DiasHabilesVencRevisionCoes"]);
            int DiasHabilesTercerInvCoes = Convert.ToInt32(ConfigurationManager.AppSettings["DiasHabilesTercerInvCoes"]);
            int DiasHabilesVencTercerInvCoes = Convert.ToInt32(ConfigurationManager.AppSettings["DiasHabilesVencTercerInvCoes"]);

            List<EpoRevisionEpoDTO> listadoRevisionCoesEPO = FactorySic.GetEpoRevisionEpoRepository().GetByCriteriaRevisionEstudio(DiasHabilesRevisionCoes, DiasHabilesVencRevisionCoes);
            List<EpoRevisionEpoDTO> listadoRevisionTercerInvEPO = FactorySic.GetEpoRevisionEpoRepository().GetByCriteriaRevisionEstudio(DiasHabilesTercerInvCoes, DiasHabilesVencTercerInvCoes);
            List<EpoRevisionEoDTO> listadoRevisionCoesEO = FactorySic.GetEpoRevisionEoRepository().GetByCriteriaRevisionEstudio(DiasHabilesRevisionCoes, DiasHabilesVencRevisionCoes);
            List<EpoRevisionEoDTO> listadoRevisionTercerInvEO = FactorySic.GetEpoRevisionEoRepository().GetByCriteriaRevisionEstudio(DiasHabilesTercerInvCoes, DiasHabilesVencTercerInvCoes);

            StringBuilder sb = new StringBuilder();
            if (listadoRevisionCoesEO.Count > 0)
            {
                int c = 1;
                foreach (var item in listadoRevisionCoesEO)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteocodiusu));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteonomb));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", c.ToString()));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", string.Format("<a href='{1}' target='_blank'>{0}</a>", item.Reveorevcoescartarevisiontit, item.Reveorevcoescartarevisionenl)));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Reveorevcoesfechaini.Value.ToShortDateString()));
                    sb.AppendLine("</tr>");

                    c++;
                }

                plantilla = plantilla.Replace("{detalle_oe_rcoes}", sb.ToString());
                sb = new StringBuilder();
            }
            else
            {
                plantilla = plantilla.Replace("{detalle_oe_rcoes}", "");
            }

            if (listadoRevisionTercerInvEO.Count > 0)
            {
                int c = 1;
                foreach (var item in listadoRevisionTercerInvEO)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteocodiusu));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteonomb));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", c.ToString()));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", string.Format("<a href='{1}' target='_blank'>{0}</a>", item.Reveoenvesttercinvtit, item.Reveoenvesttercinvenl)));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Reveoenvesttercinvfechaini.Value.ToShortDateString()));
                    sb.AppendLine("</tr>");

                    c++;
                }

                plantilla = plantilla.Replace("{detalle_oe_eti}", sb.ToString());
                sb = new StringBuilder();
            }
            else
            {
                plantilla = plantilla.Replace("{detalle_oe_eti}", "");
            }

            if (listadoRevisionCoesEPO.Count > 0)
            {
                int c = 1;
                foreach (var item in listadoRevisionCoesEPO)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Estepocodiusu));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteponomb));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", c.ToString()));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", string.Format("<a href='{1}' target='_blank'>{0}</a>", item.Reveporevcoescartarevisiontit, item.Reveporevcoescartarevisionenl)));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Reveporevcoesfechaini.Value.ToShortDateString()));
                    sb.AppendLine("</tr>");

                    c++;
                }

                plantilla = plantilla.Replace("{detalle_ope_rcoes}", sb.ToString());
                sb = new StringBuilder();
            }
            else
            {
                plantilla = plantilla.Replace("{detalle_ope_rcoes}", "");
            }

            if (listadoRevisionTercerInvEPO.Count > 0)
            {
                int c = 1;
                foreach (var item in listadoRevisionTercerInvEPO)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Estepocodiusu));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteponomb));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", c.ToString()));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", string.Format("<a href='{1}' target='_blank'>{0}</a>", item.Revepoenvesttercinvtit, item.Revepoenvesttercinvenl)));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Revepoenvesttercinvfechaini.Value.ToShortDateString()));
                    sb.AppendLine("</tr>");

                    c++;
                }

                plantilla = plantilla.Replace("{detalle_ope_eti}", sb.ToString());
                sb = new StringBuilder();
            }
            else
            {
                plantilla = plantilla.Replace("{detalle_ope_eti}", "");
            }

            return plantilla;
        }

        #region Métodos Tabla EPO_ZONAS
        public List<EpoZonaDTO> ListarZona()
        {
            return FactorySic.GetEpoZonaRepository().List();
        }

        public EpoZonaDTO GetByIdEpoZonaEpo(int ZonCodi)
        {
            EpoZonaDTO zonaEPO = FactorySic.GetEpoZonaRepository().GetById(ZonCodi);
            return zonaEPO;
        }


        public EpoZonaDTO MostrarZonaXPunto(int PuntCodi)
        {
            EpoZonaDTO zonaEPO = FactorySic.GetEpoZonaRepository().GetByCriteria(PuntCodi);
            return zonaEPO;
        }
        #endregion


        #region Métodos Tabla EPO_PUNTO_CONEXION
        public List<EpoPuntoConexionDTO> ListarPuntoConexion()
        {
            return FactorySic.GetEpoPuntoConexionRepository().List();
        }

        public List<EpoPuntoConexionDTO> GetByCriteriaEpoPuntoConexionEpos(EpoPuntoConexionDTO estudioepo)
        {
            return FactorySic.GetEpoPuntoConexionRepository().GetByCriteria(estudioepo);
        }

        public int ObtenerNroRegistroBusquedaEpoPuntoConexionEpos(EpoPuntoConexionDTO estudioepo)
        {
            return FactorySic.GetEpoPuntoConexionRepository().ObtenerNroRegistroBusqueda(estudioepo);
        }

        public EpoPuntoConexionDTO GetByIdEpoPuntoConexionEpo(int estepocodi)
        {
            EpoPuntoConexionDTO estudioEPO = FactorySic.GetEpoPuntoConexionRepository().GetById(estepocodi);
            return estudioEPO;
        }

        public int SaveEpoPuntoConexionEpo(EpoPuntoConexionDTO entity)
        {
            try
            {
                //entity.Estepoacumdiascoes = entity.Estepoacumdiascoes.HasValue ? entity.Estepoacumdiascoes : 0;
                return FactorySic.GetEpoPuntoConexionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        public void UpdateEpoPuntoConexionEpo(EpoPuntoConexionDTO entity)
        {
            try
            {
                //entity.Estepoacumdiascoes = entity.Estepoacumdiascoes.HasValue ? entity.Estepoacumdiascoes : 0;
                FactorySic.GetEpoPuntoConexionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        public EpoPuntoConexionDTO GetByDescripcionEpoPuntoConexionEpo(string descripcion)
        {
            EpoPuntoConexionDTO estudioEPO = FactorySic.GetEpoPuntoConexionRepository().GetByCodigo(descripcion);
            return estudioEPO;
        }




        #endregion

        #region Mejoras EO-EPO-II
        public void ProcesosEoEpo()
        {
            try
            {
                string mailTo = "";
                int emprcodi = 0;
                string proceso = "";
                string asunto = "";
                string contenido = "";
                int nDiasLevObs;
                int nDiasLevObsVenc;
                DateTime fVencer;
                DateTime fNoConclu;
                DateTime fVencerNoHabil;
                DateTime fNoConcluNoHabil;
                string strFechaAnioIngreso = System.Configuration.ConfigurationManager.AppSettings["FechaAnioVigencia"];
                string strFechaAlertaVigencia = System.Configuration.ConfigurationManager.AppSettings["FechaVencimientoVigencia"]  + Convert.ToString(DateTime.Now.Year);
                string[] sEstadoColores = { "#51a351", "#f89406", "#bd362f" };

                DateTime fechaAnioIng = DateTime.Now;
                List<EpoEstudioEpoDTO> ListestudioEpo = new List<EpoEstudioEpoDTO>();
                List<EpoEstudioEpoDTO> ListestudioEpo36 = new List<EpoEstudioEpoDTO>();
                List<EpoEstudioEpoDTO> ListestudioEpo48 = new List<EpoEstudioEpoDTO>();
                List<EpoEstudioEoDTO> ListestudioEo = new List<EpoEstudioEoDTO>();
                List<DocDiaEspDTO> diasFeriados = new List<DocDiaEspDTO>();
                List<EpoRevisionEpoDTO> ListEposExcAbsObs = new List<EpoRevisionEpoDTO>();
                List<EpoRevisionEoDTO> ListEosExcAbsObs = new List<EpoRevisionEoDTO>();

                SiPlantillacorreoDTO siPlantillaCorreo = FactorySic.GetSiPlantillacorreoRepository().GetById(108);
                DateTime periodo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                mailTo = string.Format(siPlantillaCorreo.Planticorreos); //Correo de destinatario

                ListEposExcAbsObs = FactorySic.GetEpoRevisionEpoRepository().ListEposExcAbsObs(); //Listado de Epos para proceso de actualización de Estado a Rechazado / Concluido sin aprobación PR20
                ListEosExcAbsObs = FactorySic.GetEpoRevisionEoRepository().ListEosExcAbsObs(); //Listado de Eos para proceso de actualización de Estado a Rechazado / Concluido sin aprobación PR20
                
                ListestudioEpo = FactorySic.GetEpoEstudioEpoRepository().ListVigenciaAnioIngreso(strFechaAnioIngreso); //Alerta EPO que llega 3 meses de último año de vigencia.
                ListestudioEpo36 = FactorySic.GetEpoEstudioEpoRepository().ListVigencia36Meses(); //Alerta EPO por 36 Meses después de fecha de conformidad.
                ListestudioEpo48 = FactorySic.GetEpoEstudioEpoRepository().ListVigencia48Meses(); //Alerta EPO por 48 Meses después de fecha de conformidad.            

                #region Actualiza Estados EO/EPOS
                //Actualiza Estado de Epos según PR20
                if (ListEposExcAbsObs.Count > 0)
                {
                    foreach(EpoRevisionEpoDTO item in ListEposExcAbsObs)
                    {
                        EpoEstudioEpoDTO estudioEPO = FactorySic.GetEpoEstudioEpoRepository().GetById(item.Estepocodi);
                       
                        if (item.Revepolevobsfechaini.HasValue && estudioEPO.TipoConfig == 1) // Cálculo en base a PR20 (antiguo)
                        {
                            nDiasLevObs = estudioEPO.Estepoplazolevobsporv.HasValue ? estudioEPO.Estepoplazolevobsporv.Value : 0;
                            nDiasLevObsVenc = estudioEPO.Estepoplazolevobsvenc.HasValue ? estudioEPO.Estepoplazolevobsvenc.Value : 0;

                            if (item.Revepolevobsfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                DateTime fFinal = DateTime.Now;
                                DateTime fFinalNoHabil = DateTime.Now;

                                if (item.Revepolevobsfechafin.HasValue)
                                {
                                    if (item.Revepolevobsfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                                    {
                                        fFinal = item.Revepolevobsfechafin.Value;
                                        fFinalNoHabil = item.Revepolevobsfechafin.Value;
                                    }
                                }
                                fVencer = item.Revepolevobsfechaini.Value.AddDays(nDiasLevObs);
                                fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabSinFS(item.Revepolevobsfechaini.Value, nDiasLevObs);

                                fNoConclu = item.Revepolevobsfechaini.Value.AddDays(nDiasLevObsVenc);
                                fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabSinFS(item.Revepolevobsfechaini.Value, nDiasLevObsVenc);

                                //if (DateTime.Now <= fVencer)
                                if (fFinal <= fVencer || fFinalNoHabil <= fVencerNoHabil)
                                {
                                    item.LevantamientObservacionEstadoColor = sEstadoColores[0]; //verde #00FF00
                                }
                                else
                                {
                                    //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                                    if ((fFinal <= fNoConclu && fFinal >= fVencer) || (fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                                    {
                                        item.LevantamientObservacionEstadoColor = sEstadoColores[1]; //ambar    FFBF00                                     
                                    }
                                    else
                                    {
                                        item.LevantamientObservacionEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                        estudioEPO.Estacodi = 2;
                                        FactorySic.GetEpoEstudioEpoRepository().Update(estudioEPO);
                                    }
                                }
                            }
                        }
                        else if (item.Revepolevobsfechaini.HasValue) // Cálculo en base a PR20 (nuevo)
                        {
                            int nMesesLevObs;
                            int nMesesLevObsVenc;
                            DateTime fFinal = DateTime.Now;

                            nMesesLevObs = estudioEPO.Estepoplazolevobsporv.HasValue ? estudioEPO.Estepoplazolevobsporv.Value : 0;
                            nMesesLevObsVenc = estudioEPO.Estepoplazolevobsvenc.HasValue ? estudioEPO.Estepoplazolevobsvenc.Value : 0;

                            DateTime fFinLevObs = item.Revepolevobsfechaini.Value.AddMonths(nMesesLevObs);
                            DateTime fFinLevObsVenc = item.Revepolevobsfechaini.Value.AddMonths(nMesesLevObsVenc);

                            if (item.Revepolevobsfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                if (item.Revepolevobsfechafin.HasValue)
                                    fFinal = item.Revepolevobsfechafin.Value; //Fecha Fin ingresada

                                //Validar y obtener día útil de F.Lev.Obs y F.Lev.Obs.Vencido
                                fFinLevObs = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObs.AddDays(-1), 1);
                                fFinLevObsVenc = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObsVenc.AddDays(-1), 1);

                                //Validamos si tiene días de ampliación
                                if (item.Revepopreampl > 0)
                                {
                                    fFinLevObs = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObs, item.Revepopreampl);
                                    fFinLevObsVenc = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObsVenc, item.Revepopreampl);
                                }

                                if (fFinal > fFinLevObsVenc && fFinal > fFinLevObsVenc)
                                {
                                    estudioEPO.Estacodi = 10;
                                    FactorySic.GetEpoEstudioEpoRepository().Update(estudioEPO);
                                }

                            }
                        }
                    }
                }

                //Actualiza Estado de Eos según PR20
                if(ListEosExcAbsObs.Count > 0)
                {
                    foreach (EpoRevisionEoDTO item in ListEosExcAbsObs)
                    {
                        EpoEstudioEoDTO estudioEO = FactorySic.GetEpoEstudioEoRepository().GetById(item.Esteocodi);
                        if (item.Reveolevobsfechaini.HasValue && estudioEO.TipoConfig == 1) // Cálculo en base a PR20 (antiguo)
                        {
                            nDiasLevObs = estudioEO.Esteoplazolevobsporv.HasValue ? estudioEO.Esteoplazolevobsporv.Value : 0;
                            nDiasLevObsVenc = estudioEO.Esteoplazolevobsvenc.HasValue ? estudioEO.Esteoplazolevobsvenc.Value : 0;

                            if (item.Reveolevobsfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                DateTime fFinal = DateTime.Now;
                                DateTime fFinalNoHabil = DateTime.Now;

                                if (item.Reveolevobsfechafin.HasValue)
                                {
                                    if (item.Reveolevobsfechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                                    {
                                        fFinal = item.Reveolevobsfechafin.Value;
                                        fFinalNoHabil = item.Reveolevobsfechafin.Value;
                                    }
                                }
                                fVencer = item.Reveolevobsfechaini.Value.AddDays(nDiasLevObs);
                                fVencerNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabSinFS(item.Reveolevobsfechaini.Value, nDiasLevObs);
                                fNoConclu = item.Reveolevobsfechaini.Value.AddDays(nDiasLevObsVenc);
                                fNoConcluNoHabil = FactorySic.GetEpoDocDiaNoHabilRepository().ObtenerDiasNoHabSinFS(item.Reveolevobsfechaini.Value, nDiasLevObsVenc);

                                //if (DateTime.Now <= fVencer)
                                if (fFinal <= fVencer || fFinalNoHabil <= fVencerNoHabil)
                                {
                                    item.LevantamientObservacionEstadoColor = sEstadoColores[0]; //verde #00FF00
                                }
                                else
                                {
                                    //if (DateTime.Now <= fNoConclu && DateTime.Now >= fVencer)
                                    if ((fFinal <= fNoConclu && fFinal >= fVencer) || (fFinalNoHabil <= fNoConcluNoHabil && fFinalNoHabil >= fVencerNoHabil))
                                    {
                                        item.LevantamientObservacionEstadoColor = sEstadoColores[1]; //ambar    FFBF00
                                    }
                                    else
                                    {
                                        item.LevantamientObservacionEstadoColor = sEstadoColores[2]; //rojo #FF0000
                                        estudioEO.Estacodi = 2;
                                        FactorySic.GetEpoEstudioEoRepository().Update(estudioEO);
                                    }
                                }
                            }
                        }

                        else if (item.Reveolevobsfechaini.HasValue) // Cálculo en base a PR20 (nuevo)
                        {
                            int nMesesLevObs;
                            int nMesesLevObsVenc;
                            DateTime fFinal = DateTime.Now;

                            nMesesLevObs = estudioEO.Esteoplazolevobsporv.HasValue ? estudioEO.Esteoplazolevobsporv.Value : 0;
                            nMesesLevObsVenc = estudioEO.Esteoplazolevobsvenc.HasValue ? estudioEO.Esteoplazolevobsvenc.Value : 0;

                            DateTime fFinLevObs = item.Reveolevobsfechaini.Value.AddMonths(nMesesLevObs);
                            DateTime fFinLevObsVenc = item.Reveolevobsfechaini.Value.AddMonths(nMesesLevObsVenc);

                            if (item.Reveolevobsfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                            {
                                if (item.Reveolevobsfechafin.HasValue)
                                    fFinal = item.Reveolevobsfechafin.Value; //Fecha Fin ingresada

                                //Validar y obtener día útil de F.Lev.Obs y F.Lev.Obs.Vencido
                                fFinLevObs = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObs.AddDays(-1), 1);
                                fFinLevObsVenc = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObsVenc.AddDays(-1), 1);

                                //Validamos si tiene días de ampliación
                                if (item.Reveopreampl > 0)
                                {
                                    fFinLevObs = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObs, item.Reveopreampl);
                                    fFinLevObsVenc = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fFinLevObsVenc, item.Reveopreampl);
                                }

                                if (fFinal > fFinLevObsVenc)
                                {
                                    if (fFinal < fFinLevObs && fFinal > fFinLevObsVenc)
                                    {
                                        estudioEO.Estacodi = 10;
                                        FactorySic.GetEpoEstudioEoRepository().Update(estudioEO);
                                    }                                      
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Envío de correos
                //string Fch = strFechaAlertaVigencia + "/" + Convert.ToString(DateTime.Now.Year);
                //DateTime FechaFinal = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(Convert.ToDateTime(strFechaAlertaVigencia).AddDays(-1), 1);
                string hoy = DateTime.Now.ToString("dd/MM/yyyy");
                if (ListestudioEpo.Count > 0 && strFechaAlertaVigencia == hoy)
                {
                    ListestudioEo = null;
                    proceso = "EPO";
                    string causal = "Faltan 3 meses para que culmine el año posterior al año de puesta en servicio";
                    asunto = string.Format(siPlantillaCorreo.Plantasunto) + proceso;
                    contenido = this.ObtenerVigencia(siPlantillaCorreo.Plantcontenido, ListestudioEpo, ListestudioEo, causal);
                    COES.Base.Tools.Util.SendEmail(mailTo, asunto, contenido);//Enviar correo sin CCO

                    SiCorreoDTO correo = new SiCorreoDTO();
                    correo.Corrasunto = asunto;
                    correo.Corrcontenido = contenido;
                    correo.Corrfechaenvio = DateTime.Now;
                    correo.Corrto = mailTo;
                    correo.Emprcodi = emprcodi;
                    correo.Corrfechaperiodo = periodo;
                    correo.Plantcodi = siPlantillaCorreo.Plantcodi;
                    FactorySic.GetSiCorreoRepository().Save(correo);
                }

                if (ListestudioEpo36.Count > 0)
                {
                    ListestudioEo = null;
                    proceso = "EPO";
                    string causal = "Faltan 3 meses para que se venza el plazo de 36 meses para obtener la certificación ambiental";
                    asunto = string.Format(siPlantillaCorreo.Plantasunto) + proceso;
                    contenido = this.ObtenerVigencia(siPlantillaCorreo.Plantcontenido, ListestudioEpo36, ListestudioEo, causal);
                    COES.Base.Tools.Util.SendEmail(mailTo, asunto, contenido);//Enviar correo sin CCO

                    SiCorreoDTO correo = new SiCorreoDTO();
                    correo.Corrasunto = asunto;
                    correo.Corrcontenido = contenido;
                    correo.Corrfechaenvio = DateTime.Now;
                    correo.Corrto = mailTo;
                    correo.Emprcodi = emprcodi;
                    correo.Corrfechaperiodo = periodo;
                    correo.Plantcodi = siPlantillaCorreo.Plantcodi;
                    FactorySic.GetSiCorreoRepository().Save(correo);
                }

                if (ListestudioEpo48.Count > 0)
                {
                    ListestudioEo = null;
                    proceso = "EPO";
                    string causal = "Faltan 3 meses para que se venza el plazo de 48 meses para obtener la certificación ambiental";
                    asunto = string.Format(siPlantillaCorreo.Plantasunto) + proceso;
                    contenido = this.ObtenerVigencia(siPlantillaCorreo.Plantcontenido, ListestudioEpo48, ListestudioEo, causal);
                    COES.Base.Tools.Util.SendEmail(mailTo, asunto, contenido);//Enviar correo sin CCO

                    SiCorreoDTO correo = new SiCorreoDTO();
                    correo.Corrasunto = asunto;
                    correo.Corrcontenido = contenido;
                    correo.Corrfechaenvio = DateTime.Now;
                    correo.Corrto = mailTo;
                    correo.Emprcodi = emprcodi;
                    correo.Corrfechaperiodo = periodo;
                    correo.Plantcodi = siPlantillaCorreo.Plantcodi;
                    FactorySic.GetSiCorreoRepository().Save(correo);
                }

                ListestudioEo = FactorySic.GetEpoEstudioEoRepository().ListVigencia12Meses(); //Alerta EO por 12 Meses después de fecha de conformidad.
                if (ListestudioEo.Count > 0)
                {
                    ListestudioEpo48 = null;
                    proceso = "EO";
                    string causal = "Se venció el plazo de 12 meses de vigencia del estudio";
                    asunto = string.Format(siPlantillaCorreo.Plantasunto) + proceso;
                    contenido = this.ObtenerVigencia(siPlantillaCorreo.Plantcontenido, ListestudioEpo48, ListestudioEo, causal);
                    COES.Base.Tools.Util.SendEmail(mailTo, asunto, contenido);//Enviar correo sin CCO

                    SiCorreoDTO correo = new SiCorreoDTO();
                    correo.Corrasunto = asunto;
                    correo.Corrcontenido = contenido;
                    correo.Corrfechaenvio = DateTime.Now;
                    correo.Corrto = mailTo;
                    correo.Emprcodi = emprcodi;
                    correo.Corrfechaperiodo = periodo;
                    correo.Plantcodi = siPlantillaCorreo.Plantcodi;
                    FactorySic.GetSiCorreoRepository().Save(correo);
                }
                #endregion
            }
            catch (Exception ex)
            {
                //log.Error("El proceso Alerta Equipamiento ejecutado a las  {0} ha sido fallido. "+DateTime.Now,ex);
                throw ex;
            }
        }

        public string ObtenerVigencia(string plantilla, List<EpoEstudioEpoDTO> ListestudioEpo, List<EpoEstudioEoDTO> ListestudioEo, string causal)
        {

            StringBuilder sb = new StringBuilder();
            if (ListestudioEpo != null && ListestudioEpo.Count > 0)
            {
                foreach (var item in ListestudioEpo)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteponomb));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Estepoanospuestaservicio));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Estepopuntoconexion));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", causal));
                    sb.AppendLine("</tr>");
                }

                plantilla = plantilla.Replace("{detalle_oe_rcoes}", sb.ToString());
                //sb = new StringBuilder();
            }
            //else
            //{
            //    plantilla = plantilla.Replace("{detalle_oe_rcoes}", "");
            //}

            if (ListestudioEo != null && ListestudioEo.Count > 0)
            {
                sb = new StringBuilder();
                foreach (var item in ListestudioEo)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteonomb));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteoanospuestaservicio));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", item.Esteopuntoconexion));
                    sb.AppendLine(string.Format("<td style='text-align:center; padding: 5px'>{0}</td>", causal));
                    sb.AppendLine("</tr>");
                }

                plantilla = plantilla.Replace("{detalle_oe_rcoes}", sb.ToString());
                //sb = new StringBuilder();
            }
            //else
            //{
            //    plantilla = plantilla.Replace("{detalle_oe_rcoes}", "");
            //}



            return plantilla;
        }

        public List<EpoEstudioEstadoDTO> GetByCriteriaEpoEstudioEstadosVigencia()
        {
            return FactorySic.GetEpoEstudioEstadoRepository().GetByCriteriaEstadosVigencia();
        }


        #endregion
    }
}
