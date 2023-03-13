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

namespace Pma_obraOntology
{
	[ExcludeFromCodeCoverage]
	public class E36_Visual_Item : GnossOCBase
	{

		public E36_Visual_Item() : base() { } 

		public E36_Visual_Item(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Ecidoc_p1_is_identified_by = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p1_is_identified_by"));
			this.Ecidoc_p2_has_type = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p2_has_type"));
			this.Pm_imageHeight = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#imageHeight"));
			this.Pm_isMain= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#isMain"));
			this.Pm_imageWidth = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#imageWidth"));
			this.Ecidoc_p3_has_note = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/ecidoc.owl#p3_has_note"));
		}

		public virtual string RdfType { get { return "http://www.cidoc-crm.org/cidoc-crm#E36_Visual_Item"; } }
		public virtual string RdfsLabel { get { return "http://www.cidoc-crm.org/cidoc-crm#E36_Visual_Item"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Identificador")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p1_is_identified_by")]
		public  string Ecidoc_p1_is_identified_by { get; set;}

		[LABEL(LanguageEnum.es,"Tipo")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p2_has_type")]
		public  string Ecidoc_p2_has_type { get; set;}

		[LABEL(LanguageEnum.es,"Alto")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#imageHeight")]
		public  int? Pm_imageHeight { get; set;}

		[LABEL(LanguageEnum.es,"esPrincipal")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#isMain")]
		public  bool Pm_isMain { get; set;}

		[LABEL(LanguageEnum.es,"Ancho")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#imageWidth")]
		public  int? Pm_imageWidth { get; set;}

		[LABEL(LanguageEnum.es,"Url")]
		[RDFProperty("http://museodelprado.es/ontologia/ecidoc.owl#p3_has_note")]
		public  string Ecidoc_p3_has_note { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("ecidoc:p1_is_identified_by", this.Ecidoc_p1_is_identified_by));
			propList.Add(new StringOntologyProperty("ecidoc:p2_has_type", this.Ecidoc_p2_has_type));
			propList.Add(new StringOntologyProperty("pm:imageHeight", this.Pm_imageHeight.ToString()));
			propList.Add(new BoolOntologyProperty("pm:isMain", this.Pm_isMain));
			propList.Add(new StringOntologyProperty("pm:imageWidth", this.Pm_imageWidth.ToString()));
			propList.Add(new StringOntologyProperty("ecidoc:p3_has_note", this.Ecidoc_p3_has_note));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
