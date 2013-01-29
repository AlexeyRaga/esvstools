// Guids.cs
// MUST match guids.h
using System;

namespace EventStore.EventStore_VS_Tools
{
    static class GuidList
    {
        public const string guidEventStore_VS_ToolsPkgString = "ecca8856-78a1-4c99-a6a4-6471bb82cc86";
        public const string guidEventStore_VS_ToolsCmdSetString = "99a1c786-9f8c-48c9-bb7a-7839f10e0249";
        public const string guidToolWindowPersistanceString = "f0a336be-32fe-4e02-bb4d-bb455632d8b0";

        public const string guidEventStore_VS_ProjectionsProjectString = "1F9597DD-BEAF-4316-BF99-CBA44153BBAD";

        public static readonly Guid guidEventStore_VS_ToolsCmdSet = new Guid(guidEventStore_VS_ToolsCmdSetString);
        public static readonly Guid guidEventStore_VS_ProjectionsProject = new Guid(guidEventStore_VS_ProjectionsProjectString);
    };
}