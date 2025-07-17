using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_MESCALENDARIO
    /// </summary>
    public interface IWbMescalendarioRepository
    {
        int Save(WbMescalendarioDTO entity);
        void Update(WbMescalendarioDTO entity);
        void Delete(int mescalcodi);
        WbMescalendarioDTO GetById(int mescalcodi);
        List<WbMescalendarioDTO> List();
        List<WbMescalendarioDTO> GetByCriteria(int? anio, int? mes);
        void QuitarImagen(int mescalcodi);
        void ActualizarInfografia(int id, string archivo);

    }
}
