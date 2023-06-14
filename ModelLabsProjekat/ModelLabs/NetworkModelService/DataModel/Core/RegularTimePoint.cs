using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class RegularTimePoint : IdentifiedObject
    {
        private int sequenceNumber;
        private float value1;
        private float value2;
        private long intervalSchedule;

        public RegularTimePoint(long globalId) : base(globalId)
        {
        }
        public int SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }
        public float Value1
        {
            get { return value1; }
            set { value1 = value; }
        }
        public float Value2
        {
            get { return value2; }
            set { value2 = value; }
        }
        public long IntervalSchedule
        {
            get { return intervalSchedule; }
            set { intervalSchedule = value; }
        }
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegularTimePoint x = (RegularTimePoint)obj;
                return (x.sequenceNumber == this.sequenceNumber &&
                        x.value1 == this.value1 &&
                        x.value2 == this.value2 &&
                        x.intervalSchedule == this.intervalSchedule);
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
                case ModelCode.REGULARTIMEPOINT_SEQUENCENUMBER:
                case ModelCode.REGULARTIMEPOINT_VALUE1:
                case ModelCode.REGULARTIMEPOINT_VALUE2:
                case ModelCode.REGULARTIMEPOINT_REGULARINTERVALSCHEDULE:

                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULARTIMEPOINT_SEQUENCENUMBER:
                    property.SetValue(sequenceNumber);
                    return;

                case ModelCode.REGULARTIMEPOINT_VALUE1:
                    property.SetValue(value1);
                    return;

                case ModelCode.REGULARTIMEPOINT_VALUE2:
                    property.SetValue(value2);
                    return;

                case ModelCode.REGULARTIMEPOINT_REGULARINTERVALSCHEDULE:
                    property.SetValue(intervalSchedule);
                    return;

            }
            base.GetProperty(property);
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULARTIMEPOINT_SEQUENCENUMBER:
                    sequenceNumber = property.AsInt();
                    return;

                case ModelCode.REGULARTIMEPOINT_VALUE1:
                    value1 = property.AsFloat();
                    return;

                case ModelCode.REGULARTIMEPOINT_VALUE2:
                    value2 = property.AsFloat();
                    return;

                case ModelCode.REGULARTIMEPOINT_REGULARINTERVALSCHEDULE:
                    intervalSchedule = property.AsReference();
                    return;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation	

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (intervalSchedule != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.REGULARTIMEPOINT_REGULARINTERVALSCHEDULE] = new List<long>();
                references[ModelCode.REGULARTIMEPOINT_REGULARINTERVALSCHEDULE].Add(intervalSchedule);
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation
    }
}
