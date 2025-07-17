using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RE_INTERRUPCION_SUMINISTRO
    /// </summary>
    public class ReInterrupcionSuministroRepository: RepositoryBase, IReInterrupcionSuministroRepository
    {
        public ReInterrupcionSuministroRepository(string strConn): base(strConn)
        {
        }

        ReInterrupcionSuministroHelper helper = new ReInterrupcionSuministroHelper();

        public int Save(ReInterrupcionSuministroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reintpadre, DbType.Int32, entity.Reintpadre);
            dbProvider.AddInParameter(command, helper.Reintfinal, DbType.String, entity.Reintfinal);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reintestado, DbType.String, entity.Reintestado);
            dbProvider.AddInParameter(command, helper.Reintmotivoanulacion, DbType.String, entity.Reintmotivoanulacion);
            dbProvider.AddInParameter(command, helper.Reintusueliminacion, DbType.String, entity.Reintusueliminacion);
            dbProvider.AddInParameter(command, helper.Reintfecanulacion, DbType.DateTime, entity.Reintfecanulacion);
            dbProvider.AddInParameter(command, helper.Reintcorrelativo, DbType.Int32, entity.Reintcorrelativo);
            dbProvider.AddInParameter(command, helper.Reinttipcliente, DbType.String, entity.Reinttipcliente);
            dbProvider.AddInParameter(command, helper.Reintcliente, DbType.Int32, entity.Reintcliente);
            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, entity.Repentcodi);
            dbProvider.AddInParameter(command, helper.Reintptoentrega, DbType.String, entity.Reintptoentrega);
            dbProvider.AddInParameter(command, helper.Reintnrosuministro, DbType.String, entity.Reintnrosuministro);
            dbProvider.AddInParameter(command, helper.Rentcodi, DbType.Int32, entity.Rentcodi);
            dbProvider.AddInParameter(command, helper.Reintaplicacionnumeral, DbType.Int32, entity.Reintaplicacionnumeral);
            dbProvider.AddInParameter(command, helper.Reintenergiasemestral, DbType.Decimal, entity.Reintenergiasemestral);
            dbProvider.AddInParameter(command, helper.Reintinctolerancia, DbType.String, entity.Reintinctolerancia);
            dbProvider.AddInParameter(command, helper.Retintcodi, DbType.Int32, entity.Retintcodi);
            dbProvider.AddInParameter(command, helper.Recintcodi, DbType.Int32, entity.Recintcodi);
            dbProvider.AddInParameter(command, helper.Reintni, DbType.Decimal, entity.Reintni);
            dbProvider.AddInParameter(command, helper.Reintki, DbType.Decimal, entity.Reintki);
            dbProvider.AddInParameter(command, helper.Reintfejeinicio, DbType.DateTime, entity.Reintfejeinicio);
            dbProvider.AddInParameter(command, helper.Reintfejefin, DbType.DateTime, entity.Reintfejefin);
            dbProvider.AddInParameter(command, helper.Reintfproginicio, DbType.DateTime, entity.Reintfproginicio);
            dbProvider.AddInParameter(command, helper.Reintfprogfin, DbType.DateTime, entity.Reintfprogfin);
            dbProvider.AddInParameter(command, helper.Reintcausaresumida, DbType.String, entity.Reintcausaresumida);
            dbProvider.AddInParameter(command, helper.Reinteie, DbType.Decimal, entity.Reinteie);
            dbProvider.AddInParameter(command, helper.Reintresarcimiento, DbType.Decimal, entity.Reintresarcimiento);
            dbProvider.AddInParameter(command, helper.Reintevidencia, DbType.String, entity.Reintevidencia);
            dbProvider.AddInParameter(command, helper.Reintdescontroversia, DbType.String, entity.Reintdescontroversia);
            dbProvider.AddInParameter(command, helper.Reintcomentario, DbType.String, entity.Reintcomentario);
            dbProvider.AddInParameter(command, helper.Reintusucreacion, DbType.String, entity.Reintusucreacion);
            dbProvider.AddInParameter(command, helper.Reintfeccreacion, DbType.DateTime, entity.Reintfeccreacion);
            dbProvider.AddInParameter(command, helper.Reintreftrimestral, DbType.String, entity.Reintreftrimestral);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReInterrupcionSuministroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                       
            dbProvider.AddInParameter(command, helper.Reintpadre, DbType.Int32, entity.Reintpadre);
            dbProvider.AddInParameter(command, helper.Reintfinal, DbType.String, entity.Reintfinal);            
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, entity.Reintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, reintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReInterrupcionSuministroDTO GetById(int reintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, reintcodi);
            ReInterrupcionSuministroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReInterrupcionSuministroDTO> List()
        {
            List<ReInterrupcionSuministroDTO> entitys = new List<ReInterrupcionSuministroDTO>();
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

        public List<ReInterrupcionSuministroDTO> GetByCriteria()
        {
            List<ReInterrupcionSuministroDTO> entitys = new List<ReInterrupcionSuministroDTO>();
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

        public List<ReInterrupcionSuministroDTO> ObtenerPorEmpresaPeriodo(int idEmpresa, int idPeriodo)
        {
            List<ReInterrupcionSuministroDTO> entitys = new List<ReInterrupcionSuministroDTO>();
            string sql = string.Format(helper.SqlObtenerPorEmpresaPeriodo, idEmpresa, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void AnularInterrupcion(int id, string comentario, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlAnularInterrupcion);

            dbProvider.AddInParameter(command, helper.Reintmotivoanulacion, DbType.String, comentario);
            dbProvider.AddInParameter(command, helper.Reintusueliminacion, DbType.String, username);           
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);

        }

        public List<ReInterrupcionSuministroDTO> ObtenerInterrupcionesPorResponsable(int idEmpresa, int idPeriodo)
        {
            List<ReInterrupcionSuministroDTO> entitys = new List<ReInterrupcionSuministroDTO>();
            string sql = string.Format(helper.SqlObtenerInterrupcionPorResponsable, idEmpresa, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReInterrupcionSuministroDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprresponsable = dr.GetOrdinal(helper.Emprresponsable);
                    if (!dr.IsDBNull(iEmprresponsable)) entity.Emprresponsable = Convert.ToInt32(dr.GetValue(iEmprresponsable));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReInterrupcionSuministroDTO> ObtenerConsolidado(int idPeriodo, int suministrador, int idCausaInterrupcion, 
            string estado, int ptoEntrega, string final, int responsable, string disposicion, string compensacion)
        {
            List<ReInterrupcionSuministroDTO> entitys = new List<ReInterrupcionSuministroDTO>();
            ReInterrupcionSuministroDetHelper detHelper = new ReInterrupcionSuministroDetHelper();
            string sql = string.Format(helper.SqlObtenerConsolidado, idPeriodo, suministrador, idCausaInterrupcion, estado, ptoEntrega, final,
                responsable, disposicion, compensacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReInterrupcionSuministroDTO entity = new ReInterrupcionSuministroDTO();

                    int iReintcodi = dr.GetOrdinal(helper.Reintcodi);
                    if (!dr.IsDBNull(iReintcodi)) entity.Reintcodi = Convert.ToInt32(dr.GetValue(iReintcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iReintestado = dr.GetOrdinal(helper.Reintestado);
                    if (!dr.IsDBNull(iReintestado)) entity.Reintestado = dr.GetString(iReintestado);

                    int iReintcorrelativo = dr.GetOrdinal(helper.Reintcorrelativo);
                    if (!dr.IsDBNull(iReintcorrelativo)) entity.Reintcorrelativo = Convert.ToInt32(dr.GetValue(iReintcorrelativo));

                    int iReinttipcliente = dr.GetOrdinal(helper.Reinttipcliente);
                    if (!dr.IsDBNull(iReinttipcliente)) entity.Reinttipcliente = dr.GetString(iReinttipcliente);

                    int iCliente = dr.GetOrdinal(helper.Cliente);
                    if (!dr.IsDBNull(iCliente)) entity.Cliente = dr.GetString(iCliente);

                    int iReintptoentrega = dr.GetOrdinal(helper.Reintptoentrega);
                    if (!dr.IsDBNull(iReintptoentrega)) entity.Reintptoentrega = dr.GetString(iReintptoentrega);

                    int iRepentcodi = dr.GetOrdinal(helper.Repentcodi);
                    if (!dr.IsDBNull(iRepentcodi)) entity.Repentcodi = Convert.ToInt32(dr.GetValue(iRepentcodi));

                    int iReintnrosuministro = dr.GetOrdinal(helper.Reintnrosuministro);
                    if (!dr.IsDBNull(iReintnrosuministro)) entity.Reintnrosuministro = dr.GetString(iReintnrosuministro);

                    int iNivelTension = dr.GetOrdinal(helper.NivelTension);
                    if (!dr.IsDBNull(iNivelTension)) entity.NivelTension = dr.GetString(iNivelTension);

                    int iReintaplicacionnumeral = dr.GetOrdinal(helper.Reintaplicacionnumeral);
                    if (!dr.IsDBNull(iReintaplicacionnumeral)) entity.Reintaplicacionnumeral = Convert.ToInt32(dr.GetValue(iReintaplicacionnumeral));

                    int iReintenergiasemestral = dr.GetOrdinal(helper.Reintenergiasemestral);
                    if (!dr.IsDBNull(iReintenergiasemestral)) entity.Reintenergiasemestral = dr.GetDecimal(iReintenergiasemestral);

                    int iReintinctolerancia = dr.GetOrdinal(helper.Reintinctolerancia);
                    if (!dr.IsDBNull(iReintinctolerancia)) entity.Reintinctolerancia = dr.GetString(iReintinctolerancia);

                    int iTipoInterrupcion = dr.GetOrdinal(helper.TipoInterrupcion);
                    if (!dr.IsDBNull(iTipoInterrupcion)) entity.TipoInterrupcion = dr.GetString(iTipoInterrupcion);

                    int iCausaInterrupcion = dr.GetOrdinal(helper.CausaInterrupcion);
                    if (!dr.IsDBNull(iCausaInterrupcion)) entity.CausaInterrupcion = dr.GetString(iCausaInterrupcion);

                    int iRecintcodi = dr.GetOrdinal(helper.Recintcodi);
                    if (!dr.IsDBNull(iRecintcodi)) entity.Recintcodi = Convert.ToInt32(dr.GetValue(iRecintcodi));

                    int iReintni = dr.GetOrdinal(helper.Reintni);
                    if (!dr.IsDBNull(iReintni)) entity.Reintni = dr.GetDecimal(iReintni);

                    int iReintki = dr.GetOrdinal(helper.Reintki);
                    if (!dr.IsDBNull(iReintki)) entity.Reintki = dr.GetDecimal(iReintki);

                    int iReintfejeinicio = dr.GetOrdinal(helper.Reintfejeinicio);
                    if (!dr.IsDBNull(iReintfejeinicio)) entity.Reintfejeinicio = dr.GetDateTime(iReintfejeinicio);

                    int iReintfejefin = dr.GetOrdinal(helper.Reintfejefin);
                    if (!dr.IsDBNull(iReintfejefin)) entity.Reintfejefin = dr.GetDateTime(iReintfejefin);

                    int iReintfproginicio = dr.GetOrdinal(helper.Reintfproginicio);
                    if (!dr.IsDBNull(iReintfproginicio)) entity.Reintfproginicio = dr.GetDateTime(iReintfproginicio);

                    int iReintfprogfin = dr.GetOrdinal(helper.Reintfprogfin);
                    if (!dr.IsDBNull(iReintfprogfin)) entity.Reintfprogfin = dr.GetDateTime(iReintfprogfin);

                    int iReintcausaresumida = dr.GetOrdinal(helper.Reintcausaresumida);
                    if (!dr.IsDBNull(iReintcausaresumida)) entity.Reintcausaresumida = dr.GetString(iReintcausaresumida);

                    int iReinteie = dr.GetOrdinal(helper.Reinteie);
                    if (!dr.IsDBNull(iReinteie)) entity.Reinteie = dr.GetDecimal(iReinteie);

                    int iReintresarcimiento = dr.GetOrdinal(helper.Reintresarcimiento);
                    if (!dr.IsDBNull(iReintresarcimiento)) entity.Reintresarcimiento = dr.GetDecimal(iReintresarcimiento);

                    int iReintevidencia = dr.GetOrdinal(helper.Reintevidencia);
                    if (!dr.IsDBNull(iReintevidencia)) entity.Reintevidencia = dr.GetString(iReintevidencia);

                    int iReintdescontroversia = dr.GetOrdinal(helper.Reintdescontroversia);
                    if (!dr.IsDBNull(iReintdescontroversia)) entity.Reintdescontroversia = dr.GetString(iReintdescontroversia);

                    int iReintcomentario = dr.GetOrdinal(helper.Reintcomentario);
                    if (!dr.IsDBNull(iReintcomentario)) entity.Reintcomentario = dr.GetString(iReintcomentario);

                    int iReintusucreacion = dr.GetOrdinal(helper.Reintusucreacion);
                    if (!dr.IsDBNull(iReintusucreacion)) entity.Reintusucreacion = dr.GetString(iReintusucreacion);

                    int iReintfeccreacion = dr.GetOrdinal(helper.Reintfeccreacion);
                    if (!dr.IsDBNull(iReintfeccreacion)) entity.Reintfeccreacion = dr.GetDateTime(iReintfeccreacion);

                    int iReintdorden = dr.GetOrdinal(detHelper.Reintdorden);
                    if (!dr.IsDBNull(iReintdorden)) entity.OrdenDetalle = Convert.ToInt32(dr.GetValue(iReintdorden));

                    int iEmprresponsable = dr.GetOrdinal(helper.Emprresponsable);
                    if (!dr.IsDBNull(iEmprresponsable)) entity.Responsable = dr.GetString(iEmprresponsable);

                    int iReintdorcentaje = dr.GetOrdinal(detHelper.Reintdorcentaje);
                    if (!dr.IsDBNull(iReintdorcentaje)) entity.Reintdorcentaje = dr.GetDecimal(iReintdorcentaje);

                    int iReintdconformidadresp = dr.GetOrdinal(detHelper.Reintdconformidadresp);
                    if (!dr.IsDBNull(iReintdconformidadresp)) entity.Reintdconformidadresp = dr.GetString(iReintdconformidadresp);

                    int iReintdobservacionresp = dr.GetOrdinal(detHelper.Reintdobservacionresp);
                    if (!dr.IsDBNull(iReintdobservacionresp)) entity.Reintdobservacionresp = dr.GetString(iReintdobservacionresp);

                    int iReintddetalleresp = dr.GetOrdinal(detHelper.Reintddetalleresp);
                    if (!dr.IsDBNull(iReintddetalleresp)) entity.Reintddetalleresp = dr.GetString(iReintddetalleresp);

                    int iReintdcomentarioresp = dr.GetOrdinal(detHelper.Reintdcomentarioresp);
                    if (!dr.IsDBNull(iReintdcomentarioresp)) entity.Reintdcomentarioresp = dr.GetString(iReintdcomentarioresp);

                    int iReintdevidenciaresp = dr.GetOrdinal(detHelper.Reintdevidenciaresp);
                    if (!dr.IsDBNull(iReintdevidenciaresp)) entity.Reintdevidenciaresp = dr.GetString(iReintdevidenciaresp);

                    int iReintdconformidadsumi = dr.GetOrdinal(detHelper.Reintdconformidadsumi);
                    if (!dr.IsDBNull(iReintdconformidadsumi)) entity.Reintdconformidadsumi = dr.GetString(iReintdconformidadsumi);

                    int iReintdcomentariosumi = dr.GetOrdinal(detHelper.Reintdcomentariosumi);
                    if (!dr.IsDBNull(iReintdcomentariosumi)) entity.Reintdcomentariosumi = dr.GetString(iReintdcomentariosumi);

                    int iReintdevidenciasumi = dr.GetOrdinal(detHelper.Reintdevidenciasumi);
                    if (!dr.IsDBNull(iReintdevidenciasumi)) entity.Reintdevidenciasumi = dr.GetString(iReintdevidenciasumi);

                    int iReintddisposicion = dr.GetOrdinal(detHelper.Reintddisposicion);
                    if (!dr.IsDBNull(iReintddisposicion)) entity.Reintddispocision = dr.GetString(iReintddisposicion);

                    int iReintdcompensacion = dr.GetOrdinal(detHelper.Reintdcompcero);
                    if (!dr.IsDBNull(iReintdcompensacion)) entity.Reintdcompceo = dr.GetString(iReintdcompensacion);

                    int iReintreftrimestral = dr.GetOrdinal(helper .Reintreftrimestral);
                    if (!dr.IsDBNull(iReintreftrimestral)) entity.Reintreftrimestral = dr.GetString(iReintreftrimestral);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarDecisionControveria(int id, string decision, string comentario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarDecisionControversia);

            dbProvider.AddInParameter(command, helper.Reintdescontroversia, DbType.String, decision);
            dbProvider.AddInParameter(command, helper.Reintcomentario, DbType.String, comentario);
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<ReInterrupcionSuministroDTO> ObtenerTrazabilidad(int idPeriodo, int idSuministrador)
        {
            List<ReInterrupcionSuministroDTO> entitys = new List<ReInterrupcionSuministroDTO>();            
            string sql = string.Format(helper.SqlObtenerTrazabilidad, idPeriodo, idSuministrador);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReInterrupcionSuministroDTO entity = new ReInterrupcionSuministroDTO();

                    int iReintcodi = dr.GetOrdinal(helper.Reintcodi);
                    if (!dr.IsDBNull(iReintcodi)) entity.Reintcodi = Convert.ToInt32(dr.GetValue(iReintcodi));

                    int iReintpadre = dr.GetOrdinal(helper.Reintpadre);
                    if (!dr.IsDBNull(iReintpadre)) entity.Reintpadre = Convert.ToInt32(dr.GetValue(iReintpadre));

                    int iReintfinal = dr.GetOrdinal(helper.Reintfinal);
                    if (!dr.IsDBNull(iReintfinal)) entity.Reintfinal = dr.GetString(iReintfinal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarArchivo(int id, string extension)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarArchivo);

            dbProvider.AddInParameter(command, helper.Reintevidencia, DbType.String, extension);            
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<ReInterrupcionSuministroDTO> ObtenerNotificacionInterrupcion(List<int> ids)
        {
            List<ReInterrupcionSuministroDTO> entitys = new List<ReInterrupcionSuministroDTO>();
            ReInterrupcionSuministroDetHelper detHelper = new ReInterrupcionSuministroDetHelper();
            string sql = string.Format(helper.SqlObtenerNotificacionInterrupcion, string.Join(",", ids.Select(n => n.ToString()).ToArray()));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReInterrupcionSuministroDTO entity = new ReInterrupcionSuministroDTO();

                    int iReintcodi = dr.GetOrdinal(helper.Reintcodi);
                    if (!dr.IsDBNull(iReintcodi)) entity.Reintcodi = Convert.ToInt32(dr.GetValue(iReintcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iReintestado = dr.GetOrdinal(helper.Reintestado);
                    if (!dr.IsDBNull(iReintestado)) entity.Reintestado = dr.GetString(iReintestado);

                    int iReintcorrelativo = dr.GetOrdinal(helper.Reintcorrelativo);
                    if (!dr.IsDBNull(iReintcorrelativo)) entity.Reintcorrelativo = Convert.ToInt32(dr.GetValue(iReintcorrelativo));

                    int iReinttipcliente = dr.GetOrdinal(helper.Reinttipcliente);
                    if (!dr.IsDBNull(iReinttipcliente)) entity.Reinttipcliente = dr.GetString(iReinttipcliente);

                    int iCliente = dr.GetOrdinal(helper.Cliente);
                    if (!dr.IsDBNull(iCliente)) entity.Cliente = dr.GetString(iCliente);

                    int iReintptoentrega = dr.GetOrdinal(helper.Reintptoentrega);
                    if (!dr.IsDBNull(iReintptoentrega)) entity.Reintptoentrega = dr.GetString(iReintptoentrega);

                    int iReintnrosuministro = dr.GetOrdinal(helper.Reintnrosuministro);
                    if (!dr.IsDBNull(iReintnrosuministro)) entity.Reintnrosuministro = dr.GetString(iReintnrosuministro);

                    int iNivelTension = dr.GetOrdinal(helper.NivelTension);
                    if (!dr.IsDBNull(iNivelTension)) entity.NivelTension = dr.GetString(iNivelTension);

                    int iReintaplicacionnumeral = dr.GetOrdinal(helper.Reintaplicacionnumeral);
                    if (!dr.IsDBNull(iReintaplicacionnumeral)) entity.Reintaplicacionnumeral = Convert.ToInt32(dr.GetValue(iReintaplicacionnumeral));

                    int iReintenergiasemestral = dr.GetOrdinal(helper.Reintenergiasemestral);
                    if (!dr.IsDBNull(iReintenergiasemestral)) entity.Reintenergiasemestral = dr.GetDecimal(iReintenergiasemestral);

                    int iReintinctolerancia = dr.GetOrdinal(helper.Reintinctolerancia);
                    if (!dr.IsDBNull(iReintinctolerancia)) entity.Reintinctolerancia = dr.GetString(iReintinctolerancia);

                    int iTipoInterrupcion = dr.GetOrdinal(helper.TipoInterrupcion);
                    if (!dr.IsDBNull(iTipoInterrupcion)) entity.TipoInterrupcion = dr.GetString(iTipoInterrupcion);

                    int iCausaInterrupcion = dr.GetOrdinal(helper.CausaInterrupcion);
                    if (!dr.IsDBNull(iCausaInterrupcion)) entity.CausaInterrupcion = dr.GetString(iCausaInterrupcion);

                    int iReintni = dr.GetOrdinal(helper.Reintni);
                    if (!dr.IsDBNull(iReintni)) entity.Reintni = dr.GetDecimal(iReintni);

                    int iReintki = dr.GetOrdinal(helper.Reintki);
                    if (!dr.IsDBNull(iReintki)) entity.Reintki = dr.GetDecimal(iReintki);

                    int iReintfejeinicio = dr.GetOrdinal(helper.Reintfejeinicio);
                    if (!dr.IsDBNull(iReintfejeinicio)) entity.Reintfejeinicio = dr.GetDateTime(iReintfejeinicio);

                    int iReintfejefin = dr.GetOrdinal(helper.Reintfejefin);
                    if (!dr.IsDBNull(iReintfejefin)) entity.Reintfejefin = dr.GetDateTime(iReintfejefin);

                    int iReintfproginicio = dr.GetOrdinal(helper.Reintfproginicio);
                    if (!dr.IsDBNull(iReintfproginicio)) entity.Reintfproginicio = dr.GetDateTime(iReintfproginicio);

                    int iReintfprogfin = dr.GetOrdinal(helper.Reintfprogfin);
                    if (!dr.IsDBNull(iReintfprogfin)) entity.Reintfprogfin = dr.GetDateTime(iReintfprogfin);

                    int iReintcausaresumida = dr.GetOrdinal(helper.Reintcausaresumida);
                    if (!dr.IsDBNull(iReintcausaresumida)) entity.Reintcausaresumida = dr.GetString(iReintcausaresumida);

                    int iReinteie = dr.GetOrdinal(helper.Reinteie);
                    if (!dr.IsDBNull(iReinteie)) entity.Reinteie = dr.GetDecimal(iReinteie);

                    int iReintresarcimiento = dr.GetOrdinal(helper.Reintresarcimiento);
                    if (!dr.IsDBNull(iReintresarcimiento)) entity.Reintresarcimiento = dr.GetDecimal(iReintresarcimiento);

                    int iReintevidencia = dr.GetOrdinal(helper.Reintevidencia);
                    if (!dr.IsDBNull(iReintevidencia)) entity.Reintevidencia = dr.GetString(iReintevidencia);

                    int iReintdescontroversia = dr.GetOrdinal(helper.Reintdescontroversia);
                    if (!dr.IsDBNull(iReintdescontroversia)) entity.Reintdescontroversia = dr.GetString(iReintdescontroversia);

                    int iReintcomentario = dr.GetOrdinal(helper.Reintcomentario);
                    if (!dr.IsDBNull(iReintcomentario)) entity.Reintcomentario = dr.GetString(iReintcomentario);

                    int iReintusucreacion = dr.GetOrdinal(helper.Reintusucreacion);
                    if (!dr.IsDBNull(iReintusucreacion)) entity.Reintusucreacion = dr.GetString(iReintusucreacion);

                    int iReintfeccreacion = dr.GetOrdinal(helper.Reintfeccreacion);
                    if (!dr.IsDBNull(iReintfeccreacion)) entity.Reintfeccreacion = dr.GetDateTime(iReintfeccreacion);

                    int iReintdorden = dr.GetOrdinal(detHelper.Reintdorden);
                    if (!dr.IsDBNull(iReintdorden)) entity.OrdenDetalle = Convert.ToInt32(dr.GetValue(iReintdorden));

                    int iEmprresponsable = dr.GetOrdinal(helper.Emprresponsable);
                    if (!dr.IsDBNull(iEmprresponsable)) entity.Responsable = dr.GetString(iEmprresponsable);

                    int iReintdorcentaje = dr.GetOrdinal(detHelper.Reintdorcentaje);
                    if (!dr.IsDBNull(iReintdorcentaje)) entity.Reintdorcentaje = dr.GetDecimal(iReintdorcentaje);

                    int iReintdcodi = dr.GetOrdinal(detHelper.Reintdcodi);
                    if (!dr.IsDBNull(iReintdcodi)) entity.Reintdcodi = Convert.ToInt32(dr.GetValue(iReintdcodi));

                    int iReintdconformidadresp = dr.GetOrdinal(detHelper.Reintdconformidadresp);
                    if (!dr.IsDBNull(iReintdconformidadresp)) entity.Reintdconformidadresp = dr.GetString(iReintdconformidadresp);

                    int iReintdobservacionresp = dr.GetOrdinal(detHelper.Reintdobservacionresp);
                    if (!dr.IsDBNull(iReintdobservacionresp)) entity.Reintdobservacionresp = dr.GetString(iReintdobservacionresp);

                    int iReintddetalleresp = dr.GetOrdinal(detHelper.Reintddetalleresp);
                    if (!dr.IsDBNull(iReintddetalleresp)) entity.Reintddetalleresp = dr.GetString(iReintddetalleresp);

                    int iReintdcomentarioresp = dr.GetOrdinal(detHelper.Reintdcomentarioresp);
                    if (!dr.IsDBNull(iReintdcomentarioresp)) entity.Reintdcomentarioresp = dr.GetString(iReintdcomentarioresp);

                    int iReintdevidenciaresp = dr.GetOrdinal(detHelper.Reintdevidenciaresp);
                    if (!dr.IsDBNull(iReintdevidenciaresp)) entity.Reintdevidenciaresp = dr.GetString(iReintdevidenciaresp);

                    int iReintdconformidadsumi = dr.GetOrdinal(detHelper.Reintdconformidadsumi);
                    if (!dr.IsDBNull(iReintdconformidadsumi)) entity.Reintdconformidadsumi = dr.GetString(iReintdconformidadsumi);

                    int iReintdcomentariosumi = dr.GetOrdinal(detHelper.Reintdcomentariosumi);
                    if (!dr.IsDBNull(iReintdcomentariosumi)) entity.Reintdcomentariosumi = dr.GetString(iReintdcomentariosumi);

                    int iReintdevidenciasumi = dr.GetOrdinal(detHelper.Reintdevidenciasumi);
                    if (!dr.IsDBNull(iReintdevidenciasumi)) entity.Reintdevidenciasumi = dr.GetString(iReintdevidenciasumi);

                    int iReintreftrimestral = dr.GetOrdinal(helper.Reintreftrimestral);
                    if (!dr.IsDBNull(iReintreftrimestral)) entity.Reintreftrimestral = dr.GetString(iReintreftrimestral);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarResarcimiento(int id, decimal ei, decimal resarcimiento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarResarcimiento);

            dbProvider.AddInParameter(command, helper.Reinteie, DbType.Decimal, ei);
            dbProvider.AddInParameter(command, helper.Reintresarcimiento, DbType.Decimal, resarcimiento);
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
