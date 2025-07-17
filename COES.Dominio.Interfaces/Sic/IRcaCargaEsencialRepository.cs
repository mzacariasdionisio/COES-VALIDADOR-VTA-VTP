using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_CARGA_ESENCIAL
    /// </summary>
    public interface IRcaCargaEsencialRepository
    {
        int Save(RcaCargaEsencialDTO entity);
        void Update(RcaCargaEsencialDTO entity);
        void Delete(int rccarecodi);
        RcaCargaEsencialDTO GetById(int rccarecodi);
        List<RcaCargaEsencialDTO> List();
        List<RcaCargaEsencialDTO> GetByCriteria();

        List<RcaCargaEsencialDTO> ListarCargaEsencialFiltro(string vigente, string empresa, string documento, string cargaIni, string cargaFin, 
            string fecIni, string fecFin, string estadoRegistro, int origen, int regIni, int regFin);
        List<RcaCargaEsencialDTO> ListarCargaEsencialHistorial(int emprcodi, int equicodi, string estadoRegistro);

        RcaCargaEsencialDTO ObtenerPorCodigo(int rccarecodi);

        List<RcaCargaEsencialDTO> ListarCargaEsencialPorPuntoMedicion(string puntoMedicion, string empresa);

        int ListarCargaEsencialCount(string vigente, string empresa, string documento, string cargaIni,
          string cargaFin, string fecIni, string fecFin, string estadoRegistro, int origen);

        List<RcaCargaEsencialDTO> ListarCargaEsencialExcel(string vigente, string empresa, string documento, string cargaIni,
          string cargaFin, string fecIni, string fecFin, string estadoRegistro, int origen);
    }
}
