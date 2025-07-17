using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RPF_MEDICION60
    /// </summary>
    public interface IRpfMedicion60Repository
    {
        int Save(RpfMedicion60DTO entity);
        void Update(RpfMedicion60DTO entity);
        void Delete(int rpfmedcodi, string tableName);
        RpfMedicion60DTO GetById(int rpfmedcodi);
        List<RpfMedicion60DTO> List();
        List<RpfMedicion60DTO> GetByCriteria();
        void GrabarMasivo(List<RpfMedicion60DTO> entitys, string nombreTabla);
        int GetMaxId(string tableName);
        void CrearTabla(int i, string nombre);
        int VerificarExistenciaTabla(string nombre);
        List<RpfMedicion60DTO> GetInformacionAGCExtranet(DateTime fechaConsulta,string strFecha, string strIdsUrs, string strIdsEquipos, string strTipoDato);
        List<RpfMedicion60DTO> ObtenerConsulta(int idUrs, int idEquipo, int tipoDato, DateTime fechaInicio, DateTime fechaFin, string tableName);
        List<RpfMedicion60DTO> ObtenerReporteCumplimiento(DateTime fecha, string grupos, string tableName);
    }
}
