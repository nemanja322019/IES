using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.IES_Projects
{
    public class RegulatingControl : PowerSystemResource
    {
        private bool discrete;
        private RegulatingControlModeKind mode;
        private PhaseCode monitoredPhase;
        private float targetRange;
        private float targetValue;
        List<long> regulationSchedule = new List<long>();

        public RegulatingControl(long globalId) : base(globalId)
        {
        }
        public bool Discrete
        {
            get { return discrete; }
            set { discrete = value; }
        }
        public RegulatingControlModeKind Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        public PhaseCode MonitoredPhase
        {
            get { return monitoredPhase; }
            set { monitoredPhase = value; }
        }
        public float TargetRange
        {
            get { return targetRange; }
            set { targetRange = value; }
        }
        public float TargetValue
        {
            get { return targetValue; }
            set { targetValue = value; }
        }
        public List<long> RegulationSchedule
        {
            get { return regulationSchedule; }
            set { regulationSchedule = value; }
        }
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegulatingControl x = (RegulatingControl)obj;
                return ((x.discrete == this.discrete) &&
                        (x.mode == this.mode) &&
                        (x.monitoredPhase == this.monitoredPhase) &&
                        (x.targetRange == this.targetRange) &&
                        (x.targetValue == this.targetRange) &&
                        (CompareHelper.CompareLists(x.regulationSchedule, this.regulationSchedule)));
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.REGULATINGCONTROL_DISCRETE:
                case ModelCode.REGULATINGCONTROL_MODE:
                case ModelCode.REGULATINGCONTROL_MONITOREDPHASE:
                case ModelCode.REGULATINGCONTROL_TARGETRANGE:
                case ModelCode.REGULATINGCONTROL_TARGETVALUE:
                case ModelCode.REGULATINGCONTROL_REGULATIONSCHEDULES:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.REGULATINGCONTROL_DISCRETE:
                    prop.SetValue(discrete);
                    return;
                case ModelCode.REGULATINGCONTROL_MODE:
                    prop.SetValue((short)mode);
                    return;
                case ModelCode.REGULATINGCONTROL_MONITOREDPHASE:
                    prop.SetValue((short)monitoredPhase);
                    return;
                case ModelCode.REGULATINGCONTROL_TARGETRANGE:
                    prop.SetValue(targetRange);
                    return;
                case ModelCode.REGULATINGCONTROL_TARGETVALUE:
                    prop.SetValue(targetValue);
                    return;
                case ModelCode.REGULATINGCONTROL_REGULATIONSCHEDULES:
                    prop.SetValue(regulationSchedule);
                    return;

            }
            base.GetProperty(prop);
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATINGCONTROL_DISCRETE:
                    discrete = property.AsBool();
                    break;
                case ModelCode.REGULATINGCONTROL_MODE:
                    mode = (RegulatingControlModeKind)property.AsEnum();
                    break;
                case ModelCode.REGULATINGCONTROL_MONITOREDPHASE:
                    monitoredPhase = (PhaseCode)property.AsEnum();
                    break;
                case ModelCode.REGULATINGCONTROL_TARGETRANGE:
                    targetRange = property.AsFloat();
                    break;
                case ModelCode.REGULATINGCONTROL_TARGETVALUE:
                    targetValue = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation	

        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return regulationSchedule.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (regulationSchedule != null && regulationSchedule.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULATINGCONTROL_REGULATIONSCHEDULES] = regulationSchedule.GetRange(0, regulationSchedule.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL:
                    regulationSchedule.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL:

                    if (regulationSchedule.Contains(globalId))
                    {
                        regulationSchedule.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion IReference implementation	
    }
}
