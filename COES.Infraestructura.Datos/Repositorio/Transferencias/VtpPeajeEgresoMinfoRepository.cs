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
    public class VtpPeajeEgresoMinfoRepository : RepositoryBase, IVtpPeajeEgresoMinfoRepository
    {
        public VtpPeajeEgresoMinfoRepository(string strConn) : base(strConn)
        {
        }
        VtpPeajeEgresoMinfoHelper helper = new VtpPeajeEgresoMinfoHelper();


        public int Save(VtpPeajeEgresoMinfoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pegrmicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, entity.Pegrcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, entity.Genemprcodi);
            dbProvider.AddInParameter(command, helper.Pegrdcodi, DbType.Int32, entity.Pegrdcodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, entity.Cliemprcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Pegrmitipousuario, DbType.String, entity.Pegrmitipousuario);
            dbProvider.AddInParameter(command, helper.Pegrmilicitacion, DbType.String, entity.Pegrmilicitacion);
            dbProvider.AddInParameter(command, helper.Pegrmipotecalculada, DbType.Decimal, entity.Pegrmipotecalculada);
            dbProvider.AddInParameter(command, helper.Pegrmipotedeclarada, DbType.Decimal, entity.Pegrmipotedeclarada);
            dbProvider.AddInParameter(command, helper.Pegrmicalidad, DbType.String, entity.Pegrmicalidad);
            dbProvider.AddInParameter(command, helper.Pegrmipreciopote, DbType.Decimal, entity.Pegrmipreciopote);
            dbProvider.AddInParameter(command, helper.Pegrmipoteegreso, DbType.Decimal, entity.Pegrmipoteegreso);
            dbProvider.AddInParameter(command, helper.Pegrmipeajeunitario, DbType.Decimal, entity.Pegrmipeajeunitario);
            dbProvider.AddInParameter(command, helper.Barrcodifco, DbType.Int32, entity.Barrcodifco);
            dbProvider.AddInParameter(command, helper.Pegrmipoteactiva, DbType.Decimal, entity.Pegrmipoteactiva);
            dbProvider.AddInParameter(command, helper.Pegrmipotereactiva, DbType.Decimal, entity.Pegrmipotereactiva);
            dbProvider.AddInParameter(command, helper.Pegrmiusucreacion, DbType.String, entity.Pegrmiusucreacion);
            dbProvider.AddInParameter(command, helper.Pegrmifeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Pegrmipotecoincidente, DbType.Decimal, entity.Pegrmipotecoincidente);
            dbProvider.AddInParameter(command, helper.Pegrmifacperdida, DbType.Decimal, entity.Pegrmifacperdida);
            dbProvider.AddInParameter(command, helper.Coregecodvtp, DbType.String, entity.Coregecodvtp);
            dbProvider.AddInParameter(command, helper.Tipconcodi, DbType.Int32, entity.Tipconcodi);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPeajeEgresoMinfoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, entity.Pegrcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, entity.Genemprcodi);
            dbProvider.AddInParameter(command, helper.Pegrdcodi, DbType.Int32, entity.Pegrdcodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, entity.Cliemprcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Pegrmitipousuario, DbType.String, entity.Pegrmitipousuario);
            dbProvider.AddInParameter(command, helper.Pegrmilicitacion, DbType.String, entity.Pegrmilicitacion);
            dbProvider.AddInParameter(command, helper.Pegrmipotecalculada, DbType.Decimal, entity.Pegrmipotecalculada);
            dbProvider.AddInParameter(command, helper.Pegrmipotedeclarada, DbType.Decimal, entity.Pegrmipotedeclarada);
            dbProvider.AddInParameter(command, helper.Pegrmicalidad, DbType.String, entity.Pegrmicalidad);
            dbProvider.AddInParameter(command, helper.Pegrmipreciopote, DbType.Decimal, entity.Pegrmipreciopote);
            dbProvider.AddInParameter(command, helper.Pegrmipoteegreso, DbType.Decimal, entity.Pegrmipoteegreso);
            dbProvider.AddInParameter(command, helper.Pegrmipeajeunitario, DbType.Decimal, entity.Pegrmipeajeunitario);
            dbProvider.AddInParameter(command, helper.Barrcodifco, DbType.Int32, entity.Barrcodifco);
            dbProvider.AddInParameter(command, helper.Pegrmipoteactiva, DbType.Decimal, entity.Pegrmipoteactiva);
            dbProvider.AddInParameter(command, helper.Pegrmipotereactiva, DbType.Decimal, entity.Pegrmipotereactiva);
            dbProvider.AddInParameter(command, helper.Pegrmiusucreacion, DbType.String, entity.Pegrmiusucreacion);
            dbProvider.AddInParameter(command, helper.Pegrmifeccreacion, DbType.DateTime, entity.Pegrmifeccreacion);

            dbProvider.AddInParameter(command, helper.Pegrmicodi, DbType.Int32, entity.Pegrmicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pegrmicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Pegrmicodi, DbType.Int32, pegrmicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeEgresoMinfoDTO GetById(int pegrmicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pegrmicodi, DbType.Int32, pegrmicodi);
            VtpPeajeEgresoMinfoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }

        public List<VtpPeajeEgresoMinfoDTO> List(int pericodi, int recpotcodi)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
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

        public List<VtpPeajeEgresoMinfoDTO> ListCabecera(int pericodi, int recpotcodi, int recacodi)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCabecera);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recacodi);

            //ASSETEC 20190627: A la consulta le agregamos los saldos de periodos pasados.
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = new VtpPeajeEgresoMinfoDTO();
                     
                    int iGenemprcodi = dr.GetOrdinal(helper.Genemprcodi);
                    if (!dr.IsDBNull(iGenemprcodi)) entity.Genemprcodi = Convert.ToInt32(dr.GetValue(iGenemprcodi));

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtpPeajeEgresoMinfoDTO> ListEmpresa(int pericodi, int recpotcodi, int emprcodi)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresa);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = helper.Create(dr);

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    int iCliemprnombre = dr.GetOrdinal(helper.Cliemprnombre);
                    if (!dr.IsDBNull(iCliemprnombre)) entity.Cliemprnombre = Convert.ToString(dr.GetValue(iCliemprnombre));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    int iPegrdpotecoincidente = dr.GetOrdinal(helper.Pegrdpotecoincidente);
                    if (!dr.IsDBNull(iPegrdpotecoincidente)) entity.Pegrdpotecoincidente = Convert.ToDecimal(dr.GetValue(iPegrdpotecoincidente));

                    int iPegrdfacperdida = dr.GetOrdinal(helper.Pegrdfacperdida);
                    if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrdfacperdida = Convert.ToDecimal(dr.GetValue(iPegrdfacperdida));

                    if (dr[helper.Coregecodvtp] != null)
                    {
                        int iCodcncodivtp = dr.GetOrdinal(helper.Coregecodvtp);
                        if (!dr.IsDBNull(iCodcncodivtp)) entity.Coregecodvtp = Convert.ToString(dr.GetValue(iCodcncodivtp));
                    }
                    if (dr[helper.Tipconcodi] != null)
                    {
                        int iTipconcodi = dr.GetOrdinal(helper.Tipconcodi);
                        if (!dr.IsDBNull(iTipconcodi)) entity.Tipconcodi = Convert.ToInt32(dr.GetValue(iTipconcodi));
                    }
                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<VtpPeajeEgresoMinfoDTO> ListEmpresaRecalculo(int pericodi, int recpotcodi, int emprcodi)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaRecalculo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = helper.Create(dr);

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    int iCliemprnombre = dr.GetOrdinal(helper.Cliemprnombre);
                    if (!dr.IsDBNull(iCliemprnombre)) entity.Cliemprnombre = Convert.ToString(dr.GetValue(iCliemprnombre));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeEgresoMinfoDTO> ListPotenciaValor(int pericodi, int recpotcodi)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPotenciaValor);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);            
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = new VtpPeajeEgresoMinfoDTO();

                    int iGenemprcodi = dr.GetOrdinal(helper.Genemprcodi);
                    if (!dr.IsDBNull(iGenemprcodi)) entity.Genemprcodi = Convert.ToInt32(dr.GetValue(iGenemprcodi));

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    int iPegrmipotecalculada = dr.GetOrdinal(this.helper.Pegrmipotecalculada);
                    if (!dr.IsDBNull(iPegrmipotecalculada)) entity.Pegrmipotecalculada = dr.GetDecimal(iPegrmipotecalculada);

                    int iPegrmipotedeclarada = dr.GetOrdinal(this.helper.Pegrmipotedeclarada);
                    if (!dr.IsDBNull(iPegrmipotedeclarada)) entity.Pegrmipotedeclarada = dr.GetDecimal(iPegrmipotedeclarada);

                    int iPegrmipreciopote = dr.GetOrdinal(this.helper.Pegrmipreciopote);
                    if (!dr.IsDBNull(iPegrmipreciopote)) entity.Pegrmipreciopote = dr.GetDecimal(iPegrmipreciopote);

                    int iPegrmipoteegreso = dr.GetOrdinal(this.helper.Pegrmipoteegreso);
                    if (!dr.IsDBNull(iPegrmipoteegreso)) entity.Pegrmipoteegreso = dr.GetDecimal(iPegrmipoteegreso);

                    int iPegrmimpeajeunitario = dr.GetOrdinal(this.helper.Pegrmipeajeunitario);
                    if (!dr.IsDBNull(iPegrmimpeajeunitario)) entity.Pegrmipeajeunitario = dr.GetDecimal(iPegrmimpeajeunitario);

                    int iPegrmipoteactiva = dr.GetOrdinal(this.helper.Pegrmipoteactiva);
                    if (!dr.IsDBNull(iPegrmipoteactiva)) entity.Pegrmipoteactiva = dr.GetDecimal(iPegrmipoteactiva);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtpPeajeEgresoMinfoDTO> GetByCriteria(int pericodi, int recpotcodi)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = helper.Create(dr);

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    int iCliemprnombre = dr.GetOrdinal(helper.Cliemprnombre);
                    if (!dr.IsDBNull(iCliemprnombre)) entity.Cliemprnombre = Convert.ToString(dr.GetValue(iCliemprnombre));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    //- Linea agregada egjunin
                    int iGenemprcodi = dr.GetOrdinal(helper.Genemprcodi);
                    if (!dr.IsDBNull(iGenemprcodi)) entity.Genemprcodi = Convert.ToInt32(dr.GetValue(iGenemprcodi));

                    if (dr[helper.Pegrdpotecoincidente] != null)
                    {
                        int iPegrdpotecoincidente = dr.GetOrdinal(helper.Pegrdpotecoincidente);
                        if (!dr.IsDBNull(iPegrdpotecoincidente)) entity.Pegrdpotecoincidente = dr.GetDecimal(iPegrdpotecoincidente);
                    }

                    if (dr[helper.Pegrdfacperdida] != null)
                    {
                        int iPegrdfacperdida = dr.GetOrdinal(helper.Pegrdfacperdida);
                        if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrdfacperdida = dr.GetDecimal(iPegrdfacperdida);
                    }

                    if (dr[helper.Coregecodvtp] != null)
                    {
                        int iCoregecodvtp = dr.GetOrdinal(helper.Coregecodvtp);
                        if (!dr.IsDBNull(iCoregecodvtp)) entity.Coregecodvtp = dr.GetString(iCoregecodvtp);
                    }

                    if (dr[helper.Tipconcodi] != null)
                    {
                        int iTipconcodi = dr.GetOrdinal(helper.Tipconcodi);
                        if (!dr.IsDBNull(iTipconcodi)) entity.Tipconcodi = Convert.ToInt32(dr.GetValue(iTipconcodi));
                    }

                    if (dr[helper.Tipconnombre] != null)
                    {
                        int iTipconnombre = dr.GetOrdinal(helper.Tipconnombre);
                        if (!dr.IsDBNull(iTipconnombre)) entity.Tipconnombre = dr.GetString(iTipconnombre);
                    }

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtpPeajeEgresoMinfoDTO> GetByCriteriaVista(int pericodi, int recpotcodi, int emprcodi, int cliemprcodi, int barrcodi, int barrcodifco, string pegrmitipousuario, string pegrmilicitacion, string pegrmicalidad, string pegrmicalidad2)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaVista);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, emprcodi);

            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);

            dbProvider.AddInParameter(command, helper.Barrcodifco, DbType.Int32, barrcodifco);
            dbProvider.AddInParameter(command, helper.Barrcodifco, DbType.Int32, barrcodifco);

            dbProvider.AddInParameter(command, helper.Pegrmitipousuario, DbType.String, pegrmitipousuario);
            dbProvider.AddInParameter(command, helper.Pegrmitipousuario, DbType.String, pegrmitipousuario);

            dbProvider.AddInParameter(command, helper.Pegrmilicitacion, DbType.String, pegrmilicitacion);
            dbProvider.AddInParameter(command, helper.Pegrmilicitacion, DbType.String, pegrmilicitacion);

            dbProvider.AddInParameter(command, helper.Pegrmicalidad, DbType.String, pegrmicalidad);

            dbProvider.AddInParameter(command, helper.Pegrmiusucreacion, DbType.String, pegrmicalidad2);
            dbProvider.AddInParameter(command, helper.Pegrmicalidad, DbType.String, pegrmicalidad);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = helper.Create(dr);

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    int iCliemprnombre = dr.GetOrdinal(helper.Cliemprnombre);
                    if (!dr.IsDBNull(iCliemprnombre)) entity.Cliemprnombre = Convert.ToString(dr.GetValue(iCliemprnombre));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    //- Linea agregada egjunin
                    int iGenemprcodi = dr.GetOrdinal(helper.Genemprcodi);
                    if (!dr.IsDBNull(iGenemprcodi)) entity.Genemprcodi = Convert.ToInt32(dr.GetValue(iGenemprcodi));

                    if (dr[helper.Pegrdpotecoincidente] != null)
                    {
                        int iPegrdpotecoincidente = dr.GetOrdinal(helper.Pegrdpotecoincidente);
                        if (!dr.IsDBNull(iPegrdpotecoincidente)) entity.Pegrdpotecoincidente = dr.GetDecimal(iPegrdpotecoincidente);
                    }

                    if (dr[helper.Pegrdfacperdida] != null)
                    {
                        int iPegrdfacperdida = dr.GetOrdinal(helper.Pegrdfacperdida);
                        if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrdfacperdida = dr.GetDecimal(iPegrdfacperdida);
                        if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrmifacperdida = dr.GetDecimal(iPegrdfacperdida);
                    }

                    if (dr[helper.Coregecodvtp] != null)
                    {
                        int iCoregecodvtp = dr.GetOrdinal(helper.Coregecodvtp);
                        if (!dr.IsDBNull(iCoregecodvtp)) entity.Coregecodvtp = dr.GetString(iCoregecodvtp);
                    }

                    if (dr[helper.Tipconcodi] != null)
                    {
                        int iTipconcodi = dr.GetOrdinal(helper.Tipconcodi);
                        if (!dr.IsDBNull(iTipconcodi)) entity.Tipconcodi = Convert.ToInt32(dr.GetValue(iTipconcodi));
                    }

                    if (dr[helper.Tipconnombre] != null)
                    {
                        int iTipconnombre = dr.GetOrdinal(helper.Tipconnombre);
                        if (!dr.IsDBNull(iTipconnombre)) entity.Tipconnombre = dr.GetString(iTipconnombre);
                    }

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtpPeajeEgresoMinfoDTO> GetByCriteriaInfoFaltante(int pericodi, int recpotcodi, int pericodianterior, int recpotcodianterior)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaInfoFaltante);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, pericodianterior);
            dbProvider.AddInParameter(command, helper.Barrcodifco, DbType.Int32, recpotcodianterior);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = new VtpPeajeEgresoMinfoDTO();

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    int iCliemprnombre = dr.GetOrdinal(helper.Cliemprnombre);
                    if (!dr.IsDBNull(iCliemprnombre)) entity.Cliemprnombre = Convert.ToString(dr.GetValue(iCliemprnombre));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iPegrmitipousuario = dr.GetOrdinal(helper.Pegrmitipousuario);
                    if (!dr.IsDBNull(iPegrmitipousuario)) entity.Pegrmitipousuario = Convert.ToString(dr.GetValue(iPegrmitipousuario));

                    int iPegrmilicitacion = dr.GetOrdinal(helper.Pegrmilicitacion);
                    if (!dr.IsDBNull(iPegrmilicitacion)) entity.Pegrmilicitacion = dr.GetString(iPegrmilicitacion);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<VtpPeajeEgresoMinfoDTO> ListCabeceraRecalculo(int pericodi, int recpotcodi)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCabeceraRecalculo);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = new VtpPeajeEgresoMinfoDTO();

                    int iGenemprcodi = dr.GetOrdinal(helper.Genemprcodi);
                    if (!dr.IsDBNull(iGenemprcodi)) entity.Genemprcodi = Convert.ToInt32(dr.GetValue(iGenemprcodi));

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<CodigoConsolidadoDTO> ListarCodigosVTP(int emprCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarCodigosVTP);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, emprCodi);
            List<CodigoConsolidadoDTO> entitys = new List<CodigoConsolidadoDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CodigoConsolidadoDTO entity = new CodigoConsolidadoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iClicodi = dr.GetOrdinal(helper.Clicodi);
                    if (!dr.IsDBNull(iClicodi)) entity.Clicodi = dr.GetInt32(iClicodi);

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = dr.GetInt32(iBarrcodi);

                    int iCodcncodivtp = dr.GetOrdinal(helper.Codcncodivtp);
                    if (!dr.IsDBNull(iCodcncodivtp)) entity.Codcncodivtp = dr.GetString(iCodcncodivtp);

                    int iTipconcodi = dr.GetOrdinal(helper.Tipconcodi);
                    if (!dr.IsDBNull(iTipconcodi)) entity.Tipconcodi = dr.GetInt32(iTipconcodi);

                    int iTipusucodi = dr.GetOrdinal(helper.Tipusucodi);
                    if (!dr.IsDBNull(iTipusucodi)) entity.Tipusucodi = dr.GetInt32(iTipusucodi);

                    int iTipconnombre = dr.GetOrdinal(helper.Tipconnombre);
                    if (!dr.IsDBNull(iTipconnombre)) entity.Tipconnombre = dr.GetString(iTipconnombre);

                    int iTipusunombre = dr.GetOrdinal(helper.Tipusunombre);
                    if (!dr.IsDBNull(iTipusunombre)) entity.Tipusunombre = dr.GetString(iTipusunombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeEgresoMinfoDTO> ListarCodigosByCriteria(int emprcodi, int cliemprcodi, int barrcodi, string pegrmitipousuario,int pericodi, string pgrmilicitacion)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarCodigosByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);

            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, emprcodi);

            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);

            dbProvider.AddInParameter(command, helper.Pegrmitipousuario, DbType.String, pegrmitipousuario);
            dbProvider.AddInParameter(command, helper.Pegrmitipousuario, DbType.String, pegrmitipousuario);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);

            dbProvider.AddInParameter(command, helper.Pegrmilicitacion, DbType.String, pgrmilicitacion);
            dbProvider.AddInParameter(command, helper.Pegrmilicitacion, DbType.String, pgrmilicitacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = helper.Create(dr);

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    int iCliemprnombre = dr.GetOrdinal(helper.Cliemprnombre);
                    if (!dr.IsDBNull(iCliemprnombre)) entity.Cliemprnombre = Convert.ToString(dr.GetValue(iCliemprnombre));

                    //int iClicodi = dr.GetOrdinal(helper.Clicodi);
                    //if (!dr.IsDBNull(iClicodi)) entity.Cliemprcodi = Convert.ToInt32(dr.GetValue(iClicodi));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    //- Linea agregada egjunin
                    int iGenemprcodi = dr.GetOrdinal(helper.Genemprcodi);
                    if (!dr.IsDBNull(iGenemprcodi)) entity.Genemprcodi = Convert.ToInt32(dr.GetValue(iGenemprcodi));

                    if (dr[helper.Pegrdpotecoincidente] != null)
                    {
                        int iPegrdpotecoincidente = dr.GetOrdinal(helper.Pegrdpotecoincidente);
                        if (!dr.IsDBNull(iPegrdpotecoincidente)) entity.Pegrdpotecoincidente = dr.GetDecimal(iPegrdpotecoincidente);
                    }

                    if (dr[helper.Pegrdfacperdida] != null)
                    {
                        int iPegrdfacperdida = dr.GetOrdinal(helper.Pegrdfacperdida);
                        if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrdfacperdida = dr.GetDecimal(iPegrdfacperdida);
                        if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrmifacperdida = dr.GetDecimal(iPegrdfacperdida);
                    }

                    if (dr[helper.Coregecodvtp] != null)
                    {
                        int iCoregecodvtp = dr.GetOrdinal(helper.Coregecodvtp);
                        if (!dr.IsDBNull(iCoregecodvtp)) entity.Coregecodvtp = dr.GetString(iCoregecodvtp);
                    }

                    if (dr[helper.Tipconcodi] != null)
                    {
                        int iTipconcodi = dr.GetOrdinal(helper.Tipconcodi);
                        if (!dr.IsDBNull(iTipconcodi)) entity.Tipconcodi = Convert.ToInt32(dr.GetValue(iTipconcodi));
                    }

                    if (dr[helper.Tipconnombre] != null)
                    {
                        int iTipconnombre = dr.GetOrdinal(helper.Tipconnombre);
                        if (!dr.IsDBNull(iTipconnombre)) entity.Tipconnombre = dr.GetString(iTipconnombre);
                    }

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtpPeajeEgresoMinfoDTO> GetByCriteriaVistaNuevo(int pericodi, int recpotcodi, int emprcodi, int cliemprcodi, int barrcodi, string pegrmitipousuario, string pegrmilicitacion, string pegrmicalidad, string pegrmicalidad2)
        {
            List<VtpPeajeEgresoMinfoDTO> entitys = new List<VtpPeajeEgresoMinfoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaVistaNuevo);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, emprcodi);

            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);

            dbProvider.AddInParameter(command, helper.Pegrmitipousuario, DbType.String, pegrmitipousuario);
            dbProvider.AddInParameter(command, helper.Pegrmitipousuario, DbType.String, pegrmitipousuario);

            dbProvider.AddInParameter(command, helper.Pegrmilicitacion, DbType.String, pegrmilicitacion);
            dbProvider.AddInParameter(command, helper.Pegrmilicitacion, DbType.String, pegrmilicitacion);

            dbProvider.AddInParameter(command, helper.Pegrmicalidad, DbType.String, pegrmicalidad);

            dbProvider.AddInParameter(command, helper.Pegrmiusucreacion, DbType.String, pegrmicalidad2);
            dbProvider.AddInParameter(command, helper.Pegrmicalidad, DbType.String, pegrmicalidad);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoMinfoDTO entity = helper.Create(dr);

                    int iGenemprnombre = dr.GetOrdinal(helper.Genemprnombre);
                    if (!dr.IsDBNull(iGenemprnombre)) entity.Genemprnombre = Convert.ToString(dr.GetValue(iGenemprnombre));

                    int iCliemprnombre = dr.GetOrdinal(helper.Cliemprnombre);
                    if (!dr.IsDBNull(iCliemprnombre)) entity.Cliemprnombre = Convert.ToString(dr.GetValue(iCliemprnombre));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = Convert.ToString(dr.GetValue(iBarrnombre));

                    int iBarrnombrefco = dr.GetOrdinal(helper.Barrnombrefco);
                    if (!dr.IsDBNull(iBarrnombrefco)) entity.Barrnombrefco = Convert.ToString(dr.GetValue(iBarrnombrefco));

                    //- Linea agregada egjunin
                    int iGenemprcodi = dr.GetOrdinal(helper.Genemprcodi);
                    if (!dr.IsDBNull(iGenemprcodi)) entity.Genemprcodi = Convert.ToInt32(dr.GetValue(iGenemprcodi));

                    if (dr[helper.Pegrdpotecoincidente] != null)
                    {
                        int iPegrdpotecoincidente = dr.GetOrdinal(helper.Pegrdpotecoincidente);
                        if (!dr.IsDBNull(iPegrdpotecoincidente)) entity.Pegrdpotecoincidente = dr.GetDecimal(iPegrdpotecoincidente);
                    }

                    if (dr[helper.Pegrdfacperdida] != null)
                    {
                        int iPegrdfacperdida = dr.GetOrdinal(helper.Pegrdfacperdida);
                        if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrdfacperdida = dr.GetDecimal(iPegrdfacperdida);
                        if (!dr.IsDBNull(iPegrdfacperdida)) entity.Pegrmifacperdida = dr.GetDecimal(iPegrdfacperdida);
                    }

                    if (dr[helper.Coregecodvtp] != null)
                    {
                        int iCoregecodvtp = dr.GetOrdinal(helper.Coregecodvtp);
                        if (!dr.IsDBNull(iCoregecodvtp)) entity.Coregecodvtp = dr.GetString(iCoregecodvtp);
                    }

                    if (dr[helper.Tipconcodi] != null)
                    {
                        int iTipconcodi = dr.GetOrdinal(helper.Tipconcodi);
                        if (!dr.IsDBNull(iTipconcodi)) entity.Tipconcodi = Convert.ToInt32(dr.GetValue(iTipconcodi));
                    }

                    if (dr[helper.Tipconnombre] != null)
                    {
                        int iTipconnombre = dr.GetOrdinal(helper.Tipconnombre);
                        if (!dr.IsDBNull(iTipconnombre)) entity.Tipconnombre = dr.GetString(iTipconnombre);
                    }

                    entitys.Add(entity);
                }
            }
            return entitys;
        }


    }
}
