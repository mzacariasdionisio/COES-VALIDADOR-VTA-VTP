using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_EVENTO_MEDICION
    /// </summary>
    public interface IReEventoMedicionRepository
    {
        int Save(ReEventoMedicionDTO entity);
        void Update(ReEventoMedicionDTO entity);
        void Delete(int evento, int empresa);
        ReEventoMedicionDTO GetById(int reemedcodi);
        List<ReEventoMedicionDTO> List();
        List<ReEventoMedicionDTO> GetByCriteria();
        List<ReEventoMedicionDTO> ObtenerMedicion(int idEvento, int idEmpresa);
    }
}
