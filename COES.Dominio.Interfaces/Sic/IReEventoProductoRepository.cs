using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_EVENTO_PRODUCTO
    /// </summary>
    public interface IReEventoProductoRepository
    {
        int Save(ReEventoProductoDTO entity);
        void Update(ReEventoProductoDTO entity);
        void Delete(int reevprcodi);
        ReEventoProductoDTO GetById(int reevprcodi);
        List<ReEventoProductoDTO> List();
        List<ReEventoProductoDTO> GetByCriteria(int anio, int mes);
        List<ReEventoProductoDTO> ObtenerEventosPorSuministrador(int empresa, int anio, int mes, string buscar);
    }
}
