using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_MEDICION24
    /// </summary>
    public interface IMeMedicion24Repository
    {
        void Save(MeMedicion24DTO entity);
        void Update(MeMedicion24DTO entity);
        void Delete(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi);
        MeMedicion24DTO GetById();
        List<MeMedicion24DTO> List(DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion24DTO> GetByCriteria();
        void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa);
        List<MeMedicion24DTO> GetEnvioArchivo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion24DTO> GetHidrologia(int idLectura, int idOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion);
        List<MeMedicion24DTO> GetHidrologiaTiempoReal(int reporcodi, int idOrigenLectura, string idsEmpresa, DateTime fechaInicio, DateTime fechaFin, string idsTipoPtoMedicion, int lectCodi);
        List<MeMedicion24DTO> GetInterconexiones(int idLectura, int idOrigenLectura, string ptomedicodi, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion24DTO> GetDataFormatoSecundario(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion24DTO> GetLista24PresionGas(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, int TipotomedicodiPresionGas, int tipoinfocodiPresion, string strCentralInt);
        List<MeMedicion24DTO> GetLista24TemperaturaAmbiente(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicio, DateTime fechaFin, string strCentralInt);
        List<MeMedicion24DTO> GetDataHistoricoHidrologia(int reportecodi, DateTime fechaInicio, DateTime fechaFin);
        //inicio modificado Reportes Hidrología
        List<MeMedicion24DTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, string ptomedicodi);
        //fin modificado

        #region SIOSEIN
        List<MeMedicion24DTO> GetHidrologiaSioSein(int reporcodi, DateTime dfechaIni, DateTime dfechaFin);
        #endregion

        #region MigracionSGOCOES-GrupoB
        List<MeMedicion24DTO> ListaGeneracionDIgSILENT(DateTime fecha);
        List<MeMedicion24DTO> ListaDemandaDigsilent(string propcodi, string famcodi, DateTime fecha);
        void SaveMemedicion24masivo(List<MeMedicion24DTO> entitys);
        void DeleteMasivo(int lectcodi, DateTime medifecha, string tipoinfocodi, string ptomedicodi);
        #endregion

        #region Siosein2
        List<MeMedicion24DTO> ObtenerVolumenUtil(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, string ptomedicodi);
        #endregion

        #region Mejoras RDO
        List<MeMedicion24DTO> GetEnvioArchivoIntranet(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, string horario);
        void SaveEjecutados(MeMedicion24DTO entity, int idEnvio, string usuario, int idEmpresa);
        List<MeMedicion24DTO> GetEnvioArchivoEjecutados(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, string horario);
        void SaveIntranet(MeMedicion24DTO entity, int idEnvio);
        List<MeMedicion24DTO> GetEnvioMeMedicion24Intranet(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin);

        #endregion
    }
}
