using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_DETALLE_MES
    /// </summary>
    public interface IIndDetalleMesRepository
    {
        int Save(IndDetalleMesDTO entity);
        void Update(IndDetalleMesDTO entity);
        void Delete(int detmescodi, int anio, int mes);
        IndDetalleMesDTO GetById(int detmescodi);
        List<IndDetalleMesDTO> List();
        List<IndDetalleMesDTO> GetByCriteria(int anio, int mes);
        int GetMaxIndDetalleMes();
        void SaveMasivo(List<IndDetalleMesDTO> Lista);
    }
}
