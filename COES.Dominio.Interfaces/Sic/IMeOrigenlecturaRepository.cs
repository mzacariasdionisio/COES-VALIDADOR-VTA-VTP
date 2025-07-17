using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_ORIGENLECTURA
    /// </summary>
    public interface IMeOrigenlecturaRepository
    {
        void Save(MeOrigenlecturaDTO entity);
        void Update(MeOrigenlecturaDTO entity);
        void Delete(int origlectcodi);
        MeOrigenlecturaDTO GetById(int origlectcodi);
        List<MeOrigenlecturaDTO> List();
        List<MeOrigenlecturaDTO> GetByCriteria();

        #region Titularidad-Instalaciones-Empresas
        List<MeOrigenlecturaDTO> ListByEmprcodi(int emprcodi);
        #endregion
    }
}
