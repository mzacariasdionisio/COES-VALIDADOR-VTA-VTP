using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Framework.Base.Tools;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Text;



namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SCO_TABLA_SYNC
    /// </summary>
    public class IioTablaSyncRepository :RepositoryBase, IIioTablaSyncRepository
    {
        private readonly IioTablaSyncHelper helper = new IioTablaSyncHelper();


        public IioTablaSyncRepository(string strConn) : 
            base(strConn)
        {
            
        }

        public List<IioTablaSyncDTO> List(int pseincodi)
        {
            List<IioTablaSyncDTO> entitys = new List<IioTablaSyncDTO>();

            var command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinCodi, DbType.String, pseincodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;

        }

        public string GetPath(string periodo, string query, string delimitador, string path, string tabla, string nombreCortoTabla, int idEnvio) 
        {
            string zipPath = path + "\\" + nombreCortoTabla + "_" + idEnvio + ".zip";
            int cont = 0;
            string queryString = string.Format(query, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var file = archive.CreateEntry(tabla + ".txt");
                    using (var entryStream = file.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        string data = "";
                        using (IDataReader dr = dbProvider.ExecuteReader(command))
                        {
                            var columns = new List<string>();
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                columns.Add(dr.GetName(i));
                            }

                            while (dr.Read())
                            {
                                data = "";
                                foreach (var item in columns)
                                {
                                    if (data == "")
                                    {
                                        data = data + dr[item.Trim()].ToString();
                                    }
                                    else
                                    {
                                        data = data + delimitador + dr[item.Trim()].ToString();
                                    }
                                }
                                streamWriter.WriteLine(data);
                                cont++;
                            }
                        }
                    }
                }

                using (var fileStream = new FileStream(zipPath, FileMode.Create))
                {
                    memoryStream.Position = 0;
                    memoryStream.WriteTo(fileStream);
                }

            }
            return zipPath+"|"+cont;
        }

        public IDataReader GetDataReader(string periodo, string query)
        {
            string queryString = string.Format(query, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            IDataReader dr = dbProvider.ExecuteReader(command);
            return dr;
        }

    }
}