using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Framework.Base.Tools;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SH_CAUDAL
    /// </summary>
    public class SHCaudalRepository : RepositoryBase, ISHCaudalRepository
    {
        private string strConexion;
        SHCaudalHelper helper = new SHCaudalHelper();

        public SHCaudalRepository(string strConn)
            : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }      

        public int Save(SHCaudalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.IdCaudal, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.EmpresaCodi, DbType.Int32, entity.EmpresaCodi);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, entity.TipoSerieCodi);
            dbProvider.AddInParameter(command, helper.TPtoMediCodi, DbType.Int32, entity.TPtoMediCodi);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, entity.PtoMediCodi);
            dbProvider.AddInParameter(command, helper.UsuarioRegistro, DbType.String, entity.UsuarioRegistro);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public int Update(SHCaudalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.UsuarioRegistro, DbType.String, entity.UsuarioRegistro);
            dbProvider.AddInParameter(command, helper.EmpresaCodi, DbType.Int32, entity.EmpresaCodi);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, entity.TipoSerieCodi);
            dbProvider.AddInParameter(command, helper.TPtoMediCodi, DbType.Int32, entity.TPtoMediCodi);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, entity.PtoMediCodi);
            

            dbProvider.ExecuteNonQuery(command);

            return 1;
        }

        public int SaveDetalle(SHCaudalDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdDetalle);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveDetalle);
            dbProvider.AddInParameter(command, helper.IdDet, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.IdCaudal, DbType.Int32, entity.IdCaudal);
            dbProvider.AddInParameter(command, helper.Anio, DbType.Int32, entity.Anio);
            dbProvider.AddInParameter(command, helper.M1, DbType.Decimal, entity.M1);
            dbProvider.AddInParameter(command, helper.M2, DbType.Decimal, entity.M2);
            dbProvider.AddInParameter(command, helper.M3, DbType.Decimal, entity.M3);
            dbProvider.AddInParameter(command, helper.M4, DbType.Decimal, entity.M4);
            dbProvider.AddInParameter(command, helper.M5, DbType.Decimal, entity.M5);
            dbProvider.AddInParameter(command, helper.M6, DbType.Decimal, entity.M6);
            dbProvider.AddInParameter(command, helper.M7, DbType.Decimal, entity.M7);
            dbProvider.AddInParameter(command, helper.M8, DbType.Decimal, entity.M8);
            dbProvider.AddInParameter(command, helper.M9, DbType.Decimal, entity.M9);
            dbProvider.AddInParameter(command, helper.M10, DbType.Decimal, entity.M10);
            dbProvider.AddInParameter(command, helper.M11, DbType.Decimal, entity.M11);
            dbProvider.AddInParameter(command, helper.M12, DbType.Decimal, entity.M12);
            dbProvider.AddInParameter(command, helper.INDM1, DbType.String, entity.IndM1);
            dbProvider.AddInParameter(command, helper.INDM2, DbType.String, entity.IndM2);
            dbProvider.AddInParameter(command, helper.INDM3, DbType.String, entity.IndM3);
            dbProvider.AddInParameter(command, helper.INDM4, DbType.String, entity.IndM4);
            dbProvider.AddInParameter(command, helper.INDM5, DbType.String, entity.IndM5);
            dbProvider.AddInParameter(command, helper.INDM6, DbType.String, entity.IndM6);
            dbProvider.AddInParameter(command, helper.INDM7, DbType.String, entity.IndM7);
            dbProvider.AddInParameter(command, helper.INDM8, DbType.String, entity.IndM8);
            dbProvider.AddInParameter(command, helper.INDM9, DbType.String, entity.IndM9);
            dbProvider.AddInParameter(command, helper.INDM10, DbType.String, entity.IndM10);
            dbProvider.AddInParameter(command, helper.INDM11, DbType.String, entity.IndM11);
            dbProvider.AddInParameter(command, helper.INDM12, DbType.String, entity.IndM12);


            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public List<SHCaudalDTO> List(SHCaudalDTO entity)
        {
            List<SHCaudalDTO> entitys = new List<SHCaudalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.EmpresaCodi, DbType.Int32, entity.EmpresaCodi);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, entity.TipoSerieCodi);
            dbProvider.AddInParameter(command, helper.TPtoMediCodi, DbType.Int32, entity.TPtoMediCodi);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, entity.PtoMediCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public SHCaudalDTO GetCaudalActivo(SHCaudalDTO entity)
        {
            SHCaudalDTO ent = new SHCaudalDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetCaudalActivo);
            dbProvider.AddInParameter(command, helper.EmpresaCodi, DbType.Int32, entity.EmpresaCodi);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, entity.TipoSerieCodi);
            dbProvider.AddInParameter(command, helper.TPtoMediCodi, DbType.Int32, entity.TPtoMediCodi);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, entity.PtoMediCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ent = helper.Create(dr);
                }
            }

            return ent;
        }

        public List<SHCaudalDetalleDTO> ListDetalle(SHCaudalDTO entity)
        {
            List<SHCaudalDetalleDTO> entitys = new List<SHCaudalDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetCaudalDetalle);
            dbProvider.AddInParameter(command, helper.IdCaudal, DbType.Int32, entity.IdCaudal);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDetalle(dr));
                }
            }

            return entitys;
        }


    }
}
