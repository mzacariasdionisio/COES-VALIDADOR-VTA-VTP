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
    /// Clase de acceso a datos de la tabla F_LECTURA
    /// </summary>
    public class FLecturaRepository: RepositoryBase, IFLecturaRepository
    {
        public FLecturaRepository(string strConn): base(strConn)
        {
        }

        FLecturaHelper helper = new FLecturaHelper();

        public void Update(FLecturaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, entity.Fechahora);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, entity.Gpscodi);
            dbProvider.AddInParameter(command, helper.Vsf, DbType.Decimal, entity.Vsf);
            dbProvider.AddInParameter(command, helper.Maximo, DbType.Decimal, entity.Maximo);
            dbProvider.AddInParameter(command, helper.Minimo, DbType.Decimal, entity.Minimo);
            dbProvider.AddInParameter(command, helper.Voltaje, DbType.Decimal, entity.Voltaje);
            dbProvider.AddInParameter(command, helper.Num, DbType.Int32, entity.Num);
            dbProvider.AddInParameter(command, helper.Desv, DbType.Decimal, entity.Desv);
            dbProvider.AddInParameter(command, helper.H0, DbType.Decimal, entity.H0);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.Devsec, DbType.Decimal, entity.Devsec);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public FLecturaDTO GetById(DateTime FechaHora, Int32 GpsCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, FechaHora);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, GpsCodi);
            FLecturaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FLecturaDTO> List()
        {
            List<FLecturaDTO> entitys = new List<FLecturaDTO>();
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

        public List<FLecturaDTO> GetByCriteria(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            List<FLecturaDTO> entitys = new List<FLecturaDTO>();
            FLecturaDTO entity = null;
            string sFechaIni = FechaInicio.ToString("dd-MM-yyyy") + " 00:00:00";
            string sFechaFin = FechaFin.ToString("dd-MM-yyyy") + " 23:59:59";
            string sCommand = string.Format(helper.SqlGetByCriteria, GpsCodi, sFechaIni, sFechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new FLecturaDTO();
                    entity = helper.Create(dr);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DataTable GetByCriteriaDatatable(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            var entitys = new DataTable();
            string sFechaIni = FechaInicio.ToString("dd-MM-yyyy") + " 00:00:00";
            string sFechaFin = FechaFin.ToString("dd-MM-yyyy") + " 23:59:59";
            string sCommand = string.Format(helper.SqlGetByCriteria, GpsCodi, sFechaIni, sFechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys.Load(dr);
            }

            return entitys;
        }

        public DataTable GetByCriteriaDatatable2(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            var entitys = new DataTable();
            string sFechaIni = FechaInicio.ToString("dd-MM-yyyy") + " 00:00:00";
            string sFechaFin = FechaFin.ToString("dd-MM-yyyy") + " 23:59:59";
            string sCommand = string.Format(helper.SqlGetByCriteria2, GpsCodi, sFechaIni, sFechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys.Load(dr);
            }

            return entitys;
        }


        public DataTable GetFechaDesvNumPorGpsFecha(int GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            var entitys = new DataTable();
            string sFechaIni = FechaInicio.ToString("dd-MM-yyyy") + " 00:00:00";
            string sFechaFin = FechaFin.ToString("dd-MM-yyyy") + " 23:59:59";
            string sCommand = string.Format(helper.SqlGetFechaDesvNumPorGpsFecha, GpsCodi, sFechaIni, sFechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys.Load(dr);
            }

            return entitys;
        }

        public List<FLecturaDTO> ObtenerFrecuenciaSein(int iGpscodi)
        {
            List<FLecturaDTO> entitys = new List<FLecturaDTO>();
            string squery = string.Format(helper.SqlObtenerFrecuenciaSein, iGpscodi);
            DbCommand command = dbProvider.GetSqlStringCommand(squery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FLecturaDTO entity = new FLecturaDTO();

                    int iFechahora = dr.GetOrdinal(helper.Fechahora);
                    if (!dr.IsDBNull(iFechahora)) entity.Fechahora = dr.GetDateTime(iFechahora);

                    for (int i = 0; i <= 59; i++)
                    {
                        int ordinal = dr.GetOrdinal(helper.CaracterH + i);
                        if (!dr.IsDBNull(ordinal))
                            entity.GetType().GetProperty(helper.CaracterH + i).SetValue(entity, dr.GetDecimal(ordinal));
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FLecturaDTO> ListByFechaDesvNumPorGpsFecha(int gpsCodi, DateTime fecha)
        {
            List<FLecturaDTO> entitys = new List<FLecturaDTO>();

            string sFechaIni = fecha.ToString("dd-MM-yyyy") + " 00:00:00";
            string sFechaFin = fecha.ToString("dd-MM-yyyy") + " 23:59:59";
            string sCommand = string.Format(helper.SqlGetFechaDesvNumPorGpsFecha, gpsCodi, sFechaIni, sFechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FLecturaDTO entity = new FLecturaDTO();

                    int iFechahora = dr.GetOrdinal(helper.Fechahora);
                    if (!dr.IsDBNull(iFechahora)) entity.Fechahora = dr.GetDateTime(iFechahora);

                    int iNum = dr.GetOrdinal(helper.Num);
                    if (!dr.IsDBNull(iNum)) entity.Num = Convert.ToInt32(dr.GetValue(iNum));

                    int iDesv = dr.GetOrdinal(helper.Desv);
                    if (!dr.IsDBNull(iDesv)) entity.Desv = dr.GetDecimal(iDesv);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public int ContarRegistrosRepetidos(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin)
        {
            int cantidad=0;
            string sFechaIni = FechaInicio.ToString("dd-MM-yyyy") + " 00:00:00";
            string sFechaFin = FechaFin.ToString("dd-MM-yyyy") + " 23:59:59";
            string sCommand = string.Format(helper.SqlContarRegistrosRepetidos, GpsCodi, sFechaIni, sFechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    cantidad = dr.GetInt32(0);
                }
            }

            return cantidad;
        }
    }
}
