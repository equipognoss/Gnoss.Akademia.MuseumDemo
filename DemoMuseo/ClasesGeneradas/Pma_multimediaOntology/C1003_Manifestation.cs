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
using E22_Man_Made_Object = Pma_obraOntology.E22_Man_Made_Object;

namespace Pma_multimediaOntology
{
	[ExcludeFromCodeCoverage]
	public class C1003_Manifestation : GnossOCBase
	{

		public C1003_Manifestation() : base() { } 

		public C1003_Manifestation(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Pm_relatedArtWork = new List<E22_Man_Made_Object>();
			SemanticPropertyModel propPm_relatedArtWork = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#relatedArtWork");
			if(propPm_relatedArtWork != null && propPm_relatedArtWork.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_relatedArtWork.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E22_Man_Made_Object pm_relatedArtWork = new E22_Man_Made_Object(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_relatedArtWork.Add(pm_relatedArtWork);
					}
				}
			}
			this.Pm_publicationDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#publicationDate"));
			this.Efrbrer_p3020_has_title_of_the_manifestation = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation"));
		}

		public C1003_Manifestation(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Pm_relatedArtWork = new List<E22_Man_Made_Object>();
			SemanticPropertyModel propPm_relatedArtWork = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#relatedArtWork");
			if(propPm_relatedArtWork != null && propPm_relatedArtWork.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_relatedArtWork.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E22_Man_Made_Object pm_relatedArtWork = new E22_Man_Made_Object(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_relatedArtWork.Add(pm_relatedArtWork);
					}
				}
			}
			this.Pm_publicationDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#publicationDate"));
			this.Efrbrer_p3020_has_title_of_the_manifestation = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation"));
		}

		public virtual string RdfType { get { return "http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation"; } }
		public virtual string RdfsLabel { get { return "http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation"; } }
		[LABEL(LanguageEnum.es,"Obra")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#relatedArtWork")]
		public  List<E22_Man_Made_Object> Pm_relatedArtWork { get; set;}
		public List<string> IdsPm_relatedArtWork { get; set;}

		[LABEL(LanguageEnum.es,"Fecha de publicacion")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#publicationDate")]
		public  DateTime? Pm_publicationDate { get; set;}

		[LABEL(LanguageEnum.es,"http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation")]
		[RDFProperty("http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation")]
		public  string Efrbrer_p3020_has_title_of_the_manifestation { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("pm:relatedArtWork", this.IdsPm_relatedArtWork));
			if (this.Pm_publicationDate.HasValue){
				propList.Add(new DateOntologyProperty("pm:publicationDate", this.Pm_publicationDate.Value));
				}
			propList.Add(new StringOntologyProperty("efrbrer:p3020_has_title_of_the_manifestation", this.Efrbrer_p3020_has_title_of_the_manifestation));
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
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdsPm_relatedArtWork != null)
				{
					foreach(var item2 in this.IdsPm_relatedArtWork)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#relatedArtWork", $"<{item2}>", list, " . ");
					}
				}
				if(this.Pm_publicationDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/pradomuseum.owl#publicationDate", $"\"{this.Pm_publicationDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Efrbrer_p3020_has_title_of_the_manifestation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation", $"\"{GenerarTextoSinSaltoDeLinea(this.Efrbrer_p3020_has_title_of_the_manifestation)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"pma_multimedia\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Efrbrer_p3020_has_title_of_the_manifestation)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Efrbrer_p3020_has_title_of_the_manifestation)}\"", list, " . ");
			string search = string.Empty;
				if(this.IdsPm_relatedArtWork != null)
				{
					foreach(var item2 in this.IdsPm_relatedArtWork)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/pradomuseum.owl#relatedArtWork", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Pm_publicationDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/pradomuseum.owl#publicationDate", $"{this.Pm_publicationDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Efrbrer_p3020_has_title_of_the_manifestation != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation", $"\"{GenerarTextoSinSaltoDeLinea(this.Efrbrer_p3020_has_title_of_the_manifestation).ToLower()}\"", list, " . ");
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
			string titulo = $"{this.Efrbrer_p3020_has_title_of_the_manifestation.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/Pma_multimediaOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Efrbrer_p3020_has_title_of_the_manifestation;
		}





	}
}
