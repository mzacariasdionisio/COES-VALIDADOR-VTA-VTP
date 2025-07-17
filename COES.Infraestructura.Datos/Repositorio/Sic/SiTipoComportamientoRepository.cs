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
    /// Clase de acceso a datos de la tabla SI_TIPO_COMPORTAMIENTO
    /// </summary>
    public class SiTipoComportamientoRepository : RepositoryBase, ISiTipoComportamientoRepository
    {
        public SiTipoComportamientoRepository(string strConn)
            : base(strConn)
        {
        }

        SiTipoComportamientoHelper helper = new SiTipoComportamientoHelper();

        public int Save(SiTipoComportamientoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tipocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tipoprincipal, DbType.String, entity.Tipoprincipal);
            dbProvider.AddInParameter(command, helper.Tipotipagente, DbType.String, entity.Tipotipagente);
            dbProvider.AddInParameter(command, helper.Tipodocsustentatorio, DbType.String, entity.Tipodocsustentatorio);
            dbProvider.AddInParameter(command, helper.Tipoarcdigitalizado, DbType.String, entity.Tipoarcdigitalizado);
            dbProvider.AddInParameter(command, helper.Tipoarcdigitalizadofilename, DbType.String, entity.Tipoarcdigitalizadofilename);
            dbProvider.AddInParameter(command, helper.Tipopotenciainstalada, DbType.String, entity.Tipopotenciainstalada);
            dbProvider.AddInParameter(command, helper.Tiponrocentrales, DbType.String, entity.Tiponrocentrales);
            dbProvider.AddInParameter(command, helper.Tipolineatrans500, DbType.String, entity.Tipolineatrans500);
            dbProvider.AddInParameter(command, helper.Tipolineatrans220, DbType.String, entity.Tipolineatrans220);
            dbProvider.AddInParameter(command, helper.Tipolineatrans138, DbType.String, entity.Tipolineatrans138);
            dbProvider.AddInParameter(command, helper.Tipolineatrans500km, DbType.String, entity.Tipolineatrans500km);
            dbProvider.AddInParameter(command, helper.Tipolineatrans220km, DbType.String, entity.Tipolineatrans220km);
            dbProvider.AddInParameter(command, helper.Tipolineatrans138km, DbType.String, entity.Tipolineatrans138km);
            dbProvider.AddInParameter(command, helper.Tipototallineastransmision, DbType.String, entity.Tipototallineastransmision);
            dbProvider.AddInParameter(command, helper.Tipomaxdemandacoincidente, DbType.String, entity.Tipomaxdemandacoincidente);
            dbProvider.AddInParameter(command, helper.Tipomaxdemandacontratada, DbType.String, entity.Tipomaxdemandacontratada);
            dbProvider.AddInParameter(command, helper.Tiponumsuministrador, DbType.String, entity.Tiponumsuministrador);
            dbProvider.AddInParameter(command, helper.Tipousucreacion, DbType.String, entity.Tipousucreacion);
            dbProvider.AddInParameter(command, helper.Tipofeccreacion, DbType.DateTime, entity.Tipofeccreacion);
            dbProvider.AddInParameter(command, helper.Tipousumodificacion, DbType.String, entity.Tipousumodificacion);
            dbProvider.AddInParameter(command, helper.Tipofecmodificacion, DbType.DateTime, entity.Tipofecmodificacion);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            //dbProvider.AddInParameter(command, helper.Tipoarcdigitalizadofilename, DbType.String, entity.Tipoarcdigitalizadofilename);
            dbProvider.AddInParameter(command, helper.Tipodocname1, DbType.String, entity.Tipodocname1);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename1, DbType.String, entity.Tipodocadjfilename1);
            dbProvider.AddInParameter(command, helper.Tipodocname2, DbType.String, entity.Tipodocname2);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename2, DbType.String, entity.Tipodocadjfilename2);
            dbProvider.AddInParameter(command, helper.Tipodocname3, DbType.String, entity.Tipodocname3);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename3, DbType.String, entity.Tipodocadjfilename3);
            dbProvider.AddInParameter(command, helper.Tipodocname4, DbType.String, entity.Tipodocname4);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename4, DbType.String, entity.Tipodocadjfilename4);
            dbProvider.AddInParameter(command, helper.Tipodocname5, DbType.String, entity.Tipodocname5);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename5, DbType.String, entity.Tipodocadjfilename5);
            //dbProvider.AddInParameter(command, helper.Tipoarcdigitalizadofilename, DbType.String, entity.Tipoarcdigitalizadofilename);
            dbProvider.AddInParameter(command, helper.TipoBaja, DbType.String, entity.Tipobaja);
            dbProvider.AddInParameter(command, helper.TipoInicial, DbType.String, entity.Tipoinicial);
            dbProvider.AddInParameter(command, helper.Tipocomentario, DbType.String, entity.Tipocomentario);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(SiTipoComportamientoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);


            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Tipocodi;
            param.Value = id;
            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Tipoprincipal; param.Value = entity.Tipoprincipal; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipotipagente; param.Value = entity.Tipotipagente; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocsustentatorio; param.Value = entity.Tipodocsustentatorio; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipoarcdigitalizado; param.Value = entity.Tipoarcdigitalizado; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipoarcdigitalizadofilename; param.Value = entity.Tipoarcdigitalizadofilename; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipopotenciainstalada; param.Value = entity.Tipopotenciainstalada == null ? "" : entity.Tipopotenciainstalada; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tiponrocentrales; param.Value = entity.Tiponrocentrales == null ? "" : entity.Tiponrocentrales; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans500; param.Value = entity.Tipolineatrans500 == null ? "" : entity.Tipolineatrans500; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans220; param.Value = entity.Tipolineatrans220 == null ? "" : entity.Tipolineatrans220; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans138; param.Value = entity.Tipolineatrans138 == null ? "" : entity.Tipolineatrans138; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans500km; param.Value = entity.Tipolineatrans500km == null ? "" : entity.Tipolineatrans500km; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans220km; param.Value = entity.Tipolineatrans220km == null ? "" : entity.Tipolineatrans220km; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans138km; param.Value = entity.Tipolineatrans138km == null ? "" : entity.Tipolineatrans138km; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipototallineastransmision; param.Value = entity.Tipototallineastransmision == null ? "" : entity.Tipototallineastransmision; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipomaxdemandacoincidente; param.Value = entity.Tipomaxdemandacoincidente == null ? "" : entity.Tipomaxdemandacoincidente; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipomaxdemandacontratada; param.Value = entity.Tipomaxdemandacontratada == null ? "" : entity.Tipomaxdemandacontratada; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tiponumsuministrador; param.Value = entity.Tiponumsuministrador == null ? "" : entity.Tiponumsuministrador; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipousucreacion; param.Value = entity.Tipousucreacion == null ? "" : entity.Tipousucreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipofeccreacion; param.Value = entity.Tipofeccreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipousumodificacion; param.Value = entity.Tipousumodificacion == null ? "" : entity.Tipousumodificacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipofecmodificacion; param.Value = entity.Tipofeccreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipoemprcodi; param.Value = entity.Tipoemprcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Emprcodi; param.Value = entity.Emprcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname1; param.Value = entity.Tipodocname1 == null ? "" : entity.Tipodocname1; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename1; param.Value = entity.Tipodocadjfilename1 == null ? "" : entity.Tipodocadjfilename1; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname2; param.Value = entity.Tipodocname2 == null ? "" : entity.Tipodocname2; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename2; param.Value = entity.Tipodocadjfilename2 == null ? "" : entity.Tipodocadjfilename2; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname3; param.Value = entity.Tipodocname3 == null ? "" : entity.Tipodocname3; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename3; param.Value = entity.Tipodocadjfilename3 == null ? "" : entity.Tipodocadjfilename3; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname4; param.Value = entity.Tipodocname4 == null ? "" : entity.Tipodocname4; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename4; param.Value = entity.Tipodocadjfilename4 == null ? "" : entity.Tipodocadjfilename4; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname5; param.Value = entity.Tipodocname5 == null ? "" : entity.Tipodocname5; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename5; param.Value = entity.Tipodocadjfilename5 == null ? "" : entity.Tipodocadjfilename5; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.TipoBaja; param.Value = entity.Tipobaja == null ? "" : entity.Tipobaja; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.TipoInicial; param.Value = entity.Tipoinicial == null ? "" : entity.Tipoinicial; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipocomentario; param.Value = entity.Tipocomentario == null ? "" : entity.Tipocomentario; command2.Parameters.Add(param);

            command2.ExecuteNonQuery();

            return id;
        }
        public void Update(SiTipoComportamientoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tipoprincipal, DbType.String, entity.Tipoprincipal);
            dbProvider.AddInParameter(command, helper.Tipotipagente, DbType.String, entity.Tipotipagente);
            dbProvider.AddInParameter(command, helper.Tipodocsustentatorio, DbType.String, entity.Tipodocsustentatorio);
            dbProvider.AddInParameter(command, helper.Tipoarcdigitalizado, DbType.String, entity.Tipoarcdigitalizado);            
            dbProvider.AddInParameter(command, helper.Tipoarcdigitalizadofilename, DbType.String, entity.Tipoarcdigitalizadofilename);
            dbProvider.AddInParameter(command, helper.Tipopotenciainstalada, DbType.String, entity.Tipopotenciainstalada);
            dbProvider.AddInParameter(command, helper.Tiponrocentrales, DbType.String, entity.Tiponrocentrales);
            dbProvider.AddInParameter(command, helper.Tipolineatrans500, DbType.String, entity.Tipolineatrans500);
            dbProvider.AddInParameter(command, helper.Tipolineatrans220, DbType.String, entity.Tipolineatrans220);
            dbProvider.AddInParameter(command, helper.Tipolineatrans138, DbType.String, entity.Tipolineatrans138);
            dbProvider.AddInParameter(command, helper.Tipolineatrans500km, DbType.String, entity.Tipolineatrans500km);
            dbProvider.AddInParameter(command, helper.Tipolineatrans220km, DbType.String, entity.Tipolineatrans220km);
            dbProvider.AddInParameter(command, helper.Tipolineatrans138km, DbType.String, entity.Tipolineatrans138km);
            dbProvider.AddInParameter(command, helper.Tipototallineastransmision, DbType.String, entity.Tipototallineastransmision);
            dbProvider.AddInParameter(command, helper.Tipomaxdemandacoincidente, DbType.String, entity.Tipomaxdemandacoincidente);
            dbProvider.AddInParameter(command, helper.Tipomaxdemandacontratada, DbType.String, entity.Tipomaxdemandacontratada);
            dbProvider.AddInParameter(command, helper.Tiponumsuministrador, DbType.String, entity.Tiponumsuministrador);
            dbProvider.AddInParameter(command, helper.Tipousucreacion, DbType.String, entity.Tipousucreacion);
            dbProvider.AddInParameter(command, helper.Tipofeccreacion, DbType.DateTime, entity.Tipofeccreacion);
            dbProvider.AddInParameter(command, helper.Tipousumodificacion, DbType.String, entity.Tipousumodificacion);
            dbProvider.AddInParameter(command, helper.Tipofecmodificacion, DbType.DateTime, entity.Tipofecmodificacion);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tipodocname1, DbType.String, entity.Tipodocname1);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename1, DbType.String, entity.Tipodocadjfilename1);
            dbProvider.AddInParameter(command, helper.Tipodocname2, DbType.String, entity.Tipodocname2);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename2, DbType.String, entity.Tipodocadjfilename2);
            dbProvider.AddInParameter(command, helper.Tipodocname3, DbType.String, entity.Tipodocname3);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename3, DbType.String, entity.Tipodocadjfilename3);
            dbProvider.AddInParameter(command, helper.Tipodocname4, DbType.String, entity.Tipodocname4);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename4, DbType.String, entity.Tipodocadjfilename4);
            dbProvider.AddInParameter(command, helper.Tipodocname5, DbType.String, entity.Tipodocname5);
            dbProvider.AddInParameter(command, helper.Tipodocadjfilename5, DbType.String, entity.Tipodocadjfilename5);
            dbProvider.AddInParameter(command, helper.TipoBaja, DbType.String, entity.Tipobaja);
            dbProvider.AddInParameter(command, helper.TipoInicial, DbType.String, entity.Tipoinicial);
            dbProvider.AddInParameter(command, helper.Tipocomentario, DbType.String, entity.Tipocomentario);

            dbProvider.AddInParameter(command, helper.Tipocodi, DbType.Int32, entity.Tipocodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(SiTipoComportamientoDTO entity, IDbConnection conn, DbTransaction tran)
        {

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlUpdate;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = null;

            param = command2.CreateParameter(); param.ParameterName = helper.Tipoprincipal; param.Value = entity.Tipoprincipal; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipotipagente; param.Value = entity.Tipotipagente; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocsustentatorio; param.Value = entity.Tipodocsustentatorio; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipoarcdigitalizado; param.Value = entity.Tipoarcdigitalizado; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipoarcdigitalizadofilename; param.Value = entity.Tipoarcdigitalizadofilename == null ? "" : entity.Tipoarcdigitalizadofilename; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipopotenciainstalada; param.Value = entity.Tipopotenciainstalada == null ? "" : entity.Tipopotenciainstalada; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tiponrocentrales; param.Value = entity.Tiponrocentrales == null ? "" : entity.Tiponrocentrales; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans500; param.Value = entity.Tipolineatrans500 == null ? "" : entity.Tipolineatrans500; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans220; param.Value = entity.Tipolineatrans220 == null ? "" : entity.Tipolineatrans220; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans138; param.Value = entity.Tipolineatrans138 == null ? "" : entity.Tipolineatrans138; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans500km; param.Value = entity.Tipolineatrans500km == null ? "" : entity.Tipolineatrans500km; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans220km; param.Value = entity.Tipolineatrans220km == null ? "" : entity.Tipolineatrans220km; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipolineatrans138km; param.Value = entity.Tipolineatrans138km == null ? "" : entity.Tipolineatrans138km; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipototallineastransmision; param.Value = entity.Tipototallineastransmision == null ? "" : entity.Tipototallineastransmision; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipomaxdemandacoincidente; param.Value = entity.Tipomaxdemandacoincidente == null ? "" : entity.Tipomaxdemandacoincidente; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipomaxdemandacontratada; param.Value = entity.Tipomaxdemandacontratada == null ? "" : entity.Tipomaxdemandacontratada; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tiponumsuministrador; param.Value = entity.Tiponumsuministrador == null ? "" : entity.Tiponumsuministrador; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipousucreacion; param.Value = entity.Tipousucreacion == null ? "" : entity.Tipousucreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipofeccreacion; param.Value = entity.Tipofeccreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipousumodificacion; param.Value = entity.Tipousumodificacion == null ? "" : entity.Tipousumodificacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipofecmodificacion; param.Value = entity.Tipofecmodificacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipoemprcodi; param.Value = entity.Tipoemprcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Emprcodi; param.Value = entity.Emprcodi; command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname1; param.Value = entity.Tipodocname1 == null ? "" : entity.Tipodocname1; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename1; param.Value = entity.Tipodocadjfilename1 == null ? "" : entity.Tipodocadjfilename1; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname2; param.Value = entity.Tipodocname2 == null ? "" : entity.Tipodocname2; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename2; param.Value = entity.Tipodocadjfilename2 == null ? "" : entity.Tipodocadjfilename2; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname3; param.Value = entity.Tipodocname3 == null ? "" : entity.Tipodocname3; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename3; param.Value = entity.Tipodocadjfilename3 == null ? "" : entity.Tipodocadjfilename3; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname4; param.Value = entity.Tipodocname4 == null ? "" : entity.Tipodocname4; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename4; param.Value = entity.Tipodocadjfilename4 == null ? "" : entity.Tipodocadjfilename4; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocname5; param.Value = entity.Tipodocname5 == null ? "" : entity.Tipodocname5; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipodocadjfilename5; param.Value = entity.Tipodocadjfilename5 == null ? "" : entity.Tipodocadjfilename5; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.TipoBaja; param.Value = entity.Tipobaja == null ? "" : entity.Tipobaja; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.TipoInicial; param.Value = entity.Tipoinicial == null ? "" : entity.Tipoinicial; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipocomentario; param.Value = entity.Tipocomentario == null ? "" : entity.Tipocomentario; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipocodi; param.Value = entity.Tipocodi;
            command2.Parameters.Add(param);


            command2.ExecuteNonQuery();
        }

        public void Delete(int tipocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tipocodi, DbType.Int32, tipocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiTipoComportamientoDTO GetById(int tipocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tipocodi, DbType.Int32, tipocodi);
            SiTipoComportamientoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiTipoComportamientoDTO> List()
        {
            List<SiTipoComportamientoDTO> entitys = new List<SiTipoComportamientoDTO>();
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

        public List<SiTipoComportamientoDTO> ListByEmprcodi(int emprcodi)
        {
            List<SiTipoComportamientoDTO> entitys = new List<SiTipoComportamientoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(String.Format(helper.SqlListByEmprcodi, emprcodi));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiTipoComportamientoDTO> GetByCriteria()
        {
            List<SiTipoComportamientoDTO> entitys = new List<SiTipoComportamientoDTO>();
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
    }
}
