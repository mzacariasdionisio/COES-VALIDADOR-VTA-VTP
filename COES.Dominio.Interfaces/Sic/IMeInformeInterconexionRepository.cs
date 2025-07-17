using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_INFORME_INTERCONEXION
    /// </summary>
    public interface IMeInformeInterconexionRepository
    {
        int Save(MeInformeInterconexionDTO entity);
        void Update(MeInformeInterconexionDTO entity);
        void Delete(int infintcodi);
        MeInformeInterconexionDTO GetById(int infintcodi);
        List<MeInformeInterconexionDTO> List();
        List<MeInformeInterconexionDTO> GetByCriteria(int anio, int semana);
    }
}
