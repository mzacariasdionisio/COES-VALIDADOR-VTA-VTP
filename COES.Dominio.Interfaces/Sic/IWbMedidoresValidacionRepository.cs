using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_MEDIDORES_VALIDACION
    /// </summary>
    public interface IWbMedidoresValidacionRepository
    {
        int Save(WbMedidoresValidacionDTO entity);
        void Update(WbMedidoresValidacionDTO entity);
        void Delete(int medivalcodi);
        WbMedidoresValidacionDTO GetById(int medivalcodi);
        List<WbMedidoresValidacionDTO> List();
        List<WbMedidoresValidacionDTO> GetByCriteria();
        List<WbMedidoresValidacionDTO> ObtenerPuntosPorEmpresa(int origLectCodi, int emprCodi);
        int ValidarExistencia(int idMedicion, int idDespacho);
        List<WbMedidoresValidacionDTO> ObtenerRelaciones(int idEmpresa);
        List<WbMedidoresValidacionDTO> ObtenerEmpresasGrafico();
        List<WbMedidoresValidacionDTO> ObtenerGruposGrafico(int idEmpresa);
    }
}
