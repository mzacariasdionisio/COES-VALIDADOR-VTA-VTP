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
    /// Clase de acceso a datos de la tabla ME_GPS
    /// </summary>
    public class MeGpsRepository: RepositoryBase, IMeGpsRepository
    {
        public MeGpsRepository(string strConn): base(strConn)
        {
        }

        MeGpsHelper helper = new MeGpsHelper();

        public int Save(MeGpsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Nombre, DbType.String, entity.Nombre);
            dbProvider.AddInParameter(command, helper.Gpsoficial, DbType.String, entity.Gpsoficial);
            dbProvider.AddInParameter(command, helper.Gpsosinerg, DbType.String, entity.Gpsosinerg);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeGpsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Nombre, DbType.String, entity.Nombre);
            dbProvider.AddInParameter(command, helper.Gpsoficial, DbType.String, entity.Gpsoficial);
            dbProvider.AddInParameter(command, helper.Gpsosinerg, DbType.String, entity.Gpsosinerg);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, entity.Gpscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int gpscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, gpscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeGpsDTO GetById(int gpscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, gpscodi);
            MeGpsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create2(dr);
                }
            }

            return entity;
        }

        public List<MeGpsDTO> List()
        {
            List<MeGpsDTO> entitys = new List<MeGpsDTO>();
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

        public List<MeGpsDTO> GetByCriteria(int? empresa, string nombre, string oficial)
        {
            List<MeGpsDTO> entitys = new List<MeGpsDTO>();

            if (string.IsNullOrEmpty(oficial)) oficial = "-1";
            if (empresa == null) empresa = 0;
            if (string.IsNullOrEmpty(nombre)) nombre = "-1";

            string sql = String.Format(helper.GetSqlXml("BuscarGps"), oficial, nombre, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            
            //dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, empresa);
            //dbProvider.AddInParameter(command, helper.Nombre, DbType.String, nombre);
            //dbProvider.AddInParameter(command, helper.Gpsoficial, DbType.String, oficial);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeGpsDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal("emprnomb");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeGpsDTO> ObtenerListadoGPS()
        {
            List<MeGpsDTO> entitys = new List<MeGpsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerListadoGPS);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeGpsDTO entity = helper.Create(dr);

                    int iGpsindieod = dr.GetOrdinal(helper.Gpsindieod);
                    if (!dr.IsDBNull(iGpsindieod)) entity.Gpsindieod = dr.GetString(iGpsindieod);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public void ActualizarGPSIEOD(int gpscodi, string estado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarGPSIEOD);

            dbProvider.AddInParameter(command, helper.Gpsindieod, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, gpscodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
