using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.GMM
{
    public class IncumplimientoAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IncumplimientoAppServicio));


        /// <summary>
        /// Inserta un registro de la tabla ...
        /// </summary>
        public int Save(GmmIncumplimientoDTO entity)
        {
            try
            {

                //using (TransactionScope trx =)
                //{
                int _inccodi = FactorySic.GetGmmIncumplimientoRepository().Save(entity);
                return _inccodi;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ...
        /// </summary>
        public void Update(GmmIncumplimientoDTO entity)
        {
            try
            {
                FactorySic.GetGmmIncumplimientoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ...
        /// </summary>
        public void UpdateTrienio(GmmIncumplimientoDTO entity)
        {
            try
            {
                FactorySic.GetGmmIncumplimientoRepository().UpdateTrienio(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Elimina un registro de la tabla ...
        /// </summary>
        public void Delete(int empgcodi)
        {
            try
            {
                FactorySic.GetGmmIncumplimientoRepository().Delete(empgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ...
        /// </summary>
        public GmmIncumplimientoDTO GetById(int empgcodi)
        {
            return FactorySic.GetGmmIncumplimientoRepository().GetById(empgcodi);
        }

        /// <summary>
        /// Permite obtener los datos de una empresa para edición
        /// </summary>
        public GmmIncumplimientoDTO GetByIdEdit(int incucodi)
        {
            return FactorySic.GetGmmIncumplimientoRepository().GetByIdEdit(incucodi);
        }

        /// <summary>
        /// ListarFiltroIncumplimientoAfectada
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="razonSocial"></param>
        /// <param name="numDocumento"></param>
        /// <returns></returns>
        public List<GmmIncumplimientoDTO> ListarFiltroIncumplimientoAfectada(int anio, string mes, string razonSocial, string numDocumento)
        {
            mes = string.IsNullOrEmpty(mes) ? "-" : mes;
            razonSocial = string.IsNullOrEmpty(razonSocial) ? "-" : razonSocial;
            numDocumento = string.IsNullOrEmpty(numDocumento) ? "-" : numDocumento;

            return FactorySic.GetGmmIncumplimientoRepository().ListarFiltroIncumplimientoAfectada(anio, mes, razonSocial, numDocumento);
        }
        /// <summary>
        /// ListarAgentes
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="razonSocial"></param>
        /// <param name="numDocumento"></param>
        /// <returns></returns>
        public List<GmmIncumplimientoDTO> ListarFiltroIncumplimientoDeudora(int anio, string mes, string razonSocial, string numDocumento)
        {
            mes = string.IsNullOrEmpty(mes) ? "-" : mes;
            razonSocial = string.IsNullOrEmpty(razonSocial) ? "-" : razonSocial;
            numDocumento = string.IsNullOrEmpty(numDocumento) ? "-" : numDocumento;
            return FactorySic.GetGmmIncumplimientoRepository().ListarFiltroIncumplimientoDeudora(anio, mes, razonSocial, numDocumento);
        }


        public List<GmmEmpresaDTO> ListarMaestroEmpresaCliente(string empresa, int tipoEmpresa, string estadoRegistro)
        {
            //return FactorySic.GetSiEmpresaRepository().ListaEmpresasRechazoCarga(empresa, tipoEmpresa, estadoRegistro);
            return FactorySic.GetGmmAgentesRepository().ListarMaestroEmpresasCliente(empresa, estadoRegistro);

        }
        public List<GmmEmpresaDTO> ListaEmpresasAgentes(string razonsocial)
        {
            return FactorySic.GetGmmAgentesRepository().ListarAgentes(razonsocial);
        }
    }
}
