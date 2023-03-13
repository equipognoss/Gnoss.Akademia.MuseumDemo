using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Newtonsoft.Json;
using Pma_obraOntology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Desarrollo
{

    public class CargadorObras
    {
        public ResourceApi mResourceApiCargador;
        public ResourceApi mResourceApiCuidadoPradoCargador;

        public CargadorObras(ResourceApi mResourceApi, ResourceApi mResourceApiCuidadoPrado)
        {
            this.mResourceApiCargador = mResourceApi;
            this.mResourceApiCuidadoPradoCargador = mResourceApiCuidadoPrado;
        }

        private void cargarObrasDeTodosLosAutores()
        {
            //CONSEGUIR TODOS LOS IDS DE LOS AUTORES CARGADOS
            List<string> l_urisDeAutores = GetUrisDeAutores("pma_autor");
            if (!l_urisDeAutores.Any()) { Console.WriteLine($"No se han encontrado URIs de Autores cargados\n"); return; }
            foreach (var uriAutor in l_urisDeAutores)
            {
                List<Tuple<String, String, String>> l_infoAutor = GetInfoAutor(uriAutor);
                string idAutor = l_infoAutor.Where(i => i.Item2 == "http://museodelprado.es/ontologia/pradomuseum.owl#identifier").ToList().Select(i => i.Item3).FirstOrDefault();
                string nombreAutor = l_infoAutor.Where(i => i.Item2 == "http://museodelprado.es/ontologia/ecidoc.owl#p131_E82_p102_has_title").ToList().Select(i => i.Item3).FirstOrDefault();
                if (!String.IsNullOrEmpty(idAutor))
                {
                    List<E22_Man_Made_Object> l_obrasDeAutor = cargarObraDeUnAutorPorId(idAutor, uriAutor); //BUSCAR TODAS LAS OBRAS DE ESE AUTOR EN PRE DEL PRADO Y CARGARLAS                    
                    escribirJSONObra(l_obrasDeAutor, idAutor, nombreAutor);
                }
                Console.WriteLine($"No se ha podido cargar la obra de {uriAutor} porque no se ha encontrado su id\n");
            }
        }

        private List<String> GetUrisDeAutores(string p_pOntology, string idAutorEnPrado = "")
        {
            string pOntology = p_pOntology;
            if (p_pOntology == "pmauthor") { mResourceApiCuidadoPradoCargador.ChangeOntoly(pOntology); }
            else if (p_pOntology == "pma_autor") { mResourceApiCargador.ChangeOntoly(pOntology); }
            else { return null; }
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT DISTINCT ?s";
            where += $@" WHERE {{ ";
            where += $@"?s a <http://www.cidoc-crm.org/cidoc-crm#E39_Actor>.";
            if (!String.IsNullOrEmpty(idAutorEnPrado)) { where += $@"?s <http://museodelprado.es/ontologia/pradomuseum.owl#identifier> '{idAutorEnPrado}'"; }
            where += $@"}}";
            SparqlObject resultadoQuery = null;
            if (p_pOntology == "pmauthor") { resultadoQuery = mResourceApiCuidadoPradoCargador.VirtuosoQuery(select, where, pOntology); }
            else if (p_pOntology == "pma_autor") { resultadoQuery = mResourceApiCargador.VirtuosoQuery(select, where, pOntology); }
            else { return null; }
            List<String> l_UrisAutores = new List<string>();
            //Si está ya en el grafo, obtengo la URI
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (var item in resultadoQuery.results.bindings)
                {
                    l_UrisAutores.Add(item["s"].value);
                }
            }
            return l_UrisAutores;
        }

        private List<Tuple<string, string, string>> GetInfoAutor(string p_uriAutor)
        {
            List<Tuple<string, string, string>> l_infoAutor = new List<Tuple<string, string, string>>();
            string pOntology = "pma_autor";
            mResourceApiCargador.ChangeOntoly(pOntology);
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT ?p ?o";
            where += $@" WHERE {{ ";
            where += $@"<{p_uriAutor}> ?p ?o";
            where += $@"}}";
            SparqlObject resultadoQuery = mResourceApiCargador.VirtuosoQuery(select, where, pOntology);
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (var item in resultadoQuery.results.bindings)
                {
                    Tuple<string, string, string> t_infoAutor = new Tuple<string, string, string>(
                        p_uriAutor,
                        item["p"].value,
                        item["o"].value);
                    l_infoAutor.Add(t_infoAutor);
                }
            }
            return l_infoAutor;
        }

        private List<E22_Man_Made_Object> cargarObraDeUnAutorPorId(string idAutor, string uriAutorComunidad)
        {
            //PRADO_PRE
            List<String> l_urisObrasAutor = GetUrisObrasDeAutor("pmartwork", idAutor);
            List<E22_Man_Made_Object> l_obrasDeAutor = new List<E22_Man_Made_Object>();
            int contObrasMax = 0;
            foreach (var uriObraPrado in l_urisObrasAutor)
            {
                if (contObrasMax == 10) break;
                E22_Man_Made_Object obra = DatosObraPorUri("pmartwork", uriObraPrado, uriAutorComunidad);
                l_obrasDeAutor.Add(obra);
                contObrasMax++;
            }

            foreach (var obra in l_obrasDeAutor)
            {
                mResourceApiCargador.ChangeOntoly("pma_obra");
                ComplexOntologyResource objeto = obra.ToGnossApiResource(mResourceApiCargador, null, Guid.NewGuid(), Guid.NewGuid());
                string result = mResourceApiCargador.LoadComplexSemanticResource(objeto);
            }
            return l_obrasDeAutor;
        }

        private List<String> GetUrisObrasDeAutor(string p_pOntology, string idAutor = "")
        {
            string pOntology = p_pOntology;
            mResourceApiCuidadoPradoCargador.ChangeOntoly(pOntology);
            string uriAutor = GetUrisDeAutores("pmauthor", idAutor).First();
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT DISTINCT ?uriObra";
            where += $@" WHERE {{ ";
            where += $@"?uriObra rdf:type <http://www.cidoc-crm.org/cidoc-crm#E22_Man-Made_Object>.";
            where += $@"?uriObra <http://www.cidoc-crm.org/cidoc-crm#p14_carried_out_by> ?autoria.";
            where += $@"?autoria <http://museodelprado.es/ontologia/pradomuseum.owl#author> <{uriAutor}>.";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApiCuidadoPradoCargador.VirtuosoQuery(select, where, pOntology);
            List<String> l_UrisObras = new List<string>();
            //Si está ya en el grafo, obtengo la URI
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (var item in resultadoQuery.results.bindings)
                {
                    l_UrisObras.Add(item["uriObra"].value);
                }
            }
            return l_UrisObras;
        }

        private E22_Man_Made_Object DatosObraPorUri(string p_pOntology, string uriObra, string uriAutorComunidad)
        {
            string pOntology = p_pOntology;
            mResourceApiCuidadoPradoCargador.ChangeOntoly(pOntology);
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT DISTINCT ?p ?valor";
            where += $@" WHERE {{ ";
            where += $@"<{uriObra}> ?p ?valor.";
            where += $@"FILTER(?p LIKE <http://www.cidoc-crm.org/cidoc-crm#p1_is_identified_by> ";
            where += $@"OR (?p LIKE <http://www.cidoc-crm.org/cidoc-crm#p3_has_note> AND lang(?valor) = 'es') ";
            where += $@"OR ?p LIKE <http://museodelprado.es/ontologia/ecidoc.owl#p102_E35_p3_has_title> ";
            where += $@"OR ?p LIKE <http://www.cidoc-crm.org/cidoc-crm#p14_carried_out_by> ";
            where += $@"OR ?p LIKE <http://www.cidoc-crm.org/cidoc-crm#p130i_features_are_also_found_on> ";
            where += $@"OR ?p LIKE <http://www.cidoc-crm.org/cidoc-crm#p108i_E12_p126_employed_medium> ";
            where += $@"OR ?p LIKE <http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p32_used_general_technique> ";
            where += $@"OR ?p LIKE <http://www.cidoc-crm.org/cidoc-crm#p70i_is_documented_in>) ";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApiCuidadoPradoCargador.VirtuosoQuery(select, where, pOntology);
            List<Tuple<string, string, string>> l_AtrubutosObras = new List<Tuple<string, string, string>>();
            //Si está ya en el grafo, obtengo la URI
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (var item in resultadoQuery.results.bindings)
                {
                    Tuple<string, string, string> t_TripreObra = new Tuple<string, string, string>(
                    uriObra,
                    item["p"].value,
                    item["valor"].value);
                    l_AtrubutosObras.Add(t_TripreObra);
                }
            }
            E22_Man_Made_Object obra = GetObjtObra(l_AtrubutosObras, uriAutorComunidad);  //Trasformo los triples en una obra de la API  
            return obra;
        }

        private E22_Man_Made_Object GetObjtObra(List<Tuple<String, String, String>> p_l_infoObra, string uriAutorComunidad)
        {
            E22_Man_Made_Object obra = new E22_Man_Made_Object();
            obra.Cidoc_p14_carried_out_by = new List<E22_E39_Man_Made_Object__Actor>();
            obra.Cidoc_p70i_is_documented_in = new List<C1003_Manifestation>();
            int auxContDocumentos = 0; //SOLO VOY A CARGAR UN DOCUMENTO PARA QUE NO TARDE TANTO LA CARGA
            foreach (var triple in p_l_infoObra)
            {
                switch (triple.Item2)
                {
                    case "http://www.cidoc-crm.org/cidoc-crm#p1_is_identified_by":
                        obra.Cidoc_p1_is_identified_by = triple.Item3;
                        break;
                    case "http://www.cidoc-crm.org/cidoc-crm#p3_has_note":
                        obra.Cidoc_p3_has_note = triple.Item3;
                        break;
                    case "http://museodelprado.es/ontologia/ecidoc.owl#p102_E35_p3_has_title":
                        obra.Ecidoc_p102_E35_p3_has_title = triple.Item3;
                        break;
                    case "http://www.cidoc-crm.org/cidoc-crm#p14_carried_out_by":
                        E22_E39_Man_Made_Object__Actor aux_obra_autor = GetEntAuxObraAutorPorURI("pmartwork", triple.Item3, uriAutorComunidad);
                        obra.Cidoc_p14_carried_out_by.Add(aux_obra_autor);
                        break;
                    /*case "http://www.cidoc-crm.org/cidoc-crm#p130i_features_are_also_found_on":    OBRAS RELACIONADAS -- PUEDE HABER VARIAS
                        obra.Cidoc_p130i_features_are_also_found_on 
                        break;
                    case "http://www.cidoc-crm.org/cidoc-crm#p108i_E12_p126_employed_medium":
                        autor.Ecidoc_p100i_E69_p7_death_place = triple.Item3;
                        break;
                    case "http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p32_used_general_technique":
                        autor.Ecidoc_p96_E67_p4_gave_birth_date = triple.Item3;
                        break;*/
                    case "http://www.cidoc-crm.org/cidoc-crm#p70i_is_documented_in":
                        if (auxContDocumentos == 0)
                        {
                            C1003_Manifestation aux_documento = GetEntAuxDocumentoPorURI("pmartwork", triple.Item3);
                            obra.Cidoc_p70i_is_documented_in.Add(aux_documento);
                            auxContDocumentos++;
                        }
                        break;
                    //TODO::Imagen
                    default:
                        Console.Write($"El {triple.Item2} no se ha asignado a ninguna propiedad\n");
                        break;
                }
            }
            return obra;
        }

        private E22_E39_Man_Made_Object__Actor GetEntAuxObraAutorPorURI(string p_pOntology, string uriEntAuxObraAutor, string uriAutorComunidad)
        {
            string pOntology = p_pOntology;
            mResourceApiCuidadoPradoCargador.ChangeOntoly(pOntology);
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT ?uriAutor ?uriAutoria ?orden ?tipoAutoria";
            where += $@" WHERE {{ ";
            where += $@"OPTIONAL{{<{uriEntAuxObraAutor}> <http://museodelprado.es/ontologia/pradomuseum.owl#author> ?uriAutor.}}";
            where += $@"OPTIONAL{{<{uriEntAuxObraAutor}> <http://museodelprado.es/ontologia/pradomuseum.owl#authorship> ?uriAutoria.}}";
            where += $@"OPTIONAL{{<{uriEntAuxObraAutor}> <http://museodelprado.es/ontologia/ecidoc.owl#order> ?orden.}}";
            where += $@"OPTIONAL{{<{uriEntAuxObraAutor}> <http://museodelprado.es/ontologia/ecidoc.owl#p2_has_author_type> ?tipoAutoria.}}";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApiCuidadoPradoCargador.VirtuosoQuery(select, where, pOntology);
            E22_E39_Man_Made_Object__Actor entAuxObraAutor = new E22_E39_Man_Made_Object__Actor();
            //Si está ya en el grafo, obtengo la URI
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (var item in resultadoQuery.results.bindings)
                {
                    entAuxObraAutor.IdPm_author = uriAutorComunidad;
                    if (item.Where(i => i.Key == "orden").Any()) entAuxObraAutor.Ecidoc_order = Int32.Parse(item["orden"].value);
                    if (item.Where(i => i.Key == "tipoAutoria").Any()) entAuxObraAutor.Ecidoc_p2_has_author_type = item["tipoAutoria"].value;
                }
            }
            return entAuxObraAutor;
        }

        private C1003_Manifestation GetEntAuxDocumentoPorURI(string p_pOntology, string uriEntAuxDocumento)
        {
            string pOntology = p_pOntology;
            mResourceApiCuidadoPradoCargador.ChangeOntoly(pOntology);
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT DISTINCT ?fechaDePublicacion ?tituloDePublicacion";
            where += $@" WHERE {{ ";
            where += $@"<{uriEntAuxDocumento}> <http://museodelprado.es/ontologia/efrbrer.owl#P3055_has_date_of_publication_or_distribution> ?fechaDePublicacion.";
            where += $@"<{uriEntAuxDocumento}> <http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation> ?tituloDePublicacion.";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApiCuidadoPradoCargador.VirtuosoQuery(select, where, pOntology);
            C1003_Manifestation entAuxDocumento = new C1003_Manifestation();
            //Si está ya en el grafo, obtengo la URI
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (var item in resultadoQuery.results.bindings)
                {
                    entAuxDocumento.Efrbrer_P3055_has_date_of_publication_or_distribution = item["fechaDePublicacion"].value;
                    entAuxDocumento.Efrbrer_p3020_has_title_of_the_manifestation = item["tituloDePublicacion"].value;
                }
            }
            return entAuxDocumento;
        }

        private static void escribirJSONObra(List<E22_Man_Made_Object> l_obrasDeAutor, string idAutor, string nombreAutor)
        {
            foreach (var obra in l_obrasDeAutor)
            {
                string json = JsonConvert.SerializeObject(obra);
                string path = $@"C:\Users\sdedios\OneDrive - RIAM I+L LAB S.L\Programa carga\PruebaApiGNOSS\PruebaApiGNOSS\JsonObras\{idAutor}_{nombreAutor}\{obra.Ecidoc_p102_E35_p3_has_title}";
                System.IO.Directory.CreateDirectory($@"C:\Users\sdedios\OneDrive - RIAM I+L LAB S.L\Programa carga\PruebaApiGNOSS\PruebaApiGNOSS\JsonObras\{idAutor}_{nombreAutor}");
                System.IO.File.WriteAllText(path, json);
            }
        }
    }
}


