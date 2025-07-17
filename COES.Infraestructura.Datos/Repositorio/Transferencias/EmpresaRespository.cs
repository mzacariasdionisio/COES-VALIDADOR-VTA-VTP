using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class EmpresaRepository : RepositoryBase, IEmpresaRepository
    {
        public EmpresaRepository(string strConn) : base(strConn)
        {
        }

        EmpresaHelper helper = new EmpresaHelper();

        public EmpresaDTO SaveUpdateAbreaviatura(EmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarAbreviatura);

            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.EmprAbrevCodi, DbType.String, entity.EmprAbrevCodi); 
            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }

        public EmpresaDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, id);
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

        public EmpresaDTO GetByNombre(string sEmprNomb)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByNombre);

            dbProvider.AddInParameter(command, helper.EmprNombre, DbType.String, sEmprNomb);
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

        public EmpresaDTO GetByNombreEstado(string sEmprNomb, int iPeriCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByNombreEstado);

            dbProvider.AddInParameter(command, helper.EmprNombre, DbType.String, sEmprNomb);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
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

        public List<EmpresaDTO> ListGeneradoras()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListGeneradoras);

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
            dbProvider.AddInParameter(command, helper.EmprNombre, DbType.String, nombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListEmpresasSTR()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresasSTR);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListaInterCodEnt()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCodEnt);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListaInterCoReSoGen()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCoReSoGen);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListaInterCoReSoCli()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCoReSoCli);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        public List<EmpresaDTO> ListaInterCoReSoCliPorEmpresa(int emprcodi)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCoReSoCliPorEmpresa);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListaRetiroCliente(int emprcodi)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaRetiroCliente);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EmpresaDTO entity = new EmpresaDTO();
                    entity.EmprCodi = Convert.ToInt32(dr["EMPRCODI"].ToString());
                    entity.EmprNombre = dr["EMPRNOMB"].ToString();
                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public List<EmpresaDTO> ListaInterCoReSC()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCoReSC);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListaInterValorTrans()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterValorTrans);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListaInterCodInfoBase()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCodInfoBase);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListInterCodEntregaRetiro()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListInterCodEntregaRetiro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListInterCodEntregaRetiroxPeriodo(int pericodi, int version)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListInterCodEntregaRetiroxPeriodo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Verscodi, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Verscodi, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListEmpresasCombo()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresasCombo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListarEmpresasComboActivos()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresasComboActivos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListEmpresasConfPtoMME()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresasConfPtoMME);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public EmpresaDTO GetByNombreSic(string sEmprNomb)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByNombreSic);

            dbProvider.AddInParameter(command, helper.Emprrazsocial, DbType.String, sEmprNomb);
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
    }
}
