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
    /// Clase de acceso a datos de la tabla RPF_MEDICION60
    /// </summary>
    public class RpfMedicion60Repository: RepositoryBase, IRpfMedicion60Repository
    {
        public RpfMedicion60Repository(string strConn): base(strConn)
        {
        }

        RpfMedicion60Helper helper = new RpfMedicion60Helper();

        public int GetMaxId(string tableName)
        {
            string query = string.Format(helper.SqlObtenerMaxId, tableName);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(RpfMedicion60DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rpfmedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, entity.Cotidacodi);
            dbProvider.AddInParameter(command, helper.Rpfenvcodi, DbType.Int32, entity.Rpfenvcodi);
            dbProvider.AddInParameter(command, helper.Rpfmedfecha, DbType.DateTime, entity.Rpfmedfecha);
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

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RpfMedicion60DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, entity.Cotidacodi);
            dbProvider.AddInParameter(command, helper.Rpfenvcodi, DbType.Int32, entity.Rpfenvcodi);
            dbProvider.AddInParameter(command, helper.Rpfmedfecha, DbType.DateTime, entity.Rpfmedfecha);
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
            dbProvider.AddInParameter(command, helper.Rpfmedcodi, DbType.Int32, entity.Rpfmedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int idEnvio, string tableName)
        {
            string query = string.Format(helper.SqlDelete, tableName, idEnvio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public RpfMedicion60DTO GetById(int rpfmedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rpfmedcodi, DbType.Int32, rpfmedcodi);
            RpfMedicion60DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RpfMedicion60DTO> List()
        {
            List<RpfMedicion60DTO> entitys = new List<RpfMedicion60DTO>();
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

        public List<RpfMedicion60DTO> GetByCriteria()
        {
            List<RpfMedicion60DTO> entitys = new List<RpfMedicion60DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void GrabarMasivo(List<RpfMedicion60DTO> entitys, string nombreTabla)
        {          
            dbProvider.AddColumnMapping(helper.Rpfmedcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Grupocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cotidacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rpfenvcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rpfmedfecha, DbType.DateTime);


            dbProvider.AddColumnMapping(helper.H1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H60, DbType.Decimal);            

            dbProvider.BulkInsertRSF<RpfMedicion60DTO>(entitys, nombreTabla);
        }

        public List<RpfMedicion60DTO> ObtenerConsulta(int idUrs, int idEquipo, int tipoDato, DateTime fechaInicio, DateTime fechaFin, string tableName)
        {
            List<RpfMedicion60DTO> entitys = new List<RpfMedicion60DTO>();
            string sql = string.Format(helper.SqlObtenerConsulta, idUrs, idEquipo, tipoDato, 
                fechaInicio.ToString(ConstantesBase.FormatoFechaHora), fechaFin.ToString(ConstantesBase.FormatoFechaHora), tableName);
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

        public int VerificarExistenciaTabla(string nombre)
        {
            string sql = string.Format(helper.SqlVerificarExistenciaTabla, nombre);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }
            return 0;
        }

        public void CrearTabla(int i, string nombre)
        {
            string sql = "";

            switch (i)
            {
                case 0:
                    sql = string.Format(this.helper.SqlCrearTabla, nombre);
                    break;
                case 1:
                    sql = string.Format(this.helper.SqlCrearIndex, nombre);
                    break;
                case 2:
                    sql = string.Format(this.helper.SqlCrearPKMedicion60, nombre);
                    break;
                case 3:
                    sql = string.Format(this.helper.SqlCrearFKGrupo, nombre);
                    break;
                case 4:
                    sql = string.Format(this.helper.SqlCrearFKEquipo, nombre);
                    break;
                case 5:
                    sql = string.Format(this.helper.SqlCrearFKTipoDato, nombre);
                    break;
                case 6:
                    sql = string.Format(this.helper.SqlCrearFKEnvio, nombre);
                    break;

            }

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteScalar(command);
        }

        public List<RpfMedicion60DTO> GetInformacionAGCExtranet(DateTime fechaConsulta, string strFecha, string strIdsUrs, string strIdsEquipos, string strTipoDato)
        {            
            string nombTabla = fechaConsulta.Year.ToString() + fechaConsulta.Month.ToString("00");

            List<RpfMedicion60DTO> entitys = new List<RpfMedicion60DTO>();
            string sql = string.Format(helper.SqlGetInformacionAGC, nombTabla, strFecha, strIdsUrs, strIdsEquipos, strTipoDato);
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

        public List<RpfMedicion60DTO> ObtenerReporteCumplimiento(DateTime fecha, string grupos, string tableName)
        {           
            List<RpfMedicion60DTO> entitys = new List<RpfMedicion60DTO>();
            string sql = string.Format(helper.SqlObtenerReporteCumplimiento, fecha.ToString(ConstantesBase.FormatoFecha), grupos, tableName);
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
