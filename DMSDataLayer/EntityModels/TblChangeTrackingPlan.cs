using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblChangeTrackingPlan
    {
        public int Id { get; set; }
        public int? SourceToSiteStartTime { get; set; }
        public int? SiteToDestStartTime { get; set; }
        public int? SourceToSitePeriod { get; set; }
        public int? SiteToDestPeriod { get; set; }
        public bool? SourceToSiteAppLock { get; set; }
        public bool? SiteToDestAppLock { get; set; }
        public int? DestToSiteStartTime { get; set; }
        public int? SiteToSourceStartTime { get; set; }
        public int? DestToSitePeriod { get; set; }
        public int? SiteToSourcePeriod { get; set; }
        public bool? DestToSiteAppLock { get; set; }
        public bool? SiteToSourceAppLock { get; set; }
    }
}
