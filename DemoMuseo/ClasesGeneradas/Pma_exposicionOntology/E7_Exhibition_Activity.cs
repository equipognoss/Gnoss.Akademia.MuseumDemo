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

namespace Pma_exposicionOntology
{
	[ExcludeFromCodeCoverage]
	public class E7_Exhibition_Activity : GnossOCBase
	{

		public E7_Exhibition_Activity() : base() { } 

		public E7_Exhibition_Activity(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Pm_activityArtWork = new List<E7_Activity_E22_Man_Made_Object>();
			SemanticPropertyModel propPm_activityArtWork = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#activityArtWork");
			if(propPm_activityArtWork != null && propPm_activityArtWork.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_activityArtWork.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E7_Activity_E22_Man_Made_Object pm_activityArtWork = new E7_Activity_E22_Man_Made_Object(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_activityArtWork.Add(pm_activityArtWork);
					}
				}
			}
			this.Cidoc_p3_has_note = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p3_has_note"));
			this.Ecidoc_p4_p79_has_time_span_beginning = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p4_p79_has_time-span_beginning"));
			this.Cidoc_p7_took_place_at = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p7_took_place_at"));
			this.Ecidoc_p4_p80_has_time_span_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p4_p80_has_time-span_end"));
		}

		public E7_Exhibition_Activity(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Pm_activityArtWork = new List<E7_Activity_E22_Man_Made_Object>();
			SemanticPropertyModel propPm_activityArtWork = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#activityArtWork");
			if(propPm_activityArtWork != null && propPm_activityArtWork.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_activityArtWork.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E7_Activity_E22_Man_Made_Object pm_activityArtWork = new E7_Activity_E22_Man_Made_Object(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_activityArtWork.Add(pm_activityArtWork);
					}
				}
			}
			this.Cidoc_p3_has_note = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p3_has_note"));
			this.Ecidoc_p4_p79_has_time_span_beginning = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p4_p79_has_time-span_beginning"));
			this.Cidoc_p7_took_place_at = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p7_took_place_at"));
			this.Ecidoc_p4_p80_has_time_span_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p4_p80_has_time-span_end"));
		}

		public virtual string RdfType { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E7_Exhibition_Activity"; } }
		public virtual string RdfsLabel { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E7_Exhibition_Activity"; } }
		[LABEL(LanguageEnum.es,"Obras")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#activityArtWork")]
		public  List<E7_Activity_E22_Man_Made_Object> Pm_activityArtWork { get; set;}

		[LABEL(LanguageEnum.es,"Titulo")]
		[RDFProperty("http://www.cidoc-crm.org/cidoc-crm#p3_has_note")]
		public  string Cidoc_p3_has_note { get; set;}

		[LABEL(LanguageEnum.es,"Fecha de comienzo")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p4_p79_has_time-span_beginning")]
		public  DateTime? Ecidoc_p4_p79_has_time_span_beginning { get; set;}

		[LABEL(LanguageEnum.es,"Lugar")]
		[RDFProperty("http://www.cidoc-crm.org/cidoc-crm#p7_took_place_at")]
		public  string Cidoc_p7_took_place_at { get; set;}

		[LABEL(LanguageEnum.es,"Fecha de fin")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p4_p80_has_time-span_end")]
		public  DateTime? Ecidoc_p4_p80_has_time_span_end { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("cidoc:p3_has_note", this.Cidoc_p3_has_note));
			if (this.Ecidoc_p4_p79_has_time_span_beginning.HasValue){
				propList.Add(new DateOntologyProperty("ecidoc:p4_p79_has_time-span_beginning", this.Ecidoc_p4_p79_has_time_span_beginning.Value));
				}
			propList.Add(new StringOntologyProperty("cidoc:p7_took_place_at", this.Cidoc_p7_took_place_at));
			if (this.Ecidoc_p4_p80_has_time_span_end.HasValue){
				propList.Add(new DateOntologyProperty("ecidoc:p4_p80_has_time-span_end", this.Ecidoc_p4_p80_has_time_span_end.Value));
				}
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Pm_activityArtWork!=null){
				foreach(E7_Activity_E22_Man_Made_Object prop in Pm_activityArtWork){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityE7_Activity_E22_Man_Made_Object = new OntologyEntity("http://museodelprado.es/ontologia/ecidoc.owl#E7_Activity_E22_Man-Made_Object", "http://museodelprado.es/ontologia/ecidoc.owl#E7_Activity_E22_Man-Made_Object", "pm:activityArtWork", prop.propList, prop.entList);
				entList.Add(entityE7_Activity_E22_Man_Made_Object);
				prop.Entity= entityE7_Activity_E22_Man_Made_Object;
				}
			}
		} 
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology=null;
			GetEntities();
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Exhibition_Activity_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://museodelprado.es/ontologia/ecidoc.owl#E7_Exhibition_Activity>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Exhibition_Activity_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://museodelprado.es/ontologia/ecidoc.owl#E7_Exhibition_Activity\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/E7_Exhibition_Activity_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Pm_activityArtWork != null)
			{
			foreach(var item0 in this.Pm_activityArtWork)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Activity_E22_Man-Made_Object_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://museodelprado.es/ontologia/ecidoc.owl#E7_Activity_E22_Man-Made_Object>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Activity_E22_Man-Made_Object_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://museodelprado.es/ontologia/ecidoc.owl#E7_Activity_E22_Man-Made_Object\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/E7_Activity_E22_Man-Made_Object_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Exhibition_Activity_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#activityArtWork", $"<{resourceAPI.GraphsUrl}items/E7_Activity_E22_Man-Made_Object_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Pm_artWorkOrder != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Activity_E22_Man-Made_Object_{ResourceID}_{item0.ArticleID}",  "http://museodelprado.es/ontologia/pradomuseum.owl#artWorkOrder", $"{item0.Pm_artWorkOrder.Value.ToString()}", list, " . ");
				}
			}
			}
				if(this.Cidoc_p3_has_note != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Exhibition_Activity_{ResourceID}_{ArticleID}", "http://www.cidoc-crm.org/cidoc-crm#p3_has_note",  $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p3_has_note)}\"", list, " . ");
				}
				if(this.Ecidoc_p4_p79_has_time_span_beginning != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Exhibition_Activity_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/ecidoc.owl#p4_p79_has_time-span_beginning", $"\"{this.Ecidoc_p4_p79_has_time_span_beginning.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Cidoc_p7_took_place_at != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Exhibition_Activity_{ResourceID}_{ArticleID}",  "http://www.cidoc-crm.org/cidoc-crm#p7_took_place_at", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p7_took_place_at)}\"", list, " . ");
				}
				if(this.Ecidoc_p4_p80_has_time_span_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E7_Exhibition_Activity_{ResourceID}_{ArticleID}",  "http://museodelprado.es/ontologia/ecidoc.owl#p4_p80_has_time-span_end", $"\"{this.Ecidoc_p4_p80_has_time_span_end.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"pma_exposicion\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://museodelprado.es/ontologia/ecidoc.owl#E7_Exhibition_Activity\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p3_has_note)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p3_has_note)}\"", list, " . ");
			string search = string.Empty;
			if(this.Pm_activityArtWork != null)
			{
			foreach(var item0 in this.Pm_activityArtWork)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/pradomuseum.owl#activityArtWork", $"<{resourceAPI.GraphsUrl}items/e7_activity_e22_man-made_object_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Pm_artWorkOrder != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e7_activity_e22_man-made_object_{ResourceID}_{item0.ArticleID}",  "http://museodelprado.es/ontologia/pradomuseum.owl#artWorkOrder", $"{item0.Pm_artWorkOrder.Value.ToString()}", list, " . ");
				}
			}
			}
				if(this.Cidoc_p3_has_note != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.cidoc-crm.org/cidoc-crm#p3_has_note",  $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p3_has_note).ToLower()}\"", list, " . ");
				}
				if(this.Ecidoc_p4_p79_has_time_span_beginning != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/ecidoc.owl#p4_p79_has_time-span_beginning", $"{this.Ecidoc_p4_p79_has_time_span_beginning.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Cidoc_p7_took_place_at != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.cidoc-crm.org/cidoc-crm#p7_took_place_at", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p7_took_place_at).ToLower()}\"", list, " . ");
				}
				if(this.Ecidoc_p4_p80_has_time_span_end != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://museodelprado.es/ontologia/ecidoc.owl#p4_p80_has_time-span_end", $"{this.Ecidoc_p4_p80_has_time_span_end.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
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
			string titulo = $"{this.Cidoc_p3_has_note.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/Pma_exposicionOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Cidoc_p3_has_note;
		}





	}
}
