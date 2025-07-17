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
    /// Clase de acceso a datos de la tabla ME_MEDICION48
    /// </summary>
    public class MeMedicion48Repository : RepositoryBase, IMeMedicion48Repository
    {
        public MeMedicion48Repository(string strConn)
            : base(strConn)
        {
        } 

        MeMedicion48Helper helper = new MeMedicion48Helper();

        public void Save(MeMedicion48DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
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
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeMedicion48DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);

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

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeMedicion48DTO GetById(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            MeMedicion48DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeMedicion48DTO> List()
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
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

        public List<MeMedicion48DTO> GetByCriteria()
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
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

        public List<MeMedicion48DTO> ObtenerGeneracionRER(int idEmpresa, int lectCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            string query = String.Format(helper.SqlGetGeneracionRER, lectCodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int EliminarValoresCargadosPorPunto(List<int> ptos, int lectcodi, DateTime fechaInicio, DateTime fechaFIn)
        {
            int contador = 0;

            string puntos = string.Join<int>(",", ptos);
            string query = string.Format(helper.SqlObtenerGeneracionRERPorPunto, puntos, lectcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFIn.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                contador = Convert.ToInt32(result);

                if (contador > 0)
                {
                    query = String.Format(helper.SqlDeleteGeneracionRERPorPunto, puntos, lectcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                            fechaFIn.ToString(ConstantesBase.FormatoFecha));
                    command = dbProvider.GetSqlStringCommand(query);
                    dbProvider.ExecuteNonQuery(command);
                }
            }

            return contador;
        }

        public int ObtenerEmpresaPorPuntoMedicion(int ptoMedicion)
        {
            string query = String.Format(helper.SqlObtenerEmpresaPtoMedicion, ptoMedicion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public int ObtenerNroRegistrosEjecutado(DateTime fechaInicial, DateTime fechaFinal, string empresas, string tiposGeneracion)
        {
            String query = String.Format(this.helper.SqlObtenerNroRegistrosEjecutado, empresas, tiposGeneracion,
                   fechaInicial.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<MeMedicion48DTO> ObtenerConsultaEjecutado(DateTime fechaInicial, DateTime fechaFinal, string empresas,
            string tiposGeneracion, int nroPagina, int nroRegistros)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            String sql = String.Format(this.helper.SqlObtenerConsultaEjecutado, empresas, tiposGeneracion,
                fechaInicial.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPagina, nroRegistros);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerTotalConsultaEjecutado(DateTime fechaInicial, DateTime fechaFinal, string empresas,
            string tiposGeneracion)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            String sql = String.Format(helper.SqlObtenerTotalConsultaEjecutado, empresas, tiposGeneracion,
                fechaInicial.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    decimal totalValue = 0;
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valorSuma = 0;

                        int iSuma = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iSuma)) valorSuma = dr.GetDecimal(iSuma) / 4;
                        entity.GetType().GetProperty("H" + i).SetValue(entity, valorSuma);
                        totalValue = totalValue + valorSuma;
                    }

                    entity.Meditotal = totalValue;
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerExportacionConsultaEjecutado(DateTime fechaInicial, DateTime fechaFinal,
            string empresas, string tiposGeneracion)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            String sql = String.Format(this.helper.SqlObtenerExportacionConsultaEjecutado, empresas, tiposGeneracion,
                fechaInicial.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);



                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerConsolidadoEjecutado(DateTime fechaInicial, DateTime fechaFinal,
            string empresas, string fuentesEnergia)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            String query = String.Format(this.helper.SqlObtenerEjecutadoConsolidado, empresas, fuentesEnergia,
                       fechaInicial.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = this.helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenergabrev = dr.GetOrdinal(this.helper.Fenergabrev);
                    if (!dr.IsDBNull(iFenergabrev)) entity.Fenergabrev = dr.GetString(iFenergabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerConsultaCMgRealPorArea(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            String query = String.Format(this.helper.SqlObtenerCmgRealPorArea,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = this.helper.Create(dr);

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iAnio = dr.GetOrdinal(helper.Anio);
                    if (!dr.IsDBNull(iAnio)) entity.Anio = dr.GetString(iAnio);

                    int iMes = dr.GetOrdinal(helper.Mes);
                    if (!dr.IsDBNull(iMes)) entity.Mes = dr.GetString(iMes);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        /// <summary>
        /// Borra las mediciones enviadas en un archivo
        /// </summary>
        /// <param name="medifecha"></param>
        public void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioArchivo, idLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
              fechaFin.ToString(ConstantesBase.FormatoFecha), idFormato, idEmpresa);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicion48DTO> GetEnvioArchivo(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin, int lectocodi = -1)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivo, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), lectocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener data segun me_hoja
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetEnvioArchivo2(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivo2, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);
                    int iHojacodi = dr.GetOrdinal(this.helper.Hojacodi);
                    if (!dr.IsDBNull(iHojacodi)) entity.Hojacodi = Convert.ToInt32(dr.GetValue(iHojacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> GetByPtoMedicion(int ptomedicodi)
        {
            var entitys = new List<MeMedicion48DTO>();
            string queryString = string.Format(helper.SqlGetByPtoMedicion, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> GetInterconexiones(int idLectura, int idOrigenLectura, string ptomedicodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string sqlQuery = string.Format(helper.SqlGetInterconexiones, idLectura, idOrigenLectura, ptomedicodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTipoptomedinomb = dr.GetOrdinal(helper.Tipoptomedinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    // Siosein2 28/03/2019 Levantamiento Obs Mape
                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iBarrcodi = dr.GetOrdinal(this.helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerGeneracionPorEmpresa(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerGeneracionPorEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerGeneracionPorEmpresaTipoGeneracion(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerGeneracionPorEmpresaTipoGeneracion, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerGeneracionPorEmpresaTipoGeneracionMovil(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerGeneracionPorEmpresaTipoGeneracionMovil, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iTgenercolor = dr.GetOrdinal(helper.Tgenercolor);
                    if (!dr.IsDBNull(iTgenercolor)) entity.Tgenercolor = dr.GetString(iTgenercolor);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iTipogenerrer = dr.GetOrdinal(helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal ObtenerGeneracionAcumuladaAnual(DateTime fecha)
        {
            string query = String.Format(helper.SqlObtenerGeneracionAcumuladaAnual, fecha.Year, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToDecimal(result);
            }

            return 0;
        }

        public List<MeMedicion48DTO> ObtenerConsultaWebReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = String.Format(helper.SqlObtenerConsultaWebReporte, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iTegenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTegenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTegenercodi));

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerDemandaPortalWeb(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlDemandaDespachoOfertaNCP, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iLectcodi = dr.GetOrdinal(helper.Lectcodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerDemandaPortalWebTipo(int lectcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerDemandaPortalWebTipo, lectcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iLectcodi = dr.GetOrdinal(helper.Lectcodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<MeMedicion48DTO> ObtenerDemandaPicoDiaAnterior(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerDemandaPicoDiaAnterior, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iLectcodi = dr.GetOrdinal(helper.Lectcodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

                    int iOrdinal = dr.GetOrdinal("H" + 1);
                    if (!dr.IsDBNull(iOrdinal)) entity.H1 = dr.GetDecimal(iOrdinal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerConsultaDemandaBarras(int lectcodi, string empresas, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerConsultaDemandaBarras, lectcodi, empresas,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerDatosHidrologiaPortal(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerDatosHidrologiaPortal, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedinomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedinomb = dr.GetString(iPtomedinomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoinfodesc = dr.GetOrdinal(helper.Tipoinfodesc);
                    if (!dr.IsDBNull(iTipoinfodesc)) entity.Tipoinfodesc = dr.GetString(iTipoinfodesc).Trim();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> SqlObtenerDatosEjecutado(DateTime fecha)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerDatosEjecutado, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    entity = helper.Create(dr);
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<MeMedicion48DTO> GetListReporteInformacion30min(int formato, string inicio, string lectCodiPR16, string empresas, string tipos, string fechaIni, string periodoSicli, string lectCodiAlpha, int regIni, int regFin)
        {
            string qEmpresas = " 1 = 1";
            string qTipoEmpresas = "AND 1 = 1";
            string qpaginado = "";

            if (empresas != "" && empresas != null)
            {
                qEmpresas = " empr.EMPRCODI IN (" + empresas + ") ";
            }
            if (tipos != "")
            {
                qTipoEmpresas = " AND empr.TIPOEMPRCODI = " + tipos + " ";
            }

            if (regIni != 0 && regFin != 0)
            {
                qpaginado = "WHERE exte.item >=" + regIni + " AND exte.item <=" + regFin;
            }

            string sqlQuery = string.Format(this.helper.SqlListReporteInformacion
                                          , formato
                                          , inicio
                                          , lectCodiPR16
                                          , qEmpresas
                                          , qTipoEmpresas
                                          , fechaIni
                                          , periodoSicli
                                          , lectCodiAlpha
                                          , qpaginado
                                          , tipos);
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iItem = dr.GetOrdinal(helper.Item);
                    if (!dr.IsDBNull(iItem)) entity.Item = Convert.ToInt32(dr.GetValue(iItem));

                    int iPeriodo = dr.GetOrdinal(helper.Periodo);
                    if (!dr.IsDBNull(iPeriodo)) entity.Periodo = dr.GetString(iPeriodo);

                    int iFuente = dr.GetOrdinal(helper.Fuente);
                    if (!dr.IsDBNull(iFuente)) entity.Fuente = dr.GetString(iFuente);

                    int iFechaFila = dr.GetOrdinal(helper.FechaFila);
                    if (!dr.IsDBNull(iFechaFila)) entity.FechaFila = dr.GetDateTime(iFechaFila);

                    int iCumplimiento = dr.GetOrdinal(helper.Cumplimiento);
                    if (!dr.IsDBNull(iCumplimiento)) entity.Cumplimiento = dr.GetString(iCumplimiento);

                    int iCodigoCliente = dr.GetOrdinal(helper.CodigoCliente);
                    if (!dr.IsDBNull(iCodigoCliente)) entity.CodigoCliente = dr.GetString(iCodigoCliente);

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iEmprCodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iRucEmpresa = dr.GetOrdinal(helper.RucEmpresa);
                    if (!dr.IsDBNull(iRucEmpresa)) entity.RucEmpresa = dr.GetString(iRucEmpresa);

                    int iNombreEmpresa = dr.GetOrdinal(helper.NombreEmpresa);
                    if (!dr.IsDBNull(iNombreEmpresa)) entity.NombreEmpresa = dr.GetString(iNombreEmpresa);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = dr.GetString(iTension);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iNroEnvios = dr.GetOrdinal(helper.NroEnvios);
                    if (!dr.IsDBNull(iNroEnvios)) entity.NroEnvios = Convert.ToInt32(dr.GetValue(iNroEnvios));

                    int iFechaPrimerEnvio = dr.GetOrdinal(helper.FechaPrimerEnvio);
                    if (!dr.IsDBNull(iFechaPrimerEnvio)) entity.FechaPrimerEnvio = dr.GetDateTime(iFechaPrimerEnvio);

                    int iFechaUltimoEnvio = dr.GetOrdinal(helper.FechaUltimoEnvio);
                    if (!dr.IsDBNull(iFechaUltimoEnvio)) entity.FechaUltimoEnvio = dr.GetDateTime(iFechaUltimoEnvio);

                    int iIniRemision = dr.GetOrdinal(helper.IniRemision);
                    if (!dr.IsDBNull(iIniRemision)) entity.IniRemision = dr.GetDateTime(iIniRemision);

                    int iFinRemision = dr.GetOrdinal(helper.FinRemision);
                    if (!dr.IsDBNull(iFinRemision)) entity.FinRemision = dr.GetDateTime(iFinRemision);

                    int iIniPeriodo = dr.GetOrdinal(helper.IniPeriodo);
                    if (!dr.IsDBNull(iIniPeriodo)) entity.IniPeriodo = dr.GetDateTime(iIniPeriodo);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int GetListReporteInformacion30minCount(int formato, string inicio, string lectCodiPR16, string empresas, string tipos, string fechaIni, string periodoSicli, string lectCodiAlpha)
        {
            string qEmpresas = " 1 = 1";
            string qTipoEmpresas = "AND 1 = 1";

            if (empresas != "" && empresas != null)
            {
                qEmpresas = " empr.EMPRCODI IN (" + empresas + ") ";
            }
            if (tipos != "")
            {
                qTipoEmpresas = " AND empr.TIPOEMPRCODI = " + tipos + " ";
            }
            //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
            //string sqlQuery = string.Format(this.helper.SqlListReporteInformacionCount, formato, inicio, lectCodiPR16, qEmpresas, qTipoEmpresas, fechaIni, fechaFin, lectCodiAlpha);
            string sqlQuery = string.Format(this.helper.SqlListReporteInformacionCount
                                          , formato
                                          , inicio
                                          , lectCodiPR16
                                          , qEmpresas
                                          , qTipoEmpresas
                                          , fechaIni
                                          , periodoSicli
                                          , lectCodiAlpha
                                          , tipos);
            //- HDT Fin

            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            int cant = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iQregistros = dr.GetOrdinal(helper.Qregistros);
                    cant = Convert.ToInt32(dr.GetValue(iQregistros));
                }
            }
            return cant;
        }

        public List<MeMedicion48DTO> ObtenerDatosEquipoLectura(string equicodi, int lectcodi, string fecha)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerDatosEquipoLectura, equicodi, lectcodi, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iGrupotipo = dr.GetOrdinal(helper.Grupotipo);
                    if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerDatosPtoMedicionLectura(string ptomedicodi, int lectcodi, string fecha)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerDatosPtoMedicionLectura, ptomedicodi, lectcodi, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iGrupotipo = dr.GetOrdinal(helper.Grupotipo);
                    if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerDatosPtoMedicionLectura(string ptomedicodi, int lectcodi, int tipoinfocodi, string fecha)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerDatosPtoMedicionLecturaInfo, ptomedicodi, lectcodi, tipoinfocodi, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iGrupotipo = dr.GetOrdinal(helper.Grupotipo);
                    if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> GetDespachoProgramado(DateTime fecha, int lectcodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlGetDespachoProgramado, lectcodi, fecha.ToString(ConstantesBase.FormatoFecha), fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerListaMedicion48Ptomedicion(DateTime fechaInicio, DateTime fechaFin, string tipoinformacion, string lectocodi, string emprecodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            string sqlQuery = string.Format(helper.SqlObtenerListaMedicion48Ptomedicion, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), tipoinformacion, lectocodi, emprecodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #region "COSTO OPORTUNIDAD"
        public List<MeMedicion48DTO> GetReservaProgramado(DateTime fecha, int lectcodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlGetReservaProgramado, lectcodi, fecha.ToString(ConstantesBase.FormatoFecha), fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iGrupourspadre = dr.GetOrdinal(helper.Grupourspadre);
                    if (!dr.IsDBNull(iGrupourspadre)) entity.Grupourspadre = dr.GetInt32(iGrupourspadre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<MeMedicion48DTO> GetListadoReserva(DateTime fecha, int lectcodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlGetListadoReserva, lectcodi, fecha.ToString(ConstantesBase.FormatoFecha), fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    entity = helper.Create(dr);
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        public List<MeMedicion48DTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, string lectcodi, int tipoinfocodi, string ptomedicodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            string sql = String.Format(helper.SqlObtenerMedicion48, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), lectcodi, tipoinfocodi, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    var entity = helper.Create(dr);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iMaxDemanda = dr.GetOrdinal(helper.MaxDemanda);
                    if (!dr.IsDBNull(iMaxDemanda)) entity.MaxDemanda = dr.GetDecimal(iMaxDemanda);

                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int SaveMeMedicion48Id(MeMedicion48DTO entity)
        {
            try
            {
                Delete(entity.Lectcodi, entity.Medifecha, entity.Tipoinfocodi, entity.Ptomedicodi);
                Save(entity);
                return 1;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<MeMedicion48DTO> BuscarOperaciones(int lectcodi, int tipoinfocodi, int ptomedicodi, DateTime medifecha, DateTime lastdate, int nroPage, int pageSize)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            String sql = String.Format(this.helper.ObtenerListado, lectcodi, tipoinfocodi, ptomedicodi, medifecha.ToString(ConstantesBase.FormatoFecha),
                lastdate.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iLectcodi = dr.GetOrdinal(this.helper.Lectcodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

                    int iMedifecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iTipoinfocodi = dr.GetOrdinal(this.helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iMeditotal = dr.GetOrdinal(this.helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iMediestado = dr.GetOrdinal(this.helper.Mediestado);
                    if (!dr.IsDBNull(iMediestado)) entity.Mediestado = dr.GetString(iMediestado);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iLectnomb = dr.GetOrdinal(this.helper.Lectnomb);
                    if (!dr.IsDBNull(iLectnomb)) entity.Lectnomb = dr.GetString(iLectnomb);

                    int iTipoinfoabrev = dr.GetOrdinal(this.helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iPtomedidesc = dr.GetOrdinal(this.helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int lectcodi, int tipoinfocodi, int ptomedicodi, DateTime medifecha, DateTime lastdate)
        {
            String sql = String.Format(this.helper.TotalRegistros, lectcodi, tipoinfocodi, ptomedicodi, medifecha.ToString(ConstantesBase.FormatoFecha), lastdate.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        #region PR5

        public List<MeMedicion48DTO> ListarMeMedicion48ByFiltro(string lectcodi, DateTime fechaini, DateTime fechafin, string ptomedicodis, string tipoinfocodi = "-1")
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            string sqlQuery = string.Format(this.helper.SqlListarMeMedicion48ByFiltro, lectcodi,
                fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha), ptomedicodis, tipoinfocodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);

                    int iTipoptomedicodi = dr.GetOrdinal(helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));
                    
                    for (int i = 1; i <= 48; i++)
                    {
                        int iE = dr.GetOrdinal("E" + i);
                        int valor = -1;
                        if (!dr.IsDBNull(iE)) valor = Convert.ToInt32(dr.GetValue(iE)); 
                        entity.GetType().GetProperty("T" + i).SetValue(entity, valor);
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> GetConsolidadoMaximaDemanda48SinGrupoIntegrante(int tipoCentral, string tipoGeneracion, DateTime fechaIni, DateTime fechaFin, string idEmpresa, int lectcodi, int tipoinfocodi, int tptomedicodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            string query = String.Format(this.helper.SqlListarDetalleGeneracion48, tipoCentral, tipoGeneracion
                , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    entity = helper.Create(dr);

                    int itipogrupocodi = dr.GetOrdinal(helper.Tipogrupocodi);
                    if (!dr.IsDBNull(itipogrupocodi)) entity.Tipogrupocodi = dr.GetInt32(itipogrupocodi);
                    int iTipogenerrer = dr.GetOrdinal(this.helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);
                    int iGrupointegrante = dr.GetOrdinal(this.helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);
                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt16(dr.GetValue(iTgenercodi));
                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iEquinomb)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprorden = dr.GetOrdinal(this.helper.Emprorden);
                    if (!dr.IsDBNull(iEmprorden)) entity.Emprorden = Convert.ToInt32(dr.GetValue(iEmprorden));
                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);
                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));
                    int iGrupoabrev = dr.GetOrdinal(this.helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupoorden = dr.GetOrdinal(helper.Grupoorden);
                    if (!dr.IsDBNull(iGrupoorden)) entity.Grupoorden = Convert.ToInt32(dr.GetValue(iGrupoorden));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedinomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedinomb = dr.GetString(iPtomedinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Indisponibilidaes 
        public List<MeMedicion48DTO> GetIndisponibilidadesMedicion48Cuadro5(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string sqlQuery = string.Format(helper.SqlIndisponibilidadesMedicion48Cuadro5, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (
                IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iMediFecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMediFecha)) entity.Medifecha = dr.GetDateTime(iMediFecha);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    entity.Equipadre = dr.GetInt32(dr.GetOrdinal("centralid"));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<MeMedicion48DTO> GetIndisponibilidadesMedicion48Cuadro2(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string sqlQuery = string.Format(helper.SqlIndisponibilidadesMedicion48Cuadro2, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (
                IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iMediFecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMediFecha)) entity.Medifecha = dr.GetDateTime(iMediFecha);
                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);
                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);
                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);
                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);
                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);
                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);
                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);
                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);
                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);
                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);
                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);
                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);
                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);
                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region SIOSEIN

        public List<MeMedicion48DTO> GetCostosMarginalesProgPorRangoFechaStaRosa(DateTime fechaI, DateTime fechaF, int lectcodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlGetCostosMarginalesProgPorRangoFechaStaRosa, lectcodi, fechaI.ToString(ConstantesBase.FormatoFecha), fechaF.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iMediFecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMediFecha)) entity.Medifecha = dr.GetDateTime(iMediFecha);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    string stTcambio = string.Empty;
                    int iTipocambio = dr.GetOrdinal(helper.Tipocambio);
                    if (!dr.IsDBNull(iTipocambio)) stTcambio = dr.GetString(iTipocambio);

                    decimal dtcambio = 0;
                    if (decimal.TryParse(stTcambio, out dtcambio))
                        entity.Tipocambio = dtcambio;
                    else
                        entity.Tipocambio = 0;
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region MigracionSGOCOES-GrupoB

        public void SaveMemedicion48masivo(List<MeMedicion48DTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Lectcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Medifecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Tipoinfocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.H1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Meditotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Mediestado, DbType.String);
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
            dbProvider.AddColumnMapping(helper.Lastuser, DbType.String);
            dbProvider.AddColumnMapping(helper.Lastdate, DbType.DateTime);

            dbProvider.BulkInsert<MeMedicion48DTO>(entitys, helper.TableName);
        }

        public void DeleteMasivo(int lectcodi, DateTime medifecha, string tipoinfocodi, string ptomedicodi)
        {
            string query = string.Format(helper.SqlDeleteMasivo, lectcodi, medifecha.ToString(ConstantesBase.FormatoFecha), ptomedicodi, tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicion48DTO> RptCmgCortoPlazo(string lectcodi, int tipoinfocodi, int ptomedicodi, DateTime fecha1, DateTime fecha2)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlRptCmgCortoPlazo, lectcodi, tipoinfocodi, ptomedicodi, fecha1.ToString(ConstantesBase.FormatoFecha), fecha2.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedinomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedinomb = dr.GetString(iPtomedinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> GetListaMedicion48xlectcodi(int lectcodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlGetListaMedicion48xlectcodi, lectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new MeMedicion48DTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> GetListaDemandaBarras(string ptomedicodi, string lectcodi, DateTime fecInicio, DateTime fecFin)
        {
            var entitys = new List<MeMedicion48DTO>();
            var query = string.Format(helper.SqlGetListaDemandaBarras, ptomedicodi, lectcodi, fecInicio.ToString(ConstantesBase.FormatoFecha), fecFin.ToString(ConstantesBase.FormatoFecha));

            using (var command = dbProvider.GetSqlStringCommand(query))
            {

                using (var dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        var entity = helper.Create(dr);

                        //int iPtomedinomb = dr.GetOrdinal(helper.Ptomedinomb);
                        //if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedinomb = dr.GetString(iPtomedinomb);

                        entitys.Add(entity);
                    }
                }

            }

            return entitys;
        }

        #endregion

        #region FIT - SGOCOES Grupo A - Soporte

        public void DeleteSCOActualizacion()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteSCOActualizacion);
            dbProvider.ExecuteNonQuery(command);
        }

        #endregion

        public List<MeMedicion48DTO> ObtenerProgramaReProgramaDia(DateTime fecha, int origlectcodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string sql = string.Format(helper.SqlObtenerProgramaReProgramaDia, fecha.ToString(ConstantesBase.FormatoFecha), origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    entitys.Add(entity);
                }
            }


            return entitys;
        }

        #region Fit - VALORIZACION DIARIA
        public MeMedicion48DTO ObtenerIntervaloPuntaMes(DateTime fechaInicio, DateTime fechaFin)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGetIntervaloPuntaMes, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    int iMedifecha = dr.GetOrdinal("MEDIFECHA");
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);
                    int iH1 = dr.GetOrdinal("H1");
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal("H2");
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal("H3");
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal("H4");
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal("H5");
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal("H6");
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal("H7");
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal("H8");
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal("H9");
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal("H10");
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal("H11");
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal("H12");
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal("H13");
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal("H14");
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal("H15");
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal("H16");
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal("H17");
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal("H18");
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal("H19");
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal("H20");
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal("H21");
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal("H22");
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal("H23");
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal("H24");
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal("H25");
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal("H26");
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal("H27");
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal("H28");
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal("H29");
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal("H30");
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal("H31");
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal("H32");
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal("H33");
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal("H34");
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal("H35");
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal("H36");
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal("H37");
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal("H38");
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal("H39");
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal("H40");
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal("H41");
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal("H42");
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal("H43");
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal("H44");
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal("H45");
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal("H46");
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal("H47");
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal("H48");
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    return entity;
                }
            }

            return null;
        }
        #endregion

        #region Numerales Datos Base
        public List<MeMedicion48DTO> ListaNumerales_DatosBase_5_8_4(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_8_4, fechaIni, fechaFin);

            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public List<MeMedicion48DTO> ListaNumerales_DatosBase_5_8_5(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_8_5, fechaIni, fechaFin);

            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion48DTO> ListaNumerales_DatosBase_5_9_1(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_9_1, fechaIni, fechaFin);

            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetDecimal(iValor);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion48DTO> ListaNumerales_DatosBase_5_9_2(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_9_2, fechaIni, fechaFin);

            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetDecimal(iValor);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion48DTO> ListaNumerales_DatosBase_5_9_3(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_9_3, fechaIni, fechaFin);

            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetDecimal(iValor);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion48DTO> ListaMedUsuariosLibres(DateTime fechaIni, DateTime fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlListaMedUsuariosLibres, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    helper.GetH1To48(dr, entity);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iLectcodi = dr.GetOrdinal(helper.Lectcodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = dr.GetInt32(iLectcodi);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerConsultaMedidores(DateTime fechaIni, DateTime fechaFin, int nroPagina, int nroRegistros)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            String sql = String.Format(this.helper.SqlObtenerConsultaTipoGeneracion, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                nroPagina, nroRegistros);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerConsultaMedidores(DateTime fechaIni, DateTime fechaFin, int nroPagina, object nroRegistros)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            String sql = String.Format(this.helper.SqlObtenerConsultaTipoGeneracion, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                nroPagina, nroRegistros);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroElementosConsultaMedidores(DateTime fechaInicio, DateTime fechaFin)
        {
            string query = String.Format(this.helper.SqlNroRegistrosConsultasTipoGeneracion, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<MeMedicion48DTO> ObtenerConsultaMedidores(DateTime fechaIni, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            String sql = String.Format(this.helper.SqlObtenerConsultaTipoGeneracion, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        public List<MeMedicion48DTO> ObtenerDatosDespachoComparativo(DateTime fechaInicio, DateTime fechaFin, string puntos, int lectcodi, int tipoinfocodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerDatosDespachoComparativo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), puntos,
                lectcodi, tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    this.helper.GetH1To48(dr, entity);
                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        //Yupana Continuo
        public List<MeMedicion48DTO> ObtenerAportesHidro(DateTime fechaIni, DateTime fechaFin, int idEscenario, int idlecturaHidro)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string queryString = string.Format(helper.SqlObtenerAportesHidro, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEscenario, idlecturaHidro);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            var entity = new MeMedicion48DTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iRecurcodi = dr.GetOrdinal(helper.Recurcodi);
                    if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = dr.GetInt32(iRecurcodi);
                    int iRecptok = dr.GetOrdinal(helper.Recptok);
                    if (!dr.IsDBNull(iRecptok)) entity.Recptok = dr.GetInt32(iRecptok);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Mejoras RDO
        public void SaveMeMedicion48Ejecutados(MeMedicion48DTO entity, int idEnvio, string usuario, int idEmpresa)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveEjecutados48);
            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, idEnvio);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
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
            dbProvider.AddInParameter(command, helper.E1, DbType.String, entity.E1);
            dbProvider.AddInParameter(command, helper.E2, DbType.String, entity.E2);
            dbProvider.AddInParameter(command, helper.E3, DbType.String, entity.E3);
            dbProvider.AddInParameter(command, helper.E4, DbType.String, entity.E4);
            dbProvider.AddInParameter(command, helper.E5, DbType.String, entity.E5);
            dbProvider.AddInParameter(command, helper.E6, DbType.String, entity.E6);
            dbProvider.AddInParameter(command, helper.E7, DbType.String, entity.E7);
            dbProvider.AddInParameter(command, helper.E8, DbType.String, entity.E8);
            dbProvider.AddInParameter(command, helper.E9, DbType.String, entity.E9);
            dbProvider.AddInParameter(command, helper.E10, DbType.String, entity.E10);
            dbProvider.AddInParameter(command, helper.E11, DbType.String, entity.E11);
            dbProvider.AddInParameter(command, helper.E12, DbType.String, entity.E12);
            dbProvider.AddInParameter(command, helper.E13, DbType.String, entity.E13);
            dbProvider.AddInParameter(command, helper.E14, DbType.String, entity.E14);
            dbProvider.AddInParameter(command, helper.E15, DbType.String, entity.E15);
            dbProvider.AddInParameter(command, helper.E16, DbType.String, entity.E16);
            dbProvider.AddInParameter(command, helper.E17, DbType.String, entity.E17);
            dbProvider.AddInParameter(command, helper.E18, DbType.String, entity.E18);
            dbProvider.AddInParameter(command, helper.E19, DbType.String, entity.E19);
            dbProvider.AddInParameter(command, helper.E20, DbType.String, entity.E20);
            dbProvider.AddInParameter(command, helper.E21, DbType.String, entity.E21);
            dbProvider.AddInParameter(command, helper.E22, DbType.String, entity.E22);
            dbProvider.AddInParameter(command, helper.E23, DbType.String, entity.E23);
            dbProvider.AddInParameter(command, helper.E24, DbType.String, entity.E24);
            dbProvider.AddInParameter(command, helper.E25, DbType.String, entity.E25);
            dbProvider.AddInParameter(command, helper.E26, DbType.String, entity.E26);
            dbProvider.AddInParameter(command, helper.E27, DbType.String, entity.E27);
            dbProvider.AddInParameter(command, helper.E28, DbType.String, entity.E28);
            dbProvider.AddInParameter(command, helper.E29, DbType.String, entity.E29);
            dbProvider.AddInParameter(command, helper.E30, DbType.String, entity.E30);
            dbProvider.AddInParameter(command, helper.E31, DbType.String, entity.E31);
            dbProvider.AddInParameter(command, helper.E32, DbType.String, entity.E32);
            dbProvider.AddInParameter(command, helper.E33, DbType.String, entity.E33);
            dbProvider.AddInParameter(command, helper.E34, DbType.String, entity.E34);
            dbProvider.AddInParameter(command, helper.E35, DbType.String, entity.E35);
            dbProvider.AddInParameter(command, helper.E36, DbType.String, entity.E36);
            dbProvider.AddInParameter(command, helper.E37, DbType.String, entity.E37);
            dbProvider.AddInParameter(command, helper.E38, DbType.String, entity.E38);
            dbProvider.AddInParameter(command, helper.E39, DbType.String, entity.E39);
            dbProvider.AddInParameter(command, helper.E40, DbType.String, entity.E40);
            dbProvider.AddInParameter(command, helper.E41, DbType.String, entity.E41);
            dbProvider.AddInParameter(command, helper.E42, DbType.String, entity.E42);
            dbProvider.AddInParameter(command, helper.E43, DbType.String, entity.E43);
            dbProvider.AddInParameter(command, helper.E44, DbType.String, entity.E44);
            dbProvider.AddInParameter(command, helper.E45, DbType.String, entity.E45);
            dbProvider.AddInParameter(command, helper.E46, DbType.String, entity.E46);
            dbProvider.AddInParameter(command, helper.E47, DbType.String, entity.E47);
            dbProvider.AddInParameter(command, helper.E48, DbType.String, entity.E48);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }
        public List<MeMedicion48DTO> ObtenerGeneracionRERNC(int idEmpresa, int lectCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>(); string query = String.Format(helper.SqlGetGeneracionRERNC, lectCodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
            fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa); DbCommand command = dbProvider.GetSqlStringCommand(query); using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }
        public List<MeMedicion48DTO> GetEnvioArchivoEjecutados(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin, int lectocodi, string horario)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivoEjecutados, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), lectocodi, horario != null ? Convert.ToInt32(horario) : 0);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEjecutados(dr));
                }
            }

            return entitys;
        }
        public void SaveMeMedicion48Intranet(MeMedicion48DTO entity, int idEnvio)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveIntranet);
            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, idEnvio);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
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
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }
        public List<MeMedicion48DTO> GetEnvioMeMedicion48Intranet(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin, int Lectcodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string queryString = string.Format(helper.SqlGetEnvioMeMedicion48Intranet, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), Lectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion
        #region Mejoras RDO-II
        public List<MeMedicion48DTO> GetEnvioArchivoUltimosEjecutados(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin, int lectocodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivoUltimosEjecutados, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), lectocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEjecutados(dr));
                }
            }

            return entitys;
        }
        public List<MeMedicion48DTO> GetEnvioArchivoEjecutadosIntranet(int idFormato, DateTime fechaInicio, DateTime fechaFin, int lectocodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivoEjecutadosIntranet, idFormato, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), lectocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            var entity = new MeMedicion48DTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.CreateEjecutados(dr);
                    int iEnviocodi = dr.GetOrdinal(helper.Enviocodi);
                    if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<MeMedicion48DTO> GetEnvioArchivoUltimoEjecutado(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin, int lectocodi, int enviocodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivoUltimoEjecutado, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), lectocodi, enviocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEjecutados(dr));
                }
            }

            return entitys;
        }
        #endregion


        public List<MeMedicion48DTO> ObtenerDatosPorReporte(int idReporte, DateTime fechaInicio, DateTime fechaFin, int tipoinfocodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string sql = string.Format(helper.SqlObtenerDatosPorReporte, idReporte, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iLectcodi = dr.GetOrdinal(helper.Lectcodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iMediestado = dr.GetOrdinal(helper.Mediestado);
                    if (!dr.IsDBNull(iMediestado)) entity.Mediestado = dr.GetString(iMediestado);

                    helper.GetH1To48(dr, entity);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iE = dr.GetOrdinal("E" + i);
                        if (!dr.IsDBNull(iE)) entity.GetType().GetProperty("T" + i).SetValue(entity, Convert.ToInt32(dr.GetValue(iE)));
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void SaveInfoAdicional(MeMedicion48DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveInfoAdicional);          

            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

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

            dbProvider.AddInParameter(command, helper.E1, DbType.Int32, entity.T1);
            dbProvider.AddInParameter(command, helper.E2, DbType.Int32, entity.T2);
            dbProvider.AddInParameter(command, helper.E3, DbType.Int32, entity.T3);
            dbProvider.AddInParameter(command, helper.E4, DbType.Int32, entity.T4);
            dbProvider.AddInParameter(command, helper.E5, DbType.Int32, entity.T5);
            dbProvider.AddInParameter(command, helper.E6, DbType.Int32, entity.T6);
            dbProvider.AddInParameter(command, helper.E7, DbType.Int32, entity.T7);
            dbProvider.AddInParameter(command, helper.E8, DbType.Int32, entity.T8);
            dbProvider.AddInParameter(command, helper.E9, DbType.Int32, entity.T9);
            dbProvider.AddInParameter(command, helper.E10, DbType.Int32, entity.T10);
            dbProvider.AddInParameter(command, helper.E11, DbType.Int32, entity.T11);
            dbProvider.AddInParameter(command, helper.E12, DbType.Int32, entity.T12);
            dbProvider.AddInParameter(command, helper.E13, DbType.Int32, entity.T13);
            dbProvider.AddInParameter(command, helper.E14, DbType.Int32, entity.T14);
            dbProvider.AddInParameter(command, helper.E15, DbType.Int32, entity.T15);
            dbProvider.AddInParameter(command, helper.E16, DbType.Int32, entity.T16);
            dbProvider.AddInParameter(command, helper.E17, DbType.Int32, entity.T17);
            dbProvider.AddInParameter(command, helper.E18, DbType.Int32, entity.T18);
            dbProvider.AddInParameter(command, helper.E19, DbType.Int32, entity.T19);
            dbProvider.AddInParameter(command, helper.E20, DbType.Int32, entity.T20);
            dbProvider.AddInParameter(command, helper.E21, DbType.Int32, entity.T21);
            dbProvider.AddInParameter(command, helper.E22, DbType.Int32, entity.T22);
            dbProvider.AddInParameter(command, helper.E23, DbType.Int32, entity.T23);
            dbProvider.AddInParameter(command, helper.E24, DbType.Int32, entity.T24);
            dbProvider.AddInParameter(command, helper.E25, DbType.Int32, entity.T25);
            dbProvider.AddInParameter(command, helper.E26, DbType.Int32, entity.T26);
            dbProvider.AddInParameter(command, helper.E27, DbType.Int32, entity.T27);
            dbProvider.AddInParameter(command, helper.E28, DbType.Int32, entity.T28);
            dbProvider.AddInParameter(command, helper.E29, DbType.Int32, entity.T29);
            dbProvider.AddInParameter(command, helper.E30, DbType.Int32, entity.T30);
            dbProvider.AddInParameter(command, helper.E31, DbType.Int32, entity.T31);
            dbProvider.AddInParameter(command, helper.E32, DbType.Int32, entity.T32);
            dbProvider.AddInParameter(command, helper.E33, DbType.Int32, entity.T33);
            dbProvider.AddInParameter(command, helper.E34, DbType.Int32, entity.T34);
            dbProvider.AddInParameter(command, helper.E35, DbType.Int32, entity.T35);
            dbProvider.AddInParameter(command, helper.E36, DbType.Int32, entity.T36);
            dbProvider.AddInParameter(command, helper.E37, DbType.Int32, entity.T37);
            dbProvider.AddInParameter(command, helper.E38, DbType.Int32, entity.T38);
            dbProvider.AddInParameter(command, helper.E39, DbType.Int32, entity.T39);
            dbProvider.AddInParameter(command, helper.E40, DbType.Int32, entity.T40);
            dbProvider.AddInParameter(command, helper.E41, DbType.Int32, entity.T41);
            dbProvider.AddInParameter(command, helper.E42, DbType.Int32, entity.T42);
            dbProvider.AddInParameter(command, helper.E43, DbType.Int32, entity.T43);
            dbProvider.AddInParameter(command, helper.E44, DbType.Int32, entity.T44);
            dbProvider.AddInParameter(command, helper.E45, DbType.Int32, entity.T45);
            dbProvider.AddInParameter(command, helper.E46, DbType.Int32, entity.T46);
            dbProvider.AddInParameter(command, helper.E47, DbType.Int32, entity.T47);
            dbProvider.AddInParameter(command, helper.E48, DbType.Int32, entity.T48);

            dbProvider.ExecuteNonQuery(command);
        }

        #region Demanda PO
        public List<MeMedicion48DTO> ObtenerDemandaGeneracionPO(
            string ptomedicodi, string fechaIni, string fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string sql = string.Format(helper.SqlObtenerDemandaGeneracionPO,
                ptomedicodi, fechaIni, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iMedifecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> LeerMedidoresHidrologia(DateTime fecha)
        {
            var lista = new List<MeMedicion48DTO>();
            String query = String.Format(helper.SqlLeerMedidoresHidrologia, fecha.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new MeMedicion48DTO();
                    entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("PTOMEDICODI")));
                    entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("TIPOINFOCODI")));
                    entity.H1 = dr.IsDBNull(dr.GetOrdinal("H1")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H1"));
                    entity.H2 = dr.IsDBNull(dr.GetOrdinal("H2")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H2"));
                    entity.H3 = dr.IsDBNull(dr.GetOrdinal("H3")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H3"));
                    entity.H4 = dr.IsDBNull(dr.GetOrdinal("H4")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H4"));
                    entity.H5 = dr.IsDBNull(dr.GetOrdinal("H5")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H5"));
                    entity.H6 = dr.IsDBNull(dr.GetOrdinal("H6")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H6"));
                    entity.H7 = dr.IsDBNull(dr.GetOrdinal("H7")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H7"));
                    entity.H8 = dr.IsDBNull(dr.GetOrdinal("H8")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H8"));
                    entity.H9 = dr.IsDBNull(dr.GetOrdinal("H9")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H9"));
                    entity.H10 = dr.IsDBNull(dr.GetOrdinal("H10")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H10"));
                    entity.H11 = dr.IsDBNull(dr.GetOrdinal("H11")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H11"));
                    entity.H12 = dr.IsDBNull(dr.GetOrdinal("H12")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H12"));
                    entity.H13 = dr.IsDBNull(dr.GetOrdinal("H13")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H13"));
                    entity.H14 = dr.IsDBNull(dr.GetOrdinal("H14")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H14"));
                    entity.H15 = dr.IsDBNull(dr.GetOrdinal("H15")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H15"));
                    entity.H16 = dr.IsDBNull(dr.GetOrdinal("H16")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H16"));
                    entity.H17 = dr.IsDBNull(dr.GetOrdinal("H17")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H17"));
                    entity.H18 = dr.IsDBNull(dr.GetOrdinal("H18")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H18"));
                    entity.H19 = dr.IsDBNull(dr.GetOrdinal("H19")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H19"));
                    entity.H20 = dr.IsDBNull(dr.GetOrdinal("H20")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H20"));
                    entity.H21 = dr.IsDBNull(dr.GetOrdinal("H21")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H21"));
                    entity.H22 = dr.IsDBNull(dr.GetOrdinal("H22")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H22"));
                    entity.H23 = dr.IsDBNull(dr.GetOrdinal("H23")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H23"));
                    entity.H24 = dr.IsDBNull(dr.GetOrdinal("H24")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H24"));
                    entity.H25 = dr.IsDBNull(dr.GetOrdinal("H25")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H25"));
                    entity.H26 = dr.IsDBNull(dr.GetOrdinal("H26")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H26"));
                    entity.H27 = dr.IsDBNull(dr.GetOrdinal("H27")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H27"));
                    entity.H28 = dr.IsDBNull(dr.GetOrdinal("H28")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H28"));
                    entity.H29 = dr.IsDBNull(dr.GetOrdinal("H29")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H29"));
                    entity.H30 = dr.IsDBNull(dr.GetOrdinal("H30")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H30"));
                    entity.H31 = dr.IsDBNull(dr.GetOrdinal("H31")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H31"));
                    entity.H32 = dr.IsDBNull(dr.GetOrdinal("H32")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H32"));
                    entity.H33 = dr.IsDBNull(dr.GetOrdinal("H33")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H33"));
                    entity.H34 = dr.IsDBNull(dr.GetOrdinal("H34")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H34"));
                    entity.H35 = dr.IsDBNull(dr.GetOrdinal("H35")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H35"));
                    entity.H36 = dr.IsDBNull(dr.GetOrdinal("H36")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H36"));
                    entity.H37 = dr.IsDBNull(dr.GetOrdinal("H37")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H37"));
                    entity.H38 = dr.IsDBNull(dr.GetOrdinal("H38")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H38"));
                    entity.H39 = dr.IsDBNull(dr.GetOrdinal("H39")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H39"));
                    entity.H40 = dr.IsDBNull(dr.GetOrdinal("H40")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H40"));
                    entity.H41 = dr.IsDBNull(dr.GetOrdinal("H41")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H41"));
                    entity.H42 = dr.IsDBNull(dr.GetOrdinal("H42")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H42"));
                    entity.H43 = dr.IsDBNull(dr.GetOrdinal("H43")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H43"));
                    entity.H44 = dr.IsDBNull(dr.GetOrdinal("H44")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H44"));
                    entity.H45 = dr.IsDBNull(dr.GetOrdinal("H45")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H45"));
                    entity.H46 = dr.IsDBNull(dr.GetOrdinal("H46")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H46"));
                    entity.H47 = dr.IsDBNull(dr.GetOrdinal("H47")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H47"));
                    entity.H48 = dr.IsDBNull(dr.GetOrdinal("H48")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H48"));

                    lista.Add(entity);
                }
            }
            return lista;
        }


        public List<Medicion48DTO> ObtenerDemandaProgramadaDiariaAreas(DateTime dtFecha)
        {

            var lista = new List<Medicion48DTO>();
            String query = String.Format(helper.SqlDemandaProgramadaxAreas, dtFecha.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new Medicion48DTO();
                    entity.PTOMEDICODI = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("PTOMEDICODI")));
                    entity.PTOMEDIELENOMB = Convert.ToString(dr.GetValue(dr.GetOrdinal("PTOMEDIDESC")));
                    entity.MEDIFECHA = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("MEDIFECHA")));
                    entity.H1 = dr.IsDBNull(dr.GetOrdinal("H1")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H1"));
                    entity.H2 = dr.IsDBNull(dr.GetOrdinal("H2")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H2"));
                    entity.H3 = dr.IsDBNull(dr.GetOrdinal("H3")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H3"));
                    entity.H4 = dr.IsDBNull(dr.GetOrdinal("H4")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H4"));
                    entity.H5 = dr.IsDBNull(dr.GetOrdinal("H5")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H5"));
                    entity.H6 = dr.IsDBNull(dr.GetOrdinal("H6")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H6"));
                    entity.H7 = dr.IsDBNull(dr.GetOrdinal("H7")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H7"));
                    entity.H8 = dr.IsDBNull(dr.GetOrdinal("H8")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H8"));
                    entity.H9 = dr.IsDBNull(dr.GetOrdinal("H9")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H9"));
                    entity.H10 = dr.IsDBNull(dr.GetOrdinal("H10")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H10"));
                    entity.H11 = dr.IsDBNull(dr.GetOrdinal("H11")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H11"));
                    entity.H12 = dr.IsDBNull(dr.GetOrdinal("H12")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H12"));
                    entity.H13 = dr.IsDBNull(dr.GetOrdinal("H13")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H13"));
                    entity.H14 = dr.IsDBNull(dr.GetOrdinal("H14")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H14"));
                    entity.H15 = dr.IsDBNull(dr.GetOrdinal("H15")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H15"));
                    entity.H16 = dr.IsDBNull(dr.GetOrdinal("H16")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H16"));
                    entity.H17 = dr.IsDBNull(dr.GetOrdinal("H17")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H17"));
                    entity.H18 = dr.IsDBNull(dr.GetOrdinal("H18")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H18"));
                    entity.H19 = dr.IsDBNull(dr.GetOrdinal("H19")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H19"));
                    entity.H20 = dr.IsDBNull(dr.GetOrdinal("H20")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H20"));
                    entity.H21 = dr.IsDBNull(dr.GetOrdinal("H21")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H21"));
                    entity.H22 = dr.IsDBNull(dr.GetOrdinal("H22")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H22"));
                    entity.H23 = dr.IsDBNull(dr.GetOrdinal("H23")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H23"));
                    entity.H24 = dr.IsDBNull(dr.GetOrdinal("H24")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H24"));
                    entity.H25 = dr.IsDBNull(dr.GetOrdinal("H25")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H25"));
                    entity.H26 = dr.IsDBNull(dr.GetOrdinal("H26")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H26"));
                    entity.H27 = dr.IsDBNull(dr.GetOrdinal("H27")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H27"));
                    entity.H28 = dr.IsDBNull(dr.GetOrdinal("H28")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H28"));
                    entity.H29 = dr.IsDBNull(dr.GetOrdinal("H29")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H29"));
                    entity.H30 = dr.IsDBNull(dr.GetOrdinal("H30")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H30"));
                    entity.H31 = dr.IsDBNull(dr.GetOrdinal("H31")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H31"));
                    entity.H32 = dr.IsDBNull(dr.GetOrdinal("H32")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H32"));
                    entity.H33 = dr.IsDBNull(dr.GetOrdinal("H33")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H33"));
                    entity.H34 = dr.IsDBNull(dr.GetOrdinal("H34")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H34"));
                    entity.H35 = dr.IsDBNull(dr.GetOrdinal("H35")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H35"));
                    entity.H36 = dr.IsDBNull(dr.GetOrdinal("H36")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H36"));
                    entity.H37 = dr.IsDBNull(dr.GetOrdinal("H37")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H37"));
                    entity.H38 = dr.IsDBNull(dr.GetOrdinal("H38")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H38"));
                    entity.H39 = dr.IsDBNull(dr.GetOrdinal("H39")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H39"));
                    entity.H40 = dr.IsDBNull(dr.GetOrdinal("H40")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H40"));
                    entity.H41 = dr.IsDBNull(dr.GetOrdinal("H41")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H41"));
                    entity.H42 = dr.IsDBNull(dr.GetOrdinal("H42")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H42"));
                    entity.H43 = dr.IsDBNull(dr.GetOrdinal("H43")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H43"));
                    entity.H44 = dr.IsDBNull(dr.GetOrdinal("H44")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H44"));
                    entity.H45 = dr.IsDBNull(dr.GetOrdinal("H45")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H45"));
                    entity.H46 = dr.IsDBNull(dr.GetOrdinal("H46")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H46"));
                    entity.H47 = dr.IsDBNull(dr.GetOrdinal("H47")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H47"));
                    entity.H48 = dr.IsDBNull(dr.GetOrdinal("H48")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H48"));

                    lista.Add(entity);
                }
            }
            return lista;
        }


        public List<Medicion48DTO> ObtenerProgramadaDiariaCOES(DateTime dtFecha)
        {
            var lista = new List<Medicion48DTO>();
            String query = String.Format(helper.SqlDemandaProgramadaDiariaCOES, dtFecha.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new Medicion48DTO();
                    entity.PTOMEDICODI = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("PTOMEDICODI")));
                    entity.PTOMEDIELENOMB = Convert.ToString(dr.GetValue(dr.GetOrdinal("PTOMEDIDESC")));
                    entity.MEDIFECHA = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("MEDIFECHA")));
                    entity.H1 = dr.IsDBNull(dr.GetOrdinal("H1")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H1"));
                    entity.H2 = dr.IsDBNull(dr.GetOrdinal("H2")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H2"));
                    entity.H3 = dr.IsDBNull(dr.GetOrdinal("H3")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H3"));
                    entity.H4 = dr.IsDBNull(dr.GetOrdinal("H4")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H4"));
                    entity.H5 = dr.IsDBNull(dr.GetOrdinal("H5")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H5"));
                    entity.H6 = dr.IsDBNull(dr.GetOrdinal("H6")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H6"));
                    entity.H7 = dr.IsDBNull(dr.GetOrdinal("H7")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H7"));
                    entity.H8 = dr.IsDBNull(dr.GetOrdinal("H8")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H8"));
                    entity.H9 = dr.IsDBNull(dr.GetOrdinal("H9")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H9"));
                    entity.H10 = dr.IsDBNull(dr.GetOrdinal("H10")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H10"));
                    entity.H11 = dr.IsDBNull(dr.GetOrdinal("H11")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H11"));
                    entity.H12 = dr.IsDBNull(dr.GetOrdinal("H12")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H12"));
                    entity.H13 = dr.IsDBNull(dr.GetOrdinal("H13")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H13"));
                    entity.H14 = dr.IsDBNull(dr.GetOrdinal("H14")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H14"));
                    entity.H15 = dr.IsDBNull(dr.GetOrdinal("H15")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H15"));
                    entity.H16 = dr.IsDBNull(dr.GetOrdinal("H16")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H16"));
                    entity.H17 = dr.IsDBNull(dr.GetOrdinal("H17")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H17"));
                    entity.H18 = dr.IsDBNull(dr.GetOrdinal("H18")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H18"));
                    entity.H19 = dr.IsDBNull(dr.GetOrdinal("H19")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H19"));
                    entity.H20 = dr.IsDBNull(dr.GetOrdinal("H20")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H20"));
                    entity.H21 = dr.IsDBNull(dr.GetOrdinal("H21")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H21"));
                    entity.H22 = dr.IsDBNull(dr.GetOrdinal("H22")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H22"));
                    entity.H23 = dr.IsDBNull(dr.GetOrdinal("H23")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H23"));
                    entity.H24 = dr.IsDBNull(dr.GetOrdinal("H24")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H24"));
                    entity.H25 = dr.IsDBNull(dr.GetOrdinal("H25")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H25"));
                    entity.H26 = dr.IsDBNull(dr.GetOrdinal("H26")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H26"));
                    entity.H27 = dr.IsDBNull(dr.GetOrdinal("H27")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H27"));
                    entity.H28 = dr.IsDBNull(dr.GetOrdinal("H28")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H28"));
                    entity.H29 = dr.IsDBNull(dr.GetOrdinal("H29")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H29"));
                    entity.H30 = dr.IsDBNull(dr.GetOrdinal("H30")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H30"));
                    entity.H31 = dr.IsDBNull(dr.GetOrdinal("H31")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H31"));
                    entity.H32 = dr.IsDBNull(dr.GetOrdinal("H32")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H32"));
                    entity.H33 = dr.IsDBNull(dr.GetOrdinal("H33")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H33"));
                    entity.H34 = dr.IsDBNull(dr.GetOrdinal("H34")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H34"));
                    entity.H35 = dr.IsDBNull(dr.GetOrdinal("H35")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H35"));
                    entity.H36 = dr.IsDBNull(dr.GetOrdinal("H36")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H36"));
                    entity.H37 = dr.IsDBNull(dr.GetOrdinal("H37")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H37"));
                    entity.H38 = dr.IsDBNull(dr.GetOrdinal("H38")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H38"));
                    entity.H39 = dr.IsDBNull(dr.GetOrdinal("H39")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H39"));
                    entity.H40 = dr.IsDBNull(dr.GetOrdinal("H40")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H40"));
                    entity.H41 = dr.IsDBNull(dr.GetOrdinal("H41")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H41"));
                    entity.H42 = dr.IsDBNull(dr.GetOrdinal("H42")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H42"));
                    entity.H43 = dr.IsDBNull(dr.GetOrdinal("H43")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H43"));
                    entity.H44 = dr.IsDBNull(dr.GetOrdinal("H44")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H44"));
                    entity.H45 = dr.IsDBNull(dr.GetOrdinal("H45")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H45"));
                    entity.H46 = dr.IsDBNull(dr.GetOrdinal("H46")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H46"));
                    entity.H47 = dr.IsDBNull(dr.GetOrdinal("H47")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H47"));
                    entity.H48 = dr.IsDBNull(dr.GetOrdinal("H48")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H48"));

                    lista.Add(entity);
                }
            }
            return lista;
        }


        public List<Medicion48DTO> ObtenerDemandaDiariaReal(DateTime fechaInicio)
        {

            var lista = new List<Medicion48DTO>();
            String query = String.Format(helper.SqlDemandaDiariaxAreas, fechaInicio.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new Medicion48DTO();
                    entity.PTOMEDICODI = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("PTOMEDICODI")));
                    entity.PTOMEDIELENOMB = Convert.ToString(dr.GetValue(dr.GetOrdinal("PTOMEDIDESC")));
                    entity.MEDIFECHA = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("MEDIFECHA")));
                    entity.H1 = dr.IsDBNull(dr.GetOrdinal("H1")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H1"));
                    entity.H2 = dr.IsDBNull(dr.GetOrdinal("H2")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H2"));
                    entity.H3 = dr.IsDBNull(dr.GetOrdinal("H3")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H3"));
                    entity.H4 = dr.IsDBNull(dr.GetOrdinal("H4")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H4"));
                    entity.H5 = dr.IsDBNull(dr.GetOrdinal("H5")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H5"));
                    entity.H6 = dr.IsDBNull(dr.GetOrdinal("H6")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H6"));
                    entity.H7 = dr.IsDBNull(dr.GetOrdinal("H7")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H7"));
                    entity.H8 = dr.IsDBNull(dr.GetOrdinal("H8")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H8"));
                    entity.H9 = dr.IsDBNull(dr.GetOrdinal("H9")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H9"));
                    entity.H10 = dr.IsDBNull(dr.GetOrdinal("H10")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H10"));
                    entity.H11 = dr.IsDBNull(dr.GetOrdinal("H11")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H11"));
                    entity.H12 = dr.IsDBNull(dr.GetOrdinal("H12")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H12"));
                    entity.H13 = dr.IsDBNull(dr.GetOrdinal("H13")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H13"));
                    entity.H14 = dr.IsDBNull(dr.GetOrdinal("H14")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H14"));
                    entity.H15 = dr.IsDBNull(dr.GetOrdinal("H15")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H15"));
                    entity.H16 = dr.IsDBNull(dr.GetOrdinal("H16")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H16"));
                    entity.H17 = dr.IsDBNull(dr.GetOrdinal("H17")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H17"));
                    entity.H18 = dr.IsDBNull(dr.GetOrdinal("H18")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H18"));
                    entity.H19 = dr.IsDBNull(dr.GetOrdinal("H19")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H19"));
                    entity.H20 = dr.IsDBNull(dr.GetOrdinal("H20")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H20"));
                    entity.H21 = dr.IsDBNull(dr.GetOrdinal("H21")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H21"));
                    entity.H22 = dr.IsDBNull(dr.GetOrdinal("H22")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H22"));
                    entity.H23 = dr.IsDBNull(dr.GetOrdinal("H23")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H23"));
                    entity.H24 = dr.IsDBNull(dr.GetOrdinal("H24")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H24"));
                    entity.H25 = dr.IsDBNull(dr.GetOrdinal("H25")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H25"));
                    entity.H26 = dr.IsDBNull(dr.GetOrdinal("H26")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H26"));
                    entity.H27 = dr.IsDBNull(dr.GetOrdinal("H27")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H27"));
                    entity.H28 = dr.IsDBNull(dr.GetOrdinal("H28")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H28"));
                    entity.H29 = dr.IsDBNull(dr.GetOrdinal("H29")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H29"));
                    entity.H30 = dr.IsDBNull(dr.GetOrdinal("H30")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H30"));
                    entity.H31 = dr.IsDBNull(dr.GetOrdinal("H31")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H31"));
                    entity.H32 = dr.IsDBNull(dr.GetOrdinal("H32")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H32"));
                    entity.H33 = dr.IsDBNull(dr.GetOrdinal("H33")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H33"));
                    entity.H34 = dr.IsDBNull(dr.GetOrdinal("H34")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H34"));
                    entity.H35 = dr.IsDBNull(dr.GetOrdinal("H35")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H35"));
                    entity.H36 = dr.IsDBNull(dr.GetOrdinal("H36")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H36"));
                    entity.H37 = dr.IsDBNull(dr.GetOrdinal("H37")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H37"));
                    entity.H38 = dr.IsDBNull(dr.GetOrdinal("H38")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H38"));
                    entity.H39 = dr.IsDBNull(dr.GetOrdinal("H39")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H39"));
                    entity.H40 = dr.IsDBNull(dr.GetOrdinal("H40")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H40"));
                    entity.H41 = dr.IsDBNull(dr.GetOrdinal("H41")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H41"));
                    entity.H42 = dr.IsDBNull(dr.GetOrdinal("H42")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H42"));
                    entity.H43 = dr.IsDBNull(dr.GetOrdinal("H43")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H43"));
                    entity.H44 = dr.IsDBNull(dr.GetOrdinal("H44")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H44"));
                    entity.H45 = dr.IsDBNull(dr.GetOrdinal("H45")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H45"));
                    entity.H46 = dr.IsDBNull(dr.GetOrdinal("H46")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H46"));
                    entity.H47 = dr.IsDBNull(dr.GetOrdinal("H47")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H47"));
                    entity.H48 = dr.IsDBNull(dr.GetOrdinal("H48")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H48"));

                    lista.Add(entity);
                }
            }
            return lista;
        }


        public List<Medicion48DTO> LeerDemandaAreas(DateTime inicioFecha)
        {
            var lista = new List<Medicion48DTO>();
            String query = String.Format(helper.SqlDemandaDiariaxAreas, inicioFecha.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new Medicion48DTO();
                    entity.MEDIFECHA = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("MEDIFECHA")));
                    entity.PTOMEDICODI = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("PTOMEDICODI")));
                    entity.H1 = dr.IsDBNull(dr.GetOrdinal("H1")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H1"));
                    entity.H2 = dr.IsDBNull(dr.GetOrdinal("H2")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H2"));
                    entity.H3 = dr.IsDBNull(dr.GetOrdinal("H3")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H3"));
                    entity.H4 = dr.IsDBNull(dr.GetOrdinal("H4")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H4"));
                    entity.H5 = dr.IsDBNull(dr.GetOrdinal("H5")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H5"));
                    entity.H6 = dr.IsDBNull(dr.GetOrdinal("H6")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H6"));
                    entity.H7 = dr.IsDBNull(dr.GetOrdinal("H7")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H7"));
                    entity.H8 = dr.IsDBNull(dr.GetOrdinal("H8")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H8"));
                    entity.H9 = dr.IsDBNull(dr.GetOrdinal("H9")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H9"));
                    entity.H10 = dr.IsDBNull(dr.GetOrdinal("H10")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H10"));
                    entity.H11 = dr.IsDBNull(dr.GetOrdinal("H11")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H11"));
                    entity.H12 = dr.IsDBNull(dr.GetOrdinal("H12")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H12"));
                    entity.H13 = dr.IsDBNull(dr.GetOrdinal("H13")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H13"));
                    entity.H14 = dr.IsDBNull(dr.GetOrdinal("H14")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H14"));
                    entity.H15 = dr.IsDBNull(dr.GetOrdinal("H15")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H15"));
                    entity.H16 = dr.IsDBNull(dr.GetOrdinal("H16")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H16"));
                    entity.H17 = dr.IsDBNull(dr.GetOrdinal("H17")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H17"));
                    entity.H18 = dr.IsDBNull(dr.GetOrdinal("H18")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H18"));
                    entity.H19 = dr.IsDBNull(dr.GetOrdinal("H19")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H19"));
                    entity.H20 = dr.IsDBNull(dr.GetOrdinal("H20")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H20"));
                    entity.H21 = dr.IsDBNull(dr.GetOrdinal("H21")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H21"));
                    entity.H22 = dr.IsDBNull(dr.GetOrdinal("H22")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H22"));
                    entity.H23 = dr.IsDBNull(dr.GetOrdinal("H23")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H23"));
                    entity.H24 = dr.IsDBNull(dr.GetOrdinal("H24")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H24"));
                    entity.H25 = dr.IsDBNull(dr.GetOrdinal("H25")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H25"));
                    entity.H26 = dr.IsDBNull(dr.GetOrdinal("H26")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H26"));
                    entity.H27 = dr.IsDBNull(dr.GetOrdinal("H27")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H27"));
                    entity.H28 = dr.IsDBNull(dr.GetOrdinal("H28")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H28"));
                    entity.H29 = dr.IsDBNull(dr.GetOrdinal("H29")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H29"));
                    entity.H30 = dr.IsDBNull(dr.GetOrdinal("H30")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H30"));
                    entity.H31 = dr.IsDBNull(dr.GetOrdinal("H31")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H31"));
                    entity.H32 = dr.IsDBNull(dr.GetOrdinal("H32")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H32"));
                    entity.H33 = dr.IsDBNull(dr.GetOrdinal("H33")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H33"));
                    entity.H34 = dr.IsDBNull(dr.GetOrdinal("H34")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H34"));
                    entity.H35 = dr.IsDBNull(dr.GetOrdinal("H35")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H35"));
                    entity.H36 = dr.IsDBNull(dr.GetOrdinal("H36")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H36"));
                    entity.H37 = dr.IsDBNull(dr.GetOrdinal("H37")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H37"));
                    entity.H38 = dr.IsDBNull(dr.GetOrdinal("H38")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H38"));
                    entity.H39 = dr.IsDBNull(dr.GetOrdinal("H39")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H39"));
                    entity.H40 = dr.IsDBNull(dr.GetOrdinal("H40")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H40"));
                    entity.H41 = dr.IsDBNull(dr.GetOrdinal("H41")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H41"));
                    entity.H42 = dr.IsDBNull(dr.GetOrdinal("H42")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H42"));
                    entity.H43 = dr.IsDBNull(dr.GetOrdinal("H43")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H43"));
                    entity.H44 = dr.IsDBNull(dr.GetOrdinal("H44")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H44"));
                    entity.H45 = dr.IsDBNull(dr.GetOrdinal("H45")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H45"));
                    entity.H46 = dr.IsDBNull(dr.GetOrdinal("H46")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H46"));
                    entity.H47 = dr.IsDBNull(dr.GetOrdinal("H47")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H47"));
                    entity.H48 = dr.IsDBNull(dr.GetOrdinal("H48")) ? (decimal?)null : dr.GetDecimal(dr.GetOrdinal("H48"));

                    lista.Add(entity);
                }
            }
            return lista;
        }



        #endregion

        public List<MeMedicion48DTO> GetConsolidadoMaximaDemanda48(int tipoCentral, string tipoGeneracion, DateTime fechaIni, DateTime fechaFin, string idEmpresa, int lectcodi, int tipoinfocodi, int tptomedicodi)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            String query = String.Format(this.helper.SqlGetConsolidadoMaximaDemanda, tipoCentral, tipoGeneracion
                , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    entity = helper.Create(dr);

                    int itipogrupocodi = dr.GetOrdinal(helper.Tipogrupocodi);
                    if (!dr.IsDBNull(itipogrupocodi)) entity.Tipogrupocodi = Convert.ToInt32(dr.GetValue(itipogrupocodi));
                    int iTipogenerrer = dr.GetOrdinal(this.helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);
                    int iGrupointegrante = dr.GetOrdinal(this.helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt16(dr.GetValue(iTgenercodi));
                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iEquinomb)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprorden = dr.GetOrdinal(this.helper.Emprorden);
                    if (!dr.IsDBNull(iEmprorden)) entity.Emprorden = Convert.ToInt32(dr.GetValue(iEmprorden));

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);
                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));
                    int iGrupoabrev = dr.GetOrdinal(this.helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupoorden = dr.GetOrdinal(helper.Grupoorden);
                    if (!dr.IsDBNull(iGrupoorden)) entity.Grupoorden = Convert.ToInt32(dr.GetValue(iGrupoorden));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> GetConsolidadoMaximaDemanda48SinGrupoIntegrante(int tipoCentral, string tipoGeneracion, DateTime fechaIni, DateTime fechaFin, string idEmpresa, int lectcodi, int tipoinfocodi, int tptomedicodi, int tipoReporte)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

            String query = "";

            //if(tipoReporte == 1)
            //{
            //    query = String.Format(this.helper.SqlGetConsolidadoMaximaDemandaSinGrupoIntegrante1, tipoCentral, tipoGeneracion
            //    , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tptomedicodi);
            //}
            //else
            //{

            //la query anterior, que está comentado, no muestra los datos de Punta Lomitas ni tampoco grupos que ya no estan activos pero s� tuvieron data
            query = String.Format(this.helper.SqlGetConsolidadoMaximaDemandaSinGrupoIntegrante, tipoCentral, tipoGeneracion
            , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tptomedicodi);
            //}            

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    entity = helper.Create(dr);

                    int itipogrupocodi = dr.GetOrdinal(helper.Tipogrupocodi);
                    if (!dr.IsDBNull(itipogrupocodi)) entity.Tipogrupocodi = dr.GetInt32(itipogrupocodi);
                    int iTipogenerrer = dr.GetOrdinal(this.helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);
                    int iGrupointegrante = dr.GetOrdinal(this.helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);
                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt16(dr.GetValue(iTgenercodi));
                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iEquinomb)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprorden = dr.GetOrdinal(this.helper.Emprorden);
                    if (!dr.IsDBNull(iEmprorden)) entity.Emprorden = Convert.ToInt32(dr.GetValue(iEmprorden));
                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);
                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));
                    int iGrupoabrev = dr.GetOrdinal(this.helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupoorden = dr.GetOrdinal(helper.Grupoorden);
                    if (!dr.IsDBNull(iGrupoorden)) entity.Grupoorden = Convert.ToInt32(dr.GetValue(iGrupoorden));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iPtomedinomb = dr.GetOrdinal(helper.Ptomedinomb);
                    if (!dr.IsDBNull(iPtomedinomb)) entity.Ptomedinomb = dr.GetString(iPtomedinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<MeMedicion48DTO> GetDemandaEjecutadaConEcuador(int lectcodi, int tipoinfocodi, DateTime fechaInicio, DateTime fechaFin, int lectcodiArea, int ptomedicodiL2280Ecuador)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string sql = string.Format(helper.SqlGetDemandaEjecutadaConEcuador,
                lectcodi, tipoinfocodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), lectcodiArea, ptomedicodiL2280Ecuador);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);
                    helper.GetH1To48(dr, entity);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> GetDemandaCOESPtoMedicion48(int lectcodi, int tipoinfocodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string sql = string.Format(helper.SqlGetDemandaCOESPtoMedicion48,
                lectcodi, tipoinfocodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);
                    helper.GetH1To48(dr, entity);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }

}