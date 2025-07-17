using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnEstimadorRawRepository
    {
        int GetMaxId();
        int GetMaxIdSco();
        List<PrnEstimadorRawDTO> List();
        PrnEstimadorRawDTO GetById(int codigo);
        void Delete(int codigo);
        void Save(PrnEstimadorRawDTO entity);
        void Update(PrnEstimadorRawDTO entity);
        void BulkInsert(List<PrnEstimadorRawDTO> entitys, string nombreTabla);
        void DeletePorFechaIntervalo(int fuente, string fecha);
        List<PrnEstimadorRawDTO> ListDemandaTnaPorUnidad(string unidades, string fecInicio, string fecFin, string variables);
        List<PrnEstimadorRawDTO> ListEstimadorRawPorRangoPorUnidad(int unidad, DateTime fechaInicio, DateTime fechaFin, int idVariable, int idFuente, int modulo);
        List<PrnEstimadorRawDTO> ListEstimadorRawPorRangoPorAsociacion(int unidad, DateTime fechaInicio, DateTime fechaFin, int idVariable, int idFuente, int modulo);
        void InsertTableIntoPrnEstimadorRaw(string nombreTabla, string fecha);
        void TruncateTablaTemporal(string nombreTabla);
    }
}
