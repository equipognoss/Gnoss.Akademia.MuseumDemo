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
using Concept = Pma_taxonomyOntology.Concept;

namespace Pma_taxonomyOntology
{
	[ExcludeFromCodeCoverage]
	public class Concept : GnossOCBase
	{

		public Concept() : base() { } 

		public Concept(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Skos2_related = new List<Concept>();
			SemanticPropertyModel propSkos2_related = pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#related");
			if(propSkos2_related != null && propSkos2_related.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSkos2_related.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Concept skos2_related = new Concept(propValue.RelatedEntity,idiomaUsuario);
						this.Skos2_related.Add(skos2_related);
					}
				}
			}
			this.Skos2_broader = new List<Concept>();
			SemanticPropertyModel propSkos2_broader = pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#broader");
			if(propSkos2_broader != null && propSkos2_broader.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSkos2_broader.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Concept skos2_broader = new Concept(propValue.RelatedEntity,idiomaUsuario);
						this.Skos2_broader.Add(skos2_broader);
					}
				}
			}
			this.Skos2_narrower = new List<Concept>();
			SemanticPropertyModel propSkos2_narrower = pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#narrower");
			if(propSkos2_narrower != null && propSkos2_narrower.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSkos2_narrower.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Concept skos2_narrower = new Concept(propValue.RelatedEntity,idiomaUsuario);
						this.Skos2_narrower.Add(skos2_narrower);
					}
				}
			}
			this.Skos2_symbol = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#symbol"));
			this.Skos2_note = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#note"));
			this.Dc_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/identifier"));
			this.Skos2_prefLabel = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#prefLabel"));
			this.Dc_source = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/source"));
		}

		public virtual string RdfType { get { return "http://www.w3.org/2008/05/skos#Concept"; } }
		public virtual string RdfsLabel { get { return "http://www.w3.org/2008/05/skos#Concept"; } }
		[LABEL(LanguageEnum.es,"Nodos relacionados")]
		[RDFProperty("http://www.w3.org/2008/05/skos#related")]
		public  List<Concept> Skos2_related { get; set;}
		public List<string> IdsSkos2_related { get; set;}

		[LABEL(LanguageEnum.es,"Padres")]
		[RDFProperty("http://www.w3.org/2008/05/skos#broader")]
		public  List<Concept> Skos2_broader { get; set;}
		public List<string> IdsSkos2_broader { get; set;}

		[LABEL(LanguageEnum.es,"Hijos")]
		[RDFProperty("http://www.w3.org/2008/05/skos#narrower")]
		public  List<Concept> Skos2_narrower { get; set;}
		public List<string> IdsSkos2_narrower { get; set;}

		[LABEL(LanguageEnum.es,"Símbolo")]
		[RDFProperty("http://www.w3.org/2008/05/skos#symbol")]
		public  string Skos2_symbol { get; set;}

		[RDFProperty("http://www.w3.org/2008/05/skos#note")]
		public  string Skos2_note { get; set;}

		[LABEL(LanguageEnum.es,"Identificador")]
		[RDFProperty("http://purl.org/dc/elements/1.1/identifier")]
		public  string Dc_identifier { get; set;}

		[LABEL(LanguageEnum.es,"Nombre de la categoria")]
		[RDFProperty("http://www.w3.org/2008/05/skos#prefLabel")]
		public  string Skos2_prefLabel { get; set;}

		[LABEL(LanguageEnum.es,"Fuente")]
		[RDFProperty("http://purl.org/dc/elements/1.1/source")]
		public  string Dc_source { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("skos2:related", this.IdsSkos2_related));
			propList.Add(new ListStringOntologyProperty("skos2:broader", this.IdsSkos2_broader));
			propList.Add(new ListStringOntologyProperty("skos2:narrower", this.IdsSkos2_narrower));
			propList.Add(new StringOntologyProperty("skos2:symbol", this.Skos2_symbol));
			propList.Add(new StringOntologyProperty("skos2:note", this.Skos2_note));
			propList.Add(new StringOntologyProperty("dc:identifier", this.Dc_identifier));
			propList.Add(new StringOntologyProperty("skos2:prefLabel", this.Skos2_prefLabel));
			propList.Add(new StringOntologyProperty("dc:source", this.Dc_source));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
		public virtual SecondaryResource ToGnossApiResource(ResourceApi resourceAPI,string identificador)
		{
			SecondaryResource resource = new SecondaryResource();
			List<SecondaryEntity> listSecondaryEntity = null;
			GetProperties();
			SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "http://www.w3.org/2008/05/skos#Concept", "http://www.w3.org/2008/05/skos#Concept", prefList, propList,identificador,listSecondaryEntity, null);
			resource.SecondaryOntology = ontology;
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://www.w3.org/2008/05/skos#Concept>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://www.w3.org/2008/05/skos#Concept\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdsSkos2_related != null)
				{
					foreach(var item2 in this.IdsSkos2_related)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}", "http://www.w3.org/2008/05/skos#related", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSkos2_broader != null)
				{
					foreach(var item2 in this.IdsSkos2_broader)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}", "http://www.w3.org/2008/05/skos#broader", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSkos2_narrower != null)
				{
					foreach(var item2 in this.IdsSkos2_narrower)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}", "http://www.w3.org/2008/05/skos#narrower", $"<{item2}>", list, " . ");
					}
				}
				if(this.Skos2_symbol != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}",  "http://www.w3.org/2008/05/skos#symbol", $"\"{GenerarTextoSinSaltoDeLinea(this.Skos2_symbol)}\"", list, " . ");
				}
				if(this.Skos2_note != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}",  "http://www.w3.org/2008/05/skos#note", $"\"{GenerarTextoSinSaltoDeLinea(this.Skos2_note)}\"", list, " . ");
				}
				if(this.Dc_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/identifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_identifier)}\"", list, " . ");
				}
				if(this.Skos2_prefLabel != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}",  "http://www.w3.org/2008/05/skos#prefLabel", $"\"{GenerarTextoSinSaltoDeLinea(this.Skos2_prefLabel)}\"", list, " . ");
				}
				if(this.Dc_source != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Concept_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/source", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_source)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			string search = string.Empty;
				if(this.IdsSkos2_related != null)
				{
					foreach(var item2 in this.IdsSkos2_related)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/2008/05/skos#related", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSkos2_broader != null)
				{
					foreach(var item2 in this.IdsSkos2_broader)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/2008/05/skos#broader", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSkos2_narrower != null)
				{
					foreach(var item2 in this.IdsSkos2_narrower)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/2008/05/skos#narrower", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Skos2_symbol != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.w3.org/2008/05/skos#symbol", $"\"{GenerarTextoSinSaltoDeLinea(this.Skos2_symbol).ToLower()}\"", list, " . ");
				}
				if(this.Skos2_note != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.w3.org/2008/05/skos#note", $"\"{GenerarTextoSinSaltoDeLinea(this.Skos2_note).ToLower()}\"", list, " . ");
				}
				if(this.Dc_identifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/elements/1.1/identifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_identifier).ToLower()}\"", list, " . ");
				}
				if(this.Skos2_prefLabel != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.w3.org/2008/05/skos#prefLabel", $"\"{GenerarTextoSinSaltoDeLinea(this.Skos2_prefLabel).ToLower()}\"", list, " . ");
				}
				if(this.Dc_source != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/elements/1.1/source", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_source).ToLower()}\"", list, " . ");
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
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>();

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/Pma_taxonomyOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
		}





	}
}
