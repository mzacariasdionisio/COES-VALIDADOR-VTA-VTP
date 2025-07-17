using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_PARAMETRO_PRIMA
    /// </summary>
    public interface IRerParametroPrimaRepository
    {
        int Save(RerParametroPrimaDTO entity);
        void Update(RerParametroPrimaDTO entity);
        void Delete(int rerPprCodi);
        RerParametroPrimaDTO GetById(int rerPprCodi);
        List<RerParametroPrimaDTO> GetByCriteria(string sAnio, int iMes);
        List<RerParametroPrimaDTO> List();
        List<RerParametroPrimaDTO> GetByAnioVersion(int reravcodi); 
        List<RerParametroPrimaDTO> GetByAnioVersionByMes(int reravcodi, string rerpprmes);
        List<RerParametroPrimaDTO> listaParametroPrimaRerByAnio(int Reravaniotarif);
    }
}
