using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTP_INGRESO_POTEFR
    /// </summary>
    public class VtpIngresoPotefrRepository: RepositoryBase, IVtpIngresoPotefrRepository
    {
        public VtpIngresoPotefrRepository(string strConn): base(strConn)
        {
        }

        VtpIngresoPotefrHelper helper = new VtpIngresoPotefrHelper();

        public int Save(VtpIngresoPotefrDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Ipefrintervalo, DbType.Int32, entity.Ipefrintervalo);
            dbProvider.AddInParameter(command, helper.Ipefrdia, DbType.Int32, entity.Ipefrdia);
            dbProvider.AddInParameter(command, helper.Ipefrdescripcion, DbType.String, entity.Ipefrdescripcion);
            dbProvider.AddInParameter(command, helper.Ipefrusucreacion, DbType.String, entity.Ipefrusucreacion);
            dbProvider.AddInParameter(command, helper.Ipefrfeccreacion, DbType.DateTime, entity.Ipefrfeccreacion);
            dbProvider.AddInParameter(command, helper.Ipefrusumodificacion, DbType.String, entity.Ipefrusumodificacion);
            dbProvider.AddInParameter(command, helper.Ipefrfecmodificacion, DbType.DateTime, entity.Ipefrfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpIngresoPotefrDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Ipefrintervalo, DbType.Int32, entity.Ipefrintervalo);
            dbProvider.AddInParameter(command, helper.Ipefrdia, DbType.Int32, entity.Ipefrdia);
            dbProvider.AddInParameter(command, helper.Ipefrdescripcion, DbType.String, entity.Ipefrdescripcion);
            dbProvider.AddInParameter(command, helper.Ipefrusucreacion, DbType.String, entity.Ipefrusucreacion);
            dbProvider.AddInParameter(command, helper.Ipefrfeccreacion, DbType.DateTime, entity.Ipefrfeccreacion);
            dbProvider.AddInParameter(command, helper.Ipefrusumodificacion, DbType.String, entity.Ipefrusumodificacion);
            dbProvider.AddInParameter(command, helper.Ipefrfecmodificacion, DbType.DateTime, entity.Ipefrfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, entity.Ipefrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ipefrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, ipefrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpIngresoPotefrDTO GetById(int ipefrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, ipefrcodi);
            VtpIngresoPotefrDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpIngresoPotefrDTO> List()
        {
            List<VtpIngresoPotefrDTO> entitys = new List<VtpIngresoPotefrDTO>();
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

        public List<VtpIngresoPotefrDTO> GetByCriteria(int pericodi, int recpotcodi)
        {
            List<VtpIngresoPotefrDTO> entitys = new List<VtpIngresoPotefrDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }


        public string GetResultSave(int? ipefrdia, int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetResultSave);
            string cad;

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Ipefrdia, DbType.Int32, ipefrdia);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Ipefrdia, DbType.Int32, ipefrdia);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null) 
               cad= Convert.ToString(result);
            else{
               cad = null;
            }

            return cad;
        }

        public string GetResultUpdate(int ipefrcodi,int? ipefrdia, int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetResultUpdate);
            string cad;

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, ipefrcodi);
            dbProvider.AddInParameter(command, helper.Ipefrdia, DbType.Int32, ipefrdia);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
        

            object result = dbProvider.ExecuteScalar(command);

            if (result != null) 
               cad= Convert.ToString(result);
            else{
               cad = null;
            }

            return cad;
        }

        
    }
}
