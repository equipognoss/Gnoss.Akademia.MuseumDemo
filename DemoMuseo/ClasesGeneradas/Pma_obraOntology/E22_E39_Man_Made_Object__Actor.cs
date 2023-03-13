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
using E39_Actor = Pma_autorOntology.E39_Actor;

namespace Pma_obraOntology
{
	[ExcludeFromCodeCoverage]
	public class E22_E39_Man_Made_Object__Actor : GnossOCBase
	{

		public E22_E39_Man_Made_Object__Actor() : base() { } 

		public E22_E39_Man_Made_Object__Actor(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propPm_author = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#author");
			if(propPm_author != null && propPm_author.PropertyValues.Count > 0)
			{
				this.Pm_author = new E39_Actor(propPm_author.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propPm_authorship = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#authorship");
			if(propPm_authorship != null && propPm_authorship.PropertyValues.Count > 0)
			{
				this.Pm_authorship = new E39_Authorship(propPm_authorship.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Ecidoc_order = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#order"));
			this.Ecidoc_p2_has_author_type = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p2_has_author_type"));
		}

		public virtual string RdfType { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E22_E39_Man-Made_Object__Actor"; } }
		public virtual string RdfsLabel { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E22_E39_Man-Made_Object__Actor"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Autor")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#author")]
		public  E39_Actor Pm_author  { get; set;} 
		public string IdPm_author  { get; set;} 

		[LABEL(LanguageEnum.es,"Autor de la obra original")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#authorship")]
		public  E39_Authorship Pm_authorship { get; set;}

		[LABEL(LanguageEnum.es,"Orden")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#order")]
		public  int? Ecidoc_order { get; set;}

		[LABEL(LanguageEnum.es,"Tipo de autoria")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p2_has_author_type")]
		public  string Ecidoc_p2_has_author_type { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("pm:author", this.IdPm_author));
			propList.Add(new StringOntologyProperty("ecidoc:order", this.Ecidoc_order.ToString()));
			propList.Add(new StringOntologyProperty("ecidoc:p2_has_author_type", this.Ecidoc_p2_has_author_type));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Pm_authorship!=null){
				Pm_authorship.GetProperties();
				Pm_authorship.GetEntities();
				OntologyEntity entityPm_authorship = new OntologyEntity("http://museodelprado.es/ontologia/ecidoc.owl#E39_Authorship", "http://museodelprado.es/ontologia/ecidoc.owl#E39_Authorship", "pm:authorship", Pm_authorship.propList, Pm_authorship.entList);
				entList.Add(entityPm_authorship);
			}
		} 











	}
}
