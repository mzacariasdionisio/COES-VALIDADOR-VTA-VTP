using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using log4net;
using System;
using System.Collections.Generic;
using COES.Servicios.Aplicacion.Evaluacion.Helper;

namespace COES.Servicios.Aplicacion.Evaluacion
{
    /// <summary>
    /// Clases con métodos de calculo de formulas
    /// </summary>
    public class CalculosAppServicio : AppServicioBase
    {

    

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LineaAppServicio));


        #region GESPROTEC

        /// <summary>
        ///  Devuelve una lista de parametros y formulas asociados a Linea
        /// </summary>
        /// <param name="equipo"></param>
        /// <param name="flgOrigen"></param>
        /// <returns></returns>
        public List<EprCalculosDTO> ListCalculosFormulasLinea(EprEquipoDTO equipo, int flgOrigen)
        {
            return FactorySic.GetEprCalculosRepository().ListCalculosFormulasLinea(equipo, flgOrigen);
        }

        /// <summary>
        /// Devuelve una lista de parametros y formulas asociados a Reactor
        /// </summary>
        /// <param name="equipo"></param>
        /// <param name="flgOrigen"></param>
        /// <returns></returns>
        public List<EprCalculosDTO> ListCalculosFormulasReactor(EprEquipoDTO equipo, int flgOrigen)
        {
            return FactorySic.GetEprCalculosRepository().ListCalculosFormulasReactor(equipo, flgOrigen);
        }

        /// <summary>
        /// Devuelve una lista de parametros y formulas asociados a Celda Acoplamiento
        /// </summary>
        /// <param name="equipo"></param>
        /// <param name="flgOrigen"></param>
        /// <returns></returns>
        public List<EprCalculosDTO> ListCalculosFormulasCelda(EprEquipoDTO equipo, int flgOrigen)
        {
            return FactorySic.GetEprCalculosRepository().ListCalculosFormulasCelda(equipo, flgOrigen);
        }

        /// <summary>
        /// Devuelve una lista de parametros y formulas asociados al Transformador de acuerdo al valor de FamCodigo
        /// </summary>
        /// <param name="equipo"></param>       
        /// <returns></returns>
        public List<EprCalculosDTO> ListCalculosFormulasTransformadores(EprEquipoDTO equipo)
        {
            var listaCalculosTransformadores = new List<EprCalculosDTO>();

            switch (Convert.ToInt32(equipo.FamCodigo))
            {
                case ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS:
                    {
                        listaCalculosTransformadores = FactorySic.GetEprCalculosRepository().ListCalculosTransformadorDosDevanados(equipo);
                        break;
                    }
                case ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS:
                    {
                        listaCalculosTransformadores = FactorySic.GetEprCalculosRepository().ListCalculosTransformadorTresDevanados(equipo);
                        break;
                    }
                case ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS:
                    {
                        listaCalculosTransformadores = FactorySic.GetEprCalculosRepository().ListCalculosTransformadorCuatroDevanados(equipo);
                        break;
                    }
            }

            return listaCalculosTransformadores;
        }

        /// <summary>
        /// Permite evaluar la formula Celda Posición Nucleo
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public static double? EvaluarCeldaPosicionNucleo(int equicodi)
        {
            return FactorySic.GetEprCalculosRepository().EvaluarCeldaPosicionNucleo(equicodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public static double? EvaluarCeldaPickUp(int equicodi)
        {
            return FactorySic.GetEprCalculosRepository().EvaluarCeldaPickUp(equicodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="tipoPropiedad"></param>
        /// <returns></returns>
        public static double? EvaluarPropiedadEquipo(int equicodi, string tipoPropiedad)
        {
            return FactorySic.GetEprCalculosRepository().EvaluarPropiedadEquipo(equicodi, tipoPropiedad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public static double? EvaluarTensionEquipo(int equicodi)
        {
            return FactorySic.GetEprCalculosRepository().EvaluarTensionEquipo(equicodi);
        }

        /// <summary>
        /// Lista las funciones base para editar una formula
        /// </summary>
        /// <returns></returns>
        public List<EprPropCatalogoDataDTO> ListFunciones()
        {
            return FactorySic.GetEprCalculosRepository().ListFunciones();
        }

        /// <summary>
        ///  Lista las propiedades para editar una formula
        /// </summary>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqPropiedadDTO> ListPropiedades(int famcodi)
        {
            return FactorySic.GetEprCalculosRepository().ListPropiedades(famcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="famcodi"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        public List<EprCalculosDTO> ListValidarFormulas(int famcodi, string formula)
        {
            return FactorySic.GetEprCalculosRepository().ListValidarFormulas(famcodi, formula);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaCodigosEquipo"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EprCalculosDTO> ListCalculosLineaMasivo(string listaCodigosEquipo, int famcodi)
        {
            return FactorySic.GetEprCalculosRepository().ListCalculosFormulasLineaMasivo(listaCodigosEquipo, famcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipo"></param>
        /// <returns></returns>
        public List<EprCalculosDTO> ListCalculosFormulasInterruptor(EprEquipoDTO equipo)
        {
            return FactorySic.GetEprCalculosRepository().ListCalculosFormulasInterruptor(equipo);
        }

        #endregion

    }
}
