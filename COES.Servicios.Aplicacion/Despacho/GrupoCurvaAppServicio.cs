using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace COES.Servicios.Aplicacion.Despacho
{
    /// <summary>
    /// Clase para manejo de logica de agrupar despacho
    /// </summary>
    public class GrupoCurvaAppServicio
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GrupoCurvaAppServicio));


        /// <summary>
        /// Permite obtener los grupos de curva de consumo
        /// </summary>
        /// <returns></returns>
        public List<PrCurvaDTO> ObtenerGruposCurva()
        {

            return FactorySic.GetPrAgruparCurvaRepository().List();
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_CURVA
        /// </summary>
        public int Insertar(PrCurvaDTO entity)
        {
            try
            {
                return FactorySic.GetPrAgruparCurvaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        // <summary>
        /// Permite actualizar grupo curva
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="tipo"></param>
        public void Actualizar(PrCurvaDTO entity)
        {
            try
            {
                FactorySic.GetPrAgruparCurvaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        // <summary>
        /// Permite actualizar grupo curva
        /// </summary>
        /// <param name="codigoCurva"></param>
        /// <param name="codigoGrupo"></param>
        public void AgregarDetalle(int codigoCurva, int codigoGrupo, bool principal)
        {
            try
            {
                FactorySic.GetPrAgruparCurvaRepository().AddDetail(codigoCurva, codigoGrupo);
                if (principal)
                {
                    //actualizar en pr_curva el curvgrupocurvaprincial al codigo de curva
                    FactorySic.GetPrAgruparCurvaRepository().UpdatePrincipal(codigoCurva, codigoGrupo);
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla PR_CURVA
        /// </summary>
        public PrCurvaDTO GetById(int CurvCodi)
        {
            return FactorySic.GetPrAgruparCurvaRepository().GetById(CurvCodi);
        }


        /// <summary>
        /// Elimina un registro de la tabla PR_CURVA
        /// </summary>
        public void Delete(int CurvCodi)
        {
            try
            {
                FactorySic.GetPrAgruparCurvaRepository().Delete(CurvCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de detalle 
        /// </summary>
        public void DeleteDetail(int CurvCodi, int GrupoCodi)
        {
            try
            {
                FactorySic.GetPrAgruparCurvaRepository().DeleteDetail(CurvCodi, GrupoCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener las centrales por tipo
        /// </summary>
        /// <param name="tipoGrupo"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ObtenerCentrales(string tipoGrupo)
        {
            return FactorySic.GetPrGrupoRepository().ObtenerCentralesPorTipoCurva(tipoGrupo);
        }

        /// <summary>
        /// Permite obtener grupos por codigo grupo
        /// </summary>
        /// <param name="codigoGrupo"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ObtenerGrupo(string codigoGrupo)
        {
            return FactorySic.GetPrGrupoRepository().ObtenerGruposPorCodigo(codigoGrupo);
        }

        /// <summary>
        /// Permite obtener el detalle de los grupos de curva de consumo
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ObtenerDetalleGrupoCurva(int CurvCodi)
        {

            return FactorySic.GetPrGrupoRepository().ListarDetalleGrupoCurva(CurvCodi);
        }
    }

}
