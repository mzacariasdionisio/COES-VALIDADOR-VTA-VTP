using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMPO_OBRA
    /// </summary>
    public interface IPmpoObraRepository
    {
        int Save(PmpoObraDTO entity);
        void Update(PmpoObraDTO entity);
        void Delete(int obracodi);
        PmpoObraDTO GetById(int obracodi, int tipoObra);
        
        List<PmpoObraDTO> List(string idEmpresa, int idTipoObra, DateTime fechaIni, DateTime fechaFin,string formatList);

        List<PmpoObraDTO> ListObras(string idEmpresa, int idTipoObra, DateTime fechaIni, DateTime fechaFin);

    }
}
