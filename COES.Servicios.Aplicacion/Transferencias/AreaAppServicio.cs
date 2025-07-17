using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    /// <summary>
    /// Clases con métodos del módulo Área
    /// </summary>
    public class AreaAppServicio : AppServicioBase
    {

        /// <summary>
        /// Permite obtener el área en base al iAreaCodi
        /// </summary>
        /// <param name="iAreaCodi">Código del área</param>
        /// <returns>AreaDTO</returns>
        public AreaDTO GetByIdArea(int iAreaCodi)
        {
            return FactoryTransferencia.GetAreaRepository().GetById(iAreaCodi);
        }

        /// <summary>
        /// Permite listar todas las áreas
        /// </summary>
        /// <returns>Lista de AreaDTO</returns>
        public List<AreaDTO> ListAreas()
        {
            return FactoryTransferencia.GetAreaRepository().List();
        }

        /// <summary>
        /// Permite listad las áreas en base al nombre pasado como dato sAreaNomb
        /// </summary>
        /// <param name="sAreaNomb">Nombre del área</param>
        /// <returns>Lista de AreaDTO</returns>
        public List<AreaDTO> BuscarArea(string sAreaNomb)
        {
            return FactoryTransferencia.GetAreaRepository().GetByCriteria(sAreaNomb);
        }

        public List<AreaDTO> ListArea()
        {
            return FactoryTransferencia.GetAreaRepository().ListArea();
        }

        public List<AreaDTO> ListAreaProceso()
        {
            return FactoryTransferencia.GetAreaRepository().ListAreaProceso();
        }
        //GESPROTEC - 20241029
        #region GESPROTEC
        public List<Dominio.DTO.Sic.EqAreaDTO> ListUbicacionCoes(string codigoTipoArea, string nombreArea, String programaExistente)
        {
            return FactoryTransferencia.GetEqAreaRepository().ListarUbicacionCoes(codigoTipoArea, nombreArea, programaExistente);
        }
        #endregion
    }
}