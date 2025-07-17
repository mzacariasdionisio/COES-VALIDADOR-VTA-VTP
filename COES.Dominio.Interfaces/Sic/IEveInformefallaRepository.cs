using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_INFORMEFALLA
    /// </summary>
    public interface IEveInformefallaRepository
    {
        int SaveEvento(EveInformefallaDTO entity);
        void Update(EveInformefallaDTO entity);
        void Delete(int eveninfcodi);
        EveInformefallaDTO GetById(int eveninfcodi);
        List<EveInformefallaDTO> List();
        List<EveInformefallaDTO> GetByCriteria();
        int ValidarInformeFallaN1(int idEvento);
        void EliminarInformeFallaN1(int idEvento);
        void ObtenerCorrelativoInformeFalla(int nroAnio, out int correlativoMen, out int correlativo, out int correlativoSco);
        int Save(EveInformefallaDTO entity);
        List<EveInformefallaDTO> BuscarOperaciones(string infMem, string infEmitido, int emprCodi, string equiAbrev,
            DateTime fechaIni, DateTime fechaFin, int nroPage, int PageSize);
        int ObtenerNroFilas(string infMem, string infEmitido, int emprCodi, string equiAbrev,
            DateTime fechaIni, DateTime fechaFin);
        List<EveInformefallaDTO> ObtenerAlertaInformeFalla();

        EveInformefallaDTO MostrarEventoInformeFalla(int evencodi);
        void ActualizarAmpliacion(EveInformefallaDTO entity);
    }
}
