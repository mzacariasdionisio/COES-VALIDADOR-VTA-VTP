using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPmoDatGenerateRepository
    {
        void GenerateDat(int PeriCodi, string TableName, string Usuario);

        void GenerateDatMgndPtoInstFactOpe(DateTime fecha);
        void DeleteDataPorPeriodoYtipo(int pericodi, string tipo);
        Dictionary<string, object> GetFechasProcesamientoDisponibilidad(int pericodi);
        List<PrGrupoRelasoDTO> GetDataGrupoRelaso(string strGrrdefcodi);  

    }
}
