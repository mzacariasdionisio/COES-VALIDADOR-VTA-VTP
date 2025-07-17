using COES.Base.Core;
using log4net;
using System;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.RegistroIntegrantes
{
    /// <summary>
    /// Clases con métodos del módulo RegistroIntegrantes
    /// </summary>
    public class EmpresaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EmpresaAppServicio));

        #region Métodos Tabla SI_EMPRESA

        /// <summary>
        /// Permite obtener un registro de la tabla SI_EMPRESA
        /// </summary>
        public SiEmpresaDTO GetByIdSiEmpresa(int emprcodi)
        {
            return FactorySic.GetSiEmpresaRIRepository().GetByIdGestionModificacion(emprcodi);
        }

        /// <summary>
        /// Permite obtener un registro de flujos de SGDOC 
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        public List<SiEmpresaDTO> ListarFlujoEmpresa(int emprcodi)
        {
            return FactorySic.GetSiEmpresaRIRepository().ListarFlujoEmpresa(emprcodi);
        }

        /// <summary>
        /// Permite obtener un registro de flujos de SGDOC 
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        public List<SiEmpresaDTO> ListarFlujoEmpresaSolicitud(int emprcodi, int solicodi)
        {
            return FactorySic.GetSiEmpresaRIRepository().ListarFlujoEmpresaSolicitud(emprcodi, solicodi);
        }

        /// <summary>
        /// Actualiza los datos de identificación permitidos de un registro de la tabla SI_EMPRESA
        /// </summary>
        //public void UpdateSiEmpresa(SiEmpresaDTO entity)
        //{
        //    try
        //    {
        //        FactorySic.GetSiEmpresaRepository().Update(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ConstantesAppServicio.LogError, ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Actualiza los datos de identificación permitidos de un registro de la tabla SI_EMPRESA
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <param name="domicilioLegal"></param>
        /// <param name="telefono"></param>
        /// <param name="fax"></param>
        /// <param name="paginaWeb"></param>
        /// <returns></returns>
        public int ActualizarEmpresaGestionModificacion(int idEmpresa, string nombreComercial, string razonSocial,
            string domicilioLegal, string sigla, string nroPartida, string telefono, string fax, string paginaWeb, string nroRegistro)
        {
            try
            {
                return FactorySic.GetSiEmpresaRIRepository().ActualizarEmpresaGestionModificacion(idEmpresa, nombreComercial, razonSocial,
                    domicilioLegal, sigla, nroPartida, telefono, fax, paginaWeb, nroRegistro);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int ActualizarEmpresaGestionModificacion(int idEmpresa, string domicilioLegal, string telefono, string fax, string paginaWeb)
        {
            try
            {
                return FactorySic.GetSiEmpresaRIRepository().ActualizarEmpresaGestionModificacion(idEmpresa, domicilioLegal, telefono, fax, paginaWeb);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int ActualizarEmpresaCambioDenom(int idEmpresa, string nombreComercial, string razonSocial, string sigla)
        {
            try
            {
                return FactorySic.GetSiEmpresaRIRepository().ActualizarEmpresaCambioDenom(idEmpresa, nombreComercial, razonSocial,sigla);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite hacer una consulta de los datos de la empresa
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        public BeanEmpresa ConsultarPorRUC(string ruc)
        {
            return (new ServicioSunat()).ObtenerDatosSunat(ruc);
        }

        /// <summary>
        ///  Permite validar que el ruc y el nombre no existan
        /// </summary>
        /// <param name="nombreEmpresa">nombre de empresa</param>
        /// <param name="rucEmpresa">nombre ruc</param>
        /// <returns></returns>
        public bool ValidaEmpresa(string nombreEmpresa, string rucEmpresa)
        {
            try
            {
                bool flagEmpresa = FactorySic.GetSiEmpresaRepository().VerificarExistenciaPorNombre(nombreEmpresa);
                bool flagRuc = FactorySic.GetSiEmpresaRepository().VerificarExistenciaPorRuc(rucEmpresa);

                if (!flagEmpresa && !flagRuc)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
