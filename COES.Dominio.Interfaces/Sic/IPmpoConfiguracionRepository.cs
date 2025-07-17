using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMPO_CONFIGURACION
    /// </summary>
    public interface IPmpoConfiguracionRepository
    {
        int Save(PmpoConfiguracionDTO entity);
        void Update(PmpoConfiguracionDTO entity);
        void Delete(int obracodi);
        PmpoConfiguracionDTO GetById(int configuracioncodi);

        List<PmpoConfiguracionDTO> List();
        List<PmpoConfiguracionDTO> List(string atributo);

        void Delete(string user, int confsmcorrelativo);
        PmpoConfiguracionDTO List(string atributo, string parametro);
        List<PmpoConfiguracionDTO> List(string atributo, string parametro, string estado);
        void UpdateEstado(PmpoConfiguracionDTO entity);
        PmpoConfiguracionDTO GetByParmanetroFech(string Fech, string Parametro);

    }
}
