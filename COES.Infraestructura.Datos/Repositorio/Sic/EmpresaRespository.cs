using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class EmpresaRepository: RepositoryBase, IEmpresaRepository
    {
        public EmpresaRepository(string strConn): base(strConn)
        {
        }

        EmpresaHelper helper = new EmpresaHelper();

        public int Save(EmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            //dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.Emprnomb);
            //dbProvider.AddInParameter(command, helper.Compcode, DbType.Int32, entity.Compcode);
            //dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            //dbProvider.AddInParameter(command, helper.Emprdire, DbType.String, entity.Emprdire);
            //dbProvider.AddInParameter(command, helper.Emprtele, DbType.String, entity.Emprtele);
            //dbProvider.AddInParameter(command, helper.Emprnumedocu, DbType.String, entity.Emprnumedocu);
            //dbProvider.AddInParameter(command, helper.Tipodocucodi, DbType.String, entity.Tipodocucodi);
            //dbProvider.AddInParameter(command, helper.Emprruc, DbType.String, entity.Emprruc);
            //dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            //dbProvider.AddInParameter(command, helper.Emprorden, DbType.Int32, entity.Emprorden);
            //dbProvider.AddInParameter(command, helper.Emprdom, DbType.String, entity.Emprdom);
            //dbProvider.AddInParameter(command, helper.Emprsein, DbType.String, entity.Emprsein);
            //dbProvider.AddInParameter(command, helper.Emprrazsocial, DbType.String, entity.Emprrazsocial);
            //dbProvider.AddInParameter(command, helper.Emprcoes, DbType.String, entity.Emprcoes);
            //dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            //dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            //dbProvider.AddInParameter(command, helper.Inddemanda, DbType.String, entity.Inddemanda);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            //dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.Emprnomb);
            //dbProvider.AddInParameter(command, helper.Compcode, DbType.Int32, entity.Compcode);
            //dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            //dbProvider.AddInParameter(command, helper.Emprdire, DbType.String, entity.Emprdire);
            //dbProvider.AddInParameter(command, helper.Emprtele, DbType.String, entity.Emprtele);
            //dbProvider.AddInParameter(command, helper.Emprnumedocu, DbType.String, entity.Emprnumedocu);
            //dbProvider.AddInParameter(command, helper.Tipodocucodi, DbType.String, entity.Tipodocucodi);
            //dbProvider.AddInParameter(command, helper.Emprruc, DbType.String, entity.Emprruc);
            //dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            //dbProvider.AddInParameter(command, helper.Emprorden, DbType.Int32, entity.Emprorden);
            //dbProvider.AddInParameter(command, helper.Emprdom, DbType.String, entity.Emprdom);
            //dbProvider.AddInParameter(command, helper.Emprsein, DbType.String, entity.Emprsein);
            //dbProvider.AddInParameter(command, helper.Emprrazsocial, DbType.String, entity.Emprrazsocial);
            //dbProvider.AddInParameter(command, helper.Emprcoes, DbType.String, entity.Emprcoes);
            //dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            //dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            //dbProvider.AddInParameter(command, helper.Inddemanda, DbType.String, entity.Inddemanda);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public EmpresaDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, id);
            EmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EmpresaDTO> List()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
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

        public List<EmpresaDTO> GetByCriteria(string nombre)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> GetPorTipo(string nombre)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombre);

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
