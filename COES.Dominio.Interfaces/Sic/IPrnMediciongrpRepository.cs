using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnMediciongrpRepository
    {
        void Save(PrnMediciongrpDTO entity);
        void Update(PrnMediciongrpDTO entity);
        void Delete(int grupocodi, int prnmgrtipo, DateTime medifecha, int vergrpcodi);
        List<PrnMediciongrpDTO> List();
        PrnMediciongrpDTO GetById(int grupocodi, int prnmgrtipo, DateTime medifecha, int vergrpcodi);
        //0805 Assetec
        void DeleteSA(int grupocodi, int prnmgrtipo);
        //0805 Assetec
        List<PrnMediciongrpDTO> ListBarraBy(string barrapm, int tipo, string barracp);

        //0924
        List<PrnMediciongrpDTO> ListaPronosticoBarraTotal(string inicio, string fin, int vergrpcodi);
        List<PrnMediciongrpDTO> ListaPronosticoBarraDetalle(string inicio, string fin, int area, int vergrpcodi);

        // Para formato Demanda CP
        List<PrnMediciongrpDTO> GetDataFormatoPronosticoDemanda(int formatcodi, DateTime fechaini, DateTime fechafin);
        int ValidarEjecucionPronostiPorBarras(string medifecha);

        void PronosticoPorBarrasDuplicarVersion(int refVersion, string refFecha, int idVersion, string regFecha);
        void EliminarVersion(int idVersion);
        List<MePtomedicionDTO> ObtenerUnidadesPorId(int idUnidad);
        List<MePtomedicionDTO> ListUnidadesEstimadorByTipo(int tipo);

        //Assetec 20220225
        PrnMediciongrpDTO GetMedicionByAgrupacion(int agrupacion, int tipo, string fecha, int version);
        PrnMediciongrpDTO GetMedicionByAgrupacionAjuste(int agrupacion, int tipo, string fecha, int version);
        PrnMediciongrpDTO GetMedicionByBarraAjuste(int barra, int tipo, string fecha, int version);
        List<PrnMediciongrpDTO> GetMedicionAgrupacionByBarra(int agrupacion, int tipo, string fecha, int version);
        PrnMediciongrpDTO GetMedicionBarrasOtraAgrupacion(int agrupacion, int tipo, string fecha, int version);
        PrnMediciongrpDTO GetDemandaBarraByAreaVersion(int areacodi, string medifecha, int prnmgrtipo, int version);
        List<PrnMediciongrpDTO> GetDemandaBarraByAreaTipo(int areacodi, string medifecha, string tipo, int version);
        //Assetec 20220304
        PrnMediciongrpDTO GetDemandaBarraByArea(int areacodi, string medifecha, string prnmgrtipo, int version);
        //0805 Assetec
    }
}
