using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoCasoDetalleRepository
    {
        void Save(DpoCasoDetalleDTO entity);
        void Update(DpoCasoDetalleDTO entity);
        void Delete(int id);
        DpoCasoDetalleDTO GetById(int id);
        List<DpoCasoDetalleDTO> List();

        void DeleteByIdCaso(int idCaso);
        List<DpoCasoDetalleDTO> ListByIdCaso(int idCaso);
        List<DpoFuncionDataMaestraDTO> ListFuncionesDataMaestra(int idCaso);
        List<DpoFuncionDataProcesarDTO> ListFuncionesDataProcesar(int idCaso);
        List<DpoCasoDetalleDTO> GetParametrosDataMaestra(int idCaso);
        List<DpoCasoDetalleDTO> GetParametrosDataProcesar(int idCaso);


        List<DpoParametrosR1DTO> ListParametrosR1(int idCaso, int idCasoDetalle);
        List<DpoParametrosR2DTO> ListParametrosR2(int idCaso, int idCasoDetalle);
        List<DpoParametrosF1DTO> ListParametrosF1(int idCaso, int idCasoDetalle);
        List<DpoParametrosF2DTO> ListParametrosF2(int idCaso, int idCasoDetalle);
        List<DpoParametrosA1DTO> ListParametrosA1(int idCaso, int idCasoDetalle);
        List<DpoParametrosA2DTO> ListParametrosA2(int idCaso, int idCasoDetalle);

        List<DpoParametrosA1DTO> GetParametrosA1(int idCaso, int idFuncion, string tipFuncion);
        List<DpoParametrosA2DTO> GetParametrosA2(int idCaso, int idFuncion, string tipFuncion);
        List<DpoParametrosF1DTO> GetParametrosF1(int idCaso, int idFuncion, string tipFuncion);
        List<DpoParametrosF2DTO> GetParametrosF2(int idCaso, int idFuncion, string tipFuncion);
        List<DpoParametrosR1DTO> GetParametrosR1(int idCaso, int idFuncion, string tipFuncion);
        List<DpoParametrosR2DTO> GetParametrosR2(int idCaso, int idFuncion, string tipFuncion);


        List<DpoHistorico48DTO> FiltrarHistorico48PorRangoFechas(DateTime fechaini, DateTime fechafin);
        List<DpoHistorico96DTO> FiltrarHistorico96PorRangoFechas(DateTime fechaini, DateTime fechafin);

        List<DpoHistorico48DTO> ObtenerColumnaDatos48();
        List<DpoHistorico96DTO> ObtenerColumnaDatos96();

        List<DpoHistorico48DTO> ObtenerSerieDatos48();
        List<DpoHistorico96DTO> ObtenerSerieDatos96();
    }
}
