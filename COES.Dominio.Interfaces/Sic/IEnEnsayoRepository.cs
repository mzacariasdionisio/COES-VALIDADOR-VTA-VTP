using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EN_ENSAYO
    /// </summary>
    public interface IEnEnsayoRepository
    {
        void Update(EnEnsayoDTO entity);
        void UpdateEstadoEnsayo(int icodiensayo, DateTime dfechaEvento, int iCodEstado, DateTime lastdate, string lastuser);
        int Save(EnEnsayoDTO entity);
        void Delete();
        EnEnsayoDTO GetById(int ensayocodi);
        List<EnEnsayoDTO> List();
        List<EnEnsayoDTO> GetByCriteria();
        List<EnEnsayoDTO> ListaDetalleFiltro(string emprcodi, string equicodi, string estados, DateTime fechaInicio, DateTime fechaFin, int nroPaginas, int pageSize);
        List<EnEnsayoDTO> ListaDetalleFiltroXls(string emprcodi, string equicodi, string estados, DateTime fechaInicio, DateTime fechaFin);
        int ObtenerTotalListaEnsayo(string emprcodi, string grupocodi, string estenvcodi, DateTime fechaInicio, DateTime fechaFin);
        void SaveEnsMaster(int maxCodIngreso, int icodiensayo, DateTime lastdate, string lastuser);
        int GetMaxIdEnsMaster();

    }
}
