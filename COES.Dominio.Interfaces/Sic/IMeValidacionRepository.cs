using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_VALIDACION
    /// </summary>
    public interface IMeValidacionRepository
    {
        void Save(MeValidacionDTO entity);
        void Update(MeValidacionDTO entity);
        void UpdateById(MeValidacionDTO entity);
        void Delete(int formatcodi, int emprcodi, DateTime validfechaperiodo);
        MeValidacionDTO GetById(int formatcodi, int emprcodi, DateTime validfechaperiodo);
        List<MeValidacionDTO> List(DateTime fecha, int formato);
        void DeleteAllEmpresa(int formatcodi, int emprcodi);
        void ValidarEmpresa(DateTime fecha, int formatcodi, string usuario, string empresas, int estado);
        List<MeValidacionDTO> GetByCriteria(int formatcodi, int emprcodi);
        List<MeValidacionDTO> ListarValidacionXFormatoYFecha(string formatcodis, DateTime validfechaperiodo);
    }
}
