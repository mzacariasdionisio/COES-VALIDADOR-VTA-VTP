// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Base.Core.HelperBase
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using System;
using System.Xml;

namespace COES.Storage.App.Base.Core
{
  public class HelperBase
  {
    private string className;
    private string xmlFile;

    public HelperBase(string file) => this.xmlFile = file;

    public HelperBase()
    {
    }

    public string SpGetById => "USP_GETBYID_" + this.className.ToUpper();

    public string SpList => "USP_LIST_" + this.className.ToUpper();

    public string SpGetByCriteria => "USP_GETBYCRITERIA_" + this.className.ToUpper();

    public string SpSave => "USP_SAVE_" + this.className.ToUpper();

    public string SpUpdate => "USP_UPDATE_" + this.className.ToUpper();

    public string SpDelete => "USP_DELETE_" + this.className.ToUpper();

    public string SqlGetById => this.GetSqlXml("GetById");

    public string SqlList => this.GetSqlXml("List");

    public string SqlGetByCriteria => this.GetSqlXml("GetByCriteria");

    public string SqlSave => this.GetSqlXml("Save");

    public string SqlUpdate => this.GetSqlXml("Update");

    public string SqlDelete => this.GetSqlXml("Delete");

    public string SqlTotalRecords => this.GetSqlXml("TotalRecords");

    public string SqlGetMaxId => this.GetSqlXml("GetMaxId");

    public string GetSqlXml(string idSql)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(this.xmlFile);
        foreach (XmlElement xmlElement in ((XmlElement) xmlDocument.GetElementsByTagName("Sqls")[0]).GetElementsByTagName("Sql"))
        {
          if (xmlElement.GetElementsByTagName("key")[0].InnerText == idSql)
            return xmlElement.GetElementsByTagName("query")[0].InnerText;
        }
        return string.Empty;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }
  }
}
