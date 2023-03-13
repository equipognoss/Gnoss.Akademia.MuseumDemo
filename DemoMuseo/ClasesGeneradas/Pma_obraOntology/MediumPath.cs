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

namespace Pma_obraOntology
{
	[ExcludeFromCodeCoverage]
	public class MediumPath : GnossOCBase
	{

		public MediumPath() : base() { } 

		public MediumPath(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Pm_mediumNode = new List<Concept>();
			SemanticPropertyModel propPm_mediumNode = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#mediumNode");
			if(propPm_mediumNode != null && propPm_mediumNode.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_mediumNode.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Concept pm_mediumNode = new Concept(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_mediumNode.Add(pm_mediumNode);
					}
				}
			}
		}

		public virtual string RdfType { get { return "http://museodelprado.es/ontologia/pradomuseum.owl#MediumPath"; } }
		public virtual string RdfsLabel { get { return "http://museodelprado.es/ontologia/pradomuseum.owl#MediumPath"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#mediumNode")]
		public  List<Concept> Pm_mediumNode { get; set;}
		public List<string> IdsPm_mediumNode { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("pm:mediumNode", this.IdsPm_mediumNode));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
