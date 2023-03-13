using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using Newtonsoft.Json;
using Pma_autorOntology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Desarrollo
{

    public class CargadorAutores 
    {
       
        public ResourceApi mResourceApiCargador;

        public CargadorAutores(ResourceApi mResourceApi)
        {
            this.mResourceApiCargador = mResourceApi;
        }

        public void CargarAutores()
        {
            string path_JSON = @"C:\Users\Sergio de Dios\OneDrive - RIAM I+L LAB S.L\Programa carga\PruebaApiGNOSS_en_v5\ConsoleApp2\ConsoleApp2\JsonAutores";
            string[] files = Directory.GetFiles(path_JSON, "*.json");
            List<E39_Actor> l_autores = new List<E39_Actor>();
            foreach (var file in files)
            {
                E39_Actor autor = leerAutoresJson(file);
                l_autores.Add(autor);
                AddAutorToGnossApiResource(autor);
            }
        }

        private E39_Actor leerAutoresJson(string pathFichero)
        {
            string path = pathFichero;
            StreamReader r = new StreamReader(path);
            string jsonString = r.ReadToEnd();
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonString);
            E39_Actor autor = new E39_Actor();
            autor.Pm_period = new List<string>();
            string epoca = "";
            //Asignación de propiedades al objeto
            {

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/pradomuseum.owl#identifier")).Count() != 0)
                {
                    Binding obj = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/pradomuseum.owl#identifier")).First();
                    autor.Pm_identifier = obj.valor.value;
                }

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p131_E82_p102_has_title")).Count() != 0)
                {
                    autor.Ecidoc_p131_E82_p102_has_title = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, string>();
                    List<Binding> objt = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p131_E82_p102_has_title")).ToList();
                    foreach (var item in objt)
                    {
                        if (item.valor.XmlLang.Equals("es")) { autor.Ecidoc_p131_E82_p102_has_title.Add(GnossBase.GnossOCBase.LanguageEnum.es, item.valor.value); }
                        else if (item.valor.XmlLang.Equals("en")) { autor.Ecidoc_p131_E82_p102_has_title.Add(GnossBase.GnossOCBase.LanguageEnum.en, item.valor.value); }
                    }
                }

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://www.cidoc-crm.org/cidoc-crm#p3_has_note")).Count() != 0)
                {
                    List<Binding> objt = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://www.cidoc-crm.org/cidoc-crm#p3_has_note")).ToList();
                    foreach (var item in objt)
                    {
                        if (item.valor.XmlLang.Equals("es")) { autor.Cidoc_p3_has_note = item.valor.value; }
                        //else if (item.valor.XmlLang.Equals("en")) { autor.Ecidoc_p131_E82_p102_has_title.Add(GnossBase.GnossOCBase.LanguageEnum.en, $"{{obj.valor.value}}"); }
                    }
                }

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_date")).Count() != 0)
                {
                    Binding obj = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_date")).First();
                    autor.Ecidoc_p100i_E69_p4_death_date = obj.valor.value;
                }

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_year")).Count() != 0)
                {
                    Binding obj = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_year")).First();
                    autor.Ecidoc_p100i_E69_p4_death_year = Int32.Parse(obj.valor.value);

                    //Asigno el autor a una época
                    epoca = ObtenerEpoca(autor.Ecidoc_p100i_E69_p4_death_year);
                    if (!String.IsNullOrEmpty(epoca)) { autor.Pm_period.Add(epoca); }
                }

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p7_death_place")).Count() != 0)
                {
                    Binding obj = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p7_death_place")).First();
                    autor.Ecidoc_p100i_E69_p7_death_place = obj.valor.value;
                }

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_date")).Count() != 0)
                {
                    Binding obj = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_date")).First();
                    autor.Ecidoc_p96_E67_p4_gave_birth_date = obj.valor.value;
                }

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_year")).Count() != 0)
                {
                    Binding obj = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_year")).First();
                    autor.Ecidoc_p96_E67_p4_gave_birth_year = Int32.Parse(obj.valor.value);

                    //Asigno el autor a una época
                    epoca = ObtenerEpoca(autor.Ecidoc_p96_E67_p4_gave_birth_year);
                    if (!String.IsNullOrEmpty(epoca)) { autor.Pm_period.Add(epoca); }
                }

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p7_gave_birth_place")).Count() != 0)
                {
                    Binding obj = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p7_gave_birth_place")).First();
                    autor.Ecidoc_p96_E67_p7_gave_birth_place = obj.valor.value;
                }

                if (myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p65_E36_shows_visual_item")).Count() != 0)
                {
                    Binding obj = myDeserializedClass.results.bindings.Where(b => b.p.value.Equals("http://museodelprado.es/ontologia/ecidoc.owl#p65_E36_shows_visual_item")).First();
                    if (String.IsNullOrEmpty(autor.Ecidoc_p65_E36_shows_visual_item)) autor.Ecidoc_p65_E36_shows_visual_item = "Asignar despues"; //obj.valor.value;
                }
            }
            return autor;
        }

        private string ObtenerEpoca(int? anio)
        {
            if (anio >= 1300 && anio < 1400) { return "s.XV"; }
            if (anio >= 1400 && anio < 1500) { return "s.XV"; }
            if (anio >= 1500 && anio < 1600) { return "s.XVI"; }
            if (anio >= 1600 && anio < 1700) { return "s.XVII"; }
            if (anio >= 1700 && anio < 1800) { return "s.XVIII"; }
            if (anio >= 1800 && anio < 1900) { return "s.XIX"; }
            if (anio >= 1900 && anio < 2000) { return "s.XX"; }
            if (anio >= 2000 && anio < 2100) { return "s.XX"; }
            return "";
        }

        private string AddAutorToGnossApiResource(E39_Actor autor)
        {
            mResourceApiCargador.ChangeOntoly("pma_autor");
            ComplexOntologyResource objeto = null;
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            
            /*
            if (autor.Pm_identifier.Equals("2712"))
            { //Si es Velazquez        
                guid1 = new Guid("bb2c2448-ec14-42fa-a663-e372f626b771");
                guid2 = new Guid("4a154c4e-babb-402c-bd90-ab83102fdbe1");
            }

            //Actualizar URL img en la propiedad

            //string primeraCarpeta = guid1.ToString().Substring(0, 2);
            //string segundaCarpeta = guid1.ToString().Substring(0, 4);
            
            if (!String.IsNullOrEmpty(autor.Ecidoc_p65_E36_shows_visual_item))
            {
                autor.Ecidoc_p65_E36_shows_visual_item = null;
                autor.Ecidoc_p65_E36_shows_visual_item = "imagenes/Documentos/imgsem/" + primeraCarpeta + "/" + segundaCarpeta + "/" + guid1.ToString() + "/70dcbd0d-aae0-4825-b70b-e9840f9ba65b.jpg";
                //string pathImg = "https://content3.cdnprado.net/imagenes/Documentos/imgsem/43/4343/434337e9-77e4-4597-a962-ef47304d930d/70dcbd0d-aae0-4825-b70b-e9840f9ba65b.jpg";
            }*/

            objeto = autor.ToGnossApiResource(mResourceApiCargador, null, guid1, guid2);
            string pathImg = @"C:\Users\Sergio de Dios\OneDrive - RIAM I+L LAB S.L\Programa carga\PruebaApiGNOSS\PruebaApiGNOSS\Imagenes\70dcbd0d-aae0-4825-b70b-e9840f9ba65b.jpg";
            //if (String.IsNullOrEmpty(autor.Ecidoc_p65_E36_shows_visual_item)) objeto.AttachImage(File.ReadAllBytes(pathImg), new List<ImageAction>(), "ecidoc:p65_E36_shows_visual_item", true, "jpg");
            string result = mResourceApiCargador.LoadComplexSemanticResource(objeto);
            return result;
        }   
    }
}
