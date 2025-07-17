using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_Relacion
    /// </summary>
    public interface ICpRelacionRepository
    {
        void Save(CpRelacionDTO entity);
        void Update(CpRelacionDTO entity);
        void Delete(int recurcodi1, int recurcodi2, int topcodi, int cptrelcodi);
        void DeleteAll(int recurcodimo, int topcodi);
        void DeleteAllTipo(int recurcodi, int cptrelcodi, int topcodi);
        CpRelacionDTO GetById(int recurcodi1, int recurcodi2, int topcodi, int cptrelcodi);
        List<CpRelacionDTO> GetByCriteria(int recurcodi, string cptrelcodi, int topcodi);
        List<CpRelacionDTO> List(int topcodi, string scptrelcodi);
        void DeleteEscenario(int topcodi);
        void CrearCopia(int topcodi1, int topcodi2);
    }
}
