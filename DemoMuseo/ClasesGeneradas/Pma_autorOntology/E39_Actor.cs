using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper.Helpers;
using GnossBase;
using Es.Riam.Gnoss.Web.MVC.Models;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using Gnoss.ApiWrapper.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Pma_autorOntology
{
	[ExcludeFromCodeCoverage]
	public class E39_Actor : GnossOCBase
	{

		public E39_Actor() : base() { } 

		public E39_Actor(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Ecidoc_p65_E36_shows_visual_item = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p65_E36_shows_visual_item"));
			this.Ecidoc_p96_E67_p7_gave_birth_place = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p7_gave_birth_place"));
			this.Cidoc_p3_has_note = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p3_has_note"));
			this.Ecidoc_p96_E67_p4_gave_birth_year = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_year"));
			this.Ecidoc_p96_E67_p4_gave_birth_date = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_date"));
			this.Ecidoc_p100i_E69_p4_death_date = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_date"));
			this.Ecidoc_p131_E82_p102_has_title = new Dictionary<LanguageEnum,string>();
			this.Ecidoc_p131_E82_p102_has_title.Add(idiomaUsuario , GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p131_E82_p102_has_title")));
			
			this.Pm_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#identifier"));
			SemanticPropertyModel propPm_period = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#period");
			this.Pm_period = new List<string>();
			if (propPm_period != null && propPm_period.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_period.PropertyValues)
				{
					this.Pm_period.Add(propValue.Value);
				}
			}
			this.Ecidoc_p100i_E69_p4_death_year = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_year"));
			this.Ecidoc_p100i_E69_p7_death_place = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p7_death_place"));
		}

		public E39_Actor(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Ecidoc_p65_E36_shows_visual_item = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p65_E36_shows_visual_item"));
			this.Ecidoc_p96_E67_p7_gave_birth_place = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p7_gave_birth_place"));
			this.Cidoc_p3_has_note = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p3_has_note"));
			this.Ecidoc_p96_E67_p4_gave_birth_year = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_year"));
			this.Ecidoc_p96_E67_p4_gave_birth_date = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_date"));
			this.Ecidoc_p100i_E69_p4_death_date = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_date"));
			this.Ecidoc_p131_E82_p102_has_title = new Dictionary<LanguageEnum,string>();
			this.Ecidoc_p131_E82_p102_has_title.Add(idiomaUsuario , GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p131_E82_p102_has_title")));
			
			this.Pm_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#identifier"));
			SemanticPropertyModel propPm_period = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#period");
			this.Pm_period = new List<string>();
			if (propPm_period != null && propPm_period.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_period.PropertyValues)
				{
					this.Pm_period.Add(propValue.Value);
				}
			}
			this.Ecidoc_p100i_E69_p4_death_year = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_year"));
			this.Ecidoc_p100i_E69_p7_death_place = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p7_death_place"));
		}

		public virtual string RdfType { get { return "http://www.cidoc-crm.org/cidoc-crm#E39_Actor"; } }
		public virtual string RdfsLabel { get { return "http://www.cidoc-crm.org/cidoc-crm#E39_Actor"; } }
		[LABEL(LanguageEnum.es,"Foto")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p65_E36_shows_visual_item")]
		public  string Ecidoc_p65_E36_shows_visual_item { get; set;}

		[LABEL(LanguageEnum.es,"Lugar de nacimiento")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p7_gave_birth_place")]
		public  string Ecidoc_p96_E67_p7_gave_birth_place { get; set;}

		[LABEL(LanguageEnum.es,"Reseña")]
		[RDFProperty("http://www.cidoc-crm.org/cidoc-crm#p3_has_note")]
		public  string Cidoc_p3_has_note { get; set;}

		[LABEL(LanguageEnum.es,"Año de nacimiento")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_year")]
		public  int? Ecidoc_p96_E67_p4_gave_birth_year { get; set;}

		[LABEL(LanguageEnum.es,"Fecha de nacimiento")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_date")]
		public  string Ecidoc_p96_E67_p4_gave_birth_date { get; set;}

		[LABEL(LanguageEnum.es,"Fecha de defuncion")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_date")]
		public  string Ecidoc_p100i_E69_p4_death_date { get; set;}

		[LABEL(LanguageEnum.es,"Nombre")]
		[LABEL(LanguageEnum.en,"Name")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p131_E82_p102_has_title")]
		public  Dictionary<LanguageEnum,string> Ecidoc_p131_E82_p102_has_title { get; set;}

		[LABEL(LanguageEnum.es,"Identificador")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#identifier")]
		public  string Pm_identifier { get; set;}

		[LABEL(LanguageEnum.es,"Siglo")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#period")]
		public  List<string> Pm_period { get; set;}

		[LABEL(LanguageEnum.es,"Año de defuncion")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_year")]
		public  int? Ecidoc_p100i_E69_p4_death_year { get; set;}

		[LABEL(LanguageEnum.es,"Lugar de defuncion")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p7_death_place")]
		public  string Ecidoc_p100i_E69_p7_death_place { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("ecidoc:p65_E36_shows_visual_item", this.Ecidoc_p65_E36_shows_visual_item));
			propList.Add(new StringOntologyProperty("ecidoc:p96_E67_p7_gave_birth_place", this.Ecidoc_p96_E67_p7_gave_birth_place));
			propList.Add(new StringOntologyProperty("cidoc:p3_has_note", this.Cidoc_p3_has_note));
			propList.Add(new StringOntologyProperty("ecidoc:p96_E67_p4_gave_birth_year", this.Ecidoc_p96_E67_p4_gave_birth_year.ToString()));
			propList.Add(new StringOntologyProperty("ecidoc:p96_E67_p4_gave_birth_date", this.Ecidoc_p96_E67_p4_gave_birth_date));
			propList.Add(new StringOntologyProperty("ecidoc:p100i_E69_p4_death_date", this.Ecidoc_p100i_E69_p4_death_date));
			if(this.Ecidoc_p131_E82_p102_has_title != null)
			{
				foreach (LanguageEnum idioma in this.Ecidoc_p131_E82_p102_has_title.Keys)
				{
					propList.Add(new StringOntologyProperty("ecidoc:p131_E82_p102_has_title", this.Ecidoc_p131_E82_p102_has_title[idioma], idioma.ToString()));
				}
			}
			else
			{
				throw new GnossAPIException($"La propiedad ecidoc:p131_E82_p102_has_title debe tener al menos un valor en el recurso: {resourceID}");
			}
			propList.Add(new StringOntologyProperty("pm:identifier", this.Pm_identifier));
			propList.Add(new ListStringOntologyProperty("pm:period", this.Pm_period));
			propList.Add(new StringOntologyProperty("ecidoc:p100i_E69_p4_death_year", this.Ecidoc_p100i_E69_p4_death_year.ToString()));
			propList.Add(new StringOntologyProperty("ecidoc:p100i_E69_p7_death_place", this.Ecidoc_p100i_E69_p7_death_place));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology=null;
			GetProperties();
			if(idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList,idrecurso,idarticulo);
			}
			resource.Id = GNOSSID;
			resource.Ontology = ontology;
			resource.TextCategories=listaDeCategorias;
			AddResourceTitle(resource);
			AddResourceDescription(resource);
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://www.cidoc-crm.org/cidoc-crm#E39_Actor>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://www.cidoc-crm.org/cidoc-crm#E39_Actor\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.Ecidoc_p65_E36_shows_visual_item != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/ecidoc.owl#p65_E36_shows_visual_item", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p65_E36_shows_visual_item)}\"", list, " . ");
				}
				if(this.Ecidoc_p96_E67_p7_gave_birth_place != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p7_gave_birth_place", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p96_E67_p7_gave_birth_place)}\"", list, " . ");
				}
				if(this.Cidoc_p3_has_note != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}",  "http://www.cidoc-crm.org/cidoc-crm#p3_has_note", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p3_has_note)}\"", list, " . ");
				}
				if(this.Ecidoc_p96_E67_p4_gave_birth_year != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_year", $"{this.Ecidoc_p96_E67_p4_gave_birth_year.Value.ToString()}", list, " . ");
				}
				if(this.Ecidoc_p96_E67_p4_gave_birth_date != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_date", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p96_E67_p4_gave_birth_date)}\"", list, " . ");
				}
				if(this.Ecidoc_p100i_E69_p4_death_date != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_date", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p100i_E69_p4_death_date)}\"", list, " . ");
				}
				if(this.Ecidoc_p131_E82_p102_has_title != null)
				{
							foreach (LanguageEnum idioma in this.Ecidoc_p131_E82_p102_has_title.Keys)
							{
								AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p131_E82_p102_has_title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p131_E82_p102_has_title[idioma])}\"", list,  $"@{idioma} . ");
							}
				}
				if(this.Pm_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#identifier",  $"\"{GenerarTextoSinSaltoDeLinea(this.Pm_identifier)}\"", list, " . ");
				}
				if(this.Pm_period != null)
				{
					foreach(var item2 in this.Pm_period)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#period", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Ecidoc_p100i_E69_p4_death_year != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_year", $"{this.Ecidoc_p100i_E69_p4_death_year.Value.ToString()}", list, " . ");
				}
				if(this.Ecidoc_p100i_E69_p7_death_place != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Actor_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p7_death_place", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p100i_E69_p7_death_place)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"pma_autor\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://www.cidoc-crm.org/cidoc-crm#E39_Actor\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Pm_identifier)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Pm_identifier)}\"", list, " . ");
			string search = string.Empty;
				if(this.Ecidoc_p65_E36_shows_visual_item != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/ecidoc.owl#p65_E36_shows_visual_item", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p65_E36_shows_visual_item).ToLower()}\"", list, " . ");
				}
				if(this.Ecidoc_p96_E67_p7_gave_birth_place != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p7_gave_birth_place", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p96_E67_p7_gave_birth_place).ToLower()}\"", list, " . ");
				}
				if(this.Cidoc_p3_has_note != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.cidoc-crm.org/cidoc-crm#p3_has_note", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p3_has_note).ToLower()}\"", list, " . ");
				}
				if(this.Ecidoc_p96_E67_p4_gave_birth_year != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_year", $"{this.Ecidoc_p96_E67_p4_gave_birth_year.Value.ToString()}", list, " . ");
				}
				if(this.Ecidoc_p96_E67_p4_gave_birth_date != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/ecidoc.owl#p96_E67_p4_gave_birth_date", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p96_E67_p4_gave_birth_date).ToLower()}\"", list, " . ");
				}
				if(this.Ecidoc_p100i_E69_p4_death_date != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_date", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p100i_E69_p4_death_date).ToLower()}\"", list, " . ");
				}
				if(this.Ecidoc_p131_E82_p102_has_title != null)
				{
							foreach (LanguageEnum idioma in this.Ecidoc_p131_E82_p102_has_title.Keys)
							{
								AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/ecidoc.owl#p131_E82_p102_has_title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p131_E82_p102_has_title[idioma]).ToLower()}\"", list,  $"@{idioma} . ");
							}
				}
				if(this.Pm_identifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/pradomuseum.owl#identifier",  $"\"{GenerarTextoSinSaltoDeLinea(this.Pm_identifier).ToLower()}\"", list, " . ");
				}
				if(this.Pm_period != null)
				{
					foreach(var item2 in this.Pm_period)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/pradomuseum.owl#period", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Ecidoc_p100i_E69_p4_death_year != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p4_death_year", $"{this.Ecidoc_p100i_E69_p4_death_year.Value.ToString()}", list, " . ");
				}
				if(this.Ecidoc_p100i_E69_p7_death_place != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/ecidoc.owl#p100i_E69_p7_death_place", $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p100i_E69_p7_death_place).ToLower()}\"", list, " . ");
				}
			if (listaSearch != null && listaSearch.Count > 0)
			{
				foreach(string valorSearch in listaSearch)
				{
					search += $"{valorSearch} ";
				}
			}
			if(!string.IsNullOrEmpty(search))
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/search", $"\"{GenerarTextoSinSaltoDeLinea(search.ToLower())}\"", list, " . ");
			}
			return list;
		}

		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{

			//Insert en la tabla Documento
			string tags = "";
			foreach(string tag in tagList)
			{
				tags += $"{tag}, ";
			}
			if (!string.IsNullOrEmpty(tags))
			{
				tags = tags.Substring(0, tags.LastIndexOf(','));
			}
			string titulo = $"{this.Pm_identifier.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Ecidoc_p131_E82_p102_has_title.Values.First().Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/Pma_autorOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Pm_identifier;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			List<Multilanguage> listMultilanguageDescription = new List<Multilanguage>();
			foreach (LanguageEnum idioma in this.Ecidoc_p131_E82_p102_has_title.Keys)
			{
				listMultilanguageDescription.Add(new Multilanguage(this.Ecidoc_p131_E82_p102_has_title[idioma], idioma.ToString()));
			}
			resource.MultilanguageDescription = listMultilanguageDescription;
		}



		internal override void AddImages(ComplexOntologyResource pResource)
		{
			base.AddImages(pResource);
			List<ImageAction> actionListp65_E36_shows_visual_item = new List<ImageAction>();
		}

	}
}
