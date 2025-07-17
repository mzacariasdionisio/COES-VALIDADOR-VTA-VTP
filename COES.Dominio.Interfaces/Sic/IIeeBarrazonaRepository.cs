using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IEE_BARRAZONA
    /// </summary>
    public interface IIeeBarrazonaRepository
    {
        int Save(IeeBarrazonaDTO entity);
        void Update(IeeBarrazonaDTO entity);
        void Delete(int barrzcodi);
        IeeBarrazonaDTO GetById(int barrzcodi);
        List<IeeBarrazonaDTO> List();
        List<IeeBarrazonaDTO> GetByCriteria(int mrepcodi);
        List<IeeBarrazonaDTO> ObtenerBarrasPorAreas();
        List<IeeBarrazonaDTO> ObtenerAgrupacionPorZona(int mrepcodi);
        List<IeeBarrazonaDTO> ObtenerBarrasPorAgrupacion(int mrepcodi, string agrupacion);
        void EliminarAgrupacion(int zona, string agrupacion);
        bool ValidarExistencia(int zona, string nombre);
        bool ValidarExistenciaEdicion(int zona, string nombre, string agrupacion);
    }
}
