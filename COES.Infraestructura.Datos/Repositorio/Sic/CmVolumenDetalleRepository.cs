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
    /// Clase de acceso a datos de la tabla CM_VOLUMEN_DETALLE
    /// </summary>
    public class CmVolumenDetalleRepository : RepositoryBase, ICmVolumenDetalleRepository
    {
        public CmVolumenDetalleRepository(string strConn) : base(strConn)
        {
        }

        CmVolumenDetalleHelper helper = new CmVolumenDetalleHelper();

        public int Save(CmVolumenDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Voldetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Volcalcodi, DbType.Int32, entity.Volcalcodi);
            dbProvider.AddInParameter(command, helper.Modcomcodi, DbType.Int32, entity.Modcomcodi);
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
            dbProvider.AddInParameter(command, helper.T1, DbType.Int32, entity.T1);
            dbProvider.AddInParameter(command, helper.T2, DbType.Int32, entity.T2);
            dbProvider.AddInParameter(command, helper.T3, DbType.Int32, entity.T3);
            dbProvider.AddInParameter(command, helper.T4, DbType.Int32, entity.T4);
            dbProvider.AddInParameter(command, helper.T5, DbType.Int32, entity.T5);
            dbProvider.AddInParameter(command, helper.T6, DbType.Int32, entity.T6);
            dbProvider.AddInParameter(command, helper.T7, DbType.Int32, entity.T7);
            dbProvider.AddInParameter(command, helper.T8, DbType.Int32, entity.T8);
            dbProvider.AddInParameter(command, helper.T9, DbType.Int32, entity.T9);
            dbProvider.AddInParameter(command, helper.T10, DbType.Int32, entity.T10);
            dbProvider.AddInParameter(command, helper.T11, DbType.Int32, entity.T11);
            dbProvider.AddInParameter(command, helper.T12, DbType.Int32, entity.T12);
            dbProvider.AddInParameter(command, helper.T13, DbType.Int32, entity.T13);
            dbProvider.AddInParameter(command, helper.T14, DbType.Int32, entity.T14);
            dbProvider.AddInParameter(command, helper.T15, DbType.Int32, entity.T15);
            dbProvider.AddInParameter(command, helper.T16, DbType.Int32, entity.T16);
            dbProvider.AddInParameter(command, helper.T17, DbType.Int32, entity.T17);
            dbProvider.AddInParameter(command, helper.T18, DbType.Int32, entity.T18);
            dbProvider.AddInParameter(command, helper.T19, DbType.Int32, entity.T19);
            dbProvider.AddInParameter(command, helper.T20, DbType.Int32, entity.T20);
            dbProvider.AddInParameter(command, helper.T21, DbType.Int32, entity.T21);
            dbProvider.AddInParameter(command, helper.T22, DbType.Int32, entity.T22);
            dbProvider.AddInParameter(command, helper.T23, DbType.Int32, entity.T23);
            dbProvider.AddInParameter(command, helper.T24, DbType.Int32, entity.T24);
            dbProvider.AddInParameter(command, helper.T25, DbType.Int32, entity.T25);
            dbProvider.AddInParameter(command, helper.T26, DbType.Int32, entity.T26);
            dbProvider.AddInParameter(command, helper.T27, DbType.Int32, entity.T27);
            dbProvider.AddInParameter(command, helper.T28, DbType.Int32, entity.T28);
            dbProvider.AddInParameter(command, helper.T29, DbType.Int32, entity.T29);
            dbProvider.AddInParameter(command, helper.T30, DbType.Int32, entity.T30);
            dbProvider.AddInParameter(command, helper.T31, DbType.Int32, entity.T31);
            dbProvider.AddInParameter(command, helper.T32, DbType.Int32, entity.T32);
            dbProvider.AddInParameter(command, helper.T33, DbType.Int32, entity.T33);
            dbProvider.AddInParameter(command, helper.T34, DbType.Int32, entity.T34);
            dbProvider.AddInParameter(command, helper.T35, DbType.Int32, entity.T35);
            dbProvider.AddInParameter(command, helper.T36, DbType.Int32, entity.T36);
            dbProvider.AddInParameter(command, helper.T37, DbType.Int32, entity.T37);
            dbProvider.AddInParameter(command, helper.T38, DbType.Int32, entity.T38);
            dbProvider.AddInParameter(command, helper.T39, DbType.Int32, entity.T39);
            dbProvider.AddInParameter(command, helper.T40, DbType.Int32, entity.T40);
            dbProvider.AddInParameter(command, helper.T41, DbType.Int32, entity.T41);
            dbProvider.AddInParameter(command, helper.T42, DbType.Int32, entity.T42);
            dbProvider.AddInParameter(command, helper.T43, DbType.Int32, entity.T43);
            dbProvider.AddInParameter(command, helper.T44, DbType.Int32, entity.T44);
            dbProvider.AddInParameter(command, helper.T45, DbType.Int32, entity.T45);
            dbProvider.AddInParameter(command, helper.T46, DbType.Int32, entity.T46);
            dbProvider.AddInParameter(command, helper.T47, DbType.Int32, entity.T47);
            dbProvider.AddInParameter(command, helper.T48, DbType.Int32, entity.T48);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmVolumenDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.T6, DbType.Int32, entity.T6);
            dbProvider.AddInParameter(command, helper.T7, DbType.Int32, entity.T7);
            dbProvider.AddInParameter(command, helper.T8, DbType.Int32, entity.T8);
            dbProvider.AddInParameter(command, helper.T9, DbType.Int32, entity.T9);
            dbProvider.AddInParameter(command, helper.T10, DbType.Int32, entity.T10);
            dbProvider.AddInParameter(command, helper.T11, DbType.Int32, entity.T11);
            dbProvider.AddInParameter(command, helper.T12, DbType.Int32, entity.T12);
            dbProvider.AddInParameter(command, helper.T13, DbType.Int32, entity.T13);
            dbProvider.AddInParameter(command, helper.T14, DbType.Int32, entity.T14);
            dbProvider.AddInParameter(command, helper.T15, DbType.Int32, entity.T15);
            dbProvider.AddInParameter(command, helper.T16, DbType.Int32, entity.T16);
            dbProvider.AddInParameter(command, helper.T17, DbType.Int32, entity.T17);
            dbProvider.AddInParameter(command, helper.T18, DbType.Int32, entity.T18);
            dbProvider.AddInParameter(command, helper.T19, DbType.Int32, entity.T19);
            dbProvider.AddInParameter(command, helper.T20, DbType.Int32, entity.T20);
            dbProvider.AddInParameter(command, helper.T21, DbType.Int32, entity.T21);
            dbProvider.AddInParameter(command, helper.T22, DbType.Int32, entity.T22);
            dbProvider.AddInParameter(command, helper.T23, DbType.Int32, entity.T23);
            dbProvider.AddInParameter(command, helper.T24, DbType.Int32, entity.T24);
            dbProvider.AddInParameter(command, helper.T25, DbType.Int32, entity.T25);
            dbProvider.AddInParameter(command, helper.T26, DbType.Int32, entity.T26);
            dbProvider.AddInParameter(command, helper.T27, DbType.Int32, entity.T27);
            dbProvider.AddInParameter(command, helper.T28, DbType.Int32, entity.T28);
            dbProvider.AddInParameter(command, helper.T29, DbType.Int32, entity.T29);
            dbProvider.AddInParameter(command, helper.T30, DbType.Int32, entity.T30);
            dbProvider.AddInParameter(command, helper.T31, DbType.Int32, entity.T31);
            dbProvider.AddInParameter(command, helper.T32, DbType.Int32, entity.T32);
            dbProvider.AddInParameter(command, helper.T33, DbType.Int32, entity.T33);
            dbProvider.AddInParameter(command, helper.T34, DbType.Int32, entity.T34);
            dbProvider.AddInParameter(command, helper.T35, DbType.Int32, entity.T35);
            dbProvider.AddInParameter(command, helper.T36, DbType.Int32, entity.T36);
            dbProvider.AddInParameter(command, helper.T37, DbType.Int32, entity.T37);
            dbProvider.AddInParameter(command, helper.T38, DbType.Int32, entity.T38);
            dbProvider.AddInParameter(command, helper.T39, DbType.Int32, entity.T39);
            dbProvider.AddInParameter(command, helper.T40, DbType.Int32, entity.T40);
            dbProvider.AddInParameter(command, helper.T41, DbType.Int32, entity.T41);
            dbProvider.AddInParameter(command, helper.T42, DbType.Int32, entity.T42);
            dbProvider.AddInParameter(command, helper.T43, DbType.Int32, entity.T43);
            dbProvider.AddInParameter(command, helper.T44, DbType.Int32, entity.T44);
            dbProvider.AddInParameter(command, helper.T45, DbType.Int32, entity.T45);
            dbProvider.AddInParameter(command, helper.T46, DbType.Int32, entity.T46);
            dbProvider.AddInParameter(command, helper.T47, DbType.Int32, entity.T47);
            dbProvider.AddInParameter(command, helper.T48, DbType.Int32, entity.T48);
            dbProvider.AddInParameter(command, helper.Voldetcodi, DbType.Int32, entity.Voldetcodi);
            dbProvider.AddInParameter(command, helper.Volcalcodi, DbType.Int32, entity.Volcalcodi);
            dbProvider.AddInParameter(command, helper.Modcomcodi, DbType.Int32, entity.Modcomcodi);
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
            dbProvider.AddInParameter(command, helper.T1, DbType.Int32, entity.T1);
            dbProvider.AddInParameter(command, helper.T2, DbType.Int32, entity.T2);
            dbProvider.AddInParameter(command, helper.T3, DbType.Int32, entity.T3);
            dbProvider.AddInParameter(command, helper.T4, DbType.Int32, entity.T4);
            dbProvider.AddInParameter(command, helper.T5, DbType.Int32, entity.T5);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int voldetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Voldetcodi, DbType.Int32, voldetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmVolumenDetalleDTO GetById(int voldetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Voldetcodi, DbType.Int32, voldetcodi);
            CmVolumenDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmVolumenDetalleDTO> List()
        {
            List<CmVolumenDetalleDTO> entitys = new List<CmVolumenDetalleDTO>();
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

        public List<CmVolumenDetalleDTO> GetByCriteria(int volcalcodi)
        {
            List<CmVolumenDetalleDTO> entitys = new List<CmVolumenDetalleDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, volcalcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
