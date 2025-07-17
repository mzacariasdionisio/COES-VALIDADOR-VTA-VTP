using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_PTOMEDCANAL
    /// </summary>
    public interface IMePtomedcanalRepository
    {
        void Save(MePtomedcanalDTO entity);
        void Update(MePtomedcanalDTO entity);
        void Delete(int canalcodi, int ptomedicodi, int tipoinfocodi);
        MePtomedcanalDTO GetById(int canalcodi, int ptomedicodi, int tipoinfocodi);
        List<MePtomedcanalDTO> List();
        List<MePtomedcanalDTO> GetByCriteria();
        List<MePtomedcanalDTO> ListarEquivalencia(string idEmpresa, int origlectcodi, int medida, string famcodis);
        List<MePtomedcanalDTO> ObtenerEquivalencia(string ptomedicodis, int tipoinfocodi);
    }
}
