using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.WLib
{
    public class WLibAppServicio:AppServicioBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(WLibAppServicio));

        /// <summary>
        /// Devuelve el maximo codigo ERATCODI de la tabla Ext_Ratio
        /// </summary>
        /// <returns></returns>
        public int GetMaxCodi()
        {
            return FactorySic.GetExtRatioRepository().GetMaxCodi();
        }

        /// <summary>
        /// Actualiza el campo maxcount de la tabla Fw_Counter
        /// </summary>
        /// <returns></returns>
        public void UpdateMaxCount(string tablename)
        {
            try
            {
                FactorySic.GetFwCounterRepository().UpdateMaxCount(tablename);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el campo MAXCOUNT de la tabla Fw_Counter segun el campo TABLENAME
        /// </summary>
        /// <returns></returns>
        public int GetMaxCount(string tablename) {

            FwCounterDTO fwCounter = new FwCounterDTO();
            fwCounter=FactorySic.GetFwCounterRepository().GetById(tablename);

            return fwCounter.Maxcount??0;
        }
    }
}
