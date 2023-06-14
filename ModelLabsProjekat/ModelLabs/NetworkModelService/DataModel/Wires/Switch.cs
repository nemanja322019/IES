using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.IES_Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Switch : ConductingEquipment
    {
        private bool normalOpen;
        private bool retained;
        private int switchOnCount;
        private DateTime switchOnDate;

        public Switch(long globalId) : base(globalId)
        {
        }
        public bool NormalOpen
        {
            get { return normalOpen; }
            set { normalOpen = value; }
        }
        public bool Retained
        {
            get { return retained; }
            set { retained = value; }
        }
        public int SwitchOnCount
        {
            get { return switchOnCount; }
            set { switchOnCount = value; }
        }
        public DateTime SwitchOnDate
        {
            get { return switchOnDate; }
            set { switchOnDate = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Switch x = (Switch)obj;
                return ((x.normalOpen == this.normalOpen) &&
                        (x.retained == this.retained) &&
                        (x.switchOnCount == this.switchOnCount) &&
                        (x.switchOnDate == this.switchOnDate));
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
                case ModelCode.SWITCH_NORMALOPEN:
                case ModelCode.SWITCH_RETAINED:
                case ModelCode.SWITCH_SWITCHONCOUNT:
                case ModelCode.SWITCH_SWITCHONDATE:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.SWITCH_NORMALOPEN:
                    prop.SetValue(normalOpen);
                    return;
                case ModelCode.SWITCH_RETAINED:
                    prop.SetValue(retained);
                    return;
                case ModelCode.SWITCH_SWITCHONCOUNT:
                    prop.SetValue(switchOnCount);
                    return;
                case ModelCode.SWITCH_SWITCHONDATE:
                    prop.SetValue(switchOnDate);
                    return;

            }
            base.GetProperty(prop);
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SWITCH_NORMALOPEN:
                    normalOpen = property.AsBool();
                    break;
                case ModelCode.SWITCH_RETAINED:
                    retained = property.AsBool();
                    break;
                case ModelCode.SWITCH_SWITCHONCOUNT:
                    switchOnCount = property.AsInt();
                    break;
                case ModelCode.SWITCH_SWITCHONDATE:
                    switchOnDate = property.AsDateTime();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

    }
}
