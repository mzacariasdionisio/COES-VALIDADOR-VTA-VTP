using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ISiMensajeRepository
    {
        int Save(string fechaActual, int idFuente, int TipoCorreo, int EstMsg, string Mensaje, string Periodo, int CodModulo, int EmprCodi, int FormatCodi, string usuario, string Correo, string CorreoFrom,string usuarioNom, string Asunto, int flagAdj);
        int UpdateEstado(int estado, int MsgCodi, int CodModulo, int EmprCodi, string usuariomod, string fechamod);
        List<SiMensajeDTO> Listar(string correo, int nroPagina, string orden, int pageSize);
        int TotalMensajes(string correo, string orden);

        #region Siosein
        List<SiMensajeDTO> GetLista(string mailto, string mailfrom, int modcodi, int tipomensaje, int carpeta, int estmsgcodi, DateTime periodo, string msgestado);
        int TotalMensajesxCategoria(string correo, string orden, int modulo, int categoria, int carpeta, string periodo);
        int SaveCorreoSiosein(SiMensajeDTO siMensaje);
        int UpdateCarpeta(int carcodi, string Correo, int modcodi, string msgcodi);
        #endregion

        #region INTERVENCIONES
        int GetMaxId();
        SiMensajeDTO GetMensajePorId(int idMensaje);
        List<SiMensajeDTO> ListSiMensajesIntervencion(int modcodi, int progrcodi, string intercodi, DateTime? dFechaInicio, DateTime? dFechaFin);
        void Save(SiMensajeDTO entity, IDbConnection conn, DbTransaction tran);
        List<SiMensajeDTO> ReporteMensajes(int modcodi, DateTime fechaInicio, DateTime fechaFin, int tipoMensaje);
        #endregion

        #region GESTOR SIOSEIN
        void EliminarMensaje(string msgcodi, string username);
        #endregion

        #region Mejoras Intervenciones

        List<SiMensajeDTO> BusquedaSiMensajesIntervencion(int modcodi, int evenclasecodi, int progrcodi, DateTime? fechaInicio, DateTime? fechaFin);

        #endregion
    }
}