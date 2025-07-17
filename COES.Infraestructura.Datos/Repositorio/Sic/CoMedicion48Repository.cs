using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CO_MEDICION48
    /// </summary>
    public class CoMedicion48Repository: RepositoryBase, ICoMedicion48Repository
    {
        public CoMedicion48Repository(string strConn): base(strConn)
        {
        }

        CoMedicion48Helper helper = new CoMedicion48Helper();

        public int Save(CoMedicion48DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Comedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cotinfcodi, DbType.Int32, entity.Cotinfcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Comedfecha, DbType.DateTime, entity.Comedfecha);
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

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoMedicion48DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cotinfcodi, DbType.Int32, entity.Cotinfcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Comedfecha, DbType.DateTime, entity.Comedfecha);
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
            dbProvider.AddInParameter(command, helper.Comedcodi, DbType.Int32, entity.Comedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int comedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Comedcodi, DbType.Int32, comedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoMedicion48DTO GetById(int comedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Comedcodi, DbType.Int32, comedcodi);
            CoMedicion48DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoMedicion48DTO> List()
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
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

        public List<CoMedicion48DTO> GetByCriteria()
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
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

        public List<CoMedicion48DTO> ObtenerTopologias(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerTopologias, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();

                    int iTopcodi1 = dr.GetOrdinal(helper.Topcodi1);
                    if (!dr.IsDBNull(iTopcodi1)) entity.Topcodi1 = Convert.ToInt32(dr.GetValue(iTopcodi1));

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iTopfecha = dr.GetOrdinal(helper.Topfecha);
                    if (!dr.IsDBNull(iTopfecha)) entity.Topfecha = dr.GetDateTime(iTopfecha);

                    int iTopfinal = dr.GetOrdinal(helper.Topfinal);
                    if (!dr.IsDBNull(iTopfinal)) entity.Topfinal = Convert.ToInt32(dr.GetValue(iTopfinal));

                    int iTopiniciohora = dr.GetOrdinal(helper.Topiniciohora);
                    if (!dr.IsDBNull(iTopiniciohora)) entity.Topiniciohora = Convert.ToInt32(dr.GetValue(iTopiniciohora));

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<CoMedicion48DTO> ObtenerReporteReprograma(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerReporteReprograma, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();                    

                    int iTopfecha = dr.GetOrdinal(helper.Topfecha);
                    if (!dr.IsDBNull(iTopfecha)) entity.Topfecha = dr.GetDateTime(iTopfecha);                  

                    int iTopiniciohora = dr.GetOrdinal(helper.Topiniciohora);
                    if (!dr.IsDBNull(iTopiniciohora)) entity.Topiniciohora = Convert.ToInt32(dr.GetValue(iTopiniciohora));

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iTopnombre = dr.GetOrdinal(helper.Topnombre);
                    if (!dr.IsDBNull(iTopnombre)) entity.Topnombre = dr.GetString(iTopnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }               

        public List<CoMedicion48DTO> ObtenerReporteReprogramaSinRSF(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerReporteReprogramaSinRSF, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();

                    int iTopfecha = dr.GetOrdinal(helper.Topfecha);
                    if (!dr.IsDBNull(iTopfecha)) entity.Topfecha = dr.GetDateTime(iTopfecha);

                    int iTopiniciohora = dr.GetOrdinal(helper.Topiniciohora);
                    if (!dr.IsDBNull(iTopiniciohora)) entity.Topiniciohora = Convert.ToInt32(dr.GetValue(iTopiniciohora));

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iTopnombre = dr.GetOrdinal(helper.Topnombre);
                    if (!dr.IsDBNull(iTopnombre)) entity.Topnombre = dr.GetString(iTopnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoMedicion48DTO> ObtenerTopologiasSinReserva(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerTopologiasSinReserva, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();

                    int iTopcodi1 = dr.GetOrdinal(helper.Topcodi1);
                    if (!dr.IsDBNull(iTopcodi1)) entity.Topcodi1 = Convert.ToInt32(dr.GetValue(iTopcodi1));

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iTopfecha = dr.GetOrdinal(helper.Topfecha);
                    if (!dr.IsDBNull(iTopfecha)) entity.Topfecha = dr.GetDateTime(iTopfecha);

                    int iTopfinal = dr.GetOrdinal(helper.Topfinal);
                    if (!dr.IsDBNull(iTopfinal)) entity.Topfinal = Convert.ToInt32(dr.GetValue(iTopfinal));

                    int iTopiniciohora = dr.GetOrdinal(helper.Topiniciohora);
                    if (!dr.IsDBNull(iTopiniciohora)) entity.Topiniciohora = Convert.ToInt32(dr.GetValue(iTopiniciohora));

                    entitys.Add(entity);

                }
            }

            return entitys;
        }


        public List<CpMedicion48DTO> GetByCriteria(string topcodi, DateTime medifecha, string srestcodi)
        {
            CpMedicion48Helper helperCP = new CpMedicion48Helper();
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            
            string query = string.Format(helper.SqlObtenerDatosYupana, medifecha.ToString(ConstantesBase.FormatoFecha), topcodi, srestcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpMedicion48DTO entity = new CpMedicion48DTO();

                    //int iRecurcodi = dr.GetOrdinal(helperCP.Recurcodi);
                    //if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

                    int iSrestcodi = dr.GetOrdinal(helperCP.Srestcodi);
                    if (!dr.IsDBNull(iSrestcodi)) entity.Srestcodi = Convert.ToInt32(dr.GetValue(iSrestcodi));

                    int iTopcodi = dr.GetOrdinal(helperCP.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iMedifecha = dr.GetOrdinal(helperCP.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iGrupocodi = dr.GetOrdinal(helperCP.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;


                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);


                        entity.GetType().GetProperty("H" + i).SetValue(entity, valor);
                    }

                    int iRecurcodisicoes = dr.GetOrdinal(helperCP.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodisicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecurcodisicoes));                    

                    int iRecurnombre = dr.GetOrdinal(helperCP.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    int iUrsnomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iUrsnomb)) entity.Equinomb = dr.GetString(iUrsnomb);

                    //int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    //if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    //int iFamcodi = dr.GetOrdinal(helperCP.Famcodi);
                    //if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<CpMedicion48DTO> ObtenerDatosAgrupados(string topcodi, DateTime medifecha, string srestcodi)
        {
            CpMedicion48Helper helperCP = new CpMedicion48Helper();
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerDatosYupanaAgrupado, medifecha.ToString(ConstantesBase.FormatoFecha), topcodi, srestcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpMedicion48DTO entity = new CpMedicion48DTO();

                    //int iRecurcodi = dr.GetOrdinal(helperCP.Recurcodi);
                    //if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

                    int iSrestcodi = dr.GetOrdinal(helperCP.Srestcodi);
                    if (!dr.IsDBNull(iSrestcodi)) entity.Srestcodi = Convert.ToInt32(dr.GetValue(iSrestcodi));

                    int iTopcodi = dr.GetOrdinal(helperCP.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iMedifecha = dr.GetOrdinal(helperCP.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);


                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;

                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        entity.GetType().GetProperty("H" + i).SetValue(entity, valor);
                    }

                    int iRecurcodisicoes = dr.GetOrdinal(helperCP.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodisicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecurcodisicoes));

                    int iFamcodi = dr.GetOrdinal(helperCP.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iRecurnomb = dr.GetOrdinal(helperCP.Recurnombre);
                    if (!dr.IsDBNull(iRecurnomb)) entity.Recurnombre = dr.GetString(iRecurnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoMedicion48DTO> ObtenerDatosRAEjecutado(DateTime fechaInicio, DateTime fechaFin, out List<CoRaejecutadadetDTO> detalle)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            List<CoMedicion48DTO> result = new List<CoMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerDatosRAEjecutado, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();

                    int iRsffecha = dr.GetOrdinal(helper.Rsffecha);
                    if (!dr.IsDBNull(iRsffecha)) entity.Rsffecha = dr.GetDateTime(iRsffecha);

                    int iRsffechainicio = dr.GetOrdinal(helper.Rsffechainicio);
                    if (!dr.IsDBNull(iRsffechainicio)) entity.Rsffechainicio = dr.GetDateTime(iRsffechainicio);

                    int iRsffechafin = dr.GetOrdinal(helper.Rsffechafin);
                    if (!dr.IsDBNull(iRsffechafin)) entity.Rsffechafin = dr.GetDateTime(iRsffechafin);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iUrsnomb = dr.GetOrdinal(helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iRsfsubida = dr.GetOrdinal(helper.Rsfsubida);
                    if (!dr.IsDBNull(iRsfsubida)) entity.Rsfsubida = dr.GetDecimal(iRsfsubida);

                    int iRsfbajada = dr.GetOrdinal(helper.Rsfbajada);
                    if (!dr.IsDBNull(iRsfbajada)) entity.Rsfbajada = dr.GetDecimal(iRsfbajada);

                    //int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    //if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    //int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    //if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            List<DateTime> fechas = entitys.Select(x => x.Rsffecha).Distinct().ToList();
            var urs = entitys.Select(x => new { Grupocodi = x.Grupocodi, Gruponomb = x.Gruponomb, Ursnomb = x.Ursnomb }).Distinct().ToList();
            List<CoRaejecutadadetDTO> listDetalle = new List<CoRaejecutadadetDTO>();

            foreach (DateTime fecha in fechas)
            {
                foreach (var item in urs)
                {
                    List<CoMedicion48DTO> subList = entitys.Where(x => x.Rsffecha == fecha && x.Grupocodi == item.Grupocodi).
                        OrderBy(x => x.Rsffechainicio).ToList();

                    if (subList.Count == 48)
                    {
                        foreach (CoMedicion48DTO itemList in subList)
                        {
                            int minutos = (int)itemList.Rsffechafin.Subtract(itemList.Rsffechainicio).TotalMinutes;

                            if (minutos != 30)
                            {
                                //marcamos para la cantidad de minutos

                                int periodo = this.CalcularPeriodo(itemList.Rsffechainicio);
                                CoRaejecutadadetDTO itemDetalle = new CoRaejecutadadetDTO
                                {
                                    Coradefecha = fecha,
                                    Coradeindice = periodo,
                                    Corademinutos = minutos,
                                    Grupocodi = item.Grupocodi,
                                    Coraderasub = itemList.Rsfsubida,
                                    Coraderabaj = itemList.Rsfbajada
                                };
                                listDetalle.Add(itemDetalle);

                                itemList.Rsfindicador = 1;
                            }

                            result.Add(itemList);
                        }
                    }
                    else
                    {
                        //hacemos el tratamiento de los datos

                        int count = 0;
                        decimal ponSubir = 0;
                        decimal ponBajar = 0;
                        bool flag = false;
                        foreach (CoMedicion48DTO itemList in subList)
                        {
                            int minutos = (int)itemList.Rsffechafin.Subtract(itemList.Rsffechainicio).TotalMinutes;

                            if (minutos == 30)
                            {
                                result.Add(itemList);
                            }
                            else
                            {
                                count++;

                                decimal ponSubirOrig = (decimal)(itemList.Rsfsubida != null ? itemList.Rsfsubida : 0);
                                decimal ponBajarOrig = (decimal)(itemList.Rsfbajada != null ? itemList.Rsfbajada : 0);

                                ponSubir = (ponSubirOrig != 0) ? ponSubirOrig : ponSubir;
                                ponBajar = (ponBajarOrig != 0) ? ponBajarOrig : ponBajar;

                                int periodo = this.CalcularPeriodo(itemList.Rsffechainicio);
                                CoRaejecutadadetDTO itemDetalle = new CoRaejecutadadetDTO
                                {
                                    Coradefecha = fecha,
                                    Coradeindice = periodo,
                                    Corademinutos = minutos,
                                    Grupocodi = item.Grupocodi,
                                    Coraderasub = ponSubirOrig,
                                    Coraderabaj = ponBajarOrig
                                };

                                if (ponSubirOrig > 0 || ponBajarOrig > 0)
                                {
                                    listDetalle.Add(itemDetalle);
                                    flag = true;
                                }

                                if (count == 2)
                                {
                                    count = 0;
                                    itemList.Rsfsubida = Math.Round(ponSubir, 2);
                                    itemList.Rsfbajada = Math.Round(ponBajar, 2);

                                    if (flag)
                                    {
                                        itemList.Rsfindicador = 1;
                                    }

                                    result.Add(itemList);
                                    flag = false;
                                    ponSubir = 0;
                                    ponBajar = 0;
                                }

                            }
                        }
                    }
                }
            }

            detalle = listDetalle;

            return result;
        }
        /// <summary>
        /// Calcula el periodo en el que se está ejecutando el proceso
        /// </summary>
        /// <returns></returns>
        public  int CalcularPeriodo(DateTime fecha)
        {
            if (fecha.Minute == 0 || fecha.Minute == 30) fecha = fecha.AddMinutes(1);
            int totalMinutes = fecha.Hour * 60 + fecha.Minute;
            return Convert.ToInt32(Math.Ceiling(((decimal)totalMinutes / 30.0M)));
        }

        public void GrabarDatosBulk(List<CoMedicion48DTO> entitys, int idPeriodo, int idVersion, int tipoInfo, DateTime fechaInicio, DateTime fechaFin)
        {
            string sqlDelete = string.Format(helper.SqlDelete, idPeriodo, idVersion, tipoInfo, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete); 
            command.CommandTimeout = 0;
            //dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, idPeriodo);
            //dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, idVersion);
            //dbProvider.AddInParameter(command, helper.Cotinfcodi, DbType.Int32, tipoInfo);

            dbProvider.ExecuteNonQuery(command);
            command.Parameters.Clear();

            #region AddColumnMapping
            dbProvider.AddColumnMapping(helper.Cotinfcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Grupocodi, DbType.Int32);           
            dbProvider.AddColumnMapping(helper.Copercodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Covercodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comedfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comedtipo, DbType.Int32);
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
            
            #endregion

            dbProvider.BulkInsert<CoMedicion48DTO>(entitys, helper.NombreTabla);
        }

        public void GrabarDatosBulkResult(List<CoMedicion48DTO> entitys, int idPeriodo, int idVersion, int tipoInfo, DateTime fechaInicio, DateTime fechaFin)
        {
            string sqlDelete = string.Format(helper.SqlDelete, idPeriodo, idVersion, tipoInfo, fechaInicio.ToString(ConstantesBase.FormatoFecha),
               fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            command.CommandTimeout = 0;
            //dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, idPeriodo);
            //dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, idVersion);
            //dbProvider.AddInParameter(command, helper.Cotinfcodi, DbType.Int32, tipoInfo);
            dbProvider.ExecuteNonQuery(command);
            command.Parameters.Clear();

            #region AddColumnMapping
            dbProvider.AddColumnMapping(helper.Cotinfcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Grupocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Copercodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Covercodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comedfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comedtipo, DbType.Int32);
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
            dbProvider.AddColumnMapping(helper.T1, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T2, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T3, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T4, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T5, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T6, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T7, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T8, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T9, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T10, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T11, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T12, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T13, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T14, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T15, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T16, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T17, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T18, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T19, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T20, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T21, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T22, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T23, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T24, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T25, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T26, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T27, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T28, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T29, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T30, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T31, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T32, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T33, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T34, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T35, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T36, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T37, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T38, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T39, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T40, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T41, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T42, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T43, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T44, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T45, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T46, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T47, DbType.Int32);
            dbProvider.AddColumnMapping(helper.T48, DbType.Int32);

            #endregion

            dbProvider.BulkInsert<CoMedicion48DTO>(entitys, helper.NombreTabla);
        }

        public List<CoMedicion48DTO> ObtenerDatosProgramaFinal(int idVersion, DateTime fechaInicio, DateTime fechaFin, int idTipoInfo)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerDatosProgramaFinal, idVersion, idTipoInfo, 
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int Tipo = 0;
                    int Grupocodi = 0;
                    int Equicodi = 0;
                    DateTime Fecha = DateTime.Now;
                    string Grupo = string.Empty;                    

                    int iComedtipo = dr.GetOrdinal(helper.Comedtipo);
                    if (!dr.IsDBNull(iComedtipo)) Tipo = Convert.ToInt32(dr.GetValue(iComedtipo));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iComedfecha = dr.GetOrdinal(helper.Comedfecha);
                    if (!dr.IsDBNull(iComedfecha)) Fecha = dr.GetDateTime(iComedfecha);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) Grupo = dr.GetString(iGruponomb);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        CoMedicion48DTO entity = new CoMedicion48DTO();

                        entity.Comedtipo = Tipo;
                        entity.Grupocodi = Grupocodi;
                        entity.Equicodi = Equicodi;
                        entity.Comedfecha = Fecha;
                        entity.Gruponomb = Grupo;
                        entity.Valor = valor;
                        entity.Orden = i;

                        entitys.Add(entity);
                    }                    
                }
            }

            return entitys;
        }

        public List<CoMedicion48DTO> ObtenerDatosResultadoFinal(int idVersion, int idTipoInfo)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerDatosResultadoFinal, idVersion, idTipoInfo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {        
                    int Tipo = 0;
                    int Grupocodi = 0;
                    int Equicodi = 0;
                    DateTime Fecha = DateTime.Now;
                    string Grupo = string.Empty;
                    string Urs = string.Empty;

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) Tipo = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi1);
                    if (!dr.IsDBNull(iEquicodi)) Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iComedfecha = dr.GetOrdinal(helper.Comedfecha);
                    if (!dr.IsDBNull(iComedfecha)) Fecha = dr.GetDateTime(iComedfecha);

                    int iUrsnomb = dr.GetOrdinal(helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) Urs = dr.GetString(iUrsnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) Grupo = dr.GetString(iGruponomb);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        int color = 0;
                        int indiceColor = dr.GetOrdinal("T" + i);
                        if (!dr.IsDBNull(indiceColor)) color = Convert.ToInt32(dr.GetValue(indiceColor));

                        CoMedicion48DTO entity = new CoMedicion48DTO();

                        entity.Famcodi = Tipo;
                        entity.Grupocodi = Grupocodi;
                        entity.Equicodi = Equicodi;
                        entity.Comedfecha = Fecha;
                        entity.Gruponomb = Grupo;
                        entity.Ursnomb = Urs;
                        entity.Valor = valor;
                        entity.Color = color;
                        entity.Orden = i;

                        entitys.Add(entity);
                    }
                }
            }

            return entitys;
        }

        public List<CoMedicion48DTO> ObtenerDatosReservaFinal(int idVersion, DateTime fechaInicio, DateTime fechaFin, int idTipoInfo)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerDatosReservaFinal, idVersion, idTipoInfo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int Tipo = 0;
                    int Grupocodi = 0;
                    int Equicodi = 0;
                    DateTime Fecha = DateTime.Now;
                    string Grupo = string.Empty;
                    string Urs = string.Empty;

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) Tipo = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi1);
                    if (!dr.IsDBNull(iEquicodi)) Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iComedfecha = dr.GetOrdinal(helper.Comedfecha);
                    if (!dr.IsDBNull(iComedfecha)) Fecha = dr.GetDateTime(iComedfecha);

                    int iUrsnomb = dr.GetOrdinal(helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) Urs = dr.GetString(iUrsnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) Grupo = dr.GetString(iGruponomb);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        CoMedicion48DTO entity = new CoMedicion48DTO();

                        entity.Famcodi = Tipo;
                        entity.Grupocodi = Grupocodi;
                        entity.Equicodi = Equicodi;
                        entity.Comedfecha = Fecha;
                        entity.Gruponomb = Grupo;
                        entity.Ursnomb = Urs;
                        entity.Valor = valor;
                        entity.Orden = i;

                        entitys.Add(entity);
                    }
                }
            }

            return entitys;
        }


        public List<CoMedicion48DTO> ObtenerDatosReservaEjecutadaFinal(int idVersion, DateTime fechaInicio, DateTime fechaFin, 
            int idTipoInfo)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerDatosReservaFinal, idVersion, idTipoInfo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int Tipo = 0;
                    int Grupocodi = 0;
                    int Equicodi = 0;
                    DateTime Fecha = DateTime.Now;
                    string Grupo = string.Empty;
                    string Urs = string.Empty;

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) Tipo = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi1);
                    if (!dr.IsDBNull(iEquicodi)) Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iComedfecha = dr.GetOrdinal(helper.Comedfecha);
                    if (!dr.IsDBNull(iComedfecha)) Fecha = dr.GetDateTime(iComedfecha);

                    int iUrsnomb = dr.GetOrdinal(helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) Urs = dr.GetString(iUrsnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) Grupo = dr.GetString(iGruponomb);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indicador = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        int indiceD = dr.GetOrdinal("T" + i);
                        if (!dr.IsDBNull(indiceD)) indicador = Convert.ToInt32(dr.GetInt32(indiceD));

                        CoMedicion48DTO entity = new CoMedicion48DTO();

                        entity.Famcodi = Tipo;
                        entity.Grupocodi = Grupocodi;
                        entity.Equicodi = Equicodi;
                        entity.Comedfecha = Fecha;
                        entity.Gruponomb = Grupo;
                        entity.Ursnomb = Urs;
                        entity.Valor = valor;
                        entity.Rsfindicador = indicador;
                        entity.Orden = i;

                        entitys.Add(entity);
                    }
                }
            }

            return entitys;
        }

        public List<CoMedicion48DTO> ObtenerListadoURS()
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerListadoURS);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi1);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));                   

                    int iUrsnomb = dr.GetOrdinal(helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);                   
                }
            }

            return entitys;
        }
        
        public List<CoMedicion48DTO> ObtenerPropiedadesHidraulica(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string sql = string.Format(helper.SqlObtenerPropiedadHidraulica, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iPmin = dr.GetOrdinal(helper.Pmin);
                    if (!dr.IsDBNull(iPmin)) entity.Pmin = dr.GetDecimal(iPmin);

                    int iPmax = dr.GetOrdinal(helper.Pmax);
                    if (!dr.IsDBNull(iPmax)) entity.Pmax = dr.GetDecimal(iPmax);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }



        public List<CoMedicion48DTO> ObtenerReporteBandas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string sql = string.Format(helper.SqlObtenerReporteBandas, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iTopfecha = dr.GetOrdinal(helper.Topfecha);
                    if (!dr.IsDBNull(iTopfecha)) entity.Topfecha = dr.GetDateTime(iTopfecha);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPmin = dr.GetOrdinal(helper.Pmin);
                    if (!dr.IsDBNull(iPmin)) entity.Pmin = dr.GetDecimal(iPmin);

                    int iPmax = dr.GetOrdinal(helper.Pmax);
                    if (!dr.IsDBNull(iPmax)) entity.Pmax = dr.GetDecimal(iPmax);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoMedicion48DTO> ObtenerPropiedadTermica(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string sql = string.Format(helper.SqlObtenerPropiedadTermica, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iPmin = dr.GetOrdinal(helper.Pmin);
                    if (!dr.IsDBNull(iPmin)) entity.Pmin = dr.GetDecimal(iPmin);

                    int iPmax = dr.GetOrdinal(helper.Pmax);
                    if (!dr.IsDBNull(iPmax)) entity.Pmax = dr.GetDecimal(iPmax);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
        public List<CoMedicion48DTO> ObtenerPropiedadPotenciaEfectiva(int idEquipo)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string sql = string.Format(helper.SqlObtenerPropiedadPotenciaEfectiva, idEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();                   

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));                    

                    int iPefec = dr.GetOrdinal(helper.Pefec);
                    if (!dr.IsDBNull(iPefec)) entity.Pefec = dr.GetDecimal(iPefec);
                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<CoMedicion48DTO> ObtenerPropiedadPotenciaMinima(int idEquipo)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string sql = string.Format(helper.SqlObtenerPropiedadPotenciaMinima, idEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iPefec = dr.GetOrdinal(helper.Pefec);
                    if (!dr.IsDBNull(iPefec)) entity.Pefec = dr.GetDecimal(iPefec);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoMedicion48DTO> ObtenerHorasOperacion(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            string sql = string.Format(helper.SqlObtenerHorasOperacion, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion48DTO entity = new CoMedicion48DTO();                  

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFecha = dr.GetOrdinal(helper.Fechaop);
                    if (!dr.IsDBNull(iFecha)) entity.Fechahop = dr.GetDateTime(iFecha);

                    int iHophorini = dr.GetOrdinal(helper.Hophorini);
                    if (!dr.IsDBNull(iHophorini)) entity.Hophorini = dr.GetDateTime(iHophorini);

                    int iHophorfin = dr.GetOrdinal(helper.Hophorfin);
                    if (!dr.IsDBNull(iHophorfin)) entity.Hophorfin = dr.GetDateTime(iHophorfin);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<CoMedicion48DTO> ObtenerReporteProgramadoFinal(int idVersion, int idTipoInformacion, DateTime fechaInicio, DateTime fechaFin, int idUrs)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerReporteProgramadoFinal, idVersion, idTipoInformacion,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idUrs);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int Tipo = 0;
                    int Grupocodi = 0;
                    int Equicodi = 0;
                    DateTime Fecha = DateTime.Now;
                    string Grupo = string.Empty;

                    int iComedtipo = dr.GetOrdinal(helper.Comedtipo);
                    if (!dr.IsDBNull(iComedtipo)) Tipo = Convert.ToInt32(dr.GetValue(iComedtipo));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iComedfecha = dr.GetOrdinal(helper.Comedfecha);
                    if (!dr.IsDBNull(iComedfecha)) Fecha = dr.GetDateTime(iComedfecha);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) Grupo = dr.GetString(iGruponomb);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        CoMedicion48DTO entity = new CoMedicion48DTO();

                        entity.Comedtipo = Tipo;
                        entity.Grupocodi = Grupocodi;
                        entity.Equicodi = Equicodi;
                        entity.Comedfecha = Fecha;
                        entity.Gruponomb = Grupo;
                        entity.Valor = valor;
                        entity.Orden = i;

                        entitys.Add(entity);
                    }
                }
            }

            return entitys;            
        }

        public List<CoMedicion48DTO> ObtenerReporteReservaFinal(int idVersion, int idTipoInformacion, DateTime fechaInicio, DateTime fechaFin, int idUrs)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerReporteReservaFinal, idVersion, idTipoInformacion,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idUrs);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int Tipo = 0;
                    int Grupocodi = 0;
                    int Equicodi = 0;
                    DateTime Fecha = DateTime.Now;
                    string Grupo = string.Empty;
                    string Urs = string.Empty;

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) Tipo = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi1);
                    if (!dr.IsDBNull(iEquicodi)) Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iComedfecha = dr.GetOrdinal(helper.Comedfecha);
                    if (!dr.IsDBNull(iComedfecha)) Fecha = dr.GetDateTime(iComedfecha);

                    int iUrsnomb = dr.GetOrdinal(helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) Urs = dr.GetString(iUrsnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) Grupo = dr.GetString(iGruponomb);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        CoMedicion48DTO entity = new CoMedicion48DTO();

                        entity.Famcodi = Tipo;
                        entity.Grupocodi = Grupocodi;
                        entity.Equicodi = Equicodi;
                        entity.Comedfecha = Fecha;
                        entity.Gruponomb = Grupo;
                        entity.Ursnomb = Urs;
                        entity.Valor = valor;
                        entity.Orden = i;

                        entitys.Add(entity);
                    }
                }
            }

            return entitys;
        }

        public List<CoMedicion48DTO> ObtenerReporteDespachoFinal(int idVersion, int idTipoInformacion, DateTime fechaInicio, DateTime fechaFin, int idUrs)
        {
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerReporteDespachoFinal, idVersion, idTipoInformacion,
                 fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idUrs);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int Tipo = 0;
                    int Grupocodi = 0;
                    int Equicodi = 0;
                    DateTime Fecha = DateTime.Now;
                    string Grupo = string.Empty;
                    string Urs = string.Empty;

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) Tipo = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi1);
                    if (!dr.IsDBNull(iEquicodi)) Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iComedfecha = dr.GetOrdinal(helper.Comedfecha);
                    if (!dr.IsDBNull(iComedfecha)) Fecha = dr.GetDateTime(iComedfecha);

                    int iUrsnomb = dr.GetOrdinal(helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) Urs = dr.GetString(iUrsnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) Grupo = dr.GetString(iGruponomb);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        int color = 0;
                        int indiceColor = dr.GetOrdinal("T" + i);
                        if (!dr.IsDBNull(indiceColor)) color = Convert.ToInt32(dr.GetValue(indiceColor));

                        CoMedicion48DTO entity = new CoMedicion48DTO();

                        entity.Famcodi = Tipo;
                        entity.Grupocodi = Grupocodi;
                        entity.Equicodi = Equicodi;
                        entity.Comedfecha = Fecha;
                        entity.Gruponomb = Grupo;
                        entity.Ursnomb = Urs;
                        entity.Valor = valor;
                        entity.Color = color;
                        entity.Orden = i;

                        entitys.Add(entity);
                    }
                }
            }

            return entitys;
        }

        public void EliminarLiquidacionFinal(int recacodi)
        {
            string query = string.Format(helper.SqlEliminarLiquidacion, recacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<Dominio.DTO.Transferencias.VcrDespachoursDTO> ObtenerLiquidacionResultadoFinal(int idVersion, 
            int idTipoInfo, int recacodi, string usuario)
        {
            List<Dominio.DTO.Transferencias.VcrDespachoursDTO> entitys = new List<Dominio.DTO.Transferencias.VcrDespachoursDTO>();

            string query = string.Format(helper.SqlObtenerLiquidacionResultadoFinal, idVersion, idTipoInfo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                  
                    int Grupocodi = 0;
                    int Equicodi = 0;
                    DateTime Fecha = DateTime.Now;
                    string Grupo = string.Empty;                    
                    int Empresa = 0;                    

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi1);
                    if (!dr.IsDBNull(iEquicodi)) Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iComedfecha = dr.GetOrdinal(helper.Comedfecha);
                    if (!dr.IsDBNull(iComedfecha)) Fecha = dr.GetDateTime(iComedfecha);                    

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) Grupo = dr.GetString(iGruponomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) Empresa = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        Dominio.DTO.Transferencias.VcrDespachoursDTO entity = new Dominio.DTO.Transferencias.VcrDespachoursDTO();
                        
                        entity.Grupocodi = Grupocodi;
                        entity.Equicodi = Equicodi;
                        entity.Emprcodi = Empresa;
                        entity.Vcdursfecha = Fecha.AddMinutes((i - 1) * 30);
                        entity.Vcdursdespacho = valor;
                        entity.Gruponomb = Grupo;
                        entity.Vcdurstipo = (idTipoInfo == 11) ? "C" : "S";
                        entity.Vcrecacodi = recacodi;
                        entity.Vcdursusucreacion = usuario;
                        entity.Vcdursfeccreacion = DateTime.Now;
                        
                        entitys.Add(entity);
                    }
                }
            }

            return entitys;
        }
    }
}
