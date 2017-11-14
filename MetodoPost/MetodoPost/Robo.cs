using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodoPost
{
    /// <summary>
    /// Classe para utilização do Web Request.
    /// </summary>
    public class Robo
    {
        public RoboWebClient RoboWebClient { get; set; }
        
        /// <summary>
        /// Métodos para efetuar chamadas via GET.
        /// </summary>
        /// <param name="url">Url a ser pesquisada.</param>
        /// <returns>HtmlDocumento - utilizado para facilitar o parse do Html.</returns>
        public HtmlDocument HttpGet(string url)
        {
            lock (this)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(RoboWebClient.DownloadString(url));

                return htmlDocument;
            }
        }

        /// <summary>
        /// Método para efetuar chamadas via POST
        /// </summary>
        /// <param name="url">Url a ser pesquisada.</param>
        /// <param name="parametros">Parâmetros da página.</param>
        /// <returns>HtmlDocumento - utilizado para facilitar o parse do Html.</returns>
        public HtmlDocument HttpPost(string url, NameValueCollection parametros)
        {
            var htmlDocument = new HtmlDocument();
            byte[] pagina = RoboWebClient.UploadValues(url, parametros);
            htmlDocument.LoadHtml(Encoding.Default.GetString(pagina, 0, pagina.Count()));

            return htmlDocument;
        }

        /// <summary>
        /// Método para efetuar chamadas via POST
        /// </summary>
        /// <param name="url">Url a ser pesquisada.</param>
        /// <param name="parametros">Parâmetros da página.</param>
        /// <returns>HtmlDocumento - utilizado para facilitar o parse do Html.</returns>
        public byte[] HttpDownload(string url, NameValueCollection parametros)
        {
            var htmlDocument = new HtmlDocument();
            byte[] dados = RoboWebClient.UploadValues(url, parametros);

            return dados;
        }
    }

}
