using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_PARAMETRO
    /// </summary>
    public interface ISiParametroRepository
    {
        int Save(SiParametroDTO entity);
        void Update(SiParametroDTO entity);
        void Delete(int siparcodi);
        SiParametroDTO GetById(int siparcodi);
        List<SiParametroDTO> List();
        List<SiParametroDTO> GetByCriteria();
        int SaveSiParametroId(SiParametroDTO entity);
        List<SiParametroDTO> BuscarOperaciones(string abreviatura,string descripcion,int nroPage, int PageSize);
        int ObtenerNroFilas(string abreviatura, string descripcion);
    }
}
