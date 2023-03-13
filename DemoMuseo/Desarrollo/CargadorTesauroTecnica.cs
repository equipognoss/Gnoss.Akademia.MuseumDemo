using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Desarrollo
{
    class CargadorTesauroTecnica
    {
        public ResourceApi mResourceApiCargadorTesauroTecnica;

        public CargadorTesauroTecnica(ResourceApi mResourceApi)
        {
            this.mResourceApiCargadorTesauroTecnica = mResourceApi;
        }

        public void cargarTesauroTécnicas()
        {
            List<ElemntoTesauro> l_nivel1 = new List<ElemntoTesauro>() {
                new ElemntoTesauro("1_n1", "Técnicas Pictóricas"),
                new ElemntoTesauro("2_n1", "Técnicas Escultóricas y de Obtención de Formas"),
                new ElemntoTesauro("3_n1", "Técnicas Decorativas y Acabados de Superficies"),
                new ElemntoTesauro("4_n1", "Técnicas de Dibujo"),
                new ElemntoTesauro("5_n1", "Técnicas de Fotografía"),
            };

            List<ElemntoTesauro> l_nivel2 = new List<ElemntoTesauro>() {
                new ElemntoTesauro("p1_1_n2", "Grisalla"),
                new ElemntoTesauro("p1_2_n2", "Óleo"),
                new ElemntoTesauro("p2_1_n2", "Modelado"),
                new ElemntoTesauro("p2_2_n2", "Moldeado"),
                new ElemntoTesauro("p3_1_n2", "Policromato"),
            };

            mResourceApiCargadorTesauroTecnica.ChangeOntoly("pma_taxonomy");
            Pma_taxonomyOntology.Collection tesauroTipoBase = new Pma_taxonomyOntology.Collection();
            string base_uri = "http://testing.gnoss.com/items/tpm_tec_";
            tesauroTipoBase.Dc_source = "pma_taxonomy";
            tesauroTipoBase.Skos2_scopeNote = "tpm_tec";

            /*Asigno los hijos que van a estar en el primer nivel*/
            string uri = "";
            tesauroTipoBase.IdsSkos2_member = new List<string>();
            foreach (var elemento in l_nivel1)
            {
                uri = base_uri + elemento.id;
                tesauroTipoBase.IdsSkos2_member.Add(uri);
            }
            mResourceApiCargadorTesauroTecnica.LoadSecondaryResource(tesauroTipoBase.ToGnossApiResource(mResourceApiCargadorTesauroTecnica, "tpm_tec"));

            /*Cargo el primer nivel*/
            Dictionary<string, string> nombre_Uri_Tax = new Dictionary<string, string>();
            foreach (var elemento_n1 in l_nivel1)
            {
                Pma_taxonomyOntology.Concept concept_tax = new Pma_taxonomyOntology.Concept();
                List<string> list_ident_nivel_2 = new List<string>();
                concept_tax.Dc_identifier = elemento_n1.id;
                concept_tax.Dc_source = "pma_taxonomy";
                concept_tax.Skos2_prefLabel = elemento_n1.nombre;
                concept_tax.Skos2_symbol = "1";

                //Asigno al padre, que en el primer nivel es el propio Tesauro
                concept_tax.IdsSkos2_broader = new List<string>()
                {
                    "http://testing.gnoss.com/items/tpm_tec"
                };

                concept_tax.IdsSkos2_narrower = new List<string>();
                foreach (var elemento_n2 in l_nivel2)
                {
                    string id_n1 = elemento_n1.id.Split('_')[0];
                    string id_padre_n2 = elemento_n2.id.Split('_')[0].Substring(1, 1);
                    if (id_n1.Equals(id_padre_n2))
                    {
                        uri = base_uri + elemento_n2.id;
                        concept_tax.IdsSkos2_narrower.Add(uri);
                    }
                }
                mResourceApiCargadorTesauroTecnica.LoadSecondaryResource(concept_tax.ToGnossApiResource(mResourceApiCargadorTesauroTecnica, "tpm_tec_" + elemento_n1.id));
            }

            /*Cargo el segundo nivel*/
            Pma_taxonomyOntology.Concept concept_tax_2 = new Pma_taxonomyOntology.Concept();

            foreach (var elemento_n2 in l_nivel2)
            {
                concept_tax_2 = new Pma_taxonomyOntology.Concept();
                concept_tax_2.Dc_identifier = elemento_n2.id;
                concept_tax_2.Dc_source = "pma_taxonomy";
                concept_tax_2.Skos2_symbol = "2";
                concept_tax_2.Skos2_prefLabel = elemento_n2.nombre;
                concept_tax_2.IdsSkos2_broader = new List<string>();
                ElemntoTesauro padre = l_nivel1.Where(e => e.id.Split('_')[0].Equals(elemento_n2.id.Split('_')[0].Substring(1, 1))).Last();
                string uriPadre = base_uri + padre.id;
                concept_tax_2.IdsSkos2_broader.Add(uriPadre);
                mResourceApiCargadorTesauroTecnica.LoadSecondaryResource(concept_tax_2.ToGnossApiResource(mResourceApiCargadorTesauroTecnica, "tpm_tec_" + elemento_n2.id));
            }
        }

        public void borrarNivelesTesauro(string nivel)
        {
            mResourceApiCargadorTesauroTecnica.ChangeOntoly("pma_taxonomy");
            //Consulta -> Todos los de un nivel.
            string select = string.Empty, where = string.Empty;
            string pOntology = "pma_taxonomy";
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p <http://www.w3.org/2008/05/skos#Concept>";
            //where += $@"FILTER regex(str(?s), 'http://testing.gnoss.com/items/carqc_{nivel}_').";
            where += $@"FILTER regex(str(?s), '*n{nivel}').";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApiCargadorTesauroTecnica.VirtuosoQuery(select, where, pOntology);
            List<string> listaUrisABorrar = new List<string>();
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    listaUrisABorrar.Add(fila["s"].value);
                }
                mResourceApiCargadorTesauroTecnica.DeleteSecondaryEntitiesList(ref listaUrisABorrar);
            }
        }

        public void borrarTesauroEntero()
        {
            mResourceApiCargadorTesauroTecnica.ChangeOntoly("pma_taxonomy");
            //Consulta -> Todos los de un nivel.
            string select = string.Empty, where = string.Empty;
            string pOntology = "pma_taxonomy";
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p 'pma_taxonomy'";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApiCargadorTesauroTecnica.VirtuosoQuery(select, where, pOntology);
            List<string> listaUrisABorrar = new List<string>();
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    listaUrisABorrar.Add(fila["s"].value);
                }
                mResourceApiCargadorTesauroTecnica.DeleteSecondaryEntitiesList(ref listaUrisABorrar);
            }
        }
    }

    public class ElemntoTesauro
    {
        public string id { get; set; }
        public string nombre { get; set; }

        public ElemntoTesauro(string id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }
}
