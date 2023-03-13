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
	public class Collection : GnossOCBase
	{

		public Collection() : base() { } 

		public Collection(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Skos2_member = new List<Concept>();
			SemanticPropertyModel propSkos2_member = pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#member");
			if(propSkos2_member != null && propSkos2_member.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSkos2_member.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Concept skos2_member = new Concept(propValue.RelatedEntity,idiomaUsuario);
						this.Skos2_member.Add(skos2_member);
					}
				}
			}
			this.Dc_source = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/source"));
			this.Skos2_scopeNote = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#scopeNote"));
		}

		public virtual string RdfType { get { return "http://www.w3.org/2008/05/skos#Collection"; } }
		public virtual string RdfsLabel { get { return "http://www.w3.org/2008/05/skos#Collection"; } }
		[LABEL(LanguageEnum.es,"Nodo raiz:")]
		[RDFProperty("http://www.w3.org/2008/05/skos#member")]
		public  List<Concept> Skos2_member { get; set;}
		public List<string> IdsSkos2_member { get; set;}

		[LABEL(LanguageEnum.es,"Identificador")]
		[RDFProperty("http://purl.org/dc/elements/1.1/source")]
		public  string Dc_source { get; set;}

		[LABEL(LanguageEnum.es,"Proposito")]
		[RDFProperty("http://www.w3.org/2008/05/skos#scopeNote")]
		public  string Skos2_scopeNote { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("skos2:member", this.IdsSkos2_member));
			propList.Add(new StringOntologyProperty("dc:source", this.Dc_source));
			propList.Add(new StringOntologyProperty("skos2:scopeNote", this.Skos2_scopeNote));
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
			SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "http://www.w3.org/2008/05/skos#Collection", "http://www.w3.org/2008/05/skos#Collection", prefList, propList,identificador,listSecondaryEntity, null);
			resource.SecondaryOntology = ontology;
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Collection_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://www.w3.org/2008/05/skos#Collection>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Collection_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://www.w3.org/2008/05/skos#Collection\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Collection_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdsSkos2_member != null)
				{
					foreach(var item2 in this.IdsSkos2_member)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Collection_{ResourceID}_{ArticleID}", "http://www.w3.org/2008/05/skos#member", $"<{item2}>", list, " . ");
					}
				}
				if(this.Dc_source != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Collection_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/source", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_source)}\"", list, " . ");
				}
				if(this.Skos2_scopeNote != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Collection_{ResourceID}_{ArticleID}",  "http://www.w3.org/2008/05/skos#scopeNote", $"\"{GenerarTextoSinSaltoDeLinea(this.Skos2_scopeNote)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			string search = string.Empty;
				if(this.IdsSkos2_member != null)
				{
					foreach(var item2 in this.IdsSkos2_member)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/2008/05/skos#member", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Dc_source != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/elements/1.1/source", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_source).ToLower()}\"", list, " . ");
				}
				if(this.Skos2_scopeNote != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.w3.org/2008/05/skos#scopeNote", $"\"{GenerarTextoSinSaltoDeLinea(this.Skos2_scopeNote).ToLower()}\"", list, " . ");
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
