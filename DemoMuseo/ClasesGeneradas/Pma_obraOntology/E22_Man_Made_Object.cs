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
using C1003_Manifestation = Pma_multimediaOntology.C1003_Manifestation;
using E7_Exhibition_Activity = Pma_exposicionOntology.E7_Exhibition_Activity;

namespace Pma_obraOntology
{
	[ExcludeFromCodeCoverage]
	public class E22_Man_Made_Object : GnossOCBase
	{

		public E22_Man_Made_Object() : base() { } 

		public E22_Man_Made_Object(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Ecidoc_p108i_E12_p126_employed_medium = new List<MediumPath>();
			SemanticPropertyModel propEcidoc_p108i_E12_p126_employed_medium = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p126_employed_medium");
			if(propEcidoc_p108i_E12_p126_employed_medium != null && propEcidoc_p108i_E12_p126_employed_medium.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEcidoc_p108i_E12_p126_employed_medium.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						MediumPath ecidoc_p108i_E12_p126_employed_medium = new MediumPath(propValue.RelatedEntity,idiomaUsuario);
						this.Ecidoc_p108i_E12_p126_employed_medium.Add(ecidoc_p108i_E12_p126_employed_medium);
					}
				}
			}
			this.Cidoc_p14_carried_out_by = new List<E22_E39_Man_Made_Object__Actor>();
			SemanticPropertyModel propCidoc_p14_carried_out_by = pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p14_carried_out_by");
			if(propCidoc_p14_carried_out_by != null && propCidoc_p14_carried_out_by.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propCidoc_p14_carried_out_by.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E22_E39_Man_Made_Object__Actor cidoc_p14_carried_out_by = new E22_E39_Man_Made_Object__Actor(propValue.RelatedEntity,idiomaUsuario);
						this.Cidoc_p14_carried_out_by.Add(cidoc_p14_carried_out_by);
					}
				}
			}
			this.Cidoc_p65_shows_visual_item = new List<E36_Visual_Item>();
			SemanticPropertyModel propCidoc_p65_shows_visual_item = pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p65_shows_visual_item");
			if(propCidoc_p65_shows_visual_item != null && propCidoc_p65_shows_visual_item.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propCidoc_p65_shows_visual_item.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E36_Visual_Item cidoc_p65_shows_visual_item = new E36_Visual_Item(propValue.RelatedEntity,idiomaUsuario);
						this.Cidoc_p65_shows_visual_item.Add(cidoc_p65_shows_visual_item);
					}
				}
			}
			this.Cidoc_p70i_is_documented_in = new List<C1003_Manifestation>();
			SemanticPropertyModel propCidoc_p70i_is_documented_in = pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p70i_is_documented_in");
			if(propCidoc_p70i_is_documented_in != null && propCidoc_p70i_is_documented_in.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propCidoc_p70i_is_documented_in.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						C1003_Manifestation cidoc_p70i_is_documented_in = new C1003_Manifestation(propValue.RelatedEntity,idiomaUsuario);
						this.Cidoc_p70i_is_documented_in.Add(cidoc_p70i_is_documented_in);
					}
				}
			}
			this.Ecidoc_p108i_E12_p32_used_general_technique = new List<TechniquePath>();
			SemanticPropertyModel propEcidoc_p108i_E12_p32_used_general_technique = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p32_used_general_technique");
			if(propEcidoc_p108i_E12_p32_used_general_technique != null && propEcidoc_p108i_E12_p32_used_general_technique.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEcidoc_p108i_E12_p32_used_general_technique.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						TechniquePath ecidoc_p108i_E12_p32_used_general_technique = new TechniquePath(propValue.RelatedEntity,idiomaUsuario);
						this.Ecidoc_p108i_E12_p32_used_general_technique.Add(ecidoc_p108i_E12_p32_used_general_technique);
					}
				}
			}
			this.Cidoc_p130i_features_are_also_found_on = new List<E22_Related_Man_Made_Object>();
			SemanticPropertyModel propCidoc_p130i_features_are_also_found_on = pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p130i_features_are_also_found_on");
			if(propCidoc_p130i_features_are_also_found_on != null && propCidoc_p130i_features_are_also_found_on.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propCidoc_p130i_features_are_also_found_on.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E22_Related_Man_Made_Object cidoc_p130i_features_are_also_found_on = new E22_Related_Man_Made_Object(propValue.RelatedEntity,idiomaUsuario);
						this.Cidoc_p130i_features_are_also_found_on.Add(cidoc_p130i_features_are_also_found_on);
					}
				}
			}
			this.Pm_i_artwork_multimediaresource = new List<C1003_Manifestation>();
			SemanticPropertyModel propPm_i_artwork_multimediaresource = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_multimediaresource");
			if(propPm_i_artwork_multimediaresource != null && propPm_i_artwork_multimediaresource.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_i_artwork_multimediaresource.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						C1003_Manifestation pm_i_artwork_multimediaresource = new C1003_Manifestation(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_i_artwork_multimediaresource.Add(pm_i_artwork_multimediaresource);
					}
				}
			}
			this.Pm_i_artwork_exhibitionactivity = new List<E7_Exhibition_Activity>();
			SemanticPropertyModel propPm_i_artwork_exhibitionactivity = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_exhibitionactivity");
			if(propPm_i_artwork_exhibitionactivity != null && propPm_i_artwork_exhibitionactivity.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_i_artwork_exhibitionactivity.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E7_Exhibition_Activity pm_i_artwork_exhibitionactivity = new E7_Exhibition_Activity(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_i_artwork_exhibitionactivity.Add(pm_i_artwork_exhibitionactivity);
					}
				}
			}
			this.Cidoc_p3_has_note = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p3_has_note"));
			this.Cidoc_p1_is_identified_by = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p1_is_identified_by"));
			this.Ecidoc_p102_E35_p3_has_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p102_E35_p3_has_title"));
		}

		public E22_Man_Made_Object(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Ecidoc_p108i_E12_p126_employed_medium = new List<MediumPath>();
			SemanticPropertyModel propEcidoc_p108i_E12_p126_employed_medium = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p126_employed_medium");
			if(propEcidoc_p108i_E12_p126_employed_medium != null && propEcidoc_p108i_E12_p126_employed_medium.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEcidoc_p108i_E12_p126_employed_medium.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						MediumPath ecidoc_p108i_E12_p126_employed_medium = new MediumPath(propValue.RelatedEntity,idiomaUsuario);
						this.Ecidoc_p108i_E12_p126_employed_medium.Add(ecidoc_p108i_E12_p126_employed_medium);
					}
				}
			}
			this.Cidoc_p14_carried_out_by = new List<E22_E39_Man_Made_Object__Actor>();
			SemanticPropertyModel propCidoc_p14_carried_out_by = pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p14_carried_out_by");
			if(propCidoc_p14_carried_out_by != null && propCidoc_p14_carried_out_by.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propCidoc_p14_carried_out_by.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E22_E39_Man_Made_Object__Actor cidoc_p14_carried_out_by = new E22_E39_Man_Made_Object__Actor(propValue.RelatedEntity,idiomaUsuario);
						this.Cidoc_p14_carried_out_by.Add(cidoc_p14_carried_out_by);
					}
				}
			}
			this.Cidoc_p65_shows_visual_item = new List<E36_Visual_Item>();
			SemanticPropertyModel propCidoc_p65_shows_visual_item = pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p65_shows_visual_item");
			if(propCidoc_p65_shows_visual_item != null && propCidoc_p65_shows_visual_item.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propCidoc_p65_shows_visual_item.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E36_Visual_Item cidoc_p65_shows_visual_item = new E36_Visual_Item(propValue.RelatedEntity,idiomaUsuario);
						this.Cidoc_p65_shows_visual_item.Add(cidoc_p65_shows_visual_item);
					}
				}
			}
			this.Cidoc_p70i_is_documented_in = new List<C1003_Manifestation>();
			SemanticPropertyModel propCidoc_p70i_is_documented_in = pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p70i_is_documented_in");
			if(propCidoc_p70i_is_documented_in != null && propCidoc_p70i_is_documented_in.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propCidoc_p70i_is_documented_in.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						C1003_Manifestation cidoc_p70i_is_documented_in = new C1003_Manifestation(propValue.RelatedEntity,idiomaUsuario);
						this.Cidoc_p70i_is_documented_in.Add(cidoc_p70i_is_documented_in);
					}
				}
			}
			this.Ecidoc_p108i_E12_p32_used_general_technique = new List<TechniquePath>();
			SemanticPropertyModel propEcidoc_p108i_E12_p32_used_general_technique = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p32_used_general_technique");
			if(propEcidoc_p108i_E12_p32_used_general_technique != null && propEcidoc_p108i_E12_p32_used_general_technique.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEcidoc_p108i_E12_p32_used_general_technique.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						TechniquePath ecidoc_p108i_E12_p32_used_general_technique = new TechniquePath(propValue.RelatedEntity,idiomaUsuario);
						this.Ecidoc_p108i_E12_p32_used_general_technique.Add(ecidoc_p108i_E12_p32_used_general_technique);
					}
				}
			}
			this.Cidoc_p130i_features_are_also_found_on = new List<E22_Related_Man_Made_Object>();
			SemanticPropertyModel propCidoc_p130i_features_are_also_found_on = pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p130i_features_are_also_found_on");
			if(propCidoc_p130i_features_are_also_found_on != null && propCidoc_p130i_features_are_also_found_on.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propCidoc_p130i_features_are_also_found_on.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E22_Related_Man_Made_Object cidoc_p130i_features_are_also_found_on = new E22_Related_Man_Made_Object(propValue.RelatedEntity,idiomaUsuario);
						this.Cidoc_p130i_features_are_also_found_on.Add(cidoc_p130i_features_are_also_found_on);
					}
				}
			}
			this.Pm_i_artwork_multimediaresource = new List<C1003_Manifestation>();
			SemanticPropertyModel propPm_i_artwork_multimediaresource = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_multimediaresource");
			if(propPm_i_artwork_multimediaresource != null && propPm_i_artwork_multimediaresource.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_i_artwork_multimediaresource.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						C1003_Manifestation pm_i_artwork_multimediaresource = new C1003_Manifestation(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_i_artwork_multimediaresource.Add(pm_i_artwork_multimediaresource);
					}
				}
			}
			this.Pm_i_artwork_exhibitionactivity = new List<E7_Exhibition_Activity>();
			SemanticPropertyModel propPm_i_artwork_exhibitionactivity = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_exhibitionactivity");
			if(propPm_i_artwork_exhibitionactivity != null && propPm_i_artwork_exhibitionactivity.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_i_artwork_exhibitionactivity.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						E7_Exhibition_Activity pm_i_artwork_exhibitionactivity = new E7_Exhibition_Activity(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_i_artwork_exhibitionactivity.Add(pm_i_artwork_exhibitionactivity);
					}
				}
			}
			this.Cidoc_p3_has_note = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p3_has_note"));
			this.Cidoc_p1_is_identified_by = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.cidoc-crm.org/cidoc-crm#p1_is_identified_by"));
			this.Ecidoc_p102_E35_p3_has_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p102_E35_p3_has_title"));
		}

		public virtual string RdfType { get { return "http://www.cidoc-crm.org/cidoc-crm#E22_Man_Made_Object"; } }
		public virtual string RdfsLabel { get { return "http://www.cidoc-crm.org/cidoc-crm#E22_Man_Made_Object"; } }
		[LABEL(LanguageEnum.es,"Material")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p126_employed_medium")]
		public  List<MediumPath> Ecidoc_p108i_E12_p126_employed_medium { get; set;}

		[LABEL(LanguageEnum.es,"Autor")]
		[RDFProperty("http://www.cidoc-crm.org/cidoc-crm#p14_carried_out_by")]
		public  List<E22_E39_Man_Made_Object__Actor> Cidoc_p14_carried_out_by { get; set;}

		[LABEL(LanguageEnum.es,"Imagen")]
		[RDFProperty("http://www.cidoc-crm.org/cidoc-crm#p65_shows_visual_item")]
		public  List<E36_Visual_Item> Cidoc_p65_shows_visual_item { get; set;}

		[LABEL(LanguageEnum.es,"Documentos")]
		[RDFProperty("http://www.cidoc-crm.org/cidoc-crm#p70i_is_documented_in")]
		public  List<C1003_Manifestation> Cidoc_p70i_is_documented_in { get; set;}

		[LABEL(LanguageEnum.es,"Tecnica")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p32_used_general_technique")]
		public  List<TechniquePath> Ecidoc_p108i_E12_p32_used_general_technique { get; set;}

		[LABEL(LanguageEnum.es,"http://www.cidoc-crm.org/cidoc-crm#p130i_features_are_also_found_on")]
		[RDFProperty("http://www.cidoc-crm.org/cidoc-crm#p130i_features_are_also_found_on")]
		public  List<E22_Related_Man_Made_Object> Cidoc_p130i_features_are_also_found_on { get; set;}

		[LABEL(LanguageEnum.es,"Recurso Multimedia")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_multimediaresource")]
		public  List<C1003_Manifestation> Pm_i_artwork_multimediaresource { get; set;}
		public List<string> IdsPm_i_artwork_multimediaresource { get; set;}

		[LABEL(LanguageEnum.es,"Exposiciones")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_exhibitionactivity")]
		public  List<E7_Exhibition_Activity> Pm_i_artwork_exhibitionactivity { get; set;}
		public List<string> IdsPm_i_artwork_exhibitionactivity { get; set;}

		[LABEL(LanguageEnum.es,"Reseña")]
		[RDFProperty("http://www.cidoc-crm.org/cidoc-crm#p3_has_note")]
		public  string Cidoc_p3_has_note { get; set;}

		[LABEL(LanguageEnum.es,"Identificador")]
		[RDFProperty("http://www.cidoc-crm.org/cidoc-crm#p1_is_identified_by")]
		public  string Cidoc_p1_is_identified_by { get; set;}

		[LABEL(LanguageEnum.es,"Titulo")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p102_E35_p3_has_title")]
		public  string Ecidoc_p102_E35_p3_has_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("pm:i_artwork_multimediaresource", this.IdsPm_i_artwork_multimediaresource));
			propList.Add(new ListStringOntologyProperty("pm:i_artwork_exhibitionactivity", this.IdsPm_i_artwork_exhibitionactivity));
			propList.Add(new StringOntologyProperty("cidoc:p3_has_note", this.Cidoc_p3_has_note));
			propList.Add(new StringOntologyProperty("cidoc:p1_is_identified_by", this.Cidoc_p1_is_identified_by));
			propList.Add(new StringOntologyProperty("ecidoc:p102_E35_p3_has_title", this.Ecidoc_p102_E35_p3_has_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Ecidoc_p108i_E12_p126_employed_medium!=null){
				foreach(MediumPath prop in Ecidoc_p108i_E12_p126_employed_medium){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityMediumPath = new OntologyEntity("http://museodelprado.es/ontologia/pradomuseum.owl#MediumPath", "http://museodelprado.es/ontologia/pradomuseum.owl#MediumPath", "ecidoc:p108i_E12_p126_employed_medium", prop.propList, prop.entList);
				entList.Add(entityMediumPath);
				prop.Entity= entityMediumPath;
				}
			}
			if(Cidoc_p14_carried_out_by!=null){
				foreach(E22_E39_Man_Made_Object__Actor prop in Cidoc_p14_carried_out_by){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityE22_E39_Man_Made_Object__Actor = new OntologyEntity("http://museodelprado.es/ontologia/ecidoc.owl#E22_E39_Man-Made_Object__Actor", "http://museodelprado.es/ontologia/ecidoc.owl#E22_E39_Man-Made_Object__Actor", "cidoc:p14_carried_out_by", prop.propList, prop.entList);
				entList.Add(entityE22_E39_Man_Made_Object__Actor);
				prop.Entity= entityE22_E39_Man_Made_Object__Actor;
				}
			}
			if(Cidoc_p65_shows_visual_item!=null){
				foreach(E36_Visual_Item prop in Cidoc_p65_shows_visual_item){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityE36_Visual_Item = new OntologyEntity("http://www.cidoc-crm.org/cidoc-crm#E36_Visual_Item", "http://www.cidoc-crm.org/cidoc-crm#E36_Visual_Item", "cidoc:p65_shows_visual_item", prop.propList, prop.entList);
				entList.Add(entityE36_Visual_Item);
				prop.Entity= entityE36_Visual_Item;
				}
			}
			if(Cidoc_p70i_is_documented_in!=null){
				foreach(C1003_Manifestation prop in Cidoc_p70i_is_documented_in){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityC1003_Manifestation = new OntologyEntity("http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation", "http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation", "cidoc:p70i_is_documented_in", prop.propList, prop.entList);
				entList.Add(entityC1003_Manifestation);
				prop.Entity= entityC1003_Manifestation;
				}
			}
			if(Ecidoc_p108i_E12_p32_used_general_technique!=null){
				foreach(TechniquePath prop in Ecidoc_p108i_E12_p32_used_general_technique){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityTechniquePath = new OntologyEntity("http://museodelprado.es/ontologia/pradomuseum.owl#TechniquePath", "http://museodelprado.es/ontologia/pradomuseum.owl#TechniquePath", "ecidoc:p108i_E12_p32_used_general_technique", prop.propList, prop.entList);
				entList.Add(entityTechniquePath);
				prop.Entity= entityTechniquePath;
				}
			}
			if(Cidoc_p130i_features_are_also_found_on!=null){
				foreach(E22_Related_Man_Made_Object prop in Cidoc_p130i_features_are_also_found_on){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityE22_Related_Man_Made_Object = new OntologyEntity("http://museodelprado.es/ontologia/ecidoc.owl#E22_Related_Man-Made_Object", "http://museodelprado.es/ontologia/ecidoc.owl#E22_Related_Man-Made_Object", "cidoc:p130i_features_are_also_found_on", prop.propList, prop.entList);
				entList.Add(entityE22_Related_Man_Made_Object);
				prop.Entity= entityE22_Related_Man_Made_Object;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://www.cidoc-crm.org/cidoc-crm#E22_Man_Made_Object>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://www.cidoc-crm.org/cidoc-crm#E22_Man_Made_Object\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Ecidoc_p108i_E12_p126_employed_medium != null)
			{
			foreach(var item0 in this.Ecidoc_p108i_E12_p126_employed_medium)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MediumPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://museodelprado.es/ontologia/pradomuseum.owl#MediumPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MediumPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://museodelprado.es/ontologia/pradomuseum.owl#MediumPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/MediumPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p126_employed_medium", $"<{resourceAPI.GraphsUrl}items/MediumPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsPm_mediumNode != null)
				{
					foreach(var item2 in item0.IdsPm_mediumNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MediumPath_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#mediumNode", $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Cidoc_p14_carried_out_by != null)
			{
			foreach(var item0 in this.Cidoc_p14_carried_out_by)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_E39_Man-Made_Object__Actor_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://museodelprado.es/ontologia/ecidoc.owl#E22_E39_Man-Made_Object__Actor>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_E39_Man-Made_Object__Actor_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://museodelprado.es/ontologia/ecidoc.owl#E22_E39_Man-Made_Object__Actor\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/E22_E39_Man-Made_Object__Actor_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://www.cidoc-crm.org/cidoc-crm#p14_carried_out_by", $"<{resourceAPI.GraphsUrl}items/E22_E39_Man-Made_Object__Actor_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Pm_authorship != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Authorship_{ResourceID}_{item0.Pm_authorship.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://museodelprado.es/ontologia/ecidoc.owl#E39_Authorship>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Authorship_{ResourceID}_{item0.Pm_authorship.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://museodelprado.es/ontologia/ecidoc.owl#E39_Authorship\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/E39_Authorship_{ResourceID}_{item0.Pm_authorship.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_E39_Man-Made_Object__Actor_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#authorship", $"<{resourceAPI.GraphsUrl}items/E39_Authorship_{ResourceID}_{item0.Pm_authorship.ArticleID}>", list, " . ");
				if(item0.Pm_authorship.IdPm_author != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Authorship_{ResourceID}_{item0.Pm_authorship.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#author",  $"<{item0.Pm_authorship.IdPm_author}>", list, " . ");
				}
				if(item0.Pm_authorship.Ecidoc_p2_has_type_authorship != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E39_Authorship_{ResourceID}_{item0.Pm_authorship.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p2_has_type_authorship",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Pm_authorship.Ecidoc_p2_has_type_authorship)}\"", list, " . ");
				}
			}
				if(item0.IdPm_author != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_E39_Man-Made_Object__Actor_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#author",  $"<{item0.IdPm_author}>", list, " . ");
				}
				if(item0.Ecidoc_order != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_E39_Man-Made_Object__Actor_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#order",  $"{item0.Ecidoc_order.Value.ToString()}", list, " . ");
				}
				if(item0.Ecidoc_p2_has_author_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_E39_Man-Made_Object__Actor_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p2_has_author_type",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Ecidoc_p2_has_author_type)}\"", list, " . ");
				}
			}
			}
			if(this.Cidoc_p65_shows_visual_item != null)
			{
			foreach(var item0 in this.Cidoc_p65_shows_visual_item)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://www.cidoc-crm.org/cidoc-crm#E36_Visual_Item>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://www.cidoc-crm.org/cidoc-crm#E36_Visual_Item\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://www.cidoc-crm.org/cidoc-crm#p65_shows_visual_item", $"<{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Ecidoc_p1_is_identified_by != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p1_is_identified_by",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Ecidoc_p1_is_identified_by)}\"", list, " . ");
				}
				if(item0.Ecidoc_p2_has_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p2_has_type",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Ecidoc_p2_has_type)}\"", list, " . ");
				}
				if(item0.Pm_imageHeight != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#imageHeight",  $"{item0.Pm_imageHeight.Value.ToString()}", list, " . ");
				}
				if(item0.Pm_isMain != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#isMain",  $"\"{item0.Pm_isMain.ToString()}\"", list, " . ");
				}
				if(item0.Pm_imageWidth != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#imageWidth",  $"{item0.Pm_imageWidth.Value.ToString()}", list, " . ");
				}
				if(item0.Ecidoc_p3_has_note != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E36_Visual_Item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p3_has_note",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Ecidoc_p3_has_note)}\"", list, " . ");
				}
			}
			}
			if(this.Cidoc_p70i_is_documented_in != null)
			{
			foreach(var item0 in this.Cidoc_p70i_is_documented_in)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://www.cidoc-crm.org/cidoc-crm#p70i_is_documented_in", $"<{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Efrbrer_P3055_has_date_of_publication_or_distribution != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{item0.ArticleID}",  "http://museodelprado.es/ontologia/efrbrer.owl#P3055_has_date_of_publication_or_distribution", $"\"{GenerarTextoSinSaltoDeLinea(item0.Efrbrer_P3055_has_date_of_publication_or_distribution)}\"", list, " . ");
				}
				if(item0.Efrbrer_p3020_has_title_of_the_manifestation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/C1003_Manifestation_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Efrbrer_p3020_has_title_of_the_manifestation)}\"", list, " . ");
				}
			}
			}
			if(this.Ecidoc_p108i_E12_p32_used_general_technique != null)
			{
			foreach(var item0 in this.Ecidoc_p108i_E12_p32_used_general_technique)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechniquePath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://museodelprado.es/ontologia/pradomuseum.owl#TechniquePath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechniquePath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://museodelprado.es/ontologia/pradomuseum.owl#TechniquePath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/TechniquePath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p32_used_general_technique", $"<{resourceAPI.GraphsUrl}items/TechniquePath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsPm_techniqueNode != null)
				{
					foreach(var item2 in item0.IdsPm_techniqueNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechniquePath_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#techniqueNode", $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Cidoc_p130i_features_are_also_found_on != null)
			{
			foreach(var item0 in this.Cidoc_p130i_features_are_also_found_on)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Related_Man-Made_Object_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://museodelprado.es/ontologia/ecidoc.owl#E22_Related_Man-Made_Object>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Related_Man-Made_Object_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://museodelprado.es/ontologia/ecidoc.owl#E22_Related_Man-Made_Object\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/E22_Related_Man-Made_Object_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://www.cidoc-crm.org/cidoc-crm#p130i_features_are_also_found_on", $"<{resourceAPI.GraphsUrl}items/E22_Related_Man-Made_Object_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdPm_art_work != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Related_Man-Made_Object_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#art_work",  $"<{item0.IdPm_art_work}>", list, " . ");
				}
				if(item0.Pm_relation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Related_Man-Made_Object_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#relation",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Pm_relation)}\"", list, " . ");
				}
			}
			}
				if(this.IdsPm_i_artwork_multimediaresource != null)
				{
					foreach(var item2 in this.IdsPm_i_artwork_multimediaresource)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_multimediaresource", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsPm_i_artwork_exhibitionactivity != null)
				{
					foreach(var item2 in this.IdsPm_i_artwork_exhibitionactivity)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_exhibitionactivity", $"<{item2}>", list, " . ");
					}
				}
				if(this.Cidoc_p3_has_note != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}",  "http://www.cidoc-crm.org/cidoc-crm#p3_has_note", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p3_has_note)}\"", list, " . ");
				}
				if(this.Cidoc_p1_is_identified_by != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://www.cidoc-crm.org/cidoc-crm#p1_is_identified_by",  $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p1_is_identified_by)}\"", list, " . ");
				}
				if(this.Ecidoc_p102_E35_p3_has_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/E22_Man_Made_Object_{ResourceID}_{ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p102_E35_p3_has_title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p102_E35_p3_has_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"pma_obra\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://www.cidoc-crm.org/cidoc-crm#E22_Man_Made_Object\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p1_is_identified_by)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p1_is_identified_by)}\"", list, " . ");
			string search = string.Empty;
			if(this.Ecidoc_p108i_E12_p126_employed_medium != null)
			{
			foreach(var item0 in this.Ecidoc_p108i_E12_p126_employed_medium)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p126_employed_medium", $"<{resourceAPI.GraphsUrl}items/mediumpath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsPm_mediumNode != null)
				{
					foreach(var item2 in item0.IdsPm_mediumNode)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/mediumpath_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#mediumNode", $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Cidoc_p14_carried_out_by != null)
			{
			foreach(var item0 in this.Cidoc_p14_carried_out_by)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.cidoc-crm.org/cidoc-crm#p14_carried_out_by", $"<{resourceAPI.GraphsUrl}items/e22_e39_man-made_object__actor_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Pm_authorship != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e22_e39_man-made_object__actor_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#authorship", $"<{resourceAPI.GraphsUrl}items/e39_authorship_{ResourceID}_{item0.Pm_authorship.ArticleID}>", list, " . ");
				if(item0.Pm_authorship.IdPm_author != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.Pm_authorship.IdPm_author;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e39_authorship_{ResourceID}_{item0.Pm_authorship.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#author",  $"<{itemRegex}>", list, " . ");
				}
				if(item0.Pm_authorship.Ecidoc_p2_has_type_authorship != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e39_authorship_{ResourceID}_{item0.Pm_authorship.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p2_has_type_authorship",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Pm_authorship.Ecidoc_p2_has_type_authorship).ToLower()}\"", list, " . ");
				}
			}
				if(item0.IdPm_author != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdPm_author;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e22_e39_man-made_object__actor_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#author",  $"<{itemRegex}>", list, " . ");
				}
				if(item0.Ecidoc_order != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e22_e39_man-made_object__actor_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#order",  $"{item0.Ecidoc_order.Value.ToString()}", list, " . ");
				}
				if(item0.Ecidoc_p2_has_author_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e22_e39_man-made_object__actor_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p2_has_author_type",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Ecidoc_p2_has_author_type).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Cidoc_p65_shows_visual_item != null)
			{
			foreach(var item0 in this.Cidoc_p65_shows_visual_item)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.cidoc-crm.org/cidoc-crm#p65_shows_visual_item", $"<{resourceAPI.GraphsUrl}items/e36_visual_item_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Ecidoc_p1_is_identified_by != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e36_visual_item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p1_is_identified_by",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Ecidoc_p1_is_identified_by).ToLower()}\"", list, " . ");
				}
				if(item0.Ecidoc_p2_has_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e36_visual_item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p2_has_type",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Ecidoc_p2_has_type).ToLower()}\"", list, " . ");
				}
				if(item0.Pm_imageHeight != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e36_visual_item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#imageHeight",  $"{item0.Pm_imageHeight.Value.ToString()}", list, " . ");
				}
				if(item0.Pm_isMain != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e36_visual_item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#isMain",  $"\"{item0.Pm_isMain.ToString().ToLower()}\"", list, " . ");
				}
				if(item0.Pm_imageWidth != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e36_visual_item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#imageWidth",  $"{item0.Pm_imageWidth.Value.ToString()}", list, " . ");
				}
				if(item0.Ecidoc_p3_has_note != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e36_visual_item_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/ecidoc.owl#p3_has_note",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Ecidoc_p3_has_note).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Cidoc_p70i_is_documented_in != null)
			{
			foreach(var item0 in this.Cidoc_p70i_is_documented_in)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.cidoc-crm.org/cidoc-crm#p70i_is_documented_in", $"<{resourceAPI.GraphsUrl}items/c1003_manifestation_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Efrbrer_P3055_has_date_of_publication_or_distribution != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/c1003_manifestation_{ResourceID}_{item0.ArticleID}",  "http://museodelprado.es/ontologia/efrbrer.owl#P3055_has_date_of_publication_or_distribution", $"\"{GenerarTextoSinSaltoDeLinea(item0.Efrbrer_P3055_has_date_of_publication_or_distribution).ToLower()}\"", list, " . ");
				}
				if(item0.Efrbrer_p3020_has_title_of_the_manifestation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/c1003_manifestation_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Efrbrer_p3020_has_title_of_the_manifestation).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Ecidoc_p108i_E12_p32_used_general_technique != null)
			{
			foreach(var item0 in this.Ecidoc_p108i_E12_p32_used_general_technique)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/ecidoc.owl#p108i_E12_p32_used_general_technique", $"<{resourceAPI.GraphsUrl}items/techniquepath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsPm_techniqueNode != null)
				{
					foreach(var item2 in item0.IdsPm_techniqueNode)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/techniquepath_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#techniqueNode", $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Cidoc_p130i_features_are_also_found_on != null)
			{
			foreach(var item0 in this.Cidoc_p130i_features_are_also_found_on)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.cidoc-crm.org/cidoc-crm#p130i_features_are_also_found_on", $"<{resourceAPI.GraphsUrl}items/e22_related_man-made_object_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdPm_art_work != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdPm_art_work;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e22_related_man-made_object_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#art_work",  $"<{itemRegex}>", list, " . ");
				}
				if(item0.Pm_relation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/e22_related_man-made_object_{ResourceID}_{item0.ArticleID}", "http://museodelprado.es/ontologia/pradomuseum.owl#relation",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Pm_relation).ToLower()}\"", list, " . ");
				}
			}
			}
				if(this.IdsPm_i_artwork_multimediaresource != null)
				{
					foreach(var item2 in this.IdsPm_i_artwork_multimediaresource)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_multimediaresource", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsPm_i_artwork_exhibitionactivity != null)
				{
					foreach(var item2 in this.IdsPm_i_artwork_exhibitionactivity)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/pradomuseum.owl#i_artwork_exhibitionactivity", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Cidoc_p3_has_note != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.cidoc-crm.org/cidoc-crm#p3_has_note", $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p3_has_note).ToLower()}\"", list, " . ");
				}
				if(this.Cidoc_p1_is_identified_by != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.cidoc-crm.org/cidoc-crm#p1_is_identified_by",  $"\"{GenerarTextoSinSaltoDeLinea(this.Cidoc_p1_is_identified_by).ToLower()}\"", list, " . ");
				}
				if(this.Ecidoc_p102_E35_p3_has_title != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://museodelprado.es/ontologia/ecidoc.owl#p102_E35_p3_has_title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Ecidoc_p102_E35_p3_has_title).ToLower()}\"", list, " . ");
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
			string titulo = $"{this.Cidoc_p1_is_identified_by.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/Pma_obraOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Cidoc_p1_is_identified_by;
		}





	}
}
