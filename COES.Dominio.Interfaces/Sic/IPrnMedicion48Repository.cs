using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnMedicion48Repository
    {
        void Save(PrnMedicion48DTO entity);
        void Update(PrnMedicion48DTO entity);
        void Delete(int ptomedicodi, int prnm48tipo, DateTime medifecha);
        List<PrnMedicion48DTO> List();
        PrnMedicion48DTO GetById(int ptomedicodi, int prnm48tipo, DateTime medifecha);
        List<PrnMedicion48DTO> ListById(int ptomedicodi, int prnm48tipo, DateTime fecini, DateTime fecfin);
        void BulkInsert(List<PrnMedicion48DTO> entitys);
        void DeleteRangoPrnMedicion48(int ptomedicodi, int prnm48tipo, DateTime fechaIni, DateTime fechaFin);
        List<PrnMedicion48DTO> ListByIdEnvio(int IdEnvio);

        //Old - INICIO
        List<PrnMedicion48DTO> ListMeMed48Historicos(int ptomedicodi, int lectcodi, int tipoinfocodi, string feriado, string atipico, DateTime fecini, DateTime fecfin);
        List<PrnMedicion48DTO> ListPrnMed48Historicos(int ptomedicodi, int prnm48tipo, string feriado, string atipico, DateTime fecini, DateTime fecfin);
        List<PrnMedicion48DTO> ListPrnMed48HistoricosNoConfig(int ptomedicodi, int prnm48tipo, DateTime fecini, DateTime fecfin);
        PrnMedicion48DTO GetDespachoEjecutadoByArea(int areacodi, DateTime medifecha);
        PrnMedicion48DTO GetMeMedicionesULByArea(int areapadre, int lectcodi, DateTime medifecha);
        //Old - FIN

        //Obtención de datos para la formación del perfíl patrón segun proceso - INICIO
        List<PrnMedicion48DTO> DataPatronDemandaPorPunto(int ptomedicodi, string prnm48tipo, string fecini, string fecfin,
            int lectcodi, int tipoinfocodi, PrnConfiguracionDTO regConfiguracion, PrnConfiguracionDTO regDefecto);
        List<PrnMedicion48DTO> DataPatronDemandaPorAgrupacion(int origlectcodi, int origlectcodi2, int lectcodi, int tipoinfocodi,
            string fecini, string fecfin, string prnm48tipo, int prnm48tipo2, int ptomedicodi);
        List<PrnMedicion48DTO> DataPatronDemandaPorArea(int ptomedicodi, int areacodi, string fecini, string fecfin, string prnm48tipo);

        //Obtención de datos para la formación del perfíl patrón segun proceso - FIN


        //Obtención de datos historicos para el cálculo del pronóstico - INICIO
        List<PrnMedicion48DTO> DataHistoricaBarraPMDistr(int prnredbarrapm, decimal prnredgauss, decimal prnredperdida, int origlectcodi, 
            int tipoinfocodi, int lectcodi, string fecini, string fecfin, string prnm48tipo);
        List<PrnMedicion48DTO> DataHistoricaBarraPMUlibre(int prnredbarracp, int prnredbarrapm, int origlectcodi, 
            int tipoinfocodi, int lectcodi, string fecini, string fecfin, string prnm48tipo, int origlectcodi2, int prnm48tipo2);
        PrnMedicion48DTO DataBarraPMUlibrePorDia(int prnredbarrapm, decimal prnredgauss, decimal prnredperdida, int origlectcodi,
            int tipoinfocodi, int lectcodi, string medifecha, string prnm48tipo, int origlectcodi2, int prnm48tipo2);
        PrnMedicion48DTO DataPtrBarraPMUlibrePorDia(int prnredbarrapm, decimal prnredgauss, decimal prnredperdida, int origlectcodi,
            string medifecha, int prnm48tipo, int origlectcodi2);
        //Obtención de datos historicos para el cálculo del pronóstico - FIN


        List<PrnMedicion48DTO> GetDemandaPorPunto(int ptomedicodi, int lectcodi, int prnm48tipo, int prnm48tipo2, string medifecha, int tipoinfocodi);
        List<PrnMedicion48DTO> GetDemandaPorAgrupacion(int origlectcodi, int origlectcodi2, int lectcodi, int tipoinfocodi,
            string medifecha, string prnm48tipo, int prnm48tipo2, int ptomedicodi);
        List<PrnMedicion48DTO> GetDemandaEjecutadaPorPunto(int ptomedicodi, int lectcodi, int prnm48tipo, int prnm48tipo2, string medifecha, int tipoinfocodi);
        List<PrnMedicion48DTO> GetDemandaEjecutadaPorAgrupacion(int origlectcodi, int origlectcodi2, int lectcodi, int tipoinfocodi,
            string medifecha, string prnm48tipo, int prnm48tipo2, int ptomedicodi);
        List<PrnMedicion48DTO> GetDemandaPrevistaPorPunto(int ptomedicodi, int lectcodi, int prnm48tipo, string medifecha, int tipoinfocodi);
        List<PrnMedicion48DTO> GetDemandaPrevistaPorAgrupacion(int origlectcodi, int origlectcodi2, int lectcodi, int tipoinfocodi,
            string medifecha, int prnm48tipo, int prnm48tipo2, int ptomedicodi);

        List<PrnMedicion48DTO> GetDespachoEjecutadoPorArea(int ptomedicodi, int areacodi, string medifecha,  string prnm48tipo);
        List<PrnMedicion48DTO> GetAjusteAlCentroPorTipo(string ptomedicodi, string medifecha, int prnm48tipo);
        PrnMedicion48DTO GetDemandaULibresPorArea(int origlectcodi, int origlectcodi2, int lectcodi, int tipoinfocodi, string medifecha,
            string prnm48tipo, int areacodi, int ptogrppronostico, int prnm48tipo2);

        string GetNombrePtomedicion(int ptomedicodi);

        //agregado para reporte
        List<PrnMedicion48DTO> ObtenerConsultaPronostico48(int lectcodi, string empresas, DateTime fechaInicio, DateTime fechaFin, int origen);

        //Parametros Barras
        List<PrnMedicion48DTO> ListBarraBy(string barrapm, int tipo, int catecodi, string barracp);
        //void DeleteSA(int ptomedicodi, int prnm48tipo);

        //Filtros PR03
        List<MePtomedicionDTO> PR03Puntos();
        List<MePtomedicionDTO> PR03PuntosNoAgrupados();
        List<SiEmpresaDTO> PR03Empresas();
        List<EqAreaDTO> PR03Ubicaciones();

        List<MePtomedicionDTO> PR03PuntosPorBarrasCP(string areacodi);
        List<PrnMedicion48DTO> GetDemandaPorTipoPorRango(int tipodemanda, string ptomedicodi, int lectcodi, string prnm48tipo, 
            string fechaini, string fechafin);
        List<PrnMedicion48DTO> DataPatronDemandaPorPuntoPorFecha(int ptomedicodi, string prnm48tipo, string fecini, string fecfin,
            int lectcodi, int tipoinfocodi, PrnConfiguracionDTO regConfiguracion, PrnConfiguracionDTO regDefecto);
        List<MePtomedicionDTO> PR03PuntosPorBarrasCPPronosticoDemanda(string areacodi);
        int ValidarEjecucionPronosticoPorAreas(string medifecha);
    }
}
