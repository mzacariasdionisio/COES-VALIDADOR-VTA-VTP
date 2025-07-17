using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_MEDICION1
    /// </summary>
    public interface IMeMedicion1Repository
    {
        void Save(MeMedicion1DTO entity);
        void Delete(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi);
        MeMedicion1DTO GetById(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi);
        List<MeMedicion1DTO> List();
        List<MeMedicion1DTO> GetByCriteria2(DateTime fechaInicio, DateTime fechaFin, int? idEmpresa, int? idGrupo, int? idTipoCombustible);
        List<SiEmpresaDTO> ObtenerEmpresasStock();
        List<PrGrupoDTO> ObtenerGruposStock(int idEmpresa);
        List<MeMedicion1DTO> ObtenerTipoCombustible();
        List<MeMedicion1DTO> ObtenerEstructura(int? idEmpresa, int? idGrupo, int? idTipoCombustible);
        void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa);
        List<MeMedicion1DTO> GetEnvioArchivo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, int lectocodi = -1);
        List<MeMedicion1DTO> GetHidrologia(int idLectura, int idOrigenLectura, string idsEmpresa, string idsCuenca,
            string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion);
        List<MeMedicion1DTO> GetDataFormatoSecundario(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion1DTO> GetListaMedicion1(int lectCodiRecepcion, DateTime fechaInicial, DateTime fechaFinal);
        List<MeMedicion1DTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, string ptomedicodi);
        List<MeMedicion1DTO> GetDataPronosticoHidrologia(int reportecodi, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion1DTO> GetDataPronosticoHidrologiaByPtoCalculadoAndFecha(int reportecodi, int ptocalculadocodi, DateTime fecha);

        void DeleteEnvioArchivo2(int idTptomedi, int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa);

        List<MeMedicion1DTO> GetDataSemanalPowel(DateTime fechaInicio, DateTime fechaFin, int reportecodi);

        #region INDISPONIBILIDADES
        List<MeMedicion1DTO> GetListaMedicion1ContratoCombustible(int lectcodi, DateTime fechaInicial, DateTime fechaFinal);
        #endregion

        #region Siosein2
        List<MeMedicion1DTO> GetDataEjecCaudales(DateTime fechaInicio, DateTime fechaFin, string ptomedicodi, int lectcodi, int tipoinfocodi);
        #endregion

        #region Numerales Datos Base
        List<MeMedicion1DTO> ListaNumerales_DatosBase_5_7_1(DateTime fecha, string fechaIni, string fechaFin);
        List<MeMedicion1DTO> ListaNumerales_DatosBase_5_7_2(string fechaIni, string fechaFin);
        #endregion
    }
}
