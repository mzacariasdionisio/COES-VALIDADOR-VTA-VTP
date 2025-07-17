using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RPF_ENVIO
    /// </summary>
    public interface IRpfEnvioRepository
    {
        int Save(RpfEnvioDTO entity);
        void Update(RpfEnvioDTO entity);
        void Delete(int rpfenvcodi);
        RpfEnvioDTO GetById(int rpfenvcodi);
        List<RpfEnvioDTO> List();
        List<RpfEnvioDTO> GetByCriteria();
        RpfEnvioDTO ObtenerPorFecha(DateTime fecha, int idEmpresa);
        List<RpfEnvioDTO> ObtenerEnviosPorFecha(DateTime fecha);
    }
}
