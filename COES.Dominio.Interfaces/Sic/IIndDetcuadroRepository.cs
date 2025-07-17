using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_DETCUADRO
    /// </summary>
    public interface IIndDetcuadroRepository
    {
        int Save(IndDetcuadroDTO entity);
        void Update(IndDetcuadroDTO entity);
        void Delete(int indcuacodi);
        void DeletexPercuacodi(int percuacodi);
        IndDetcuadroDTO GetById(int indcuacodi);
        List<IndDetcuadroDTO> List();
        List<IndDetcuadroDTO> GetByCriteria(int percuacodi);

        List<IndDetcuadroDTO> GetCargarViewCuadro1(int percuacodi);
        List<IndDetcuadroDTO> GetCargarViewCuadro1Final(int id, DateTime fechaini, DateTime fechafin);

        List<IndDetcuadroDTO> GetCargarViewCuadro2(int percuacodi);
        List<IndDetcuadroDTO> GetCargarViewCuadro2Final(int id, DateTime fechaini, DateTime fechafin, int cuad);

        List<IndDetcuadroDTO> GetCargarViewCuadro4(int percuacodi);
        List<IndDetcuadroDTO> GetCargarViewCuadro4Final(int id, DateTime fechaini, DateTime fechafin, int tipogrupocodiNoIntegrante);

        List<IndDetcuadroDTO> GetCargarEmpresaCentralModoOperacion();
        List<IndDetcuadroDTO> ListaAllFactorK(int percuacodi);
        List<IndDetcuadroDTO> GetCargarUnidadesTermo();
    }
}
