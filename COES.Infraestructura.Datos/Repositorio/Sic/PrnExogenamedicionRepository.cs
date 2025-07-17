using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.Interfaces.Sic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnExogenamedicionRepository : RepositoryBase, IPrnExogenamedicionRepository
    {
        public PrnExogenamedicionRepository(string strConn)
            : base(strConn)
        {
        }

        PrnExogenamedicionHelper helper = new PrnExogenamedicionHelper();

        public void Save(PrnExogenamedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Exmedicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Varexocodi, DbType.Int32, entity.Varexocodi);
            dbProvider.AddInParameter(command, helper.Aremedcodi, DbType.Int32, entity.Aremedcodi);
            dbProvider.AddInParameter(command, helper.Exmedifecha, DbType.DateTime, entity.Exmedifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.Exmeditotal, DbType.Decimal, entity.Exmeditotal);
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
            dbProvider.AddInParameter(command, helper.Exmedifeccreacion, DbType.DateTime, entity.Exmedifeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnExogenamedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Varexocodi, DbType.Int32, entity.Varexocodi);
            dbProvider.AddInParameter(command, helper.Aremedcodi, DbType.Int32, entity.Aremedcodi);
            dbProvider.AddInParameter(command, helper.Exmedifecha, DbType.DateTime, entity.Exmedifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.Exmeditotal, DbType.Decimal, entity.Exmeditotal);
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

            dbProvider.AddInParameter(command, helper.Exmedicodi, DbType.Int32, entity.Exmedicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int exmedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Exmedicodi, DbType.Int32, exmedicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnExogenamedicionDTO> List()
        {
            List<PrnExogenamedicionDTO> entitys = new List<PrnExogenamedicionDTO>();
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

        public PrnExogenamedicionDTO GetById(int varexocodi, int aremedcodi, DateTime fecha)
        {
            PrnExogenamedicionDTO entity = new PrnExogenamedicionDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Varexocodi, DbType.Int32, varexocodi);
            dbProvider.AddInParameter(command, helper.Aremedcodi, DbType.Int32, aremedcodi);
            dbProvider.AddInParameter(command, helper.Exmedifecha, DbType.DateTime, fecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrnExogenamedicionDTO> ListExomedicionByCiudadDate(int aremedcodi, DateTime fecha)
        {
            List<PrnExogenamedicionDTO> entitys = new List<PrnExogenamedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListExomedicionByCiudadDate);
            dbProvider.AddInParameter(command, helper.Aremedcodi, DbType.String, aremedcodi);
            dbProvider.AddInParameter(command, helper.Exmedifecha, DbType.DateTime, fecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.ListExomedicionByCiudadDate(dr));
                }
            }

            return entitys;
        }


        public List<PrnHorasolDTO> ListHorasol()
        {
            List<PrnHorasolDTO> entitys = new List<PrnHorasolDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListHorasol);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateHorasol(dr));
                }
            }

            return entitys;
        }
    }
}
