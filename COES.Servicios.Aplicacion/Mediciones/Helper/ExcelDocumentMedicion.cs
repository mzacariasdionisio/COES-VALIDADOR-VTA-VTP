using System.Xml;
using OfficeOpenXml;
using System.Net;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.Mediciones.Helper
{
    public static class ExcelDocumentMedicion
    {
        public static void SetPieChartPointColors(ExcelPieChart pieChart, List<string> listaColor)
        {
            string PIE_PATH = "c:chartSpace/c:chart/c:plotArea/c:pieChart/c:ser";

            //Get the nodes
            var ws = pieChart.WorkSheet;
            var nsm = ws.Drawings.NameSpaceManager;
            var nschart = nsm.LookupNamespace("c");
            var nsa = nsm.LookupNamespace("a");
            var node = pieChart.ChartXml.SelectSingleNode(PIE_PATH, nsm);
            var doc = pieChart.ChartXml;

            //Add the node
            var rand = new Random();
            for (var i = 0; i < listaColor.Count; i++)
            {
                //Create the data point node
                var dPt = doc.CreateElement("dPt", nschart);

                var idx = dPt.AppendChild(doc.CreateElement("idx", nschart));
                var valattrib = idx.Attributes.Append(doc.CreateAttribute("val"));
                valattrib.Value = i.ToString(CultureInfo.InvariantCulture);
                node.AppendChild(dPt);

                //Add the solid fill node
                var spPr = doc.CreateElement("spPr", nschart);
                var solidFill = spPr.AppendChild(doc.CreateElement("solidFill", nsa));
                var srgbClr = solidFill.AppendChild(doc.CreateElement("srgbClr", nsa));
                valattrib = srgbClr.Attributes.Append(doc.CreateAttribute("val"));

                //Set the color
                var color = ColorTranslator.FromHtml(listaColor[i]);
                valattrib.Value = ColorTranslator.ToHtml(color).Replace("#", String.Empty);
                dPt.AppendChild(spPr);
            }
        }

        public static void SetPieChartDataLabelPercent(ExcelPieChart pieChart)
        {
            var xdoc = pieChart.ChartXml;
            var nsuri = xdoc.DocumentElement.NamespaceURI;
            var nsm = new XmlNamespaceManager(xdoc.NameTable);
            nsm.AddNamespace("c", nsuri);

            //Added the number format node via XML
            var numFmtNode = xdoc.CreateElement("c:numFmt", nsuri);

            var formatCodeAtt = xdoc.CreateAttribute("formatCode", nsuri);
            formatCodeAtt.Value = "0.00%";
            numFmtNode.Attributes.Append(formatCodeAtt);

            var sourceLinkedAtt = xdoc.CreateAttribute("sourceLinked", nsuri);
            sourceLinkedAtt.Value = "0";
            numFmtNode.Attributes.Append(sourceLinkedAtt);

            var dLblsNode = xdoc.SelectSingleNode("c:chartSpace/c:chart/c:plotArea/c:pieChart/c:ser/c:dLbls", nsm);
            dLblsNode.AppendChild(numFmtNode);

            //Format the legend
            pieChart.Legend.Add();
            pieChart.Legend.Position = eLegendPosition.Right;
        }
    }
}
