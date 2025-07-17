using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FW_AREA
    /// </summary>
    public interface IFwAreaRepository
    {
        int Save(FwAreaDTO entity);
        void Update(FwAreaDTO entity);
        void Delete(int areacode);
        FwAreaDTO GetById(int areacode);
        List<FwAreaDTO> List();
        List<FwAreaDTO> GetByCriteria(int idEmpresa);
        List<FwAreaDTO> ListAreaXFormato(int idOrigen);
        int GetDirResponsable(int areacode); //Agregado por STS

        List<FwAreaDTO> ListarArea();
    }
}
