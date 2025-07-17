using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AGC_CONTROL
    /// </summary>
    public interface IAgcControlRepository
    {
        int Save(AgcControlDTO entity);
        void Update(AgcControlDTO entity);
        void Delete(int agcccodi);
        AgcControlDTO GetById(int agcccodi);
        List<AgcControlDTO> List(string estado, int nroPage, int pageSize);        
        List<AgcControlDTO> GetByCriteria();
        int SaveAgcControlId(AgcControlDTO entity);
        //void UpdateMePtoMedicion(MePtomedicionDTO entity);
        int ObtenerNroFilas(string estado);
        //void UpdateMePtoMedicionCVariable(MePtomedicionDTO entity);
    }
}
