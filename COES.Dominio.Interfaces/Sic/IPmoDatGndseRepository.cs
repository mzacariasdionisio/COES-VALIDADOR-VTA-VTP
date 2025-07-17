using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPmoDatGndseRepository
    {
        List<PmoDatGndseDTO> ListDatGndse(int periCodi);//Método modificado - NET 2019-03-06
        List<PrGrupoDTO> ListDatGndseCabeceras(int periCodi);
        int CountDatGndse(int periCodi);
        List<PmoDatGndseDTO> ListGndse(int periCodi, string central);
        int Save(PmoDatGndseDTO entity);
        int Delete(int periCodi, string central);

        #region NET 20190306
        List<PmoDatGndseDTO> GetDataProcesamiento(DateTime fechaInicio, DateTime fechaFin, DateTime Fechaperiodo);
        List<PmoDatGndseDTO> GetDataByFilter(int codigoPeriodo, int grupoCodi, string numSemana, int bloqCodi);
        List<PrGrupoDTO> GetCentralesByPeriodo(int codigoPeriodo);
        void CompletarUnidadesGenModelo(int PeriCodi);
        #endregion
    }

}
