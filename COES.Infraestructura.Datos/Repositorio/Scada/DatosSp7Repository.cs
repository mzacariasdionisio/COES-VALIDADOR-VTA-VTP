using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using System.Data.Odbc;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla DATOS_SP7
    /// </summary>
    public class DatosSp7Repository : RepositoryBase, IDatosSp7Repository
    {
        public DatosSp7Repository(string strConn)
            : base(strConn)
        {
        }

        DatosSp7Helper helper = new DatosSp7Helper();
        /*
        public int Save(DatosSP7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, entity.Fecha);
            dbProvider.AddInParameter(command, helper.Fechasistema, DbType.DateTime, entity.FechaSistema);
            dbProvider.AddInParameter(command, helper.Path, DbType.String, entity.Path);
            dbProvider.AddInParameter(command, helper.Valor, DbType.Decimal, entity.Valor);
            dbProvider.AddInParameter(command, helper.Calidad, DbType.Int32, entity.Calidad);
            dbProvider.AddInParameter(command, helper.Calidadtexto, DbType.String, entity.CalidadTexto);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }*/





        public List<DatosSP7DTO> ObtenerListadoSp7(string tabla, DateTime fechaInicial, DateTime fechaFinal, string path)
        {
            List<DatosSP7DTO> entitys = new List<DatosSP7DTO>();
            int desfase = 5;

            fechaInicial = fechaInicial.AddHours(desfase);
            fechaFinal = fechaFinal.AddHours(desfase);

            try
            {
                string sql = "select timestamp ,SystemTimeStamp, Path, value ,QualityTextUser  from " + tabla + " " +
                                         " where SystemTimeStamp >= {ts '" + fechaInicial.ToString(ConstantesBase.FormatoFechaExtendido) + "'} " +
                                         " and SystemTimeStamp <= {ts '" + fechaFinal.ToString(ConstantesBase.FormatoFechaExtendido) + "'} " +
                        " and path like '" + path + "'";

                using (OdbcCommand command = new OdbcCommand(sql))
                {
                    using (IDataReader dr = dbProvider.ExecuteReader(command))
                    {
                        while (dr.Read())
                        {
                            int iFecha = dr.GetOrdinal(this.helper.Fecha);
                            int iFechasistema = dr.GetOrdinal(this.helper.Fechasistema);
                            int iPath = dr.GetOrdinal(this.helper.Path);
                            int iValor = dr.GetOrdinal(this.helper.Valor1);
                            int iCalidadtexto = dr.GetOrdinal(this.helper.Calidadtexto);

                            DateTime fecha = dr.GetDateTime(iFecha).AddHours(-desfase);
                            DateTime fechaSistema = dr.GetDateTime(iFechasistema).AddHours(-desfase);
                            string path2 = dr.GetString(iPath);
                            decimal valor = dr.GetDecimal(iValor);
                            int calidad = Calidad(dr.GetString(iCalidadtexto));


                            //DatosSP7DTO dat = new DatosSP7DTO
                            //{
                            //    Fecha = fecha,
                            //    FechaSistema = fechaSistema,
                            //    Path = path2,
                            //    Valor = valor,
                            //    Calidad = calidad
                            //};

                            DatosSP7DTO dat = new DatosSP7DTO(fecha, fechaSistema, path2, valor, calidad);

                            entitys.Add(dat);
                        }

                        try
                        {
                            dr.Close();
                            dr.Dispose();
                        }
                        catch { }

                    }

                    try
                    {
                        command.Dispose();
                    }
                    catch
                    { }
                }
            }
            catch { }

            try
            {
                OdbcConnection.ReleaseObjectPool();
            }
            catch { }

            if (entitys == null)
                entitys = new List<DatosSP7DTO>();

            return entitys;
        }

        private int Calidad(string CalidadSP7)
        {
            switch (CalidadSP7.ToUpper())
            {
                case "ACTUAL":
                    return 0;
                case "VALID":
                    return 0;


                case "HELD":
                    return 16;


                case "NOTRENEW":
                    return 32;
                case "SUSPECT":
                    return 32;


                case "INVALID":
                    return 48;
                case "NOTVALID":
                    return 48;



            }

            return 49;
        }


    }
}
