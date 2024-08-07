﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DevToys.Tools.Tools.Converters.Date {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DateConverter {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DateConverter() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DevToys.Tools.Tools.Converters.Date.DateConverter", typeof(DateConverter).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Date converter tool.
        /// </summary>
        internal static string AccessibleName {
            get {
                return ResourceManager.GetString("AccessibleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Date to convert. Supported format: &quot;ISO 8601&quot; example 2023-11-22T19:58:07.0000000+00:00.
        /// </summary>
        internal static string DateOptionDescription {
            get {
                return ResourceManager.GetString("DateOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Daylight saving time..
        /// </summary>
        internal static string DaylightSavingTime {
            get {
                return ResourceManager.GetString("DaylightSavingTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Day.
        /// </summary>
        internal static string DayTitle {
            get {
                return ResourceManager.GetString("DayTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Convert date to human-readable date and vice versa.
        /// </summary>
        internal static string Description {
            get {
                return ResourceManager.GetString("Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no daylight saving time..
        /// </summary>
        internal static string DisabledDaylightSavingTime {
            get {
                return ResourceManager.GetString("DisabledDaylightSavingTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DST Ambiguous time..
        /// </summary>
        internal static string DSTAmbiguousTime {
            get {
                return ResourceManager.GetString("DSTAmbiguousTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Epoch to use. Supported format: &quot;ISO 8601&quot; example 1970-01-01T00:00:00.0000000+00:00.
        /// </summary>
        internal static string EpochOptionDescription {
            get {
                return ResourceManager.GetString("EpochOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use a user defined epoch.
        /// </summary>
        internal static string EpochSwitchDescription {
            get {
                return ResourceManager.GetString("EpochSwitchDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Default Epoch.
        /// </summary>
        internal static string EpochSwitchTitle {
            get {
                return ResourceManager.GetString("EpochSwitchTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Format to use default Seconds.
        /// </summary>
        internal static string FormatOptionDescription {
            get {
                return ResourceManager.GetString("FormatOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Format.
        /// </summary>
        internal static string FormatTitle {
            get {
                return ResourceManager.GetString("FormatTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hour (24 hour).
        /// </summary>
        internal static string HourTitle {
            get {
                return ResourceManager.GetString("HourTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input to convert in long or Date supported format &quot;ISO 8601&quot; example 2023-11-22T19:58:07.0000000+00:00.
        /// </summary>
        internal static string InputDescription {
            get {
                return ResourceManager.GetString("InputDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to parse date.
        /// </summary>
        internal static string InvalidDateTimeCommand {
            get {
                return ResourceManager.GetString("InvalidDateTimeCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to parse epoch.
        /// </summary>
        internal static string InvalidEpochCommand {
            get {
                return ResourceManager.GetString("InvalidEpochCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find the Timezone Provided please check if the timezone exist here https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones?view=windows-11&amp;WT.mc_id=DT-MVP-5001664&amp;source=post_page-----cff0e2b37f52--------------------------------#time-zones.
        /// </summary>
        internal static string InvalidTimeZoneCommand {
            get {
                return ResourceManager.GetString("InvalidTimeZoneCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please specify an input number or date.
        /// </summary>
        internal static string InvalidValue {
            get {
                return ResourceManager.GetString("InvalidValue", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to ISO8601 Title.
        /// </summary>
        internal static string ISO8601Title
        {
            get
            {
                return ResourceManager.GetString("ISO8601Title", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Local Date and Time.
        /// </summary>
        internal static string LocalDateTime {
            get {
                return ResourceManager.GetString("LocalDateTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Date Converter.
        /// </summary>
        internal static string LongDisplayTitle {
            get {
                return ResourceManager.GetString("LongDisplayTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Milliseconds.
        /// </summary>
        internal static string Milliseconds {
            get {
                return ResourceManager.GetString("Milliseconds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Milliseconds.
        /// </summary>
        internal static string MillisecondsTitle {
            get {
                return ResourceManager.GetString("MillisecondsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minutes.
        /// </summary>
        internal static string MinutesTitle {
            get {
                return ResourceManager.GetString("MinutesTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Month.
        /// </summary>
        internal static string MonthTitle {
            get {
                return ResourceManager.GetString("MonthTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No.
        /// </summary>
        internal static string No {
            get {
                return ResourceManager.GetString("No", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Now.
        /// </summary>
        internal static string Now {
            get {
                return ResourceManager.GetString("Now", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Offset.
        /// </summary>
        internal static string OffsetTitle {
            get {
                return ResourceManager.GetString("OffsetTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Time Date Timezone Epoch Timestamp Ticks.
        /// </summary>
        internal static string SearchKeywords {
            get {
                return ResourceManager.GetString("SearchKeywords", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Seconds.
        /// </summary>
        internal static string Seconds {
            get {
                return ResourceManager.GetString("Seconds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Seconds.
        /// </summary>
        internal static string SecondsTitle {
            get {
                return ResourceManager.GetString("SecondsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Date.
        /// </summary>
        internal static string ShortDisplayTitle {
            get {
                return ResourceManager.GetString("ShortDisplayTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is daylight saving time..
        /// </summary>
        internal static string SupportsDaylightSavingTime {
            get {
                return ResourceManager.GetString("SupportsDaylightSavingTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ticks.
        /// </summary>
        internal static string Ticks {
            get {
                return ResourceManager.GetString("Ticks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Date.
        /// </summary>
        internal static string TimestampTitle {
            get {
                return ResourceManager.GetString("TimestampTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Timezone to use (supported timezones: https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones?view=windows-11&amp;WT.mc_id=DT-MVP-5001664&amp;source=post_page-----cff0e2b37f52--------------------------------#time-zones)..
        /// </summary>
        internal static string TimezoneOptionDescription {
            get {
                return ResourceManager.GetString("TimezoneOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Time zone.
        /// </summary>
        internal static string TimeZoneTitle {
            get {
                return ResourceManager.GetString("TimeZoneTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UTC Date and Time.
        /// </summary>
        internal static string UTCDateTime {
            get {
                return ResourceManager.GetString("UTCDateTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UtcTicks.
        /// </summary>
        internal static string UtcTicksTitle {
            get {
                return ResourceManager.GetString("UtcTicksTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Year.
        /// </summary>
        internal static string YearTitle {
            get {
                return ResourceManager.GetString("YearTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yes.
        /// </summary>
        internal static string Yes {
            get {
                return ResourceManager.GetString("Yes", resourceCulture);
            }
        }
    }
}
