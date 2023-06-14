using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

			//// import all concrete model types (DMSType enum)

			ImportSeason();
			ImportDayType();
			ImportRegulationControl();
			ImportRegulationSchedule();
			ImportRegularTimePoint();
			ImportFuse();
			ImportDisconnector();
			//ImportBaseVoltages();
			//ImportLocations();
			//ImportPowerTransformers();
			//ImportTransformerWindings();
			//ImportWindingTests();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

		#region Import
		private void ImportSeason()
		{
			SortedDictionary<string, object> cimSeasons = concreteModel.GetAllObjectsOfType("FTN.Season");
			if (cimSeasons != null)
			{
				foreach (KeyValuePair<string, object> cimSeasonPair in cimSeasons)
				{
					FTN.Season cimSeason = (FTN.Season)cimSeasonPair.Value;

					ResourceDescription rd = CreateSeasonResourceDescription(cimSeason);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Season ID = ").Append(cimSeason.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Season ID = ").Append(cimSeason.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateSeasonResourceDescription(FTN.Season cimSeason)
		{
			ResourceDescription rd = null;
			if (cimSeason != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SEASON, importHelper.CheckOutIndexForDMSType(DMSType.SEASON));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimSeason.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateSeasonProperties(cimSeason, rd);
			}
			return rd;
		}

		private void ImportDayType()
		{
			SortedDictionary<string, object> cimDayTypes = concreteModel.GetAllObjectsOfType("FTN.DayType");
			if (cimDayTypes != null)
			{
				foreach (KeyValuePair<string, object> cimDayTypePair in cimDayTypes)
				{
					FTN.DayType cimDayType = cimDayTypePair.Value as FTN.DayType;

					ResourceDescription rd = CreateDayTypeResourceDescription(cimDayType);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("DayType ID = ").Append(cimDayType.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("DayType ID = ").Append(cimDayType.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}
		private ResourceDescription CreateDayTypeResourceDescription(FTN.DayType cimDayType)
		{
			ResourceDescription rd = null;
			if (cimDayType != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DAYTYPE, importHelper.CheckOutIndexForDMSType(DMSType.DAYTYPE));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimDayType.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateDayTypeProperties(cimDayType, rd);
			}
			return rd;
		}

		private void ImportRegulationControl()
		{
			//uzmu se svi objekti tipa RegulationControl
			SortedDictionary<string, object> cimRegulationControls = concreteModel.GetAllObjectsOfType("FTN.RegulatingControl");
			if (cimRegulationControls != null)
			{
				foreach (KeyValuePair<string, object> cimRegulatingControlPair in cimRegulationControls)
				{
					//ovo je cim klasa onaj korak gde od cim profila kreiram klase i onaj dll 

					FTN.RegulatingControl cimRegulatingControl = cimRegulatingControlPair.Value as FTN.RegulatingControl;

					//pravljenje delta objekta 
					//Kreiranje CreateRegulatingControlResourceDescription resource descripiona  iz preko cima
					ResourceDescription rd = CreateRegulatingControlResourceDescription(cimRegulatingControl);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("RegulatingControl ID = ").Append(cimRegulatingControl.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("RegulatingControl ID = ").Append(cimRegulatingControl.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}
		private ResourceDescription CreateRegulatingControlResourceDescription(FTN.RegulatingControl cimRegulatingControl)
		{
			//svaki RD ima svoj Gid i cim Id tjst id onih 
			ResourceDescription rd = null;
			if (cimRegulatingControl != null)
			{
				//modelCodeHelper pravi taj GID (negativne vrednosti)
				//gid se sastoji iz 3 dela systemId,type i onaj brojac o kome vodi racuna importhelper


				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULATINGCONTROL, importHelper.CheckOutIndexForDMSType(DMSType.REGULATINGCONTROL));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimRegulatingControl.ID, gid);


				////populate ResourceDescription
				///metoda koja popunjava sve propertije

				PowerTransformerConverter.PopulateRegulatingControlProperties(cimRegulatingControl, rd);
			}
			return rd;
		}

		private void ImportRegulationSchedule()
		{
			SortedDictionary<string, object> cimRegulationSchedules = concreteModel.GetAllObjectsOfType("FTN.RegulationSchedule");
			if (cimRegulationSchedules != null)
			{
				foreach (KeyValuePair<string, object> cimRegulationSchedulePair in cimRegulationSchedules)
				{
					FTN.RegulationSchedule cimRegulationSchedule = cimRegulationSchedulePair.Value as FTN.RegulationSchedule;

					ResourceDescription rd = CreateRegulationScheduleResourceDescription(cimRegulationSchedule);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("RegulationSchedule ID = ").Append(cimRegulationSchedule.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("RegulationSchedule ID = ").Append(cimRegulationSchedule.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}
		private ResourceDescription CreateRegulationScheduleResourceDescription(FTN.RegulationSchedule cimRegulationSchedule)
		{
			ResourceDescription rd = null;
			if (cimRegulationSchedule != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULATIONSCHEDULE, importHelper.CheckOutIndexForDMSType(DMSType.REGULATIONSCHEDULE));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimRegulationSchedule.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateRegulationScheduleProperties(cimRegulationSchedule, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportRegularTimePoint()
		{
			SortedDictionary<string, object> cimRegularTimePoints = concreteModel.GetAllObjectsOfType("FTN.RegularTimePoint");
			if (cimRegularTimePoints != null)
			{
				foreach (KeyValuePair<string, object> cimRegularTimePointPair in cimRegularTimePoints)
				{
					FTN.RegularTimePoint cimRegularTimePoint = cimRegularTimePointPair.Value as FTN.RegularTimePoint;

					ResourceDescription rd = CreateRegularTimePointResourceDescription(cimRegularTimePoint);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("RegularTimePoint ID = ").Append(cimRegularTimePoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("RegularTimePoint ID = ").Append(cimRegularTimePoint.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}
		private ResourceDescription CreateRegularTimePointResourceDescription(FTN.RegularTimePoint cimRegularTimePoint)
		{
			ResourceDescription rd = null;
			if (cimRegularTimePoint != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULARTIMEPOINT, importHelper.CheckOutIndexForDMSType(DMSType.REGULARTIMEPOINT));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimRegularTimePoint.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateRegularTimePointProperties(cimRegularTimePoint, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportFuse()
		{
			SortedDictionary<string, object> cimFuses = concreteModel.GetAllObjectsOfType("FTN.Fuse");
			if (cimFuses != null)
			{
				foreach (KeyValuePair<string, object> cimFusePair in cimFuses)
				{
					FTN.Fuse cimFuse = cimFusePair.Value as FTN.Fuse;

					ResourceDescription rd = CreateFuseResourceDescription(cimFuse);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Fuse ID = ").Append(cimFuse.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Fuse ID = ").Append(cimFuse.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}
		private ResourceDescription CreateFuseResourceDescription(FTN.Fuse cimFuse)
		{
			ResourceDescription rd = null;
			if (cimFuse != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.FUSE, importHelper.CheckOutIndexForDMSType(DMSType.FUSE));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimFuse.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateFuseProperties(cimFuse, rd);
			}
			return rd;
		}

		private void ImportDisconnector()
		{
			SortedDictionary<string, object> cimDisconnectors = concreteModel.GetAllObjectsOfType("FTN.Disconnector");
			if (cimDisconnectors != null)
			{
				foreach (KeyValuePair<string, object> cimDisconnectorPair in cimDisconnectors)
				{
					FTN.Disconnector cimDisconnector = cimDisconnectorPair.Value as FTN.Disconnector;

					ResourceDescription rd = CreateDisconnectorResourceDescription(cimDisconnector);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Disconnector ID = ").Append(cimDisconnector.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Disconnector ID = ").Append(cimDisconnector.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}
		private ResourceDescription CreateDisconnectorResourceDescription(FTN.Disconnector cimDisconnector)
		{
			ResourceDescription rd = null;
			if (cimDisconnector != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DISCONNECTOR, importHelper.CheckOutIndexForDMSType(DMSType.DISCONNECTOR));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimDisconnector.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateDisconnectorProperties(cimDisconnector, rd);
			}
			return rd;
		}

		#endregion Import
	}
}

