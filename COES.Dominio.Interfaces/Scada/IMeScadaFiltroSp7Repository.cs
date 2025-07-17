using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_SCADA_FILTRO_SP7
    /// </summary>
    public interface IMeScadaFiltroSp7Repository
    {
        int Save(MeScadaFiltroSp7DTO entity);
        void Update(MeScadaFiltroSp7DTO entity);
        void Delete(int filtrocodi);
        MeScadaFiltroSp7DTO GetById(int filtrocodi);
        List<MeScadaFiltroSp7DTO> List();
        List<MeScadaFiltroSp7DTO> GetByCriteria();
        int SaveMeScadaFiltroSp7Id(MeScadaFiltroSp7DTO entity);
        List<MeScadaFiltroSp7DTO> BuscarOperaciones(string filtro, string creador, string modificador, int nroPage, int PageSize);
        int ObtenerNroFilas(string filtro, string creador, string modificador);
    }
}
