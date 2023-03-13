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

namespace Pma_exposicionOntology
{
	[ExcludeFromCodeCoverage]
	public class E7_Activity_E22_Man_Made_Object : GnossOCBase
	{

		public E7_Activity_E22_Man_Made_Object() : base() { } 

		public E7_Activity_E22_Man_Made_Object(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propPm_relatedInternalArtWork = pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#relatedInternalArtWork");
			if(propPm_relatedInternalArtWork != null && propPm_relatedInternalArtWork.PropertyValues.Count > 0)
			{
				this.Pm_relatedInternalArtWork = new E7_Activity_E22_Man_Made_Object(propPm_relatedInternalArtWork.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Pm_artWorkOrder = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://museodelprado.es/ontologia/pradomuseum.owl#artWorkOrder"));
		}

		public virtual string RdfType { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E7_Activity_E22_Man-Made_Object"; } }
		public virtual string RdfsLabel { get { return "http://museodelprado.es/ontologia/ecidoc.owl#E7_Activity_E22_Man-Made_Object"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Obra")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#relatedInternalArtWork")]
		public  E7_Activity_E22_Man_Made_Object Pm_relatedInternalArtWork { get; set;}

		[LABEL(LanguageEnum.es,"Orden")]
		[RDFProperty("http://museodelprado.es/ontologia/pradomuseum.owl#artWorkOrder")]
		public  int? Pm_artWorkOrder { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("pm:artWorkOrder", this.Pm_artWorkOrder.ToString()));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Pm_relatedInternalArtWork!=null){
				Pm_relatedInternalArtWork.GetProperties();
				Pm_relatedInternalArtWork.GetEntities();
				OntologyEntity entityPm_relatedInternalArtWork = new OntologyEntity("http://museodelprado.es/ontologia/ecidoc.owl#E7_Activity_E22_Man-Made_Object", "http://museodelprado.es/ontologia/ecidoc.owl#E7_Activity_E22_Man-Made_Object", "pm:relatedInternalArtWork", Pm_relatedInternalArtWork.propList, Pm_relatedInternalArtWork.entList);
				entList.Add(entityPm_relatedInternalArtWork);
			}
		} 











	}
}
