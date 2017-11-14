using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodoPost
{
    public class DiarioOficial : Robo
    {
        public DiarioOficial()
        {
            RoboWebClient = new RoboWebClient();
            RoboWebClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.89 Safari/537.36";
        }
        /// <summary>
        /// Efetua o download de um diario oficial
        /// </summary>
        /// <returns>pdf em bytes</returns>
        public byte[] BaixarDiario()
        {

            string urlSite = @"http://dejt.jt.jus.br/dejt/f/n/diariocon";

            var htmlDoc = base.HttpGet(urlSite);

            HtmlNodeCollection nodesForm = htmlDoc.DocumentNode.SelectNodes("//form [@id='corpo:formulario']");

            NameValueCollection nvcParametros = new NameValueCollection();
            nvcParametros.Add("corpo:formulario:dataIni", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
            nvcParametros.Add("corpo:formulario:dataFim", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
            nvcParametros.Add("corpo:formulario:tipoCaderno", "1");
            nvcParametros.Add("corpo:formulario:tribunal", "0");
            nvcParametros.Add("corpo:formulario:ordenacaoPlc", Pegar_Value(htmlDoc, "input", "corpo:formulario:ordenacaoPlc"));
            nvcParametros.Add("navDe", Pegar_Value(htmlDoc, "input", "navDe"));
            nvcParametros.Add("detCorrPlc", Pegar_Value(htmlDoc, "input", "detCorrPlc"));
            nvcParametros.Add("tabCorrPlc", Pegar_Value(htmlDoc, "input", "tabCorrPlc"));
            nvcParametros.Add("detCorrPlcPaginado", Pegar_Value(htmlDoc, "input", "detCorrPlcPaginado"));
            nvcParametros.Add("exibeEdDocPlc", Pegar_Value(htmlDoc, "input", "exibeEdDocPlc"));
            nvcParametros.Add("indExcDetPlc", Pegar_Value(htmlDoc, "input", "indExcDetPlc"));
            nvcParametros.Add("corpo:formulario:alertaAlteracaoPlc", Pegar_Value(htmlDoc, "input", "corpo:formulario:alertaAlteracaoPlc"));
            nvcParametros.Add("org.apache.myfaces.trinidad.faces.FORM", Pegar_Value(htmlDoc, "input", "org.apache.myfaces.trinidad.faces.FORM"));
            nvcParametros.Add("_noJavaScript", Pegar_Value(htmlDoc, "input", "_noJavaScript"));
            nvcParametros.Add("javax.faces.ViewState", Pegar_Value(htmlDoc, "input", "javax.faces.ViewState"));
            nvcParametros.Add("source", "corpo:formulario:botaoAcaoPesquisar");

            NameValueCollection parametros = new NameValueCollection();

            htmlDoc = this.HttpPost(urlSite, nvcParametros);

            nodesForm = htmlDoc.DocumentNode.SelectNodes("//button [@class='bt af_commandButton']");

            if ((nodesForm != null) && (nodesForm.Count > 0))
            {
                string strParametroSource = nodesForm[0].Attributes["onclick"].Value.ToString();

                int intInicio = strParametroSource.IndexOf('{'),
                        intFim = strParametroSource.IndexOf('}');

                if ((intInicio > -1) && (intFim > -1))
                {
                    strParametroSource = strParametroSource.Substring(intInicio + 1, intFim - 1 - intInicio).Replace("source:", "").Replace("'", "");

                    nvcParametros = new NameValueCollection();
                    nvcParametros.Add("corpo:formulario:dataIni", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
                    nvcParametros.Add("corpo:formulario:dataFim", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
                    nvcParametros.Add("corpo:formulario:tipoCaderno", "1");
                    nvcParametros.Add("corpo:formulario:tribunal", "0");
                    nvcParametros.Add("corpo:formulario:ordenacaoPlc", Pegar_Value(htmlDoc, "input", "corpo:formulario:ordenacaoPlc"));
                    nvcParametros.Add("navDe", Pegar_Value(htmlDoc, "input", "navDe"));
                    nvcParametros.Add("detCorrPlc", Pegar_Value(htmlDoc, "input", "detCorrPlc"));
                    nvcParametros.Add("tabCorrPlc", Pegar_Value(htmlDoc, "input", "tabCorrPlc"));
                    nvcParametros.Add("detCorrPlcPaginado", Pegar_Value(htmlDoc, "input", "detCorrPlcPaginado"));
                    nvcParametros.Add("exibeEdDocPlc", Pegar_Value(htmlDoc, "input", "exibeEdDocPlc"));
                    nvcParametros.Add("indExcDetPlc", Pegar_Value(htmlDoc, "input", "indExcDetPlc"));
                    nvcParametros.Add("org.apache.myfaces.trinidad.faces.FORM", Pegar_Value(htmlDoc, "input", "org.apache.myfaces.trinidad.faces.FORM"));
                    nvcParametros.Add("_noJavaScript", Pegar_Value(htmlDoc, "input", "_noJavaScript"));
                    nvcParametros.Add("javax.faces.ViewState", Pegar_Value(htmlDoc, "input", "javax.faces.ViewState"));
                    nvcParametros.Add("source", "corpo:formulario:plcLogicaItens:0:j_id132");

                    return HttpDownload(urlSite, nvcParametros);
                }
            }

            return null;

        }

        private string Pegar_Value(HtmlDocument docTribunal, string strTag, string strName)
        {
            string strValue = string.Empty;

            try
            {
                HtmlNodeCollection nodesValue = docTribunal.DocumentNode.SelectNodes("//" + strTag + " [@name='" + strName + "']");

                if ((nodesValue != null) && (nodesValue.Count > 0))
                    strValue = nodesValue[0].Attributes["value"].Value.ToString();
            }
            catch { strValue = string.Empty; }

            return strValue;
        }
    }
}
