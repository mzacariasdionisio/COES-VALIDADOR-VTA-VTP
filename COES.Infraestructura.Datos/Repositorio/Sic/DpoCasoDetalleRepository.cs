using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoCasoDetalleRepository : RepositoryBase, IDpoCasoDetalleRepository
    {
        public DpoCasoDetalleRepository(string strConn) : base(strConn)
        {
        }

        DpoCasoDetalleHelper helper = new DpoCasoDetalleHelper();

        #region Métodos Tabla DPO_CASO_DETALLE
        public void Save(DpoCasoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, entity.Dpocsocodi);

            dbProvider.AddInParameter(command, helper.Dpodetmafscada, DbType.Int32, entity.Dpodetmafscada);
            dbProvider.AddInParameter(command, helper.Dpodetmatinicio, DbType.DateTime, entity.Dpodetmatinicio);
            dbProvider.AddInParameter(command, helper.Dpodetmatfin, DbType.DateTime, entity.Dpodetmatfin);
            dbProvider.AddInParameter(command, helper.Dpofnccodima, DbType.Int32, entity.Dpofnccodima);
            dbProvider.AddInParameter(command, helper.Dposecuencma, DbType.String, entity.Dposecuencma);

            dbProvider.AddInParameter(command, helper.Dpotipfuncion, DbType.String, entity.Dpotipfuncion);

            dbProvider.AddInParameter(command, helper.Pafunr1dtg1, DbType.String, entity.Pafunr1dtg1);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg2, DbType.String, entity.Pafunr1dtg2);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg3, DbType.String, entity.Pafunr1dtg3);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg4, DbType.String, entity.Pafunr1dtg4);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg5, DbType.String, entity.Pafunr1dtg5);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg6, DbType.String, entity.Pafunr1dtg6);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg7, DbType.String, entity.Pafunr1dtg7);

            dbProvider.AddInParameter(command, helper.Pafunr1deg7, DbType.String, entity.Pafunr1deg7);
            dbProvider.AddInParameter(command, helper.Pafunr1hag7, DbType.String, entity.Pafunr1hag7);

            dbProvider.AddInParameter(command, helper.Pafunr2dtg1, DbType.String, entity.Pafunr2dtg1);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg2, DbType.String, entity.Pafunr2dtg2);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg3, DbType.String, entity.Pafunr2dtg3);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg4, DbType.String, entity.Pafunr2dtg4);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg5, DbType.String, entity.Pafunr2dtg5);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg6, DbType.String, entity.Pafunr2dtg6);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg7, DbType.String, entity.Pafunr2dtg7);

            dbProvider.AddInParameter(command, helper.Pafunf1toram, DbType.String, entity.Pafunf1toram);
            dbProvider.AddInParameter(command, helper.Pafunf2factk, DbType.String, entity.Pafunf2factk);

            dbProvider.AddInParameter(command, helper.Pafuna1aniof, DbType.String, entity.Pafuna1aniof);
            dbProvider.AddInParameter(command, helper.Pafuna1idfer, DbType.String, entity.Pafuna1idfer);

            dbProvider.AddInParameter(command, helper.Pafuna1dtg1, DbType.String, entity.Pafuna1dtg1);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg2, DbType.String, entity.Pafuna1dtg2);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg3, DbType.String, entity.Pafuna1dtg3);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg4, DbType.String, entity.Pafuna1dtg4);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg5, DbType.String, entity.Pafuna1dtg5);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg6, DbType.String, entity.Pafuna1dtg6);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg7, DbType.String, entity.Pafuna1dtg7);

            dbProvider.AddInParameter(command, helper.Pafuna2aniof, DbType.String, entity.Pafuna2aniof);
            dbProvider.AddInParameter(command, helper.Pafuna2idfer, DbType.String, entity.Pafuna2idfer);

            dbProvider.AddInParameter(command, helper.Pafuna2dtg1, DbType.String, entity.Pafuna2dtg1);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg2, DbType.String, entity.Pafuna2dtg2);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg3, DbType.String, entity.Pafuna2dtg3);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg4, DbType.String, entity.Pafuna2dtg4);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg5, DbType.String, entity.Pafuna2dtg5);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg6, DbType.String, entity.Pafuna2dtg6);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg7, DbType.String, entity.Pafuna2dtg7);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoCasoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, entity.Dpocsocodi);

            dbProvider.AddInParameter(command, helper.Dpodetmafscada, DbType.String, entity.Dpodetmafscada);
            dbProvider.AddInParameter(command, helper.Dpodetmatinicio, DbType.DateTime, entity.Dpodetmatinicio);
            dbProvider.AddInParameter(command, helper.Dpodetmatfin, DbType.DateTime, entity.Dpodetmatfin);
            dbProvider.AddInParameter(command, helper.Dpofnccodima, DbType.Int32, entity.Dpofnccodima);
            dbProvider.AddInParameter(command, helper.Dposecuencma, DbType.String, entity.Dposecuencma);

            dbProvider.AddInParameter(command, helper.Pafunr1dtg1, DbType.String, entity.Pafunr1dtg1);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg2, DbType.String, entity.Pafunr1dtg2);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg3, DbType.String, entity.Pafunr1dtg3);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg4, DbType.String, entity.Pafunr1dtg4);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg5, DbType.String, entity.Pafunr1dtg5);
            dbProvider.AddInParameter(command, helper.Pafunr1dtg6, DbType.String, entity.Pafunr1dtg6);

            dbProvider.AddInParameter(command, helper.Pafunr1dtg7, DbType.String, entity.Pafunr1dtg7);
            dbProvider.AddInParameter(command, helper.Pafunr1deg7, DbType.String, entity.Pafunr1deg7);

            dbProvider.AddInParameter(command, helper.Pafunr1hag7, DbType.String, entity.Pafunr1hag7);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg1, DbType.String, entity.Pafunr2dtg1);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg2, DbType.String, entity.Pafunr2dtg2);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg3, DbType.String, entity.Pafunr2dtg3);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg4, DbType.String, entity.Pafunr2dtg4);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg5, DbType.String, entity.Pafunr2dtg5);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg6, DbType.String, entity.Pafunr2dtg6);
            dbProvider.AddInParameter(command, helper.Pafunr2dtg7, DbType.String, entity.Pafunr2dtg7);

            dbProvider.AddInParameter(command, helper.Pafunf1toram, DbType.String, entity.Pafunf1toram);
            dbProvider.AddInParameter(command, helper.Pafunf2factk, DbType.String, entity.Pafunf2factk);

            dbProvider.AddInParameter(command, helper.Pafuna1aniof, DbType.String, entity.Pafuna1aniof);
            dbProvider.AddInParameter(command, helper.Pafuna1idfer, DbType.String, entity.Pafuna1idfer);

            dbProvider.AddInParameter(command, helper.Pafuna1dtg1, DbType.String, entity.Pafuna1dtg1);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg2, DbType.String, entity.Pafuna1dtg2);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg3, DbType.String, entity.Pafuna1dtg3);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg4, DbType.String, entity.Pafuna1dtg4);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg5, DbType.String, entity.Pafuna1dtg5);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg6, DbType.String, entity.Pafuna1dtg6);
            dbProvider.AddInParameter(command, helper.Pafuna1dtg7, DbType.String, entity.Pafuna1dtg7);

            dbProvider.AddInParameter(command, helper.Pafuna2aniof, DbType.String, entity.Pafuna2aniof);
            dbProvider.AddInParameter(command, helper.Pafuna2idfer, DbType.String, entity.Pafuna2idfer);

            dbProvider.AddInParameter(command, helper.Pafuna2dtg1, DbType.String, entity.Pafuna2dtg1);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg2, DbType.String, entity.Pafuna2dtg2);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg3, DbType.String, entity.Pafuna2dtg3);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg4, DbType.String, entity.Pafuna2dtg4);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg5, DbType.String, entity.Pafuna2dtg5);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg6, DbType.String, entity.Pafuna2dtg6);
            dbProvider.AddInParameter(command, helper.Pafuna2dtg7, DbType.String, entity.Pafuna2dtg7);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, entity.Dpocasdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public DpoCasoDetalleDTO GetById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, id);

            DpoCasoDetalleDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateDpoCasoDetalle(dr);
                }
            }

            return entity;
        }

        public List<DpoCasoDetalleDTO> List()
        {
            List<DpoCasoDetalleDTO> entitys = new List<DpoCasoDetalleDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoCasoDetalle(dr));
                }
            }

            return entitys;
        }



        public void DeleteByIdCaso(int idCaso)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByIdCaso);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoCasoDetalleDTO> ListByIdCaso(int idCaso)
        {
            List<DpoCasoDetalleDTO> entitys = new List<DpoCasoDetalleDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByIdCaso);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoCasoDetalle(dr));
                }
            }

            return entitys;
        }

        public List<DpoFuncionDataMaestraDTO> ListFuncionesDataMaestra(int idCaso)
        {
            List<DpoFuncionDataMaestraDTO> entitys = new List<DpoFuncionDataMaestraDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFuncionesDataMaestraByIdCaso);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoFuncionDataMaestra(dr));
                }
            }

            return entitys;
        }

        public List<DpoFuncionDataProcesarDTO> ListFuncionesDataProcesar(int idCaso)
        {
            List<DpoFuncionDataProcesarDTO> entitys = new List<DpoFuncionDataProcesarDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFuncionesDataProcesarByIdCaso);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoFuncionDataProcesar(dr));
                }
            }

            return entitys;
        }

        public List<DpoCasoDetalleDTO> GetParametrosDataMaestra(int idCaso)
        {
            List<DpoCasoDetalleDTO> entitys = new List<DpoCasoDetalleDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetParametrosDataMaestraByIdCaso);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametros(dr));
                }
            }

            return entitys;
        }

        public List<DpoCasoDetalleDTO> GetParametrosDataProcesar(int idCaso)
        {
            List<DpoCasoDetalleDTO> entitys = new List<DpoCasoDetalleDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetParametrosDataProcesarByIdCaso);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametros(dr));
                }
            }

            return entitys;
        }


        public List<DpoParametrosR1DTO> ListParametrosR1(int idCaso, int idCasoDetalle)
        {
            List<DpoParametrosR1DTO> entitys = new List<DpoParametrosR1DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListParametrosR1);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCasoDetalle);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosR1(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosR2DTO> ListParametrosR2(int idCaso, int idCasoDetalle)
        {
            List<DpoParametrosR2DTO> entitys = new List<DpoParametrosR2DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListParametrosR2);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCasoDetalle);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosR2(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosF1DTO> ListParametrosF1(int idCaso, int idCasoDetalle)
        {
            List<DpoParametrosF1DTO> entitys = new List<DpoParametrosF1DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListParametrosF1);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCasoDetalle);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosF1(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosF2DTO> ListParametrosF2(int idCaso, int idCasoDetalle)
        {
            List<DpoParametrosF2DTO> entitys = new List<DpoParametrosF2DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListParametrosF2);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCasoDetalle);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosF2(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosA1DTO> ListParametrosA1(int idCaso, int idCasoDetalle)
        {
            List<DpoParametrosA1DTO> entitys = new List<DpoParametrosA1DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListParametrosA1);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCasoDetalle);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosA1(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosA2DTO> ListParametrosA2(int idCaso, int idCasoDetalle)
        {
            List<DpoParametrosA2DTO> entitys = new List<DpoParametrosA2DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListParametrosA2);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCasoDetalle);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idCaso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosA2(dr));
                }
            }

            return entitys;
        }


        public List<DpoParametrosA1DTO> GetParametrosA1(int idCaso, int idFuncion, string tipFuncion)
        {
            List<DpoParametrosA1DTO> entitys = new List<DpoParametrosA1DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetParametrosA1);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCaso);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idFuncion);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.String, tipFuncion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosA1(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosA2DTO> GetParametrosA2(int idCaso, int idFuncion, string tipFuncion)
        {
            List<DpoParametrosA2DTO> entitys = new List<DpoParametrosA2DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetParametrosA2);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCaso);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idFuncion);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.String, tipFuncion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosA2(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosF1DTO> GetParametrosF1(int idCaso, int idFuncion, string tipFuncion)
        {
            List<DpoParametrosF1DTO> entitys = new List<DpoParametrosF1DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetParametrosF1);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCaso);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idFuncion);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.String, tipFuncion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosF1(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosF2DTO> GetParametrosF2(int idCaso, int idFuncion, string tipFuncion)
        {
            List<DpoParametrosF2DTO> entitys = new List<DpoParametrosF2DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetParametrosF2);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCaso);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idFuncion);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.String, tipFuncion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosF2(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosR1DTO> GetParametrosR1(int idCaso, int idFuncion, string tipFuncion)
        {
            List<DpoParametrosR1DTO> entitys = new List<DpoParametrosR1DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetParametrosR1);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCaso);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idFuncion);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.String, tipFuncion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosR1(dr));
                }
            }

            return entitys;
        }

        public List<DpoParametrosR2DTO> GetParametrosR2(int idCaso, int idFuncion, string tipFuncion)
        {
            List<DpoParametrosR2DTO> entitys = new List<DpoParametrosR2DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetParametrosR2);

            dbProvider.AddInParameter(command, helper.Dpocasdetcodi, DbType.Int32, idCaso);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, idFuncion);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.String, tipFuncion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoParametrosR2(dr));
                }
            }

            return entitys;
        }



        public List<DpoHistorico48DTO> FiltrarHistorico48PorRangoFechas(DateTime fechaini, DateTime fechafin)
        {
            List<DpoHistorico48DTO> entitys = new List<DpoHistorico48DTO>();

            string query = string.Format(helper.SqlFiltrarHistorico48PorRangoFechas,
                                         fechaini.ToString(ConstantesBase.FormatoFechaBase),
                                         fechafin.ToString(ConstantesBase.FormatoFechaBase));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoHistorico48(dr));
                }
            }

            return entitys;
        }

        public List<DpoHistorico96DTO> FiltrarHistorico96PorRangoFechas(DateTime fechaini, DateTime fechafin)
        {
            List<DpoHistorico96DTO> entitys = new List<DpoHistorico96DTO>();

            string query = string.Format(helper.SqlFiltrarHistorico96PorRangoFechas,
                                         fechaini.ToString(ConstantesBase.FormatoFechaBase),
                                         fechafin.ToString(ConstantesBase.FormatoFechaBase));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoHistorico96(dr));
                }
            }

            return entitys;
        }

        public List<DpoHistorico48DTO> ObtenerColumnaDatos48()
        {
            List<DpoHistorico48DTO> entitys = new List<DpoHistorico48DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerColumnaDatos48);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoHistorico48(dr));
                }
            }

            return entitys;
        }

        public List<DpoHistorico96DTO> ObtenerColumnaDatos96()
        {
            List<DpoHistorico96DTO> entitys = new List<DpoHistorico96DTO>();

            string query = string.Format(helper.SqlObtenerColumnaDatos96);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoHistorico96(dr));
                }
            }

            return entitys;
        }


        public List<DpoHistorico48DTO> ObtenerSerieDatos48()
        {
            List<DpoHistorico48DTO> entitys = new List<DpoHistorico48DTO>();

            string query = string.Format(helper.SqlObtenerSerieDatos48);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoHistorico48(dr));
                }
            }

            return entitys;
        }

        public List<DpoHistorico96DTO> ObtenerSerieDatos96()
        {
            List<DpoHistorico96DTO> entitys = new List<DpoHistorico96DTO>();

            string query = string.Format(helper.SqlObtenerSerieDatos96);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoHistorico96(dr));
                }
            }

            return entitys;
        }
        #endregion
    }
}
