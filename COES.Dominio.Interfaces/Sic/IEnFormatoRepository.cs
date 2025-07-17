using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EN_FORMATO
    /// </summary>
    public interface IEnFormatoRepository
    {
        void Update(EnFormatoDTO entity);
        int Save(EnFormatoDTO entity);
        void Delete(int formatocodi);
        EnFormatoDTO GetById(int formatocodi);
        List<EnFormatoDTO> List();
        List<EnFormatoDTO> GetByCriteria();
        List<EnFormatoDTO> ListarFormatosActuales();
        List<EnFormatoDTO> ListarFormatosActualesTodos();
        List<EnFormatoDTO> ListarFormatosPorPadre(int idPadre);

        List<EnFormatoDTO> ListarFormatosActivos();
    }
}
