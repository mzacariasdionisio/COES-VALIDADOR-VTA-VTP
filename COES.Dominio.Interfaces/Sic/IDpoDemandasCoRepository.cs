using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoDemandaScoRepository
    {
        void Save(DpoDemandaScoDTO entity);
        void Update(DpoDemandaScoDTO entity);
        void Delete(int ptomedicodi, DateTime medifecha, int prnvarcodi);
        List<DpoDemandaScoDTO> List();
        DpoDemandaScoDTO GetById(int ptomedicodi, DateTime medifecha, int prnvarcodi);
        void BulkInsert(List<DpoDemandaScoDTO> entitys, string nombreTabla);
        void TruncateTablaTemporal(string nombreTabla);
        List<DateTime> ReporteEstadoProceso(int dporawfuente, string fecIni, string fecFin);
        void DeleteRangoFecha(int punto, string fecIni, string fecFin);
        #region Demadna DPO - Iteracion 2
        List<DpoDemandaScoDTO> ListGroupByMonthYear(string anio, string mes, string cargas, string tipo);
        List<DpoDemandaScoDTO> ListMedidorDemandaTna(string punto, string inicio, string fin, string tipo);
        List<DpoDemandaScoDTO> ListDatosTNA(int anio, string mes, string cargas, string tipo);
        List<DpoDemandaScoDTO> ObtenerDemandaSco(string ptomedicodi,
            string prnvarcodi, string medifecha);
        #endregion
    }
}
