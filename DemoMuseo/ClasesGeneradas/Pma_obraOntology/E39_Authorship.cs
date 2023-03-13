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
	public class E39_Authorship : GnossOCBase
	{

		public E39_Authorship() : base() { } 

		public E39_Authorship(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propPm_author = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#author");
			if(propPm_author != null && propPm_author.PropertyValues.Count > 0)
			{
				this.Pm_author = new E39_Actor(propPm_author.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Ecidoc_p2_has_type_authorship = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p2_has_type_authorship"));
		}

		public virtual string RdfType { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E39_Authorship"; } }
		public virtual string RdfsLabel { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E39_Authorship"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Autor")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#author")]
		public  E39_Actor Pm_author  { get; set;} 
		public string IdPm_author  { get; set;} 

		[LABEL(LanguageEnum.es,"Autoria")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p2_has_type_authorship")]
		public  string Ecidoc_p2_has_type_authorship { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("pm:author", this.IdPm_author));
			propList.Add(new StringOntologyProperty("ecidoc:p2_has_type_authorship", this.Ecidoc_p2_has_type_authorship));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
