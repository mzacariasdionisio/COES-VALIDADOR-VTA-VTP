using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EVE_MAILS
    /// </summary>
    public class EveMailsRepository : RepositoryBase, IEveMailsRepository
    {
        public EveMailsRepository(string strConn) : base(strConn)
        {
        }

        EveMailsHelper helper = new EveMailsHelper();

        public int Save(EveMailsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mailcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mailturnonum, DbType.Int32, entity.Mailturnonum);
            dbProvider.AddInParameter(command, helper.Mailreprogcausa, DbType.String, entity.Mailreprogcausa);
            dbProvider.AddInParameter(command, helper.Mailcheck1, DbType.String, entity.Mailcheck1);
            dbProvider.AddInParameter(command, helper.Mailhoja, DbType.String, entity.Mailhoja);
            dbProvider.AddInParameter(command, helper.Mailprogramador, DbType.String, entity.Mailprogramador);
            dbProvider.AddInParameter(command, helper.Mailbloquehorario, DbType.Int32, entity.Mailbloquehorario);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Mailfecha, DbType.DateTime, entity.Mailfecha);
            dbProvider.AddInParameter(command, helper.Mailcheck2, DbType.String, entity.Mailcheck2);
            dbProvider.AddInParameter(command, helper.Mailemitido, DbType.String, entity.Mailemitido);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Mailfechaini, DbType.DateTime, entity.Mailfechaini);
            dbProvider.AddInParameter(command, helper.Mailfechafin, DbType.DateTime, entity.Mailfechafin);
            dbProvider.AddInParameter(command, helper.Lastuserproc, DbType.String, entity.Lastuserproc);
            dbProvider.AddInParameter(command, helper.Mailespecialista, DbType.String, entity.Mailespecialista);
            dbProvider.AddInParameter(command, helper.Mailtipoprograma, DbType.Int32, entity.Mailtipoprograma);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Mailhora, DbType.DateTime, entity.Mailhora);
            dbProvider.AddInParameter(command, helper.Mailconsecuencia, DbType.String, entity.Mailconsecuencia);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Mailcoordinador, DbType.String, entity.CoordinadorTurno);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveMailsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Mailturnonum, DbType.Int32, entity.Mailturnonum);
            dbProvider.AddInParameter(command, helper.Mailreprogcausa, DbType.String, entity.Mailreprogcausa);
            dbProvider.AddInParameter(command, helper.Mailcheck1, DbType.String, entity.Mailcheck1);
            dbProvider.AddInParameter(command, helper.Mailhoja, DbType.String, entity.Mailhoja);
            dbProvider.AddInParameter(command, helper.Mailprogramador, DbType.String, entity.Mailprogramador);
            dbProvider.AddInParameter(command, helper.Mailbloquehorario, DbType.Int32, entity.Mailbloquehorario);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Mailfecha, DbType.DateTime, entity.Mailfecha);
            dbProvider.AddInParameter(command, helper.Mailcheck2, DbType.String, entity.Mailcheck2);
            dbProvider.AddInParameter(command, helper.Mailemitido, DbType.String, entity.Mailemitido);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Mailfechaini, DbType.DateTime, entity.Mailfechaini);
            dbProvider.AddInParameter(command, helper.Mailfechafin, DbType.DateTime, entity.Mailfechafin);
            dbProvider.AddInParameter(command, helper.Lastuserproc, DbType.String, entity.Lastuserproc);
            dbProvider.AddInParameter(command, helper.Mailespecialista, DbType.String, entity.Mailespecialista);
            dbProvider.AddInParameter(command, helper.Mailtipoprograma, DbType.Int32, entity.Mailtipoprograma);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Mailhora, DbType.DateTime, entity.Mailhora);
            dbProvider.AddInParameter(command, helper.Mailconsecuencia, DbType.String, entity.Mailconsecuencia);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Mailcoordinador, DbType.String, entity.CoordinadorTurno);
            dbProvider.AddInParameter(command, helper.Mailcodi, DbType.Int32, entity.Mailcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mailcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mailcodi, DbType.Int32, mailcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveMailsDTO GetById(int mailcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mailcodi, DbType.Int32, mailcodi);
            EveMailsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveMailsDTO> List()
        {
            List<EveMailsDTO> entitys = new List<EveMailsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EveMailsDTO> GetByCriteria()
        {
            List<EveMailsDTO> entitys = new List<EveMailsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EveMailsDTO> BuscarOperaciones(int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<EveMailsDTO> entitys = new List<EveMailsDTO>();
            String sql = String.Format(this.helper.ObtenerListado, subCausacodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);


            DbCommand command = dbProvider.GetSqlStringCommand(sql);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveMailsDTO entity = new EveMailsDTO();

                    int iMailcodi = dr.GetOrdinal(this.helper.Mailcodi);
                    if (!dr.IsDBNull(iMailcodi)) entity.Mailcodi = Convert.ToInt32(dr.GetValue(iMailcodi));

                    int iMailfecha = dr.GetOrdinal(this.helper.Mailfecha);
                    if (!dr.IsDBNull(iMailfecha)) entity.Mailfecha = dr.GetDateTime(iMailfecha);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iT = dr.GetOrdinal(this.helper.T);
                    if (!dr.IsDBNull(iT)) entity.T = dr.GetString(iT);

                    int iMailprogramador = dr.GetOrdinal(this.helper.Mailprogramador);
                    if (!dr.IsDBNull(iMailprogramador)) entity.Mailprogramador = dr.GetString(iMailprogramador);

                    int iMailespecialista= dr.GetOrdinal(this.helper.Mailespecialista);
                    if (!dr.IsDBNull(iMailespecialista)) entity.Mailespecialista = dr.GetString(iMailespecialista);

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);


                    int iMailemitido = dr.GetOrdinal(this.helper.Mailemitido);
                    if (!dr.IsDBNull(iMailemitido)) entity.Mailemitido = dr.GetString(iMailemitido);

                    #region REPROGRAMAS 
                    int iMailhoja = dr.GetOrdinal(this.helper.Mailhoja);
                    if (!dr.IsDBNull(iMailhoja)) entity.Mailhoja = dr.GetString(iMailhoja);

                    int iMailreprogcausa = dr.GetOrdinal(this.helper.Mailreprogcausa);
                    if (!dr.IsDBNull(iMailreprogcausa)) entity.Mailreprogcausa = dr.GetString(iMailreprogcausa);

                    int iMailbloquehorario = dr.GetOrdinal(this.helper.Mailbloquehorario);
                    if (!dr.IsDBNull(iMailbloquehorario)) entity.Mailbloquehorario = Convert.ToInt32(dr.GetValue(iMailbloquehorario));
                    #endregion

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<EveMailsDTO> BuscarOperacionesDelTipoReProgramaPorFecha(string fecha)
        {
            List<EveMailsDTO> entitys = new List<EveMailsDTO>();
            String sql = String.Format(this.helper.ObtenerListadoReProgramasPorFecha, fecha);


            DbCommand command = dbProvider.GetSqlStringCommand(sql);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveMailsDTO entity = new EveMailsDTO();

                    int iMailcodi = dr.GetOrdinal(this.helper.Mailcodi);
                    if (!dr.IsDBNull(iMailcodi)) entity.Mailcodi = Convert.ToInt32(dr.GetValue(iMailcodi));

                    int iMailfecha = dr.GetOrdinal(this.helper.Mailfecha);
                    if (!dr.IsDBNull(iMailfecha)) entity.Mailfecha = dr.GetDateTime(iMailfecha);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iT = dr.GetOrdinal(this.helper.T);
                    if (!dr.IsDBNull(iT)) entity.T = dr.GetString(iT);

                    int iMailprogramador = dr.GetOrdinal(this.helper.Mailprogramador);
                    if (!dr.IsDBNull(iMailprogramador)) entity.Mailprogramador = dr.GetString(iMailprogramador);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);


                    int iMailemitido = dr.GetOrdinal(this.helper.Mailemitido);
                    if (!dr.IsDBNull(iMailemitido)) entity.Mailemitido = dr.GetString(iMailemitido);

                    #region REPROGRAMAS 
                    int iMailhoja = dr.GetOrdinal(this.helper.Mailhoja);
                    if (!dr.IsDBNull(iMailhoja)) entity.Mailhoja = dr.GetString(iMailhoja);

                    int iMailreprogcausa = dr.GetOrdinal(this.helper.Mailreprogcausa);
                    if (!dr.IsDBNull(iMailreprogcausa)) entity.Mailreprogcausa = dr.GetString(iMailreprogcausa);

                    int iMailbloquehorario = dr.GetOrdinal(this.helper.Mailbloquehorario);
                    if (!dr.IsDBNull(iMailbloquehorario)) entity.Mailbloquehorario = Convert.ToInt32(dr.GetValue(iMailbloquehorario));
                    #endregion

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public int ObtenerNroFilas(int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<EveMailsDTO> entitys = new List<EveMailsDTO>();
            String sql = String.Format(this.helper.TotalRegistros, subCausacodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);


            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<EveMailsDTO> ExportarEnvioCorreos(int? idTipoOperacion, DateTime fechaInicio, DateTime fechaFin)
        {
            String query = String.Format(helper.SqlExportarEnvioCorreos, idTipoOperacion,
                           fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EveMailsDTO> entitys = new List<EveMailsDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveMailsDTO entity = new EveMailsDTO();

                    int iMAILCODI = dr.GetOrdinal(helper.Mailcodi);
                    if (!dr.IsDBNull(iMAILCODI)) entity.Mailcodi = dr.GetInt32(iMAILCODI);

                    int iFECHA = dr.GetOrdinal(helper.Mailfecha);
                    if (!dr.IsDBNull(iFECHA)) entity.Mailfecha = dr.GetDateTime(iFECHA);

                    int iTIPO = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iTIPO)) entity.Subcausadesc = dr.GetString(iTIPO);

                    int iMailreprogcausa = dr.GetOrdinal(this.helper.Mailreprogcausa);
                    if (!dr.IsDBNull(iMailreprogcausa)) entity.Mailreprogcausa = dr.GetString(iMailreprogcausa);

                    int iT = dr.GetOrdinal(this.helper.T);
                    if (!dr.IsDBNull(iT)) entity.T = dr.GetString(iT);

                    int iMailprogramador = dr.GetOrdinal(this.helper.Mailprogramador);
                    if (!dr.IsDBNull(iMailprogramador)) entity.Mailprogramador = dr.GetString(iMailprogramador);

                    int iMailespecialista = dr.GetOrdinal(this.helper.Mailespecialista);
                    if (!dr.IsDBNull(iMailespecialista)) entity.Mailespecialista = dr.GetString(iMailespecialista);

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iMailemitido = dr.GetOrdinal(this.helper.Mailemitido);
                    if (!dr.IsDBNull(iMailemitido)) entity.Mailemitido = dr.GetString(iMailemitido);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region "COSTO OPORTUNIDAD"
        public List<EveMailsDTO> GetListaReprogramado(DateTime fechaInicio)
        {
            List<EveMailsDTO> entitys = new List<EveMailsDTO>();
            String sql = String.Format(this.helper.SqlGetListaReprogramado, fechaInicio.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //var entity = helper.Create(dr);
                    EveMailsDTO entity = new EveMailsDTO();
                    int iMailfecha = dr.GetOrdinal(this.helper.Mailfecha);
                    if (!dr.IsDBNull(iMailfecha)) entity.Mailfecha = dr.GetDateTime(iMailfecha);

                    int iBloque = dr.GetOrdinal(helper.Mailbloquehorario);
                    if (!dr.IsDBNull(iBloque)) entity.Mailbloquehorario = Convert.ToInt32(dr.GetValue(iBloque));

                    int iMailreprogcausa = dr.GetOrdinal(helper.Mailreprogcausa);
                    if (!dr.IsDBNull(iBloque)) entity.Mailreprogcausa = dr.GetString(iMailreprogcausa);

                    int iMailhoja = dr.GetOrdinal(helper.Mailhoja);
                    if (!dr.IsDBNull(iBloque)) entity.Mailhoja = dr.GetString(iMailhoja);
                    entitys.Add(entity);

                }
            }

            return entitys;
        }
        #endregion

        #region INFORMES SGI

        
        public EveMailsDTO GetFechaMaxProgramaEmitido(DateTime fecha)
        {

            String query = String.Format(helper.SqlGetFechaMaxProgramaEmitido, fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);


            EveMailsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {                    
                    entity = new EveMailsDTO();
                    int iLastdate = dr.GetOrdinal("Lastdate");
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);
                }
            }

            return entity;
        }

        #endregion
    }
}


