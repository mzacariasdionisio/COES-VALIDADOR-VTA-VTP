using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_SUBCAUSA_FAMILIA
    /// </summary>
    public interface IEveSubcausaFamiliaRepository
    {
        int Save(EveSubcausaFamiliaDTO entity);
        void Update(EveSubcausaFamiliaDTO entity);
        void Delete(int scaufacodi);
        EveSubcausaFamiliaDTO GetById(int scaufacodi);
        List<EveSubcausaFamiliaDTO> List();
        string ListFamilia(int subcausacodi);
        List<EveSubcausaFamiliaDTO> GetByCriteria();
        int SaveEveSubcausaFamiliaId(EveSubcausaFamiliaDTO entity);
        List<EveSubcausaFamiliaDTO> BuscarOperaciones(string estado, int subcausaCodi,int famCodi,int nroPage, int pageSize);
        int ObtenerNroFilas(string estado, int subcausaCodi,int famCodi);
    }
}
