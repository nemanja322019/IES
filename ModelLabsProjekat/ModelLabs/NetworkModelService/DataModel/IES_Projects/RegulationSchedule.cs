using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.IES_Projects
{
    public class RegulationSchedule : SeasonDayTypeSchedule
    {
        private long regulationControl;

        public RegulationSchedule(long globalId) : base(globalId)
        {
        }
        public long RegulationControl
        {
            get { return regulationControl; }
            set { regulationControl = value; }
        }
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegulationSchedule x = (RegulationSchedule)obj;
                return (x.regulationControl == this.regulationControl);
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
                case ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL:

                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL:
                    property.SetValue(regulationControl);
                    return;

            }
            base.GetProperty(property);
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL:
                    regulationControl = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation	

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (regulationControl != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL] = new List<long>();
                references[ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL].Add(regulationControl);
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation
    }
}
