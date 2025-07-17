using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_EMPRESA_CORREO
    /// </summary>
    public interface ISiEmpresaCorreoRepository
    {
        int Save(SiEmpresaCorreoDTO entity);        
        void Update(SiEmpresaCorreoDTO entity);
        void Delete(int empcorcodi);
        SiEmpresaCorreoDTO GetById(int empcorcodi);
        List<SiEmpresaCorreoDTO> List();
        List<SiEmpresaCorreoDTO> GetByCriteria(int modcodi, string tipoempresa, int emprcodi);
        List<SiEmpresaCorreoDTO> ObtenerEmpresasIncumplimiento(int etacodi, DateTime fecha);
        List<int> ObtenerEmpresasDisponibles();
        void ActualizarIndNotifacion(int emprcodi, string indnotificacion, string lastuser);
        List<SiEmpresaCorreoDTO> ObtenerCorreosPorMoodulo(int modCodi);
        List<SiEmpresaCorreoDTO> ObtenerCorreosPorEmpresaModulo(int idModulo, int idEmpresa);
        List<SiEmpresaCorreoDTO> ObtenerCorreosPorEmpresaModuloAdicional(int idEmpresa, int idModulo);
        List<SiEmpresaCorreoDTO> ObtenerPesonasContactoExportacion(string tipoAgente, int tipoEmpresa);
        List<SiEmpresaCorreoDTO> ObtenerCorreosNotificacion(string ruc);
        List<String> ObtenerListaCorreosNotificacion(string ruc,string tipo);

        #region Resarcimiento
        void SaveTransaccional(SiEmpresaCorreoDTO entity, IDbConnection connection, IDbTransaction transaction);
        int GetMaxId();
        List<SiEmpresaCorreoDTO> ObtenerCorreosPorEmpresaResarcimiento(int idEmpresa);
        void DeleteResarcimiento(int emprcodi);
        List<SiEmpresaCorreoDTO> ListarSoloResarcimiento();
        #endregion
    }
}
