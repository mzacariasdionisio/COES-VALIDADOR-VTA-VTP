using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using COES.Servicios.Aplicacion.Helper;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class CodigoConsolidadoAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CodigoConsolidadoAppServicio));
        public VtpCodigoConsolidadoDTO GetByCodigoVTP(string codigovtp)
        {
            try
            {
                return FactoryTransferencia.GetVtpCodigoConsolidadoRepository().GetByCodigoVTP(codigovtp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
