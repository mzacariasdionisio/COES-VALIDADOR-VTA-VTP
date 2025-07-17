using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_INGTRANSMISION
    /// </summary>
    public interface ICaiIngtransmisionRepository
    {
        int Save(CaiIngtransmisionDTO entity);
        void Update(CaiIngtransmisionDTO entity);
        void Delete(int caiajcodi);
        CaiIngtransmisionDTO GetById(int caitrcodi);
        List<CaiIngtransmisionDTO> List();
        List<CaiIngtransmisionDTO> GetByCriteria();
        void SaveCaiIngtransmisionAsSelectMeMedicion1(int caitrcodi, int caiajcodi, string caitrcalidadinfo, string tipodato, string user, int Formatcodi, int Lectcodi, string FechaInicio, string FechaFin);
        int GetCodigoGenerado();
    }
}
