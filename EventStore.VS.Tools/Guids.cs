// Guids.cs
// MUST match guids.h
using System;

namespace EventStore.VS.Tools
{
    static class GuidList
    {
        public const string guidEventStore_VS_ToolsPkgString = "ecca8856-78a1-4c99-a6a4-6471bb82cc86";
        public const string guidEventStore_VS_ToolsCmdSetString = "99a1c786-9f8c-48c9-bb7a-7839f10e0249";
        public const string guidToolWindowPersistanceString = "f0a336be-32fe-4e02-bb4d-bb455632d8b0";

        public const string guidEventStore_VS_ProjectionsProjectString = "1F9597DD-BEAF-4316-BF99-CBA44153BBAD";

        public static readonly Guid guidEventStore_VS_ToolsCmdSet = new Guid(guidEventStore_VS_ToolsCmdSetString);
        public static readonly Guid guidEventStore_VS_ProjectionsProject = new Guid(guidEventStore_VS_ProjectionsProjectString);

        public const string guidGeneralPropertyPage = "131DEA5D-FFDF-4499-AFE5-5B5A2640A921";
        public const string guidDeployPropertyPage = "C8C29102-2DC5-4F2F-B276-25A880C856BA";

        public const string guidProjectionFileProperties = "01EBE871-0280-45F5-B2CD-24FB42985C55";
        public const string guidProjectionsProjectProperties = "B20EF74F-60C6-484B-99D0-CF7D010F8267";
    };
}