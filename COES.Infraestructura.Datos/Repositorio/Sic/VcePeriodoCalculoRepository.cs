using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// Clase que contiene las operaciones con la base de datos
    public class VcePeriodoCalculoRepository : RepositoryBase, IVcePeriodoCalculoRepository

    {

        public VcePeriodoCalculoRepository(string strConn)
            : base(strConn)
        {
        }

        VcePeriodoCalculoHelper helper = new VcePeriodoCalculoHelper();

        public int Save(VcePeriodoCalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            int iPecaCodi = GetCodigoGenerado();
            entity.PecaCodi = iPecaCodi;

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, iPecaCodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.Pecaversioncomp, DbType.Int32, entity.PecaVersionComp);
            dbProvider.AddInParameter(command, helper.Pecanombre, DbType.String, entity.PecaNombre);
            dbProvider.AddInParameter(command, helper.Pecaversionvtea, DbType.Int32, entity.PecaVersionVtea);
            dbProvider.AddInParameter(command, helper.Pecatipocambio, DbType.Decimal, entity.PecaTipoCambio);
            dbProvider.AddInParameter(command, helper.Pecaestregistro, DbType.Int32, entity.PecaEstRegistro);
            dbProvider.AddInParameter(command, helper.Pecausucreacion, DbType.String, entity.PecaUsuCreacion);
            dbProvider.AddInParameter(command, helper.Pecafeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Pecamotivo, DbType.String, entity.PecaMotivo);
            dbProvider.ExecuteNonQuery(command);
            return iPecaCodi;
        }

        public void Update(VcePeriodoCalculoDTO entity)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pecaversioncomp, DbType.Int32, entity.PecaVersionComp);
            dbProvider.AddInParameter(command, helper.Pecanombre, DbType.String, entity.PecaNombre);
            dbProvider.AddInParameter(command, helper.Pecaversionvtea, DbType.Int32, entity.PecaVersionVtea);
            dbProvider.AddInParameter(command, helper.Pecatipocambio, DbType.Decimal, entity.PecaTipoCambio);
            dbProvider.AddInParameter(command, helper.Pecaestregistro, DbType.Int32, entity.PecaEstRegistro);
            dbProvider.AddInParameter(command, helper.Pecausumodificacion, DbType.String, entity.PecaUsuModificacion);
            dbProvider.AddInParameter(command, helper.Pecafecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Pecamotivo, DbType.String, entity.PecaMotivo);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcePeriodoCalculoDTO GetById(System.Int32? id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, id);

            VcePeriodoCalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcePeriodoCalculoDTO> List()
        {
            List<VcePeriodoCalculoDTO> entitys = new List<VcePeriodoCalculoDTO>();
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

        public List<VcePeriodoCalculoDTO> GetByCriteria(string nombre)
        {
            List<VcePeriodoCalculoDTO> entitys = new List<VcePeriodoCalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Perinombre, DbType.String, nombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcePeriodoCalculoDTO> GetByAnioMes(int iPeriAnioMes)
        {
            List<VcePeriodoCalculoDTO> entitys = new List<VcePeriodoCalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByAnioMes);
            dbProvider.AddInParameter(command, helper.Perianiomes, DbType.Int32, iPeriAnioMes);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcePeriodoCalculoDTO> GetByIdPeriodo(int iPeriCodi)
        {
            List<VcePeriodoCalculoDTO> entitys = new List<VcePeriodoCalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdPeriodo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);


                    int iRecanombre = dr.GetOrdinal("RECANOMBRE");
                    if (!dr.IsDBNull(iRecanombre)) entity.RecaNombre = dr.GetString(iRecanombre);

                    int iPeriEstado = dr.GetOrdinal("PERIESTADO");
                    if (!dr.IsDBNull(iPeriEstado)) entity.PeriEstado = dr.GetString(iPeriEstado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public Int32 ObtenerNroCalculosActivosPeriodo(int pericodi)
        {
            int cont = 0;
            string queryString = string.Format(helper.SqlNroCalculosActivosPeriodo, pericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            String result = dbProvider.ExecuteScalar(command).ToString();
            cont = Int32.Parse(result);

            return cont;
        }
       
        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }
        public Int32 GetIdAnteriorCalculo(int pecacodi)
        {
            int cont = 0;
            string queryString = string.Format(helper.SqlGetIdAnteriorCalculo, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            cont = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());
            
            return cont;
        }
        public Int32 GetIdAnteriorConfig(int pecacodi)
        {
            int cont = 0;
            string queryString = string.Format(helper.SqlGetIdAnteriorConfig, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            cont = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return cont;
        }

        //Cambios Req

        public VcePeriodoCalculoDTO GetPeriodo(int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPeriodo);

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            VcePeriodoCalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcePeriodoCalculoDTO();

                    int iPericodi = dr.GetOrdinal(helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.PeriCodi = dr.GetInt32(iPericodi);

                    int iPeriAnioMes = dr.GetOrdinal(helper.Perianiomes);
                    if (!dr.IsDBNull(iPeriAnioMes)) entity.PeriAnioMes = dr.GetInt32(iPeriAnioMes);
                }
            }

            return entity;
        }

        public Int32 GetPeriodoMaximo(int pericodi)
        {
            int cont = 0;
            string queryString = string.Format(helper.SqlGetPeriodoMaximo, pericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            String result = dbProvider.ExecuteScalar(command).ToString();
            cont = Int32.Parse(result);

            return cont;
        }

        public int UpdateCompensacionInforme(int pericodi, string nombreinforme)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCompensacionInforme);
                        
            dbProvider.AddInParameter(command, helper.Periinforme, DbType.String, nombreinforme);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            
            return dbProvider.ExecuteNonQuery(command);
        }
    }
}
