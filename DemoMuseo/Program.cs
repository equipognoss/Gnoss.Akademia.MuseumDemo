using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Pma_autorOntology;
using Pma_obraOntology;
using System.IO;
using Newtonsoft.Json;
using System.Web;
using ConsoleApp2.Desarrollo;

namespace ConsoleApp2
{
    class Program
    {
        public static string NOMBRE_CORT_COMUNIDAD = "";
        public static ResourceApi mResourceApi = null;
        public static ResourceApi mResourceApiCuidadoPrado = null;

        static void Main(string[] args)
        {
            ConsultorActoresWikidata consultorActores = new();
            consultorActores.consguirActores();
            string path_Prueba = $@"C:\Users\Sergio de Dios\OneDrive - RIAM I+L LAB S.L\Programa carga\PruebaApiGNOSS_en_v5\ConsoleApp2\ConsoleApp2\ConfigOAuth\oAuth_akademiaprueba.config";
            mResourceApi = new ResourceApi(path_Prueba);
            //CargadorAutores cargadorAutores = new CargadorAutores(mResourceApi);
            //cargadorAutores.CargarAutores();
            //borrarTesauroEntero();
            //cargarTesauroTécnicas();

            //PruebaGIT3
            //string pathPrado = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}\ConfigOAuth\oAuth_elprado.config";
            //mResourceApiCuidadoPrado = new ResourceApi(pathPrado);

            //CARGAR IMAGEN POR CARGA MASIVA
            /*
            List<AttachedResource> archivos = new List<AttachedResource>();
            AttachedResource img = new AttachedResource();

            img.file_rdf_property = "35c27080-b922-48d1-8560-7fa7b9d6d71d.jpg"; //con el .jpg
            img.file_property_type = 1;
            string pathLocalImg = @"C:\Users\sdedios\OneDrive - RIAM I+L LAB S.L\Programa carga\PruebaApiGNOSS\PruebaApiGNOSS\Imagenes\35c27080-b922-48d1-8560-7fa7b9d6d71d.jpg";
            img.rdf_attached_file = File.ReadAllBytes(pathLocalImg);
            archivos.Add(img);
            SendToServer("http://try.gnoss.com/items/E39_Actor_766fdda1-3d55-4266-b534-29f65bffd04e_a60411f8-62f7-4a9b-8b2e-94fc051f84fd", archivos);
            */

            //CARGAR AUTORES

            string path_JSON = @"C:\Users\Sergio de Dios\OneDrive - RIAM I+L LAB S.L\Programa carga\PruebaApiGNOSS_en_v5\ConsoleApp2\ConsoleApp2\JsonAutores";
            string[] files = Directory.GetFiles(path_JSON, "*.json");
            List<E39_Actor> l_autores = new List<E39_Actor>();
            foreach (var file in files)
            {
                E39_Actor autor = leerAutoresJson(file);
                l_autores.Add(autor);
                AddAutorToGnossApiResource(autor);
            }

            //CARGAR OBRAS Y GENERAR JSON

            cargarObrasDeTodosLosAutores();

            List<String> l_urisDeAutores = GetUrisDeAutores("pma_autor");
            foreach (var uriAutor in l_urisDeAutores)
            {
                List<Tuple<String, String, String>> l_infoAutor = GetInfoAutor(uriAutor);
                //E39_Actor autor = GetObjAutor(l_infoAutor);
                //GetObjAutorUpdateBirthDate(autor,10);
                //UpdateAutorToGnossApiResource(uriAutor, autor);
                //GetRDFString(uriAutor);      
            }

            E39_Actor autorVelzaquez = CreateVelazquez();
            AddAutorToGnossApiResource(autorVelzaquez);

            E22_Man_Made_Object obraMeninas = CreateMeninas();
            AddObraToGnossApiResource(obraMeninas);

            addTriplesMeninasImagen();

            setTriplesMeninas();

            borrarTesauroEntero();
            cargarTesauroTécnicas();
        }

        private static void cargarObrasDeTodosLosAutores()
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

        private static List<E22_Man_Made_Object> cargarObraDeUnAutorPorId(string idAutor, string uriAutorComunidad)
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
                mResourceApi.ChangeOntoly("pma_obra");
                ComplexOntologyResource objeto = obra.ToGnossApiResource(mResourceApi, null, Guid.NewGuid(), Guid.NewGuid());
                string result = mResourceApi.LoadComplexSemanticResource(objeto);
            }
            return l_obrasDeAutor;
        }

        private static void SendToServer(string uriRecursoPricipal, List<SemanticAttachedResource> archivos)
        {
            LoadResourceParams loadResourceParamas = new LoadResourceParams();
            loadResourceParamas.resource_id = mResourceApi.GetShortGuid(uriRecursoPricipal);
            loadResourceParamas.resource_attached_files = archivos;
            mResourceApi.MassiveUploadFiles(loadResourceParamas);
        }

        private static E39_Actor leerAutoresJson(string pathFichero)
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

        private static string ObtenerEpoca(int? anio)
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

        private static string AddObraToGnossApiResource(E22_Man_Made_Object obra)
        {
            mResourceApi.ChangeOntoly("pma_obra");
            ComplexOntologyResource objeto = obra.ToGnossApiResource(mResourceApi, null, Guid.NewGuid(), Guid.NewGuid());
            string result = mResourceApi.LoadComplexSemanticResource(objeto);
            return result;
        }

        private static string AddAutorToGnossApiResource(E39_Actor autor)
        {
            mResourceApi.ChangeOntoly("pma_autor");
            ComplexOntologyResource objeto = null;
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            //objeto = autor.ToGnossApiResource(mResourceApi, null, guid1, guid2);

            if (autor.Pm_identifier.Equals("2712"))
            { //Si es Velazquez        
                guid1 = new Guid("bb2c2448-ec14-42fa-a663-e372f626b771");
                guid2 = new Guid("4a154c4e-babb-402c-bd90-ab83102fdbe1");
            }
            //Actualizar URL img en la propiedad

            string primeraCarpeta = guid1.ToString().Substring(0, 2);
            string segundaCarpeta = guid1.ToString().Substring(0, 4);

            if (!String.IsNullOrEmpty(autor.Ecidoc_p65_E36_shows_visual_item))
            {
                autor.Ecidoc_p65_E36_shows_visual_item = null;
                autor.Ecidoc_p65_E36_shows_visual_item = "imagenes/Documentos/imgsem/" + primeraCarpeta + "/" + segundaCarpeta + "/" + guid1.ToString() + "/70dcbd0d-aae0-4825-b70b-e9840f9ba65b.jpg";
                //string pathImg = "https://content3.cdnprado.net/imagenes/Documentos/imgsem/43/4343/434337e9-77e4-4597-a962-ef47304d930d/70dcbd0d-aae0-4825-b70b-e9840f9ba65b.jpg";

            }

            objeto = autor.ToGnossApiResource(mResourceApi, null, guid1, guid2);

            string pathImg = @"C:\Users\Sergio de Dios\OneDrive - RIAM I+L LAB S.L\Programa carga\PruebaApiGNOSS\PruebaApiGNOSS\Imagenes\70dcbd0d-aae0-4825-b70b-e9840f9ba65b.jpg";
            if (String.IsNullOrEmpty(autor.Ecidoc_p65_E36_shows_visual_item)) objeto.AttachImage(File.ReadAllBytes(pathImg), new List<ImageAction>(), "ecidoc:p65_E36_shows_visual_item", true, "jpg");
            string result = mResourceApi.LoadComplexSemanticResource(objeto);
            return result;
        }

        private static string GetRDFString(string uriAutor)
        {
            Guid guidResource = mResourceApi.GetShortGuid(uriAutor);
            string rdf = mResourceApi.GetRDF(guidResource);
            return rdf;
        }

        private static void UpdateAutorToGnossApiResource(string uriAutor, E39_Actor autor)
        {
            string[] a_ids = uriAutor.Split('_');
            ComplexOntologyResource resourceAutor = autor.ToGnossApiResource(mResourceApi, null, new Guid(a_ids[a_ids.Length - 2]), new Guid(a_ids[a_ids.Length - 1]));
            mResourceApi.ModifyComplexOntologyResource(resourceAutor, false, true);
        }

        private static E39_Actor GetObjAutorUpdateBirthDate(E39_Actor p_autor, int p_years_add)
        {
            E39_Actor autorAux = p_autor;
            /*
            DateTime dateUpdate = ((DateTime)autorAux.Ecidoc_p96_E67_p4_gave_birth_date).AddYears(p_years_add);
            int yearUpdate = ((int)autorAux.Ecidoc_p96_E67_p4_gave_birth_year) + p_years_add;
            autorAux.Ecidoc_p96_E67_p4_gave_birth_date = dateUpdate;
            autorAux.Ecidoc_p96_E67_p4_gave_birth_year = yearUpdate;
            */
            return autorAux;
        }

        private static E39_Actor GetObjAutor(List<Tuple<String, String, String>> p_l_infoAutor)
        {
            E39_Actor autor = new E39_Actor();
            foreach (var triple in p_l_infoAutor)
            {
                switch (triple.Item2)
                {
                    case "http://museodelprado.es/ontologia/ecidoc.owl#p131_E82_p102_has_title":
                        autor.Ecidoc_p131_E82_p102_has_title = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, string>();
                        autor.Ecidoc_p131_E82_p102_has_title.Add(GnossBase.GnossOCBase.LanguageEnum.es, $"{{triple.Item3}} (Español)");
                        autor.Ecidoc_p131_E82_p102_has_title.Add(GnossBase.GnossOCBase.LanguageEnum.en, $"{{triple.Item3}} (Inglés)");
                        break;
                    case "http://museodelprado.es/ontologia/pradomuseum.owl#identifier":
                        autor.Pm_identifier = triple.Item3;
                        break;
                    case "http://www.cidoc-crm.org/cidoc-crm#p3_has_note":
                        autor.Cidoc_p3_has_note = triple.Item3;
                        break;
                    case "http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_date":
                        autor.Ecidoc_p100i_E69_p4_death_date = triple.Item3;
                        break;
                    case "http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_year":
                        autor.Ecidoc_p100i_E69_p4_death_year = Int32.Parse(triple.Item3);
                        break;
                    case "http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p7_death_place":
                        autor.Ecidoc_p100i_E69_p7_death_place = triple.Item3;
                        break;
                    case "http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_date":
                        autor.Ecidoc_p96_E67_p4_gave_birth_date = triple.Item3;
                        break;
                    case "http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_year":
                        autor.Ecidoc_p96_E67_p4_gave_birth_year = Int32.Parse(triple.Item3);
                        break;
                    case "http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p7_gave_birth_place":
                        autor.Ecidoc_p96_E67_p7_gave_birth_place = triple.Item3;
                        break;
                    default:
                        Console.Write($"El {triple.Item3} no se ha asignado a ninguna propiedad");
                        break;
                }
            }
            return autor;
        }

        private static E22_Man_Made_Object GetObjtObra(List<Tuple<String, String, String>> p_l_infoObra, string uriAutorComunidad)
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

        private static C1003_Manifestation GetEntAuxDocumentoPorURI(string p_pOntology, string uriEntAuxDocumento)
        {
            string pOntology = p_pOntology;
            mResourceApiCuidadoPrado.ChangeOntoly(pOntology);
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT DISTINCT ?fechaDePublicacion ?tituloDePublicacion";
            where += $@" WHERE {{ ";
            where += $@"<{uriEntAuxDocumento}> <http://museodelprado.es/ontologia/efrbrer.owl#P3055_has_date_of_publication_or_distribution> ?fechaDePublicacion.";
            where += $@"<{uriEntAuxDocumento}> <http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation> ?tituloDePublicacion.";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApiCuidadoPrado.VirtuosoQuery(select, where, pOntology);
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

        private static E22_E39_Man_Made_Object__Actor GetEntAuxObraAutorPorURI(string p_pOntology, string uriEntAuxObraAutor, string uriAutorComunidad)
        {
                string pOntology = p_pOntology;
                mResourceApiCuidadoPrado.ChangeOntoly(pOntology);
                string select = string.Empty, where = string.Empty;
                select += $@"SELECT ?uriAutor ?uriAutoria ?orden ?tipoAutoria";
                where += $@" WHERE {{ ";
                where += $@"OPTIONAL{{<{uriEntAuxObraAutor}> <http://museodelprado.es/ontologia/pradomuseum.owl#author> ?uriAutor.}}";
                where += $@"OPTIONAL{{<{uriEntAuxObraAutor}> <http://museodelprado.es/ontologia/pradomuseum.owl#authorship> ?uriAutoria.}}";
                where += $@"OPTIONAL{{<{uriEntAuxObraAutor}> <http://museodelprado.es/ontologia/ecidoc.owl#order> ?orden.}}";
                where += $@"OPTIONAL{{<{uriEntAuxObraAutor}> <http://museodelprado.es/ontologia/ecidoc.owl#p2_has_author_type> ?tipoAutoria.}}";
                where += $@"}}";

                SparqlObject resultadoQuery = mResourceApiCuidadoPrado.VirtuosoQuery(select, where, pOntology);
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

        private static List<Tuple<string, string, string>> GetInfoAutor(string p_uriAutor)
        {
            List<Tuple<string, string, string>> l_infoAutor = new List<Tuple<string, string, string>>();
            string pOntology = "pma_autor";
            mResourceApi.ChangeOntoly(pOntology);
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT ?p ?o";
            where += $@" WHERE {{ ";
            where += $@"<{p_uriAutor}> ?p ?o";
            where += $@"}}";
            SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology);
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

        private static List<String> GetUrisDeAutores(string p_pOntology, string idAutorEnPrado = "")
        {
            string pOntology = p_pOntology;
            if (p_pOntology == "pmauthor") { mResourceApiCuidadoPrado.ChangeOntoly(pOntology); }
            else if (p_pOntology == "pma_autor") { mResourceApi.ChangeOntoly(pOntology); }
            else { return null; }
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT DISTINCT ?s";
            where += $@" WHERE {{ ";
            where += $@"?s a <http://www.cidoc-crm.org/cidoc-crm#E39_Actor>.";
            if (!String.IsNullOrEmpty(idAutorEnPrado)) { where += $@"?s <http://museodelprado.es/ontologia/pradomuseum.owl#identifier> '{idAutorEnPrado}'"; }
            where += $@"}}";
            SparqlObject resultadoQuery = null;
            if (p_pOntology == "pmauthor") { resultadoQuery = mResourceApiCuidadoPrado.VirtuosoQuery(select, where, pOntology); }
            else if (p_pOntology == "pma_autor") { resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology); }
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

        //Método que obtiene las obras de un autor por su ID del Prado
        private static List<String> GetUrisObrasDeAutor(string p_pOntology, string idAutor = "")
        {
            string pOntology = p_pOntology;
            mResourceApiCuidadoPrado.ChangeOntoly(pOntology);
            string uriAutor = GetUrisDeAutores("pmauthor", idAutor).First();
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT DISTINCT ?uriObra";
            where += $@" WHERE {{ ";
            where += $@"?uriObra rdf:type <http://www.cidoc-crm.org/cidoc-crm#E22_Man-Made_Object>.";
            where += $@"?uriObra <http://www.cidoc-crm.org/cidoc-crm#p14_carried_out_by> ?autoria.";
            where += $@"?autoria <http://museodelprado.es/ontologia/pradomuseum.owl#author> <{uriAutor}>.";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApiCuidadoPrado.VirtuosoQuery(select, where, pOntology);
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

        private static E22_Man_Made_Object DatosObraPorUri(string p_pOntology, string uriObra, string uriAutorComunidad)
        {
            string pOntology = p_pOntology;
            mResourceApiCuidadoPrado.ChangeOntoly(pOntology);
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

            SparqlObject resultadoQuery = mResourceApiCuidadoPrado.VirtuosoQuery(select, where, pOntology);
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

        private static DateTime GetDatetimeFromVirtuoso(String p_fechaVirtuoso)
        {

            int anio = Int32.Parse(p_fechaVirtuoso.Substring(0, 4));
            if (p_fechaVirtuoso.Length > 4)
            {
                int mes = Int32.Parse(p_fechaVirtuoso.Substring(4, 2));
                int dia = Int32.Parse(p_fechaVirtuoso.Substring(6, 2));
                return new DateTime(anio, mes, dia);
            }
            return new DateTime().AddYears(anio);
        }

        private static void DeleteAutorToGnossApiResource(string uriAutor)
        {
            mResourceApi.PersistentDelete(mResourceApi.GetShortGuid(uriAutor), true, true);
        }

        private static E39_Actor CreateVelazquez()
        {
            E39_Actor autor = new E39_Actor();
            autor.Pm_identifier = "2712";
            autor.Ecidoc_p100i_E69_p4_death_date = "1660";//new DateTime(1660, 1, 1);
            autor.Ecidoc_p100i_E69_p4_death_year = 1660;
            autor.Ecidoc_p100i_E69_p7_death_place = "Madrid";
            autor.Ecidoc_p131_E82_p102_has_title = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, string>();

            autor.Ecidoc_p131_E82_p102_has_title.Add(GnossBase.GnossOCBase.LanguageEnum.es, "Velázquez, Diego Rodríguez de Silva y (Español)");


            autor.Ecidoc_p131_E82_p102_has_title.Add(GnossBase.GnossOCBase.LanguageEnum.en, "Velázquez, Diego Rodríguez de Silva y (Inglés)");

            autor.Cidoc_p3_has_note = "Adoptó el apellido de su madre, según uso frecuente en Andalucía, firmando ''Diego Velázquez'' o ''Diego de Silva Velázquez''. Estudió y practicó el arte de la pintura en su ciudad natal hasta cumplir los veinticuatro años, cuando se trasladó con su familia a Madrid y entró a servir al rey desde entonces hasta su muerte en 1660. Gran parte de su obra iba destinada a las colecciones reales y pasó luego al Prado, donde se conserva. La mayoría de los cuadros pintados en Sevilla, en cambio, ha ido a parar a colecciones extranjeras, sobre todo a partir del siglo XIX. A pesar del creciente número de documentos que tenemos relacionados con la vida y obra del pintor, dependemos para muchos datos de sus primeros biógrafos. Francisco Pacheco, maestro y después suegro de Velázquez, en un tratado terminado en 1638 y publicado en 1649, da importantes fragmentos de información acerca de su aprendizaje, sus primeros años en la corte y su primer viaje a Italia, con muchos detalles personales. La primera biografía completa -la de Antonio Palomino- fue publicada en 1724, más de sesenta años después de la muerte de Velázquez, pero tiene el valor de haberse basado en unas notas biográficas redactadas por uno de los últimos discípulos del pintor, Juan de Alfaro. Palomino, por otra parte, como pintor de corte, conocía a fondo las obras de Velázquez que se encontraban en las colecciones reales, y había tratado, además, con personas que habían coincidido de joven con el pintor. ­Palomino añade mucho a la información fragmentaria dada por Pacheco y aporta importantes datos de su segundo viaje a Italia, su actividad como pintor de cámara, como funcionario de Palacio y encargado de las obras de arte para el Rey. Nos proporciona asimismo una lista de las mercedes que le hizo Felipe IV junto con los oficios que desempeñaba en la Casa Real, y el texto del epitafio redactado en latín por Juan de Alfaro y su hermano médico, dedicado al ''eximio pintor de Sevilla'' (''Hispalensis. Pictor eximius''). Sevilla, en tiempo de Velázquez, era una ciudad de enorme riqueza, centro del comercio del Nuevo Mundo, sede eclesiástica importantísima, cuna de los grandes pintores religiosos del siglo y conservadora de su arte. Según Palomino, Velázquez fue discípulo de Francisco de Herrera antes de ingresar con once años en el estudio de Francisco Pacheco, el más prestigioso maestro en Sevilla por entonces, hombre culto, escritor y poeta. Después de seis años en aquella ''cárcel dorada del arte'', como la llamó Palomino, Velázquez fue aprobado como ''maestro pintor de ymagineria y al ólio [...] con licencia de practicar su arte en todo el reino, tener tienda pública y aprendices''. No sabemos si se aprovechó de la licencia en cuanto a lo de tener aprendices en Sevilla, pero no es inverosímil, dadas las repeticiones hechas de los bodegones que pintó en la capital andaluza. En 1618 Pacheco le casó con su hija, ''movido de su virtud [...] y de las esperanzas de su natural y grande ingenio''. Nacieron luego en Sevilla las dos hijas del pintor. Escribiendo cuando su discípulo y yerno ya estaba establecido en la corte, Pacheco atribuye su éxito a sus estudios, insistiendo en la importancia de trabajar del natural y de hacer dibujos. Velázquez tenía un joven aldeano que posaba para él, al parecer, y aunque no se ha conservado ningún dibujo de los que sacara de este modelo, llama la atención la repetición de las mismas caras y personas en algunas de sus obras juveniles. Pacheco no menciona ninguna de las pinturas religiosas efectuadas en Sevilla aunque habría tenido que aprobarlas, como especialista en la iconografía religiosa y censor de la Inquisición. Lo que sí menciona y elogia, en cambio, son sus bodegones, escenas de cocina o taberna con figuras y objetos de naturaleza muerta, nuevo tipo de composición cuya popularidad en España se debe en gran parte a Velázquez. En tales obras y en sus retratos el discípulo de Pacheco alcanzó ''la verdadera imitación de la naturaleza'', siguiendo el camino de Caravaggio y Ribera. Velázquez, en realidad, fue uno de los primeros exponentes en España del nuevo naturalismo que procedía directa o indirectamente de Caravaggio y, por cierto, ''El aguador de Sevilla'' (h. 1619, Wellington Museum, Londres) fue atribuido al gran genio italiano al llegar a la capital inglesa en 1813. El aguador fue una de las primeras obras en difundir la fama del gran talento de Velázquez por la corte española, pero cuando se marchó a Madrid por primera vez en 1622 fue con la esperanza -no realizada- de pintar a los reyes. Hizo para Pacheco, aquel año, el retrato del poeta Don Luis de Góngora y Argote (Museum of Fine Arts, Boston), que fue muy celebrado y copiado luego al pincel y al buril, y esto, sin duda, fomentó su reputación de retratista en la capital. Cuando volvió a Madrid al año siguiente, llamado por el conde-duque de Olivares, realizó la efigie del joven Felipe IV, rey desde hacía dos años. Su majestad le nombró en seguida pintor de cámara, el primero de sus muchos cargos palatinos, algunos de los cuales le acarrearían pesados deberes administrativos. A partir de entonces ya no volvería a Sevilla, ni tampoco salió mucho de Madrid, salvo para acompañar al rey y su corte. Tan solo estuvo fuera del país en dos ocasiones, en Italia, la primera en viaje de estudios y la segunda con una comisión del rey. En el nuevo ambiente de la corte, famosa por su extravagancia ceremonial y su rígida etiqueta, pudo contemplar y estudiar las obras maestras de las colecciones reales y, sobre todo, los Tizianos. Como el gran genio veneciano, Velázquez se dedicó a pintar retratos de la familia real, de cortesanos y distinguidos viajeros, contando, sin duda, con la ayuda de un taller para hacer las réplicas de las efigies reales. Su primer retrato ecuestre del rey, expuesto en la calle Mayor ''con admiración de toda la corte e invidia de los del arte'' según Pacheco, fue colocado en el lugar de honor, frente al famoso retrato ecuestre de ''El emperador Carlos v, a caballo, en Mühlberg'', por Tiziano (Prado), en la sala decorada para la visita del cardenal Francesco Barberini en 1626. Su retrato del cardenal, en cambio, no gustó, por su índole ''melancólica y severa'', y al año siguiente se le tachó de solo saber pintar cabezas. Esta acusación provocó un concurso entre Velázquez y tres pintores del rey, que ganó el pintor sevillano con su ''Expulsión de los moriscos'' (hoy perdido). Si su retrato de Barberini no fue del gusto italiano, sus obras y su ''modestia'' merecieron en seguida los elogios del gran pintor flamenco Pedro Pablo Rubens, cuando vino a España por segunda vez en 1628, según Pacheco. Ya habían cruzado correspondencia los dos y colaborado en un retrato de Olivares, grabado por Paulus Pontius en Amberes en 1626, cuya cabeza fue delineada por Velázquez y el marco alegórico diseñado por Rubens. Durante la estancia de Rubens en Madrid, Velázquez le habría visto pintar retratos reales y copiar cuadros de Tiziano, aumentando al contemplarle su admiración para con los dos pintores que más influencia tendrían sobre su propia obra. Su ejemplo inspiró, sin duda, su primer cuadro mitológico ''El triunfo de Baco'' o ''los borrachos'' (1628-1629, Prado), tema que, en ­manos de Velázquez, recordaría más el mundo de los bodegones que el mundo clásico. Parece que una visita a El Escorial con Rubens renovó su deseo de ir a Italia, y partió en agosto de 1629. Según los representantes italianos en España, el joven pintor de retratos, favorito del rey y de Olivares, se iba con la intención de rematar sus estudios. Cuenta Pacheco que copió a Tintoretto en Venecia y a Miguel Ángel y Rafael en el Vaticano. Luego pidió permiso para pasar el verano en la Villa Médicis, donde había estatuas antiguas que copiar. No ha sobrevivido ninguna de estas copias ni tampoco el autorretrato que se hizo a ruego de Pacheco, quien lo elogia por estar ejecutado ''a la manera del gran Tiziano y (si es lícito hablar así) no inferior a sus cabezas''. Prueba de sus avances en esta época son las dos telas grandes que trajo de Roma. ''La fragua de Vulcano'' (1630, Prado) y ''La túnica de José'' (1630, El Escorial) justifican ampliamente las palabras de su amigo Jusepe Martínez, según las cuales ''vino muy mejorado en cuanto a la perspectiva y arquitectura se refería''. Además, tanto el tema bíblico como el mi­tológico, tratados por Velázquez, ­demuestran la independencia de su interpretación de las estatuas antiguas en los torsos desnudos sacados de modelos vivos. De regreso en Madrid a principios de 1631, Velázquez volvió a su principal oficio de pintor de retratos, entrando en un periodo de grande y variada producción. Dirigió o participó, por otra parte, en los dos grandes proyectos del momento y del reino: la decoración del nuevo palacio del Buen Retiro en las afueras de Madrid, y el pabellón que usaba el rey cuando iba de caza, la Torre de la Parada. Pronto adornaban suntuosamente el Salón de Reinos (terminado en 1635), sala principal del primero de estos palacios, sus cinco retratos ecuestres reales, más ''Las lanzas o la rendición de Breda'' (Prado), su contribución a la serie de triunfos militares. La tela grande de San Antonio Abad y San Pablo, primer ermitaño (h. 1633, Prado), destinada al altar de una de las ermitas de los jardines del Retiro, demuestra el talento del pintor para el paisaje, y es obra notable en su línea destacando entre los muchos ejemplos del género traí­dos de Italia para decorar el palacio. Para la Torre de la Parada, Velázquez pintó retratos del rey, su hermano y su hijo, vestidos de cazadores, con fondos de paisaje, lo mismo que los retratos ecuestres del Buen Retiro y la Tela Real o Cacería de jabalíes en Hoyo de Manzanares (h. 1635-1637, National Gallery, Londres). Pintó también para la misma Torre las ­figuras de ''Esopo'', ''Menipo'' y ''El dios Marte'' (todas en el Prado), temas apropiados al sitio y no ajenos a las escenas mitológicas encargadas a Rubens y a su escuela en Amberes. Por esta época, Velázquez pintó también sus retratos de bufones y enanos, de los cuales sobresalían las cuatro figuras sentadas, por su matizada caracterización y la diversidad de sus posturas, adaptadas a sus deformados cuerpos. Con igual sensibilidad creó un aire festivo adecuado para ''La Coronación de la Virgen'' (h. 1641-1642, Prado), destinada para el oratorio de la reina en el Alcázar: vívida traducción de una composición de Rubens al idioma propio de Velázquez. Parecido por la riqueza de su colorido, aunque con más ecos de Van Dyck que de ­Rubens, es el vistoso retrato de ''Felipe IV en Fraga'' (1643, Frick Collection, Nueva York), realizado para celebrar una de las más recientes victorias del ejército. A partir de entonces no volvería a retratar al rey durante más de nueve años. A pesar de sus muchos problemas militares, económicos y familiares, Felipe IV no perdió su pasión por el arte ni sus deseos de seguir enriqueciendo su colección. Por este motivo encargó a Velázquez que fuera de nuevo a Italia a buscarle pinturas y esculturas antiguas. Partió Velázquez en enero de 1649, recién nombrado ayuda de cámara del rey, y llevó consigo pinturas para el papa Inocencio X en su Jubileo. Este segundo viaje a Italia de Velázquez tuvo consecuencias importantes para su vida personal, lo mismo que para su carrera profesional. En Roma tuvo un hijo natural, llamado Antonio, y dio la libertad a su esclavo de muchos años, Juan de Pareja. En cuanto a su comisión, sabemos el éxito que tuvo gracias a Palomino y los documentos al respecto, y también cómo se le honró al elegirle académico de San Lucas y socio de la Congregación de los Virtuosos. Se ganó asimismo el patrocinio de la curia durante su estancia en Roma. En cuanto a su retrato de Juan de Pareja (1650, Metropolitan Museum of Art, Nueva York), expuesto en el Panteón romano, Palomino cuenta cómo ''a voto de todos los pintores de todas las naciones [a la vista del cuadro], todo lo demás parecía pintura, pero éste solo verdad''. Su mayor triunfo por entonces fue granjearse el favor del papa para que le dejara retratarle, favor concedido a pocos extranjeros, retrato (Galleria Doria Pamphilj, Roma) que le valió más adelante el apoyo del pontífice a la hora de solicitar permiso para entrar en una de las órdenes militares. Pintado en el verano de 1650, «ha sido el pasmo de Roma, copiándolo todos por estudio y admirándolo por milagro», según Palomino, que no exageraba, por cierto, con estas palabras. Existen múltiples copias del cuadro que ha inspirado a numerosos pintores desde que se pintó hasta hoy. La impresión de formas y texturas creada con luz y color mediante pinceladas sueltas recuerda la deuda de Velázquez a Tiziano, y anuncia el estilo avanzado y tan personal de sus últimas obras. A esta estancia en Italia se atribuye también, por su estilo, originalidad e historia, ''La Venus del espejo'' (1650-1651, National Gallery, Londres), el único desnudo femenino conservado de su mano. Se trata de una obra de ricas resonancias, una vez más, de Tiziano y de las estatuas antiguas, pero el concepto de una diosa en forma de mujer viva es característico del lenguaje personal del maestro español, único en su tiempo. De vuelta en Madrid en 1652, y con el nuevo cargo de aposentador de Palacio, Velázquez se entregó al adorno de las salas del Alcázar, aprovechándose en parte de las obras de arte adquiridas en Italia, entre las cuales, según los testimonios conservados, había alrededor de trescientas esculturas. En 1656, el rey le mandó llevar cuarenta y una pinturas a El Escorial, entre ellas las compradas en la almoneda londinense del malogrado monarca inglés Carlos I. Según Palomino, redactó una memoria acerca de ellas en la que manifestó su erudición y gran conocimiento del arte. Luego, para el salón de los espejos, donde estaban colgadas las pinturas venecianas preferidas del rey, pintó cuatro mitologías e hizo el proyecto para el techo, con la distribución de temas, que ejecutarían dos pintores boloñeses contratados por Velázquez en Italia. A pesar de estas ocupaciones, Velázquez no dejó de pintar, y encontró nuevos modelos en la joven reina Mariana y sus hijos. ''La reina Mariana de Austria'' (h. 1651-1652, Prado) y ''La infanta María Teresa'', ­hija del primer matrimonio del rey (1652-1653, Kunsthistorisches Museum, Viena), resultan muy parecidas en estos retratos en cuanto a sus caras y sus figuras, emparejadas por las extravagancias de la nueva moda. Supo crear con pincelada suelta, sin definir los detalles, la forma y los elementos decorativos del enorme guardainfante, así como los exagerados peinados y maquillajes. Consiguió, con la misma libertad de toques, resaltar la tierna vitalidad de los jóvenes infantes, dentro de la rígida funda de sus vestidos. Los últimos dos retratos de Felipe IV, copiados al óleo y al buril, son bien diferentes (1653-1657, Prado; h. 1656, National Gallery, Londres): bustos sencillos vestidos de trajes oscuros, informales e íntimos, que reflejan el decaimiento físico y moral del monarca del cual se dio cuenta. Hacía nueve años que no se le había retratado y, como él mismo dijo en 1653: «No me inclino a pasar por la flema de ­Velázquez, como por no verme ir envejeciendo». En los últimos años de su vida, pese a su conocida flema y sus muchas preocupaciones, Velázquez añadió dos magistrales lienzos a su obra, de índole original y nueva y difíciles de clasificar: ''Las hilanderas'' o ''La fábula de Aracne'' (h. 1657, Prado) y el más famoso de todos, ''Las meninas'' o ''la familia de Felipe IV'' (1656, Prado). En ellos vemos cómo el naturalismo «moderno» en su tiempo, el realismo detallado del pintor sevillano, se había ido transformando a lo largo de su carrera en una visión fugaz del personaje o de la escena. Los pinta con toques audaces que parecen incoherentes desde cerca, aunque muy justos y exactos a su debida distancia, y se anticipa en cierto modo al arte de Édouard ­Manet y al de otros pintores del siglo XIX en los que tanta mella hizo su estilo. Su último acto público fue el de acompañar a la corte a la frontera francesa y decorar con tapices el pabellón español en la isla de los Faisanes para el matrimonio de la infanta María Teresa y Luis XIV (7 de junio de 1660). Pocos días después de su vuelta a Madrid, cayó enfermo y murió el 6 de agosto de 1660. Su cuerpo fue amortajado con el uniforme de la orden de Santiago, que se le había impuesto el año anterior. Fue enterrado, según las palabras de Palomino, ''con la mayor pompa y enormes gastos, pero no demasiado enormes para tan gran hombre'' (Harris, E. en Enciclopedia M.N.P., 2006, VI, pp. 2148-2156).</br></br>Su autorretrato es un detalle de ''Las meninas'', obra P01174 del Museo del Prado.";
            autor.Ecidoc_p96_E67_p4_gave_birth_date = "1599";// new DateTime(1599, 1, 1);
            autor.Ecidoc_p96_E67_p4_gave_birth_year = 1599;
            autor.Ecidoc_p96_E67_p7_gave_birth_place = "Sevilla";
            return autor;
        }

        private static E39_Actor CreateGoya()
        {
            E39_Actor autor = new E39_Actor();
            autor.Pm_identifier = "1154";
            autor.Ecidoc_p100i_E69_p4_death_date = "1828"; //new DateTime(1828, 4, 16);
            autor.Ecidoc_p100i_E69_p4_death_year = 1828;
            autor.Ecidoc_p100i_E69_p7_death_place = "Bordeaux (France)";
            //autor.Ecidoc_p131_E82_p102_has_title = "Goya y Lucientes, Francisco de";
            autor.Cidoc_p3_has_note = "Goya nació accidentalmente en Fuendetodos, pueblo de su familia materna. Braulio José Goya, dorador, de ascendencia vizcaína, y Gracia Lucientes, de familia campesina acomodada, residían en Zaragoza, donde contrajeron matrimonio en 1736. Francisco fue el cuarto de seis hermanos: Rita (1737); Tomás (1739), dorador también, citado a veces como pintor; Jacinta (1743); Mariano (1750), muerto en la infancia, y Camilo (1753), eclesiástico y capellán desde 1784 de la colegiata de Chinchón.</br>Tras la escuela, que la tradición acepta con reservas como la de los padres escolapios de Zaragoza, entró en el taller de José Luzán (1710-1785), hijo también de un dorador vecino de los Goya, de formación napolitana y vinculado a la Academia de Dibujo.</br>Javier Goya, en las notas biográficas sobre su padre para la Academia de San Fernando (1832), aseguraba que “estudió el dibujo desde los trece años en la Academia de Zaragoza bajo la dirección de José Luzán”, y Goya, en la autobiografía del catálogo del Museo del Prado (1828), decía que le “había dado a copiar las más bellas estampas que tenía”, aunque en sus primeras obras conocidas apenas hay huellas del estilo tardo-barroco de aquél. Más adelante siguió su formación con Francisco Bayeu Subías (1734-1795), relacionado por lejano parentesco con los Goya, y que, años después, sería su cuñado por esa unión tradicional entre familias de artistas. En 1771, la Academia de Parma citaba a Goya como “scolaro del Signor Francesco Vajeu” y lo confirmaba él mismo en 1783, con motivo del matrimonio de su cuñada María Bayeu, a la que conocía desde hacía veinte años por haber estudiado “en casa” de Bayeu.</br>Después de los modestos inicios en Aragón, donde se le atribuye, de hacia 1765, el relicario, destruido, de la parroquia de Fuendetodos con la Aparición de la Virgen del Pilar a Santiago, así como varias pinturas para la devoción privada confirmadas como de su mano en los últimos años, la carrera cortesana se perfilaba como la única posible para un joven con ambiciones. Se trasladó a Madrid en 1763, siguiendo a Bayeu, que trabajaba en la decoración del Palacio Real. En diciembre de 1763 Goya quiso obtener una pensión de la Academia de Bellas Artes de San Fernando y en 1766 se presentó al premio de primera clase de pintura, aunque fracasó en ambos.</br>En un arranque de independencia Goya viajó a Italia por sus propios medios, según decía más tarde en un memorial a Carlos III (24 de julio de 1779), y está documentado en Roma en la primavera de 1771, aunque viajó a Italia en junio de 1769, como ha revelado la última documentación localizada de sus capitulaciones de boda. La tradición le situó en Roma en el n.º 48 de la Strada Felice (Via Sistina), barrio de los artistas, en casa del pintor polaco Taddeus Kuntz, amigo de Mengs, como la familia de éste aseguraría años más tarde, aunque no hay noticias documentales de ello. El Cuaderno italiano (Madrid, Museo del Prado), álbum de apuntes comprado en Italia, recogía anotaciones de las ciudades que visitó, todas del norte, entre ellas Bolonia, Venecia, Parma y Milán, viajando de regreso a través de  Génova y Marsella.</br>En abril de 1771 envió el cuadro Aníbal que por primera vez mira Italia desde los Alpes (Fundación Selgas Fagalde) al concurso de la Academia de Parma por el que recibió una mención, reseñada en el Mercure de France en 1772. Varios dibujos del Cuaderno italiano copian esculturas clásicas de Roma, un fresco de Giaquinto, así como presentan composiciones propias y las primeras ideas documentadas de cuadros tempranos, algunos pintados ya en España, donde regresó entre mayo y julio de 1771. Su primer encargo fue el fresco del coreto de la basílica del Pilar, de 1771-1772, con la Adoración del Nombre de Dios, de excepcional y moderna grandeza, auto titulándose en esa ocasión “Profesor de Dibujo en esta Ciudad [Zaragoza]” (22 de noviembre de 1772).</br>Se casó, sin embargo, en Madrid, en la iglesia de San Martín (25 de julio 1773) con Josefa Bayeu (nacida el 19 de marzo de 1747), hermana de Francisco, que fue padrino de la boda con su mujer, Sebastiana Merklein, aunque el primer hijo de Goya, documentado en el Cuaderno italiano, Antonio Juan Ramón Carlos, nació en Zaragoza (29 de agosto de 1774), donde el artista trabajaba en los frescos de la cartuja de Aula Dei.</br>Goya dejó nuevamente Zaragoza el 3 de enero de 1775, llegando a Madrid el día 10 (anotación en su Cuaderno italiano) para iniciar su trabajo como pintor de cartones de tapices para la Real Fábrica de Santa Bárbara con el sueldo de 8.000 reales de vellón al año. Recomendado por Bayeu, Goya aseguraba más tarde, orgulloso, que fue Mengs quien le hizo volver de Roma para el Real Servicio, aunque sus primeros cartones, fechados en la primavera de 1775 para la serie ya comenzada con destino al comedor de los príncipes de Asturias en El Escorial, se pintaron según ideas de Bayeu. Los temas representados, elegidos por el rey, eran de caza, que fue la afición más importante del artista a lo largo de su vida. Perros, escopetas y lugares de caza favoritos, a veces en compañía de sus egregios mecenas, aparecerán en la importante correspondencia con su amigo de infancia, el comerciante zaragozano Martín Zapater (Madrid, Museo del Prado), como los alrededores de Madrid, la sierra de Guadarrama y El Escorial, Arenas de San Pedro, Chinchón, la Albufera de Valencia y el Coto de Doñana en las tierras de Sanlúcar de Barrameda (Cádiz). El segundo hijo, Eusebio Ramón, nació en Madrid (15 de diciembre de 1775); vivía Goya entonces en la calle del Reloj, tal vez en casa de Bayeu, y entre 1776 y 1778 pintó los cartones para el comedor de los príncipes de Asturias en el palacio de El Pardo con escenas de la vida popular en Madrid, como el Baile a orillas del Manzanares y El quitasol, ya de su propia invención, con tipos populares caracterizados con perfección magistral, y divertidas historias cargadas de contenido satírico y moralizante, enlazando con la corriente popular favorecida por la Ilustración. El artista se acercaba más que otros compañeros suyos, en el naturalismo y sentido del humor de sus escenas, a los tipos y situaciones descritos en los sainetes de un amigo de esos años, el autor teatral Don Ramón de la Cruz, mientras que en la caracterización de sus figuras demostraba el mismo conocimiento de tipos y modas que las ilustraciones del grabador Juan de la Cruz, hermano del anterior, en su Colección de Trages de España. El Cuaderno italiano revela que Goya intentó regresar a Italia con Mengs, en 1777, pero enfermó gravemente a fines de ese año, asegurando a Zapater que había “escapado de buena”, y había recibido el encargo de dos series más para la Fábrica de Tapices. Nacieron en Madrid otros dos hijos, Vicente Anastasio (21 de enero de 1777) y María del Pilar Dionisia (9 de octubre de 1779), cuando Goya vivía en la carrera de San Jerónimo, en la casa de la marquesa de Campollano, aunque poco después habitaba ya en casa propia, en el nº 1 de la calle del Desengaño, donde vivió hasta junio de 1800. Entre 1778 y 1780 pintó las series de cartones de tapices del dormitorio de los príncipes de Asturias, en El Pardo, con escenas como El cacharrero y El ciego de la guitarra. Se fechan entonces las estampas sobre obras de Velázquez de la colección real, que valoró el erudito Antonio Ponz, y El agarrotado y El ciego de la guitarra, de invención propia. En enero de ese mismo año, según contaba a Zapater, había sido presentado al Rey y a los príncipes de Asturias, “... y les besé la mano que aun no abia tenido tanta dicha jamás”.</br>El 7 de julio de 1780, con el clasicista Cristo en la cruz (Madrid, Museo del Prado), ingresó como miembro de mérito, por unanimidad, en la Real Academia de Bellas Artes de San Fernando. Nació su quinto hijo (22 de agosto de 1780), Francisco de Paula Hipólito Antonio Benito, y en el otoño Goya se trasladó con su familia a Zaragoza para pintar el fresco de la cúpula de Regina Martyrum en el Pilar. La obra fue rechazada en 1781 por la Junta de la basílica a causa de la incorrección de la figura de la Caridad y la oscuridad general del colorido, imponiéndosele la supervisión de Bayeu, a lo que se negó. Se produjo la ruptura entre los cuñados que duró varios años y afectó a la actividad de Goya, que perdió encargos proporcionados por su cuñado. Su honor de artista, que tan profundamente sintió toda su vida, quedó restaurado al encargársele por orden del ministro de Estado, el conde de Floridablanca, uno de los cuadros para la basílica de San Francisco el Grande, la Predicación de San Bernardino de Siena, concluido en enero de 1783. Había nacido una niña, Hermenegilda Francisca de Paula (13 de abril de 1782), admirándose su cuñado cartujo, fray Manuel Bayeu, en sus cartas a Zapater, de la fecundidad del artista, aunque todos sus hijos, salvo el último, Javier (2 de octubre de 1784), murieron en la infancia. En el decenio de 1780 comenzó de lleno la actividad de Goya como retratista, de la que no se conoce hasta entonces más que un Autorretrato, de hacia 1772-1775 (Zaragoza, Museo de Bellas Artes). Destaca el Retrato del conde de Floridablanca como protector del Canal de Aragón (1783, Madrid, Banco de España), y los que pintó para su nuevo mecenas, el infante don Luis de Borbón, en el exilio de su palacio de Arenas de San Pedro (Ávila). Para entonces Goya era ya reconocido por importantes figuras de la cultura de su tiempo, como el citado Antonio Ponz, Gaspar Melchor de Jovellanos, a quien retrató hacia 1783 (Oviedo, Museo de Bellas Artes), y el erudito y coleccionista Juan Agustín Ceán Bermúdez, aunque su amistad con otras figuras, como Leandro Fernández de Moratín y Juan Meléndez Valdés, debió de comenzar algo después, según las noticias de Ceán Bermúdez.</br>Goya fue nombrado teniente director de Pintura de la Academia de San Fernando (1 de mayo de 1785) con el sueldo de 25 doblones anuales (2.000 reales), y al año siguiente, en julio de 1786, limadas sus diferencias con Bayeu y a propuesta de Maella, se le nombró pintor del Rey con el sueldo de 15.000 reales.</br>Fray Manuel Bayeu le refería a Zapater el nombramiento de su cuñado: “Esta accion de Francho [Bayeu], como azía tiempo no se trataban, a sido para mí la de más satisfacción que he tenido. Dios quiera vivan en paz y como Dios manda”. Se reanudaron efectivamente sus trabajos para la Fábrica de Tapices tras seis años de inactividad, en 1786-1787 realizó la serie de las Cuatro Estaciones para el comedor del príncipe en El Pardo y en 1788 los bocetos para los cartones de tapices del dormitorio de las infantas, entre los que destacan La pradera de San Isidro y La gallina ciega, único del que ejecutó el cartón porque se suspendieron los trabajos por la muerte de Carlos III. En ese período había comenzado, además, su larga relación con la casa ducal de Osuna, mantenida hasta después de la guerra de la Independencia.</br>Goya había alcanzado una excelente situación en la Corte, que le halagaba profundamente, pintando ya entonces, como le contaba a Zapater, sólo para la más alta aristocracia, y desde luego para el Rey, de quien realizó el retrato como cazador hacia 1787 (Madrid, Museo del Prado). Concluyó ese decenio con su nombramiento como pintor de Cámara (30 de abril de 1789) y con los retratos de los nuevos reyes, Carlos IV y María Luisa de Parma (Madrid, Academia de la Historia). La correspondencia con Zapater, numerosa en la década de 1780, revela la amistad de Goya con ilustres zaragozanos, entre los que se hallaban Juan Martín de Goicoechea; Manuel Fumanal, director del seminario de San Carlos; Tomás Pallás, militar y de la Real Sociedad Económica Aragonesa; Alejandro Ortiz y Márquez, médico y catedrático de la universidad; José Yoldi, administrador general del Canal de Aragón, y Ramón Pignatelli, fundador de la Real Sociedad Económica Aragonesa, rector de la Universidad e impulsor del Canal de Aragón. Esas relaciones culminaron con la elección del artista como socio de mérito de la Real Sociedad Económica Aragonesa de Amigos del País (22 de octubre de 1790). En Valencia, donde había pintado las obras para la capilla de la duquesa de Osuna en la catedral y tenía relación con la Academia de Bellas Artes de san Carlos a través de su secretario, Mariano Ferrer y Aulet, fue nombrado académico de mérito de ésta (20 de octubre de 1790). Según le refería a Zapater en noviembre de 1787, estaba aprendiendo el francés, el italiano ya lo hablaba y se había vuelto “viejo con muchas arrugas que no me conocerías sino por lo romo y por los ojos undidos [...] lo que es cierto es que boy notando mucho los 41”. La correspondencia con su íntimo amigo revela las aficiones de Goya, además de demostrar lo buen hijo, hermano y padre que era y la cálida relación con sus amigos. Le gustaban las corridas de toros, incluso Moratín decía, ya en los años de Burdeos, que Goya se jactaba de haber toreado en su juventud, pero también asistía al teatro, a la ópera y a los conciertos de música en la Corte. Su carácter alegre se revela en su gusto por las “tiranas”, las fiestas con sus amigos y familiares, los viajes a Valencia, por ejemplo, para “tomar los aires marítimos”, o a Zaragoza para asistir a las fiestas del Pilar. Desde 1783 se firmaba como “Francisco de Goya”, señalando así orígenes hidalgos por su ascendencia vizcaína, pero todos los esfuerzos que hizo para obtener una infanzonía resultaron fallidos, ya que nada encontró en los archivos zaragozanos que probara su pretendida nobleza.</br>Hacia fines del año 1790 aparecieron los primeros síntomas de la grave enfermedad que le sobrevino a principios de 1793, temblores y mareos a los que se refiere en cartas de ese período a Zapater. En 1791 Goya puso dificultades para seguir pintando cartones de tapices a las órdenes de Maella y fue acusado de ello al Rey por Livinio Stuyck, director de la Real Fábrica, aunque la intervención de Bayeu y la amenaza de que se reduciría su salario le hicieron reconsiderar su postura; tras ello el artista comenzó la preparación de su última serie, trece cartones para el despacho del rey Carlos IV en El Escorial, con escenas “campestres y Jocosas”, de las que sólo pintó seis, entre las que destacan La boda o El pelele. El 14 de octubre de 1792 Goya firmaba el informe solicitado por la Academia de San Fernando sobre las enseñanzas de las Bellas Artes, en el que expresaba la necesidad de libertad en el estudio de la pintura, que definió como “Sagrada ciencia”. Asistió a la Junta Extraordinaria del 28 de octubre, pero no a la del 18 de noviembre por unos cólicos que padeció, y recogió su nómina de académico a principios de enero de 1793. Con licencia real viajó entonces a Sevilla, donde cayó enfermo en febrero, según se deduce de la correspondencia entre sus amigos Zapater y el comerciante gaditano Sebastián Martínez, a cuya casa en Cádiz fue trasladado Goya, ya enfermo, por su amigo Ceán Bermúdez. A principios de marzo Martínez aseguraba que “la naturaleza del mal es de las más temibles” y a fines del mismo mes que “el ruido en la cabeza y la sordera nada han cedido, pero está mucho mejor de la vista y no tiene la turbación que tenía que le hacia perder el equilibrio”. Zapater decía que “a Goya, como te dije, le ha precipitado su poca reflexión”. Esa ambigua frase ha dado pie a numerosas interpretaciones sobre la naturaleza de la enfermedad, de la que Goya quedó definitivamente sordo: sífilis, saturnismo, “cólico de Madrid” —envenenamiento por metales en los utensilios de preparar la comida—, y “perlesía” —hemiplejia—.</br>Regresó a Madrid a principios de mayo de 1793  y en enero del año siguiente presentó a la Academia de san Fernando la decisiva serie de cuadros de gabinete sobre hojalata, con escenas de toros y otras “diversiones nacionales”, como Los cómicos ambulantes (Madrid, Museo del Prado), y temas de carácter dramático, El naufragio, Incendio de noche (colecciones privadas), Corral de locos (Dallas, Meadows Museum) y Prisioneros en una cueva (Bowes Museum), pintadas con independencia y sin estar sometido a las imposiciones de la clientela. Es posible que fuera en los meses que siguieron cuando Goya terminó los últimos cartones para los tapices del comedor del rey, comenzados antes de su enfermedad. Aunque sin éxito, se sometió a una electroterapia contra la sordera, procedimiento que se documenta en una carta fechada en septiembre de 1794, según la cual el artista solicitó que se arreglara el disco de vidrio de una “máquina eléctrica” que le había proporcionado el director del Real Laboratorio de Química, Pierre François Chavaneau. Reanudó plenamente su actividad en otoño de aquel año y en 1795, con retratos y cuadros de encargo, como los de la Santa Cueva de Cádiz. Se fecha entonces su acercamiento a Godoy, así como el mecenazgo de los duques de Alba, que dio pie incluso a la moderna leyenda, basada en débiles indicios, de los amores del artista con la duquesa. A la muerte de Bayeu, en agosto de 1795, Goya fue nombrado director de Pintura de la Academia e inició en ese brillante período sus álbumes de dibujos, los llamados Álbum de Sanlúcar y Álbum de Madrid, fechados ahora hacia 1794-1795, sin que se advierta en su técnica segura y delicada rastro alguno de los temblores de su enfermedad. Planteó en ellos las primeras ideas para esas obras maestras de la sátira contra vicios y costumbres de la sociedad que fueron Los Caprichos, publicados en enero de 1799, y, según su primer biógrafo, L. Matheron, gestados en el entorno de las duquesas de Alba y de Osuna. </br>Gran parte del año 1796 lo pasó en Andalucía, en Cádiz, donde tuvo casa propia, según Moratín, y en Sevilla, de donde las noticias que llegaban sobre su salud no eran muy favorables. Visitó a la duquesa de Alba en Sanlúcar y pintó el célebre retrato de la dama vestida de negro (Nueva York, Hispanic Society, 1797). A principios de 1797 se encontraba de regreso en Madrid para renunciar a su cargo de director de Pintura de la Academia porque “ve en el dia que en vez de haber cedido sus males se han exacerbado más”. La liberación de las responsabilidades de la Academia determinó los años más prolíficos de la vida de Goya con retratos excepcionales entre los que destacan el de Jovellanos (1798) (Madrid, Museo del Prado) y La Tirana (1799) (Madrid, Real Academia de Bellas Artes de San Fernando), y obras como la Maja desnuda, documentada en el palacio de Godoy en 1800, y la Maja vestida, más tardía, documentada en enero de 1808. Se fechan a fines de ese decenio también los cuadros con Asuntos de brujas para los duques de  Osuna, los frescos de la ermita de San Antonio de la Florida y, en diciembre de 1798, el Prendimiento para la catedral de Toledo, así como los nuevos retratos reales, María Luisa con mantilla, Carlos IV cazador y el Retrato ecuestre de María Luisa en el otoño de 1799.</br>Culminó la década de 1790 con el nombramiento de Goya como primer pintor de Cámara, escalón supremo de su carrera cortesana, firmado el 31 de octubre de 1799 por el Primer Ministro, Mariano de Urquijo, y con el sueldo de 50.000 reales de vellón. Goya escribía ese día su última carta a Zapater (fallecido en 1803): “Los Reyes estan locos con tu amigo”. Ese patrocinio real, y de Godoy, siguió durante los primeros años del siglo XIX, iniciándose en junio de 1800 con la espectacular Familia de Carlos IV. En ese mismo mes Goya se había trasladado a su nueva casa de la calle de Valverde n.º 35  y había vendido su anterior vivienda a Godoy, cuyo servicio se hace patente en el Retrato de la condesa de Chinchón (abril de 1800)  (Madrid, Museo del Prado), en el del ministro con motivo de la Guerra de las Naranjas (1801) (Madrid, Real Academia de Bellas Artes de San Fernando) y en los grandes lienzos alegóricos para la decoración de su palacio (c. 1802-1804), como la Alegoría del Tiempo y la Historia (Estocolmo, Nationalgalleriet). La faceta de los retratos privados es excepcionalmente rica en estos años; en ellos Goya define el retrato aristocrático, de inusitada variedad, como los del Conde y la condesa de Fernán Núñez (1803) (Madrid, colección Fernán Núñez), de la Marquesa de Villafranca pintando a su marido (1804) (Madrid, Museo del Prado), de la Marquesa de Santa Cruz (1804) (Madrid, Museo del Prado) y del Marqués de San Adrián (1804) (Pamplona, Museo de Bellas Artes). Al mismo tiempo, el auge de la sociedad burguesa propició el retrato de esa nueva clase social que Goya pintó desde sus inicios en obras más íntimas, sobrias y realistas, de aguda psicología, como el del pintor Bartolomé Sureda y su mujer Teresa Sureda, 1804-1805 (Washington, National Gallery), de Antonio Porcel (1806) (Buenos Aires, Jockey Club, destruido), de los actores Isidoro Máiquez (Chicago, Art Institute) o de la actriz retirada Antonia Zárate (c. 1808) (Dublín, National Gallery).</br>En junio de 1803 Goya compró una nueva casa en el nº 7 de la calle de los Reyes, que no llegó a habitar, y el 7 de julio, en carta a Miguel Cayetano Soler, regaló al Rey las planchas de cobre de los Caprichos y doscientos ejemplares a cambio de una pensión de 12.000 reales para su hijo Javier, que deseaba estudiar Pintura. Goya preparaba el futuro de su hijo, de quien no hay noticias fidedignas sobre su actividad como pintor, y que casó el 8 de julio de 1805 con Gumersinda Goicoechea, hija del comerciante madrileño Martín Miguel de Goicoechea, a los que también retrató el artista en dos espléndidos y novedosos retratos de cuerpo entero (colección Noailles). En 1806 nació el único nieto del artista, Mariano, retratado por su abuelo en 1810 vestido con elegancia y tirando de un carretón de juguete (Madrid, col. Larios). En los inicios del siglo XIX comienzan los elogios al artista por la altura de su arte, entre los que destaca el poema de Manuel José Quintana, de 1805, valorando su figura por encima de la de Rafael de Urbino y augurándole la fama universal en siglos venideros. Debió de continuar con sus cuadernos de dibujos, de difícil y amplia datación, pues las composiciones se relacionan con temas que van desde principios del siglo XIX hasta 1820, como los de los Álbumes C, D, E y F.</br>Goya permaneció en Madrid durante la Guerra contra Napoleón (1808-1814) y juró fidelidad a José Bonaparte como oficial de palacio, recibió también la orden de España, que no recogió, y retrató a alguno de los ministros y autoridades del nuevo gobierno francés. Por otra parte, como pintor de cámara del nuevo rey proporcionó listas de cuadros de la colección real con destino al museo creado por Napoleón en París, aunque se tienen escasas noticias documentales de su actividad en esas fechas, con largos períodos de silencio. El inventario de sus bienes y de Josefa Bayeu, fallecida al final de la guerra (20 de junio de 1812), registra numerosas obras que revelan su actividad incesante. Trabajó también en los aguafuertes de los Desastres de la guerra, denuncia de la violencia sobre el pueblo indefenso, y en la Tauromaquia, publicada en 1816. En febrero y marzo de 1814 se gestó, por la Regencia, el encargo de los dos grandes lienzos del Dos y Tres de mayo en Madrid (Madrid, Museo del Prado), con el brutal ataque de los patriotas víctimas de la invasión y de la despiadada respuesta de los franceses.</br>En mayo Goya pasó favorablemente la depuración de los funcionarios de palacio al servicio del gobierno francés, recuperó su salario y sus derechos y pintó de nuevo para la Corona y sus altos dignatarios (Retrato de Fernando VII con manto real, Madrid Museo del Prado, y Retrato del duque de San Carlos, Zaragoza, Museo de Bellas Artes). A partir de 1815 el artista se fue alejando de la Corte, sustituido en el gusto del monarca por Vicente López, y se centró entonces en su actividad privada: retratos (Retrato del X duque de Osuna, Bayonne, Musée Bonnat), cuadros para la Iglesia, que había sido fiel mecenas desde su juventud (Santas Justa y Rufina, Sevilla, Sacristía de los cálices de la Catedral, y La última comunión de san José de Calasanz, Madrid, Padres Escolapios), en los dibujos de los álbumes de ese período, C, D y E, en  las últimas láminas de los Desastres, los llamados Caprichos enfáticos, y en la serie de los Disparates.</br>Vivía Goya aún en la calle de Valverde cuando en 1819 adquirió una casa de campo a las afueras de Madrid, conocida como la “Quinta del Sordo”, que guardaría sus “Pinturas negras”. Desde 1815 vivían en su casa Leocadia Zorrilla y sus dos hijos; la joven, que hacía las veces de gobernanta, era prima de la mujer de su hijo y ha sido considerada por indicios y por algunas referencias de Moratín, ya en Burdeos, como la compañera de sus últimos años, aunque no existen noticias fidedignas al respecto.</br>El 23 de septiembre de 1823 Goya hizo donación de la Quinta a su nieto Mariano. El 2 de mayo de 1824 Goya solicitó del Rey permiso para marchar a Francia a tomar las aguas minerales de Plombières (Vosgos). Desde la llegada a España en abril de 1823 de los Cien Mil Hijos de San Luis, para restituir el poder absoluto del Rey, Goya pudo decidir su exilio, al que se habían visto obligados muchos de sus amigos y familiares. No hay, sin embargo, noticias fiables de su marcha pues los viajes que hizo entre 1824 y 1828 a Madrid desde Burdeos, así como sus cartas al Rey para solicitar su licencia y jubilación, no indican que estuviera perseguido. En febrero de 1824 Goya otorgó un poder general a Gabriel Ramiro para administrar su sueldo como funcionario de palacio y en junio, tras obtener la licencia del rey, marchó a Burdeos. Moratín narraba al abate Melón (27 de julio de 1824), amigo común, la llegada del artista, “sordo, viejo, torpe y débil, y sin saber una palabra de francés [...] y tan contento y deseoso de ver mundo”. De inmediato siguió viaje a París, donde los documentos de la policía, que espiaba a los exiliados políticos españoles, revelan que vivía solo, paseaba por los lugares públicos y visitaba los monumentos. Es posible que la intención del artista fuera visitar el Salon, que ese año se había retrasado hasta el mes de agosto.</br>Goya pintó en París dos sobrios retratos de sorprendente modernidad, los del político exiliado Joaquín María Ferrer y su mujer, y regresó a Burdeos en septiembre, donde se reunió con Leocadia Zorrilla y sus hijos. Tuvo algunos achaques y enfermedades en esos años que, sin embargo, no le impidieron hacer cuatro viajes a Madrid para solucionar sus asuntos y, seguramente, visitar a su hijo y a su nieto. Moratín dio cuenta regularmente a sus conocidos madrileños de la vida y la salud de Goya: “Goya, con sus setenta y nueve pascuas floridas y sus alifafes ni sabe lo que espera ni lo que quiere [...] le gusta la ciudad, el campo, el clima, los comestibles, la independencia y la tranquilidad que disfruta”. Pintó sólo retratos de algunos de sus amigos, como el del propio Moratín (Bilbao, Museo de Bellas Artes), el de Jacques Galos (Filadelfia, Barnes Foundation) y los últimos, pocos meses antes de morir, de su nieto Mariano (Dallas - Texas-, Meadows Museum) y del comerciante Juan Bautista de Muguiro (Madrid, Museo del Prado). Su actividad se centró en obras íntimas, de pequeño formato, como una serie de miniaturas sobre marfil, de las que se conocen algunos ejemplos, con figuras singulares, expresivas y mordaces, que Goya describió como más cercanas a “los pinceles de Velázquez que a los de Mengs” por la libertad y expresiva fuerza de las pinceladas. El período de Burdeos se define, sin duda, por las obras sobre papel, como los dibujos a lápiz negro de los Álbumes G y H, con escenas inspiradas en la realidad y otras en las que recurrió a recuerdos y temas que le habían interesado siempre, como las sátiras contra el clero o sobre el engaño y la locura, y figuras distorsionadas con una estética que precede al expresionismo del siglo XX. Se apasionó entonces por una técnica nueva, la litografía,  y se sirvió del establecimiento de Cyprien Gaulon para imprimir la serie de los Toros de Burdeos, impresionantes visiones de la “fiesta nacional” que sobrecogen por su gran tamaño y su brutal denuncia de la violencia del ser humano, idea que le había preocupado toda su vida. Cuando murió era apreciado solamente por el pequeño grupo de amigos y familiares que le acompañaron fielmente hasta el final, pues su arte, profundamente individual, estaba lejos de las modas del momento. Murió en la noche del 15 al 16 de abril de 1828, descrita con realismo estremecedor por Leocadia Zorrilla, y fue enterrado en el cementerio de la Chartreuse en la misma tumba que su consuegro, Martín Miguel de Goicoechea. Años después, los que se creyeron sus restos mortales se trasladaron a Madrid, donde reposan en la ermita de San Antonio de la Florida, bajos los frescos que había pintado en 1798.</br></br>Obras de ~: Adoración del Nombre de Dios, coreto, basílica del Pilar, Zaragoza, 1772; Escenas de la vida de la Virgen, cartuja de Aula Dei, Zaragoza, 1772-1774; La riña en la Venta Nueva, 1777; La cometa, 1777; El cacharrero, 1779; Niños jugando a soldados, 1779; Regina Martyrum, cúpula de la basílica del Pilar, Zaragoza, 1780-1781; Cristo en la Cruz, 1780; La familia del infante don Luis de Borbón, 1783; Las Cuatro Estaciones, 1786; Carlos III, cazador, c. 1786-1787; La familia de los duques de Osuna, 1787; El conde de Cabarrús, 1788; El niño Manuel Osorio y Zúñiga, c. 1788; San Francisco de Borja despidiéndose de su familia, 1788; La pradera de San Isidro, 1788; La gallina ciega, 1788; La boda, 1792; El pelele, 1792-1794; Cómicos ambulantes, 1993; El naufragio, 1793; Incendio de noche, 1793; Retrato de D. Agustín Ceán Bermúdez, c. 1795; Retrato de la marquesa de la Solana, 1795; Álbum de Sanlúcar (dibujos), c. 1795-1796; Álbum de Madrid (dibujos), c. 1795-1796; Retrato de la duquesa de Alba, vestida de blanco; Retrato de la actriz La Tirana, c. 1798; Frescos de la ermita de San Antonio de la Florida, ermita de San Antonio, Madrid, 1798; El Prendimiento, 1798; Retrato de Don Gaspar Melchor de Jovellanos, 1798; Serie de aguafuertes de “Los Caprichos”, 1799; Retrato ecuestre de la reina María Luisa de Parma, 1799; La maja desnuda, 1800; La maja vestida, c. 1800-1808; La familia de Carlos IV, 1800; La condesa de Chinchón, 1800; Retrato de don Manuel Godoy, 1801; El marqués de San Adrián, 1804; La marquesa de Santa Cruz, 1805; Fernando VII a caballo, 1808; Alegoría de la ciudad de Madrid, 1810; Serie de bodegones, c. 1808-1812; Serie del marqués de la Romana, c. 1800- 1808; Maja y celestina al balcón, c. 1808-1812; Las majas al balcón, c. 1808- 1812; Las viejas o El Tiempo, c. 1808-1812; El dos de mayo en Madrid, 1814; El tres de mayo en Madrid, 1814; Retrato de Fernando VII con manto real, 1814; Retrato del duque de San Carlos, 1815; El entierro de la sardina, c. 1815; Procesión de flagelantes, c. 1815; Escena de Inquisición, c. 1815; Escena de manicomio, c. 1815; Corrida de toros, c. 1815; Serie de aguafuertes de los “Desastres de la guerra”, 1812-1815; Serie de aguafuertes de “La Tauromaquia”, 1815; Fray Juan Fernández de Rojas, c. 1815; El X duque de Osuna, 1816; La duquesa de Abrantes, 1816; Santas Justa y Rufina, sacristía de los Cálices, catedral de Sevilla, 1817; Santa Isabel de Hungría curando a una enferma, 1816-1817; La última comunión de San José de Calasanz, Madrid, Escuelas Pías, 1819; Serie de aguafuertes de “Los Disparates”, c. 1820; Autorretrato con el médico Arrieta, 1820; Serie de las “Pinturas Negras”, 1821-1823; Retrato de María Martínez de Puga, 1824; Retratos de Joaquín María Ferrer y Manuela Álvarez Coiñas de Ferrer, 1824; Retrato de Leandro Fernández de Moratín, 1824; Álbumes G y H (dibujos en Burdeos); Litografías de los “Toros de Burdeos”, 1825; Retrato de Juan Bautista de Muguiro, 1828 (Manuela B. Mena Marqués).</br></br>Su retrato realizado por Vicente López Portaña corresponde a la obra P00864 del Museo del Prado.";
            autor.Ecidoc_p96_E67_p4_gave_birth_date = "1746"; //new DateTime(1746, 3, 30);
            autor.Ecidoc_p96_E67_p4_gave_birth_year = 1976;
            autor.Ecidoc_p96_E67_p7_gave_birth_place = "Fuendetodos, Zaragoza";
            return autor;
        }

        private static E22_Man_Made_Object CreateMeninas()
        {
            //Documento
            C1003_Manifestation documento = new C1003_Manifestation();
            documento.Efrbrer_p3020_has_title_of_the_manifestation = "Velazquez";
            documento.Efrbrer_P3055_has_date_of_publication_or_distribution = "1962";

            //Imagen
            E36_Visual_Item imagen = new E36_Visual_Item();
            imagen.Ecidoc_p1_is_identified_by = "9516";
            imagen.Ecidoc_p2_has_type = "Imagen de la obra completa";
            imagen.Ecidoc_p3_has_note = "imagenes/Documentos/imgsem/9f/9fdc/9fdc7800-9ade-48b0-ab8b-edee94ea877f/668702bb-e92e-467c-bd03-ce2a3ddc6277.jpg";
            imagen.Pm_imageHeight = 2719;
            imagen.Pm_imageWidth = 2362;
            imagen.Pm_isMain = true;

            //Autoría
            E22_E39_Man_Made_Object__Actor autoria = new E22_E39_Man_Made_Object__Actor();
            autoria.Ecidoc_order = 1;
            autoria.IdPm_author = "http://try.gnoss.com/items/E39_Actor_bb2c2448-ec14-42fa-a663-e372f626b771_4a154c4e-babb-402c-bd90-ab83102fdbe1";

            E22_Man_Made_Object obra = new E22_Man_Made_Object();
            obra.Cidoc_p1_is_identified_by = "OBR01";
            obra.Cidoc_p70i_is_documented_in = new List<C1003_Manifestation>() { documento };
            obra.Cidoc_p14_carried_out_by = new List<E22_E39_Man_Made_Object__Actor>() { autoria };
            obra.Cidoc_p14_carried_out_by = new List<E22_E39_Man_Made_Object__Actor>() { autoria };
            obra.Ecidoc_p102_E35_p3_has_title = "Las Meninas";
            obra.Cidoc_p3_has_note = "<p>Es una de las obras de mayor tamaño de Velázquez y en la que puso un mayor empeño para crear una composición a la vez compleja y creíble, que transmitiera la sensación de vida y realidad, y al mismo tiempo encerrara una densa red de significados. El pintor alcanzó su objetivo y el cuadro se convirtió en la única pintura a la que el tratadista Antonio Palomino dedicó un epígrafe en su historia de los pintores españoles (1724). Lo tituló <em>En que se describe la más ilustre obra de don Diego Velázquez, </em>y desde entonces no ha perdido su estatus de obra maestra. Gracias a Palomino sabemos que se pintó en 1656 en el Cuarto del Príncipe del Alcázar de Madrid, que es el escenario de la acción. El tratadista cordobés también identificó a la mayor parte de los personajes: son servidores palaciegos, que se disponen alrededor de la infanta Margarita, a la que atienden doña María Agustina Sarmiento y doña Isabel de Velasco, <em>meninas </em>de la reina. Además de ese grupo, vemos a Velázquez trabajar ante un gran lienzo, a los enanos Mari Bárbola y Nicolasito Pertusato, que azuza a un mastín, a la dama de honor doña Marcela de Ulloa, junto a un guardadamas, y, al fondo, tras la puerta, asoma José Nieto, aposentador. En el espejo se ven reflejados los rostros de Felipe IV y Mariana de Austria, padres de la infanta y testigos de la escena. Los personajes habitan un espacio modelado no sólo mediante las leyes de la perspectiva científica sino también de la perspectiva aérea, en cuya definición representa un papel importante la multiplicación de las fuentes de luz.</p><p><em>Las meninas</em> tiene un significado inmediato accesible a cualquier espectador. Es un retrato de grupo realizado en un espacio concreto y protagonizado por personajes identificables que llevan a cabo acciones comprensibles. Sus valores estéticos son también evidentes: su escenario es uno de los espacios más creíbles que nos ha dejado la pintura occidental; su composición aúna la unidad con la variedad; los detalles de extraordinaria belleza se reparten por toda la superficie pictórica; y el pintor ha dado un paso decisivo en el camino hacia el ilusionismo, que fue una de las metas de la pintura europea de la Edad Moderna, pues ha ido más allá de la transmisión del <em>parecido </em>y ha buscado con éxito la representación de la <em>vida </em>o la animación. Pero, como es habitual en Velázquez, en esta escena en la que la infanta y los servidores interrumpen lo que hacen ante la aparición de los reyes, subyacen numerosos significados, que pertenecen a campos de la experiencia diferentes y que la convierten en una de las obras maestras de la pintura occidental que ha sido objeto de una mayor cantidad y variedad de interpretaciones. Existe, por ejemplo, una reflexión sobre la identidad regia de la infanta, lo que, por extensión llena el cuadro de contenido político. Pero también hay varias referencias importantes de carácter histórico-artístico, que se encarnan en el propio pintor o en los cuadros que cuelgan de la pared del fondo; y la presencia del espejo convierte el cuadro en una reflexión sobre el acto de ver y hace que el espectador se pregunte sobre las leyes de la representación, sobre los límites entre pintura y realidad y sobre su propio papel dentro del cuadro.</p><p>Esa riqueza y variedad de contenidos, así como la complejidad de su composición y la variedad de las acciones que representa, hacen que <em>Las meninas</em> sea un <em>retrato </em>en el que su autor utiliza estrategias de representación y persigue unos objetivos que desbordan los habituales en ese género y lo acercan a la <em>pintura de historia. </em>En ese sentido, constituye uno de los lugares principales a través de los cuales Velázquez reivindicó las posibilidades del principal género pictórico al que se había dedicado desde que se estableció en la corte en 1623 (Texto extractado de Portús, J.: <em>Velázquez y la familia de Felipe IV,</em> Museo Nacional del Prado, 2013, p. 126).</p>";
            return obra;
        }

        private static bool addTriplesMeninasImagen()
        {
            E36_Visual_Item imagen = new E36_Visual_Item();
            imagen.Ecidoc_p1_is_identified_by = "9516";
            imagen.Ecidoc_p2_has_type = "Imagen de la obra completa";
            imagen.Ecidoc_p3_has_note = "imagenes/Documentos/imgsem/9f/9fdc/9fdc7800-9ade-48b0-ab8b-edee94ea877f/668702bb-e92e-467c-bd03-ce2a3ddc6277.jpg";
            imagen.Pm_imageHeight = 2719;
            imagen.Pm_imageWidth = 2362;
            imagen.Pm_isMain = true;

            string guid1EntPrincipal = "d1e48925-53b8-4266-9af5-e38840dfb352"; //ResourceID de las meninas
            string guidImagen = Guid.NewGuid().ToString(); // Article Id
            string valorBase = "http://try.gnoss.com/items/E36_Visual_Item_" + guid1EntPrincipal + "_" + guidImagen;
            List<TriplesToInclude> listTriples = new List<TriplesToInclude>();
            string propiedadApunataAAuxiliar = "http://www.cidoc-crm.org/cidoc-crm#p65_shows_visual_item";
            listTriples.Add(new TriplesToInclude
            {
                Description = false,
                Title = false,
                Predicate = $"{propiedadApunataAAuxiliar}|http://museodelprado.es/ontologia/ecidoc.owl#p1_is_identified_by",
                NewValue = $"{valorBase}|{imagen.Ecidoc_p1_is_identified_by}"
            });
            listTriples.Add(new TriplesToInclude
            {
                Description = false,
                Title = false,
                Predicate = $"{propiedadApunataAAuxiliar}|http://museodelprado.es/ontologia/ecidoc.owl#p2_has_type",
                NewValue = $"{valorBase}|{imagen.Ecidoc_p2_has_type}"
            });
            listTriples.Add(new TriplesToInclude
            {
                Description = false,
                Title = false,
                Predicate = $"{propiedadApunataAAuxiliar}|http://museodelprado.es/ontologia/pradomuseum.owl#imageHeight",
                NewValue = $"{valorBase}|{imagen.Pm_imageHeight}"
            });
            listTriples.Add(new TriplesToInclude
            {
                Description = false,
                Title = false,
                Predicate = $"{propiedadApunataAAuxiliar}|http://museodelprado.es/ontologia/pradomuseum.owl#imageWidth",
                NewValue = $"{valorBase}|{imagen.Pm_imageWidth}"
            });
            listTriples.Add(new TriplesToInclude
            {
                Description = false,
                Title = false,
                Predicate = $"{propiedadApunataAAuxiliar}|http://museodelprado.es/ontologia/pradomuseum.owl#isMain",
                NewValue = $"{valorBase}|{imagen.Pm_isMain}"
            });
            listTriples.Add(new TriplesToInclude
            {
                Description = false,
                Title = false,
                Predicate = $"{propiedadApunataAAuxiliar}|http://museodelprado.es/ontologia/ecidoc.owl#p3_has_note",
                NewValue = $"{valorBase}|{imagen.Ecidoc_p3_has_note}"
            }

         
            /*
            listTriples.Add(new TriplesToInclude
            {
                Description = false, ¿Es la propiedad de la descripción del recurso?
                Title = false, ¿Es la propiedad del título del recurso?
                Predicate = $"http://schema.org/name", URI de la propiedad
                NewValue = $"Pedro"  , Nuevo valor
            }
            
            */
            );

            Dictionary<Guid, List<TriplesToInclude>> dictionaryToModify = new Dictionary<Guid, List<TriplesToInclude>>();

            dictionaryToModify.Add(new Guid(guid1EntPrincipal), listTriples);

            if (!mResourceApi.InsertPropertiesLoadedResources(dictionaryToModify)[new Guid(guid1EntPrincipal)])
            {
                throw new Exception();
            }
            else
            {
                Console.WriteLine("Propiedades insertadas CORRECTAMENTE");
                mResourceApi.Log.Info("Propiedades insertadas CORRECTAMENTE.");
            }

            return true;
        }

        private static bool setTriplesMeninas()
        {

            bool esImgPrincipal = false;
            string guid1EntPrincipal = "d1e48925-53b8-4266-9af5-e38840dfb352";
            string guidImagen = "e961d8f6-6a4a-4352-8df8-b65a7f3d0a21";
            string valorBase = "http://try.gnoss.com/items/E36_Visual_Item_" + guid1EntPrincipal + "_" + guidImagen;
            List<TriplesToModify> listTriples = new List<TriplesToModify>();
            string propiedadApunataAAuxiliar = "http://www.cidoc-crm.org/cidoc-crm#p65_shows_visual_item";

            listTriples.Add(new TriplesToModify
            {
                Description = false,
                Title = false,
                Predicate = $"{propiedadApunataAAuxiliar}|http://museodelprado.es/ontologia/pradomuseum.owl#isMain",
                NewValue = $"{valorBase}|{esImgPrincipal}",
                OldValue = $"{valorBase}|{!esImgPrincipal}"
            });

            Dictionary<Guid, List<TriplesToModify>> dictionaryToModify = new Dictionary<Guid, List<TriplesToModify>>();

            dictionaryToModify.Add(new Guid(guid1EntPrincipal), listTriples);

            if (!mResourceApi.ModifyPropertiesLoadedResources(dictionaryToModify)[new Guid(guid1EntPrincipal)])
            {
                throw new Exception();
            }
            else
            {
                Console.WriteLine("Propiedades modificadas CORRECTAMENTE");
                mResourceApi.Log.Info("Propiedades modificadas CORRECTAMENTE.");
            }

            return true;
        }

        private static bool deleteTriplesMeninas()
        {

            bool esImgPrincipal = false;
            string guid1EntPrincipal = "d1e48925-53b8-4266-9af5-e38840dfb352";
            string guidImagen = "e961d8f6-6a4a-4352-8df8-b65a7f3d0a21";
            string valorBase = "http://try.gnoss.com/items/E36_Visual_Item_" + guid1EntPrincipal + "_" + guidImagen;
            List<RemoveTriples> listTriples = new List<RemoveTriples>();
            string propiedadApunataAAuxiliar = "http://www.cidoc-crm.org/cidoc-crm#p65_shows_visual_item";

            listTriples.Add(new RemoveTriples
            {
                Description = false,
                Title = false,
                Predicate = $"{propiedadApunataAAuxiliar}|http://museodelprado.es/ontologia/pradomuseum.owl#isMain",
                Value = $"{valorBase}|{esImgPrincipal}"
            });

            Dictionary<Guid, List<RemoveTriples>> dictionaryToModify = new Dictionary<Guid, List<RemoveTriples>>();

            dictionaryToModify.Add(new Guid(guid1EntPrincipal), listTriples);

            if (!mResourceApi.DeletePropertiesLoadedResources(dictionaryToModify)[new Guid(guid1EntPrincipal)])
            {
                throw new Exception();
            }
            else
            {
                Console.WriteLine("Propiedades eliminadas CORRECTAMENTE");
                mResourceApi.Log.Info("Propiedades eliminadas CORRECTAMENTE.");
            }

            return true;
        }


        /*------------------------     TESAURO     -------------------------------------------*/

        public static void cargarTesauroTécnicas()
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

            mResourceApi.ChangeOntoly("pma_taxonomy");
            Pma_taxonomyOntology.Collection tesauroTipoBase = new Pma_taxonomyOntology.Collection();
            string base_uri = "http://testing.gnoss.com/items/technique_"; 
            tesauroTipoBase.Dc_source = "technique";
            tesauroTipoBase.Skos2_scopeNote = "technique"; //Funciona:tpm_tec

            /*Creo las uris de los Concept del primer nivel*/
            string uri = "";
            tesauroTipoBase.IdsSkos2_member = new List<string>();
            foreach (var elemento in l_nivel1)
            {
                uri = base_uri + elemento.id;
                tesauroTipoBase.IdsSkos2_member.Add(uri);
            }

            //Cargo la Collection que tienen asignados los Concept del primer nivel
             
            mResourceApi.LoadSecondaryResource(tesauroTipoBase.ToGnossApiResource(mResourceApi, "technique"));

            /*Cargo el primer nivel*/
            Dictionary<string, string> nombre_Uri_Tax = new Dictionary<string, string>();
            foreach (var elemento_n1 in l_nivel1)
            {
                Pma_taxonomyOntology.Concept concept_tax = new Pma_taxonomyOntology.Concept();
                List<string> list_ident_nivel_2 = new List<string>();
                concept_tax.Dc_identifier = elemento_n1.id;
                concept_tax.Dc_source = "technique";
                concept_tax.Skos2_prefLabel = elemento_n1.nombre;
                concept_tax.Skos2_symbol = "1";

                //Asigno al padre, que en el primer nivel es el propio Tesauro
                concept_tax.IdsSkos2_broader = new List<string>()
                {
                    "http://testing.gnoss.com/items/technique"
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
                mResourceApi.LoadSecondaryResource(concept_tax.ToGnossApiResource(mResourceApi, "technique_" + elemento_n1.id));
            }

            /*Cargo el segundo nivel*/
            Pma_taxonomyOntology.Concept concept_tax_2 = new Pma_taxonomyOntology.Concept();

            foreach (var elemento_n2 in l_nivel2)
            {
                concept_tax_2 = new Pma_taxonomyOntology.Concept();
                concept_tax_2.Dc_identifier = elemento_n2.id;
                concept_tax_2.Dc_source = "technique";
                concept_tax_2.Skos2_symbol = "2";
                concept_tax_2.Skos2_prefLabel = elemento_n2.nombre;
                concept_tax_2.IdsSkos2_broader = new List<string>();
                ElemntoTesauro padre = l_nivel1.Where(e => e.id.Split('_')[0].Equals(elemento_n2.id.Split('_')[0].Substring(1, 1))).Last();
                string uriPadre = base_uri + padre.id;
                concept_tax_2.IdsSkos2_broader.Add(uriPadre);
                mResourceApi.LoadSecondaryResource(concept_tax_2.ToGnossApiResource(mResourceApi, "technique_" + elemento_n2.id));
            }
        }

        public static void borrarNivelesTesauro(string nivel)
        {
            mResourceApi.ChangeOntoly("pma_taxonomy");
            //Consulta -> Todos los de un nivel.
            string select = string.Empty, where = string.Empty;
            string pOntology = "pma_taxonomy";
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p <http://www.w3.org/2008/05/skos#Concept>";
            //where += $@"FILTER regex(str(?s), 'http://testing.gnoss.com/items/carqc_{nivel}_').";
            where += $@"FILTER regex(str(?s), '*n{nivel}').";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology);
            List<string> listaUrisABorrar = new List<string>();
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    listaUrisABorrar.Add(fila["s"].value);
                }
                mResourceApi.DeleteSecondaryEntitiesList(ref listaUrisABorrar);
            }
        }

        public static void borrarTesauroEntero()
        {
            mResourceApi.ChangeOntoly("pma_taxonomy");
            //Consulta -> Todos los de un nivel.
            string select = string.Empty, where = string.Empty;
            string pOntology = "pma_taxonomy";
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p 'technique'";
            where += $@"}}";

            SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology);
            List<string> listaUrisABorrar = new List<string>();
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    listaUrisABorrar.Add(fila["s"].value);
                }
                mResourceApi.DeleteSecondaryEntitiesList(ref listaUrisABorrar);
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


    //DESERIALIZACIÓN DE JSON//


    public class Actor
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Binding
    {
        public Actor actor { get; set; }
        public P p { get; set; }
        public Valor valor { get; set; }
    }

    public class Head
    {
        public List<object> link { get; set; }
        public List<string> vars { get; set; }
    }

    public class P
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Results
    {
        public bool distinct { get; set; }
        public bool ordered { get; set; }
        public List<Binding> bindings { get; set; }
    }

    public class Root
    {
        public Head head { get; set; }
        public Results results { get; set; }
    }

    public class Valor
    {
        public string type { get; set; }
        public string value { get; set; }

        [JsonProperty("xml:lang")]
        public string XmlLang { get; set; }
    }






}