using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_SCADA_PTOFILTRO_SP7
    /// </summary>
    public interface IMeScadaPtofiltroSp7Repository
    {
        int Save(MeScadaPtofiltroSp7DTO entity);
        void Update(MeScadaPtofiltroSp7DTO entity);
        void Delete(int scdpficodi);
        void DeleteFiltro(int scdpficodi);
        MeScadaPtofiltroSp7DTO GetById(int scdpficodi);
        List<MeScadaPtofiltroSp7DTO> List();
        List<MeScadaPtofiltroSp7DTO> GetByCriteria();
        int SaveMeScadaPtofiltroSp7Id(MeScadaPtofiltroSp7DTO entity);
    }
}
