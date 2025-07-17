using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General
{
    public class DesviacionAppServicio : AppServicioBase
    {
        /// <summary>
        /// Inserta o actualiza un registro de desviaciones
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDesviacion(List<DesviacionDTO> entitys, DateTime fecha, string user)
        {
            try
            {
                FactorySic.GetDesviacionRepositoryOracle().Delete(fecha);

                foreach (DesviacionDTO item in entitys)
                {
                    item.Desvfecha = fecha;
                    item.Lastuser = user;
                    item.Lectcodi = 4;
                    FactorySic.GetDesviacionRepositoryOracle().Save(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar los datos de un dia
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<DesviacionDTO> ListarDesviacion(DateTime fecha)
        {
            return FactorySic.GetDesviacionRepositoryOracle().ListarDesviacion(fecha);
        }

    }
}
