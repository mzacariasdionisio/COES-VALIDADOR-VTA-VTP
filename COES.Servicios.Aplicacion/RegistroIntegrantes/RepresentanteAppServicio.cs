using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;


namespace COES.Servicios.Aplicacion.RegistroIntegrantes
{
    public class RepresentanteAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EmpresaAppServicio));

        #region Métodos Tabla SI_REPRESENTANTE (Legal)

        /// <summary>
        /// Permite listar los registros de una empresa de la tabla SI_REPRESENTANTE
        /// </summary>
        public List<SiRepresentanteDTO> GetByEmpresaSiRepresentante(int emprcodi)
        {
            return FactorySic.GetSiRepresentanteRepository().GetByEmprcodi(emprcodi);
            
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_REPRESENTANTE
        /// </summary>
        public SiRepresentanteDTO GetByIdSiRepresentante(int rptecodi)
        {
            return FactorySic.GetSiRepresentanteRepository().GetById(rptecodi);
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_REPRESENTANTE
        /// </summary>
        public void UpdateSiRepresentante(SiRepresentanteDTO entity)
        {
            try
            {
                FactorySic.GetSiRepresentanteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DarBajaRepresentante(int idRepresentante, string usuario)
        {
            try
            {
                FactorySic.GetSiRepresentanteRepository().DarBajaRepresentante(idRepresentante, usuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_REPRESENTANTE
        /// </summary>
        public void DeleteSiRepresentante(int rptecodi)
        {
            try
            {
                FactorySic.GetSiRepresentanteRepository().Delete(rptecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_REPRESENTANTE
        /// </summary>
        public int InsertSiRepresentante(SiRepresentanteDTO entity)
        {
            try
            {
                return FactorySic.GetSiRepresentanteRepository().Save(entity);                
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de identificación permitidos de un registro de la tabla SI_EMPRESA
        /// </summary>
        /// <param name="idRepresentante"></param>
        /// <param name="telefono"></param>
        /// <param name="telefonoMovil"></param>
        /// <returns></returns>
        public int ActualizarRepresentanteGestionModificacion(int idRepresentante,
            string tipoRepresentante,
            string dni,
            string nombre,
            string apellido,
            string cargo,
            string telefono,
            string telefonoMovil,
            DateTime? fechaVigenciaPoder, string usuario, string email)
        {
            try
            {
                return FactorySic.GetSiRepresentanteRepository().ActualizarRepresentanteGestionModificacion(idRepresentante,
                                tipoRepresentante,
                                dni,
                                nombre,
                                apellido,
                                cargo,
                                telefono,
                                telefonoMovil,
                                fechaVigenciaPoder, usuario, email);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de identificación permitidos de un registro de la tabla SI_EMPRESA
        /// </summary>
        /// <param name="idRepresentante"></param>
        /// <param name="telefono"></param>
        /// <param name="telefonoMovil"></param>
        /// <returns></returns>
        public int ActualizarRepresentanteGestionModificacion(int idRepresentante, string telefono, string telefonoMovil, DateTime fechaVigenciaPoder)
        {
            try
            {
                return FactorySic.GetSiRepresentanteRepository().ActualizarRepresentanteGestionModificacionVigenciaPoder(idRepresentante, telefono, telefonoMovil, fechaVigenciaPoder);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int ActualizarRepresentanteGestionModificacion(int idRepresentante, string telefono, string telefonoMovil)
        {
            try
            {
                return FactorySic.GetSiRepresentanteRepository().ActualizarRepresentanteGestionModificacion(idRepresentante, telefono, telefonoMovil);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion
    }
}
