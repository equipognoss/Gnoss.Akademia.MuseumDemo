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
	public class TechniquePath : GnossOCBase
	{

		public TechniquePath() : base() { } 

		public TechniquePath(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Pm_techniqueNode = new List<Concept>();
			SemanticPropertyModel propPm_techniqueNode = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#techniqueNode");
			if(propPm_techniqueNode != null && propPm_techniqueNode.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propPm_techniqueNode.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Concept pm_techniqueNode = new Concept(propValue.RelatedEntity,idiomaUsuario);
						this.Pm_techniqueNode.Add(pm_techniqueNode);
					}
				}
			}
		}

		public virtual string RdfType { get { return "http://museodelprado.es/ontologia/pradomuseum.owl#TechniquePath"; } }
		public virtual string RdfsLabel { get { return "http://museodelprado.es/ontologia/pradomuseum.owl#TechniquePath"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#techniqueNode")]
		public  List<Concept> Pm_techniqueNode { get; set;}
		public List<string> IdsPm_techniqueNode { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("pm:techniqueNode", this.IdsPm_techniqueNode));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
