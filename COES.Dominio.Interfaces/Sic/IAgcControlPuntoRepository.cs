using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AGC_CONTROL_PUNTO
    /// </summary>
    public interface IAgcControlPuntoRepository
    {
        int Save(AgcControlPuntoDTO entity);
        void Update(AgcControlPuntoDTO entity);
        void Delete(int agcccodi);
        List<AgcControlPuntoDTO> GetById(int agcccodi);
        List<AgcControlPuntoDTO> List();
        List<AgcControlPuntoDTO> GetByCriteria();
        int SaveAgcControlPuntoId(AgcControlPuntoDTO entity);
        List<AgcControlPuntoDTO> BuscarOperaciones(int agccCodi,int ptomediCodi,int equiCodi,int nroPage, int PageSize);
        int ObtenerNroFilas(int agccCodi,int ptomediCodi,int equiCodi);

        List<AgcControlPuntoDTO> ObtenerPorControl(int agccCodi);
    }
}
