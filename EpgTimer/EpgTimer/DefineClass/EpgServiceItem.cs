using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CtrlCmdCLI.Def;

namespace EpgTimer
{
    public class EpgServiceItem : EpgServiceInfo
    {
        public EpgServiceItem(EpgServiceInfo info)
        {
            network_name = info.network_name;
            ONID = info.ONID;
            partialReceptionFlag = info.partialReceptionFlag;
            remote_control_key_id = info.remote_control_key_id;
            service_name = info.service_name;
            service_provider_name = info.service_provider_name;
            service_type = info.service_type;
            SID = info.SID;
            ts_name = info.ts_name;
            TSID = info.TSID;

            GroupID = 0;
            GroupNext = 0;
            LeftPos = 0;
            Width = 0;
            GroupWidth = 0;
        }
        public UInt64 GroupID { get; set; }
        public UInt64 GroupNext { get; set; }
        public Double LeftPos { get; set; }
        public Double Width { get; set; }
        public Double GroupWidth { get; set; }
    }
}
