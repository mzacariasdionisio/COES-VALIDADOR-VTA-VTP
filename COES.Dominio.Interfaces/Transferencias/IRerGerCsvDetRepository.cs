using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerGerCsvDetRepository
    {
        int Save(RerGerCsvDetDTO entity);
        void Update(RerGerCsvDetDTO entity);
        void Delete(int rerGerCsvDetId);
        RerGerCsvDetDTO GetById(int rerGerCsvDetId);
        List<RerGerCsvDetDTO> List();
        List<RerGerCsvDetDTO> GetByEmprcodiEquicodiTipo(int emprcodi, int equicodi, string tipo, int anioTarifario);

        List<RerGerCsvDetDTO> ListByEquipo(int equicodi, DateTime fechainicio, DateTime fechafin);
        int GetMaxId();
        void BulkInsertRerGerCsvDet(List<RerGerCsvDetDTO> entitys, string nombreTabla);
    }
}

