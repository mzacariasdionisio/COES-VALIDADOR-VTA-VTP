using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_INFORMEFALLA_N2
    /// </summary>
    public interface IEveInformefallaN2Repository
    {
        int Save(EveInformefallaN2DTO entity);
        void Update(EveInformefallaN2DTO entity);
        void Delete(int eveninfn2codi);
        EveInformefallaN2DTO GetById(int eveninfn2codi);
        List<EveInformefallaN2DTO> List();
        List<EveInformefallaN2DTO> GetByCriteria();
        int ValidarInformeFallaN2(int idEvento);
        void EliminarInformeFallaN2(int idEvento);
        void ObtenerCorrelativoInformeFallaN2(int nroAnio, out int correlativo);
        int SaveEvento(EveInformefallaN2DTO entity);        
        List<EveInformefallaN2DTO> BuscarOperaciones(string infEmitido, int emprCodi, string equiAbrev,
            DateTime fechaIni, DateTime fechaFin, int nroPage, int PageSize);
        int ObtenerNroFilas(string infEmitido, int emprCodi, string equiAbrev,
            DateTime fechaIni, DateTime fechaFin);
        EveInformefallaN2DTO MostrarEventoInformeFallaN2(int evencodi);
        void ActualizarAmpliacionN2(EveInformefallaN2DTO entity);
    }
}
