using Gnoss.ApiWrapper.ApiModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Desarrollo
{
    class ConsultorActoresWikidata
    {
        public ConsultorActoresWikidata() { }

        public void consguirActores()
        {
            /*string consulta = @"SELECT ?actor ?actorNombre
                    {
                       ?actor wdt:P106 wd:Q10800557;
                             rdfs:label ?actorNombre.
                            
                      FILTER(LANG(?actorNombre) = ""en"").
                      FILTER(?actorNombre = ""Leonardo DiCaprio""@en).
                    }";*/



            string consulta = @"SELECT ?actor ?actorNombre ?fechaNacimiento ?nombreLugarNacimiento
                                {
                                   ?actor wdt:P106 wd:Q10800557;
                                          wdt:P569 ?fechaNacimiento;
                                          wdt:P19 ?lugarNacimiento;
                                         rdfs:label ?actorNombre.
                                  ?lugarNacimiento rdfs:label ?nombreLugarNacimiento.  
                                  FILTER(LANG(?actorNombre) = ""en"").
                                  FILTER(LANG(?nombreLugarNacimiento) = ""en"").
                                  #FILTER(?actorNombre = ""Leonardo DiCaprio""@en).
                                  #FILTER(CONTAINS(?actorNombre, ""Dicaprio""))
                                  FILTER(CONTAINS(LCASE(?actorNombre), ""dicaprio""))
                                }";

            SparqlObject datos = null;
            string urlConsulta = "https://query.wikidata.org/sparql";
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Collections.Specialized.NameValueCollection parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("query", consulta);
            webClient.Headers.Add("accept", "application/sparql-results+json");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            byte[] responseArray = null;
            int numIntentos = 0;
            string error = "";
            while (responseArray == null && numIntentos < 5)
            {
                numIntentos++;
                try
                {
                    responseArray = webClient.UploadValues(urlConsulta, "POST", parametros);
                    webClient.Encoding = Encoding.UTF8;
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                    webClient.Headers.Add("accept", "application/sparql-results+json");
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            string jsonRespuesta = System.Text.Encoding.UTF8.GetString(responseArray);
            if (!string.IsNullOrEmpty(jsonRespuesta))
            {
                datos = JsonConvert.DeserializeObject<SparqlObject>(jsonRespuesta);
            }
        }
    }
}
