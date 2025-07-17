using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_INFORME_ANTECEDENTE
    /// </summary>
    public interface IMeInformeAntecedenteRepository
    {
        int Save(MeInformeAntecedenteDTO entity);
        void Update(MeInformeAntecedenteDTO entity);
        void Delete(int infantcodi);
        MeInformeAntecedenteDTO GetById(int infantcodi);
        List<MeInformeAntecedenteDTO> List();
        List<MeInformeAntecedenteDTO> GetByCriteria();
    }
}
