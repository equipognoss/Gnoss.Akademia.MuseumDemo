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
	public class C1003_Manifestation : GnossOCBase
	{

		public C1003_Manifestation() : base() { } 

		public C1003_Manifestation(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Efrbrer_P3055_has_date_of_publication_or_distribution = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/efrbrer.owl#P3055_has_date_of_publication_or_distribution"));
			this.Efrbrer_p3020_has_title_of_the_manifestation = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation"));
		}

		public virtual string RdfType { get { return "http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation"; } }
		public virtual string RdfsLabel { get { return "http://museodelprado.es/ontologia/efrbrer.owl#C1003_Manifestation"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Fecha de publicacion")]
		[RDFProperty("http://museodelprado.es/ontologia/efrbrer.owl#P3055_has_date_of_publication_or_distribution")]
		public  string Efrbrer_P3055_has_date_of_publication_or_distribution { get; set;}

		[LABEL(LanguageEnum.es,"Titulo del documento")]
		[RDFProperty("http://museodelprado.es/ontologia/efrbrer.owl#p3020_has_title_of_the_manifestation")]
		public  string Efrbrer_p3020_has_title_of_the_manifestation { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("efrbrer:P3055_has_date_of_publication_or_distribution", this.Efrbrer_P3055_has_date_of_publication_or_distribution));
			propList.Add(new StringOntologyProperty("efrbrer:p3020_has_title_of_the_manifestation", this.Efrbrer_p3020_has_title_of_the_manifestation));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
