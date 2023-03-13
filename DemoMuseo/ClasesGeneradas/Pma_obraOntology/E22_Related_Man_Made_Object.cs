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

namespace Pma_obraOntology
{
	[ExcludeFromCodeCoverage]
	public class E22_Related_Man_Made_Object : GnossOCBase
	{

		public E22_Related_Man_Made_Object() : base() { } 

		public E22_Related_Man_Made_Object(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propPm_art_work = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#art_work");
			if(propPm_art_work != null && propPm_art_work.PropertyValues.Count > 0)
			{
				this.Pm_art_work = new E22_Man_Made_Object(propPm_art_work.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Pm_relation = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#relation"));
		}

		public virtual string RdfType { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E22_Related_Man-Made_Object"; } }
		public virtual string RdfsLabel { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E22_Related_Man-Made_Object"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Obra relacionada")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#art_work")]
		public  E22_Man_Made_Object Pm_art_work  { get; set;} 
		public string IdPm_art_work  { get; set;} 

		[LABEL(LanguageEnum.es,"Relacion")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#relation")]
		public  string Pm_relation { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("pm:art_work", this.IdPm_art_work));
			propList.Add(new StringOntologyProperty("pm:relation", this.Pm_relation));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
