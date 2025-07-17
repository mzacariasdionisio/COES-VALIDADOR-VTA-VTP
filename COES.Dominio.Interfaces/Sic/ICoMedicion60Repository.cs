using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_MEDICION60
    /// </summary>
    public interface ICoMedicion60Repository
    {
        int Save(CoMedicion60DTO entity);
        void Update(CoMedicion60DTO entity);
        void Delete(int comedicodi);
        CoMedicion60DTO GetById(int comedicodi);
        List<CoMedicion60DTO> List();
        List<CoMedicion60DTO> GetByCriteria();
        List<CoMedicion60DTO> ListarUltimoMinutoPorRango(string rangoDias, string lstStrCanalcodis);
        List<CoMedicion60DTO> ListarUltimoMinutoDiaAnteriorMuchasTablas(string query);
        int Save(CoMedicion60DTO entity, IDbConnection connection, IDbTransaction transaction);
        int GetMaximoID();
        void GrabarDatosXBloquesMed60(List<CoMedicion60DTO> entitys);
        void GrabarDataXBloquesMed60(List<CoMedicion60DTO> entitys, string nombTabla);        
        void EliminarMedicion60XTabla(string nombTabla, string strProdiacodis);        
        List<CoMedicion60DTO> ListarTablas(string tablas);
        List<CoMedicion60DTO> ObtenerDataReporte(int prodiacodi, string canalcodis, string tiposdatosids, string nombreTabla);
        List<CoMedicion60DTO> ObtenerDataReporteFP(int prodiacodi, int idUrs, int cotidacodi, string nombreTabla);
        void EliminarDataTabla(string nombreTabla);
        int ObtenerIdInicial(string nombTabla);
        List<CoMedicion60DTO> ObtenerDataReporteFU(int prodiacodi, string nombreTabla, int tipo);
        void CrearTabla(int i, string nombre);
        int VerificarExistenciaTabla(string nombre);

        void ProcesarTabla(string tableName, string prodiasCodi);
        List<CoMedicion60DTO> GetInformacionAGC(DateTime fechaConsulta, string strfecha, string strGrupocodis, string strEquicodis, string strCotidacodis);
    }
}
