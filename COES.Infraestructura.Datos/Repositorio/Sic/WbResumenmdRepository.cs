using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla WB_RESUMENMD
    /// </summary>
    public class WbResumenmdRepository : RepositoryBase, IWbResumenmdRepository
    {
        public WbResumenmdRepository(string strConn) : base(strConn)
        {
        }

        WbResumenmdHelper helper = new WbResumenmdHelper();

        public int Save(WbResumenmdDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Resmdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Resmdfecha, DbType.DateTime, entity.Resmdfecha);
            dbProvider.AddInParameter(command, helper.Resmdhora, DbType.DateTime, entity.Resmdhora);
            dbProvider.AddInParameter(command, helper.Resmdvalor, DbType.Decimal, entity.Resmdvalor);
            dbProvider.AddInParameter(command, helper.Resmdmes, DbType.DateTime, entity.Resmdmes);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
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
            dbProvider.AddInParameter(command, helper.H60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H61, DbType.Decimal, entity.H61);
            dbProvider.AddInParameter(command, helper.H62, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.H63, DbType.Decimal, entity.H63);
            dbProvider.AddInParameter(command, helper.H64, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.H65, DbType.Decimal, entity.H65);
            dbProvider.AddInParameter(command, helper.H66, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.H67, DbType.Decimal, entity.H67);
            dbProvider.AddInParameter(command, helper.H68, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.H69, DbType.Decimal, entity.H69);
            dbProvider.AddInParameter(command, helper.H70, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.H71, DbType.Decimal, entity.H71);
            dbProvider.AddInParameter(command, helper.H72, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.H73, DbType.Decimal, entity.H73);
            dbProvider.AddInParameter(command, helper.H74, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.H75, DbType.Decimal, entity.H75);
            dbProvider.AddInParameter(command, helper.H76, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.H77, DbType.Decimal, entity.H77);
            dbProvider.AddInParameter(command, helper.H78, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.H79, DbType.Decimal, entity.H79);
            dbProvider.AddInParameter(command, helper.H80, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.H81, DbType.Decimal, entity.H81);
            dbProvider.AddInParameter(command, helper.H82, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.H83, DbType.Decimal, entity.H83);
            dbProvider.AddInParameter(command, helper.H84, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.H85, DbType.Decimal, entity.H85);
            dbProvider.AddInParameter(command, helper.H86, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.H87, DbType.Decimal, entity.H87);
            dbProvider.AddInParameter(command, helper.H88, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.H89, DbType.Decimal, entity.H89);
            dbProvider.AddInParameter(command, helper.H90, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.H91, DbType.Decimal, entity.H91);
            dbProvider.AddInParameter(command, helper.H92, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.H93, DbType.Decimal, entity.H93);
            dbProvider.AddInParameter(command, helper.H94, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.H95, DbType.Decimal, entity.H95);
            dbProvider.AddInParameter(command, helper.H96, DbType.Decimal, entity.H96);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbResumenmdDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Resmdfecha, DbType.DateTime, entity.Resmdfecha);
            dbProvider.AddInParameter(command, helper.Resmdhora, DbType.DateTime, entity.Resmdhora);
            dbProvider.AddInParameter(command, helper.Resmdvalor, DbType.Decimal, entity.Resmdvalor);
            dbProvider.AddInParameter(command, helper.Resmdmes, DbType.DateTime, entity.Resmdmes);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
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
            dbProvider.AddInParameter(command, helper.H60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H61, DbType.Decimal, entity.H61);
            dbProvider.AddInParameter(command, helper.H62, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.H63, DbType.Decimal, entity.H63);
            dbProvider.AddInParameter(command, helper.H64, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.H65, DbType.Decimal, entity.H65);
            dbProvider.AddInParameter(command, helper.H66, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.H67, DbType.Decimal, entity.H67);
            dbProvider.AddInParameter(command, helper.H68, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.H69, DbType.Decimal, entity.H69);
            dbProvider.AddInParameter(command, helper.H70, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.H71, DbType.Decimal, entity.H71);
            dbProvider.AddInParameter(command, helper.H72, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.H73, DbType.Decimal, entity.H73);
            dbProvider.AddInParameter(command, helper.H74, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.H75, DbType.Decimal, entity.H75);
            dbProvider.AddInParameter(command, helper.H76, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.H77, DbType.Decimal, entity.H77);
            dbProvider.AddInParameter(command, helper.H78, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.H79, DbType.Decimal, entity.H79);
            dbProvider.AddInParameter(command, helper.H80, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.H81, DbType.Decimal, entity.H81);
            dbProvider.AddInParameter(command, helper.H82, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.H83, DbType.Decimal, entity.H83);
            dbProvider.AddInParameter(command, helper.H84, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.H85, DbType.Decimal, entity.H85);
            dbProvider.AddInParameter(command, helper.H86, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.H87, DbType.Decimal, entity.H87);
            dbProvider.AddInParameter(command, helper.H88, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.H89, DbType.Decimal, entity.H89);
            dbProvider.AddInParameter(command, helper.H90, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.H91, DbType.Decimal, entity.H91);
            dbProvider.AddInParameter(command, helper.H92, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.H93, DbType.Decimal, entity.H93);
            dbProvider.AddInParameter(command, helper.H94, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.H95, DbType.Decimal, entity.H95);
            dbProvider.AddInParameter(command, helper.H96, DbType.Decimal, entity.H96);
            dbProvider.AddInParameter(command, helper.Resmdcodi, DbType.Int32, entity.Resmdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int resmdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Resmdcodi, DbType.Int32, resmdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByMes(DateTime fechaPeriodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByMes);

            dbProvider.AddInParameter(command, helper.Resmdmes, DbType.DateTime, fechaPeriodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbResumenmdDTO GetById(int resmdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Resmdcodi, DbType.Int32, resmdcodi);
            WbResumenmdDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbResumenmdDTO> List()
        {
            List<WbResumenmdDTO> entitys = new List<WbResumenmdDTO>();
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

        public List<WbResumenmdDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<WbResumenmdDTO> entitys = new List<WbResumenmdDTO>();
            var querySql = string.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(querySql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public WbResumenmdDTO VerificarExistencia(DateTime fechaProceso)
        {
            WbResumenmdDTO entity = null;
            string sql = string.Format(helper.SqlVerificarExistencia, fechaProceso.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public WbResumenmdDTO GetMaxMdvalor(DateTime fechaInicio, DateTime fechaFin)
        {
            var querySql = string.Format(helper.SqlGetMaxMdvalor, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(querySql);
            WbResumenmdDTO entity = null;
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
