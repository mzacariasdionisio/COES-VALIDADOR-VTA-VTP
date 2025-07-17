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
    /// Clase de acceso a datos de la tabla FT_EXT_RELPLTCORRETAPA
    /// </summary>
    public class FtExtRelpltcorretapaRepository : RepositoryBase, IFtExtRelpltcorretapaRepository
    {
        public FtExtRelpltcorretapaRepository(string strConn) : base(strConn)
        {
        }

        FtExtRelpltcorretapaHelper helper = new FtExtRelpltcorretapaHelper();

        public int Save(FtExtRelpltcorretapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fcoretcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Plantcodi, DbType.Int32, entity.Plantcodi);
            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            dbProvider.AddInParameter(command, helper.Tpcorrcodi, DbType.Int32, entity.Tpcorrcodi);
            dbProvider.AddInParameter(command, helper.Ftrpcetipoespecial, DbType.Int32, entity.Ftrpcetipoespecial); 
            dbProvider.AddInParameter(command, helper.Ftrpcetipoampliacion, DbType.Int32, entity.Ftrpcetipoampliacion);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtRelpltcorretapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Plantcodi, DbType.Int32, entity.Plantcodi);
            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            dbProvider.AddInParameter(command, helper.Tpcorrcodi, DbType.Int32, entity.Tpcorrcodi);
            dbProvider.AddInParameter(command, helper.Ftrpcetipoespecial, DbType.Int32, entity.Ftrpcetipoespecial);
            dbProvider.AddInParameter(command, helper.Ftrpcetipoampliacion, DbType.Int32, entity.Ftrpcetipoampliacion);
            dbProvider.AddInParameter(command, helper.Fcoretcodi, DbType.Int32, entity.Fcoretcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int fcoretcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fcoretcodi, DbType.Int32, fcoretcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtRelpltcorretapaDTO GetById(int fcoretcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fcoretcodi, DbType.Int32, fcoretcodi);
            FtExtRelpltcorretapaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtRelpltcorretapaDTO> List()
        {
            List<FtExtRelpltcorretapaDTO> entitys = new List<FtExtRelpltcorretapaDTO>();
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

        public List<FtExtRelpltcorretapaDTO> GetByCriteria(int tpcorrcodi, int ftetcodi)
        {
            List<FtExtRelpltcorretapaDTO> entitys = new List<FtExtRelpltcorretapaDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, tpcorrcodi, ftetcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFtetnombre = dr.GetOrdinal(helper.Ftetnombre);
                    if (!dr.IsDBNull(iFtetnombre)) entity.Ftetnombre = dr.GetString(iFtetnombre);
                    

                    int iTpcorrdescrip = dr.GetOrdinal(helper.Tpcorrdescrip);
                    if (!dr.IsDBNull(iTpcorrdescrip)) entity.Tpcorrdescrip = dr.GetString(iTpcorrdescrip);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public FtExtRelpltcorretapaDTO ObtenerPorEtapaYCarpeta(int ftetcodi, int estenvcodi, int tpcorrcodi, int? esEspecial, int? esAmpliacion)
        {
            FtExtRelpltcorretapaDTO entity = null;

            string sql = "";
            
            //Si es diferente de Solicitud o cancelado
            if (estenvcodi != 1 && estenvcodi != 8)
            {
                //es ESPECIAL: osea es dar de baja o aprobacion (en conexion)
                if (esEspecial != null)
                {                   
                    if (esAmpliacion != null) //si es ampliacion
                    {
                        if(esEspecial == 1 && esAmpliacion == 1)
                        {
                            sql = string.Format(helper.SqlGetRelacionEspecialYAmpliacion, ftetcodi, estenvcodi, tpcorrcodi, esEspecial, esAmpliacion);
                        }
                    }
                    else
                    {
                        if (esEspecial == 1)
                            sql = string.Format(helper.SqlGetRelacionEspecial, ftetcodi, estenvcodi, tpcorrcodi, esEspecial);
                    }
                }
                else
                {
                    if (esAmpliacion != null) 
                    {
                        if (esAmpliacion == 1)
                            sql = string.Format(helper.SqlGetRelacionAmpliacion, ftetcodi, estenvcodi, tpcorrcodi, esAmpliacion);
                    }
                    else
                    {
                        sql = string.Format(helper.SqlGetRelacionSimple, ftetcodi, estenvcodi, tpcorrcodi);
                    }
                }
            }
            else // si es solicitud y cancelado y no importa si sea especial o ampliacion
            {
                if (esEspecial != null)
                {
                    if (esEspecial == 1)
                        sql = string.Format(helper.SqlGetRelacionEspecial, ftetcodi, estenvcodi, tpcorrcodi, esEspecial);
                }
                else
                {
                    sql = string.Format(helper.SqlGetRelacionSimple, ftetcodi, estenvcodi, tpcorrcodi);
                }
                    
            }
           

            if (sql != "") {
                DbCommand command = dbProvider.GetSqlStringCommand(sql);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        entity = helper.Create(dr);
                    }
                }
            }

            return entity;
        }
    }
}
